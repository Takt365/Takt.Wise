// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Materials
// 文件名称：TaktMaterialDocumentItemDtos.cs
// 创建时间：2026-09-20
// 创建人：Takt365(Auto Generated)
// 功能描述：MaterialDocumentItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktMaterialDocumentItem 生成，请按需审阅）
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
// MaterialDocumentItem 响应 DTO
// ========================================

/// <summary>
/// Takt物料凭证行项目实体（公司级；主子表关系见 MaterialDocumentId）
/// 对应前端 TaktMaterialDocumentItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktMaterialDocumentItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// MaterialDocumentItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialDocumentItemId { get; set; }

    /// <summary>
    /// 物料凭证ID（选项 TaktMaterialDocuments/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialDocumentId { get; set; }

    /// <summary>
    /// 物料凭证名称（填充字段）
    /// </summary>
    public string? MaterialDocumentName { get; set; }

    /// <summary>
    /// 物料凭证（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string MaterialDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证项目（行号步长生成器用 int，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 行标识
    /// </summary>
    public string? LineId { get; set; } = string.Empty;

    /// <summary>
    /// 行标识
    /// </summary>
    public string? LineName { get; set; }

    /// <summary>
    /// 上级行 ID
    /// </summary>
    public string? ParentLineId { get; set; } = string.Empty;

    /// <summary>
    /// 上级行 名称（填充字段）
    /// </summary>
    public string? ParentLineName { get; set; }

    /// <summary>
    /// 层次结构级别
    /// </summary>
    public string? LineDepth { get; set; } = string.Empty;

    /// <summary>
    /// 移动类型（字典 logistics_materials_movement_type）
    /// </summary>
    public string MovementType { get; set; } = string.Empty;

    /// <summary>
    /// 项目自动创建
    /// </summary>
    public string? AutoCreatedFlag { get; set; } = string.Empty;

    /// <summary>
    /// 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 库存地点（选项 TaktWarehouses/options；DictValue=WarehouseCode）
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 库存类型（字典 logistics_stock_type）
    /// </summary>
    public string? StockType { get; set; } = string.Empty;

    /// <summary>
    /// 批次限制
    /// </summary>
    public string? RestrictedStockFlag { get; set; } = string.Empty;

    /// <summary>
    /// 特殊库存（字典 logistics_materials_special_stock_type）
    /// </summary>
    public string? SpecialStock { get; set; } = string.Empty;

    /// <summary>
    /// 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 借/贷标识
    /// </summary>
    public string? DebitCreditIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 货币（字典 accounting_financial_currency_code）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 本位币金额
    /// </summary>
    public decimal LocalCurrencyAmount { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    public decimal? AlternativeAmount { get; set; }

    /// <summary>
    /// 数量（基本计量单位）
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// 基本计量单位（字典 logistics_materials_unit_of_measure_code）
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 输入单位数量
    /// </summary>
    public decimal? EntryQuantity { get; set; }

    /// <summary>
    /// 条目单位
    /// </summary>
    public string? EntryUnit { get; set; } = string.Empty;

    /// <summary>
    /// 订单价格单位数量
    /// </summary>
    public decimal? PoPriceQuantity { get; set; }

    /// <summary>
    /// 订单价格单位
    /// </summary>
    public string? PoPriceUnit { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单项目
    /// </summary>
    public int? PurchaseOrderItem { get; set; }

    /// <summary>
    /// 参考凭证会计年度
    /// </summary>
    public string? ReferenceDocumentYear { get; set; } = string.Empty;

    /// <summary>
    /// 参考凭证
    /// </summary>
    public string? ReferenceDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 参考凭证项目
    /// </summary>
    public int? ReferenceDocumentItem { get; set; }

    /// <summary>
    /// 冲销物料凭证的年份
    /// </summary>
    public string? OriginalMaterialDocumentYear { get; set; } = string.Empty;

    /// <summary>
    /// 冲销物料凭证
    /// </summary>
    public string? OriginalMaterialDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 冲销物料凭证项目
    /// </summary>
    public int? OriginalLineNumber { get; set; }

    /// <summary>
    /// 交货已完成
    /// </summary>
    public string? DeliveryCompletedFlag { get; set; } = string.Empty;

    /// <summary>
    /// 文本（项目文本最长 50，故 Length=50）
    /// </summary>
    public string? ItemText { get; set; } = string.Empty;

    /// <summary>
    /// 设备
    /// </summary>
    public string? EquipmentCode { get; set; } = string.Empty;

    /// <summary>
    /// 收货方（最长 12，故 Length=12）
    /// </summary>
    public string? GoodsRecipient { get; set; } = string.Empty;

    /// <summary>
    /// 卸货点（最长 25，故 Length=25）
    /// </summary>
    public string? UnloadingPoint { get; set; } = string.Empty;

    /// <summary>
    /// 业务范围
    /// </summary>
    public string? BusinessAreaCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本控制域
    /// </summary>
    public string? ControllingAreaCode { get; set; } = string.Empty;

    /// <summary>
    /// 伙伴业务范围
    /// </summary>
    public string? TradingPartnerBusinessArea { get; set; } = string.Empty;

    /// <summary>
    /// 订单
    /// </summary>
    public string? ProductionOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 资产
    /// </summary>
    public string? AssetCode { get; set; } = string.Empty;

    /// <summary>
    /// 次级编号
    /// </summary>
    public string? AssetSubCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计年度
    /// </summary>
    public string? FiscalYear { get; set; } = string.Empty;

    /// <summary>
    /// 允许前期记帐
    /// </summary>
    public string? PostToPreviousPeriodFlag { get; set; } = string.Empty;

    /// <summary>
    /// 上年度记帐
    /// </summary>
    public string? PostToPreviousYearFlag { get; set; } = string.Empty;

    /// <summary>
    /// 会计凭证编号
    /// </summary>
    public string? AccountingDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计凭证行项目
    /// </summary>
    public int? AccountingDocumentItem { get; set; }

    /// <summary>
    /// 再评估凭证编号
    /// </summary>
    public string? RevaluationDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 再评估凭证行项目
    /// </summary>
    public string? RevaluationDocumentItem { get; set; } = string.Empty;

    /// <summary>
    /// 预留编号
    /// </summary>
    public string? ReservationCode { get; set; } = string.Empty;

    /// <summary>
    /// 项目编号库存转储预留
    /// </summary>
    public int? ReservationItem { get; set; }

    /// <summary>
    /// 最终发货标识
    /// </summary>
    public string? FinalIssueFlag { get; set; } = string.Empty;

    /// <summary>
    /// 预留已处理数量
    /// </summary>
    public decimal? ReservationQuantity { get; set; }

    /// <summary>
    /// 接收物料
    /// </summary>
    public string? ReceivingMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 收货工厂
    /// </summary>
    public string? ReceivingPlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 收货库存地点
    /// </summary>
    public string? ReceivingWarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 过帐前总计估价库存
    /// </summary>
    public decimal? ValuatedStockQuantity { get; set; }

    /// <summary>
    /// 过帐前总计评估的库存的价值
    /// </summary>
    public decimal? TotalValuatedStockValue { get; set; }

    /// <summary>
    /// 价格控制
    /// </summary>
    public string? PriceControl { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料编码
    /// </summary>
    public string? ManufacturerPartMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 参考（最长 32，故 Length=32）
    /// </summary>
    public string? MkpfReferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 交货
    /// </summary>
    public string? ImDeliveryCode { get; set; } = string.Empty;

    /// <summary>
    /// 交货项目
    /// </summary>
    public int? ImDeliveryItem { get; set; }

    /// <summary>
    /// 过账人（当前登录用户对应的 EmployeeCode）
    /// </summary>
    public string? PostedBy { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 物料凭证主表
    /// （主表：TaktMaterialDocument）
    /// </summary>
    public TaktMaterialDocumentDto? MaterialDocument { get; set; }

}

// ========================================
// MaterialDocumentItem 查询 DTO
// ========================================

/// <summary>
/// MaterialDocumentItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktMaterialDocumentItemQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
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
    /// 物料凭证ID（选项 TaktMaterialDocuments/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaterialDocumentId { get; set; }

    /// <summary>
    /// 物料凭证（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? MaterialDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证项目（行号步长生成器用 int，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 行标识
    /// </summary>
    public string? LineId { get; set; } = string.Empty;

    /// <summary>
    /// 上级行 ID
    /// </summary>
    public string? ParentLineId { get; set; } = string.Empty;

    /// <summary>
    /// 层次结构级别
    /// </summary>
    public string? LineDepth { get; set; } = string.Empty;

    /// <summary>
    /// 移动类型（字典 logistics_materials_movement_type）
    /// </summary>
    public string? MovementType { get; set; } = string.Empty;

    /// <summary>
    /// 项目自动创建
    /// </summary>
    public string? AutoCreatedFlag { get; set; } = string.Empty;

    /// <summary>
    /// 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 库存地点（选项 TaktWarehouses/options；DictValue=WarehouseCode）
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 库存类型（字典 logistics_stock_type）
    /// </summary>
    public string? StockType { get; set; } = string.Empty;

    /// <summary>
    /// 批次限制
    /// </summary>
    public string? RestrictedStockFlag { get; set; } = string.Empty;

    /// <summary>
    /// 特殊库存（字典 logistics_materials_special_stock_type）
    /// </summary>
    public string? SpecialStock { get; set; } = string.Empty;

    /// <summary>
    /// 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 借/贷标识
    /// </summary>
    public string? DebitCreditIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 货币（字典 accounting_financial_currency_code）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 本位币金额
    /// </summary>
    public decimal? LocalCurrencyAmount { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    public decimal? AlternativeAmount { get; set; }

    /// <summary>
    /// 数量（基本计量单位）
    /// </summary>
    public decimal? Quantity { get; set; }

    /// <summary>
    /// 基本计量单位（字典 logistics_materials_unit_of_measure_code）
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 输入单位数量
    /// </summary>
    public decimal? EntryQuantity { get; set; }

    /// <summary>
    /// 条目单位
    /// </summary>
    public string? EntryUnit { get; set; } = string.Empty;

    /// <summary>
    /// 订单价格单位数量
    /// </summary>
    public decimal? PoPriceQuantity { get; set; }

    /// <summary>
    /// 订单价格单位
    /// </summary>
    public string? PoPriceUnit { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单项目
    /// </summary>
    public int? PurchaseOrderItem { get; set; }

    /// <summary>
    /// 参考凭证会计年度
    /// </summary>
    public string? ReferenceDocumentYear { get; set; } = string.Empty;

    /// <summary>
    /// 参考凭证
    /// </summary>
    public string? ReferenceDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 参考凭证项目
    /// </summary>
    public int? ReferenceDocumentItem { get; set; }

    /// <summary>
    /// 冲销物料凭证的年份
    /// </summary>
    public string? OriginalMaterialDocumentYear { get; set; } = string.Empty;

    /// <summary>
    /// 冲销物料凭证
    /// </summary>
    public string? OriginalMaterialDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 冲销物料凭证项目
    /// </summary>
    public int? OriginalLineNumber { get; set; }

    /// <summary>
    /// 交货已完成
    /// </summary>
    public string? DeliveryCompletedFlag { get; set; } = string.Empty;

    /// <summary>
    /// 文本（项目文本最长 50，故 Length=50）
    /// </summary>
    public string? ItemText { get; set; } = string.Empty;

    /// <summary>
    /// 设备
    /// </summary>
    public string? EquipmentCode { get; set; } = string.Empty;

    /// <summary>
    /// 收货方（最长 12，故 Length=12）
    /// </summary>
    public string? GoodsRecipient { get; set; } = string.Empty;

    /// <summary>
    /// 卸货点（最长 25，故 Length=25）
    /// </summary>
    public string? UnloadingPoint { get; set; } = string.Empty;

    /// <summary>
    /// 业务范围
    /// </summary>
    public string? BusinessAreaCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本控制域
    /// </summary>
    public string? ControllingAreaCode { get; set; } = string.Empty;

    /// <summary>
    /// 伙伴业务范围
    /// </summary>
    public string? TradingPartnerBusinessArea { get; set; } = string.Empty;

    /// <summary>
    /// 订单
    /// </summary>
    public string? ProductionOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 资产
    /// </summary>
    public string? AssetCode { get; set; } = string.Empty;

    /// <summary>
    /// 次级编号
    /// </summary>
    public string? AssetSubCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计年度
    /// </summary>
    public string? FiscalYear { get; set; } = string.Empty;

    /// <summary>
    /// 允许前期记帐
    /// </summary>
    public string? PostToPreviousPeriodFlag { get; set; } = string.Empty;

    /// <summary>
    /// 上年度记帐
    /// </summary>
    public string? PostToPreviousYearFlag { get; set; } = string.Empty;

    /// <summary>
    /// 会计凭证编号
    /// </summary>
    public string? AccountingDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计凭证行项目
    /// </summary>
    public int? AccountingDocumentItem { get; set; }

    /// <summary>
    /// 再评估凭证编号
    /// </summary>
    public string? RevaluationDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 再评估凭证行项目
    /// </summary>
    public string? RevaluationDocumentItem { get; set; } = string.Empty;

    /// <summary>
    /// 预留编号
    /// </summary>
    public string? ReservationCode { get; set; } = string.Empty;

    /// <summary>
    /// 项目编号库存转储预留
    /// </summary>
    public int? ReservationItem { get; set; }

    /// <summary>
    /// 最终发货标识
    /// </summary>
    public string? FinalIssueFlag { get; set; } = string.Empty;

    /// <summary>
    /// 预留已处理数量
    /// </summary>
    public decimal? ReservationQuantity { get; set; }

    /// <summary>
    /// 接收物料
    /// </summary>
    public string? ReceivingMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 收货工厂
    /// </summary>
    public string? ReceivingPlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 收货库存地点
    /// </summary>
    public string? ReceivingWarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 过帐前总计估价库存
    /// </summary>
    public decimal? ValuatedStockQuantity { get; set; }

    /// <summary>
    /// 过帐前总计评估的库存的价值
    /// </summary>
    public decimal? TotalValuatedStockValue { get; set; }

    /// <summary>
    /// 价格控制
    /// </summary>
    public string? PriceControl { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料编码
    /// </summary>
    public string? ManufacturerPartMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 参考（最长 32，故 Length=32）
    /// </summary>
    public string? MkpfReferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 交货
    /// </summary>
    public string? ImDeliveryCode { get; set; } = string.Empty;

    /// <summary>
    /// 交货项目
    /// </summary>
    public int? ImDeliveryItem { get; set; }

    /// <summary>
    /// 过账人（当前登录用户对应的 EmployeeCode）
    /// </summary>
    public string? PostedBy { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
// 创建MaterialDocumentItem DTO
// ========================================

/// <summary>
/// 创建MaterialDocumentItem DTO
/// </summary>
public class TaktMaterialDocumentItemCreateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证ID（选项 TaktMaterialDocuments/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialDocumentId { get; set; }

    /// <summary>
    /// 物料凭证（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string MaterialDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证项目（行号步长生成器用 int，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 行标识
    /// </summary>
    public string? LineId { get; set; } = string.Empty;

    /// <summary>
    /// 上级行 ID
    /// </summary>
    public string? ParentLineId { get; set; } = string.Empty;

    /// <summary>
    /// 层次结构级别
    /// </summary>
    public string? LineDepth { get; set; } = string.Empty;

    /// <summary>
    /// 移动类型（字典 logistics_materials_movement_type）
    /// </summary>
    [Required(ErrorMessage = "移动类型（字典 logistics_materials_movement_type）不能为空")]
    public string MovementType { get; set; } = string.Empty;

    /// <summary>
    /// 项目自动创建
    /// </summary>
    public string? AutoCreatedFlag { get; set; } = string.Empty;

    /// <summary>
    /// 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 库存地点（选项 TaktWarehouses/options；DictValue=WarehouseCode）
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 库存类型（字典 logistics_stock_type）
    /// </summary>
    public string? StockType { get; set; } = string.Empty;

    /// <summary>
    /// 批次限制
    /// </summary>
    public string? RestrictedStockFlag { get; set; } = string.Empty;

    /// <summary>
    /// 特殊库存（字典 logistics_materials_special_stock_type）
    /// </summary>
    public string? SpecialStock { get; set; } = string.Empty;

    /// <summary>
    /// 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 借/贷标识
    /// </summary>
    public string? DebitCreditIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 货币（字典 accounting_financial_currency_code）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 本位币金额
    /// </summary>
    public decimal LocalCurrencyAmount { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    public decimal? AlternativeAmount { get; set; }

    /// <summary>
    /// 数量（基本计量单位）
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// 基本计量单位（字典 logistics_materials_unit_of_measure_code）
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 输入单位数量
    /// </summary>
    public decimal? EntryQuantity { get; set; }

    /// <summary>
    /// 条目单位
    /// </summary>
    public string? EntryUnit { get; set; } = string.Empty;

    /// <summary>
    /// 订单价格单位数量
    /// </summary>
    public decimal? PoPriceQuantity { get; set; }

    /// <summary>
    /// 订单价格单位
    /// </summary>
    public string? PoPriceUnit { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单项目
    /// </summary>
    public int? PurchaseOrderItem { get; set; }

    /// <summary>
    /// 参考凭证会计年度
    /// </summary>
    public string? ReferenceDocumentYear { get; set; } = string.Empty;

    /// <summary>
    /// 参考凭证
    /// </summary>
    public string? ReferenceDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 参考凭证项目
    /// </summary>
    public int? ReferenceDocumentItem { get; set; }

    /// <summary>
    /// 冲销物料凭证的年份
    /// </summary>
    public string? OriginalMaterialDocumentYear { get; set; } = string.Empty;

    /// <summary>
    /// 冲销物料凭证
    /// </summary>
    public string? OriginalMaterialDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 冲销物料凭证项目
    /// </summary>
    public int? OriginalLineNumber { get; set; }

    /// <summary>
    /// 交货已完成
    /// </summary>
    public string? DeliveryCompletedFlag { get; set; } = string.Empty;

    /// <summary>
    /// 文本（项目文本最长 50，故 Length=50）
    /// </summary>
    public string? ItemText { get; set; } = string.Empty;

    /// <summary>
    /// 设备
    /// </summary>
    public string? EquipmentCode { get; set; } = string.Empty;

    /// <summary>
    /// 收货方（最长 12，故 Length=12）
    /// </summary>
    public string? GoodsRecipient { get; set; } = string.Empty;

    /// <summary>
    /// 卸货点（最长 25，故 Length=25）
    /// </summary>
    public string? UnloadingPoint { get; set; } = string.Empty;

    /// <summary>
    /// 业务范围
    /// </summary>
    public string? BusinessAreaCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本控制域
    /// </summary>
    public string? ControllingAreaCode { get; set; } = string.Empty;

    /// <summary>
    /// 伙伴业务范围
    /// </summary>
    public string? TradingPartnerBusinessArea { get; set; } = string.Empty;

    /// <summary>
    /// 订单
    /// </summary>
    public string? ProductionOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 资产
    /// </summary>
    public string? AssetCode { get; set; } = string.Empty;

    /// <summary>
    /// 次级编号
    /// </summary>
    public string? AssetSubCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计年度
    /// </summary>
    public string? FiscalYear { get; set; } = string.Empty;

    /// <summary>
    /// 允许前期记帐
    /// </summary>
    public string? PostToPreviousPeriodFlag { get; set; } = string.Empty;

    /// <summary>
    /// 上年度记帐
    /// </summary>
    public string? PostToPreviousYearFlag { get; set; } = string.Empty;

    /// <summary>
    /// 会计凭证编号
    /// </summary>
    public string? AccountingDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计凭证行项目
    /// </summary>
    public int? AccountingDocumentItem { get; set; }

    /// <summary>
    /// 再评估凭证编号
    /// </summary>
    public string? RevaluationDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 再评估凭证行项目
    /// </summary>
    public string? RevaluationDocumentItem { get; set; } = string.Empty;

    /// <summary>
    /// 预留编号
    /// </summary>
    public string? ReservationCode { get; set; } = string.Empty;

    /// <summary>
    /// 项目编号库存转储预留
    /// </summary>
    public int? ReservationItem { get; set; }

    /// <summary>
    /// 最终发货标识
    /// </summary>
    public string? FinalIssueFlag { get; set; } = string.Empty;

    /// <summary>
    /// 预留已处理数量
    /// </summary>
    public decimal? ReservationQuantity { get; set; }

    /// <summary>
    /// 接收物料
    /// </summary>
    public string? ReceivingMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 收货工厂
    /// </summary>
    public string? ReceivingPlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 收货库存地点
    /// </summary>
    public string? ReceivingWarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 过帐前总计估价库存
    /// </summary>
    public decimal? ValuatedStockQuantity { get; set; }

    /// <summary>
    /// 过帐前总计评估的库存的价值
    /// </summary>
    public decimal? TotalValuatedStockValue { get; set; }

    /// <summary>
    /// 价格控制
    /// </summary>
    public string? PriceControl { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料编码
    /// </summary>
    public string? ManufacturerPartMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 参考（最长 32，故 Length=32）
    /// </summary>
    public string? MkpfReferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 交货
    /// </summary>
    public string? ImDeliveryCode { get; set; } = string.Empty;

    /// <summary>
    /// 交货项目
    /// </summary>
    public int? ImDeliveryItem { get; set; }

    /// <summary>
    /// 过账人（当前登录用户对应的 EmployeeCode）
    /// </summary>
    public string? PostedBy { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

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
// 更新MaterialDocumentItem DTO
// ========================================

/// <summary>
/// 更新MaterialDocumentItem DTO
/// 继承 TaktMaterialDocumentItemCreateDto，添加 MaterialDocumentItemId 字段
/// </summary>
public class TaktMaterialDocumentItemUpdateDto : TaktMaterialDocumentItemCreateDto
{
    /// <summary>
    /// MaterialDocumentItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialDocumentItemId { get; set; }

}

// ========================================
// MaterialDocumentItem 作废 DTO
// ========================================

/// <summary>
/// MaterialDocumentItem 作废/撤销作废 DTO
/// </summary>
public class TaktMaterialDocumentItemObsoleteDto
{
    /// <summary>
    /// MaterialDocumentItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialDocumentItemId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// MaterialDocumentItem 导入模板行 DTO
/// </summary>
public class TaktMaterialDocumentItemTemplateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证ID（选项 TaktMaterialDocuments/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaterialDocumentId { get; set; }

    /// <summary>
    /// 物料凭证（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? MaterialDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证项目（行号步长生成器用 int，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 行标识
    /// </summary>
    public string? LineId { get; set; } = string.Empty;

    /// <summary>
    /// 上级行 ID
    /// </summary>
    public string? ParentLineId { get; set; } = string.Empty;

    /// <summary>
    /// 层次结构级别
    /// </summary>
    public string? LineDepth { get; set; } = string.Empty;

    /// <summary>
    /// 移动类型（字典 logistics_materials_movement_type）
    /// </summary>
    public string? MovementType { get; set; } = string.Empty;

    /// <summary>
    /// 项目自动创建
    /// </summary>
    public string? AutoCreatedFlag { get; set; } = string.Empty;

    /// <summary>
    /// 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 库存地点（选项 TaktWarehouses/options；DictValue=WarehouseCode）
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 库存类型（字典 logistics_stock_type）
    /// </summary>
    public string? StockType { get; set; } = string.Empty;

    /// <summary>
    /// 批次限制
    /// </summary>
    public string? RestrictedStockFlag { get; set; } = string.Empty;

    /// <summary>
    /// 特殊库存（字典 logistics_materials_special_stock_type）
    /// </summary>
    public string? SpecialStock { get; set; } = string.Empty;

    /// <summary>
    /// 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 借/贷标识
    /// </summary>
    public string? DebitCreditIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 货币（字典 accounting_financial_currency_code）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 本位币金额
    /// </summary>
    public decimal? LocalCurrencyAmount { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    public decimal? AlternativeAmount { get; set; }

    /// <summary>
    /// 数量（基本计量单位）
    /// </summary>
    public decimal? Quantity { get; set; }

    /// <summary>
    /// 基本计量单位（字典 logistics_materials_unit_of_measure_code）
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 输入单位数量
    /// </summary>
    public decimal? EntryQuantity { get; set; }

    /// <summary>
    /// 条目单位
    /// </summary>
    public string? EntryUnit { get; set; } = string.Empty;

    /// <summary>
    /// 订单价格单位数量
    /// </summary>
    public decimal? PoPriceQuantity { get; set; }

    /// <summary>
    /// 订单价格单位
    /// </summary>
    public string? PoPriceUnit { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单项目
    /// </summary>
    public int? PurchaseOrderItem { get; set; }

    /// <summary>
    /// 参考凭证会计年度
    /// </summary>
    public string? ReferenceDocumentYear { get; set; } = string.Empty;

    /// <summary>
    /// 参考凭证
    /// </summary>
    public string? ReferenceDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 参考凭证项目
    /// </summary>
    public int? ReferenceDocumentItem { get; set; }

    /// <summary>
    /// 冲销物料凭证的年份
    /// </summary>
    public string? OriginalMaterialDocumentYear { get; set; } = string.Empty;

    /// <summary>
    /// 冲销物料凭证
    /// </summary>
    public string? OriginalMaterialDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 冲销物料凭证项目
    /// </summary>
    public int? OriginalLineNumber { get; set; }

    /// <summary>
    /// 交货已完成
    /// </summary>
    public string? DeliveryCompletedFlag { get; set; } = string.Empty;

    /// <summary>
    /// 文本（项目文本最长 50，故 Length=50）
    /// </summary>
    public string? ItemText { get; set; } = string.Empty;

    /// <summary>
    /// 设备
    /// </summary>
    public string? EquipmentCode { get; set; } = string.Empty;

    /// <summary>
    /// 收货方（最长 12，故 Length=12）
    /// </summary>
    public string? GoodsRecipient { get; set; } = string.Empty;

    /// <summary>
    /// 卸货点（最长 25，故 Length=25）
    /// </summary>
    public string? UnloadingPoint { get; set; } = string.Empty;

    /// <summary>
    /// 业务范围
    /// </summary>
    public string? BusinessAreaCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本控制域
    /// </summary>
    public string? ControllingAreaCode { get; set; } = string.Empty;

    /// <summary>
    /// 伙伴业务范围
    /// </summary>
    public string? TradingPartnerBusinessArea { get; set; } = string.Empty;

    /// <summary>
    /// 订单
    /// </summary>
    public string? ProductionOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 资产
    /// </summary>
    public string? AssetCode { get; set; } = string.Empty;

    /// <summary>
    /// 次级编号
    /// </summary>
    public string? AssetSubCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计年度
    /// </summary>
    public string? FiscalYear { get; set; } = string.Empty;

    /// <summary>
    /// 允许前期记帐
    /// </summary>
    public string? PostToPreviousPeriodFlag { get; set; } = string.Empty;

    /// <summary>
    /// 上年度记帐
    /// </summary>
    public string? PostToPreviousYearFlag { get; set; } = string.Empty;

    /// <summary>
    /// 会计凭证编号
    /// </summary>
    public string? AccountingDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计凭证行项目
    /// </summary>
    public int? AccountingDocumentItem { get; set; }

    /// <summary>
    /// 再评估凭证编号
    /// </summary>
    public string? RevaluationDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 再评估凭证行项目
    /// </summary>
    public string? RevaluationDocumentItem { get; set; } = string.Empty;

    /// <summary>
    /// 预留编号
    /// </summary>
    public string? ReservationCode { get; set; } = string.Empty;

    /// <summary>
    /// 项目编号库存转储预留
    /// </summary>
    public int? ReservationItem { get; set; }

    /// <summary>
    /// 最终发货标识
    /// </summary>
    public string? FinalIssueFlag { get; set; } = string.Empty;

    /// <summary>
    /// 预留已处理数量
    /// </summary>
    public decimal? ReservationQuantity { get; set; }

    /// <summary>
    /// 接收物料
    /// </summary>
    public string? ReceivingMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 收货工厂
    /// </summary>
    public string? ReceivingPlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 收货库存地点
    /// </summary>
    public string? ReceivingWarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 过帐前总计估价库存
    /// </summary>
    public decimal? ValuatedStockQuantity { get; set; }

    /// <summary>
    /// 过帐前总计评估的库存的价值
    /// </summary>
    public decimal? TotalValuatedStockValue { get; set; }

    /// <summary>
    /// 价格控制
    /// </summary>
    public string? PriceControl { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料编码
    /// </summary>
    public string? ManufacturerPartMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 参考（最长 32，故 Length=32）
    /// </summary>
    public string? MkpfReferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 交货
    /// </summary>
    public string? ImDeliveryCode { get; set; } = string.Empty;

    /// <summary>
    /// 交货项目
    /// </summary>
    public int? ImDeliveryItem { get; set; }

    /// <summary>
    /// 过账人（当前登录用户对应的 EmployeeCode）
    /// </summary>
    public string? PostedBy { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
/// MaterialDocumentItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktMaterialDocumentItemImportDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证ID（选项 TaktMaterialDocuments/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaterialDocumentId { get; set; }

    /// <summary>
    /// 物料凭证（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? MaterialDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证项目（行号步长生成器用 int，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 行标识
    /// </summary>
    public string? LineId { get; set; } = string.Empty;

    /// <summary>
    /// 上级行 ID
    /// </summary>
    public string? ParentLineId { get; set; } = string.Empty;

    /// <summary>
    /// 层次结构级别
    /// </summary>
    public string? LineDepth { get; set; } = string.Empty;

    /// <summary>
    /// 移动类型（字典 logistics_materials_movement_type）
    /// </summary>
    public string? MovementType { get; set; } = string.Empty;

    /// <summary>
    /// 项目自动创建
    /// </summary>
    public string? AutoCreatedFlag { get; set; } = string.Empty;

    /// <summary>
    /// 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 库存地点（选项 TaktWarehouses/options；DictValue=WarehouseCode）
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 库存类型（字典 logistics_stock_type）
    /// </summary>
    public string? StockType { get; set; } = string.Empty;

    /// <summary>
    /// 批次限制
    /// </summary>
    public string? RestrictedStockFlag { get; set; } = string.Empty;

    /// <summary>
    /// 特殊库存（字典 logistics_materials_special_stock_type）
    /// </summary>
    public string? SpecialStock { get; set; } = string.Empty;

    /// <summary>
    /// 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 借/贷标识
    /// </summary>
    public string? DebitCreditIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 货币（字典 accounting_financial_currency_code）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 本位币金额
    /// </summary>
    public decimal? LocalCurrencyAmount { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    public decimal? AlternativeAmount { get; set; }

    /// <summary>
    /// 数量（基本计量单位）
    /// </summary>
    public decimal? Quantity { get; set; }

    /// <summary>
    /// 基本计量单位（字典 logistics_materials_unit_of_measure_code）
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 输入单位数量
    /// </summary>
    public decimal? EntryQuantity { get; set; }

    /// <summary>
    /// 条目单位
    /// </summary>
    public string? EntryUnit { get; set; } = string.Empty;

    /// <summary>
    /// 订单价格单位数量
    /// </summary>
    public decimal? PoPriceQuantity { get; set; }

    /// <summary>
    /// 订单价格单位
    /// </summary>
    public string? PoPriceUnit { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单项目
    /// </summary>
    public int? PurchaseOrderItem { get; set; }

    /// <summary>
    /// 参考凭证会计年度
    /// </summary>
    public string? ReferenceDocumentYear { get; set; } = string.Empty;

    /// <summary>
    /// 参考凭证
    /// </summary>
    public string? ReferenceDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 参考凭证项目
    /// </summary>
    public int? ReferenceDocumentItem { get; set; }

    /// <summary>
    /// 冲销物料凭证的年份
    /// </summary>
    public string? OriginalMaterialDocumentYear { get; set; } = string.Empty;

    /// <summary>
    /// 冲销物料凭证
    /// </summary>
    public string? OriginalMaterialDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 冲销物料凭证项目
    /// </summary>
    public int? OriginalLineNumber { get; set; }

    /// <summary>
    /// 交货已完成
    /// </summary>
    public string? DeliveryCompletedFlag { get; set; } = string.Empty;

    /// <summary>
    /// 文本（项目文本最长 50，故 Length=50）
    /// </summary>
    public string? ItemText { get; set; } = string.Empty;

    /// <summary>
    /// 设备
    /// </summary>
    public string? EquipmentCode { get; set; } = string.Empty;

    /// <summary>
    /// 收货方（最长 12，故 Length=12）
    /// </summary>
    public string? GoodsRecipient { get; set; } = string.Empty;

    /// <summary>
    /// 卸货点（最长 25，故 Length=25）
    /// </summary>
    public string? UnloadingPoint { get; set; } = string.Empty;

    /// <summary>
    /// 业务范围
    /// </summary>
    public string? BusinessAreaCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本控制域
    /// </summary>
    public string? ControllingAreaCode { get; set; } = string.Empty;

    /// <summary>
    /// 伙伴业务范围
    /// </summary>
    public string? TradingPartnerBusinessArea { get; set; } = string.Empty;

    /// <summary>
    /// 订单
    /// </summary>
    public string? ProductionOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 资产
    /// </summary>
    public string? AssetCode { get; set; } = string.Empty;

    /// <summary>
    /// 次级编号
    /// </summary>
    public string? AssetSubCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计年度
    /// </summary>
    public string? FiscalYear { get; set; } = string.Empty;

    /// <summary>
    /// 允许前期记帐
    /// </summary>
    public string? PostToPreviousPeriodFlag { get; set; } = string.Empty;

    /// <summary>
    /// 上年度记帐
    /// </summary>
    public string? PostToPreviousYearFlag { get; set; } = string.Empty;

    /// <summary>
    /// 会计凭证编号
    /// </summary>
    public string? AccountingDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计凭证行项目
    /// </summary>
    public int? AccountingDocumentItem { get; set; }

    /// <summary>
    /// 再评估凭证编号
    /// </summary>
    public string? RevaluationDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 再评估凭证行项目
    /// </summary>
    public string? RevaluationDocumentItem { get; set; } = string.Empty;

    /// <summary>
    /// 预留编号
    /// </summary>
    public string? ReservationCode { get; set; } = string.Empty;

    /// <summary>
    /// 项目编号库存转储预留
    /// </summary>
    public int? ReservationItem { get; set; }

    /// <summary>
    /// 最终发货标识
    /// </summary>
    public string? FinalIssueFlag { get; set; } = string.Empty;

    /// <summary>
    /// 预留已处理数量
    /// </summary>
    public decimal? ReservationQuantity { get; set; }

    /// <summary>
    /// 接收物料
    /// </summary>
    public string? ReceivingMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 收货工厂
    /// </summary>
    public string? ReceivingPlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 收货库存地点
    /// </summary>
    public string? ReceivingWarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 过帐前总计估价库存
    /// </summary>
    public decimal? ValuatedStockQuantity { get; set; }

    /// <summary>
    /// 过帐前总计评估的库存的价值
    /// </summary>
    public decimal? TotalValuatedStockValue { get; set; }

    /// <summary>
    /// 价格控制
    /// </summary>
    public string? PriceControl { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料编码
    /// </summary>
    public string? ManufacturerPartMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 参考（最长 32，故 Length=32）
    /// </summary>
    public string? MkpfReferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 交货
    /// </summary>
    public string? ImDeliveryCode { get; set; } = string.Empty;

    /// <summary>
    /// 交货项目
    /// </summary>
    public int? ImDeliveryItem { get; set; }

    /// <summary>
    /// 过账人（当前登录用户对应的 EmployeeCode）
    /// </summary>
    public string? PostedBy { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
/// MaterialDocumentItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktMaterialDocumentItemExportDto
{
    /// <summary>
    /// MaterialDocumentItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialDocumentItemId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证ID（选项 TaktMaterialDocuments/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialDocumentId { get; set; }

    /// <summary>
    /// 物料凭证（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string MaterialDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证项目（行号步长生成器用 int，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 行标识
    /// </summary>
    public string? LineId { get; set; } = string.Empty;

    /// <summary>
    /// 上级行 ID
    /// </summary>
    public string? ParentLineId { get; set; } = string.Empty;

    /// <summary>
    /// 层次结构级别
    /// </summary>
    public string? LineDepth { get; set; } = string.Empty;

    /// <summary>
    /// 移动类型（字典 logistics_materials_movement_type）
    /// </summary>
    public string MovementType { get; set; } = string.Empty;

    /// <summary>
    /// 项目自动创建
    /// </summary>
    public string? AutoCreatedFlag { get; set; } = string.Empty;

    /// <summary>
    /// 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 库存地点（选项 TaktWarehouses/options；DictValue=WarehouseCode）
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次
    /// </summary>
    public string? BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 库存类型（字典 logistics_stock_type）
    /// </summary>
    public string? StockType { get; set; } = string.Empty;

    /// <summary>
    /// 批次限制
    /// </summary>
    public string? RestrictedStockFlag { get; set; } = string.Empty;

    /// <summary>
    /// 特殊库存（字典 logistics_materials_special_stock_type）
    /// </summary>
    public string? SpecialStock { get; set; } = string.Empty;

    /// <summary>
    /// 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 借/贷标识
    /// </summary>
    public string? DebitCreditIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 货币（字典 accounting_financial_currency_code）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 本位币金额
    /// </summary>
    public decimal LocalCurrencyAmount { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    public decimal? AlternativeAmount { get; set; }

    /// <summary>
    /// 数量（基本计量单位）
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// 基本计量单位（字典 logistics_materials_unit_of_measure_code）
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 输入单位数量
    /// </summary>
    public decimal? EntryQuantity { get; set; }

    /// <summary>
    /// 条目单位
    /// </summary>
    public string? EntryUnit { get; set; } = string.Empty;

    /// <summary>
    /// 订单价格单位数量
    /// </summary>
    public decimal? PoPriceQuantity { get; set; }

    /// <summary>
    /// 订单价格单位
    /// </summary>
    public string? PoPriceUnit { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单项目
    /// </summary>
    public int? PurchaseOrderItem { get; set; }

    /// <summary>
    /// 参考凭证会计年度
    /// </summary>
    public string? ReferenceDocumentYear { get; set; } = string.Empty;

    /// <summary>
    /// 参考凭证
    /// </summary>
    public string? ReferenceDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 参考凭证项目
    /// </summary>
    public int? ReferenceDocumentItem { get; set; }

    /// <summary>
    /// 冲销物料凭证的年份
    /// </summary>
    public string? OriginalMaterialDocumentYear { get; set; } = string.Empty;

    /// <summary>
    /// 冲销物料凭证
    /// </summary>
    public string? OriginalMaterialDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 冲销物料凭证项目
    /// </summary>
    public int? OriginalLineNumber { get; set; }

    /// <summary>
    /// 交货已完成
    /// </summary>
    public string? DeliveryCompletedFlag { get; set; } = string.Empty;

    /// <summary>
    /// 文本（项目文本最长 50，故 Length=50）
    /// </summary>
    public string? ItemText { get; set; } = string.Empty;

    /// <summary>
    /// 设备
    /// </summary>
    public string? EquipmentCode { get; set; } = string.Empty;

    /// <summary>
    /// 收货方（最长 12，故 Length=12）
    /// </summary>
    public string? GoodsRecipient { get; set; } = string.Empty;

    /// <summary>
    /// 卸货点（最长 25，故 Length=25）
    /// </summary>
    public string? UnloadingPoint { get; set; } = string.Empty;

    /// <summary>
    /// 业务范围
    /// </summary>
    public string? BusinessAreaCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本控制域
    /// </summary>
    public string? ControllingAreaCode { get; set; } = string.Empty;

    /// <summary>
    /// 伙伴业务范围
    /// </summary>
    public string? TradingPartnerBusinessArea { get; set; } = string.Empty;

    /// <summary>
    /// 订单
    /// </summary>
    public string? ProductionOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 资产
    /// </summary>
    public string? AssetCode { get; set; } = string.Empty;

    /// <summary>
    /// 次级编号
    /// </summary>
    public string? AssetSubCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计年度
    /// </summary>
    public string? FiscalYear { get; set; } = string.Empty;

    /// <summary>
    /// 允许前期记帐
    /// </summary>
    public string? PostToPreviousPeriodFlag { get; set; } = string.Empty;

    /// <summary>
    /// 上年度记帐
    /// </summary>
    public string? PostToPreviousYearFlag { get; set; } = string.Empty;

    /// <summary>
    /// 会计凭证编号
    /// </summary>
    public string? AccountingDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计凭证行项目
    /// </summary>
    public int? AccountingDocumentItem { get; set; }

    /// <summary>
    /// 再评估凭证编号
    /// </summary>
    public string? RevaluationDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 再评估凭证行项目
    /// </summary>
    public string? RevaluationDocumentItem { get; set; } = string.Empty;

    /// <summary>
    /// 预留编号
    /// </summary>
    public string? ReservationCode { get; set; } = string.Empty;

    /// <summary>
    /// 项目编号库存转储预留
    /// </summary>
    public int? ReservationItem { get; set; }

    /// <summary>
    /// 最终发货标识
    /// </summary>
    public string? FinalIssueFlag { get; set; } = string.Empty;

    /// <summary>
    /// 预留已处理数量
    /// </summary>
    public decimal? ReservationQuantity { get; set; }

    /// <summary>
    /// 接收物料
    /// </summary>
    public string? ReceivingMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 收货工厂
    /// </summary>
    public string? ReceivingPlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 收货库存地点
    /// </summary>
    public string? ReceivingWarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 过帐前总计估价库存
    /// </summary>
    public decimal? ValuatedStockQuantity { get; set; }

    /// <summary>
    /// 过帐前总计评估的库存的价值
    /// </summary>
    public decimal? TotalValuatedStockValue { get; set; }

    /// <summary>
    /// 价格控制
    /// </summary>
    public string? PriceControl { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料编码
    /// </summary>
    public string? ManufacturerPartMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 参考（最长 32，故 Length=32）
    /// </summary>
    public string? MkpfReferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 交货
    /// </summary>
    public string? ImDeliveryCode { get; set; } = string.Empty;

    /// <summary>
    /// 交货项目
    /// </summary>
    public int? ImDeliveryItem { get; set; }

    /// <summary>
    /// 过账人（当前登录用户对应的 EmployeeCode）
    /// </summary>
    public string? PostedBy { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

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
