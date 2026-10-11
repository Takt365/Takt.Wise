// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Routine.DocumentCenter
// 文件名称：TaktDocumentDtos.cs
// 创建时间：2026-08-24
// 创建人：Takt365(Auto Generated)
// 功能描述：Document 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktDocument 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Routine.DocumentCenter;

// ========================================
// Document 响应 DTO
// ========================================

/// <summary>
/// 文管中心主实体 支持制度、流程、模板等文档的分类、版本与权限控制；需审批通过后发布（草稿→审批→发布） 审批态见基类 ApprovalStatus，字典 sys_approval_status
/// 对应前端 TaktDocumentDto
/// 继承 TaktApprovalDtoBase
/// </summary>
public class TaktDocumentDto : TaktApprovalDtoBase
{
    /// <summary>
    /// DocumentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DocumentId { get; set; }

    /// <summary>
    /// 文档编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 文档编码规则生成并展示，非手输；单据类型菜单：文档管理）
    /// </summary>
    public string DocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 文档标题
    /// </summary>
    public string DocumentTitle { get; set; } = string.Empty;

    /// <summary>
    /// 文档分类（字典 routine_document_center_category；0=制度 1=流程 2=模板 3=规范 4=其他）
    /// </summary>
    public int DocumentCategory { get; set; } = 0;

    /// <summary>
    /// 密级（字典 routine_document_center_confidential_level；0=公开 1=内部 2=机密 3=绝密）
    /// </summary>
    public int ConfidentialLevel { get; set; } = 0;

    /// <summary>
    /// 当前版本号
    /// </summary>
    public int Version { get; set; } = 0;

    /// <summary>
    /// 文档内容（富文本 HTML）
    /// </summary>
    public string? DocumentContent { get; set; } = string.Empty;

