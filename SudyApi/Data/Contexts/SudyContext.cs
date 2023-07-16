﻿using Microsoft.EntityFrameworkCore;
using StudandoApi.Models.User;
using SudyApi.Models.Subject;

namespace StudandoApi.Data.Contexts
{
    public class SudyContext : DbContext
    {

        public SudyContext(DbContextOptions<SudyContext> options) : base(options) { }

        public DbSet<UserModel> Users { get; set; }

        public DbSet<UserInformation> UsersInformation { get; set; }

        public DbSet<SubjectModel> Subjects { get; set; }

        public DbSet<ChapterModel> Chapters { get; set; }

        public DbSet<ChapterInformationModel> ChaptersInformation { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().HasIndex(x => x.Email).IsUnique();
            modelBuilder.Entity<UserInformation>().HasIndex(x => x.Cpf).IsUnique();
            modelBuilder.Entity<UserInformation>().ToTable("Users_Information");
        }
    }
}