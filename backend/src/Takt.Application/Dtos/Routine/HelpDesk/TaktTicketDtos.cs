// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Routine.HelpDesk
// 文件名称：TaktTicketDtos.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Auto Generated)
// 功能描述：Ticket 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktTicket 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Routine.HelpDesk;

// ========================================
// Ticket 响应 DTO
// ========================================

/// <summary>
/// 服务台工单实体
/// 对应前端 TaktTicketDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktTicketDto : TaktCompanyDtoBase
{
    /// <summary>
    /// TicketID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TicketId { get; set; }

    /// <summary>
    /// 工单编码（唯一）
    /// </summary>
    public string TicketCode { get; set; } = string.Empty;

    /// <summary>
    /// 工单标题
    /// </summary>
    public string TicketTitle { get; set; } = string.Empty;

    /// <summary>
    /// 工单内容描述
    /// </summary>
    public string? TicketContent { get; set; } = string.Empty;

    /// <summary>
    /// 附件（JSON 列表形式，由 TaktFile 统一上传到服务器）
    /// </summary>
    public string? attachments { get; set; } = string.Empty;

    /// <summary>
    /// 优先级（字典 sys_priority_level；1=最高 2=高 3=普通 4=低）
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// 紧急度（字典 sys_urgency_level；1=高 2=中 3=低）
    /// </summary>
    public int Urgency { get; set; } = 0;

    /// <summary>
    /// 影响范围（字典 sys_impact_level；1=高 2=中 3=低）
    /// </summary>
    public int Impact { get; set; } = 0;

    /// <summary>
    /// 分类编码（业务编码；与 TaktTicketCategoryAssign.CategoryCode 一致）
    /// </summary>
    public string? CategoryCode { get; set; } = string.Empty;

    /// <summary>
    /// 工单来源（字典 routine_help_desk_ticket_source；0=门户 1=邮件 2=电话 3=API）
    /// </summary>
    public int TicketSource { get; set; } = 0;

    /// <summary>
    /// 提交人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SubmitterId { get; set; }

    /// <summary>
    /// 提交人姓名（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? SubmitterName { get; set; } = string.Empty;

    /// <summary>
    /// 处理人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssigneeId { get; set; }

    /// <summary>
    /// 处理人姓名（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? AssigneeName { get; set; } = string.Empty;

    /// <summary>
    /// 关联知识 ID（选项 TaktKnowledges/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? KnowledgeId { get; set; }

    /// <summary>
    /// 关联知识 名称（填充字段）
    /// </summary>
    public string? KnowledgeName { get; set; }

    /// <summary>
    /// 父工单 ID（选项 TaktTickets/options；为空表示顶级工单，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentTicketId { get; set; }

    /// <summary>
    /// 父工单 名称（填充字段）
    /// </summary>
    public string? ParentTicketName { get; set; }

    /// <summary>
    /// 首次响应时间（支持人员首次回复用户的时间，SLA/OLA 时间追踪）
    /// </summary>
    public DateTime? FirstResponseAt { get; set; }

    /// <summary>
    /// 首次响应期限（根据 SLA 计算出的首次响应截止时间）
    /// </summary>
    public DateTime? FirstResponseDueBy { get; set; }

    /// <summary>
    /// 解决时间（问题被标记为已解决的时间）
    /// </summary>
    public DateTime? ResolvedAt { get; set; }

    /// <summary>
    /// 解决期限（根据 SLA 计算出的解决截止时间）
    /// </summary>
    public DateTime? ResolutionDueBy { get; set; }

    /// <summary>
    /// 关闭时间（工单最终关闭的时间）
    /// </summary>
    public DateTime? ClosedAt { get; set; }

    /// <summary>
    /// IT 设备保修扩展 ID（选项 TaktItAssets/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ItAssetId { get; set; }

    /// <summary>
    /// IT 设备保修扩展 名称（填充字段）
    /// </summary>
    public string? ItAssetName { get; set; }

    /// <summary>
    /// 资产号码（冗余字段，便于查询；与 TaktItAsset.AssetCode 一致）
    /// </summary>
    public string? AssetCode { get; set; } = string.Empty;

    /// <summary>
    /// 申请部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApplicantDeptId { get; set; }

    /// <summary>
    /// 申请部门名称（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? ApplicantDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 申请人 ID（选项 TaktUsers/options；代理人代提时填被代理人，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApplicantBy { get; set; }

    /// <summary>
    /// 申请人名称（冗余：按 ApplicantBy 取 TaktUser.UserName 联动）
    /// </summary>
    public string? ApplicantName { get; set; } = string.Empty;

    /// <summary>
    /// 工单状态（字典 sys_ticket_status；0=新建 1=已分配 2=处理中 3=待确认 4=已完成 5=已关闭 6=已取消 7=重新打开）
    /// </summary>
    public int TicketStatus { get; set; } = 0;

    /// <summary>
    /// 子工单列表（父工单时有效；外键：本表 Id = 子工单 ParentTicketId）
    /// （子表：TaktTicket）
    /// </summary>
    public List<TaktTicketDto>? ChildTickets { get; set; }

}

