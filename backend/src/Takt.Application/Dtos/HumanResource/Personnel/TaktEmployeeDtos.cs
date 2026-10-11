// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Personnel
// 文件名称：TaktEmployeeDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：Employee 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktEmployee 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Application.Dtos.HumanResource.Organization;

namespace Takt.Application.Dtos.HumanResource.Personnel;

// ========================================
// Employee 响应 DTO
// ========================================

/// <summary>
/// 员工实体（人事主档，公司级档案非审批单） 仅保留身份与档案基本属性；明细见导航子表： 教育→Education；地址→Address；家庭/紧急联系人→Family； 上岗日期/试用/转正/主部门岗位→Joined；离职→Resignation； 合同→Contract；调动→Reassignment；技能→Skill；履历→Experience； 附件→Attachment；代理→Delegation；入职待办→Onboarding
/// 对应前端 TaktEmployeeDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktEmployeeDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EmployeeID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 员工编码（租户+公司内唯一）
    /// </summary>
    public string EmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 姓名（2–80 位）
    /// </summary>
    public string EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 性别（字典 sys_user_gender；0=未知 1=男 2=女）
    /// </summary>
    public int Gender { get; set; } = 0;

    /// <summary>
    /// 出生日期（人事档案必填）
    /// </summary>
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// 身份证号（人事档案必填）
    /// </summary>
    public string IdCardCode { get; set; } = string.Empty;

    /// <summary>
    /// 手机号码（人事档案必填）
    /// </summary>
    public string Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 电子邮箱
    /// </summary>
    public string? Email { get; set; } = string.Empty;

    /// <summary>
    /// 籍贯（字典 humanresource_personnel_native_place_code；列存 6 位 GB 行政区划代码，人事档案必填；与住址子表无关）
    /// </summary>
    public string NativePlace { get; set; } = string.Empty;

    /// <summary>
    /// 民族（字典 humanresource_personnel_ethnic_code；DictValue 1～56）
    /// </summary>
    public int Ethnicity { get; set; } = 0;

    /// <summary>
    /// 政治面貌（字典 humanresource_personnel_political_affiliation；0～12；人事档案必填）
    /// </summary>
    public int PoliticalAffiliation { get; set; } = 0;

    /// <summary>
    /// 婚姻状况（字典 humanresource_personnel_marital_status；0=未婚 1=已婚 2=离异 3=丧偶；人事档案必填）
    /// </summary>
    public int MaritalStatus { get; set; } = 0;

    /// <summary>
    /// 员工状态（字典 humanresource_personnel_employee_status；1=试用期 2=正式 3=离职 4=退休）
    /// </summary>
    public int EmployeeStatus { get; set; } = 0;

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是；种子员工不可删）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 头像URL（展示用；档案附件明细见 EmployeeAttachments）
    /// </summary>
    public string? Avatar { get; set; } = string.Empty;

    /// <summary>
    /// 员工部门关联（RBAC，表 takt_human_resource_organization_employeedept）
    /// （RBAC：TaktEmployeeDept）
    /// </summary>
    public List<TaktEmployeeDeptDto>? EmployeeDepts { get; set; }

    /// <summary>
    /// 员工岗位关联（RBAC，表 takt_human_resource_organization_employeepost）
    /// （RBAC：TaktEmployeePost）
    /// </summary>
    public List<TaktEmployeePostDto>? EmployeePosts { get; set; }

    /// <summary>
    /// 员工地址（家庭/工作/常住）
    /// （子表：TaktEmployeeAddress）
    /// </summary>
    public List<TaktEmployeeAddressDto>? EmployeeAddresses { get; set; }

    /// <summary>
    /// 教育经历（含最高学历 IsHighest）
    /// （子表：TaktEmployeeEducation）
    /// </summary>
    public List<TaktEmployeeEducationDto>? EmployeeEducations { get; set; }

    /// <summary>
    /// 家庭成员（含紧急联系人 IsEmergencyContact）
    /// （子表：TaktEmployeeFamily）
    /// </summary>
    public List<TaktEmployeeFamilyDto>? EmployeeFamilies { get; set; }

    /// <summary>
    /// 外部工作经历
    /// （子表：TaktEmployeeExperience）
    /// </summary>
    public List<TaktEmployeeExperienceDto>? EmployeeExperiences { get; set; }

    /// <summary>
    /// 技能与证书
    /// （子表：TaktEmployeeSkill）
    /// </summary>
    public List<TaktEmployeeSkillDto>? EmployeeSkills { get; set; }

    /// <summary>
    /// 劳动合同
    /// （子表：TaktEmployeeContract）
    /// </summary>
    public List<TaktEmployeeContractDto>? EmployeeContracts { get; set; }

    /// <summary>
    /// 入职上岗办理（实际上岗日/试用/转正/部门岗位）
    /// （子表：TaktEmployeeJoined）
    /// </summary>
    public List<TaktEmployeeJoinedDto>? EmployeeJoineds { get; set; }

    /// <summary>
    /// 入职待办
    /// （子表：TaktEmployeeOnboarding）
    /// </summary>
    public List<TaktEmployeeOnboardingDto>? EmployeeOnboardings { get; set; }

    /// <summary>
    /// 调动记录
    /// （子表：TaktEmployeeReassignment）
    /// </summary>
    public List<TaktEmployeeReassignmentDto>? EmployeeReassignments { get; set; }

    /// <summary>
    /// 离职办理
    /// （子表：TaktEmployeeResignation）
    /// </summary>
    public List<TaktEmployeeResignationDto>? EmployeeResignations { get; set; }

    /// <summary>
    /// 档案附件
    /// （子表：TaktEmployeeAttachment）
    /// </summary>
    public List<TaktEmployeeAttachmentDto>? EmployeeAttachments { get; set; }

    /// <summary>
    /// 员工代理（被代理人视角；外键 OriginalEmployeeId）
    /// （子表：TaktEmployeeDelegation）
    /// </summary>
    public List<TaktEmployeeDelegationDto>? EmployeeDelegations { get; set; }

}

