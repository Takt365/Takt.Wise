// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialCostItemLineCostHelper.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 物料成本行金额与期间快照纯计算辅助；参与计算须 ProductionRelated=X、PcbSectIndicator 为空、PurchaseType=F；行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 物料成本行金额计算（无状态纯函数）
/// </summary>
public static class TaktBomMaterialCostItemLineCostHelper
{
    /// <summary>
    /// 成品物料类型（字典 logistics_materials_material_type；成本合计仅统计此类型。机种月均按工厂+物料类型+机种+月份分组，各类型各自平均；空类型分组视为 FERT）
    /// </summary>
    public const string FertMaterialTypeCode = "FERT";

    /// <summary>
    /// 成本/单价金额小数位数（行成本、产品月成本、机种月均、差异金额等统一口径）
    /// </summary>
    public const int CostDecimalDigits = 5;

    /// <summary>
    /// 环比百分点小数位数（界面/API：-0.34 表示 -0.34%）
    /// </summary>
    public const int PercentPointsDecimalDigits = 2;

    /// <summary>
    /// Excel 百分比单元格小数位数（÷100 后保留 4 位，匹配 0.00% 两位百分点）
    /// </summary>
    public const int ExcelPercentDecimalDigits = 4;

    /// <summary>
    /// 组件描述中「PCB 区段」标记（含子树排除；大小写不敏感）
    /// </summary>
    public const string PcbSectDescriptionMarker = "PCB SECT";

    /// <summary>
    /// PCB SECT 标识列取值（与生产相关等 X 标记一致）
    /// </summary>
    public const string PcbSectIndicatorMarkValue = "X";

    /// <summary>
    /// 用量参与下限（仅最近采购成本合计等仍用；成本合计与零价清单不再用用量门槛）
    /// </summary>
    public const decimal MinParticipatingComponentQuantity = 0.001m;

    /// <summary>
    /// 是否参与成本合计/分析统计（资格已由 CountsTowardBomMaterialCostItem：X + PcbSectIndicator 空 + F 判定；本方法恒为 true，保留调用点兼容）
    /// </summary>
    /// <param name="row">成本明细行</param>
    /// <returns>恒为 true</returns>
    public static bool CountsTowardCostStatistics(TaktBomMaterialCostItem row)
    {
        ArgumentNullException.ThrowIfNull(row);
        return true;
    }

    /// <summary>
    /// 是否列入零价清单/0价格组（仅移动平均价=0；资格由 QualifiesAsZeroPriceListLine 再筛 X+空标识+F）
    /// </summary>
    /// <param name="row">成本明细行</param>
    /// <returns>是否列入零价统计</returns>
    public static bool CountsTowardZeroPriceList(TaktBomMaterialCostItem row)
    {
        ArgumentNullException.ThrowIfNull(row);
        return row.MovingAveragePrice == 0m;
    }

    /// <summary>
    /// 金额四舍五入（AwayFromZero，保留 CostDecimalDigits 位）
    /// </summary>
    /// <param name="value">原始金额</param>
    /// <returns>舍入后金额</returns>
    public static decimal RoundCost(decimal value)
    {
        return Math.Round(value, CostDecimalDigits, MidpointRounding.AwayFromZero);
    }

    /// <summary>
    /// 将变动比率转为百分点（×100，保留两位）
    /// </summary>
    /// <param name="ratio">差额÷基准（如 -0.0034）</param>
    /// <returns>百分点（如 -0.34）</returns>
    public static decimal RoundPercentPoints(decimal ratio)
    {
        return Math.Round(ratio * 100m, PercentPointsDecimalDigits, MidpointRounding.AwayFromZero);
    }

    /// <summary>
    /// 百分点 → Excel 百分比小数（÷100）；Excel 设为百分比格式后显示与界面一致
    /// </summary>
    /// <param name="percentPoints">百分点（如 -0.34）</param>
    /// <returns>Excel 用小数（如 -0.0034）；空则 null</returns>
    public static decimal? ToExcelPercent(decimal? percentPoints)
    {
        if (!percentPoints.HasValue || percentPoints.Value == 0m)
        {
            return null;
        }
        return Math.Round(
            percentPoints.Value / 100m,
            ExcelPercentDecimalDigits,
            MidpointRounding.AwayFromZero);
    }

    /// <summary>
    /// 规范化核算日期：仅保留日历日，Kind=Unspecified（UTC 先转本地再取日，避免串月）
    /// </summary>
    /// <param name="costingDate">原始核算日期</param>
    /// <returns>日期部分（00:00:00 Unspecified）</returns>
    public static DateTime NormalizeCostingDate(DateTime costingDate)
    {
        var local = costingDate.Kind == DateTimeKind.Utc
            ? costingDate.ToLocalTime()
            : costingDate;
        return DateTime.SpecifyKind(local.Date, DateTimeKind.Unspecified);
    }

    /// <summary>
    /// 将核算日期映射为期间键 yyyy-MM（基于规范化后的日历日）
    /// </summary>
    /// <param name="costingDate">核算日期</param>
    /// <returns>期间键</returns>
    public static string ToPeriodKey(DateTime costingDate)
    {
        var d = NormalizeCostingDate(costingDate);
        return $"{d.Year:D4}-{d.Month:D2}";
    }

