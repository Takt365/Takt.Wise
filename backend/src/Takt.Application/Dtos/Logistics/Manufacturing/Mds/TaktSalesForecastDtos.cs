// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Mds
// 文件名称：TaktSalesForecastDtos.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Auto Generated)
// 功能描述：SalesForecast 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSalesForecast 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Mds;

// ========================================
// SalesForecast 响应 DTO
// ========================================

/// <summary>
/// Takt销售预测实体（公司级；客户发给我方的销售预测，可进 MDS；同编码多版靠接收版本号）
/// 对应前端 TaktSalesForecastDto
/// 继承 TaktApprovalDtoBase
/// </summary>
public class TaktSalesForecastDto : TaktApprovalDtoBase
{
    /// <summary>
    /// SalesForecastID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesForecastId { get; set; }

    /// <summary>
    /// 销售预测编码（租户+公司+工厂内与接收版本号组合业务唯一）
    /// </summary>
    public string SalesForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划编制日期（客户侧业务计划日；与接收日期分离）
    /// </summary>
    public DateTime PlanDate { get; set; }

    /// <summary>
    /// 接收日期（我方收到该版客户销售预测的日期）
    /// </summary>
    public DateTime ReceiveDate { get; set; }

    /// <summary>
    /// 接收版本号（同工厂+预测编码下递增；从 1 起）
    /// </summary>
    public int ReceiveVersionNo { get; set; } = 0;

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
    /// 客户编码（选项 TaktCustomers/options；汇总计划时可为空，DictValue=Id）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
    /// </summary>
    public string? CustomerName1 { get; set; } = string.Empty;

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
    /// 已转生产/销售数量（基本单位数量）
    /// </summary>
    public decimal ConvertedQuantity { get; set; }

    /// <summary>
    /// 已转生产/销售金额
    /// </summary>
    public decimal ConvertedAmount { get; set; }

    /// <summary>
    /// 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
    /// </summary>
    public int PlanStatus { get; set; } = 0;

    /// <summary>
    /// 转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
    /// </summary>
    public int ConvertedStatus { get; set; } = 0;

    /// <summary>
    /// 计划说明
    /// </summary>
    public string? PlanDescription { get; set; } = string.Empty;

    /// <summary>
    /// 销售预测明细列表（主子表；一行=财年×月计划量 001/002/增减；维度在主表）
    /// （子表：TaktSalesForecastItem）
    /// </summary>
    public List<TaktSalesForecastItemDto>? Items { get; set; }

}

// ========================================
// SalesForecast 查询 DTO
// ========================================

/// <summary>
/// SalesForecast 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSalesForecastQueryDto : TaktPagedQuery
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
    /// 销售预测编码（租户+公司+工厂内与接收版本号组合业务唯一）
    /// </summary>
    public string? SalesForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划编制日期（客户侧业务计划日；与接收日期分离）（范围查询-开始）
    /// </summary>
    public DateTime? PlanDateStart { get; set; }

    /// <summary>
    /// 计划编制日期（客户侧业务计划日；与接收日期分离）（范围查询-结束）
    /// </summary>
    public DateTime? PlanDateEnd { get; set; }

    /// <summary>
    /// 接收日期（我方收到该版客户销售预测的日期）（范围查询-开始）
    /// </summary>
    public DateTime? ReceiveDateStart { get; set; }

    /// <summary>
    /// 接收日期（我方收到该版客户销售预测的日期）（范围查询-结束）
    /// </summary>
    public DateTime? ReceiveDateEnd { get; set; }

    /// <summary>
    /// 接收版本号（同工厂+预测编码下递增；从 1 起）
    /// </summary>
    public int? ReceiveVersionNo { get; set; }

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
    /// 客户编码（选项 TaktCustomers/options；汇总计划时可为空，DictValue=Id）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
    /// </summary>
    public string? CustomerName1 { get; set; } = string.Empty;

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
    /// 已转生产/销售数量（基本单位数量）
    /// </summary>
    public decimal? ConvertedQuantity { get; set; }

    /// <summary>
    /// 已转生产/销售金额
    /// </summary>
    public decimal? ConvertedAmount { get; set; }

    /// <summary>
    /// 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
    /// </summary>
    public int? PlanStatus { get; set; }

    /// <summary>
    /// 转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
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
// 创建SalesForecast DTO
// ========================================

