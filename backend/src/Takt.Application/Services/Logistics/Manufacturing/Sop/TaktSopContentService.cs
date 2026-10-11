// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Sop
// 文件名称：TaktSopContentService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP多语言正文应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Sop;
using Takt.Domain.Entities.Logistics.Manufacturing.Sop;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Sop;

/// <summary>
/// SOP多语言正文应用服务
/// </summary>
public class TaktSopContentService : TaktServiceBase, ITaktSopContentService
{
    private readonly ITaktCompanyRepository<TaktSopContent> _sopContentRepository;
    private readonly ITaktCompanyRepository<TaktSopStep> _sopStepRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sopContentRepository">SOP多语言正文仓储</param>
    /// <param name="sopStepRepository">SopStep仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSopContentService(
        ITaktCompanyRepository<TaktSopContent> sopContentRepository,
        ITaktCompanyRepository<TaktSopStep> sopStepRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _sopContentRepository = sopContentRepository;
        _sopStepRepository = sopStepRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取SOP多语言正文列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSopContentDto>> GetSopContentListAsync(TaktSopContentQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktSopContentDto>.Create(
                new List<TaktSopContentDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _sopContentRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSopContentDto>.Create(
            data.Adapt<List<TaktSopContentDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取SOP多语言正文
    /// </summary>
    /// <param name="id">SOP多语言正文ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopContentDto?> GetSopContentByIdAsync(long id)
    {
        var entity = await _sopContentRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktSopContentDto>();
        await FillSopContentDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取SOP多语言正文选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSopContentOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _sopContentRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.ContentTitle ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.ContentTitle ?? string.Empty,
            DictLabel = e.ContentTitle ?? string.Empty,
        }).ToList();
    }

    /// <summary>
    /// 创建SOP多语言正文
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopContentDto> CreateSopContentAsync(TaktSopContentCreateDto dto)
    {
        var entity = dto.Adapt<TaktSopContent>();
        var isUnique_ix_takt_logistics_manufacturing_sop_content_culture_unique = await _uniqueValidator.IsUniqueAsync(
            _sopContentRepository,
            x => x.RevisionId == entity.RevisionId
                && x.CultureCode == entity.CultureCode);
        if (!isUnique_ix_takt_logistics_manufacturing_sop_content_culture_unique)
        {
            throw new TaktBusinessException("SOP多语言正文的RevisionId、CultureCode已存在");
        }
        entity = await _sopContentRepository.CreateAsync(entity);
                await SaveSopContentChildrenAsync(entity, dto);
        return await GetSopContentByIdAsync(entity.Id) ?? entity.Adapt<TaktSopContentDto>();
    }

    /// <summary>
    /// 更新SOP多语言正文
    /// </summary>
    /// <param name="id">SOP多语言正文ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopContentDto> UpdateSopContentAsync(long id, TaktSopContentUpdateDto dto)
    {
        var entity = await _sopContentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP多语言正文不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_sop_content_culture_unique = await _uniqueValidator.IsUniqueAsync(
            _sopContentRepository,
            x => x.RevisionId == entity.RevisionId
                && x.CultureCode == entity.CultureCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_sop_content_culture_unique)
        {
            throw new TaktBusinessException("SOP多语言正文的RevisionId、CultureCode已存在");
        }
        await _sopContentRepository.UpdateAsync(entity);
                await SaveSopContentChildrenAsync(entity, dto);
        return await GetSopContentByIdAsync(id) ?? throw new TaktBusinessException("SOP多语言正文不存在");
    }

    /// <summary>
    /// 删除SOP多语言正文
    /// </summary>
    /// <param name="id">SOP多语言正文ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSopContentByIdAsync(long id)
    {
        var entity = await _sopContentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP多语言正文不存在或已删除");
        }
        await _sopStepRepository.DeleteAsync(x => x.ContentId == entity.Id);
        var deleted = await _sopContentRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("SOP多语言正文不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除SOP多语言正文
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSopContentBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSopContentByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSopContentTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSopContentTemplateDto>(
            sheetName ?? "SOP多语言正文导入模板",
            fileName ?? "SOP多语言正文导入模板.xlsx");
    }

    /// <summary>
    /// 导入SOP多语言正文
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSopContentAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSopContentImportDto>(fileStream, sheetName ?? "SOP多语言正文导入模板");
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
                var entity = rows[i].Adapt<TaktSopContent>();
                var importKey = $"{entity.RevisionId}|{entity.CultureCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（RevisionId、CultureCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_sop_content_culture_unique = await _uniqueValidator.IsUniqueAsync(
                    _sopContentRepository,
                    x => x.RevisionId == entity.RevisionId
                        && x.CultureCode == entity.CultureCode);
                if (!isUnique_ix_takt_logistics_manufacturing_sop_content_culture_unique)
                {
                    throw new TaktBusinessException("SOP多语言正文的RevisionId、CultureCode已存在");
                }
                await _sopContentRepository.CreateAsync(entity);
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
    /// 导出SOP多语言正文
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSopContentAsync(TaktSopContentQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktSopContentQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSopContentExportDto>(),
                sheetName ?? "SOP多语言正文数据",
                fileName ?? "SOP多语言正文导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _sopContentRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSopContentExportDto>(),
                sheetName ?? "SOP多语言正文数据",
                fileName ?? "SOP多语言正文导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSopContentExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "SOP多语言正文数据",
            fileName ?? "SOP多语言正文导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充SOP多语言正文详情（加载 OneToMany 子表：SOP工步）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillSopContentDetailsAsync(TaktSopContentDto dto, TaktSopContent entity)
    {
        if (dto == null)
        {
            return;
        }
        // SOP工步 → dto.Steps
        var steps = await _sopStepRepository.GetListAsync(x => x.ContentId == entity.Id);
        dto.Steps = steps.Adapt<List<TaktSopStepDto>>();
    }

    /// <summary>
    /// 保存SOP多语言正文子表级联（SOP工步；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveSopContentChildrenAsync(TaktSopContent entity, TaktSopContentCreateDto dto)
    {
        // SOP工步（Steps）
        List<TaktSopStepUpdateDto>? stepsForSave;
        if (dto is TaktSopContentUpdateDto updateDtoForSteps && updateDtoForSteps.Steps != null)
        {
            stepsForSave = updateDtoForSteps.Steps;
        }
        else if (dto.Steps != null)
        {
            stepsForSave = dto.Steps.Adapt<List<TaktSopStepUpdateDto>>();
        }
        else
        {
            stepsForSave = null;
        }
        if (stepsForSave is not { Count: > 0 })
        {
            await _sopStepRepository.DeleteAsync(x => x.ContentId == entity.Id);
        }
        else
        {
            var existingList = await _sopStepRepository.GetListAsync(x => x.ContentId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktSopStep>();
            for (var i = 0; i < stepsForSave.Count; i++)
            {
                var childDto = stepsForSave[i];
                childDto.ContentId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                if (childDto.SopStepId > 0)
                {
                    if (!existingById.TryGetValue(childDto.SopStepId, out var target))
                    {
                        throw new TaktBusinessException("SOP工步不存在（SopStepId={childDto.SopStepId}）");
                    }
                    if (target.ContentId != entity.Id)
                    {
                        throw new TaktBusinessException("SOP工步不属于当前主表（SopStepId={childDto.SopStepId}）");
                    }
                    submittedIds.Add(childDto.SopStepId);
                    childDto.Adapt(target);
                    target.Id = childDto.SopStepId;
                    target.ContentId = entity.Id;
                    await _sopStepRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktSopStep>();
                    child.Id = 0;
                    child.ContentId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _sopStepRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                await _sopStepRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建SOP多语言正文查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSopContent, bool>> QueryExpression(TaktSopContentQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSopContent>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ContentTitle != null && x.ContentTitle.Contains(keywords))
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

        if (queryDto?.RevisionId.HasValue == true)
        {
            var revisionId = queryDto.RevisionId.Value;
            exp = exp.And(x => x.RevisionId == revisionId);
        }

        if (queryDto?.SopId.HasValue == true)
        {
            var sopId = queryDto.SopId.Value;
            exp = exp.And(x => x.SopId == sopId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ContentTitle))
        {
            var contentTitle = queryDto.ContentTitle;
            exp = exp.And(x => x.ContentTitle != null && x.ContentTitle.Contains(contentTitle));
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
    private static bool HasAnyListQueryFilter(TaktSopContentQueryDto? queryDto)
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
        if (queryDto.RevisionId.HasValue)
        {
            return true;
        }
        if (queryDto.SopId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ContentTitle))
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
        if (queryDto.CreatedAtStart.HasValue || queryDto.CreatedAtEnd.HasValue)
        {
            return true;
        }
        return false;
    }
}