// ========================================
// Employee 查询 DTO
// ========================================

/// <summary>
/// Employee 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktEmployeeQueryDto : TaktPagedQuery
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
    /// 员工编码（租户+公司内唯一）
    /// </summary>
    public string? EmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 姓名（2–80 位）
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 性别（字典 sys_user_gender；0=未知 1=男 2=女）
    /// </summary>
    public int? Gender { get; set; }

    /// <summary>
    /// 出生日期（人事档案必填）（范围查询-开始）
    /// </summary>
    public DateTime? BirthDateStart { get; set; }

    /// <summary>
    /// 出生日期（人事档案必填）（范围查询-结束）
    /// </summary>
    public DateTime? BirthDateEnd { get; set; }

    /// <summary>
    /// 身份证号（人事档案必填）
    /// </summary>
    public string? IdCardCode { get; set; } = string.Empty;

    /// <summary>
    /// 手机号码（人事档案必填）
    /// </summary>
    public string? Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 电子邮箱
    /// </summary>
    public string? Email { get; set; } = string.Empty;

    /// <summary>
    /// 籍贯（字典 humanresource_personnel_native_place_code；列存 6 位 GB 行政区划代码，人事档案必填；与住址子表无关）
    /// </summary>
    public string? NativePlace { get; set; } = string.Empty;

    /// <summary>
    /// 民族（字典 humanresource_personnel_ethnic_code；DictValue 1～56）
    /// </summary>
    public int? Ethnicity { get; set; }

    /// <summary>
    /// 政治面貌（字典 humanresource_personnel_political_affiliation；0～12；人事档案必填）
    /// </summary>
    public int? PoliticalAffiliation { get; set; }

    /// <summary>
    /// 婚姻状况（字典 humanresource_personnel_marital_status；0=未婚 1=已婚 2=离异 3=丧偶；人事档案必填）
    /// </summary>
    public int? MaritalStatus { get; set; }

    /// <summary>
    /// 员工状态（字典 humanresource_personnel_employee_status；1=试用期 2=正式 3=离职 4=退休）
    /// </summary>
    public int? EmployeeStatus { get; set; }

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是；种子员工不可删）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 头像URL（展示用；档案附件明细见 EmployeeAttachments）
    /// </summary>
    public string? Avatar { get; set; } = string.Empty;

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
// 创建Employee DTO
// ========================================

