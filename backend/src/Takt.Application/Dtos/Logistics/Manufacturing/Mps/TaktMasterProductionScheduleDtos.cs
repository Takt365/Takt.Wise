// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Mps
// 文件名称：TaktMasterProductionScheduleDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：MasterProductionSchedule 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktMasterProductionSchedule 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Mps;

// ========================================
// MasterProductionSchedule 响应 DTO
// ========================================

/// <summary>
/// 主生产计划 MPS 头表（公司级；MDS 下推，成品级何时做多少、粗产能校验）
/// 对应前端 TaktMasterProductionScheduleDto
/// 继承 TaktApprovalDtoBase
/// </summary>
public class TaktMasterProductionScheduleDto : TaktApprovalDtoBase
{
    /// <summary>
    /// MasterProductionScheduleID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MasterProductionScheduleId { get; set; }

    /// <summary>
    /// MPS 编码
    /// </summary>
    public string MpsCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源 MDS 头表 ID（Demand 层上游，关联 TaktMasterDemandSchedule.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterDemandScheduleId { get; set; }

    /// <summary>
    /// 来源 MDS 头表 名称（填充字段）
    /// </summary>
    public string? MasterDemandScheduleName { get; set; }

    /// <summary>
    /// 来源 MDS 编码（冗余）
    /// </summary>
    public string? MdsCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划周期开始
    /// </summary>
    public DateTime PlanPeriodStart { get; set; }

    /// <summary>
    /// 计划周期结束
    /// </summary>
    public DateTime PlanPeriodEnd { get; set; }

    /// <summary>
    /// 时间桶粒度（字典 mps_time_bucket_type；0=日，1=周，2=月）
    /// </summary>
    public int BucketType { get; set; } = 0;

    /// <summary>
    /// 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
    /// </summary>
    public int ScheduleStatus { get; set; } = 0;

    /// <summary>
    /// MPS 明细行
    /// （子表：TaktMasterProductionScheduleLine）
    /// </summary>
    public List<TaktMasterProductionScheduleLineDto>? Lines { get; set; }

}

// ========================================
// MasterProductionSchedule 查询 DTO
// ========================================

/// <summary>
/// MasterProductionSchedule 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktMasterProductionScheduleQueryDto : TaktPagedQuery
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
    /// MPS 编码
    /// </summary>
    public string? MpsCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源 MDS 头表 ID（Demand 层上游，关联 TaktMasterDemandSchedule.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterDemandScheduleId { get; set; }

    /// <summary>
    /// 来源 MDS 编码（冗余）
    /// </summary>
    public string? MdsCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划周期开始（范围查询-开始）
    /// </summary>
    public DateTime? PlanPeriodStartStart { get; set; }

    /// <summary>
    /// 计划周期开始（范围查询-结束）
    /// </summary>
    public DateTime? PlanPeriodStartEnd { get; set; }

    /// <summary>
    /// 计划周期结束（范围查询-开始）
    /// </summary>
    public DateTime? PlanPeriodEndStart { get; set; }

    /// <summary>
    /// 计划周期结束（范围查询-结束）
    /// </summary>
    public DateTime? PlanPeriodEndEnd { get; set; }

    /// <summary>
    /// 时间桶粒度（字典 mps_time_bucket_type；0=日，1=周，2=月）
    /// </summary>
    public int? BucketType { get; set; }

    /// <summary>
    /// 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
    /// </summary>
    public int? ScheduleStatus { get; set; }

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
// 创建MasterProductionSchedule DTO
// ========================================

/// <summary>
/// 创建MasterProductionSchedule DTO
/// </summary>
public class TaktMasterProductionScheduleCreateDto
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
    /// MPS 编码
    /// </summary>
    [Required(ErrorMessage = "MPS 编码不能为空")]
    public string MpsCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源 MDS 头表 ID（Demand 层上游，关联 TaktMasterDemandSchedule.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterDemandScheduleId { get; set; }

    /// <summary>
    /// 来源 MDS 编码（冗余）
    /// </summary>
    public string? MdsCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划周期开始
    /// </summary>
    public DateTime PlanPeriodStart { get; set; }

    /// <summary>
    /// 计划周期结束
    /// </summary>
    public DateTime PlanPeriodEnd { get; set; }

    /// <summary>
    /// 时间桶粒度（字典 mps_time_bucket_type；0=日，1=周，2=月）
    /// </summary>
    public int BucketType { get; set; } = 0;

    /// <summary>
    /// 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
    /// </summary>
    public int ScheduleStatus { get; set; } = 0;

    /// <summary>
    /// MPS 明细行（子表，级联保存）
    /// </summary>
    public List<TaktMasterProductionScheduleLineCreateDto>? Lines { get; set; }

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
// 更新MasterProductionSchedule DTO
// ========================================

