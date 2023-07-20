using SudyApi.Models;
using SudyApi.Properties.Enuns;

namespace SudyApi.Data.Interfaces
{
    public interface IInstitutionRepository
    {
        #region GetAllInstitutions

        Task<List<InstitutionModel>> GetAllInstitutions(Ordering ordering = Ordering.Asc, string attributeName = nameof(InstitutionModel.Name));

        Task<List<InstitutionModel>> GetAllInstitutionsNoTracking(Ordering ordering = Ordering.Asc, string attributeName = nameof(InstitutionModel.Name));

        #endregion

        #region GetInstitutionById

        Task<InstitutionModel> GetInstitutionById(int institutionId);

        Task<InstitutionModel> GetInstitutionByIdNoTracking(int institutionId);

        #endregion
    }
}
