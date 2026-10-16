// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcGijutsuDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：EcGijutsu 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktEcGijutsu 生成，请按需审阅）
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
// EcGijutsu 响应 DTO
// ========================================

/// <summary>
/// 设变技术课主表实体（技术阶段一 ①）。流程：TaktEcGijutsu → TaktEcAttachment → TaktEcDetail → 系统自动生成 TaktEcNotification 并派发； 通知到达各部门后各部门在 TaktEcExec* 填报执行；技术通过看板/批次等监控执行情况。
/// 对应前端 TaktEcGijutsuDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktEcGijutsuDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EcGijutsuID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcGijutsuId { get; set; }

    /// <summary>
    /// 设变单号（唯一）
    /// </summary>
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 发行日期
    /// </summary>
    public DateTime EcIssueDate { get; set; }

    /// <summary>
    /// 变更状态（字典 logistics_manufacturing_ec_status；1=工作的，2=取消的，3=发行的，4=P.P中变更的，5=固定的，6=挂起的，7=拒绝的；默认 5=固定的，表示设变将被实施）
    /// </summary>
    public int ChangeStatus { get; set; } = 5;

    /// <summary>
    /// 设变标题
    /// </summary>
    public string EcTitle { get; set; } = string.Empty;

    /// <summary>
    /// 设变内容（富文本 HTML）
    /// </summary>
    public string EcContent { get; set; } = string.Empty;

    /// <summary>
    /// 后台派生各部门执行行摘要（非持久化；仅 Create/Update 返回给后台任务拼完成消息）
    /// </summary>
    [JsonIgnore]
    public string? PersistDeptExecSummary { get; set; }

    /// <summary>
    /// 后台保存后的未作废明细行数（表 takt_logistics_manufacturing_ec_detail；非请求体 EcDetails 长度）
    /// </summary>
    [JsonIgnore]
    public int PersistDetailCount { get; set; }

    /// <summary>
    /// 设变担当（选项 TaktEcGroups/options?ecGroupCategory=1；DictValue=EcGroupCode，DictLabel=EcGroupName）
    /// </summary>
    public string EcLeader { get; set; } = string.Empty;

    /// <summary>
    /// 损失金额
    /// </summary>
    public decimal EcLossAmount { get; set; }

    /// <summary>
    /// 实施范围（字典 logistics_manufacturing_ec_scope_category；1=全仕向，2=部管，3=内部，4=技术）
    /// </summary>
    public int EcScope { get; set; } = 0;

    /// <summary>
    /// 制二/制一/品管实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）。只记录这三课
    /// </summary>
    public int EcDeptScope { get; set; } = 2;

    /// <summary>
    /// 录入日期
    /// </summary>
    public DateTime EcEntryDate { get; set; }

    /// <summary>
    /// 设变状态（字典 logistics_manufacturing_ec_gijutsu_status；1=发行，2=执行中，3=完成；自动回写，客户端勿改）
    /// </summary>
    public int EcStatus { get; set; } = 0;

    /// <summary>
    /// 设变明细列表（技术阶段一：③，BOM/料号变更行）
    /// （子表：TaktEcDetail）
    /// </summary>
    public List<TaktEcDetailDto>? EcDetails { get; set; }

    /// <summary>
    /// 设变附件列表（技术阶段一：②，联络/EPP/FPP 等文档）
    /// （子表：TaktEcAttachment）
    /// </summary>
    public List<TaktEcAttachmentDto>? Attachments { get; set; }

    /// <summary>
    /// 设变通知列表（技术阶段一：④，发行通知至各部门）
    /// （子表：TaktEcNotification）
    /// </summary>
    public List<TaktEcNotificationDto>? Notifications { get; set; }

}

// ========================================
// EcGijutsu 查询 DTO
// ========================================

