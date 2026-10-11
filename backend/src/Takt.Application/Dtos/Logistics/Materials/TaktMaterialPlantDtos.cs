// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Materials
// 文件名称：TaktMaterialPlantDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：MaterialPlant 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktMaterialPlant 生成，请按需审阅）
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
// MaterialPlant 响应 DTO
// ========================================

/// <summary>
/// Takt工厂物料实体
/// 对应前端 TaktMaterialPlantDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktMaterialPlantDto : TaktCompanyDtoBase
{
    /// <summary>
    /// MaterialPlantID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialPlantId { get; set; }


    /// <summary>
    /// 物料编码（选项 TaktGeneralMaterials/options；DictValue=MaterialCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode、PlantCode→CultureCode 取 TaktMaterialDescription.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格（回填：随物料）
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 行业领域（字典 logistics_materials_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）
    /// </summary>
    public string IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 物料层级
    /// </summary>
    public string? MaterialHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
    /// </summary>
    public string MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB 等；默认 ROH）
    /// </summary>
    public string MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 基本单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    public string PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 采购类型（字典 logistics_procurement_type；E=自制生产，F=外部采购，X=两种采购类型；默认 F）
    /// </summary>
    public string PurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 特殊采购（字典 logistics_procurement_special_procurement_type；0=无，10=寄售，30=外协加工，50=虚设品号；默认 0）
    /// </summary>
    public int SpecialProcurement { get; set; } = 0;

    /// <summary>
    /// 是否散装（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int IsBulk { get; set; } = 0;

    /// <summary>
    /// 最小起订量（基本单位数量，整数）
    /// </summary>
    public int MinOrderQuantity { get; set; } = 0;

    /// <summary>
    /// 舍入值（基本单位数量，用于数量舍入，整数）
    /// </summary>
    public int RoundingValue { get; set; } = 0;

    /// <summary>
    /// 计划交货时间（天数，整数）
    /// </summary>
    public int PlannedDeliveryTimeDays { get; set; } = 0;

    /// <summary>
    /// 自制生产天数（内部生产所需天数，支持 1 位小数，如 0.5、2.5）
    /// </summary>
    public decimal InHouseProductionDays { get; set; }

    /// <summary>
    /// 制造商（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料编码（选项 TaktManufacturerMaterials/options；DictValue=ManufacturerMaterialCode）
    /// </summary>
    public string? ManufacturerMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 价格控制（字典 logistics_materials_price_control；S=标准价格，V=移动平均价格/周期单价；默认 V）
    /// </summary>
    public string PriceControl { get; set; } = string.Empty;

    /// <summary>
    /// 价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）
    /// </summary>
    public int PriceUnit { get; set; } = 0;

    /// <summary>
    /// 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
    /// </summary>
    public string Valuation { get; set; } = string.Empty;

    /// <summary>
    /// 移动价格（decimal，4 位小数）
    /// </summary>
    public decimal MovingPrice { get; set; }

    /// <summary>
    /// 差异码（6）
    /// </summary>
    public string? DifferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
    /// </summary>
    public string ProfitCenter { get; set; } = string.Empty;

    /// <summary>
    /// 当前库存（基本单位数量，decimal，4 位小数）
    /// </summary>
    public decimal CurrentStock { get; set; }

    /// <summary>
    /// 生产仓储（选项 TaktWarehouses/options；DictValue=WarehouseCode）
    /// </summary>
    public string ProductionLocation { get; set; } = string.Empty;

    /// <summary>
    /// 采购仓储（选项 TaktWarehouses/options；DictValue=WarehouseCode）
    /// </summary>
    public string PurchasingLocation { get; set; } = string.Empty;

    /// <summary>
    /// 库位（选项 TaktStorageLocations/options；DictValue=LocationCode）
    /// </summary>
    public string StorageLocation { get; set; } = string.Empty;

    /// <summary>
    /// 是否需检验（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int RequiresInspection { get; set; } = 0;

    /// <summary>
    /// 批次标识（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int IsBatch { get; set; } = 0;

    /// <summary>
    /// 停产状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料）
    /// </summary>
    public string DiscontinuedStatus { get; set; } = string.Empty;

    /// <summary>
    /// 物料状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int MaterialStatus { get; set; } = 0;

}

