using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StudandoApi.Data.Contexts;
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

        public DisciplineNameRepository(SudyContext sudyContext, ICacheService cacheService)
        {
            _sudyContext = sudyContext;
            _cachingService = cacheService;
        }

        #region methods

        public async Task<List<DisciplineNameModel>> GetDisciplineNameByName(string name, int take = 10, int skip = 0)
        {
            return await _sudyContext.DisciplinesName
                .Where(x => x.Name.Contains(name))
                .Take(take)
                .Skip(skip)
                .ToListAsync();
        }

        public async Task<List<DisciplineNameModel>> GetDisciplineNameByNameNoTracking(string name, int take = 10, int skip = 0)
        {
            return await _sudyContext.DisciplinesName
                .AsNoTracking()
                .Where(x => x.Name.Contains(name))
                .Take(take)
                .Skip(skip)
                .ToListAsync();
        }

        public async Task<DisciplineNameModel> GetDisciplineNameById(int disciplineNameId)
        {
            return await _sudyContext.DisciplinesName.SingleOrDefaultAsync(x => x.DisciplineNameId == disciplineNameId);
        }

        public async Task<DisciplineNameModel> GetDisciplineNameByIdNoTracking(int disciplineNameId)
        {
            if (!bool.Parse(AppSettings.GetKey(ConfigKeys.RedisCache)))
                return await _sudyContext.DisciplinesName.SingleOrDefaultAsync(x => x.DisciplineNameId == disciplineNameId);

            string resultCache = await _cachingService.Get(nameof(DisciplineNameModel) + disciplineNameId);

            if (!string.IsNullOrEmpty(resultCache))
                return JsonConvert.DeserializeObject<DisciplineNameModel>(resultCache);

            DisciplineNameModel? discplineName = await _sudyContext.DisciplinesName.SingleOrDefaultAsync(x => x.DisciplineNameId == disciplineNameId);

            if (discplineName != null)
                await _cachingService.Set(nameof(DisciplineNameModel) + disciplineNameId, JsonConvert.SerializeObject(discplineName));

            return discplineName;
        }

        #endregion
    }
}
