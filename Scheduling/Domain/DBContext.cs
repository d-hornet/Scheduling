﻿using Microsoft.EntityFrameworkCore;
using Scheduling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheduling.Domain
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<UserTeams> userTeams { get; set; }
        public DbSet<VacationRequest> VacationRequests { get; set; }
        public DbSet<VacationResponse> VacationResponses { get; set; }
        public DbSet<CalendarEvent> CalendarEvents { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<TimerHistory> TimerHistories { get; set; }
        public DbSet<UserTimerHistory> UserTimerHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1321313,
                Email = "admin@gmail.com",
                Password = "5dj3bhWCfxuHmONkBdvFrA==",
                Name = "Denis",
                Surname = "Pensiya",
                Position = "Nachalnik",
                Department = "Nachalstvo",
                Salt = "91ed90df-3289-4fdf-a927-024b24bea8b7",
            });

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1321314,
                Email = "admin2@gmail.com",
                Password = "5dj3bhWCfxuHmONkBdvFrA==",
                Name = "Arkadiy",
                Surname = "Cisterna",
                Position = "Pochti nachalstvo",
                Department = "Nachalstvo",
                Salt = "91ed90df-3289-4fdf-a927-024b24bea8b7",
            });

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1321315,
                Email = "admin3@gmail.com",
                Password = "5dj3bhWCfxuHmONkBdvFrA==",
                Name = "Tolik",
                Surname = "Balkon",
                Position = "Pochti nachalstvo",
                Department = "Nachalstvo",
                Salt = "91ed90df-3289-4fdf-a927-024b24bea8b7",
            });

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 13213133,
                Email = "user@gmail.com",
                Password = "u9DAYiHl+liIqRMvuuciBA==",
                Name = "Yarik",
                Surname = "Obichniy",
                Position = "lol",
                Department = "Memes",
                Salt = "f0e30e73-fac3-4182-8641-ecba862fed69",
            });

            modelBuilder.Entity<Permission>().HasData(new Permission { 
                Id = 1,
                Name = "Manager"
            });

            modelBuilder.Entity<Permission>().HasData(new Permission { 
                Id = 2,
                Name = "Accountant"
            });
            
            modelBuilder.Entity<Permission>().HasData(new Permission { 
                Id = 3,
                Name = "Part-time"
            });
            
            modelBuilder.Entity<Permission>().HasData(new Permission { 
                Id = 4,
                Name = "Full-time"
            });
            
            modelBuilder.Entity<Permission>().HasData(new Permission { 
                Id = 5,
                Name = "Access to reports"
            });
            
            modelBuilder.Entity<Permission>().HasData(new Permission { 
                Id = 6,
                Name = "Access to calendar"
            });

            modelBuilder.Entity<UserPermission>().HasData(new UserPermission
            {
                Id = 1,
                PermisionId = 3,
                UserId = 13213133
            });

            modelBuilder.Entity<UserPermission>().HasData(new UserPermission
            { 
                Id = 2,
                PermisionId = 1,
                UserId = 1321313
            });
            
            modelBuilder.Entity<UserPermission>().HasData(new UserPermission
            { 
                Id = 3,
                PermisionId = 2,
                UserId = 1321313
            });

            modelBuilder.Entity<UserPermission>().HasData(new UserPermission
            {
                Id = 4,
                PermisionId = 1,
                UserId = 1321314
            });

            modelBuilder.Entity<UserPermission>().HasData(new UserPermission
            {
                Id = 5,
                PermisionId = 2,
                UserId = 1321314
            });

            modelBuilder.Entity<UserPermission>().HasData(new UserPermission
            {
                Id = 6,
                PermisionId = 1,
                UserId = 1321315
            });

            modelBuilder.Entity<UserPermission>().HasData(new UserPermission
            {
                Id = 7,
                PermisionId = 2,
                UserId = 1321315
            });

            modelBuilder.Entity<Team>().HasData(new Team
            { 
                Id = 6,
                CreatorId = 1321313,
                Name = "Development"
            });

            modelBuilder.Entity<UserTeams>().HasData(new UserTeams
            {
                Id = 1,
                UserId = 13213133,
                TeamId = 6
            });

            modelBuilder.Entity<VacationRequest>().HasData(new VacationRequest
            {
                Id = 15001,
                UserId = 13213133,
                UserName = "Yarik Obichniy",
                StartDate = new DateTime(2021, 04, 20),
                FinishDate = new DateTime(2021, 05, 20),
                Comment = "I want to see a bober.",
                Status = VacationRequest.StatusType.Declined
            });

            modelBuilder.Entity<VacationRequest>().HasData(new VacationRequest
            {
                Id = 15002,
                UserId = 13213133,
                UserName = "Yarik Obichniy",
                StartDate = new DateTime(2021, 04, 22),
                FinishDate = new DateTime(2021, 04, 28),
                Comment = "I really want to see a bober.",
                Status = VacationRequest.StatusType.PendingConsideration
            });

            modelBuilder.Entity<VacationRequest>().HasData(new VacationRequest
            {
                Id = 15003,
                UserId = 13213133,
                UserName = "Yarik Obichniy",
                StartDate = new DateTime(2021, 04, 25),
                FinishDate = new DateTime(2021, 04, 29),
                Comment = "Please, it`s my dream to see a bober.",
                Status = VacationRequest.StatusType.PendingConsideration
            });

            modelBuilder.Entity<VacationResponse>().HasData(new VacationResponse
            {
                Id = 6001,
                RequestId = 15001,
                ResponderId = 1321313,
                ResponderName = "Denis Pensiya",
                Response = true,
                Comment = "No problem. Let`s go!"
            });

            modelBuilder.Entity<VacationResponse>().HasData(new VacationResponse
            {
                Id = 6002,
                RequestId = 15001,
                ResponderId = 1321314,
                ResponderName = "Arkadiy Cisterna",
                Response = true,
                Comment = "Oke. Goodbye :((((((((("
            });

            modelBuilder.Entity<VacationResponse>().HasData(new VacationResponse
            {
                Id = 6003,
                RequestId = 15001,
                ResponderId = 1321315,
                ResponderName = "Tolik Balkon",
                Response = false,
                Comment = "Nea. Sidi tut!"
            });

            modelBuilder.Entity<TimerHistory>().HasData(new TimerHistory
            {
                Id = 1,
                StartTime = new DateTime(2021, 1, 1, 1, 1, 1),
                FinishTime = new DateTime(2021, 1, 1, 1, 1, 2)
            });

            modelBuilder.Entity<UserTimerHistory>().HasData(new UserTimerHistory
            {
                Id = 1,
                TimerHistoryId = 1,
                UserId = 1321313
            });

            modelBuilder.Entity<CalendarEvent>().HasData(new CalendarEvent
            {
                Id = 1,
                UserId = 13213133,
                WorkDate = new DateTime(2021, 5, 15),
                StartWorkTime = new DateTime(2021, 5, 15, 9, 00, 00),
                EndWorkTime = new DateTime(2021, 5, 15, 18, 00, 00)
            });
        }
    }
}
