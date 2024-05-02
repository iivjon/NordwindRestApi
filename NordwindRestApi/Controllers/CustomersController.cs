using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NordwindRestApi.Models;

namespace NordwindRestApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        // Alustetaan tietokantayhteys
        //Perinteinen tapa
        //NorthwindOriginalContext db = new NorthwindOriginalContext();

        //Dependency injektion tapa
        private NorthwindOriginalContext db;

        public CustomersController(NorthwindOriginalContext dbparametri) 
        {
            db = dbparametri;
        }


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

        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            try {
                var asiakas = db.Customers.Find(id);

                if (asiakas != null) //Jos id:llä löytyy asiakas
                {
                    db.Customers.Remove(asiakas);
                    db.SaveChanges();
                    return Ok("Asiakas " + asiakas.CompanyName + " poistettiin");
                }
                return NotFound("Asiakasta id:llä" + id + " ei löytynyt");
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
            }
        }

        //Asiakkaan muokkaus
        [HttpPut("{id}")]
        public ActionResult EditCustomer(string id, Customer customer)
        {
            var asiakas = db.Customers.Find(id);
            if (asiakas != null)
            {
                asiakas.CompanyName = customer.CompanyName;
                asiakas.ContactName = customer.ContactName;
                asiakas.Address = customer.Address;
                asiakas.City = customer.City;
                asiakas.Region = customer.Region;
                asiakas.PostalCode = customer.PostalCode;
                asiakas.Country = customer.Country;
                asiakas.Phone = customer.Phone;
                asiakas.Fax = customer.Fax;

                db.SaveChanges();
                return Ok("Muokattu asiakasta " + asiakas.CompanyName);
            }
            return NotFound("Asiakasta ei löytynyt id:llä " + id);
        }
        //Hakee nimen osalla: /api/companyname/hakusana
        [HttpGet("companyname /{cname}")]

        public ActionResult GetByName(string cname) 
        {
            try 
            {
                var cust = db.Customers.Where(c => c.CompanyName.Contains(cname)); //Contains metodilla voidaan hakea nimen osalla.. ei tarvitse olla koko nimeä
                //var cust = from c in db.Customers where c.CompanyName.Contains(cname) select c; <-- sama, mutta traditional 
                //var cust = db.Customers.Where(c => c.CompanyName == cname); <-- Perfect match
                return Ok(cust);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
