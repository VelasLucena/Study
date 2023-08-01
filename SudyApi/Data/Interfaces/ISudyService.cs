using SudyApi.Data.Interfaces;

namespace StudandoApi.Data.Interfaces
{
    public interface ISudyService
    {
        public IUserRepository UserRepository { get; }

        public IChapterRepository ChapterRepository { get; }

        public ISubjectRepository SubjectRepository { get; }

        public IUserInformationRepository UserInformationRepository { get; }

        public ISemesterRepository SemesterRepository { get; }

        public ICourseRepository CourseRepository { get; }

        public IInstitutionRepository InstitutionRepository { get; }

        public IDisciplineRepository DisciplineRepository { get; }

        public IDisciplineNameRepository DisciplineNameRepository { get; }

        public IImportantDateRepository ImportanteDateRepository { get; }

        Task Create<T>(T user, bool removeCache = false, bool manualDesactiveCache = false);

        Task CreateMany<T>(List<T> user, bool removeCache = false, bool manualDesactiveCache = false);

        Task Update<T>(T obj, bool removeCache = false, bool manualDesactiveCache = false);

        Task UpdateMany<T>(List<T> obj, bool removeCache = false, bool manualDesactiveCache = false);

        Task Delete<T>(T obj, bool manualDesactiveCache = false);

        Task DeleteMany<T>(List<T> obj, bool manualDesactiveCache = false);
    }
}
