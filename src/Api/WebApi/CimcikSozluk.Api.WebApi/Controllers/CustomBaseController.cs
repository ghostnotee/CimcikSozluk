using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace CimcikSozluk.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        public Guid? UserId => new Guid(); //new(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
    }
}