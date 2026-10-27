// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcViewDtos.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变看板/投入批次/实施管控/部门视图/旧品管制 专用 DTO
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;

// ========================================
// 部门视图（技术/采购/生管等）共用 DTO
// ========================================

/// <summary>
/// 设变部门视图行（明细 + 部门记录合并展示）
/// </summary>
public class TaktEcDeptViewDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EcDeptViewID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcDeptViewId { get; set; }

    /// <summary>
    /// 部门记录 ID；未创建时为空
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcExecId { get; set; }
    /// <summary>
    /// 设变明细 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcDetailId { get; set; }
    /// <summary>
    /// 设变主表 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcTeId { get; set; }
    /// <summary>
    /// 设变单号
    /// </summary>
    public string EcNo { get; set; } = string.Empty;
    /// <summary>
    /// 明细行号
    /// </summary>
    public int LineNumber { get; set; }
    /// <summary>
    /// 机种（Ec_model）
    /// </summary>
    public string EcModelCode { get; set; } = string.Empty;
    /// <summary>
    /// 旧料号
    /// </summary>
    public string? EcOldMaterialCode { get; set; }
    /// <summary>
    /// 新料号
    /// </summary>
    public string? EcNewMaterialCode { get; set; }
    /// <summary>
    /// 旧料号描述
    /// </summary>
    public string? EcOldMaterialDescription { get; set; }
    /// <summary>
    /// 新料号描述
    /// </summary>
    public string? EcNewMaterialDescription { get; set; }
    /// <summary>
    /// 部门编码
    /// </summary>
    public string DeptCode { get; set; } = string.Empty;
    /// <summary>
    /// 是否实施（0=否 1=是）
    /// </summary>
    public int IsImplemented { get; set; }
    /// <summary>
    /// 内容
    /// </summary>
    public string? Content { get; set; }
    /// <summary>
    /// 停产状态（字典 logistics_materials_material_discontinued_status；来自明细，用于判断 EOL）
    /// </summary>
    public string DiscontinuedStatus { get; set; } = string.Empty;
    /// <summary>
    /// 录入日期
    /// </summary>
    public DateTime? EntryDate { get; set; }
    /// <summary>
    /// 担当（EcLeader）
    /// </summary>
    public string? EcLeader { get; set; }
    /// <summary>
    /// 预定日期
    /// </summary>
    public DateTime? ScheduledDate { get; set; }
    /// <summary>
    /// 预定批次
    /// </summary>
    public string? ScheduledBatch { get; set; }
    /// <summary>
    /// Po残（可空；非空须为采购单10位-序号4位:数量，如 4500123456-0010:18.5）
    /// </summary>
    public string? PoRemainder { get; set; }
    /// <summary>
    /// 结余数量
    /// </summary>
    public decimal? BalanceQuantity { get; set; }
    /// <summary>
    /// 旧品处理（生管/采购执行行存字典 logistics_manufacturing_ec_old_part_disposition；视图字段名兼容）
    /// </summary>
    public string? OldProductHandling { get; set; }
    /// <summary>
    /// 采购订单发行日期
    /// </summary>
    public DateTime? PurchaseOrderIssueDate { get; set; }
    /// <summary>
    /// 供应商
    /// </summary>
    public string? Supplier { get; set; }
    /// <summary>
    /// 采购订单号码
    /// </summary>
    public string? PurchaseOrderNo { get; set; }
    /// <summary>
    /// 受检单号
    /// </summary>
    public string? IqcOrderNo { get; set; }
    /// <summary>
    /// 检验日期
    /// </summary>
    public DateTime? InspectionDate { get; set; }
    /// <summary>
    /// 出库批次
    /// </summary>
    public string? OutboundBatch { get; set; }
    /// <summary>
    /// 出库日期
    /// </summary>
    public DateTime? OutboundDate { get; set; }
    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime? ProductionDate { get; set; }
    /// <summary>
    /// 生产批次
    /// </summary>
    public string? ProductionBatch { get; set; }
    /// <summary>
    /// 出库单号
    /// </summary>
    public string? OutboundOrderNo { get; set; }
    /// <summary>
    /// 生产班组
    /// </summary>
    public string? ProductionTeam { get; set; }
    /// <summary>
    /// 实施日期
    /// </summary>
    public DateTime? ImplementationDate { get; set; }
    /// <summary>
    /// 实施批次
    /// </summary>
    public string? ImplementationBatch { get; set; }
    /// <summary>
    /// 检验批次
    /// </summary>
    public string? InspectionBatch { get; set; }
    /// <summary>
    /// 抽样号码
    /// </summary>
    public string? SamplingNo { get; set; }
    /// <summary>
    /// SOP日期
    /// </summary>
    public DateTime? SopDate { get; set; }
    /// <summary>
    /// 是否更新 SOP（0=否 1=是）
    /// </summary>
    public int IsSopUpdated { get; set; }


    /// <summary>
    /// PurchaseOrderCode
    /// </summary>
    public string? PurchaseOrderCode { get; set; }

    /// <summary>
    /// IqcOrderCode
    /// </summary>
    public string? IqcOrderCode { get; set; }

    /// <summary>
    /// OutboundOrderCode
    /// </summary>
    public string? OutboundOrderCode { get; set; }

    /// <summary>
    /// SamplingCode
    /// </summary>
    public string? SamplingCode { get; set; }
}

