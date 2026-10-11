// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialCostItemService.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM物料成本明细应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq;
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
/// BOM物料成本明细应用服务
/// </summary>
public class TaktBomMaterialCostItemService : TaktServiceBase, ITaktBomMaterialCostItemService
{
    private readonly ITaktCompanyRepository<TaktBomMaterialCostItem> _bomMaterialCostItemRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;
    private const string BomItemYearShardBaseTable = "takt_logistics_manufacturing_bom_material_cost_item";

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bomMaterialCostItemRepository">BOM物料成本明细仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktBomMaterialCostItemService(
        ITaktCompanyRepository<TaktBomMaterialCostItem> bomMaterialCostItemRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _bomMaterialCostItemRepository = bomMaterialCostItemRepository;
        _uniqueValidator = uniqueValidator;
    }

    // ========================================
    // 按年分表路由（{base}_{yyyy}）
    // ========================================

    /// <summary>
    /// 生成 BOM 成本明细年分表名
    /// </summary>
    /// <param name="year">年份</param>
    /// <returns>物理表名</returns>
    private static string BuildBomItemYearTable(int year) =>
        TaktYearShardTableHelper.BuildYearTableName(BomItemYearShardBaseTable, year);

    /// <summary>
    /// 解析 BOM 成本明细物理表：年分表存在则用之，否则 null（回退实体基表）
    /// </summary>
    /// <param name="year">自然年</param>
    /// <returns>年分表名；不存在时为 null</returns>
    private async Task<string?> ResolveBomItemPhysicalTableAsync(int year)
    {
        var table = BuildBomItemYearTable(year);
        return await _bomMaterialCostItemRepository.PhysicalTableExistsAsync(table) ? table : null;
    }

    /// <summary>
    /// 获取BOM物料成本明细列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktBomMaterialCostItemDto>> GetBomMaterialCostItemListAsync(TaktBomMaterialCostItemQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktBomMaterialCostItemDto>.Create(
                new List<TaktBomMaterialCostItemDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _bomMaterialCostItemRepository.GetPagedAsync(
            predicate,
            queryDto.PageIndex,
            queryDto.PageSize,
            x => x.ProductCode,
            isDesc: false,
            thenBy: x => x.LineNumber,
            thenByDesc: false);
        return TaktPagedResult<TaktBomMaterialCostItemDto>.Create(
            data.Adapt<List<TaktBomMaterialCostItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取BOM物料成本明细
    /// </summary>
    /// <param name="id">BOM物料成本明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktBomMaterialCostItemDto?> GetBomMaterialCostItemByIdAsync(long id)
    {
        var entity = await _bomMaterialCostItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktBomMaterialCostItemDto>();
    }

    /// <summary>
    /// 获取BOM物料成本选项列表（产品编码去重，DictValue=ProductCode）
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于缩小候选产品）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetBomMaterialCostItemOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var yearTable = await ResolveBomItemPhysicalTableAsync(DateTime.Now.Year);
        var list = await _bomMaterialCostItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && (string.IsNullOrWhiteSpace(plantCode) || (x.PlantCode != null && x.PlantCode == plantCode)),
            x => x.ProductCode ?? string.Empty,
            false,
            yearTable);
        return list
            .Where(e => !string.IsNullOrWhiteSpace(e.ProductCode))
            .GroupBy(e => e.ProductCode!, StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g =>
            {
                var first = g.First();
                var description = first.ProductDescription?.Trim();
                var label = string.IsNullOrWhiteSpace(description) ? g.Key : $"{g.Key} - {description}";
                return new TaktSelectOption
                {
                    DictValue = g.Key,
                    DictLabel = label,
                };
            })
            .ToList();
    }

    /// <summary>
    /// 创建BOM物料成本明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBomMaterialCostItemDto> CreateBomMaterialCostItemAsync(TaktBomMaterialCostItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktBomMaterialCostItem>();
        entity = await _bomMaterialCostItemRepository.CreateAsync(entity);
        return await GetBomMaterialCostItemByIdAsync(entity.Id) ?? entity.Adapt<TaktBomMaterialCostItemDto>();
    }

    /// <summary>
    /// 更新BOM物料成本明细
    /// </summary>
    /// <param name="id">BOM物料成本明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBomMaterialCostItemDto> UpdateBomMaterialCostItemAsync(long id, TaktBomMaterialCostItemUpdateDto dto)
    {
        var entity = await _bomMaterialCostItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("BOM物料成本明细不存在");
        }
        dto.Adapt(entity);
        await _bomMaterialCostItemRepository.UpdateAsync(entity);
        return await GetBomMaterialCostItemByIdAsync(id) ?? throw new TaktBusinessException("BOM物料成本明细不存在");
    }

    /// <summary>
    /// 删除BOM物料成本明细
    /// </summary>
    /// <param name="id">BOM物料成本明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteBomMaterialCostItemByIdAsync(long id)
    {
        var deleted = await _bomMaterialCostItemRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("BOM物料成本明细不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除BOM物料成本明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteBomMaterialCostItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteBomMaterialCostItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetBomMaterialCostItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktBomMaterialCostItemTemplateDto>(
            sheetName ?? "BOM物料成本明细导入模板",
            fileName ?? "BOM物料成本明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入BOM物料成本明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportBomMaterialCostItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktBomMaterialCostItemImportDto>(fileStream, sheetName ?? "BOM物料成本明细导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktBomMaterialCostItem>();
                await _bomMaterialCostItemRepository.CreateAsync(entity);
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
    /// 导出BOM物料成本明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostItemAsync(TaktBomMaterialCostItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktBomMaterialCostItemQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktBomMaterialCostItemExportDto>(),
                sheetName ?? "BOM物料成本明细数据",
                fileName ?? "BOM物料成本明细导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _bomMaterialCostItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktBomMaterialCostItemExportDto>(),
                sheetName ?? "BOM物料成本明细数据",
                fileName ?? "BOM物料成本明细导出.xlsx");
        }
        var exportData = list
            .OrderBy(x => x.ProductCode, StringComparer.OrdinalIgnoreCase)
            .ThenBy(x => x.LineNumber)
            .Adapt<List<TaktBomMaterialCostItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "BOM物料成本明细数据",
            fileName ?? "BOM物料成本明细导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建BOM物料成本明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktBomMaterialCostItem, bool>> QueryExpression(TaktBomMaterialCostItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktBomMaterialCostItem>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.BomLevel != null && x.BomLevel.Contains(keywords))
                || (x.BomItemCode != null && x.BomItemCode.Contains(keywords))
                || (x.ProductCode != null && x.ProductCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.ProductDescription != null && x.ProductDescription.Contains(keywords))
                || (x.ComponentCode != null && x.ComponentCode.Contains(keywords))
                || (x.ComponentDescription != null && x.ComponentDescription.Contains(keywords))
                || (x.BatchIndicator != null && x.BatchIndicator.Contains(keywords))
                || (x.ProductionRelated != null && x.ProductionRelated.Contains(keywords))
                || (x.PcbSectIndicator != null && x.PcbSectIndicator.Contains(keywords))
                || (x.PurchaseType != null && x.PurchaseType.Contains(keywords))
                || (x.SpecialProcurementType != null && x.SpecialProcurementType.Contains(keywords))
                || (x.ProfitCenterCode != null && x.ProfitCenterCode.Contains(keywords))
                || (x.MovingPriceCurrencyCode != null && x.MovingPriceCurrencyCode.Contains(keywords))
                || (x.PurchaseOrganization != null && x.PurchaseOrganization.Contains(keywords))
                || (x.PurchaseGroup != null && x.PurchaseGroup.Contains(keywords))
                || (x.SupplierCode != null && x.SupplierCode.Contains(keywords))
                || (x.PurchaseCurrencyCode != null && x.PurchaseCurrencyCode.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.BomLevel))
        {
            var bomLevel = queryDto.BomLevel;
            exp = exp.And(x => x.BomLevel != null && x.BomLevel.Contains(bomLevel));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BomItemCode))
        {
            var bomItemCode = queryDto.BomItemCode;
            exp = exp.And(x => x.BomItemCode != null && x.BomItemCode.Contains(bomItemCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProductCode))
        {
            var productCode = queryDto.ProductCode;
            exp = exp.And(x => x.ProductCode != null && x.ProductCode.Contains(productCode));
        }

        if (queryDto?.ProductCodes != null && queryDto.ProductCodes.Count > 0)
        {
            var productCodes = queryDto.ProductCodes;
            exp = exp.And(x => productCodes.Contains(x.ProductCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            var lineNumber = queryDto.LineNumber.Value;
            exp = exp.And(x => x.LineNumber == lineNumber);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProductDescription))
        {
            var productDescription = queryDto.ProductDescription;
            exp = exp.And(x => x.ProductDescription != null && x.ProductDescription.Contains(productDescription));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ComponentCode))
        {
            var componentCode = queryDto.ComponentCode;
            exp = exp.And(x => x.ComponentCode != null && x.ComponentCode.Contains(componentCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ComponentDescription))
        {
            var componentDescription = queryDto.ComponentDescription;
            exp = exp.And(x => x.ComponentDescription != null && x.ComponentDescription.Contains(componentDescription));
        }

        if (queryDto?.ComponentQuantity.HasValue == true)
        {
            var componentQuantity = queryDto.ComponentQuantity.Value;
            exp = exp.And(x => x.ComponentQuantity == componentQuantity);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BatchIndicator))
        {
            var batchIndicator = queryDto.BatchIndicator;
            exp = exp.And(x => x.BatchIndicator != null && x.BatchIndicator.Contains(batchIndicator));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProductionRelated))
        {
            var productionRelated = queryDto.ProductionRelated;
            exp = exp.And(x => x.ProductionRelated != null && x.ProductionRelated.Contains(productionRelated));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PcbSectIndicator))
        {
            var pcbSectIndicator = queryDto.PcbSectIndicator;
            exp = exp.And(x => x.PcbSectIndicator != null && x.PcbSectIndicator.Contains(pcbSectIndicator));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseType))
        {
            var purchaseType = queryDto.PurchaseType;
            exp = exp.And(x => x.PurchaseType != null && x.PurchaseType.Contains(purchaseType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SpecialProcurementType))
        {
            var specialProcurementType = queryDto.SpecialProcurementType;
            exp = exp.And(x => x.SpecialProcurementType != null && x.SpecialProcurementType.Contains(specialProcurementType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProfitCenterCode))
        {
            var profitCenterCode = queryDto.ProfitCenterCode;
            exp = exp.And(x => x.ProfitCenterCode != null && x.ProfitCenterCode.Contains(profitCenterCode));
        }

        if (queryDto?.MovingAveragePrice.HasValue == true)
        {
            var movingAveragePrice = queryDto.MovingAveragePrice.Value;
            exp = exp.And(x => x.MovingAveragePrice == movingAveragePrice);
        }

        if (queryDto?.MovingPriceUnit.HasValue == true)
        {
            var movingPriceUnit = queryDto.MovingPriceUnit.Value;
            exp = exp.And(x => x.MovingPriceUnit == movingPriceUnit);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MovingPriceCurrencyCode))
        {
            var movingPriceCurrencyCode = queryDto.MovingPriceCurrencyCode;
            exp = exp.And(x => x.MovingPriceCurrencyCode != null && x.MovingPriceCurrencyCode.Contains(movingPriceCurrencyCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseOrganization))
        {
            var purchaseOrganization = queryDto.PurchaseOrganization;
            exp = exp.And(x => x.PurchaseOrganization != null && x.PurchaseOrganization.Contains(purchaseOrganization));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseGroup))
        {
            var purchaseGroup = queryDto.PurchaseGroup;
            exp = exp.And(x => x.PurchaseGroup != null && x.PurchaseGroup.Contains(purchaseGroup));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SupplierCode))
        {
            var supplierCode = queryDto.SupplierCode;
            exp = exp.And(x => x.SupplierCode != null && x.SupplierCode.Contains(supplierCode));
        }

        if (queryDto?.NetPurchasePrice.HasValue == true)
        {
            var netPurchasePrice = queryDto.NetPurchasePrice.Value;
            exp = exp.And(x => x.NetPurchasePrice == netPurchasePrice);
        }

        if (queryDto?.PurchasePriceUnit.HasValue == true)
        {
            var purchasePriceUnit = queryDto.PurchasePriceUnit.Value;
            exp = exp.And(x => x.PurchasePriceUnit == purchasePriceUnit);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseCurrencyCode))
        {
            var purchaseCurrencyCode = queryDto.PurchaseCurrencyCode;
            exp = exp.And(x => x.PurchaseCurrencyCode != null && x.PurchaseCurrencyCode.Contains(purchaseCurrencyCode));
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

        if (queryDto?.CostingDateStart.HasValue == true)
        {
            var costingDateStart = queryDto.CostingDateStart.Value;
            exp = exp.And(x => x.CostingDate >= costingDateStart);
        }

        if (queryDto?.CostingDateEnd.HasValue == true)
        {
            var costingDateEnd = queryDto.CostingDateEnd.Value;
            exp = exp.And(x => x.CostingDate <= costingDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktBomMaterialCostItemQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.BomLevel))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BomItemCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProductCode))
        {
            return true;
        }

        if (queryDto.ProductCodes != null && queryDto.ProductCodes.Count > 0)
        {
            return true;
        }
        if (queryDto.LineNumber.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProductDescription))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ComponentCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ComponentDescription))
        {
            return true;
        }
        if (queryDto.ComponentQuantity.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BatchIndicator))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProductionRelated))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PcbSectIndicator))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SpecialProcurementType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProfitCenterCode))
        {
            return true;
        }
        if (queryDto.MovingAveragePrice.HasValue)
        {
            return true;
        }
        if (queryDto.MovingPriceUnit.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MovingPriceCurrencyCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseOrganization))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SupplierCode))
        {
            return true;
        }
        if (queryDto.NetPurchasePrice.HasValue)
        {
            return true;
        }
        if (queryDto.PurchasePriceUnit.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseCurrencyCode))
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
        if (queryDto.CostingDateStart.HasValue || queryDto.CostingDateEnd.HasValue)
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
