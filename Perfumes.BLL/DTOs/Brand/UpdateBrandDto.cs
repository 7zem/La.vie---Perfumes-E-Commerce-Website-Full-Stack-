using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Perfumes.BLL.DTOs.Brand
{
    public class UpdateBrandDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }

        // Optional new logo image
        public IFormFile? Logo { get; set; }
        public string? LogoUrl { get; set; }
    }
}
