using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace EYOkulProjectWebUI.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? UserSurName { get; set; }
        public string? UserUserName { get; set; }
        public string? Password { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime InsertedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int SysUserId { get; set; }
        public int UserType { get; set; }
        public int SchoolId { get; set; }

        //////
        [NotMapped]
        public string? SchoolName { get; set; }

        [NotMapped]
        public string? UserTypeName { get; set; }

        [NotMapped]
        public string? ConfirmPassword { get; set; }

        //hash
        public string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                Password = builder.ToString();
                return Password;
            }
        }

        //verify hash
        public bool VerifyPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                string hashedPassword = builder.ToString();

                return hashedPassword == Password;
            }
        }

    }
}
