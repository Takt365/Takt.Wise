// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBomCostOptionService.cs
// 创建时间：2026-08-21
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 成本查询栏共用选项（工厂 / 期间 / 机种 / 产品 / 物料；仅 IsDeleted=0）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Globalization;
using System.Text;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Options;
using Takt.Shared.Validation;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 成本查询栏共用选项（机种推移 / 差异推移 / 零价格）
/// </summary>
public class TaktBomCostOptionService : TaktServiceBase, ITaktBomCostOptionService
{
    /// <summary>
    /// BOM 成本明细按年分表基表名
    /// </summary>
    private const string BomItemYearShardBaseTable = "takt_logistics_manufacturing_bom_material_cost_item";
    private readonly ITaktCompanyRepository<TaktBomMaterialCostItem> _bomMaterialCostItemRepository;
    private readonly ITaktCompanyRepository<TaktBomMaterialCost> _bomMaterialCostRepository;
    private readonly ITaktTenantRepository<TaktModelDestination> _modelDestinationRepository;
    private readonly ITaktTenantRepository<TaktCompany> _companyRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bomMaterialCostItemRepository">BOM 成本明细仓储</param>
    /// <param name="bomMaterialCostRepository">BOM 成本汇总仓储</param>
    /// <param name="modelDestinationRepository">型号目的地仓储（机种名）</param>
    /// <param name="companyRepository">公司仓储（RelatedPlant）</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktBomCostOptionService(
        ITaktCompanyRepository<TaktBomMaterialCostItem> bomMaterialCostItemRepository,
        ITaktCompanyRepository<TaktBomMaterialCost> bomMaterialCostRepository,
        ITaktTenantRepository<TaktModelDestination> modelDestinationRepository,
        ITaktTenantRepository<TaktCompany> companyRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _bomMaterialCostItemRepository = bomMaterialCostItemRepository;
        _bomMaterialCostRepository = bomMaterialCostRepository;
        _modelDestinationRepository = modelDestinationRepository;
        _companyRepository = companyRepository;
    }

    /// <summary>
    /// 工厂选项：当前公司 RelatedPlant ∩ 头表未删除 PlantCode
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetBomCostOptionPlantOptionsAsync(string? plantCode = null, string? keyword = null)
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
        var exists = await _bomMaterialCostRepository.ExistsAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == relatedPlant
            && x.IsDeleted == 0);
        if (!exists)
        {
            return new List<TaktSelectOption>();
        }
        return new List<TaktSelectOption>
        {
            new() { DictValue = relatedPlant, DictLabel = relatedPlant },
        };
    }

    /// <summary>
    /// 物料类型去重（头表；工厂+期间；仅未删除）
    /// </summary>
    /// <param name="queryDto">工厂 + 期间</param>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>物料类型选项</returns>
    public async Task<List<TaktSelectOption>> GetBomCostOptionMaterialTypeOptionsAsync(string? plantCode = null, string? keyword = null, TaktBomCostOptionDto? queryDto = null)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var headers = await LoadHeaderOptionsAsync(queryDto, requireModelCode: false, requireProductCode: false);
        return headers
            .Select(e => e.MaterialType.Trim())
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(t => t, StringComparer.Ordinal)
            .Select(t => new TaktSelectOption { DictValue = t, DictLabel = t })
            .ToList();
    }

    /// <summary>
    /// 机种去重（头表 ModelCode；工厂+期间；仅未删除）
    /// </summary>
    /// <param name="queryDto">工厂 + 期间；MaterialType 可选</param>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>机种选项</returns>
    public async Task<List<TaktSelectOption>> GetBomCostOptionModelOptionsAsync(string? plantCode = null, string? keyword = null, TaktBomCostOptionDto? queryDto = null)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var kw = keyword?.Trim();
        if (string.IsNullOrEmpty(kw))
        {
            kw = queryDto.Keyword?.Trim();
        }
        var modelNameLookup = await BuildModelNameLookupAsync();
        IReadOnlyList<string>? nameCodes = null;
        if (!string.IsNullOrEmpty(kw))
        {
            nameCodes = modelNameLookup
                .Where(kv =>
                    kv.Key.Contains(kw, StringComparison.OrdinalIgnoreCase)
                    || kv.Value.Contains(kw, StringComparison.OrdinalIgnoreCase))
                .Select(kv => kv.Key)
                .ToList();
        }
        var headers = await LoadHeaderOptionsAsync(
            queryDto,
            requireModelCode: true,
            requireProductCode: false,
            modelKeyword: kw,
            modelCodesMatchingName: nameCodes);
        return headers
            .GroupBy(e => e.ModelCode!.Trim(), StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g =>
            {
                var modelCode = g.Key;
                var label = modelNameLookup.TryGetValue(modelCode, out var modelName)
                    && !string.IsNullOrWhiteSpace(modelName)
                    ? modelName
                    : modelCode;
                return new TaktSelectOption { DictValue = modelCode, DictLabel = label };
            })
            .ToList();
    }

    /// <summary>
    /// 产品去重（头表 ProductCode；工厂+期间；仅未删除）
    /// </summary>
    /// <param name="queryDto">工厂 + 期间；MaterialType/ModelCode 可选</param>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>产品选项</returns>
    public async Task<List<TaktSelectOption>> GetBomCostOptionProductOptionsAsync(string? plantCode = null, string? keyword = null, TaktBomCostOptionDto? queryDto = null)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var headers = await LoadHeaderOptionsAsync(queryDto, requireModelCode: false, requireProductCode: true);
        return headers
            .GroupBy(e => e.ProductCode!.Trim(), StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g =>
            {
                var first = g.OrderByDescending(x => x.CostingDate).First();
                var description = first.ProductDescription?.Trim();
                var label = string.IsNullOrWhiteSpace(description) ? g.Key : $"{g.Key} - {description}";
                return new TaktSelectOption
                {
                    DictValue = g.Key,
                    DictLabel = label,
                    ExtValue = first.ModelCode?.Trim() ?? string.Empty,
                    ExtLabel = first.MaterialType?.Trim() ?? string.Empty,
                };
            })
            .ToList();
    }

    /// <summary>
    /// 物料/组件去重（明细表；工厂+期间；X+F+未删除；keyword 远程）
    /// 机种/产品可空：空则不过滤；有值则经头表产品编码再过滤明细
    /// </summary>
    /// <param name="queryDto">工厂 + 期间；ModelCode/ModelCodes/ProductCode/Keyword 均可空</param>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>物料选项</returns>
    public async Task<List<TaktSelectOption>> GetBomCostOptionMaterialOptionsAsync(string? plantCode = null, string? keyword = null, TaktBomCostOptionDto? queryDto = null)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var plant = queryDto.PlantCode?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(plant)
            || !TryResolveOptionsPeriod(queryDto, out _, out _, out var rangeStart, out var rangeEndExclusive))
        {
            return new List<TaktSelectOption>();
        }
        var models = ParseModelFilters(queryDto);
        var productFilter = queryDto.ProductCode?.Trim();
        List<string>? allowedProducts = null;
        if (models.Count > 0 || !string.IsNullOrEmpty(productFilter))
        {
            var headers = await LoadHeaderOptionsAsync(queryDto, requireModelCode: models.Count > 0, requireProductCode: true);
            IEnumerable<TaktBomMaterialCost> scoped = headers;
            if (!string.IsNullOrEmpty(productFilter))
            {
                scoped = scoped.Where(h =>
                    string.Equals(h.ProductCode?.Trim(), productFilter, StringComparison.OrdinalIgnoreCase));
            }
            allowedProducts = scoped
                .Select(h => h.ProductCode!.Trim())
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
            if (allowedProducts.Count == 0)
            {
                return new List<TaktSelectOption>();
            }
        }
        var years = TaktYearShardTableHelper.ResolveYears(rangeStart, rangeEndExclusive.AddDays(-1));
        var queriedTables = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var year in years)
        {
            var table = await ResolveBomItemPhysicalTableAsync(year) ?? BomItemYearShardBaseTable;
            if (!queriedTables.Add(table))
            {
                continue;
            }
            await MergeComponentOptionsAsync(
                map, table, plant, rangeStart, rangeEndExclusive, queryDto.Keyword, allowedProducts);
        }
        if (queriedTables.Add(BomItemYearShardBaseTable))
        {
            await MergeComponentOptionsAsync(
                map, BomItemYearShardBaseTable, plant, rangeStart, rangeEndExclusive, queryDto.Keyword, allowedProducts);
        }
        return map
            .OrderBy(kv => kv.Key, StringComparer.Ordinal)
            .Select(kv => new TaktSelectOption
            {
                DictValue = kv.Key,
                DictLabel = string.IsNullOrWhiteSpace(kv.Value) ? kv.Key : $"{kv.Key} {kv.Value}",
            })
            .ToList();
    }

    /// <summary>
    /// 头表选项公共加载：期间合集 = 核算日落在期间内且 IsDeleted=0 的主表并集（不用 CostingPeriod 字符串）
    /// </summary>
    /// <param name="queryDto">选项查询</param>
    /// <param name="requireModelCode">是否要求 ModelCode 非空</param>
    /// <param name="requireProductCode">是否要求 ProductCode 非空</param>
    /// <param name="modelKeyword">机种远程搜索；空则不按关键字收窄</param>
    /// <param name="modelCodesMatchingName">名称命中的机种编码（与关键字 OR）</param>
    /// <returns>未删除头表行</returns>
    private async Task<List<TaktBomMaterialCost>> LoadHeaderOptionsAsync(
        TaktBomCostOptionDto queryDto,
        bool requireModelCode,
        bool requireProductCode,
        string? modelKeyword = null,
        IReadOnlyList<string>? modelCodesMatchingName = null)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var plant = queryDto.PlantCode?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(plant)
            || !TryResolveOptionsPeriod(queryDto, out _, out _, out var rangeStart, out var rangeEndExclusive))
        {
            return new List<TaktBomMaterialCost>();
        }
        var materialType = NormalizeMaterialTypeFilter(queryDto.MaterialType);
        var models = ParseModelFilters(queryDto);
        var exp = Expressionable.Create<TaktBomMaterialCost>();
        // 期间合集 = 核算日落在 [起, 止月下一月) 且未软删的主表并集。
        // 禁止用 CostingPeriod 字符串区间：否则 6 无数据、7/8 已删时，仍可能被脏 CostingPeriod 行混入。
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plant
            && x.IsDeleted == 0
            && x.CostingDate >= rangeStart
            && x.CostingDate < rangeEndExclusive);
        if (requireModelCode)
        {
            exp = exp.And(x => x.ModelCode != null && x.ModelCode != string.Empty);
        }
        if (requireProductCode)
        {
            exp = exp.And(x => x.ProductCode != null && x.ProductCode != string.Empty);
        }
        if (materialType != null)
        {
            var type = materialType;
            exp = exp.And(x => x.MaterialType == type);
        }
        if (models.Count == 1)
        {
            var modelCode = models[0];
            exp = exp.And(x => x.ModelCode == modelCode);
        }
        else if (models.Count > 1)
        {
            exp = exp.And(x => models.Contains(x.ModelCode));
        }
        var kw = modelKeyword?.Trim();
        if (!string.IsNullOrEmpty(kw))
        {
            var nameCodes = modelCodesMatchingName?.ToList() ?? new List<string>();
            if (nameCodes.Count == 0)
            {
                exp = exp.And(x => x.ModelCode != null && x.ModelCode.Contains(kw));
            }
            else
            {
                exp = exp.And(x =>
                    x.ModelCode != null
                    && (x.ModelCode.Contains(kw) || nameCodes.Contains(x.ModelCode)));
            }
        }
        return await _bomMaterialCostRepository.GetListAsync(exp.ToExpression());
    }

    /// <summary>
    /// 解析机种过滤（ModelCode + ModelCodes；空=不过滤）
    /// </summary>
    /// <param name="queryDto">选项查询</param>
    /// <returns>机种编码列表</returns>
    private static List<string> ParseModelFilters(TaktBomCostOptionDto queryDto)
    {
        var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        AddTrimmedCode(set, queryDto.ModelCode);
        if (!string.IsNullOrWhiteSpace(queryDto.ModelCodes))
        {
            foreach (var part in queryDto.ModelCodes.Split(
                         ',',
                         StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
            {
                AddTrimmedCode(set, part);
            }
        }
        return set.ToList();
    }

    /// <summary>
    /// 写入非空编码
    /// </summary>
    /// <param name="set">集合</param>
    /// <param name="value">编码</param>
    private static void AddTrimmedCode(HashSet<string> set, string? value)
    {
        var trimmed = value?.Trim();
        if (!string.IsNullOrEmpty(trimmed))
        {
            set.Add(trimmed);
        }
    }

    /// <summary>
    /// 规范化物料类型筛选（空=不按类型过滤）
    /// </summary>
    /// <param name="materialType">查询传入类型</param>
    /// <returns>非空类型码；空则 null</returns>
    private static string? NormalizeMaterialTypeFilter(string? materialType)
    {
        var trimmed = materialType?.Trim();
        return string.IsNullOrEmpty(trimmed) ? null : trimmed;
    }

    /// <summary>
    /// 解析选项期间（yyyy-MM；单月时起止相同）
    /// </summary>
    /// <param name="queryDto">选项查询</param>
    /// <param name="periodStart">期间起 yyyy-MM</param>
    /// <param name="periodEnd">期间止 yyyy-MM</param>
    /// <param name="rangeStart">起月首日</param>
    /// <param name="rangeEndExclusive">止月下一月首日</param>
    /// <returns>是否解析成功</returns>
    private static bool TryResolveOptionsPeriod(
        TaktBomCostOptionDto queryDto,
        out string periodStart,
        out string periodEnd,
        out DateTime rangeStart,
        out DateTime rangeEndExclusive)
    {
        periodStart = string.Empty;
        periodEnd = string.Empty;
        rangeStart = default;
        rangeEndExclusive = default;
        var startRaw = queryDto.PeriodStart?.Trim();
        var endRaw = queryDto.PeriodEnd?.Trim();
        if (string.IsNullOrEmpty(startRaw))
        {
            startRaw = endRaw;
        }
        if (string.IsNullOrEmpty(endRaw))
        {
            endRaw = startRaw;
        }
        if (!TryParseYearMonth(startRaw, out var startMonth) || !TryParseYearMonth(endRaw, out var endMonth))
        {
            return false;
        }
        if (startMonth > endMonth)
        {
            (startMonth, endMonth) = (endMonth, startMonth);
        }
        periodStart = startMonth.ToString("yyyy-MM");
        periodEnd = endMonth.ToString("yyyy-MM");
        rangeStart = startMonth;
        rangeEndExclusive = endMonth.AddMonths(1);
        return true;
    }

    /// <summary>
    /// 解析 yyyy-MM 为当月首日
    /// </summary>
    /// <param name="value">yyyy-MM</param>
    /// <param name="monthStart">月初</param>
    /// <returns>是否成功</returns>
    private static bool TryParseYearMonth(string? value, out DateTime monthStart)
    {
        monthStart = default;
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }
        if (!DateTime.TryParseExact(
                value.Trim(),
                "yyyy-MM",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var parsed))
        {
            return false;
        }
        monthStart = new DateTime(parsed.Year, parsed.Month, 1);
        return true;
    }

    /// <summary>
    /// 构建机种名称查找表（型号目的地 ModelCode → ModelName）
    /// </summary>
    /// <returns>机种名称字典</returns>
    private async Task<Dictionary<string, string>> BuildModelNameLookupAsync()
    {
        var list = await _modelDestinationRepository.GetListAsync(x => x.TenantCode == CurrentTenantCode);
        return list
            .Where(x => !string.IsNullOrWhiteSpace(x.ModelCode))
            .GroupBy(x => x.ModelCode!.Trim(), StringComparer.OrdinalIgnoreCase)
            .ToDictionary(
                g => g.Key,
                g => g.First().ModelName?.Trim() ?? g.Key,
                StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 合并指定物理表的组件去重结果
    /// </summary>
    /// <param name="map">组件编码→描述</param>
    /// <param name="tableName">物理表名</param>
    /// <param name="plantCode">工厂</param>
    /// <param name="rangeStart">核算日起</param>
    /// <param name="rangeEndExclusive">核算日止（不含）</param>
    /// <param name="keyword">关键字</param>
    /// <param name="allowedProducts">可选产品编码（机种/产品可空过滤后的集合；空=不过滤产品）</param>
    /// <returns>异步完成</returns>
    private async Task MergeComponentOptionsAsync(
        Dictionary<string, string> map,
        string tableName,
        string plantCode,
        DateTime rangeStart,
        DateTime rangeEndExclusive,
        string? keyword,
        IReadOnlyList<string>? allowedProducts)
    {
        if (allowedProducts == null)
        {
            await MergeComponentChunkAsync(
                map, tableName, plantCode, rangeStart, rangeEndExclusive, keyword, null);
            return;
        }
        if (allowedProducts.Count == 0)
        {
            return;
        }
        const int chunkSize = 200;
        for (var offset = 0; offset < allowedProducts.Count; offset += chunkSize)
        {
            var take = Math.Min(chunkSize, allowedProducts.Count - offset);
            var chunk = allowedProducts.Skip(offset).Take(take).ToList();
            await MergeComponentChunkAsync(
                map, tableName, plantCode, rangeStart, rangeEndExclusive, keyword, chunk);
        }
    }

    /// <summary>
    /// 合并一组分表查询结果
    /// </summary>
    /// <param name="map">组件编码→描述</param>
    /// <param name="tableName">物理表名</param>
    /// <param name="plantCode">工厂</param>
    /// <param name="rangeStart">核算日起</param>
    /// <param name="rangeEndExclusive">核算日止（不含）</param>
    /// <param name="keyword">关键字</param>
    /// <param name="productCodes">产品编码块；null=不过滤</param>
    /// <returns>异步完成</returns>
    private async Task MergeComponentChunkAsync(
        Dictionary<string, string> map,
        string tableName,
        string plantCode,
        DateTime rangeStart,
        DateTime rangeEndExclusive,
        string? keyword,
        IReadOnlyList<string>? productCodes)
    {
        var rows = await QueryDistinctComponentOptionsAsync(
            tableName, plantCode, rangeStart, rangeEndExclusive, keyword, productCodes);
        foreach (var (code, description) in rows)
        {
            if (!map.ContainsKey(code) || string.IsNullOrWhiteSpace(map[code]))
            {
                map[code] = description;
            }
        }
    }

    /// <summary>
    /// 指定物理表查询组件去重（期间内 X+F+未删除）
    /// </summary>
    /// <param name="tableName">物理表名</param>
    /// <param name="plantCode">工厂</param>
    /// <param name="rangeStart">核算日起</param>
    /// <param name="rangeEndExclusive">核算日止（不含）</param>
    /// <param name="keyword">关键字</param>
    /// <param name="productCodes">产品编码过滤；null 或空=不过滤</param>
    /// <returns>组件编码+描述</returns>
    private async Task<List<(string Code, string Description)>> QueryDistinctComponentOptionsAsync(
        string tableName,
        string plantCode,
        DateTime rangeStart,
        DateTime rangeEndExclusive,
        string? keyword,
        IReadOnlyList<string>? productCodes)
    {
        var kw = keyword?.Trim();
        var sql = new StringBuilder();
        sql.Append(
            """
            SELECT
              LTRIM(RTRIM(component_code)) AS ComponentCode,
              MAX(LTRIM(RTRIM(ISNULL(component_description, N'')))) AS ComponentDescription
            FROM 
            """);
        sql.Append(tableName.Trim());
        sql.Append(
            """
             WHERE is_deleted = 0
              AND tenant_code = @tenantCode
              AND company_code = @companyCode
              AND plant_code = @plantCode
              AND production_related = N'X'
              AND purchase_type = N'F'
              AND UPPER(LTRIM(RTRIM(ISNULL(pcb_sect_indicator, N'')))) <> N'X'
              AND component_code IS NOT NULL
              AND LTRIM(RTRIM(component_code)) <> N''
              AND costing_date >= @costingStart
              AND costing_date < @costingEndExclusive
            """);
        var parameters = new Dictionary<string, object?>
        {
            ["tenantCode"] = CurrentTenantCode,
            ["companyCode"] = CurrentCompanyCode,
            ["plantCode"] = plantCode,
            ["costingStart"] = rangeStart,
            ["costingEndExclusive"] = rangeEndExclusive,
        };
        if (productCodes is { Count: > 0 })
        {
            var names = new List<string>(productCodes.Count);
            for (var i = 0; i < productCodes.Count; i++)
            {
                var name = "pc" + i.ToString(CultureInfo.InvariantCulture);
                names.Add("@" + name);
                parameters[name] = productCodes[i];
            }
            sql.Append(" AND LTRIM(RTRIM(product_code)) IN (");
            sql.Append(string.Join(", ", names));
            sql.Append(')');
        }
        if (!string.IsNullOrEmpty(kw))
        {
            sql.Append(
                """
                  AND (
                    component_code LIKE @keywordLike
                    OR component_description LIKE @keywordLike
                  )
                """);
            parameters["keywordLike"] = "%" + EscapeSqlLikePattern(kw) + "%";
        }
        sql.Append(
            """
             GROUP BY LTRIM(RTRIM(component_code))
            ORDER BY ComponentCode
            """);
        var script = sql.ToString();
        TaktSqlExecutorValidator.Validate(script);
        var raw = await _bomMaterialCostItemRepository.QueryReadOnlySqlAsync(script, parameters);
        var result = new List<(string Code, string Description)>(raw.Count);
        foreach (var row in raw)
        {
            var code = ReadSqlString(row, "ComponentCode");
            if (string.IsNullOrWhiteSpace(code))
            {
                continue;
            }
            result.Add((code, ReadSqlString(row, "ComponentDescription")));
        }
        return result;
    }

    /// <summary>
    /// 生成 BOM 成本明细年分表名
    /// </summary>
    /// <param name="year">年份</param>
    /// <returns>物理表名</returns>
    private static string BuildBomItemYearTable(int year) =>
        TaktYearShardTableHelper.BuildYearTableName(BomItemYearShardBaseTable, year);

    /// <summary>
    /// 解析 BOM 成本明细物理表：年分表存在则用之，否则 null
    /// </summary>
    /// <param name="year">自然年</param>
    /// <returns>年分表名；不存在时为 null</returns>
    private async Task<string?> ResolveBomItemPhysicalTableAsync(int year)
    {
        var table = BuildBomItemYearTable(year);
        return await _bomMaterialCostItemRepository.PhysicalTableExistsAsync(table) ? table : null;
    }

    /// <summary>
    /// 转义 LIKE 通配符
    /// </summary>
    /// <param name="value">原始关键字</param>
    /// <returns>转义后</returns>
    private static string EscapeSqlLikePattern(string value) =>
        value
            .Replace("[", "[[]", StringComparison.Ordinal)
            .Replace("%", "[%]", StringComparison.Ordinal)
            .Replace("_", "[_]", StringComparison.Ordinal);

    /// <summary>
    /// 读取只读 SQL 行字符串列
    /// </summary>
    /// <param name="row">行</param>
    /// <param name="column">列名</param>
    /// <returns>Trim 后文本</returns>
    private static string ReadSqlString(IReadOnlyDictionary<string, object> row, string column)
    {
        if (!row.TryGetValue(column, out var value) || value == null || value is DBNull)
        {
            return string.Empty;
        }
        return Convert.ToString(value)?.Trim() ?? string.Empty;
    }
}
