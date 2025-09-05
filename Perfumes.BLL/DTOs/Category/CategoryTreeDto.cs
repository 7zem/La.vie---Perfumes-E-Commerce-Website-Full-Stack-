using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfumes.BLL.DTOs.Category
{
    public class CategoryTreeDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<CategoryTreeDto> SubCategories { get; set; } = new();
    }
}

