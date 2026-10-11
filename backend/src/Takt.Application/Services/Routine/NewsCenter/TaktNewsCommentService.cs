// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Routine.NewsCenter
// 文件名称：TaktNewsCommentService.cs
// 创建时间：2026-08-24
// 创建人：Takt365(Cursor AI)
// 功能描述：新闻中心评论应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Routine.NewsCenter;
using Takt.Domain.Entities.Routine.NewsCenter;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Routine.NewsCenter;

/// <summary>
/// 新闻中心评论应用服务
/// </summary>
public class TaktNewsCommentService : TaktServiceBase, ITaktNewsCommentService
{
    private readonly ITaktApprovalRepository<TaktNewsComment> _newsCommentRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="newsCommentRepository">新闻中心评论仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktNewsCommentService(
        ITaktApprovalRepository<TaktNewsComment> newsCommentRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _newsCommentRepository = newsCommentRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取新闻中心评论列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktNewsCommentDto>> GetNewsCommentListAsync(TaktNewsCommentQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktNewsCommentDto>.Create(
                new List<TaktNewsCommentDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _newsCommentRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktNewsCommentDto>.Create(
            data.Adapt<List<TaktNewsCommentDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取新闻中心评论
    /// </summary>
    /// <param name="id">新闻中心评论ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsCommentDto?> GetNewsCommentByIdAsync(long id)
    {
        var entity = await _newsCommentRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktNewsCommentDto>();
    }

    /// <summary>
    /// 获取新闻中心评论树形选项列表（懒加载：仅 parentId 直接子级一层）
    /// </summary>
    /// <param name="parentId">父级ID（0=根）</param>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>树形选项（一层）</returns>
    public async Task<List<TaktTreeSelectOption>> GetNewsCommentTreeOptionsAsync(long parentId = 0, string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _newsCommentRepository.GetListAsync(x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ParentId == parentId && x.CommentStatus == 1);
        return list
            .OrderBy(x => x.Id)
            .Select(item =>
            {
                var isLeaf = false;
                return new TaktTreeSelectOption
                {
                    DictValue = item.Id.ToString(),
                    DictLabel = item.UserName,
                    SortOrder = 0,
                    IsLeaf = isLeaf,
                    Children = null,
                };
            })
            .ToList();
    }

    /// <summary>
    /// 获取新闻中心评论树形列表（懒加载：仅 parentId 直接子级一层；不整表加载、不递归构树）
    /// </summary>
    /// <param name="parentId">父级ID（0=根）</param>
    /// <param name="includeDisabled">是否包含禁用项</param>
    /// <returns>树形列表（一层）</returns>
    public async Task<List<TaktNewsCommentTreeDto>> GetNewsCommentTreeAsync(long parentId = 0, bool includeDisabled = false)
    {
        EnsureThreeLayerContext();
        Expression<Func<TaktNewsComment, bool>> predicate = includeDisabled
            ? (x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ParentId == parentId && x.IsObsolete == 0)
            : (x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ParentId == parentId && x.IsObsolete == 0 && x.CommentStatus == 1);
        var list = await _newsCommentRepository.GetListAsync(predicate);
        return list
            .OrderBy(x => x.Id)
            .Select(item =>
            {
                var treeDto = item.Adapt<TaktNewsCommentTreeDto>();
                treeDto.Children = null;
                return treeDto;
            })
            .ToList();
    }

    /// <summary>
    /// 创建新闻中心评论
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsCommentDto> CreateNewsCommentAsync(TaktNewsCommentCreateDto dto)
    {
        var entity = dto.Adapt<TaktNewsComment>();
        entity.IsObsolete = 0;
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _newsCommentRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.NewsId == entity.NewsId,
                x => x.LineNumber);
            var businessCode = entity.NewsId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _newsCommentRepository.CreateAsync(entity);
        return await GetNewsCommentByIdAsync(entity.Id) ?? entity.Adapt<TaktNewsCommentDto>();
    }

    /// <summary>
    /// 更新新闻中心评论
    /// </summary>
    /// <param name="id">新闻中心评论ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsCommentDto> UpdateNewsCommentAsync(long id, TaktNewsCommentUpdateDto dto)
    {
        var entity = await _newsCommentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("新闻中心评论不存在");
        }
        dto.Adapt(entity);
        await _newsCommentRepository.UpdateAsync(entity);
        return await GetNewsCommentByIdAsync(id) ?? throw new TaktBusinessException("新闻中心评论不存在");
    }

    /// <summary>
    /// 删除新闻中心评论
    /// </summary>
    /// <param name="id">新闻中心评论ID</param>
    /// <returns>任务</returns>
    public async Task DeleteNewsCommentByIdAsync(long id)
    {

        var hasChildren = await _newsCommentRepository.ExistsAsync(x => x.ParentId == id);
        if (hasChildren)
        {
            throw new TaktBusinessException("存在子节点，无法删除");
        }
        var entity = await _newsCommentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("新闻中心评论不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("新闻中心评论不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("新闻中心评论已作废");
        }
        entity.IsObsolete = 1;
        await _newsCommentRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除新闻中心评论
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteNewsCommentBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteNewsCommentByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新新闻中心评论状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsCommentDto> UpdateNewsCommentStatusAsync(TaktNewsCommentStatusDto dto)
    {
        var entity = await _newsCommentRepository.GetByIdAsync(dto.NewsCommentId);
        if (entity == null)
        {
            throw new TaktBusinessException("新闻中心评论不存在");
        }
        entity.CommentStatus = dto.CommentStatus;
        await _newsCommentRepository.UpdateAsync(entity);
        return await GetNewsCommentByIdAsync(dto.NewsCommentId) ?? throw new TaktBusinessException("新闻中心评论不存在");
    }

    /// <summary>
    /// 更新新闻中心评论作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsCommentDto> UpdateNewsCommentObsoleteAsync(TaktNewsCommentObsoleteDto dto)
    {
        var entity = await _newsCommentRepository.GetByIdAsync(dto.NewsCommentId);
        if (entity == null)
        {
            throw new TaktBusinessException("新闻中心评论不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("新闻中心评论不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _newsCommentRepository.UpdateAsync(entity);
        return await GetNewsCommentByIdAsync(dto.NewsCommentId) ?? throw new TaktBusinessException("新闻中心评论不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetNewsCommentTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktNewsCommentTemplateDto>(
            sheetName ?? "新闻中心评论导入模板",
            fileName ?? "新闻中心评论导入模板.xlsx");
    }

    /// <summary>
    /// 导入新闻中心评论
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportNewsCommentAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktNewsCommentImportDto>(fileStream, sheetName ?? "新闻中心评论导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktNewsComment>();
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _newsCommentRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.NewsId == entity.NewsId,
                        x => x.LineNumber);
                    var businessCode = entity.NewsId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _newsCommentRepository.CreateAsync(entity);
                success += 1;
            }
            catch (Exception ex)
            {
                fail += 1;
                errors.Add($"第{i + 2}行: {ex.Message}");
            }
        }
        return (success, fail, errors);
    }

    /// <summary>
    /// 导出新闻中心评论
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportNewsCommentAsync(TaktNewsCommentQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktNewsCommentQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktNewsCommentExportDto>(),
                sheetName ?? "新闻中心评论数据",
                fileName ?? "新闻中心评论导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _newsCommentRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktNewsCommentExportDto>(),
                sheetName ?? "新闻中心评论数据",
                fileName ?? "新闻中心评论导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktNewsCommentExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "新闻中心评论数据",
            fileName ?? "新闻中心评论导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建新闻中心评论查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktNewsComment, bool>> QueryExpression(TaktNewsCommentQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktNewsComment>();

        if (queryDto?.IsObsolete.HasValue == true)
        {
            exp = exp.And(x => x.IsObsolete == queryDto.IsObsolete);
        }
        else
        {
            exp = exp.And(x => x.IsObsolete == 0);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.UserName != null && x.UserName.Contains(keywords))
                || (x.UserAvatar != null && x.UserAvatar.Contains(keywords))
                || (x.ReplyToUserName != null && x.ReplyToUserName.Contains(keywords))
                || (x.CommentContent != null && x.CommentContent.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
            );
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CultureCode))
        {
            var cultureCode = queryDto.CultureCode;
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(cultureCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlantCode))
        {
            var plantCode = queryDto.PlantCode;
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(plantCode));
        }

