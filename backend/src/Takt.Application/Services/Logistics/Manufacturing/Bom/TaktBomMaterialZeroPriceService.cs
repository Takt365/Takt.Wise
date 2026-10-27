// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialZeroPriceService.cs
// 创建时间：2026-08-13
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 组件零价格清单（视图只含生产相关=X、PCB SECT 标识为空、采购类型=F、移动平均价=0；FERT+建议代替末字母逆推）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Application.Services.Foundation;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 组件零价格清单服务（与成本分析服务分离）
/// </summary>
public class TaktBomMaterialZeroPriceService : TaktServiceBase, ITaktBomMaterialZeroPriceService
{
    /// <summary>
    /// 零价合并/建议价加载行上限
    /// </summary>
    private const int MaxRowLoad = 20000;

    /// <summary>
    /// BOM 成本明细按年分表基表名
    /// </summary>
    private const string BomItemYearShardBaseTable = "takt_logistics_manufacturing_bom_material_cost_item";

    /// <summary>
    /// 移动价格按年分表基表名
    /// </summary>
    private const string MovingPriceYearShardBaseTable = "takt_logistics_materials_material_moving_price";

    private readonly ITaktCompanyRepository<TaktBomMaterialCostItem> _bomMaterialCostItemRepository;
    private readonly ITaktCompanyRepository<TaktBomMaterialCost> _bomMaterialCostRepository;
    private readonly ITaktCompanyRepository<TaktMaterialMovingPrice> _materialMovingPriceRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bomMaterialCostItemRepository">BOM 成本明细仓储</param>
    /// <param name="bomMaterialCostRepository">BOM 成本汇总仓储</param>
    /// <param name="materialMovingPriceRepository">移动价格仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktBomMaterialZeroPriceService(
        ITaktCompanyRepository<TaktBomMaterialCostItem> bomMaterialCostItemRepository,
        ITaktCompanyRepository<TaktBomMaterialCost> bomMaterialCostRepository,
        ITaktCompanyRepository<TaktMaterialMovingPrice> materialMovingPriceRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _bomMaterialCostItemRepository = bomMaterialCostItemRepository;
        _bomMaterialCostRepository = bomMaterialCostRepository;
        _materialMovingPriceRepository = materialMovingPriceRepository;
    }

    /// <summary>
    /// 组件零价格合并清单
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <returns>分页合并结果</returns>
    public async Task<TaktBomMaterialZeroPriceResultDto> GetBomMaterialZeroPriceListAsync(
        TaktBomMaterialZeroPriceQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);

        var (costingStart, costingEnd, costingPeriod) = PrepareCostingMonth(queryDto);
        var plantCode = queryDto.PlantCode.Trim();
        var modelFilters = ParseModelCodes(queryDto.ModelCodes, queryDto.ModelCode);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);

        var (productCodes, productToModels, allRows) = await BuildMergedRowsAsync(
            plantCode,
            modelFilters,
            costingStart,
            costingEnd,
            costingPeriod);

        if (allRows.Count == 0)
        {
            return new TaktBomMaterialZeroPriceResultDto
            {
                Paged = TaktPagedResult<TaktBomMaterialZeroPriceDto>.Create(
                    new List<TaktBomMaterialZeroPriceDto>(), 0, pageIndex, pageSize),
                ProductCodes = productCodes,
                ComponentCount = 0,
                CostingPeriod = costingPeriod,
            };
        }

        await FillSuggestedRevisionsAsync(plantCode, costingPeriod, allRows);

        var pageRows = allRows.Skip(skip).Take(pageSize).ToList();
        return new TaktBomMaterialZeroPriceResultDto
        {
            Paged = TaktPagedResult<TaktBomMaterialZeroPriceDto>.Create(
                pageRows, allRows.Count, pageIndex, pageSize),
            ProductCodes = productCodes,
            ComponentCount = allRows.Count,
            CostingPeriod = costingPeriod,
        };
    }

    /// <summary>
    /// 导出组件零价格合并清单
    /// </summary>
    /// <param name="query">查询</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportBomMaterialZeroPriceAsync(
        TaktBomMaterialZeroPriceQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentException.ThrowIfNullOrWhiteSpace(query.PlantCode);

        var (costingStart, costingEnd, costingPeriod) = PrepareCostingMonth(query);
        var plantCode = query.PlantCode.Trim();
        var modelFilters = ParseModelCodes(query.ModelCodes, query.ModelCode);
        var (_, _, allRows) = await BuildMergedRowsAsync(
            plantCode,
            modelFilters,
            costingStart,
            costingEnd,
            costingPeriod);
        await FillSuggestedRevisionsAsync(plantCode, costingPeriod, allRows);

        var columnKeys = new List<string>
        {
            "plantCode", "modelCode", "componentCode", "componentDescription",
            "productCodes", "productCount", "movingAveragePrice",
            "suggestedComponentCode", "suggestedMovingPrice", "costingPeriod",
        };
        var columnLabels = new List<string>
        {
            "工厂代码", "机种编码", "组件编码", "组件描述",
            "共用产品", "产品数", "移动平均价",
            "建议代替组件", "建议移动价格", "核算月",
        };
        var exportRows = allRows
            .Select(row => (IReadOnlyDictionary<string, object?>)new Dictionary<string, object?>(StringComparer.Ordinal)
            {
                ["plantCode"] = row.PlantCode,
                ["modelCode"] = row.ModelCode,
                ["componentCode"] = row.ComponentCode,
                ["componentDescription"] = row.ComponentDescription,
                ["productCodes"] = row.ProductCodes,
                ["productCount"] = row.ProductCount,
                ["movingAveragePrice"] = row.MovingAveragePrice,
                ["suggestedComponentCode"] = row.SuggestedComponentCode,
                ["suggestedMovingPrice"] = row.SuggestedMovingPrice,
                ["costingPeriod"] = row.CostingPeriod,
            })
            .ToList();

        var plant = plantCode;
        var modelPart = modelFilters.Count == 0
            ? "ALL"
            : string.Join("-", modelFilters);
        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            columnKeys,
            columnLabels,
            sheetName ?? "零材料成本",
            fileName ?? $"零材料成本_{plant}_{modelPart}_{costingPeriod}.xlsx");
    }

    /// <summary>
    /// 批量/直接回填移动平均价（ComponentCode 空=批量发现组件；有值=单组件；与手工更新同一核心逻辑）
    /// </summary>
    /// <param name="dto">工厂+核算月；组件可选；机种可选（仅批量发现组件时用）</param>
    /// <returns>回填统计</returns>
    public Task<TaktBomMaterialZeroPriceMovingBackfillResultDto> BackfillBomMaterialZeroPriceMovingAsync(
        TaktBomMaterialZeroPriceMovingBackfillDto dto) =>
        BackfillBomMaterialZeroPriceMovingCoreAsync(dto, null);

    /// <summary>
    /// 批量/直接回填移动平均价
    /// </summary>
    /// <param name="dto">工厂+核算月；组件可选；机种可选（仅批量发现组件时用）</param>
    /// <param name="preloadedHeaders">已从当前库查出的主表行；有值时不再按日期重查主表</param>
    /// <returns>回填统计</returns>
    private async Task<TaktBomMaterialZeroPriceMovingBackfillResultDto> BackfillBomMaterialZeroPriceMovingCoreAsync(
        TaktBomMaterialZeroPriceMovingBackfillDto dto,
        IReadOnlyList<TaktBomMaterialCost>? preloadedHeaders)
    {
        EnsureThreeLayerContext();
        ArgumentNullException.ThrowIfNull(dto);
        ArgumentException.ThrowIfNullOrWhiteSpace(dto.PlantCode);

        var query = new TaktBomMaterialZeroPriceQueryDto
        {
            PlantCode = dto.PlantCode,
            ModelCode = dto.ModelCode,
            ModelCodes = dto.ModelCodes,
            CostingDateStart = dto.CostingDateStart,
            CostingDateEnd = dto.CostingDateEnd,
        };
        var (costingStart, costingEnd, costingPeriod) = PrepareCostingMonth(query);
        var plantCode = dto.PlantCode.Trim();
        var singleComponent = string.IsNullOrWhiteSpace(dto.ComponentCode) ? null : dto.ComponentCode.Trim();
        var modelFilters = ParseModelCodes(dto.ModelCodes, dto.ModelCode);

        List<string> componentTargets;
        if (singleComponent != null)
        {
            componentTargets = new List<string> { singleComponent };
        }
        else
        {
            var (_, _, mergedRows) = await BuildMergedRowsAsync(
                plantCode,
                modelFilters,
                costingStart,
                costingEnd,
                costingPeriod,
                includeDeleted: true,
                preloadedHeaders: preloadedHeaders);
            componentTargets = mergedRows
                .Where(r => !string.IsNullOrWhiteSpace(r.ComponentCode))
                .Select(r => r.ComponentCode.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }
        if (componentTargets.Count == 0)
        {
            return new TaktBomMaterialZeroPriceMovingBackfillResultDto
            {
                ProcessedMonth = costingPeriod,
                ItemPhysicalTable = await ResolveBomItemTableLabelAsync(costingStart),
            };
        }

        var componentSources = new List<(string ComponentCode, SuggestedMovingSource Source)>();
        var skippedNoPrice = 0;
        foreach (var componentCode in componentTargets)
        {
            var source = await ResolveSuggestedMovingSourceAsync(
                plantCode,
                costingPeriod,
                componentCode);
            if (source == null)
            {
                skippedNoPrice = checked(skippedNoPrice + 1);
                continue;
            }
            componentSources.Add((componentCode, source));
        }
        var result = await ApplyMovingPriceBackfillCoreAsync(
            plantCode,
            costingStart,
            costingPeriod,
            componentSources);
        result.SkippedNoPriceCount = skippedNoPrice;
        result.ItemPhysicalTable = await ResolveBomItemTableLabelAsync(costingStart);
        return result;
    }

    /// <summary>
    /// 手工更新移动平均价（与批量/直接回填同一核心逻辑）
    /// </summary>
    /// <param name="dto">工厂+核算月+原组件+新组件+价/单位/币种</param>
    /// <returns>更新统计</returns>
    public async Task<TaktBomMaterialZeroPriceMovingBackfillResultDto> ManualUpdateBomMaterialZeroPriceMovingAsync(
        TaktBomMaterialZeroPriceManualMovingDto dto)
    {
        EnsureThreeLayerContext();
        ArgumentNullException.ThrowIfNull(dto);
        ArgumentException.ThrowIfNullOrWhiteSpace(dto.PlantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(dto.ComponentCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(dto.SourceComponentCode);
        if (dto.MovingAveragePrice <= 0m)
        {
            throw new TaktBusinessException("替换移动平均价必须大于 0");
        }

        var query = new TaktBomMaterialZeroPriceQueryDto
        {
            PlantCode = dto.PlantCode,
            CostingDateStart = dto.CostingDateStart,
            CostingDateEnd = dto.CostingDateEnd,
        };
        var (costingStart, _, costingPeriod) = PrepareCostingMonth(query);
        var plantCode = dto.PlantCode.Trim();
        var componentCode = dto.ComponentCode.Trim();
        var priceUnit = dto.MovingPriceUnit <= 0 ? 1000 : dto.MovingPriceUnit;
        var currency = string.IsNullOrWhiteSpace(dto.MovingPriceCurrencyCode)
            ? "CNY"
            : dto.MovingPriceCurrencyCode.Trim().ToUpperInvariant();
        var source = new SuggestedMovingSource
        {
            SourceComponentCode = dto.SourceComponentCode.Trim(),
            ValuationPeriod = costingPeriod,
            MovingPrice = dto.MovingAveragePrice,
            PriceUnit = priceUnit,
            CurrencyCode = currency,
        };
        return await ApplyMovingPriceBackfillCoreAsync(
            plantCode,
            costingStart,
            costingPeriod,
            new List<(string ComponentCode, SuggestedMovingSource Source)> { (componentCode, source) });
    }

    /// <summary>
    /// Quartz：判定日所在自然月，按表内 PlantCode 批量回填零价移动价（不显式传工厂）
    /// </summary>
    /// <param name="asOfDate">判定日；默认今天</param>
    /// <returns>回填统计；无工厂数据时 null</returns>
    public async Task<TaktBomMaterialZeroPriceMovingBackfillResultDto?> RunScheduledBomMaterialZeroPriceMovingBackfillAsync(
        DateTime? asOfDate = null)
    {
        EnsureThreeLayerContext();
        var asOf = TaktBomMaterialCostItemLineCostHelper.NormalizeCostingDate(asOfDate ?? DateTime.Today);
        var costingStart = new DateTime(asOf.Year, asOf.Month, 1);
        var costingEnd = new DateTime(
            asOf.Year,
            asOf.Month,
            DateTime.DaysInMonth(asOf.Year, asOf.Month),
            23,
            59,
            59,
            999);
        var periodKey = TaktBomMaterialCostItemLineCostHelper.ToPeriodKey(asOf);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        var periodHeaders = await _bomMaterialCostRepository.GetListAsync(
            x => x.TenantCode == tenantCode
                && x.CompanyCode == companyCode
                && x.CostingPeriod == periodKey,
            includeSoftDeleted: true);
        var plantCodes = periodHeaders
            .Select(x => x.PlantCode?.Trim() ?? string.Empty)
            .Where(p => !string.IsNullOrEmpty(p))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(p => p, StringComparer.OrdinalIgnoreCase)
            .ToList();
        if (plantCodes.Count == 0)
        {
            return null;
        }

        var monthEndExclusive = costingStart.AddMonths(1);
        var fertType = TaktBomMaterialCostItemLineCostHelper.FertMaterialTypeCode;
        TaktBomMaterialZeroPriceMovingBackfillResultDto? aggregate = null;
        foreach (var code in plantCodes)
        {
            // 主表行已从当前连接（选中库）取出，按工厂+FERT+核算月在内存筛选，避免再查时条件被丢掉
            var plantHeaders = periodHeaders
                .Where(h => string.Equals(h.PlantCode?.Trim(), code, StringComparison.OrdinalIgnoreCase)
                    && string.Equals((h.MaterialType ?? string.Empty).Trim(), fertType, StringComparison.OrdinalIgnoreCase)
                    && h.CostingDate >= costingStart
                    && h.CostingDate < monthEndExclusive
                    && !string.IsNullOrWhiteSpace(h.ProductCode))
                .ToList();
            var part = await BackfillBomMaterialZeroPriceMovingCoreAsync(
                new TaktBomMaterialZeroPriceMovingBackfillDto
                {
                    PlantCode = code,
                    CostingDateStart = costingStart,
                    CostingDateEnd = costingEnd,
                },
                plantHeaders);
            if (aggregate == null)
            {
                aggregate = part;
                continue;
            }
            aggregate.ScannedRowCount = checked(aggregate.ScannedRowCount + part.ScannedRowCount);
            aggregate.UpdatedRowCount = checked(aggregate.UpdatedRowCount + part.UpdatedRowCount);
            aggregate.SkippedNoPriceCount = checked(aggregate.SkippedNoPriceCount + part.SkippedNoPriceCount);
            aggregate.UnchangedRowCount = checked(aggregate.UnchangedRowCount + part.UnchangedRowCount);
            aggregate.ComponentProcessedCount =
                checked(aggregate.ComponentProcessedCount + part.ComponentProcessedCount);
            aggregate.ProductMonthlyCostUpdatedCount =
                checked(aggregate.ProductMonthlyCostUpdatedCount + part.ProductMonthlyCostUpdatedCount);
            aggregate.ModelMonthlyAverageUpdatedCount =
                checked(aggregate.ModelMonthlyAverageUpdatedCount + part.ModelMonthlyAverageUpdatedCount);
            aggregate.ProcessedMonth = part.ProcessedMonth;
            if (string.IsNullOrWhiteSpace(aggregate.ItemPhysicalTable))
            {
                aggregate.ItemPhysicalTable = part.ItemPhysicalTable;
            }
        }
        return aggregate;
    }

    /// <summary>
    /// 将 PCB SECT 整树明细写入 PcbSectIndicator=X（描述含 PCB SECT 的节点及其子孙；已标则跳过）
    /// </summary>
    /// <param name="dto">工厂+核算月；机种可选</param>
    /// <returns>打标统计</returns>
    public async Task<TaktBomMaterialZeroPricePcbSectMarkResultDto> MarkBomMaterialZeroPricePcbSectAsync(
        TaktBomMaterialZeroPricePcbSectMarkDto dto)
    {
        EnsureThreeLayerContext();
        ArgumentNullException.ThrowIfNull(dto);
        ArgumentException.ThrowIfNullOrWhiteSpace(dto.PlantCode);

        var query = new TaktBomMaterialZeroPriceQueryDto
        {
            PlantCode = dto.PlantCode,
            ModelCode = dto.ModelCode,
            ModelCodes = dto.ModelCodes,
            CostingDateStart = dto.CostingDateStart,
            CostingDateEnd = dto.CostingDateEnd,
        };
        var (costingStart, costingEnd, costingPeriod) = PrepareCostingMonth(query);
        var plantCode = dto.PlantCode.Trim();
        var modelFilters = ParseModelCodes(dto.ModelCodes, dto.ModelCode);
        var productCodes = await ResolveFertProductCodesAsync(
            plantCode,
            modelFilters,
            costingStart,
            costingEnd,
            includeDeleted: false);
        if (productCodes.Count == 0)
        {
            return new TaktBomMaterialZeroPricePcbSectMarkResultDto
            {
                ProcessedMonth = costingPeriod,
            };
        }

        var allItems = await LoadBomCostItemsRawForProductsAsync(
            plantCode,
            productCodes,
            costingStart,
            costingEnd);
        var pcbRows = TaktBomMaterialCostItemLineCostHelper
            .CollectPcbSectHierarchyRows(allItems)
            .ToList();

        var updated = 0;
        var unchanged = 0;
        var rowsToUpdate = new List<TaktBomMaterialCostItem>();
        var at = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
        foreach (var row in pcbRows)
        {
            if (TaktBomMaterialCostItemLineCostHelper.HasPcbSectIndicatorMark(row.PcbSectIndicator))
            {
                unchanged = checked(unchanged + 1);
                continue;
            }
            if (!TaktBomMaterialCostItemLineCostHelper.TryApplyPcbSectIndicatorMark(row))
            {
                continue;
            }
            row.ExtField = TaktBomExtFieldBackfillHistoryHelper.Append(
                row.ExtField,
                TaktBomExtFieldBackfillHistoryHelper.ScopePcbSect,
                new System.Text.Json.Nodes.JsonObject
                {
                    ["at"] = at,
                    ["pcb_sect_indicator"] = TaktBomMaterialCostItemLineCostHelper.PcbSectIndicatorMarkValue,
                    ["costing_period"] = costingPeriod,
                    ["plant_code"] = plantCode,
                    ["product_code"] = row.ProductCode?.Trim() ?? string.Empty,
                    ["component_code"] = row.ComponentCode?.Trim() ?? string.Empty,
                    ["bom_level"] = row.BomLevel?.Trim() ?? string.Empty,
                });
            rowsToUpdate.Add(row);
            updated = checked(updated + 1);
        }

        if (rowsToUpdate.Count > 0)
        {
            var yearTable = await ResolveBomItemPhysicalTableAsync(costingStart.Year);
            const int updateChunkSize = 500;
            for (var offset = 0; offset < rowsToUpdate.Count; offset = checked(offset + updateChunkSize))
            {
                var chunk = rowsToUpdate.Skip(offset).Take(updateChunkSize).ToList();
                await _bomMaterialCostItemRepository.UpdateRangeAsync(chunk, yearTable);
            }
        }

        return new TaktBomMaterialZeroPricePcbSectMarkResultDto
        {
            ScannedRowCount = allItems.Count,
            PcbSectRowCount = pcbRows.Count,
            UpdatedRowCount = updated,
            UnchangedRowCount = unchanged,
            SkippedOverflowCount = 0,
            ProcessedMonth = costingPeriod,
        };
    }

    /// <summary>
    /// 去掉当前工厂纯数字编码的前导 0。成本汇总只改 ProductCode；明细年分表与基表改 ProductCode 与 ComponentCode。含非数字字符的编码不改。
    /// </summary>
    /// <param name="dto">工厂</param>
    /// <returns>更新与冲突跳过行数</returns>
    public async Task<TaktBomMaterialZeroPriceStripLeadingZeroResultDto> StripBomMaterialZeroPriceLeadingZerosAsync(
        TaktBomMaterialZeroPriceStripLeadingZeroDto dto)
    {
        EnsureThreeLayerContext();
        ArgumentNullException.ThrowIfNull(dto);
        ArgumentException.ThrowIfNullOrWhiteSpace(dto.PlantCode);
        var plantCode = dto.PlantCode.Trim();
        var result = new TaktBomMaterialZeroPriceStripLeadingZeroResultDto();
        await StripHeaderProductLeadingZerosAsync(plantCode, result);
        foreach (var table in await ListBomItemPhysicalTablesAsync())
        {
            await StripItemCodeLeadingZerosAsync(plantCode, table, result);
        }
        return result;
    }

    /// <summary>
    /// 统一核心（三步，顺序固定）：
    /// 1）明细移动价：工厂 + 核算月 + 组件写价；PcbSectIndicator 已标记（=X）则永不回填；
    /// 2）主表产品月计算：按明细全量重算；
    /// 3）机种月平均成本：同工厂+月+物料类型+机种算术平均。
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="costingStart">核算月初</param>
    /// <param name="costingPeriod">核算月 yyyy-MM</param>
    /// <param name="componentSources">组件与移动价源</param>
    /// <returns>回填统计</returns>
    private async Task<TaktBomMaterialZeroPriceMovingBackfillResultDto> ApplyMovingPriceBackfillCoreAsync(
        string plantCode,
        DateTime costingStart,
        string costingPeriod,
        IReadOnlyList<(string ComponentCode, SuggestedMovingSource Source)> componentSources)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(plantCode);
        ArgumentNullException.ThrowIfNull(componentSources);
        if (componentSources.Count == 0)
        {
            return new TaktBomMaterialZeroPriceMovingBackfillResultDto
            {
                ProcessedMonth = costingPeriod,
            };
        }

        var yearTable = await ResolveBomItemPhysicalTableAsync(costingStart.Year);
        var scanned = 0;
        var updated = 0;
        var unchanged = 0;
        var componentProcessed = 0;
        var affectedProductSources = new Dictionary<string, List<(string Component, SuggestedMovingSource Source)>>(
            StringComparer.OrdinalIgnoreCase);
        var lineDeltas = new List<MovingPriceLineDelta>();
        SuggestedMovingSource? firstSource = null;

        // ----------------------------------------
        // 步骤1：工厂 + 日期 + 组件 → 更新明细移动价格（PcbSectIndicator 已标记则永不回填）
        // ----------------------------------------
        foreach (var (componentCodeRaw, source) in componentSources)
        {
            ArgumentNullException.ThrowIfNull(source);
            var componentCode = componentCodeRaw.Trim();
            if (string.IsNullOrWhiteSpace(componentCode))
            {
                continue;
            }
            var items = await LoadBomCostItemsByComponentAsync(
                plantCode,
                componentCode,
                costingStart,
                costingStart);
            if (items.Count == 0)
            {
                continue;
            }
            scanned = checked(scanned + items.Count);
            firstSource ??= source;
            var anyPriceUpdated = false;
            var rowsToUpdate = new List<TaktBomMaterialCostItem>();
            foreach (var row in items)
            {
                // 标记存在：永不回填价格（Mark 由人工控制）
                if (TaktBomMaterialCostItemLineCostHelper.HasPcbSectIndicatorMark(row.PcbSectIndicator))
                {
                    continue;
                }
                // 用量为 0：永不回填
                if (row.ComponentQuantity <= 0m)
                {
                    continue;
                }
                var oldPrice = row.MovingAveragePrice;
                var oldUnit = row.MovingPriceUnit <= 0 ? 1 : row.MovingPriceUnit;
                if (!TaktBomMaterialZeroPriceMovingBackfillHelper.ApplyMovingAveragePriceFields(
                        row,
                        source.SourceComponentCode,
                        source.ValuationPeriod,
                        source.MovingPrice,
                        source.PriceUnit,
                        source.CurrencyCode,
                        forceOverwrite: true))
                {
                    unchanged = checked(unchanged + 1);
                    continue;
                }
                var delta = ComputeMovingPriceLineCostDelta(row, oldPrice, oldUnit);
                if (delta != 0m && !string.IsNullOrWhiteSpace(row.ProductCode))
                {
                    lineDeltas.Add(new MovingPriceLineDelta
                    {
                        ProductCode = row.ProductCode.Trim(),
                        ComponentKey = TaktBomMaterialCostItemLineCostHelper.BuildComponentKey(row),
                        CostingDate = TaktBomMaterialCostItemLineCostHelper.NormalizeCostingDate(row.CostingDate),
                        IsDeleted = row.IsDeleted,
                        Id = row.Id,
                        Delta = delta,
                        CurrencyCode = row.MovingPriceCurrencyCode?.Trim() ?? string.Empty,
                    });
                }
                rowsToUpdate.Add(row);
                updated = checked(updated + 1);
                anyPriceUpdated = true;
            }
            if (rowsToUpdate.Count > 0)
            {
                const int updateChunkSize = 500;
                for (var offset = 0; offset < rowsToUpdate.Count; offset = checked(offset + updateChunkSize))
                {
                    var chunk = rowsToUpdate.Skip(offset).Take(updateChunkSize).ToList();
                    await _bomMaterialCostItemRepository.UpdateRangeAsync(chunk, yearTable);
                }
            }
            if (!anyPriceUpdated)
            {
                continue;
            }
            componentProcessed = checked(componentProcessed + 1);
            RegisterAffectedProductsFromComponentItems(
                rowsToUpdate,
                componentCode,
                source,
                affectedProductSources);
        }

        var productDeltas = AggregateProductMonthlyCostDeltas(lineDeltas);

        // 步骤2+3：按明细行全量重算产品月计算 → 机种月平均
        var productCostUpdated = 0;
        var modelAverageUpdated = 0;
        if (affectedProductSources.Count > 0)
        {
            (productCostUpdated, modelAverageUpdated) = await RefreshHeadersAfterMovingBackfillAsync(
                plantCode,
                costingPeriod,
                affectedProductSources,
                productDeltas);
        }

        return new TaktBomMaterialZeroPriceMovingBackfillResultDto
        {
            ScannedRowCount = scanned,
            UpdatedRowCount = updated,
            UnchangedRowCount = unchanged,
            SkippedNoPriceCount = 0,
            ComponentProcessedCount = componentProcessed,
            SourceComponentCode = firstSource?.SourceComponentCode,
            ValuationPeriod = firstSource?.ValuationPeriod,
            PriceInfo = firstSource == null
                ? null
                : TaktBomMaterialZeroPriceMovingBackfillHelper.FormatMovingPriceInfo(
                    firstSource.ValuationPeriod,
                    firstSource.MovingPrice,
                    firstSource.PriceUnit,
                    firstSource.CurrencyCode),
            ProductMonthlyCostUpdatedCount = productCostUpdated,
            ModelMonthlyAverageUpdatedCount = modelAverageUpdated,
            ProcessedMonth = costingPeriod,
        };
    }

    /// <summary>
    /// 步骤2：按明细行（工厂/日期/产品/组件/数量/生产相关/PCB SECT/采购类型/移动价/单位）全量重算产品月计算；
    /// 步骤3：刷机种月均（工厂+日期+物料类型+机种）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="costingPeriod">核算月 yyyy-MM</param>
    /// <param name="affectedProductSources">产品 → 回填来源组件与移动价源</param>
    /// <param name="productDeltas">保留参数兼容；产品月计算已改为明细全量重算，不再使用差额</param>
    /// <returns>产品月成本更新行数、机种月成本更新行数</returns>
    private async Task<(int ProductCostUpdated, int ModelAverageUpdated)> RefreshHeadersAfterMovingBackfillAsync(
        string plantCode,
        string costingPeriod,
        IReadOnlyDictionary<string, List<(string Component, SuggestedMovingSource Source)>> affectedProductSources,
        IReadOnlyDictionary<string, (decimal Delta, string? CurrencyCode, DateTime? LatestCostingDate)> productDeltas)
    {
        if (affectedProductSources.Count == 0)
        {
            return (0, 0);
        }
        _ = productDeltas;
        var affectedProductCodes = affectedProductSources.Keys.ToList();
        var periodHeaders = await _bomMaterialCostRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plantCode
                && x.CostingPeriod == costingPeriod,
            asTableName: null,
            includeSoftDeleted: true);
        var (monthStart, monthEnd) = TaktBomMaterialCostItemLineCostHelper.ResolvePeriodDateRange(costingPeriod);
        var productCostUpdated = 0;
        var touchedModelGroups = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var modelSourceByGroup = new Dictionary<string, (string Component, SuggestedMovingSource Source)>(
            StringComparer.OrdinalIgnoreCase);
        var headersToUpdate = new List<TaktBomMaterialCost>();
        foreach (var productCode in affectedProductCodes)
        {
            var product = productCode.Trim();
            if (string.IsNullOrWhiteSpace(product)
                || !affectedProductSources.TryGetValue(product, out var sources)
                || sources.Count == 0)
            {
                continue;
            }
            var productItems = await LoadBomCostItemsRawForProductsAsync(
                plantCode,
                new[] { product },
                monthStart,
                monthEnd);
            var snapshot = TaktBomMaterialCostItemLineCostHelper.ResolvePeriodSnapshot(
                productItems,
                plantCode,
                product,
                costingPeriod);
            var nextCost = TaktBomMaterialCostItemLineCostHelper.SumSnapshotCost(snapshot);
            var writeCurrency = snapshot
                .Select(TaktBomMaterialCostItemLineCostHelper.ResolveCurrency)
                .FirstOrDefault(c => !string.IsNullOrWhiteSpace(c))
                ?? productItems
                    .Select(x => x.MovingPriceCurrencyCode)
                    .FirstOrDefault(c => !string.IsNullOrWhiteSpace(c))
                ?? string.Empty;
            var writeDate = TaktBomMaterialCostItemLineCostHelper.ResolveLatestCostingDate(
                productItems,
                plantCode,
                product,
                costingPeriod);
            var matchedHeaders = periodHeaders
                .Where(h => TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(h.ProductCode, product))
                .ToList();
            foreach (var header in matchedHeaders)
            {
                var currentCost = TaktBomMaterialCostItemLineCostHelper.RoundCost(header.ProductMonthlyCalculation);
                var effectiveCurrency = string.IsNullOrWhiteSpace(writeCurrency)
                    ? header.CurrencyCode
                    : writeCurrency.Trim();
                var effectiveDate = writeDate ?? header.CostingDate;
                if (currentCost != nextCost
                    || !string.Equals(
                        header.CurrencyCode?.Trim() ?? string.Empty,
                        effectiveCurrency?.Trim() ?? string.Empty,
                        StringComparison.OrdinalIgnoreCase)
                    || header.CostingDate != effectiveDate)
                {
                    foreach (var (componentCode, source) in sources)
                    {
                        TaktBomMaterialZeroPriceMovingBackfillHelper.ApplyHeaderProductCostMovingBackfillHistory(
                            header,
                            componentCode,
                            source.SourceComponentCode,
                            source.ValuationPeriod,
                            source.MovingPrice,
                            source.PriceUnit,
                            source.CurrencyCode,
                            currentCost,
                            nextCost);
                    }
                }
                header.ProductMonthlyCalculation = nextCost;
                header.CurrencyCode = effectiveCurrency ?? string.Empty;
                header.CostingDate = effectiveDate;
                header.CostingPeriod = costingPeriod;
                headersToUpdate.Add(header);
                productCostUpdated = checked(productCostUpdated + 1);
                var materialType = string.IsNullOrWhiteSpace(header.MaterialType)
                    ? TaktBomMaterialCostItemLineCostHelper.FertMaterialTypeCode
                    : header.MaterialType.Trim();
                var modelCode = header.ModelCode?.Trim() ?? string.Empty;
                if (!string.IsNullOrWhiteSpace(modelCode))
                {
                    var groupKey = BuildModelAverageGroupKey(materialType, modelCode);
                    if (!string.IsNullOrWhiteSpace(groupKey))
                    {
                        touchedModelGroups.Add(groupKey);
                        modelSourceByGroup[groupKey] = sources[0];
                    }
                }
            }
        }
        if (headersToUpdate.Count > 0)
        {
            await _bomMaterialCostRepository.UpdateRangeAsync(headersToUpdate);
        }
        var modelAverageUpdated = 0;
        foreach (var groupKey in touchedModelGroups.OrderBy(k => k, StringComparer.OrdinalIgnoreCase))
        {
            if (!modelSourceByGroup.TryGetValue(groupKey, out var sourcePair))
            {
                continue;
            }
            var parts = groupKey.Split('|', 2);
            if (parts.Length != 2)
            {
                continue;
            }
            modelAverageUpdated = checked(
                modelAverageUpdated
                + await RefreshModelMonthlyAverageForPeriodAsync(
                    plantCode,
                    parts[0],
                    parts[1],
                    costingPeriod,
                    sourcePair.Component,
                    sourcePair.Source));
        }
        return (productCostUpdated, modelAverageUpdated);
    }

    /// <summary>
    /// 单行回填前后成本差额（仅 X+F 且用量&gt;0.001；行金额=数量×(价÷单位)）
    /// </summary>
    /// <param name="row">已写新价的明细行</param>
    /// <param name="oldPrice">回填前移动价</param>
    /// <param name="oldUnit">回填前价格单位</param>
    /// <returns>新行成本 − 旧行成本</returns>
    private static decimal ComputeMovingPriceLineCostDelta(
        TaktBomMaterialCostItem row,
        decimal oldPrice,
        int oldUnit)
    {
        ArgumentNullException.ThrowIfNull(row);
        if (!TaktBomMaterialCostItemLineCostHelper.CountsTowardBomMaterialCostItem(row))
        {
            return 0m;
        }
        var unitOld = oldUnit <= 0 ? 1 : oldUnit;
        var unitNew = TaktBomMaterialCostItemLineCostHelper.ResolveMovingPriceUnit(row);
        var oldLine = oldPrice == 0m
            ? 0m
            : TaktBomMaterialCostItemLineCostHelper.RoundCost(row.ComponentQuantity * (oldPrice / unitOld));
        var newLine = row.MovingAveragePrice == 0m
            ? 0m
            : TaktBomMaterialCostItemLineCostHelper.RoundCost(
                row.ComponentQuantity * (row.MovingAveragePrice / unitNew));
        return TaktBomMaterialCostItemLineCostHelper.RoundCost(newLine - oldLine);
    }

    /// <summary>
    /// 按产品汇总差额：同产品取最后核算日，同组件键优先未软删再取最大 Id，避免重复加计
    /// </summary>
    /// <param name="lineDeltas">行差额</param>
    /// <returns>产品 → (差额合计, 币种, 最后核算日)</returns>
    private static Dictionary<string, (decimal Delta, string? CurrencyCode, DateTime? LatestCostingDate)> AggregateProductMonthlyCostDeltas(
        IReadOnlyList<MovingPriceLineDelta> lineDeltas)
    {
        ArgumentNullException.ThrowIfNull(lineDeltas);
        var result = new Dictionary<string, (decimal Delta, string? CurrencyCode, DateTime? LatestCostingDate)>(
            StringComparer.OrdinalIgnoreCase);
        foreach (var productGroup in lineDeltas.GroupBy(d => d.ProductCode, StringComparer.OrdinalIgnoreCase))
        {
            var product = productGroup.Key;
            if (string.IsNullOrWhiteSpace(product))
            {
                continue;
            }
            var latestDay = productGroup.Max(d => d.CostingDate);
            var dayRows = productGroup.Where(d => d.CostingDate == latestDay).ToList();
            var delta = dayRows
                .GroupBy(d => d.ComponentKey, StringComparer.Ordinal)
                .Select(g => g.OrderByDescending(x => x.Id).First())
                .Sum(x => x.Delta);
            var currency = dayRows
                .Select(x => x.CurrencyCode)
                .FirstOrDefault(c => !string.IsNullOrWhiteSpace(c));
            result[product] = (
                TaktBomMaterialCostItemLineCostHelper.RoundCost(delta),
                currency,
                latestDay);
        }
        return result;
    }

    /// <summary>
    /// 步骤3：刷新同「工厂 + 核算月(日期) + 物料类型 + 机种」的机种月平均成本（主表 ExtField 记履历）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="materialType">物料类型</param>
    /// <param name="modelCode">机种</param>
    /// <param name="periodKey">核算月</param>
    /// <param name="componentCode">回填组件</param>
    /// <param name="source">移动价源</param>
    /// <returns>更新行数</returns>
    private async Task<int> RefreshModelMonthlyAverageForPeriodAsync(
        string plantCode,
        string materialType,
        string modelCode,
        string periodKey,
        string componentCode,
        SuggestedMovingSource source)
    {
        if (string.IsNullOrWhiteSpace(modelCode) || string.IsNullOrWhiteSpace(materialType))
        {
            return 0;
        }
        ArgumentNullException.ThrowIfNull(source);
        var mt = materialType.Trim();
        var model = modelCode.Trim();
        var plant = plantCode.Trim();
        // 工厂 + 日期 + 物料类型（机种在内存 Trim 比对，兼容空类型对齐 FERT）
        var headers = await _bomMaterialCostRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plant
                && x.CostingPeriod == periodKey,
            asTableName: null,
            includeSoftDeleted: true);
        var matched = headers
            .Where(h =>
                string.Equals((h.ModelCode ?? string.Empty).Trim(), model, StringComparison.OrdinalIgnoreCase)
                && MaterialTypeMatchesModelAverageGroup(h.MaterialType, mt))
            .ToList();
        return await ApplyModelMonthlyAverageAndSaveAsync(matched, componentCode, source);
    }

    /// <summary>
    /// 按「工厂+核算月+物料类型+机种」写机种月成本（固定口径：&lt;2026-06 产品月成本，≥2026-06 产品月计算）；
    /// 比较后有变化才落库并追加 ExtField 履历
    /// </summary>
    /// <param name="headers">同工厂+日期+物料类型+机种主表行</param>
    /// <param name="componentCode">回填组件</param>
    /// <param name="source">移动价源</param>
    /// <returns>实际更新行数</returns>
    private async Task<int> ApplyModelMonthlyAverageAndSaveAsync(
        List<TaktBomMaterialCost> headers,
        string componentCode,
        SuggestedMovingSource source)
    {
        ArgumentNullException.ThrowIfNull(headers);
        ArgumentNullException.ThrowIfNull(source);
        if (headers.Count == 0)
        {
            return 0;
        }
        // 按产品去重；核算月&lt;2026-06 用产品月成本，≥2026-06 用产品月计算 → 算术平均
        var average = TaktBomMaterialCostItemModelEnrichmentHelper.ComputeModelMonthlyAverageFromHeaders(headers);
        var toSave = new List<TaktBomMaterialCost>();
        foreach (var header in headers)
        {
            var current = TaktBomMaterialCostItemLineCostHelper.RoundCost(header.ModelMonthlyAverageCost);
            // 先全量算 average，再比较：仅新旧相等才跳过（不是「原值非 0 就跳过」）
            if (current == average)
            {
                continue;
            }
            TaktBomMaterialZeroPriceMovingBackfillHelper.ApplyHeaderModelAverageMovingBackfillHistory(
                header,
                componentCode,
                source.SourceComponentCode,
                source.ValuationPeriod,
                source.MovingPrice,
                source.PriceUnit,
                source.CurrencyCode,
                current,
                average);
            header.ModelMonthlyAverageCost = average;
            toSave.Add(header);
        }
        if (toSave.Count == 0)
        {
            return 0;
        }
        await _bomMaterialCostRepository.UpdateRangeAsync(toSave);
        return toSave.Count;
    }

    /// <summary>
    /// 机种月成本分组键（物料类型|机种；类型空时用 FERT）
    /// </summary>
    /// <param name="materialType">物料类型</param>
    /// <param name="modelCode">机种</param>
    /// <returns>分组键；机种空则空串</returns>
    private static string BuildModelAverageGroupKey(string? materialType, string? modelCode)
    {
        var model = modelCode?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(model))
        {
            return string.Empty;
        }
        var mt = string.IsNullOrWhiteSpace(materialType)
            ? TaktBomMaterialCostItemLineCostHelper.FertMaterialTypeCode
            : materialType.Trim();
        return $"{mt}|{model}";
    }

    /// <summary>
    /// 空物料类型是否与分组期望类型视为同组（空对齐 FERT）
    /// </summary>
    /// <param name="stored">主表已存类型</param>
    /// <param name="expected">分组期望类型</param>
    /// <returns>是否同组</returns>
    private static bool MaterialTypeMatchesModelAverageGroup(string? stored, string expected)
    {
        var actual = stored?.Trim() ?? string.Empty;
        if (string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        return string.IsNullOrEmpty(actual)
            && string.Equals(
                expected,
                TaktBomMaterialCostItemLineCostHelper.FertMaterialTypeCode,
                StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 步骤2 登记：将步骤1「工厂+日期+组件」明细中出现的全部产品记入待刷新产品月成本
    /// </summary>
    /// <param name="componentItems">该组件在工厂+核算月下的明细</param>
    /// <param name="componentCode">组件编码</param>
    /// <param name="source">移动价源</param>
    /// <param name="affectedProductSources">产品 → 来源列表</param>
    private static void RegisterAffectedProductsFromComponentItems(
        IEnumerable<TaktBomMaterialCostItem> componentItems,
        string componentCode,
        SuggestedMovingSource source,
        IDictionary<string, List<(string Component, SuggestedMovingSource Source)>> affectedProductSources)
    {
        ArgumentNullException.ThrowIfNull(componentItems);
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(affectedProductSources);
        var component = (componentCode ?? string.Empty).Trim();
        foreach (var row in componentItems)
        {
            if (string.IsNullOrWhiteSpace(row.ProductCode))
            {
                continue;
            }
            var product = row.ProductCode.Trim();
            if (!affectedProductSources.TryGetValue(product, out var list))
            {
                list = new List<(string Component, SuggestedMovingSource Source)>();
                affectedProductSources[product] = list;
            }
            if (!list.Exists(x =>
                    string.Equals(x.Component, component, StringComparison.OrdinalIgnoreCase)))
            {
                list.Add((component, source));
            }
        }
    }

    /// <summary>
    /// 加载产品 BOM 明细（明细无软删）。zeroPriceViewScope 为 true 时查询即限定零价格视图行。
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="productCodes">产品编码</param>
    /// <param name="costingMonthStart">核算月初</param>
    /// <param name="costingMonthEnd">核算月末（同月初则整月）</param>
    /// <param name="zeroPriceViewScope">true=只查生产相关=X、PCB SECT 标识为空、采购类型=F、移动平均价=0</param>
    /// <returns>明细行</returns>
    private async Task<List<TaktBomMaterialCostItem>> LoadBomCostItemsRawForProductsAsync(
        string plantCode,
        IReadOnlyList<string> productCodes,
        DateTime? costingMonthStart,
        DateTime? costingMonthEnd,
        bool zeroPriceViewScope = false)
    {
        var allItems = new List<TaktBomMaterialCostItem>();
        const int chunkSize = 200;
        DateTime? costingExclusiveEnd = costingMonthEnd.HasValue
            ? costingMonthEnd.Value.AddMonths(1)
            : null;
        var lookupCodes = productCodes
            .SelectMany(TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        if (lookupCodes.Count == 0)
        {
            return allItems;
        }
        for (var i = 0; i < lookupCodes.Count; i += chunkSize)
        {
            var chunk = lookupCodes.Skip(i).Take(chunkSize).ToList();
            var tenantCode = CurrentTenantCode;
            var companyCode = CurrentCompanyCode;
            var plant = plantCode.Trim();
            var exp = Expressionable.Create<TaktBomMaterialCostItem>();
            exp = exp.And(x =>
                x.TenantCode == tenantCode
                && x.CompanyCode == companyCode
                && x.PlantCode == plant
                && chunk.Contains(x.ProductCode));
            if (costingMonthStart.HasValue)
            {
                var start = costingMonthStart.Value;
                exp = exp.And(x => x.CostingDate >= start);
            }
            if (costingExclusiveEnd.HasValue)
            {
                var endExclusive = costingExclusiveEnd.Value;
                exp = exp.And(x => x.CostingDate < endExclusive);
            }
            if (zeroPriceViewScope)
            {
                exp = exp.And(x =>
                    x.MovingAveragePrice == 0
                    && x.ProductionRelated != null
                    && (x.ProductionRelated == "X" || x.ProductionRelated == "x")
                    && x.PurchaseType != null
                    && (x.PurchaseType == "F" || x.PurchaseType == "f")
                    && (x.PcbSectIndicator == null || x.PcbSectIndicator == ""));
            }
            string? yearTable;
            try
            {
                yearTable = await ResolveBomItemPhysicalTableAsync(
                    TaktYearShardTableHelper.RequireSingleYear(costingMonthStart, costingMonthEnd));
            }
            catch (ArgumentException ex)
            {
                throw new TaktBusinessException(ex.Message);
            }
            var part = await _bomMaterialCostItemRepository.GetListAsync(exp.ToExpression(), yearTable);
            allItems.AddRange(part);
        }
        return allItems;
    }

    /// <summary>
    /// 解析机种多选（ModelCodes + 兼容 ModelCode）
    /// </summary>
    /// <param name="multiCodes">多选逗号串</param>
    /// <param name="singleCode">兼容单值</param>
    /// <returns>去重后的机种列表</returns>
    private static List<string> ParseModelCodes(string? multiCodes, string? singleCode)
    {
        var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        void AddRaw(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
            {
                return;
            }
            foreach (var part in raw.Split(
                         new[] { ',', ';' },
                         StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                if (!string.IsNullOrWhiteSpace(part))
                {
                    set.Add(part);
                }
            }
        }
        AddRaw(multiCodes);
        AddRaw(singleCode);
        return set.OrderBy(c => c, StringComparer.Ordinal).ToList();
    }

    /// <summary>
    /// 取工厂+核算月下 FERT 主表产品编码（机种可选；不跑零价合并）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="modelFilters">机种过滤（空=全部）</param>
    /// <param name="costingStart">核算日起</param>
    /// <param name="costingEnd">核算日止</param>
    /// <param name="includeDeleted">是否含软删主表</param>
    /// <returns>去重产品编码</returns>
    private async Task<List<string>> ResolveFertProductCodesAsync(
        string plantCode,
        IReadOnlyList<string> modelFilters,
        DateTime costingStart,
        DateTime costingEnd,
        bool includeDeleted = false)
    {
        var fertType = TaktBomMaterialCostItemLineCostHelper.FertMaterialTypeCode;
        var exp = Expressionable.Create<TaktBomMaterialCost>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode
            && x.MaterialType == fertType
            && x.CostingDate >= costingStart
            && x.CostingDate <= costingEnd);
        if (!includeDeleted)
        {
            exp = exp.And(x => x.IsDeleted == 0);
        }
        List<TaktBomMaterialCost> headers;
        if (modelFilters.Count > 0)
        {
            var headersAll = await _bomMaterialCostRepository.GetListAsync(
                exp.ToExpression(),
                asTableName: null,
                includeSoftDeleted: includeDeleted);
            var modelSet = new HashSet<string>(modelFilters, StringComparer.OrdinalIgnoreCase);
            headers = headersAll
                .Where(h => !string.IsNullOrWhiteSpace(h.ModelCode)
                    && modelSet.Contains(h.ModelCode.Trim()))
                .ToList();
        }
        else
        {
            headers = await _bomMaterialCostRepository.GetListAsync(
                exp.ToExpression(),
                asTableName: null,
                includeSoftDeleted: includeDeleted);
        }
        return headers
            .Where(h => !string.IsNullOrWhiteSpace(h.ProductCode))
            .Select(h => h.ProductCode.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(c => c, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>
    /// 规范化核算月（须单月）
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <returns>月初、月末、yyyy-MM</returns>
    private static (DateTime Start, DateTime End, string Period) PrepareCostingMonth(
        TaktBomMaterialZeroPriceQueryDto queryDto)
    {
        if (!queryDto.CostingDateStart.HasValue || !queryDto.CostingDateEnd.HasValue)
        {
            throw new TaktBusinessException("请选择核算月份");
        }
        var startMonth = new DateTime(
            queryDto.CostingDateStart.Value.Year,
            queryDto.CostingDateStart.Value.Month,
            1);
        var endMonth = new DateTime(
            queryDto.CostingDateEnd.Value.Year,
            queryDto.CostingDateEnd.Value.Month,
            1);
        if (startMonth != endMonth)
        {
            throw new TaktBusinessException("零价格清单仅支持单个核算月份");
        }
        var lastDay = DateTime.DaysInMonth(startMonth.Year, startMonth.Month);
        var end = new DateTime(startMonth.Year, startMonth.Month, lastDay, 23, 59, 59, 999);
        return (startMonth, end, $"{startMonth.Year:D4}-{startMonth.Month:D2}");
    }

    /// <summary>
    /// 构建零价合并全量行（未分页；主表 MaterialType=FERT；列表排除软删主表，回填/计算含软删主表）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="modelFilters">机种过滤（空=全部）</param>
    /// <param name="costingStart">核算日起</param>
    /// <param name="costingEnd">核算日止</param>
    /// <param name="costingPeriod">核算月 yyyy-MM</param>
    /// <param name="includeDeleted">true=含软删主表（回填）；false=列表仅未删主表；明细无软删</param>
    /// <param name="preloadedHeaders">当前库已查出的主表；有值时不再按日期重查</param>
    /// <returns>产品码、产品→机种、合并行</returns>
    private async Task<(
        List<string> ProductCodes,
        Dictionary<string, HashSet<string>> ProductToModels,
        List<TaktBomMaterialZeroPriceDto> Rows)> BuildMergedRowsAsync(
        string plantCode,
        IReadOnlyList<string> modelFilters,
        DateTime costingStart,
        DateTime costingEnd,
        string costingPeriod,
        bool includeDeleted = false,
        IReadOnlyList<TaktBomMaterialCost>? preloadedHeaders = null)
    {
        var fertType = TaktBomMaterialCostItemLineCostHelper.FertMaterialTypeCode;
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        var plant = plantCode.Trim();
        var rangeStart = costingStart;
        var rangeEnd = costingEnd;
        var exp = Expressionable.Create<TaktBomMaterialCost>();
        exp = exp.And(x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.PlantCode == plant
            && x.MaterialType == fertType
            && x.CostingDate >= rangeStart
            && x.CostingDate <= rangeEnd);
        if (!includeDeleted)
        {
            exp = exp.And(x => x.IsDeleted == 0);
        }
        List<TaktBomMaterialCost> headers;
        if (preloadedHeaders != null)
        {
            headers = preloadedHeaders.ToList();
            if (modelFilters.Count > 0)
            {
                var modelSet = new HashSet<string>(modelFilters, StringComparer.OrdinalIgnoreCase);
                headers = headers
                    .Where(h => !string.IsNullOrWhiteSpace(h.ModelCode)
                        && modelSet.Contains(h.ModelCode.Trim()))
                    .ToList();
            }
        }
        else if (modelFilters.Count > 0)
        {
            // 机种在内存 Trim 比对（单选/多选一致），避免库内尾随空格导致漏产品
            // includeDeleted 须传仓储 includeSoftDeleted（ApplyReadScope 默认过滤软删）
            var headersAll = await _bomMaterialCostRepository.GetListAsync(
                exp.ToExpression(),
                asTableName: null,
                includeSoftDeleted: includeDeleted);
            var modelSet = new HashSet<string>(modelFilters, StringComparer.OrdinalIgnoreCase);
            headers = headersAll
                .Where(h => !string.IsNullOrWhiteSpace(h.ModelCode)
                    && modelSet.Contains(h.ModelCode.Trim()))
                .ToList();
        }
        else
        {
            headers = await _bomMaterialCostRepository.GetListAsync(
                exp.ToExpression(),
                asTableName: null,
                includeSoftDeleted: includeDeleted);
        }
        var productToModels = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
        foreach (var header in headers)
        {
            if (string.IsNullOrWhiteSpace(header.ProductCode))
            {
                continue;
            }
            var product = header.ProductCode.Trim();
            if (!productToModels.TryGetValue(product, out var models))
            {
                models = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                productToModels[product] = models;
            }
            if (!string.IsNullOrWhiteSpace(header.ModelCode))
            {
                models.Add(header.ModelCode.Trim());
            }
        }
        var productCodes = productToModels.Keys
            .OrderBy(c => c, StringComparer.Ordinal)
            .ToList();
        if (productCodes.Count == 0)
        {
            return (productCodes, productToModels, new List<TaktBomMaterialZeroPriceDto>());
        }

        var items = await LoadBomCostItemsForProductsAsync(
            plantCode,
            productCodes,
            costingStart,
            costingStart);
        // 视图只保留：生产相关=X、PCB SECT 标识为空、采购类型=F、移动平均价=0
        var zeroItems = items
            .Where(TaktBomMaterialCostItemLineCostHelper.QualifiesAsZeroPriceListLine)
            .ToList();
        if (zeroItems.Count >= MaxRowLoad)
        {
            ThrowBusinessException($"零价 BOM 明细行为 {zeroItems.Count}，达到上限 {MaxRowLoad}，请缩小机种范围");
        }

        var map = new Dictionary<string, MergedBomComponent>(StringComparer.OrdinalIgnoreCase);
        var modelsByComponent = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
        foreach (var item in zeroItems)
        {
            if (string.IsNullOrWhiteSpace(item.ComponentCode))
            {
                continue;
            }
            var code = item.ComponentCode.Trim();
            if (!map.TryGetValue(code, out var merged))
            {
                merged = new MergedBomComponent
                {
                    ComponentCode = code,
                    ComponentDescription = item.ComponentDescription?.Trim() ?? string.Empty,
                };
                map[code] = merged;
                modelsByComponent[code] = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            }
            else if (string.IsNullOrWhiteSpace(merged.ComponentDescription)
                && !string.IsNullOrWhiteSpace(item.ComponentDescription))
            {
                merged.ComponentDescription = item.ComponentDescription.Trim();
            }
            if (!string.IsNullOrWhiteSpace(item.ProductCode))
            {
                var product = item.ProductCode.Trim();
                merged.ProductCodes.Add(product);
                if (productToModels.TryGetValue(product, out var models))
                {
                    foreach (var m in models)
                    {
                        modelsByComponent[code].Add(m);
                    }
                }
            }
        }

        var allRows = map.Values
            .Select(c => new TaktBomMaterialZeroPriceDto
            {
                PlantCode = plantCode,
                ModelCode = modelsByComponent.TryGetValue(c.ComponentCode, out var ms)
                    ? string.Join(",", ms.OrderBy(m => m, StringComparer.OrdinalIgnoreCase))
                    : string.Empty,
                ComponentCode = c.ComponentCode,
                ComponentDescription = c.ComponentDescription,
                ProductCodes = string.Join(",", c.ProductCodes.OrderBy(p => p, StringComparer.Ordinal)),
                ProductCount = c.ProductCodes.Count,
                MovingAveragePrice = 0m,
                CostingPeriod = costingPeriod,
            })
            .OrderBy(r => r.ComponentCode, StringComparer.OrdinalIgnoreCase)
            .ThenBy(r => r.ModelCode, StringComparer.OrdinalIgnoreCase)
            .ToList();
        return (productCodes, productToModels, allRows);
    }

    /// <summary>
    /// 为零价组件填充建议移动价格：先用本组件自身，从核算月往前取最近一次移动价大于 0 的月份（当月无记录或价格不大于 0 视为没有购买，继续前推，直到 19 年内确认没有价）。自身仍无价时，再按末字母逆推替代编码，同样按月前推。
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="costingPeriod">核算月 yyyy-MM</param>
    /// <param name="rows">零价合并行</param>
    private async Task FillSuggestedRevisionsAsync(
        string plantCode,
        string costingPeriod,
        List<TaktBomMaterialZeroPriceDto> rows)
    {
        if (rows.Count == 0 || string.IsNullOrWhiteSpace(costingPeriod))
        {
            return;
        }

        var periodKey = costingPeriod.Trim();
        if (!DateTime.TryParseExact(
                periodKey + "-01",
                "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out var costingMonth))
        {
            return;
        }

        // 年分表查询跨度上限 20 年，这里取 19 年，覆盖核算月及以前全部有价月
        var lookbackStart = costingMonth.AddYears(-19).ToString("yyyy-MM");

        var candidateSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var candidatesByComponent = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        foreach (var row in rows)
        {
            var componentCode = row.ComponentCode?.Trim() ?? string.Empty;
            if (componentCode.Length == 0)
            {
                continue;
            }
            var candidates = new List<string> { componentCode };
            candidates.AddRange(EnumeratePreviousLetterRevisions(componentCode));
            candidatesByComponent[componentCode] = candidates;
            foreach (var code in candidates)
            {
                foreach (var variant in TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(code))
                {
                    candidateSet.Add(variant);
                }
            }
        }
        if (candidateSet.Count == 0)
        {
            return;
        }

        var samePeriodByMaterial = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
        var priceByMaterial = new Dictionary<string, (string Period, decimal Price)>(StringComparer.OrdinalIgnoreCase);
        const int chunkSize = 200;
        var lookupCodes = candidateSet.ToList();
        for (var i = 0; i < lookupCodes.Count; i += chunkSize)
        {
            var chunk = lookupCodes.Skip(i).Take(chunkSize).ToList();
            Expression<Func<TaktMaterialMovingPrice, bool>> predicate = x =>
                x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plantCode
                && chunk.Contains(x.MaterialCode)
                && x.MovingPrice > 0;
            var remaining = MaxRowLoad - Math.Max(samePeriodByMaterial.Count, priceByMaterial.Count);
            if (remaining <= 0)
            {
                break;
            }
            var part = await GetMovingPriceListForRangeAsync(
                predicate,
                lookbackStart,
                periodKey,
                remaining);
            foreach (var price in part)
            {
                if (string.IsNullOrWhiteSpace(price.MaterialCode) || price.MovingPrice <= 0m)
                {
                    continue;
                }
                var unitPrice = TaktBomMaterialCostItemLineCostHelper.ResolveMaterialMovingUnitPrice(price);
                if (unitPrice <= 0m)
                {
                    continue;
                }
                var ym = (price.ValuationPeriod ?? string.Empty).Trim().Replace('/', '-');
                if (ym.Length >= 7 && ym[4] == '-')
                {
                    ym = ym[..7];
                }
                if (ym.Length == 0
                    || string.CompareOrdinal(ym, lookbackStart) < 0
                    || string.CompareOrdinal(ym, periodKey) > 0)
                {
                    continue;
                }
                foreach (var key in TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(price.MaterialCode))
                {
                    if (string.Equals(ym, periodKey, StringComparison.Ordinal))
                    {
                        if (!samePeriodByMaterial.TryGetValue(key, out var existingSame)
                            || unitPrice > existingSame)
                        {
                            samePeriodByMaterial[key] = unitPrice;
                        }
                    }
                    if (!priceByMaterial.TryGetValue(key, out var existing)
                        || string.CompareOrdinal(ym, existing.Period) > 0
                        || (string.Equals(ym, existing.Period, StringComparison.Ordinal)
                            && unitPrice > existing.Price))
                    {
                        priceByMaterial[key] = (ym, unitPrice);
                    }
                }
            }
        }

        foreach (var row in rows)
        {
            var componentCode = row.ComponentCode?.Trim() ?? string.Empty;
            if (!candidatesByComponent.TryGetValue(componentCode, out var list))
            {
                continue;
            }
            foreach (var candidate in list)
            {
                decimal? suggestedPrice = null;
                foreach (var variant in TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(candidate))
                {
                    if (samePeriodByMaterial.TryGetValue(variant, out var exact) && exact > 0m)
                    {
                        suggestedPrice = exact;
                        break;
                    }
                }
                if (!suggestedPrice.HasValue)
                {
                    foreach (var variant in TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(candidate))
                    {
                        if (!priceByMaterial.TryGetValue(variant, out var hit))
                        {
                            continue;
                        }
                        suggestedPrice = hit.Price;
                        break;
                    }
                }
                if (!suggestedPrice.HasValue)
                {
                    continue;
                }
                row.SuggestedComponentCode = candidate;
                row.SuggestedMovingPrice = suggestedPrice.Value;
                break;
            }
        }
    }

    /// <summary>
    /// 按工厂+组件+核算月加载 BOM 明细（明细无软删业务，IsDeleted 恒 0）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="componentCode">组件编码</param>
    /// <param name="costingMonthStart">核算月初</param>
    /// <param name="costingMonthEnd">核算月末（同月初则整月）</param>
    /// <returns>明细行</returns>
    private async Task<List<TaktBomMaterialCostItem>> LoadBomCostItemsByComponentAsync(
        string plantCode,
        string componentCode,
        DateTime? costingMonthStart,
        DateTime? costingMonthEnd)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(plantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(componentCode);
        var component = componentCode.Trim();
        var componentLookup = TaktBomMaterialCostItemLineCostHelper
            .ExpandProductCodeLookupVariants(component)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        var plant = plantCode.Trim();
        DateTime? costingExclusiveEnd = costingMonthEnd.HasValue
            ? costingMonthEnd.Value.AddMonths(1)
            : null;
        var exp = Expressionable.Create<TaktBomMaterialCostItem>();
        exp = exp.And(x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.PlantCode == plant
            && componentLookup.Contains(x.ComponentCode));
        if (costingMonthStart.HasValue)
        {
            var start = costingMonthStart.Value;
            exp = exp.And(x => x.CostingDate >= start);
        }
        if (costingExclusiveEnd.HasValue)
        {
            var endExclusive = costingExclusiveEnd.Value;
            exp = exp.And(x => x.CostingDate < endExclusive);
        }
        string? yearTable;
        try
        {
            yearTable = await ResolveBomItemPhysicalTableAsync(
                TaktYearShardTableHelper.RequireSingleYear(costingMonthStart, costingMonthEnd));
        }
        catch (ArgumentException ex)
        {
            throw new TaktBusinessException(ex.Message);
        }
        var part = await _bomMaterialCostItemRepository.GetListAsync(exp.ToExpression(), yearTable);
        // 调用方跳过 PcbSectIndicator 已标记行
        return part.ToList();
    }

    /// <summary>
    /// 零价格视图取数：只查生产相关=X、PCB SECT 标识为空、采购类型=F、移动平均价=0
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="productCodes">产品编码</param>
    /// <param name="costingMonthStart">核算月初</param>
    /// <param name="costingMonthEnd">核算月末（同月初则整月）</param>
    /// <returns>明细行</returns>
    private async Task<List<TaktBomMaterialCostItem>> LoadBomCostItemsForProductsAsync(
        string plantCode,
        IReadOnlyList<string> productCodes,
        DateTime? costingMonthStart,
        DateTime? costingMonthEnd)
    {
        var raw = await LoadBomCostItemsRawForProductsAsync(
            plantCode,
            productCodes,
            costingMonthStart,
            costingMonthEnd,
            zeroPriceViewScope: true);
        return raw
            .Where(TaktBomMaterialCostItemLineCostHelper.QualifiesAsZeroPriceListLine)
            .ToList();
    }

    /// <summary>
    /// 按年分表查询移动价格
    /// </summary>
    private async Task<List<TaktMaterialMovingPrice>> GetMovingPriceListForRangeAsync(
        Expression<Func<TaktMaterialMovingPrice, bool>> predicate,
        string? valuationPeriodStart,
        string? valuationPeriodEnd,
        int? maxRows = null)
    {
        var years = TaktYearShardTableHelper.ResolveYearsFromYyyyMmPeriod(valuationPeriodStart, valuationPeriodEnd);
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
            result.AddRange(basePart.Where(r =>
                !string.IsNullOrWhiteSpace(r.ValuationPeriod)
                && r.ValuationPeriod.Length >= 4
                && int.TryParse(r.ValuationPeriod.AsSpan(0, 4), out var y)
                && yearSet.Contains(y)));
        }
        return result;
    }

    /// <summary>
    /// 成本汇总表：纯数字产品编码去前导 0
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="result">累计结果</param>
    private async Task StripHeaderProductLeadingZerosAsync(
        string plantCode,
        TaktBomMaterialZeroPriceStripLeadingZeroResultDto result)
    {
        long lastId = 0;
        const int pageSize = 100;
        for (var guard = 0; guard < 100000; guard++)
        {
            var (items, _) = await _bomMaterialCostRepository.GetPagedAsync(
                x => x.PlantCode == plantCode && x.Id > lastId && x.ProductCode.StartsWith("0"),
                1,
                pageSize,
                x => x.Id,
                false);
            if (items.Count == 0)
            {
                break;
            }
            lastId = items[^1].Id;
            var planned = new List<(TaktBomMaterialCost Row, string NewCode)>();
            foreach (var row in items)
            {
                var stripped = TryStripPureNumericLeadingZeros(row.ProductCode);
                if (stripped == null)
                {
                    continue;
                }
                planned.Add((row, stripped));
            }
            if (planned.Count == 0)
            {
                continue;
            }
            var codes = planned.Select(x => x.NewCode).Distinct(StringComparer.Ordinal).ToList();
            var existing = await _bomMaterialCostRepository.GetListAsync(
                x => x.PlantCode == plantCode && codes.Contains(x.ProductCode),
                includeSoftDeleted: true);
            var occupied = existing
                .Select(x => HeaderProductKey(x.PlantCode, x.ProductCode, x.CostingDate))
                .ToHashSet(StringComparer.Ordinal);
            var toSave = new List<TaktBomMaterialCost>();
            foreach (var (row, newCode) in planned)
            {
                var key = HeaderProductKey(row.PlantCode, newCode, row.CostingDate);
                if (!occupied.Add(key))
                {
                    result.SkippedConflictCount = checked(result.SkippedConflictCount + 1);
                    continue;
                }
                row.ProductCode = newCode;
                toSave.Add(row);
            }
            if (toSave.Count > 0)
            {
                await _bomMaterialCostRepository.UpdateRangeAsync(toSave);
                result.HeaderProductUpdatedCount = checked(result.HeaderProductUpdatedCount + toSave.Count);
            }
        }
    }

    /// <summary>
    /// 明细物理表：纯数字产品编码、组件编码去前导 0
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="table">物理表名</param>
    /// <param name="result">累计结果</param>
    private async Task StripItemCodeLeadingZerosAsync(
        string plantCode,
        string table,
        TaktBomMaterialZeroPriceStripLeadingZeroResultDto result)
    {
        long lastId = 0;
        const int pageSize = 100;
        for (var guard = 0; guard < 100000; guard++)
        {
            var (items, _) = await _bomMaterialCostItemRepository.GetPagedAsync(
                x => x.PlantCode == plantCode
                    && x.Id > lastId
                    && (x.ProductCode.StartsWith("0") || x.ComponentCode.StartsWith("0")),
                1,
                pageSize,
                x => x.Id,
                false,
                table);
            if (items.Count == 0)
            {
                break;
            }
            lastId = items[^1].Id;
            var planned = new List<(TaktBomMaterialCostItem Row, string ProductCode, string ComponentCode, bool ProductChanged, bool ComponentChanged)>();
            foreach (var row in items)
            {
                var product = TryStripPureNumericLeadingZeros(row.ProductCode);
                var component = TryStripPureNumericLeadingZeros(row.ComponentCode);
                if (product == null && component == null)
                {
                    continue;
                }
                planned.Add((
                    row,
                    product ?? row.ProductCode.Trim(),
                    component ?? row.ComponentCode.Trim(),
                    product != null,
                    component != null));
            }
            if (planned.Count == 0)
            {
                continue;
            }
            var productCodes = planned.Select(x => x.ProductCode).Distinct(StringComparer.Ordinal).ToList();
            var componentCodes = planned.Select(x => x.ComponentCode).Distinct(StringComparer.Ordinal).ToList();
            var existing = await _bomMaterialCostItemRepository.GetListAsync(
                x => x.PlantCode == plantCode
                    && (productCodes.Contains(x.ProductCode) || componentCodes.Contains(x.ComponentCode)),
                table,
                includeSoftDeleted: true);
            var occupied = existing
                .Select(ItemLineKey)
                .ToHashSet(StringComparer.Ordinal);
            var toSave = new List<TaktBomMaterialCostItem>();
            foreach (var plan in planned)
            {
                var key = ItemLineKey(plan.Row.PlantCode, plan.Row.BomLevel, plan.Row.BomItemCode, plan.ProductCode, plan.Row.LineNumber, plan.ComponentCode, plan.Row.CostingDate);
                if (!occupied.Add(key))
                {
                    result.SkippedConflictCount = checked(result.SkippedConflictCount + 1);
                    continue;
                }
                if (plan.ProductChanged)
                {
                    plan.Row.ProductCode = plan.ProductCode;
                    result.ItemProductUpdatedCount = checked(result.ItemProductUpdatedCount + 1);
                }
                if (plan.ComponentChanged)
                {
                    plan.Row.ComponentCode = plan.ComponentCode;
                    result.ItemComponentUpdatedCount = checked(result.ItemComponentUpdatedCount + 1);
                }
                toSave.Add(plan.Row);
            }
            if (toSave.Count > 0)
            {
                await _bomMaterialCostItemRepository.UpdateRangeAsync(toSave, table);
            }
        }
    }

    /// <summary>
    /// 明细基表与已存在的年分表
    /// </summary>
    /// <returns>物理表名</returns>
    private async Task<List<string>> ListBomItemPhysicalTablesAsync()
    {
        var tables = new List<string>();
        if (await _bomMaterialCostItemRepository.PhysicalTableExistsAsync(BomItemYearShardBaseTable))
        {
            tables.Add(BomItemYearShardBaseTable);
        }
        var year = DateTime.Now.Year;
        for (var cursor = year - 15; cursor <= year + 1; cursor++)
        {
            var name = TaktYearShardTableHelper.BuildYearTableName(BomItemYearShardBaseTable, cursor);
            if (await _bomMaterialCostItemRepository.PhysicalTableExistsAsync(name))
            {
                tables.Add(name);
            }
        }
        return tables;
    }

    /// <summary>
    /// 纯数字且有前导 0 时去掉前导 0；全 0 保留一个 0；含非数字则不改
    /// </summary>
    /// <param name="code">原编码</param>
    /// <returns>新编码；无需修改时 null</returns>
    private static string? TryStripPureNumericLeadingZeros(string? code)
    {
        var text = code?.Trim() ?? string.Empty;
        if (text.Length < 2 || text[0] != '0')
        {
            return null;
        }
        foreach (var ch in text)
        {
            if (ch < '0' || ch > '9')
            {
                return null;
            }
        }
        var stripped = text.TrimStart('0');
        if (stripped.Length == 0)
        {
            stripped = "0";
        }
        return stripped == text ? null : stripped;
    }

    /// <summary>
    /// 成本汇总唯一键
    /// </summary>
    private static string HeaderProductKey(string plantCode, string productCode, DateTime costingDate)
    {
        return string.Join('|', plantCode.Trim(), productCode.Trim(), costingDate.Ticks.ToString(System.Globalization.CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// 明细行唯一键
    /// </summary>
    private static string ItemLineKey(TaktBomMaterialCostItem row)
    {
        return ItemLineKey(row.PlantCode, row.BomLevel, row.BomItemCode, row.ProductCode, row.LineNumber, row.ComponentCode, row.CostingDate);
    }

    /// <summary>
    /// 明细行唯一键
    /// </summary>
    private static string ItemLineKey(
        string plantCode,
        string bomLevel,
        string bomItemCode,
        string productCode,
        int lineNumber,
        string componentCode,
        DateTime costingDate)
    {
        return string.Join(
            '|',
            plantCode.Trim(),
            bomLevel.Trim(),
            bomItemCode.Trim(),
            productCode.Trim(),
            lineNumber.ToString(System.Globalization.CultureInfo.InvariantCulture),
            componentCode.Trim(),
            costingDate.Ticks.ToString(System.Globalization.CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// 解析 BOM 成本明细物理表
    /// </summary>
    private async Task<string?> ResolveBomItemPhysicalTableAsync(int year)
    {
        var table = TaktYearShardTableHelper.BuildYearTableName(BomItemYearShardBaseTable, year);
        return await _bomMaterialCostItemRepository.PhysicalTableExistsAsync(table) ? table : null;
    }

    /// <summary>
    /// 当前连接（选中库）上本次核算年对应的明细物理表；年分表不存在则用基表
    /// </summary>
    /// <param name="costingStart">核算月初</param>
    /// <returns>物理表名</returns>
    private async Task<string> ResolveBomItemTableLabelAsync(DateTime costingStart)
    {
        var yearTable = await ResolveBomItemPhysicalTableAsync(costingStart.Year);
        return yearTable ?? BomItemYearShardBaseTable;
    }

    /// <summary>
    /// 解析移动价格物理表
    /// </summary>
    /// <param name="year">年份</param>
    /// <returns>存在则返回表名；否则 null</returns>
    private async Task<string?> ResolveMovingPricePhysicalTableAsync(int year)
    {
        var table = TaktYearShardTableHelper.BuildYearTableName(MovingPriceYearShardBaseTable, year);
        return await _materialMovingPriceRepository.PhysicalTableExistsAsync(table) ? table : null;
    }

    /// <summary>
    /// 逆推建议代替源：末字母前推；优先核算月同 ValuationPeriod，否则核算月及以前最近有价月；返回移动价原值/单位/币种
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="costingPeriod">核算月 yyyy-MM</param>
    /// <param name="componentCode">零价组件</param>
    /// <returns>源价；无则 null</returns>
    private async Task<SuggestedMovingSource?> ResolveSuggestedMovingSourceAsync(
        string plantCode,
        string costingPeriod,
        string componentCode)
    {
        var periodKey = costingPeriod.Trim();
        if (!DateTime.TryParseExact(
                periodKey + "-01",
                "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out var costingMonth))
        {
            return null;
        }
        var candidates = EnumeratePreviousLetterRevisions(componentCode).ToList();
        if (candidates.Count == 0)
        {
            return null;
        }
        var lookbackStart = costingMonth.AddMonths(-24).ToString("yyyy-MM");
        var candidateSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var code in candidates)
        {
            foreach (var variant in TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(code))
            {
                candidateSet.Add(variant);
            }
        }
        // key → (源码、期间、原价、单位、币种)；同期间优先，否则期间最新
        var samePeriod = new Dictionary<string, SuggestedMovingSource>(StringComparer.OrdinalIgnoreCase);
        var anyPeriod = new Dictionary<string, SuggestedMovingSource>(StringComparer.OrdinalIgnoreCase);
        const int chunkSize = 200;
        var lookupCodes = candidateSet.ToList();
        for (var i = 0; i < lookupCodes.Count; i += chunkSize)
        {
            var chunk = lookupCodes.Skip(i).Take(chunkSize).ToList();
            Expression<Func<TaktMaterialMovingPrice, bool>> predicate = x =>
                x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plantCode
                && chunk.Contains(x.MaterialCode)
                && x.MovingPrice > 0;
            var remaining = MaxRowLoad - Math.Max(samePeriod.Count, anyPeriod.Count);
            if (remaining <= 0)
            {
                break;
            }
            var part = await GetMovingPriceListForRangeAsync(
                predicate,
                lookbackStart,
                periodKey,
                remaining);
            foreach (var price in part)
            {
                if (string.IsNullOrWhiteSpace(price.MaterialCode) || price.MovingPrice <= 0m)
                {
                    continue;
                }
                var ym = TaktBomMaterialZeroPriceMovingBackfillHelper.NormalizeValuationPeriod(price.ValuationPeriod);
                if (ym.Length == 0
                    || string.CompareOrdinal(ym, lookbackStart) < 0
                    || string.CompareOrdinal(ym, periodKey) > 0)
                {
                    continue;
                }
                var unit = price.PriceUnit <= 0 ? 1 : price.PriceUnit;
                var currency = price.CurrencyCode?.Trim() ?? string.Empty;
                var material = price.MaterialCode.Trim();
                var hit = new SuggestedMovingSource
                {
                    SourceComponentCode = material,
                    ValuationPeriod = ym,
                    MovingPrice = price.MovingPrice,
                    PriceUnit = unit,
                    CurrencyCode = currency,
                };
                foreach (var key in TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(material))
                {
                    if (string.Equals(ym, periodKey, StringComparison.Ordinal))
                    {
                        if (!samePeriod.TryGetValue(key, out var existingSame)
                            || price.MovingPrice > existingSame.MovingPrice)
                        {
                            samePeriod[key] = hit;
                        }
                    }
                    if (!anyPeriod.TryGetValue(key, out var existing)
                        || string.CompareOrdinal(ym, existing.ValuationPeriod) > 0
                        || (string.Equals(ym, existing.ValuationPeriod, StringComparison.Ordinal)
                            && price.MovingPrice > existing.MovingPrice))
                    {
                        anyPeriod[key] = hit;
                    }
                }
            }
        }
        foreach (var candidate in candidates)
        {
            foreach (var variant in TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(candidate))
            {
                if (samePeriod.TryGetValue(variant, out var exact))
                {
                    exact.SourceComponentCode = candidate;
                    return exact;
                }
            }
            foreach (var variant in TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(candidate))
            {
                if (!anyPeriod.TryGetValue(variant, out var hit))
                {
                    continue;
                }
                hit.SourceComponentCode = candidate;
                return hit;
            }
        }
        return null;
    }

    /// <summary>
    /// 枚举组件编码末字母前推版本
    /// </summary>
    /// <param name="componentCode">组件编码</param>
    /// <returns>按字母逆序的前一版本编码</returns>
    private static IEnumerable<string> EnumeratePreviousLetterRevisions(string componentCode)
    {
        if (string.IsNullOrWhiteSpace(componentCode))
        {
            yield break;
        }
        var code = componentCode.Trim();
        if (code.Length < 2)
        {
            yield break;
        }
        var last = code[^1];
        if (!char.IsAsciiLetter(last))
        {
            yield break;
        }
        var prefix = code[..^1];
        var min = char.IsUpper(last) ? 'A' : 'a';
        for (var c = (char)(last - 1); c >= min; c--)
        {
            yield return prefix + c;
        }
    }

    /// <summary>
    /// 合并组件中间结果
    /// </summary>
    private sealed class MergedBomComponent
    {
        /// <summary>
        /// 组件编码
        /// </summary>
        public string ComponentCode { get; set; } = string.Empty;

        /// <summary>
        /// 组件描述
        /// </summary>
        public string ComponentDescription { get; set; } = string.Empty;

        /// <summary>
        /// 共用产品编码集合
        /// </summary>
        public HashSet<string> ProductCodes { get; } = new(StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 建议代替移动价源（原值，非÷单位）
    /// </summary>
    private sealed class SuggestedMovingSource
    {
        /// <summary>
        /// 建议代替组件编码
        /// </summary>
        public string SourceComponentCode { get; set; } = string.Empty;

        /// <summary>
        /// 评估期间 yyyy-MM
        /// </summary>
        public string ValuationPeriod { get; set; } = string.Empty;

        /// <summary>
        /// 移动价格原值
        /// </summary>
        public decimal MovingPrice { get; set; }

        /// <summary>
        /// 价格单位
        /// </summary>
        public int PriceUnit { get; set; } = 1;

        /// <summary>
        /// 币种
        /// </summary>
        public string CurrencyCode { get; set; } = string.Empty;
    }

    /// <summary>
    /// 步骤1 写价后的单行成本差额（供步骤2 增量加到产品月成本）
    /// </summary>
    private sealed class MovingPriceLineDelta
    {
        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; } = string.Empty;

        /// <summary>
        /// 组件业务键
        /// </summary>
        public string ComponentKey { get; set; } = string.Empty;

        /// <summary>
        /// 核算日
        /// </summary>
        public DateTime CostingDate { get; set; }

        /// <summary>
        /// 是否软删
        /// </summary>
        public int IsDeleted { get; set; }

        /// <summary>
        /// 明细 Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 行成本差额（新−旧）
        /// </summary>
        public decimal Delta { get; set; }

        /// <summary>
        /// 币种
        /// </summary>
        public string CurrencyCode { get; set; } = string.Empty;
    }
}
