// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Mds
// 文件名称：TaktMasterDemandScheduleDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：MasterDemandSchedule 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktMasterDemandSchedule 生成，请按需审阅）
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
// MasterDemandSchedule 响应 DTO
// ========================================

/// <summary>
/// 主需求计划 MDS 头表（公司级；承接销售订单与预测，下推 MPS）
/// 对应前端 TaktMasterDemandScheduleDto
/// 继承 TaktApprovalDtoBase
/// </summary>
public class TaktMasterDemandScheduleDto : TaktApprovalDtoBase
{
    /// <summary>
    /// MasterDemandScheduleID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MasterDemandScheduleId { get; set; }

    /// <summary>
    /// MDS 编码（租户+公司+工厂内业务唯一）
    /// </summary>
    public string MdsCode { get; set; } = string.Empty;

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
    /// MDS 明细行（按物料与时间桶）
    /// （子表：TaktMasterDemandScheduleLine）
    /// </summary>
    public List<TaktMasterDemandScheduleLineDto>? Lines { get; set; }

}

// ========================================
// MasterDemandSchedule 查询 DTO
// ========================================

/// <summary>
/// MasterDemandSchedule 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktMasterDemandScheduleQueryDto : TaktPagedQuery
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
    /// MDS 编码（租户+公司+工厂内业务唯一）
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
// 创建MasterDemandSchedule DTO
// ========================================

/// <summary>
/// 创建MasterDemandSchedule DTO
/// </summary>
public class TaktMasterDemandScheduleCreateDto
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
    /// MDS 编码（租户+公司+工厂内业务唯一）
    /// </summary>
    [Required(ErrorMessage = "MDS 编码（租户+公司+工厂内业务唯一）不能为空")]
    public string MdsCode { get; set; } = string.Empty;

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
    /// MDS 明细行（按物料与时间桶）（子表，级联保存）
    /// </summary>
    public List<TaktMasterDemandScheduleLineCreateDto>? Lines { get; set; }

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
// 更新MasterDemandSchedule DTO
// ========================================

/// <summary>
/// 更新MasterDemandSchedule DTO
/// 继承 TaktMasterDemandScheduleCreateDto，添加 MasterDemandScheduleId 字段
/// </summary>
public class TaktMasterDemandScheduleUpdateDto : TaktMasterDemandScheduleCreateDto
{
    /// <summary>
    /// MasterDemandScheduleID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MasterDemandScheduleId { get; set; }

    /// <summary>
    /// MDS 明细行（按物料与时间桶）（子表，级联保存）
    /// </summary>
    public new List<TaktMasterDemandScheduleLineUpdateDto>? Lines { get; set; }

}

// ========================================
// MasterDemandSchedule 状态 DTO
// ========================================

/// <summary>
/// MasterDemandSchedule 状态更新 DTO
/// </summary>
public class TaktMasterDemandScheduleStatusDto
{
    /// <summary>
    /// MasterDemandScheduleID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MasterDemandScheduleId { get; set; }

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
/// MasterDemandSchedule 导入模板行 DTO
/// </summary>
public class TaktMasterDemandScheduleTemplateDto
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
    /// MDS 编码（租户+公司+工厂内业务唯一）
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
    /// MDS 明细行（按物料与时间桶）（子表，级联保存）
    /// </summary>
    public List<TaktMasterDemandScheduleLineCreateDto>? Lines { get; set; }

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
/// MasterDemandSchedule 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktMasterDemandScheduleImportDto
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
    /// MDS 编码（租户+公司+工厂内业务唯一）
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
    /// MDS 明细行（按物料与时间桶）（子表，级联保存）
    /// </summary>
    public List<TaktMasterDemandScheduleLineCreateDto>? Lines { get; set; }

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
/// MasterDemandSchedule 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktMasterDemandScheduleExportDto
{
    /// <summary>
    /// MasterDemandScheduleID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MasterDemandScheduleId { get; set; }

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
    /// MDS 编码（租户+公司+工厂内业务唯一）
    /// </summary>
    public string MdsCode { get; set; } = string.Empty;

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
