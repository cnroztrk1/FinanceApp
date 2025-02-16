using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace FinanceApp.Data.Entities
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public int TenantId => Id;

        // Şifreleme Fonksiyonu
        public static string HashPassword(string password)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashedBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
