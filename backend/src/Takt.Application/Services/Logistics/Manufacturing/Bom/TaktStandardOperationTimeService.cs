// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktStandardOperationTimeService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：标准工序时间应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// 标准工序时间应用服务
/// </summary>
public class TaktStandardOperationTimeService : TaktServiceBase, ITaktStandardOperationTimeService
{
    private const int MaxStandardOperationTimeRowsByMaterial = 50;

    private readonly ITaktApprovalRepository<TaktStandardOperationTime> _standardOperationTimeRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="standardOperationTimeRepository">标准工序时间仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktStandardOperationTimeService(
        ITaktApprovalRepository<TaktStandardOperationTime> standardOperationTimeRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _standardOperationTimeRepository = standardOperationTimeRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取标准工序时间列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktStandardOperationTimeDto>> GetStandardOperationTimeListAsync(TaktStandardOperationTimeQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _standardOperationTimeRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktStandardOperationTimeDto>.Create(
            data.Adapt<List<TaktStandardOperationTimeDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取标准工序时间
    /// </summary>
    /// <param name="id">标准工序时间ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktStandardOperationTimeDto?> GetStandardOperationTimeByIdAsync(long id)
    {
        var entity = await _standardOperationTimeRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktStandardOperationTimeDto>();
        await FillStandardOperationTimeDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取标准工序时间选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetStandardOperationTimeOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _standardOperationTimeRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.PlantCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PlantCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 根据物料编码获取当前有效的标准工序时间列表
    /// </summary>
    /// <param name="materialCode">物料编码</param>
    /// <param name="plantCode">工厂代码（可选）</param>
    /// <param name="prodDate">生产日期（可选；未传时按当天解析有效期）</param>
    /// <returns>标准工序时间 DTO 列表</returns>
    public async Task<List<TaktStandardOperationTimeDto>> GetStandardOperationTimeByMaterialAsync(string materialCode, string? plantCode = null, DateTime? prodDate = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(materialCode);
        var list = await QueryEffectiveStandardOperationTimesByMaterialAsync(materialCode.Trim(), plantCode?.Trim(), prodDate);
        var effectiveRows = TaktStandardOperationTimeEffectiveResolver.SelectLatestPerWorkCenter(list);
        return effectiveRows.Adapt<List<TaktStandardOperationTimeDto>>();
    }

    /// <summary>
    /// 根据物料编码获取当前有效的标准工时选项列表
    /// </summary>
    /// <param name="materialCode">物料编码</param>
    /// <param name="plantCode">工厂代码（可选）</param>
    /// <param name="prodDate">生产日期（可选；未传时按当天解析有效期）</param>
    /// <returns>下拉选项（ExtValue=标准工时分钟数）</returns>
    public async Task<List<TaktSelectOption>> GetStandardOperationTimeOptionsByMaterialAsync(string materialCode, string? plantCode = null, DateTime? prodDate = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(materialCode);
        var list = await QueryEffectiveStandardOperationTimesByMaterialAsync(materialCode, plantCode, prodDate);
        var effectiveRows = TaktStandardOperationTimeEffectiveResolver.SelectLatestPerWorkCenter(list);
        return effectiveRows.Select(e =>
        {
            var minutes = e.ConvertedMinutes > 0 ? e.ConvertedMinutes : e.StandardMinutes;
            var label = string.IsNullOrWhiteSpace(e.OperationDesc)
                ? $"{e.WorkCenter} ({minutes}min)"
                : $"{e.WorkCenter} - {e.OperationDesc} ({minutes}min)";
            return new TaktSelectOption
            {
                DictValue = e.Id,
                DictLabel = label,
                ExtValue = minutes,
                ExtLabel = e.WorkCenter,
            };
        }).ToList();
    }

    /// <summary>
    /// 查询物料当前有效的标准工序时间实体列表
    /// </summary>
    /// <param name="materialCode">物料编码</param>
    /// <param name="plantCode">工厂代码（可选）</param>
    /// <param name="prodDate">生产日期（可选；未传时按当天解析有效期）</param>
    /// <returns>标准工序时间实体列表</returns>
    private async Task<List<TaktStandardOperationTime>> QueryEffectiveStandardOperationTimesByMaterialAsync(string materialCode, string? plantCode, DateTime? prodDate = null)
    {
        EnsureThreeLayerContext();
        var effectiveDate = (prodDate ?? DateTime.Today).Date;
        var list = await GetEffectiveStandardOperationTimesByMaterialCoreAsync(materialCode, plantCode, effectiveDate);
        if (list.Count == 0 && !string.IsNullOrWhiteSpace(plantCode))
        {
            list = await GetEffectiveStandardOperationTimesByMaterialCoreAsync(materialCode, null, effectiveDate);
        }
        return list.Take(MaxStandardOperationTimeRowsByMaterial).ToList();
    }

    /// <summary>
    /// 按物料与可选工厂查询生产日期有效的标准工序时间（不含工厂回退）
    /// </summary>
    /// <param name="materialCode">物料编码（已 trim）</param>
    /// <param name="plantCode">工厂代码（可选，已 trim）</param>
    /// <param name="effectiveDate">生产日期（仅日期部分）</param>
    /// <returns>标准工序时间实体列表</returns>
    private async Task<List<TaktStandardOperationTime>> GetEffectiveStandardOperationTimesByMaterialCoreAsync(
        string materialCode,
        string? plantCode,
        DateTime effectiveDate)
    {
        return await _standardOperationTimeRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.MaterialCode == materialCode
                && x.ApprovalStatus == TaktProductionStatHelper.StandardOperationTimeApprovalCompleted
                && x.EffectiveDate <= effectiveDate
                && (x.ExpiryDate == null || x.ExpiryDate >= effectiveDate)
                && (string.IsNullOrWhiteSpace(plantCode) || x.PlantCode == plantCode),
            x => x.EffectiveDate,
            true);
    }

    /// <summary>
    /// 创建标准工序时间
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktStandardOperationTimeDto> CreateStandardOperationTimeAsync(TaktStandardOperationTimeCreateDto dto)
    {
        var entity = dto.Adapt<TaktStandardOperationTime>();
        var isUnique_ix_takt_logistics_manufacturing_bom_standard_operation_time_unique = await _uniqueValidator.IsUniqueAsync(
            _standardOperationTimeRepository,
            x => x.PlantCode == entity.PlantCode
                && x.MaterialCode == entity.MaterialCode
                && x.WorkCenter == entity.WorkCenter);
        if (!isUnique_ix_takt_logistics_manufacturing_bom_standard_operation_time_unique)
        {
            throw new TaktBusinessException("标准工序时间的PlantCode、MaterialCode、WorkCenter已存在");
        }
        entity = await _standardOperationTimeRepository.CreateAsync(entity);
                await SaveStandardOperationTimeChildrenAsync(entity, dto);
        return await GetStandardOperationTimeByIdAsync(entity.Id) ?? entity.Adapt<TaktStandardOperationTimeDto>();
    }

    /// <summary>
    /// 更新标准工序时间
    /// </summary>
    /// <param name="id">标准工序时间ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktStandardOperationTimeDto> UpdateStandardOperationTimeAsync(long id, TaktStandardOperationTimeUpdateDto dto)
    {
        var entity = await _standardOperationTimeRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("标准工序时间不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_bom_standard_operation_time_unique = await _uniqueValidator.IsUniqueAsync(
            _standardOperationTimeRepository,
            x => x.PlantCode == entity.PlantCode
                && x.MaterialCode == entity.MaterialCode
                && x.WorkCenter == entity.WorkCenter,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_bom_standard_operation_time_unique)
        {
            throw new TaktBusinessException("标准工序时间的PlantCode、MaterialCode、WorkCenter已存在");
        }
        await _standardOperationTimeRepository.UpdateAsync(entity);
                await SaveStandardOperationTimeChildrenAsync(entity, dto);
        return await GetStandardOperationTimeByIdAsync(id) ?? throw new TaktBusinessException("标准工序时间不存在");
    }

    /// <summary>
    /// 删除标准工序时间
    /// </summary>
    /// <param name="id">标准工序时间ID</param>
    /// <returns>任务</returns>
    public async Task DeleteStandardOperationTimeByIdAsync(long id)
    {
        var entity = await _standardOperationTimeRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("标准工序时间不存在或已删除");
        }        var deleted = await _standardOperationTimeRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("标准工序时间不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除标准工序时间
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteStandardOperationTimeBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteStandardOperationTimeByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetStandardOperationTimeTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktStandardOperationTimeTemplateDto>(
            sheetName ?? "标准工序时间导入模板",
            fileName ?? "标准工序时间导入模板.xlsx");
    }

    /// <summary>
    /// 导入标准工序时间
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportStandardOperationTimeAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktStandardOperationTimeImportDto>(fileStream, sheetName ?? "标准工序时间导入模板");
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
                var entity = rows[i].Adapt<TaktStandardOperationTime>();
                var importKey = $"{entity.PlantCode}|{entity.MaterialCode}|{entity.WorkCenter}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、MaterialCode、WorkCenter）");
                }
                var isUnique_ix_takt_logistics_manufacturing_bom_standard_operation_time_unique = await _uniqueValidator.IsUniqueAsync(
                    _standardOperationTimeRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.MaterialCode == entity.MaterialCode
                        && x.WorkCenter == entity.WorkCenter);
                if (!isUnique_ix_takt_logistics_manufacturing_bom_standard_operation_time_unique)
                {
                    throw new TaktBusinessException("标准工序时间的PlantCode、MaterialCode、WorkCenter已存在");
                }
                await _standardOperationTimeRepository.CreateAsync(entity);
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
    /// 导出标准工序时间
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportStandardOperationTimeAsync(TaktStandardOperationTimeQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktStandardOperationTimeQueryDto());
        var list = await _standardOperationTimeRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktStandardOperationTimeExportDto>(),
                sheetName ?? "标准工序时间数据",
                fileName ?? "标准工序时间导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktStandardOperationTimeExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "标准工序时间数据",
            fileName ?? "标准工序时间导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充标准工序时间详情（加载 OneToMany 子表：标准工序时间变更记录）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillStandardOperationTimeDetailsAsync(TaktStandardOperationTimeDto dto, TaktStandardOperationTime entity)
    {
        if (dto == null)
        {
            return;
        }
    }

    /// <summary>
    /// 保存标准工序时间子表级联（标准工序时间变更记录；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveStandardOperationTimeChildrenAsync(TaktStandardOperationTime entity, TaktStandardOperationTimeCreateDto dto)
    {
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建标准工序时间查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktStandardOperationTime, bool>> QueryExpression(TaktStandardOperationTimeQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktStandardOperationTime>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.WorkCenter != null && x.WorkCenter.Contains(keywords))
                || (x.OperationDesc != null && x.OperationDesc.Contains(keywords))
                || SqlFunc.ToString(x.StandardMinutes).Contains(keywords)
                || (x.TimeUnit != null && x.TimeUnit.Contains(keywords))
                || SqlFunc.ToString(x.StandardShorts).Contains(keywords)
                || (x.PointsUnit != null && x.PointsUnit.Contains(keywords))
                || SqlFunc.ToString(x.PointsToMinutesRate).Contains(keywords)
                || SqlFunc.ToString(x.ConvertedMinutes).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.EffectiveDate).Contains(keywords)
                || SqlFunc.ToString(x.ExpiryDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialCode))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(queryDto.MaterialCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.WorkCenter))
        {
            exp = exp.And(x => x.WorkCenter != null && x.WorkCenter.Contains(queryDto.WorkCenter));
        }

        if (!string.IsNullOrEmpty(queryDto?.OperationDesc))
        {
            exp = exp.And(x => x.OperationDesc != null && x.OperationDesc.Contains(queryDto.OperationDesc));
        }

        if (queryDto?.StandardMinutes.HasValue == true)
        {
            exp = exp.And(x => x.StandardMinutes == queryDto.StandardMinutes);
        }

        if (!string.IsNullOrEmpty(queryDto?.TimeUnit))
        {
            exp = exp.And(x => x.TimeUnit != null && x.TimeUnit.Contains(queryDto.TimeUnit));
        }

        if (queryDto?.StandardShorts.HasValue == true)
        {
            exp = exp.And(x => x.StandardShorts == queryDto.StandardShorts);
        }

        if (!string.IsNullOrEmpty(queryDto?.PointsUnit))
        {
            exp = exp.And(x => x.PointsUnit != null && x.PointsUnit.Contains(queryDto.PointsUnit));
        }

        if (queryDto?.PointsToMinutesRate.HasValue == true)
        {
            exp = exp.And(x => x.PointsToMinutesRate == queryDto.PointsToMinutesRate);
        }

        if (queryDto?.ConvertedMinutes.HasValue == true)
        {
            exp = exp.And(x => x.ConvertedMinutes == queryDto.ConvertedMinutes);
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

        if (queryDto?.EffectiveDateStart.HasValue == true)
        {
            exp = exp.And(x => x.EffectiveDate >= queryDto.EffectiveDateStart);
        }

        if (queryDto?.EffectiveDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.EffectiveDate <= queryDto.EffectiveDateEnd);
        }

        if (queryDto?.ExpiryDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ExpiryDate >= queryDto.ExpiryDateStart);
        }

        if (queryDto?.ExpiryDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ExpiryDate <= queryDto.ExpiryDateEnd);
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt >= queryDto.CreatedAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt <= queryDto.CreatedAtEnd);
        }

        return exp.ToExpression();
    }
}