// ========================================
// Ticket 查询 DTO
// ========================================

/// <summary>
/// Ticket 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktTicketQueryDto : TaktPagedQuery
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
    /// 工单编码（唯一）
    /// </summary>
    public string? TicketCode { get; set; } = string.Empty;

    /// <summary>
    /// 工单标题
    /// </summary>
    public string? TicketTitle { get; set; } = string.Empty;

    /// <summary>
    /// 工单内容描述
    /// </summary>
    public string? TicketContent { get; set; } = string.Empty;

    /// <summary>
    /// 附件（JSON 列表形式，由 TaktFile 统一上传到服务器）
    /// </summary>
    public string? attachments { get; set; } = string.Empty;

    /// <summary>
    /// 优先级（字典 sys_priority_level；1=最高 2=高 3=普通 4=低）
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// 紧急度（字典 sys_urgency_level；1=高 2=中 3=低）
    /// </summary>
    public int? Urgency { get; set; }

    /// <summary>
    /// 影响范围（字典 sys_impact_level；1=高 2=中 3=低）
    /// </summary>
    public int? Impact { get; set; }

    /// <summary>
    /// 分类编码（业务编码；与 TaktTicketCategoryAssign.CategoryCode 一致）
    /// </summary>
    public string? CategoryCode { get; set; } = string.Empty;

    /// <summary>
    /// 工单来源（字典 routine_help_desk_ticket_source；0=门户 1=邮件 2=电话 3=API）
    /// </summary>
    public int? TicketSource { get; set; }

    /// <summary>
    /// 提交人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SubmitterId { get; set; }

    /// <summary>
    /// 提交人姓名（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? SubmitterName { get; set; } = string.Empty;

    /// <summary>
    /// 处理人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssigneeId { get; set; }

    /// <summary>
    /// 处理人姓名（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? AssigneeName { get; set; } = string.Empty;

    /// <summary>
    /// 关联知识 ID（选项 TaktKnowledges/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? KnowledgeId { get; set; }

    /// <summary>
    /// 父工单 ID（选项 TaktTickets/options；为空表示顶级工单，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentTicketId { get; set; }

    /// <summary>
    /// 首次响应时间（支持人员首次回复用户的时间，SLA/OLA 时间追踪）（范围查询-开始）
    /// </summary>
    public DateTime? FirstResponseAtStart { get; set; }

    /// <summary>
    /// 首次响应时间（支持人员首次回复用户的时间，SLA/OLA 时间追踪）（范围查询-结束）
    /// </summary>
    public DateTime? FirstResponseAtEnd { get; set; }

    /// <summary>
    /// 首次响应期限（根据 SLA 计算出的首次响应截止时间）（范围查询-开始）
    /// </summary>
    public DateTime? FirstResponseDueByStart { get; set; }

    /// <summary>
    /// 首次响应期限（根据 SLA 计算出的首次响应截止时间）（范围查询-结束）
    /// </summary>
    public DateTime? FirstResponseDueByEnd { get; set; }

    /// <summary>
    /// 解决时间（问题被标记为已解决的时间）（范围查询-开始）
    /// </summary>
    public DateTime? ResolvedAtStart { get; set; }

    /// <summary>
    /// 解决时间（问题被标记为已解决的时间）（范围查询-结束）
    /// </summary>
    public DateTime? ResolvedAtEnd { get; set; }

    /// <summary>
    /// 解决期限（根据 SLA 计算出的解决截止时间）（范围查询-开始）
    /// </summary>
    public DateTime? ResolutionDueByStart { get; set; }

    /// <summary>
    /// 解决期限（根据 SLA 计算出的解决截止时间）（范围查询-结束）
    /// </summary>
    public DateTime? ResolutionDueByEnd { get; set; }

    /// <summary>
    /// 关闭时间（工单最终关闭的时间）（范围查询-开始）
    /// </summary>
    public DateTime? ClosedAtStart { get; set; }

    /// <summary>
    /// 关闭时间（工单最终关闭的时间）（范围查询-结束）
    /// </summary>
    public DateTime? ClosedAtEnd { get; set; }

    /// <summary>
    /// IT 设备保修扩展 ID（选项 TaktItAssets/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ItAssetId { get; set; }

    /// <summary>
    /// 资产号码（冗余字段，便于查询；与 TaktItAsset.AssetCode 一致）
    /// </summary>
    public string? AssetCode { get; set; } = string.Empty;

    /// <summary>
    /// 申请部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApplicantDeptId { get; set; }

    /// <summary>
    /// 申请部门名称（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? ApplicantDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 申请人 ID（选项 TaktUsers/options；代理人代提时填被代理人，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApplicantBy { get; set; }

    /// <summary>
    /// 申请人名称（冗余：按 ApplicantBy 取 TaktUser.UserName 联动）
    /// </summary>
    public string? ApplicantName { get; set; } = string.Empty;

    /// <summary>
    /// 工单状态（字典 sys_ticket_status；0=新建 1=已分配 2=处理中 3=待确认 4=已完成 5=已关闭 6=已取消 7=重新打开）
    /// </summary>
    public int? TicketStatus { get; set; }

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
// 创建Ticket DTO
// ========================================