/// <summary>
/// 创建Employee DTO
/// </summary>
public class TaktEmployeeCreateDto
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
    /// 员工编码（租户+公司内唯一）
    /// </summary>
    [Required(ErrorMessage = "员工编码（租户+公司内唯一）不能为空")]
    public string EmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 编码规则编码（表单选性别后自动取号；对应 TaktNumbering.RuleCode；不落库）
    /// </summary>
    public string? NumberingRuleCode { get; set; }

    /// <summary>
    /// 姓名（2–80 位）
    /// </summary>
    [Required(ErrorMessage = "姓名不能为空")]
    public string EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 性别（字典 sys_user_gender；0=未知 1=男 2=女）
    /// </summary>
    public int Gender { get; set; } = 0;

    /// <summary>
    /// 出生日期（人事档案必填）
    /// </summary>
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// 身份证号（人事档案必填）
    /// </summary>
    [Required(ErrorMessage = "身份证号（人事档案必填）不能为空")]
    public string IdCardCode { get; set; } = string.Empty;

    /// <summary>
    /// 手机号码（人事档案必填）
    /// </summary>
    [Required(ErrorMessage = "手机号码（人事档案必填）不能为空")]
    public string Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 电子邮箱
    /// </summary>
    public string? Email { get; set; } = string.Empty;

    /// <summary>
    /// 籍贯（字典 humanresource_personnel_native_place_code；列存 6 位 GB 行政区划代码，人事档案必填；与住址子表无关）
    /// </summary>
    [Required(ErrorMessage = "籍贯（字典 humanresource_personnel_native_place_code；列存 6 位 GB 行政区划代码，人事档案必填；与住址子表无关）不能为空")]
    public string NativePlace { get; set; } = string.Empty;

    /// <summary>
    /// 民族（字典 humanresource_personnel_ethnic_code；DictValue 1～56）
    /// </summary>
    public int Ethnicity { get; set; } = 0;

    /// <summary>
    /// 政治面貌（字典 humanresource_personnel_political_affiliation；0～12；人事档案必填）
    /// </summary>
    public int PoliticalAffiliation { get; set; } = 0;

    /// <summary>
    /// 婚姻状况（字典 humanresource_personnel_marital_status；0=未婚 1=已婚 2=离异 3=丧偶；人事档案必填）
    /// </summary>
    public int MaritalStatus { get; set; } = 0;

    /// <summary>
    /// 员工状态（字典 humanresource_personnel_employee_status；1=试用期 2=正式 3=离职 4=退休）
    /// </summary>
    public int EmployeeStatus { get; set; } = 0;

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是；种子员工不可删）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 头像URL（展示用；档案附件明细见 EmployeeAttachments）
    /// </summary>
    public string? Avatar { get; set; } = string.Empty;

    /// <summary>
    /// 员工部门关联（RBAC 全量覆盖，分配走 ITaktRbacService）
    /// </summary>
    public long[]? EmployeeDeptIds { get; set; }

    /// <summary>
    /// 员工岗位关联（RBAC 全量覆盖，分配走 ITaktRbacService）
    /// </summary>
    public long[]? EmployeePostIds { get; set; }

    /// <summary>
    /// 员工地址（家庭/工作/常住）（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeAddressCreateDto>? EmployeeAddresses { get; set; }

    /// <summary>
    /// 教育经历（含最高学历 IsHighest）（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeEducationCreateDto>? EmployeeEducations { get; set; }

    /// <summary>
    /// 家庭成员（含紧急联系人 IsEmergencyContact）（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeFamilyCreateDto>? EmployeeFamilies { get; set; }

    /// <summary>
    /// 外部工作经历（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeExperienceCreateDto>? EmployeeExperiences { get; set; }

    /// <summary>
    /// 技能与证书（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeSkillCreateDto>? EmployeeSkills { get; set; }

    /// <summary>
    /// 劳动合同（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeContractCreateDto>? EmployeeContracts { get; set; }

    /// <summary>
    /// 入职上岗办理（实际上岗日/试用/转正/部门岗位）（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeJoinedCreateDto>? EmployeeJoineds { get; set; }

    /// <summary>
    /// 入职待办（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeOnboardingCreateDto>? EmployeeOnboardings { get; set; }

    /// <summary>
    /// 调动记录（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeReassignmentCreateDto>? EmployeeReassignments { get; set; }

    /// <summary>
    /// 离职办理（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeResignationCreateDto>? EmployeeResignations { get; set; }

    /// <summary>
    /// 档案附件（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeAttachmentCreateDto>? EmployeeAttachments { get; set; }

    /// <summary>
    /// 员工代理（被代理人视角；外键 OriginalEmployeeId）（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeDelegationCreateDto>? EmployeeDelegations { get; set; }

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
// 更新Employee DTO
// ========================================

