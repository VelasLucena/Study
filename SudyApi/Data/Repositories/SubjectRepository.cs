﻿using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StudandoApi.Data.Contexts;
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

        #endregion

        #region Constructor

        public SubjectRepository(SudyContext sudyContext, ICacheService cacheService)
        {
            _sudyContext = sudyContext;
            _cachingService = cacheService;
        }

        #endregion

        #region Methods

        #region GetAllSubjects

        public async Task<List<SubjectModel>> GetAllSubjects(int limit, Ordering ordering, string attributeName)
        {
            switch (ordering)
            {
                case Ordering.Asc:
                    return await _sudyContext.Subjects.Include(x => x.Chapters).OrderBy(x => EF.Property<object>(x, attributeName)).Take(limit).ToListAsync();
                case Ordering.Desc:
                    return await _sudyContext.Subjects.Include(x => x.Chapters).OrderByDescending(x => EF.Property<object>(x, attributeName)).Take(limit).ToListAsync();
            }

            return null;
        }

        public async Task<List<SubjectModel>> GetAllSubjectsNoTracking(int limit, Ordering ordering, string attributeName)
        {
            switch (ordering)
            {
                case Ordering.Asc:
                    return await _sudyContext.Subjects.Include(x => x.Chapters).AsNoTracking().OrderBy(x => EF.Property<object>(x, attributeName)).Take(limit).ToListAsync();
                case Ordering.Desc:
                    return await _sudyContext.Subjects.Include(x => x.Chapters).AsNoTracking().OrderByDescending(x => EF.Property<object>(x, attributeName)).Take(limit).ToListAsync();
            }

            return null;
        }

        #endregion

        #region GetSubjectBySubjectId

        public async Task<SubjectModel> GetSubjectBySubjectId (int subjectId)
        {
            return await _sudyContext.Subjects.Include(x => x.Chapters).SingleOrDefaultAsync(x => x.SubjectId == subjectId);
        }

        public async Task<SubjectModel> GetSubjectBySubjectIdNoTracking(int subjectId)
        {
            if (!bool.Parse(AppSettings.GetKey(ConfigKeys.RedisCache)))
                return await _sudyContext.Subjects.Include(x => x.Chapters).AsNoTracking().SingleOrDefaultAsync(x => x.SubjectId == subjectId);

            string resultCache = await _cachingService.Get(nameof(SubjectModel) + subjectId);

            if (!string.IsNullOrEmpty(resultCache))
                return JsonConvert.DeserializeObject<SubjectModel>(resultCache);

            SubjectModel subject = await _sudyContext.Subjects.Include(x => x.Chapters).AsNoTracking().SingleOrDefaultAsync(x => x.SubjectId == subjectId);

            if (subject != null)
                await _cachingService.Set(nameof(SubjectModel) + subjectId, JsonConvert.SerializeObject(subject));

            return subject;
        }

        #endregion

        #region GetSubjectByCollegeSubjectId

        public async Task<List<SubjectModel>> GetSubjectByDisciplineId(int disciplineId)
        {
            return await _sudyContext.Subjects.Include(x => x.Chapters).Where(x => x.DisciplineId == disciplineId).ToListAsync();
        }

        public async Task<List<SubjectModel>> GetSubjectByDisciplineIdNoTracking(int disciplineId)
        {
            return await _sudyContext.Subjects.Include(x => x.Chapters).Where(x => x.DisciplineId == disciplineId).ToListAsync();
        }

        #endregion

        #region GetSubjectByName

        public async Task<SubjectModel> GetSubjectByNameFirst(string name)
        {
            return await _sudyContext.Subjects.Include(x => x.Chapters).SingleOrDefaultAsync(x => x.Name.Contains(name));
        }

        public async Task<SubjectModel> GetSubjectByNameFirstNoTracking(string name)
        {
            return await _sudyContext.Subjects.Include(x => x.Chapters).SingleOrDefaultAsync(x => x.Name.Contains(name));
        }

        #endregion

        #endregion
    }
}
