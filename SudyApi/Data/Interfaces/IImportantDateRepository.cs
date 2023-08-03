﻿using SudyApi.Models;
using SudyApi.Properties.Enuns;

namespace SudyApi.Data.Interfaces
{
    public interface IImportantDateRepository
    {
        Task<List<ImportantDateModel>> GetAllImportantDateBySemesterId(int semesterId);

        Task<List<ImportantDateModel>> GetAllImportantDateBySemesterIdNoTracking(int semesterId);

        Task<ImportantDateModel> GetImportantDateById(int importantDateId);

        Task<ImportantDateModel> GetImportantDateByIdNoTracking(int importantDateId);

        Task<List<ImportantDateModel>> GetImportantDateByDate(DateOnly date);

        Task<List<ImportantDateModel>> GetImportantDateByDateNoTracking(DateOnly date);

    }
}
