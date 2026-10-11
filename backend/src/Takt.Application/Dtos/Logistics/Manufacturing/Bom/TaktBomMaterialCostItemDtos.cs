// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialCostItemDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：BomMaterialCostItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktBomMaterialCostItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Bom;

// ========================================
// BomMaterialCostItem 响应 DTO
// ========================================

/// <summary>
/// BOM 物料成本明细行（业务源数据：先导入/维护明细，再按工厂+产品+核算月聚合写入 TaktBomMaterialCost；无线上主表外键）
/// 对应前端 TaktBomMaterialCostItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktBomMaterialCostItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// BomMaterialCostItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BomMaterialCostItemId { get; set; }


    /// <summary>
    /// 层级（BOM 展开层级，如 01/02）
    /// </summary>
    public string BomLevel { get; set; } = string.Empty;

    /// <summary>
    /// BOM 项目号（子件行项目号，如 0010）
    /// </summary>
    public string BomItemCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码（父件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位
    /// </summary>
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 10;

    /// <summary>
    /// 产品描述
    /// </summary>
    public string ProductDescription { get; set; } = string.Empty;

    /// <summary>
    /// 组件编码（子件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位
    /// </summary>
    public string ComponentCode { get; set; } = string.Empty;

    /// <summary>
    /// 组件描述
    /// </summary>
    public string ComponentDescription { get; set; } = string.Empty;

    /// <summary>
    /// 组件数量
    /// </summary>
    public decimal ComponentQuantity { get; set; }

    /// <summary>
    /// 批量标识（空或 X）
    /// </summary>
    public string? BatchIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 生产相关（空或 X）
    /// </summary>
    public string? ProductionRelated { get; set; } = string.Empty;

    /// <summary>
    /// PCB SECT 标识（空或 X；为 X 时本行不参与任何成本计算）
    /// </summary>
    public string? PcbSectIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 采购类型（F=外部采购，E=自制生产）；仅生产相关=X、PCB SECT 标识为空且 F 行参与产品 BOM 材料成本汇总，行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数
    /// </summary>
    public string PurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 特殊采购类（空或业务码，最长 50）
    /// </summary>
    public string? SpecialProcurementType { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=Id）
    /// </summary>
    public string ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 移动平均价（5 位小数）
    /// </summary>
    public decimal MovingAveragePrice { get; set; }

    /// <summary>
    /// 移动价格单位
    /// </summary>
    public int MovingPriceUnit { get; set; } = 0;

    /// <summary>
    /// 移动价格货币（字典 accounting_financial_currency_code；如 CNY/USD）
    /// </summary>
    public string MovingPriceCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购组织
    /// </summary>
    public string PurchaseOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options；DictValue=Id）
    /// </summary>
    public string PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 净价（采购价格，5 位小数）
    /// </summary>
    public decimal NetPurchasePrice { get; set; }

    /// <summary>
    /// 采购价格单位
    /// </summary>
    public int PurchasePriceUnit { get; set; } = 0;

    /// <summary>
    /// 采购货币（字典 accounting_financial_currency_code；如 CNY/USD）
    /// </summary>
    public string PurchaseCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 核算日期
    /// </summary>
    public DateTime CostingDate { get; set; }

}

// ========================================
// BomMaterialCostItem 查询 DTO
// ========================================

