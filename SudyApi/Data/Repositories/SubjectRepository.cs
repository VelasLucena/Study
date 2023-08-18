using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StudandoApi.Data.Contexts;
using SudyApi.Data.Helper;
using SudyApi.Data.Interfaces;
using SudyApi.Models;
using SudyApi.Properties.Enuns;
using SudyApi.Utility;

namespace SudyApi.Data.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        #region Field

        private readonly SudyContext _sudyContext;
        private readonly ICacheService _cachingService;
        private readonly DataOptionsModel _dataOptions;

        #endregion

        #region Constructor

        public SubjectRepository(SudyContext sudyContext, ICacheService cacheService, DataOptionsModel dataOptions)
        {
            _sudyContext = sudyContext;
            _cachingService = cacheService;
            _dataOptions = dataOptions;
        }

        #endregion

        #region Methods

        public async Task<List<SubjectModel>> GetAllSubjects()
        {
            return await _sudyContext.Subjects
                .Include(x => x.Chapters)
                .Take(_dataOptions.Take)
                .Skip(_dataOptions.Skip)
                .ApplyOrderBy(_dataOptions.KeyOrder, _dataOptions.Ordering)
                .ApplyTracking(_dataOptions.IsTracking)
                .ToListAsync();
        }

        public async Task<SubjectModel> GetSubjectBySubjectId(int subjectId)
        {
            bool cache = bool.Parse(AppSettings.GetKey(ConfigKeys.RedisCache));

            if (_dataOptions.IsTracking == true)
                cache = false;

            if (!cache) 
                return await _sudyContext.Subjects
                    .Include(x => x.Chapters)
                    .SingleOrDefaultAsync(x => x.SubjectId == subjectId);

            string resultCache = await _cachingService.Get(nameof(SubjectModel) + subjectId);

            if (!string.IsNullOrEmpty(resultCache))
                return JsonConvert.DeserializeObject<SubjectModel>(resultCache);

            SubjectModel? subject = await _sudyContext.Subjects
                .Include(x => x.Chapters)
                .ApplyTracking(_dataOptions.IsTracking)
                .SingleOrDefaultAsync(x => x.SubjectId == subjectId);

            if (subject != null)
                await _cachingService.Set(nameof(SubjectModel) + subjectId, JsonConvert.SerializeObject(subject));

            return subject;
        }       

        public async Task<List<SubjectModel>> GetSubjectByDisciplineId(int disciplineId)
        {
            return await _sudyContext.Subjects
                .Include(x => x.Chapters)
                .Where(x => x.DisciplineId == disciplineId)
                .Take(_dataOptions.Take)
                .Skip(_dataOptions.Skip)
                .ApplyOrderBy(_dataOptions.KeyOrder, _dataOptions.Ordering)
                .ApplyTracking(_dataOptions.IsTracking)
                .ToListAsync();
        }       

        public async Task<SubjectModel> GetSubjectByNameFirst(string name)
        {
            return await _sudyContext.Subjects
                .Include(x => x.Chapters)
                .ApplyTracking(_dataOptions.IsTracking)
                .SingleOrDefaultAsync(x => x.Name.Contains(name));
        }

        #endregion

    }
}
