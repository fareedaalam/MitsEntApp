namespace API.Controllers;

public class AccountController : BaseApiController
{

    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _mapper = mapper;
        // _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        UserParams userParams = new UserParams();

        if (await UserExists(registerDto.UserName)) return BadRequest("User Already Taken");
        var user = _mapper.Map<AppUser>(registerDto);

        int RegistrationCount = _userManager.Users.Count(x => x.KnownAs == "contestant");
        if (RegistrationCount >= userParams.RegistrationCount) return BadRequest("Registration is over!");
              
        if (await MobileExists(registerDto.PhoneNumber)) return BadRequest("Mobile number already registerd");

        // using var hmac = new HMACSHA512();
        //Create a user hasing 
        user.UserName = registerDto.UserName.ToLower();
        // user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
        // user.PasswordSalt = hmac.Key;
        //user.DateOfBirth= DateTime.SpecifyKind(user.DateOfBirth,DateTimeKind.Utc);


        var result = await _userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded) return BadRequest(result.Errors);

        var roleResult = await _userManager.AddToRoleAsync(user, "Member");
        if (!roleResult.Succeeded) return BadRequest(result.Errors);
        // _context.Users.Add(user);
        // await _context.SaveChangesAsync();

        return new UserDto
        {
            Username = user.UserName,
            Token = await _tokenService.CreateToken(user),
            //PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
            KnownAs = user.KnownAs,
            Gender = user.Gender
        };

    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        //get user from db
        try
        {
            var usr = await _userManager.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.UserName == loginDto.username.ToLower());

            if (usr == null) return Unauthorized("Invalid UserName");

            var result = await _signInManager
                    .CheckPasswordSignInAsync(usr, loginDto.password, false);

            if (!result.Succeeded) return Unauthorized();

            // using var hmac = new HMACSHA512(usr.PasswordSalt);

            // var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.password));

            // for (int i = 0; i < computeHash.Length; i++)
            // {
            //     if (computeHash[i] != usr.PasswordHash[i]) return Unauthorized("Invalid Password");
            // }

            return new UserDto
            {
                Username = usr.UserName,
                Token = await _tokenService.CreateToken(usr),
                PhotoUrl = usr.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = usr.KnownAs,
                Gender = usr.Gender
            };
        }

        catch (System.Exception)
        {

            throw;
        }

    }
   
   
    private async Task<bool> UserExists(String username)
    {

        return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
    }

    private async Task<bool> MobileExists(String mobile)
    {
        return await _userManager.Users.AnyAsync(m => m.PhoneNumber == mobile);
    }



}