/// <summary>
/// EcGijutsu 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktEcGijutsuQueryDto : TaktPagedQuery
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
    /// 设变单号（唯一）
    /// </summary>
    public string? EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 发行日期（范围查询-开始）
    /// </summary>
    public DateTime? EcIssueDateStart { get; set; }

    /// <summary>
    /// 发行日期（范围查询-结束）
    /// </summary>
    public DateTime? EcIssueDateEnd { get; set; }

    /// <summary>
    /// 变更状态（字典 logistics_manufacturing_ec_status；1=工作的，2=取消的，3=发行的，4=P.P中变更的，5=固定的，6=挂起的，7=拒绝的）
    /// </summary>
    public int? ChangeStatus { get; set; }

    /// <summary>
    /// 设变标题
    /// </summary>
    public string? EcTitle { get; set; } = string.Empty;

    /// <summary>
    /// 设变内容（富文本 HTML）
    /// </summary>
    public string? EcContent { get; set; } = string.Empty;

    /// <summary>
    /// 设变担当（选项 TaktEcGroups/options?ecGroupCategory=1；DictValue=EcGroupCode，DictLabel=EcGroupName）
    /// </summary>
    public string? EcLeader { get; set; } = string.Empty;

    /// <summary>
    /// 损失金额
    /// </summary>
    public decimal? EcLossAmount { get; set; }

    /// <summary>
    /// 实施范围（字典 logistics_manufacturing_ec_scope_category；1=全仕向，2=部管，3=内部，4=技术）
    /// </summary>
    public int? EcScope { get; set; }

    /// <summary>
    /// 制二/制一/品管实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）。只记录这三课
    /// </summary>
    public int? EcDeptScope { get; set; }

    /// <summary>
    /// 录入日期（范围查询-开始）
    /// </summary>
    public DateTime? EcEntryDateStart { get; set; }

    /// <summary>
    /// 录入日期（范围查询-结束）
    /// </summary>
    public DateTime? EcEntryDateEnd { get; set; }

    /// <summary>
    /// 设变状态（字典 logistics_manufacturing_ec_gijutsu_status；1=发行，2=执行中，3=完成）
    /// </summary>
    public int? EcStatus { get; set; }

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

    /// <summary>
    /// 执行部门编码。传入时只返回该部门执行表中仍有未作废行的设变主表。
    /// </summary>
    public string? ExecDeptCode { get; set; }
}

// ========================================
// 创建EcGijutsu DTO
// ========================================

/// <summary>
/// 创建EcGijutsu DTO
/// </summary>
public class TaktEcGijutsuCreateDto
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
    /// 设变单号（唯一）
    /// </summary>
    [Required(ErrorMessage = "设变单号（唯一）不能为空")]
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 发行日期
    /// </summary>
    public DateTime EcIssueDate { get; set; }

    /// <summary>
    /// 变更状态（字典 logistics_manufacturing_ec_status；1=工作的，2=取消的，3=发行的，4=P.P中变更的，5=固定的，6=挂起的，7=拒绝的；默认 5=固定的，表示设变将被实施）
    /// </summary>
    public int ChangeStatus { get; set; } = 5;

    /// <summary>
    /// 设变标题
    /// </summary>
    [Required(ErrorMessage = "设变标题不能为空")]
    public string EcTitle { get; set; } = string.Empty;

    /// <summary>
    /// 设变内容（富文本 HTML）
    /// </summary>
    [Required(ErrorMessage = "设变内容不能为空")]
    public string EcContent { get; set; } = string.Empty;

    /// <summary>
    /// 设变担当（选项 TaktEcGroups/options?ecGroupCategory=1；DictValue=EcGroupCode，DictLabel=EcGroupName）
    /// </summary>
    [Required(ErrorMessage = "设变担当（选项 TaktEcGroups/options?ecGroupCategory=1；DictValue=EcGroupCode，DictLabel=EcGroupName）不能为空")]
    public string EcLeader { get; set; } = string.Empty;

    /// <summary>
    /// 损失金额
    /// </summary>
    public decimal EcLossAmount { get; set; }

    /// <summary>
    /// 实施范围（字典 logistics_manufacturing_ec_scope_category；1=全仕向，2=部管，3=内部，4=技术；默认 1=全仕向）
    /// </summary>
    public int EcScope { get; set; } = 1;

    /// <summary>
    /// 制二/制一/品管实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）。只记录这三课
    /// </summary>
    public int EcDeptScope { get; set; } = 2;

    /// <summary>
    /// 录入日期
    /// </summary>
    public DateTime EcEntryDate { get; set; }

    /// <summary>
    /// 设变状态（字典 logistics_manufacturing_ec_gijutsu_status；1=发行，2=执行中，3=完成；自动回写，客户端勿改）
    /// </summary>
    public int EcStatus { get; set; } = 0;

    /// <summary>
    /// 设变明细列表（技术阶段一：③，BOM/料号变更行）（子表，级联保存）
    /// </summary>
    public List<TaktEcDetailCreateDto>? EcDetails { get; set; }

    /// <summary>
    /// 来源设变主表 ID（string）。大明细时草稿可不带 EcDetails，落库时由服务端按此 ID 从 TaktSourceEcDetail 物化。
    /// </summary>
    public string? SourceEcId { get; set; }

    /// <summary>
    /// 草稿是否省略明细（true 时 EcDetails 为空，须带 SourceEcId 提交）
    /// </summary>
    public bool DetailsDeferred { get; set; }

    /// <summary>
    /// 省略明细时的来源明细行数（仅提示；非持久化）
    /// </summary>
    public int DeferredDetailCount { get; set; }

    /// <summary>
    /// 设变附件列表（技术阶段一：②，联络/EPP/FPP 等文档）（子表，级联保存）
    /// </summary>
    public List<TaktEcAttachmentCreateDto>? Attachments { get; set; }

    /// <summary>
    /// 设变通知列表（技术阶段一：④，发行通知至各部门）（子表，级联保存）
    /// </summary>
    public List<TaktEcNotificationCreateDto>? Notifications { get; set; }

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
// 更新EcGijutsu DTO
// ========================================

