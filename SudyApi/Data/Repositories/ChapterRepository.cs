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

        public async Task<ChapterModel> GetChapterById(int chapterId)
        {
            return await _sudyContext.Chapters
                .SingleOrDefaultAsync(x => x.ChapterId == chapterId);
        }

        public async Task<ChapterModel> GetChapterByIdNoTracking(int chapterId)
        {
            if (!bool.Parse(AppSettings.GetKey(ConfigKeys.RedisCache)))
                return await _sudyContext.Chapters
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.ChapterId == chapterId);

            string resultCache = await _cachingService.Get(nameof(ChapterModel) + chapterId);

            if (!string.IsNullOrEmpty(resultCache))
                return JsonConvert.DeserializeObject<ChapterModel>(resultCache);

            ChapterModel? chapter = await _sudyContext.Chapters.AsNoTracking().SingleOrDefaultAsync(x => x.ChapterId == chapterId);

            if (chapter != null)
                await _cachingService.Set(nameof(ChapterModel) + chapterId, JsonConvert.SerializeObject(chapter));
            else
                return null;

            return chapter;
        }

        public async Task<List<ChapterModel>> GetAllChaptersBySubjectId(int subjectId, int take = 10, int skip = 0)
        {
            return await _sudyContext.Chapters
                .Where(x => x.SubjectId == subjectId)
                .Take(take)
                .Skip(skip)
                .ToListAsync();
        }

        public async Task<List<ChapterModel>> GetAllChaptersBySubjectIdNoTracking(int subjectId, int take = 10, int skip = 0)
        {
            return await _sudyContext.Chapters
                .AsNoTracking()
                .Where(x => x.SubjectId == subjectId)
                .Take(take)
                .Skip(skip)
                .ToListAsync();
        }

        public async Task<List<ChapterModel>> GetChapterByName(string name, int take = 10, int skip = 0)
        {
            return await _sudyContext.Chapters
                .Where(x => x.Name.Contains(name))
                .Take(take)
                .Skip(skip)
                .ToListAsync();
        }

        public async Task<List<ChapterModel>> GetChapterByNameNoTracking(string name, int take = 10, int skip = 0)
        {
            return await _sudyContext.Chapters
                .AsNoTracking()
                .Where(x => x.Name.Contains(name))
                .Take(take)
                .Skip(skip)
                .ToListAsync();
        }

        #endregion
    }
}
