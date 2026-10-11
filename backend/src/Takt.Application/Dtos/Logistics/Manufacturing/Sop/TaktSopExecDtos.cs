// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Sop
// 文件名称：TaktSopExecDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：SopExec 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSopExec 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Sop;

// ========================================
// SopExec 响应 DTO
// ========================================

/// <summary>
/// SOP 工位执行追溯实体
/// 对应前端 TaktSopExecDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSopExecDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SopExecID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopExecId { get; set; }

    /// <summary>
    /// 生产工单 ID（选项 TaktProductionOrders/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProductionOrderId { get; set; }

    /// <summary>
    /// 生产工单 名称（填充字段）
    /// </summary>
    public string? ProductionOrderName { get; set; }

    /// <summary>
    /// MES 工单号（冗余，便于追溯查询）
    /// </summary>
    public string WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品序列号 SN
    /// </summary>
    public string? SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 产品/机种物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序 ID（选项 TaktRoutingItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoutingItemId { get; set; }

    /// <summary>
    /// 工序 名称（填充字段）
    /// </summary>
    public string? RoutingItemName { get; set; }

    /// <summary>
    /// 工艺段类型（字典 logistics_manufacturing_process_segment_type；1=SMT，2=自插，3=手插，4=修正，5=总装）
    /// </summary>
    public int ProcessSegmentType { get; set; } = 0;

    /// <summary>
    /// 工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long WorkstationId { get; set; }

    /// <summary>
    /// 工位 名称（填充字段）
    /// </summary>
    public string? WorkstationName { get; set; }

    /// <summary>
    /// 员工 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 员工 名称（填充字段）
    /// </summary>
    public string? EmployeeName { get; set; }

    /// <summary>
    /// SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopId { get; set; }

    /// <summary>
    /// SOP 主档 名称（填充字段）
    /// </summary>
    public string? SopName { get; set; }

    /// <summary>
    /// SOP 版本 ID（选项 TaktSopRevisions/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RevisionId { get; set; }

    /// <summary>
    /// SOP 版本 名称（填充字段）
    /// </summary>
    public string? RevisionName { get; set; }

    /// <summary>
    /// 版本号快照
    /// </summary>
    public string Revision { get; set; } = string.Empty;

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartedAt { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndedAt { get; set; }

    /// <summary>
    /// 自检结果（字典 logistics_manufacturing_sop_check_result；1=合格，2=不合格，3=不适用/跳过）
    /// </summary>
    public int? SelfCheckResult { get; set; }

    /// <summary>
    /// 执行状态（字典 logistics_manufacturing_sop_exec_status；1=进行中，2=完成，3=中断）
    /// </summary>
    public int ExecStatus { get; set; } = 0;

    /// <summary>
    /// 当前工步 ID（选项 TaktSopSteps/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CurrentStepId { get; set; }

    /// <summary>
    /// 当前工步 名称（填充字段）
    /// </summary>
    public string? CurrentStepName { get; set; }

    /// <summary>
    /// 工位
    /// （主表：TaktSopWorkstation）
    /// </summary>
    public TaktSopWorkstationDto? Workstation { get; set; }

    /// <summary>
    /// 工步执行明细
    /// （子表：TaktSopExecStep）
    /// </summary>
    public List<TaktSopExecStepDto>? Steps { get; set; }

    /// <summary>
    /// 扫码记录
    /// （子表：TaktSopExecScan）
    /// </summary>
    public List<TaktSopExecScanDto>? Scans { get; set; }

    /// <summary>
    /// 作业参数
    /// （子表：TaktSopArgument）
    /// </summary>
    public List<TaktSopArgumentDto>? Arguments { get; set; }

}

// ========================================
// SopExec 查询 DTO
// ========================================