/// <summary>
/// 设变部门视图查询 DTO
/// </summary>
public class TaktEcDeptViewQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 设变单号
    /// </summary>
    public string? EcNo { get; set; }
    /// <summary>
    /// 机种（Ec_model）
    /// </summary>
    public string? EcModelCode { get; set; }
    /// <summary>
    /// 是否实施
    /// </summary>
    public int? IsImplemented { get; set; }
    /// <summary>
    /// 旧料号
    /// </summary>
    public string? EcOldMaterialCode { get; set; }
    /// <summary>
    /// 新料号
    /// </summary>
    public string? EcNewMaterialCode { get; set; }

    /// <summary>
    /// EcCode
    /// </summary>
    public string? EcCode { get; set; }
}

/// <summary>
/// 设变部门视图更新 DTO
/// </summary>
public class TaktEcDeptViewUpdateDto
{
    /// <summary>
    /// 设变明细 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcDetailId { get; set; }
    /// <summary>
    /// 是否实施
    /// </summary>
    public int IsImplemented { get; set; }
    /// <summary>
    /// 内容
    /// </summary>
    public string? Content { get; set; }
    /// <summary>
    /// 录入日期
    /// </summary>
    public DateTime? EntryDate { get; set; }
    /// <summary>
    /// 担当（EcLeader）
    /// </summary>
    public string? EcLeader { get; set; }
    /// <summary>
    /// 预定日期
    /// </summary>
    public DateTime? ScheduledDate { get; set; }
    /// <summary>
    /// 预定批次
    /// </summary>
    public string? ScheduledBatch { get; set; }
    /// <summary>
    /// Po残（可空；非空须为采购单10位-序号4位:数量，如 4500123456-0010:18.5）
    /// </summary>
    public string? PoRemainder { get; set; }
    /// <summary>
    /// 结余数量
    /// </summary>
    public decimal? BalanceQuantity { get; set; }
    /// <summary>
    /// 旧品处理（生管/采购执行行存字典 logistics_manufacturing_ec_old_part_disposition；视图字段名兼容）
    /// </summary>
    public string? OldProductHandling { get; set; }
    /// <summary>
    /// 采购订单发行日期
    /// </summary>
    public DateTime? PurchaseOrderIssueDate { get; set; }
    /// <summary>
    /// 供应商
    /// </summary>
    public string? Supplier { get; set; }
    /// <summary>
    /// 采购订单号码
    /// </summary>
    public string? PurchaseOrderNo { get; set; }
    /// <summary>
    /// 受检单号
    /// </summary>
    public string? IqcOrderNo { get; set; }
    /// <summary>
    /// 检验日期
    /// </summary>
    public DateTime? InspectionDate { get; set; }
    /// <summary>
    /// 出库批次
    /// </summary>
    public string? OutboundBatch { get; set; }
    /// <summary>
    /// 出库日期
    /// </summary>
    public DateTime? OutboundDate { get; set; }
    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime? ProductionDate { get; set; }
    /// <summary>
    /// 生产批次
    /// </summary>
    public string? ProductionBatch { get; set; }
    /// <summary>
    /// 出库单号
    /// </summary>
    public string? OutboundOrderNo { get; set; }
    /// <summary>
    /// 生产班组
    /// </summary>
    public string? ProductionTeam { get; set; }
    /// <summary>
    /// 实施日期
    /// </summary>
    public DateTime? ImplementationDate { get; set; }
    /// <summary>
    /// 实施批次
    /// </summary>
    public string? ImplementationBatch { get; set; }
    /// <summary>
    /// 检验批次
    /// </summary>
    public string? InspectionBatch { get; set; }
    /// <summary>
    /// 抽样号码
    /// </summary>
    public string? SamplingNo { get; set; }
    /// <summary>
    /// SOP日期
    /// </summary>
    public DateTime? SopDate { get; set; }
    /// <summary>
    /// 是否更新 SOP
    /// </summary>
    public int IsSopUpdated { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }


