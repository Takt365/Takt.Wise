// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Sop
// 文件名称：TaktSopDocService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP文档头应用服务实现
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
/// SOP文档头应用服务
/// </summary>
public class TaktSopDocService : TaktServiceBase, ITaktSopDocService
{
    private readonly ITaktApprovalRepository<TaktSopDoc> _sopDocRepository;
    private readonly ITaktCompanyRepository<TaktSopRevision> _sopRevisionRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sopDocRepository">SOP文档头仓储</param>
    /// <param name="sopRevisionRepository">SopRevision仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSopDocService(
        ITaktApprovalRepository<TaktSopDoc> sopDocRepository,
        ITaktCompanyRepository<TaktSopRevision> sopRevisionRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _sopDocRepository = sopDocRepository;
        _sopRevisionRepository = sopRevisionRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取SOP文档头列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSopDocDto>> GetSopDocListAsync(TaktSopDocQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktSopDocDto>.Create(
                new List<TaktSopDocDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _sopDocRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSopDocDto>.Create(
            data.Adapt<List<TaktSopDocDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取SOP文档头
    /// </summary>
    /// <param name="id">SOP文档头ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopDocDto?> GetSopDocByIdAsync(long id)
    {
        var entity = await _sopDocRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktSopDocDto>();
        await FillSopDocDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取SOP文档头选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSopDocOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _sopDocRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SopStatus == 1,
            x => x.SopName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.SopCode,
            DictLabel = e.SopName ?? e.SopCode,
        }).ToList();
    }

    /// <summary>
    /// 创建SOP文档头
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopDocDto> CreateSopDocAsync(TaktSopDocCreateDto dto)
    {
        var entity = dto.Adapt<TaktSopDoc>();
        var isUnique_ix_takt_logistics_manufacturing_sop_doc_code_unique = await _uniqueValidator.IsUniqueAsync(
            _sopDocRepository,
            x => x.PlantCode == entity.PlantCode
                && x.SopCode == entity.SopCode);
        if (!isUnique_ix_takt_logistics_manufacturing_sop_doc_code_unique)
        {
            throw new TaktBusinessException("SOP文档头的PlantCode、SopCode已存在");
        }
        entity = await _sopDocRepository.CreateAsync(entity);
                await SaveSopDocChildrenAsync(entity, dto);
        return await GetSopDocByIdAsync(entity.Id) ?? entity.Adapt<TaktSopDocDto>();
    }

    /// <summary>
    /// 更新SOP文档头
    /// </summary>
    /// <param name="id">SOP文档头ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopDocDto> UpdateSopDocAsync(long id, TaktSopDocUpdateDto dto)
    {
        var entity = await _sopDocRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP文档头不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_sop_doc_code_unique = await _uniqueValidator.IsUniqueAsync(
            _sopDocRepository,
            x => x.PlantCode == entity.PlantCode
                && x.SopCode == entity.SopCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_sop_doc_code_unique)
        {
            throw new TaktBusinessException("SOP文档头的PlantCode、SopCode已存在");
        }
        await _sopDocRepository.UpdateAsync(entity);
                await SaveSopDocChildrenAsync(entity, dto);
        return await GetSopDocByIdAsync(id) ?? throw new TaktBusinessException("SOP文档头不存在");
    }

    /// <summary>
    /// 删除SOP文档头
    /// </summary>
    /// <param name="id">SOP文档头ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSopDocByIdAsync(long id)
    {
        var entity = await _sopDocRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP文档头不存在或已删除");
        }
        await _sopRevisionRepository.DeleteAsync(x => x.SopId == entity.Id);
        var deleted = await _sopDocRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("SOP文档头不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除SOP文档头
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSopDocBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSopDocByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新SOP文档头状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopDocDto> UpdateSopDocStatusAsync(TaktSopDocStatusDto dto)
    {
        var entity = await _sopDocRepository.GetByIdAsync(dto.SopDocId);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP文档头不存在");
        }
        entity.SopStatus = dto.SopStatus;
        await _sopDocRepository.UpdateAsync(entity);
        return await GetSopDocByIdAsync(dto.SopDocId) ?? throw new TaktBusinessException("SOP文档头不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSopDocTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSopDocTemplateDto>(
            sheetName ?? "SOP文档头导入模板",
            fileName ?? "SOP文档头导入模板.xlsx");
    }

    /// <summary>
    /// 导入SOP文档头
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSopDocAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSopDocImportDto>(fileStream, sheetName ?? "SOP文档头导入模板");
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
                var entity = rows[i].Adapt<TaktSopDoc>();
                var importKey = $"{entity.PlantCode}|{entity.SopCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、SopCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_sop_doc_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _sopDocRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.SopCode == entity.SopCode);
                if (!isUnique_ix_takt_logistics_manufacturing_sop_doc_code_unique)
                {
                    throw new TaktBusinessException("SOP文档头的PlantCode、SopCode已存在");
                }
                await _sopDocRepository.CreateAsync(entity);
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
    /// 导出SOP文档头
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSopDocAsync(TaktSopDocQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktSopDocQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSopDocExportDto>(),
                sheetName ?? "SOP文档头数据",
                fileName ?? "SOP文档头导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _sopDocRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSopDocExportDto>(),
                sheetName ?? "SOP文档头数据",
                fileName ?? "SOP文档头导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSopDocExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "SOP文档头数据",
            fileName ?? "SOP文档头导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充SOP文档头详情（加载 OneToMany 子表：SOP版本）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillSopDocDetailsAsync(TaktSopDocDto dto, TaktSopDoc entity)
    {
        if (dto == null)
        {
            return;
        }
        // SOP版本 → dto.Revisions
        var revisions = await _sopRevisionRepository.GetListAsync(x => x.SopId == entity.Id);
        dto.Revisions = revisions.Adapt<List<TaktSopRevisionDto>>();
    }

    /// <summary>
    /// 保存SOP文档头子表级联（SOP版本；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveSopDocChildrenAsync(TaktSopDoc entity, TaktSopDocCreateDto dto)
    {
        // SOP版本（Revisions）
        List<TaktSopRevisionUpdateDto>? revisionsForSave;
        if (dto is TaktSopDocUpdateDto updateDtoForRevisions && updateDtoForRevisions.Revisions != null)
        {
            revisionsForSave = updateDtoForRevisions.Revisions;
        }
        else if (dto.Revisions != null)
        {
            revisionsForSave = dto.Revisions.Adapt<List<TaktSopRevisionUpdateDto>>();
        }
        else
        {
            revisionsForSave = null;
        }
        if (revisionsForSave is not { Count: > 0 })
        {
            await _sopRevisionRepository.DeleteAsync(x => x.SopId == entity.Id);
        }
        else
        {
            var existingList = await _sopRevisionRepository.GetListAsync(x => x.SopId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktSopRevision>();
            for (var i = 0; i < revisionsForSave.Count; i++)
            {
                var childDto = revisionsForSave[i];
                childDto.SopId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                if (childDto.SopRevisionId > 0)
                {
                    if (!existingById.TryGetValue(childDto.SopRevisionId, out var target))
                    {
                        throw new TaktBusinessException("SOP版本不存在（SopRevisionId={childDto.SopRevisionId}）");
                    }
                    if (target.SopId != entity.Id)
                    {
                        throw new TaktBusinessException("SOP版本不属于当前主表（SopRevisionId={childDto.SopRevisionId}）");
                    }
                    submittedIds.Add(childDto.SopRevisionId);
                    childDto.Adapt(target);
                    target.Id = childDto.SopRevisionId;
                    target.SopId = entity.Id;
                    await _sopRevisionRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktSopRevision>();
                    child.Id = 0;
                    child.SopId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _sopRevisionRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                await _sopRevisionRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建SOP文档头查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSopDoc, bool>> QueryExpression(TaktSopDocQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSopDoc>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.SopCode != null && x.SopCode.Contains(keywords))
                || (x.SopName != null && x.SopName.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.SopCode))
        {
            var sopCode = queryDto.SopCode;
            exp = exp.And(x => x.SopCode != null && x.SopCode.Contains(sopCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SopName))
        {
            var sopName = queryDto.SopName;
            exp = exp.And(x => x.SopName != null && x.SopName.Contains(sopName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialCode))
        {
            var materialCode = queryDto.MaterialCode;
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(materialCode));
        }

        if (queryDto?.RoutingItemId.HasValue == true)
        {
            var routingItemId = queryDto.RoutingItemId.Value;
            exp = exp.And(x => x.RoutingItemId == routingItemId);
        }

        if (queryDto?.WorkstationId.HasValue == true)
        {
            var workstationId = queryDto.WorkstationId.Value;
            exp = exp.And(x => x.WorkstationId == workstationId);
        }

        if (queryDto?.CurrentRevisionId.HasValue == true)
        {
            var currentRevisionId = queryDto.CurrentRevisionId.Value;
            exp = exp.And(x => x.CurrentRevisionId == currentRevisionId);
        }

        if (queryDto?.SopStatus.HasValue == true)
        {
            var sopStatus = queryDto.SopStatus.Value;
            exp = exp.And(x => x.SopStatus == sopStatus);
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
    private static bool HasAnyListQueryFilter(TaktSopDocQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.SopCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SopName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialCode))
        {
            return true;
        }
        if (queryDto.RoutingItemId.HasValue)
        {
            return true;
        }
        if (queryDto.WorkstationId.HasValue)
        {
            return true;
        }
        if (queryDto.CurrentRevisionId.HasValue)
        {
            return true;
        }
        if (queryDto.SopStatus.HasValue)
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