/// <summary>
/// 更新EcGijutsu DTO
/// 继承 TaktEcGijutsuCreateDto，添加 EcGijutsuId 字段
/// </summary>
public class TaktEcGijutsuUpdateDto : TaktEcGijutsuCreateDto
{
    /// <summary>
    /// EcGijutsuID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcGijutsuId { get; set; }

    /// <summary>
    /// 设变明细列表（技术阶段一：③，BOM/料号变更行）（子表，级联保存）
    /// </summary>
    public new List<TaktEcDetailUpdateDto>? EcDetails { get; set; }

    /// <summary>
    /// 设变附件列表（技术阶段一：②，联络/EPP/FPP 等文档）（子表，级联保存）
    /// </summary>
    public new List<TaktEcAttachmentUpdateDto>? Attachments { get; set; }

    /// <summary>
    /// 设变通知列表（技术阶段一：④，发行通知至各部门）（子表，级联保存）
    /// </summary>
    public new List<TaktEcNotificationUpdateDto>? Notifications { get; set; }

}

// ========================================
// EcGijutsu 状态 DTO
// ========================================

/// <summary>
/// EcGijutsu 状态更新 DTO
/// </summary>
public class TaktEcGijutsuStatusDto
{
    /// <summary>
    /// EcGijutsuID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcGijutsuId { get; set; }

    /// <summary>
    /// 变更状态（字典 logistics_manufacturing_ec_status；1=工作的，2=取消的，3=发行的，4=P.P中变更的，5=固定的，6=挂起的，7=拒绝的）
    /// </summary>
    [Required(ErrorMessage = "变更状态（字典 logistics_manufacturing_ec_status；1=工作的，2=取消的，3=发行的，4=P.P中变更的，5=固定的，6=挂起的，7=拒绝的）不能为空")]
    public int ChangeStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// EcGijutsu 导入模板行 DTO
/// </summary>
public class TaktEcGijutsuTemplateDto
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
    /// 设变单号（唯一）
    /// </summary>
    public string? EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 发行日期
    /// </summary>
    public DateTime? EcIssueDate { get; set; }

    /// <summary>
    /// 变更状态（字典 logistics_manufacturing_ec_status；1=工作的，2=取消的，3=发行的，4=P.P中变更的，5=固定的，6=挂起的，7=拒绝的）
    /// </summary>
    public int? ChangeStatus { get; set; }

    /// <summary>
    /// 设变标题
    /// </summary>
    public string? EcTitle { get; set; } = string.Empty;

    /// <summary>
    /// 设变内容（富文本 HTML）
    /// </summary>
    public string? EcContent { get; set; } = string.Empty;

    /// <summary>
    /// 设变担当（选项 TaktEcGroups/options?ecGroupCategory=1；DictValue=EcGroupCode，DictLabel=EcGroupName）
    /// </summary>
    public string? EcLeader { get; set; } = string.Empty;

    /// <summary>
    /// 损失金额
    /// </summary>
    public decimal? EcLossAmount { get; set; }

    /// <summary>
    /// 实施范围（字典 logistics_manufacturing_ec_scope_category；1=全仕向，2=部管，3=内部，4=技术）
    /// </summary>
    public int? EcScope { get; set; }

    /// <summary>
    /// 制二/制一/品管实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）。只记录这三课
    /// </summary>
    public int? EcDeptScope { get; set; }

    /// <summary>
    /// 录入日期
    /// </summary>
    public DateTime? EcEntryDate { get; set; }

    /// <summary>
    /// 设变状态（字典 logistics_manufacturing_ec_gijutsu_status；1=发行，2=执行中，3=完成）
    /// </summary>
    public int? EcStatus { get; set; }

    /// <summary>
    /// 设变明细列表（技术阶段一：③，BOM/料号变更行）（子表，级联保存）
    /// </summary>
    public List<TaktEcDetailCreateDto>? EcDetails { get; set; }

    /// <summary>
    /// 设变附件列表（技术阶段一：②，联络/EPP/FPP 等文档）（子表，级联保存）
    /// </summary>
    public List<TaktEcAttachmentCreateDto>? Attachments { get; set; }

    /// <summary>
    /// 设变通知列表（技术阶段一：④，发行通知至各部门）（子表，级联保存）
    /// </summary>
    public List<TaktEcNotificationCreateDto>? Notifications { get; set; }

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
/// EcGijutsu 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktEcGijutsuImportDto
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
    /// 设变单号（唯一）
    /// </summary>
    public string? EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 发行日期
    /// </summary>
    public DateTime? EcIssueDate { get; set; }

