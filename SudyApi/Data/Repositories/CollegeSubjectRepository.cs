using Microsoft.EntityFrameworkCore;
using StudandoApi.Data.Contexts;
using SudyApi.Data.Interfaces;
using SudyApi.Data.Services;
using SudyApi.Models;

namespace SudyApi.Data.Repositories
{
    public class CollegeSubjectRepository : ICollegeSubjectRepository
    {
        private readonly SudyContext _sudyContext;
        private readonly ICacheService _cacheService;

        public CollegeSubjectRepository(SudyContext sudyContext, ICacheService cacheService)
        {
            _sudyContext = sudyContext;
            _cacheService = cacheService;
        }

        async Task<List<CollegeSubjectModel>> ICollegeSubjectRepository.GetAllCollegeSubjects(int semesterId)
        {
            return await _sudyContext.CollegeSubjects.Where(x => x.SemesterId == semesterId).ToListAsync();
        }

        async Task<List<CollegeSubjectModel>> ICollegeSubjectRepository.GetAllCollegeSubjectsNoTracking(int semesterId)
        {
            return await _sudyContext.CollegeSubjects.AsNoTracking().Where(x => x.SemesterId == semesterId).ToListAsync();
        }
    }
}
