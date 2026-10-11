// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/organization
// 文件名称：dept.types.ts
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/organization 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 部门实体 代表组织架构中的部门（树形结构）
 * 对应前端 TaktDeptDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Dept
 * @description 对应后端 TaktDeptDto
 */
export interface Dept extends CompanyDtoBase {
  /**
   * DeptID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  deptId: string;

  /**
   * 部门编码（唯一索引：租户+公司内唯一，见 ix_dept_code_unique）
   */
  deptCode: string;

  /**
   * 部门简称（与 ISO 编码一致，长度 6）
   */
  deptShortName: string;

  /**
   * 部门名称1
   */
  deptName1: string;

  /**
   * 部门名称2
   */
  deptName2: string;

  /**
   * 父部门（关联 TaktDept.Id，选项 TaktDepts/tree-options；0=根部门）
   */
  parentId: string;

  /**
   * 层级（1=一级部门，2=二级部门，以此类推）
   */
  level: number;

  /**
   * 部门路径（如：/1/3/5/，用于快速查询子部门）
   */
  deptPath: string;

  /**
   * 叶子节点（字典 sys_yes_no；0=否 1=是）
   */
  isLeaf: number;

  /**
   * ISO 编码（与部门简称一致，长度 6）
   */
  isoCode: string;

  /**
   * 成本中心编码（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options）
   */
  costCenterCode: string;

  /**
   * 费用类别（字典 humanresource_organization_dept_cost_category；1=直接 2=间接）
   */
  costCategory: number;

  /**
   * 部门负责人（选项 TaktUsers/options，DictValue=Id）
   */
  headUserId: string;

  /**
   * 部门负责人名称（冗余：按 HeadUserId 取 TaktUser.NickName联动）
   */
  headUserName: string;

  /**
   * 联系电话
   */
  phone: string;

  /**
   * 邮箱
   */
  email: string;

  /**
   * 办公地点
   */
  location: string;

  /**
   * 内置（字典 sys_yes_no；0=否 1=是；种子部门为内置，不允许删除）
   */
  isBuiltIn: number;

  /**
   * 部门描述
   */
  deptDescription: string;

  /**
   * 排序号（回填）（同级部门排序）
   */
  sortOrder: number;

  /**
   * 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
   */
  deptStatus: number;

  /**
   * 角色数据权限关联该部门（RBAC，表 takt_human_resource_organization_roledept） （RBAC：TaktRoleDept）
   */
  roleDepts?: RoleDept[];

  /**
   * 员工部门关联（RBAC，表 takt_human_resource_organization_employeedept） （RBAC：TaktEmployeeDept）
   */
  employeeDepts?: EmployeeDept[];

}


/**
 * Dept 树形列表/树选择 DTO（含子节点）
 * 对应 GetDeptTreeAsync 等接口
 * 对应前端 DeptTree
 * @description 对应后端 TaktDeptTreeDto
 */
export interface DeptTree extends Dept {
  /**
   * 子节点（懒加载树接口返回 null，表示尚未加载；勿用空 List 冒充已加载）
   */
  children?: DeptTree[];

}


/**
 * Dept 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 DeptQuery
 * @description 对应后端 TaktDeptQueryDto
 */
export interface DeptQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 部门编码（唯一索引：租户+公司内唯一，见 ix_dept_code_unique）
   */
  deptCode?: string;

  /**
   * 部门简称（与 ISO 编码一致，长度 6）
   */
  deptShortName?: string;

  /**
   * 部门名称1
   */
  deptName1?: string;

  /**
   * 部门名称2
   */
  deptName2?: string;

  /**
   * 父部门（关联 TaktDept.Id，选项 TaktDepts/tree-options；0=根部门）
   */
  parentId?: string;

  /**
   * 层级（1=一级部门，2=二级部门，以此类推）
   */
  level?: number;

  /**
   * 部门路径（如：/1/3/5/，用于快速查询子部门）
   */
  deptPath?: string;

  /**
   * 叶子节点（字典 sys_yes_no；0=否 1=是）
   */
  isLeaf?: number;

  /**
   * ISO 编码（与部门简称一致，长度 6）
   */
  isoCode?: string;

  /**
   * 成本中心编码（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options）
   */
  costCenterCode?: string;

  /**
   * 费用类别（字典 humanresource_organization_dept_cost_category；1=直接 2=间接）
   */
  costCategory?: number;

  /**
   * 部门负责人（选项 TaktUsers/options，DictValue=Id）
   */
  headUserId?: string;

  /**
   * 部门负责人名称（冗余：按 HeadUserId 取 TaktUser.NickName联动）
   */
  headUserName?: string;

  /**
   * 联系电话
   */
  phone?: string;

  /**
   * 邮箱
   */
  email?: string;

  /**
   * 办公地点
   */
  location?: string;

  /**
   * 内置（字典 sys_yes_no；0=否 1=是；种子部门为内置，不允许删除）
   */
  isBuiltIn?: number;

  /**
   * 部门描述
   */
  deptDescription?: string;

  /**
   * 排序号（回填）（同级部门排序）
   */
  sortOrder?: number;

  /**
   * 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
   */
  deptStatus?: number;

  /**
   * 创建时间（范围查询-开始）
   */
  createdAtStart?: string;

  /**
   * 创建时间（范围查询-结束）
   */
  createdAtEnd?: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建Dept DTO
 * 对应前端 DeptCreate
 * @description 对应后端 TaktDeptCreateDto
 */
