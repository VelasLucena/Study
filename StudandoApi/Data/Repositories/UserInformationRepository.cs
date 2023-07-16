using StudandoApi.Data.Contexts;
using SudyApi.Data.Interfaces;

namespace SudyApi.Data.Repositories
{
    public class UserInformationRepository : IUserInformationRepository
    {
        #region Field

        private readonly SudyContext _sudyContext;
        private readonly ICacheService _cachingService;

        #endregion

        #region Constructor

        public UserInformationRepository(SudyContext sudyContext, ICacheService cacheService)
        {
            _sudyContext = sudyContext;
            _cachingService = cacheService;
        }

        #endregion
    }
}
