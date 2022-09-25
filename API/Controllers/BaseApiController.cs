namespace API.Controllers;

    //Added LogUserActivity to trac user last login activity
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController: ControllerBase
    {
        
    }