// ========================================
// MaterialPlant 查询 DTO
// ========================================

/// <summary>
/// MaterialPlant 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktMaterialPlantQueryDto : TaktPagedQuery
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
    /// 物料编码（选项 TaktGeneralMaterials/options；DictValue=MaterialCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode、PlantCode→CultureCode 取 TaktMaterialDescription.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格（回填：随物料）
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 行业领域（字典 logistics_materials_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 物料层级
    /// </summary>
    public string? MaterialHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
    /// </summary>
    public string? MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB 等；默认 ROH）
    /// </summary>
    public string? MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 基本单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 采购类型（字典 logistics_procurement_type；E=自制生产，F=外部采购，X=两种采购类型；默认 F）
    /// </summary>
    public string? PurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 特殊采购（字典 logistics_procurement_special_procurement_type；0=无，10=寄售，30=外协加工，50=虚设品号；默认 0）
    /// </summary>
    public int? SpecialProcurement { get; set; }

    /// <summary>
    /// 是否散装（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int? IsBulk { get; set; }

    /// <summary>
    /// 最小起订量（基本单位数量，整数）
    /// </summary>
    public int? MinOrderQuantity { get; set; }

    /// <summary>
    /// 舍入值（基本单位数量，用于数量舍入，整数）
    /// </summary>
    public int? RoundingValue { get; set; }

    /// <summary>
    /// 计划交货时间（天数，整数）
    /// </summary>
    public int? PlannedDeliveryTimeDays { get; set; }

    /// <summary>
    /// 自制生产天数（内部生产所需天数，支持 1 位小数，如 0.5、2.5）
    /// </summary>
    public decimal? InHouseProductionDays { get; set; }

    /// <summary>
    /// 制造商（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料编码（选项 TaktManufacturerMaterials/options；DictValue=ManufacturerMaterialCode）
    /// </summary>
    public string? ManufacturerMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 价格控制（字典 logistics_materials_price_control；S=标准价格，V=移动平均价格/周期单价；默认 V）
    /// </summary>
    public string? PriceControl { get; set; } = string.Empty;

    /// <summary>
    /// 价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）
    /// </summary>
    public int? PriceUnit { get; set; }

    /// <summary>
    /// 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
    /// </summary>
    public string? Valuation { get; set; } = string.Empty;

    /// <summary>
    /// 移动价格（decimal，4 位小数）
    /// </summary>
    public decimal? MovingPrice { get; set; }

    /// <summary>
    /// 差异码（6）
    /// </summary>
    public string? DifferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
    /// </summary>
    public string? ProfitCenter { get; set; } = string.Empty;

    /// <summary>
    /// 当前库存（基本单位数量，decimal，4 位小数）
    /// </summary>
    public decimal? CurrentStock { get; set; }

    /// <summary>
    /// 生产仓储（选项 TaktWarehouses/options；DictValue=WarehouseCode）
    /// </summary>
    public string? ProductionLocation { get; set; } = string.Empty;

    /// <summary>
    /// 采购仓储（选项 TaktWarehouses/options；DictValue=WarehouseCode）
    /// </summary>
    public string? PurchasingLocation { get; set; } = string.Empty;

    /// <summary>
    /// 库位（选项 TaktStorageLocations/options；DictValue=LocationCode）
    /// </summary>
    public string? StorageLocation { get; set; } = string.Empty;

    /// <summary>
    /// 是否需检验（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int? RequiresInspection { get; set; }

    /// <summary>
    /// 批次标识（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int? IsBatch { get; set; }

    /// <summary>
    /// 停产状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料）
    /// </summary>
    public string? DiscontinuedStatus { get; set; } = string.Empty;

    /// <summary>
    /// 物料状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int? MaterialStatus { get; set; }

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
// 创建MaterialPlant DTO
// ========================================

