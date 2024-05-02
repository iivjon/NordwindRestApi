using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NordwindRestApi.Models;

namespace NordwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        // Alustetaan tietokantayhteys
        //Perinteinen tapa
        //NorthwindOriginalContext db = new NorthwindOriginalContext();

        //Dependency injektion tapa
        private NorthwindOriginalContext db;

        public ProductController(NorthwindOriginalContext dbparametri)
        {
            db = dbparametri;
        }


        //Hakee kaikki tuotteet
        [HttpGet]
        public ActionResult GetAllProducts()
        {
            try
            {
                var tuotteet = db.Products.ToList();
                return Ok(tuotteet);
            }
            catch (Exception ex)
            {
                return BadRequest("Tapahtui virhe. LUe lisää" + ex.InnerException);
            }
        }

        //Hakee yhden tuotteen pääavaimella
        [HttpGet]
        [Route("{id}")]
        public ActionResult GetOneProductById(string id)
        {
            try
            {
                
                var tuote = db.Products.Find(id);

                if (tuote != null)
                {
                    return Ok(tuote);
                }
                else
                {
                    
                    return NotFound($"Tuotetta id:llä {id}  ei löydy."); //string interpolation
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Tapahtui virhe haettaessa avain tuotetta" + ex);
            }
        }

        //Uuden lisääminen
        [HttpPost]
        public ActionResult AddNew([FromBody] Product prod)
        {
            try
            {
                db.Products.Add(prod);
                db.SaveChanges();
                return Ok("Lisättiin uusi tuote " + prod.ProductName);
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe. Lue lisää:" + e.InnerException);
            }


        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int? id)
        {
            try
            {
                var tuote = db.Products.Find(id);

                if (tuote != null) //Jos id:llä löytyy tuotetta
                {
                    db.Products.Remove(tuote);
                    db.SaveChanges();
                    return Ok("Tuote " + tuote.ProductName + " poistettiin");
                }
                return NotFound("Tuotetta id:llä" + id + " ei löytynyt");
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
            }
        }

        //Tuotteen muokkaus
        [HttpPut("{id}")]
        public ActionResult EditProduct(int id, Product prod)
        {
            var tuote = db.Products.Find(id);
            if (tuote != null)
            {
                tuote.ProductName = prod.ProductName;
                tuote.SupplierId = prod.SupplierId;
                tuote.CategoryId = prod.CategoryId;
                tuote.QuantityPerUnit = prod.QuantityPerUnit;
                tuote.UnitPrice = prod.UnitPrice;
                tuote.UnitsInStock = prod.UnitsInStock;
                tuote.UnitsOnOrder = prod.UnitsOnOrder;
                tuote.ReorderLevel = prod.ReorderLevel;
                tuote.Discontinued = prod.Discontinued;


                db.SaveChanges();
                return Ok("Muokattu tuotetta " + tuote.ProductName);
            }
            return NotFound("Tuotetta ei löytynyt id:llä " + id);
        }
        //Hakee nimen osalla: /api/companyname/hakusana
        [HttpGet("productname /{pname}")]

        public ActionResult GetByName(string pname)
        {
            try
            {
                var prod = db.Products.Where(c => c.ProductName.Contains(pname)); //Contains metodilla voidaan hakea nimen osalla.. ei tarvitse olla koko nimeä
                //var cust = from c in db.Customers where c.CompanyName.Contains(cname) select c; <-- sama, mutta traditional 
                //var cust = db.Customers.Where(c => c.CompanyName == cname); <-- Perfect match
                return Ok(prod);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