/// <summary>
/// 创建Ticket DTO
/// </summary>
public class TaktTicketCreateDto
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
    /// 工单编码（唯一）
    /// </summary>
    [Required(ErrorMessage = "工单编码（唯一）不能为空")]
    public string TicketCode { get; set; } = string.Empty;

    /// <summary>
    /// 工单标题
    /// </summary>
    [Required(ErrorMessage = "工单标题不能为空")]
    public string TicketTitle { get; set; } = string.Empty;

    /// <summary>
    /// 工单内容描述
    /// </summary>
    public string? TicketContent { get; set; } = string.Empty;

    /// <summary>
    /// 附件（JSON 列表形式，由 TaktFile 统一上传到服务器）
    /// </summary>
    public string? attachments { get; set; } = string.Empty;

    /// <summary>
    /// 优先级（字典 sys_priority_level；1=最高 2=高 3=普通 4=低）
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// 紧急度（字典 sys_urgency_level；1=高 2=中 3=低）
    /// </summary>
    public int Urgency { get; set; } = 0;

    /// <summary>
    /// 影响范围（字典 sys_impact_level；1=高 2=中 3=低）
    /// </summary>
    public int Impact { get; set; } = 0;

    /// <summary>
    /// 分类编码（业务编码；与 TaktTicketCategoryAssign.CategoryCode 一致）
    /// </summary>
    public string? CategoryCode { get; set; } = string.Empty;

    /// <summary>
    /// 工单来源（字典 routine_help_desk_ticket_source；0=门户 1=邮件 2=电话 3=API）
    /// </summary>
    public int TicketSource { get; set; } = 0;

    /// <summary>
    /// 提交人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SubmitterId { get; set; }

    /// <summary>
    /// 提交人姓名（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? SubmitterName { get; set; } = string.Empty;

    /// <summary>
    /// 处理人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssigneeId { get; set; }

    /// <summary>
    /// 处理人姓名（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? AssigneeName { get; set; } = string.Empty;

    /// <summary>
    /// 关联知识 ID（选项 TaktKnowledges/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? KnowledgeId { get; set; }

    /// <summary>
    /// 父工单 ID（选项 TaktTickets/options；为空表示顶级工单，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentTicketId { get; set; }

    /// <summary>
    /// 首次响应时间（支持人员首次回复用户的时间，SLA/OLA 时间追踪）
    /// </summary>
    public DateTime? FirstResponseAt { get; set; }

    /// <summary>
    /// 首次响应期限（根据 SLA 计算出的首次响应截止时间）
    /// </summary>
    public DateTime? FirstResponseDueBy { get; set; }

    /// <summary>
    /// 解决时间（问题被标记为已解决的时间）
    /// </summary>
    public DateTime? ResolvedAt { get; set; }

    /// <summary>
    /// 解决期限（根据 SLA 计算出的解决截止时间）
    /// </summary>
    public DateTime? ResolutionDueBy { get; set; }

    /// <summary>
    /// 关闭时间（工单最终关闭的时间）
    /// </summary>
    public DateTime? ClosedAt { get; set; }

    /// <summary>
    /// IT 设备保修扩展 ID（选项 TaktItAssets/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ItAssetId { get; set; }

    /// <summary>
    /// 资产号码（冗余字段，便于查询；与 TaktItAsset.AssetCode 一致）
    /// </summary>
    public string? AssetCode { get; set; } = string.Empty;

    /// <summary>
    /// 申请部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApplicantDeptId { get; set; }

    /// <summary>
    /// 申请部门名称（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? ApplicantDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 申请人 ID（选项 TaktUsers/options；代理人代提时填被代理人，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApplicantBy { get; set; }

    /// <summary>
    /// 申请人名称（冗余：按 ApplicantBy 取 TaktUser.UserName 联动）
    /// </summary>
    public string? ApplicantName { get; set; } = string.Empty;

    /// <summary>
    /// 工单状态（字典 sys_ticket_status；0=新建 1=已分配 2=处理中 3=待确认 4=已完成 5=已关闭 6=已取消 7=重新打开）
    /// </summary>
    public int TicketStatus { get; set; } = 0;

    /// <summary>
    /// 子工单列表（父工单时有效；外键：本表 Id = 子工单 ParentTicketId）（子表，级联保存）
    /// </summary>
    public List<TaktTicketCreateDto>? ChildTickets { get; set; }

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
// 更新Ticket DTO
// ========================================

