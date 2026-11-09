// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcKanbanLineDtos.cs
// 创建时间：2026-10-29
// 创建人：Takt365(Cursor AI)
// 功能描述：设变看板部门格与采购/受检/部管/物料页签行 DTO（与 TaktEcKanbanService 投影字段对齐）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Newtonsoft.Json;
using SqlSugar;

namespace Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变看板部门执行行（实施标记 + 日期/批次；生管行另带本行机种/成品）
/// </summary>
public class TaktEcKanbanDeptLineDto
{
    /// <summary>
    /// 是否实施（字典 sys_yes_no；0=否 1=是；制技为是否更新 SOP；-1=该生管行下无对应执行行）
    /// </summary>
    public int IsImplemented { get; set; }

    /// <summary>
    /// 命中执行行数
    /// </summary>
    public int LineCount { get; set; }

    /// <summary>
    /// 已实施行数
    /// </summary>
    public int ImplementedCount { get; set; }

    /// <summary>
    /// 未实施行数
    /// </summary>
    public int NotImplementedCount { get; set; }

    /// <summary>
    /// 机种（生管/采购等本行 EcModelCodeGroup 原样展示）
    /// </summary>
    public string EcModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 成品/根物料（生管/采购等本行 EcRootMaterialCodeGroup 原样展示）
    /// </summary>
    public string EcRootMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 阶段日期
    /// </summary>
    public DateTime? StageDate { get; set; }

    /// <summary>
    /// 日期展示（yyyyMMdd）
    /// </summary>
    public string DateDisplayText { get; set; } = string.Empty;

    /// <summary>
    /// 批次/单号
    /// </summary>
    public string ProdBatch { get; set; } = string.Empty;
}

/// <summary>
/// 设变看板·采购页签行（TaktEcPur 业务列原样）
/// </summary>
public class TaktEcKanbanPurLineDto
{
    /// <summary>采购执行行主键</summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcPurId { get; set; }
    /// <summary>行号</summary>
    public int LineNumber { get; set; }
    /// <summary>停产状态</summary>
    public string DiscontinuedStatus { get; set; } = string.Empty;
    /// <summary>设变单号</summary>
    public string EcCode { get; set; } = string.Empty;
    /// <summary>机种编码组</summary>
    public string EcModelCodeGroup { get; set; } = string.Empty;
    /// <summary>根物料编码组</summary>
    public string EcRootMaterialCodeGroup { get; set; } = string.Empty;
    /// <summary>新物料编码组</summary>
    public string EcNewMaterialCodeGroup { get; set; } = string.Empty;
    /// <summary>新采购类型</summary>
    public string EcNewPurchaseType { get; set; } = string.Empty;
    /// <summary>是否实施</summary>
    public int IsImplemented { get; set; }
    /// <summary>执行内容</summary>
    public string? ExecContent { get; set; }
    /// <summary>旧品处理</summary>
    public string? EcOldPartBalance { get; set; }
    /// <summary>进货日期</summary>
    public DateTime? InboundDate { get; set; }
    /// <summary>部门编码</summary>
    public string DeptCode { get; set; } = string.Empty;
    /// <summary>部门名称</summary>
    public string DeptName { get; set; } = string.Empty;
    /// <summary>实施范围</summary>
    public int EcImplScope { get; set; }
    /// <summary>控制范围</summary>
    public int EcControlScope { get; set; }
    /// <summary>执行去重键</summary>
    public string EcGrainKey { get; set; } = string.Empty;
    /// <summary>是否作废</summary>
    public int IsObsolete { get; set; }
}

/// <summary>
/// 设变看板·受检页签行（TaktEcIqc 业务列原样）
/// </summary>
public class TaktEcKanbanIqcLineDto
{
    /// <summary>受检执行行主键</summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcIqcId { get; set; }
    /// <summary>行号</summary>
    public int LineNumber { get; set; }
    /// <summary>停产状态</summary>
    public string DiscontinuedStatus { get; set; } = string.Empty;
    /// <summary>设变单号</summary>
    public string EcCode { get; set; } = string.Empty;
    /// <summary>机种编码组</summary>
    public string EcModelCodeGroup { get; set; } = string.Empty;
    /// <summary>根物料编码组</summary>
    public string EcRootMaterialCodeGroup { get; set; } = string.Empty;
    /// <summary>新物料编码组</summary>
    public string EcNewMaterialCodeGroup { get; set; } = string.Empty;
    /// <summary>新品检验</summary>
    public int EcNewRequiresInspection { get; set; }
    /// <summary>是否实施</summary>
    public int IsImplemented { get; set; }
    /// <summary>执行内容</summary>
    public string? ExecContent { get; set; }
    /// <summary>检验日期</summary>
    public DateTime? InspectionDate { get; set; }
    /// <summary>部门编码</summary>
    public string DeptCode { get; set; } = string.Empty;
    /// <summary>部门名称</summary>
    public string DeptName { get; set; } = string.Empty;
    /// <summary>实施范围</summary>
    public int EcImplScope { get; set; }
    /// <summary>控制范围</summary>
    public int EcControlScope { get; set; }
    /// <summary>执行去重键</summary>
    public string EcGrainKey { get; set; } = string.Empty;
    /// <summary>是否作废</summary>
    public int IsObsolete { get; set; }
}

