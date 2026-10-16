// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Procurement
// 文件名称：TaktPurchaseForecastDtos.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Auto Generated)
// 功能描述：PurchaseForecast 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPurchaseForecast 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Procurement;

// ========================================
// PurchaseForecast 响应 DTO
// ========================================

/// <summary>
/// Takt采购预测实体（公司级；我方发给供应商的需求预测，结构对齐 TaktSalesForecast；同编码多版靠发出版本号；不进入我方 MDS/MRP 采购计划）
/// 对应前端 TaktPurchaseForecastDto
/// 继承 TaktApprovalDtoBase
/// </summary>
public class TaktPurchaseForecastDto : TaktApprovalDtoBase
{
    /// <summary>
    /// PurchaseForecastID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseForecastId { get; set; }

    /// <summary>
    /// 采购预测编码（租户+公司+工厂内与发出版本号组合业务唯一）
    /// </summary>
    public string PurchaseForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划编制日期（业务计划日；与发出日期分离）
    /// </summary>
    public DateTime PlanDate { get; set; }

    /// <summary>
    /// 发出日期（我方将该版采购预测发给供应商的日期；对应销售预测的接收日期）
    /// </summary>
    public DateTime SendDate { get; set; }

    /// <summary>
    /// 发出版本号（同工厂+预测编码下递增；从 1 起；对应销售预测的接收版本号）
    /// </summary>
    public int SendVersionNo { get; set; } = 0;

    /// <summary>
    /// 产品（四阶第 1 层；仅允许固定字面量 Product，长度固定 7；服务层写入强制覆盖）
    /// </summary>
    public string SalesProduct { get; set; } = string.Empty;

    /// <summary>
    /// 产品类别（字典 logistics_manufacturing_mds_product_category；DictValue=CAD/ISD/PAD；四阶第 2 层）
    /// </summary>
    public string ProductCategoryCode { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode；四阶第 3 层）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（关联 TaktModelDestination.ModelCode；四阶第 4 层）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode；具体 SKU）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；汇总计划时可为空，DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
    /// </summary>
    public string? SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）
    /// </summary>
    public string? PlannerGroupCode { get; set; }


    /// <summary>
    /// 计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）
    /// </summary>
    public string? PlannerGroupName { get; set; } = string.Empty;

    /// <summary>
    /// 计划总数量（基本单位数量；通常汇总版本 002）
    /// </summary>
    public decimal TotalQuantity { get; set; }

    /// <summary>
    /// 计划总金额
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 已转采购数量（基本单位数量）
    /// </summary>
    public decimal ConvertedQuantity { get; set; }

    /// <summary>
    /// 已转采购金额
    /// </summary>
    public decimal ConvertedAmount { get; set; }

    /// <summary>
    /// 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
    /// </summary>
    public int PlanStatus { get; set; } = 0;

    /// <summary>
    /// 转换状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
    /// </summary>
    public int ConvertedStatus { get; set; } = 0;

    /// <summary>
    /// 计划说明
    /// </summary>
    public string? PlanDescription { get; set; } = string.Empty;

    /// <summary>
    /// 采购预测明细列表（主子表；一行=财年×月计划量 001/002/增减；维度在主表）
    /// （子表：TaktPurchaseForecastItem）
    /// </summary>
    public List<TaktPurchaseForecastItemDto>? Items { get; set; }

}

// ========================================
// PurchaseForecast 查询 DTO
// ========================================

/// <summary>
/// PurchaseForecast 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPurchaseForecastQueryDto : TaktPagedQuery
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
    /// 采购预测编码（租户+公司+工厂内与发出版本号组合业务唯一）
    /// </summary>
    public string? PurchaseForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划编制日期（业务计划日；与发出日期分离）（范围查询-开始）
    /// </summary>
    public DateTime? PlanDateStart { get; set; }

    /// <summary>
    /// 计划编制日期（业务计划日；与发出日期分离）（范围查询-结束）
    /// </summary>
    public DateTime? PlanDateEnd { get; set; }