    /// <summary>
    /// PurchaseOrderCode
    /// </summary>
    public string? PurchaseOrderCode { get; set; }

    /// <summary>
    /// IqcOrderCode
    /// </summary>
    public string? IqcOrderCode { get; set; }

    /// <summary>
    /// OutboundOrderCode
    /// </summary>
    public string? OutboundOrderCode { get; set; }

    /// <summary>
    /// SamplingCode
    /// </summary>
    public string? SamplingCode { get; set; }
}

/// <summary>
/// 设变部门视图导入模板 DTO
/// </summary>
public class TaktEcDeptViewTemplateDto
{
    /// <summary>
    /// 设变明细 ID（与 EcNo+LineNumber 二选一）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcDetailId { get; set; }
    /// <summary>
    /// 设变单号
    /// </summary>
    public string? EcNo { get; set; }
    /// <summary>
    /// 设变明细行号
    /// </summary>
    public int? LineNumber { get; set; }
    /// <summary>
    /// 是否实施
    /// </summary>
    public int? IsImplemented { get; set; }
    /// <summary>
    /// 内容
    /// </summary>
    public string? Content { get; set; }
    /// <summary>
    /// 录入日期
    /// </summary>
    public DateTime? EntryDate { get; set; }
    /// <summary>
    /// 担当（EcLeader）
    /// </summary>
    public string? EcLeader { get; set; }
    /// <summary>
    /// 预定日期
    /// </summary>
    public DateTime? ScheduledDate { get; set; }
    /// <summary>
    /// 预定批次
    /// </summary>
    public string? ScheduledBatch { get; set; }
    /// <summary>
    /// Po残（可空；非空须为采购单10位-序号4位:数量，如 4500123456-0010:18.5）
    /// </summary>
    public string? PoRemainder { get; set; }
    /// <summary>
    /// 结余数量
    /// </summary>
    public decimal? BalanceQuantity { get; set; }
    /// <summary>
    /// 旧品处理（生管/采购执行行存字典 logistics_manufacturing_ec_old_part_disposition；视图字段名兼容）
    /// </summary>
    public string? OldProductHandling { get; set; }
    /// <summary>
    /// 采购订单发行日期
    /// </summary>
    public DateTime? PurchaseOrderIssueDate { get; set; }
    /// <summary>
    /// 供应商
    /// </summary>
    public string? Supplier { get; set; }
    /// <summary>
    /// 采购订单号码
    /// </summary>
    public string? PurchaseOrderNo { get; set; }
    /// <summary>
    /// 受检单号
    /// </summary>
    public string? IqcOrderNo { get; set; }
    /// <summary>
    /// 检验日期
    /// </summary>
    public DateTime? InspectionDate { get; set; }
    /// <summary>
    /// 出库批次
    /// </summary>
    public string? OutboundBatch { get; set; }
    /// <summary>
    /// 出库日期
    /// </summary>
    public DateTime? OutboundDate { get; set; }
    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime? ProductionDate { get; set; }
    /// <summary>
    /// 生产批次
    /// </summary>
    public string? ProductionBatch { get; set; }
    /// <summary>
    /// 出库单号
    /// </summary>
    public string? OutboundOrderNo { get; set; }
    /// <summary>
    /// 生产班组
    /// </summary>
    public string? ProductionTeam { get; set; }
    /// <summary>
    /// 实施日期
    /// </summary>
    public DateTime? ImplementationDate { get; set; }
    /// <summary>
    /// 实施批次
    /// </summary>
    public string? ImplementationBatch { get; set; }
    /// <summary>
    /// 检验批次
    /// </summary>
    public string? InspectionBatch { get; set; }
    /// <summary>
    /// 抽样号码
    /// </summary>
    public string? SamplingNo { get; set; }
    /// <summary>
    /// SOP日期
    /// </summary>
    public DateTime? SopDate { get; set; }
    /// <summary>
    /// 是否更新 SOP
    /// </summary>
    public int? IsSopUpdated { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
}

/// <summary>
/// 设变部门视图导入 DTO
/// </summary>
public class TaktEcDeptViewImportDto : TaktEcDeptViewTemplateDto
{



