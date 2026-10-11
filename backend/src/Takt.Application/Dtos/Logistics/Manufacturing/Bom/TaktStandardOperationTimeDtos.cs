// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Bom
// 文件名称：TaktStandardOperationTimeDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：StandardOperationTime 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktStandardOperationTime 生成，请按需审阅）
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
// StandardOperationTime 响应 DTO
// ========================================

/// <summary>
/// 标准工序时间实体（基于标准工时）
/// 对应前端 TaktStandardOperationTimeDto
/// 继承 TaktApprovalDtoBase
/// </summary>
public class TaktStandardOperationTimeDto : TaktApprovalDtoBase
{
    /// <summary>
    /// StandardOperationTimeID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StandardOperationTimeId { get; set; }


    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode，ExtValue=PlantCode）
    /// </summary>
    public string WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 工序描述
    /// </summary>
    public string? OperationDesc { get; set; } = string.Empty;

    /// <summary>
    /// 标准工时（分钟）
    /// </summary>
    public decimal StandardMinutes { get; set; }

    /// <summary>
    /// 工时单位（字典 logistics_manufacturing_time_unit；默认 MIN）
    /// </summary>
    public string TimeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 标准点数
    /// </summary>
    public int StandardShorts { get; set; } = 0;

    /// <summary>
    /// 点数单位（字典 logistics_manufacturing_points_unit；默认 SHORT）
    /// </summary>
    public string PointsUnit { get; set; } = string.Empty;

    /// <summary>
    /// 点数转分钟汇率（decimal，精度 3 位小数；可选值参见字典 logistics_manufacturing_points_to_minutes_rate：普通=1，AI=0.028，SMT=0.045）
    /// </summary>
    public decimal PointsToMinutesRate { get; set; }

    /// <summary>
    /// 转换后标准工时（分钟）
    /// </summary>
    public decimal ConvertedMinutes { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

}

// ========================================
// StandardOperationTime 查询 DTO
// ========================================

/// <summary>
/// StandardOperationTime 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktStandardOperationTimeQueryDto : TaktPagedQuery
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
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode，ExtValue=PlantCode）
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 工序描述
    /// </summary>
    public string? OperationDesc { get; set; } = string.Empty;

    /// <summary>
    /// 标准工时（分钟）
    /// </summary>
    public decimal? StandardMinutes { get; set; }

    /// <summary>
    /// 工时单位（字典 logistics_manufacturing_time_unit；默认 MIN）
    /// </summary>
    public string? TimeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 标准点数
    /// </summary>
    public int? StandardShorts { get; set; }

    /// <summary>
    /// 点数单位（字典 logistics_manufacturing_points_unit；默认 SHORT）
    /// </summary>
    public string? PointsUnit { get; set; } = string.Empty;

    /// <summary>
    /// 点数转分钟汇率（decimal，精度 3 位小数；可选值参见字典 logistics_manufacturing_points_to_minutes_rate：普通=1，AI=0.028，SMT=0.045）
    /// </summary>
    public decimal? PointsToMinutesRate { get; set; }

    /// <summary>
    /// 转换后标准工时（分钟）
    /// </summary>
    public decimal? ConvertedMinutes { get; set; }

    /// <summary>
    /// 生效日期（范围查询-开始）
    /// </summary>
    public DateTime? EffectiveDateStart { get; set; }

    /// <summary>
    /// 生效日期（范围查询-结束）
    /// </summary>
    public DateTime? EffectiveDateEnd { get; set; }

    /// <summary>
    /// 失效日期（范围查询-开始）
    /// </summary>
    public DateTime? ExpiryDateStart { get; set; }

    /// <summary>
    /// 失效日期（范围查询-结束）
    /// </summary>
    public DateTime? ExpiryDateEnd { get; set; }

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
// 创建StandardOperationTime DTO
// ========================================

/// <summary>
/// 创建StandardOperationTime DTO
/// </summary>
public class TaktStandardOperationTimeCreateDto
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
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode，ExtValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode，ExtValue=PlantCode）不能为空")]
    public string WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 工序描述
    /// </summary>
    public string? OperationDesc { get; set; } = string.Empty;

    /// <summary>
    /// 标准工时（分钟）
    /// </summary>
    public decimal StandardMinutes { get; set; }

    /// <summary>
    /// 工时单位（字典 logistics_manufacturing_time_unit；默认 MIN）
    /// </summary>
    [Required(ErrorMessage = "工时单位（字典 logistics_manufacturing_time_unit；默认 MIN）不能为空")]
    public string TimeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 标准点数
    /// </summary>
    public int StandardShorts { get; set; } = 0;

