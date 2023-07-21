using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StudandoApi.Data.Contexts;
using SudyApi.Data.Interfaces;
using SudyApi.Models;
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

        async Task<ChapterModel> IChapterRepository.GetChapterById(int chapterId)
        {
            return await _sudyContext.Chapters.FirstOrDefaultAsync(x => x.ChapterId == chapterId);
        }

        async Task<ChapterModel> IChapterRepository.GetChapterByIdNoTracking(int chapterId)
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

        async Task<List<ChapterModel>> IChapterRepository.GetAllChaptersBySubjectId(int subjectId)
        {
            return await _sudyContext.Chapters.Where(x => x.SubjectId == subjectId).ToListAsync();
        }

        async Task<List<ChapterModel>> IChapterRepository.GetAllChaptersBySubjectIdNoTracking(int subjectId)
        {
            return await _sudyContext.Chapters.AsNoTracking().Where(x => x.SubjectId == subjectId).ToListAsync();
        }

        #endregion

        #region GetChapterByName

        async Task<List<ChapterModel>> IChapterRepository.GetChapterByName(string name)
        {
            return await _sudyContext.Chapters.Where(x => x.Name.Contains(name)).ToListAsync();
        }

        async Task<List<ChapterModel>> IChapterRepository.GetChapterByNameNoTracking(string name)
        {
            return await _sudyContext.Chapters.AsNoTracking().Where(x => x.Name.Contains(name)).ToListAsync();
        }

        #endregion

        #endregion
    }
}
