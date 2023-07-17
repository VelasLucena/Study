using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StudandoApi.Data.Contexts;
using StudandoApi.Models.User;
using SudyApi.Data.Interfaces;
using SudyApi.Models.Subject;
using SudyApi.Properties.Enuns;
using SudyApi.Utility;

namespace SudyApi.Data.Repositories
{
    public class ChapterRepository : IChapterRepository
    {
        #region Field

        private readonly SudyContext _sudyContext;
        private readonly ICacheService _cachingService;

        #endregion

        #region Constructor

        public ChapterRepository(SudyContext sudyContext, ICacheService cacheService)
        {
            _sudyContext = sudyContext;
            _cachingService = cacheService;
        }

        #endregion

        #region Methods

        #region GetChapterByChapterId

        public async Task<ChapterModel> GetChapterById(int chapterId)
        {
            if (!bool.Parse(AppSettings.GetKey(ConfigKeys.RedisCache)))
                return await _sudyContext.Chapters.FirstOrDefaultAsync(x => x.ChapterId == chapterId);

            string resultCache = await _cachingService.Get(nameof(ChapterModel) + chapterId);

            if (!string.IsNullOrEmpty(resultCache))
                return JsonConvert.DeserializeObject<ChapterModel>(resultCache);

            ChapterModel chapter = await _sudyContext.Chapters.FirstOrDefaultAsync(x => x.ChapterId == chapterId);

            if (chapter != null)
                await _cachingService.Set(nameof(ChapterModel) + chapterId, JsonConvert.SerializeObject(chapter));

            return chapter;
        }

        public async Task<ChapterModel> GetChapterByIdNoTracking(int chapterId)
        {
            if (!bool.Parse(AppSettings.GetKey(ConfigKeys.RedisCache)))
                return await _sudyContext.Chapters.AsNoTracking().FirstOrDefaultAsync(x => x.ChapterId == chapterId);

            string resultCache = await _cachingService.Get(nameof(ChapterModel) + chapterId);

            if (!string.IsNullOrEmpty(resultCache))
                return JsonConvert.DeserializeObject<ChapterModel>(resultCache);

            ChapterModel chapter = await _sudyContext.Chapters.AsNoTracking().FirstOrDefaultAsync(x => x.ChapterId == chapterId);

            if (chapter != null)
                await _cachingService.Set(nameof(ChapterModel) + chapterId, JsonConvert.SerializeObject(chapter));

            return chapter;
        }

        #endregion

        #region GetChapterBySubjectId

        public async Task<List<ChapterModel>> GetAllChaptersBySubjectId(int subjectId)
        {
            return await _sudyContext.Chapters.Where(x => x.Subject.SubjectId == subjectId).ToListAsync();
        }

        public async Task<List<ChapterModel>> GetAllChaptersBySubjectIdNoTracking(int subjectId)
        {
            return await _sudyContext.Chapters.AsNoTracking().Where(x => x.Subject.SubjectId == subjectId).ToListAsync();
        }

        #endregion

        #region GetChaptersByIds

        public async Task<List<ChapterModel>> GetChaptersByIds(int?[] chapterIds)
        {
            return await _sudyContext.Chapters.Where(x => chapterIds.Contains(x.ChapterId)).ToListAsync();
        }

        public async Task<List<ChapterModel>> GetChaptersByIdsNoTracking(int?[] chapterIds)
        {
            return await _sudyContext.Chapters.AsNoTracking().Where(x => chapterIds.Contains(x.ChapterId)).ToListAsync();
        }

        #endregion

        #endregion
    }
}
