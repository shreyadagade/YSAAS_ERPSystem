using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using UserManagement.Application.DTOs.Branch;
using UserManagement.Infrastructure.Persistence.Identity;
using UserManagement.Infrastructure.Persistence.Models;

namespace UserManagement.Infrastructure.Persistence.Context
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("erpsystem");
            builder.Entity<RefreshToken>()
    .ToTable("RefreshTokens", "erpsystem");

            builder.Entity<RegisterEmployeeResult>()
            .HasNoKey();

            builder.Entity<EmployeeCodeResult>()
            .HasNoKey();

            builder.Entity<BranchResponseDto>(entity =>
            {
                entity.HasNoKey();

                entity.Property(x => x.BranchId)
                    .HasColumnName("branch_id");

                entity.Property(x => x.BranchName)
                    .HasColumnName("branch_name");
            });
        }
    }
}
