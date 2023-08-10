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

        public async Task<List<ImportantDateModel>> GetAllImportantDateBySemesterId(int semesterId, int take = 10, int skip = 0)
        {
            return await _sudyContext.ImportantDates
                .Where(x => x.SemesterId == semesterId)
                .Take(take)
                .Skip(skip)
                .ToListAsync();
        }

        public async Task<List<ImportantDateModel>> GetAllImportantDateBySemesterIdNoTracking(int semesterId, int take = 10, int skip = 0)
        {
            return await _sudyContext.ImportantDates
                .AsNoTracking()
                .Where(x => x.SemesterId == semesterId)
                .Take(take)
                .Skip(skip)
                .ToListAsync();
        }

        public async Task<List<ImportantDateModel>> GetImportantDateByDate(DateOnly date, int take = 10, int skip = 0)
        {
            return await _sudyContext.ImportantDates
                .Where(x => x.Date == date)
                .Take(take)
                .Skip(skip)
                .ToListAsync();
        }

        public async Task<List<ImportantDateModel>> GetImportantDateByDateNoTracking(DateOnly date, int take = 10, int skip = 0)
        {
            return await _sudyContext.ImportantDates
                .AsNoTracking()
                .Where(x => x.Date == date)
                .Take(take)
                .Skip(skip)
                .ToListAsync();
        }

        public async Task<ImportantDateModel> GetImportantDateById(int importantDateId)
        {
            return await _sudyContext.ImportantDates.SingleOrDefaultAsync(x => x.ImportantDateId == importantDateId);
        }

        public async Task<ImportantDateModel> GetImportantDateByIdNoTracking(int importantDateId)
        {
            if (!bool.Parse(AppSettings.GetKey(ConfigKeys.RedisCache)))
                return await _sudyContext.ImportantDates.SingleOrDefaultAsync(x => x.ImportantDateId == importantDateId);

            string resultCache = await _cacheService.Get(nameof(ImportantDateModel) + importantDateId);

            if (!string.IsNullOrEmpty(resultCache))
                return JsonConvert.DeserializeObject<ImportantDateModel>(resultCache);

            ImportantDateModel? user = await _sudyContext.ImportantDates.SingleOrDefaultAsync(x => x.ImportantDateId == importantDateId);

            if (user != null)
                await _cacheService.Set(nameof(ImportantDateModel) + importantDateId, JsonConvert.SerializeObject(user));

            return user;
        }
    }
}
