﻿using System;
using System.Collections.Generic;
using System.Net;
using Lumle.Data.Data.Abstracts;
using Lumle.Module.Blog.Models;
using Lumle.Module.Blog.Services;
using Lumle.Module.Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Lumle.Infrastructure.Constants.AuthorizeRules;
using Lumle.Module.Blog.Entities;
using Lumle.Module.Audit.Services;
using Lumle.Module.Audit.Enums;
using static Lumle.Infrastructure.Helpers.DataTableHelper;
using Lumle.Module.Blog.Helpers;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using Lumle.Core.Attributes;
using Lumle.Data.Models;
using Microsoft.AspNetCore.Identity;
using Lumle.Module.Audit.Models;
using Microsoft.Extensions.Localization;
using Lumle.Core.Localizer;
using Lumle.Infrastructure.Constants.Localization;
using Lumle.Infrastructure.Utilities.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Lumle.Core.Services.Abstracts;
using Lumle.Core.Models;
using System.Security.Claims;
using Lumle.Infrastructure.Constants.ActionConstants;
using System.Text.RegularExpressions;
using System.Xml;

namespace Lumle.Module.Blog.Controllers
{
    [Route("blog/[controller]")]
    [Authorize]
    public class ArticleController : Controller
    {
        private readonly IBaseRoleClaimService _baseRoleClaimService;
        private readonly IArticleService _articleService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuditLogService _auditLogService;
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer<ResourceString> _localizer;
        private readonly ITimeZoneHelper _timeZoneHelper;
        private readonly IFileHandler _fileHandler;
        private readonly IHostingEnvironment _environment;
        private readonly IUrlHelper _urlHelper;
        private string _imageUrl;

        public ArticleController(
            IBaseRoleClaimService baseRoleClaimService,
            IArticleService articleService,
            IUnitOfWork unitOfWork,
            IAuditLogService auditLogService,
            UserManager<User> userManager,
            IStringLocalizer<ResourceString> localizer,
            ITimeZoneHelper timeZoneHelper,
            IFileHandler fileHandler,
            IHostingEnvironment environment,
            IUrlHelper urlHelper
        )
        {
            _baseRoleClaimService = baseRoleClaimService;
            _articleService = articleService;
            _unitOfWork = unitOfWork;
            _auditLogService = auditLogService;
            _userManager = userManager;
             _localizer = localizer;
            _timeZoneHelper = timeZoneHelper;
            _fileHandler = fileHandler;
            _environment = environment;
            _urlHelper = urlHelper;
        }

        [HttpGet]
        [ClaimRequirement(CustomClaimtypes.Permission, Permissions.BlogArticleView)]
        public async Task<ViewResult> Index()
        {
            // Make map to check for the action previleges
            var map = new Dictionary<string, Claim>
            {
                { OperationActionConstant.CreateAction, new Claim("permission", Permissions.BlogArticleCreate) },
                { OperationActionConstant.UpdateAction, new Claim("permission", Permissions.BlogArticleUpdate) },
                { OperationActionConstant.DeleteAction, new Claim("permission", Permissions.BlogArticleDelete) }
            };

            // Get action previlege according to actions provided
            var actionClaimResult = await _baseRoleClaimService.GetActionPrevilegeAsync(map, User);
            var actionModel = new ActionOperation
            {
                CreateAction = actionClaimResult[OperationActionConstant.CreateAction],
                UpdateAction = actionClaimResult[OperationActionConstant.UpdateAction],
                DeleteAction = actionClaimResult[OperationActionConstant.DeleteAction]
            };

            return View(actionModel);
        }

        [HttpGet("new")]
        [ClaimRequirement(CustomClaimtypes.Permission, Permissions.BlogArticleCreate)]
        public ViewResult New()
        {
            return View();
        }

