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
    public class DisciplineNameRepository : IDisciplineNameRepository
    {
        private readonly SudyContext _sudyContext;
        private readonly ICacheService _cachingService;
        private readonly DataOptionsModel _dataOptions;

        public DisciplineNameRepository(SudyContext sudyContext, ICacheService cacheService, DataOptionsModel dataOptions)
        {
            _sudyContext = sudyContext;
            _cachingService = cacheService;
            _dataOptions = dataOptions;
        }

        #region methods

        public async Task<List<DisciplineNameModel>> GetDisciplineNameByName(string name)
        {
            return await _sudyContext.DisciplinesName
                .Where(x => x.Name.Contains(name))
                .Take(_dataOptions.Take)
                .Skip(_dataOptions.Skip)
                .ApplyOrderBy(_dataOptions.KeyOrder, _dataOptions.Ordering)
                .ApplyTracking(_dataOptions.IsTracking)
                .ToListAsync();
        }

        public async Task<DisciplineNameModel> GetDisciplineNameById(int disciplineNameId)
        {
            bool cache = !bool.Parse(AppSettings.GetKey(ConfigKeys.RedisCache));

            if (_dataOptions.IsTracking == true)
                cache = false;

            if (!cache)
                return await _sudyContext.DisciplinesName
                    .SingleOrDefaultAsync(x => x.DisciplineNameId == disciplineNameId);

            string resultCache = await _cachingService.Get(nameof(DisciplineNameModel) + disciplineNameId);

            if (!string.IsNullOrEmpty(resultCache))
                return JsonConvert.DeserializeObject<DisciplineNameModel>(resultCache);

            DisciplineNameModel? discplineName = await _sudyContext.DisciplinesName
                .ApplyTracking(_dataOptions.IsTracking)
                .SingleOrDefaultAsync(x => x.DisciplineNameId == disciplineNameId);

            if (discplineName != null)
                await _cachingService.Set(nameof(DisciplineNameModel) + disciplineNameId, JsonConvert.SerializeObject(discplineName));

            return discplineName;
        }       

        #endregion
    }
}
