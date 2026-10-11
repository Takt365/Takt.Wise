// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Materials
// 文件名称：TaktInventoryReserveDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：InventoryReserve 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktInventoryReserve 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Materials;

// ========================================
// InventoryReserve 响应 DTO
// ========================================

/// <summary>
/// 存货跌价准备实体（CAS 存货准则 / IAS 2） 计量原则：资产负债表日按「成本与可变现净值孰低」；成本高于可变现净值时计提跌价准备； 可变现净值回升时，在原已计提金额内转回（CAS/IFRS 允许；与 US GAAP ASC 330 一般禁止转回不同）。 唯一键：租户 + 公司 + 工厂 + 期间 + 物料 + 评估类别（期间存当月首日表示年月）
/// 对应前端 TaktInventoryReserveDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktInventoryReserveDto : TaktCompanyDtoBase
{
    /// <summary>
    /// InventoryReserveID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InventoryReserveId { get; set; }


    /// <summary>
    /// 期间（资产负债表日所属会计期间；业务存当月首日，表示年月，如 2026-07-01 → 2026年7月）
    /// </summary>
    public DateTime PeriodDate { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余展示）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
    /// </summary>
    public string Valuation { get; set; } = string.Empty;

    /// <summary>
    /// 计提范围（字典 logistics_materials_inventory_reserve_scope；1=按单个存货项目，2=按存货类别；CAS 优先单个项目，数量繁多单价较低时可按类别）
    /// </summary>
    public int ProvisionScope { get; set; } = 0;

    /// <summary>
    /// 库存数量（基本单位，4 位小数）
    /// </summary>
    public decimal StockQuantity { get; set; }

    /// <summary>
    /// 单位成本（存货取得/结存单位成本；与币种一致，5 位小数）
    /// </summary>
    public decimal UnitCost { get; set; }

    /// <summary>
    /// 存货成本合计（CAS/IAS 2 之 Cost；通常≈库存数量×单位成本；跌价前账面余额）
    /// </summary>
    public decimal InventoryCost { get; set; }

    /// <summary>
    /// 估计售价合计（CAS/IAS 2：预计售价；普通销售过程中的估计售价）
    /// </summary>
    public decimal EstimatedSellingPrice { get; set; }

    /// <summary>
    /// 至完工估计将要发生的成本合计（在产品/半成品完工尚需成本；产成品一般为 0）
    /// </summary>
    public decimal EstimatedCompletionCost { get; set; }

    /// <summary>
    /// 销售估计费用及税费合计（CAS/IAS 2：销售所需估计费用；含相关税费）
    /// </summary>
    public decimal EstimatedSellingCost { get; set; }

    /// <summary>
    /// 可变现净值（NRV = 估计售价 − 至完工估计成本 − 估计销售费用及税费；不得为负时业务层可钳制为 0）
    /// </summary>
    public decimal NetRealizableValue { get; set; }

    /// <summary>
    /// 单位可变现净值（便于与单位成本比较；可为 0 表示未单独维护）
    /// </summary>
    public decimal UnitNetRealizableValue { get; set; }

    /// <summary>
    /// 期初跌价准备余额（存货跌价准备科目期初贷方余额）
    /// </summary>
    public decimal OpeningProvision { get; set; }

    /// <summary>
    /// 本期计提金额（成本高于可变现净值时新增计提；计入资产减值损失/营业成本等，按公司会计政策）
    /// </summary>
    public decimal ProvisionAmount { get; set; }

    /// <summary>
    /// 本期转回金额（可变现净值回升时，在原已计提跌价准备金额内转回；CAS/IAS 2 允许）
    /// </summary>
    public decimal ReversalAmount { get; set; }

    /// <summary>
    /// 期末跌价准备余额（= 期初 + 本期计提 − 本期转回；不得低于 0，且不应使账面价值低于可变现净值）
    /// </summary>
    public decimal ClosingProvision { get; set; }

    /// <summary>
    /// 本期净损益影响（= 本期计提 − 本期转回；正数为净计提，负数为净转回）
    /// </summary>
    public decimal ImpairmentLoss { get; set; }

    /// <summary>
    /// 账面价值（Carrying amount = 存货成本 − 期末跌价准备；报表列示金额，应 ≤ 可变现净值当成本更高时取孰低）
    /// </summary>
    public decimal CarryingAmount { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 跌价原因说明（业务备注：滞销、毁损、市价下跌等）
    /// </summary>
    public string? ImpairmentReason { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    public int ProvisionStatus { get; set; } = 0;

}

// ========================================
// InventoryReserve 查询 DTO
// ========================================

/// <summary>
/// InventoryReserve 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktInventoryReserveQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 期间（资产负债表日所属会计期间；业务存当月首日，表示年月，如 2026-07-01 → 2026年7月）（范围查询-开始）
    /// </summary>
    public DateTime? PeriodDateStart { get; set; }

    /// <summary>
    /// 期间（资产负债表日所属会计期间；业务存当月首日，表示年月，如 2026-07-01 → 2026年7月）（范围查询-结束）
    /// </summary>
    public DateTime? PeriodDateEnd { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余展示）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
    /// </summary>
    public string? Valuation { get; set; } = string.Empty;

    /// <summary>
    /// 计提范围（字典 logistics_materials_inventory_reserve_scope；1=按单个存货项目，2=按存货类别；CAS 优先单个项目，数量繁多单价较低时可按类别）
    /// </summary>
    public int? ProvisionScope { get; set; }

    /// <summary>
    /// 库存数量（基本单位，4 位小数）
    /// </summary>
    public decimal? StockQuantity { get; set; }

    /// <summary>
    /// 单位成本（存货取得/结存单位成本；与币种一致，5 位小数）
    /// </summary>
    public decimal? UnitCost { get; set; }

    /// <summary>
    /// 存货成本合计（CAS/IAS 2 之 Cost；通常≈库存数量×单位成本；跌价前账面余额）
    /// </summary>
    public decimal? InventoryCost { get; set; }

    /// <summary>
    /// 估计售价合计（CAS/IAS 2：预计售价；普通销售过程中的估计售价）
    /// </summary>
    public decimal? EstimatedSellingPrice { get; set; }

    /// <summary>
    /// 至完工估计将要发生的成本合计（在产品/半成品完工尚需成本；产成品一般为 0）
    /// </summary>
    public decimal? EstimatedCompletionCost { get; set; }

    /// <summary>
    /// 销售估计费用及税费合计（CAS/IAS 2：销售所需估计费用；含相关税费）
    /// </summary>
    public decimal? EstimatedSellingCost { get; set; }

    /// <summary>
    /// 可变现净值（NRV = 估计售价 − 至完工估计成本 − 估计销售费用及税费；不得为负时业务层可钳制为 0）
    /// </summary>
    public decimal? NetRealizableValue { get; set; }

    /// <summary>
    /// 单位可变现净值（便于与单位成本比较；可为 0 表示未单独维护）
    /// </summary>
    public decimal? UnitNetRealizableValue { get; set; }

    /// <summary>
    /// 期初跌价准备余额（存货跌价准备科目期初贷方余额）
    /// </summary>
    public decimal? OpeningProvision { get; set; }

    /// <summary>
    /// 本期计提金额（成本高于可变现净值时新增计提；计入资产减值损失/营业成本等，按公司会计政策）
    /// </summary>
    public decimal? ProvisionAmount { get; set; }

    /// <summary>
    /// 本期转回金额（可变现净值回升时，在原已计提跌价准备金额内转回；CAS/IAS 2 允许）
    /// </summary>
    public decimal? ReversalAmount { get; set; }

    /// <summary>
    /// 期末跌价准备余额（= 期初 + 本期计提 − 本期转回；不得低于 0，且不应使账面价值低于可变现净值）
    /// </summary>
    public decimal? ClosingProvision { get; set; }

    /// <summary>
    /// 本期净损益影响（= 本期计提 − 本期转回；正数为净计提，负数为净转回）
    /// </summary>
    public decimal? ImpairmentLoss { get; set; }

    /// <summary>
    /// 账面价值（Carrying amount = 存货成本 − 期末跌价准备；报表列示金额，应 ≤ 可变现净值当成本更高时取孰低）
    /// </summary>
    public decimal? CarryingAmount { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 跌价原因说明（业务备注：滞销、毁损、市价下跌等）
    /// </summary>
    public string? ImpairmentReason { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    public int? ProvisionStatus { get; set; }

    /// <summary>
    /// 创建时间（范围查询-开始）
    /// </summary>
    public DateTime? CreatedAtStart { get; set; }

    /// <summary>
    /// 创建时间（范围查询-结束）
    /// </summary>
    public DateTime? CreatedAtEnd { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建InventoryReserve DTO
// ========================================

/// <summary>
/// 创建InventoryReserve DTO
/// </summary>
public class TaktInventoryReserveCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "工厂代码（选项 TaktPlants/options；DictValue=PlantCode）不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 期间（资产负债表日所属会计期间；业务存当月首日，表示年月，如 2026-07-01 → 2026年7月）
    /// </summary>
    public DateTime PeriodDate { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余展示）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
    /// </summary>
    [Required(ErrorMessage = "评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）不能为空")]
    public string Valuation { get; set; } = string.Empty;

    /// <summary>
    /// 计提范围（字典 logistics_materials_inventory_reserve_scope；1=按单个存货项目，2=按存货类别；CAS 优先单个项目，数量繁多单价较低时可按类别）
    /// </summary>
    public int ProvisionScope { get; set; } = 0;

    /// <summary>
    /// 库存数量（基本单位，4 位小数）
    /// </summary>
    public decimal StockQuantity { get; set; }

    /// <summary>
    /// 单位成本（存货取得/结存单位成本；与币种一致，5 位小数）
    /// </summary>
    public decimal UnitCost { get; set; }

    /// <summary>
    /// 存货成本合计（CAS/IAS 2 之 Cost；通常≈库存数量×单位成本；跌价前账面余额）
    /// </summary>
    public decimal InventoryCost { get; set; }

    /// <summary>
    /// 估计售价合计（CAS/IAS 2：预计售价；普通销售过程中的估计售价）
    /// </summary>
    public decimal EstimatedSellingPrice { get; set; }

    /// <summary>
    /// 至完工估计将要发生的成本合计（在产品/半成品完工尚需成本；产成品一般为 0）
    /// </summary>
    public decimal EstimatedCompletionCost { get; set; }

    /// <summary>
    /// 销售估计费用及税费合计（CAS/IAS 2：销售所需估计费用；含相关税费）
    /// </summary>
    public decimal EstimatedSellingCost { get; set; }

    /// <summary>
    /// 可变现净值（NRV = 估计售价 − 至完工估计成本 − 估计销售费用及税费；不得为负时业务层可钳制为 0）
    /// </summary>
    public decimal NetRealizableValue { get; set; }

    /// <summary>
    /// 单位可变现净值（便于与单位成本比较；可为 0 表示未单独维护）
    /// </summary>
    public decimal UnitNetRealizableValue { get; set; }

    /// <summary>
    /// 期初跌价准备余额（存货跌价准备科目期初贷方余额）
    /// </summary>
    public decimal OpeningProvision { get; set; }

    /// <summary>
    /// 本期计提金额（成本高于可变现净值时新增计提；计入资产减值损失/营业成本等，按公司会计政策）
    /// </summary>
    public decimal ProvisionAmount { get; set; }

    /// <summary>
    /// 本期转回金额（可变现净值回升时，在原已计提跌价准备金额内转回；CAS/IAS 2 允许）
    /// </summary>
    public decimal ReversalAmount { get; set; }

    /// <summary>
    /// 期末跌价准备余额（= 期初 + 本期计提 − 本期转回；不得低于 0，且不应使账面价值低于可变现净值）
    /// </summary>
    public decimal ClosingProvision { get; set; }

    /// <summary>
    /// 本期净损益影响（= 本期计提 − 本期转回；正数为净计提，负数为净转回）
    /// </summary>
    public decimal ImpairmentLoss { get; set; }

    /// <summary>
    /// 账面价值（Carrying amount = 存货成本 − 期末跌价准备；报表列示金额，应 ≤ 可变现净值当成本更高时取孰低）
    /// </summary>
    public decimal CarryingAmount { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    [Required(ErrorMessage = "币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）不能为空")]
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 跌价原因说明（业务备注：滞销、毁损、市价下跌等）
    /// </summary>
    public string? ImpairmentReason { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    public int ProvisionStatus { get; set; } = 0;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新InventoryReserve DTO
// ========================================

/// <summary>
/// 更新InventoryReserve DTO
/// 继承 TaktInventoryReserveCreateDto，添加 InventoryReserveId 字段
/// </summary>
public class TaktInventoryReserveUpdateDto : TaktInventoryReserveCreateDto
{
    /// <summary>
    /// InventoryReserveID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InventoryReserveId { get; set; }

}

// ========================================
// InventoryReserve 状态 DTO
// ========================================

/// <summary>
/// InventoryReserve 状态更新 DTO
/// </summary>
public class TaktInventoryReserveStatusDto
{
    /// <summary>
    /// InventoryReserveID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InventoryReserveId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    [Required(ErrorMessage = "状态（字典 sys_normal_disable；1=启用，0=停用）不能为空")]
    public int ProvisionStatus { get; set; } = 0;
}

// ========================================
// InventoryReserve 排序 DTO
// ========================================

/// <summary>
/// InventoryReserve 排序更新 DTO
/// </summary>
public class TaktInventoryReserveSortDto
{
    /// <summary>
    /// InventoryReserveID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InventoryReserveId { get; set; }

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [Required(ErrorMessage = "排序号（越小越靠前）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// InventoryReserve 导入模板行 DTO
/// </summary>
public class TaktInventoryReserveTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 期间（资产负债表日所属会计期间；业务存当月首日，表示年月，如 2026-07-01 → 2026年7月）
    /// </summary>
    public DateTime? PeriodDate { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余展示）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
    /// </summary>
    public string? Valuation { get; set; } = string.Empty;

    /// <summary>
    /// 计提范围（字典 logistics_materials_inventory_reserve_scope；1=按单个存货项目，2=按存货类别；CAS 优先单个项目，数量繁多单价较低时可按类别）
    /// </summary>
    public int? ProvisionScope { get; set; }

    /// <summary>
    /// 库存数量（基本单位，4 位小数）
    /// </summary>
    public decimal? StockQuantity { get; set; }

    /// <summary>
    /// 单位成本（存货取得/结存单位成本；与币种一致，5 位小数）
    /// </summary>
    public decimal? UnitCost { get; set; }

    /// <summary>
    /// 存货成本合计（CAS/IAS 2 之 Cost；通常≈库存数量×单位成本；跌价前账面余额）
    /// </summary>
    public decimal? InventoryCost { get; set; }

    /// <summary>
    /// 估计售价合计（CAS/IAS 2：预计售价；普通销售过程中的估计售价）
    /// </summary>
    public decimal? EstimatedSellingPrice { get; set; }

    /// <summary>
    /// 至完工估计将要发生的成本合计（在产品/半成品完工尚需成本；产成品一般为 0）
    /// </summary>
    public decimal? EstimatedCompletionCost { get; set; }

    /// <summary>
    /// 销售估计费用及税费合计（CAS/IAS 2：销售所需估计费用；含相关税费）
    /// </summary>
    public decimal? EstimatedSellingCost { get; set; }

    /// <summary>
    /// 可变现净值（NRV = 估计售价 − 至完工估计成本 − 估计销售费用及税费；不得为负时业务层可钳制为 0）
    /// </summary>
    public decimal? NetRealizableValue { get; set; }

    /// <summary>
    /// 单位可变现净值（便于与单位成本比较；可为 0 表示未单独维护）
    /// </summary>
    public decimal? UnitNetRealizableValue { get; set; }

    /// <summary>
    /// 期初跌价准备余额（存货跌价准备科目期初贷方余额）
    /// </summary>
    public decimal? OpeningProvision { get; set; }

    /// <summary>
    /// 本期计提金额（成本高于可变现净值时新增计提；计入资产减值损失/营业成本等，按公司会计政策）
    /// </summary>
    public decimal? ProvisionAmount { get; set; }

    /// <summary>
    /// 本期转回金额（可变现净值回升时，在原已计提跌价准备金额内转回；CAS/IAS 2 允许）
    /// </summary>
    public decimal? ReversalAmount { get; set; }

    /// <summary>
    /// 期末跌价准备余额（= 期初 + 本期计提 − 本期转回；不得低于 0，且不应使账面价值低于可变现净值）
    /// </summary>
    public decimal? ClosingProvision { get; set; }

    /// <summary>
    /// 本期净损益影响（= 本期计提 − 本期转回；正数为净计提，负数为净转回）
    /// </summary>
    public decimal? ImpairmentLoss { get; set; }

    /// <summary>
    /// 账面价值（Carrying amount = 存货成本 − 期末跌价准备；报表列示金额，应 ≤ 可变现净值当成本更高时取孰低）
    /// </summary>
    public decimal? CarryingAmount { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 跌价原因说明（业务备注：滞销、毁损、市价下跌等）
    /// </summary>
    public string? ImpairmentReason { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    public int? ProvisionStatus { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// InventoryReserve 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktInventoryReserveImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 期间（资产负债表日所属会计期间；业务存当月首日，表示年月，如 2026-07-01 → 2026年7月）
    /// </summary>
    public DateTime? PeriodDate { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余展示）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
    /// </summary>
    public string? Valuation { get; set; } = string.Empty;

    /// <summary>
    /// 计提范围（字典 logistics_materials_inventory_reserve_scope；1=按单个存货项目，2=按存货类别；CAS 优先单个项目，数量繁多单价较低时可按类别）
    /// </summary>
    public int? ProvisionScope { get; set; }

    /// <summary>
    /// 库存数量（基本单位，4 位小数）
    /// </summary>
    public decimal? StockQuantity { get; set; }

    /// <summary>
    /// 单位成本（存货取得/结存单位成本；与币种一致，5 位小数）
    /// </summary>
    public decimal? UnitCost { get; set; }

    /// <summary>
    /// 存货成本合计（CAS/IAS 2 之 Cost；通常≈库存数量×单位成本；跌价前账面余额）
    /// </summary>
    public decimal? InventoryCost { get; set; }

    /// <summary>
    /// 估计售价合计（CAS/IAS 2：预计售价；普通销售过程中的估计售价）
    /// </summary>
    public decimal? EstimatedSellingPrice { get; set; }

    /// <summary>
    /// 至完工估计将要发生的成本合计（在产品/半成品完工尚需成本；产成品一般为 0）
    /// </summary>
    public decimal? EstimatedCompletionCost { get; set; }

    /// <summary>
    /// 销售估计费用及税费合计（CAS/IAS 2：销售所需估计费用；含相关税费）
    /// </summary>
    public decimal? EstimatedSellingCost { get; set; }

    /// <summary>
    /// 可变现净值（NRV = 估计售价 − 至完工估计成本 − 估计销售费用及税费；不得为负时业务层可钳制为 0）
    /// </summary>
    public decimal? NetRealizableValue { get; set; }

    /// <summary>
    /// 单位可变现净值（便于与单位成本比较；可为 0 表示未单独维护）
    /// </summary>
    public decimal? UnitNetRealizableValue { get; set; }

    /// <summary>
    /// 期初跌价准备余额（存货跌价准备科目期初贷方余额）
    /// </summary>
    public decimal? OpeningProvision { get; set; }

    /// <summary>
    /// 本期计提金额（成本高于可变现净值时新增计提；计入资产减值损失/营业成本等，按公司会计政策）
    /// </summary>
    public decimal? ProvisionAmount { get; set; }

    /// <summary>
    /// 本期转回金额（可变现净值回升时，在原已计提跌价准备金额内转回；CAS/IAS 2 允许）
    /// </summary>
    public decimal? ReversalAmount { get; set; }

    /// <summary>
    /// 期末跌价准备余额（= 期初 + 本期计提 − 本期转回；不得低于 0，且不应使账面价值低于可变现净值）
    /// </summary>
    public decimal? ClosingProvision { get; set; }

    /// <summary>
    /// 本期净损益影响（= 本期计提 − 本期转回；正数为净计提，负数为净转回）
    /// </summary>
    public decimal? ImpairmentLoss { get; set; }

    /// <summary>
    /// 账面价值（Carrying amount = 存货成本 − 期末跌价准备；报表列示金额，应 ≤ 可变现净值当成本更高时取孰低）
    /// </summary>
    public decimal? CarryingAmount { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 跌价原因说明（业务备注：滞销、毁损、市价下跌等）
    /// </summary>
    public string? ImpairmentReason { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    public int? ProvisionStatus { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// InventoryReserve 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktInventoryReserveExportDto
{
    /// <summary>
    /// InventoryReserveID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InventoryReserveId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 期间（资产负债表日所属会计期间；业务存当月首日，表示年月，如 2026-07-01 → 2026年7月）
    /// </summary>
    public DateTime PeriodDate { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余展示）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
    /// </summary>
    public string Valuation { get; set; } = string.Empty;

    /// <summary>
    /// 计提范围（字典 logistics_materials_inventory_reserve_scope；1=按单个存货项目，2=按存货类别；CAS 优先单个项目，数量繁多单价较低时可按类别）
    /// </summary>
    public int ProvisionScope { get; set; } = 0;

    /// <summary>
    /// 库存数量（基本单位，4 位小数）
    /// </summary>
    public decimal StockQuantity { get; set; }

    /// <summary>
    /// 单位成本（存货取得/结存单位成本；与币种一致，5 位小数）
    /// </summary>
    public decimal UnitCost { get; set; }

    /// <summary>
    /// 存货成本合计（CAS/IAS 2 之 Cost；通常≈库存数量×单位成本；跌价前账面余额）
    /// </summary>
    public decimal InventoryCost { get; set; }

    /// <summary>
    /// 估计售价合计（CAS/IAS 2：预计售价；普通销售过程中的估计售价）
    /// </summary>
    public decimal EstimatedSellingPrice { get; set; }

    /// <summary>
    /// 至完工估计将要发生的成本合计（在产品/半成品完工尚需成本；产成品一般为 0）
    /// </summary>
    public decimal EstimatedCompletionCost { get; set; }

    /// <summary>
    /// 销售估计费用及税费合计（CAS/IAS 2：销售所需估计费用；含相关税费）
    /// </summary>
    public decimal EstimatedSellingCost { get; set; }

    /// <summary>
    /// 可变现净值（NRV = 估计售价 − 至完工估计成本 − 估计销售费用及税费；不得为负时业务层可钳制为 0）
    /// </summary>
    public decimal NetRealizableValue { get; set; }

    /// <summary>
    /// 单位可变现净值（便于与单位成本比较；可为 0 表示未单独维护）
    /// </summary>
    public decimal UnitNetRealizableValue { get; set; }

    /// <summary>
    /// 期初跌价准备余额（存货跌价准备科目期初贷方余额）
    /// </summary>
    public decimal OpeningProvision { get; set; }

    /// <summary>
    /// 本期计提金额（成本高于可变现净值时新增计提；计入资产减值损失/营业成本等，按公司会计政策）
    /// </summary>
    public decimal ProvisionAmount { get; set; }

    /// <summary>
    /// 本期转回金额（可变现净值回升时，在原已计提跌价准备金额内转回；CAS/IAS 2 允许）
    /// </summary>
    public decimal ReversalAmount { get; set; }

    /// <summary>
    /// 期末跌价准备余额（= 期初 + 本期计提 − 本期转回；不得低于 0，且不应使账面价值低于可变现净值）
    /// </summary>
    public decimal ClosingProvision { get; set; }

    /// <summary>
    /// 本期净损益影响（= 本期计提 − 本期转回；正数为净计提，负数为净转回）
    /// </summary>
    public decimal ImpairmentLoss { get; set; }

    /// <summary>
    /// 账面价值（Carrying amount = 存货成本 − 期末跌价准备；报表列示金额，应 ≤ 可变现净值当成本更高时取孰低）
    /// </summary>
    public decimal CarryingAmount { get; set; }

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 跌价原因说明（业务备注：滞销、毁损、市价下跌等）
    /// </summary>
    public string? ImpairmentReason { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    public int ProvisionStatus { get; set; } = 0;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
