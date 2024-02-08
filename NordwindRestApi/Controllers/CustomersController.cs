using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NordwindRestApi.Models;

namespace NordwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        // Alustetaan tietokantayhteys
        NorthwindOriginalContext db = new NorthwindOriginalContext();
        //Hakee kaikki asiakkaat
        [HttpGet]
        public ActionResult GetAllCustomers()
        {
            try
            {
                var asiakkaat = db.Customers.ToList();
                return Ok(asiakkaat);
            }
            catch (Exception ex)
            {
                return BadRequest("Tapahtui virhe. LUe lisää" + ex.InnerException);
            }
        }

        //Hakee yhden asiakkaan pääavaimella
        [HttpGet]
        [Route("{id}")]
        public ActionResult GetOneCustomerById(string id)
        {
            try
            {
                //Customer asiakas = db.Customers.Find(); //Tämä toimii samalla tavalla, mut on vahvasti tyypitetty
                var asiakas = db.Customers.Find(id);

                if (asiakas != null)
                {
                    return Ok(asiakas);
                }
                else
                {
                    //return BadRequest("Asiakasta id:llä" + id + " ei löydy.");
                    return NotFound($"Asiakasta id:llä {id}  ei löydy."); //string interpolation
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Tapahtui virhe haettaessa avain asiakasta" + ex);
            }
        }

        //Uuden lisääminen
        [HttpPost]
        public ActionResult AddNew([FromBody] Customer cust) 
        {
            try 
            {
                db.Customers.Add(cust);
                db.SaveChanges();
                return Ok($"Lisättiin uusi asiakas {cust.CompanyName} from {cust.City} ");
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe. Lue lisää:" + e.InnerException);
            }
        
        
        }

    }
}
