using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StudandoApi.Data.Contexts;
using SudyApi.Data.Interfaces;
using SudyApi.Models;
using SudyApi.Properties.Enuns;
using SudyApi.Utility;

namespace SudyApi.Data.Repositories
{
    public class InstitutionRepository : IInstitutionRepository
    {
        #region Field

        private readonly SudyContext _sudyContext;
        private readonly ICacheService _cachingService;

        #endregion

        #region Constructor

        public InstitutionRepository(SudyContext sudyContext, ICacheService cacheService)
        {
            _sudyContext = sudyContext;
            _cachingService = cacheService;
        }
        #endregion

        #region Methods

        #region GetAllInstitutions

        async Task<List<InstitutionModel>> IInstitutionRepository.GetAllInstitutions(Ordering ordering = Ordering.Asc, string attributeName = nameof(InstitutionModel.Name))
        {
            switch (ordering)
            {
                case Ordering.Asc:
                    return await _sudyContext.Institutions.OrderBy(x => EF.Property<object>(x, attributeName)).ToListAsync();
                case Ordering.Desc:
                    return await _sudyContext.Institutions.OrderByDescending(x => EF.Property<object>(x, attributeName)).ToListAsync();
            }

            return null;
        }

        async Task<List<InstitutionModel>> IInstitutionRepository.GetAllInstitutionsNoTracking(Ordering ordering = Ordering.Asc, string attributeName = nameof(InstitutionModel.Name))
        {
            switch (ordering)
            {
                case Ordering.Asc:
                    return await _sudyContext.Institutions.OrderBy(x => EF.Property<object>(x, attributeName)).ToListAsync();
                case Ordering.Desc:
                    return await _sudyContext.Institutions.OrderByDescending(x => EF.Property<object>(x, attributeName)).ToListAsync();
            }

            return null;
        }

        #endregion

        #region GetInstitutionById

        async Task<InstitutionModel> IInstitutionRepository.GetInstitutionById(int institutionId)
        {
            return await _sudyContext.Institutions.SingleOrDefaultAsync(x => x.institutionId == institutionId);
        }

        async Task<InstitutionModel> IInstitutionRepository.GetInstitutionByIdNoTracking(int institutionId)
        {
            if (!bool.Parse(AppSettings.GetKey(ConfigKeys.RedisCache)))
                return await _sudyContext.Institutions.AsNoTracking().SingleOrDefaultAsync(x => x.institutionId == institutionId);

            string resultCache = await _cachingService.Get(nameof(InstitutionModel) + institutionId);

            if (!string.IsNullOrEmpty(resultCache))
                return JsonConvert.DeserializeObject<InstitutionModel>(resultCache);

            InstitutionModel institution = await _sudyContext.Institutions.AsNoTracking().SingleOrDefaultAsync(x => x.institutionId == institutionId);
            if (institution != null)
                await _cachingService.Set(nameof(InstitutionModel) + institutionId, JsonConvert.SerializeObject(institution));

            return institution;
        }

        #endregion

        #endregion
    }
}
