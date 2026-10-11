// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：TaktMaterialTrendAnalysisService.cs
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：物料移动价格推移分析核心分析服务实现
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using System.Text;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Materials;
using Takt.Application.Services.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Shared.Validation;

namespace Takt.Application.Services.Logistics.Materials;

/// <summary>
/// 物料移动价格推移分析核心分析服务（读移动价格本表；与 CRUD 服务分离）
/// </summary>
public class TaktMaterialTrendAnalysisService : TaktServiceBase, ITaktMaterialTrendAnalysisService
{
    /// <summary>
    /// 物料名称按编码分批查询，避免超长 IN 列表
    /// </summary>
    private const int MaterialNameLookupBatchSize = 500;

    /// <summary>
    /// 移动价格按年分表基表名
    /// </summary>
    private const string MovingPriceYearShardBaseTable = "takt_logistics_materials_material_moving_price";

    /// <summary>
    /// 推移跨年取数探测年数
    /// </summary>
    private const int YearShardProbeYears = 6;

    private readonly ITaktCompanyRepository<TaktMaterialMovingPrice> _materialMovingPriceRepository;
    private readonly ITaktCompanyRepository<TaktMaterialPlant> _materialPlantRepository;
    private readonly ITaktCompanyRepository<TaktBomMaterialCostItem> _bomMaterialCostItemRepository;
    private readonly ITaktCompanyRepository<TaktBomMaterialCost> _bomMaterialCostRepository;
    private readonly ITaktTenantRepository<TaktModelDestination> _modelDestinationRepository;
    private readonly ITaktTenantRepository<TaktCompany> _companyRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="materialMovingPriceRepository">移动价格仓储</param>
    /// <param name="materialPlantRepository">工厂物料仓储</param>
    /// <param name="bomMaterialCostItemRepository">BOM 物料成本明细仓储</param>
    /// <param name="bomMaterialCostRepository">BOM 物料成本汇总仓储</param>
    /// <param name="modelDestinationRepository">型号目的地仓储</param>
    /// <param name="companyRepository">公司仓储（RelatedPlant）</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMaterialTrendAnalysisService(
        ITaktCompanyRepository<TaktMaterialMovingPrice> materialMovingPriceRepository,
        ITaktCompanyRepository<TaktMaterialPlant> materialPlantRepository,
        ITaktCompanyRepository<TaktBomMaterialCostItem> bomMaterialCostItemRepository,
        ITaktCompanyRepository<TaktBomMaterialCost> bomMaterialCostRepository,
        ITaktTenantRepository<TaktModelDestination> modelDestinationRepository,
        ITaktTenantRepository<TaktCompany> companyRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _materialMovingPriceRepository = materialMovingPriceRepository;
        _materialPlantRepository = materialPlantRepository;
        _bomMaterialCostItemRepository = bomMaterialCostItemRepository;
        _bomMaterialCostRepository = bomMaterialCostRepository;
        _modelDestinationRepository = modelDestinationRepository;
        _companyRepository = companyRepository;
    }

    /// <summary>
    /// 推移查询栏工厂选项：当前公司 RelatedPlant ∩ 移动价格本表有数（对齐 BOM 成本差异推移）
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMaterialMovingTrendPlantOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var companies = await _companyRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode);
        var relatedPlant = companies
            .Select(c => c.RelatedPlant?.Trim() ?? string.Empty)
            .FirstOrDefault(p => !string.IsNullOrEmpty(p))
            ?? string.Empty;
        if (string.IsNullOrEmpty(relatedPlant))
        {
            return new List<TaktSelectOption>();
        }
        // 年分表探测：RelatedPlant 在近年分表或基表有任一移动价格行即可
        var probeStart = new DateTime(DateTime.Now.Year - YearShardProbeYears, 1, 1);
        var probeEnd = DateTime.Now;
        Expression<Func<TaktMaterialMovingPrice, bool>> existsExp = x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == relatedPlant;
        var sample = await GetMovingPriceListForRangeAsync(existsExp, probeStart, probeEnd, maxRows: 1);
        if (sample.Count == 0)
        {
            return new List<TaktSelectOption>();
        }
        return new List<TaktSelectOption>
        {
            new() { DictValue = relatedPlant, DictLabel = relatedPlant },
        };
    }

    /// <summary>
    /// 推移查询栏：按工厂去重评估类别（级联第 2 级）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMaterialMovingTrendValuationOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var plant = plantCode?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(plant))
        {
            return new List<TaktSelectOption>();
        }
        var list = await _materialMovingPriceRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plant
                && x.Valuation != null
                && x.Valuation != string.Empty);
        return list
            .GroupBy(e => e.Valuation.Trim(), StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g => new TaktSelectOption
            {
                DictValue = g.Key,
                DictLabel = g.Key,
            })
            .ToList();
    }

    /// <summary>
    /// 推移查询栏：按工厂去重物料；有评估类别时再按评估过滤（级联第 3 级，查询时可空）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="valuation">评估类别（可空；有值时收窄物料列表）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMaterialMovingTrendMaterialOptionsAsync(string? plantCode = null, string? keyword = null, string? valuation = null)
    {
        EnsureThreeLayerContext();
        var plant = plantCode?.Trim() ?? string.Empty;
        var val = valuation?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(plant))
        {
            return new List<TaktSelectOption>();
        }
        var list = string.IsNullOrEmpty(val)
            ? await _materialMovingPriceRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && x.PlantCode == plant
                    && x.MaterialCode != null
                    && x.MaterialCode != string.Empty)
            : await _materialMovingPriceRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && x.PlantCode == plant
                    && x.Valuation == val
                    && x.MaterialCode != null
                    && x.MaterialCode != string.Empty);
        return list
            .GroupBy(e => e.MaterialCode.Trim(), StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g => new TaktSelectOption
            {
                DictValue = g.Key,
                DictLabel = g.Key,
            })
            .ToList();
    }

    /// <summary>
    /// 物料月移动价格推移分析（分页）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>转置分析结果</returns>
    public async Task<TaktMaterialMovingTrendResultDto> GetMaterialMovingTrendAnalysisAsync(
        TaktMaterialMovingTrendQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var built = await BuildMonthlyTrendAnalysisAsync(queryDto);
        var pageRows = built.OrderedRows.Skip(skip).Take(pageSize).ToList();
        return new TaktMaterialMovingTrendResultDto
        {
            Paged = TaktPagedResult<TaktMaterialMovingTrendDto>.Create(
                pageRows, built.OrderedRows.Count, pageIndex, pageSize),
            PeriodOrder = built.PeriodOrder,
            MaterialCount = built.OrderedRows.Count,
            BasePeriod = pageRows.FirstOrDefault()?.BasePeriod ?? built.BasePeriod,
            ComparePeriod = built.ComparePeriod,
            UpCount = built.UpCount,
            DownCount = built.DownCount,
            FlatCount = built.FlatCount,
            NoneCount = built.NoneCount,
        };
    }

    /// <summary>
    /// 导出物料月移动价格推移分析（全量）
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMaterialMovingTrendAnalysisAsync(
        TaktMaterialMovingTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var built = await BuildMonthlyTrendAnalysisAsync(query);
        var columnKeys = new List<string>
        {
            "plantCode", "materialCode", "materialName", "valuation", "currency",
        };
        var columnLabels = new List<string>
        {
            "工厂代码", "物料编码", "物料名称", "评估类别", "币种",
        };
        foreach (var period in built.PeriodOrder)
        {
            columnKeys.Add($"period_{period}");
            columnLabels.Add(period);
        }
        columnKeys.AddRange(new[] { "basePeriod", "comparePeriod", "varianceAmount", "variancePercent", "trend" });
        columnLabels.AddRange(new[] { "基准月", "对比月", "环比差额", "环比%", "涨跌" });
        var exportRows = built.OrderedRows.Select(row =>
        {
            var dict = new Dictionary<string, object?>(StringComparer.Ordinal)
            {
                ["plantCode"] = row.PlantCode,
                ["materialCode"] = row.MaterialCode,
                ["materialName"] = row.MaterialName,
                ["valuation"] = row.Valuation,
                ["currencyCode"] = row.CurrencyCode,
                ["basePeriod"] = row.BasePeriod,
                ["comparePeriod"] = row.ComparePeriod,
                ["varianceAmount"] = row.VarianceAmount,
                ["variancePercent"] = row.VariancePercent.HasValue
                    ? Math.Round(row.VariancePercent.Value, 4, MidpointRounding.AwayFromZero)
                    : null,
                ["trend"] = row.Trend,
            };
            foreach (var period in built.PeriodOrder)
            {
                if (!row.PeriodUnitPrices.TryGetValue(period, out var price))
                {
                    dict[$"period_{period}"] = null;
                    continue;
                }
                var isCarried = row.PeriodPriceSourcePeriods.TryGetValue(period, out var source)
                    && !string.IsNullOrWhiteSpace(source)
                    && !string.Equals(source, period, StringComparison.Ordinal);
                dict[$"period_{period}"] = isCarried
                    ? $"{price.ToString("0.00000", System.Globalization.CultureInfo.InvariantCulture)}*"
                    : price;
            }
            return (IReadOnlyDictionary<string, object?>)dict;
        }).ToList();
        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            columnKeys,
            columnLabels,
            sheetName ?? "物料移动价格推移清单",
            fileName ?? TaktNamingHelper.PlantPrefixedTrendExportBase(query.PlantCode, "移动价格推移"));
    }

    /// <summary>
    /// 按物料加载 BOM 使用关系：Component → Product（明细 DISTINCT）→ Model（汇总 DISTINCT）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="materialCodes">物料编码清单</param>
    /// <param name="materialType">产品物料类型（空则默认 FERT）</param>
    /// <returns>物料 → 产品/机种</returns>
    private async Task<Dictionary<string, BomMaterialUsageInfo>> LoadBomMaterialUsageLookupAsync(
        string plantCode,
        IReadOnlyList<string> materialCodes,
        string? materialType = null)
    {
        var result = new Dictionary<string, BomMaterialUsageInfo>(StringComparer.OrdinalIgnoreCase);
        var codes = materialCodes
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Select(c => c.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        if (codes.Count == 0)
        {
            return result;
        }

        var materialToProducts = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
        var allProducts = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // 小批量：按组件 IN 查 DISTINCT；大批量导出：工厂级 DISTINCT 再内存过滤（避免拉全量 CostingDate 明细实体）
        if (codes.Count <= MaterialNameLookupBatchSize)
        {
            await FillComponentProductPairsByCodesAsync(plantCode, codes, materialToProducts, allProducts);
        }
        else
        {
            await FillComponentProductPairsForPlantAsync(plantCode, new HashSet<string>(codes, StringComparer.OrdinalIgnoreCase), materialToProducts, allProducts);
        }
        var resolvedType = ResolveBomUsageMaterialType(materialType);
        // 机种/产品组按查询物料类型过滤（空则 FERT）
        await FilterBomUsageProductsByMaterialTypeAsync(
            plantCode,
            resolvedType,
            materialToProducts,
            allProducts);

        var productToModels = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
        var productList = allProducts.ToList();
        // 与 BOM 汇总同步同口径：产品→机种优先型号目的地，再回退成本汇总表
        await FillProductModelsFromModelDestinationAsync(productList, productToModels);
        var missingProducts = productList
            .Where(p => !productToModels.TryGetValue(p, out var models) || models.Count == 0)
            .ToList();
        if (missingProducts.Count > 0)
        {
            await FillProductModelPairsByCodesAsync(plantCode, missingProducts, productToModels, resolvedType);
        }

        foreach (var material in codes)
        {
            if (!materialToProducts.TryGetValue(material, out var products) || products.Count == 0)
            {
                result[material] = new BomMaterialUsageInfo();
                continue;
            }
            var modelSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var product in products)
            {
                if (!productToModels.TryGetValue(product, out var models))
                {
                    continue;
                }
                foreach (var model in models)
                {
                    modelSet.Add(model);
                }
            }
            result[material] = new BomMaterialUsageInfo
            {
                ProductCodes = products.OrderBy(p => p, StringComparer.OrdinalIgnoreCase).ToList(),
                ModelCodes = modelSet.OrderBy(m => m, StringComparer.OrdinalIgnoreCase).ToList(),
            };
        }
        return result;
    }

    /// <summary>
    /// DISTINCT 组件→产品（指定组件编码）
    /// </summary>
    private async Task FillComponentProductPairsByCodesAsync(
        string plantCode,
        IReadOnlyList<string> componentCodes,
        Dictionary<string, HashSet<string>> materialToProducts,
        HashSet<string> allProducts)
    {
        if (componentCodes.Count == 0)
        {
            return;
        }
        var sql = new StringBuilder();
        sql.Append(
            """
            SELECT DISTINCT
              LTRIM(RTRIM(component_code)) AS ComponentCode,
              LTRIM(RTRIM(product_code)) AS ProductCode
            FROM takt_logistics_manufacturing_bom_material_cost_item
            WHERE is_deleted = 0
              AND tenant_code = @tenantCode
              AND company_code = @companyCode
              AND plant_code = @plantCode
              AND component_code IN (
            """);
        var parameters = new Dictionary<string, object?>
        {
            ["tenantCode"] = CurrentTenantCode,
            ["companyCode"] = CurrentCompanyCode,
            ["plantCode"] = plantCode,
        };
        for (var i = 0; i < componentCodes.Count; i++)
        {
            var name = $"c{i}";
            if (i > 0)
            {
                sql.Append(',');
            }
            sql.Append('@').Append(name);
            parameters[name] = componentCodes[i];
        }
        sql.Append(')');
        var script = sql.ToString();
        TaktSqlExecutorValidator.Validate(script);
        var rows = await _bomMaterialCostItemRepository.QueryReadOnlySqlAsync(script, parameters);
        AddComponentProductPairs(rows, materialToProducts, allProducts);
    }

    /// <summary>
    /// 工厂级 DISTINCT 组件→产品，仅保留目标物料
    /// </summary>
    private async Task FillComponentProductPairsForPlantAsync(
        string plantCode,
        HashSet<string> targetComponents,
        Dictionary<string, HashSet<string>> materialToProducts,
        HashSet<string> allProducts)
    {
        const string script = """
            SELECT DISTINCT
              LTRIM(RTRIM(component_code)) AS ComponentCode,
              LTRIM(RTRIM(product_code)) AS ProductCode
            FROM takt_logistics_manufacturing_bom_material_cost_item
            WHERE is_deleted = 0
              AND tenant_code = @tenantCode
              AND company_code = @companyCode
              AND plant_code = @plantCode
            """;
        TaktSqlExecutorValidator.Validate(script);
        var parameters = new Dictionary<string, object?>
        {
            ["tenantCode"] = CurrentTenantCode,
            ["companyCode"] = CurrentCompanyCode,
            ["plantCode"] = plantCode,
        };
        var rows = await _bomMaterialCostItemRepository.QueryReadOnlySqlAsync(script, parameters);
        foreach (var row in rows)
        {
            var component = ReadSqlString(row, "ComponentCode");
            if (string.IsNullOrWhiteSpace(component) || !targetComponents.Contains(component))
            {
                continue;
            }
            var product = ReadSqlString(row, "ProductCode");
            if (string.IsNullOrWhiteSpace(product))
            {
                continue;
            }
            if (!materialToProducts.TryGetValue(component, out var products))
            {
                products = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                materialToProducts[component] = products;
            }
            products.Add(product);
            allProducts.Add(product);
        }
    }

    /// <summary>
    /// 产品→机种：型号目的地（MaterialCode=产品编码，与 ResolveModelCodeByProductAsync 同口径）
    /// </summary>
    /// <param name="productCodes">产品编码</param>
    /// <param name="productToModels">产品→机种集合</param>
    private async Task FillProductModelsFromModelDestinationAsync(
        IReadOnlyList<string> productCodes,
        Dictionary<string, HashSet<string>> productToModels)
    {
        if (productCodes.Count == 0)
        {
            return;
        }
        var destinations = await _modelDestinationRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.MaterialCode != null);
        if (destinations.Count == 0)
        {
            return;
        }
        foreach (var product in productCodes)
        {
            if (string.IsNullOrWhiteSpace(product))
            {
                continue;
            }
            foreach (var dest in destinations)
            {
                if (string.IsNullOrWhiteSpace(dest.ModelCode))
                {
                    continue;
                }
                if (!TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(dest.MaterialCode, product))
                {
                    continue;
                }
                if (!productToModels.TryGetValue(product, out var models))
                {
                    models = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    productToModels[product] = models;
                }
                models.Add(dest.ModelCode.Trim());
            }
        }
    }

    /// <summary>
 /// DISTINCT 产品→机种（成本汇总表回退；支持 物料码归一匹配）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="productCodes">产品编码</param>
    /// <param name="productToModels">产品→机种</param>
    /// <param name="materialType">产品物料类型</param>
    private async Task FillProductModelPairsByCodesAsync(
        string plantCode,
        IReadOnlyList<string> productCodes,
        Dictionary<string, HashSet<string>> productToModels,
        string materialType)
    {
        if (productCodes.Count == 0)
        {
            return;
        }
        ArgumentException.ThrowIfNullOrWhiteSpace(materialType);
        var targetProducts = new HashSet<string>(productCodes, StringComparer.OrdinalIgnoreCase);
        const string script = """
            SELECT DISTINCT
              LTRIM(RTRIM(product_code)) AS ProductCode,
              LTRIM(RTRIM(model_code)) AS ModelCode
            FROM takt_logistics_manufacturing_bom_material_cost
            WHERE is_deleted = 0
              AND tenant_code = @tenantCode
              AND company_code = @companyCode
              AND plant_code = @plantCode
              AND UPPER(LTRIM(RTRIM(ISNULL(material_type, '')))) = @materialType
              AND LEN(LTRIM(RTRIM(ISNULL(model_code, '')))) > 0
            """;
        TaktSqlExecutorValidator.Validate(script);
        var parameters = new Dictionary<string, object?>
        {
            ["tenantCode"] = CurrentTenantCode,
            ["companyCode"] = CurrentCompanyCode,
            ["plantCode"] = plantCode,
            ["materialType"] = materialType.Trim().ToUpperInvariant(),
        };
        var rows = await _bomMaterialCostRepository.QueryReadOnlySqlAsync(script, parameters);
        foreach (var row in rows)
        {
            var storedProduct = ReadSqlString(row, "ProductCode");
            var model = ReadSqlString(row, "ModelCode");
            if (string.IsNullOrWhiteSpace(storedProduct) || string.IsNullOrWhiteSpace(model))
            {
                continue;
            }
            foreach (var requested in targetProducts)
            {
                if (!TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(storedProduct, requested))
                {
                    continue;
                }
                if (!productToModels.TryGetValue(requested, out var models))
                {
                    models = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    productToModels[requested] = models;
                }
                models.Add(model);
            }
        }
    }

    /// <summary>
    /// BOM 产品组过滤用物料类型（空则默认 FERT）
    /// </summary>
    /// <param name="materialType">查询类型</param>
    /// <returns>类型码</returns>
    private static string ResolveBomUsageMaterialType(string? materialType)
    {
        var type = materialType?.Trim();
        return string.IsNullOrWhiteSpace(type)
            ? TaktBomMaterialCostItemLineCostHelper.FertMaterialTypeCode
            : type;
    }

    /// <summary>
    /// 仅保留 BOM 成本汇总中指定 MaterialType 的产品（机种推移口径）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="materialType">产品物料类型</param>
    /// <param name="materialToProducts">组件→产品</param>
    /// <param name="allProducts">产品集合</param>
    private async Task FilterBomUsageProductsByMaterialTypeAsync(
        string plantCode,
        string materialType,
        Dictionary<string, HashSet<string>> materialToProducts,
        HashSet<string> allProducts)
    {
        if (allProducts.Count == 0)
        {
            return;
        }
        ArgumentException.ThrowIfNullOrWhiteSpace(materialType);
        const string script = """
            SELECT DISTINCT
              LTRIM(RTRIM(product_code)) AS ProductCode
            FROM takt_logistics_manufacturing_bom_material_cost
            WHERE is_deleted = 0
              AND tenant_code = @tenantCode
              AND company_code = @companyCode
              AND plant_code = @plantCode
              AND UPPER(LTRIM(RTRIM(ISNULL(material_type, '')))) = @materialType
              AND LEN(LTRIM(RTRIM(ISNULL(product_code, '')))) > 0
            """;
        TaktSqlExecutorValidator.Validate(script);
        var parameters = new Dictionary<string, object?>
        {
            ["tenantCode"] = CurrentTenantCode,
            ["companyCode"] = CurrentCompanyCode,
            ["plantCode"] = plantCode,
            ["materialType"] = materialType.Trim().ToUpperInvariant(),
        };
        var rows = await _bomMaterialCostRepository.QueryReadOnlySqlAsync(script, parameters);
        var typedProducts = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var row in rows)
        {
            var product = ReadSqlString(row, "ProductCode");
            if (!string.IsNullOrWhiteSpace(product))
            {
                typedProducts.Add(product);
            }
        }
        var excluded = allProducts
            .Where(p => !typedProducts.Any(f => TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(f, p)))
            .ToList();
        foreach (var product in excluded)
        {
            allProducts.Remove(product);
        }
        foreach (var pair in materialToProducts)
        {
            pair.Value.RemoveWhere(p =>
                !typedProducts.Any(f => TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(f, p)));
        }
    }

    /// <summary>
    /// 解析 DISTINCT 组件-产品行
    /// </summary>
    private static void AddComponentProductPairs(
        IReadOnlyList<Dictionary<string, object>> rows,
        Dictionary<string, HashSet<string>> materialToProducts,
        HashSet<string> allProducts)
    {
        foreach (var row in rows)
        {
            var component = ReadSqlString(row, "ComponentCode");
            var product = ReadSqlString(row, "ProductCode");
            if (string.IsNullOrWhiteSpace(component) || string.IsNullOrWhiteSpace(product))
            {
                continue;
            }
            if (!materialToProducts.TryGetValue(component, out var products))
            {
                products = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                materialToProducts[component] = products;
            }
            products.Add(product);
            allProducts.Add(product);
        }
    }

    /// <summary>
    /// 读取只读 SQL 行字符串列
    /// </summary>
    private static string ReadSqlString(Dictionary<string, object> row, string column)
    {
        if (!row.TryGetValue(column, out var value) || value == null)
        {
            return string.Empty;
        }
        return Convert.ToString(value)?.Trim() ?? string.Empty;
    }

    /// <summary>
    /// 构建物料月移动价格推移全量结果（不分页；列表/导出共用）。
    /// ① 移动价格表去重物料码；指定物料且无价时，若 BOM 有机种使用仍纳入清单（单价按 0）；
    /// ② 再按查询期间转置各月单价（缺月回填用历史价；无历史则为 0）。
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <param name="materialType">物料类型过滤（可选；为空则不按类型收窄）</param>
    /// <returns>排序后的全量行与汇总</returns>
    private async Task<MonthlyTrendAnalysisBuilt> BuildMonthlyTrendAnalysisAsync(
        TaktMaterialMovingTrendQueryDto queryDto,
        string? materialType = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        EnsureThreeLayerContext();
        var plantCode = queryDto.PlantCode.Trim();
        var valuationFilter = string.IsNullOrWhiteSpace(queryDto.Valuation) ? null : queryDto.Valuation.Trim();
        var materialFilter = string.IsNullOrWhiteSpace(queryDto.MaterialCode) ? null : queryDto.MaterialCode.Trim();
        var (periodStart, periodEnd) = NormalizePeriodBounds(queryDto.PeriodDateStart, queryDto.PeriodDateEnd);

        // 清单源：工厂下近年分表内移动价格行（不含 PeriodDate 条件），再 Distinct MaterialCode
        var rosterExp = BuildTrendSourceExpression(plantCode, valuationFilter, materialFilter, periodStart: null, periodEnd: null);
        var rosterStart = periodStart?.AddYears(-5) ?? new DateTime(DateTime.Now.Year - YearShardProbeYears, 1, 1);
        var rosterEnd = periodEnd ?? DateTime.Now;
        var rosterSourceRows = await GetMovingPriceListForRangeAsync(rosterExp, rosterStart, rosterEnd);
        var materialCodeSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var code in rosterSourceRows
            .Where(r => !string.IsNullOrWhiteSpace(r.MaterialCode))
            .Select(r => r.MaterialCode.Trim()))
        {
            materialCodeSet.Add(code);
        }

        // 指定物料编码：无移动价时，若 BOM 组件存在且已解析到机种，仍纳入（价格为 0）
        if (!string.IsNullOrWhiteSpace(materialFilter))
        {
            var bomWithModel = await ResolveBomComponentCodesWithModelUsageAsync(
                plantCode,
                materialFilter,
                materialType);
            foreach (var code in bomWithModel)
            {
                materialCodeSet.Add(code);
            }
        }

        if (materialCodeSet.Count == 0)
        {
            return MonthlyTrendAnalysisBuilt.Empty();
        }

        var materialCodes = materialCodeSet
            .OrderBy(c => c, StringComparer.OrdinalIgnoreCase)
            .ToList();

        // 转置/缺月回填：保留展示期止以前的全部历史价（不再截断为仅前 36 个月，
        // 否则如 2023-04-30 在查 2026 年时会被裁掉，既无回填也无 *）
        IEnumerable<TaktMaterialMovingPrice> priceQuery = rosterSourceRows;
        if (periodEnd.HasValue)
        {
            var periodEndExclusive = periodEnd.Value.AddMonths(1);
            var endYmExclusive = ToPeriodKey(periodEndExclusive);
            priceQuery = priceQuery.Where(r => string.CompareOrdinal(NormalizeYm(r.ValuationPeriod), endYmExclusive) < 0);
        }
        var priceRows = priceQuery.ToList();
        var priceByMaterial = priceRows
            .Where(r => !string.IsNullOrWhiteSpace(r.MaterialCode))
            .GroupBy(r => r.MaterialCode.Trim(), StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => (IReadOnlyList<TaktMaterialMovingPrice>)g.ToList(), StringComparer.OrdinalIgnoreCase);

        var periodOrder = BuildPeriodOrder(priceRows, periodStart, periodEnd);
        if (periodOrder.Count == 0)
        {
            return MonthlyTrendAnalysisBuilt.Empty();
        }
        var focusPeriod = ResolveFocusPeriod(queryDto.FocusPeriod, periodOrder);
        var nameLookup = await LoadMaterialNameLookupAsync(plantCode, materialCodes);
        var emptyRows = (IReadOnlyList<TaktMaterialMovingPrice>)Array.Empty<TaktMaterialMovingPrice>();
        var allRows = materialCodes
            .Select(code =>
            {
                if (!priceByMaterial.TryGetValue(code, out var materialRows))
                {
                    materialRows = emptyRows;
                }
                nameLookup.TryGetValue(code, out var materialName);
                var valuation = ResolveDisplayValuation(materialRows, focusPeriod);
                if (string.IsNullOrWhiteSpace(valuation) && !string.IsNullOrWhiteSpace(valuationFilter))
                {
                    valuation = valuationFilter;
                }
                return BuildMonthlyTrendRow(
                    plantCode,
                    code,
                    materialName ?? string.Empty,
                    valuation,
                    materialRows,
                    periodOrder,
                    focusPeriod);
            })
            .ToList();

        var filtered = FilterTrendRows(allRows, queryDto.TrendFilter);
        var ordered = OrderTrendRows(filtered);
        return new MonthlyTrendAnalysisBuilt
        {
            OrderedRows = ordered,
            PeriodOrder = periodOrder,
            BasePeriod = allRows.FirstOrDefault()?.BasePeriod,
            ComparePeriod = focusPeriod,
            UpCount = allRows.Count(r => r.Trend == "up"),
            DownCount = allRows.Count(r => r.Trend == "down"),
            FlatCount = allRows.Count(r => r.Trend == "flat"),
            NoneCount = allRows.Count(r => r.Trend == "none"),
        };
    }

    /// <summary>
    /// 按物料关键字从 BOM 明细取组件码，并仅保留已解析到机种的组件（无移动价也可进推移清单）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="materialFilter">物料编码关键字（包含匹配）</param>
    /// <param name="materialType">产品物料类型（空则默认 FERT）</param>
    /// <returns>有机种使用的组件编码</returns>
    private async Task<List<string>> ResolveBomComponentCodesWithModelUsageAsync(
        string plantCode,
        string materialFilter,
        string? materialType = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(plantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(materialFilter);
        const string script = """
            SELECT DISTINCT LTRIM(RTRIM(component_code)) AS ComponentCode
            FROM takt_logistics_manufacturing_bom_material_cost_item
            WHERE is_deleted = 0
              AND tenant_code = @tenantCode
              AND company_code = @companyCode
              AND plant_code = @plantCode
              AND component_code LIKE @materialPattern
            """;
        TaktSqlExecutorValidator.Validate(script);
        var parameters = new Dictionary<string, object?>
        {
            ["tenantCode"] = CurrentTenantCode,
            ["companyCode"] = CurrentCompanyCode,
            ["plantCode"] = plantCode,
            ["materialPattern"] = $"%{materialFilter.Trim()}%",
        };
        var rows = await _bomMaterialCostItemRepository.QueryReadOnlySqlAsync(script, parameters);
        var candidates = rows
            .Select(r => ReadSqlString(r, "ComponentCode"))
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Take(MaterialNameLookupBatchSize)
            .ToList();
        if (candidates.Count == 0)
        {
            return new List<string>();
        }
        var usage = await LoadBomMaterialUsageLookupAsync(plantCode, candidates, materialType);
        return candidates
            .Where(c => usage.TryGetValue(c, out var info) && info.ModelCodes.Count > 0)
            .ToList();
    }

    /// <summary>
    /// 构建推移分析源数据条件（租户/公司/工厂；可选评估、物料关键字、期间）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="valuationFilter">评估类别；空则不过滤</param>
    /// <param name="materialFilter">物料编码包含；空则不过滤</param>
    /// <param name="periodStart">期间起（含）；空则不按期间过滤</param>
    /// <param name="periodEnd">期间止（月初，含当月）；空则不按期间过滤</param>
    /// <returns>表达式</returns>
    private Expression<Func<TaktMaterialMovingPrice, bool>> BuildTrendSourceExpression(
        string plantCode,
        string? valuationFilter,
        string? materialFilter,
        DateTime? periodStart,
        DateTime? periodEnd)
    {
        var exp = Expressionable.Create<TaktMaterialMovingPrice>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode);
        if (periodStart.HasValue)
        {
            var startYm = periodStart.Value.ToString("yyyy-MM");
            exp = exp.And(x => x.ValuationPeriod != null && string.Compare(x.ValuationPeriod, startYm) >= 0);
        }
        if (periodEnd.HasValue)
        {
            var endYm = periodEnd.Value.ToString("yyyy-MM");
            exp = exp.And(x => x.ValuationPeriod != null && string.Compare(x.ValuationPeriod, endYm) <= 0);
        }
        if (!string.IsNullOrWhiteSpace(valuationFilter))
        {
            exp = exp.And(x => x.Valuation == valuationFilter);
        }
        if (!string.IsNullOrWhiteSpace(materialFilter))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(materialFilter));
        }
        return exp.ToExpression();
    }

    /// <summary>
    /// 加载工厂物料名称字典
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="materialCodes">物料编码</param>
    /// <returns>编码→名称</returns>
    private async Task<Dictionary<string, string>> LoadMaterialNameLookupAsync(
        string plantCode,
        IReadOnlyList<string> materialCodes)
    {
        if (materialCodes.Count == 0)
        {
            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }
        var codes = materialCodes.Where(c => !string.IsNullOrWhiteSpace(c)).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        for (var offset = 0; offset < codes.Count; offset = checked(offset + MaterialNameLookupBatchSize))
        {
            var batch = codes.Skip(offset).Take(MaterialNameLookupBatchSize).ToList();
            var plants = await _materialPlantRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && x.PlantCode == plantCode
                    && batch.Contains(x.MaterialCode));
            foreach (var group in plants
                .Where(p => !string.IsNullOrWhiteSpace(p.MaterialCode))
                .GroupBy(p => p.MaterialCode.Trim(), StringComparer.OrdinalIgnoreCase))
            {
                if (map.ContainsKey(group.Key))
                {
                    continue;
                }
                map[group.Key] = group.Select(x => x.MaterialDescription)
                    .FirstOrDefault(n => !string.IsNullOrWhiteSpace(n))?.Trim() ?? string.Empty;
            }
        }
        return map;
    }

    /// <summary>
    /// 构建单行物料月推移
    /// </summary>
    private static TaktMaterialMovingTrendDto BuildMonthlyTrendRow(
        string plantCode,
        string materialCode,
        string materialName,
        string valuation,
        IReadOnlyList<TaktMaterialMovingPrice> materialRows,
        IReadOnlyList<string> periodOrder,
        string? focusPeriod)
    {
        // 使用无自定义比较器的字典，避免 JSON 序列化后前端读不到来源月（* 标记）
        var periodUnitPrices = new Dictionary<string, decimal>();
        var periodPriceSourcePeriods = new Dictionary<string, string>();
        string currency = string.Empty;
        decimal? lastUnitPrice = null;
        string? lastSourcePeriod = null;
        string lastCurrency = string.Empty;

        if (periodOrder.Count > 0)
        {
            // 展示期首月之前的最近有价月（含 2023-04-30 这类月末日期）作为回填种子
            var seed = PickMostRecentPositiveOnOrBefore(materialRows, periodOrder[0], exclusive: true);
            if (seed != null && TryResolveTrendUnitPrice(seed, out var seedPrice))
            {
                lastUnitPrice = seedPrice;
                lastSourcePeriod = NormalizeYm(seed.ValuationPeriod);
                lastCurrency = seed.CurrencyCode?.Trim() ?? string.Empty;
            }
        }
        foreach (var period in periodOrder)
        {
            var picked = PickForPeriod(materialRows, period);
            if (picked != null && TryResolveTrendUnitPrice(picked, out var periodUnit))
            {
                // 当月有正移动价：采用当月，来源=当月（前端不标 *）
                lastUnitPrice = periodUnit;
                lastSourcePeriod = period;
                if (!string.IsNullOrWhiteSpace(picked.CurrencyCode))
                {
                    lastCurrency = picked.CurrencyCode.Trim();
                }
            }
            // 无历史价：展示 0（有机种使用但无移动价仍需出列）
            if (!lastUnitPrice.HasValue || string.IsNullOrWhiteSpace(lastSourcePeriod))
            {
                periodUnitPrices[period] = 0m;
                periodPriceSourcePeriods[period] = period;
                continue;
            }
            // 当月无行或无正价：沿用最近有价月；来源月≠展示月 → 前端标 *
            periodUnitPrices[period] = lastUnitPrice.Value;
            periodPriceSourcePeriods[period] = lastSourcePeriod;
            if (string.IsNullOrWhiteSpace(currency) && !string.IsNullOrWhiteSpace(lastCurrency))
            {
                currency = lastCurrency;
            }
        }
        var row = new TaktMaterialMovingTrendDto
        {
            PlantCode = plantCode,
            MaterialCode = materialCode,
            MaterialName = materialName,
            Valuation = valuation,
            CurrencyCode = currency,
            PeriodUnitPrices = periodUnitPrices,
            PeriodPriceSourcePeriods = periodPriceSourcePeriods,
        };
        ApplyFocusTrend(row, focusPeriod);
        return row;
    }

    /// <summary>
    /// PeriodDate → yyyy-MM（按年月，兼容月末 2023-04-30）
    /// </summary>
    /// <param name="periodDate">期间日期</param>
    /// <returns>yyyy-MM</returns>
    private static string ToPeriodKey(DateTime periodDate) =>
        new DateTime(periodDate.Year, periodDate.Month, 1).ToString("yyyy-MM");

    /// <summary>
    /// 展示用评估类别：优先关注月选中行，否则取最近一期有评估类别的行
    /// </summary>
    /// <param name="materialRows">同一物料全部期间行</param>
    /// <param name="focusPeriod">关注期间 yyyy-MM</param>
    /// <returns>评估类别</returns>
    private static string ResolveDisplayValuation(
        IReadOnlyList<TaktMaterialMovingPrice> materialRows,
        string? focusPeriod)
    {
        if (!string.IsNullOrWhiteSpace(focusPeriod))
        {
            var focused = PickForPeriod(materialRows, focusPeriod.Trim());
            if (!string.IsNullOrWhiteSpace(focused?.Valuation))
            {
                return focused.Valuation.Trim();
            }
        }
        return materialRows
            .OrderByDescending(r => r.ValuationPeriod)
            .Select(r => r.Valuation?.Trim())
            .FirstOrDefault(v => !string.IsNullOrWhiteSpace(v))
            ?? string.Empty;
    }

    /// <summary>
    /// 选取某期间价格行（同月多行：V 优先、库存数量较大优先；可跨评估类别）
    /// </summary>
    private static TaktMaterialMovingPrice? PickForPeriod(
        IReadOnlyList<TaktMaterialMovingPrice> materialRows,
        string periodKey)
    {
        return materialRows
            .Where(r => NormalizeYm(r.ValuationPeriod) == periodKey)
            .OrderByDescending(r => string.Equals(r.PriceControl, "V", StringComparison.OrdinalIgnoreCase))
            .ThenByDescending(r => r.StockQuantity)
            .ThenBy(r => r.Valuation, StringComparer.OrdinalIgnoreCase)
            .FirstOrDefault();
    }

    /// <summary>
    /// 选取严格早于/不晚于目标月、且有正移动价的最近有价行（用于区间前种子与缺月回填）
    /// </summary>
    private static TaktMaterialMovingPrice? PickMostRecentPositiveOnOrBefore(
        IReadOnlyList<TaktMaterialMovingPrice> materialRows,
        string periodKey,
        bool exclusive)
    {
        if (!DateTime.TryParseExact(
                periodKey + "-01",
                "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out var targetMonth))
        {
            return null;
        }
        var candidates = materialRows
            .Select(r => (Row: r, Month: ParseYmToDateRequired(r.ValuationPeriod)))
            .Where(x => exclusive ? x.Month < targetMonth : x.Month <= targetMonth)
            .Where(x => TryResolveTrendUnitPrice(x.Row, out _))
            .ToList();
        if (candidates.Count == 0)
        {
            return null;
        }
        var latestMonth = candidates.Max(x => x.Month);
        return candidates
            .Where(x => x.Month == latestMonth)
            .Select(x => x.Row)
            .OrderByDescending(r => string.Equals(r.PriceControl, "V", StringComparison.OrdinalIgnoreCase))
            .ThenByDescending(r => r.StockQuantity)
            .First();
    }

    /// <summary>
    /// 解析推移展示单价：MovingPrice&gt;0 视为有价；单价=MovingPrice/PriceUnit（过大单位导致四舍五入为 0 时回退 MovingPrice）
    /// </summary>
    /// <param name="row">价格行</param>
    /// <param name="unitPrice">展示单价</param>
    /// <returns>是否有价</returns>
    private static bool TryResolveTrendUnitPrice(TaktMaterialMovingPrice row, out decimal unitPrice)
    {
        ArgumentNullException.ThrowIfNull(row);
        unitPrice = 0m;
        if (row.MovingPrice <= 0m)
        {
            return false;
        }
        var unit = row.PriceUnit <= 0 ? 1 : row.PriceUnit;
        unitPrice = RoundUnitPrice(row.MovingPrice / unit);
        if (unitPrice <= 0m)
        {
            unitPrice = RoundUnitPrice(row.MovingPrice);
        }
        return unitPrice > 0m;
    }

    /// <summary>
    /// 按关注期间应用环比（基准月/对比月单价任一为 0 → 差额为 0、涨跌 flat）
    /// </summary>
    private static void ApplyFocusTrend(TaktMaterialMovingTrendDto row, string? focusPeriod)
    {
        if (string.IsNullOrWhiteSpace(focusPeriod))
        {
            return;
        }
        var comparePeriod = focusPeriod.Trim();
        if (!DateTime.TryParseExact(
                comparePeriod + "-01",
                "yyyy-MM-dd",
                null,
                System.Globalization.DateTimeStyles.None,
                out var compareMonth))
        {
            return;
        }
        var basePeriod = compareMonth.AddMonths(-1).ToString("yyyy-MM");
        row.BasePeriod = basePeriod;
        row.ComparePeriod = comparePeriod;
        if (!row.PeriodUnitPrices.TryGetValue(basePeriod, out var basePrice)
            || !row.PeriodUnitPrices.TryGetValue(comparePeriod, out var comparePrice))
        {
            row.Trend = "none";
            return;
        }
        // 基准月或对比月单价为 0：环比差额强制 0（不参与涨跌）
        if (basePrice == 0m || comparePrice == 0m)
        {
            row.VarianceAmount = 0m;
            row.VariancePercent = null;
            row.Trend = "flat";
            return;
        }
        row.VarianceAmount = RoundUnitPrice(comparePrice - basePrice);
        // 小数比率（非百分数）：0.2978 → Excel 百分比列显示 29.78%
        row.VariancePercent = Math.Round(
            row.VarianceAmount.Value / basePrice,
            4,
            MidpointRounding.AwayFromZero);
        if (comparePrice > basePrice)
        {
            row.Trend = "up";
        }
        else if (comparePrice < basePrice)
        {
            row.Trend = "down";
        }
        else
        {
            row.Trend = "flat";
        }
    }

    /// <summary>
    /// 涨跌筛选
    /// </summary>
    /// <param name="rows">全量行</param>
    /// <param name="trendFilter">筛选码：空/all/leading=不按涨跌码过滤；up/down/flat/none/changed</param>
    /// <returns>筛选后行</returns>
    private static List<TaktMaterialMovingTrendDto> FilterTrendRows(
        IReadOnlyList<TaktMaterialMovingTrendDto> rows,
        string? trendFilter)
    {
        if (string.IsNullOrWhiteSpace(trendFilter))
        {
            return rows.ToList();
        }
        var filter = trendFilter.Trim().ToLowerInvariant();
        if (filter is "all" or "leading")
        {
            return rows.ToList();
        }
        if (filter == "changed")
        {
            return rows.Where(r => r.Trend is "up" or "down").ToList();
        }
        return rows.Where(r => string.Equals(r.Trend, filter, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    /// <summary>
    /// 涨跌优先排序
    /// </summary>
    private static List<TaktMaterialMovingTrendDto> OrderTrendRows(
        IReadOnlyList<TaktMaterialMovingTrendDto> rows)
    {
        static int TrendRank(string? trend) => trend switch
        {
            "up" => 0,
            "down" => 1,
            "flat" => 2,
            _ => 3,
        };
        return rows
            .OrderBy(r => TrendRank(r.Trend))
            .ThenByDescending(r => Math.Abs(r.VarianceAmount ?? 0m))
            .ThenBy(r => r.MaterialCode, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>
    /// 归一化期间起止到月初
    /// </summary>
    private static (DateTime? Start, DateTime? End) NormalizePeriodBounds(DateTime? periodDateStart, DateTime? periodDateEnd)
    {
        DateTime? start = periodDateStart.HasValue
            ? new DateTime(periodDateStart.Value.Year, periodDateStart.Value.Month, 1)
            : null;
        DateTime? end = periodDateEnd.HasValue
            ? new DateTime(periodDateEnd.Value.Year, periodDateEnd.Value.Month, 1)
            : null;
        if (start.HasValue && end.HasValue && start > end)
        {
            (start, end) = (end, start);
        }
        return (start, end);
    }

    /// <summary>
    /// 构建期间列顺序
    /// </summary>
    private static List<string> BuildPeriodOrder(
        IReadOnlyList<TaktMaterialMovingPrice> priceRows,
        DateTime? periodStart,
        DateTime? periodEnd)
    {
        if (periodStart.HasValue && periodEnd.HasValue)
        {
            var order = new List<string>();
            for (var cursor = periodStart.Value; cursor <= periodEnd.Value; cursor = cursor.AddMonths(1))
            {
                order.Add(cursor.ToString("yyyy-MM"));
            }
            return order;
        }
        return priceRows
            .Select(r => ParseYmToDateRequired(r.ValuationPeriod).ToString("yyyy-MM"))
            .Distinct(StringComparer.Ordinal)
            .OrderBy(p => p, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>
    /// 解析关注期间
    /// </summary>
    private static string? ResolveFocusPeriod(string? focusPeriod, IReadOnlyList<string> periodOrder)
    {
        if (!string.IsNullOrWhiteSpace(focusPeriod))
        {
            return focusPeriod.Trim();
        }
        return periodOrder.Count > 0 ? periodOrder[^1] : null;
    }

    /// <summary>
    /// 单价四舍五入至 5 位
    /// </summary>
    private static decimal RoundUnitPrice(decimal value) =>
        Math.Round(value, 5, MidpointRounding.AwayFromZero);

    /// <summary>
    /// 物料月推移内存构建结果
    /// </summary>
    private sealed class MonthlyTrendAnalysisBuilt
    {
        /// <summary>
        /// 过滤并排序后的全量行
        /// </summary>
        public List<TaktMaterialMovingTrendDto> OrderedRows { get; init; } = new();

        /// <summary>
        /// 期间列顺序
        /// </summary>
        public List<string> PeriodOrder { get; init; } = new();

        /// <summary>
        /// 基准期间
        /// </summary>
        public string? BasePeriod { get; init; }

        /// <summary>
        /// 对比期间
        /// </summary>
        public string? ComparePeriod { get; init; }

        /// <summary>
        /// 上涨行数
        /// </summary>
        public int UpCount { get; init; }

        /// <summary>
        /// 下跌行数
        /// </summary>
        public int DownCount { get; init; }

        /// <summary>
        /// 持平行数
        /// </summary>
        public int FlatCount { get; init; }

        /// <summary>
        /// 无趋势行数
        /// </summary>
        public int NoneCount { get; init; }

        /// <summary>
        /// 空结果
        /// </summary>
        public static MonthlyTrendAnalysisBuilt Empty() => new();
    }

    /// <summary>
    /// BOM 物料使用信息（产品组 / 机种组）
    /// </summary>
    private sealed class BomMaterialUsageInfo
    {
        /// <summary>
        /// 产品编码列表
        /// </summary>
        public List<string> ProductCodes { get; init; } = new();

        /// <summary>
        /// 机种编码列表
        /// </summary>
        public List<string> ModelCodes { get; init; } = new();

        /// <summary>
        /// 组件描述（物料描述回退）
        /// </summary>
        public string ComponentDescription { get; init; } = string.Empty;
    }

    // ========================================
    // 按年分表路由（{base}_{yyyy}）
    // ========================================

    private static string BuildMovingPriceYearTable(int year) =>
        TaktYearShardTableHelper.BuildYearTableName(MovingPriceYearShardBaseTable, year);

    /// <summary>
    /// 解析移动价格物理表：年分表存在则用之，否则 null（回退基表）
    /// </summary>
    /// <param name="year">自然年</param>
    /// <returns>年分表名；不存在时为 null</returns>
    private async Task<string?> ResolveMovingPricePhysicalTableAsync(int year)
    {
        var table = BuildMovingPriceYearTable(year);
        return await _materialMovingPriceRepository.PhysicalTableExistsAsync(table) ? table : null;
    }

    /// <summary>
    /// 解析评估期间年份（yyyy-MM）
    /// </summary>
    /// <param name="valuationPeriod">评估期间</param>
    /// <returns>年份</returns>
    private static int ParseValuationPeriodYear(string? valuationPeriod)
    {
        var ym = NormalizeYm(valuationPeriod);
        if (ym.Length < 4 || !int.TryParse(ym.AsSpan(0, 4), out var year))
        {
            throw new TaktBusinessException("评估期间格式无效，须为 yyyy-MM");
        }
        return year;
    }

    /// <summary>
    /// 规范化 yyyy-MM
    /// </summary>
    /// <param name="valuationPeriod">评估期间</param>
    /// <returns>yyyy-MM</returns>
    private static string NormalizeYm(string? valuationPeriod)
    {
        var v = (valuationPeriod ?? string.Empty).Trim();
        return v.Length >= 7 ? v.Substring(0, 7) : v;
    }

    /// <summary>
    /// yyyy-MM → 当月首日
    /// </summary>
    /// <param name="valuationPeriod">评估期间</param>
    /// <returns>当月首日</returns>
    private static DateTime ParseYmToDateRequired(string? valuationPeriod)
    {
        var ym = NormalizeYm(valuationPeriod);
        if (!DateTime.TryParseExact(ym + "-01", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out var d))
        {
            throw new TaktBusinessException("评估期间格式无效，须为 yyyy-MM");
        }
        return d;
    }

    /// <summary>
    /// 按年分表查询移动价格（可跨年合并；年分表未建时回退基表）
    /// </summary>
    /// <param name="predicate">条件</param>
    /// <param name="start">起</param>
    /// <param name="end">止</param>
    /// <param name="maxRows">总行上限</param>
    /// <returns>列表</returns>
    private async Task<List<TaktMaterialMovingPrice>> GetMovingPriceListForRangeAsync(
        Expression<Func<TaktMaterialMovingPrice, bool>> predicate,
        DateTime? start,
        DateTime? end,
        int? maxRows = null)
    {
        var years = TaktYearShardTableHelper.ResolveYears(start, end);
        var result = new List<TaktMaterialMovingPrice>();
        var yearsNeedBase = new List<int>();
        foreach (var year in years)
        {
            var table = await ResolveMovingPricePhysicalTableAsync(year);
            if (table == null)
            {
                yearsNeedBase.Add(year);
                continue;
            }
            if (maxRows.HasValue)
            {
                var remaining = maxRows.Value - result.Count;
                if (remaining <= 0)
                {
                    break;
                }
                var part = await _materialMovingPriceRepository.GetListForExportAsync(predicate, remaining, table);
                result.AddRange(part);
            }
            else
            {
                var part = await _materialMovingPriceRepository.GetListAsync(predicate, table);
                result.AddRange(part);
            }
        }
        if (yearsNeedBase.Count == 0)
        {
            return result;
        }
        if (maxRows.HasValue && result.Count >= maxRows.Value)
        {
            return result;
        }
        List<TaktMaterialMovingPrice> basePart;
        if (maxRows.HasValue)
        {
            var remaining = maxRows.Value - result.Count;
            basePart = await _materialMovingPriceRepository.GetListForExportAsync(predicate, remaining);
        }
        else
        {
            basePart = await _materialMovingPriceRepository.GetListAsync(predicate);
        }
        if (yearsNeedBase.Count == years.Count)
        {
            result.AddRange(basePart);
        }
        else
        {
            var yearSet = yearsNeedBase.ToHashSet();
            result.AddRange(basePart.Where(r => yearSet.Contains(ParseValuationPeriodYear(r.ValuationPeriod))));
        }
        return result;
    }
}
