﻿using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class Db : DbContext
    {
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<SComp> Components { get; set; }
        public DbSet<AnwserModule> CompModules { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {


            //  optionsBuilder.UseSqlServer("Server=DESKTOP-G01L98C;Database=ChatApi;Integrated Security=SSPI;TrustServerCertificate=True;").UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            optionsBuilder.UseSqlServer("Server=DESKTOP-J3SBDBO\\SQLEXPRESS;Database=Survey;Integrated Security=SSPI;TrustServerCertificate=True;").UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SComp>()
         .HasMany(x => x.SingleAnwser)
         .WithOne(r => r.CompSingle)
         .HasForeignKey(r => r.CompSingleId);



            modelBuilder.Entity<SComp>()
                .HasMany(x => x.MultiAnwsers)
                .WithOne(r => r.CompMulti)
                .HasForeignKey(r => r.CompMultiId);
           
        }
    }
}