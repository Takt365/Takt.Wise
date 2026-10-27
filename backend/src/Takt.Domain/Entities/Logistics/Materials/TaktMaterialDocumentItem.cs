// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Materials
// 文件名称：TaktMaterialDocumentItem.cs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt物料凭证行项目实体（必要业务字段按 MSEG 清单顺序）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Materials;

/// <summary>
/// Takt物料凭证行项目实体（公司级；主子表关系见 MaterialDocumentId）
/// </summary>
[SugarTable("takt_logistics_materials_material_document_item", "物料凭证行项目表")]
[SugarIndex("ix_material_document_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_material_document_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_material_document_item_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MaterialDocumentId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_materials_material_document_item_document_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MaterialDocumentId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_material_document_item_material", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_material_document_item_purchase_order", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PurchaseOrderCode), OrderByType.Asc, false)]
public class TaktMaterialDocumentItem : TaktCompanyEntityBase
{
    /// <summary>
    /// 物料凭证ID（选项 TaktMaterialDocuments/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "material_document_id", ColumnDescription = "物料凭证ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialDocumentId { get; set; }

    /// <summary>
    /// 物料凭证（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "material_document_code", ColumnDescription = "物料凭证", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string MaterialDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料凭证项目（行号步长生成器用 int，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "物料凭证项目", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 行标识
    /// </summary>
    [SugarColumn(ColumnName = "line_id", ColumnDescription = "行标识", ColumnDataType = "nvarchar", Length = 6, IsNullable = true)]
    public string? LineId { get; set; }

    /// <summary>
    /// 上级行 ID
    /// </summary>
    [SugarColumn(ColumnName = "parent_line_id", ColumnDescription = "上级行 ID", ColumnDataType = "nvarchar", Length = 6, IsNullable = true)]
    public string? ParentLineId { get; set; }

    /// <summary>
    /// 层次结构级别
    /// </summary>
    [SugarColumn(ColumnName = "line_depth", ColumnDescription = "层次结构级别", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? LineDepth { get; set; }

    /// <summary>
    /// 移动类型（字典 logistics_materials_movement_type）
    /// </summary>
    [SugarColumn(ColumnName = "movement_type", ColumnDescription = "移动类型", ColumnDataType = "nvarchar", Length = 3, IsNullable = false, DefaultValue = "101")]
    public string MovementType { get; set; } = "101";

    /// <summary>
    /// 项目自动创建
    /// </summary>
    [SugarColumn(ColumnName = "auto_created_flag", ColumnDescription = "项目自动创建", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? AutoCreatedFlag { get; set; }

    /// <summary>
    /// 物料（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 库存地点（选项 TaktWarehouses/options；DictValue=WarehouseCode）
    /// </summary>
    [SugarColumn(ColumnName = "warehouse_code", ColumnDescription = "库存地点", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? WarehouseCode { get; set; }

    /// <summary>
    /// 批次
    /// </summary>
    [SugarColumn(ColumnName = "batch_code", ColumnDescription = "批次", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? BatchCode { get; set; }

    /// <summary>
    /// 库存类型（字典 logistics_stock_type）
    /// </summary>
    [SugarColumn(ColumnName = "stock_type", ColumnDescription = "库存类型", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? StockType { get; set; }

    /// <summary>
    /// 批次限制
    /// </summary>
    [SugarColumn(ColumnName = "restricted_stock_flag", ColumnDescription = "批次限制", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? RestrictedStockFlag { get; set; }

    /// <summary>
    /// 特殊库存（字典 logistics_materials_special_stock_type）
    /// </summary>
    [SugarColumn(ColumnName = "special_stock", ColumnDescription = "特殊库存", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? SpecialStock { get; set; }

    /// <summary>
    /// 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_code", ColumnDescription = "供应商", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? SupplierCode { get; set; }

    /// <summary>
    /// 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    [SugarColumn(ColumnName = "customer_code", ColumnDescription = "客户", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? CustomerCode { get; set; }

    /// <summary>
    /// 借/贷标识（S=借方入库/增加，H=贷方冲销退货/减少；入库实绩统计按此加减，不以移动类型取反）
    /// </summary>
    [SugarColumn(ColumnName = "debit_credit_indicator", ColumnDescription = "借/贷标识", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? DebitCreditIndicator { get; set; }

    /// <summary>
    /// 货币（字典 accounting_financial_currency_code）
    /// </summary>
    [SugarColumn(ColumnName = "currency_code", ColumnDescription = "货币", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? CurrencyCode { get; set; }

    /// <summary>
    /// 本位币金额
    /// </summary>
    [SugarColumn(ColumnName = "local_currency_amount", ColumnDescription = "本位币金额", ColumnDataType = "decimal", Length = 13, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal LocalCurrencyAmount { get; set; } = 0;

    /// <summary>
    /// 金额
    /// </summary>
    [SugarColumn(ColumnName = "alternative_amount", ColumnDescription = "金额", ColumnDataType = "decimal", Length = 13, DecimalDigits = 2, IsNullable = true)]
    public decimal? AlternativeAmount { get; set; }

    /// <summary>
    /// 数量（基本计量单位）
    /// </summary>
    [SugarColumn(ColumnName = "quantity", ColumnDescription = "数量", ColumnDataType = "decimal", Length = 13, DecimalDigits = 3, IsNullable = false, DefaultValue = "0")]
    public decimal Quantity { get; set; } = 0;

    /// <summary>
    /// 基本计量单位（字典 logistics_materials_unit_of_measure_code）
    /// </summary>
    [SugarColumn(ColumnName = "base_unit", ColumnDescription = "基本计量单位", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? BaseUnit { get; set; }

    /// <summary>
    /// 输入单位数量
    /// </summary>
    [SugarColumn(ColumnName = "entry_quantity", ColumnDescription = "输入单位数量", ColumnDataType = "decimal", Length = 13, DecimalDigits = 3, IsNullable = true)]
    public decimal? EntryQuantity { get; set; }

    /// <summary>
    /// 条目单位
    /// </summary>
    [SugarColumn(ColumnName = "entry_unit", ColumnDescription = "条目单位", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? EntryUnit { get; set; }

    /// <summary>
    /// 订单价格单位数量
    /// </summary>
    [SugarColumn(ColumnName = "po_price_quantity", ColumnDescription = "订单价格单位数量", ColumnDataType = "decimal", Length = 13, DecimalDigits = 3, IsNullable = true)]
    public decimal? PoPriceQuantity { get; set; }

    /// <summary>
    /// 订单价格单位
    /// </summary>
    [SugarColumn(ColumnName = "po_price_unit", ColumnDescription = "订单价格单位", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? PoPriceUnit { get; set; }

    /// <summary>
    /// 采购订单
    /// </summary>
    [SugarColumn(ColumnName = "purchase_order_code", ColumnDescription = "采购订单", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? PurchaseOrderCode { get; set; }

    /// <summary>
    /// 采购订单项目
    /// </summary>
    [SugarColumn(ColumnName = "purchase_order_item", ColumnDescription = "项目", ColumnDataType = "int", IsNullable = true)]
    public int? PurchaseOrderItem { get; set; }

    /// <summary>
    /// 参考凭证会计年度
    /// </summary>
    [SugarColumn(ColumnName = "reference_document_year", ColumnDescription = "参考凭证会计年度", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? ReferenceDocumentYear { get; set; }

    /// <summary>
    /// 参考凭证
    /// </summary>
    [SugarColumn(ColumnName = "reference_document_code", ColumnDescription = "参考凭证", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? ReferenceDocumentCode { get; set; }

    /// <summary>
    /// 参考凭证项目
    /// </summary>
    [SugarColumn(ColumnName = "reference_document_item", ColumnDescription = "参考凭证项目", ColumnDataType = "int", IsNullable = true)]
    public int? ReferenceDocumentItem { get; set; }

    /// <summary>
    /// 冲销物料凭证的年份
    /// </summary>
    [SugarColumn(ColumnName = "original_material_document_year", ColumnDescription = "物料凭证的年份", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? OriginalMaterialDocumentYear { get; set; }

    /// <summary>
    /// 冲销物料凭证
    /// </summary>
    [SugarColumn(ColumnName = "original_material_document_code", ColumnDescription = "物料凭证", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? OriginalMaterialDocumentCode { get; set; }

    /// <summary>
    /// 冲销物料凭证项目
    /// </summary>
    [SugarColumn(ColumnName = "original_line_number", ColumnDescription = "物料凭证项目", ColumnDataType = "int", IsNullable = true)]
    public int? OriginalLineNumber { get; set; }

    /// <summary>
    /// 交货已完成
    /// </summary>
    [SugarColumn(ColumnName = "delivery_completed_flag", ColumnDescription = "交货已完成", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? DeliveryCompletedFlag { get; set; }

    /// <summary>
    /// 文本（项目文本最长 50，故 Length=50）
    /// </summary>
    [SugarColumn(ColumnName = "item_text", ColumnDescription = "文本", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? ItemText { get; set; }

    /// <summary>
    /// 设备
    /// </summary>
    [SugarColumn(ColumnName = "equipment_code", ColumnDescription = "设备", ColumnDataType = "nvarchar", Length = 18, IsNullable = true)]
    public string? EquipmentCode { get; set; }

    /// <summary>
    /// 收货方（最长 12，故 Length=12）
    /// </summary>
    [SugarColumn(ColumnName = "goods_recipient", ColumnDescription = "收货方", ColumnDataType = "nvarchar", Length = 12, IsNullable = true)]
    public string? GoodsRecipient { get; set; }

    /// <summary>
    /// 卸货点（最长 25，故 Length=25）
    /// </summary>
    [SugarColumn(ColumnName = "unloading_point", ColumnDescription = "卸货点", ColumnDataType = "nvarchar", Length = 25, IsNullable = true)]
    public string? UnloadingPoint { get; set; }

    /// <summary>
    /// 业务范围
    /// </summary>
    [SugarColumn(ColumnName = "business_area_code", ColumnDescription = "业务范围", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? BusinessAreaCode { get; set; }

    /// <summary>
    /// 成本控制域
    /// </summary>
    [SugarColumn(ColumnName = "controlling_area_code", ColumnDescription = "成本控制域", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? ControllingAreaCode { get; set; }

    /// <summary>
    /// 伙伴业务范围
    /// </summary>
    [SugarColumn(ColumnName = "trading_partner_business_area", ColumnDescription = "伙伴业务范围", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? TradingPartnerBusinessArea { get; set; }

    /// <summary>
    /// 订单
    /// </summary>
    [SugarColumn(ColumnName = "production_order_code", ColumnDescription = "订单", ColumnDataType = "nvarchar", Length = 12, IsNullable = true)]
    public string? ProductionOrderCode { get; set; }

    /// <summary>
    /// 资产
    /// </summary>
    [SugarColumn(ColumnName = "asset_code", ColumnDescription = "资产", ColumnDataType = "nvarchar", Length = 12, IsNullable = true)]
    public string? AssetCode { get; set; }

    /// <summary>
    /// 次级编号
    /// </summary>
    [SugarColumn(ColumnName = "asset_sub_code", ColumnDescription = "次级编号", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? AssetSubCode { get; set; }

    /// <summary>
    /// 会计年度
    /// </summary>
    [SugarColumn(ColumnName = "fiscal_year", ColumnDescription = "会计年度", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? FiscalYear { get; set; }

    /// <summary>
    /// 允许前期记帐
    /// </summary>
    [SugarColumn(ColumnName = "post_to_previous_period_flag", ColumnDescription = "允许前期记帐", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? PostToPreviousPeriodFlag { get; set; }

    /// <summary>
    /// 上年度记帐
    /// </summary>
    [SugarColumn(ColumnName = "post_to_previous_year_flag", ColumnDescription = "上年度记帐", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? PostToPreviousYearFlag { get; set; }

    /// <summary>
    /// 会计凭证编号
    /// </summary>
    [SugarColumn(ColumnName = "accounting_document_code", ColumnDescription = "凭证编号", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? AccountingDocumentCode { get; set; }

    /// <summary>
    /// 会计凭证行项目
    /// </summary>
    [SugarColumn(ColumnName = "accounting_document_item", ColumnDescription = "行项目", ColumnDataType = "int", IsNullable = true)]
    public int? AccountingDocumentItem { get; set; }

    /// <summary>
    /// 再评估凭证编号
    /// </summary>
    [SugarColumn(ColumnName = "revaluation_document_code", ColumnDescription = "凭证编号", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? RevaluationDocumentCode { get; set; }

    /// <summary>
    /// 再评估凭证行项目
    /// </summary>
    [SugarColumn(ColumnName = "revaluation_document_item", ColumnDescription = "行项目", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? RevaluationDocumentItem { get; set; }

    /// <summary>
    /// 预留编号
    /// </summary>
    [SugarColumn(ColumnName = "reservation_code", ColumnDescription = "预留编号", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? ReservationCode { get; set; }

    /// <summary>
    /// 项目编号库存转储预留
    /// </summary>
    [SugarColumn(ColumnName = "reservation_item", ColumnDescription = "项目编号库存转储预留", ColumnDataType = "int", IsNullable = true)]
    public int? ReservationItem { get; set; }

    /// <summary>
    /// 最终发货标识
    /// </summary>
    [SugarColumn(ColumnName = "final_issue_flag", ColumnDescription = "最终发货标识", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? FinalIssueFlag { get; set; }

    /// <summary>
    /// 预留已处理数量
    /// </summary>
    [SugarColumn(ColumnName = "reservation_quantity", ColumnDescription = "预留已处理数量", ColumnDataType = "decimal", Length = 13, DecimalDigits = 3, IsNullable = true)]
    public decimal? ReservationQuantity { get; set; }

    /// <summary>
    /// 接收物料
    /// </summary>
    [SugarColumn(ColumnName = "receiving_material_code", ColumnDescription = "接收物料", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? ReceivingMaterialCode { get; set; }

    /// <summary>
    /// 收货工厂
    /// </summary>
    [SugarColumn(ColumnName = "receiving_plant_code", ColumnDescription = "收货工厂", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? ReceivingPlantCode { get; set; }

    /// <summary>
    /// 收货库存地点
    /// </summary>
    [SugarColumn(ColumnName = "receiving_warehouse_code", ColumnDescription = "收货库存地点", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? ReceivingWarehouseCode { get; set; }

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
    /// </summary>
    [SugarColumn(ColumnName = "profit_center_code", ColumnDescription = "利润中心", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? ProfitCenterCode { get; set; }

    /// <summary>
    /// 过帐前总计估价库存
    /// </summary>
    [SugarColumn(ColumnName = "valuated_stock_quantity", ColumnDescription = "过帐前总计估价库存", ColumnDataType = "decimal", Length = 13, DecimalDigits = 3, IsNullable = true)]
    public decimal? ValuatedStockQuantity { get; set; }

    /// <summary>
    /// 过帐前总计评估的库存的价值
    /// </summary>
    [SugarColumn(ColumnName = "total_valuated_stock_value", ColumnDescription = "过帐前总计评估的库存的价值", ColumnDataType = "decimal", Length = 13, DecimalDigits = 2, IsNullable = true)]
    public decimal? TotalValuatedStockValue { get; set; }

    /// <summary>
    /// 价格控制
    /// </summary>
    [SugarColumn(ColumnName = "price_control", ColumnDescription = "价格控制", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? PriceControl { get; set; }

    /// <summary>
    /// 制造商物料编码
    /// </summary>
    [SugarColumn(ColumnName = "manufacturer_part_material_code", ColumnDescription = "制造商物料编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? ManufacturerPartMaterialCode { get; set; }

    /// <summary>
    /// 参考（最长 32，故 Length=32）
    /// </summary>
    [SugarColumn(ColumnName = "mkpf_reference_code", ColumnDescription = "参考", ColumnDataType = "nvarchar", Length = 32, IsNullable = true)]
    public string? MkpfReferenceCode { get; set; }

    /// <summary>
    /// 交货
    /// </summary>
    [SugarColumn(ColumnName = "im_delivery_code", ColumnDescription = "交货", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? ImDeliveryCode { get; set; }

    /// <summary>
    /// 交货项目
    /// </summary>
    [SugarColumn(ColumnName = "im_delivery_item", ColumnDescription = "交货项目", ColumnDataType = "int", IsNullable = true)]
    public int? ImDeliveryItem { get; set; }

    /// <summary>
    /// 过账人（当前登录用户对应的 EmployeeCode）
    /// </summary>
    [SugarColumn(ColumnName = "posted_by", ColumnDescription = "过账人", ColumnDataType = "nvarchar", Length = 6, IsNullable = true)]
    public string? PostedBy { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 物料凭证主表
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(MaterialDocumentId))]
    public TaktMaterialDocument? MaterialDocument { get; set; }
}