    /// <summary>
    /// 文档摘要（用于列表展示）
    /// </summary>
    public string? DocumentSummary { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? DocumentTags { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 生效时间
    /// </summary>
    public DateTime? DocumentEffectiveTime { get; set; }

    /// <summary>
    /// 失效时间
    /// </summary>
    public DateTime? DocumentExpireTime { get; set; }

    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTime? DocumentPublishTime { get; set; }

    /// <summary>
    /// 发布人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PublisherId { get; set; }

    /// <summary>
    /// 发布人姓名（冗余字段，便于查询）
    /// </summary>
    public string PublisherName { get; set; } = string.Empty;

    /// <summary>
    /// 归属部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 归属部门名称（冗余字段，便于查询）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 置顶（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int DocumentIsTop { get; set; } = 0;

    /// <summary>
    /// 浏览次数
    /// </summary>
    public int DocumentViewCount { get; set; } = 0;

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
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 文档状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
    /// </summary>
    public int DocumentStatus { get; set; } = 0;

    /// <summary>
    /// 版本历史列表（主子表关系）
    /// （子表：TaktDocumentVersion）
    /// </summary>
    public List<TaktDocumentVersionDto>? Versions { get; set; }

}

// ========================================
// Document 查询 DTO
// ========================================

/// <summary>
/// Document 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktDocumentQueryDto : TaktPagedQuery
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
    /// 文档编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 文档编码规则生成并展示，非手输；单据类型菜单：文档管理）
    /// </summary>
    public string? DocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 文档标题
    /// </summary>
    public string? DocumentTitle { get; set; } = string.Empty;

    /// <summary>
    /// 文档分类（字典 routine_document_center_category；0=制度 1=流程 2=模板 3=规范 4=其他）
    /// </summary>
    public int? DocumentCategory { get; set; }

    /// <summary>
    /// 密级（字典 routine_document_center_confidential_level；0=公开 1=内部 2=机密 3=绝密）
    /// </summary>
    public int? ConfidentialLevel { get; set; }

    /// <summary>
    /// 当前版本号
    /// </summary>
    public int? Version { get; set; }

    /// <summary>
    /// 文档内容（富文本 HTML）
    /// </summary>
    public string? DocumentContent { get; set; } = string.Empty;

    /// <summary>
    /// 文档摘要（用于列表展示）
    /// </summary>
    public string? DocumentSummary { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? DocumentTags { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 生效时间（范围查询-开始）
    /// </summary>
    public DateTime? DocumentEffectiveTimeStart { get; set; }

    /// <summary>
    /// 生效时间（范围查询-结束）
    /// </summary>
    public DateTime? DocumentEffectiveTimeEnd { get; set; }

    /// <summary>
    /// 失效时间（范围查询-开始）
    /// </summary>
    public DateTime? DocumentExpireTimeStart { get; set; }

    /// <summary>
    /// 失效时间（范围查询-结束）
    /// </summary>
    public DateTime? DocumentExpireTimeEnd { get; set; }

    /// <summary>
    /// 发布时间（范围查询-开始）
    /// </summary>
    public DateTime? DocumentPublishTimeStart { get; set; }

    /// <summary>
    /// 发布时间（范围查询-结束）
    /// </summary>
    public DateTime? DocumentPublishTimeEnd { get; set; }

    /// <summary>
    /// 发布人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PublisherId { get; set; }

    /// <summary>
    /// 发布人姓名（冗余字段，便于查询）
    /// </summary>
    public string? PublisherName { get; set; } = string.Empty;

    /// <summary>
    /// 归属部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 归属部门名称（冗余字段，便于查询）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 置顶（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? DocumentIsTop { get; set; }

    /// <summary>
    /// 浏览次数
    /// </summary>
    public int? DocumentViewCount { get; set; }

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
    /// 排序号（回填）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 文档状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
    /// </summary>
    public int? DocumentStatus { get; set; }

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
// 创建Document DTO
// ========================================

/// <summary>
/// 创建Document DTO
/// </summary>
public class TaktDocumentCreateDto
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
    /// 文档编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 文档编码规则生成并展示，非手输；单据类型菜单：文档管理）
    /// </summary>
    [Required(ErrorMessage = "文档编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 文档编码规则生成并展示，非手输；单据类型菜单：文档管理）不能为空")]
    public string DocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 编码规则编码（前端表单从 TaktNumberings/options 选择；对应 TaktNumbering.RuleCode；不落库）
    /// </summary>
    public string? NumberingRuleCode { get; set; }

    /// <summary>
    /// 文档标题
    /// </summary>
    [Required(ErrorMessage = "文档标题不能为空")]
    public string DocumentTitle { get; set; } = string.Empty;

    /// <summary>
    /// 文档分类（字典 routine_document_center_category；0=制度 1=流程 2=模板 3=规范 4=其他）
    /// </summary>
    public int DocumentCategory { get; set; } = 0;

    /// <summary>
    /// 密级（字典 routine_document_center_confidential_level；0=公开 1=内部 2=机密 3=绝密）
    /// </summary>
    public int ConfidentialLevel { get; set; } = 0;

    /// <summary>
    /// 当前版本号
    /// </summary>
    public int Version { get; set; } = 0;

    /// <summary>
    /// 文档内容（富文本 HTML）
    /// </summary>
    public string? DocumentContent { get; set; } = string.Empty;

    /// <summary>
    /// 文档摘要（用于列表展示）
    /// </summary>
    public string? DocumentSummary { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? DocumentTags { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 生效时间
    /// </summary>
    public DateTime? DocumentEffectiveTime { get; set; }

    /// <summary>
    /// 失效时间
    /// </summary>
    public DateTime? DocumentExpireTime { get; set; }

    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTime? DocumentPublishTime { get; set; }

    /// <summary>
    /// 发布人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PublisherId { get; set; }

    /// <summary>
    /// 发布人姓名（冗余字段，便于查询）
    /// </summary>
    public string PublisherName { get; set; } = string.Empty;

    /// <summary>
    /// 归属部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 归属部门名称（冗余字段，便于查询）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 置顶（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int DocumentIsTop { get; set; } = 0;

    /// <summary>
    /// 浏览次数
    /// </summary>
    public int DocumentViewCount { get; set; } = 0;

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
    /// 文档状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
    /// </summary>
    public int DocumentStatus { get; set; } = 0;

    /// <summary>
    /// 版本历史列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktDocumentVersionCreateDto>? Versions { get; set; }

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
// 更新Document DTO
// ========================================

/// <summary>
/// 更新Document DTO
/// 继承 TaktDocumentCreateDto，添加 DocumentId 字段
/// </summary>
public class TaktDocumentUpdateDto : TaktDocumentCreateDto
{
    /// <summary>
    /// DocumentID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DocumentId { get; set; }

    /// <summary>
    /// 版本历史列表（主子表关系）（子表，级联保存）
    /// </summary>
    public new List<TaktDocumentVersionUpdateDto>? Versions { get; set; }

}

// ========================================
// Document 状态 DTO
// ========================================

/// <summary>
/// Document 状态更新 DTO
/// </summary>
public class TaktDocumentStatusDto
{
    /// <summary>
    /// DocumentID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DocumentId { get; set; }

    /// <summary>
    /// 文档状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
    /// </summary>
    [Required(ErrorMessage = "文档状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）不能为空")]
    public int DocumentStatus { get; set; } = 0;
}

// ========================================
// Document 排序 DTO
// ========================================

/// <summary>
/// Document 排序更新 DTO
/// </summary>
public class TaktDocumentSortDto
{
    /// <summary>
    /// DocumentID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DocumentId { get; set; }

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [Required(ErrorMessage = "排序号（回填）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Document 导入模板行 DTO
/// </summary>
public class TaktDocumentTemplateDto
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
    /// 文档编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 文档编码规则生成并展示，非手输；单据类型菜单：文档管理）
    /// </summary>
    public string? DocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 文档标题
    /// </summary>
    public string? DocumentTitle { get; set; } = string.Empty;

    /// <summary>
    /// 文档分类（字典 routine_document_center_category；0=制度 1=流程 2=模板 3=规范 4=其他）
    /// </summary>
    public int? DocumentCategory { get; set; }

    /// <summary>
    /// 密级（字典 routine_document_center_confidential_level；0=公开 1=内部 2=机密 3=绝密）
    /// </summary>
    public int? ConfidentialLevel { get; set; }

    /// <summary>
    /// 当前版本号
    /// </summary>
    public int? Version { get; set; }

    /// <summary>
    /// 文档内容（富文本 HTML）
    /// </summary>
    public string? DocumentContent { get; set; } = string.Empty;

    /// <summary>
    /// 文档摘要（用于列表展示）
    /// </summary>
    public string? DocumentSummary { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? DocumentTags { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 生效时间
    /// </summary>
    public DateTime? DocumentEffectiveTime { get; set; }

    /// <summary>
    /// 失效时间
    /// </summary>
    public DateTime? DocumentExpireTime { get; set; }

    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTime? DocumentPublishTime { get; set; }

    /// <summary>
    /// 发布人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PublisherId { get; set; }

    /// <summary>
    /// 发布人姓名（冗余字段，便于查询）
    /// </summary>
    public string? PublisherName { get; set; } = string.Empty;

    /// <summary>
    /// 归属部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 归属部门名称（冗余字段，便于查询）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 置顶（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? DocumentIsTop { get; set; }

    /// <summary>
    /// 浏览次数
    /// </summary>
    public int? DocumentViewCount { get; set; }

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
    /// 文档状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
    /// </summary>
    public int? DocumentStatus { get; set; }

    /// <summary>
    /// 版本历史列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktDocumentVersionCreateDto>? Versions { get; set; }

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
/// Document 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktDocumentImportDto
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
    /// 文档编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 文档编码规则生成并展示，非手输；单据类型菜单：文档管理）
    /// </summary>
    public string? DocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 文档标题
    /// </summary>
    public string? DocumentTitle { get; set; } = string.Empty;

    /// <summary>
    /// 文档分类（字典 routine_document_center_category；0=制度 1=流程 2=模板 3=规范 4=其他）
    /// </summary>
    public int? DocumentCategory { get; set; }

    /// <summary>
    /// 密级（字典 routine_document_center_confidential_level；0=公开 1=内部 2=机密 3=绝密）
    /// </summary>
    public int? ConfidentialLevel { get; set; }

    /// <summary>
    /// 当前版本号
    /// </summary>
    public int? Version { get; set; }

    /// <summary>
    /// 文档内容（富文本 HTML）
    /// </summary>
    public string? DocumentContent { get; set; } = string.Empty;

    /// <summary>
    /// 文档摘要（用于列表展示）
    /// </summary>
    public string? DocumentSummary { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? DocumentTags { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 生效时间
    /// </summary>
    public DateTime? DocumentEffectiveTime { get; set; }

    /// <summary>
    /// 失效时间
    /// </summary>
    public DateTime? DocumentExpireTime { get; set; }

    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTime? DocumentPublishTime { get; set; }

    /// <summary>
    /// 发布人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PublisherId { get; set; }

    /// <summary>
    /// 发布人姓名（冗余字段，便于查询）
    /// </summary>
    public string? PublisherName { get; set; } = string.Empty;

    /// <summary>
    /// 归属部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 归属部门名称（冗余字段，便于查询）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 置顶（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? DocumentIsTop { get; set; }

    /// <summary>
    /// 浏览次数
    /// </summary>
    public int? DocumentViewCount { get; set; }

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
    /// 文档状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
    /// </summary>
    public int? DocumentStatus { get; set; }

    /// <summary>
    /// 版本历史列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktDocumentVersionCreateDto>? Versions { get; set; }

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
/// Document 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktDocumentExportDto
{
    /// <summary>
    /// DocumentID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DocumentId { get; set; }

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
    /// 文档编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 文档编码规则生成并展示，非手输；单据类型菜单：文档管理）
    /// </summary>
    public string DocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 文档标题
    /// </summary>
    public string DocumentTitle { get; set; } = string.Empty;

    /// <summary>
    /// 文档分类（字典 routine_document_center_category；0=制度 1=流程 2=模板 3=规范 4=其他）
    /// </summary>
    public int DocumentCategory { get; set; } = 0;

    /// <summary>
    /// 密级（字典 routine_document_center_confidential_level；0=公开 1=内部 2=机密 3=绝密）
    /// </summary>
    public int ConfidentialLevel { get; set; } = 0;

    /// <summary>
    /// 当前版本号
    /// </summary>
    public int Version { get; set; } = 0;

    /// <summary>
    /// 文档内容（富文本 HTML）
    /// </summary>
    public string? DocumentContent { get; set; } = string.Empty;

    /// <summary>
    /// 文档摘要（用于列表展示）
    /// </summary>
    public string? DocumentSummary { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? DocumentTags { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 生效时间
    /// </summary>
    public DateTime? DocumentEffectiveTime { get; set; }

    /// <summary>
    /// 失效时间
    /// </summary>
    public DateTime? DocumentExpireTime { get; set; }

    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTime? DocumentPublishTime { get; set; }

    /// <summary>
    /// 发布人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PublisherId { get; set; }

    /// <summary>
    /// 发布人姓名（冗余字段，便于查询）
    /// </summary>
    public string PublisherName { get; set; } = string.Empty;

    /// <summary>
    /// 归属部门 ID（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 归属部门名称（冗余字段，便于查询）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 置顶（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int DocumentIsTop { get; set; } = 0;

    /// <summary>
    /// 浏览次数
    /// </summary>
    public int DocumentViewCount { get; set; } = 0;

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
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 文档状态（字典 sys_publish_status；0=草稿 1=已发布 2=已撤回 3=已过期）
    /// </summary>
    public int DocumentStatus { get; set; } = 0;

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
