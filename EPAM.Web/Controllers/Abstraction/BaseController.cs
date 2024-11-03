using Microsoft.AspNetCore.Mvc;

namespace EPAM.Web.Controllers.Abstraction
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController<T> : ControllerBase
    {
        public readonly ILogger<T> Logger;

        protected BaseController(ILogger<T> logger)
        {
            Logger = logger;
        }
    }
}
