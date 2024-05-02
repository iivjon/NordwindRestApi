using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NordwindRestApi.Models;
using NordwindRestApi.Services.Interfaces;

namespace NordwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IAuthenticate _authentiate;

        public AuthenticationController(IAuthenticate authenticate)
        {
            _authentiate = authenticate;
        }

        //Tähän tulee front endin kirjautumis
        [HttpPost]
        public ActionResult Post([FromBody] Credentials tunnukset) 
        {
            var loggedUser = _authentiate.Authenticate(tunnukset.Username, tunnukset.Password);

            if (loggedUser == null)
                return BadRequest(new { message = "Käyttäjätunnus tai salasana on virheellinen" });
            return Ok(loggedUser);
        }
    }
}