/// <summary>
/// 创建SalesForecast DTO
/// </summary>
public class TaktSalesForecastCreateDto
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
    /// 销售预测编码（租户+公司+工厂内与接收版本号组合业务唯一）
    /// </summary>
    [Required(ErrorMessage = "销售预测编码（租户+公司+工厂内与接收版本号组合业务唯一）不能为空")]
    public string SalesForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划编制日期（客户侧业务计划日；与接收日期分离）
    /// </summary>
    public DateTime PlanDate { get; set; }

    /// <summary>
    /// 接收日期（我方收到该版客户销售预测的日期）
    /// </summary>
    public DateTime ReceiveDate { get; set; }

    /// <summary>
    /// 接收版本号（同工厂+预测编码下递增；从 1 起）
    /// </summary>
    public int ReceiveVersionNo { get; set; } = 0;

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
    /// 客户编码（选项 TaktCustomers/options；汇总计划时可为空，DictValue=Id）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
    /// </summary>
    public string? CustomerName1 { get; set; } = string.Empty;

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
    /// 已转生产/销售数量（基本单位数量）
    /// </summary>
    public decimal ConvertedQuantity { get; set; }

    /// <summary>
    /// 已转生产/销售金额
    /// </summary>
    public decimal ConvertedAmount { get; set; }

    /// <summary>
    /// 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
    /// </summary>
    public int PlanStatus { get; set; } = 0;

    /// <summary>
    /// 转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
    /// </summary>
    public int ConvertedStatus { get; set; } = 0;

    /// <summary>
    /// 计划说明
    /// </summary>
    public string? PlanDescription { get; set; } = string.Empty;

    /// <summary>
    /// 销售预测明细列表（主子表；一行=财年×月计划量 001/002/增减；维度在主表）（子表，级联保存）
    /// </summary>
    public List<TaktSalesForecastItemCreateDto>? Items { get; set; }

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
// 更新SalesForecast DTO
// ========================================

/// <summary>
/// 更新SalesForecast DTO
/// 继承 TaktSalesForecastCreateDto，添加 SalesForecastId 字段
/// </summary>
public class TaktSalesForecastUpdateDto : TaktSalesForecastCreateDto
{
    /// <summary>
    /// SalesForecastID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesForecastId { get; set; }

    /// <summary>
    /// 销售预测明细列表（主子表；一行=财年×月计划量 001/002/增减；维度在主表）（子表，级联保存）
    /// </summary>
    public new List<TaktSalesForecastItemUpdateDto>? Items { get; set; }

}

// ========================================
// SalesForecast 状态 DTO
// ========================================

/// <summary>
/// SalesForecast 状态更新 DTO
/// </summary>
public class TaktSalesForecastStatusDto
{
    /// <summary>
    /// SalesForecastID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesForecastId { get; set; }

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
/// SalesForecast 导入模板行 DTO
/// </summary>
public class TaktSalesForecastTemplateDto
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
    /// 销售预测编码（租户+公司+工厂内与接收版本号组合业务唯一）
    /// </summary>
    public string? SalesForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划编制日期（客户侧业务计划日；与接收日期分离）
    /// </summary>
    public DateTime? PlanDate { get; set; }

    /// <summary>
    /// 接收日期（我方收到该版客户销售预测的日期）
    /// </summary>
    public DateTime? ReceiveDate { get; set; }

    /// <summary>
    /// 接收版本号（同工厂+预测编码下递增；从 1 起）
    /// </summary>
    public int? ReceiveVersionNo { get; set; }

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
    /// 客户编码（选项 TaktCustomers/options；汇总计划时可为空，DictValue=Id）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
    /// </summary>
    public string? CustomerName1 { get; set; } = string.Empty;

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
    /// 已转生产/销售数量（基本单位数量）
    /// </summary>
    public decimal? ConvertedQuantity { get; set; }