        if (queryDto?.NewsId.HasValue == true)
        {
            var newsId = queryDto.NewsId.Value;
            exp = exp.And(x => x.NewsId == newsId);
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            var lineNumber = queryDto.LineNumber.Value;
            exp = exp.And(x => x.LineNumber == lineNumber);
        }

        if (queryDto?.ParentId.HasValue == true)
        {
            var parentId = queryDto.ParentId.Value;
            exp = exp.And(x => x.ParentId == parentId);
        }

        if (queryDto?.UserId.HasValue == true)
        {
            var userId = queryDto.UserId.Value;
            exp = exp.And(x => x.UserId == userId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.UserName))
        {
            var UserName = queryDto.UserName;
            exp = exp.And(x => x.UserName != null && x.UserName.Contains(UserName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.UserAvatar))
        {
            var userAvatar = queryDto.UserAvatar;
            exp = exp.And(x => x.UserAvatar != null && x.UserAvatar.Contains(userAvatar));
        }

        if (queryDto?.ReplyToUserId.HasValue == true)
        {
            var replyToUserId = queryDto.ReplyToUserId.Value;
            exp = exp.And(x => x.ReplyToUserId == replyToUserId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ReplyToUserName))
        {
            var replyToUserName = queryDto.ReplyToUserName;
            exp = exp.And(x => x.ReplyToUserName != null && x.ReplyToUserName.Contains(replyToUserName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CommentContent))
        {
            var commentContent = queryDto.CommentContent;
            exp = exp.And(x => x.CommentContent != null && x.CommentContent.Contains(commentContent));
        }

        if (queryDto?.NewsCommentLikeCount.HasValue == true)
        {
            var newsCommentLikeCount = queryDto.NewsCommentLikeCount.Value;
            exp = exp.And(x => x.NewsCommentLikeCount == newsCommentLikeCount);
        }

        if (queryDto?.ReplyCount.HasValue == true)
        {
            var replyCount = queryDto.ReplyCount.Value;
            exp = exp.And(x => x.ReplyCount == replyCount);
        }

        if (queryDto?.CommentLevel.HasValue == true)
        {
            var commentLevel = queryDto.CommentLevel.Value;
            exp = exp.And(x => x.CommentLevel == commentLevel);
        }

        if (queryDto?.CommentStatus.HasValue == true)
        {
            var commentStatus = queryDto.CommentStatus.Value;
            exp = exp.And(x => x.CommentStatus == commentStatus);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ExtField))
        {
            var extField = queryDto.ExtField;
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(extField));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Remark))
        {
            var remark = queryDto.Remark;
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(remark));
        }

        if (queryDto?.CommentTimeStart.HasValue == true)
        {
            var commentTimeStart = queryDto.CommentTimeStart.Value;
            exp = exp.And(x => x.CommentTime >= commentTimeStart);
        }

        if (queryDto?.CommentTimeEnd.HasValue == true)
        {
            var commentTimeEnd = queryDto.CommentTimeEnd.Value;
            exp = exp.And(x => x.CommentTime <= commentTimeEnd);
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            var createdAtStart = queryDto.CreatedAtStart.Value;
            exp = exp.And(x => x.CreatedAt >= createdAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            var createdAtEnd = queryDto.CreatedAtEnd.Value;
            exp = exp.And(x => x.CreatedAt <= createdAtEnd);
        }

        return exp.ToExpression();
    }

    /// <summary>
    /// 是否存在任一业务查询条件（KeyWords / 字段 / 日期范围）；无参时列表与导出返回空，避免全表扫描
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>有条件为 true</returns>
    private static bool HasAnyListQueryFilter(TaktNewsCommentQueryDto? queryDto)
    {
        if (queryDto == null)
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CultureCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlantCode))
        {
            return true;
        }
        if (queryDto.NewsId.HasValue)
        {
            return true;
        }
        if (queryDto.LineNumber.HasValue)
        {
            return true;
        }
        if (queryDto.ParentId.HasValue)
        {
            return true;
        }
        if (queryDto.UserId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.UserName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.UserAvatar))
        {
            return true;
        }
        if (queryDto.ReplyToUserId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ReplyToUserName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CommentContent))
        {
            return true;
        }
        if (queryDto.NewsCommentLikeCount.HasValue)
        {
            return true;
        }
        if (queryDto.ReplyCount.HasValue)
        {
            return true;
        }
        if (queryDto.CommentLevel.HasValue)
        {
            return true;
        }
        if (queryDto.CommentStatus.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ExtField))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Remark))
        {
            return true;
        }
        if (queryDto.IsObsolete.HasValue)
        {
            return true;
        }
        if (queryDto.CommentTimeStart.HasValue || queryDto.CommentTimeEnd.HasValue)
        {
            return true;
        }
        if (queryDto.CreatedAtStart.HasValue || queryDto.CreatedAtEnd.HasValue)
        {
            return true;
        }
        return false;
    }
}
