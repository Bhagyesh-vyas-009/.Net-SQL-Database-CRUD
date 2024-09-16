using System.ComponentModel.DataAnnotations;

namespace Coffee_Shop.Models
{
    public class ProductModel
    {
        public int ProductID {  get; set; }
        //[StringLength(50)]
        [Required(ErrorMessage="Product Name is required")]
        public string? ProductName { get; set; }

        [DataType(DataType.Currency)]
        [Required(ErrorMessage="Product Price is required")]
        [Range(1,int.MaxValue,ErrorMessage ="Product Price must be greater than 0")]
        public double ProductPrice { get; set; }

        [Required(ErrorMessage ="Product Code is required")]
        public string ProductCode { get; set; }

        [MaxLength(10000)]
        [MinLength(10)]
        [Display(Name ="Product Description")]
        [Required(ErrorMessage = "Product Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage ="User Name is required")]        
        public int UserID { get; set; }
    }

    
}
