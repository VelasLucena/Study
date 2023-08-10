using SudyApi.Models;
using SudyApi.Properties.Enuns;

namespace SudyApi.Data.Interfaces
{
    public interface IInstitutionRepository
    {
        #region GetAllInstitutions

        Task<List<InstitutionModel>> GetAllInstitutions(Ordering ordering = Ordering.Asc, string keySelector = nameof(InstitutionModel.Name), bool isTracking = true, int take = 10, int skip = 0);

        Task<List<InstitutionModel>> GetAllInstitutionsNoTracking(Ordering ordering = Ordering.Asc, string attributeName = nameof(InstitutionModel.Name));

        #endregion

        #region GetInstitutionById

        Task<InstitutionModel> GetInstitutionById(int institutionId);

        Task<InstitutionModel> GetInstitutionByIdNoTracking(int institutionId);

        #endregion
    }
}
