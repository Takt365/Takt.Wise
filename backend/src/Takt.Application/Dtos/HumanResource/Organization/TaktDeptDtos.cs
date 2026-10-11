// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Organization
// 文件名称：TaktDeptDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：Dept 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktDept 生成，请按需审阅）
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
// Dept 响应 DTO
// ========================================

/// <summary>
/// 部门实体 代表组织架构中的部门（树形结构）
/// 对应前端 TaktDeptDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktDeptDto : TaktCompanyDtoBase
{
    /// <summary>
    /// DeptID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeptId { get; set; }

    /// <summary>
    /// 部门编码（唯一索引：租户+公司内唯一，见 ix_dept_code_unique）
    /// </summary>
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门简称（与 ISO 编码一致，长度 6）
    /// </summary>
    public string DeptShortName { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称1
    /// </summary>
    public string DeptName1 { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称2
    /// </summary>
    public string DeptName2 { get; set; } = string.Empty;

    /// <summary>
    /// 父部门（关联 TaktDept.Id，选项 TaktDepts/tree-options；0=根部门）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentId { get; set; }

    /// <summary>
    /// 层级（1=一级部门，2=二级部门，以此类推）
    /// </summary>
    public int Level { get; set; } = 0;

    /// <summary>
    /// 部门路径（如：/1/3/5/，用于快速查询子部门）
    /// </summary>
    public string DeptPath { get; set; } = string.Empty;

    /// <summary>
    /// 叶子节点（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsLeaf { get; set; } = 0;

    /// <summary>
    /// ISO 编码（与部门简称一致，长度 6）
    /// </summary>
    public string IsoCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心编码（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options）
    /// </summary>
    public string CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 费用类别（字典 humanresource_organization_dept_cost_category；1=直接 2=间接）
    /// </summary>
    public int CostCategory { get; set; } = 0;

    /// <summary>
    /// 部门负责人（选项 TaktUsers/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long HeadUserId { get; set; }

    /// <summary>
    /// 部门负责人名称（冗余：按 HeadUserId 取 TaktUser.NickName联动）
    /// </summary>
    public string HeadUserName { get; set; } = string.Empty;

    /// <summary>
    /// 联系电话
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// 办公地点
    /// </summary>
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是；种子部门为内置，不允许删除）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 部门描述
    /// </summary>
    public string DeptDescription { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（同级部门排序）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
    /// </summary>
    public int DeptStatus { get; set; } = 0;

    /// <summary>
    /// 角色数据权限关联该部门（RBAC，表 takt_human_resource_organization_roledept）
    /// （RBAC：TaktRoleDept）
    /// </summary>
    public List<TaktRoleDeptDto>? RoleDepts { get; set; }

    /// <summary>
    /// 员工部门关联（RBAC，表 takt_human_resource_organization_employeedept）
    /// （RBAC：TaktEmployeeDept）
    /// </summary>
    public List<TaktEmployeeDeptDto>? EmployeeDepts { get; set; }

}

// ========================================
// Dept 树形响应 DTO
// ========================================

/// <summary>
/// Dept 树形列表/树选择 DTO（含子节点）
/// 对应 GetDeptTreeAsync 等接口
/// </summary>
public class TaktDeptTreeDto : TaktDeptDto
{
    /// <summary>
    /// 子节点（懒加载树接口返回 null，表示尚未加载；勿用空 List 冒充已加载）
    /// </summary>
    public List<TaktDeptTreeDto>? Children { get; set; }
}

// ========================================
// Dept 查询 DTO
// ========================================

/// <summary>
/// Dept 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktDeptQueryDto : TaktPagedQuery
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
    /// 部门编码（唯一索引：租户+公司内唯一，见 ix_dept_code_unique）
    /// </summary>
    public string? DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门简称（与 ISO 编码一致，长度 6）
    /// </summary>
    public string? DeptShortName { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称1
    /// </summary>
    public string? DeptName1 { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称2
    /// </summary>
    public string? DeptName2 { get; set; } = string.Empty;

    /// <summary>
    /// 父部门（关联 TaktDept.Id，选项 TaktDepts/tree-options；0=根部门）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentId { get; set; }

    /// <summary>
    /// 层级（1=一级部门，2=二级部门，以此类推）
    /// </summary>
    public int? Level { get; set; }

    /// <summary>
    /// 部门路径（如：/1/3/5/，用于快速查询子部门）
    /// </summary>
    public string? DeptPath { get; set; } = string.Empty;

    /// <summary>
    /// 叶子节点（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsLeaf { get; set; }

    /// <summary>
    /// ISO 编码（与部门简称一致，长度 6）
    /// </summary>
    public string? IsoCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心编码（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options）
    /// </summary>
    public string? CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 费用类别（字典 humanresource_organization_dept_cost_category；1=直接 2=间接）
    /// </summary>
    public int? CostCategory { get; set; }

    /// <summary>
    /// 部门负责人（选项 TaktUsers/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? HeadUserId { get; set; }

    /// <summary>
    /// 部门负责人名称（冗余：按 HeadUserId 取 TaktUser.NickName联动）
    /// </summary>
    public string? HeadUserName { get; set; } = string.Empty;

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? Phone { get; set; } = string.Empty;

    /// <summary>
    /// 邮箱
    /// </summary>
    public string? Email { get; set; } = string.Empty;

    /// <summary>
    /// 办公地点
    /// </summary>
    public string? Location { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是；种子部门为内置，不允许删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 部门描述
    /// </summary>
    public string? DeptDescription { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（同级部门排序）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
    /// </summary>
    public int? DeptStatus { get; set; }

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
// 创建Dept DTO
// ========================================

/// <summary>
/// 创建Dept DTO
/// </summary>
public class TaktDeptCreateDto
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
    /// 部门编码（唯一索引：租户+公司内唯一，见 ix_dept_code_unique）
    /// </summary>
    [Required(ErrorMessage = "部门编码（唯一索引：租户+公司内唯一，见 ix_dept_code_unique）不能为空")]
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门简称（与 ISO 编码一致，长度 6）
    /// </summary>
    [Required(ErrorMessage = "部门简称不能为空")]
    public string DeptShortName { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称1
    /// </summary>
    [Required(ErrorMessage = "部门名称1不能为空")]
    public string DeptName1 { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称2
    /// </summary>
    [Required(ErrorMessage = "部门名称2不能为空")]
    public string DeptName2 { get; set; } = string.Empty;

    /// <summary>
    /// 父部门（关联 TaktDept.Id，选项 TaktDepts/tree-options；0=根部门）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentId { get; set; }

    /// <summary>
    /// ISO 编码（与部门简称一致，长度 6）
    /// </summary>
    [Required(ErrorMessage = "ISO 编码不能为空")]
    public string IsoCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心编码（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options）
    /// </summary>
    [Required(ErrorMessage = "成本中心编码（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options）不能为空")]
    public string CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 费用类别（字典 humanresource_organization_dept_cost_category；1=直接 2=间接）
    /// </summary>
    public int CostCategory { get; set; } = 0;

    /// <summary>
    /// 部门负责人（选项 TaktUsers/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long HeadUserId { get; set; }

    /// <summary>
    /// 部门负责人名称（冗余：按 HeadUserId 取 TaktUser.NickName联动）
    /// </summary>
    public string HeadUserName { get; set; } = string.Empty;

    /// <summary>
    /// 联系电话
    /// </summary>
    [Required(ErrorMessage = "联系电话不能为空")]
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// 邮箱
    /// </summary>
    [Required(ErrorMessage = "邮箱不能为空")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// 办公地点
    /// </summary>
    [Required(ErrorMessage = "办公地点不能为空")]
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是；种子部门为内置，不允许删除）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 部门描述
    /// </summary>
    [Required(ErrorMessage = "部门描述不能为空")]
    public string DeptDescription { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
    /// </summary>
    public int DeptStatus { get; set; } = 0;

    /// <summary>
    /// 数据权限关联该部门的角色 ID 列表（RBAC 反向合并）
    /// </summary>
    public long[]? RoleIds { get; set; }

    /// <summary>
    /// 关联该部门的员工 ID 列表（RBAC 反向合并）
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
// 更新Dept DTO
// ========================================

/// <summary>
/// 更新Dept DTO
/// 继承 TaktDeptCreateDto，添加 DeptId 字段
/// </summary>
public class TaktDeptUpdateDto : TaktDeptCreateDto
{
    /// <summary>
    /// DeptID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeptId { get; set; }

}

// ========================================
// Dept 状态 DTO
// ========================================

/// <summary>
/// Dept 状态更新 DTO
/// </summary>
public class TaktDeptStatusDto
{
    /// <summary>
    /// DeptID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeptId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
    /// </summary>
    [Required(ErrorMessage = "状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）不能为空")]
    public int DeptStatus { get; set; } = 0;
}

// ========================================
// Dept 排序 DTO
// ========================================

/// <summary>
/// Dept 排序更新 DTO
/// </summary>
public class TaktDeptSortDto
{
    /// <summary>
    /// DeptID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeptId { get; set; }

    /// <summary>
    /// 排序号（回填）（同级部门排序）
    /// </summary>
    [Required(ErrorMessage = "排序号（回填）（同级部门排序）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// Dept 内置 DTO
// ========================================

/// <summary>
/// Dept 内置更新 DTO
/// </summary>
public class TaktDeptBuiltInDto
{
    /// <summary>
    /// DeptID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeptId { get; set; }

    /// <summary>
    /// 内置（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    [Required(ErrorMessage = "内置不能为空")]
    public int IsBuiltIn { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Dept 导入模板行 DTO
/// </summary>
public class TaktDeptTemplateDto
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
    /// 部门编码（唯一索引：租户+公司内唯一，见 ix_dept_code_unique）
    /// </summary>
    public string? DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门简称（与 ISO 编码一致，长度 6）
    /// </summary>
    public string? DeptShortName { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称1
    /// </summary>
    public string? DeptName1 { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称2
    /// </summary>
    public string? DeptName2 { get; set; } = string.Empty;

    /// <summary>
    /// 父部门（关联 TaktDept.Id，选项 TaktDepts/tree-options；0=根部门）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentId { get; set; }

    /// <summary>
    /// ISO 编码（与部门简称一致，长度 6）
    /// </summary>
    public string? IsoCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心编码（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options）
    /// </summary>
    public string? CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 费用类别（字典 humanresource_organization_dept_cost_category；1=直接 2=间接）
    /// </summary>
    public int? CostCategory { get; set; }

    /// <summary>
    /// 部门负责人（选项 TaktUsers/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? HeadUserId { get; set; }

    /// <summary>
    /// 部门负责人名称（冗余：按 HeadUserId 取 TaktUser.NickName联动）
    /// </summary>
    public string? HeadUserName { get; set; } = string.Empty;

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? Phone { get; set; } = string.Empty;

    /// <summary>
    /// 邮箱
    /// </summary>
    public string? Email { get; set; } = string.Empty;

    /// <summary>
    /// 办公地点
    /// </summary>
    public string? Location { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是；种子部门为内置，不允许删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 部门描述
    /// </summary>
    public string? DeptDescription { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
    /// </summary>
    public int? DeptStatus { get; set; }

    /// <summary>
    /// 数据权限关联该部门的角色 ID 列表（RBAC 反向合并）
    /// </summary>
    public long[]? RoleIds { get; set; }

    /// <summary>
    /// 关联该部门的员工 ID 列表（RBAC 反向合并）
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
/// Dept 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktDeptImportDto
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
    /// 部门编码（唯一索引：租户+公司内唯一，见 ix_dept_code_unique）
    /// </summary>
    public string? DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门简称（与 ISO 编码一致，长度 6）
    /// </summary>
    public string? DeptShortName { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称1
    /// </summary>
    public string? DeptName1 { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称2
    /// </summary>
    public string? DeptName2 { get; set; } = string.Empty;

    /// <summary>
    /// 父部门（关联 TaktDept.Id，选项 TaktDepts/tree-options；0=根部门）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentId { get; set; }

    /// <summary>
    /// ISO 编码（与部门简称一致，长度 6）
    /// </summary>
    public string? IsoCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心编码（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options）
    /// </summary>
    public string? CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 费用类别（字典 humanresource_organization_dept_cost_category；1=直接 2=间接）
    /// </summary>
    public int? CostCategory { get; set; }

    /// <summary>
    /// 部门负责人（选项 TaktUsers/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? HeadUserId { get; set; }

    /// <summary>
    /// 部门负责人名称（冗余：按 HeadUserId 取 TaktUser.NickName联动）
    /// </summary>
    public string? HeadUserName { get; set; } = string.Empty;

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? Phone { get; set; } = string.Empty;

    /// <summary>
    /// 邮箱
    /// </summary>
    public string? Email { get; set; } = string.Empty;

    /// <summary>
    /// 办公地点
    /// </summary>
    public string? Location { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是；种子部门为内置，不允许删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 部门描述
    /// </summary>
    public string? DeptDescription { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
    /// </summary>
    public int? DeptStatus { get; set; }

    /// <summary>
    /// 数据权限关联该部门的角色 ID 列表（RBAC 反向合并）
    /// </summary>
    public long[]? RoleIds { get; set; }

    /// <summary>
    /// 关联该部门的员工 ID 列表（RBAC 反向合并）
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
/// Dept 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktDeptExportDto
{
    /// <summary>
    /// DeptID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeptId { get; set; }

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
    /// 部门编码（唯一索引：租户+公司内唯一，见 ix_dept_code_unique）
    /// </summary>
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门简称（与 ISO 编码一致，长度 6）
    /// </summary>
    public string DeptShortName { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称1
    /// </summary>
    public string DeptName1 { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称2
    /// </summary>
    public string DeptName2 { get; set; } = string.Empty;

    /// <summary>
    /// 父部门（关联 TaktDept.Id，选项 TaktDepts/tree-options；0=根部门）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentId { get; set; }

    /// <summary>
    /// 层级（1=一级部门，2=二级部门，以此类推）
    /// </summary>
    public int Level { get; set; } = 0;

    /// <summary>
    /// 部门路径（如：/1/3/5/，用于快速查询子部门）
    /// </summary>
    public string DeptPath { get; set; } = string.Empty;

    /// <summary>
    /// 叶子节点（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsLeaf { get; set; } = 0;

    /// <summary>
    /// ISO 编码（与部门简称一致，长度 6）
    /// </summary>
    public string IsoCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心编码（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options）
    /// </summary>
    public string CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 费用类别（字典 humanresource_organization_dept_cost_category；1=直接 2=间接）
    /// </summary>
    public int CostCategory { get; set; } = 0;

    /// <summary>
    /// 部门负责人（选项 TaktUsers/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long HeadUserId { get; set; }

    /// <summary>
    /// 部门负责人名称（冗余：按 HeadUserId 取 TaktUser.NickName联动）
    /// </summary>
    public string HeadUserName { get; set; } = string.Empty;

    /// <summary>
    /// 联系电话
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// 办公地点
    /// </summary>
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是；种子部门为内置，不允许删除）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 部门描述
    /// </summary>
    public string DeptDescription { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（同级部门排序）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
    /// </summary>
    public int DeptStatus { get; set; } = 0;

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