    /// <summary>
    /// EcCode
    /// </summary>
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// PurchaseOrderCode
    /// </summary>
    public string? PurchaseOrderCode { get; set; }

    /// <summary>
    /// IqcOrderCode
    /// </summary>
    public string? IqcOrderCode { get; set; }

    /// <summary>
    /// OutboundOrderCode
    /// </summary>
    public string? OutboundOrderCode { get; set; }

    /// <summary>
    /// SamplingCode
    /// </summary>
    public string? SamplingCode { get; set; }
}

// ========================================
// 设变看板
// ========================================

/// <summary>
/// 设变看板转置行（一行一个设变号；各部门列是该部门每一条执行行的实施标记）
/// </summary>
public class TaktEcKanbanDto : TaktCompanyDtoBase
{
    /// <summary>
    /// 设变主表 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcTeId { get; set; }
    /// <summary>
    /// 发行日期
    /// </summary>
    public DateTime EcIssueDate { get; set; }
    /// <summary>
    /// 技术担当（设变组编码）
    /// </summary>
    public string EcLeader { get; set; } = string.Empty;
    /// <summary>
    /// 设变单号
    /// </summary>
    public string EcCode { get; set; } = string.Empty;
    /// <summary>
    /// 机种编码
    /// </summary>
    public string EcModelCode { get; set; } = string.Empty;
    /// <summary>
    /// 成品（根物料编码）
    /// </summary>
    public string EcRootMaterialCode { get; set; } = string.Empty;
    /// <summary>
    /// 技术课（技术主表录入日期）
    /// </summary>
    public DateTime EcEntryDate { get; set; }
    /// <summary>
    /// 采购课每一行的实施标记（字典 sys_yes_no；0=否 1=是）。行数即列表长度
    /// </summary>
    public List<int> PurImplemented { get; set; } = [];
    /// <summary>
    /// 生管课每一行的实施标记（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public List<int> PcImplemented { get; set; } = [];
    /// <summary>
    /// 受检课每一行的实施标记（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public List<int> IqcImplemented { get; set; } = [];
    /// <summary>
    /// 部管课每一行的实施标记（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public List<int> McImplemented { get; set; } = [];
    /// <summary>
    /// 制二课每一行的实施标记（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public List<int> Mfg2Implemented { get; set; } = [];
    /// <summary>
    /// 制一课每一行的实施标记（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public List<int> Mfg1Implemented { get; set; } = [];
    /// <summary>
    /// 品管课每一行的实施标记（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public List<int> QcImplemented { get; set; } = [];
    /// <summary>
    /// 物料每一行的实施标记（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public List<int> MtlImplemented { get; set; } = [];
    /// <summary>
    /// 制技每一行的更新 SOP 标记（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public List<int> MeImplemented { get; set; } = [];
}

/// <summary>
/// 设变看板导出行
/// </summary>
public class TaktEcKanbanExportDto
{
    /// <summary>
    /// 发行日期
    /// </summary>
    public string EcIssueDate { get; set; } = string.Empty;
    /// <summary>
    /// 技术担当
    /// </summary>
    public string EcLeader { get; set; } = string.Empty;
    /// <summary>
    /// 设变号码
    /// </summary>
    public string EcCode { get; set; } = string.Empty;
    /// <summary>
    /// 机种
    /// </summary>
    public string EcModelCode { get; set; } = string.Empty;
    /// <summary>
    /// 成品
    /// </summary>
    public string EcRootMaterialCode { get; set; } = string.Empty;
    /// <summary>
    /// 技术课
    /// </summary>
    public string EcEntryDate { get; set; } = string.Empty;
    /// <summary>
    /// 生管课
    /// </summary>
    public string PcImplemented { get; set; } = string.Empty;
    /// <summary>
    /// 采购课
    /// </summary>
    public string PurImplemented { get; set; } = string.Empty;
    /// <summary>
    /// 受检课
    /// </summary>
    public string IqcImplemented { get; set; } = string.Empty;
    /// <summary>
    /// 部管课
    /// </summary>
    public string McImplemented { get; set; } = string.Empty;
    /// <summary>
    /// 物料
    /// </summary>
    public string MtlImplemented { get; set; } = string.Empty;
    /// <summary>
    /// 制二课
    /// </summary>
    public string Mfg2Implemented { get; set; } = string.Empty;
    /// <summary>
    /// 制一课
    /// </summary>
    public string Mfg1Implemented { get; set; } = string.Empty;
    /// <summary>
    /// 品管课
    /// </summary>
    public string QcImplemented { get; set; } = string.Empty;
    /// <summary>
    /// 制技
    /// </summary>
    public string MeImplemented { get; set; } = string.Empty;
}

/// <summary>
/// 设变看板查询 DTO
/// </summary>
public class TaktEcKanbanQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 设变单号
    /// </summary>
    public string? EcNo { get; set; }
    /// <summary>
    /// 变更状态
    /// </summary>
    public int? ChangeStatus { get; set; }
    /// <summary>
    /// 设变状态
    /// </summary>
    public int? EcStatus { get; set; }
    /// <summary>
    /// 当前待实施部门编码
    /// </summary>
    public string? CurrentDeptCode { get; set; }
    /// <summary>
    /// 实施路径状态（0 未开始 1 实施中 2 正式完成 3 全部完成）。数据看板仍用此字段；页面筛选用 IsImplemented
    /// </summary>
    public int? ImplementationStatus { get; set; }

