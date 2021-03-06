﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Lumle.Data.Data;

namespace Lumle.Web.Migrations
{
    [DbContext(typeof(BaseContext))]
    partial class BaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("Lumle.Core.Models.ApplicationToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigserial ")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<bool>("IsUsed");

                    b.Property<DateTime>("LastUpdated");

                    b.Property<string>("Token")
                        .IsRequired();

                    b.Property<string>("TokenType")
                        .IsRequired();

                    b.Property<DateTime>("UsedDate");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Core_ApplicationToken");
                });

            modelBuilder.Entity("Lumle.Core.Models.AppSystem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigserial ")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime>("LastUpdated");

                    b.Property<string>("LastUpdatedBy")
                        .IsRequired();

                    b.Property<string>("Slug")
                        .IsRequired();

                    b.Property<string>("Status")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Slug")
                        .IsUnique()
                        .HasName("SlugIndex");

                    b.ToTable("AdminConfig_AppSystem");
                });

            modelBuilder.Entity("Lumle.Core.Models.BaseRoleClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigserial ")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<string>("ClaimType")
                        .IsRequired();

                    b.Property<string>("ClaimValue")
                        .IsRequired();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime>("LastUpdated");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Core_BaseRoleClaim");
                });

            modelBuilder.Entity("Lumle.Core.Models.Culture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigserial ")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("DisplayName")
                        .IsRequired();

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsEnable");

                    b.Property<DateTime>("LastUpdated");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Localization_Culture");
                });

            modelBuilder.Entity("Lumle.Core.Models.Resource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigserial ")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("CultureId");

                    b.Property<string>("Key")
                        .IsRequired();

                    b.Property<DateTime>("LastUpdated");

                    b.Property<int>("ResourceCategoryId");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("ResourceCategoryId");

                    b.HasIndex("CultureId", "Key")
                        .IsUnique();

                    b.ToTable("Localization_Resource");
                });

            modelBuilder.Entity("Lumle.Core.Models.ResourceCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigserial ")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime>("LastUpdated");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Localization_ResourceCategory");
                });

            modelBuilder.Entity("Lumle.Core.Models.UserProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigserial ")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<string>("AboutMe");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime>("DeletedDate");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .HasMaxLength(100);

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastName")
                        .HasMaxLength(100);

                    b.Property<DateTime>("LastUpdated");

                    b.Property<string>("Phone");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.Property<string>("Website")
                        .HasMaxLength(500);

                    b.HasKey("Id");

                    b.ToTable("Authorization_UserProfile");
                });

            modelBuilder.Entity("Lumle.Data.Models.Role", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Description");

                    b.Property<bool>("IsBlocked");

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.Property<int>("Priority");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("Core_Role");
                });

            modelBuilder.Entity("Lumle.Data.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<int>("AccountStatus");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Culture")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("en-US")
                        .HasMaxLength(20);

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("TimeZone")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.HasIndex("Email", "NormalizedEmail")
                        .IsUnique();

                    b.ToTable("Core_User");
                });

            modelBuilder.Entity("Lumle.Data.Models.UserRole", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("Core_UserRole");
                });

            modelBuilder.Entity("Lumle.Module.AdminConfig.Entities.Credential", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigserial ")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("CredentialCategoryId");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<DateTime>("LastUpdated");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Value")
                        .HasMaxLength(1000);

                    b.HasKey("Id");

                    b.HasIndex("CredentialCategoryId", "Slug")
                        .IsUnique();

                    b.ToTable("AdminConfig_Credential");
                });

            modelBuilder.Entity("Lumle.Module.AdminConfig.Entities.CredentialCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigserial ")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime>("LastUpdated");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.ToTable("AdminConfig_CredentialCategory");
                });

            modelBuilder.Entity("Lumle.Module.AdminConfig.Entities.EmailTemplate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigserial ")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<string>("Body")
                        .IsRequired();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("DefaultBody")
                        .IsRequired();

                    b.Property<string>("DefaultSlugDisplayName")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("DefaultSubject")
                        .IsRequired()
                        .HasMaxLength(500);

                    b.Property<string>("LastBody")
                        .IsRequired();

                    b.Property<string>("LastSlugDisplayName")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("LastSubject")
                        .IsRequired()
                        .HasMaxLength(500);

                    b.Property<DateTime>("LastUpdated");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("SlugDisplayName")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasMaxLength(500);

                    b.HasKey("Id");

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.ToTable("AdminConfig_EmailTemplate");
                });

            modelBuilder.Entity("Lumle.Module.AdminConfig.Entities.ServiceHealth", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigserial ")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime>("LastUpdated");

                    b.Property<string>("Message");

                    b.Property<string>("ServiceName");

                    b.Property<bool>("Status");

                    b.Property<int>("SystemHealthId");

                    b.HasKey("Id");

                    b.HasIndex("SystemHealthId");

                    b.ToTable("AdminConfig_ServiceHealth");
                });

            modelBuilder.Entity("Lumle.Module.AdminConfig.Entities.SystemHealth", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigserial ")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime>("LastUpdated");

                    b.Property<string>("Username")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("AdminConfig_SystemHealth");
                });

            modelBuilder.Entity("Lumle.Module.Audit.Entities.AuditLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigserial ")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<string>("AuditSummary");

                    b.Property<string>("AuditType")
                        .IsRequired();

                    b.Property<string>("Changes")
                        .IsRequired();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("IpAddress");

                    b.Property<string>("KeyField")
                        .IsRequired();

                    b.Property<DateTime>("LastUpdated");

                    b.Property<string>("MachineName");

                    b.Property<string>("ModuleInfo");

                    b.Property<string>("NewValue")
                        .IsRequired();

                    b.Property<string>("OldValue")
                        .IsRequired();

                    b.Property<string>("TableName")
                        .IsRequired();

                    b.Property<string>("UserAgent");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Audit_AuditLog");
                });

            modelBuilder.Entity("Lumle.Module.Audit.Entities.CustomLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigserial ")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<string>("CallSite");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Exception");

                    b.Property<DateTime>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Level");

                    b.Property<string>("LoggedDate");

                    b.Property<string>("Message");

                    b.Property<string>("Port");

                    b.Property<string>("RemoteAddress");

                    b.Property<string>("RequestMethod");

                    b.Property<string>("ServerAddress");

                    b.Property<string>("ServerName");

                    b.Property<string>("Url");

                    b.Property<string>("UserAgent");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("Audit_CustomLog");
                });

            modelBuilder.Entity("Lumle.Module.Authorization.Entities.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigserial ")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("DisplayName")
                        .IsRequired();

                    b.Property<DateTime>("LastUpdated");

                    b.Property<string>("Menu")
                        .IsRequired();

                    b.Property<string>("Slug")
                        .IsRequired();

                    b.Property<string>("SubMenu");

                    b.HasKey("Id");

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.ToTable("Authorization_Permission");
                });

            modelBuilder.Entity("Lumle.Module.Blog.Entities.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigserial ")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<string>("Author")
                        .IsRequired();

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("FeaturedImageUrl");

                    b.Property<DateTime>("LastUpdated");

                    b.Property<string>("Slug")
                        .IsRequired();

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Blog_Article");
                });

            modelBuilder.Entity("Lumle.Module.Calendar.Entities.CalendarEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigserial ")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime>("End");

                    b.Property<DateTime>("LastUpdated");

                    b.Property<string>("Remarks")
                        .HasMaxLength(250);

                    b.Property<DateTime>("Start");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Calendar_Event");
                });

            modelBuilder.Entity("Lumle.Module.Calendar.Entities.CalendarHoliday", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigserial ")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime>("Date");

                    b.Property<DateTime>("LastUpdated");

                    b.Property<string>("Remarks")
                        .HasMaxLength(250);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Calendar_Holiday");
                });

            modelBuilder.Entity("Lumle.Module.PublicUser.Entities.CustomUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigserial ")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Email");

                    b.Property<string>("Gender");

                    b.Property<bool>("IsBlocked");

                    b.Property<bool>("IsEmailVerified");

                    b.Property<bool>("IsStaff");

                    b.Property<DateTime>("LastUpdated");

                    b.Property<string>("PasswordHash")
                        .IsRequired();

                    b.Property<string>("PasswordSalt")
                        .IsRequired();

                    b.Property<string>("PhoneNo");

                    b.Property<string>("ProfileUrl");

                    b.Property<string>("Provider")
                        .IsRequired();

                    b.Property<string>("SubjectId")
                        .IsRequired();

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("PublicUser_CustomUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Core_RoleClaim");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Core_UserClaim");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("Core_UserLogin");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("LoginProvider")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Value");

                    b.HasKey("UserId");

                    b.HasAlternateKey("UserId", "LoginProvider", "Name");

                    b.ToTable("Core_UserToken");
                });

            modelBuilder.Entity("Lumle.Core.Models.ApplicationToken", b =>
                {
                    b.HasOne("Lumle.Data.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Lumle.Core.Models.Resource", b =>
                {
                    b.HasOne("Lumle.Core.Models.Culture", "Culture")
                        .WithMany("Resources")
                        .HasForeignKey("CultureId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Lumle.Core.Models.ResourceCategory", "ResourceCategory")
                        .WithMany("Resources")
                        .HasForeignKey("ResourceCategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Lumle.Data.Models.UserRole", b =>
                {
                    b.HasOne("Lumle.Data.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Lumle.Data.Models.User", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Lumle.Module.AdminConfig.Entities.Credential", b =>
                {
                    b.HasOne("Lumle.Module.AdminConfig.Entities.CredentialCategory", "CredentialCategory")
                        .WithMany("Credentials")
                        .HasForeignKey("CredentialCategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Lumle.Module.AdminConfig.Entities.ServiceHealth", b =>
                {
                    b.HasOne("Lumle.Module.AdminConfig.Entities.SystemHealth", "SystemHealth")
                        .WithMany("ServiceHealths")
                        .HasForeignKey("SystemHealthId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Lumle.Data.Models.Role")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Lumle.Data.Models.User")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Lumle.Data.Models.User")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
