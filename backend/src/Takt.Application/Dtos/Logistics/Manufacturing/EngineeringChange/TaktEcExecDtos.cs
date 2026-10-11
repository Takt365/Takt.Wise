// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcExecDtos.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：EcExec 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktEcExec 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;

// ========================================
// EcExec 响应 DTO
// ========================================

/// <summary>
/// 设变部门执行 DTO。DeptCode 为 TaktDept 部门编码（5 位，见 TaktEcDeptCodes / TaktDeptSeedData）。
/// 对应前端 EcExec
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktEcExecDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EcExecID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcExecId { get; set; }

    /// <summary>
    /// 设变部门执行 ID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// 设变明细ID（TaktEcDetail 主键）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcDetailId { get; set; }

    /// <summary>
    /// 设变明细名称（填充字段）
    /// </summary>
    public string? EcDetailName { get; set; }

    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    public string EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 部门编码（TaktDept.DeptCode，5 位；如 D0710 技术课、D0420 生管课、D0810 受检课、D0620 制造2课）
    /// </summary>
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 是否实施（0=否 1=是）
    /// </summary>
    public int IsImplemented { get; set; } = 0;

    /// <summary>
    /// 内容（各部门通用）
    /// </summary>
    public string? ExecContent { get; set; } = string.Empty;

    /// <summary>
    /// 录入日期
    /// </summary>
    public DateTime? EntryDate { get; set; }

    /// <summary>
    /// 担当（EcLeader）
    /// </summary>
    public string? EcLeader { get; set; } = string.Empty;

    /// <summary>
    /// 预定日期
    /// </summary>
    public DateTime? ScheduledDate { get; set; }

    /// <summary>
    /// 预定批次
    /// </summary>
    public string? ScheduledBatch { get; set; } = string.Empty;

    /// <summary>
    /// Po残（采购订单残）
    /// </summary>
    public string? PoRemainder { get; set; } = string.Empty;

    /// <summary>
    /// 结余
    /// </summary>
    public string? Balance { get; set; } = string.Empty;

    /// <summary>
    /// 旧品处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
    /// </summary>
    public string? EcOldPartDisposition { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单发行日期
    /// </summary>
    public DateTime? PurchaseOrderIssueDate { get; set; }

    /// <summary>
    /// 供应商
    /// </summary>
    public string? Supplier { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单号码
    /// </summary>
    public string? PurchaseOrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 受检单号
    /// </summary>
    public string? IqcOrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 检验/检查日期
    /// </summary>
    public DateTime? InspectionDate { get; set; }

    /// <summary>
    /// 出库批次
    /// </summary>
    public string? OutboundBatch { get; set; } = string.Empty;

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
    public string? ProductionBatch { get; set; } = string.Empty;

    /// <summary>
    /// 出库单号
    /// </summary>
    public string? OutboundOrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组
    /// </summary>
    public string? ProductionTeam { get; set; } = string.Empty;

    /// <summary>
    /// 实施日期
    /// </summary>
    public DateTime? ImplementationDate { get; set; }

    /// <summary>
    /// 实施批次
    /// </summary>
    public string? ImplementationBatch { get; set; } = string.Empty;

    /// <summary>
    /// 检验批次
    /// </summary>
    public string? InspectionBatch { get; set; } = string.Empty;

    /// <summary>
    /// 抽样号码
    /// </summary>
    public string? SamplingNo { get; set; } = string.Empty;

    /// <summary>
    /// SOP日期
    /// </summary>
    public DateTime? SopDate { get; set; }

    /// <summary>
    /// 更新SOP（0=否 1=是）
    /// </summary>
    public int IsSopUpdated { get; set; } = 0;

    /// <summary>
    /// 设变明细（多对一）
    /// （主表：TaktEcDetail）
    /// </summary>
    public TaktEcDetailDto? EcDetail { get; set; }

}

// ========================================
// EcDept 查询 DTO
// ========================================

/// <summary>
/// EcDept 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktEcExecQueryDto : TaktPagedQuery
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
    /// 设变明细ID（TaktEcDetail 主键）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcDetailId { get; set; }

    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    public string? EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 部门编码（TaktDept.DeptCode，5 位；如 D0710 技术课、D0420 生管课、D0810 受检课、D0620 制造2课）
    /// </summary>
    public string? DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 是否实施（0=否 1=是）
    /// </summary>
    public int? IsImplemented { get; set; }

    /// <summary>
    /// 内容（各部门通用）
    /// </summary>
    public string? ExecContent { get; set; } = string.Empty;

    /// <summary>
    /// 录入日期（范围查询-开始）
    /// </summary>
    public DateTime? EntryDateStart { get; set; }

    /// <summary>
    /// 录入日期（范围查询-结束）
    /// </summary>
    public DateTime? EntryDateEnd { get; set; }

    /// <summary>
    /// 担当（EcLeader）
    /// </summary>
    public string? EcLeader { get; set; } = string.Empty;

    /// <summary>
    /// 预定日期（范围查询-开始）
    /// </summary>
    public DateTime? ScheduledDateStart { get; set; }

    /// <summary>
    /// 预定日期（范围查询-结束）
    /// </summary>
    public DateTime? ScheduledDateEnd { get; set; }

    /// <summary>
    /// 预定批次
    /// </summary>
    public string? ScheduledBatch { get; set; } = string.Empty;

    /// <summary>
    /// Po残（采购订单残）
    /// </summary>
    public string? PoRemainder { get; set; } = string.Empty;

    /// <summary>
    /// 结余
    /// </summary>
    public string? Balance { get; set; } = string.Empty;

    /// <summary>
    /// 旧品处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
    /// </summary>
    public string? EcOldPartDisposition { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单发行日期（范围查询-开始）
    /// </summary>
    public DateTime? PurchaseOrderIssueDateStart { get; set; }

    /// <summary>
    /// 采购订单发行日期（范围查询-结束）
    /// </summary>
    public DateTime? PurchaseOrderIssueDateEnd { get; set; }

    /// <summary>
    /// 供应商
    /// </summary>
    public string? Supplier { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单号码
    /// </summary>
    public string? PurchaseOrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 受检单号
    /// </summary>
    public string? IqcOrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 检验/检查日期（范围查询-开始）
    /// </summary>
    public DateTime? InspectionDateStart { get; set; }

    /// <summary>
    /// 检验/检查日期（范围查询-结束）
    /// </summary>
    public DateTime? InspectionDateEnd { get; set; }

    /// <summary>
    /// 出库批次
    /// </summary>
    public string? OutboundBatch { get; set; } = string.Empty;

    /// <summary>
    /// 出库日期（范围查询-开始）
    /// </summary>
    public DateTime? OutboundDateStart { get; set; }

    /// <summary>
    /// 出库日期（范围查询-结束）
    /// </summary>
    public DateTime? OutboundDateEnd { get; set; }

    /// <summary>
    /// 生产日期（范围查询-开始）
    /// </summary>
    public DateTime? ProductionDateStart { get; set; }

    /// <summary>
    /// 生产日期（范围查询-结束）
    /// </summary>
    public DateTime? ProductionDateEnd { get; set; }

    /// <summary>
    /// 生产批次
    /// </summary>
    public string? ProductionBatch { get; set; } = string.Empty;

    /// <summary>
    /// 出库单号
    /// </summary>
    public string? OutboundOrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组
    /// </summary>
    public string? ProductionTeam { get; set; } = string.Empty;

    /// <summary>
    /// 实施日期（范围查询-开始）
    /// </summary>
    public DateTime? ImplementationDateStart { get; set; }

    /// <summary>
    /// 实施日期（范围查询-结束）
    /// </summary>
    public DateTime? ImplementationDateEnd { get; set; }

    /// <summary>
    /// 实施批次
    /// </summary>
    public string? ImplementationBatch { get; set; } = string.Empty;

    /// <summary>
    /// 检验批次
    /// </summary>
    public string? InspectionBatch { get; set; } = string.Empty;

    /// <summary>
    /// 抽样号码
    /// </summary>
    public string? SamplingNo { get; set; } = string.Empty;

    /// <summary>
    /// SOP日期（范围查询-开始）
    /// </summary>
    public DateTime? SopDateStart { get; set; }

    /// <summary>
    /// SOP日期（范围查询-结束）
    /// </summary>
    public DateTime? SopDateEnd { get; set; }

    /// <summary>
    /// 更新SOP（0=否 1=是）
    /// </summary>
    public int? IsSopUpdated { get; set; }

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
// 创建EcDept DTO
// ========================================

/// <summary>
/// 创建EcDept DTO
/// </summary>
public class TaktEcExecCreateDto
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
    /// 设变明细ID（TaktEcDetail 主键）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcDetailId { get; set; }

    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    [Required(ErrorMessage = "设变单号（冗余字段,便于查询）不能为空")]
    public string EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 部门编码（TaktDept.DeptCode，5 位；如 D0710 技术课、D0420 生管课、D0810 受检课、D0620 制造2课）
    /// </summary>
    [Required(ErrorMessage = "部门编码。顺序严格为：Eng=技术, Pmc=生管, Mp=采购, Iqc=受检, Mc=部管, Pcba=制二, Assy=制一, Qa=品管, Te=制技。不能为空")]
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 是否实施（0=否 1=是）
    /// </summary>
    public int IsImplemented { get; set; } = 0;

    /// <summary>
    /// 内容（各部门通用）
    /// </summary>
    public string? ExecContent { get; set; } = string.Empty;

    /// <summary>
    /// 录入日期
    /// </summary>
    public DateTime? EntryDate { get; set; }

    /// <summary>
    /// 担当（EcLeader）
    /// </summary>
    public string? EcLeader { get; set; } = string.Empty;

    /// <summary>
    /// 预定日期
    /// </summary>
    public DateTime? ScheduledDate { get; set; }

    /// <summary>
    /// 预定批次
    /// </summary>
    public string? ScheduledBatch { get; set; } = string.Empty;

    /// <summary>
    /// Po残（采购订单残）
    /// </summary>
    public string? PoRemainder { get; set; } = string.Empty;

    /// <summary>
    /// 结余
    /// </summary>
    public string? Balance { get; set; } = string.Empty;

    /// <summary>
    /// 旧品处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
    /// </summary>
    public string? EcOldPartDisposition { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单发行日期
    /// </summary>
    public DateTime? PurchaseOrderIssueDate { get; set; }

    /// <summary>
    /// 供应商
    /// </summary>
    public string? Supplier { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单号码
    /// </summary>
    public string? PurchaseOrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 受检单号
    /// </summary>
    public string? IqcOrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 检验/检查日期
    /// </summary>
    public DateTime? InspectionDate { get; set; }

    /// <summary>
    /// 出库批次
    /// </summary>
    public string? OutboundBatch { get; set; } = string.Empty;

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
    public string? ProductionBatch { get; set; } = string.Empty;

    /// <summary>
    /// 出库单号
    /// </summary>
    public string? OutboundOrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组
    /// </summary>
    public string? ProductionTeam { get; set; } = string.Empty;

    /// <summary>
    /// 实施日期
    /// </summary>
    public DateTime? ImplementationDate { get; set; }

    /// <summary>
    /// 实施批次
    /// </summary>
    public string? ImplementationBatch { get; set; } = string.Empty;

    /// <summary>
    /// 检验批次
    /// </summary>
    public string? InspectionBatch { get; set; } = string.Empty;

    /// <summary>
    /// 抽样号码
    /// </summary>
    public string? SamplingNo { get; set; } = string.Empty;

    /// <summary>
    /// SOP日期
    /// </summary>
    public DateTime? SopDate { get; set; }

    /// <summary>
    /// 更新SOP（0=否 1=是）
    /// </summary>
    public int IsSopUpdated { get; set; } = 0;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }



    /// <summary>
    /// EcCode
    /// </summary>
    public string EcCode { get; set; } = string.Empty;
}

// ========================================
// 更新 EcExec DTO
// ========================================

/// <summary>
/// 更新设变部门执行 DTO
/// 继承 TaktEcExecCreateDto，添加 Id 字段
/// </summary>
public class TaktEcExecUpdateDto : TaktEcExecCreateDto
{
    /// <summary>
    /// 设变部门执行 ID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public string Id { get; set; } = string.Empty;

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// EcDept 导入模板行 DTO
/// </summary>
public class TaktEcSeizougijutsumplateDto
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
    /// 设变明细ID（TaktEcDetail 主键）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcDetailId { get; set; }

    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    public string? EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 部门编码（TaktDept.DeptCode，5 位；如 D0710 技术课、D0420 生管课、D0810 受检课、D0620 制造2课）
    /// </summary>
    public string? DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 是否实施（0=否 1=是）
    /// </summary>
    public int? IsImplemented { get; set; }

    /// <summary>
    /// 内容（各部门通用）
    /// </summary>
    public string? ExecContent { get; set; } = string.Empty;

    /// <summary>
    /// 录入日期
    /// </summary>
    public DateTime? EntryDate { get; set; }

    /// <summary>
    /// 担当（EcLeader）
    /// </summary>
    public string? EcLeader { get; set; } = string.Empty;

    /// <summary>
    /// 预定日期
    /// </summary>
    public DateTime? ScheduledDate { get; set; }

    /// <summary>
    /// 预定批次
    /// </summary>
    public string? ScheduledBatch { get; set; } = string.Empty;

    /// <summary>
    /// Po残（采购订单残）
    /// </summary>
    public string? PoRemainder { get; set; } = string.Empty;

    /// <summary>
    /// 结余
    /// </summary>
    public string? Balance { get; set; } = string.Empty;

    /// <summary>
    /// 旧品处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
    /// </summary>
    public string? EcOldPartDisposition { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单发行日期
    /// </summary>
    public DateTime? PurchaseOrderIssueDate { get; set; }

    /// <summary>
    /// 供应商
    /// </summary>
    public string? Supplier { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单号码
    /// </summary>
    public string? PurchaseOrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 受检单号
    /// </summary>
    public string? IqcOrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 检验/检查日期
    /// </summary>
    public DateTime? InspectionDate { get; set; }

    /// <summary>
    /// 出库批次
    /// </summary>
    public string? OutboundBatch { get; set; } = string.Empty;

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
    public string? ProductionBatch { get; set; } = string.Empty;

    /// <summary>
    /// 出库单号
    /// </summary>
    public string? OutboundOrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组
    /// </summary>
    public string? ProductionTeam { get; set; } = string.Empty;

    /// <summary>
    /// 实施日期
    /// </summary>
    public DateTime? ImplementationDate { get; set; }

    /// <summary>
    /// 实施批次
    /// </summary>
    public string? ImplementationBatch { get; set; } = string.Empty;

    /// <summary>
    /// 检验批次
    /// </summary>
    public string? InspectionBatch { get; set; } = string.Empty;

    /// <summary>
    /// 抽样号码
    /// </summary>
    public string? SamplingNo { get; set; } = string.Empty;

    /// <summary>
    /// SOP日期（范围查询-开始）
    /// </summary>
    public DateTime? SopDateStart { get; set; }

    /// <summary>
    /// SOP日期（范围查询-结束）
    /// </summary>
    public DateTime? SopDateEnd { get; set; }

    /// <summary>
    /// 更新SOP（0=否 1=是）
    /// </summary>
    public int? IsSopUpdated { get; set; }

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
/// EcDept 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktEcExecImportDto
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
    /// 设变明细ID（TaktEcDetail 主键）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcDetailId { get; set; }

    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    public string? EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 部门编码（TaktDept.DeptCode，5 位；如 D0710 技术课、D0420 生管课、D0810 受检课、D0620 制造2课）
    /// </summary>
    public string? DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 是否实施（0=否 1=是）
    /// </summary>
    public int? IsImplemented { get; set; }

    /// <summary>
    /// 内容（各部门通用）
    /// </summary>
    public string? ExecContent { get; set; } = string.Empty;

    /// <summary>
    /// 录入日期
    /// </summary>
    public DateTime? EntryDate { get; set; }

    /// <summary>
    /// 担当（EcLeader）
    /// </summary>
    public string? EcLeader { get; set; } = string.Empty;

    /// <summary>
    /// 预定日期
    /// </summary>
    public DateTime? ScheduledDate { get; set; }

    /// <summary>
    /// 预定批次
    /// </summary>
    public string? ScheduledBatch { get; set; } = string.Empty;

    /// <summary>
    /// Po残（采购订单残）
    /// </summary>
    public string? PoRemainder { get; set; } = string.Empty;

    /// <summary>
    /// 结余
    /// </summary>
    public string? Balance { get; set; } = string.Empty;

    /// <summary>
    /// 旧品处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
    /// </summary>
    public string? EcOldPartDisposition { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单发行日期
    /// </summary>
    public DateTime? PurchaseOrderIssueDate { get; set; }

    /// <summary>
    /// 供应商
    /// </summary>
    public string? Supplier { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单号码
    /// </summary>
    public string? PurchaseOrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 受检单号
    /// </summary>
    public string? IqcOrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 检验/检查日期
    /// </summary>
    public DateTime? InspectionDate { get; set; }

    /// <summary>
    /// 出库批次
    /// </summary>
    public string? OutboundBatch { get; set; } = string.Empty;

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
    public string? ProductionBatch { get; set; } = string.Empty;

    /// <summary>
    /// 出库单号
    /// </summary>
    public string? OutboundOrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组
    /// </summary>
    public string? ProductionTeam { get; set; } = string.Empty;

    /// <summary>
    /// 实施日期
    /// </summary>
    public DateTime? ImplementationDate { get; set; }

    /// <summary>
    /// 实施批次
    /// </summary>
    public string? ImplementationBatch { get; set; } = string.Empty;

    /// <summary>
    /// 检验批次
    /// </summary>
    public string? InspectionBatch { get; set; } = string.Empty;

    /// <summary>
    /// 抽样号码
    /// </summary>
    public string? SamplingNo { get; set; } = string.Empty;

    /// <summary>
    /// SOP日期（范围查询-开始）
    /// </summary>
    public DateTime? SopDateStart { get; set; }

    /// <summary>
    /// SOP日期（范围查询-结束）
    /// </summary>
    public DateTime? SopDateEnd { get; set; }

    /// <summary>
    /// 更新SOP（0=否 1=是）
    /// </summary>
    public int? IsSopUpdated { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }



    /// <summary>
    /// EcCode
    /// </summary>
    public string EcCode { get; set; } = string.Empty;
}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// 设变部门执行导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktEcExecExportDto
{
    /// <summary>
    /// 设变部门执行 ID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 设变明细ID（TaktEcDetail 主键）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcDetailId { get; set; }

    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    public string EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 部门编码（TaktDept.DeptCode，5 位；如 D0710 技术课、D0420 生管课、D0810 受检课、D0620 制造2课）
    /// </summary>
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 是否实施（0=否 1=是）
    /// </summary>
    public int IsImplemented { get; set; } = 0;

    /// <summary>
    /// 内容（各部门通用）
    /// </summary>
    public string? ExecContent { get; set; } = string.Empty;

    /// <summary>
    /// 录入日期
    /// </summary>
    public DateTime? EntryDate { get; set; }

    /// <summary>
    /// 担当（EcLeader）
    /// </summary>
    public string? EcLeader { get; set; } = string.Empty;

    /// <summary>
    /// 预定日期
    /// </summary>
    public DateTime? ScheduledDate { get; set; }

    /// <summary>
    /// 预定批次
    /// </summary>
    public string? ScheduledBatch { get; set; } = string.Empty;

    /// <summary>
    /// Po残（采购订单残）
    /// </summary>
    public string? PoRemainder { get; set; } = string.Empty;

    /// <summary>
    /// 结余
    /// </summary>
    public string? Balance { get; set; } = string.Empty;

    /// <summary>
    /// 旧品处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
    /// </summary>
    public string? EcOldPartDisposition { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单发行日期
    /// </summary>
    public DateTime? PurchaseOrderIssueDate { get; set; }

    /// <summary>
    /// 供应商
    /// </summary>
    public string? Supplier { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单号码
    /// </summary>
    public string? PurchaseOrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 受检单号
    /// </summary>
    public string? IqcOrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 检验/检查日期
    /// </summary>
    public DateTime? InspectionDate { get; set; }

    /// <summary>
    /// 出库批次
    /// </summary>
    public string? OutboundBatch { get; set; } = string.Empty;

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
    public string? ProductionBatch { get; set; } = string.Empty;

    /// <summary>
    /// 出库单号
    /// </summary>
    public string? OutboundOrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组
    /// </summary>
    public string? ProductionTeam { get; set; } = string.Empty;

    /// <summary>
    /// 实施日期
    /// </summary>
    public DateTime? ImplementationDate { get; set; }

    /// <summary>
    /// 实施批次
    /// </summary>
    public string? ImplementationBatch { get; set; } = string.Empty;

    /// <summary>
    /// 检验批次
    /// </summary>
    public string? InspectionBatch { get; set; } = string.Empty;

    /// <summary>
    /// 抽样号码
    /// </summary>
    public string? SamplingNo { get; set; } = string.Empty;

    /// <summary>
    /// SOP日期
    /// </summary>
    public DateTime? SopDate { get; set; }

    /// <summary>
    /// 更新SOP（0=否 1=是）
    /// </summary>
    public int IsSopUpdated { get; set; } = 0;

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

// ========================================
// 设变部门执行转置（行=设变明细，列=各部门实施状态）
// ========================================

/// <summary>
/// 设变部门执行转置单元格 DTO
/// </summary>
public class TaktEcExecTransposedCellDto
{
    /// <summary>
    /// 部门编码
    /// </summary>
    public string DeptCode { get; set; } = string.Empty;
    /// <summary>
    /// 是否实施（0=否 1=是）
    /// </summary>
    public int IsImplemented { get; set; }
    /// <summary>
    /// 完成日期
    /// </summary>
    public DateTime? CompletedDate { get; set; }
    /// <summary>
    /// 展示文本（已实施 yyyyMMdd；未实施 null）
    /// </summary>
    public string? DisplayText { get; set; }
}

/// <summary>
/// 设变部门执行转置行 DTO（一行=一条设变明细 + 各部门列）
/// </summary>
public class TaktEcExecTransposedDto
{
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
    /// 明细行号
    /// </summary>
    public int LineNumber { get; set; }
    /// <summary>
    /// 发行日期（主表 EcIssueDate）
    /// </summary>
    public DateTime EcIssueDate { get; set; }
    /// <summary>
    /// 设变EC担当/负责人（主表 EcLeader）
    /// </summary>
    public string EcLeader { get; set; } = string.Empty;
    /// <summary>
    /// 设变单号
    /// </summary>
    public string EcNo { get; set; } = string.Empty;
    /// <summary>
    /// 机种（EcModelCode）
    /// </summary>
    public string EcModelCode { get; set; } = string.Empty;
    /// <summary>
    /// 成品（EcNewMaterialCode）
    /// </summary>
    public string? EcNewMaterialCode { get; set; }
    /// <summary>
    /// 各部门单元格；键为 DeptCode
    /// </summary>
    public Dictionary<string, TaktEcExecTransposedCellDto> DeptCells { get; set; } = new();

    /// <summary>
    /// EcCode
    /// </summary>
    public string EcCode { get; set; } = string.Empty;
}

/// <summary>
/// 设变部门执行转置查询 DTO
/// </summary>
public class TaktEcExecTransposedQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 设变单号
    /// </summary>
    public string? EcNo { get; set; }
    /// <summary>
    /// 机种
    /// </summary>
    public string? EcModelCode { get; set; }
    /// <summary>
    /// 成品（新料号）
    /// </summary>
    public string? EcNewMaterialCode { get; set; }
    /// <summary>
    /// 设变EC担当
    /// </summary>
    public string? EcLeader { get; set; }
    /// <summary>
    /// 发行日期（范围-开始）
    /// </summary>
    public DateTime? EcIssueDateStart { get; set; }
    /// <summary>
    /// 发行日期（范围-结束）
    /// </summary>
    public DateTime? EcIssueDateEnd { get; set; }
    /// <summary>
    /// 部门编码（与 IsImplemented 组合筛选某部门实施状态）
    /// </summary>
    public string? DeptCode { get; set; }
    /// <summary>
    /// 是否实施（须配合 DeptCode）
    /// </summary>
    public int? IsImplemented { get; set; }

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; }

    /// <summary>
    /// EcCode
    /// </summary>
    public string? EcCode { get; set; }
}

/// <summary>
/// 设变部门执行转置分页结果 DTO（含部门列顺序）
/// </summary>
public class TaktEcExecTransposedResultDto
{
    /// <summary>
    /// 分页数据
    /// </summary>
    public TaktPagedResult<TaktEcExecTransposedDto> Paged { get; set; } = null!;
    /// <summary>
    /// 部门列顺序（表头从左到右）
    /// </summary>
    public IReadOnlyList<string> DeptCodeOrder { get; set; } = Array.Empty<string>();
}

// ========================================
// 设变部门批次转置（行=设变明细，列=各阶段日期+批次）
// ========================================

/// <summary>
/// 设变批次转置阶段单元格 DTO
/// </summary>
public class TaktEcExecBatchTransposedStageDto
{
    /// <summary>
    /// 阶段编码（TaktEcBatchStageCodes）
    /// </summary>
    public string StageCode { get; set; } = string.Empty;
    /// <summary>
    /// 阶段日期
    /// </summary>
    public DateTime? StageDate { get; set; }
    /// <summary>
    /// 批次号/批次说明
    /// </summary>
    public string? BatchNo { get; set; }
    /// <summary>
    /// 日期展示文本（yyyyMMdd）
    /// </summary>
    public string? DateDisplayText { get; set; }

    /// <summary>
    /// BatchCode
    /// </summary>
    public string BatchCode { get; set; } = string.Empty;
}

/// <summary>
/// 设变批次转置行 DTO（一行=一条设变明细 + 各阶段批次列）
/// </summary>
public class TaktEcExecBatchTransposedDto
{
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
    /// 明细行号
    /// </summary>
    public int LineNumber { get; set; }
    /// <summary>
    /// 设变单号
    /// </summary>
    public string EcNo { get; set; } = string.Empty;
    /// <summary>
    /// 技联 No.（附件 TL DocNo）
    /// </summary>
    public string? TechnicalLiaisonNo { get; set; }
    /// <summary>
    /// P番 No.（附件 FPP DocNo）
    /// </summary>
    public string? PNo { get; set; }
    /// <summary>
    /// TCJ 技联 No.（附件 TCJ DocNo）
    /// </summary>
    public string? TcjLiaisonNo { get; set; }
    /// <summary>
    /// 发行日期（主表 EcIssueDate）
    /// </summary>
    public DateTime EcIssueDate { get; set; }
    /// <summary>
    /// 机种（EcModelCode）
    /// </summary>
    public string EcModelCode { get; set; } = string.Empty;
    /// <summary>
    /// 成品（EcNewMaterialCode）
    /// </summary>
    public string? EcNewMaterialCode { get; set; }
    /// <summary>
    /// 登入日期（主表 EcEntryDate）
    /// </summary>
    public DateTime EcEntryDate { get; set; }
    /// <summary>
    /// 各批次阶段单元格；键为 StageCode
    /// </summary>
    public Dictionary<string, TaktEcExecBatchTransposedStageDto> StageCells { get; set; } = new();

    /// <summary>
    /// 设变单号
    /// </summary>
    public string EcCode { get; set; } = string.Empty;
}

/// <summary>
/// 设变批次转置查询 DTO
/// </summary>
public class TaktEcExecBatchTransposedQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 设变单号
    /// </summary>
    public string? EcNo { get; set; }
    /// <summary>
    /// 机种
    /// </summary>
    public string? EcModelCode { get; set; }
    /// <summary>
    /// 成品（新料号）
    /// </summary>
    public string? EcNewMaterialCode { get; set; }
    /// <summary>
    /// 发行日期（范围-开始）
    /// </summary>
    public DateTime? EcIssueDateStart { get; set; }
    /// <summary>
    /// 发行日期（范围-结束）
    /// </summary>
    public DateTime? EcIssueDateEnd { get; set; }
    /// <summary>
    /// 批次号（预定/出库/生产批次模糊）
    /// </summary>
    public string? BatchNo { get; set; }

    /// <summary>
    /// 批次号（预定/出库/生产批次模糊）
    /// </summary>
    public string BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 设变单号
    /// </summary>
    public string EcCode { get; set; } = string.Empty;
}

/// <summary>
/// 设变批次转置分页结果 DTO（含阶段列顺序）
/// </summary>
public class TaktEcExecBatchTransposedResultDto
{
    /// <summary>
    /// 分页数据
    /// </summary>
    public TaktPagedResult<TaktEcExecBatchTransposedDto> Paged { get; set; } = null!;
    /// <summary>
    /// 阶段列顺序（表头从左到右）
    /// </summary>
    public IReadOnlyList<string> StageCodeOrder { get; set; } = Array.Empty<string>();
}
