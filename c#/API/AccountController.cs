using System.Threading.Tasks;
using System.Web.Http;
using eticketing_mvc.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;

namespace eticketing_mvc.Controllers.API
{
    [Route("api/accounts/{action}")]
    public class AccountController : ApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<IHttpActionResult> UserLogin(Login userLogin, string returnUrl = "")
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("An error occured processing your request");
            }

            var user = await _userManager.FindByNameAsync(userLogin.Username);
            return Ok("");
        }
    }
}
