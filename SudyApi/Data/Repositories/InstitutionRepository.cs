using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StudandoApi.Data.Contexts;
using SudyApi.Data.Helper;
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
        private readonly DataOptionsModel _dataOptions;

        #endregion

        #region Constructor

        public InstitutionRepository(SudyContext sudyContext, ICacheService cacheService, DataOptionsModel dataOptions)
        {
            _sudyContext = sudyContext;
            _cachingService = cacheService;
            _dataOptions = dataOptions;
        }
        #endregion

        #region Methods

        public async Task<List<InstitutionModel>> GetAllInstitutions()
        {
            return await _sudyContext.Institutions
                .Take(_dataOptions.Take)
                .Skip(_dataOptions.Skip)
                .ApplyOrderBy(_dataOptions.KeyOrder, _dataOptions.Ordering)
                .ApplyTracking(_dataOptions.IsTracking)
                .ToListAsync();
        }


        public async Task<InstitutionModel> GetInstitutionById(int institutionId)
        {
            bool cache = !bool.Parse(AppSettings.GetKey(ConfigKeys.RedisCache));

            if (_dataOptions.IsTracking == true)
                cache = false;

            if (!cache) 
                return await _sudyContext.Institutions
                    .SingleOrDefaultAsync(x => x.institutionId == institutionId);

            string resultCache = await _cachingService.Get(nameof(InstitutionModel) + institutionId);

            if (!string.IsNullOrEmpty(resultCache))
                return JsonConvert.DeserializeObject<InstitutionModel>(resultCache);

            InstitutionModel? institution = await _sudyContext.Institutions
                .ApplyTracking(_dataOptions.IsTracking)
                .SingleOrDefaultAsync(x => x.institutionId == institutionId);

            if (institution != null)
                await _cachingService.Set(nameof(InstitutionModel) + institutionId, JsonConvert.SerializeObject(institution));

            return institution;
        }       

        #endregion
    }
}
