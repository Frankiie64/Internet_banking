using Internet_banking.Core.Application.Dtos.Account;
using Internet_banking.Core.Application.helper;
using Internet_banking.Core.Application.ViewModels.Users;
using Internet_banking.Core.Domain.Common;
using Internet_banking.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internet_banking.Infrastructure.Persistence.Context
{
    public class ApplicationDbContext: DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private UserVM user;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;

        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {                      
            if (_httpContextAccessor.HttpContext == null)
            {
                List<string> Rol = new();

                Rol.Add("SuperAdmin");

                user = new UserVM
                {
                    Id = "01",
                    Username = "masterAdmin",
                    Email = "masterAdmin@gmail.com",
                    Roles = Rol,
                    IsVerified = true
                };                                
            }
            else
            {
                user = _httpContextAccessor.HttpContext.Session.Get<UserVM>("user");
            }

            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.Entity.LastModified = DateTime.Now;
                        entry.Entity.LastModifiedBy = user.Username;
                        break;
                    case EntityState.Added:
                        entry.Entity.Creadted = DateTime.Now;
                        entry.Entity.CreatedBy = user.Username;
                        break;
                }
            }
        
            return base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<Products> Products { get; set; }
        public DbSet<TypeAccount> TypeAccounts { get; set; }
        public DbSet<Beneficiary> Beneficiary { get; set; }
        public DbSet<Transactional> Transactional { get; set; }
        


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //FLUENT API

            #region Table

            
            modelBuilder.Entity<Products>().ToTable("products");
            modelBuilder.Entity<TypeAccount>().ToTable("typeAccount");
            modelBuilder.Entity<Beneficiary>().ToTable("Beneficiary");
            modelBuilder.Entity<Transactional>().ToTable("Transactional");


            #endregion

            #region constraint

            //Primary Key


            modelBuilder.Entity<Products>().HasKey(x => x.Id);
            modelBuilder.Entity<TypeAccount>().HasKey(x => x.Id);
            modelBuilder.Entity<Beneficiary>().HasKey(x => x.Id);
            modelBuilder.Entity<Transactional>().HasKey(x => x.Id);


            //Relationships

            modelBuilder.Entity<TypeAccount > ()
         .HasMany<Products>(ty => ty.Products)
         .WithOne(pr => pr.TypeAccount)
         .HasForeignKey(pr => pr.IdAccount)
         .OnDelete(deleteBehavior: DeleteBehavior.Cascade);



            #endregion

            #region "Validation Required"

            modelBuilder.Entity<TypeAccount>()
                .Property(a => a.Title)
                .IsRequired();
           
            #endregion
        }
    }
}