    /// <summary>
    /// 实施标记（字典 sys_yes_no；0=否 1=是）。选定部门时只比较该部门，未选部门时任一部门命中即可
    /// </summary>
    public int? IsImplemented { get; set; }

    /// <summary>
    /// 录入日期起（含当天）
    /// </summary>
    public DateTime? EcEntryDateStart { get; set; }

    /// <summary>
    /// 录入日期止（含当天）
    /// </summary>
    public DateTime? EcEntryDateEnd { get; set; }

    /// <summary>
    /// EcCode
    /// </summary>
    public string? EcCode { get; set; }

    /// <summary>
    /// CultureCode
    /// </summary>
    public string? CultureCode { get; set; }
}

/// <summary>
/// 设变看板统计页签：实施状态、部门执行、按月录入
/// </summary>
public class TaktEcKanbanBoardStatDto
{
    /// <summary>
    /// 设变主表总数（全仕向/部管，当前查询条件）
    /// </summary>
    public int EcTotal { get; set; }

    /// <summary>
    /// 未开始
    /// </summary>
    public int NotStarted { get; set; }

    /// <summary>
    /// 实施中
    /// </summary>
    public int InProgress { get; set; }

    /// <summary>
    /// 正式完成
    /// </summary>
    public int OfficiallyCompleted { get; set; }