/// <summary>
/// 更新Ticket DTO
/// 继承 TaktTicketCreateDto，添加 TicketId 字段
/// </summary>
public class TaktTicketUpdateDto : TaktTicketCreateDto
{
    /// <summary>
    /// TicketID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TicketId { get; set; }

    /// <summary>
    /// 子工单列表（父工单时有效；外键：本表 Id = 子工单 ParentTicketId）（子表，级联保存）
    /// </summary>
    public new List<TaktTicketUpdateDto>? ChildTickets { get; set; }

}

// ========================================
// Ticket 状态 DTO
// ========================================

/// <summary>
/// Ticket 状态更新 DTO
/// </summary>
public class TaktTicketStatusDto
{
    /// <summary>
    /// TicketID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TicketId { get; set; }

    /// <summary>
    /// 工单状态（字典 sys_ticket_status；0=新建 1=已分配 2=处理中 3=待确认 4=已完成 5=已关闭 6=已取消 7=重新打开）
    /// </summary>
    [Required(ErrorMessage = "工单状态（字典 sys_ticket_status；0=新建 1=已分配 2=处理中 3=待确认 4=已完成 5=已关闭 6=已取消 7=重新打开）不能为空")]
    public int TicketStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Ticket 导入模板行 DTO
/// </summary>
public class TaktTicketTemplateDto
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
    /// 工单编码（唯一）
    /// </summary>
    public string? TicketCode { get; set; } = string.Empty;

