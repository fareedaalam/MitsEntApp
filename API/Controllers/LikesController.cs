namespace API.Controllers;

// [Authorize]
public class LikesController : BaseApiController
{

    private readonly IUnitOfWork _unitOfWork;
    public LikesController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

    }

    [HttpPost("{username}")]
    public async Task<ActionResult> AddLike(string username)
    {
        var sourceUserId = User.GetUserId();
        var likedUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
        var SourceUser = await _unitOfWork.LikesRepository.GetUserWithLikes(sourceUserId);

        if (likedUser == null) return NotFound();

        if (SourceUser.UserName == username) return BadRequest("You cannot like Yourself");

        var UserLike = await _unitOfWork.LikesRepository.GetUserLike(sourceUserId, likedUser.Id);

        if (UserLike != null) return BadRequest("You already like this user");

        UserLike = new UserLike
        {
            SourceUserId = sourceUserId,
            LikedUserId = likedUser.Id
        };

        SourceUser.LikedUsers.Add(UserLike);

        if (await _unitOfWork.Complete()) return Ok();

        return BadRequest("Faild to like user");

    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LikeDto>>> GetUsersLikes([FromQuery] LikesParams likesParams)
    {
        likesParams.UserId = User.GetUserId();
        var users = await _unitOfWork.LikesRepository.GetUserLike(likesParams);
        Response.AddPaginationHeader(users.CurrentPage,
            users.PageSize, users.TotalCount, users.TotalPages);


        return Ok(users);
    }
}