/// <summary>
/// 更新Employee DTO
/// 继承 TaktEmployeeCreateDto，添加 EmployeeId 字段
/// </summary>
public class TaktEmployeeUpdateDto : TaktEmployeeCreateDto
{
    /// <summary>
    /// EmployeeID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 员工地址（家庭/工作/常住）（子表，级联保存）
    /// </summary>
    public new List<TaktEmployeeAddressUpdateDto>? EmployeeAddresses { get; set; }

    /// <summary>
    /// 教育经历（含最高学历 IsHighest）（子表，级联保存）
    /// </summary>
    public new List<TaktEmployeeEducationUpdateDto>? EmployeeEducations { get; set; }

    /// <summary>
    /// 家庭成员（含紧急联系人 IsEmergencyContact）（子表，级联保存）
    /// </summary>
    public new List<TaktEmployeeFamilyUpdateDto>? EmployeeFamilies { get; set; }

    /// <summary>
    /// 外部工作经历（子表，级联保存）
    /// </summary>
    public new List<TaktEmployeeExperienceUpdateDto>? EmployeeExperiences { get; set; }

    /// <summary>
    /// 技能与证书（子表，级联保存）
    /// </summary>
    public new List<TaktEmployeeSkillUpdateDto>? EmployeeSkills { get; set; }

    /// <summary>
    /// 劳动合同（子表，级联保存）
    /// </summary>
    public new List<TaktEmployeeContractUpdateDto>? EmployeeContracts { get; set; }

    /// <summary>
    /// 入职上岗办理（实际上岗日/试用/转正/部门岗位）（子表，级联保存）
    /// </summary>
    public new List<TaktEmployeeJoinedUpdateDto>? EmployeeJoineds { get; set; }

    /// <summary>
    /// 入职待办（子表，级联保存）
    /// </summary>
    public new List<TaktEmployeeOnboardingUpdateDto>? EmployeeOnboardings { get; set; }

    /// <summary>
    /// 调动记录（子表，级联保存）
    /// </summary>
    public new List<TaktEmployeeReassignmentUpdateDto>? EmployeeReassignments { get; set; }

    /// <summary>
    /// 离职办理（子表，级联保存）
    /// </summary>
    public new List<TaktEmployeeResignationUpdateDto>? EmployeeResignations { get; set; }

    /// <summary>
    /// 档案附件（子表，级联保存）
    /// </summary>
    public new List<TaktEmployeeAttachmentUpdateDto>? EmployeeAttachments { get; set; }

    /// <summary>
    /// 员工代理（被代理人视角；外键 OriginalEmployeeId）（子表，级联保存）
    /// </summary>
    public new List<TaktEmployeeDelegationUpdateDto>? EmployeeDelegations { get; set; }

}

// ========================================
// Employee 状态 DTO
// ========================================

/// <summary>
/// Employee 状态更新 DTO
/// </summary>
public class TaktEmployeeStatusDto
{
    /// <summary>
    /// EmployeeID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 婚姻状况（字典 humanresource_personnel_marital_status；0=未婚 1=已婚 2=离异 3=丧偶；人事档案必填）
    /// </summary>
    [Required(ErrorMessage = "婚姻状况（字典 humanresource_personnel_marital_status；0=未婚 1=已婚 2=离异 3=丧偶；人事档案必填）不能为空")]
    public int MaritalStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Employee 导入模板行 DTO
/// </summary>
public class TaktEmployeeTemplateDto
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
    /// 员工编码（租户+公司内唯一）
    /// </summary>
    public string? EmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 姓名（2–80 位）
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 性别（字典 sys_user_gender；0=未知 1=男 2=女）
    /// </summary>
    public int? Gender { get; set; }

    /// <summary>
    /// 出生日期（人事档案必填）
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// 身份证号（人事档案必填）
    /// </summary>
    public string? IdCardCode { get; set; } = string.Empty;

    /// <summary>
    /// 手机号码（人事档案必填）
    /// </summary>
    public string? Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 电子邮箱
    /// </summary>
    public string? Email { get; set; } = string.Empty;

    /// <summary>
    /// 籍贯（字典 humanresource_personnel_native_place_code；列存 6 位 GB 行政区划代码，人事档案必填；与住址子表无关）
    /// </summary>
    public string? NativePlace { get; set; } = string.Empty;

    /// <summary>
    /// 民族（字典 humanresource_personnel_ethnic_code；DictValue 1～56）
    /// </summary>
    public int? Ethnicity { get; set; }

