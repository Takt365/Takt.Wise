// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Organization
// 文件名称：TaktDept.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：部门实体，代表组织架构中的部门（树形结构）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities.HumanResource.Organization;

/// <summary>
/// 部门实体
/// 代表组织架构中的部门（树形结构）
/// </summary>
[SugarTable("takt_human_resource_organization_dept", "部门表")]
[SugarIndex("ix_dept_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_dept_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_dept_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(DeptCode), OrderByType.Asc, true)]
[SugarIndex("ix_dept_parent", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ParentId), OrderByType.Asc, false)]
public class TaktDept : TaktCompanyEntityBase
{
    /// <summary>
    /// 部门编码（唯一索引：租户+公司内唯一，见 ix_dept_code_unique；与成本中心编码同长 6；前端表单选择编码规则后自动通过 TaktNumbering 部门编码规则生成并展示，非手输；单据类型菜单：部门管理）
    /// </summary>
    [SugarColumn(ColumnName = "dept_code", ColumnDescription = "部门编码", ColumnDataType = "varchar", Length = 6, IsNullable = false)]
    public string DeptCode { get; set; } = string.Empty;
    /// <summary>
    /// 部门简称（与 ISO 编码一致，长度 6）
    /// </summary>
    [SugarColumn(ColumnName = "dept_short_name", ColumnDescription = "部门简称", ColumnDataType = "varchar", Length = 6, IsNullable = false)]
    public string DeptShortName { get; set; } = string.Empty;
    /// <summary>
    /// 部门名称1
    /// </summary>
    [SugarColumn(ColumnName = "dept_name1", ColumnDescription = "部门名称1", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string DeptName1 { get; set; } = string.Empty;
    /// <summary>
    /// 部门名称2
    /// </summary>
    [SugarColumn(ColumnName = "dept_name2", ColumnDescription = "部门名称2", ColumnDataType = "nvarchar", Length = 70, IsNullable = false, DefaultValue = "")]
    public string DeptName2 { get; set; } = string.Empty;
    /// <summary>
    /// 父部门（关联 TaktDept.Id，选项 TaktDepts/tree-options；0=根部门）
    /// </summary>
    [SugarColumn(ColumnName = "parent_id", ColumnDescription = "父部门ID", ColumnDataType = "bigint", IsNullable = false, DefaultValue = "0")]
    public long ParentId { get; set; } = 0;
    /// <summary>
    /// 层级（1=一级部门，2=二级部门，以此类推）
    /// </summary>
    [SugarColumn(ColumnName = "level", ColumnDescription = "层级", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int Level { get; set; } = 1;
    /// <summary>
    /// 部门路径（如：/1/3/5/，用于快速查询子部门）
    /// </summary>
    [SugarColumn(ColumnName = "dept_path", ColumnDescription = "部门路径", ColumnDataType = "varchar", Length = 500, IsNullable = false, DefaultValue = "")]
    public string DeptPath { get; set; } = string.Empty;
    /// <summary>
    /// 叶子节点（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_leaf", ColumnDescription = "叶子节点", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsLeaf { get; set; } = 0;
    /// <summary>
    /// ISO 编码（与部门简称 dept_short_name 一致，长度 6）
    /// </summary>
    [SugarColumn(ColumnName = "iso_code", ColumnDescription = "ISO编码", ColumnDataType = "varchar", Length = 6, IsNullable = false, DefaultValue = "")]
    public string IsoCode { get; set; } = string.Empty;
    /// <summary>
    /// 成本中心编码（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options；默认与部门编码一致，长度 6）
    /// </summary>
    [SugarColumn(ColumnName = "cost_center_code", ColumnDescription = "成本中心编码", ColumnDataType = "varchar", Length = 6, IsNullable = false, DefaultValue = "")]
    public string CostCenterCode { get; set; } = string.Empty;
    /// <summary>
    /// 费用类别（字典 humanresource_organization_dept_cost_category；1=直接 2=间接）
    /// </summary>
    [SugarColumn(ColumnName = "cost_category", ColumnDescription = "费用类别", ColumnDataType = "int", IsNullable = false, DefaultValue = "2")]
    public int CostCategory { get; set; } = 2;
    /// <summary>
    /// 部门负责人（选项 TaktUsers/options，DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "head_user_id", ColumnDescription = "部门负责人ID", ColumnDataType = "bigint", IsNullable = false, DefaultValue = "0")]
    public long HeadUserId { get; set; }
    /// <summary>
    /// 部门负责人名称（冗余：按 HeadUserId 取 TaktUser.NickName联动）
    /// </summary>
    [SugarColumn(ColumnName = "head_user_name", ColumnDescription = "部门负责人名称", ColumnDataType = "nvarchar", Length = 40, IsNullable = false, DefaultValue = "")]
    public string HeadUserName { get; set; } = string.Empty;
    /// <summary>
    /// 联系电话
    /// </summary>
    [SugarColumn(ColumnName = "phone", ColumnDescription = "联系电话", ColumnDataType = "varchar", Length = 20, IsNullable = false, DefaultValue = "")]
    public string Phone { get; set; } = string.Empty;
    /// <summary>
    /// 邮箱
    /// </summary>
    [SugarColumn(ColumnName = "email", ColumnDescription = "邮箱", ColumnDataType = "varchar", Length = 100, IsNullable = false, DefaultValue = "")]
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// 办公地点
    /// </summary>
    [SugarColumn(ColumnName = "location", ColumnDescription = "办公地点", ColumnDataType = "nvarchar", Length = 200, IsNullable = false, DefaultValue = "")]
    public string Location { get; set; } = string.Empty;
    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是；种子部门为内置，不允许删除）
    /// </summary>
    [SugarColumn(ColumnName = "is_built_in", ColumnDescription = "内置", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsBuiltIn { get; set; } = 0;
    /// <summary>
    /// 部门描述
    /// </summary>
    [SugarColumn(ColumnName = "dept_description", ColumnDescription = "部门描述", ColumnDataType = "nvarchar", Length = 500, IsNullable = false, DefaultValue = "")]
    public string DeptDescription { get; set; } = string.Empty;
    /// <summary>
    /// 排序号（回填）（同级部门排序）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;
    /// <summary>
    /// 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
    /// </summary>
    [SugarColumn(ColumnName = "dept_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int DeptStatus { get; set; } = 1;

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 角色数据权限关联该部门（RBAC，表 takt_human_resource_organization_roledept）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktRoleDept.DeptId))]
    public List<TaktRoleDept>? RoleDepts { get; set; }

    /// <summary>
    /// 员工部门关联（RBAC，表 takt_human_resource_organization_employeedept）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktEmployeeDept.DeptId))]
    public List<TaktEmployeeDept>? EmployeeDepts { get; set; }

}