    /// <summary>
    /// 组件描述是否含 PCB SECT（不参与成本分析的区段标记）
    /// </summary>
    /// <param name="componentDescription">组件描述</param>
    /// <returns>是否匹配</returns>
    public static bool IsPcbSectComponentDescription(string? componentDescription)
    {
        if (string.IsNullOrWhiteSpace(componentDescription))
        {
            return false;
        }
        return componentDescription.Contains(PcbSectDescriptionMarker, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// PCB SECT 标识是否为空（空/空白才可参与成本合计与零价清单；非空一律排除）
    /// </summary>
    /// <param name="pcbSectIndicator">PCB SECT 标识列</param>
    /// <returns>是否为空</returns>
    public static bool IsPcbSectIndicatorEmpty(string? pcbSectIndicator)
    {
        return string.IsNullOrWhiteSpace(pcbSectIndicator);
    }

    /// <summary>
    /// 行是否已打 PCB SECT 标识（PcbSectIndicator=X）
    /// </summary>
    /// <param name="pcbSectIndicator">PCB SECT 标识列</param>
    /// <returns>是否已标记</returns>
    public static bool HasPcbSectIndicatorMark(string? pcbSectIndicator)
    {
        return string.Equals(
            pcbSectIndicator?.Trim(),
            PcbSectIndicatorMarkValue,
            StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 是否为 PCB SECT 树根/已标记节点（描述含 PCB SECT，或 PcbSectIndicator=X）
    /// </summary>
    /// <param name="row">成本明细行</param>
    /// <returns>是否 PCB SECT 节点</returns>
    public static bool IsPcbSectTreeNode(TaktBomMaterialCostItem row)
    {
        ArgumentNullException.ThrowIfNull(row);
        return IsPcbSectComponentDescription(row.ComponentDescription)
            || HasPcbSectIndicatorMark(row.PcbSectIndicator);
    }

    /// <summary>
    /// 写入 PcbSectIndicator=X（已存在则不变）
    /// </summary>
    /// <param name="row">成本明细行</param>
    /// <returns>本次是否改写了标识列</returns>
    public static bool TryApplyPcbSectIndicatorMark(TaktBomMaterialCostItem row)
    {
        ArgumentNullException.ThrowIfNull(row);
        if (HasPcbSectIndicatorMark(row.PcbSectIndicator))
        {
            return false;
        }
        row.PcbSectIndicator = PcbSectIndicatorMarkValue;
        return true;
    }

    /// <summary>
    /// 是否参与 BOM 材料成本汇总（生产相关=X 且 PCB SECT 标识为空 且 采购类型=F；比较前 Trim）
    /// </summary>
    /// <param name="row">成本行</param>
    /// <returns>是否计入产品 BOM 材料成本</returns>
    public static bool CountsTowardBomMaterialCostItem(TaktBomMaterialCostItem row)
    {
        ArgumentNullException.ThrowIfNull(row);
        return string.Equals(row.ProductionRelated?.Trim(), "X", StringComparison.OrdinalIgnoreCase)
            && string.Equals(row.PurchaseType?.Trim(), "F", StringComparison.OrdinalIgnoreCase)
            && IsPcbSectIndicatorEmpty(row.PcbSectIndicator);
    }

    /// <summary>
    /// 是否可作为零价格清单的「单行」候选：生产相关=X、PCB SECT 标识为空、采购类型=F，且移动平均价=0（与用量无关）。
    /// 同一 ComponentCode 在不同 BomLevel / LineNumber / BomItemCode 上的多笔须各自判定；任一笔满足即可进入合并清单。
    /// </summary>
    /// <param name="row">成本明细行</param>
    /// <returns>是否计入零价清单</returns>
    public static bool QualifiesAsZeroPriceListLine(TaktBomMaterialCostItem row)
    {
        ArgumentNullException.ThrowIfNull(row);
        return CountsTowardBomMaterialCostItem(row) && CountsTowardZeroPriceList(row);
    }

    /// <summary>
    /// 按 BOM 展开结构收集「组件描述含 PCB SECT 或 PcbSectIndicator=X」的节点及其子层级整树。
    /// </summary>
    /// <param name="rows">同一批展开行（可含非 X+F；须含完整树）</param>
    /// <returns>PCB SECT 整树行</returns>
    public static IEnumerable<TaktBomMaterialCostItem> CollectPcbSectHierarchyRows(
        IEnumerable<TaktBomMaterialCostItem> rows)
    {
        ArgumentNullException.ThrowIfNull(rows);
        var list = rows as IList<TaktBomMaterialCostItem> ?? rows.ToList();
        if (list.Count == 0)
        {
            return list;
        }
        var keys = BuildPcbSectHierarchyKeys(list);
        if (keys.Count == 0)
        {
            return Array.Empty<TaktBomMaterialCostItem>();
        }
        return list.Where(r => keys.Contains(BuildHierarchyExclusionKey(r)));
    }

    /// <summary>
    /// 按 BOM 展开结构排除「组件描述含 PCB SECT 或 PcbSectIndicator=X」的节点及其子层级（须在 X+F 筛选前对全量展开行判定，以免父件非 X+F 时漏掉子树）。
    /// 以工厂+规范化产品码+核算日为一棵树；按展开序维护祖先栈，仅排除 PCB SECT 节点自身及其子孙，不误伤其它分支同组件的其它位置行。
    /// </summary>
    /// <param name="rows">同一批展开行（可含非 X+F；须含完整树以便识别 PCB SECT 父节点）</param>
    /// <returns>排除 PCB SECT 子树后的行</returns>
    public static IEnumerable<TaktBomMaterialCostItem> ExcludePcbSectHierarchyRows(
        IEnumerable<TaktBomMaterialCostItem> rows)
    {
        ArgumentNullException.ThrowIfNull(rows);
        var list = rows as IList<TaktBomMaterialCostItem> ?? rows.ToList();
        if (list.Count == 0)
        {
            return list;
        }
        var excludedKeys = BuildPcbSectHierarchyKeys(list);
        if (excludedKeys.Count == 0)
        {
            return list;
        }
        return list.Where(r => !excludedKeys.Contains(BuildHierarchyExclusionKey(r)));
    }

    /// <summary>
    /// 扫描展开树，返回 PCB SECT 整树行键集合（描述含 PCB SECT 或标识列已标，及其子孙）
    /// </summary>
    /// <param name="list">展开行</param>
    /// <returns>行排除/收集键</returns>
    private static HashSet<string> BuildPcbSectHierarchyKeys(IList<TaktBomMaterialCostItem> list)
    {
        ArgumentNullException.ThrowIfNull(list);
        var keys = new HashSet<string>(StringComparer.Ordinal);
        foreach (var tree in list.GroupBy(r => (
            Plant: (r.PlantCode ?? string.Empty).Trim(),
            Product: NormalizeProductCodeForTree(r.ProductCode),
            Costing: NormalizeCostingDate(r.CostingDate))))
        {
            var ordered = tree
                .OrderBy(r => r, BomExplosionRowComparer.Instance)
                .ToList();
            var ancestorStack = new List<(int Depth, bool UnderPcbSect)>();
            foreach (var row in ordered)
            {
                var depth = ParseBomLevelDotDepth(row.BomLevel);
                while (ancestorStack.Count > 0 && ancestorStack[^1].Depth >= depth)
                {
                    ancestorStack.RemoveAt(ancestorStack.Count - 1);
                }
                var underPcbSect = ancestorStack.Count > 0 && ancestorStack.Exists(a => a.UnderPcbSect);
                var isPcbSectNode = IsPcbSectTreeNode(row);
                if (underPcbSect || isPcbSectNode)
                {
                    keys.Add(BuildHierarchyExclusionKey(row));
                }
                ancestorStack.Add((depth, underPcbSect || isPcbSectNode));
            }
        }
        return keys;
    }

    /// <summary>
 /// 展开树分组用产品码（18 位纯数字 码归一为后 10 位，避免同产品不同写法拆成两棵残树）
    /// </summary>
    /// <param name="productCode">产品编码</param>
    /// <returns>规范化产品码</returns>
    public static string NormalizeProductCodeForTree(string? productCode)
    {
        if (string.IsNullOrWhiteSpace(productCode))
        {
            return string.Empty;
        }
        var trimmed = productCode.Trim();
        var normalized = TaktStringHelper.NormalizeSapNumericMaterialCode(trimmed);
        return string.IsNullOrEmpty(normalized) ? trimmed : normalized;
    }

    /// <summary>
    /// PCB SECT 子树排除键（同展开树内唯一定位一行）
    /// </summary>
    private static string BuildHierarchyExclusionKey(TaktBomMaterialCostItem row)
    {
        ArgumentNullException.ThrowIfNull(row);
        return string.Join(
            "|",
            row.Id.ToString(System.Globalization.CultureInfo.InvariantCulture),
            BuildComponentKey(row),
            NormalizeCostingDate(row.CostingDate).ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// 筛选参与 BOM 材料成本汇总的行：ProductionRelated=X 且 PcbSectIndicator 为空 且 PurchaseType=F
    /// </summary>
    /// <param name="rows">成本行</param>
    /// <returns>可参与计算分析的行</returns>
    public static IEnumerable<TaktBomMaterialCostItem> FilterBomMaterialCostItemRows(IEnumerable<TaktBomMaterialCostItem> rows)
    {
        ArgumentNullException.ThrowIfNull(rows);
        return rows.Where(CountsTowardBomMaterialCostItem);
    }

    /// <summary>
    /// 计算单行组件成本（仅生产相关=X、PCB SECT 标识为空、采购类型=F：组件数量×(移动平均价÷移动价格单位)，保留 5 位小数；否则返回 0）
    /// </summary>
    /// <param name="row">成本行</param>
    /// <returns>行成本</returns>
    public static decimal CalculateLineCost(TaktBomMaterialCostItem row)
    {
        ArgumentNullException.ThrowIfNull(row);
        if (!CountsTowardBomMaterialCostItem(row))
        {
            return 0m;
        }
        var priceUnit = ResolveMovingPriceUnit(row);
        var unitPrice = row.MovingAveragePrice / priceUnit;
        return RoundCost(row.ComponentQuantity * unitPrice);
    }

    /// <summary>
    /// 计算单行最近采购成本（仅生产相关=X、PCB SECT 标识为空且 F：组件数量×(净价÷采购价格单位)，保留 5 位小数；否则返回 0）
    /// </summary>
    /// <param name="row">成本行</param>
    /// <returns>行采购成本</returns>
    public static decimal CalculateLinePurchaseCost(TaktBomMaterialCostItem row)
    {
        ArgumentNullException.ThrowIfNull(row);
        if (!CountsTowardBomMaterialCostItem(row) || !CountsTowardPurchaseCostStatistics(row))
        {
            return 0m;
        }
        var priceUnit = ResolvePurchasePriceUnit(row);
        var unitPrice = row.NetPurchasePrice / priceUnit;
        return RoundCost(row.ComponentQuantity * unitPrice);
    }

    /// <summary>
    /// 是否参与最近采购成本合计（参与资格由 CountsTowardBomMaterialCostItem 先筛；本方法：用量 &gt; 0.001 且净价 ≠ 0）
    /// </summary>
    /// <param name="row">成本明细行</param>
    /// <returns>是否参与采购成本统计</returns>
    public static bool CountsTowardPurchaseCostStatistics(TaktBomMaterialCostItem row)
    {
        ArgumentNullException.ThrowIfNull(row);
        return row.ComponentQuantity > MinParticipatingComponentQuantity
            && row.NetPurchasePrice != 0m;
    }

    /// <summary>
    /// 取采购价格单位（≤0 时按 1 处理）
    /// </summary>
    /// <param name="row">成本行</param>
    /// <returns>采购价格单位</returns>
    public static int ResolvePurchasePriceUnit(TaktBomMaterialCostItem row)
    {
        ArgumentNullException.ThrowIfNull(row);
        return row.PurchasePriceUnit <= 0 ? 1 : row.PurchasePriceUnit;
    }

    /// <summary>
    /// 取有效单价（汇总口径仅生产相关=X、PCB SECT 标识为空且 F：移动平均价原值，保留 5 位小数；与 ResolvePriceUnit 配对后得每基本计量单位价；否则返回 0）
    /// </summary>
    /// <param name="row">成本行</param>
    /// <returns>移动平均价（未除以价格单位）</returns>
    public static decimal ResolveEffectiveUnitPrice(TaktBomMaterialCostItem row)
    {
        ArgumentNullException.ThrowIfNull(row);
        if (!CountsTowardBomMaterialCostItem(row))
        {
            return 0m;
        }
        return RoundCost(row.MovingAveragePrice);
    }

    /// <summary>
    /// 取每基本计量单位单价（生产相关=X 且 F：移动平均价÷移动价格单位，保留 5 位小数；否则 0）
    /// </summary>
    /// <param name="row">成本行</param>
    /// <returns>单位单价</returns>
    public static decimal ResolvePerBaseUnitPrice(TaktBomMaterialCostItem row)
    {
        ArgumentNullException.ThrowIfNull(row);
        if (!CountsTowardBomMaterialCostItem(row))
        {
            return 0m;
        }
        var priceUnit = ResolveMovingPriceUnit(row);
        return RoundCost(row.MovingAveragePrice / priceUnit);
    }

    /// <summary>
    /// 取价格单位（汇总口径仅生产相关=X、PCB SECT 标识为空且 F：移动价格单位，与移动平均价配对）
    /// </summary>
    /// <param name="row">成本行</param>
    /// <returns>价格单位</returns>
    public static int ResolvePriceUnit(TaktBomMaterialCostItem row)
    {
        ArgumentNullException.ThrowIfNull(row);
        if (!CountsTowardBomMaterialCostItem(row))
        {
            return 1;
        }
        return ResolveMovingPriceUnit(row);
    }

    /// <summary>
    /// 取移动价格单位（≤0 时按 1 处理）
    /// </summary>
    /// <param name="row">成本行</param>
    /// <returns>移动价格单位</returns>
    public static int ResolveMovingPriceUnit(TaktBomMaterialCostItem row)
    {
        ArgumentNullException.ThrowIfNull(row);
        return row.MovingPriceUnit <= 0 ? 1 : row.MovingPriceUnit;
    }

    /// <summary>
    /// 移动价格表每基本计量单位价（MovingPrice÷PriceUnit，保留 CostDecimalDigits；PriceUnit≤0 按 1；MovingPrice≤0 返回 0）
    /// </summary>
    /// <param name="row">移动价格行</param>
    /// <returns>单价</returns>
    public static decimal ResolveMaterialMovingUnitPrice(TaktMaterialMovingPrice row)
    {
        ArgumentNullException.ThrowIfNull(row);
        if (row.MovingPrice <= 0m)
        {
            return 0m;
        }
        var unit = row.PriceUnit <= 0 ? 1 : row.PriceUnit;
        return RoundCost(row.MovingPrice / unit);
    }

    /// <summary>
    /// 取货币码（汇总口径仅生产相关=X、PCB SECT 标识为空且 F：移动价格货币）
    /// </summary>
    /// <param name="row">成本行</param>
    /// <returns>货币码</returns>
    public static string ResolveCurrency(TaktBomMaterialCostItem row)
    {
        ArgumentNullException.ThrowIfNull(row);
        if (!CountsTowardBomMaterialCostItem(row))
        {
            return string.Empty;
        }
        return row.MovingPriceCurrencyCode ?? string.Empty;
    }

    /// <summary>
    /// 组件行业务键（对齐表唯一键，不含 CostingDate）：Plant+BomLevel+BomItem+Product+LineNumber+Component
    /// </summary>
    /// <param name="row">成本行</param>
    /// <returns>组件键</returns>
    public static string BuildComponentKey(TaktBomMaterialCostItem row)
    {
        ArgumentNullException.ThrowIfNull(row);
        return string.Join(
            "|",
            row.PlantCode?.Trim() ?? string.Empty,
            row.BomLevel?.Trim() ?? string.Empty,
            row.BomItemCode?.Trim() ?? string.Empty,
            row.ProductCode?.Trim() ?? string.Empty,
            row.LineNumber.ToString(System.Globalization.CultureInfo.InvariantCulture),
            row.ComponentCode?.Trim() ?? string.Empty);
    }

    /// <summary>
    /// 取产品在指定工厂、指定核算日当天的参与计算快照行（CostingDate 须与明细一致；口径=生产相关=X、PCB SECT 标识为空、采购类型=F）。
    /// </summary>
    /// <param name="rows">候选行</param>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="productCode">产品编码</param>
    /// <param name="costingDate">核算日（与明细 CostingDate 对齐）</param>
    /// <returns>快照行列表</returns>
    public static List<TaktBomMaterialCostItem> ResolveDateSnapshot(
        IEnumerable<TaktBomMaterialCostItem> rows,
        string plantCode,
        string productCode,
        DateTime costingDate)
    {
        ArgumentNullException.ThrowIfNull(rows);
        ArgumentException.ThrowIfNullOrWhiteSpace(plantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(productCode);
        var day = NormalizeCostingDate(costingDate);
        return rows
            .Where(r => string.Equals(r.PlantCode, plantCode, StringComparison.OrdinalIgnoreCase)
                && ProductCodeMatches(r.ProductCode, productCode)
                && NormalizeCostingDate(r.CostingDate) == day)
            .Where(CountsTowardBomMaterialCostItem)
            .GroupBy(BuildComponentKey, StringComparer.Ordinal)
            // 同组件多行：取最大 Id（明细无软删业务）
            .Select(g => g.OrderByDescending(r => r.Id).First())
            .ToList();
    }

    /// <summary>
    /// 取产品在指定工厂、指定期间内「最后核算日」当天的全量展开行（同月多日只取最后一日整单；不在此先按参与资格过滤）。
    /// </summary>
    /// <param name="rows">候选行</param>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="productCode">产品编码</param>
    /// <param name="periodKey">期间键 yyyy-MM</param>
    /// <returns>最后核算日全量去重行</returns>
    public static List<TaktBomMaterialCostItem> ResolvePeriodSnapshot(
        IEnumerable<TaktBomMaterialCostItem> rows,
        string plantCode,
        string productCode,
        string periodKey)
    {
        return ResolvePeriodFullDayRows(rows, plantCode, productCode, periodKey);
    }

    /// <summary>
    /// 取产品在指定期间「最后核算日」当天的全量展开行（同键取最大 Id；明细无软删），供 SumSnapshotCost / Filter 再按参与资格筛选
    /// </summary>
    /// <param name="rows">候选行</param>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="productCode">产品编码</param>
    /// <param name="periodKey">期间键 yyyy-MM</param>
    /// <returns>最后核算日全量去重行</returns>
    public static List<TaktBomMaterialCostItem> ResolvePeriodFullDayRows(
        IEnumerable<TaktBomMaterialCostItem> rows,
        string plantCode,
        string productCode,
        string periodKey)
    {
        ArgumentNullException.ThrowIfNull(rows);
        ArgumentException.ThrowIfNullOrWhiteSpace(plantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(productCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(periodKey);
        var latestCostingDate = ResolveLatestCostingDate(rows, plantCode, productCode, periodKey);
        if (latestCostingDate == null)
        {
            return new List<TaktBomMaterialCostItem>();
        }
        var day = NormalizeCostingDate(latestCostingDate.Value);
        return rows
            .Where(r => string.Equals(r.PlantCode, plantCode, StringComparison.OrdinalIgnoreCase)
                && ProductCodeMatches(r.ProductCode, productCode)
                && NormalizeCostingDate(r.CostingDate) == day)
            .GroupBy(BuildComponentKey, StringComparer.Ordinal)
            // 同组件多行：取最大 Id（明细无软删业务）
            .Select(g => g.OrderByDescending(r => r.Id).First())
            .ToList();
    }

    /// <summary>
    /// 取期间内最后核算日（规范化后的日历日；无数据时返回 null）
    /// </summary>
    /// <param name="rows">候选行</param>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="productCode">产品编码</param>
    /// <param name="periodKey">期间键 yyyy-MM</param>
    /// <returns>最后核算日</returns>
    public static DateTime? ResolveLatestCostingDate(
        IEnumerable<TaktBomMaterialCostItem> rows,
        string plantCode,
        string productCode,
        string periodKey)
    {
        ArgumentNullException.ThrowIfNull(rows);
        ArgumentException.ThrowIfNullOrWhiteSpace(plantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(productCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(periodKey);
        var periodRows = rows
            .Where(r => string.Equals(r.PlantCode, plantCode, StringComparison.OrdinalIgnoreCase)
                && ProductCodeMatches(r.ProductCode, productCode)
                && ToPeriodKey(r.CostingDate) == periodKey)
            .ToList();
        if (periodRows.Count == 0)
        {
            return null;
        }
        return periodRows.Max(r => NormalizeCostingDate(r.CostingDate));
    }

    /// <summary>
    /// 核算期间起止（含首尾）
    /// </summary>
    /// <param name="periodKey">期间键 yyyy-MM</param>
    /// <returns>起止日期</returns>
    public static (DateTime Start, DateTime End) ResolvePeriodDateRange(string periodKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(periodKey);
        if (!DateTime.TryParseExact(
                periodKey + "-01",
                "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out var start))
        {
            throw new ArgumentException("期间键须为 yyyy-MM", nameof(periodKey));
        }
        start = DateTime.SpecifyKind(start, DateTimeKind.Unspecified);
        var end = start.AddMonths(1).AddTicks(-1);
        return (start, end);
    }

    /// <summary>
    /// 汇总产品 BOM 材料成本：先按行排除 PCB SECT 整树，再 Filter（生产相关=X、PCB SECT 标识为空、采购类型=F），
    /// 行成本=组件数量×(移动平均价÷移动价格单位)（保留 5 位小数；不按用量/是否零价过滤）。
    /// 口径完全取自明细行字段，不明传工厂/价目。
    /// </summary>
    /// <param name="snapshotRows">快照行（须含全量展开以便识别 PCB SECT 父节点）</param>
    /// <returns>总成本</returns>
    public static decimal SumSnapshotCost(IEnumerable<TaktBomMaterialCostItem> snapshotRows)
    {
        ArgumentNullException.ThrowIfNull(snapshotRows);
        var list = snapshotRows as IList<TaktBomMaterialCostItem> ?? snapshotRows.ToList();
        return RoundCost(
            FilterBomMaterialCostItemRows(ExcludePcbSectHierarchyRows(list))
                .Sum(CalculateLineCost));
    }

    /// <summary>
    /// 汇总产品最近采购成本：先排除 PCB SECT 整树 → Filter（生产相关=X、PCB SECT 标识为空、采购类型=F）
    /// → 仅用量 &gt; 0.001 且净价 ≠ 0 参与合计（行金额=组件数量×(净价÷采购价格单位)，保留 5 位小数）
    /// </summary>
    /// <param name="snapshotRows">快照行（须含全量展开以便识别 PCB SECT 父节点）</param>
    /// <returns>最近采购成本合计</returns>
    public static decimal SumSnapshotPurchaseCost(IEnumerable<TaktBomMaterialCostItem> snapshotRows)
    {
        ArgumentNullException.ThrowIfNull(snapshotRows);
        var list = snapshotRows as IList<TaktBomMaterialCostItem> ?? snapshotRows.ToList();
        return RoundCost(
            FilterBomMaterialCostItemRows(ExcludePcbSectHierarchyRows(list))
                .Where(CountsTowardPurchaseCostStatistics)
                .Sum(CalculateLinePurchaseCost));
    }

    /// <summary>
 /// 产品编码是否匹配（18 位 数字码与 10 位存储码互认）
    /// </summary>
    /// <param name="storedProductCode">库内产品编码</param>
    /// <param name="requestedProductCode">查询产品编码</param>
    /// <returns>是否同一产品</returns>
    public static bool ProductCodeMatches(string? storedProductCode, string requestedProductCode)
    {
        if (string.IsNullOrWhiteSpace(storedProductCode) || string.IsNullOrWhiteSpace(requestedProductCode))
        {
            return false;
        }
        var requested = requestedProductCode.Trim();
        var stored = storedProductCode.Trim();
        if (string.Equals(stored, requested, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        var requestedNormalized = TaktStringHelper.NormalizeSapNumericMaterialCode(requested);
        var storedNormalized = TaktStringHelper.NormalizeSapNumericMaterialCode(stored);
        return string.Equals(storedNormalized, requestedNormalized, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 展开产品编码查询变体（原码 / 归一化 10 位 / 18 位零填充），供 Contains 查询命中明细表
    /// </summary>
    /// <param name="productCode">产品编码</param>
    /// <returns>去重后的查询码列表</returns>
    public static IReadOnlyList<string> ExpandProductCodeLookupVariants(string? productCode)
    {
        if (string.IsNullOrWhiteSpace(productCode))
        {
            return Array.Empty<string>();
        }
        var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var trimmed = productCode.Trim();
        set.Add(trimmed);
        var normalized = TaktStringHelper.NormalizeSapNumericMaterialCode(trimmed);
        if (!string.IsNullOrEmpty(normalized))
        {
            set.Add(normalized);
        }
        if (IsTenDigitNumeric(trimmed))
        {
            set.Add(trimmed.PadLeft(18, '0'));
        }
        if (!string.IsNullOrEmpty(normalized)
            && !string.Equals(normalized, trimmed, StringComparison.OrdinalIgnoreCase)
            && IsTenDigitNumeric(normalized))
        {
            set.Add(normalized.PadLeft(18, '0'));
        }
        return set.ToList();
    }

    /// <summary>
    /// 是否 10 位纯数字物料码
    /// </summary>
    /// <param name="code">编码</param>
    /// <returns>是否 10 位数字</returns>
    private static bool IsTenDigitNumeric(string code)
    {
        if (code.Length != 10)
        {
            return false;
        }
        for (var i = 0; i < code.Length; i++)
        {
            if (!char.IsDigit(code[i]))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// BOM 层级前导点数（1→0，.1→1，..2→2；与展开显示 1,2,…,.1,..2 对齐）
    /// </summary>
    /// <param name="bomLevel">层级</param>
    /// <returns>前导点数</returns>
    public static int ParseBomLevelDotDepth(string? bomLevel)
    {
        if (string.IsNullOrWhiteSpace(bomLevel))
        {
            return int.MaxValue;
        }
        var s = bomLevel.Trim();
        var depth = 0;
        while (depth < s.Length && s[depth] == '.')
        {
            depth++;
        }
        return depth;
    }

    /// <summary>
    /// BOM 层级去掉前导点后的数字段（.1→1，..2→2，01→1；非数字用 int.MaxValue）
    /// </summary>
    /// <param name="bomLevel">层级</param>
    /// <returns>数字键</returns>
    public static int ParseBomLevelNumericPart(string? bomLevel)
    {
        if (string.IsNullOrWhiteSpace(bomLevel))
        {
            return int.MaxValue;
        }
        var s = bomLevel.Trim();
        var i = 0;
        while (i < s.Length && s[i] == '.')
        {
            i++;
        }
        if (i >= s.Length)
        {
            return int.MaxValue;
        }
        // 兼容 01/02：取首段数字
        var end = i;
        while (end < s.Length && char.IsDigit(s[end]))
        {
            end++;
        }
        if (end == i)
        {
            return int.MaxValue;
        }
        return int.TryParse(
            s.AsSpan(i, end - i),
            System.Globalization.NumberStyles.Integer,
            System.Globalization.CultureInfo.InvariantCulture,
            out var n)
            ? n
            : int.MaxValue;
    }

    /// <summary>
    /// 列表/推移固定序：ProductCode 升序，再 LineNumber 升序
    /// </summary>
    /// <param name="productCodeA">产品 A</param>
    /// <param name="lineNumberA">行号 A</param>
    /// <param name="productCodeB">产品 B</param>
    /// <param name="lineNumberB">行号 B</param>
    /// <returns>比较结果</returns>
    public static int CompareProductCodeThenLineNumber(
        string? productCodeA,
        int lineNumberA,
        string? productCodeB,
        int lineNumberB)
    {
        var productCmp = string.Compare(
            productCodeA?.Trim() ?? string.Empty,
            productCodeB?.Trim() ?? string.Empty,
            StringComparison.OrdinalIgnoreCase);
        if (productCmp != 0)
        {
            return productCmp;
        }
        return lineNumberA.CompareTo(lineNumberB);
    }

    /// <summary>
    /// 产品成本明细展开序：ProductCode → LineNumber → BomLevel（深度再数字，如 1…6 后 .1、..2）
    /// </summary>
    /// <param name="productCodeA">产品 A</param>
    /// <param name="lineNumberA">行号 A</param>
    /// <param name="bomLevelA">层级 A</param>
    /// <param name="productCodeB">产品 B</param>
    /// <param name="lineNumberB">行号 B</param>
    /// <param name="bomLevelB">层级 B</param>
    /// <returns>比较结果</returns>
    public static int CompareBomExplosionOrder(
        string? productCodeA,
        int lineNumberA,
        string? bomLevelA,
        string? productCodeB,
        int lineNumberB,
        string? bomLevelB)
    {
        var productCmp = string.Compare(
            productCodeA?.Trim() ?? string.Empty,
            productCodeB?.Trim() ?? string.Empty,
            StringComparison.OrdinalIgnoreCase);
        if (productCmp != 0)
        {
            return productCmp;
        }
        var seqCmp = lineNumberA.CompareTo(lineNumberB);
        if (seqCmp != 0)
        {
            return seqCmp;
        }
        var depthCmp = ParseBomLevelDotDepth(bomLevelA).CompareTo(ParseBomLevelDotDepth(bomLevelB));
        if (depthCmp != 0)
        {
            return depthCmp;
        }
        var levelNumCmp = ParseBomLevelNumericPart(bomLevelA).CompareTo(ParseBomLevelNumericPart(bomLevelB));
        if (levelNumCmp != 0)
        {
            return levelNumCmp;
        }
        return string.Compare(
            bomLevelA?.Trim() ?? string.Empty,
            bomLevelB?.Trim() ?? string.Empty,
            StringComparison.Ordinal);
    }

    /// <summary>
    /// 是否成品物料类型 FERT
    /// </summary>
    /// <param name="materialType">物料类型</param>
    /// <returns>是否 FERT</returns>
    public static bool IsFertMaterialType(string? materialType)
    {
        return string.Equals(
            materialType?.Trim(),
            FertMaterialTypeCode,
            StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 工厂物料是否为可参与成本合计的成品（同工厂 + FERT + 物料编码匹配产品）
    /// </summary>
    /// <param name="materialPlants">工厂物料行（建议已过滤为本厂范围）</param>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="productCode">产品编码</param>
    /// <returns>是否参与统计</returns>
    public static bool IsFertPlantProduct(
        IReadOnlyList<TaktMaterialPlant> materialPlants,
        string plantCode,
        string productCode)
    {
        ArgumentNullException.ThrowIfNull(materialPlants);
        if (string.IsNullOrWhiteSpace(plantCode) || string.IsNullOrWhiteSpace(productCode))
        {
            return false;
        }
        var plant = plantCode.Trim();
        var product = productCode.Trim();
        foreach (var row in materialPlants)
        {
            if (!string.Equals(row.PlantCode?.Trim(), plant, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }
            if (!IsFertMaterialType(row.MaterialType))
            {
                continue;
            }
            if (ProductCodeMatches(row.MaterialCode, product))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 从工厂物料解析产品物料类型（同工厂 + 物料编码匹配；未匹配返回空）
    /// </summary>
    /// <param name="materialPlants">工厂物料行</param>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="productCode">产品编码</param>
    /// <returns>物料类型；未匹配为空</returns>
    public static string ResolveMaterialTypeFromPlant(
        IReadOnlyList<TaktMaterialPlant> materialPlants,
        string plantCode,
        string productCode)
    {
        ArgumentNullException.ThrowIfNull(materialPlants);
        if (string.IsNullOrWhiteSpace(plantCode) || string.IsNullOrWhiteSpace(productCode))
        {
            return string.Empty;
        }
        var plant = plantCode.Trim();
        var product = productCode.Trim();
        foreach (var row in materialPlants)
        {
            if (!string.Equals(row.PlantCode?.Trim(), plant, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }
            if (!ProductCodeMatches(row.MaterialCode, product))
            {
                continue;
            }
            return row.MaterialType?.Trim() ?? string.Empty;
        }
        return string.Empty;
    }

    /// <summary>
    /// 从通用物料解析产品物料类型（物料编码匹配；未匹配返回空）
    /// </summary>
    /// <param name="generalMaterials">通用物料行</param>
    /// <param name="productCode">产品编码</param>
    /// <returns>物料类型；未匹配为空</returns>
    public static string ResolveMaterialTypeFromGeneral(
        IReadOnlyList<TaktGeneralMaterial> generalMaterials,
        string productCode)
    {
        ArgumentNullException.ThrowIfNull(generalMaterials);
        if (string.IsNullOrWhiteSpace(productCode))
        {
            return string.Empty;
        }
        var product = productCode.Trim();
        foreach (var row in generalMaterials)
        {
            if (!ProductCodeMatches(row.MaterialCode, product))
            {
                continue;
            }
            return row.MaterialType?.Trim() ?? string.Empty;
        }
        return string.Empty;
    }

    /// <summary>
    /// 物料类型：通用物料优先，没有再查工厂物料（同工厂 + 物料编码匹配）
    /// </summary>
    /// <param name="generalMaterials">通用物料行</param>
    /// <param name="materialPlants">工厂物料行</param>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="productCode">产品编码</param>
    /// <returns>物料类型；两源均未匹配为空</returns>
    public static string ResolveMaterialTypeFromGeneralThenPlant(
        IReadOnlyList<TaktGeneralMaterial> generalMaterials,
        IReadOnlyList<TaktMaterialPlant> materialPlants,
        string plantCode,
        string productCode)
    {
        var fromGeneral = ResolveMaterialTypeFromGeneral(generalMaterials, productCode);
        if (!string.IsNullOrWhiteSpace(fromGeneral))
        {
            return fromGeneral;
        }
        return ResolveMaterialTypeFromPlant(materialPlants, plantCode, productCode);
    }

    /// <summary>
    /// BOM 展开行排序（与 CompareBomExplosionOrder 一致，供 PCB SECT 子树扫描）
    /// </summary>
    private sealed class BomExplosionRowComparer : IComparer<TaktBomMaterialCostItem>
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static BomExplosionRowComparer Instance { get; } = new();

        /// <summary>
        /// 比较两行展开顺序
        /// </summary>
        /// <param name="x">行 A</param>
        /// <param name="y">行 B</param>
        /// <returns>比较结果</returns>
        public int Compare(TaktBomMaterialCostItem? x, TaktBomMaterialCostItem? y)
        {
            if (ReferenceEquals(x, y))
            {
                return 0;
            }
            if (x is null)
            {
                return -1;
            }
            if (y is null)
            {
                return 1;
            }
            var order = CompareBomExplosionOrder(
                x.ProductCode,
                x.LineNumber,
                x.BomLevel,
                y.ProductCode,
                y.LineNumber,
                y.BomLevel);
            if (order != 0)
            {
                return order;
            }
            return x.Id.CompareTo(y.Id);
        }
    }
}
