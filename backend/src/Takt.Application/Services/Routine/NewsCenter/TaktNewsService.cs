// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Routine.NewsCenter
// 文件名称：TaktNewsService.cs
// 创建时间：2026-08-24
// 创建人：Takt365(Cursor AI)
// 功能描述：新闻中心应用服务实现
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
/// 新闻中心应用服务
/// </summary>
public class TaktNewsService : TaktServiceBase, ITaktNewsService
{
    private readonly ITaktApprovalRepository<TaktNews> _newsRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="newsRepository">新闻中心仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktNewsService(
        ITaktApprovalRepository<TaktNews> newsRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _newsRepository = newsRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取新闻中心列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktNewsDto>> GetNewsListAsync(TaktNewsQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktNewsDto>.Create(
                new List<TaktNewsDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _newsRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktNewsDto>.Create(
            data.Adapt<List<TaktNewsDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取新闻中心
    /// </summary>
    /// <param name="id">新闻中心ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsDto?> GetNewsByIdAsync(long id)
    {
        var entity = await _newsRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktNewsDto>();
    }

    /// <summary>
    /// 获取新闻中心选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetNewsOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _newsRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.NewsStatus == 1,
            x => x.DeptName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.NewsCode,
            DictLabel = e.DeptName ?? e.NewsCode,
        }).ToList();
    }

    /// <summary>
    /// 创建新闻中心
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsDto> CreateNewsAsync(TaktNewsCreateDto dto)
    {
        var entity = dto.Adapt<TaktNews>();
        var isUnique_ix_news_code_unique = await _uniqueValidator.IsUniqueAsync(
            _newsRepository,
            x => x.NewsCode == entity.NewsCode);
        if (!isUnique_ix_news_code_unique)
        {
            throw new TaktBusinessException("新闻中心的NewsCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _newsRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.DeptId == entity.DeptId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.DeptId.GetValueOrDefault(), maxSort);
        }
        entity = await _newsRepository.CreateAsync(entity);
        return await GetNewsByIdAsync(entity.Id) ?? entity.Adapt<TaktNewsDto>();
    }

    /// <summary>
    /// 更新新闻中心
    /// </summary>
    /// <param name="id">新闻中心ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsDto> UpdateNewsAsync(long id, TaktNewsUpdateDto dto)
    {
        var entity = await _newsRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("新闻中心不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_news_code_unique = await _uniqueValidator.IsUniqueAsync(
            _newsRepository,
            x => x.NewsCode == entity.NewsCode,
            id);
        if (!isUnique_ix_news_code_unique)
        {
            throw new TaktBusinessException("新闻中心的NewsCode已存在");
        }
        await _newsRepository.UpdateAsync(entity);
        return await GetNewsByIdAsync(id) ?? throw new TaktBusinessException("新闻中心不存在");
    }

    /// <summary>
    /// 删除新闻中心
    /// </summary>
    /// <param name="id">新闻中心ID</param>
    /// <returns>任务</returns>
    public async Task DeleteNewsByIdAsync(long id)
    {
        var deleted = await _newsRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("新闻中心不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除新闻中心
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteNewsBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteNewsByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新新闻中心状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsDto> UpdateNewsStatusAsync(TaktNewsStatusDto dto)
    {
        var entity = await _newsRepository.GetByIdAsync(dto.NewsId);
        if (entity == null)
        {
            throw new TaktBusinessException("新闻中心不存在");
        }
        entity.NewsStatus = dto.NewsStatus;
        await _newsRepository.UpdateAsync(entity);
        return await GetNewsByIdAsync(dto.NewsId) ?? throw new TaktBusinessException("新闻中心不存在");
    }

    /// <summary>
    /// 更新新闻中心排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsDto> UpdateNewsSortAsync(TaktNewsSortDto dto)
    {
        var entity = await _newsRepository.GetByIdAsync(dto.NewsId);
        if (entity == null)
        {
            throw new TaktBusinessException("新闻中心不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _newsRepository.UpdateAsync(entity);
        return await GetNewsByIdAsync(dto.NewsId) ?? throw new TaktBusinessException("新闻中心不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetNewsTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktNewsTemplateDto>(
            sheetName ?? "新闻中心导入模板",
            fileName ?? "新闻中心导入模板.xlsx");
    }

    /// <summary>
    /// 导入新闻中心
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportNewsAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktNewsImportDto>(fileStream, sheetName ?? "新闻中心导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktNews>();
                var importKey = $"{entity.NewsCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（NewsCode）");
                }
                var isUnique_ix_news_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _newsRepository,
                    x => x.NewsCode == entity.NewsCode);
                if (!isUnique_ix_news_code_unique)
                {
                    throw new TaktBusinessException("新闻中心的NewsCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _newsRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.DeptId == entity.DeptId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.DeptId.GetValueOrDefault(), maxSort);
                }
                await _newsRepository.CreateAsync(entity);
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
    /// 导出新闻中心
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportNewsAsync(TaktNewsQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktNewsQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktNewsExportDto>(),
                sheetName ?? "新闻中心数据",
                fileName ?? "新闻中心导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _newsRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktNewsExportDto>(),
                sheetName ?? "新闻中心数据",
                fileName ?? "新闻中心导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktNewsExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "新闻中心数据",
            fileName ?? "新闻中心导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建新闻中心查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktNews, bool>> QueryExpression(TaktNewsQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktNews>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.NewsCode != null && x.NewsCode.Contains(keywords))
                || (x.NewsTitle != null && x.NewsTitle.Contains(keywords))
                || (x.NewsSummary != null && x.NewsSummary.Contains(keywords))
                || (x.NewsTags != null && x.NewsTags.Contains(keywords))
                || (x.NewsContent != null && x.NewsContent.Contains(keywords))
                || (x.NewsCoverImage != null && x.NewsCoverImage.Contains(keywords))
                || (x.DeptName != null && x.DeptName.Contains(keywords))
                || (x.PublisherName != null && x.PublisherName.Contains(keywords))
                || (x.TargetDepartments != null && x.TargetDepartments.Contains(keywords))
                || (x.TargetUsers != null && x.TargetUsers.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.NewsCode))
        {
            var newsCode = queryDto.NewsCode;
            exp = exp.And(x => x.NewsCode != null && x.NewsCode.Contains(newsCode));
        }

        if (queryDto?.NewsCategory.HasValue == true)
        {
            var newsCategory = queryDto.NewsCategory.Value;
            exp = exp.And(x => x.NewsCategory == newsCategory);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.NewsTitle))
        {
            var newsTitle = queryDto.NewsTitle;
            exp = exp.And(x => x.NewsTitle != null && x.NewsTitle.Contains(newsTitle));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.NewsSummary))
        {
            var newsSummary = queryDto.NewsSummary;
            exp = exp.And(x => x.NewsSummary != null && x.NewsSummary.Contains(newsSummary));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.NewsTags))
        {
            var newsTags = queryDto.NewsTags;
            exp = exp.And(x => x.NewsTags != null && x.NewsTags.Contains(newsTags));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.NewsContent))
        {
            var newsContent = queryDto.NewsContent;
            exp = exp.And(x => x.NewsContent != null && x.NewsContent.Contains(newsContent));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.NewsCoverImage))
        {
            var newsCoverImage = queryDto.NewsCoverImage;
            exp = exp.And(x => x.NewsCoverImage != null && x.NewsCoverImage.Contains(newsCoverImage));
        }

