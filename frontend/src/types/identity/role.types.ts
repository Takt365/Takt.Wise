// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/identity
// 文件名称：role.types.ts
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：identity 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  TaktPagedQuery,
  TenantCoreDtoBase
} from '@/types/common.types';

/**
 * 角色实体 代表系统角色（RBAC权限模型）
 * 对应前端 TaktRoleDto
 * 继承 TaktTenantCoreDtoBase（组合 4）
 * 对应前端 Role
 * @description 对应后端 TaktRoleDto
 */
export interface Role extends TenantCoreDtoBase {
  /**
   * RoleID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  roleId: string;

  /**
   * 角色编码（唯一索引：租户内唯一，见 ix_role_code_unique）
   */
  roleCode: string;

  /**
   * 角色名称
   */
  roleName: string;

  /**
   * 数据权限范围（字典 sys_data_scope：0=全部数据，1=本部门，2=本部门及以下，3=仅本人，4=自定义）
   */
  dataScope: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 内置（字典 sys_yes_no；种子角色为内置，不允许删除）
   */
  isBuiltIn: number;

  /**
   * 状态（字典 sys_normal_disable）
   */
  roleStatus: number;

  /**
   * 角色描述
   */
  roleDescription?: string;

  /**
   * 角色菜单权限关联（RBAC，表 takt_identity_role_menu） （子表：TaktRoleMenu）
   */
  roleMenus?: RoleMenu[];

  /**
   * 角色可访问公司关联（RBAC，表 takt_identity_role_company） （子表：TaktRoleCompany）
   */
  roleCompanies?: RoleCompany[];

  /**
   * 自定义数据权限关联部门（RBAC，表 takt_human_resource_organization_roledept） （子表：TaktRoleDept）
   */
  roleDepts?: RoleDept[];

  /**
   * 拥有该角色的用户关联（RBAC，表 takt_identity_user_role） （子表：TaktUserRole）
   */
  userRoles?: UserRole[];

}

/**
 * Role 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 RoleQuery
 * @description 对应后端 TaktRoleQueryDto
 */
export interface RoleQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  relatedPlant?: string;

  /**
   * 角色编码（唯一索引：租户内唯一，见 ix_role_code_unique）
   */
  roleCode?: string;

  /**
   * 角色名称
   */
  roleName?: string;

  /**
   * 数据权限范围（字典 sys_data_scope：0=全部数据，1=本部门，2=本部门及以下，3=仅本人，4=自定义）
   */
  dataScope?: number;

  /**
   * 排序号
   */
  sortOrder?: number;

  /**
   * 内置（字典 sys_yes_no；种子角色为内置，不允许删除）
   */
  isBuiltIn?: number;

  /**
   * 状态（字典 sys_normal_disable）
   */
  roleStatus?: number;

  /**
   * 角色描述
   */
  roleDescription?: string;

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
 * 创建Role DTO
 * 对应前端 RoleCreate
 * @description 对应后端 TaktRoleCreateDto
 */
export interface RoleCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  relatedPlant: string;

  /**
   * 角色编码（唯一索引：租户内唯一，见 ix_role_code_unique）
   */
  roleCode: string;

  /**
   * 角色名称
   */
  roleName: string;

  /**
   * 数据权限范围（字典 sys_data_scope：0=全部数据，1=本部门，2=本部门及以下，3=仅本人，4=自定义）
   */
  dataScope: number;

  /**
   * 内置（字典 sys_yes_no；种子角色为内置，不允许删除）
   */
  isBuiltIn: number;

  /**
   * 状态（字典 sys_normal_disable）
   */
  roleStatus: number;

  /**
   * 角色描述
   */
  roleDescription?: string;

  /**
   * 角色菜单权限关联（RBAC 全量覆盖，分配走 ITaktRbacService）
   */
  roleMenuIds?: any;

  /**
   * 角色可访问公司关联（RBAC 全量覆盖，分配走 ITaktRbacService）
   */
  roleCompanyCodes?: any;

  /**
   * 自定义数据权限关联部门（RBAC 全量覆盖，分配走 ITaktRbacService）
   */
  roleDeptIds?: any;

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
 * 更新Role DTO
 * 继承 TaktRoleCreateDto，添加 RoleId 字段
 * 对应前端 RoleUpdate
 * @description 对应后端 TaktRoleUpdateDto
 */
