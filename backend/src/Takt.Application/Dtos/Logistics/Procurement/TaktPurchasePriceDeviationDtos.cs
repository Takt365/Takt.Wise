// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Procurement
// 文件名称：TaktPurchasePriceDeviationDtos.cs
// 创建时间：2026-09-20
// 创建人：Takt365(Cursor AI)
// 功能描述：采购价（PO/MIRO）相对同生效周期采购条件（PCR）的差异分析 DTO
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.Procurement;

/// <summary>
/// PO 价偏离 PCR 查询（按订单日期区间；返回 PO 折算单价在同周期 PCR 价目中不存在的行）
/// </summary>
public class TaktPurchasePriceDeviationQueryDto : TaktPagedQuery
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
    /// 供应商编码（可选）
    /// </summary>
    public string? SupplierCode { get; set; }

    /// <summary>
    /// 物料编码（可选，模糊匹配）
    /// </summary>
    public string? MaterialCode { get; set; }

    /// <summary>
    /// PCR 条件类型（可选；空则不过滤类型）
    /// </summary>
    public string? PriceType { get; set; }

    /// <summary>
    /// 涨跌筛选：空=全部；changed=有涨有跌；up=偏高；down=偏低
    /// </summary>
    public string? TrendFilter { get; set; }
}

/// <summary>
/// 同生效周期内一条 PCR 价目点（条件行 / 数量等级 / 价值等级）
/// </summary>
public class TaktPurchasePricePeriodPriceDto
{
    /// <summary>
    /// 定价记录号
    /// </summary>
    public string PurchaseConditionCode { get; set; } = string.Empty;

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
    public int PurchaseConditionSeq { get; set; }

    /// <summary>
    /// 等级序号（等级行才有）
    /// </summary>
    public int? PurchaseScaleSeq { get; set; }

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
/// PO 价在 PCR 中不存在的明细行
/// </summary>
public class TaktPurchasePriceDeviationDto
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单编码
    /// </summary>
    public string PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号
    /// </summary>
    public int LineNumber { get; set; }

    /// <summary>
    /// 订单日期
    /// </summary>
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// 价格日期（空则用订单日期）
    /// </summary>
    public DateTime PricingDate { get; set; }

    /// <summary>
    /// 供应商编码
    /// </summary>
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称
    /// </summary>
    public string SupplierName { get; set; } = string.Empty;

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
    /// PO 原始单价
    /// </summary>
    public decimal PoUnitPrice { get; set; }

    /// <summary>
    /// PO 价格单位
    /// </summary>
    public int PoPerUnit { get; set; }

    /// <summary>
    /// PO 折算单价（每 1 计量单位）
    /// </summary>
    public decimal PoNormalizedUnitPrice { get; set; }

    /// <summary>
    /// PO 行上的采购条件编码
    /// </summary>
    public string? PurchaseConditionCode { get; set; }

    /// <summary>
    /// 匹配原因：no_pir=同周期无 PIR；price_absent=偏离按数量解析的适用 PIR 价
    /// </summary>
    public string MatchReason { get; set; } = string.Empty;

    /// <summary>
    /// 同周期 PIR 价目点数
    /// </summary>
    public int PirPriceCount { get; set; }

    /// <summary>
    /// 同周期 PIR 价目（条件行+数量等级+价值等级；展示用全量）
    /// </summary>
    public List<TaktPurchasePricePeriodPriceDto> PirPricesInPeriod { get; set; } = new();

    /// <summary>
    /// 同周期 PIR 价目展示文案（生效期内全部价目）
    /// </summary>
    public string PirPricesText { get; set; } = string.Empty;

    /// <summary>
    /// 代表 PIR 定价记录号（多条时用分号拼接编码）
    /// </summary>
    public string PirPurchaseConditionCodes { get; set; } = string.Empty;

    /// <summary>
    /// 代表生效起（同周期最早 ValidFrom）
    /// </summary>
    public DateTime? PirValidFrom { get; set; }

    /// <summary>
    /// 代表生效止（同周期最晚 ValidTo）
    /// </summary>
    public DateTime? PirValidTo { get; set; }

    /// <summary>
    /// 最接近的 PIR 折算单价（有价目时）
    /// </summary>
    public decimal? ClosestPirNormalizedUnitPrice { get; set; }

    /// <summary>
    /// 信息记录生效周期展示（ValidFrom~ValidTo）
    /// </summary>
    public string PirValidPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 偏离金额（PO − 最近/适用 PIR；无 PIR 为空）
    /// </summary>
    public decimal? DeviationAmount { get; set; }

    /// <summary>
    /// 涨跌比（(PO − PIR) / PIR；小数比率，如 0.05=5%；无 PIR 或 PIR 单价为 0 为空）
    /// </summary>
    public decimal? DeviationPercent { get; set; }

    /// <summary>
    /// 偏离方向：up / down / none
    /// </summary>
    public string DeviationTrend { get; set; } = "none";
}

/// <summary>
/// PO 价偏离 PCR 分析结果
/// </summary>
public class TaktPurchasePriceDeviationResultDto
{
    /// <summary>
    /// 分页明细
    /// </summary>
    public TaktPagedResult<TaktPurchasePriceDeviationDto> Paged { get; set; } =
        TaktPagedResult<TaktPurchasePriceDeviationDto>.Create(new List<TaktPurchasePriceDeviationDto>(), 0, 1, 20);