        if (queryDto?.NewsIsTop.HasValue == true)
        {
            var newsIsTop = queryDto.NewsIsTop.Value;
            exp = exp.And(x => x.NewsIsTop == newsIsTop);
        }

        if (queryDto?.NewsIsRecommended.HasValue == true)
        {
            var newsIsRecommended = queryDto.NewsIsRecommended.Value;
            exp = exp.And(x => x.NewsIsRecommended == newsIsRecommended);
        }

        if (queryDto?.NewsReadCount.HasValue == true)
        {
            var newsReadCount = queryDto.NewsReadCount.Value;
            exp = exp.And(x => x.NewsReadCount == newsReadCount);
        }

        if (queryDto?.NewsLikeCount.HasValue == true)
        {
            var newsLikeCount = queryDto.NewsLikeCount.Value;
            exp = exp.And(x => x.NewsLikeCount == newsLikeCount);
        }

        if (queryDto?.NewsCommentCount.HasValue == true)
        {
            var newsCommentCount = queryDto.NewsCommentCount.Value;
            exp = exp.And(x => x.NewsCommentCount == newsCommentCount);
        }

        if (queryDto?.NewsFavoriteCount.HasValue == true)
        {
            var newsFavoriteCount = queryDto.NewsFavoriteCount.Value;
            exp = exp.And(x => x.NewsFavoriteCount == newsFavoriteCount);
        }

        if (queryDto?.NewsShareCount.HasValue == true)
        {
            var newsShareCount = queryDto.NewsShareCount.Value;
            exp = exp.And(x => x.NewsShareCount == newsShareCount);
        }

        if (queryDto?.DeptId.HasValue == true)
        {
            var deptId = queryDto.DeptId.Value;
            exp = exp.And(x => x.DeptId == deptId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DeptName))
        {
            var deptName = queryDto.DeptName;
            exp = exp.And(x => x.DeptName != null && x.DeptName.Contains(deptName));
        }

