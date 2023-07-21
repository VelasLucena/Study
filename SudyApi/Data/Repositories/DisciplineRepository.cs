using Microsoft.EntityFrameworkCore;
using StudandoApi.Data.Contexts;
using SudyApi.Data.Interfaces;
using SudyApi.Data.Services;
using SudyApi.Models;

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

        #endregion

        #endregion
    }
}
