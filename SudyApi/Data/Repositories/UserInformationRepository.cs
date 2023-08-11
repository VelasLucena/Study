using StudandoApi.Data.Contexts;
using SudyApi.Data.Interfaces;
using SudyApi.Models;

namespace SudyApi.Data.Repositories
{
    public class UserInformationRepository : IUserInformationRepository
    {
        #region Field

        private readonly SudyContext _sudyContext;
        private readonly ICacheService _cachingService;
        private readonly DataOptionsModel _dataOptions;

        #endregion

        #region Constructor

        public UserInformationRepository(SudyContext sudyContext, ICacheService cacheService, DataOptionsModel dataOptions)
        {
            _sudyContext = sudyContext;
            _cachingService = cacheService;
            _dataOptions = dataOptions;
        }

        #endregion
    }
}
