using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ProductsModule.Models.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }

        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }


	}
}