export interface DeptCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode: string;

  /**
   * 部门编码（唯一索引：租户+公司内唯一，见 ix_dept_code_unique）
   */
  deptCode: string;

  /**
   * 部门简称（与 ISO 编码一致，长度 6）
   */
  deptShortName: string;

  /**
   * 部门名称1
   */
  deptName1: string;

  /**
   * 部门名称2
   */
  deptName2: string;

  /**
   * 父部门（关联 TaktDept.Id，选项 TaktDepts/tree-options；0=根部门）
   */
  parentId: string;

  /**
   * ISO 编码（与部门简称一致，长度 6）
   */
  isoCode: string;

  /**
   * 成本中心编码（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options）
   */
  costCenterCode: string;

  /**
   * 费用类别（字典 humanresource_organization_dept_cost_category；1=直接 2=间接）
   */
  costCategory: number;

  /**
   * 部门负责人（选项 TaktUsers/options，DictValue=Id）
   */
  headUserId: string;

  /**
   * 部门负责人名称（冗余：按 HeadUserId 取 TaktUser.NickName联动）
   */
  headUserName: string;

  /**
   * 联系电话
   */
  phone: string;

  /**
   * 邮箱
   */
  email: string;

  /**
   * 办公地点
   */
  location: string;

  /**
   * 内置（字典 sys_yes_no；0=否 1=是；种子部门为内置，不允许删除）
   */
  isBuiltIn: number;

  /**
   * 部门描述
   */
  deptDescription: string;

  /**
   * 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
   */
  deptStatus: number;

  /**
   * 数据权限关联该部门的角色 ID 列表（RBAC 反向合并）
   */
  roleIds?: any;

  /**
   * 关联该部门的员工 ID 列表（RBAC 反向合并）
   */
  employeeIds?: any;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * 更新Dept DTO
 * 继承 TaktDeptCreateDto，添加 DeptId 字段
 * 对应前端 DeptUpdate
 * @description 对应后端 TaktDeptUpdateDto
 */
export interface DeptUpdate extends DeptCreate {
  /**
   * DeptID（标识要更新的实体）
   */
  deptId: string;

}


/**
 * Dept 状态更新 DTO
 * 对应前端 DeptStatus
 * @description 对应后端 TaktDeptStatusDto
 */
export interface DeptStatus {
  /**
   * DeptID
   */
  deptId: string;

  /**
   * 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
   */
  deptStatus: number;

}


/**
 * Dept 排序更新 DTO
 * 对应前端 DeptSort
 * @description 对应后端 TaktDeptSortDto
 */
export interface DeptSort {
  /**
   * DeptID
   */
  deptId: string;

  /**
   * 排序号（回填）（同级部门排序）
   */
  sortOrder: number;

}


/**
 * Dept 内置更新 DTO
 * 对应前端 DeptBuiltIn
 * @description 对应后端 TaktDeptBuiltInDto
 */
export interface DeptBuiltIn {
  /**
   * DeptID
   */
  deptId: string;

  /**
   * 内置（字典 sys_yes_no；1=是，0=否）
   */
  isBuiltIn: number;

}


/**
 * Dept 导入模板行 DTO
 * 对应前端 DeptTemplate
 * @description 对应后端 TaktDeptTemplateDto
 */
export interface DeptTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode?: string;

  /**
   * 部门编码（唯一索引：租户+公司内唯一，见 ix_dept_code_unique）
   */
  deptCode?: string;

  /**
   * 部门简称（与 ISO 编码一致，长度 6）
   */
  deptShortName?: string;

  /**
   * 部门名称1
   */
  deptName1?: string;

  /**
   * 部门名称2
   */
  deptName2?: string;

  /**
   * 父部门（关联 TaktDept.Id，选项 TaktDepts/tree-options；0=根部门）
   */
  parentId?: string;

  /**
   * ISO 编码（与部门简称一致，长度 6）
   */
  isoCode?: string;

  /**
   * 成本中心编码（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options）
   */
  costCenterCode?: string;

  /**
   * 费用类别（字典 humanresource_organization_dept_cost_category；1=直接 2=间接）
   */
  costCategory?: number;

  /**
   * 部门负责人（选项 TaktUsers/options，DictValue=Id）
   */
  headUserId?: string;

  /**
   * 部门负责人名称（冗余：按 HeadUserId 取 TaktUser.NickName联动）
   */
  headUserName?: string;

  /**
   * 联系电话
   */
  phone?: string;

  /**
   * 邮箱
   */
  email?: string;

  /**
   * 办公地点
   */
  location?: string;

  /**
   * 内置（字典 sys_yes_no；0=否 1=是；种子部门为内置，不允许删除）
   */
  isBuiltIn?: number;

  /**
   * 部门描述
   */
  deptDescription?: string;

  /**
   * 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
   */
  deptStatus?: number;

  /**
   * 数据权限关联该部门的角色 ID 列表（RBAC 反向合并）
   */
  roleIds?: any;

  /**
   * 关联该部门的员工 ID 列表（RBAC 反向合并）
   */
  employeeIds?: any;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * Dept 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 DeptImport
 * @description 对应后端 TaktDeptImportDto
 */