    /// <summary>
    /// 政治面貌（字典 humanresource_personnel_political_affiliation；0～12；人事档案必填）
    /// </summary>
    public int? PoliticalAffiliation { get; set; }

    /// <summary>
    /// 婚姻状况（字典 humanresource_personnel_marital_status；0=未婚 1=已婚 2=离异 3=丧偶；人事档案必填）
    /// </summary>
    public int? MaritalStatus { get; set; }

    /// <summary>
    /// 员工状态（字典 humanresource_personnel_employee_status；1=试用期 2=正式 3=离职 4=退休）
    /// </summary>
    public int? EmployeeStatus { get; set; }

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是；种子员工不可删）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 头像URL（展示用；档案附件明细见 EmployeeAttachments）
    /// </summary>
    public string? Avatar { get; set; } = string.Empty;

    /// <summary>
    /// 员工部门关联（RBAC 全量覆盖，分配走 ITaktRbacService）
    /// </summary>
    public long[]? EmployeeDeptIds { get; set; }

    /// <summary>
    /// 员工岗位关联（RBAC 全量覆盖，分配走 ITaktRbacService）
    /// </summary>
    public long[]? EmployeePostIds { get; set; }

    /// <summary>
    /// 员工地址（家庭/工作/常住）（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeAddressCreateDto>? EmployeeAddresses { get; set; }

    /// <summary>
    /// 教育经历（含最高学历 IsHighest）（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeEducationCreateDto>? EmployeeEducations { get; set; }

    /// <summary>
    /// 家庭成员（含紧急联系人 IsEmergencyContact）（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeFamilyCreateDto>? EmployeeFamilies { get; set; }

    /// <summary>
    /// 外部工作经历（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeExperienceCreateDto>? EmployeeExperiences { get; set; }

    /// <summary>
    /// 技能与证书（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeSkillCreateDto>? EmployeeSkills { get; set; }

    /// <summary>
    /// 劳动合同（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeContractCreateDto>? EmployeeContracts { get; set; }

    /// <summary>
    /// 入职上岗办理（实际上岗日/试用/转正/部门岗位）（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeJoinedCreateDto>? EmployeeJoineds { get; set; }

    /// <summary>
    /// 入职待办（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeOnboardingCreateDto>? EmployeeOnboardings { get; set; }

    /// <summary>
    /// 调动记录（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeReassignmentCreateDto>? EmployeeReassignments { get; set; }

    /// <summary>
    /// 离职办理（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeResignationCreateDto>? EmployeeResignations { get; set; }

    /// <summary>
    /// 档案附件（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeAttachmentCreateDto>? EmployeeAttachments { get; set; }

    /// <summary>
    /// 员工代理（被代理人视角；外键 OriginalEmployeeId）（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeDelegationCreateDto>? EmployeeDelegations { get; set; }

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
/// Employee 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktEmployeeImportDto
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
    /// 员工编码（租户+公司内唯一）
    /// </summary>
    public string? EmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 姓名（2–80 位）
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 性别（字典 sys_user_gender；0=未知 1=男 2=女）
    /// </summary>
    public int? Gender { get; set; }

    /// <summary>
    /// 出生日期（人事档案必填）
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// 身份证号（人事档案必填）
    /// </summary>
    public string? IdCardCode { get; set; } = string.Empty;

    /// <summary>
    /// 手机号码（人事档案必填）
    /// </summary>
    public string? Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 电子邮箱
    /// </summary>
    public string? Email { get; set; } = string.Empty;

    /// <summary>
    /// 籍贯（字典 humanresource_personnel_native_place_code；列存 6 位 GB 行政区划代码，人事档案必填；与住址子表无关）
    /// </summary>
    public string? NativePlace { get; set; } = string.Empty;

    /// <summary>
    /// 民族（字典 humanresource_personnel_ethnic_code；DictValue 1～56）
    /// </summary>
    public int? Ethnicity { get; set; }

    /// <summary>
    /// 政治面貌（字典 humanresource_personnel_political_affiliation；0～12；人事档案必填）
    /// </summary>
    public int? PoliticalAffiliation { get; set; }

    /// <summary>
    /// 婚姻状况（字典 humanresource_personnel_marital_status；0=未婚 1=已婚 2=离异 3=丧偶；人事档案必填）
    /// </summary>
    public int? MaritalStatus { get; set; }

