using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoastalPharmacyCRUD.DTOs
{
    public class BulkCreateProductDto
    {
        public string fileName { get; set; } = "default.xlsx";
        public List<ProductCreateDto> products { get; set; } = new() {};
    }
}