export interface DeptImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode?: string;

  /**
   * 部门编码（唯一索引：租户+公司内唯一，见 ix_dept_code_unique）
   */
  deptCode?: string;

  /**
   * 部门简称（与 ISO 编码一致，长度 6）
   */
  deptShortName?: string;

  /**
   * 部门名称1
   */
  deptName1?: string;

  /**
   * 部门名称2
   */
  deptName2?: string;

  /**
   * 父部门（关联 TaktDept.Id，选项 TaktDepts/tree-options；0=根部门）
   */
  parentId?: string;

  /**
   * ISO 编码（与部门简称一致，长度 6）
   */
  isoCode?: string;

  /**
   * 成本中心编码（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options）
   */
  costCenterCode?: string;

  /**
   * 费用类别（字典 humanresource_organization_dept_cost_category；1=直接 2=间接）
   */
  costCategory?: number;

  /**
   * 部门负责人（选项 TaktUsers/options，DictValue=Id）
   */
  headUserId?: string;

  /**
   * 部门负责人名称（冗余：按 HeadUserId 取 TaktUser.NickName联动）
   */
  headUserName?: string;

  /**
   * 联系电话
   */
  phone?: string;

  /**
   * 邮箱
   */
  email?: string;

  /**
   * 办公地点
   */
  location?: string;

  /**
   * 内置（字典 sys_yes_no；0=否 1=是；种子部门为内置，不允许删除）
   */
  isBuiltIn?: number;

  /**
   * 部门描述
   */
  deptDescription?: string;

  /**
   * 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
   */
  deptStatus?: number;

  /**
   * 数据权限关联该部门的角色 ID 列表（RBAC 反向合并）
   */
  roleIds?: any;

  /**
   * 关联该部门的员工 ID 列表（RBAC 反向合并）
   */
  employeeIds?: any;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * Dept 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 DeptExport
 * @description 对应后端 TaktDeptExportDto
 */
export interface DeptExport {
  /**
   * DeptID
   */
  deptId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 部门编码（唯一索引：租户+公司内唯一，见 ix_dept_code_unique）
   */
  deptCode: string;

  /**
   * 部门简称（与 ISO 编码一致，长度 6）
   */
  deptShortName: string;

  /**
   * 部门名称1
   */
  deptName1: string;

  /**
   * 部门名称2
   */
  deptName2: string;

  /**
   * 父部门（关联 TaktDept.Id，选项 TaktDepts/tree-options；0=根部门）
   */
  parentId: string;

  /**
   * 层级（1=一级部门，2=二级部门，以此类推）
   */
  level: number;

  /**
   * 部门路径（如：/1/3/5/，用于快速查询子部门）
   */
  deptPath: string;

  /**
   * 叶子节点（字典 sys_yes_no；0=否 1=是）
   */
  isLeaf: number;

  /**
   * ISO 编码（与部门简称一致，长度 6）
   */
  isoCode: string;

  /**
   * 成本中心编码（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options）
   */
  costCenterCode: string;

  /**
   * 费用类别（字典 humanresource_organization_dept_cost_category；1=直接 2=间接）
   */
  costCategory: number;

  /**
   * 部门负责人（选项 TaktUsers/options，DictValue=Id）
   */
  headUserId: string;

  /**
   * 部门负责人名称（冗余：按 HeadUserId 取 TaktUser.NickName联动）
   */
  headUserName: string;

  /**
   * 联系电话
   */
  phone: string;

  /**
   * 邮箱
   */
  email: string;

  /**
   * 办公地点
   */
  location: string;

  /**
   * 内置（字典 sys_yes_no；0=否 1=是；种子部门为内置，不允许删除）
   */
  isBuiltIn: number;

  /**
   * 部门描述
   */
  deptDescription: string;

  /**
   * 排序号（回填）（同级部门排序）
   */
  sortOrder: number;

  /**
   * 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
   */
  deptStatus: number;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

