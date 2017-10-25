﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Lumle.Core.Models;

namespace Lumle.Core.Services.Abstracts
{
    public interface IBaseRoleClaimService
    {
        int Count(Expression<Func<BaseRoleClaim, bool>> predicate);
        IEnumerable<BaseRoleClaim> GetAll(Expression<Func<BaseRoleClaim, bool>> predicate);
        IEnumerable<BaseRoleClaim> GetAll();
        BaseRoleClaim GetSingle(Expression<Func<BaseRoleClaim, bool>> predicate);
        void Add(BaseRoleClaim entity);
        void Update(BaseRoleClaim entity);
        void DeleteWhere(Expression<Func<BaseRoleClaim, bool>> predicate);
        bool IsClaimExist(Claim claim, string roleId);
        Task<Dictionary<string, bool>> GetActionPrevilegeAsync(Dictionary<string, Claim> claims, ClaimsPrincipal claimPrincipal);
    }
}
