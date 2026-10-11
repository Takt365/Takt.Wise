// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Mrp
// 文件名称：TaktProductionPlanDtos.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Auto Generated)
// 功能描述：ProductionPlan 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktProductionPlan 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Mrp;

// ========================================
// ProductionPlan 响应 DTO
// ========================================

/// <summary>
/// Takt生产计划实体（公司级；MRP 运算产出自制件计划，非 MPS 上游）
/// 对应前端 TaktProductionPlanDto
/// 继承 TaktApprovalDtoBase
/// </summary>
public class TaktProductionPlanDto : TaktApprovalDtoBase
{
    /// <summary>
    /// ProductionPlanID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionPlanId { get; set; }

    /// <summary>
    /// 生产计划编码（租户+公司+工厂内业务唯一）
    /// </summary>
    public string ProductionPlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源物料需求计划 ID（Planning 层 MRP 上游，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaterialRequirementsPlanningId { get; set; }

    /// <summary>
    /// 来源物料需求计划 名称（填充字段）
    /// </summary>
    public string? MaterialRequirementsPlanningName { get; set; }

    /// <summary>
    /// 来源 MRP 编码（冗余）
    /// </summary>
    public string? MaterialRequirementsPlanningCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源销售预测ID（Demand 层追溯，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesForecastId { get; set; }

    /// <summary>
    /// 来源销售预测名称（填充字段）
    /// </summary>
    public string? SalesForecastName { get; set; }

    /// <summary>
    /// 来源销售预测编码（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? SalesForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划编制日期
    /// </summary>
    public DateTime PlanDate { get; set; }

    /// <summary>
    /// 计划周期开始日期
    /// </summary>
    public DateTime PlanPeriodStart { get; set; }

    /// <summary>
    /// 计划周期结束日期
    /// </summary>
    public DateTime PlanPeriodEnd { get; set; }

    /// <summary>
    /// 计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）
    /// </summary>
    public string? PlannerGroupCode { get; set; }


    /// <summary>
    /// 计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）
    /// </summary>
    public string? PlannerGroupName { get; set; } = string.Empty;

    /// <summary>
    /// 计划总数量（基本单位数量）
    /// </summary>
    public decimal TotalQuantity { get; set; }

    /// <summary>
    /// 计划总金额
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 已转工单/采购数量（基本单位数量）
    /// </summary>
    public decimal ConvertedQuantity { get; set; }

    /// <summary>
    /// 已转工单/采购金额
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
    /// 生产计划明细列表（主子表关系）
    /// （子表：TaktProductionPlanItem）
    /// </summary>
    public List<TaktProductionPlanItemDto>? Items { get; set; }

}

// ========================================
// ProductionPlan 查询 DTO
// ========================================

/// <summary>
/// ProductionPlan 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktProductionPlanQueryDto : TaktPagedQuery
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
    /// 生产计划编码（租户+公司+工厂内业务唯一）
    /// </summary>
    public string? ProductionPlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源物料需求计划 ID（Planning 层 MRP 上游，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaterialRequirementsPlanningId { get; set; }

    /// <summary>
    /// 来源 MRP 编码（冗余）
    /// </summary>
    public string? MaterialRequirementsPlanningCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源销售预测ID（Demand 层追溯，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesForecastId { get; set; }

    /// <summary>
    /// 来源销售预测编码（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? SalesForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划编制日期（范围查询-开始）
    /// </summary>
    public DateTime? PlanDateStart { get; set; }

    /// <summary>
    /// 计划编制日期（范围查询-结束）
    /// </summary>
    public DateTime? PlanDateEnd { get; set; }

    /// <summary>
    /// 计划周期开始日期（范围查询-开始）
    /// </summary>
    public DateTime? PlanPeriodStartStart { get; set; }

    /// <summary>
    /// 计划周期开始日期（范围查询-结束）
    /// </summary>
    public DateTime? PlanPeriodStartEnd { get; set; }

    /// <summary>
    /// 计划周期结束日期（范围查询-开始）
    /// </summary>
    public DateTime? PlanPeriodEndStart { get; set; }

    /// <summary>
    /// 计划周期结束日期（范围查询-结束）
    /// </summary>
    public DateTime? PlanPeriodEndEnd { get; set; }

    /// <summary>
    /// 计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）
    /// </summary>
    public string? PlannerGroupCode { get; set; }

    /// <summary>
    /// 计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）
    /// </summary>
    public string? PlannerGroupName { get; set; } = string.Empty;

    /// <summary>
    /// 计划总数量（基本单位数量）
    /// </summary>
    public decimal? TotalQuantity { get; set; }

    /// <summary>
    /// 计划总金额
    /// </summary>
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// 已转工单/采购数量（基本单位数量）
    /// </summary>
    public decimal? ConvertedQuantity { get; set; }

    /// <summary>
    /// 已转工单/采购金额
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
// 创建ProductionPlan DTO
// ========================================