/// <summary>
/// SopExec 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSopExecQueryDto : TaktPagedQuery
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
    /// 生产工单 ID（选项 TaktProductionOrders/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProductionOrderId { get; set; }

    /// <summary>
    /// MES 工单号（冗余，便于追溯查询）
    /// </summary>
    public string? WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品序列号 SN
    /// </summary>
    public string? SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 产品/机种物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序 ID（选项 TaktRoutingItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RoutingItemId { get; set; }

    /// <summary>
    /// 工艺段类型（字典 logistics_manufacturing_process_segment_type；1=SMT，2=自插，3=手插，4=修正，5=总装）
    /// </summary>
    public int? ProcessSegmentType { get; set; }

    /// <summary>
    /// 工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? WorkstationId { get; set; }

    /// <summary>
    /// 员工 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SopId { get; set; }

    /// <summary>
    /// SOP 版本 ID（选项 TaktSopRevisions/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RevisionId { get; set; }

    /// <summary>
    /// 版本号快照
    /// </summary>
    public string? Revision { get; set; } = string.Empty;

    /// <summary>
    /// 开始时间（范围查询-开始）
    /// </summary>
    public DateTime? StartedAtStart { get; set; }

    /// <summary>
    /// 开始时间（范围查询-结束）
    /// </summary>
    public DateTime? StartedAtEnd { get; set; }

    /// <summary>
    /// 结束时间（范围查询-开始）
    /// </summary>
    public DateTime? EndedAtStart { get; set; }

    /// <summary>
    /// 结束时间（范围查询-结束）
    /// </summary>
    public DateTime? EndedAtEnd { get; set; }

    /// <summary>
    /// 自检结果（字典 logistics_manufacturing_sop_check_result；1=合格，2=不合格，3=不适用/跳过）
    /// </summary>
    public int? SelfCheckResult { get; set; }

    /// <summary>
    /// 执行状态（字典 logistics_manufacturing_sop_exec_status；1=进行中，2=完成，3=中断）
    /// </summary>
    public int? ExecStatus { get; set; }

    /// <summary>
    /// 当前工步 ID（选项 TaktSopSteps/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CurrentStepId { get; set; }

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
// 创建SopExec DTO
// ========================================

