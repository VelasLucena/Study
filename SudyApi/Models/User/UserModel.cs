using StudandoApi.Properties.Enuns;
using SudyApi.Models.User;
using SudyApi.Security;
using SudyApi.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System.Security.Claims;

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

        public UserModel() { }

        public UserModel(RegisterUserViewModel viewModel)
        {
            Name = viewModel.Name;
            Email = viewModel.Email;
            PasswordHash = EncryptPassord.Hash(viewModel.Password);
            CreationDate = DateTime.Now;
            CreationUser = UserLogged.UserId;

            UserInformation = new UserInformation(viewModel);
        }

        public UserModel(EditUserViewModel viewModel)
        {
            UserId = viewModel.UserId;
            Name = viewModel.Name;
            Email = viewModel.Email;
            PasswordHash = EncryptPassord.Hash(viewModel.Password);
            UpdateDate = DateTime.Now;
            UpdateUser = UserLogged.UserId;

            UserInformation = new UserInformation(viewModel);
        }

        public static IEnumerable<Claim> GetClaims(UserModel user)
        {
            var result = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };
            return result;
        }
    }
}