/// <summary>
/// 创建MaterialPlant DTO
/// </summary>
public class TaktMaterialPlantCreateDto
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
    /// 物料编码（选项 TaktGeneralMaterials/options；DictValue=MaterialCode）
    /// </summary>
    [Required(ErrorMessage = "物料编码（选项 TaktGeneralMaterials/options；DictValue=MaterialCode）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode、PlantCode→CultureCode 取 TaktMaterialDescription.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格（回填：随物料）
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 行业领域（字典 logistics_materials_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）
    /// </summary>
    [Required(ErrorMessage = "行业领域（字典 logistics_materials_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）不能为空")]
    public string IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 物料层级
    /// </summary>
    public string? MaterialHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
    /// </summary>
    [Required(ErrorMessage = "物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）不能为空")]
    public string MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB 等；默认 ROH）
    /// </summary>
    [Required(ErrorMessage = "物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB 等；默认 ROH）不能为空")]
    public string MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 基本单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    [Required(ErrorMessage = "基本单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）不能为空")]
    public string BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    [Required(ErrorMessage = "采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）不能为空")]
    public string PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 采购类型（字典 logistics_procurement_type；E=自制生产，F=外部采购，X=两种采购类型；默认 F）
    /// </summary>
    [Required(ErrorMessage = "采购类型（字典 logistics_procurement_type；E=自制生产，F=外部采购，X=两种采购类型；默认 F）不能为空")]
    public string PurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 特殊采购（字典 logistics_procurement_special_procurement_type；0=无，10=寄售，30=外协加工，50=虚设品号；默认 0）
    /// </summary>
    public int SpecialProcurement { get; set; } = 0;

    /// <summary>
    /// 是否散装（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int IsBulk { get; set; } = 0;

    /// <summary>
    /// 最小起订量（基本单位数量，整数）
    /// </summary>
    public int MinOrderQuantity { get; set; } = 0;

    /// <summary>
    /// 舍入值（基本单位数量，用于数量舍入，整数）
    /// </summary>
    public int RoundingValue { get; set; } = 0;

    /// <summary>
    /// 计划交货时间（天数，整数）
    /// </summary>
    public int PlannedDeliveryTimeDays { get; set; } = 0;

    /// <summary>
    /// 自制生产天数（内部生产所需天数，支持 1 位小数，如 0.5、2.5）
    /// </summary>
    public decimal InHouseProductionDays { get; set; }

    /// <summary>
    /// 制造商（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料编码（选项 TaktManufacturerMaterials/options；DictValue=ManufacturerMaterialCode）
    /// </summary>
    public string? ManufacturerMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    [Required(ErrorMessage = "币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）不能为空")]
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 价格控制（字典 logistics_materials_price_control；S=标准价格，V=移动平均价格/周期单价；默认 V）
    /// </summary>
    [Required(ErrorMessage = "价格控制（字典 logistics_materials_price_control；S=标准价格，V=移动平均价格/周期单价；默认 V）不能为空")]
    public string PriceControl { get; set; } = string.Empty;

    /// <summary>
    /// 价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）
    /// </summary>
    public int PriceUnit { get; set; } = 0;

    /// <summary>
    /// 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
    /// </summary>
    [Required(ErrorMessage = "评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）不能为空")]
    public string Valuation { get; set; } = string.Empty;

    /// <summary>
    /// 移动价格（decimal，4 位小数）
    /// </summary>
    public decimal MovingPrice { get; set; }

    /// <summary>
    /// 差异码（6）
    /// </summary>
    public string? DifferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
    /// </summary>
    [Required(ErrorMessage = "利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）不能为空")]
    public string ProfitCenter { get; set; } = string.Empty;

    /// <summary>
    /// 当前库存（基本单位数量，decimal，4 位小数）
    /// </summary>
    public decimal CurrentStock { get; set; }

    /// <summary>
    /// 生产仓储（选项 TaktWarehouses/options；DictValue=WarehouseCode）
    /// </summary>
    [Required(ErrorMessage = "生产仓储（选项 TaktWarehouses/options；DictValue=WarehouseCode）不能为空")]
    public string ProductionLocation { get; set; } = string.Empty;

    /// <summary>
    /// 采购仓储（选项 TaktWarehouses/options；DictValue=WarehouseCode）
    /// </summary>
    [Required(ErrorMessage = "采购仓储（选项 TaktWarehouses/options；DictValue=WarehouseCode）不能为空")]
    public string PurchasingLocation { get; set; } = string.Empty;

    /// <summary>
    /// 库位（选项 TaktStorageLocations/options；DictValue=LocationCode）
    /// </summary>
    [Required(ErrorMessage = "库位（选项 TaktStorageLocations/options；DictValue=LocationCode）不能为空")]
    public string StorageLocation { get; set; } = string.Empty;

    /// <summary>
    /// 是否需检验（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int RequiresInspection { get; set; } = 0;

    /// <summary>
    /// 批次标识（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int IsBatch { get; set; } = 0;

    /// <summary>
    /// 停产状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料）
    /// </summary>
    [Required(ErrorMessage = "停产状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料）不能为空")]
    public string DiscontinuedStatus { get; set; } = string.Empty;

    /// <summary>
    /// 物料状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int MaterialStatus { get; set; } = 0;

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
// 更新MaterialPlant DTO
// ========================================

/// <summary>
/// 更新MaterialPlant DTO
/// 继承 TaktMaterialPlantCreateDto，添加 MaterialPlantId 字段
/// </summary>
public class TaktMaterialPlantUpdateDto : TaktMaterialPlantCreateDto
{
    /// <summary>
    /// MaterialPlantID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialPlantId { get; set; }

}

// ========================================
// MaterialPlant 状态 DTO
// ========================================

/// <summary>
/// MaterialPlant 状态更新 DTO
/// </summary>
public class TaktMaterialPlantStatusDto
{
    /// <summary>
    /// MaterialPlantID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialPlantId { get; set; }

    /// <summary>
    /// 物料状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    [Required(ErrorMessage = "物料状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）不能为空")]
    public int MaterialStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// MaterialPlant 导入模板行 DTO
/// </summary>
public class TaktMaterialPlantTemplateDto
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
    /// 物料编码（选项 TaktGeneralMaterials/options；DictValue=MaterialCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode、PlantCode→CultureCode 取 TaktMaterialDescription.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格（回填：随物料）
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 行业领域（字典 logistics_materials_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 物料层级
    /// </summary>
    public string? MaterialHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
    /// </summary>
    public string? MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB 等；默认 ROH）
    /// </summary>
    public string? MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 基本单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 采购类型（字典 logistics_procurement_type；E=自制生产，F=外部采购，X=两种采购类型；默认 F）
    /// </summary>
    public string? PurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 特殊采购（字典 logistics_procurement_special_procurement_type；0=无，10=寄售，30=外协加工，50=虚设品号；默认 0）
    /// </summary>
    public int? SpecialProcurement { get; set; }

    /// <summary>
    /// 是否散装（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int? IsBulk { get; set; }

    /// <summary>
    /// 最小起订量（基本单位数量，整数）
    /// </summary>
    public int? MinOrderQuantity { get; set; }

    /// <summary>
    /// 舍入值（基本单位数量，用于数量舍入，整数）
    /// </summary>
    public int? RoundingValue { get; set; }

    /// <summary>
    /// 计划交货时间（天数，整数）
    /// </summary>
    public int? PlannedDeliveryTimeDays { get; set; }

    /// <summary>
    /// 自制生产天数（内部生产所需天数，支持 1 位小数，如 0.5、2.5）
    /// </summary>
    public decimal? InHouseProductionDays { get; set; }

    /// <summary>
    /// 制造商（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料编码（选项 TaktManufacturerMaterials/options；DictValue=ManufacturerMaterialCode）
    /// </summary>
    public string? ManufacturerMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 价格控制（字典 logistics_materials_price_control；S=标准价格，V=移动平均价格/周期单价；默认 V）
    /// </summary>
    public string? PriceControl { get; set; } = string.Empty;

    /// <summary>
    /// 价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）
    /// </summary>
    public int? PriceUnit { get; set; }

    /// <summary>
    /// 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
    /// </summary>
    public string? Valuation { get; set; } = string.Empty;

    /// <summary>
    /// 移动价格（decimal，4 位小数）
    /// </summary>
    public decimal? MovingPrice { get; set; }

    /// <summary>
    /// 差异码（6）
    /// </summary>
    public string? DifferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
    /// </summary>
    public string? ProfitCenter { get; set; } = string.Empty;

    /// <summary>
    /// 当前库存（基本单位数量，decimal，4 位小数）
    /// </summary>
    public decimal? CurrentStock { get; set; }

    /// <summary>
    /// 生产仓储（选项 TaktWarehouses/options；DictValue=WarehouseCode）
    /// </summary>
    public string? ProductionLocation { get; set; } = string.Empty;

    /// <summary>
    /// 采购仓储（选项 TaktWarehouses/options；DictValue=WarehouseCode）
    /// </summary>
    public string? PurchasingLocation { get; set; } = string.Empty;

    /// <summary>
    /// 库位（选项 TaktStorageLocations/options；DictValue=LocationCode）
    /// </summary>
    public string? StorageLocation { get; set; } = string.Empty;

    /// <summary>
    /// 是否需检验（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int? RequiresInspection { get; set; }

    /// <summary>
    /// 批次标识（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int? IsBatch { get; set; }

    /// <summary>
    /// 停产状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料）
    /// </summary>
    public string? DiscontinuedStatus { get; set; } = string.Empty;

    /// <summary>
    /// 物料状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int? MaterialStatus { get; set; }

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
/// MaterialPlant 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktMaterialPlantImportDto
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
    /// 物料编码（选项 TaktGeneralMaterials/options；DictValue=MaterialCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode、PlantCode→CultureCode 取 TaktMaterialDescription.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格（回填：随物料）
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 行业领域（字典 logistics_materials_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 物料层级
    /// </summary>
    public string? MaterialHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
    /// </summary>
    public string? MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB 等；默认 ROH）
    /// </summary>
    public string? MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 基本单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 采购类型（字典 logistics_procurement_type；E=自制生产，F=外部采购，X=两种采购类型；默认 F）
    /// </summary>
    public string? PurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 特殊采购（字典 logistics_procurement_special_procurement_type；0=无，10=寄售，30=外协加工，50=虚设品号；默认 0）
    /// </summary>
    public int? SpecialProcurement { get; set; }

    /// <summary>
    /// 是否散装（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int? IsBulk { get; set; }

    /// <summary>
    /// 最小起订量（基本单位数量，整数）
    /// </summary>
    public int? MinOrderQuantity { get; set; }

    /// <summary>
    /// 舍入值（基本单位数量，用于数量舍入，整数）
    /// </summary>
    public int? RoundingValue { get; set; }

    /// <summary>
    /// 计划交货时间（天数，整数）
    /// </summary>
    public int? PlannedDeliveryTimeDays { get; set; }

    /// <summary>
    /// 自制生产天数（内部生产所需天数，支持 1 位小数，如 0.5、2.5）
    /// </summary>
    public decimal? InHouseProductionDays { get; set; }

    /// <summary>
    /// 制造商（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料编码（选项 TaktManufacturerMaterials/options；DictValue=ManufacturerMaterialCode）
    /// </summary>
    public string? ManufacturerMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 价格控制（字典 logistics_materials_price_control；S=标准价格，V=移动平均价格/周期单价；默认 V）
    /// </summary>
    public string? PriceControl { get; set; } = string.Empty;

    /// <summary>
    /// 价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）
    /// </summary>
    public int? PriceUnit { get; set; }

    /// <summary>
    /// 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
    /// </summary>
    public string? Valuation { get; set; } = string.Empty;

    /// <summary>
    /// 移动价格（decimal，4 位小数）
    /// </summary>
    public decimal? MovingPrice { get; set; }

    /// <summary>
    /// 差异码（6）
    /// </summary>
    public string? DifferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
    /// </summary>
    public string? ProfitCenter { get; set; } = string.Empty;

    /// <summary>
    /// 当前库存（基本单位数量，decimal，4 位小数）
    /// </summary>
    public decimal? CurrentStock { get; set; }

    /// <summary>
    /// 生产仓储（选项 TaktWarehouses/options；DictValue=WarehouseCode）
    /// </summary>
    public string? ProductionLocation { get; set; } = string.Empty;

    /// <summary>
    /// 采购仓储（选项 TaktWarehouses/options；DictValue=WarehouseCode）
    /// </summary>
    public string? PurchasingLocation { get; set; } = string.Empty;

    /// <summary>
    /// 库位（选项 TaktStorageLocations/options；DictValue=LocationCode）
    /// </summary>
    public string? StorageLocation { get; set; } = string.Empty;

    /// <summary>
    /// 是否需检验（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int? RequiresInspection { get; set; }

    /// <summary>
    /// 批次标识（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int? IsBatch { get; set; }

    /// <summary>
    /// 停产状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料）
    /// </summary>
    public string? DiscontinuedStatus { get; set; } = string.Empty;

    /// <summary>
    /// 物料状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int? MaterialStatus { get; set; }

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
/// MaterialPlant 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktMaterialPlantExportDto
{
    /// <summary>
    /// MaterialPlantID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialPlantId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktGeneralMaterials/options；DictValue=MaterialCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode、PlantCode→CultureCode 取 TaktMaterialDescription.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格（回填：随物料）
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 行业领域（字典 logistics_materials_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）
    /// </summary>
    public string IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 物料层级
    /// </summary>
    public string? MaterialHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
    /// </summary>
    public string MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB 等；默认 ROH）
    /// </summary>
    public string MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 基本单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    public string PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 采购类型（字典 logistics_procurement_type；E=自制生产，F=外部采购，X=两种采购类型；默认 F）
    /// </summary>
    public string PurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 特殊采购（字典 logistics_procurement_special_procurement_type；0=无，10=寄售，30=外协加工，50=虚设品号；默认 0）
    /// </summary>
    public int SpecialProcurement { get; set; } = 0;

    /// <summary>
    /// 是否散装（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int IsBulk { get; set; } = 0;

    /// <summary>
    /// 最小起订量（基本单位数量，整数）
    /// </summary>
    public int MinOrderQuantity { get; set; } = 0;

    /// <summary>
    /// 舍入值（基本单位数量，用于数量舍入，整数）
    /// </summary>
    public int RoundingValue { get; set; } = 0;

    /// <summary>
    /// 计划交货时间（天数，整数）
    /// </summary>
    public int PlannedDeliveryTimeDays { get; set; } = 0;

    /// <summary>
    /// 自制生产天数（内部生产所需天数，支持 1 位小数，如 0.5、2.5）
    /// </summary>
    public decimal InHouseProductionDays { get; set; }

    /// <summary>
    /// 制造商（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料编码（选项 TaktManufacturerMaterials/options；DictValue=ManufacturerMaterialCode）
    /// </summary>
    public string? ManufacturerMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 价格控制（字典 logistics_materials_price_control；S=标准价格，V=移动平均价格/周期单价；默认 V）
    /// </summary>
    public string PriceControl { get; set; } = string.Empty;

    /// <summary>
    /// 价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）
    /// </summary>
    public int PriceUnit { get; set; } = 0;

    /// <summary>
    /// 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
    /// </summary>
    public string Valuation { get; set; } = string.Empty;

    /// <summary>
    /// 移动价格（decimal，4 位小数）
    /// </summary>
    public decimal MovingPrice { get; set; }

    /// <summary>
    /// 差异码（6）
    /// </summary>
    public string? DifferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
    /// </summary>
    public string ProfitCenter { get; set; } = string.Empty;

    /// <summary>
    /// 当前库存（基本单位数量，decimal，4 位小数）
    /// </summary>
    public decimal CurrentStock { get; set; }

    /// <summary>
    /// 生产仓储（选项 TaktWarehouses/options；DictValue=WarehouseCode）
    /// </summary>
    public string ProductionLocation { get; set; } = string.Empty;

    /// <summary>
    /// 采购仓储（选项 TaktWarehouses/options；DictValue=WarehouseCode）
    /// </summary>
    public string PurchasingLocation { get; set; } = string.Empty;

    /// <summary>
    /// 库位（选项 TaktStorageLocations/options；DictValue=LocationCode）
    /// </summary>
    public string StorageLocation { get; set; } = string.Empty;

    /// <summary>
    /// 是否需检验（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int RequiresInspection { get; set; } = 0;

    /// <summary>
    /// 批次标识（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int IsBatch { get; set; } = 0;

    /// <summary>
    /// 停产状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料）
    /// </summary>
    public string DiscontinuedStatus { get; set; } = string.Empty;

    /// <summary>
    /// 物料状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int MaterialStatus { get; set; } = 0;

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
