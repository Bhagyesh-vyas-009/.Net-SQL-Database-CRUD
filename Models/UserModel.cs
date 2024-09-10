using System.ComponentModel.DataAnnotations;

namespace Coffee_Shop.Models
{
    public class UserModel
    {
        public int UserID {  get; set; }
        [Required(ErrorMessage ="User Name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        //[RegularExpression("^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(8)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Mobile No is required")]
        //[Phone]
        [StringLength(10)]
        public string MobileNo {  get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "User IsActive is required")]
        public bool isActive {  get; set; }
    }
}
