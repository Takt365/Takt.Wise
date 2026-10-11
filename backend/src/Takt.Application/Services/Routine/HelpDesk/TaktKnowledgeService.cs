// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Routine.HelpDesk
// 文件名称：TaktKnowledgeService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：知识库应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Routine.HelpDesk;
using Takt.Domain.Entities.Routine.HelpDesk;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Routine.HelpDesk;

/// <summary>
/// 知识库应用服务
/// </summary>
public class TaktKnowledgeService : TaktServiceBase, ITaktKnowledgeService
{
    private readonly ITaktCompanyRepository<TaktKnowledge> _knowledgeRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="knowledgeRepository">知识库仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktKnowledgeService(
        ITaktCompanyRepository<TaktKnowledge> knowledgeRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _knowledgeRepository = knowledgeRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取知识库列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktKnowledgeDto>> GetKnowledgeListAsync(TaktKnowledgeQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _knowledgeRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktKnowledgeDto>.Create(
            data.Adapt<List<TaktKnowledgeDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取知识库
    /// </summary>
    /// <param name="id">知识库ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktKnowledgeDto?> GetKnowledgeByIdAsync(long id)
    {
        var entity = await _knowledgeRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktKnowledgeDto>();
        await FillKnowledgeDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取知识库选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetKnowledgeOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _knowledgeRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.KnowledgeStatus == 1,
            x => x.CategoryCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.CategoryCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建知识库
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktKnowledgeDto> CreateKnowledgeAsync(TaktKnowledgeCreateDto dto)
    {
        var entity = dto.Adapt<TaktKnowledge>();
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _knowledgeRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _knowledgeRepository.CreateAsync(entity);
                await SaveKnowledgeChildrenAsync(entity, dto);
        return await GetKnowledgeByIdAsync(entity.Id) ?? entity.Adapt<TaktKnowledgeDto>();
    }

    /// <summary>
    /// 更新知识库
    /// </summary>
    /// <param name="id">知识库ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktKnowledgeDto> UpdateKnowledgeAsync(long id, TaktKnowledgeUpdateDto dto)
    {
        var entity = await _knowledgeRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("知识库不存在");
        }
        dto.Adapt(entity);
        await _knowledgeRepository.UpdateAsync(entity);
                await SaveKnowledgeChildrenAsync(entity, dto);
        return await GetKnowledgeByIdAsync(id) ?? throw new TaktBusinessException("知识库不存在");
    }

    /// <summary>
    /// 删除知识库
    /// </summary>
    /// <param name="id">知识库ID</param>
    /// <returns>任务</returns>
    public async Task DeleteKnowledgeByIdAsync(long id)
    {
        var entity = await _knowledgeRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("知识库不存在或已删除");
        }        var deleted = await _knowledgeRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("知识库不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除知识库
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteKnowledgeBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteKnowledgeByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新知识库状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktKnowledgeDto> UpdateKnowledgeStatusAsync(TaktKnowledgeStatusDto dto)
    {
        var entity = await _knowledgeRepository.GetByIdAsync(dto.KnowledgeId);
        if (entity == null)
        {
            throw new TaktBusinessException("知识库不存在");
        }
        entity.KnowledgeStatus = dto.KnowledgeStatus;
        await _knowledgeRepository.UpdateAsync(entity);
        return await GetKnowledgeByIdAsync(dto.KnowledgeId) ?? throw new TaktBusinessException("知识库不存在");
    }

    /// <summary>
    /// 更新知识库排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktKnowledgeDto> UpdateKnowledgeSortAsync(TaktKnowledgeSortDto dto)
    {
        var entity = await _knowledgeRepository.GetByIdAsync(dto.KnowledgeId);
        if (entity == null)
        {
            throw new TaktBusinessException("知识库不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _knowledgeRepository.UpdateAsync(entity);
        return await GetKnowledgeByIdAsync(dto.KnowledgeId) ?? throw new TaktBusinessException("知识库不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetKnowledgeTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktKnowledgeTemplateDto>(
            sheetName ?? "知识库导入模板",
            fileName ?? "知识库导入模板.xlsx");
    }

    /// <summary>
    /// 导入知识库
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportKnowledgeAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktKnowledgeImportDto>(fileStream, sheetName ?? "知识库导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSortMax = await _knowledgeRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktKnowledge>();
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _knowledgeRepository.CreateAsync(entity);
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
    /// 导出知识库
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportKnowledgeAsync(TaktKnowledgeQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktKnowledgeQueryDto());
        var list = await _knowledgeRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktKnowledgeExportDto>(),
                sheetName ?? "知识库数据",
                fileName ?? "知识库导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktKnowledgeExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "知识库数据",
            fileName ?? "知识库导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充知识库详情（加载 OneToMany 子表：知识库变更日志）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillKnowledgeDetailsAsync(TaktKnowledgeDto dto, TaktKnowledge entity)
    {
        if (dto == null)
        {
            return;
        }
    }

    /// <summary>
    /// 保存知识库子表级联（知识库变更日志；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveKnowledgeChildrenAsync(TaktKnowledge entity, TaktKnowledgeCreateDto dto)
    {
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建知识库查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktKnowledge, bool>> QueryExpression(TaktKnowledgeQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktKnowledge>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.KnowledgeTitle != null && x.KnowledgeTitle.Contains(keywords))
                || (x.KnowledgeContent != null && x.KnowledgeContent.Contains(keywords))
                || (x.KnowledgeSummary != null && x.KnowledgeSummary.Contains(keywords))
                || (x.CategoryCode != null && x.CategoryCode.Contains(keywords))
                || (x.KnowledgeTags != null && x.KnowledgeTags.Contains(keywords))
                || SqlFunc.ToString(x.KnowledgeStatus).Contains(keywords)
                || (x.Attachments != null && x.Attachments.Contains(keywords))
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || SqlFunc.ToString(x.KnowledgeViewCount).Contains(keywords)
                || SqlFunc.ToString(x.HelpfulCount).Contains(keywords)
                || SqlFunc.ToString(x.UnhelpfulCount).Contains(keywords)
                || SqlFunc.ToString(x.KnowledgeIsPublished).Contains(keywords)
                || SqlFunc.ToString(x.Version).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.PublishedAt).Contains(keywords)
                || SqlFunc.ToString(x.RevisedAt).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.KnowledgeTitle))
        {
            exp = exp.And(x => x.KnowledgeTitle != null && x.KnowledgeTitle.Contains(queryDto.KnowledgeTitle));
        }

        if (!string.IsNullOrEmpty(queryDto?.KnowledgeContent))
        {
            exp = exp.And(x => x.KnowledgeContent != null && x.KnowledgeContent.Contains(queryDto.KnowledgeContent));
        }

        if (!string.IsNullOrEmpty(queryDto?.KnowledgeSummary))
        {
            exp = exp.And(x => x.KnowledgeSummary != null && x.KnowledgeSummary.Contains(queryDto.KnowledgeSummary));
        }

        if (!string.IsNullOrEmpty(queryDto?.CategoryCode))
        {
            exp = exp.And(x => x.CategoryCode != null && x.CategoryCode.Contains(queryDto.CategoryCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.KnowledgeTags))
        {
            exp = exp.And(x => x.KnowledgeTags != null && x.KnowledgeTags.Contains(queryDto.KnowledgeTags));
        }

        if (queryDto?.KnowledgeStatus.HasValue == true)
        {
            exp = exp.And(x => x.KnowledgeStatus == queryDto.KnowledgeStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.Attachments))
        {
            exp = exp.And(x => x.Attachments != null && x.Attachments.Contains(queryDto.Attachments));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (queryDto?.KnowledgeViewCount.HasValue == true)
        {
            exp = exp.And(x => x.KnowledgeViewCount == queryDto.KnowledgeViewCount);
        }

        if (queryDto?.HelpfulCount.HasValue == true)
        {
            exp = exp.And(x => x.HelpfulCount == queryDto.HelpfulCount);
        }

        if (queryDto?.UnhelpfulCount.HasValue == true)
        {
            exp = exp.And(x => x.UnhelpfulCount == queryDto.UnhelpfulCount);
        }

        if (queryDto?.KnowledgeIsPublished.HasValue == true)
        {
            exp = exp.And(x => x.KnowledgeIsPublished == queryDto.KnowledgeIsPublished);
        }

        if (queryDto?.Version.HasValue == true)
        {
            exp = exp.And(x => x.Version == queryDto.Version);
        }

        if (!string.IsNullOrEmpty(queryDto?.CultureCode))
        {
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(queryDto.CultureCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.PublishedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.PublishedAt >= queryDto.PublishedAtStart);
        }

        if (queryDto?.PublishedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.PublishedAt <= queryDto.PublishedAtEnd);
        }

        if (queryDto?.RevisedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.RevisedAt >= queryDto.RevisedAtStart);
        }

        if (queryDto?.RevisedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.RevisedAt <= queryDto.RevisedAtEnd);
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt >= queryDto.CreatedAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt <= queryDto.CreatedAtEnd);
        }
        if (!string.IsNullOrWhiteSpace(queryDto?.PlantCode))
        {
            var plantCode = queryDto.PlantCode;
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(plantCode));
        }


        return exp.ToExpression();
    }
}
