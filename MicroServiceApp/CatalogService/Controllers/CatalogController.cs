using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatalogService.DataBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly DatabaseContext _db;

        public CatalogController(DatabaseContext databaseContext)
        {
            _db = databaseContext;
        }

        [HttpGet("product")]
        public async Task<IActionResult> GetProducts()
        {
            var product = await _db.Products.ToListAsync();
            if (product == null) return NotFound();
            return Ok(product);
        }
        [HttpGet]
        public async Task<IActionResult> GetCategory()
        {
            var categories = await _db.Categories.ToListAsync();
            if (categories == null) return NotFound();
            return Ok(categories);
        }
    }
}
