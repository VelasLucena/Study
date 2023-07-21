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

        #region GetAllCollegeSubjects

        async Task<List<DisciplineModel>> IDisciplineRepository.GetAllDisciplines(int semesterId)
        {
            return await _sudyContext.Disciplines.Where(x => x.SemesterId == semesterId).ToListAsync();
        }

        async Task<List<DisciplineModel>> IDisciplineRepository.GetAllDisciplinesNoTracking(int semesterId)
        {
            return await _sudyContext.Disciplines.AsNoTracking().Where(x => x.SemesterId == semesterId).ToListAsync();
        }

        #region GetDisciplinesByName

        Task<List<DisciplineModel>> IDisciplineRepository.GetDisciplinesByName(string name)
        {
            throw new NotImplementedException();
        }

        Task<List<DisciplineModel>> IDisciplineRepository.GetDisciplinesByNameNoTracking(string name)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region GetDisciplineById

        async Task<DisciplineModel> IDisciplineRepository.GetDisciplineById(int disciplineid)
        {
            return await _sudyContext.Disciplines.Include(x => x.Semester).Include(x => x.Name).FirstOrDefaultAsync(x => x.DisciplineId == disciplineid);
        }

        async Task<DisciplineModel> IDisciplineRepository.GetDisciplineByIdNoTracking(int disciplineid)
        {
            if (!bool.Parse(AppSettings.GetKey(ConfigKeys.RedisCache)))
                return await _sudyContext.Disciplines.Include(x => x.Semester).Include(x => x.Name).FirstOrDefaultAsync(x => x.DisciplineId == disciplineid);

            string resultCache = await _cacheService.Get(nameof(DisciplineModel) + disciplineid);

            if (!string.IsNullOrEmpty(resultCache))
                return JsonConvert.DeserializeObject<DisciplineModel>(resultCache);

            DisciplineModel discipline = await _sudyContext.Disciplines.Include(x => x.Semester).Include(x => x.Name).FirstOrDefaultAsync(x => x.DisciplineId == disciplineid);

            if (discipline != null)
                await _cacheService.Set(nameof(DisciplineModel) + disciplineid, JsonConvert.SerializeObject(discipline));

            return discipline;
        }

        #endregion

        #endregion

        #endregion
    }
}
