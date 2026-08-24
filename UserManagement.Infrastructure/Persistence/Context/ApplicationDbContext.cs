using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using UserManagement.Application.DTOs.Branch;
using UserManagement.Application.DTOs.Menu;
using UserManagement.Application.DTOs.RoleMenu;
using UserManagement.Domain.Entities;
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
        public DbSet<MenuResponseDto> MenuResponseDtos { get; set; }
        public DbSet<Branch> Branches { get; set; }

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

            builder.Entity<Branch>(entity =>
            {
                entity.ToTable("tblbranches", "erpsystem");

                entity.HasKey(x => x.BranchId);

                entity.Property(x => x.BranchId)
                    .HasColumnName("branch_id");

                entity.Property(x => x.BranchName)
                    .HasColumnName("branch_name");
            });

            builder.Entity<BranchRestoreCheck>(entity =>
            {
                entity.HasNoKey();

                entity.Property(x => x.BranchId)
                    .HasColumnName("branch_id");

                entity.Property(x => x.Flag)
                    .HasColumnName("flag");
            });

            builder.Entity<MenuResponseDto>(entity =>
            {
                entity.HasNoKey();

                entity.Property(x => x.MenuId)
                    .HasColumnName("menu_id");

                entity.Property(x => x.MenuName)
                    .HasColumnName("menu_name");

                entity.Property(x => x.MenuUrl)
                    .HasColumnName("menu_url");

                entity.Property(x => x.ParentMenuId)
                    .HasColumnName("parent_menu_id");

                entity.Property(x => x.Icon)
                    .HasColumnName("icon");

                entity.Property(x => x.DisplayOrder)
                    .HasColumnName("display_order");
            });

            builder.Entity<RoleMenuResponseDto>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(null);

            });
        }
    }
}
