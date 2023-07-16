using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StudandoApi.Data.Contexts;
using SudyApi.Data.Interfaces;
using SudyApi.Models.Subject;
using SudyApi.Properties.Enuns;
using SudyApi.Utility;

namespace SudyApi.Data.Repositories
{
    public class ChaptersInformationRepository : IChaptersInformationRepository
    {
        #region Field

        private readonly SudyContext _sudyContext;
        private readonly ICacheService _cachingService;

        #endregion

        #region Constructor

        public ChaptersInformationRepository(SudyContext sudyContext, ICacheService cacheService)
        {
            _sudyContext = sudyContext;
            _cachingService = cacheService;
        }

        #endregion

        #region Methods

        #region ChaptersInformationByChaptersInformationId

        public async Task<ChapterInformationModel> ChaptersInformationById(int chaptersInformationId)
        {
            if (!bool.Parse(AppSettings.GetKey(ConfigKeys.RedisCache)))
                return await _sudyContext.ChaptersInformation.FirstOrDefaultAsync(x => x.SubjectId == subjectId);

            string resultCache = await _cachingService.Get(nameof(ChapterInformationModel) + subjectId);

            if (!string.IsNullOrEmpty(resultCache))
                return JsonConvert.DeserializeObject<ChapterInformationModel>(resultCache);

            ChapterInformationModel subject = await _sudyContext.Subjects.Include(x => x.User).FirstOrDefaultAsync(x => x.SubjectId == subjectId);

            if (subject != null)
                await _cachingService.Set(nameof(ChapterInformationModel) + subjectId, JsonConvert.SerializeObject(subject));

            return subject;
        }

        public async Task<SubjectModel> GetSubjectBySubjectIdNoTracking(int subjectId)
        {
            if (!bool.Parse(AppSettings.GetKey(ConfigKeys.RedisCache)))
                return await _sudyContext.Subjects.Include(x => x.User).AsNoTracking().FirstOrDefaultAsync(x => x.SubjectId == subjectId);

            string resultCache = await _cachingService.Get(nameof(SubjectModel) + subjectId);

            if (!string.IsNullOrEmpty(resultCache))
                return JsonConvert.DeserializeObject<SubjectModel>(resultCache);

            SubjectModel subject = await _sudyContext.Subjects.Include(x => x.User).AsNoTracking().FirstOrDefaultAsync(x => x.SubjectId == subjectId);

            if (subject != null)
                await _cachingService.Set(nameof(SubjectModel) + subjectId, JsonConvert.SerializeObject(subject));

            return subject;
        }

        #endregion
        #endregion
    }
}
