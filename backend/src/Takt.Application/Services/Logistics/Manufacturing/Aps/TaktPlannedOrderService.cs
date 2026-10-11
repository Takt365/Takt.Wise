// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Aps
// 文件名称：TaktPlannedOrderService.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：计划订单应用服务实现
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
/// 计划订单应用服务
/// </summary>
public class TaktPlannedOrderService : TaktServiceBase, ITaktPlannedOrderService
{
    private readonly ITaktCompanyRepository<TaktPlannedOrder> _plannedOrderRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="plannedOrderRepository">计划订单仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPlannedOrderService(
        ITaktCompanyRepository<TaktPlannedOrder> plannedOrderRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _plannedOrderRepository = plannedOrderRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取计划订单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPlannedOrderDto>> GetPlannedOrderListAsync(TaktPlannedOrderQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _plannedOrderRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPlannedOrderDto>.Create(
            data.Adapt<List<TaktPlannedOrderDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取计划订单
    /// </summary>
    /// <param name="id">计划订单ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPlannedOrderDto?> GetPlannedOrderByIdAsync(long id)
    {
        var entity = await _plannedOrderRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktPlannedOrderDto>();
    }

    /// <summary>
    /// 获取计划订单选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPlannedOrderOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _plannedOrderRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.OrderStatus == 1,
            x => x.PlantCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PlannedOrderCode,
            ExtValue = e.PlantCode,
        }).ToList();
    }

    /// <summary>
    /// 创建计划订单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPlannedOrderDto> CreatePlannedOrderAsync(TaktPlannedOrderCreateDto dto)
    {
        var entity = dto.Adapt<TaktPlannedOrder>();
        var isUnique_ix_takt_logistics_manufacturing_planning_planned_order_unique = await _uniqueValidator.IsUniqueAsync(
            _plannedOrderRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PlannedOrderCode == entity.PlannedOrderCode);
        if (!isUnique_ix_takt_logistics_manufacturing_planning_planned_order_unique)
        {
            throw new TaktBusinessException("计划订单的PlantCode、PlannedOrderCode已存在");
        }
        entity = await _plannedOrderRepository.CreateAsync(entity);
        return await GetPlannedOrderByIdAsync(entity.Id) ?? entity.Adapt<TaktPlannedOrderDto>();
    }

    /// <summary>
    /// 更新计划订单
    /// </summary>
    /// <param name="id">计划订单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPlannedOrderDto> UpdatePlannedOrderAsync(long id, TaktPlannedOrderUpdateDto dto)
    {
        var entity = await _plannedOrderRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("计划订单不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_planning_planned_order_unique = await _uniqueValidator.IsUniqueAsync(
            _plannedOrderRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PlannedOrderCode == entity.PlannedOrderCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_planning_planned_order_unique)
        {
            throw new TaktBusinessException("计划订单的PlantCode、PlannedOrderCode已存在");
        }
        await _plannedOrderRepository.UpdateAsync(entity);
        return await GetPlannedOrderByIdAsync(id) ?? throw new TaktBusinessException("计划订单不存在");
    }

