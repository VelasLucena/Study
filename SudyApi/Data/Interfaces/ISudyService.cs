using SudyApi.Data.Interfaces;
using SudyApi.Models;

namespace StudandoApi.Data.Interfaces
{
    public interface ISudyService
    {
        public DataOptionsModel DataOptions { get; set; }

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

        Task Create<T>(List<T> user, bool removeCache = false, bool manualDesactiveCache = false);

        Task Update<T>(T obj, bool removeCache = false, bool manualDesactiveCache = false);

        Task Update<T>(List<T> obj, bool removeCache = false, bool manualDesactiveCache = false);

        Task Delete<T>(T obj, bool manualDesactiveCache = false);

        Task Delete<T>(List<T> obj, bool manualDesactiveCache = false);
    }
}
