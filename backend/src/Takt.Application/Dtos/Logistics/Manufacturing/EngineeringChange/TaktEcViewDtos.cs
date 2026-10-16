// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcViewDtos.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变看板/投入批次/物料确认/部门视图/旧品管制 专用 DTO
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
    public long EcGijutsuId { get; set; }
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
    /// Po残
    /// </summary>
    public string? PoRemainder { get; set; }
    /// <summary>
    /// 结余
    /// </summary>
    public string? Balance { get; set; }
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
    /// Po残
    /// </summary>
    public string? PoRemainder { get; set; }
    /// <summary>
    /// 结余
    /// </summary>
    public string? Balance { get; set; }
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
    /// Po残
    /// </summary>
    public string? PoRemainder { get; set; }
    /// <summary>
    /// 结余
    /// </summary>
    public string? Balance { get; set; }
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
/// 设变看板行（按设变单汇总各部门实施进度）
/// </summary>
public class TaktEcKanbanDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EcKanbanID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcKanbanId { get; set; }

    /// <summary>
    /// 设变主表 ID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcGijutsuId { get; set; }
    /// <summary>
    /// 设变单号
    /// </summary>
    public string EcCode { get; set; } = string.Empty;
    /// <summary>
    /// 设变标题
    /// </summary>
    public string EcTitle { get; set; } = string.Empty;
    /// <summary>
    /// 变更状态
    /// </summary>
    public int ChangeStatus { get; set; }
    /// <summary>
    /// 设变状态
    /// </summary>
    public int EcStatus { get; set; }
    /// <summary>
    /// 负责人
    /// </summary>
    public string EcLeader { get; set; } = string.Empty;
    /// <summary>
    /// 明细行数
    /// </summary>
    public int DetailCount { get; set; }
    /// <summary>
    /// 各部门实施汇总
    /// </summary>
    public List<TaktEcKanbanDeptStageDto> DeptStages { get; set; } = [];
    /// <summary>
    /// 当前待实施部门编码（路径上首个未全部完成的部门；字典 logistics_ec_dept_code）
    /// </summary>
    public string? CurrentDeptCode { get; set; }
    /// <summary>
    /// 待实施数（各执行部门行数之和，与明细数 DetailCount 分开）
    /// </summary>
    public int PendingAtCurrentDeptCount { get; set; }
    /// <summary>
    /// 实施路径状态（0 未开始 1 实施中 2 正式完成 3 全部完成）
    /// </summary>
    public int ImplementationStatus { get; set; }
    /// <summary>
    /// 品管课是否已全部实施（正式完成）
    /// </summary>
    public int IsOfficiallyCompleted { get; set; }
}

/// <summary>
/// 看板部门阶段
/// </summary>
public class TaktEcKanbanDeptStageDto
{
    /// <summary>
    /// 部门编码
    /// </summary>
    public string DeptCode { get; set; } = string.Empty;
    /// <summary>
    /// 已实施执行行数
    /// </summary>
    public int ImplementedCount { get; set; }
    /// <summary>
    /// 该执行部门行数
    /// </summary>
    public int TotalCount { get; set; }
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
    /// 实施路径状态（0 未开始 1 实施中 2 正式完成 3 全部完成）
    /// </summary>
    public int? ImplementationStatus { get; set; }

    /// <summary>
    /// EcCode
    /// </summary>
    public string? EcCode { get; set; }

    /// <summary>
    /// CultureCode
    /// </summary>
    public string? CultureCode { get; set; }
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
// 物料确认
// ========================================

/// <summary>
/// 物料确认视图行
/// </summary>
public class TaktEcKakuninDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EcKakuninID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcKakuninId { get; set; }

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
/// 物料确认查询 DTO
/// </summary>
public class TaktEcKakuninQueryDto : TaktPagedQuery
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
}

/// <summary>
/// 物料确认更新 DTO
/// </summary>
public class TaktEcKakuninUpdateDto
{
    /// <summary>
    /// 设变明细 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcDetailId { get; set; }
    /// <summary>
    /// 旧采购类型（F=外部采购，E=自制生产）
    /// </summary>
    public string? EcOldPurchaseType { get; set; }
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
    /// 旧品处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
    /// </summary>
    public string? EcOldPartDisposition { get; set; }
    /// <summary>
    /// 新料号
    /// </summary>
    public string? EcNewMaterialCode { get; set; }
    /// <summary>
    /// 生管旧品处理（TaktEcSeikan.EcOldPartDisposition；字典 logistics_manufacturing_ec_old_part_disposition；与明细 EcOldPartDisposition 同字典，存执行行）
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
    /// 生管旧品处理（TaktEcSeikan.EcOldPartDisposition；字典 logistics_manufacturing_ec_old_part_disposition；与明细 EcOldPartDisposition 同字典，存执行行）
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