    /// <summary>
    /// 变更状态（字典 logistics_manufacturing_ec_status；1=工作的，2=取消的，3=发行的，4=P.P中变更的，5=固定的，6=挂起的，7=拒绝的）
    /// </summary>
    public int? ChangeStatus { get; set; }

    /// <summary>
    /// 设变标题
    /// </summary>
    public string? EcTitle { get; set; } = string.Empty;

    /// <summary>
    /// 设变内容（富文本 HTML）
    /// </summary>
    public string? EcContent { get; set; } = string.Empty;

    /// <summary>
    /// 设变担当（选项 TaktEcGroups/options?ecGroupCategory=1；DictValue=EcGroupCode，DictLabel=EcGroupName）
    /// </summary>
    public string? EcLeader { get; set; } = string.Empty;

    /// <summary>
    /// 损失金额
    /// </summary>
    public decimal? EcLossAmount { get; set; }

    /// <summary>
    /// 实施范围（字典 logistics_manufacturing_ec_scope_category；1=全仕向，2=部管，3=内部，4=技术）
    /// </summary>
    public int? EcScope { get; set; }

    /// <summary>
    /// 制二/制一/品管实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）。只记录这三课
    /// </summary>
    public int? EcDeptScope { get; set; }

    /// <summary>
    /// 录入日期
    /// </summary>
    public DateTime? EcEntryDate { get; set; }

    /// <summary>
    /// 设变状态（字典 logistics_manufacturing_ec_gijutsu_status；1=发行，2=执行中，3=完成）
    /// </summary>
    public int? EcStatus { get; set; }

    /// <summary>
    /// 设变明细列表（技术阶段一：③，BOM/料号变更行）（子表，级联保存）
    /// </summary>
    public List<TaktEcDetailCreateDto>? EcDetails { get; set; }

    /// <summary>
    /// 设变附件列表（技术阶段一：②，联络/EPP/FPP 等文档）（子表，级联保存）
    /// </summary>
    public List<TaktEcAttachmentCreateDto>? Attachments { get; set; }

    /// <summary>
    /// 设变通知列表（技术阶段一：④，发行通知至各部门）（子表，级联保存）
    /// </summary>
    public List<TaktEcNotificationCreateDto>? Notifications { get; set; }

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
/// EcGijutsu 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktEcGijutsuExportDto
{
    /// <summary>
    /// EcGijutsuID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcGijutsuId { get; set; }

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
    /// 设变单号（唯一）
    /// </summary>
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 发行日期
    /// </summary>
    public DateTime EcIssueDate { get; set; }

    /// <summary>
    /// 变更状态（字典 logistics_manufacturing_ec_status；1=工作的，2=取消的，3=发行的，4=P.P中变更的，5=固定的，6=挂起的，7=拒绝的；默认 5=固定的，表示设变将被实施）
    /// </summary>
    public int ChangeStatus { get; set; } = 5;

    /// <summary>
    /// 设变标题
    /// </summary>
    public string EcTitle { get; set; } = string.Empty;

    /// <summary>
    /// 设变内容（富文本 HTML）
    /// </summary>
    public string EcContent { get; set; } = string.Empty;

    /// <summary>
    /// 设变担当（选项 TaktEcGroups/options?ecGroupCategory=1；DictValue=EcGroupCode，DictLabel=EcGroupName）
    /// </summary>
    public string EcLeader { get; set; } = string.Empty;

    /// <summary>
    /// 损失金额
    /// </summary>
    public decimal EcLossAmount { get; set; }

    /// <summary>
    /// 实施范围（字典 logistics_manufacturing_ec_scope_category；1=全仕向，2=部管，3=内部，4=技术）
    /// </summary>
    public int EcScope { get; set; } = 0;

    /// <summary>
    /// 制二/制一/品管实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）。只记录这三课
    /// </summary>
    public int EcDeptScope { get; set; } = 2;

    /// <summary>
    /// 录入日期
    /// </summary>
    public DateTime EcEntryDate { get; set; }

    /// <summary>
    /// 设变状态（字典 logistics_manufacturing_ec_gijutsu_status；1=发行，2=执行中，3=完成；自动回写，客户端勿改）
    /// </summary>
    public int EcStatus { get; set; } = 0;

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

/// <summary>
/// 设变技术课主表后台保存已提交回执（Create/Update 入队后立即返回）
/// </summary>
public class TaktEcGijutsuSubmittedDto
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 设变单号
    /// </summary>
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 是否为更新（false=新增）
    /// </summary>
    public bool IsUpdate { get; set; }

    /// <summary>
    /// 明细行数（提交时快照，供提示文案）
    /// </summary>
    public int DetailCount { get; set; }

    /// <summary>
    /// 更新时的主表主键（新增为 0）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcGijutsuId { get; set; }
}
