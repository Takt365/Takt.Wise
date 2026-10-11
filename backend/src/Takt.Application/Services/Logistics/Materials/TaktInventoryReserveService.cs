// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：TaktInventoryReserveService.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：存货跌价准备应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Materials;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Materials;

/// <summary>
/// 存货跌价准备应用服务
/// </summary>
public class TaktInventoryReserveService : TaktServiceBase, ITaktInventoryReserveService
{
    private readonly ITaktCompanyRepository<TaktInventoryReserve> _inventoryReserveRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="inventoryReserveRepository">存货跌价准备仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktInventoryReserveService(
        ITaktCompanyRepository<TaktInventoryReserve> inventoryReserveRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _inventoryReserveRepository = inventoryReserveRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取存货跌价准备列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktInventoryReserveDto>> GetInventoryReserveListAsync(TaktInventoryReserveQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _inventoryReserveRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktInventoryReserveDto>.Create(
            data.Adapt<List<TaktInventoryReserveDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取存货跌价准备
    /// </summary>
    /// <param name="id">存货跌价准备ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktInventoryReserveDto?> GetInventoryReserveByIdAsync(long id)
    {
        var entity = await _inventoryReserveRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktInventoryReserveDto>();
    }

    /// <summary>
    /// 获取存货跌价准备选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetInventoryReserveOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _inventoryReserveRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ProvisionStatus == 1,
            x => x.MaterialDescription ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.MaterialDescription ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建存货跌价准备
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktInventoryReserveDto> CreateInventoryReserveAsync(TaktInventoryReserveCreateDto dto)
    {
        var entity = dto.Adapt<TaktInventoryReserve>();
        var isUnique_ix_inventory_reserve_unique = await _uniqueValidator.IsUniqueAsync(
            _inventoryReserveRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PeriodDate == entity.PeriodDate
                && x.MaterialCode == entity.MaterialCode
                && x.Valuation == entity.Valuation);
        if (!isUnique_ix_inventory_reserve_unique)
        {
            throw new TaktBusinessException("存货跌价准备的PlantCode、PeriodDate、MaterialCode、Valuation已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _inventoryReserveRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        ApplyInventoryReserveCasMeasurement(entity);
        entity = await _inventoryReserveRepository.CreateAsync(entity);
        return await GetInventoryReserveByIdAsync(entity.Id) ?? entity.Adapt<TaktInventoryReserveDto>();
    }

    /// <summary>
    /// 更新存货跌价准备
    /// </summary>
    /// <param name="id">存货跌价准备ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktInventoryReserveDto> UpdateInventoryReserveAsync(long id, TaktInventoryReserveUpdateDto dto)
    {
        var entity = await _inventoryReserveRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("存货跌价准备不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_inventory_reserve_unique = await _uniqueValidator.IsUniqueAsync(
            _inventoryReserveRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PeriodDate == entity.PeriodDate
                && x.MaterialCode == entity.MaterialCode
                && x.Valuation == entity.Valuation,
            id);
        if (!isUnique_ix_inventory_reserve_unique)
        {
            throw new TaktBusinessException("存货跌价准备的PlantCode、PeriodDate、MaterialCode、Valuation已存在");
        }
        ApplyInventoryReserveCasMeasurement(entity);
        await _inventoryReserveRepository.UpdateAsync(entity);
        return await GetInventoryReserveByIdAsync(id) ?? throw new TaktBusinessException("存货跌价准备不存在");
    }

    /// <summary>
    /// 删除存货跌价准备
    /// </summary>
    /// <param name="id">存货跌价准备ID</param>
    /// <returns>任务</returns>
    public async Task DeleteInventoryReserveByIdAsync(long id)
    {
        var deleted = await _inventoryReserveRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("存货跌价准备不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除存货跌价准备
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteInventoryReserveBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteInventoryReserveByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新存货跌价准备状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktInventoryReserveDto> UpdateInventoryReserveStatusAsync(TaktInventoryReserveStatusDto dto)
    {
        var entity = await _inventoryReserveRepository.GetByIdAsync(dto.InventoryReserveId);
        if (entity == null)
        {
            throw new TaktBusinessException("存货跌价准备不存在");
        }
        entity.ProvisionStatus = dto.ProvisionStatus;
        await _inventoryReserveRepository.UpdateAsync(entity);
        return await GetInventoryReserveByIdAsync(dto.InventoryReserveId) ?? throw new TaktBusinessException("存货跌价准备不存在");
    }

    /// <summary>
    /// 更新存货跌价准备排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktInventoryReserveDto> UpdateInventoryReserveSortAsync(TaktInventoryReserveSortDto dto)
    {
        var entity = await _inventoryReserveRepository.GetByIdAsync(dto.InventoryReserveId);
        if (entity == null)
        {
            throw new TaktBusinessException("存货跌价准备不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _inventoryReserveRepository.UpdateAsync(entity);
        return await GetInventoryReserveByIdAsync(dto.InventoryReserveId) ?? throw new TaktBusinessException("存货跌价准备不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetInventoryReserveTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktInventoryReserveTemplateDto>(
            sheetName ?? "存货跌价准备导入模板",
            fileName ?? "存货跌价准备导入模板.xlsx");
    }

    /// <summary>
    /// 导入存货跌价准备
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportInventoryReserveAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktInventoryReserveImportDto>(fileStream, sheetName ?? "存货跌价准备导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _inventoryReserveRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktInventoryReserve>();
                var importKey = $"{entity.PlantCode}|{entity.PeriodDate}|{entity.MaterialCode}|{entity.Valuation}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、PeriodDate、MaterialCode、Valuation）");
                }
                var isUnique_ix_inventory_reserve_unique = await _uniqueValidator.IsUniqueAsync(
                    _inventoryReserveRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.PeriodDate == entity.PeriodDate
                        && x.MaterialCode == entity.MaterialCode
                        && x.Valuation == entity.Valuation);
                if (!isUnique_ix_inventory_reserve_unique)
                {
                    throw new TaktBusinessException("存货跌价准备的PlantCode、PeriodDate、MaterialCode、Valuation已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                ApplyInventoryReserveCasMeasurement(entity);
                await _inventoryReserveRepository.CreateAsync(entity);
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
    /// 导出存货跌价准备
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportInventoryReserveAsync(TaktInventoryReserveQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktInventoryReserveQueryDto());
        var list = await _inventoryReserveRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktInventoryReserveExportDto>(),
                sheetName ?? "存货跌价准备数据",
                fileName ?? "存货跌价准备导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktInventoryReserveExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "存货跌价准备数据",
            fileName ?? "存货跌价准备导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建存货跌价准备查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktInventoryReserve, bool>> QueryExpression(TaktInventoryReserveQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktInventoryReserve>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialDescription != null && x.MaterialDescription.Contains(keywords))
                || (x.Valuation != null && x.Valuation.Contains(keywords))
                || SqlFunc.ToString(x.ProvisionScope).Contains(keywords)
                || SqlFunc.ToString(x.StockQuantity).Contains(keywords)
                || SqlFunc.ToString(x.UnitCost).Contains(keywords)
                || SqlFunc.ToString(x.InventoryCost).Contains(keywords)
                || SqlFunc.ToString(x.EstimatedSellingPrice).Contains(keywords)
                || SqlFunc.ToString(x.EstimatedCompletionCost).Contains(keywords)
                || SqlFunc.ToString(x.EstimatedSellingCost).Contains(keywords)
                || SqlFunc.ToString(x.NetRealizableValue).Contains(keywords)
                || SqlFunc.ToString(x.UnitNetRealizableValue).Contains(keywords)
                || SqlFunc.ToString(x.OpeningProvision).Contains(keywords)
                || SqlFunc.ToString(x.ProvisionAmount).Contains(keywords)
                || SqlFunc.ToString(x.ReversalAmount).Contains(keywords)
                || SqlFunc.ToString(x.ClosingProvision).Contains(keywords)
                || SqlFunc.ToString(x.ImpairmentLoss).Contains(keywords)
                || SqlFunc.ToString(x.CarryingAmount).Contains(keywords)
                || (x.CurrencyCode != null && x.CurrencyCode.Contains(keywords))
                || (x.ImpairmentReason != null && x.ImpairmentReason.Contains(keywords))
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || SqlFunc.ToString(x.ProvisionStatus).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.PeriodDate).Contains(keywords)
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

        if (!string.IsNullOrEmpty(queryDto?.MaterialDescription))
        {
            exp = exp.And(x => x.MaterialDescription != null && x.MaterialDescription.Contains(queryDto.MaterialDescription));
        }

        if (!string.IsNullOrEmpty(queryDto?.Valuation))
        {
            exp = exp.And(x => x.Valuation != null && x.Valuation.Contains(queryDto.Valuation));
        }

        if (queryDto?.ProvisionScope.HasValue == true)
        {
            exp = exp.And(x => x.ProvisionScope == queryDto.ProvisionScope);
        }

        if (queryDto?.StockQuantity.HasValue == true)
        {
            exp = exp.And(x => x.StockQuantity == queryDto.StockQuantity);
        }

        if (queryDto?.UnitCost.HasValue == true)
        {
            exp = exp.And(x => x.UnitCost == queryDto.UnitCost);
        }

        if (queryDto?.InventoryCost.HasValue == true)
        {
            exp = exp.And(x => x.InventoryCost == queryDto.InventoryCost);
        }

        if (queryDto?.EstimatedSellingPrice.HasValue == true)
        {
            exp = exp.And(x => x.EstimatedSellingPrice == queryDto.EstimatedSellingPrice);
        }

        if (queryDto?.EstimatedCompletionCost.HasValue == true)
        {
            exp = exp.And(x => x.EstimatedCompletionCost == queryDto.EstimatedCompletionCost);
        }

        if (queryDto?.EstimatedSellingCost.HasValue == true)
        {
            exp = exp.And(x => x.EstimatedSellingCost == queryDto.EstimatedSellingCost);
        }

        if (queryDto?.NetRealizableValue.HasValue == true)
        {
            exp = exp.And(x => x.NetRealizableValue == queryDto.NetRealizableValue);
        }

        if (queryDto?.UnitNetRealizableValue.HasValue == true)
        {
            exp = exp.And(x => x.UnitNetRealizableValue == queryDto.UnitNetRealizableValue);
        }

        if (queryDto?.OpeningProvision.HasValue == true)
        {
            exp = exp.And(x => x.OpeningProvision == queryDto.OpeningProvision);
        }

        if (queryDto?.ProvisionAmount.HasValue == true)
        {
            exp = exp.And(x => x.ProvisionAmount == queryDto.ProvisionAmount);
        }

        if (queryDto?.ReversalAmount.HasValue == true)
        {
            exp = exp.And(x => x.ReversalAmount == queryDto.ReversalAmount);
        }

        if (queryDto?.ClosingProvision.HasValue == true)
        {
            exp = exp.And(x => x.ClosingProvision == queryDto.ClosingProvision);
        }

        if (queryDto?.ImpairmentLoss.HasValue == true)
        {
            exp = exp.And(x => x.ImpairmentLoss == queryDto.ImpairmentLoss);
        }

        if (queryDto?.CarryingAmount.HasValue == true)
        {
            exp = exp.And(x => x.CarryingAmount == queryDto.CarryingAmount);
        }

        if (!string.IsNullOrEmpty(queryDto?.CurrencyCode))
        {
            exp = exp.And(x => x.CurrencyCode != null && x.CurrencyCode.Contains(queryDto.CurrencyCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ImpairmentReason))
        {
            exp = exp.And(x => x.ImpairmentReason != null && x.ImpairmentReason.Contains(queryDto.ImpairmentReason));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (queryDto?.ProvisionStatus.HasValue == true)
        {
            exp = exp.And(x => x.ProvisionStatus == queryDto.ProvisionStatus);
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

        if (queryDto?.PeriodDateStart.HasValue == true)
        {
            exp = exp.And(x => x.PeriodDate >= queryDto.PeriodDateStart);
        }

        if (queryDto?.PeriodDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.PeriodDate <= queryDto.PeriodDateEnd);
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

    /// <summary>
    /// 按中国《企业会计准则第1号——存货》与 IAS 2 勾稽计量：
    /// 可变现净值 = 估计售价 − 至完工成本 − 销售费用；期末跌价准备 = max(0, 成本 − NRV)；
    /// 未填计提/转回时自动按期初差额分配；账面价值 = 成本 − 期末跌价准备；允许转回（CAS/IFRS）。
    /// </summary>
    /// <param name="entity">存货跌价准备实体</param>
    private void ApplyInventoryReserveCasMeasurement(TaktInventoryReserve entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (entity.InventoryCost == 0 && entity.StockQuantity != 0 && entity.UnitCost != 0)
        {
            entity.InventoryCost = Math.Round(entity.StockQuantity * entity.UnitCost, 2, MidpointRounding.AwayFromZero);
        }

        var hasNrvComponents = entity.EstimatedSellingPrice != 0
            || entity.EstimatedCompletionCost != 0
            || entity.EstimatedSellingCost != 0;
        if (hasNrvComponents)
        {
            var computedNrv = entity.EstimatedSellingPrice - entity.EstimatedCompletionCost - entity.EstimatedSellingCost;
            entity.NetRealizableValue = computedNrv < 0
                ? 0
                : Math.Round(computedNrv, 2, MidpointRounding.AwayFromZero);
        }

        if (entity.UnitNetRealizableValue == 0 && entity.StockQuantity > 0 && entity.NetRealizableValue != 0)
        {
            entity.UnitNetRealizableValue = Math.Round(
                entity.NetRealizableValue / entity.StockQuantity,
                5,
                MidpointRounding.AwayFromZero);
        }

        var requiredClosing = entity.InventoryCost > entity.NetRealizableValue
            ? Math.Round(entity.InventoryCost - entity.NetRealizableValue, 2, MidpointRounding.AwayFromZero)
            : 0m;

        if (entity.ProvisionAmount == 0 && entity.ReversalAmount == 0)
        {
            var delta = requiredClosing - entity.OpeningProvision;
            if (delta > 0)
            {
                entity.ProvisionAmount = delta;
            }
            else if (delta < 0)
            {
                entity.ReversalAmount = -delta;
            }
        }

        entity.ClosingProvision = Math.Round(
            entity.OpeningProvision + entity.ProvisionAmount - entity.ReversalAmount,
            2,
            MidpointRounding.AwayFromZero);

        if (entity.ClosingProvision < 0)
        {
            throw new TaktBusinessException("期末跌价准备不能为负（转回不得超过期初余额与本期计提之和）");
        }

        if (Math.Abs(entity.ClosingProvision - requiredClosing) > 0.01m)
        {
            throw new TaktBusinessException(
                $"期末跌价准备须等于「成本与可变现净值孰低」差额（应提 {requiredClosing}，实际 {entity.ClosingProvision}）");
        }

        entity.ImpairmentLoss = Math.Round(
            entity.ProvisionAmount - entity.ReversalAmount,
            2,
            MidpointRounding.AwayFromZero);
        entity.CarryingAmount = Math.Round(
            entity.InventoryCost - entity.ClosingProvision,
            2,
            MidpointRounding.AwayFromZero);
        if (entity.CarryingAmount < 0)
        {
            entity.CarryingAmount = 0;
        }
    }
}
