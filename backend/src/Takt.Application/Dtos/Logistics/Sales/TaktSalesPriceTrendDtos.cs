// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Sales
// 文件名称：TaktSalesPriceTrendDtos.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：销售价格推移转置分析 DTO
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.Sales;

/// <summary>
/// 工厂 × 物料 × 客户销售单价转置分析查询
/// </summary>
public class TaktSalesPriceTrendQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 工厂代码（必填）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 价格期间起（当月首日语义）
    /// </summary>
    public DateTime? PeriodDateStart { get; set; }

    /// <summary>
    /// 价格期间止（当月首日语义）
    /// </summary>
    public DateTime? PeriodDateEnd { get; set; }

    /// <summary>
    /// 关注期间（可选；销售价格推移按各行最近两次 ValidFrom 比较，忽略本字段）
    /// </summary>
    public string? FocusPeriod { get; set; }

    /// <summary>
    /// 物料编码（可选，模糊匹配）
    /// </summary>
    public string? MaterialCode { get; set; }

    /// <summary>
    /// 客户编码（可选）
    /// </summary>
    public string? CustomerCode { get; set; }

    /// <summary>
    /// 价格类型（字典 logistics_procurement_price_type，如 PR00；可选）
    /// </summary>
    public string? PriceType { get; set; }

    /// <summary>
    /// 仅启用价格主表（兼容字段；当前实体无 PriceStatus，服务侧暂不按状态过滤）
    /// </summary>
    public bool? OnlyEnabled { get; set; }

    /// <summary>
    /// 涨跌筛选：空=全部；up/down/flat/none；changed=仅涨或跌
    /// </summary>
    public string? TrendFilter { get; set; }
}

/// <summary>
/// 销售价格推移转置行（行=工厂+物料+客户，列=各 ValidFrom 折算单价）
/// </summary>
public class TaktSalesPriceTrendDto
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称（回填：随物料）
    /// </summary>
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（空串表示通用价）
    /// </summary>
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称（冗余，回填：随客户）
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// 币种
    /// </summary>
    public string Currency { get; set; } = string.Empty;

    /// <summary>
    /// 销售单位
    /// </summary>
    public string Unit { get; set; } = string.Empty;

    /// <summary>
    /// 各有效起始日折算单价（键 yyyy-MM-dd，对应主表 ValidFrom）
    /// </summary>
    public Dictionary<string, decimal> PeriodUnitPrices { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 各列单价来源（与列键相同的 yyyy-MM-dd；无缺月回填）
    /// </summary>
    public Dictionary<string, string> PeriodPriceSourcePeriods { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 环比涨跌：none / up / down / flat
    /// </summary>
    public string Trend { get; set; } = "none";

    /// <summary>
    /// 环比基准期间
    /// </summary>
    public string? BasePeriod { get; set; }

    /// <summary>
    /// 环比对比期间
    /// </summary>
    public string? ComparePeriod { get; set; }

    /// <summary>
    /// 环比差额（对比单价 - 基准单价）
    /// </summary>
    public decimal? VarianceAmount { get; set; }

    /// <summary>
    /// 环比变动率（小数比率，保留 4 位；如 0.2978 表示 29.78%）
    /// </summary>
    public decimal? VariancePercent { get; set; }

    /// <summary>
    /// MaterialDescription
    /// </summary>
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// CurrencyCode
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;
}

/// <summary>
/// 销售价格推移转置分析结果
/// </summary>
public class TaktSalesPriceTrendResultDto
{
    /// <summary>
    /// 分页行
    /// </summary>
    public TaktPagedResult<TaktSalesPriceTrendDto> Paged { get; set; } = null!;

    /// <summary>
    /// 有效起始日列顺序 yyyy-MM-dd
    /// </summary>
    public List<string> PeriodOrder { get; set; } = new();

    /// <summary>
    /// 物料行总数（分页前，已应用涨跌筛选）
    /// </summary>
    public int MaterialCount { get; set; }

    /// <summary>
    /// 环比基准期间
    /// </summary>
    public string? BasePeriod { get; set; }

    /// <summary>
    /// 环比对比期间（最近有效起始日）
    /// </summary>
    public string? ComparePeriod { get; set; }

    /// <summary>
    /// 涨价行数（筛选前全量统计）
    /// </summary>
    public int UpCount { get; set; }

    /// <summary>
    /// 跌价行数（筛选前全量统计）
    /// </summary>
    public int DownCount { get; set; }

    /// <summary>
    /// 持平行数（筛选前全量统计）
    /// </summary>
    public int FlatCount { get; set; }

    /// <summary>
    /// 无法比较行数（筛选前全量统计）
    /// </summary>
    public int NoneCount { get; set; }
}