    /// <summary>
    /// 工单标题
    /// </summary>
    public string? TicketTitle { get; set; } = string.Empty;

    /// <summary>
    /// 工单内容描述
    /// </summary>
    public string? TicketContent { get; set; } = string.Empty;

    /// <summary>
    /// 附件（JSON 列表形式，由 TaktFile 统一上传到服务器）
    /// </summary>
    public string? attachments { get; set; } = string.Empty;

    /// <summary>
    /// 优先级（字典 sys_priority_level；1=最高 2=高 3=普通 4=低）
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// 紧急度（字典 sys_urgency_level；1=高 2=中 3=低）
    /// </summary>
    public int? Urgency { get; set; }

    /// <summary>
    /// 影响范围（字典 sys_impact_level；1=高 2=中 3=低）
    /// </summary>
    public int? Impact { get; set; }

    /// <summary>
    /// 分类编码（业务编码；与 TaktTicketCategoryAssign.CategoryCode 一致）
    /// </summary>
    public string? CategoryCode { get; set; } = string.Empty;

    /// <summary>
    /// 工单来源（字典 routine_help_desk_ticket_source；0=门户 1=邮件 2=电话 3=API）
    /// </summary>
    public int? TicketSource { get; set; }

    /// <summary>
    /// 提交人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SubmitterId { get; set; }

    /// <summary>
    /// 提交人姓名（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? SubmitterName { get; set; } = string.Empty;

    /// <summary>
    /// 处理人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssigneeId { get; set; }

    /// <summary>
    /// 处理人姓名（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? AssigneeName { get; set; } = string.Empty;

    /// <summary>
    /// 关联知识 ID（选项 TaktKnowledges/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? KnowledgeId { get; set; }

    /// <summary>
    /// 父工单 ID（选项 TaktTickets/options；为空表示顶级工单，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentTicketId { get; set; }

    /// <summary>
    /// 首次响应时间（支持人员首次回复用户的时间，SLA/OLA 时间追踪）
    /// </summary>
    public DateTime? FirstResponseAt { get; set; }

    /// <summary>
    /// 首次响应期限（根据 SLA 计算出的首次响应截止时间）
    /// </summary>
    public DateTime? FirstResponseDueBy { get; set; }

    /// <summary>
    /// 解决时间（问题被标记为已解决的时间）
    /// </summary>
    public DateTime? ResolvedAt { get; set; }

    /// <summary>
    /// 解决期限（根据 SLA 计算出的解决截止时间）
    /// </summary>
    public DateTime? ResolutionDueBy { get; set; }

    /// <summary>
    /// 关闭时间（工单最终关闭的时间）
    /// </summary>
    public DateTime? ClosedAt { get; set; }

    /// <summary>
    /// IT 设备保修扩展 ID（选项 TaktItAssets/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ItAssetId { get; set; }

    /// <summary>
    /// 资产号码（冗余字段，便于查询；与 TaktItAsset.AssetCode 一致）
    /// </summary>
    public string? AssetCode { get; set; } = string.Empty;

    /// <summary>
    /// 申请部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApplicantDeptId { get; set; }

    /// <summary>
    /// 申请部门名称（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? ApplicantDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 申请人 ID（选项 TaktUsers/options；代理人代提时填被代理人，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApplicantBy { get; set; }

    /// <summary>
    /// 申请人名称（冗余：按 ApplicantBy 取 TaktUser.UserName 联动）
    /// </summary>
    public string? ApplicantName { get; set; } = string.Empty;

    /// <summary>
    /// 工单状态（字典 sys_ticket_status；0=新建 1=已分配 2=处理中 3=待确认 4=已完成 5=已关闭 6=已取消 7=重新打开）
    /// </summary>
    public int? TicketStatus { get; set; }

    /// <summary>
    /// 子工单列表（父工单时有效；外键：本表 Id = 子工单 ParentTicketId）（子表，级联保存）
    /// </summary>
    public List<TaktTicketCreateDto>? ChildTickets { get; set; }

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
/// Ticket 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktTicketImportDto
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
    /// 工单编码（唯一）
    /// </summary>
    public string? TicketCode { get; set; } = string.Empty;

