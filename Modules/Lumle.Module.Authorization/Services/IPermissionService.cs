﻿using Lumle.Data.Models;
using Lumle.Module.Authorization.Entities;
using Lumle.Module.Authorization.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Lumle.Core.Models;

namespace Lumle.Module.Authorization.Services
{
    public interface IPermissionService
    {
        int Count();
        int Count(Expression<Func<Permission, bool>> predicate);
        IEnumerable<Permission> GetAll(Expression<Func<Permission, bool>> predicate);
        IEnumerable<Permission> GetAll();
        Permission GetSingle(Expression<Func<Permission, bool>> predicate);
        void Add(Permission entity);
        void Update(Permission entity);
        void DeleteWhere(Expression<Func<Permission, bool>> predicate);
        Task<List<SidebarMenuModel>> GetSideBarMenuAsync(User user);
        IEnumerable<Models.PermissionModels.Module> GetPermissionsIncludingAssigned(IEnumerable<BaseRoleClaim> roleClaims);
        List<User> GetAllUserOfRole(string roleId);
    }
}
