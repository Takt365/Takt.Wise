// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Routine.Announcement
// 文件名称：TaktAnnouncementDtos.cs
// 创建时间：2026-09-11
// 创建人：Takt365(Auto Generated)
// 功能描述：Announcement 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktAnnouncement 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Routine.Announcement;

// ========================================
// Announcement 响应 DTO
// ========================================

/// <summary>
/// 公告通知实体 用于发布系统公告、通知等信息；支持富文本、附件、置顶、定时发布；需审批通过后发布 审批态见基类 ApprovalStatus，字典 sys_approval_status
/// 对应前端 TaktAnnouncementDto
/// 继承 TaktApprovalDtoBase
/// </summary>
public class TaktAnnouncementDto : TaktApprovalDtoBase
{
    /// <summary>
    /// AnnouncementID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AnnouncementId { get; set; }

    /// <summary>
    /// 公告编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 公告编码规则生成并展示，非手输；单据类型菜单：公告通知）
    /// </summary>
    public string AnnouncementCode { get; set; } = string.Empty;

    /// <summary>
    /// 公告标题
    /// </summary>
    public string AnnouncementTitle { get; set; } = string.Empty;

    /// <summary>
    /// 公告类型（字典 sys_announcement_category；1=紧急通知 2=公告 3=通知 4=决议 5=活动 6=安全通告 7=运维通知 8=系统公告）
    /// </summary>
    public int AnnouncementType { get; set; } = 0;

