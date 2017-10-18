﻿using Lumle.Data.Data.Abstracts;
using Lumle.Module.Authorization.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using Lumle.Core.Models;
using Lumle.Core.Services.Abstracts;
using Lumle.Data.Data;
using Lumle.Module.Authorization.Models.PermissionModels;
using Microsoft.Extensions.Caching.Memory;
using Lumle.Data.Models;
using Lumle.Infrastructure.Constants.LumleLog;
using NLog;
using Lumle.Module.Authorization.ViewModels.SideBarViewModels;

namespace Lumle.Module.Authorization.Services
{
    public class PermissionService : IPermissionService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IRepository<Permission> _permissionRepository;
        private readonly IBaseRoleClaimService _baseRoleClaimService;
        private readonly BaseContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMemoryCache _memoryCache;

        public PermissionService
        (
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IRepository<Permission> permissionRepository,
            IBaseRoleClaimService baseRoleClaimService,
            BaseContext context,
            IMemoryCache memoryCache)
        {
            _permissionRepository = permissionRepository;
            _baseRoleClaimService = baseRoleClaimService;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _memoryCache = memoryCache;
        }

        public int Count()
        {
            try
            {
                var permissionCount = _permissionRepository.Count();

                return permissionCount;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ErrorLog.DataFetchError);
                throw;
            }
        }

        public int Count(Expression<Func<Permission, bool>> predicate)
        {
            try
            {
                var permissionCount = _permissionRepository.Count(predicate);
                return permissionCount;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ErrorLog.DataFetchError);
                throw;
            }
        }

        public IEnumerable<Permission> GetAll(Expression<Func<Permission, bool>> predicate)
        {
            try
            {
                var permissions = _permissionRepository.GetAll(predicate);
                return permissions;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ErrorLog.DataFetchError);
                throw;
            }
        }

        public IEnumerable<Permission> GetAll()
        {
            try
            {
                var permissions = _permissionRepository.GetAll();
                return permissions;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ErrorLog.DataFetchError);
                throw;
            }
        }

        public Permission GetSingle(Expression<Func<Permission, bool>> predicate)
        {
            try
            {
                var permission = _permissionRepository.GetSingle(predicate);
                return permission;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ErrorLog.DataFetchError);
                throw;
            }
        }

        public void Add(Permission entity)
        {
            try
            {
                _permissionRepository.Add(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ErrorLog.SaveError);
                throw;
            }
        }

        public void Update(Permission entity)
        {
            try
            {
                _permissionRepository.Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ErrorLog.UpdateError);
                throw;
            }
        }

        public void DeleteWhere(Expression<Func<Permission, bool>> predicate)
        {
            try
            {
                _permissionRepository.DeleteWhere(predicate);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ErrorLog.DeleteError);
                throw;
            }
        }

        public IEnumerable<Models.PermissionModels.Module> GetPermissionsIncludingAssigned(IEnumerable<BaseRoleClaim> roleClaims)
        {
            var rolePermissions = GetAll();

            // Get default sidebar menu list
            var sideBarVm = new SideBarVM();
            var menuList = sideBarVm.SideBarItems;
            var modules = from p in rolePermissions
                orderby p.Id
                group p by p.Menu
                into o
                select new Models.PermissionModels.Module
                {
                    Name = menuList.FirstOrDefault(y => y.Name == o.Key && y.MenuLevel == 0)?.DisplayName ?? o.Key,
                    SubModules = from j in o
                        group j by j.SubMenu
                        into k
                        select
                        new SubModule
                        {
                            Name = menuList.FirstOrDefault(y => y.Name == k.Key && y.MenuLevel == 1)?.DisplayName ?? "",
                            ModulePermissions = k.ToList()
                                .Select(x =>
                                    new ModulePermission
                                    {
                                        Id = x.Id,
                                        Slug = x.Slug,
                                        DisplayName = x.DisplayName,
                                        IsAssigned = roleClaims.Select(h => h.ClaimValue).ToList().Contains(x.Slug)
                                    })
                        }
                };

            return modules;
        }

        public List<User> GetAllUserOfRole(string roleId)
        {
            try
            {
                var users = _context.UserRoles.Where(x => x.RoleId == roleId).Select(x => x.User).ToList();
                return users;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
