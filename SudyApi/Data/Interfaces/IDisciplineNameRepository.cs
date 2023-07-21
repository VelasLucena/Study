namespace SudyApi.Data.Interfaces
{
    public interface IDisciplineNameRepository
    {
        Task<string> GetDisciplineNameByName(string name);
    }
}