/// <summary>
/// BomMaterialCostItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktBomMaterialCostItemQueryDto : TaktPagedQuery
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
    /// 机种编码（分析/成本推移查询条件；回填 ProductCodes 后用于明细过滤）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 层级（BOM 展开层级，如 01/02）
    /// </summary>
    public string? BomLevel { get; set; } = string.Empty;

    /// <summary>
    /// BOM 项目号（子件行项目号，如 0010）
    /// </summary>
    public string? BomItemCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码（父件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位
    /// </summary>
    public string? ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 共用产品编码集合（由 ModelCode 回填；不显式指定产品时用于缩小范围）
    /// </summary>
    public List<string>? ProductCodes { get; set; }

    /// <summary>
    /// 产品描述
    /// </summary>
    public string? ProductDescription { get; set; } = string.Empty;

    /// <summary>
    /// 组件编码（子件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位
    /// </summary>
    public string? ComponentCode { get; set; } = string.Empty;

    /// <summary>
    /// 组件描述
    /// </summary>
    public string? ComponentDescription { get; set; } = string.Empty;

    /// <summary>
    /// 组件数量
    /// </summary>
    public decimal? ComponentQuantity { get; set; }

    /// <summary>
    /// 批量标识（空或 X）
    /// </summary>
    public string? BatchIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 生产相关（空或 X）
    /// </summary>
    public string? ProductionRelated { get; set; } = string.Empty;

    /// <summary>
    /// PCB SECT 标识（空或 X；为 X 时本行不参与任何成本计算）
    /// </summary>
    public string? PcbSectIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 采购类型（F=外部采购，E=自制生产）；仅生产相关=X、PCB SECT 标识为空且 F 行参与产品 BOM 材料成本汇总，行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数
    /// </summary>
    public string? PurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 特殊采购类（空或业务码，最长 50）
    /// </summary>
    public string? SpecialProcurementType { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=Id）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 移动平均价（5 位小数）
    /// </summary>
    public decimal? MovingAveragePrice { get; set; }

    /// <summary>
    /// 移动价格单位
    /// </summary>
    public int? MovingPriceUnit { get; set; }

    /// <summary>
    /// 移动价格货币（字典 accounting_financial_currency_code；如 CNY/USD）
    /// </summary>
    public string? MovingPriceCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购组织
    /// </summary>
    public string? PurchaseOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options；DictValue=Id）
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 净价（采购价格，5 位小数）
    /// </summary>
    public decimal? NetPurchasePrice { get; set; }

    /// <summary>
    /// 采购价格单位
    /// </summary>
    public int? PurchasePriceUnit { get; set; }

    /// <summary>
    /// 采购货币（字典 accounting_financial_currency_code；如 CNY/USD）
    /// </summary>
    public string? PurchaseCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 核算日期（范围查询-开始）
    /// </summary>
    public DateTime? CostingDateStart { get; set; }

    /// <summary>
    /// 核算日期（范围查询-结束）
    /// </summary>
    public DateTime? CostingDateEnd { get; set; }

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
// 创建BomMaterialCostItem DTO
// ========================================

/// <summary>
/// 创建BomMaterialCostItem DTO
/// </summary>
public class TaktBomMaterialCostItemCreateDto
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
    /// 层级（BOM 展开层级，如 01/02）
    /// </summary>
    [Required(ErrorMessage = "层级（BOM 展开层级，如 01/02）不能为空")]
    public string BomLevel { get; set; } = string.Empty;

    /// <summary>
    /// BOM 项目号（子件行项目号，如 0010）
    /// </summary>
    [Required(ErrorMessage = "BOM 项目号（子件行项目号，如 0010）不能为空")]
    public string BomItemCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码（父件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位
    /// </summary>
    [Required(ErrorMessage = "产品编码（父件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位不能为空")]
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 10;

    /// <summary>
    /// 产品描述
    /// </summary>
    [Required(ErrorMessage = "产品描述不能为空")]
    public string ProductDescription { get; set; } = string.Empty;

    /// <summary>
    /// 组件编码（子件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位
    /// </summary>
    [Required(ErrorMessage = "组件编码（子件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位不能为空")]
    public string ComponentCode { get; set; } = string.Empty;

    /// <summary>
    /// 组件描述
    /// </summary>
    [Required(ErrorMessage = "组件描述不能为空")]
    public string ComponentDescription { get; set; } = string.Empty;

    /// <summary>
    /// 组件数量
    /// </summary>
    public decimal ComponentQuantity { get; set; }

    /// <summary>
    /// 批量标识（空或 X）
    /// </summary>
    public string? BatchIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 生产相关（空或 X）
    /// </summary>
    public string? ProductionRelated { get; set; } = string.Empty;

    /// <summary>
    /// PCB SECT 标识（空或 X；为 X 时本行不参与任何成本计算）
    /// </summary>
    public string? PcbSectIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 采购类型（F=外部采购，E=自制生产）；仅生产相关=X、PCB SECT 标识为空且 F 行参与产品 BOM 材料成本汇总，行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数
    /// </summary>
    [Required(ErrorMessage = "采购类型（F=外部采购，E=自制生产）；仅生产相关=X、PCB SECT 标识为空且 F 行参与产品 BOM 材料成本汇总，行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数不能为空")]
    public string PurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 特殊采购类（空或业务码，最长 50）
    /// </summary>
    public string? SpecialProcurementType { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=Id）
    /// </summary>
    [Required(ErrorMessage = "利润中心（选项 TaktProfitCenters/options；DictValue=Id）不能为空")]
    public string ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 移动平均价（5 位小数）
    /// </summary>
    public decimal MovingAveragePrice { get; set; }

    /// <summary>
    /// 移动价格单位
    /// </summary>
    public int MovingPriceUnit { get; set; } = 0;

    /// <summary>
    /// 移动价格货币（字典 accounting_financial_currency_code；如 CNY/USD）
    /// </summary>
    [Required(ErrorMessage = "移动价格货币（字典 accounting_financial_currency_code；如 CNY/USD）不能为空")]
    public string MovingPriceCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购组织
    /// </summary>
    [Required(ErrorMessage = "采购组织不能为空")]
    public string PurchaseOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options；DictValue=Id）
    /// </summary>
    [Required(ErrorMessage = "采购组（选项 TaktPurchaseGroups/options；DictValue=Id）不能为空")]
    public string PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    [Required(ErrorMessage = "供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）不能为空")]
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 净价（采购价格，5 位小数）
    /// </summary>
    public decimal NetPurchasePrice { get; set; }

    /// <summary>
    /// 采购价格单位
    /// </summary>
    public int PurchasePriceUnit { get; set; } = 0;

    /// <summary>
    /// 采购货币（字典 accounting_financial_currency_code；如 CNY/USD）
    /// </summary>
    [Required(ErrorMessage = "采购货币（字典 accounting_financial_currency_code；如 CNY/USD）不能为空")]
    public string PurchaseCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 核算日期
    /// </summary>
    public DateTime CostingDate { get; set; }

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
// 更新BomMaterialCostItem DTO
// ========================================

/// <summary>
/// 更新BomMaterialCostItem DTO
/// 继承 TaktBomMaterialCostItemCreateDto，添加 BomMaterialCostItemId 字段
/// </summary>
public class TaktBomMaterialCostItemUpdateDto : TaktBomMaterialCostItemCreateDto
{
    /// <summary>
    /// BomMaterialCostItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BomMaterialCostItemId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// BomMaterialCostItem 导入模板行 DTO
/// </summary>
public class TaktBomMaterialCostItemTemplateDto
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
    /// 层级（BOM 展开层级，如 01/02）
    /// </summary>
    public string? BomLevel { get; set; } = string.Empty;

    /// <summary>
    /// BOM 项目号（子件行项目号，如 0010）
    /// </summary>
    public string? BomItemCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码（父件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位
    /// </summary>
    public string? ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 产品描述
    /// </summary>
    public string? ProductDescription { get; set; } = string.Empty;

    /// <summary>
    /// 组件编码（子件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位
    /// </summary>
    public string? ComponentCode { get; set; } = string.Empty;

    /// <summary>
    /// 组件描述
    /// </summary>
    public string? ComponentDescription { get; set; } = string.Empty;

    /// <summary>
    /// 组件数量
    /// </summary>
    public decimal? ComponentQuantity { get; set; }

    /// <summary>
    /// 批量标识（空或 X）
    /// </summary>
    public string? BatchIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 生产相关（空或 X）
    /// </summary>
    public string? ProductionRelated { get; set; } = string.Empty;

    /// <summary>
    /// PCB SECT 标识（空或 X；为 X 时本行不参与任何成本计算）
    /// </summary>
    public string? PcbSectIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 采购类型（F=外部采购，E=自制生产）；仅生产相关=X、PCB SECT 标识为空且 F 行参与产品 BOM 材料成本汇总，行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数
    /// </summary>
    public string? PurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 特殊采购类（空或业务码，最长 50）
    /// </summary>
    public string? SpecialProcurementType { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=Id）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 移动平均价（5 位小数）
    /// </summary>
    public decimal? MovingAveragePrice { get; set; }

    /// <summary>
    /// 移动价格单位
    /// </summary>
    public int? MovingPriceUnit { get; set; }

    /// <summary>
    /// 移动价格货币（字典 accounting_financial_currency_code；如 CNY/USD）
    /// </summary>
    public string? MovingPriceCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购组织
    /// </summary>
    public string? PurchaseOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options；DictValue=Id）
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 净价（采购价格，5 位小数）
    /// </summary>
    public decimal? NetPurchasePrice { get; set; }

    /// <summary>
    /// 采购价格单位
    /// </summary>
    public int? PurchasePriceUnit { get; set; }

    /// <summary>
    /// 采购货币（字典 accounting_financial_currency_code；如 CNY/USD）
    /// </summary>
    public string? PurchaseCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 核算日期
    /// </summary>
    public DateTime? CostingDate { get; set; }

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
/// BomMaterialCostItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktBomMaterialCostItemImportDto
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
    /// 层级（BOM 展开层级，如 01/02）
    /// </summary>
    public string? BomLevel { get; set; } = string.Empty;

    /// <summary>
    /// BOM 项目号（子件行项目号，如 0010）
    /// </summary>
    public string? BomItemCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码（父件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位
    /// </summary>
    public string? ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 产品描述
    /// </summary>
    public string? ProductDescription { get; set; } = string.Empty;

    /// <summary>
    /// 组件编码（子件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位
    /// </summary>
    public string? ComponentCode { get; set; } = string.Empty;

    /// <summary>
    /// 组件描述
    /// </summary>
    public string? ComponentDescription { get; set; } = string.Empty;

    /// <summary>
    /// 组件数量
    /// </summary>
    public decimal? ComponentQuantity { get; set; }

    /// <summary>
    /// 批量标识（空或 X）
    /// </summary>
    public string? BatchIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 生产相关（空或 X）
    /// </summary>
    public string? ProductionRelated { get; set; } = string.Empty;

    /// <summary>
    /// PCB SECT 标识（空或 X；为 X 时本行不参与任何成本计算）
    /// </summary>
    public string? PcbSectIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 采购类型（F=外部采购，E=自制生产）；仅生产相关=X、PCB SECT 标识为空且 F 行参与产品 BOM 材料成本汇总，行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数
    /// </summary>
    public string? PurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 特殊采购类（空或业务码，最长 50）
    /// </summary>
    public string? SpecialProcurementType { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=Id）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 移动平均价（5 位小数）
    /// </summary>
    public decimal? MovingAveragePrice { get; set; }

    /// <summary>
    /// 移动价格单位
    /// </summary>
    public int? MovingPriceUnit { get; set; }

    /// <summary>
    /// 移动价格货币（字典 accounting_financial_currency_code；如 CNY/USD）
    /// </summary>
    public string? MovingPriceCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购组织
    /// </summary>
    public string? PurchaseOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options；DictValue=Id）
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 净价（采购价格，5 位小数）
    /// </summary>
    public decimal? NetPurchasePrice { get; set; }

    /// <summary>
    /// 采购价格单位
    /// </summary>
    public int? PurchasePriceUnit { get; set; }

    /// <summary>
    /// 采购货币（字典 accounting_financial_currency_code；如 CNY/USD）
    /// </summary>
    public string? PurchaseCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 核算日期
    /// </summary>
    public DateTime? CostingDate { get; set; }

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
/// BomMaterialCostItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktBomMaterialCostItemExportDto
{
    /// <summary>
    /// BomMaterialCostItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BomMaterialCostItemId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 层级（BOM 展开层级，如 01/02）
    /// </summary>
    public string BomLevel { get; set; } = string.Empty;

    /// <summary>
    /// BOM 项目号（子件行项目号，如 0010）
    /// </summary>
    public string BomItemCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码（父件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位
    /// </summary>
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 10;

    /// <summary>
    /// 产品描述
    /// </summary>
    public string ProductDescription { get; set; } = string.Empty;

    /// <summary>
    /// 组件编码（子件物料编码，选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）；导入时 18 位纯数字自动归一化为后 10 位
    /// </summary>
    public string ComponentCode { get; set; } = string.Empty;

    /// <summary>
    /// 组件描述
    /// </summary>
    public string ComponentDescription { get; set; } = string.Empty;

    /// <summary>
    /// 组件数量
    /// </summary>
    public decimal ComponentQuantity { get; set; }

    /// <summary>
    /// 批量标识（空或 X）
    /// </summary>
    public string? BatchIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 生产相关（空或 X）
    /// </summary>
    public string? ProductionRelated { get; set; } = string.Empty;

    /// <summary>
    /// PCB SECT 标识（空或 X；为 X 时本行不参与任何成本计算）
    /// </summary>
    public string? PcbSectIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 采购类型（F=外部采购，E=自制生产）；仅生产相关=X、PCB SECT 标识为空且 F 行参与产品 BOM 材料成本汇总，行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数
    /// </summary>
    public string PurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 特殊采购类（空或业务码，最长 50）
    /// </summary>
    public string? SpecialProcurementType { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=Id）
    /// </summary>
    public string ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 移动平均价（5 位小数）
    /// </summary>
    public decimal MovingAveragePrice { get; set; }

    /// <summary>
    /// 移动价格单位
    /// </summary>
    public int MovingPriceUnit { get; set; } = 0;

    /// <summary>
    /// 移动价格货币（字典 accounting_financial_currency_code；如 CNY/USD）
    /// </summary>
    public string MovingPriceCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购组织
    /// </summary>
    public string PurchaseOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options；DictValue=Id）
    /// </summary>
    public string PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 净价（采购价格，5 位小数）
    /// </summary>
    public decimal NetPurchasePrice { get; set; }

    /// <summary>
    /// 采购价格单位
    /// </summary>
    public int PurchasePriceUnit { get; set; } = 0;

    /// <summary>
    /// 采购货币（字典 accounting_financial_currency_code；如 CNY/USD）
    /// </summary>
    public string PurchaseCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 核算日期
    /// </summary>
    public DateTime CostingDate { get; set; }

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
