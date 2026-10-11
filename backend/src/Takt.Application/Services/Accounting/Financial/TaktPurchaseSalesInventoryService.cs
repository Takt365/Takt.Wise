// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Accounting.Financial
// 文件名称：TaktPurchaseSalesInventoryService.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：进销存应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Accounting.Financial;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Accounting.Financial;

/// <summary>
/// 进销存应用服务
/// </summary>
public class TaktPurchaseSalesInventoryService : TaktServiceBase, ITaktPurchaseSalesInventoryService
{
    private readonly ITaktCompanyRepository<TaktPurchaseSalesInventory> _purchaseSalesInventoryRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseSalesInventoryRepository">进销存仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPurchaseSalesInventoryService(
        ITaktCompanyRepository<TaktPurchaseSalesInventory> purchaseSalesInventoryRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchaseSalesInventoryRepository = purchaseSalesInventoryRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取进销存列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPurchaseSalesInventoryDto>> GetPurchaseSalesInventoryListAsync(TaktPurchaseSalesInventoryQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _purchaseSalesInventoryRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPurchaseSalesInventoryDto>.Create(
            data.Adapt<List<TaktPurchaseSalesInventoryDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取进销存
    /// </summary>
    /// <param name="id">进销存ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseSalesInventoryDto?> GetPurchaseSalesInventoryByIdAsync(long id)
    {
        var entity = await _purchaseSalesInventoryRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktPurchaseSalesInventoryDto>();
    }

    /// <summary>
    /// 获取进销存选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPurchaseSalesInventoryOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _purchaseSalesInventoryRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PsiStatus == 1,
            x => x.MaterialDescription ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.MaterialDescription ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建进销存
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseSalesInventoryDto> CreatePurchaseSalesInventoryAsync(TaktPurchaseSalesInventoryCreateDto dto)
    {
        var entity = dto.Adapt<TaktPurchaseSalesInventory>();
        var isUnique_ix_takt_accounting_financial_psi_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseSalesInventoryRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PeriodCode == entity.PeriodCode
                && x.MaterialCode == entity.MaterialCode
                && x.Valuation == entity.Valuation);
        if (!isUnique_ix_takt_accounting_financial_psi_unique)
        {
            throw new TaktBusinessException("进销存的PlantCode、PeriodCode、MaterialCode、Valuation已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _purchaseSalesInventoryRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        ApplyPurchaseSalesInventoryCasMeasurement(entity);
        entity = await _purchaseSalesInventoryRepository.CreateAsync(entity);
        return await GetPurchaseSalesInventoryByIdAsync(entity.Id) ?? entity.Adapt<TaktPurchaseSalesInventoryDto>();
    }

    /// <summary>
    /// 更新进销存
    /// </summary>
    /// <param name="id">进销存ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseSalesInventoryDto> UpdatePurchaseSalesInventoryAsync(long id, TaktPurchaseSalesInventoryUpdateDto dto)
    {
        var entity = await _purchaseSalesInventoryRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("进销存不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_accounting_financial_psi_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseSalesInventoryRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PeriodCode == entity.PeriodCode
                && x.MaterialCode == entity.MaterialCode
                && x.Valuation == entity.Valuation,
            id);
        if (!isUnique_ix_takt_accounting_financial_psi_unique)
        {
            throw new TaktBusinessException("进销存的PlantCode、PeriodCode、MaterialCode、Valuation已存在");
        }
        ApplyPurchaseSalesInventoryCasMeasurement(entity);
        await _purchaseSalesInventoryRepository.UpdateAsync(entity);
        return await GetPurchaseSalesInventoryByIdAsync(id) ?? throw new TaktBusinessException("进销存不存在");
    }

    /// <summary>
    /// 删除进销存
    /// </summary>
    /// <param name="id">进销存ID</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseSalesInventoryByIdAsync(long id)
    {
        var deleted = await _purchaseSalesInventoryRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("进销存不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除进销存
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseSalesInventoryBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePurchaseSalesInventoryByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新进销存状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseSalesInventoryDto> UpdatePurchaseSalesInventoryStatusAsync(TaktPurchaseSalesInventoryStatusDto dto)
    {
        var entity = await _purchaseSalesInventoryRepository.GetByIdAsync(dto.PurchaseSalesInventoryId);
        if (entity == null)
        {
            throw new TaktBusinessException("进销存不存在");
        }
        entity.PsiStatus = dto.PsiStatus;
        await _purchaseSalesInventoryRepository.UpdateAsync(entity);
        return await GetPurchaseSalesInventoryByIdAsync(dto.PurchaseSalesInventoryId) ?? throw new TaktBusinessException("进销存不存在");
    }

    /// <summary>
    /// 更新进销存排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseSalesInventoryDto> UpdatePurchaseSalesInventorySortAsync(TaktPurchaseSalesInventorySortDto dto)
    {
        var entity = await _purchaseSalesInventoryRepository.GetByIdAsync(dto.PurchaseSalesInventoryId);
        if (entity == null)
        {
            throw new TaktBusinessException("进销存不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _purchaseSalesInventoryRepository.UpdateAsync(entity);
        return await GetPurchaseSalesInventoryByIdAsync(dto.PurchaseSalesInventoryId) ?? throw new TaktBusinessException("进销存不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPurchaseSalesInventoryTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPurchaseSalesInventoryTemplateDto>(
            sheetName ?? "进销存导入模板",
            fileName ?? "进销存导入模板.xlsx");
    }

    /// <summary>
    /// 导入进销存
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPurchaseSalesInventoryAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPurchaseSalesInventoryImportDto>(fileStream, sheetName ?? "进销存导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _purchaseSalesInventoryRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktPurchaseSalesInventory>();
                var importKey = $"{entity.PlantCode}|{entity.PeriodCode}|{entity.MaterialCode}|{entity.Valuation}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、PeriodCode、MaterialCode、Valuation）");
                }
                var isUnique_ix_takt_accounting_financial_psi_unique = await _uniqueValidator.IsUniqueAsync(
                    _purchaseSalesInventoryRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.PeriodCode == entity.PeriodCode
                        && x.MaterialCode == entity.MaterialCode
                        && x.Valuation == entity.Valuation);
                if (!isUnique_ix_takt_accounting_financial_psi_unique)
                {
                    throw new TaktBusinessException("进销存的PlantCode、PeriodCode、MaterialCode、Valuation已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                ApplyPurchaseSalesInventoryCasMeasurement(entity);
                await _purchaseSalesInventoryRepository.CreateAsync(entity);
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
    /// 导出进销存
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPurchaseSalesInventoryAsync(TaktPurchaseSalesInventoryQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktPurchaseSalesInventoryQueryDto());
        var list = await _purchaseSalesInventoryRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseSalesInventoryExportDto>(),
                sheetName ?? "进销存数据",
                fileName ?? "进销存导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPurchaseSalesInventoryExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "进销存数据",
            fileName ?? "进销存导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建进销存查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPurchaseSalesInventory, bool>> QueryExpression(TaktPurchaseSalesInventoryQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPurchaseSalesInventory>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.PeriodCode != null && x.PeriodCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialDescription != null && x.MaterialDescription.Contains(keywords))
                || (x.Valuation != null && x.Valuation.Contains(keywords))
                || (x.UnitCode != null && x.UnitCode.Contains(keywords))
                || SqlFunc.ToString(x.OpeningQty).Contains(keywords)
                || SqlFunc.ToString(x.OpeningAmount).Contains(keywords)
                || SqlFunc.ToString(x.PurchaseQty).Contains(keywords)
                || SqlFunc.ToString(x.PurchaseAmount).Contains(keywords)
                || SqlFunc.ToString(x.ProductionQty).Contains(keywords)
                || SqlFunc.ToString(x.ProductionAmount).Contains(keywords)
                || SqlFunc.ToString(x.IssueQty).Contains(keywords)
                || SqlFunc.ToString(x.IssueCostAmount).Contains(keywords)
                || SqlFunc.ToString(x.SalesRevenueAmount).Contains(keywords)
                || SqlFunc.ToString(x.AdjustQty).Contains(keywords)
                || SqlFunc.ToString(x.AdjustAmount).Contains(keywords)
                || SqlFunc.ToString(x.ClosingQty).Contains(keywords)
                || SqlFunc.ToString(x.ClosingAmount).Contains(keywords)
                || SqlFunc.ToString(x.ClosingUnitCost).Contains(keywords)
                || (x.CurrencyCode != null && x.CurrencyCode.Contains(keywords))
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || SqlFunc.ToString(x.PsiStatus).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.PeriodCode))
        {
            exp = exp.And(x => x.PeriodCode != null && x.PeriodCode.Contains(queryDto.PeriodCode));
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

        if (!string.IsNullOrEmpty(queryDto?.UnitCode))
        {
            exp = exp.And(x => x.UnitCode != null && x.UnitCode.Contains(queryDto.UnitCode));
        }

        if (queryDto?.OpeningQty.HasValue == true)
        {
            exp = exp.And(x => x.OpeningQty == queryDto.OpeningQty);
        }

        if (queryDto?.OpeningAmount.HasValue == true)
        {
            exp = exp.And(x => x.OpeningAmount == queryDto.OpeningAmount);
        }

        if (queryDto?.PurchaseQty.HasValue == true)
        {
            exp = exp.And(x => x.PurchaseQty == queryDto.PurchaseQty);
        }

        if (queryDto?.PurchaseAmount.HasValue == true)
        {
            exp = exp.And(x => x.PurchaseAmount == queryDto.PurchaseAmount);
        }

        if (queryDto?.ProductionQty.HasValue == true)
        {
            exp = exp.And(x => x.ProductionQty == queryDto.ProductionQty);
        }

        if (queryDto?.ProductionAmount.HasValue == true)
        {
            exp = exp.And(x => x.ProductionAmount == queryDto.ProductionAmount);
        }

        if (queryDto?.IssueQty.HasValue == true)
        {
            exp = exp.And(x => x.IssueQty == queryDto.IssueQty);
        }

        if (queryDto?.IssueCostAmount.HasValue == true)
        {
            exp = exp.And(x => x.IssueCostAmount == queryDto.IssueCostAmount);
        }

        if (queryDto?.SalesRevenueAmount.HasValue == true)
        {
            exp = exp.And(x => x.SalesRevenueAmount == queryDto.SalesRevenueAmount);
        }

        if (queryDto?.AdjustQty.HasValue == true)
        {
            exp = exp.And(x => x.AdjustQty == queryDto.AdjustQty);
        }

        if (queryDto?.AdjustAmount.HasValue == true)
        {
            exp = exp.And(x => x.AdjustAmount == queryDto.AdjustAmount);
        }

        if (queryDto?.ClosingQty.HasValue == true)
        {
            exp = exp.And(x => x.ClosingQty == queryDto.ClosingQty);
        }

        if (queryDto?.ClosingAmount.HasValue == true)
        {
            exp = exp.And(x => x.ClosingAmount == queryDto.ClosingAmount);
        }

        if (queryDto?.ClosingUnitCost.HasValue == true)
        {
            exp = exp.And(x => x.ClosingUnitCost == queryDto.ClosingUnitCost);
        }

        if (!string.IsNullOrEmpty(queryDto?.CurrencyCode))
        {
            exp = exp.And(x => x.CurrencyCode != null && x.CurrencyCode.Contains(queryDto.CurrencyCode));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (queryDto?.PsiStatus.HasValue == true)
        {
            exp = exp.And(x => x.PsiStatus == queryDto.PsiStatus);
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
    /// 按 CAS《存货》/ IAS 2 成本流转勾稽：
    /// 期末数量 = 期初 + 采购 + 生产 + 调整 − 出库；
    /// 期末成本 = 期初 + 采购 + 生产 + 调整 − 出库结转成本；销售收入不参与等式。
    /// </summary>
    /// <param name="entity">进销存行</param>
    private static void ApplyPurchaseSalesInventoryCasMeasurement(TaktPurchaseSalesInventory entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        var computedQty = Math.Round(
            entity.OpeningQty + entity.PurchaseQty + entity.ProductionQty + entity.AdjustQty - entity.IssueQty,
            4,
            MidpointRounding.AwayFromZero);
        var computedAmount = Math.Round(
            entity.OpeningAmount + entity.PurchaseAmount + entity.ProductionAmount + entity.AdjustAmount - entity.IssueCostAmount,
            2,
            MidpointRounding.AwayFromZero);
        entity.ClosingQty = computedQty;
        entity.ClosingAmount = computedAmount;
        entity.ClosingUnitCost = computedQty != 0
            ? Math.Round(computedAmount / computedQty, 5, MidpointRounding.AwayFromZero)
            : 0;
    }
}
