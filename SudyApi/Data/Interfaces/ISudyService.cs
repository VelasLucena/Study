using SudyApi.Data.Interfaces;

namespace StudandoApi.Data.Interfaces
{
    public interface ISudyService
    {
        public IUserRepository UserRepository { get; }

        public IChapterRepository ChapterRepository { get; }

        public ISubjectRepository SubjectRepository { get; }

        public IUserInformationRepository UserInformationRepository { get; }

        Task Create<T>(T user);

        Task Update<T>(T obj);

        Task UpdateMany<T>(List<T> obj);

        Task Delete<T>(T obj);

    }
}