    /// <summary>
    /// 删除计划订单
    /// </summary>
    /// <param name="id">计划订单ID</param>
    /// <returns>任务</returns>
    public async Task DeletePlannedOrderByIdAsync(long id)
    {
        var deleted = await _plannedOrderRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("计划订单不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除计划订单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePlannedOrderBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePlannedOrderByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新计划订单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPlannedOrderDto> UpdatePlannedOrderStatusAsync(TaktPlannedOrderStatusDto dto)
    {
        var entity = await _plannedOrderRepository.GetByIdAsync(dto.PlannedOrderId);
        if (entity == null)
        {
            throw new TaktBusinessException("计划订单不存在");
        }
        entity.OrderStatus = dto.OrderStatus;
        await _plannedOrderRepository.UpdateAsync(entity);
        return await GetPlannedOrderByIdAsync(dto.PlannedOrderId) ?? throw new TaktBusinessException("计划订单不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPlannedOrderTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPlannedOrderTemplateDto>(
            sheetName ?? "计划订单导入模板",
            fileName ?? "计划订单导入模板.xlsx");
    }

    /// <summary>
    /// 导入计划订单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPlannedOrderAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPlannedOrderImportDto>(fileStream, sheetName ?? "计划订单导入模板");
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
                var entity = rows[i].Adapt<TaktPlannedOrder>();
                var importKey = $"{entity.PlantCode}|{entity.PlannedOrderCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、PlannedOrderCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_planning_planned_order_unique = await _uniqueValidator.IsUniqueAsync(
                    _plannedOrderRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.PlannedOrderCode == entity.PlannedOrderCode);
                if (!isUnique_ix_takt_logistics_manufacturing_planning_planned_order_unique)
                {
                    throw new TaktBusinessException("计划订单的PlantCode、PlannedOrderCode已存在");
                }
                await _plannedOrderRepository.CreateAsync(entity);
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
    /// 导出计划订单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPlannedOrderAsync(TaktPlannedOrderQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktPlannedOrderQueryDto());
        var list = await _plannedOrderRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPlannedOrderExportDto>(),
                sheetName ?? "计划订单数据",
                fileName ?? "计划订单导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPlannedOrderExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "计划订单数据",
            fileName ?? "计划订单导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建计划订单查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPlannedOrder, bool>> QueryExpression(TaktPlannedOrderQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPlannedOrder>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.PlannedOrderCode != null && x.PlannedOrderCode.Contains(keywords))
                || SqlFunc.ToString(x.MaterialRequirementsPlanningId).Contains(keywords)
                || (x.MaterialRequirementsPlanningCode != null && x.MaterialRequirementsPlanningCode.Contains(keywords))
                || SqlFunc.ToString(x.MaterialRequirementsPlanningItemId).Contains(keywords)
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || SqlFunc.ToString(x.PlannedQuantity).Contains(keywords)
                || (x.UnitOfMeasure != null && x.UnitOfMeasure.Contains(keywords))
                || (x.RoutingCode != null && x.RoutingCode.Contains(keywords))
                || SqlFunc.ToString(x.OrderStatus).Contains(keywords)
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

        if (!string.IsNullOrEmpty(queryDto?.PlannedOrderCode))
        {
            exp = exp.And(x => x.PlannedOrderCode != null && x.PlannedOrderCode.Contains(queryDto.PlannedOrderCode));
        }

        if (queryDto?.MaterialRequirementsPlanningId.HasValue == true)
        {
            exp = exp.And(x => x.MaterialRequirementsPlanningId == queryDto.MaterialRequirementsPlanningId);
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialRequirementsPlanningCode))
        {
            exp = exp.And(x => x.MaterialRequirementsPlanningCode != null && x.MaterialRequirementsPlanningCode.Contains(queryDto.MaterialRequirementsPlanningCode));
        }

        if (queryDto?.MaterialRequirementsPlanningItemId.HasValue == true)
        {
            exp = exp.And(x => x.MaterialRequirementsPlanningItemId == queryDto.MaterialRequirementsPlanningItemId);
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialCode))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(queryDto.MaterialCode));
        }

        if (queryDto?.PlannedQuantity.HasValue == true)
        {
            exp = exp.And(x => x.PlannedQuantity == queryDto.PlannedQuantity);
        }

        if (!string.IsNullOrEmpty(queryDto?.UnitOfMeasure))
        {
            exp = exp.And(x => x.UnitOfMeasure != null && x.UnitOfMeasure.Contains(queryDto.UnitOfMeasure));
        }

        if (!string.IsNullOrEmpty(queryDto?.RoutingCode))
        {
            exp = exp.And(x => x.RoutingCode != null && x.RoutingCode.Contains(queryDto.RoutingCode));
        }

        if (queryDto?.OrderStatus.HasValue == true)
        {
            exp = exp.And(x => x.OrderStatus == queryDto.OrderStatus);
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