export interface RoleUpdate extends RoleCreate {
  /**
   * RoleID（标识要更新的实体）
   */
  roleId: string;

}

/**
 * Role 状态更新 DTO
 * 对应前端 RoleStatus
 * @description 对应后端 TaktRoleStatusDto
 */
export interface RoleStatus {
  /**
   * RoleID
   */
  roleId: string;

  /**
   * 状态（字典 sys_normal_disable）
   */
  roleStatus: number;

}

/**
 * Role 排序更新 DTO
 * 对应前端 RoleSort
 * @description 对应后端 TaktRoleSortDto
 */
export interface RoleSort {
  /**
   * RoleID
   */
  roleId: string;

  /**
   * 排序号
   */
  sortOrder: number;

}

/**
 * Role 导入模板行 DTO
 * 对应前端 RoleTemplate
 * @description 对应后端 TaktRoleTemplateDto
 */
export interface RoleTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  relatedPlant?: string;

  /**
   * 角色编码（唯一索引：租户内唯一，见 ix_role_code_unique）
   */
  roleCode?: string;

  /**
   * 角色名称
   */
  roleName?: string;

  /**
   * 数据权限范围（字典 sys_data_scope：0=全部数据，1=本部门，2=本部门及以下，3=仅本人，4=自定义）
   */
  dataScope?: number;

  /**
   * 内置（字典 sys_yes_no；种子角色为内置，不允许删除）
   */
  isBuiltIn?: number;

  /**
   * 状态（字典 sys_normal_disable）
   */
  roleStatus?: number;

  /**
   * 角色描述
   */
  roleDescription?: string;

  /**
   * 角色菜单权限关联（RBAC 全量覆盖，分配走 ITaktRbacService）
   */
  roleMenuIds?: any;

  /**
   * 角色可访问公司关联（RBAC 全量覆盖，分配走 ITaktRbacService）
   */
  roleCompanyCodes?: any;

  /**
   * 自定义数据权限关联部门（RBAC 全量覆盖，分配走 ITaktRbacService）
   */
  roleDeptIds?: any;

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
 * Role 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 RoleImport
 * @description 对应后端 TaktRoleImportDto
 */
export interface RoleImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  relatedPlant?: string;

  /**
   * 角色编码（唯一索引：租户内唯一，见 ix_role_code_unique）
   */
  roleCode?: string;

  /**
   * 角色名称
   */
  roleName?: string;

  /**
   * 数据权限范围（字典 sys_data_scope：0=全部数据，1=本部门，2=本部门及以下，3=仅本人，4=自定义）
   */
  dataScope?: number;

  /**
   * 内置（字典 sys_yes_no；种子角色为内置，不允许删除）
   */
  isBuiltIn?: number;

  /**
   * 状态（字典 sys_normal_disable）
   */
  roleStatus?: number;

  /**
   * 角色描述
   */
  roleDescription?: string;

  /**
   * 角色菜单权限关联（RBAC 全量覆盖，分配走 ITaktRbacService）
   */
  roleMenuIds?: any;

  /**
   * 角色可访问公司关联（RBAC 全量覆盖，分配走 ITaktRbacService）
   */
  roleCompanyCodes?: any;

  /**
   * 自定义数据权限关联部门（RBAC 全量覆盖，分配走 ITaktRbacService）
   */
  roleDeptIds?: any;

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
 * Role 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 RoleExport
 * @description 对应后端 TaktRoleExportDto
 */
export interface RoleExport {
  /**
   * RoleID
   */
  roleId: string;

  /**
   * 角色编码（唯一索引：租户内唯一，见 ix_role_code_unique）
   */
  roleCode: string;

  /**
   * 角色名称
   */
  roleName: string;

  /**
   * 数据权限范围（字典 sys_data_scope：0=全部数据，1=本部门，2=本部门及以下，3=仅本人，4=自定义）
   */
  dataScope: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 内置（字典 sys_yes_no；种子角色为内置，不允许删除）
   */
  isBuiltIn: number;

  /**
   * 状态（字典 sys_normal_disable）
   */
  roleStatus: number;

  /**
   * 角色描述
   */
  roleDescription?: string;

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

