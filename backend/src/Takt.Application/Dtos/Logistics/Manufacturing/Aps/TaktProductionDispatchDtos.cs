// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Aps
// 文件名称：TaktProductionDispatchDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：ProductionDispatch 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktProductionDispatch 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Aps;

// ========================================
// ProductionDispatch 响应 DTO
// ========================================

/// <summary>
/// 生产派工单（Prod_Order → Dispatch → MES 报工）
/// 对应前端 TaktProductionDispatchDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktProductionDispatchDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ProductionDispatchID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionDispatchId { get; set; }


    /// <summary>
    /// 派工单编码
    /// </summary>
    public string DispatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产工单 ID（选项 TaktProductionOrders/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionOrderId { get; set; }

    /// <summary>
    /// 生产工单 名称（填充字段）
    /// </summary>
    public string? ProductionOrderName { get; set; }

    /// <summary>
    /// 工单号（关联 TaktProductionOrder.ProdOrderCode，冗余）
    /// </summary>
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// APS 工序排程 ID（选项 TaktApsOperations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsOperationId { get; set; }

    /// <summary>
    /// APS 工序排程 名称（填充字段）
    /// </summary>
    public string? ApsOperationName { get; set; }

    /// <summary>
    /// 工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    public string? WorkCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序编码
    /// </summary>
    public string? ProcessCode { get; set; } = string.Empty;

    /// <summary>
    /// 派工数量
    /// </summary>
    public decimal DispatchQuantity { get; set; }

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime? PlannedEndTime { get; set; }

    /// <summary>
    /// 派工状态（字典 production_dispatch_status；0=待执行，1=执行中，2=已完成，3=已取消）
    /// </summary>
    public int DispatchStatus { get; set; } = 0;

}

// ========================================
// ProductionDispatch 查询 DTO
// ========================================

/// <summary>
/// ProductionDispatch 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktProductionDispatchQueryDto : TaktPagedQuery
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
    /// 派工单编码
    /// </summary>
    public string? DispatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产工单 ID（选项 TaktProductionOrders/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProductionOrderId { get; set; }

    /// <summary>
    /// 工单号（关联 TaktProductionOrder.ProdOrderCode，冗余）
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// APS 工序排程 ID（选项 TaktApsOperations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsOperationId { get; set; }

    /// <summary>
    /// 工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    public string? WorkCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序编码
    /// </summary>
    public string? ProcessCode { get; set; } = string.Empty;

    /// <summary>
    /// 派工数量
    /// </summary>
    public decimal? DispatchQuantity { get; set; }

    /// <summary>
    /// 计划开始时间（范围查询-开始）
    /// </summary>
    public DateTime? PlannedStartTimeStart { get; set; }

    /// <summary>
    /// 计划开始时间（范围查询-结束）
    /// </summary>
    public DateTime? PlannedStartTimeEnd { get; set; }

    /// <summary>
    /// 计划结束时间（范围查询-开始）
    /// </summary>
    public DateTime? PlannedEndTimeStart { get; set; }

    /// <summary>
    /// 计划结束时间（范围查询-结束）
    /// </summary>
    public DateTime? PlannedEndTimeEnd { get; set; }

    /// <summary>
    /// 派工状态（字典 production_dispatch_status；0=待执行，1=执行中，2=已完成，3=已取消）
    /// </summary>
    public int? DispatchStatus { get; set; }

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
// 创建ProductionDispatch DTO
// ========================================

/// <summary>
/// 创建ProductionDispatch DTO
/// </summary>
public class TaktProductionDispatchCreateDto
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
    /// 派工单编码
    /// </summary>
    [Required(ErrorMessage = "派工单编码不能为空")]
    public string DispatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产工单 ID（选项 TaktProductionOrders/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionOrderId { get; set; }

    /// <summary>
    /// 工单号（关联 TaktProductionOrder.ProdOrderCode，冗余）
    /// </summary>
    [Required(ErrorMessage = "工单号（关联 TaktProductionOrder.ProdOrderCode，冗余）不能为空")]
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// APS 工序排程 ID（选项 TaktApsOperations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsOperationId { get; set; }

    /// <summary>
    /// 工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    public string? WorkCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序编码
    /// </summary>
    public string? ProcessCode { get; set; } = string.Empty;

    /// <summary>
    /// 派工数量
    /// </summary>
    public decimal DispatchQuantity { get; set; }

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime? PlannedEndTime { get; set; }

    /// <summary>
    /// 派工状态（字典 production_dispatch_status；0=待执行，1=执行中，2=已完成，3=已取消）
    /// </summary>
    public int DispatchStatus { get; set; } = 0;

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
// 更新ProductionDispatch DTO
// ========================================