/// <summary>
/// 创建SopExec DTO
/// </summary>
public class TaktSopExecCreateDto
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
    /// 生产工单 ID（选项 TaktProductionOrders/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProductionOrderId { get; set; }

    /// <summary>
    /// MES 工单号（冗余，便于追溯查询）
    /// </summary>
    [Required(ErrorMessage = "MES 工单号（冗余，便于追溯查询）不能为空")]
    public string WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品序列号 SN
    /// </summary>
    public string? SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 产品/机种物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "产品/机种物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序 ID（选项 TaktRoutingItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoutingItemId { get; set; }

    /// <summary>
    /// 工艺段类型（字典 logistics_manufacturing_process_segment_type；1=SMT，2=自插，3=手插，4=修正，5=总装）
    /// </summary>
    public int ProcessSegmentType { get; set; } = 0;

    /// <summary>
    /// 工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long WorkstationId { get; set; }

    /// <summary>
    /// 员工 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopId { get; set; }

    /// <summary>
    /// SOP 版本 ID（选项 TaktSopRevisions/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RevisionId { get; set; }

    /// <summary>
    /// 版本号快照
    /// </summary>
    [Required(ErrorMessage = "版本号快照不能为空")]
    public string Revision { get; set; } = string.Empty;

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartedAt { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndedAt { get; set; }

    /// <summary>
    /// 自检结果（字典 logistics_manufacturing_sop_check_result；1=合格，2=不合格，3=不适用/跳过）
    /// </summary>
    public int? SelfCheckResult { get; set; }

    /// <summary>
    /// 执行状态（字典 logistics_manufacturing_sop_exec_status；1=进行中，2=完成，3=中断）
    /// </summary>
    public int ExecStatus { get; set; } = 0;

    /// <summary>
    /// 当前工步 ID（选项 TaktSopSteps/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CurrentStepId { get; set; }

    /// <summary>
    /// 工步执行明细（子表，级联保存）
    /// </summary>
    public List<TaktSopExecStepCreateDto>? Steps { get; set; }

    /// <summary>
    /// 扫码记录（子表，级联保存）
    /// </summary>
    public List<TaktSopExecScanCreateDto>? Scans { get; set; }

    /// <summary>
    /// 作业参数（子表，级联保存）
    /// </summary>
    public List<TaktSopArgumentCreateDto>? Arguments { get; set; }

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
// 更新SopExec DTO
// ========================================

/// <summary>
/// 更新SopExec DTO
/// 继承 TaktSopExecCreateDto，添加 SopExecId 字段
/// </summary>
public class TaktSopExecUpdateDto : TaktSopExecCreateDto
{
    /// <summary>
    /// SopExecID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopExecId { get; set; }

    /// <summary>
    /// 工步执行明细（子表，级联保存）
    /// </summary>
    public new List<TaktSopExecStepUpdateDto>? Steps { get; set; }

    /// <summary>
    /// 扫码记录（子表，级联保存）
    /// </summary>
    public new List<TaktSopExecScanUpdateDto>? Scans { get; set; }

    /// <summary>
    /// 作业参数（子表，级联保存）
    /// </summary>
    public new List<TaktSopArgumentUpdateDto>? Arguments { get; set; }

}

// ========================================
// SopExec 状态 DTO
// ========================================

/// <summary>
/// SopExec 状态更新 DTO
/// </summary>
public class TaktSopExecStatusDto
{
    /// <summary>
    /// SopExecID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopExecId { get; set; }

    /// <summary>
    /// 执行状态（字典 logistics_manufacturing_sop_exec_status；1=进行中，2=完成，3=中断）
    /// </summary>
    [Required(ErrorMessage = "执行状态（字典 logistics_manufacturing_sop_exec_status；1=进行中，2=完成，3=中断）不能为空")]
    public int ExecStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SopExec 导入模板行 DTO
/// </summary>
public class TaktSopExecTemplateDto
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
    /// 生产工单 ID（选项 TaktProductionOrders/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProductionOrderId { get; set; }

    /// <summary>
    /// MES 工单号（冗余，便于追溯查询）
    /// </summary>
    public string? WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品序列号 SN
    /// </summary>
    public string? SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 产品/机种物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序 ID（选项 TaktRoutingItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RoutingItemId { get; set; }

    /// <summary>
    /// 工艺段类型（字典 logistics_manufacturing_process_segment_type；1=SMT，2=自插，3=手插，4=修正，5=总装）
    /// </summary>
    public int? ProcessSegmentType { get; set; }

    /// <summary>
    /// 工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? WorkstationId { get; set; }

    /// <summary>
    /// 员工 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SopId { get; set; }

    /// <summary>
    /// SOP 版本 ID（选项 TaktSopRevisions/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RevisionId { get; set; }

    /// <summary>
    /// 版本号快照
    /// </summary>
    public string? Revision { get; set; } = string.Empty;

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? StartedAt { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndedAt { get; set; }

    /// <summary>
    /// 自检结果（字典 logistics_manufacturing_sop_check_result；1=合格，2=不合格，3=不适用/跳过）
    /// </summary>
    public int? SelfCheckResult { get; set; }

    /// <summary>
    /// 执行状态（字典 logistics_manufacturing_sop_exec_status；1=进行中，2=完成，3=中断）
    /// </summary>
    public int? ExecStatus { get; set; }

    /// <summary>
    /// 当前工步 ID（选项 TaktSopSteps/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CurrentStepId { get; set; }

    /// <summary>
    /// 工步执行明细（子表，级联保存）
    /// </summary>
    public List<TaktSopExecStepCreateDto>? Steps { get; set; }

    /// <summary>
    /// 扫码记录（子表，级联保存）
    /// </summary>
    public List<TaktSopExecScanCreateDto>? Scans { get; set; }

    /// <summary>
    /// 作业参数（子表，级联保存）
    /// </summary>
    public List<TaktSopArgumentCreateDto>? Arguments { get; set; }

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
/// SopExec 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSopExecImportDto
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
    /// 生产工单 ID（选项 TaktProductionOrders/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProductionOrderId { get; set; }

    /// <summary>
    /// MES 工单号（冗余，便于追溯查询）
    /// </summary>
    public string? WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品序列号 SN
    /// </summary>
    public string? SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 产品/机种物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序 ID（选项 TaktRoutingItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RoutingItemId { get; set; }

    /// <summary>
    /// 工艺段类型（字典 logistics_manufacturing_process_segment_type；1=SMT，2=自插，3=手插，4=修正，5=总装）
    /// </summary>
    public int? ProcessSegmentType { get; set; }

    /// <summary>
    /// 工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? WorkstationId { get; set; }

    /// <summary>
    /// 员工 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SopId { get; set; }

    /// <summary>
    /// SOP 版本 ID（选项 TaktSopRevisions/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RevisionId { get; set; }

    /// <summary>
    /// 版本号快照
    /// </summary>
    public string? Revision { get; set; } = string.Empty;

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? StartedAt { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndedAt { get; set; }

    /// <summary>
    /// 自检结果（字典 logistics_manufacturing_sop_check_result；1=合格，2=不合格，3=不适用/跳过）
    /// </summary>
    public int? SelfCheckResult { get; set; }

    /// <summary>
    /// 执行状态（字典 logistics_manufacturing_sop_exec_status；1=进行中，2=完成，3=中断）
    /// </summary>
    public int? ExecStatus { get; set; }

    /// <summary>
    /// 当前工步 ID（选项 TaktSopSteps/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CurrentStepId { get; set; }

    /// <summary>
    /// 工步执行明细（子表，级联保存）
    /// </summary>
    public List<TaktSopExecStepCreateDto>? Steps { get; set; }

    /// <summary>
    /// 扫码记录（子表，级联保存）
    /// </summary>
    public List<TaktSopExecScanCreateDto>? Scans { get; set; }

    /// <summary>
    /// 作业参数（子表，级联保存）
    /// </summary>
    public List<TaktSopArgumentCreateDto>? Arguments { get; set; }

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
/// SopExec 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSopExecExportDto
{
    /// <summary>
    /// SopExecID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopExecId { get; set; }

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
    /// 生产工单 ID（选项 TaktProductionOrders/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProductionOrderId { get; set; }

    /// <summary>
    /// MES 工单号（冗余，便于追溯查询）
    /// </summary>
    public string WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品序列号 SN
    /// </summary>
    public string? SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 产品/机种物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序 ID（选项 TaktRoutingItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoutingItemId { get; set; }

    /// <summary>
    /// 工艺段类型（字典 logistics_manufacturing_process_segment_type；1=SMT，2=自插，3=手插，4=修正，5=总装）
    /// </summary>
    public int ProcessSegmentType { get; set; } = 0;

    /// <summary>
    /// 工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long WorkstationId { get; set; }

    /// <summary>
    /// 员工 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopId { get; set; }

    /// <summary>
    /// SOP 版本 ID（选项 TaktSopRevisions/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RevisionId { get; set; }

    /// <summary>
    /// 版本号快照
    /// </summary>
    public string Revision { get; set; } = string.Empty;

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartedAt { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndedAt { get; set; }

    /// <summary>
    /// 自检结果（字典 logistics_manufacturing_sop_check_result；1=合格，2=不合格，3=不适用/跳过）
    /// </summary>
    public int? SelfCheckResult { get; set; }

    /// <summary>
    /// 执行状态（字典 logistics_manufacturing_sop_exec_status；1=进行中，2=完成，3=中断）
    /// </summary>
    public int ExecStatus { get; set; } = 0;

    /// <summary>
    /// 当前工步 ID（选项 TaktSopSteps/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CurrentStepId { get; set; }

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