        if (queryDto?.PublisherId.HasValue == true)
        {
            var publisherId = queryDto.PublisherId.Value;
            exp = exp.And(x => x.PublisherId == publisherId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PublisherName))
        {
            var publisherName = queryDto.PublisherName;
            exp = exp.And(x => x.PublisherName != null && x.PublisherName.Contains(publisherName));
        }

        if (queryDto?.TargetScope.HasValue == true)
        {
            var targetScope = queryDto.TargetScope.Value;
            exp = exp.And(x => x.TargetScope == targetScope);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TargetDepartments))
        {
            var targetDepartments = queryDto.TargetDepartments;
            exp = exp.And(x => x.TargetDepartments != null && x.TargetDepartments.Contains(targetDepartments));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TargetUsers))
        {
            var targetUsers = queryDto.TargetUsers;
            exp = exp.And(x => x.TargetUsers != null && x.TargetUsers.Contains(targetUsers));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            var sortOrder = queryDto.SortOrder.Value;
            exp = exp.And(x => x.SortOrder == sortOrder);
        }

        if (queryDto?.NewsStatus.HasValue == true)
        {
            var newsStatus = queryDto.NewsStatus.Value;
            exp = exp.And(x => x.NewsStatus == newsStatus);
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

        if (queryDto?.NewsEffectiveTimeStart.HasValue == true)
        {
            var newsEffectiveTimeStart = queryDto.NewsEffectiveTimeStart.Value;
            exp = exp.And(x => x.NewsEffectiveTime >= newsEffectiveTimeStart);
        }

        if (queryDto?.NewsEffectiveTimeEnd.HasValue == true)
        {
            var newsEffectiveTimeEnd = queryDto.NewsEffectiveTimeEnd.Value;
            exp = exp.And(x => x.NewsEffectiveTime <= newsEffectiveTimeEnd);
        }

        if (queryDto?.NewsExpireTimeStart.HasValue == true)
        {
            var newsExpireTimeStart = queryDto.NewsExpireTimeStart.Value;
            exp = exp.And(x => x.NewsExpireTime >= newsExpireTimeStart);
        }

        if (queryDto?.NewsExpireTimeEnd.HasValue == true)
        {
            var newsExpireTimeEnd = queryDto.NewsExpireTimeEnd.Value;
            exp = exp.And(x => x.NewsExpireTime <= newsExpireTimeEnd);
        }

        if (queryDto?.NewsPublishTimeStart.HasValue == true)
        {
            var newsPublishTimeStart = queryDto.NewsPublishTimeStart.Value;
            exp = exp.And(x => x.NewsPublishTime >= newsPublishTimeStart);
        }

        if (queryDto?.NewsPublishTimeEnd.HasValue == true)
        {
            var newsPublishTimeEnd = queryDto.NewsPublishTimeEnd.Value;
            exp = exp.And(x => x.NewsPublishTime <= newsPublishTimeEnd);
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
    private static bool HasAnyListQueryFilter(TaktNewsQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.NewsCode))
        {
            return true;
        }
        if (queryDto.NewsCategory.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.NewsTitle))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.NewsSummary))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.NewsTags))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.NewsContent))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.NewsCoverImage))
        {
            return true;
        }
        if (queryDto.NewsIsTop.HasValue)
        {
            return true;
        }
        if (queryDto.NewsIsRecommended.HasValue)
        {
            return true;
        }
        if (queryDto.NewsReadCount.HasValue)
        {
            return true;
        }
        if (queryDto.NewsLikeCount.HasValue)
        {
            return true;
        }
        if (queryDto.NewsCommentCount.HasValue)
        {
            return true;
        }
        if (queryDto.NewsFavoriteCount.HasValue)
        {
            return true;
        }
        if (queryDto.NewsShareCount.HasValue)
        {
            return true;
        }
        if (queryDto.DeptId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DeptName))
        {
            return true;
        }
        if (queryDto.PublisherId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PublisherName))
        {
            return true;
        }
        if (queryDto.TargetScope.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TargetDepartments))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TargetUsers))
        {
            return true;
        }
        if (queryDto.SortOrder.HasValue)
        {
            return true;
        }
        if (queryDto.NewsStatus.HasValue)
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
        if (queryDto.NewsEffectiveTimeStart.HasValue || queryDto.NewsEffectiveTimeEnd.HasValue)
        {
            return true;
        }
        if (queryDto.NewsExpireTimeStart.HasValue || queryDto.NewsExpireTimeEnd.HasValue)
        {
            return true;
        }
        if (queryDto.NewsPublishTimeStart.HasValue || queryDto.NewsPublishTimeEnd.HasValue)
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