        [HttpPost("new")]
        [ValidateAntiForgeryToken]
        [ClaimRequirement(CustomClaimtypes.Permission, Permissions.BlogArticleCreate)]
        public async Task<IActionResult> New(ArticleVM model)
        {
            if(model.FeaturedImage != null)
            {
                _imageUrl = _fileHandler.UploadImage(model.FeaturedImage, 300, 300);
            }

            if (!ModelState.IsValid)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
  
            var loggedUser = await GetCurrentUserAsync();
            var articleModel = Mapper.Map<ArticleModel>(model);
            articleModel.Author = loggedUser.Email;
            articleModel.CreatedDate = DateTime.UtcNow;
            articleModel.LastUpdated = DateTime.UtcNow;
            articleModel.Slug = "Test slug";
            articleModel.FeaturedImageUrl = _imageUrl;

            // Check if the content field is null
            if (!String.IsNullOrEmpty(articleModel.Content))
            {
                // Convert Base64 image to image format
                articleModel.Content = await SaveFilesToDisk(articleModel.Content);
            }

            var articleEntity = Mapper.Map<Article>(articleModel);

            await _articleService.Add(articleEntity);
            await _unitOfWork.SaveAsync();

            #region AuditLog
            var newArticle = new Article(); // Storage of this null object shows data before creation = nothing!
            var auditLogModel = new AuditLogModel
            {
                AuditActionType = AuditActionType.Create,
                OldObject = newArticle,
                NewObject = articleEntity,
                KeyField = articleEntity.Id.ToString(),
                ComparisonType = ComparisonType.ObjectCompare,
                LoggedUserEmail = loggedUser.Email
            };

            await _auditLogService.Add(auditLogModel);

            await _unitOfWork.SaveAsync();
            #endregion

            TempData["SuccessMsg"] = _localizer[ActionMessageConstants.AddedSuccessfully].Value;

            return RedirectToAction("Index");
        }

        [HttpGet("edit/{articleId:int}")]
        [ClaimRequirement(CustomClaimtypes.Permission, Permissions.BlogArticleUpdate)]
        public async Task<IActionResult> Edit(int articleId)
        {
            var article = await _articleService.GetSingle(x => x.Id == articleId);
            var articleVm = Mapper.Map<ArticleVM>(article);
            _imageUrl = !string.IsNullOrEmpty(articleVm.FeaturedImageUrl) ? $"{Request.Scheme}://{Request.Host}{_urlHelper.Content("~/")}uploadedimages/{articleVm.FeaturedImageUrl}"
              : string.Empty;
            articleVm.FeaturedImageUrl = _imageUrl;

            return View(articleVm);
        }

        [HttpPost("edit/{articleId:int}")]
        [ValidateAntiForgeryToken]
        [ClaimRequirement(CustomClaimtypes.Permission, Permissions.BlogArticleUpdate)]
        public async Task<IActionResult> Edit(ArticleVM model)
        {
            if (!ModelState.IsValid)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            // Save images if any
            if (model.FeaturedImage != null)
            {
                _imageUrl = _fileHandler.UploadImage(model.FeaturedImage, 300, 360);
            }

            var article = await _articleService.GetSingle(x => x.Id == model.Id);
            if (article == null) return RedirectToAction("Index");

            var loggedUser = await GetCurrentUserAsync();
            // Add previous data in old record object for comparison
            var oldRecord = new Article
            {
                Id = article.Id,
                Author = article.Author,
                Title = article.Title,
                Content = article.Content,
                FeaturedImageUrl = article.FeaturedImageUrl,
                Slug = article.Slug
            };

            // update in database
            article.Title = model.Title;

            // Check if content field is null or empty
            if (!String.IsNullOrEmpty(model.Content))
            {
                article.Content = await SaveFilesToDisk(model.Content);
            }

            article.FeaturedImageUrl = _imageUrl ?? article.FeaturedImageUrl;

            await _articleService.Update(article);
            // For audit log
            #region AuditLog
            var auditLogModel = new AuditLogModel
            {
                AuditActionType = AuditActionType.Update,
                OldObject = oldRecord,
                NewObject = article,
                KeyField = oldRecord.Id.ToString(),
                ComparisonType = ComparisonType.ObjectCompare,
                LoggedUserEmail = loggedUser.Email
            };
            await _auditLogService.Add(auditLogModel);
            await _unitOfWork.SaveAsync();
            #endregion

            TempData["SuccessMsg"] = _localizer[ActionMessageConstants.UpdatedSuccessfully].Value;
            return RedirectToAction("Index");
        }

        [HttpPost("delete/{id}")]
        [ClaimRequirement(CustomClaimtypes.Permission, Permissions.BlogArticleDelete)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var article = await _articleService.GetSingle(x => x.Id == id);
            if (article == null) return RedirectToAction("Index");
            {

                // var sampleObject = new Article(); // Storage of this null object shows data after delete = nothing!
                await _articleService.DeleteWhere(x => x.Id == id);
                #region AuditLog
                // _auditLogService.Add(AuditActionType.Delete, id.ToString(), article, sampleObject);
                await _unitOfWork.SaveAsync();
                #endregion
            }
            TempData["SuccessMsg"] = _localizer[ActionMessageConstants.DeletedSuccessfully].Value;
            return RedirectToAction("Index");
        }

