using StudandoApi.Properties.Enuns;
using SudyApi.Models.User;
using SudyApi.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace StudandoApi.Models.User
{
    public class UserInformation
    {
        [Key]
        public int UserInformationId { get; set; }

        public string? PhoneNumber { get; set; }

        public int? Age { get; set; }

        public string? Cpf { get; set; }

        public string? Rg { get; set; }

        public Gender? Gender { get; set; }

        public DateTime? Birthday { get; set; }

        public string? Address { get; set; }

        public string? Cep { get; set; }

        public DateTime? CreationDate { get; set; }

        public int? CreationUser { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int? UpdateUser { get; set; }

        public UserInformation() { }

        public UserInformation(RegisterUserViewModel viewModel)
        {
            PhoneNumber = viewModel.PhoneNumber;
            Age = viewModel.Age;
            Cpf = viewModel.Cpf;
            Rg = viewModel.Rg;
            Gender = viewModel.Gender;
            Birthday = viewModel.Birthday;
            Address = viewModel.Address;
            Cep = viewModel.Cep;
            CreationDate = DateTime.Now;
            CreationUser = UserLogged.UserId;
        }

        public UserInformation(EditUserViewModel viewModel)
        {
            UserInformationId = viewModel.UserInformationId;
            PhoneNumber = viewModel.PhoneNumber;
            Age = viewModel.Age;
            Cpf = viewModel.Cpf;
            Rg = viewModel.Rg;
            Gender = viewModel.Gender;
            Birthday = viewModel.Birthday;
            Address = viewModel.Address;
            Cep = viewModel.Cep;
            UpdateDate = DateTime.Now;
            UpdateUser = UserLogged.UserId;
        }
    }
}