    /// <summary>
    /// 全部完成
    /// </summary>
    public int FullyCompleted { get; set; }

    /// <summary>
    /// 各部门执行行实施/未实施
    /// </summary>
    public List<TaktEcKanbanDeptStatDto> Depts { get; set; } = new();

    /// <summary>
    /// 录入期间内按月设变单数
    /// </summary>
    public List<TaktEcKanbanMonthStatDto> Months { get; set; } = new();
}

/// <summary>
/// 看板统计·部门执行行
/// </summary>
public class TaktEcKanbanDeptStatDto
{
    /// <summary>
    /// 部门编码
    /// </summary>
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 已实施行数
    /// </summary>
    public int Implemented { get; set; }

    /// <summary>
    /// 未实施行数
    /// </summary>
    public int NotImplemented { get; set; }
}

/// <summary>
/// 看板统计·按月录入
/// </summary>
public class TaktEcKanbanMonthStatDto
{
    /// <summary>
    /// 年月（yyyy-MM）
    /// </summary>
    public string YearMonth { get; set; } = string.Empty;

    /// <summary>
    /// 设变单数
    /// </summary>
    public int EcCount { get; set; }
}

// ========================================
// 投入批次
// ========================================

/// <summary>
/// 投入批次视图行
/// </summary>
public class TaktEcBatchDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EcBatchID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcBatchId { get; set; }

    /// <summary>
    /// 设变明细 ID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcDetailId { get; set; }
    /// <summary>
    /// 设变单号
    /// </summary>
    public string EcNo { get; set; } = string.Empty;
    /// <summary>
    /// 行号
    /// </summary>
    public int LineNumber { get; set; }
    /// <summary>
    /// 机种（Ec_model）
    /// </summary>
    public string EcModelCode { get; set; } = string.Empty;
    /// <summary>
    /// 新料号
    /// </summary>
    public string? EcNewMaterialCode { get; set; }
    /// <summary>
    /// 预定批次（生管）
    /// </summary>
    public string? ScheduledBatch { get; set; }
    /// <summary>
    /// 生产批次（制二）
    /// </summary>
    public string? ProductionBatch { get; set; }
    /// <summary>
    /// 预定日期
    /// </summary>
    public DateTime? ScheduledDate { get; set; }
    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime? ProductionDate { get; set; }
}

/// <summary>
/// 投入批次查询 DTO
/// </summary>
public class TaktEcBatchQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 设变单号
    /// </summary>
    public string? EcNo { get; set; }
    /// <summary>
    /// 批次号（预定/生产批次模糊）
    /// </summary>
    public string? BatchNo { get; set; }
    /// <summary>
    /// 机种（Ec_model）
    /// </summary>
    public string? EcModelCode { get; set; }

    /// <summary>
    /// 批次号（预定/生产批次模糊）
    /// </summary>
    public string BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; }

    /// <summary>
    /// 设变单号
    /// </summary>
    public string EcCode { get; set; } = string.Empty;
}

