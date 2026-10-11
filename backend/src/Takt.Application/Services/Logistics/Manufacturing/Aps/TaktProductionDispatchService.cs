// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Aps
// 文件名称：TaktProductionDispatchService.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：生产派工单应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Aps;
using Takt.Domain.Entities.Logistics.Manufacturing.Aps;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Aps;

/// <summary>
/// 生产派工单应用服务
/// </summary>
public class TaktProductionDispatchService : TaktServiceBase, ITaktProductionDispatchService
{
    private readonly ITaktCompanyRepository<TaktProductionDispatch> _productionDispatchRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="productionDispatchRepository">生产派工单仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktProductionDispatchService(
        ITaktCompanyRepository<TaktProductionDispatch> productionDispatchRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _productionDispatchRepository = productionDispatchRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取生产派工单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktProductionDispatchDto>> GetProductionDispatchListAsync(TaktProductionDispatchQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _productionDispatchRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktProductionDispatchDto>.Create(
            data.Adapt<List<TaktProductionDispatchDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取生产派工单
    /// </summary>
    /// <param name="id">生产派工单ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionDispatchDto?> GetProductionDispatchByIdAsync(long id)
    {
        var entity = await _productionDispatchRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktProductionDispatchDto>();
    }

    /// <summary>
    /// 获取生产派工单选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetProductionDispatchOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _productionDispatchRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.DispatchStatus == 1,
            x => x.PlantCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PlantCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建生产派工单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionDispatchDto> CreateProductionDispatchAsync(TaktProductionDispatchCreateDto dto)
    {
        var entity = dto.Adapt<TaktProductionDispatch>();
        var isUnique_ix_takt_logistics_manufacturing_scheduling_dispatch_unique = await _uniqueValidator.IsUniqueAsync(
            _productionDispatchRepository,
            x => x.PlantCode == entity.PlantCode
                && x.DispatchCode == entity.DispatchCode);
        if (!isUnique_ix_takt_logistics_manufacturing_scheduling_dispatch_unique)
        {
            throw new TaktBusinessException("生产派工单的PlantCode、DispatchCode已存在");
        }
        entity = await _productionDispatchRepository.CreateAsync(entity);
        return await GetProductionDispatchByIdAsync(entity.Id) ?? entity.Adapt<TaktProductionDispatchDto>();
    }

    /// <summary>
    /// 更新生产派工单
    /// </summary>
    /// <param name="id">生产派工单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionDispatchDto> UpdateProductionDispatchAsync(long id, TaktProductionDispatchUpdateDto dto)
    {
        var entity = await _productionDispatchRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("生产派工单不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_scheduling_dispatch_unique = await _uniqueValidator.IsUniqueAsync(
            _productionDispatchRepository,
            x => x.PlantCode == entity.PlantCode
                && x.DispatchCode == entity.DispatchCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_scheduling_dispatch_unique)
        {
            throw new TaktBusinessException("生产派工单的PlantCode、DispatchCode已存在");
        }
        await _productionDispatchRepository.UpdateAsync(entity);
        return await GetProductionDispatchByIdAsync(id) ?? throw new TaktBusinessException("生产派工单不存在");
    }

    /// <summary>
    /// 删除生产派工单
    /// </summary>
    /// <param name="id">生产派工单ID</param>
    /// <returns>任务</returns>
    public async Task DeleteProductionDispatchByIdAsync(long id)
    {
        var deleted = await _productionDispatchRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("生产派工单不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除生产派工单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteProductionDispatchBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteProductionDispatchByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新生产派工单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionDispatchDto> UpdateProductionDispatchStatusAsync(TaktProductionDispatchStatusDto dto)
    {
        var entity = await _productionDispatchRepository.GetByIdAsync(dto.ProductionDispatchId);
        if (entity == null)
        {
            throw new TaktBusinessException("生产派工单不存在");
        }
        entity.DispatchStatus = dto.DispatchStatus;
        await _productionDispatchRepository.UpdateAsync(entity);
        return await GetProductionDispatchByIdAsync(dto.ProductionDispatchId) ?? throw new TaktBusinessException("生产派工单不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetProductionDispatchTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktProductionDispatchTemplateDto>(
            sheetName ?? "生产派工单导入模板",
            fileName ?? "生产派工单导入模板.xlsx");
    }

    /// <summary>
    /// 导入生产派工单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportProductionDispatchAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktProductionDispatchImportDto>(fileStream, sheetName ?? "生产派工单导入模板");
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
                var entity = rows[i].Adapt<TaktProductionDispatch>();
                var importKey = $"{entity.PlantCode}|{entity.DispatchCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、DispatchCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_scheduling_dispatch_unique = await _uniqueValidator.IsUniqueAsync(
                    _productionDispatchRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.DispatchCode == entity.DispatchCode);
                if (!isUnique_ix_takt_logistics_manufacturing_scheduling_dispatch_unique)
                {
                    throw new TaktBusinessException("生产派工单的PlantCode、DispatchCode已存在");
                }
                await _productionDispatchRepository.CreateAsync(entity);
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
    /// 导出生产派工单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportProductionDispatchAsync(TaktProductionDispatchQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktProductionDispatchQueryDto());
        var list = await _productionDispatchRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktProductionDispatchExportDto>(),
                sheetName ?? "生产派工单数据",
                fileName ?? "生产派工单导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktProductionDispatchExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "生产派工单数据",
            fileName ?? "生产派工单导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建生产派工单查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktProductionDispatch, bool>> QueryExpression(TaktProductionDispatchQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktProductionDispatch>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.DispatchCode != null && x.DispatchCode.Contains(keywords))
                || SqlFunc.ToString(x.ProductionOrderId).Contains(keywords)
                || (x.ProdOrderCode != null && x.ProdOrderCode.Contains(keywords))
                || SqlFunc.ToString(x.ApsOperationId).Contains(keywords)
                || (x.WorkCenterCode != null && x.WorkCenterCode.Contains(keywords))
                || (x.ProcessCode != null && x.ProcessCode.Contains(keywords))
                || SqlFunc.ToString(x.DispatchQuantity).Contains(keywords)
                || SqlFunc.ToString(x.DispatchStatus).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.PlannedStartTime).Contains(keywords)
                || SqlFunc.ToString(x.PlannedEndTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.DispatchCode))
        {
            exp = exp.And(x => x.DispatchCode != null && x.DispatchCode.Contains(queryDto.DispatchCode));
        }

        if (queryDto?.ProductionOrderId.HasValue == true)
        {
            exp = exp.And(x => x.ProductionOrderId == queryDto.ProductionOrderId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ProdOrderCode))
        {
            exp = exp.And(x => x.ProdOrderCode != null && x.ProdOrderCode.Contains(queryDto.ProdOrderCode));
        }

        if (queryDto?.ApsOperationId.HasValue == true)
        {
            exp = exp.And(x => x.ApsOperationId == queryDto.ApsOperationId);
        }

        if (!string.IsNullOrEmpty(queryDto?.WorkCenterCode))
        {
            exp = exp.And(x => x.WorkCenterCode != null && x.WorkCenterCode.Contains(queryDto.WorkCenterCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProcessCode))
        {
            exp = exp.And(x => x.ProcessCode != null && x.ProcessCode.Contains(queryDto.ProcessCode));
        }

        if (queryDto?.DispatchQuantity.HasValue == true)
        {
            exp = exp.And(x => x.DispatchQuantity == queryDto.DispatchQuantity);
        }

        if (queryDto?.DispatchStatus.HasValue == true)
        {
            exp = exp.And(x => x.DispatchStatus == queryDto.DispatchStatus);
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

        if (queryDto?.PlannedStartTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.PlannedStartTime >= queryDto.PlannedStartTimeStart);
        }

        if (queryDto?.PlannedStartTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.PlannedStartTime <= queryDto.PlannedStartTimeEnd);
        }

        if (queryDto?.PlannedEndTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.PlannedEndTime >= queryDto.PlannedEndTimeStart);
        }

        if (queryDto?.PlannedEndTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.PlannedEndTime <= queryDto.PlannedEndTimeEnd);
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
