using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StudandoApi.Data.Contexts;
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

        #endregion

        #region Constructor

        public DisciplineRepository(SudyContext sudyContext, ICacheService cacheService)
        {
            _sudyContext = sudyContext;
            _cacheService = cacheService;
        }

        #endregion

        #region Methods

        public async Task<List<DisciplineModel>> GetAllDisciplines(int semesterId, int take = 10, int skip = 0)
        {
            return await _sudyContext.Disciplines
                .Include(x => x.Name)
                .Take(take)
                .Skip(skip)
                .Where(x => x.SemesterId == semesterId).ToListAsync();
        }

        public async Task<List<DisciplineModel>> GetAllDisciplinesNoTracking(int semesterId, int take = 10, int skip = 0)
        {
            return await _sudyContext.Disciplines
                .Include(x => x.Name)
                .AsNoTracking()
                .Where(x => x.SemesterId == semesterId)
                .ToListAsync();
        }

        public async Task<DisciplineModel> GetDisciplineByName(int disciplineNameId, int semesterId)
        {
            return await _sudyContext.Disciplines
                .Include(x => x.Name)
                .SingleOrDefaultAsync(x => x.Name.DisciplineNameId == disciplineNameId && x.SemesterId == semesterId);
        }

        public async Task<DisciplineModel> GetDisciplineByNameNoTracking(int disciplineNameId, int semesterId)
        {
            return await _sudyContext.Disciplines
                .AsNoTracking()
                .Include(x => x.Name)
                .SingleOrDefaultAsync(x => x.Name.DisciplineNameId == disciplineNameId && x.SemesterId == semesterId);
        }

        public async Task<DisciplineModel> GetDisciplineById(int disciplineid)
        {
            return await _sudyContext.Disciplines
                .Include(x => x.Semester)
                .Include(x => x.Name)
                .SingleOrDefaultAsync(x => x.DisciplineId == disciplineid);
        }

        async Task<DisciplineModel> IDisciplineRepository.GetDisciplineByIdNoTracking(int disciplineid)
        {
            if (!bool.Parse(AppSettings.GetKey(ConfigKeys.RedisCache)))
                return await _sudyContext.Disciplines
                    .Include(x => x.Semester).Include(x => x.Name)
                    .SingleOrDefaultAsync(x => x.DisciplineId == disciplineid);

            string resultCache = await _cacheService.Get(nameof(DisciplineModel) + disciplineid);

            if (!string.IsNullOrEmpty(resultCache))
                return JsonConvert.DeserializeObject<DisciplineModel>(resultCache);

            DisciplineModel? discipline = await _sudyContext.Disciplines.Include(x => x.Semester).Include(x => x.Name).SingleOrDefaultAsync(x => x.DisciplineId == disciplineid);

            if (discipline != null)
                await _cacheService.Set(nameof(DisciplineModel) + disciplineid, JsonConvert.SerializeObject(discipline));

            return discipline;
        }

        public async Task<List<DisciplineModel>> GetDisciplinesBySemesterId(int semesterId, int take = 10, int skip = 0)
        {
            return await _sudyContext.Disciplines
                .Where(x => x.SemesterId == semesterId)
                .Take(take)
                .Skip(skip)
                .ToListAsync();
        }

        public async Task<List<DisciplineModel>> GetDisciplinesBySemesterIdNoTracking(int semesterId, int take = 10, int skip = 0)
        {
            return await _sudyContext.Disciplines
                .AsNoTracking()
                .Where(x => x.SemesterId == semesterId)
                .Take(take)
                .Skip(skip)
                .ToListAsync();
        }

        #endregion
    }
}

