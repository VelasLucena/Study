using StudandoApi.Properties.Enuns;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

namespace StudandoApi.Models.User
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }

        public UserInformation? UserInformation { get; set; }

        public AcessType? AcessType { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? PasswordHash { get; set; }

        public DateTime? LastLogin { get; set; }

        public int? ResetPassawordCount { get; set; }

        public string? Token { get; set; }

        public DateTime? CreationDate { get; set; }

        public int? CreationUser { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? UpdateDate { get; set; }

        public int? UpdateUser { get; set; }
    }
}