        [HttpPost("DataHandler")]
        [ClaimRequirement(CustomClaimtypes.Permission, Permissions.BlogArticleView)]
        public async Task<JsonResult> DataHandler([FromBody] BlogDTParameters parameters)
        {
            try
            {
                var user = await GetCurrentUserAsync();
                var articles = _articleService.GetAll().Select(art => new ArticleModel
                {
                    Id = art.Id,
                    Title = art.Title,
                    Author = art.Author,
                    CreatedDate=art.CreatedDate
                });

                List<ArticleModel> filteredAtricle;
                int totalArticle;
                if (!string.IsNullOrEmpty(parameters.Search.Value.Trim())
                   && !string.IsNullOrWhiteSpace(parameters.Search.Value.Trim()))
                {
                    Expression<Func<ArticleModel, bool>> search =
                        x => (x.Author ?? "").ToString().ToLower().Contains(parameters.Search.Value.ToLower()) ||
                             (x.Title ?? "").ToString().ToLower().Contains(parameters.Search.Value.ToLower());
                    totalArticle = articles.Count(search);
                    articles = articles.Where(search);
                    articles = SortByColumnWithOrder(parameters.Order[0].Column, parameters.Order[0].Dir.ToString(), articles);
                    filteredAtricle = articles.Skip(parameters.Start).Take(parameters.Length).ToList();
                }
                else
                {
                    totalArticle = articles.Count();
                    articles = SortByColumnWithOrder(parameters.Order[0].Column, parameters.Order[0].Dir.ToString(), articles);
                    filteredAtricle = articles.Skip(parameters.Start).Take(parameters.Length).ToList();
                }
                var counter = parameters.Start + 1;
                var articleModels = filteredAtricle.Select(x => {
                    x.SN = counter++;
                    x.FormatedCreatedDate = _timeZoneHelper.ConvertToLocalTime(x.CreatedDate, user.TimeZone).ToString("g");
                    return x;
                }).ToList();

                var article = new DTResult<ArticleModel>
                {
                    Draw = parameters.Draw,
                    Data = articleModels,
                    RecordsFiltered = totalArticle,
                    RecordsTotal = totalArticle
                };

                return Json(article);
            }
            catch (Exception ex)
            {
                return Json(new {error = ex.Message});
            }
        }


        #region Helpers
        private static IQueryable<ArticleModel> SortByColumnWithOrder(int order, string orderDirection, IQueryable<ArticleModel> data)
        {
            try
            {
                switch (order)
                {
                    case 2:
                        return orderDirection.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Title) : data.OrderBy(p => p.Title);
                    case 3:
                        return orderDirection.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Author) : data.OrderBy(p => p.Author);
                    case 4:
                        return orderDirection.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CreatedDate) : data.OrderBy(p => p.CreatedDate);
                    default:
                        return data.OrderByDescending(p => p.CreatedDate);
                }
            }
            catch (Exception)
            {
                return data;
            }
        }
        private async Task<User> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }

        private async Task<String> SaveFilesToDisk(String content)
        {
            var imgRegex = new Regex("<img[^>].+ />", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            var base64Regex = new Regex("data:[^/]+/(?<ext>[a-z]+);base64,(?<base64>.+)", RegexOptions.IgnoreCase);

            foreach (Match match in imgRegex.Matches(content))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml("<root>" + match.Value + "</root>");

                var img = doc.FirstChild.FirstChild;
                var srcNode = img.Attributes["src"];
                var fileNameNode = img.Attributes["data-filename"];

                // The HTML editor creates base64 DataURIs which we'll have to convert to image files on disk
                if (srcNode != null && fileNameNode != null)
                {
                    var base64Match = base64Regex.Match(srcNode.Value);
                    if (base64Match.Success)
                    {
                        byte[] bytes = Convert.FromBase64String(base64Match.Groups["base64"].Value);
                        srcNode.Value = await _fileHandler.SaveFile(bytes, fileNameNode.Value).ConfigureAwait(false);

                        img.Attributes.Remove(fileNameNode);
                        content = content.Replace(match.Value, img.OuterXml);
                    }
                }
            }

            return content;
        }
        #endregion
    }
}