/// <summary>
/// 设变看板·部管页签行（TaktEcMc 业务列原样）
/// </summary>
public class TaktEcKanbanMcLineDto
{
    /// <summary>部管执行行主键</summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcMcId { get; set; }
    /// <summary>行号</summary>
    public int LineNumber { get; set; }
    /// <summary>设变单号</summary>
    public string EcCode { get; set; } = string.Empty;
    /// <summary>停产状态</summary>
    public string DiscontinuedStatus { get; set; } = string.Empty;
    /// <summary>机种编码组</summary>
    public string EcModelCodeGroup { get; set; } = string.Empty;
    /// <summary>根物料编码组</summary>
    public string EcRootMaterialCodeGroup { get; set; } = string.Empty;
    /// <summary>新物料编码组</summary>
    public string EcNewMaterialCodeGroup { get; set; } = string.Empty;
    /// <summary>新采购类型</summary>
    public string EcNewPurchaseType { get; set; } = string.Empty;
    /// <summary>新品仓库</summary>
    public string EcNewWarehouse { get; set; } = string.Empty;
    /// <summary>是否实施</summary>
    public int IsImplemented { get; set; }
    /// <summary>预定日期</summary>
    public DateTime? ScheduledDate { get; set; }
    /// <summary>预定批次</summary>
    public string? ScheduledBatch { get; set; }
    /// <summary>执行内容</summary>
    public string? ExecContent { get; set; }
    /// <summary>出库日期</summary>
    public DateTime? OutboundDate { get; set; }
    /// <summary>出库批次</summary>
    public string? OutboundBatch { get; set; }
    /// <summary>部门编码</summary>
    public string DeptCode { get; set; } = string.Empty;
    /// <summary>部门名称</summary>
    public string DeptName { get; set; } = string.Empty;
    /// <summary>实施范围</summary>
    public int EcImplScope { get; set; }
    /// <summary>控制范围</summary>
    public int EcControlScope { get; set; }
    /// <summary>执行去重键</summary>
    public string EcGrainKey { get; set; } = string.Empty;
    /// <summary>是否作废</summary>
    public int IsObsolete { get; set; }
}

/// <summary>
/// 设变看板·物料页签行（TaktEcMtl 业务列原样）
/// </summary>
public class TaktEcKanbanMtlLineDto
{
    /// <summary>物料执行行主键</summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcMtlId { get; set; }
    /// <summary>行号</summary>
    public int LineNumber { get; set; }
    /// <summary>设变单号</summary>
    public string EcCode { get; set; } = string.Empty;
    /// <summary>停产状态</summary>
    public string DiscontinuedStatus { get; set; } = string.Empty;
    /// <summary>机种编码组</summary>
    public string EcModelCodeGroup { get; set; } = string.Empty;
    /// <summary>根物料编码组</summary>
    public string EcRootMaterialCodeGroup { get; set; } = string.Empty;
    /// <summary>上阶物料编码</summary>
    public string EcParentMaterialCode { get; set; } = string.Empty;
    /// <summary>上阶物料描述</summary>
    public string EcParentMaterialDescription { get; set; } = string.Empty;
    /// <summary>新物料编码组</summary>
    public string EcNewMaterialCodeGroup { get; set; } = string.Empty;
    /// <summary>新采购类型</summary>
    public string EcNewPurchaseType { get; set; } = string.Empty;
    /// <summary>新品仓库</summary>
    public string EcNewWarehouse { get; set; } = string.Empty;
    /// <summary>是否实施</summary>
    public int IsImplemented { get; set; }
    /// <summary>预定日期</summary>
    public DateTime? ScheduledDate { get; set; }
    /// <summary>预定批次</summary>
    public string? ScheduledBatch { get; set; }
    /// <summary>执行内容</summary>
    public string? ExecContent { get; set; }
    /// <summary>出库日期</summary>
    public DateTime? OutboundDate { get; set; }
    /// <summary>出库批次</summary>
    public string? OutboundBatch { get; set; }
    /// <summary>部门编码</summary>
    public string DeptCode { get; set; } = string.Empty;
    /// <summary>部门名称</summary>
    public string DeptName { get; set; } = string.Empty;
    /// <summary>实施范围</summary>
    public int EcImplScope { get; set; }
    /// <summary>控制范围</summary>
    public int EcControlScope { get; set; }
    /// <summary>执行去重键</summary>
    public string EcGrainKey { get; set; } = string.Empty;
    /// <summary>是否作废</summary>
    public int IsObsolete { get; set; }
}
