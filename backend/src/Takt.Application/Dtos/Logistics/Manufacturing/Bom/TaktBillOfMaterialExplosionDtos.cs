// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Bom
// 文件名称：TaktBillOfMaterialExplosionDtos.cs
// 创建时间：2026-06-02
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 递归展开 DTO（单层存储、运行时多层展开，参照 CS03 展开视图）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Helpers;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 递归展开查询参数
/// </summary>
public class TaktBillOfMaterialExplosionQueryDto
{
    /// <summary>
    /// 展开根 BOM ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BillOfMaterialId { get; set; }

    /// <summary>
    /// 需求数量（父件数量，默认 1）
    /// </summary>
    public decimal Quantity { get; set; } = 1;

    /// <summary>
    /// 最大展开层级（0=仅父件，1=仅直接子件；默认 20）
    /// </summary>
    public int MaxLevel { get; set; } = 20;

    /// <summary>
    /// 是否包含层级 0 父件行
    /// </summary>
    public bool IncludeLevelZero { get; set; } = true;
}

/// <summary>
/// BOM 递归展开结果
/// </summary>
public class TaktBillOfMaterialExplosionDto
{
    /// <summary>
    /// 根 BOM ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BillOfMaterialId { get; set; }

    /// <summary>
    /// BOM 编码
    /// </summary>
    public string BomCode { get; set; } = string.Empty;

    /// <summary>
    /// 父件物料编码
    /// </summary>
    public string ParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 父件物料名称
    /// </summary>
    public string ParentMaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 需求数量
    /// </summary>
    public decimal Quantity { get; set; } = 1;

    /// <summary>
    /// 展开行列表（按层级、行号排序）
    /// </summary>
    public List<TaktBillOfMaterialExplosionLineDto> Lines { get; set; } = new();

    /// <summary>
    /// 父件物料描述
    /// </summary>
    public string? ParentMaterialDescription { get; set; }
}

/// <summary>
/// BOM 展开行（运行时计算，不落库）
/// </summary>
public class TaktBillOfMaterialExplosionLineDto
{
    /// <summary>
    /// 层级（0=父件，1=直接子件，依次递增）
    /// </summary>
    public int HierarchyLevel { get; set; }

    /// <summary>
    /// 层级显示前缀（如 . / .. / ...）
    /// </summary>
    public string LevelPrefix { get; set; } = string.Empty;

    /// <summary>
    /// 来源 BOM ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SourceBillOfMaterialId { get; set; }

    /// <summary>
    /// 来源 BOM 明细行 ID（层级 0 为空）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SourceBillOfMaterialItemId { get; set; }

    /// <summary>
    /// 行号（层级 0 为 0）
    /// </summary>
    public int LineNumber { get; set; }

    /// <summary>
    /// 子项物料 ID（层级 0 为父件 ID）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialId { get; set; }

    /// <summary>
    /// 子项物料编码
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 子项物料名称
    /// </summary>
    public string? MaterialName { get; set; }

    /// <summary>
    /// 直接父件物料编码（展开路径上的上一级）
    /// </summary>
    public string ImmediateParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 单位用量（对直接父件基本数量）
    /// </summary>
    public decimal UsageQuantity { get; set; }

    /// <summary>
    /// 单位
    /// </summary>
    public string MaterialUnit { get; set; } = string.Empty;

    /// <summary>
    /// 损耗率
    /// </summary>
    public decimal ScrapRate { get; set; }

    /// <summary>
    /// 累计需求量（考虑上层数量传递）
    /// </summary>
    public decimal CumulativeQuantity { get; set; }

    /// <summary>
    /// 工序号
    /// </summary>
    public int OperationSeq { get; set; }

    /// <summary>
    /// 工作中心
    /// </summary>
    public string? WorkCenter { get; set; }

    /// <summary>
    /// 位号
    /// </summary>
    public string? Position { get; set; }

    /// <summary>
    /// 是否虚拟件
    /// </summary>
    public int IsPhantom { get; set; }

    /// <summary>
    /// 是否可选件
    /// </summary>
    public int IsOptional { get; set; }

    /// <summary>
    /// 替代组号
    /// </summary>
    public string? SubstituteGroup { get; set; }

    /// <summary>
    /// 是否存在下级 BOM
    /// </summary>
    public int HasChildBom { get; set; }

    /// <summary>
    /// 是否循环引用（检测到环时标记，不再下钻）
    /// </summary>
    public int IsCircular { get; set; }

    /// <summary>
    /// 子项物料描述
    /// </summary>
    public string MaterialDescription { get; set; } = string.Empty;
}