/// <summary>
/// 创建ProductionPlan DTO
/// </summary>
public class TaktProductionPlanCreateDto
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
    /// 生产计划编码（租户+公司+工厂内业务唯一）
    /// </summary>
    [Required(ErrorMessage = "生产计划编码（租户+公司+工厂内业务唯一）不能为空")]
    public string ProductionPlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源物料需求计划 ID（Planning 层 MRP 上游，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaterialRequirementsPlanningId { get; set; }

    /// <summary>
    /// 来源 MRP 编码（冗余）
    /// </summary>
    public string? MaterialRequirementsPlanningCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源销售预测ID（Demand 层追溯，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesForecastId { get; set; }

    /// <summary>
    /// 来源销售预测编码（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? SalesForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划编制日期
    /// </summary>
    public DateTime PlanDate { get; set; }

    /// <summary>
    /// 计划周期开始日期
    /// </summary>
    public DateTime PlanPeriodStart { get; set; }

    /// <summary>
    /// 计划周期结束日期
    /// </summary>
    public DateTime PlanPeriodEnd { get; set; }

    /// <summary>
    /// 计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）
    /// </summary>
    public string? PlannerGroupCode { get; set; }

    /// <summary>
    /// 计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）
    /// </summary>
    public string? PlannerGroupName { get; set; } = string.Empty;

    /// <summary>
    /// 计划总数量（基本单位数量）
    /// </summary>
    public decimal TotalQuantity { get; set; }

    /// <summary>
    /// 计划总金额
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 已转工单/采购数量（基本单位数量）
    /// </summary>
    public decimal ConvertedQuantity { get; set; }

    /// <summary>
    /// 已转工单/采购金额
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
    /// 生产计划明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktProductionPlanItemCreateDto>? Items { get; set; }

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
// 更新ProductionPlan DTO
// ========================================

/// <summary>
/// 更新ProductionPlan DTO
/// 继承 TaktProductionPlanCreateDto，添加 ProductionPlanId 字段
/// </summary>
public class TaktProductionPlanUpdateDto : TaktProductionPlanCreateDto
{
    /// <summary>
    /// ProductionPlanID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionPlanId { get; set; }

    /// <summary>
    /// 生产计划明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public new List<TaktProductionPlanItemUpdateDto>? Items { get; set; }

}

// ========================================
// ProductionPlan 状态 DTO
// ========================================

/// <summary>
/// ProductionPlan 状态更新 DTO
/// </summary>
public class TaktProductionPlanStatusDto
{
    /// <summary>
    /// ProductionPlanID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionPlanId { get; set; }

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
/// ProductionPlan 导入模板行 DTO
/// </summary>
public class TaktProductionPlanTemplateDto
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
    /// 生产计划编码（租户+公司+工厂内业务唯一）
    /// </summary>
    public string? ProductionPlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源物料需求计划 ID（Planning 层 MRP 上游，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaterialRequirementsPlanningId { get; set; }

    /// <summary>
    /// 来源 MRP 编码（冗余）
    /// </summary>
    public string? MaterialRequirementsPlanningCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源销售预测ID（Demand 层追溯，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesForecastId { get; set; }

    /// <summary>
    /// 来源销售预测编码（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? SalesForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划编制日期
    /// </summary>
    public DateTime? PlanDate { get; set; }

    /// <summary>
    /// 计划周期开始日期
    /// </summary>
    public DateTime? PlanPeriodStart { get; set; }

