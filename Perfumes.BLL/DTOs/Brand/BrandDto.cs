using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Perfumes.BLL.DTOs.Brand
{
    public class BrandDto
    {
        public int BrandId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
