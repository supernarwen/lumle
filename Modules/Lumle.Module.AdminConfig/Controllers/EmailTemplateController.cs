﻿using System;
using Microsoft.AspNetCore.Mvc;
using Lumle.Module.AdminConfig.Services;
using Lumle.Module.AdminConfig.ViewModels;
using Lumle.Data.Data.Abstracts;
using System.Threading.Tasks;
using Lumle.Core.Attributes;
using Lumle.Core.Localizer;
using Lumle.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Lumle.Infrastructure.Constants.AuthorizeRules;
using Lumle.Infrastructure.Constants.LumleLog;
using Lumle.Module.AdminConfig.Entities;
using Microsoft.AspNetCore.Identity;
using NLog;
using Microsoft.Extensions.Localization;
using Lumle.Infrastructure.Constants.Localization;
using Lumle.Module.Audit.Enums;
using Lumle.Module.Audit.Models;
using Lumle.Module.Audit.Services;

namespace Lumle.Module.AdminConfig.Controllers
{
    [Route("adminconfig/[controller]")]
    [Authorize]
    public class EmailTemplateController : Controller
    {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IAuditLogService _auditLogService;
        private readonly IStringLocalizer<ResourceString> _localizer;

        public EmailTemplateController(
            IEmailTemplateService emailTemplateService,
            IUnitOfWork unitOfWork,
            IAuditLogService auditLogService,
            UserManager<User> userManager,
            IStringLocalizer<ResourceString> localizer
        )
        {
            _emailTemplateService = emailTemplateService;
            _unitOfWork = unitOfWork;
            _auditLogService = auditLogService;
            _userManager = userManager;
            _localizer = localizer;
        }

        [HttpGet]
        [ClaimRequirement(CustomClaimtypes.Permission, Permissions.AdminConfigEmailTemplateView)]
        public IActionResult Index()
        {
            var emailTemplates = _emailTemplateService.GetAllEmailTemplate();

            ViewBag.EmailTemplateUpdated = TempData["EmailTemplateUpdated"]; // for email template edit case
            return View(emailTemplates);
        }

        [HttpGet]
        [Route("edit/{emailTemplateId:int}")]
        [ClaimRequirement(CustomClaimtypes.Permission, Permissions.AdminConfigEmailTemplateView)]
        public IActionResult GetEmailTemplate(int emailTemplateId)
        {
            var emailTemplate = _emailTemplateService.GetSingle(x => x.Id == emailTemplateId);
            if (emailTemplate == null)
            {
                TempData["ErrorMsg"] = _localizer[ActionMessageConstants.ResourceNotFoundErrorMessage].Value;
                return RedirectToAction("Index");
            }

            if (TempData["UseDefaultTemplate"] != null)
            {
                emailTemplate.SlugDisplayName = emailTemplate.DefaultSlugDisplayName;
                emailTemplate.Subject = emailTemplate.DefaultSubject;
                emailTemplate.Body = emailTemplate.DefaultBody;
            }

            if (TempData["UseLastTemplate"]!=null)
            {
                emailTemplate.SlugDisplayName = emailTemplate.LastSlugDisplayName;
                emailTemplate.Subject = emailTemplate.LastSubject;
                emailTemplate.Body = emailTemplate.LastBody;
            }

            var emailTemplateVm = AutoMapper.Mapper.Map<EmailTemplateVM>(emailTemplate);

            return View("Edit", emailTemplateVm);
        }

        [HttpPost]
        [Route("edit/{emailTemplateId:int}")]
        [ValidateAntiForgeryToken]
        [ClaimRequirement(CustomClaimtypes.Permission, Permissions.AdminConfigEmailTemplateUpdate)]
        public async Task<IActionResult> EditEmailTemplate(EmailTemplateVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ErrorMsg"] =_localizer[ActionMessageConstants.PleaseFillAllTheRequiredFieldErrorMessage].Value;
                    return View("Edit", model);
                }

                var emailTemplate = _emailTemplateService.GetSingle(x => x.Id == model.Id);
                if (emailTemplate == null)
                {
                    TempData["ErrorMsg"] =_localizer[ActionMessageConstants.ResourceNotFoundErrorMessage].Value;
                    return RedirectToAction("Index");
                }

                var loggedUser = await GetCurrentUserAsync(); //Get current logged in user
                // Add previous data in old record object for comparison
                var oldRecord = new EmailTemplate
                {
                    Id = emailTemplate.Id,
                    Slug = emailTemplate.Slug,
                    SlugDisplayName = emailTemplate.SlugDisplayName,
                    Subject = emailTemplate.Subject,
                    Body = emailTemplate.Body,
                    DefaultBody = emailTemplate.DefaultBody,
                    DefaultSubject = emailTemplate.DefaultSubject,
                    LastSlugDisplayName=emailTemplate.LastSlugDisplayName,
                    LastSubject=emailTemplate.LastSubject,
                    LastBody=emailTemplate.LastBody
                };

                // update in database
                emailTemplate.LastSlugDisplayName = emailTemplate.LastSlugDisplayName;
                emailTemplate.LastSubject = emailTemplate.Subject;
                emailTemplate.LastBody = emailTemplate.Body;

                emailTemplate.SlugDisplayName = model.SlugDisplayName;
                emailTemplate.Subject = model.Subject;
                emailTemplate.Body = model.Body;

                _emailTemplateService.Update(emailTemplate);

                #region EmailTemplate Audit Log

                    var auditLogModel = new AuditLogModel
                    {
                        AuditActionType = AuditActionType.Update,
                        KeyField = oldRecord.Id.ToString(),
                        OldObject = oldRecord,
                        NewObject = emailTemplate,
                        LoggedUserEmail = loggedUser.Email,
                        ComparisonType = ComparisonType.ObjectCompare
                    };

                    _auditLogService.Add(auditLogModel);
                #endregion

                _unitOfWork.Save();

                TempData["EmailTemplateUpdated"] = true;

                TempData["SuccessMsg"] =_localizer[ActionMessageConstants.UpdatedSuccessfully].Value;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ErrorLog.UpdateError);
                TempData["ErrorMsg"] =_localizer[ActionMessageConstants.InternalServerErrorMessage].Value;
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [Route("default/{emailTemplateId:int}")]
        [ClaimRequirement(CustomClaimtypes.Permission, Permissions.AdminConfigEmailTemplateUpdate)]
        public IActionResult LoadDefaultTemplate(int emailTemplateId)
        {
            TempData["UseDefaultTemplate"] = true;
            return RedirectToAction("EditEmailTemplate", emailTemplateId);
        }

        [HttpGet]
        [Route("last/{emailTemplateId:int}")]
        [ClaimRequirement(CustomClaimtypes.Permission, Permissions.AdminConfigEmailTemplateUpdate)]
        public IActionResult LastDefaultTemplate(int emailTemplateId)
        {
            TempData["UseLastTemplate"] = true;
            return RedirectToAction("EditEmailTemplate", emailTemplateId);
        }

        #region Helpers
        private Task<User> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
        #endregion

    }
}