    /// <summary>
    /// 计划周期结束日期
    /// </summary>
    public DateTime? PlanPeriodEnd { get; set; }

    /// <summary>
    /// 计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）
    /// </summary>
    public string? PlannerGroupCode { get; set; }

    /// <summary>
    /// 计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）
    /// </summary>
    public string? PlannerGroupName { get; set; } = string.Empty;

    /// <summary>
    /// 计划总数量（基本单位数量）
    /// </summary>
    public decimal? TotalQuantity { get; set; }

    /// <summary>
    /// 计划总金额
    /// </summary>
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// 已转工单/采购数量（基本单位数量）
    /// </summary>
    public decimal? ConvertedQuantity { get; set; }

    /// <summary>
    /// 已转工单/采购金额
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
    /// 生产计划明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktProductionPlanItemCreateDto>? Items { get; set; }

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
/// ProductionPlan 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktProductionPlanImportDto
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
    /// 生产计划编码（租户+公司+工厂内业务唯一）
    /// </summary>
    public string? ProductionPlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源物料需求计划 ID（Planning 层 MRP 上游，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaterialRequirementsPlanningId { get; set; }

    /// <summary>
    /// 来源 MRP 编码（冗余）
    /// </summary>
    public string? MaterialRequirementsPlanningCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源销售预测ID（Demand 层追溯，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesForecastId { get; set; }

    /// <summary>
    /// 来源销售预测编码（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? SalesForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划编制日期
    /// </summary>
    public DateTime? PlanDate { get; set; }

    /// <summary>
    /// 计划周期开始日期
    /// </summary>
    public DateTime? PlanPeriodStart { get; set; }

    /// <summary>
    /// 计划周期结束日期
    /// </summary>
    public DateTime? PlanPeriodEnd { get; set; }

    /// <summary>
    /// 计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）
    /// </summary>
    public string? PlannerGroupCode { get; set; }

    /// <summary>
    /// 计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）
    /// </summary>
    public string? PlannerGroupName { get; set; } = string.Empty;

    /// <summary>
    /// 计划总数量（基本单位数量）
    /// </summary>
    public decimal? TotalQuantity { get; set; }

    /// <summary>
    /// 计划总金额
    /// </summary>
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// 已转工单/采购数量（基本单位数量）
    /// </summary>
    public decimal? ConvertedQuantity { get; set; }

    /// <summary>
    /// 已转工单/采购金额
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
    /// 生产计划明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktProductionPlanItemCreateDto>? Items { get; set; }

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
/// ProductionPlan 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktProductionPlanExportDto
{
    /// <summary>
    /// ProductionPlanID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionPlanId { get; set; }

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
    /// 生产计划编码（租户+公司+工厂内业务唯一）
    /// </summary>
    public string ProductionPlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源物料需求计划 ID（Planning 层 MRP 上游，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaterialRequirementsPlanningId { get; set; }

    /// <summary>
    /// 来源 MRP 编码（冗余）
    /// </summary>
    public string? MaterialRequirementsPlanningCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源销售预测ID（Demand 层追溯，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesForecastId { get; set; }

    /// <summary>
    /// 来源销售预测编码（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? SalesForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划编制日期
    /// </summary>
    public DateTime PlanDate { get; set; }

    /// <summary>
    /// 计划周期开始日期
    /// </summary>
    public DateTime PlanPeriodStart { get; set; }

    /// <summary>
    /// 计划周期结束日期
    /// </summary>
    public DateTime PlanPeriodEnd { get; set; }

    /// <summary>
    /// 计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）
    /// </summary>
    public string? PlannerGroupCode { get; set; }

    /// <summary>
    /// 计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）
    /// </summary>
    public string? PlannerGroupName { get; set; } = string.Empty;

    /// <summary>
    /// 计划总数量（基本单位数量）
    /// </summary>
    public decimal TotalQuantity { get; set; }

    /// <summary>
    /// 计划总金额
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 已转工单/采购数量（基本单位数量）
    /// </summary>
    public decimal ConvertedQuantity { get; set; }

    /// <summary>
    /// 已转工单/采购金额
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
