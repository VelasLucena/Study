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
    public class ImportanteDateRepository : IImportantDateRepository
    {
        private readonly SudyContext _sudyContext;
        private readonly ICacheService _cacheService;

        public ImportanteDateRepository(SudyContext sudyContext, ICacheService cacheService)
        {
            _sudyContext = sudyContext;
            _cacheService = cacheService;
        }

        async Task<List<ImportantDateModel>> IImportantDateRepository.GetAllImportantDateBySemesterId(int semesterId)
        {
            return await _sudyContext.ImportantDates.Where(x => x.SemesterId == semesterId).ToListAsync();
        }

        async Task<List<ImportantDateModel>> IImportantDateRepository.GetAllImportantDateBySemesterIdNoTracking(int semesterId)
        {
            return await _sudyContext.ImportantDates.AsNoTracking().Where(x => x.SemesterId == semesterId).ToListAsync();
        }

        async Task<List<ImportantDateModel>> IImportantDateRepository.GetImportantDateByDate(DateOnly date)
        {
            return await _sudyContext.ImportantDates.Where(x => x.Date == date).ToListAsync();
        }

        async Task<List<ImportantDateModel>> IImportantDateRepository.GetImportantDateByDateNoTracking(DateOnly date)
        {
            return await _sudyContext.ImportantDates.AsNoTracking().Where(x => x.Date == date).ToListAsync();
        }

        async Task<ImportantDateModel> IImportantDateRepository.GetImportantDateById(int importantDateId)
        {
            return await _sudyContext.ImportantDates.FirstOrDefaultAsync(x => x.ImportantDateId == importantDateId);
        }

        async Task<ImportantDateModel> IImportantDateRepository.GetImportantDateByIdNoTracking(int importantDateId)
        {
            if (!bool.Parse(AppSettings.GetKey(ConfigKeys.RedisCache)))
                return await _sudyContext.ImportantDates.FirstOrDefaultAsync(x => x.ImportantDateId == importantDateId);

            string resultCache = await _cacheService.Get(nameof(ImportantDateModel) + importantDateId);

            if (!string.IsNullOrEmpty(resultCache))
                return JsonConvert.DeserializeObject<ImportantDateModel>(resultCache);

            ImportantDateModel user = await _sudyContext.ImportantDates.FirstOrDefaultAsync(x => x.ImportantDateId == importantDateId);

            if (user != null)
                await _cacheService.Set(nameof(ImportantDateModel) + importantDateId, JsonConvert.SerializeObject(user));

            return user;
        }
    }
}