/// <summary>
/// 更新ProductionDispatch DTO
/// 继承 TaktProductionDispatchCreateDto，添加 ProductionDispatchId 字段
/// </summary>
public class TaktProductionDispatchUpdateDto : TaktProductionDispatchCreateDto
{
    /// <summary>
    /// ProductionDispatchID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionDispatchId { get; set; }

}

// ========================================
// ProductionDispatch 状态 DTO
// ========================================

/// <summary>
/// ProductionDispatch 状态更新 DTO
/// </summary>
public class TaktProductionDispatchStatusDto
{
    /// <summary>
    /// ProductionDispatchID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionDispatchId { get; set; }

    /// <summary>
    /// 派工状态（字典 production_dispatch_status；0=待执行，1=执行中，2=已完成，3=已取消）
    /// </summary>
    [Required(ErrorMessage = "派工状态（字典 production_dispatch_status；0=待执行，1=执行中，2=已完成，3=已取消）不能为空")]
    public int DispatchStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// ProductionDispatch 导入模板行 DTO
/// </summary>
public class TaktProductionDispatchTemplateDto
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
    /// 派工单编码
    /// </summary>
    public string? DispatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产工单 ID（选项 TaktProductionOrders/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProductionOrderId { get; set; }

    /// <summary>
    /// 工单号（关联 TaktProductionOrder.ProdOrderCode，冗余）
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// APS 工序排程 ID（选项 TaktApsOperations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsOperationId { get; set; }

    /// <summary>
    /// 工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    public string? WorkCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序编码
    /// </summary>
    public string? ProcessCode { get; set; } = string.Empty;

    /// <summary>
    /// 派工数量
    /// </summary>
    public decimal? DispatchQuantity { get; set; }

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime? PlannedEndTime { get; set; }

    /// <summary>
    /// 派工状态（字典 production_dispatch_status；0=待执行，1=执行中，2=已完成，3=已取消）
    /// </summary>
    public int? DispatchStatus { get; set; }

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
/// ProductionDispatch 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktProductionDispatchImportDto
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
    /// 派工单编码
    /// </summary>
    public string? DispatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产工单 ID（选项 TaktProductionOrders/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProductionOrderId { get; set; }

    /// <summary>
    /// 工单号（关联 TaktProductionOrder.ProdOrderCode，冗余）
    /// </summary>
    public string? ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// APS 工序排程 ID（选项 TaktApsOperations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsOperationId { get; set; }

    /// <summary>
    /// 工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    public string? WorkCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序编码
    /// </summary>
    public string? ProcessCode { get; set; } = string.Empty;

    /// <summary>
    /// 派工数量
    /// </summary>
    public decimal? DispatchQuantity { get; set; }

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime? PlannedEndTime { get; set; }

    /// <summary>
    /// 派工状态（字典 production_dispatch_status；0=待执行，1=执行中，2=已完成，3=已取消）
    /// </summary>
    public int? DispatchStatus { get; set; }

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
/// ProductionDispatch 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktProductionDispatchExportDto
{
    /// <summary>
    /// ProductionDispatchID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionDispatchId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 派工单编码
    /// </summary>
    public string DispatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产工单 ID（选项 TaktProductionOrders/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionOrderId { get; set; }

    /// <summary>
    /// 工单号（关联 TaktProductionOrder.ProdOrderCode，冗余）
    /// </summary>
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// APS 工序排程 ID（选项 TaktApsOperations/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsOperationId { get; set; }

    /// <summary>
    /// 工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    public string? WorkCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序编码
    /// </summary>
    public string? ProcessCode { get; set; } = string.Empty;

    /// <summary>
    /// 派工数量
    /// </summary>
    public decimal DispatchQuantity { get; set; }

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime? PlannedEndTime { get; set; }

    /// <summary>
    /// 派工状态（字典 production_dispatch_status；0=待执行，1=执行中，2=已完成，3=已取消）
    /// </summary>
    public int DispatchStatus { get; set; } = 0;

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
