using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NordwindRestApi.Models;

namespace NordwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        //private readonly NorthwindOriginalContext db = new NorthwindOriginalContext();

        //Dependency injektion tapa
        private NorthwindOriginalContext db;

        public UsersController(NorthwindOriginalContext dbparametri)
        {
            db = dbparametri;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var users = db.Users;
            //foreach (var user in users)
            //{
            //    user.Password = null;
            //}
            return Ok(users);
        }

        //Uuden lisääminen
        [HttpPost]
        public ActionResult PostCreateNew([FromBody] User u)
        {
            try
            {
                db.Users.Add(u);
                db.SaveChanges();
                return Ok("Lisättiin käyttäjä " + u.UserName);
            }
            catch (Exception e)
            {
                return BadRequest("Lisääminen ei onnistunut. Lisätietoa tästä:"+e);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var kayt = db.Users.Find(id);

                if (kayt != null) //Jos id:llä löytyy asiakas
                {
                    db.Users.Remove(kayt);
                    db.SaveChanges();
                    return Ok("Käyttäjä " + kayt.FirstName + " poistettiin");
                }
                return NotFound("Asiakasta id:llä" + id + " ei löytynyt");
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
            }
        }

        //Käyttäjän muokkaus
        [HttpPut("{id}")]
        public ActionResult EditUser(int id, User user)
        {
            var kayttaja = db.Users.Find(id);
            if (kayttaja != null)
            {

                kayttaja.FirstName = user.FirstName;
                kayttaja.LastName = user.LastName;
                kayttaja.Email = user.Email;
                kayttaja.AcceslevelId = user.AcceslevelId;
                kayttaja.UserName = user.UserName;
                kayttaja.Password = user.Password;


                db.SaveChanges();
                return Ok("Muokattu kayttäjä " + kayttaja.FirstName + " " + kayttaja.LastName);
            }
            return NotFound("Käyttäjää ei löytynyt id:llä " + id);
        }
    }
}
