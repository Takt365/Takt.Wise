// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialUsageService.cs
// 创建时间：2026-10-21
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 通用物料 / 专用物料：成本头与成本明细 SqlSugar 联合查询后按机种数分类
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Globalization;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// 专用物料：该物料在该年月只出现在一个机种，其它机种都没有。
/// 通用物料：该物料出现在两个及以上机种。
/// 主表 TaktBomMaterialCost 与子表 TaktBomMaterialCostItem 按产品编码 InnerJoin。
/// 明细限定采购类型 F、生产相关 X。机种条件只过滤结果行。
/// </summary>
public class TaktBomMaterialUsageService : TaktServiceBase, ITaktBomMaterialUsageService
{
    /// <summary>
    /// 通用物料
    /// </summary>
    private const int KindCommon = 1;

    /// <summary>
    /// 专用物料
    /// </summary>
    private const int KindDedicated = 2;

    /// <summary>
    /// 分类结果上限（一个机种的一个物料为一行）
    /// </summary>
    private const int MaxResultRows = 100000;

    /// <summary>
    /// BOM 成本明细按年分表基表名
    /// </summary>
    private const string BomItemYearShardBaseTable = "takt_logistics_manufacturing_bom_material_cost_item";

    private readonly ISqlSugarClient _db;
    private readonly ITaktCompanyRepository<TaktBomMaterialCostItem> _bomMaterialCostItemRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="db">当前请求的 SqlSugar 客户端</param>
    /// <param name="bomMaterialCostItemRepository">成本明细仓储（年分表是否存在）</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktBomMaterialUsageService(
        ISqlSugarClient db,
        ITaktCompanyRepository<TaktBomMaterialCostItem> bomMaterialCostItemRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _db = db;
        _bomMaterialCostItemRepository = bomMaterialCostItemRepository;
    }

    /// <summary>
    /// 通用物料分页
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>分页行</returns>
    public Task<TaktPagedResult<TaktBomMaterialUsageDto>> GetBomCommonMaterialListAsync(
        TaktBomMaterialUsageQueryDto queryDto)
        => GetListAsync(queryDto, KindCommon);

    /// <summary>
    /// 专用物料分页
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>分页行</returns>
    public Task<TaktPagedResult<TaktBomMaterialUsageDto>> GetBomDedicatedMaterialListAsync(
        TaktBomMaterialUsageQueryDto queryDto)
        => GetListAsync(queryDto, KindDedicated);

    /// <summary>
    /// 导出通用物料
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public Task<(string fileName, byte[] fileContent)> ExportBomCommonMaterialAsync(
        TaktBomMaterialUsageQueryDto query,
        string? sheetName = null,
        string? fileName = null)
        => ExportAsync(query, KindCommon, "通用物料", sheetName, fileName);

    /// <summary>
    /// 导出专用物料
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public Task<(string fileName, byte[] fileContent)> ExportBomDedicatedMaterialAsync(
        TaktBomMaterialUsageQueryDto query,
        string? sheetName = null,
        string? fileName = null)
        => ExportAsync(query, KindDedicated, "专用物料", sheetName, fileName);

    /// <summary>
    /// 物料下拉取自成本明细 ComponentCode，按工厂与核算年月去重，且采购类型 F、生产相关 X、PCB SECT 标识为空
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="costingPeriod">核算年月 yyyy-MM</param>
    /// <param name="keyword">编码或描述关键字</param>
    /// <returns>选项</returns>
    public async Task<List<TaktSelectOption>> GetBomMaterialUsageComponentOptionsAsync(
        string? plantCode,
        string? costingPeriod,
        string? keyword = null)
    {
        EnsureThreeLayerContext();
        var plant = plantCode?.Trim() ?? string.Empty;
        var costingMonth = ParseCostingMonth(costingPeriod);
        if (plant.Length == 0 || !costingMonth.HasValue)
        {
            return new List<TaktSelectOption>();
        }
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var yearTable = await ResolveBomItemPhysicalTableAsync(costingMonth.Value.Year);
        if (!string.IsNullOrEmpty(yearTable))
        {
            await MergeComponentOptionsAsync(map, yearTable, plant, costingMonth.Value, keyword);
        }
        await MergeComponentOptionsAsync(map, null, plant, costingMonth.Value, keyword);
        return map
            .OrderBy(pair => pair.Key, StringComparer.Ordinal)
            .Select(pair => new TaktSelectOption
            {
                DictValue = pair.Key,
                DictLabel = string.IsNullOrWhiteSpace(pair.Value) ? pair.Key : $"{pair.Key} {pair.Value}",
            })
            .ToList();
    }

    /// <summary>
    /// 分页返回指定分类
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <param name="kind">1 通用 2 专用</param>
    /// <returns>分页行</returns>
    private async Task<TaktPagedResult<TaktBomMaterialUsageDto>> GetListAsync(
        TaktBomMaterialUsageQueryDto queryDto,
        int kind)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var rows = await BuildRowsAsync(queryDto, kind);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var page = rows.Skip(skip).Take(pageSize).ToList();
        return TaktPagedResult<TaktBomMaterialUsageDto>.Create(page, rows.Count, pageIndex, pageSize);
    }

    /// <summary>
    /// 导出全量分类行；用量清单列宽 20，其余列自适应，自然行高
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="kind">1 通用 2 专用</param>
    /// <param name="defaultSheet">默认表名</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    private async Task<(string fileName, byte[] fileContent)> ExportAsync(
        TaktBomMaterialUsageQueryDto query,
        int kind,
        string defaultSheet,
        string? sheetName,
        string? fileName)
    {
        ArgumentNullException.ThrowIfNull(query);
        var rows = await BuildRowsAsync(query, kind);
        var columnKeys = new List<string>
        {
            "costingPeriod", "modelCode", "componentCode", "componentDescription",
            "productCount", "usageText", "lineCost",
        };
        var columnLabels = new List<string>
        {
            "年月", "机种", "物料", "物料描述", "产品数", "用量清单", "行成本",
        };
        var exportRows = rows
            .Select(row => (IReadOnlyDictionary<string, object?>)new Dictionary<string, object?>(StringComparer.Ordinal)
            {
                ["costingPeriod"] = row.CostingPeriod,
                ["modelCode"] = row.ModelCode,
                ["componentCode"] = row.ComponentCode,
                ["componentDescription"] = row.ComponentDescription,
                ["productCount"] = row.ProductCount,
                ["usageText"] = row.UsageText,
                ["lineCost"] = row.LineCost,
            })
            .ToList();
        var columnWidths = new Dictionary<string, double>(StringComparer.Ordinal)
        {
            ["usageText"] = 20d,
        };
        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            columnKeys,
            columnLabels,
            sheetName ?? defaultSheet,
            fileName ?? TaktNamingHelper.PlantPrefixedTrendExportBase(query.PlantCode, defaultSheet),
            columnWidths: columnWidths,
            autoFitUnspecifiedColumns: true,
            keepNaturalRowHeight: true);
    }

    /// <summary>
    /// 按物料在指定核算年月跨越的机种数筛出通用或专用行
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <param name="kind">1 通用 2 专用</param>
    /// <returns>已排序的全量行</returns>
    private async Task<List<TaktBomMaterialUsageDto>> BuildRowsAsync(
        TaktBomMaterialUsageQueryDto queryDto,
        int kind)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        EnsureThreeLayerContext();
        var plantCode = queryDto.PlantCode.Trim();
        var costingMonth = ParseCostingMonth(queryDto.CostingPeriod);
        if (!costingMonth.HasValue)
        {
            return new List<TaktBomMaterialUsageDto>();
        }
        var costingPeriod = costingMonth.Value.ToString("yyyy-MM", CultureInfo.InvariantCulture);
        var componentCode = queryDto.ComponentCode?.Trim();
        var modelCode = string.IsNullOrEmpty(componentCode) ? queryDto.ModelCode?.Trim() : null;
        var joined = await QueryUsageJoinAsync(plantCode, costingMonth.Value, costingPeriod, componentCode);
        if (joined.Count == 0)
        {
            return new List<TaktBomMaterialUsageDto>();
        }
        var usage = new Dictionary<string, Dictionary<string, Dictionary<string, ComponentBucket>>>(StringComparer.OrdinalIgnoreCase);
        var displayByProduct = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        FillUsage(usage, displayByProduct, joined);
        var rows = Classify(plantCode, costingPeriod, usage, displayByProduct, kind, modelCode);
        if (rows.Count > MaxResultRows)
        {
            ThrowBusinessException($"分类结果超过 {MaxResultRows} 行，请指定机种或物料后再查询");
        }
        return rows;
    }

    /// <summary>
    /// 主表成本头 InnerJoin 子表成本明细：同一工厂、同一产品编码、同一核算年月。
    /// 明细限定采购类型 F、生产相关 X、PCB SECT 标识为空。按机种、产品、物料汇总用量与行成本。
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="costingMonth">核算年月月初</param>
    /// <param name="costingPeriod">yyyy-MM</param>
    /// <param name="componentCode">物料编码，空=全部</param>
    /// <returns>联合查询行</returns>
    private async Task<List<UsageJoinRow>> QueryUsageJoinAsync(
        string plantCode,
        DateTime costingMonth,
        string costingPeriod,
        string? componentCode)
    {
        var endExclusive = costingMonth.AddMonths(1);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        var yearTable = await ResolveBomItemPhysicalTableAsync(costingMonth.Year);
        var query = _db.Queryable<TaktBomMaterialCost>();
        var joined = string.IsNullOrEmpty(yearTable)
            ? query.InnerJoin<TaktBomMaterialCostItem>((h, i) =>
                h.TenantCode == i.TenantCode
                && h.CompanyCode == i.CompanyCode
                && h.PlantCode == i.PlantCode
                && h.ProductCode == i.ProductCode)
            : query.InnerJoin<TaktBomMaterialCostItem>(
                (h, i) =>
                    h.TenantCode == i.TenantCode
                    && h.CompanyCode == i.CompanyCode
                    && h.PlantCode == i.PlantCode
                    && h.ProductCode == i.ProductCode,
                yearTable);
        joined = joined.Where((h, i) =>
            h.TenantCode == tenantCode
            && h.CompanyCode == companyCode
            && h.PlantCode == plantCode
            && h.CostingPeriod == costingPeriod
            && h.IsDeleted == 0
            && h.ModelCode != null
            && h.ModelCode != ""
            && i.TenantCode == tenantCode
            && i.CompanyCode == companyCode
            && i.PlantCode == plantCode
            && i.IsDeleted == 0
            && i.CostingDate >= costingMonth
            && i.CostingDate < endExclusive
            && (i.PurchaseType == "F" || i.PurchaseType == "f")
            && (i.ProductionRelated == "X" || i.ProductionRelated == "x")
            && (i.PcbSectIndicator == null || i.PcbSectIndicator == ""));
        var componentFilter = componentCode?.Trim() ?? string.Empty;
        if (componentFilter.Length > 0)
        {
            joined = joined.Where((h, i) => i.ComponentCode == componentFilter);
        }
        return await joined
            .GroupBy((h, i) => new { h.ModelCode, h.ProductCode, i.ComponentCode })
            .Select((h, i) => new UsageJoinRow
            {
                ModelCode = h.ModelCode,
                ProductCode = h.ProductCode,
                ComponentCode = i.ComponentCode,
                ComponentDescription = SqlFunc.AggregateMax(i.ComponentDescription),
                Quantity = SqlFunc.AggregateSum(i.ComponentQuantity),
                LineCost = SqlFunc.AggregateSum(
                    i.ComponentQuantity * i.MovingAveragePrice
                    / SqlFunc.IIF(i.MovingPriceUnit <= 0, 1, i.MovingPriceUnit)),
            })
            .ToListAsync();
    }

    /// <summary>
    /// 从一张成本明细物理表汇总 ComponentCode（采购类型 F、生产相关 X、PCB SECT 标识为空）
    /// </summary>
    /// <param name="map">编码 → 描述</param>
    /// <param name="yearTable">年分表；空则基表</param>
    /// <param name="plantCode">工厂</param>
    /// <param name="costingMonth">核算年月月初</param>
    /// <param name="keyword">关键字</param>
    /// <returns>异步完成</returns>
    private async Task MergeComponentOptionsAsync(
        Dictionary<string, string> map,
        string? yearTable,
        string plantCode,
        DateTime costingMonth,
        string? keyword)
    {
        var endExclusive = costingMonth.AddMonths(1);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        var query = string.IsNullOrEmpty(yearTable)
            ? _db.Queryable<TaktBomMaterialCostItem>()
            : _db.Queryable<TaktBomMaterialCostItem>().AS(yearTable);
        query = query.Where(item =>
            item.TenantCode == tenantCode
            && item.CompanyCode == companyCode
            && item.PlantCode == plantCode
            && item.IsDeleted == 0
            && item.CostingDate >= costingMonth
            && item.CostingDate < endExclusive
            && item.ComponentCode != null
            && item.ComponentCode != ""
            && (item.PurchaseType == "F" || item.PurchaseType == "f")
            && (item.ProductionRelated == "X" || item.ProductionRelated == "x")
            && (item.PcbSectIndicator == null || item.PcbSectIndicator == ""));
        var componentKeyword = keyword?.Trim() ?? string.Empty;
        if (componentKeyword.Length > 0)
        {
            query = query.Where(item =>
                item.ComponentCode.Contains(componentKeyword)
                || item.ComponentDescription.Contains(componentKeyword));
        }
        var rows = await query
            .GroupBy(item => item.ComponentCode)
            .Select(item => new ComponentOptionRow
            {
                ComponentCode = item.ComponentCode,
                ComponentDescription = SqlFunc.AggregateMax(item.ComponentDescription),
            })
            .ToListAsync();
        foreach (var row in rows)
        {
            var code = row.ComponentCode?.Trim() ?? string.Empty;
            if (code.Length == 0)
            {
                continue;
            }
            var description = row.ComponentDescription?.Trim() ?? string.Empty;
            if (!map.TryGetValue(code, out var current) || string.IsNullOrWhiteSpace(current))
            {
                map[code] = description;
            }
        }
    }

    /// <summary>
    /// 把联合查询结果按机种、物料、产品装入用量
    /// </summary>
    /// <param name="usage">机种 → 物料 → 产品 → 用量</param>
    /// <param name="displayByProduct">产品编码 → 显示码</param>
    /// <param name="joined">联合查询行</param>
    private static void FillUsage(
        Dictionary<string, Dictionary<string, Dictionary<string, ComponentBucket>>> usage,
        Dictionary<string, string> displayByProduct,
        IReadOnlyList<UsageJoinRow> joined)
    {
        foreach (var row in joined)
        {
            if (string.IsNullOrWhiteSpace(row.ModelCode)
                || string.IsNullOrWhiteSpace(row.ComponentCode)
                || string.IsNullOrWhiteSpace(row.ProductCode))
            {
                continue;
            }
            var model = row.ModelCode.Trim();
            var componentCode = row.ComponentCode.Trim();
            var productCode = row.ProductCode.Trim();
            displayByProduct.TryAdd(productCode, productCode);
            if (!usage.TryGetValue(model, out var byComponent))
            {
                byComponent = new Dictionary<string, Dictionary<string, ComponentBucket>>(StringComparer.OrdinalIgnoreCase);
                usage[model] = byComponent;
            }
            if (!byComponent.TryGetValue(componentCode, out var perProduct))
            {
                perProduct = new Dictionary<string, ComponentBucket>(StringComparer.OrdinalIgnoreCase);
                byComponent[componentCode] = perProduct;
            }
            if (!perProduct.TryGetValue(productCode, out var bucket))
            {
                bucket = new ComponentBucket();
                perProduct[productCode] = bucket;
            }
            bucket.Quantity += row.Quantity;
            bucket.LineCost += row.LineCost;
            if (bucket.Description.Length == 0 && !string.IsNullOrWhiteSpace(row.ComponentDescription))
            {
                bucket.Description = row.ComponentDescription.Trim();
            }
        }
    }

    /// <summary>
    /// 物料只出现在一个机种为专用，出现在两个及以上机种为通用。机种条件只保留结果行。
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="costingPeriod">核算年月</param>
    /// <param name="usage">用量</param>
    /// <param name="displayByProduct">产品键 → 成本头产品编码</param>
    /// <param name="kind">1 通用 2 专用</param>
    /// <param name="modelCode">结果机种，空=全部</param>
    /// <returns>排序后的行</returns>
    private static List<TaktBomMaterialUsageDto> Classify(
        string plantCode,
        string costingPeriod,
        IReadOnlyDictionary<string, Dictionary<string, Dictionary<string, ComponentBucket>>> usage,
        IReadOnlyDictionary<string, string> displayByProduct,
        int kind,
        string? modelCode)
    {
        var modelsByComponent = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
        foreach (var modelEntry in usage)
        {
            foreach (var componentCode in modelEntry.Value.Keys)
            {
                if (!modelsByComponent.TryGetValue(componentCode, out var models))
                {
                    models = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    modelsByComponent[componentCode] = models;
                }
                models.Add(modelEntry.Key);
            }
        }
        var rows = new List<TaktBomMaterialUsageDto>();
        foreach (var modelEntry in usage)
        {
            if (!string.IsNullOrEmpty(modelCode)
                && !string.Equals(modelEntry.Key, modelCode, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }
            foreach (var component in modelEntry.Value)
            {
                var modelCount = modelsByComponent.TryGetValue(component.Key, out var models) ? models.Count : 0;
                if (modelCount <= 0)
                {
                    continue;
                }
                var isDedicated = modelCount == 1;
                if (kind == KindDedicated && !isDedicated)
                {
                    continue;
                }
                if (kind == KindCommon && isDedicated)
                {
                    continue;
                }
                rows.Add(BuildRow(plantCode, costingPeriod, modelEntry.Key, component.Key, component.Value, displayByProduct));
            }
        }
        return rows
            .OrderBy(r => r.ModelCode, StringComparer.Ordinal)
            .ThenByDescending(r => Math.Abs(r.LineCost))
            .ThenBy(r => r.ComponentCode, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>
    /// 组装一行：用量清单首行 Net，其后按行成本绝对值降序
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="costingPeriod">对比月</param>
    /// <param name="modelCode">机种</param>
    /// <param name="componentCode">组件</param>
    /// <param name="perProduct">产品用量</param>
    /// <param name="displayByKey">产品键 → 显示码</param>
    /// <returns>行</returns>
    private static TaktBomMaterialUsageDto BuildRow(
        string plantCode,
        string costingPeriod,
        string modelCode,
        string componentCode,
        IReadOnlyDictionary<string, ComponentBucket> perProduct,
        IReadOnlyDictionary<string, string> displayByKey)
    {
        var entries = new List<(string Text, decimal LineCost)>();
        decimal sum = 0m;
        string description = string.Empty;
        foreach (var pair in perProduct)
        {
            var lineCost = TaktBomMaterialCostItemLineCostHelper.RoundCost(pair.Value.LineCost);
            var qty = TaktBomMaterialCostItemLineCostHelper.RoundCost(pair.Value.Quantity);
            sum += lineCost;
            if (description.Length == 0)
            {
                description = pair.Value.Description;
            }
            var display = displayByKey.TryGetValue(pair.Key, out var code) ? code : pair.Key;
            entries.Add(($"{display}:{FormatQuantity(qty)}-{FormatMoney(lineCost)}", lineCost));
        }
        sum = TaktBomMaterialCostItemLineCostHelper.RoundCost(sum);
        var lines = new List<string> { $"Net:{FormatMoney(sum)}" };
        lines.AddRange(entries
            .OrderByDescending(e => Math.Abs(e.LineCost))
            .ThenBy(e => e.Text, StringComparer.Ordinal)
            .Select(e => e.Text));
        return new TaktBomMaterialUsageDto
        {
            UsageKey = $"{modelCode}|{componentCode}",
            PlantCode = plantCode,
            CostingPeriod = costingPeriod,
            ModelCode = modelCode,
            ComponentCode = componentCode,
            ComponentDescription = description,
            ProductCount = perProduct.Count,
            UsageText = string.Join("\n", lines),
            LineCost = sum,
        };
    }

    /// <summary>
    /// 金额文本
    /// </summary>
    /// <param name="value">金额</param>
    /// <returns>文本</returns>
    private static string FormatMoney(decimal value)
        => TaktBomMaterialCostItemLineCostHelper.RoundCost(value)
            .ToString("0.#####", CultureInfo.InvariantCulture);

    /// <summary>
    /// 数量文本（整数不带小数）
    /// </summary>
    /// <param name="quantity">数量</param>
    /// <returns>文本</returns>
    private static string FormatQuantity(decimal quantity)
    {
        return quantity == decimal.Truncate(quantity)
            ? decimal.Truncate(quantity).ToString(CultureInfo.InvariantCulture)
            : quantity.ToString("0.#####", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// 解析核算年月 yyyy-MM 为当月 1 日
    /// </summary>
    /// <param name="costingPeriod">yyyy-MM</param>
    /// <returns>月初；格式不符时为 null</returns>
    private static DateTime? ParseCostingMonth(string? costingPeriod)
    {
        if (string.IsNullOrWhiteSpace(costingPeriod))
        {
            return null;
        }
        return DateTime.TryParseExact(
            costingPeriod.Trim(),
            "yyyy-MM",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out var month)
            ? new DateTime(month.Year, month.Month, 1)
            : null;
    }

    /// <summary>
    /// 生成 BOM 成本明细年分表名
    /// </summary>
    /// <param name="year">年份</param>
    /// <returns>物理表名</returns>
    private static string BuildBomItemYearTable(int year) =>
        TaktYearShardTableHelper.BuildYearTableName(BomItemYearShardBaseTable, year);

    /// <summary>
    /// 解析 BOM 成本明细物理表：年分表存在则用之
    /// </summary>
    /// <param name="year">自然年</param>
    /// <returns>年分表名；不存在时为 null</returns>
    private async Task<string?> ResolveBomItemPhysicalTableAsync(int year)
    {
        var table = BuildBomItemYearTable(year);
        return await _bomMaterialCostItemRepository.PhysicalTableExistsAsync(table) ? table : null;
    }

    /// <summary>
    /// 明细物料去重行
    /// </summary>
    private sealed class ComponentOptionRow
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string ComponentCode { get; set; } = string.Empty;

        /// <summary>
        /// 物料描述
        /// </summary>
        public string? ComponentDescription { get; set; }
    }

    /// <summary>
    /// 成本头与明细联合查询的一行（机种 + 产品 + 物料）
    /// </summary>
    private sealed class UsageJoinRow
    {
        /// <summary>
        /// 机种编码
        /// </summary>
        public string ModelCode { get; set; } = string.Empty;

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; } = string.Empty;

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ComponentCode { get; set; } = string.Empty;

        /// <summary>
        /// 物料描述
        /// </summary>
        public string? ComponentDescription { get; set; }

        /// <summary>
        /// 用量合计
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// 行成本合计
        /// </summary>
        public decimal LineCost { get; set; }
    }

    /// <summary>
    /// 一个成品上的物料用量
    /// </summary>
    private sealed class ComponentBucket
    {
        /// <summary>
        /// 用量合计
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// 行成本合计
        /// </summary>
        public decimal LineCost { get; set; }

        /// <summary>
        /// 组件描述
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