/// <summary>
/// 投入批次更新 DTO
/// </summary>
public class TaktEcBatchUpdateDto
{
    /// <summary>
    /// 设变明细 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcDetailId { get; set; }
    /// <summary>
    /// 预定批次
    /// </summary>
    public string? ScheduledBatch { get; set; }
    /// <summary>
    /// 生产批次
    /// </summary>
    public string? ProductionBatch { get; set; }
    /// <summary>
    /// 预定日期
    /// </summary>
    public DateTime? ScheduledDate { get; set; }
    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime? ProductionDate { get; set; }
}

// ========================================
// 实施管控
// ========================================

/// <summary>
/// 实施管控视图行
/// </summary>
public class TaktEcImplCtrlDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EcImplCtrlID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcImplCtrlId { get; set; }

    /// <summary>
    /// 设变明细 ID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcDetailId { get; set; }
    /// <summary>
    /// 设变单号
    /// </summary>
    public string EcCode { get; set; } = string.Empty;
    /// <summary>
    /// 行号
    /// </summary>
    public int LineNumber { get; set; }
    /// <summary>
    /// 机种（Ec_model）
    /// </summary>
    public string EcModelCode { get; set; } = string.Empty;
    /// <summary>
    /// 旧料号
    /// </summary>
    public string? EcOldMaterialCode { get; set; }
    /// <summary>
    /// 旧物料描述
    /// </summary>
    public string? EcOldMaterialDescription { get; set; }
    /// <summary>
    /// 旧采购类型（F=外部采购，E=自制生产）
    /// </summary>
    public string? EcOldPurchaseType { get; set; }
    /// <summary>
    /// 新料号
    /// </summary>
    public string? EcNewMaterialCode { get; set; }
    /// <summary>
    /// 新物料描述
    /// </summary>
    public string? EcNewMaterialDescription { get; set; }
    /// <summary>
    /// 旧品检验（0=否 1=是）
    /// </summary>
    public int EcOldRequiresInspection { get; set; }
    /// <summary>
    /// 新采购类型（F=外部采购，E=自制生产）
    /// </summary>
    public string? EcNewPurchaseType { get; set; }
    /// <summary>
    /// 新品检验（0=否 1=是）
    /// </summary>
    public int EcNewRequiresInspection { get; set; }
}

/// <summary>
/// 实施管控查询 DTO
/// </summary>
public class TaktEcImplCtrlQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 设变单号
    /// </summary>
    public string? EcNo { get; set; }
    /// <summary>
    /// 旧品是否检查
    /// </summary>
    public int? EcOldRequiresInspection { get; set; }
    /// <summary>
    /// 新品是否检查
    /// </summary>
    public int? EcNewRequiresInspection { get; set; }
    /// <summary>
    /// 机种（Ec_model）
    /// </summary>
    public string? EcModelCode { get; set; }
    /// <summary>
    /// 新料号
    /// </summary>
    public string? EcNewMaterialCode { get; set; }

    /// <summary>
    /// 旧料号
    /// </summary>
    public string? EcOldMaterialCode { get; set; }

    /// <summary>
    /// 旧采购类型（F=外部采购，E=自制生产）
    /// </summary>
    public string? EcOldPurchaseType { get; set; }

    /// <summary>
    /// 新采购类型（F=外部采购，E=自制生产）
    /// </summary>
    public string? EcNewPurchaseType { get; set; }

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; }

    /// <summary>
    /// 设变单号
    /// </summary>
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 页签：pc 生管、assy 制一、pcba 制二、qa 品管
    /// </summary>
    public string DeptTab { get; set; } = "mfg2";

    /// <summary>
    /// 录入日期起（含当天，按技术主表 EcEntryDate）
    /// </summary>
    public DateTime? EcEntryDateStart { get; set; }

    /// <summary>
    /// 录入日期止（含当天，按技术主表 EcEntryDate）
    /// </summary>
    public DateTime? EcEntryDateEnd { get; set; }
}

