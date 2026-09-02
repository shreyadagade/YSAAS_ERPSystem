using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using UserManagement.Application.DTOs.Branch;
using UserManagement.Application.DTOs.Common;
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

            builder.Entity<OperationResultDto>()
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

            builder.Entity<EmployeeResponseDto>(entity =>
            {
                entity.HasNoKey();

                entity.Property(x => x.EmployeeId)
                    .HasColumnName("employee_id");

                entity.Property(x => x.EmployeeName)
                    .HasColumnName("employee_name");

                entity.Property(x => x.EmployeeCode)
                    .HasColumnName("employee_code");

                entity.Property(x => x.EmailAddress)
                    .HasColumnName("email_address");

                entity.Property(x => x.MobileNumber)
                    .HasColumnName("mobile_number");

                entity.Property(x => x.ProfilePhoto)
                    .HasColumnName("profile_photo");

                entity.Property(x => x.BirthDate)
                    .HasColumnName("birth_date");

                entity.Property(x => x.JoiningDate)
                    .HasColumnName("joining_date");

                entity.Property(x => x.Salary)
                    .HasColumnName("salary");

                entity.Property(x => x.Qualification)
                    .HasColumnName("qualification");

                entity.Property(x => x.Gender)
                    .HasColumnName("gender");

                entity.Property(x => x.BranchId)
                    .HasColumnName("branch_id");

                entity.Property(x => x.BranchName)
                    .HasColumnName("branch_name");

                entity.Property(x => x.AadharCardNumber)
                    .HasColumnName("aadhar_card_number");

                entity.Property(x => x.PanNumber)
                    .HasColumnName("pan_number");

                entity.Property(x => x.LocalAddress)
                    .HasColumnName("local_address");

                entity.Property(x => x.UserId)
                    .HasColumnName("user_id");
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
