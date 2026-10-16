// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Sales
// 文件名称：TaktSoPriceScrDeviationDtos.cs
// 创建时间：2026-09-20
// 创建人：Takt365(Cursor AI)
// 功能描述：SO 价在同生效周期 SCR（含等级价）中不存在的分析 DTO
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.Sales;

/// <summary>
/// SO 价偏离 SCR 查询（按订单日期区间；返回 SO 折算单价在同周期 SCR 价目中不存在的行）
/// </summary>
public class TaktSoPriceScrDeviationQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 工厂代码（必填）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 订单期间起（当月首日语义）
    /// </summary>
    public DateTime? PeriodDateStart { get; set; }

    /// <summary>
    /// 订单期间止（当月首日语义）
    /// </summary>
    public DateTime? PeriodDateEnd { get; set; }

    /// <summary>
    /// 客户编码（可选）
    /// </summary>
    public string? CustomerCode { get; set; }

    /// <summary>
    /// 物料编码（可选，模糊匹配）
    /// </summary>
    public string? MaterialCode { get; set; }

    /// <summary>
    /// SCR 条件类型（可选；空则不过滤类型）
    /// </summary>
    public string? PriceType { get; set; }
}

/// <summary>
/// 同生效周期内一条 SCR 价目点（条件行 / 数量等级 / 价值等级）
/// </summary>
public class TaktSoPriceScrPeriodPriceDto
{
    /// <summary>
    /// 定价记录号
    /// </summary>
    public string SalesConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件类型
    /// </summary>
    public string PriceType { get; set; } = string.Empty;

    /// <summary>
    /// 有效起始日
    /// </summary>
    public DateTime ValidFrom { get; set; }

    /// <summary>
    /// 有效截至日
    /// </summary>
    public DateTime ValidTo { get; set; }

    /// <summary>
    /// 价目来源：item / scaleQuantity / scaleValue
    /// </summary>
    public string PriceSource { get; set; } = string.Empty;

    /// <summary>
    /// 定价序号
    /// </summary>
    public int SalesConditionSeq { get; set; }

    /// <summary>
    /// 等级序号（等级行才有）
    /// </summary>
    public int? SalesScaleSeq { get; set; }

    /// <summary>
    /// 数量等级门槛
    /// </summary>
    public decimal? ScaleQuantity { get; set; }

    /// <summary>
    /// 价值等级门槛
    /// </summary>
    public decimal? ScaleValue { get; set; }

    /// <summary>
    /// 原始价格
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// 定价单位（取自条件行）
    /// </summary>
    public int PriceUnit { get; set; }

    /// <summary>
    /// 折算单价
    /// </summary>
    public decimal NormalizedUnitPrice { get; set; }
}

/// <summary>
/// SO 价在 SCR 中不存在的明细行
/// </summary>
public class TaktSoPriceScrDeviationDto
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售订单编码
    /// </summary>
    public string SalesOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号
    /// </summary>
    public int LineNumber { get; set; }

    /// <summary>
    /// 订单日期
    /// </summary>
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// 价格日期（取订单日期）
    /// </summary>
    public DateTime PricingDate { get; set; }

    /// <summary>
    /// 客户编码
    /// </summary>
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述
    /// </summary>
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 结算币种
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 订购数量
    /// </summary>
    public decimal OrderQuantity { get; set; }

    /// <summary>
    /// SO 原始单价
    /// </summary>
    public decimal SoUnitPrice { get; set; }

    /// <summary>
    /// SO 价格单位
    /// </summary>
    public int SoPerUnit { get; set; }

    /// <summary>
    /// SO 折算单价（每 1 计量单位）
    /// </summary>
    public decimal SoNormalizedUnitPrice { get; set; }

    /// <summary>
    /// 匹配原因：no_scr=同周期无 SCR；price_absent=有 SCR 但无此单价
    /// </summary>
    public string MatchReason { get; set; } = string.Empty;

    /// <summary>
    /// 同周期 SCR 价目点数
    /// </summary>
    public int ScrPriceCount { get; set; }

    /// <summary>
    /// 同周期 SCR 价目（条件行+数量等级+价值等级）
    /// </summary>
    public List<TaktSoPriceScrPeriodPriceDto> ScrPricesInPeriod { get; set; } = new();

    /// <summary>
    /// 同周期 SCR 价目展示文案
    /// </summary>
    public string ScrPricesText { get; set; } = string.Empty;

    /// <summary>
    /// 代表 SCR 定价记录号（多条时用分号拼接编码）
    /// </summary>
    public string ScrSalesConditionCodes { get; set; } = string.Empty;

    /// <summary>
    /// 代表生效起（同周期最早 ValidFrom）
    /// </summary>
    public DateTime? ScrValidFrom { get; set; }

    /// <summary>
    /// 代表生效止（同周期最晚 ValidTo）
    /// </summary>
    public DateTime? ScrValidTo { get; set; }

    /// <summary>
    /// 最接近的 SCR 折算单价（有价目时）
    /// </summary>
    public decimal? ClosestScrNormalizedUnitPrice { get; set; }

    /// <summary>
    /// 偏离金额（SO − 最近 SCR；无 SCR 为空）
    /// </summary>
    public decimal? DeviationAmount { get; set; }

    /// <summary>
    /// 偏离方向：up / down / none
    /// </summary>
    public string DeviationTrend { get; set; } = "none";
}

/// <summary>
/// SO 价偏离 SCR 分析结果
/// </summary>
public class TaktSoPriceScrDeviationResultDto
{
    /// <summary>
    /// 分页明细
    /// </summary>
    public TaktPagedResult<TaktSoPriceScrDeviationDto> Paged { get; set; } =
        TaktPagedResult<TaktSoPriceScrDeviationDto>.Create(new List<TaktSoPriceScrDeviationDto>(), 0, 1, 20);

    /// <summary>
    /// 命中行总数
    /// </summary>
    public int DeviationCount { get; set; }

    /// <summary>
    /// 同周期无 SCR 行数
    /// </summary>
    public int NoScrCount { get; set; }

    /// <summary>
    /// 有 SCR 但单价不在价目中的行数
    /// </summary>
    public int PriceAbsentCount { get; set; }
}