    /// <summary>
    /// 点数单位（字典 logistics_manufacturing_points_unit；默认 SHORT）
    /// </summary>
    [Required(ErrorMessage = "点数单位（字典 logistics_manufacturing_points_unit；默认 SHORT）不能为空")]
    public string PointsUnit { get; set; } = string.Empty;

    /// <summary>
    /// 点数转分钟汇率（decimal，精度 3 位小数；可选值参见字典 logistics_manufacturing_points_to_minutes_rate：普通=1，AI=0.028，SMT=0.045）
    /// </summary>
    public decimal PointsToMinutesRate { get; set; }

    /// <summary>
    /// 转换后标准工时（分钟）
    /// </summary>
    public decimal ConvertedMinutes { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

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
// 更新StandardOperationTime DTO
// ========================================

/// <summary>
/// 更新StandardOperationTime DTO
/// 继承 TaktStandardOperationTimeCreateDto，添加 StandardOperationTimeId 字段
/// </summary>
public class TaktStandardOperationTimeUpdateDto : TaktStandardOperationTimeCreateDto
{
    /// <summary>
    /// StandardOperationTimeID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StandardOperationTimeId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// StandardOperationTime 导入模板行 DTO
/// </summary>
public class TaktStandardOperationTimeTemplateDto
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
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode，ExtValue=PlantCode）
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 工序描述
    /// </summary>
    public string? OperationDesc { get; set; } = string.Empty;

    /// <summary>
    /// 标准工时（分钟）
    /// </summary>
    public decimal? StandardMinutes { get; set; }

    /// <summary>
    /// 工时单位（字典 logistics_manufacturing_time_unit；默认 MIN）
    /// </summary>
    public string? TimeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 标准点数
    /// </summary>
    public int? StandardShorts { get; set; }

    /// <summary>
    /// 点数单位（字典 logistics_manufacturing_points_unit；默认 SHORT）
    /// </summary>
    public string? PointsUnit { get; set; } = string.Empty;

    /// <summary>
    /// 点数转分钟汇率（decimal，精度 3 位小数；可选值参见字典 logistics_manufacturing_points_to_minutes_rate：普通=1，AI=0.028，SMT=0.045）
    /// </summary>
    public decimal? PointsToMinutesRate { get; set; }

    /// <summary>
    /// 转换后标准工时（分钟）
    /// </summary>
    public decimal? ConvertedMinutes { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime? EffectiveDate { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

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
/// StandardOperationTime 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktStandardOperationTimeImportDto
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
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode，ExtValue=PlantCode）
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 工序描述
    /// </summary>
    public string? OperationDesc { get; set; } = string.Empty;

    /// <summary>
    /// 标准工时（分钟）
    /// </summary>
    public decimal? StandardMinutes { get; set; }

    /// <summary>
    /// 工时单位（字典 logistics_manufacturing_time_unit；默认 MIN）
    /// </summary>
    public string? TimeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 标准点数
    /// </summary>
    public int? StandardShorts { get; set; }

    /// <summary>
    /// 点数单位（字典 logistics_manufacturing_points_unit；默认 SHORT）
    /// </summary>
    public string? PointsUnit { get; set; } = string.Empty;

    /// <summary>
    /// 点数转分钟汇率（decimal，精度 3 位小数；可选值参见字典 logistics_manufacturing_points_to_minutes_rate：普通=1，AI=0.028，SMT=0.045）
    /// </summary>
    public decimal? PointsToMinutesRate { get; set; }

    /// <summary>
    /// 转换后标准工时（分钟）
    /// </summary>
    public decimal? ConvertedMinutes { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime? EffectiveDate { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

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
/// StandardOperationTime 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktStandardOperationTimeExportDto
{
    /// <summary>
    /// StandardOperationTimeID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StandardOperationTimeId { get; set; }

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode，ExtValue=PlantCode）
    /// </summary>
    public string WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 工序描述
    /// </summary>
    public string? OperationDesc { get; set; } = string.Empty;

    /// <summary>
    /// 标准工时（分钟）
    /// </summary>
    public decimal StandardMinutes { get; set; }

    /// <summary>
    /// 工时单位（字典 logistics_manufacturing_time_unit；默认 MIN）
    /// </summary>
    public string TimeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 标准点数
    /// </summary>
    public int StandardShorts { get; set; } = 0;

    /// <summary>
    /// 点数单位（字典 logistics_manufacturing_points_unit；默认 SHORT）
    /// </summary>
    public string PointsUnit { get; set; } = string.Empty;

    /// <summary>
    /// 点数转分钟汇率（decimal，精度 3 位小数；可选值参见字典 logistics_manufacturing_points_to_minutes_rate：普通=1，AI=0.028，SMT=0.045）
    /// </summary>
    public decimal PointsToMinutesRate { get; set; }

    /// <summary>
    /// 转换后标准工时（分钟）
    /// </summary>
    public decimal ConvertedMinutes { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

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
