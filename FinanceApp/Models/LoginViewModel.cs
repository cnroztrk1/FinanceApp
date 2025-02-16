using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Presentation.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı gereklidir")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
