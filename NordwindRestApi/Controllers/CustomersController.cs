﻿using Microsoft.AspNetCore.Http;
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
            var asiakkaat = db.Customers.ToList();
            return Ok(asiakkaat);
        }
    }
}
