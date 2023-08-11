using SudyApi.Models;
using SudyApi.Properties.Enuns;

namespace SudyApi.Data.Interfaces
{
    public interface IInstitutionRepository
    {
        Task<List<InstitutionModel>> GetAllInstitutions();

        Task<InstitutionModel> GetInstitutionById(int institutionId);
    }
}
