// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Mrp
// 文件名称：TaktManufacturingPlanningFlowDtos.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：制造计划全链路编排 DTO（MDS→MPS→MRP→APS→工单 / 采购计划→PR）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Takt.Shared.Helpers;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Mrp;

/// <summary>
/// 从 MDS 生成 MPS 请求
/// </summary>
public class TaktMpsRunFromMdsDto
{
    /// <summary>
    /// 来源 MDS 头表 ID
    /// </summary>
    [Required]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MasterDemandScheduleId { get; set; }

    /// <summary>
    /// 时间桶粒度（字典 mps_time_bucket_type；默认继承 MDS 或 1=周）
    /// </summary>
    public int? BucketType { get; set; }

    /// <summary>
    /// 已存在 MPS 头表 ID 时更新行（可选）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterProductionScheduleId { get; set; }
}

/// <summary>
/// MRP 运算选项
/// </summary>
public class TaktMrpRunOptionsDto
{
    /// <summary>
    /// BOM 类型（字典 logistics_manufacturing_bom_type；默认 2=制造）
    /// </summary>
    public int BomType { get; set; } = 2;

    /// <summary>
    /// BOM 最大展开层级（默认 20）
    /// </summary>
    public int MaxBomLevel { get; set; } = 20;

    /// <summary>
    /// 是否将开放采购订单计入计划接收
    /// </summary>
    public bool IncludeOpenPurchaseOrders { get; set; } = true;

    /// <summary>
    /// 是否将已确认计划订单计入计划接收
    /// </summary>
    public bool IncludePlannedOrders { get; set; } = true;
}

/// <summary>
/// MRP 运算请求
/// </summary>
public class TaktMrpRunDto
{
    /// <summary>
    /// MRP 头表 ID
    /// </summary>
    [Required]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialRequirementsPlanningId { get; set; }

    /// <summary>
    /// 运算选项
    /// </summary>
    public TaktMrpRunOptionsDto? Options { get; set; }
}

/// <summary>
/// 计划订单释放到 APS 请求
/// </summary>
public class TaktReleasePlannedOrdersToApsDto
{
    /// <summary>
    /// 计划订单 ID 列表
    /// </summary>
    [Required]
    [MinLength(1)]
    public List<string> PlannedOrderIds { get; set; } = new();
}

/// <summary>
/// APS 排程请求
/// </summary>
public class TaktApsScheduleRunDto
{
    /// <summary>
    /// APS 订单 ID 列表
    /// </summary>
    [Required]
    [MinLength(1)]
    public List<string> ApsOrderIds { get; set; } = new();

    /// <summary>
    /// 已有 APS 排程批次 ID（可选，为空则新建）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsScheduleId { get; set; }

    /// <summary>
    /// 排程名称（新建批次时）
    /// </summary>
    public string? ScheduleName { get; set; }
}

/// <summary>
/// APS 释放生产工单请求
/// </summary>
public class TaktReleaseApsToProductionDto
{
    /// <summary>
    /// APS 订单 ID 列表
    /// </summary>
    [Required]
    [MinLength(1)]
    public List<string> ApsOrderIds { get; set; } = new();
}

/// <summary>
/// 采购计划转采购申请请求
/// </summary>
public class TaktConvertPurchasePlanToPrDto
{
    /// <summary>
    /// 是否创建后自动提交会签
    /// </summary>
    public bool SubmitForCountersign { get; set; }
}

/// <summary>
/// 制造计划编排结果摘要
/// </summary>
public class TaktManufacturingPlanningFlowResultDto
{
    /// <summary>
    /// 主实体 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EntityId { get; set; }

    /// <summary>
    /// 业务编码
    /// </summary>
    public string? EntityCode { get; set; }

    /// <summary>
    /// 处理行数
    /// </summary>
    public int ProcessedCount { get; set; }

    /// <summary>
    /// 产出子实体 ID 列表
    /// </summary>
    public List<string> CreatedEntityIds { get; set; } = new();
}