    /// <summary>
    /// 工单标题
    /// </summary>
    public string? TicketTitle { get; set; } = string.Empty;

    /// <summary>
    /// 工单内容描述
    /// </summary>
    public string? TicketContent { get; set; } = string.Empty;

    /// <summary>
    /// 附件（JSON 列表形式，由 TaktFile 统一上传到服务器）
    /// </summary>
    public string? attachments { get; set; } = string.Empty;

    /// <summary>
    /// 优先级（字典 sys_priority_level；1=最高 2=高 3=普通 4=低）
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// 紧急度（字典 sys_urgency_level；1=高 2=中 3=低）
    /// </summary>
    public int? Urgency { get; set; }

    /// <summary>
    /// 影响范围（字典 sys_impact_level；1=高 2=中 3=低）
    /// </summary>
    public int? Impact { get; set; }

    /// <summary>
    /// 分类编码（业务编码；与 TaktTicketCategoryAssign.CategoryCode 一致）
    /// </summary>
    public string? CategoryCode { get; set; } = string.Empty;

    /// <summary>
    /// 工单来源（字典 routine_help_desk_ticket_source；0=门户 1=邮件 2=电话 3=API）
    /// </summary>
    public int? TicketSource { get; set; }

    /// <summary>
    /// 提交人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SubmitterId { get; set; }

    /// <summary>
    /// 提交人姓名（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? SubmitterName { get; set; } = string.Empty;

    /// <summary>
    /// 处理人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssigneeId { get; set; }

    /// <summary>
    /// 处理人姓名（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? AssigneeName { get; set; } = string.Empty;

    /// <summary>
    /// 关联知识 ID（选项 TaktKnowledges/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? KnowledgeId { get; set; }

    /// <summary>
    /// 父工单 ID（选项 TaktTickets/options；为空表示顶级工单，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentTicketId { get; set; }

    /// <summary>
    /// 首次响应时间（支持人员首次回复用户的时间，SLA/OLA 时间追踪）
    /// </summary>
    public DateTime? FirstResponseAt { get; set; }

    /// <summary>
    /// 首次响应期限（根据 SLA 计算出的首次响应截止时间）
    /// </summary>
    public DateTime? FirstResponseDueBy { get; set; }

    /// <summary>
    /// 解决时间（问题被标记为已解决的时间）
    /// </summary>
    public DateTime? ResolvedAt { get; set; }

    /// <summary>
    /// 解决期限（根据 SLA 计算出的解决截止时间）
    /// </summary>
    public DateTime? ResolutionDueBy { get; set; }

    /// <summary>
    /// 关闭时间（工单最终关闭的时间）
    /// </summary>
    public DateTime? ClosedAt { get; set; }

    /// <summary>
    /// IT 设备保修扩展 ID（选项 TaktItAssets/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ItAssetId { get; set; }

    /// <summary>
    /// 资产号码（冗余字段，便于查询；与 TaktItAsset.AssetCode 一致）
    /// </summary>
    public string? AssetCode { get; set; } = string.Empty;

    /// <summary>
    /// 申请部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApplicantDeptId { get; set; }

    /// <summary>
    /// 申请部门名称（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? ApplicantDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 申请人 ID（选项 TaktUsers/options；代理人代提时填被代理人，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApplicantBy { get; set; }

    /// <summary>
    /// 申请人名称（冗余：按 ApplicantBy 取 TaktUser.UserName 联动）
    /// </summary>
    public string? ApplicantName { get; set; } = string.Empty;

    /// <summary>
    /// 工单状态（字典 sys_ticket_status；0=新建 1=已分配 2=处理中 3=待确认 4=已完成 5=已关闭 6=已取消 7=重新打开）
    /// </summary>
    public int? TicketStatus { get; set; }

