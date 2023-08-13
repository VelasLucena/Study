using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StudandoApi.Data.Contexts;
using SudyApi.Data.Helper;
using SudyApi.Data.Interfaces;
using SudyApi.Models;
using SudyApi.Properties.Enuns;
using SudyApi.Utility;
using System.Linq;

namespace SudyApi.Data.Repositories
{
    public class ChapterRepository : IChapterRepository
    {
        #region Field

        private readonly SudyContext _sudyContext;
        private readonly ICacheService _cachingService;
        private readonly DataOptionsModel _dataOptions;

        #endregion

        #region Constructor

        public ChapterRepository(SudyContext sudyContext, ICacheService cacheService, DataOptionsModel dataOptions)
        {
            _sudyContext = sudyContext;
            _cachingService = cacheService;
            _dataOptions = dataOptions;
        }

        #endregion

        #region Methods

        public async Task<ChapterModel> GetChapterById(int chapterId)
        {
            bool cache = !bool.Parse(AppSettings.GetKey(ConfigKeys.RedisCache));

            if (_dataOptions.IsTracking == true)
                cache = false;

            if (!cache)
                return await _sudyContext.Chapters
                    .SingleOrDefaultAsync(x => x.ChapterId == chapterId);

            string resultCache = await _cachingService.Get(nameof(ChapterModel) + chapterId);

            if (!string.IsNullOrEmpty(resultCache))
                return JsonConvert.DeserializeObject<ChapterModel>(resultCache);

            ChapterModel? chapter = await _sudyContext.Chapters
                .ApplyTracking(_dataOptions.IsTracking)
                .SingleOrDefaultAsync(x => x.ChapterId == chapterId);

            if (chapter != null)
                await _cachingService.Set(nameof(ChapterModel) + chapterId, JsonConvert.SerializeObject(chapter));

            return chapter;
        }
        
        public async Task<List<ChapterModel>> GetAllChaptersBySubjectId(int subjectId)
        {
            return await _sudyContext.Chapters
                .Where(x => x.SubjectId == subjectId)
                .Take(_dataOptions.Take)
                .Skip(_dataOptions.Skip)
                .ApplyOrderBy(_dataOptions.KeyOrder, _dataOptions.Ordering)
                .ApplyTracking(_dataOptions.IsTracking)
                .ToListAsync();
        }

        public async Task<List<ChapterModel>> GetChapterByName(string name)
        {
            return await _sudyContext.Chapters
                .Where(x => x.Name.Contains(name))
                .Take(_dataOptions.Take)
                .Skip(_dataOptions.Skip)
                .ApplyOrderBy(_dataOptions.KeyOrder, _dataOptions.Ordering)
                .ApplyTracking(_dataOptions.IsTracking)
                .ToListAsync();
        }

        #endregion
    }
}
