using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfumes.BLL.DTOs.Brand
{
    public class CreateBrandDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Image file from the form
        public IFormFile? Logo { get; set; }
        public string? LogoUrl { get; set; }
    }
}