    /// <summary>
    /// 发出日期（我方将该版采购预测发给供应商的日期；对应销售预测的接收日期）（范围查询-开始）
    /// </summary>
    public DateTime? SendDateStart { get; set; }

    /// <summary>
    /// 发出日期（我方将该版采购预测发给供应商的日期；对应销售预测的接收日期）（范围查询-结束）
    /// </summary>
    public DateTime? SendDateEnd { get; set; }

    /// <summary>
    /// 发出版本号（同工厂+预测编码下递增；从 1 起；对应销售预测的接收版本号）
    /// </summary>
    public int? SendVersionNo { get; set; }

    /// <summary>
    /// 产品（四阶第 1 层；仅允许固定字面量 Product，长度固定 7；服务层写入强制覆盖）
    /// </summary>
    public string? SalesProduct { get; set; } = string.Empty;

    /// <summary>
    /// 产品类别（字典 logistics_manufacturing_mds_product_category；DictValue=CAD/ISD/PAD；四阶第 2 层）
    /// </summary>
    public string? ProductCategoryCode { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode；四阶第 3 层）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（关联 TaktModelDestination.ModelCode；四阶第 4 层）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode；具体 SKU）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；汇总计划时可为空，DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
    /// </summary>
    public string? SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）
    /// </summary>
    public string? PlannerGroupCode { get; set; }

    /// <summary>
    /// 计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）
    /// </summary>
    public string? PlannerGroupName { get; set; } = string.Empty;

    /// <summary>
    /// 计划总数量（基本单位数量；通常汇总版本 002）
    /// </summary>
    public decimal? TotalQuantity { get; set; }

    /// <summary>
    /// 计划总金额
    /// </summary>
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// 已转采购数量（基本单位数量）
    /// </summary>
    public decimal? ConvertedQuantity { get; set; }

    /// <summary>
    /// 已转采购金额
    /// </summary>
    public decimal? ConvertedAmount { get; set; }

    /// <summary>
    /// 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
    /// </summary>
    public int? PlanStatus { get; set; }

    /// <summary>
    /// 转换状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
    /// </summary>
    public int? ConvertedStatus { get; set; }

    /// <summary>
    /// 计划说明
    /// </summary>
    public string? PlanDescription { get; set; } = string.Empty;

    /// <summary>
    /// 审批状态（字典 sys_approval_status；与 TaktApprovalEntityBase.ApprovalStatus 一致）
    /// </summary>
    public TaktApprovalStatus? ApprovalStatus { get; set; }

    /// <summary>
    /// 发起人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InitiatorId { get; set; }

    /// <summary>
    /// 发起时间（范围查询-开始）
    /// </summary>
    public DateTime? InitiatedAtStart { get; set; }

    /// <summary>
    /// 发起时间（范围查询-结束）
    /// </summary>
    public DateTime? InitiatedAtEnd { get; set; }

    /// <summary>
    /// 最终审批人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApprovedBy { get; set; }

    /// <summary>
    /// 最终审批时间（范围查询-开始）
    /// </summary>
    public DateTime? ApprovedAtStart { get; set; }

    /// <summary>
    /// 最终审批时间（范围查询-结束）
    /// </summary>
    public DateTime? ApprovedAtEnd { get; set; }

    /// <summary>
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FlowInstanceId { get; set; }

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
// 创建PurchaseForecast DTO
// ========================================

/// <summary>
/// 创建PurchaseForecast DTO
/// </summary>
public class TaktPurchaseForecastCreateDto
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
    /// 采购预测编码（租户+公司+工厂内与发出版本号组合业务唯一；前端表单选择编码规则后自动通过 TaktNumbering 采购预测编码规则生成并展示，非手输；单据类型菜单：采购预测）
    /// </summary>
    [Required(ErrorMessage = "采购预测编码（租户+公司+工厂内与发出版本号组合业务唯一）不能为空")]
    public string PurchaseForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 编码规则编码（新增时选择采购预测编码规则后取号；对应 TaktNumbering.RuleCode；不落库）
    /// </summary>
    public string? NumberingRuleCode { get; set; }

    /// <summary>
    /// 计划编制日期（业务计划日；与发出日期分离）
    /// </summary>
    public DateTime PlanDate { get; set; }

    /// <summary>
    /// 发出日期（我方将该版采购预测发给供应商的日期；对应销售预测的接收日期）
    /// </summary>
    public DateTime SendDate { get; set; }

    /// <summary>
    /// 发出版本号（同工厂+预测编码下递增；从 1 起；对应销售预测的接收版本号）
    /// </summary>
    public int SendVersionNo { get; set; } = 0;

    /// <summary>
    /// 产品（四阶第 1 层；仅允许固定字面量 Product，长度固定 7；服务层写入强制覆盖）
    /// </summary>
    [Required(ErrorMessage = "产品（四阶第 1 层；仅允许固定字面量 Product，长度固定 7；服务层写入强制覆盖）不能为空")]
    public string SalesProduct { get; set; } = string.Empty;

    /// <summary>
    /// 产品类别（字典 logistics_manufacturing_mds_product_category；DictValue=CAD/ISD/PAD；四阶第 2 层）
    /// </summary>
    [Required(ErrorMessage = "产品类别（字典 logistics_manufacturing_mds_product_category；DictValue=CAD/ISD/PAD；四阶第 2 层）不能为空")]
    public string ProductCategoryCode { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode；四阶第 3 层）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（关联 TaktModelDestination.ModelCode；四阶第 4 层）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode；具体 SKU）
    /// </summary>
    [Required(ErrorMessage = "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode；具体 SKU）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    [Required(ErrorMessage = "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）不能为空")]
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；汇总计划时可为空，DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
    /// </summary>
    public string? SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）
    /// </summary>
    public string? PlannerGroupCode { get; set; }

    /// <summary>
    /// 计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）
    /// </summary>
    public string? PlannerGroupName { get; set; } = string.Empty;

    /// <summary>
    /// 计划总数量（基本单位数量；通常汇总版本 002）
    /// </summary>
    public decimal TotalQuantity { get; set; }

    /// <summary>
    /// 计划总金额
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 已转采购数量（基本单位数量）
    /// </summary>
    public decimal ConvertedQuantity { get; set; }

    /// <summary>
    /// 已转采购金额
    /// </summary>
    public decimal ConvertedAmount { get; set; }

    /// <summary>
    /// 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
    /// </summary>
    public int PlanStatus { get; set; } = 0;

    /// <summary>
    /// 转换状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
    /// </summary>
    public int ConvertedStatus { get; set; } = 0;

    /// <summary>
    /// 计划说明
    /// </summary>
    public string? PlanDescription { get; set; } = string.Empty;

    /// <summary>
    /// 采购预测明细列表（主子表；一行=财年×月计划量 001/002/增减；维度在主表）（子表，级联保存）
    /// </summary>
    public List<TaktPurchaseForecastItemCreateDto>? Items { get; set; }

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
// 更新PurchaseForecast DTO
// ========================================

/// <summary>
/// 更新PurchaseForecast DTO
/// 继承 TaktPurchaseForecastCreateDto，添加 PurchaseForecastId 字段
/// </summary>
public class TaktPurchaseForecastUpdateDto : TaktPurchaseForecastCreateDto
{
    /// <summary>
    /// PurchaseForecastID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseForecastId { get; set; }

    /// <summary>
    /// 采购预测明细列表（主子表；一行=财年×月计划量 001/002/增减；维度在主表）（子表，级联保存）
    /// </summary>
    public new List<TaktPurchaseForecastItemUpdateDto>? Items { get; set; }

}

// ========================================
// PurchaseForecast 状态 DTO
// ========================================

/// <summary>
/// PurchaseForecast 状态更新 DTO
/// </summary>
public class TaktPurchaseForecastStatusDto
{
    /// <summary>
    /// PurchaseForecastID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseForecastId { get; set; }

    /// <summary>
    /// 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
    /// </summary>
    [Required(ErrorMessage = "计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）不能为空")]
    public int PlanStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// PurchaseForecast 导入模板行 DTO
/// </summary>
public class TaktPurchaseForecastTemplateDto
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
    /// 采购预测编码（租户+公司+工厂内与发出版本号组合业务唯一）
    /// </summary>
    public string? PurchaseForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划编制日期（业务计划日；与发出日期分离）
    /// </summary>
    public DateTime? PlanDate { get; set; }

    /// <summary>
    /// 发出日期（我方将该版采购预测发给供应商的日期；对应销售预测的接收日期）
    /// </summary>
    public DateTime? SendDate { get; set; }

    /// <summary>
    /// 发出版本号（同工厂+预测编码下递增；从 1 起；对应销售预测的接收版本号）
    /// </summary>
    public int? SendVersionNo { get; set; }

    /// <summary>
    /// 产品（四阶第 1 层；仅允许固定字面量 Product，长度固定 7；服务层写入强制覆盖）
    /// </summary>
    public string? SalesProduct { get; set; } = string.Empty;

    /// <summary>
    /// 产品类别（字典 logistics_manufacturing_mds_product_category；DictValue=CAD/ISD/PAD；四阶第 2 层）
    /// </summary>
    public string? ProductCategoryCode { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode；四阶第 3 层）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（关联 TaktModelDestination.ModelCode；四阶第 4 层）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode；具体 SKU）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；汇总计划时可为空，DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
    /// </summary>
    public string? SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）
    /// </summary>
    public string? PlannerGroupCode { get; set; }

    /// <summary>
    /// 计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）
    /// </summary>
    public string? PlannerGroupName { get; set; } = string.Empty;

    /// <summary>
    /// 计划总数量（基本单位数量；通常汇总版本 002）
    /// </summary>
    public decimal? TotalQuantity { get; set; }

    /// <summary>
    /// 计划总金额
    /// </summary>
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// 已转采购数量（基本单位数量）
    /// </summary>
    public decimal? ConvertedQuantity { get; set; }

    /// <summary>
    /// 已转采购金额
    /// </summary>
    public decimal? ConvertedAmount { get; set; }

    /// <summary>
    /// 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
    /// </summary>
    public int? PlanStatus { get; set; }

    /// <summary>
    /// 转换状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
    /// </summary>
    public int? ConvertedStatus { get; set; }

    /// <summary>
    /// 计划说明
    /// </summary>
    public string? PlanDescription { get; set; } = string.Empty;

    /// <summary>
    /// 采购预测明细列表（主子表；一行=财年×月计划量 001/002/增减；维度在主表）（子表，级联保存）
    /// </summary>
    public List<TaktPurchaseForecastItemCreateDto>? Items { get; set; }

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
/// PurchaseForecast 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPurchaseForecastImportDto
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
    /// 采购预测编码（租户+公司+工厂内与发出版本号组合业务唯一）
    /// </summary>
    public string? PurchaseForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划编制日期（业务计划日；与发出日期分离）
    /// </summary>
    public DateTime? PlanDate { get; set; }

    /// <summary>
    /// 发出日期（我方将该版采购预测发给供应商的日期；对应销售预测的接收日期）
    /// </summary>
    public DateTime? SendDate { get; set; }

    /// <summary>
    /// 发出版本号（同工厂+预测编码下递增；从 1 起；对应销售预测的接收版本号）
    /// </summary>
    public int? SendVersionNo { get; set; }

    /// <summary>
    /// 产品（四阶第 1 层；仅允许固定字面量 Product，长度固定 7；服务层写入强制覆盖）
    /// </summary>
    public string? SalesProduct { get; set; } = string.Empty;

    /// <summary>
    /// 产品类别（字典 logistics_manufacturing_mds_product_category；DictValue=CAD/ISD/PAD；四阶第 2 层）
    /// </summary>
    public string? ProductCategoryCode { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode；四阶第 3 层）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（关联 TaktModelDestination.ModelCode；四阶第 4 层）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode；具体 SKU）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；汇总计划时可为空，DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
    /// </summary>
    public string? SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）
    /// </summary>
    public string? PlannerGroupCode { get; set; }

    /// <summary>
    /// 计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）
    /// </summary>
    public string? PlannerGroupName { get; set; } = string.Empty;

    /// <summary>
    /// 计划总数量（基本单位数量；通常汇总版本 002）
    /// </summary>
    public decimal? TotalQuantity { get; set; }

    /// <summary>
    /// 计划总金额
    /// </summary>
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// 已转采购数量（基本单位数量）
    /// </summary>
    public decimal? ConvertedQuantity { get; set; }

    /// <summary>
    /// 已转采购金额
    /// </summary>
    public decimal? ConvertedAmount { get; set; }

    /// <summary>
    /// 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
    /// </summary>
    public int? PlanStatus { get; set; }

    /// <summary>
    /// 转换状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
    /// </summary>
    public int? ConvertedStatus { get; set; }

    /// <summary>
    /// 计划说明
    /// </summary>
    public string? PlanDescription { get; set; } = string.Empty;

    /// <summary>
    /// 采购预测明细列表（主子表；一行=财年×月计划量 001/002/增减；维度在主表）（子表，级联保存）
    /// </summary>
    public List<TaktPurchaseForecastItemCreateDto>? Items { get; set; }

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
/// PurchaseForecast 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPurchaseForecastExportDto
{
    /// <summary>
    /// PurchaseForecastID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseForecastId { get; set; }

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
    /// 采购预测编码（租户+公司+工厂内与发出版本号组合业务唯一）
    /// </summary>
    public string PurchaseForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划编制日期（业务计划日；与发出日期分离）
    /// </summary>
    public DateTime PlanDate { get; set; }

    /// <summary>
    /// 发出日期（我方将该版采购预测发给供应商的日期；对应销售预测的接收日期）
    /// </summary>
    public DateTime SendDate { get; set; }

    /// <summary>
    /// 发出版本号（同工厂+预测编码下递增；从 1 起；对应销售预测的接收版本号）
    /// </summary>
    public int SendVersionNo { get; set; } = 0;

    /// <summary>
    /// 产品（四阶第 1 层；仅允许固定字面量 Product，长度固定 7；服务层写入强制覆盖）
    /// </summary>
    public string SalesProduct { get; set; } = string.Empty;

    /// <summary>
    /// 产品类别（字典 logistics_manufacturing_mds_product_category；DictValue=CAD/ISD/PAD；四阶第 2 层）
    /// </summary>
    public string ProductCategoryCode { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode；四阶第 3 层）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（关联 TaktModelDestination.ModelCode；四阶第 4 层）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode；具体 SKU）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；汇总计划时可为空，DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
    /// </summary>
    public string? SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）
    /// </summary>
    public string? PlannerGroupCode { get; set; }

    /// <summary>
    /// 计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）
    /// </summary>
    public string? PlannerGroupName { get; set; } = string.Empty;

    /// <summary>
    /// 计划总数量（基本单位数量；通常汇总版本 002）
    /// </summary>
    public decimal TotalQuantity { get; set; }

    /// <summary>
    /// 计划总金额
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 已转采购数量（基本单位数量）
    /// </summary>
    public decimal ConvertedQuantity { get; set; }

    /// <summary>
    /// 已转采购金额
    /// </summary>
    public decimal ConvertedAmount { get; set; }

    /// <summary>
    /// 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
    /// </summary>
    public int PlanStatus { get; set; } = 0;

    /// <summary>
    /// 转换状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
    /// </summary>
    public int ConvertedStatus { get; set; } = 0;

    /// <summary>
    /// 计划说明
    /// </summary>
    public string? PlanDescription { get; set; } = string.Empty;

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
