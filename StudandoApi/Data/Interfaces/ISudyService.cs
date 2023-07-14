using SudyApi.Data.Interfaces;

namespace StudandoApi.Data.Interfaces
{
    public interface ISudyService
    {
        public IUserRepository UserRepository { get; }

        Task Create<T>(T user);

        Task Update<T>(T obj);

        Task UpdateMany<T>(List<T> obj);

        Task Delete<T>(T obj);

    }
}
