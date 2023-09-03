using StudandoApi.Properties.Enuns;
using SudyApi.Properties.Enuns;
using SudyApi.Security;
using SudyApi.ViewModels;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System.Security.Claims;

namespace SudyApi.Models
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }

        public UserInformationModel? UserInformation { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? PasswordHash { get; set; }

        public DateTime? LastLogin { get; set; }

        public int? ResetPassawordCount { get; set; }

        public string? Token { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public ICollection<SemesterModel>? Semesters { get; set; }

        #region Constructor

        public UserModel() { }

        public UserModel(RegisterUserViewModel viewModel)
        {
            Name = viewModel.Name;
            Email = viewModel.Email;
            PasswordHash = EncryptPassord.Hash(viewModel.Password);
            CreationDate = DateTime.Now;

            UserInformation = new UserInformationModel(viewModel);
        }

        #endregion

        #region Methods

        public void Login()
        {
            Token = Security.Token.GenerateToken(this);
            UpdateDate = DateTime.Now;
        }

        public void Logout()
        {
            Token = null;
            LastLogin = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
        }

        public void Update(EditUserViewModel viewModel)
        {
            Name = viewModel.Name != null ? viewModel.Name : Name;
            Email = viewModel.Email != null ? viewModel.Email : Email;
            PasswordHash = viewModel.Password != null ? EncryptPassord.Hash(viewModel.Password) : PasswordHash;
            UpdateDate = DateTime.Now;
            
            UserInformation.Update(viewModel);
        }

        #endregion
    }
}