    /// <summary>
    /// 公告内容（富文本 HTML）
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 公告摘要（用于列表展示）
    /// </summary>
    public string? Summary { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? Tags { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 发布时间（定时发布时使用）
    /// </summary>
    public DateTime? PublishTime { get; set; }

    /// <summary>
    /// 定时发布（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsScheduled { get; set; } = 0;

    /// <summary>
    /// 置顶（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsTop { get; set; } = 0;

    /// <summary>
    /// 置顶优先级（数字越大越靠前）
    /// </summary>
    public int TopPriority { get; set; } = 0;

    /// <summary>
    /// 过期时间（过期后自动隐藏）
    /// </summary>
    public DateTime? ExpireTime { get; set; }

    /// <summary>
    /// 查看次数
    /// </summary>
    public int ViewCount { get; set; } = 0;

    /// <summary>
    /// 目标范围（字典 sys_publish_scope；0=全部 1=指定部门 2=指定用户）
    /// </summary>
    public int TargetScope { get; set; } = 0;

    /// <summary>
    /// 目标部门编码（多个用逗号分隔；TargetScope=1 时使用）
    /// </summary>
    public string? TargetDepartments { get; set; } = string.Empty;

    /// <summary>
    /// 目标用户名（多个用逗号分隔；TargetScope=2 时使用；关联 TaktUser.UserName）
    /// </summary>
    public string? TargetUsers { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
    /// </summary>
    public int AnnouncementStatus { get; set; } = 0;

}

// ========================================
// Announcement 查询 DTO
// ========================================

/// <summary>
/// Announcement 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktAnnouncementQueryDto : TaktPagedQuery
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
    /// 公告编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 公告编码规则生成并展示，非手输；单据类型菜单：公告通知）
    /// </summary>
    public string? AnnouncementCode { get; set; } = string.Empty;

    /// <summary>
    /// 公告标题
    /// </summary>
    public string? AnnouncementTitle { get; set; } = string.Empty;

    /// <summary>
    /// 公告类型（字典 sys_announcement_category；1=紧急通知 2=公告 3=通知 4=决议 5=活动 6=安全通告 7=运维通知 8=系统公告）
    /// </summary>
    public int? AnnouncementType { get; set; }

    /// <summary>
    /// 公告内容（富文本 HTML）
    /// </summary>
    public string? Content { get; set; } = string.Empty;

    /// <summary>
    /// 公告摘要（用于列表展示）
    /// </summary>
    public string? Summary { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? Tags { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 发布时间（定时发布时使用）（范围查询-开始）
    /// </summary>
    public DateTime? PublishTimeStart { get; set; }

    /// <summary>
    /// 发布时间（定时发布时使用）（范围查询-结束）
    /// </summary>
    public DateTime? PublishTimeEnd { get; set; }

    /// <summary>
    /// 定时发布（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsScheduled { get; set; }

    /// <summary>
    /// 置顶（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsTop { get; set; }

    /// <summary>
    /// 置顶优先级（数字越大越靠前）
    /// </summary>
    public int? TopPriority { get; set; }

    /// <summary>
    /// 过期时间（过期后自动隐藏）（范围查询-开始）
    /// </summary>
    public DateTime? ExpireTimeStart { get; set; }

    /// <summary>
    /// 过期时间（过期后自动隐藏）（范围查询-结束）
    /// </summary>
    public DateTime? ExpireTimeEnd { get; set; }

    /// <summary>
    /// 查看次数
    /// </summary>
    public int? ViewCount { get; set; }

    /// <summary>
    /// 目标范围（字典 sys_publish_scope；0=全部 1=指定部门 2=指定用户）
    /// </summary>
    public int? TargetScope { get; set; }

    /// <summary>
    /// 目标部门编码（多个用逗号分隔；TargetScope=1 时使用）
    /// </summary>
    public string? TargetDepartments { get; set; } = string.Empty;

    /// <summary>
    /// 目标用户名（多个用逗号分隔；TargetScope=2 时使用；关联 TaktUser.UserName）
    /// </summary>
    public string? TargetUsers { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
    /// </summary>
    public int? AnnouncementStatus { get; set; }

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
// 创建Announcement DTO
// ========================================

/// <summary>
/// 创建Announcement DTO
/// </summary>
public class TaktAnnouncementCreateDto
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
    /// 公告编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 公告编码规则生成并展示，非手输；单据类型菜单：公告通知）
    /// </summary>
    [Required(ErrorMessage = "公告编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 公告编码规则生成并展示，非手输；单据类型菜单：公告通知）不能为空")]
    public string AnnouncementCode { get; set; } = string.Empty;

    /// <summary>
    /// 编码规则编码（前端表单从 TaktNumberings/options 选择；对应 TaktNumbering.RuleCode；不落库）
    /// </summary>
    public string? NumberingRuleCode { get; set; }

    /// <summary>
    /// 公告标题
    /// </summary>
    [Required(ErrorMessage = "公告标题不能为空")]
    public string AnnouncementTitle { get; set; } = string.Empty;

    /// <summary>
    /// 公告类型（字典 sys_announcement_category；1=紧急通知 2=公告 3=通知 4=决议 5=活动 6=安全通告 7=运维通知 8=系统公告）
    /// </summary>
    public int AnnouncementType { get; set; } = 0;

    /// <summary>
    /// 公告内容（富文本 HTML）
    /// </summary>
    [Required(ErrorMessage = "公告内容（富文本 HTML）不能为空")]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 公告摘要（用于列表展示）
    /// </summary>
    public string? Summary { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? Tags { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 发布时间（定时发布时使用）
    /// </summary>
    public DateTime? PublishTime { get; set; }

    /// <summary>
    /// 定时发布（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsScheduled { get; set; } = 0;

    /// <summary>
    /// 置顶（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsTop { get; set; } = 0;

    /// <summary>
    /// 置顶优先级（数字越大越靠前）
    /// </summary>
    public int TopPriority { get; set; } = 0;

    /// <summary>
    /// 过期时间（过期后自动隐藏）
    /// </summary>
    public DateTime? ExpireTime { get; set; }

    /// <summary>
    /// 查看次数
    /// </summary>
    public int ViewCount { get; set; } = 0;

    /// <summary>
    /// 目标范围（字典 sys_publish_scope；0=全部 1=指定部门 2=指定用户）
    /// </summary>
    public int TargetScope { get; set; } = 0;

    /// <summary>
    /// 目标部门编码（多个用逗号分隔；TargetScope=1 时使用）
    /// </summary>
    public string? TargetDepartments { get; set; } = string.Empty;

    /// <summary>
    /// 目标用户名（多个用逗号分隔；TargetScope=2 时使用；关联 TaktUser.UserName）
    /// </summary>
    public string? TargetUsers { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
    /// </summary>
    public int AnnouncementStatus { get; set; } = 0;

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
// 更新Announcement DTO
// ========================================

/// <summary>
/// 更新Announcement DTO
/// 继承 TaktAnnouncementCreateDto，添加 AnnouncementId 字段
/// </summary>
public class TaktAnnouncementUpdateDto : TaktAnnouncementCreateDto
{
    /// <summary>
    /// AnnouncementID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AnnouncementId { get; set; }

}

// ========================================
// Announcement 状态 DTO
// ========================================

/// <summary>
/// Announcement 状态更新 DTO
/// </summary>
public class TaktAnnouncementStatusDto
{
    /// <summary>
    /// AnnouncementID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AnnouncementId { get; set; }

    /// <summary>
    /// 状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
    /// </summary>
    [Required(ErrorMessage = "状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）不能为空")]
    public int AnnouncementStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Announcement 导入模板行 DTO
/// </summary>
public class TaktAnnouncementTemplateDto
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
    /// 公告编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 公告编码规则生成并展示，非手输；单据类型菜单：公告通知）
    /// </summary>
    public string? AnnouncementCode { get; set; } = string.Empty;

    /// <summary>
    /// 公告标题
    /// </summary>
    public string? AnnouncementTitle { get; set; } = string.Empty;

    /// <summary>
    /// 公告类型（字典 sys_announcement_category；1=紧急通知 2=公告 3=通知 4=决议 5=活动 6=安全通告 7=运维通知 8=系统公告）
    /// </summary>
    public int? AnnouncementType { get; set; }

    /// <summary>
    /// 公告内容（富文本 HTML）
    /// </summary>
    public string? Content { get; set; } = string.Empty;

    /// <summary>
    /// 公告摘要（用于列表展示）
    /// </summary>
    public string? Summary { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? Tags { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 发布时间（定时发布时使用）
    /// </summary>
    public DateTime? PublishTime { get; set; }

    /// <summary>
    /// 定时发布（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsScheduled { get; set; }

    /// <summary>
    /// 置顶（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsTop { get; set; }

    /// <summary>
    /// 置顶优先级（数字越大越靠前）
    /// </summary>
    public int? TopPriority { get; set; }

    /// <summary>
    /// 过期时间（过期后自动隐藏）
    /// </summary>
    public DateTime? ExpireTime { get; set; }

    /// <summary>
    /// 查看次数
    /// </summary>
    public int? ViewCount { get; set; }

    /// <summary>
    /// 目标范围（字典 sys_publish_scope；0=全部 1=指定部门 2=指定用户）
    /// </summary>
    public int? TargetScope { get; set; }

    /// <summary>
    /// 目标部门编码（多个用逗号分隔；TargetScope=1 时使用）
    /// </summary>
    public string? TargetDepartments { get; set; } = string.Empty;

    /// <summary>
    /// 目标用户名（多个用逗号分隔；TargetScope=2 时使用；关联 TaktUser.UserName）
    /// </summary>
    public string? TargetUsers { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
    /// </summary>
    public int? AnnouncementStatus { get; set; }

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
/// Announcement 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktAnnouncementImportDto
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
    /// 公告编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 公告编码规则生成并展示，非手输；单据类型菜单：公告通知）
    /// </summary>
    public string? AnnouncementCode { get; set; } = string.Empty;

    /// <summary>
    /// 公告标题
    /// </summary>
    public string? AnnouncementTitle { get; set; } = string.Empty;

    /// <summary>
    /// 公告类型（字典 sys_announcement_category；1=紧急通知 2=公告 3=通知 4=决议 5=活动 6=安全通告 7=运维通知 8=系统公告）
    /// </summary>
    public int? AnnouncementType { get; set; }

    /// <summary>
    /// 公告内容（富文本 HTML）
    /// </summary>
    public string? Content { get; set; } = string.Empty;

    /// <summary>
    /// 公告摘要（用于列表展示）
    /// </summary>
    public string? Summary { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? Tags { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 发布时间（定时发布时使用）
    /// </summary>
    public DateTime? PublishTime { get; set; }

    /// <summary>
    /// 定时发布（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsScheduled { get; set; }

    /// <summary>
    /// 置顶（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsTop { get; set; }

    /// <summary>
    /// 置顶优先级（数字越大越靠前）
    /// </summary>
    public int? TopPriority { get; set; }

    /// <summary>
    /// 过期时间（过期后自动隐藏）
    /// </summary>
    public DateTime? ExpireTime { get; set; }

    /// <summary>
    /// 查看次数
    /// </summary>
    public int? ViewCount { get; set; }

    /// <summary>
    /// 目标范围（字典 sys_publish_scope；0=全部 1=指定部门 2=指定用户）
    /// </summary>
    public int? TargetScope { get; set; }

    /// <summary>
    /// 目标部门编码（多个用逗号分隔；TargetScope=1 时使用）
    /// </summary>
    public string? TargetDepartments { get; set; } = string.Empty;

    /// <summary>
    /// 目标用户名（多个用逗号分隔；TargetScope=2 时使用；关联 TaktUser.UserName）
    /// </summary>
    public string? TargetUsers { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
    /// </summary>
    public int? AnnouncementStatus { get; set; }

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
/// Announcement 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktAnnouncementExportDto
{
    /// <summary>
    /// AnnouncementID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AnnouncementId { get; set; }

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
    /// 公告编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 公告编码规则生成并展示，非手输；单据类型菜单：公告通知）
    /// </summary>
    public string AnnouncementCode { get; set; } = string.Empty;

    /// <summary>
    /// 公告标题
    /// </summary>
    public string AnnouncementTitle { get; set; } = string.Empty;

    /// <summary>
    /// 公告类型（字典 sys_announcement_category；1=紧急通知 2=公告 3=通知 4=决议 5=活动 6=安全通告 7=运维通知 8=系统公告）
    /// </summary>
    public int AnnouncementType { get; set; } = 0;

    /// <summary>
    /// 公告内容（富文本 HTML）
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 公告摘要（用于列表展示）
    /// </summary>
    public string? Summary { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? Tags { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 发布时间（定时发布时使用）
    /// </summary>
    public DateTime? PublishTime { get; set; }

    /// <summary>
    /// 定时发布（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsScheduled { get; set; } = 0;

    /// <summary>
    /// 置顶（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsTop { get; set; } = 0;

    /// <summary>
    /// 置顶优先级（数字越大越靠前）
    /// </summary>
    public int TopPriority { get; set; } = 0;

    /// <summary>
    /// 过期时间（过期后自动隐藏）
    /// </summary>
    public DateTime? ExpireTime { get; set; }

    /// <summary>
    /// 查看次数
    /// </summary>
    public int ViewCount { get; set; } = 0;

    /// <summary>
    /// 目标范围（字典 sys_publish_scope；0=全部 1=指定部门 2=指定用户）
    /// </summary>
    public int TargetScope { get; set; } = 0;

    /// <summary>
    /// 目标部门编码（多个用逗号分隔；TargetScope=1 时使用）
    /// </summary>
    public string? TargetDepartments { get; set; } = string.Empty;

    /// <summary>
    /// 目标用户名（多个用逗号分隔；TargetScope=2 时使用；关联 TaktUser.UserName）
    /// </summary>
    public string? TargetUsers { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
    /// </summary>
    public int AnnouncementStatus { get; set; } = 0;

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
