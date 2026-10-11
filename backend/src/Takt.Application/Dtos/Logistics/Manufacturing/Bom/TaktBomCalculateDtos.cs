// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Bom
// 文件名称：TaktBomCalculateDtos.cs
// 创建时间：2026-08-14
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 计算独立 DTO（计算成本 / 重算成本 / 计算平均成本 / 回填采购价）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 计算成本 / 重算成本查询（须单个核算月）
/// </summary>
public class TaktBomCalculateQueryDto
{
    /// <summary>
    /// 工厂代码（可选；空=当前公司全部工厂）
    /// </summary>
    public string? PlantCode { get; set; }

    /// <summary>
    /// 物料类型（查询栏所选；空=不按类型过滤，如 Quartz）
    /// </summary>
    public string? MaterialType { get; set; }

    /// <summary>
    /// 机种编码（可选；空=全部机种）
    /// </summary>
    public string? ModelCode { get; set; }

    /// <summary>
    /// 产品编码（可选）
    /// </summary>
    public string? ProductCode { get; set; }

    /// <summary>
    /// 产品编码集合（机种范围展开后写入；API 可不传）
    /// </summary>
    public List<string>? ProductCodes { get; set; }

    /// <summary>
    /// 核算日期起（须与止同月）
    /// </summary>
    public DateTime? CostingDateStart { get; set; }

    /// <summary>
    /// 核算日期止（须与起同月）
    /// </summary>
    public DateTime? CostingDateEnd { get; set; }

    /// <summary>
    /// 处理工厂+产品组上限（0=全部；默认 5000）
    /// </summary>
    public int ProcessRecordCount { get; set; } = 5000;
}

/// <summary>
/// BOM 计算规范化查询（单核算月）
/// </summary>
public class TaktBomCalculatePreparedQueryDto
{
    /// <summary>
    /// 规范化后的查询（CostingDate 已收束为单月首尾）
    /// </summary>
    public TaktBomCalculateQueryDto Query { get; set; } = new();

    /// <summary>
    /// 核算月份标签（yyyy-MM）
    /// </summary>
    public string ProcessedMonth { get; set; } = string.Empty;
}

/// <summary>
/// 提交后台计算/重算回执
/// </summary>
public class TaktBomCalculateSubmittedDto
{
    /// <summary>
    /// 核算月份标签（yyyy-MM）
    /// </summary>
    public string ProcessedMonth { get; set; } = string.Empty;

    /// <summary>
    /// 是否强制重算
    /// </summary>
    public bool ForceRecalculate { get; set; }
}

/// <summary>
/// 计算成本 / 重算成本结果
/// </summary>
public class TaktBomCalculateCostResultDto
{
    /// <summary>
    /// 扫描明细行数
    /// </summary>
    public int ScannedRowCount { get; set; }

    /// <summary>
    /// 实际同步（刷新）的工厂+产品组数
    /// </summary>
    public int RefreshedGroupCount { get; set; }

    /// <summary>
    /// 跳过组数（物料类型不匹配 / 机种过滤 / 处理上限截断）
    /// </summary>
    public int SkippedGroupCount { get; set; }

    /// <summary>
    /// 强制重算时计入的重置组数
    /// </summary>
    public int ResetGroupCount { get; set; }

    /// <summary>
    /// 处理的核算月数（当前固定 1）
    /// </summary>
    public int ProcessedMonthCount { get; set; }

    /// <summary>
    /// 处理的核算月份（yyyy-MM）
    /// </summary>
    public string ProcessedMonth { get; set; } = string.Empty;
}

/// <summary>
/// 计算平均成本查询（先回填空机种/空物料类型，再按工厂+物料类型+机种+月份写月均；始终全部物料类型）
/// </summary>
public class TaktBomCalculateAverageQueryDto
{
    /// <summary>
    /// 工厂代码（必填）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 核算期间 yyyy-MM（必填）
    /// </summary>
    public string CostingPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（已忽略：平均成本始终处理全部类型；保留字段仅兼容旧请求体）
    /// </summary>
    public string? MaterialType { get; set; }

    /// <summary>
    /// 机种编码（可选；传入则仅处理该机种）
    /// </summary>
    public string? ModelCode { get; set; }
}

/// <summary>
/// 计算平均成本结果
/// </summary>
public class TaktBomCalculateAverageResultDto
{
    /// <summary>
    /// 扫描主表行数
    /// </summary>
    public int ScannedRowCount { get; set; }

    /// <summary>
    /// 机种编码更新行数
    /// </summary>
    public int ModelCodeUpdatedCount { get; set; }

    /// <summary>
    /// 物料类型更新行数
    /// </summary>
    public int MaterialTypeUpdatedCount { get; set; }

    /// <summary>
    /// 机种月平均成本更新行数
    /// </summary>
    public int AverageUpdatedCount { get; set; }

    /// <summary>
    /// 刷新的机种组数
    /// </summary>
    public int ModelGroupCount { get; set; }

    /// <summary>
    /// 扫描行中 ProductMonthlyCost &gt; 0 的行数
    /// </summary>
    public int PositiveProductCostRowCount { get; set; }

    /// <summary>
    /// 组内至少有一行产品月成本&gt;0 的机种组数
    /// </summary>
    public int GroupsWithProductCostCount { get; set; }

    /// <summary>
    /// 组内全部产品月成本为 0 的机种组数（月均只能写 0，与已是 0 时计为未更新）
    /// </summary>
    public int GroupsWithoutProductCostCount { get; set; }

    /// <summary>
    /// 处理的核算期间
    /// </summary>
    public string CostingPeriod { get; set; } = string.Empty;
}

/// <summary>
/// 回填 BOM 明细采购价结果（采购组织=主表工厂编码；净价取条件行或数量/价值等级）
/// </summary>
public class TaktBomCalculatePurchaseInfoRecordBackfillResultDto
{
    /// <summary>
    /// 扫描明细行数
    /// </summary>
    public int ScannedRowCount { get; set; }

    /// <summary>
    /// 命中采购价格并写回的行数
    /// </summary>
    public int UpdatedRowCount { get; set; }

    /// <summary>
    /// 无匹配采购价格而跳过的行数
    /// </summary>
    public int SkippedNoPriceCount { get; set; }

    /// <summary>
    /// 字段未变化而跳过的行数
    /// </summary>
    public int UnchangedRowCount { get; set; }

    /// <summary>
    /// 处理的核算月份（yyyy-MM）
    /// </summary>
    public string ProcessedMonth { get; set; } = string.Empty;
}
