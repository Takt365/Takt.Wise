// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Organization
// 文件名称：TaktPostDtos.cs
// 创建时间：2026-08-21
// 创建人：Takt365(Auto Generated)
// 功能描述：Post 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPost 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.HumanResource.Organization;

// ========================================
// Post 响应 DTO
// ========================================

/// <summary>
/// 岗位实体 代表组织架构中的岗位/职位
/// 对应前端 TaktPostDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPostDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PostID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PostId { get; set; }

    /// <summary>
    /// 岗位编码（唯一索引：租户+公司内唯一，见 ix_post_code_unique）
    /// </summary>
    public string PostCode { get; set; } = string.Empty;

    /// <summary>
    /// 岗位名称
    /// </summary>
    public string PostName { get; set; } = string.Empty;

    /// <summary>
    /// 所属部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeptId { get; set; }

    /// <summary>
    /// 所属部门名称（冗余：按 DeptId 取 TaktDept.DeptName联动）
    /// </summary>
    public string DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 岗位类别（字典 sys_post_category；列存 DictValue：MGT=管理岗 PRO=专业岗 TEC=技术岗 SUP=支持岗 OPS=操作岗）
    /// </summary>
    public string PostCategory { get; set; } = string.Empty;

    /// <summary>
    /// 岗位职级（字典 sys_post_level；列存 DictValue：P1~P4 专业序列 M1~M5 管理序列）
    /// </summary>
    public string PostLevel { get; set; } = string.Empty;

    /// <summary>
    /// 编制人数
    /// </summary>
    public int Headcount { get; set; } = 0;

    /// <summary>
    /// 当前在职人数
    /// </summary>
    public int CurrentCount { get; set; } = 0;

    /// <summary>
    /// 岗位职责
    /// </summary>
    public string Responsibilities { get; set; } = string.Empty;

    /// <summary>
    /// 任职要求
    /// </summary>
    public string Requirements { get; set; } = string.Empty;

    /// <summary>
    /// 学历要求（字典 humanresource_personnel_education_level；1=高中及以下 2=大专 3=本科 4=硕士 5=博士）
    /// </summary>
    public int EducationRequired { get; set; } = 0;

    /// <summary>
    /// 工作经验要求（年）
    /// </summary>
    public int ExperienceYears { get; set; } = 0;

    /// <summary>
    /// 薪资范围（最低）
    /// </summary>
    public decimal? SalaryMin { get; set; }

    /// <summary>
    /// 薪资范围（最高）
    /// </summary>
    public decimal? SalaryMax { get; set; }

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是；种子岗位为内置，不允许删除）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 岗位描述
    /// </summary>
    public string? PostDescription { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
    /// </summary>
    public int PostStatus { get; set; } = 0;

}

// ========================================
// Post 查询 DTO
// ========================================