    /// <summary>
    /// 已转生产/销售金额
    /// </summary>
    public decimal? ConvertedAmount { get; set; }

    /// <summary>
    /// 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
    /// </summary>
    public int? PlanStatus { get; set; }

    /// <summary>
    /// 转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
    /// </summary>
    public int? ConvertedStatus { get; set; }

    /// <summary>
    /// 计划说明
    /// </summary>
    public string? PlanDescription { get; set; } = string.Empty;

    /// <summary>
    /// 销售预测明细列表（主子表；一行=财年×月计划量 001/002/增减；维度在主表）（子表，级联保存）
    /// </summary>
    public List<TaktSalesForecastItemCreateDto>? Items { get; set; }

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
/// SalesForecast 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSalesForecastImportDto
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
    /// 销售预测编码（租户+公司+工厂内与接收版本号组合业务唯一）
    /// </summary>
    public string? SalesForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划编制日期（客户侧业务计划日；与接收日期分离）
    /// </summary>
    public DateTime? PlanDate { get; set; }

    /// <summary>
    /// 接收日期（我方收到该版客户销售预测的日期）
    /// </summary>
    public DateTime? ReceiveDate { get; set; }

    /// <summary>
    /// 接收版本号（同工厂+预测编码下递增；从 1 起）
    /// </summary>
    public int? ReceiveVersionNo { get; set; }

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
    /// 客户编码（选项 TaktCustomers/options；汇总计划时可为空，DictValue=Id）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
    /// </summary>
    public string? CustomerName1 { get; set; } = string.Empty;

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
    /// 已转生产/销售数量（基本单位数量）
    /// </summary>
    public decimal? ConvertedQuantity { get; set; }

    /// <summary>
    /// 已转生产/销售金额
    /// </summary>
    public decimal? ConvertedAmount { get; set; }

    /// <summary>
    /// 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
    /// </summary>
    public int? PlanStatus { get; set; }

    /// <summary>
    /// 转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
    /// </summary>
    public int? ConvertedStatus { get; set; }

    /// <summary>
    /// 计划说明
    /// </summary>
    public string? PlanDescription { get; set; } = string.Empty;

    /// <summary>
    /// 销售预测明细列表（主子表；一行=财年×月计划量 001/002/增减；维度在主表）（子表，级联保存）
    /// </summary>
    public List<TaktSalesForecastItemCreateDto>? Items { get; set; }

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
/// SalesForecast 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSalesForecastExportDto
{
    /// <summary>
    /// SalesForecastID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesForecastId { get; set; }

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
    /// 销售预测编码（租户+公司+工厂内与接收版本号组合业务唯一）
    /// </summary>
    public string SalesForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划编制日期（客户侧业务计划日；与接收日期分离）
    /// </summary>
    public DateTime PlanDate { get; set; }

    /// <summary>
    /// 接收日期（我方收到该版客户销售预测的日期）
    /// </summary>
    public DateTime ReceiveDate { get; set; }

    /// <summary>
    /// 接收版本号（同工厂+预测编码下递增；从 1 起）
    /// </summary>
    public int ReceiveVersionNo { get; set; } = 0;

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
    /// 客户编码（选项 TaktCustomers/options；汇总计划时可为空，DictValue=Id）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
    /// </summary>
    public string? CustomerName1 { get; set; } = string.Empty;

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
    /// 已转生产/销售数量（基本单位数量）
    /// </summary>
    public decimal ConvertedQuantity { get; set; }

    /// <summary>
    /// 已转生产/销售金额
    /// </summary>
    public decimal ConvertedAmount { get; set; }

    /// <summary>
    /// 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
    /// </summary>
    public int PlanStatus { get; set; } = 0;

    /// <summary>
    /// 转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
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