/// <summary>
/// 更新MasterProductionSchedule DTO
/// 继承 TaktMasterProductionScheduleCreateDto，添加 MasterProductionScheduleId 字段
/// </summary>
public class TaktMasterProductionScheduleUpdateDto : TaktMasterProductionScheduleCreateDto
{
    /// <summary>
    /// MasterProductionScheduleID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MasterProductionScheduleId { get; set; }

    /// <summary>
    /// MPS 明细行（子表，级联保存）
    /// </summary>
    public new List<TaktMasterProductionScheduleLineUpdateDto>? Lines { get; set; }

}

// ========================================
// MasterProductionSchedule 状态 DTO
// ========================================

/// <summary>
/// MasterProductionSchedule 状态更新 DTO
/// </summary>
public class TaktMasterProductionScheduleStatusDto
{
    /// <summary>
    /// MasterProductionScheduleID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MasterProductionScheduleId { get; set; }

    /// <summary>
    /// 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
    /// </summary>
    [Required(ErrorMessage = "计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）不能为空")]
    public int ScheduleStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// MasterProductionSchedule 导入模板行 DTO
/// </summary>
public class TaktMasterProductionScheduleTemplateDto
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
    /// MPS 编码
    /// </summary>
    public string? MpsCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源 MDS 头表 ID（Demand 层上游，关联 TaktMasterDemandSchedule.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterDemandScheduleId { get; set; }

    /// <summary>
    /// 来源 MDS 编码（冗余）
    /// </summary>
    public string? MdsCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划周期开始
    /// </summary>
    public DateTime? PlanPeriodStart { get; set; }

    /// <summary>
    /// 计划周期结束
    /// </summary>
    public DateTime? PlanPeriodEnd { get; set; }

    /// <summary>
    /// 时间桶粒度（字典 mps_time_bucket_type；0=日，1=周，2=月）
    /// </summary>
    public int? BucketType { get; set; }

    /// <summary>
    /// 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
    /// </summary>
    public int? ScheduleStatus { get; set; }

    /// <summary>
    /// MPS 明细行（子表，级联保存）
    /// </summary>
    public List<TaktMasterProductionScheduleLineCreateDto>? Lines { get; set; }

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
/// MasterProductionSchedule 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktMasterProductionScheduleImportDto
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
    /// MPS 编码
    /// </summary>
    public string? MpsCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源 MDS 头表 ID（Demand 层上游，关联 TaktMasterDemandSchedule.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterDemandScheduleId { get; set; }

    /// <summary>
    /// 来源 MDS 编码（冗余）
    /// </summary>
    public string? MdsCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划周期开始
    /// </summary>
    public DateTime? PlanPeriodStart { get; set; }

    /// <summary>
    /// 计划周期结束
    /// </summary>
    public DateTime? PlanPeriodEnd { get; set; }

    /// <summary>
    /// 时间桶粒度（字典 mps_time_bucket_type；0=日，1=周，2=月）
    /// </summary>
    public int? BucketType { get; set; }

    /// <summary>
    /// 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
    /// </summary>
    public int? ScheduleStatus { get; set; }

    /// <summary>
    /// MPS 明细行（子表，级联保存）
    /// </summary>
    public List<TaktMasterProductionScheduleLineCreateDto>? Lines { get; set; }

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
/// MasterProductionSchedule 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktMasterProductionScheduleExportDto
{
    /// <summary>
    /// MasterProductionScheduleID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MasterProductionScheduleId { get; set; }

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
    /// MPS 编码
    /// </summary>
    public string MpsCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源 MDS 头表 ID（Demand 层上游，关联 TaktMasterDemandSchedule.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterDemandScheduleId { get; set; }

    /// <summary>
    /// 来源 MDS 编码（冗余）
    /// </summary>
    public string? MdsCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划周期开始
    /// </summary>
    public DateTime PlanPeriodStart { get; set; }

    /// <summary>
    /// 计划周期结束
    /// </summary>
    public DateTime PlanPeriodEnd { get; set; }

    /// <summary>
    /// 时间桶粒度（字典 mps_time_bucket_type；0=日，1=周，2=月）
    /// </summary>
    public int BucketType { get; set; } = 0;

    /// <summary>
    /// 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
    /// </summary>
    public int ScheduleStatus { get; set; } = 0;

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