/// <summary>
/// Post 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPostQueryDto : TaktPagedQuery
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
    /// 岗位编码（唯一索引：租户+公司内唯一，见 ix_post_code_unique）
    /// </summary>
    public string? PostCode { get; set; } = string.Empty;

    /// <summary>
    /// 岗位名称
    /// </summary>
    public string? PostName { get; set; } = string.Empty;

    /// <summary>
    /// 所属部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 所属部门名称（冗余：按 DeptId 取 TaktDept.DeptName联动）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 岗位类别（字典 sys_post_category；列存 DictValue：MGT=管理岗 PRO=专业岗 TEC=技术岗 SUP=支持岗 OPS=操作岗）
    /// </summary>
    public string? PostCategory { get; set; } = string.Empty;

    /// <summary>
    /// 岗位职级（字典 sys_post_level；列存 DictValue：P1~P4 专业序列 M1~M5 管理序列）
    /// </summary>
    public string? PostLevel { get; set; } = string.Empty;

    /// <summary>
    /// 编制人数
    /// </summary>
    public int? Headcount { get; set; }

    /// <summary>
    /// 当前在职人数
    /// </summary>
    public int? CurrentCount { get; set; }

    /// <summary>
    /// 岗位职责
    /// </summary>
    public string? Responsibilities { get; set; } = string.Empty;

    /// <summary>
    /// 任职要求
    /// </summary>
    public string? Requirements { get; set; } = string.Empty;

    /// <summary>
    /// 学历要求（字典 humanresource_personnel_education_level；1=高中及以下 2=大专 3=本科 4=硕士 5=博士）
    /// </summary>
    public int? EducationRequired { get; set; }

    /// <summary>
    /// 工作经验要求（年）
    /// </summary>
    public int? ExperienceYears { get; set; }

    /// <summary>
    /// 薪资范围（最低）
    /// </summary>
    public decimal? SalaryMin { get; set; }

    /// <summary>
    /// 薪资范围（最高）
    /// </summary>
    public decimal? SalaryMax { get; set; }

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是；种子岗位为内置，不允许删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 岗位描述
    /// </summary>
    public string? PostDescription { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
    /// </summary>
    public int? PostStatus { get; set; }

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
// 创建Post DTO
// ========================================

/// <summary>
/// 创建Post DTO
/// </summary>
public class TaktPostCreateDto
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
    /// 岗位编码（唯一索引：租户+公司内唯一，见 ix_post_code_unique）
    /// </summary>
    [Required(ErrorMessage = "岗位编码（唯一索引：租户+公司内唯一，见 ix_post_code_unique）不能为空")]
    public string PostCode { get; set; } = string.Empty;

    /// <summary>
    /// 岗位名称
    /// </summary>
    [Required(ErrorMessage = "岗位名称不能为空")]
    public string PostName { get; set; } = string.Empty;

    /// <summary>
    /// 所属部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeptId { get; set; }

    /// <summary>
    /// 所属部门名称（冗余：按 DeptId 取 TaktDept.DeptName联动）
    /// </summary>
    [Required(ErrorMessage = "所属部门名称（冗余：按 DeptId 取 TaktDept.DeptName联动）不能为空")]
    public string DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 岗位类别（字典 sys_post_category；列存 DictValue：MGT=管理岗 PRO=专业岗 TEC=技术岗 SUP=支持岗 OPS=操作岗）
    /// </summary>
    [Required(ErrorMessage = "岗位类别（字典 sys_post_category；列存 DictValue：MGT=管理岗 PRO=专业岗 TEC=技术岗 SUP=支持岗 OPS=操作岗）不能为空")]
    public string PostCategory { get; set; } = string.Empty;

    /// <summary>
    /// 岗位职级（字典 sys_post_level；列存 DictValue：P1~P4 专业序列 M1~M5 管理序列）
    /// </summary>
    [Required(ErrorMessage = "岗位职级（字典 sys_post_level；列存 DictValue：P1~P4 专业序列 M1~M5 管理序列）不能为空")]
    public string PostLevel { get; set; } = string.Empty;

    /// <summary>
    /// 编制人数
    /// </summary>
    public int Headcount { get; set; } = 0;

    /// <summary>
    /// 当前在职人数
    /// </summary>
    public int CurrentCount { get; set; } = 0;

    /// <summary>
    /// 岗位职责
    /// </summary>
    [Required(ErrorMessage = "岗位职责不能为空")]
    public string Responsibilities { get; set; } = string.Empty;

    /// <summary>
    /// 任职要求
    /// </summary>
    [Required(ErrorMessage = "任职要求不能为空")]
    public string Requirements { get; set; } = string.Empty;

    /// <summary>
    /// 学历要求（字典 humanresource_personnel_education_level；1=高中及以下 2=大专 3=本科 4=硕士 5=博士）
    /// </summary>
    public int EducationRequired { get; set; } = 0;

    /// <summary>
    /// 工作经验要求（年）
    /// </summary>
    public int ExperienceYears { get; set; } = 0;

    /// <summary>
    /// 薪资范围（最低）
    /// </summary>
    public decimal? SalaryMin { get; set; }

    /// <summary>
    /// 薪资范围（最高）
    /// </summary>
    public decimal? SalaryMax { get; set; }

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是；种子岗位为内置，不允许删除）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 岗位描述
    /// </summary>
    public string? PostDescription { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
    /// </summary>
    public int PostStatus { get; set; } = 0;

    /// <summary>
    /// 关联该岗位的员工 ID 列表（RBAC 反向合并）
    /// </summary>
    public long[]? EmployeeIds { get; set; }

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
// 更新Post DTO
// ========================================

/// <summary>
/// 更新Post DTO
/// 继承 TaktPostCreateDto，添加 PostId 字段
/// </summary>
public class TaktPostUpdateDto : TaktPostCreateDto
{
    /// <summary>
    /// PostID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PostId { get; set; }

}

// ========================================
// Post 状态 DTO
// ========================================

/// <summary>
/// Post 状态更新 DTO
/// </summary>
public class TaktPostStatusDto
{
    /// <summary>
    /// PostID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PostId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
    /// </summary>
    [Required(ErrorMessage = "状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）不能为空")]
    public int PostStatus { get; set; } = 0;
}

// ========================================
// Post 排序 DTO
// ========================================

/// <summary>
/// Post 排序更新 DTO
/// </summary>
public class TaktPostSortDto
{
    /// <summary>
    /// PostID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PostId { get; set; }

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [Required(ErrorMessage = "排序号不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Post 导入模板行 DTO
/// </summary>
public class TaktPostTemplateDto
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
    /// 岗位编码（唯一索引：租户+公司内唯一，见 ix_post_code_unique）
    /// </summary>
    public string? PostCode { get; set; } = string.Empty;

    /// <summary>
    /// 岗位名称
    /// </summary>
    public string? PostName { get; set; } = string.Empty;

    /// <summary>
    /// 所属部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 所属部门名称（冗余：按 DeptId 取 TaktDept.DeptName联动）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 岗位类别（字典 sys_post_category；列存 DictValue：MGT=管理岗 PRO=专业岗 TEC=技术岗 SUP=支持岗 OPS=操作岗）
    /// </summary>
    public string? PostCategory { get; set; } = string.Empty;

    /// <summary>
    /// 岗位职级（字典 sys_post_level；列存 DictValue：P1~P4 专业序列 M1~M5 管理序列）
    /// </summary>
    public string? PostLevel { get; set; } = string.Empty;

    /// <summary>
    /// 编制人数
    /// </summary>
    public int? Headcount { get; set; }

    /// <summary>
    /// 当前在职人数
    /// </summary>
    public int? CurrentCount { get; set; }

    /// <summary>
    /// 岗位职责
    /// </summary>
    public string? Responsibilities { get; set; } = string.Empty;

    /// <summary>
    /// 任职要求
    /// </summary>
    public string? Requirements { get; set; } = string.Empty;

    /// <summary>
    /// 学历要求（字典 humanresource_personnel_education_level；1=高中及以下 2=大专 3=本科 4=硕士 5=博士）
    /// </summary>
    public int? EducationRequired { get; set; }

    /// <summary>
    /// 工作经验要求（年）
    /// </summary>
    public int? ExperienceYears { get; set; }

    /// <summary>
    /// 薪资范围（最低）
    /// </summary>
    public decimal? SalaryMin { get; set; }

    /// <summary>
    /// 薪资范围（最高）
    /// </summary>
    public decimal? SalaryMax { get; set; }

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是；种子岗位为内置，不允许删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 岗位描述
    /// </summary>
    public string? PostDescription { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
    /// </summary>
    public int? PostStatus { get; set; }

    /// <summary>
    /// 关联该岗位的员工 ID 列表（RBAC 反向合并）
    /// </summary>
    public long[]? EmployeeIds { get; set; }

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
/// Post 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPostImportDto
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
    /// 岗位编码（唯一索引：租户+公司内唯一，见 ix_post_code_unique）
    /// </summary>
    public string? PostCode { get; set; } = string.Empty;

    /// <summary>
    /// 岗位名称
    /// </summary>
    public string? PostName { get; set; } = string.Empty;

    /// <summary>
    /// 所属部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 所属部门名称（冗余：按 DeptId 取 TaktDept.DeptName联动）
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 岗位类别（字典 sys_post_category；列存 DictValue：MGT=管理岗 PRO=专业岗 TEC=技术岗 SUP=支持岗 OPS=操作岗）
    /// </summary>
    public string? PostCategory { get; set; } = string.Empty;

    /// <summary>
    /// 岗位职级（字典 sys_post_level；列存 DictValue：P1~P4 专业序列 M1~M5 管理序列）
    /// </summary>
    public string? PostLevel { get; set; } = string.Empty;

    /// <summary>
    /// 编制人数
    /// </summary>
    public int? Headcount { get; set; }

    /// <summary>
    /// 当前在职人数
    /// </summary>
    public int? CurrentCount { get; set; }

    /// <summary>
    /// 岗位职责
    /// </summary>
    public string? Responsibilities { get; set; } = string.Empty;

    /// <summary>
    /// 任职要求
    /// </summary>
    public string? Requirements { get; set; } = string.Empty;

    /// <summary>
    /// 学历要求（字典 humanresource_personnel_education_level；1=高中及以下 2=大专 3=本科 4=硕士 5=博士）
    /// </summary>
    public int? EducationRequired { get; set; }

    /// <summary>
    /// 工作经验要求（年）
    /// </summary>
    public int? ExperienceYears { get; set; }

    /// <summary>
    /// 薪资范围（最低）
    /// </summary>
    public decimal? SalaryMin { get; set; }

    /// <summary>
    /// 薪资范围（最高）
    /// </summary>
    public decimal? SalaryMax { get; set; }

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是；种子岗位为内置，不允许删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 岗位描述
    /// </summary>
    public string? PostDescription { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
    /// </summary>
    public int? PostStatus { get; set; }

    /// <summary>
    /// 关联该岗位的员工 ID 列表（RBAC 反向合并）
    /// </summary>
    public long[]? EmployeeIds { get; set; }

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
/// Post 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPostExportDto
{
    /// <summary>
    /// PostID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PostId { get; set; }

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
    /// 岗位编码（唯一索引：租户+公司内唯一，见 ix_post_code_unique）
    /// </summary>
    public string PostCode { get; set; } = string.Empty;

    /// <summary>
    /// 岗位名称
    /// </summary>
    public string PostName { get; set; } = string.Empty;

    /// <summary>
    /// 所属部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeptId { get; set; }

    /// <summary>
    /// 所属部门名称（冗余：按 DeptId 取 TaktDept.DeptName联动）
    /// </summary>
    public string DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 岗位类别（字典 sys_post_category；列存 DictValue：MGT=管理岗 PRO=专业岗 TEC=技术岗 SUP=支持岗 OPS=操作岗）
    /// </summary>
    public string PostCategory { get; set; } = string.Empty;

    /// <summary>
    /// 岗位职级（字典 sys_post_level；列存 DictValue：P1~P4 专业序列 M1~M5 管理序列）
    /// </summary>
    public string PostLevel { get; set; } = string.Empty;

    /// <summary>
    /// 编制人数
    /// </summary>
    public int Headcount { get; set; } = 0;

    /// <summary>
    /// 当前在职人数
    /// </summary>
    public int CurrentCount { get; set; } = 0;

    /// <summary>
    /// 岗位职责
    /// </summary>
    public string Responsibilities { get; set; } = string.Empty;

    /// <summary>
    /// 任职要求
    /// </summary>
    public string Requirements { get; set; } = string.Empty;

    /// <summary>
    /// 学历要求（字典 humanresource_personnel_education_level；1=高中及以下 2=大专 3=本科 4=硕士 5=博士）
    /// </summary>
    public int EducationRequired { get; set; } = 0;

    /// <summary>
    /// 工作经验要求（年）
    /// </summary>
    public int ExperienceYears { get; set; } = 0;

    /// <summary>
    /// 薪资范围（最低）
    /// </summary>
    public decimal? SalaryMin { get; set; }

    /// <summary>
    /// 薪资范围（最高）
    /// </summary>
    public decimal? SalaryMax { get; set; }

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是；种子岗位为内置，不允许删除）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 岗位描述
    /// </summary>
    public string? PostDescription { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
    /// </summary>
    public int PostStatus { get; set; } = 0;

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
