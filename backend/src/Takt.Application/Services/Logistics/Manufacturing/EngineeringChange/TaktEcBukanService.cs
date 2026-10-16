// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcBukanService.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：设变部管执行应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Shared.Constants;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变部管执行应用服务
/// </summary>
public class TaktEcBukanService : TaktServiceBase, ITaktEcBukanService
{
    private readonly ITaktCompanyRepository<TaktEcBukan> _ecBukanRepository;
    private readonly TaktEcGijutsuStatusSynchronizer _ecGijutsuStatusSynchronizer;
    private readonly TaktEcExecPersistence _ecExecPersistence;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecBukanRepository">设变部管执行仓储</param>
    /// <param name="ecGijutsuStatusSynchronizer">设变技术课状态同步</param>
    /// <param name="ecExecPersistence">设变部门执行持久化</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEcBukanService(
        ITaktCompanyRepository<TaktEcBukan> ecBukanRepository,
        TaktEcGijutsuStatusSynchronizer ecGijutsuStatusSynchronizer,
        TaktEcExecPersistence ecExecPersistence,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ecBukanRepository = ecBukanRepository;
        _ecGijutsuStatusSynchronizer = ecGijutsuStatusSynchronizer;
        _ecExecPersistence = ecExecPersistence;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取设变部管执行列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEcBukanDto>> GetEcBukanListAsync(TaktEcBukanQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _ecBukanRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEcBukanDto>.Create(
            data.Adapt<List<TaktEcBukanDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取设变部管执行
    /// </summary>
    /// <param name="id">设变部管执行ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcBukanDto?> GetEcBukanByIdAsync(long id)
    {
        var entity = await _ecBukanRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktEcBukanDto>();
        await _ecExecPersistence.FillExecViewDetailsAsync(entity, dto);
        return dto;
    }

    /// <summary>
    /// 获取设变部管执行选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEcBukanOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _ecBukanRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.DeptCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.DeptCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建设变部管执行
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcBukanDto> CreateEcBukanAsync(TaktEcBukanCreateDto dto)
    {
        var entity = dto.Adapt<TaktEcBukan>();
        entity.IsObsolete = 0;
        var isUnique_ix_takt_logistics_manufacturing_ec_bukan_unique = await _uniqueValidator.IsUniqueAsync(
            _ecBukanRepository,
            x => x.EcCode == entity.EcCode
                && x.EcDetailId == entity.EcDetailId
                && x.EcNewMaterialCode == entity.EcNewMaterialCode);
        if (!isUnique_ix_takt_logistics_manufacturing_ec_bukan_unique)
        {
            throw new TaktBusinessException("设变部管执行的设变单号、机种编码已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _ecBukanRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.EcDetailId == entity.EcDetailId,
                x => x.LineNumber);
            var businessCode = entity.EcDetailId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _ecBukanRepository.CreateAsync(entity);
        await _ecGijutsuStatusSynchronizer.RefreshByEcCodeAsync(entity.EcCode);
        return await GetEcBukanByIdAsync(entity.Id) ?? entity.Adapt<TaktEcBukanDto>();
    }

    /// <summary>
    /// 更新设变部管执行（同设变单号+机种且采购类型 F、仓库非 C003 的执行行一并写入可填字段）
    /// </summary>
    /// <param name="id">设变部管执行ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcBukanDto> UpdateEcBukanAsync(long id, TaktEcBukanUpdateDto dto)
    {
        var entity = await _ecBukanRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("设变部管执行不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_ec_bukan_unique = await _uniqueValidator.IsUniqueAsync(
            _ecBukanRepository,
            x => x.EcCode == entity.EcCode
                && x.EcDetailId == entity.EcDetailId
                && x.EcNewMaterialCode == entity.EcNewMaterialCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_ec_bukan_unique)
        {
            throw new TaktBusinessException("设变部管执行的设变单号、机种编码已存在");
        }
        await _ecBukanRepository.UpdateAsync(entity);
        await _ecExecPersistence.FanOutBukanFillableByEcModelAndNewMaterialAsync(entity);
        await _ecGijutsuStatusSynchronizer.RefreshByEcCodeAsync(entity.EcCode);
        await _ecExecPersistence.TryCascadeAfterGateDeptCompletedByDetailIdAsync(
            entity.EcDetailId,
            TaktEcDeptCodes.Mc,
            entity);
        return await GetEcBukanByIdAsync(id) ?? throw new TaktBusinessException("设变部管执行不存在");
    }

    /// <summary>
    /// 删除设变部管执行
    /// </summary>
    /// <param name="id">设变部管执行ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEcBukanByIdAsync(long id)
    {
        var entity = await _ecBukanRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("设变部管执行不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("设变部管执行不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("设变部管执行已作废");
        }
        entity.IsObsolete = 1;
        await _ecBukanRepository.UpdateAsync(entity);
        await _ecGijutsuStatusSynchronizer.RefreshByEcCodeAsync(entity.EcCode);
    }

    /// <summary>
    /// 批量删除设变部管执行
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEcBukanBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEcBukanByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新设变部管执行停产状态（同步明细并自动填充/清除执行内容）
    /// </summary>
    /// <param name="dto">停产状态 DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcBukanDto> UpdateEcBukanDiscontinuedStatusAsync(TaktEcBukanDiscontinuedStatusDto dto)
    {
        var entity = await _ecBukanRepository.GetByIdAsync(dto.EcBukanId);
        if (entity == null)
        {
            throw new TaktBusinessException("设变部管执行不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("设变部管执行不存在");
        }
        var status = string.IsNullOrWhiteSpace(dto.DiscontinuedStatus)
            ? TaktEcScopeConstants.PlannedMaterialStatus
            : dto.DiscontinuedStatus.Trim();
        await _ecExecPersistence.ApplyDiscontinuedStatusForDetailAsync(entity.EcDetailId, status);
        return await GetEcBukanByIdAsync(dto.EcBukanId) ?? throw new TaktBusinessException("设变部管执行不存在");
    }

    /// <summary>
    /// 更新设变部管执行作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcBukanDto> UpdateEcBukanObsoleteAsync(TaktEcBukanObsoleteDto dto)
    {
        var entity = await _ecBukanRepository.GetByIdAsync(dto.EcBukanId);
        if (entity == null)
        {
            throw new TaktBusinessException("设变部管执行不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("设变部管执行不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _ecBukanRepository.UpdateAsync(entity);
        await _ecGijutsuStatusSynchronizer.RefreshByEcCodeAsync(entity.EcCode);
        return await GetEcBukanByIdAsync(dto.EcBukanId) ?? throw new TaktBusinessException("设变部管执行不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEcBukanTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEcBukanTemplateDto>(
            sheetName ?? "设变部管执行导入模板",
            fileName ?? "设变部管执行导入模板.xlsx");
    }

    /// <summary>
    /// 导入设变部管执行
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEcBukanAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEcBukanImportDto>(fileStream, sheetName ?? "设变部管执行导入模板");
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
                var entity = rows[i].Adapt<TaktEcBukan>();
                var importKey = $"{entity.EcCode}|{entity.EcModelCodeGroup}|{entity.EcDetailId}|{entity.EcNewMaterialCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（设变单号、机种编码、根物料编码、新物料编码）");
                }
                var isUnique_ix_takt_logistics_manufacturing_ec_bukan_unique = await _uniqueValidator.IsUniqueAsync(
                    _ecBukanRepository,
                    x => x.EcCode == entity.EcCode
                        && x.EcDetailId == entity.EcDetailId
                        && x.EcNewMaterialCode == entity.EcNewMaterialCode);
                if (!isUnique_ix_takt_logistics_manufacturing_ec_bukan_unique)
                {
                    throw new TaktBusinessException("设变部管执行的设变单号、机种编码已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _ecBukanRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.EcDetailId == entity.EcDetailId,
                        x => x.LineNumber);
                    var businessCode = entity.EcDetailId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _ecBukanRepository.CreateAsync(entity);
                await _ecGijutsuStatusSynchronizer.RefreshByEcCodeAsync(entity.EcCode);
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
    /// 导出设变部管执行
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEcBukanAsync(TaktEcBukanQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktEcBukanQueryDto());
        var list = await _ecBukanRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEcBukanExportDto>(),
                sheetName ?? "设变部管执行数据",
                fileName ?? "设变部管执行导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEcBukanExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "设变部管执行数据",
            fileName ?? "设变部管执行导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建设变部管执行查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEcBukan, bool>> QueryExpression(TaktEcBukanQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEcBukan>();

        if (queryDto?.IsObsolete.HasValue == true)
        {
            exp = exp.And(x => x.IsObsolete == queryDto.IsObsolete);
        }
        else
        {
            exp = exp.And(x => x.IsObsolete == 0);
        }
        exp = exp.And(TaktEcBukanQueryHelper.VisibleExecExpression());

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.EcDetailId).Contains(keywords)
                || (x.EcCode != null && x.EcCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.DeptCode != null && x.DeptCode.Contains(keywords))
                || SqlFunc.ToString(x.IsImplemented).Contains(keywords)
                || (x.ExecContent != null && x.ExecContent.Contains(keywords))
                || (x.OutboundBatch != null && x.OutboundBatch.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.OutboundDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.EcDetailId.HasValue == true)
        {
            exp = exp.And(x => x.EcDetailId == queryDto.EcDetailId);
        }

        if (!string.IsNullOrEmpty(queryDto?.EcCode))
        {
            exp = exp.And(x => x.EcCode != null && x.EcCode.Contains(queryDto.EcCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (!string.IsNullOrEmpty(queryDto?.DeptCode))
        {
            exp = exp.And(x => x.DeptCode != null && x.DeptCode.Contains(queryDto.DeptCode));
        }

        if (queryDto?.IsImplemented.HasValue == true)
        {
            exp = exp.And(x => x.IsImplemented == queryDto.IsImplemented);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExecContent))
        {
            exp = exp.And(x => x.ExecContent != null && x.ExecContent.Contains(queryDto.ExecContent));
        }

        if (!string.IsNullOrEmpty(queryDto?.OutboundBatch))
        {
            exp = exp.And(x => x.OutboundBatch != null && x.OutboundBatch.Contains(queryDto.OutboundBatch));
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

        if (queryDto?.OutboundDateStart.HasValue == true)
        {
            exp = exp.And(x => x.OutboundDate >= queryDto.OutboundDateStart);
        }

        if (queryDto?.OutboundDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.OutboundDate <= queryDto.OutboundDateEnd);
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
