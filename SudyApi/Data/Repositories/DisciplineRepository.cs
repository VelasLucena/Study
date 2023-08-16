using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StudandoApi.Data.Contexts;
using SudyApi.Data.Helper;
using SudyApi.Data.Interfaces;
using SudyApi.Data.Services;
using SudyApi.Models;
using SudyApi.Properties.Enuns;
using SudyApi.Utility;

namespace SudyApi.Data.Repositories
{
    public class DisciplineRepository : IDisciplineRepository
    {
        #region Field

        private readonly SudyContext _sudyContext;
        private readonly ICacheService _cacheService;
        private readonly DataOptionsModel _dataOptions;

        #endregion

        #region Constructor

        public DisciplineRepository(SudyContext sudyContext, ICacheService cacheService, DataOptionsModel dataOptions)
        {
            _sudyContext = sudyContext;
            _cacheService = cacheService;
            _dataOptions = dataOptions;
        }

        #endregion

        #region Methods

        public async Task<List<DisciplineModel>> GetAllDisciplines(int semesterId)
        {
            return await _sudyContext.Disciplines
                .Include(x => x.DisciplineName)
                .Include(x => x.Semester)
                .Take(_dataOptions.Take)
                .Skip(_dataOptions.Skip)
                .ApplyOrderBy(_dataOptions.KeyOrder, _dataOptions.Ordering)
                .ApplyTracking(_dataOptions.IsTracking)
                .Where(x => x.SemesterId == semesterId).ToListAsync();
        }

        public async Task<DisciplineModel> GetDisciplineByName(int disciplineNameId, int semesterId)
        {
            return await _sudyContext.Disciplines
                .Include(x => x.DisciplineName)
                .Include(x => x.Semester)
                .ApplyTracking(_dataOptions.IsTracking)
                .SingleOrDefaultAsync(x => x.DisciplineName.DisciplineNameId == disciplineNameId && x.SemesterId == semesterId);
        }      

        public async Task<DisciplineModel> GetDisciplineById(int disciplineid)
        {
            bool cache = !bool.Parse(AppSettings.GetKey(ConfigKeys.RedisCache));

            if (_dataOptions.IsTracking == true)
                cache = false;

            if (!cache)
                return await _sudyContext.Disciplines
                    .Include(x => x.Semester)
                    .Include(x => x.DisciplineName)
                    .SingleOrDefaultAsync(x => x.DisciplineId == disciplineid);

            string resultCache = await _cacheService.Get(nameof(DisciplineModel) + disciplineid);

            if (!string.IsNullOrEmpty(resultCache))
                return JsonConvert.DeserializeObject<DisciplineModel>(resultCache);

            DisciplineModel? discipline = await _sudyContext.Disciplines
                    .Include(x => x.Semester)
                    .Include(x => x.DisciplineName)
                    .ApplyTracking(_dataOptions.IsTracking)
                    .SingleOrDefaultAsync(x => x.DisciplineId == disciplineid);

            if (discipline != null)
                await _cacheService.Set(nameof(DisciplineModel) + disciplineid, JsonConvert.SerializeObject(discipline));

            return discipline;
        }     

        public async Task<List<DisciplineModel>> GetDisciplinesBySemesterId(int semesterId)
        {
            return await _sudyContext.Disciplines
                .Where(x => x.SemesterId == semesterId)
                .Take(_dataOptions.Take)
                .Skip(_dataOptions.Skip)
                .ApplyOrderBy(_dataOptions.KeyOrder, _dataOptions.Ordering)
                .ApplyTracking(_dataOptions.IsTracking)
                .Include(x => x.DisciplineName)
                .Include(x => x.Semester)
                .ToListAsync();
        }

        #endregion
    }
}