    /// <summary>
    /// 员工状态（字典 humanresource_personnel_employee_status；1=试用期 2=正式 3=离职 4=退休）
    /// </summary>
    public int? EmployeeStatus { get; set; }

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是；种子员工不可删）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 头像URL（展示用；档案附件明细见 EmployeeAttachments）
    /// </summary>
    public string? Avatar { get; set; } = string.Empty;

    /// <summary>
    /// 员工部门关联（RBAC 全量覆盖，分配走 ITaktRbacService）
    /// </summary>
    public long[]? EmployeeDeptIds { get; set; }

    /// <summary>
    /// 员工岗位关联（RBAC 全量覆盖，分配走 ITaktRbacService）
    /// </summary>
    public long[]? EmployeePostIds { get; set; }

    /// <summary>
    /// 员工地址（家庭/工作/常住）（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeAddressCreateDto>? EmployeeAddresses { get; set; }

    /// <summary>
    /// 教育经历（含最高学历 IsHighest）（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeEducationCreateDto>? EmployeeEducations { get; set; }

    /// <summary>
    /// 家庭成员（含紧急联系人 IsEmergencyContact）（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeFamilyCreateDto>? EmployeeFamilies { get; set; }

    /// <summary>
    /// 外部工作经历（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeExperienceCreateDto>? EmployeeExperiences { get; set; }

    /// <summary>
    /// 技能与证书（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeSkillCreateDto>? EmployeeSkills { get; set; }

    /// <summary>
    /// 劳动合同（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeContractCreateDto>? EmployeeContracts { get; set; }

    /// <summary>
    /// 入职上岗办理（实际上岗日/试用/转正/部门岗位）（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeJoinedCreateDto>? EmployeeJoineds { get; set; }

    /// <summary>
    /// 入职待办（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeOnboardingCreateDto>? EmployeeOnboardings { get; set; }

    /// <summary>
    /// 调动记录（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeReassignmentCreateDto>? EmployeeReassignments { get; set; }

    /// <summary>
    /// 离职办理（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeResignationCreateDto>? EmployeeResignations { get; set; }

    /// <summary>
    /// 档案附件（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeAttachmentCreateDto>? EmployeeAttachments { get; set; }

    /// <summary>
    /// 员工代理（被代理人视角；外键 OriginalEmployeeId）（子表，级联保存）
    /// </summary>
    public List<TaktEmployeeDelegationCreateDto>? EmployeeDelegations { get; set; }

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
/// Employee 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktEmployeeExportDto
{
    /// <summary>
    /// EmployeeID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

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
    /// 员工编码（租户+公司内唯一）
    /// </summary>
    public string EmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 姓名（2–80 位）
    /// </summary>
    public string EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 性别（字典 sys_user_gender；0=未知 1=男 2=女）
    /// </summary>
    public int Gender { get; set; } = 0;

    /// <summary>
    /// 出生日期（人事档案必填）
    /// </summary>
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// 身份证号（人事档案必填）
    /// </summary>
    public string IdCardCode { get; set; } = string.Empty;

    /// <summary>
    /// 手机号码（人事档案必填）
    /// </summary>
    public string Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 电子邮箱
    /// </summary>
    public string? Email { get; set; } = string.Empty;

    /// <summary>
    /// 籍贯（字典 humanresource_personnel_native_place_code；列存 6 位 GB 行政区划代码，人事档案必填；与住址子表无关）
    /// </summary>
    public string NativePlace { get; set; } = string.Empty;

    /// <summary>
    /// 民族（字典 humanresource_personnel_ethnic_code；DictValue 1～56）
    /// </summary>
    public int Ethnicity { get; set; } = 0;

    /// <summary>
    /// 政治面貌（字典 humanresource_personnel_political_affiliation；0～12；人事档案必填）
    /// </summary>
    public int PoliticalAffiliation { get; set; } = 0;

    /// <summary>
    /// 婚姻状况（字典 humanresource_personnel_marital_status；0=未婚 1=已婚 2=离异 3=丧偶；人事档案必填）
    /// </summary>
    public int MaritalStatus { get; set; } = 0;

    /// <summary>
    /// 员工状态（字典 humanresource_personnel_employee_status；1=试用期 2=正式 3=离职 4=退休）
    /// </summary>
    public int EmployeeStatus { get; set; } = 0;

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是；种子员工不可删）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 头像URL（展示用；档案附件明细见 EmployeeAttachments）
    /// </summary>
    public string? Avatar { get; set; } = string.Empty;

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