    /// <summary>
    /// 子工单列表（父工单时有效；外键：本表 Id = 子工单 ParentTicketId）（子表，级联保存）
    /// </summary>
    public List<TaktTicketCreateDto>? ChildTickets { get; set; }

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
/// Ticket 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktTicketExportDto
{
    /// <summary>
    /// TicketID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TicketId { get; set; }

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
    /// 工单编码（唯一）
    /// </summary>
    public string TicketCode { get; set; } = string.Empty;

    /// <summary>
    /// 工单标题
    /// </summary>
    public string TicketTitle { get; set; } = string.Empty;

    /// <summary>
    /// 工单内容描述
    /// </summary>
    public string? TicketContent { get; set; } = string.Empty;

    /// <summary>
    /// 附件（JSON 列表形式，由 TaktFile 统一上传到服务器）
    /// </summary>
    public string? attachments { get; set; } = string.Empty;

    /// <summary>
    /// 优先级（字典 sys_priority_level；1=最高 2=高 3=普通 4=低）
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// 紧急度（字典 sys_urgency_level；1=高 2=中 3=低）
    /// </summary>
    public int Urgency { get; set; } = 0;

    /// <summary>
    /// 影响范围（字典 sys_impact_level；1=高 2=中 3=低）
    /// </summary>
    public int Impact { get; set; } = 0;

    /// <summary>
    /// 分类编码（业务编码；与 TaktTicketCategoryAssign.CategoryCode 一致）
    /// </summary>
    public string? CategoryCode { get; set; } = string.Empty;

    /// <summary>
    /// 工单来源（字典 routine_help_desk_ticket_source；0=门户 1=邮件 2=电话 3=API）
    /// </summary>
    public int TicketSource { get; set; } = 0;

    /// <summary>
    /// 提交人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SubmitterId { get; set; }

    /// <summary>
    /// 提交人姓名（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? SubmitterName { get; set; } = string.Empty;

    /// <summary>
    /// 处理人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssigneeId { get; set; }

    /// <summary>
    /// 处理人姓名（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? AssigneeName { get; set; } = string.Empty;

    /// <summary>
    /// 关联知识 ID（选项 TaktKnowledges/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? KnowledgeId { get; set; }

    /// <summary>
    /// 父工单 ID（选项 TaktTickets/options；为空表示顶级工单，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentTicketId { get; set; }

    /// <summary>
    /// 首次响应时间（支持人员首次回复用户的时间，SLA/OLA 时间追踪）
    /// </summary>
    public DateTime? FirstResponseAt { get; set; }

    /// <summary>
    /// 首次响应期限（根据 SLA 计算出的首次响应截止时间）
    /// </summary>
    public DateTime? FirstResponseDueBy { get; set; }

    /// <summary>
    /// 解决时间（问题被标记为已解决的时间）
    /// </summary>
    public DateTime? ResolvedAt { get; set; }

    /// <summary>
    /// 解决期限（根据 SLA 计算出的解决截止时间）
    /// </summary>
    public DateTime? ResolutionDueBy { get; set; }

    /// <summary>
    /// 关闭时间（工单最终关闭的时间）
    /// </summary>
    public DateTime? ClosedAt { get; set; }

    /// <summary>
    /// IT 设备保修扩展 ID（选项 TaktItAssets/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ItAssetId { get; set; }

    /// <summary>
    /// 资产号码（冗余字段，便于查询；与 TaktItAsset.AssetCode 一致）
    /// </summary>
    public string? AssetCode { get; set; } = string.Empty;

    /// <summary>
    /// 申请部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApplicantDeptId { get; set; }

    /// <summary>
    /// 申请部门名称（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    public string? ApplicantDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 申请人 ID（选项 TaktUsers/options；代理人代提时填被代理人，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApplicantBy { get; set; }

    /// <summary>
    /// 申请人名称（冗余：按 ApplicantBy 取 TaktUser.UserName 联动）
    /// </summary>
    public string? ApplicantName { get; set; } = string.Empty;

    /// <summary>
    /// 工单状态（字典 sys_ticket_status；0=新建 1=已分配 2=处理中 3=待确认 4=已完成 5=已关闭 6=已取消 7=重新打开）
    /// </summary>
    public int TicketStatus { get; set; } = 0;

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
