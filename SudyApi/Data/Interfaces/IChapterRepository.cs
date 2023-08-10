using SudyApi.Models;

namespace SudyApi.Data.Interfaces
{
    public interface IChapterRepository
    {
        #region GetChapterByChapterId

        Task<ChapterModel> GetChapterById(int chapterId);

        Task<ChapterModel> GetChapterByIdNoTracking(int chapterId);

        #endregion

        #region GetAllChaptersBySubjectId

        Task<List<ChapterModel>> GetAllChaptersBySubjectId(int subjectId, int take = 10, int skip = 0);

        Task<List<ChapterModel>> GetAllChaptersBySubjectIdNoTracking(int subjectId, int take = 10, int skip = 0);

        #endregion

        #region GetChapterByName

        Task<List<ChapterModel>> GetChapterByName(string name, int take = 10, int skip = 0);

        Task<List<ChapterModel>> GetChapterByNameNoTracking(string name, int take = 10, int skip = 0);

        #endregion
    }
}
