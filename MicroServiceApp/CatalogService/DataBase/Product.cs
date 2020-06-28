using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogService.DataBase
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int UnitPrice { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
