using StudandoApi.Data.Contexts;
using SudyApi.Data.Interfaces;

namespace SudyApi.Data.Repositories
{
    public class DisciplineNameRepository : IDisciplineNameRepository
    {
        private readonly SudyContext _sudyContext;
        private readonly ICacheService _cacheService;

        async Task<string> IDisciplineNameRepository.GetDisciplineNameByName(string name)
        {
            return await _
        }
    }
}
