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
    public class ImportanteDateRepository : IImportantDateRepository
    {
        private readonly SudyContext _sudyContext;
        private readonly ICacheService _cacheService;
        private readonly DataOptionsModel _dataOptions;

        public ImportanteDateRepository(SudyContext sudyContext, ICacheService cacheService, DataOptionsModel dataOptions)
        {
            _sudyContext = sudyContext;
            _cacheService = cacheService;
            _dataOptions = dataOptions;
        }

        public async Task<List<ImportantDateModel>> GetAllImportantDateBySemesterId(int semesterId)
        {
            return await _sudyContext.ImportantDates
                .Where(x => x.SemesterId == semesterId)
                .Take(_dataOptions.Take)
                .Skip(_dataOptions.Skip)
                .ApplyOrderBy(_dataOptions.KeyOrder, _dataOptions.Ordering)
                .ApplyTracking(_dataOptions.IsTracking)
                .ToListAsync();
        }

        public async Task<List<ImportantDateModel>> GetImportantDateByDate(DateOnly date)
        {
            return await _sudyContext.ImportantDates
                .Where(x => x.Date == date)
                .Take(_dataOptions.Take)
                .Skip(_dataOptions.Skip)
                .ApplyOrderBy(_dataOptions.KeyOrder, _dataOptions.Ordering)
                .ApplyTracking(_dataOptions.IsTracking)
                .ToListAsync();
        }

        public async Task<ImportantDateModel> GetImportantDateById(int importantDateId)
        {
            bool cache = bool.Parse(AppSettings.GetKey(ConfigKeys.RedisCache));

            if (_dataOptions.IsTracking == true)
                cache = false;

            if (!cache) 
                return await _sudyContext.ImportantDates
                    .SingleOrDefaultAsync(x => x.ImportantDateId == importantDateId);

            string resultCache = await _cacheService.Get(nameof(ImportantDateModel) + importantDateId);

            if (!string.IsNullOrEmpty(resultCache))
                return JsonConvert.DeserializeObject<ImportantDateModel>(resultCache);

            ImportantDateModel? importantDate = await _sudyContext.ImportantDates
                .ApplyTracking(_dataOptions.IsTracking)
                .SingleOrDefaultAsync(x => x.ImportantDateId == importantDateId);

            if (importantDate != null)
                await _cacheService.Set(nameof(ImportantDateModel) + importantDateId, JsonConvert.SerializeObject(importantDate));

            return importantDate;
        }   
    }
}