/// <summary>
/// 实施管控更新 DTO
/// </summary>
public class TaktEcImplCtrlUpdateDto
{
    /// <summary>
    /// 设变明细 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcDetailId { get; set; }
    /// <summary>
    /// 控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理），只记录生管/制二/制一/品管；全仕向时生管与此相同。采购/受检/部管/SMT 固定按设变，制技固定按机种
    /// </summary>
    public int EcControlScope { get; set; } = 2;

    /// <summary>
    /// 当前页签。只更新该部门执行表
    /// </summary>
    public string DeptTab { get; set; } = string.Empty;
}

// ========================================
// 旧品管制
// ========================================

/// <summary>
/// 旧品管制视图行
/// </summary>
public class TaktEcLegacyProductDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EcLegacyProductID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcLegacyProductId { get; set; }

    /// <summary>
    /// 设变明细 ID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcDetailId { get; set; }
    /// <summary>
    /// 设变单号
    /// </summary>
    public string EcCode { get; set; } = string.Empty;
    /// <summary>
    /// 行号
    /// </summary>
    public int LineNumber { get; set; }
    /// <summary>
    /// 机种（Ec_model）
    /// </summary>
    public string EcModelCode { get; set; } = string.Empty;
    /// <summary>
    /// 旧料号
    /// </summary>
    public string? EcOldMaterialCode { get; set; }
    /// <summary>
    /// 旧料号描述
    /// </summary>
    public string? EcOldMaterialDescription { get; set; }
    /// <summary>
    /// 旧用量
    /// </summary>
    public decimal? EcOldUsageQuantity { get; set; }
    /// <summary>
    /// 兼容性（两位码第1位 A=有 B=→ C=← D=无；第2位 1～9=同时变更 *=无同时变更）
    /// </summary>
    public string? EcIsCompatible { get; set; }
    /// <summary>
    /// 第二供应商（字典 logistics_manufacturing_ec_2nd_vendor；1=有 2=优先 3=无）
    /// </summary>
    public string? Ec2ndVendor { get; set; }
    /// <summary>
    /// 生产指令（字典 logistics_manufacturing_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
    /// </summary>
    public string? EcInstruction { get; set; }
    /// <summary>
    /// 明细旧品处理（TaktEcDetail.EcOldPartDisposition；字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
    /// </summary>
    public string? EcOldPartDisposition { get; set; }
    /// <summary>
    /// 新料号
    /// </summary>
    public string? EcNewMaterialCode { get; set; }
    /// <summary>
    /// 生管旧品处理（TaktEcPc.EcOldPartDisposition；字典 logistics_manufacturing_ec_old_part_disposition；按明细去重键关联生管执行行）
    /// </summary>
    public string? OldProductHandling { get; set; }
    /// <summary>
    /// 停产状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料）
    /// </summary>
    public string DiscontinuedStatus { get; set; } = string.Empty;
}

/// <summary>
/// 旧品管制查询 DTO
/// </summary>
public class TaktEcLegacyProductQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; }

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; }

    /// <summary>
    /// 设变单号
    /// </summary>
    public string? EcCode { get; set; }

    /// <summary>
    /// 机种编码
    /// </summary>
    public string? EcModelCode { get; set; }

    /// <summary>
    /// 旧物料编码
    /// </summary>
    public string? EcOldMaterialCode { get; set; }
}

/// <summary>
/// 旧品管制更新 DTO
/// </summary>
public class TaktEcLegacyProductUpdateDto
{
    /// <summary>
    /// 设变明细 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcDetailId { get; set; }
    /// <summary>
    /// 生管旧品处理（TaktEcPc.EcOldPartDisposition；字典 logistics_manufacturing_ec_old_part_disposition；与明细 EcOldPartDisposition 同字典，存执行行）
    /// </summary>
    public string? OldProductHandling { get; set; }
    /// <summary>
    /// 停产状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料）
    /// </summary>
    public string DiscontinuedStatus { get; set; } = string.Empty;
    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
}