    /// <summary>
    /// 命中行总数
    /// </summary>
    public int DeviationCount { get; set; }

    /// <summary>
    /// 同周期无 PCR 行数
    /// </summary>
    public int NoPirCount { get; set; }

    /// <summary>
    /// 有 PCR 但单价不在价目中的行数
    /// </summary>
    public int PriceAbsentCount { get; set; }
}

/// <summary>
/// MIRO 价偏离 PCR 查询（按物料凭证过账日期区间；本位币单价 vs 同周期采购条件）
/// </summary>
public class TaktMiroPricePcrDeviationQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 工厂代码（必填；物料凭证行 PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 过账期间起（当月首日语义）
    /// </summary>
    public DateTime? PeriodDateStart { get; set; }

    /// <summary>
    /// 过账期间止（当月首日语义）
    /// </summary>
    public DateTime? PeriodDateEnd { get; set; }

    /// <summary>
    /// 供应商编码（可选）
    /// </summary>
    public string? SupplierCode { get; set; }

    /// <summary>
    /// 物料编码（可选，模糊匹配）
    /// </summary>
    public string? MaterialCode { get; set; }

    /// <summary>
    /// PCR 条件类型（可选；空则不过滤类型）
    /// </summary>
    public string? PriceType { get; set; }

    /// <summary>
    /// 涨跌筛选：空=全部；changed=有涨有跌；up=偏高；down=偏低
    /// </summary>
    public string? TrendFilter { get; set; }
}

/// <summary>
/// MIRO（物料凭证行）价相对同周期 PCR 的偏离明细
/// </summary>
public class TaktMiroPricePcrDeviationDto
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证编码
    /// </summary>
    public string MaterialDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号
    /// </summary>
    public int LineNumber { get; set; }

    /// <summary>
    /// 过账日期
    /// </summary>
    public DateTime PostingDate { get; set; }

    /// <summary>
    /// 供应商编码
    /// </summary>
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称（凭证行无冗余名时为空）
    /// </summary>
    public string SupplierName { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（凭证行项目文本兜底）
    /// </summary>
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 币种
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 数量（基本计量单位绝对值）
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// MIRO 折算单价（|本位币金额|/|数量|）
    /// </summary>
    public decimal MiroUnitPrice { get; set; }

    /// <summary>
    /// 关联采购订单（冗余）
    /// </summary>
    public string? PurchaseOrderCode { get; set; }

    /// <summary>
    /// 匹配原因：no_pir=同周期无 PCR；price_absent=偏离按数量解析的适用 PCR 价
    /// </summary>
    public string MatchReason { get; set; } = string.Empty;

    /// <summary>
    /// 同周期 PCR 价目点数
    /// </summary>
    public int PirPriceCount { get; set; }

    /// <summary>
    /// 同周期 PCR 价目（展示用全量）
    /// </summary>
    public List<TaktPurchasePricePeriodPriceDto> PirPricesInPeriod { get; set; } = new();

    /// <summary>
    /// 同周期 PCR 价目展示文案
    /// </summary>
    public string PirPricesText { get; set; } = string.Empty;

    /// <summary>
    /// 代表 PCR 定价记录号（多条时用分号拼接）
    /// </summary>
    public string PirPurchaseConditionCodes { get; set; } = string.Empty;

    /// <summary>
    /// 代表生效起
    /// </summary>
    public DateTime? PirValidFrom { get; set; }

    /// <summary>
    /// 代表生效止
    /// </summary>
    public DateTime? PirValidTo { get; set; }

    /// <summary>
    /// 信息记录生效周期展示
    /// </summary>
    public string PirValidPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 最接近的 PCR 折算单价
    /// </summary>
    public decimal? ClosestPirNormalizedUnitPrice { get; set; }

    /// <summary>
    /// 偏离金额（MIRO − 适用 PCR）
    /// </summary>
    public decimal? DeviationAmount { get; set; }

    /// <summary>
    /// 涨跌比（(MIRO − PCR) / PCR）
    /// </summary>
    public decimal? DeviationPercent { get; set; }

    /// <summary>
    /// 偏离方向：up / down / none
    /// </summary>
    public string DeviationTrend { get; set; } = "none";
}

/// <summary>
/// MIRO 价偏离 PCR 分析结果
/// </summary>
public class TaktMiroPricePcrDeviationResultDto
{
    /// <summary>
    /// 分页明细
    /// </summary>
    public TaktPagedResult<TaktMiroPricePcrDeviationDto> Paged { get; set; } =
        TaktPagedResult<TaktMiroPricePcrDeviationDto>.Create(new List<TaktMiroPricePcrDeviationDto>(), 0, 1, 20);

    /// <summary>
    /// 命中行总数
    /// </summary>
    public int DeviationCount { get; set; }

    /// <summary>
    /// 同周期无 PCR 行数
    /// </summary>
    public int NoPirCount { get; set; }

    /// <summary>
    /// 有 PCR 但单价不在价目中的行数
    /// </summary>
    public int PriceAbsentCount { get; set; }
}
