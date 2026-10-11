// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/identity
// 文件名称：menu.types.ts
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
 * 菜单实体 代表系统菜单和权限（树形结构） 支持目录、菜单、按钮三种类型
 * 对应前端 TaktMenuDto
 * 继承 TaktTenantCoreDtoBase（组合 4）
 * 对应前端 Menu
 * @description 对应后端 TaktMenuDto
 */
export interface Menu extends TenantCoreDtoBase {
  /**
   * MenuID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  menuId: string;

  /**
   * 菜单编码（唯一索引：租户内唯一，见 ix_menu_code_unique）
   */
  menuCode: string;

  /**
   * 菜单名称
   */
  menuName: string;

  /**
   * 国际化键（用于多语言支持）
   */
  i18nKey: string;

  /**
   * 菜单图标
   */
  icon: string;

  /**
   * 父菜单ID（0表示根菜单）
   */
  parentId: string;

  /**
   * 层级（1=一级菜单，2=二级菜单，以此类推）
   */
  level: number;

  /**
   * 菜单路径（如：/100/1000/1001/，用于快速查询子菜单）
   */
  menuPath: string;

  /**
   * 是否叶子节点（0=否，1=是）
   */
  isLeaf: number;

  /**
   * 菜单类型（与 TaktMenuType 一致：0=目录，1=页面菜单，2=按钮）
   */
  menuType: number;

  /**
   * 权限标识（格式：module:resource:action）
   */
  permission: string;

  /**
   * 路由地址（前端路由）
   */
  routePath: string;

  /**
   * 组件路径（前端组件路径）
   */
  componentPath: string;

  /**
   * 排序号（同级菜单排序）
   */
  sortOrder: number;

  /**
   * 是否外部链接
   */
  isExternal: number;

  /**
   * 外部链接地址
   */
  externalUrl: string;

  /**
   * 是否缓存（前端keep-alive）
   */
  isCached: number;

  /**
   * 是否显示（0=隐藏，1=显示）
   */
  isVisible: number;

  /**
   * 状态（1=启用，0=禁用）
   */
  menuStatus: number;

  /**
   * 内置（1=是，0=否） 种子菜单为内置，不允许删除
   */
  isBuiltIn: number;

  /**
   * 菜单描述
   */
  menuDescription: string;

  /**
   * 拥有该菜单权限的角色关联（RBAC，表 takt_identity_role_menu） （子表：TaktRoleMenu）
   */
  roleMenus?: RoleMenu[];

}

/**
 * Menu 树形列表/树选择 DTO（含子节点）
 * 对应 GetMenuTreeAsync 等接口
 * 对应前端 MenuTree
 * @description 对应后端 TaktMenuTreeDto
 */
export interface MenuTree extends Menu {
  /**
   * 子节点
   */
  children: MenuTree[];

}

/**
 * Menu 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 MenuQuery
 * @description 对应后端 TaktMenuQueryDto
 */
export interface MenuQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  relatedPlant?: string;

  /**
   * 菜单编码（唯一索引：租户内唯一，见 ix_menu_code_unique）
   */
  menuCode?: string;

  /**
   * 菜单名称
   */
  menuName?: string;

  /**
   * 国际化键（用于多语言支持）
   */
  i18nKey?: string;

  /**
   * 菜单图标
   */
  icon?: string;

  /**
   * 父菜单ID（0表示根菜单）
   */
  parentId?: string;

  /**
   * 层级（1=一级菜单，2=二级菜单，以此类推）
   */
  level?: number;

  /**
   * 菜单路径（如：/100/1000/1001/，用于快速查询子菜单）
   */
  menuPath?: string;

  /**
   * 是否叶子节点（0=否，1=是）
   */
  isLeaf?: number;

  /**
   * 菜单类型（与 TaktMenuType 一致：0=目录，1=页面菜单，2=按钮）
   */
  menuType?: number;

  /**
   * 权限标识（格式：module:resource:action）
   */
  permission?: string;

  /**
   * 路由地址（前端路由）
   */
  routePath?: string;

  /**
   * 组件路径（前端组件路径）
   */
  componentPath?: string;

  /**
   * 排序号（同级菜单排序）
   */
  sortOrder?: number;

  /**
   * 是否外部链接
   */
  isExternal?: number;

  /**
   * 外部链接地址
   */
  externalUrl?: string;

  /**
   * 是否缓存（前端keep-alive）
   */
  isCached?: number;

  /**
   * 是否显示（0=隐藏，1=显示）
   */
  isVisible?: number;

  /**
   * 状态（1=启用，0=禁用）
   */
  menuStatus?: number;

  /**
   * 内置（1=是，0=否） 种子菜单为内置，不允许删除
   */
  isBuiltIn?: number;

  /**
   * 菜单描述
   */
  menuDescription?: string;

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
 * 创建Menu DTO
 * 对应前端 MenuCreate
 * @description 对应后端 TaktMenuCreateDto
 */
export interface MenuCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  relatedPlant: string;

  /**
   * 菜单编码（唯一索引：租户内唯一，见 ix_menu_code_unique）
   */
  menuCode: string;

  /**
   * 菜单名称
   */
  menuName: string;

  /**
   * 国际化键（用于多语言支持）
   */
  i18nKey: string;

  /**
   * 菜单图标
   */
  icon: string;

  /**
   * 父菜单ID（0表示根菜单）
   */
  parentId: string;

  /**
   * 菜单路径（如：/100/1000/1001/，用于快速查询子菜单）
   */
  menuPath: string;

  /**
   * 菜单类型（与 TaktMenuType 一致：0=目录，1=页面菜单，2=按钮）
   */
  menuType: number;

  /**
   * 权限标识（格式：module:resource:action）
   */
  permission: string;

  /**
   * 路由地址（前端路由）
   */
  routePath: string;

  /**
   * 组件路径（前端组件路径）
   */
  componentPath: string;

  /**
   * 是否外部链接
   */
  isExternal: number;

  /**
   * 外部链接地址
   */
  externalUrl: string;

  /**
   * 是否缓存（前端keep-alive）
   */
  isCached: number;

  /**
   * 是否显示（0=隐藏，1=显示）
   */
  isVisible: number;

  /**
   * 状态（1=启用，0=禁用）
   */
  menuStatus: number;

  /**
   * 内置（1=是，0=否） 种子菜单为内置，不允许删除
   */
  isBuiltIn: number;

  /**
   * 菜单描述
   */
  menuDescription: string;

  /**
   * 拥有该菜单权限的角色 ID 列表（RBAC 反向合并，分配走 ITaktRbacService）
   */
  roleIds?: any;

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
 * 更新Menu DTO
 * 继承 TaktMenuCreateDto，添加 MenuId 字段
 * 对应前端 MenuUpdate
 * @description 对应后端 TaktMenuUpdateDto
 */
export interface MenuUpdate extends MenuCreate {
  /**
   * MenuID（标识要更新的实体）
   */
  menuId: string;

}

/**
 * Menu 状态更新 DTO
 * 对应前端 MenuStatus
 * @description 对应后端 TaktMenuStatusDto
 */
export interface MenuStatus {
  /**
   * MenuID
   */
  menuId: string;

  /**
   * 状态（1=启用，0=禁用）
   */
  menuStatus: number;

}

/**
 * Menu 排序更新 DTO
 * 对应前端 MenuSort
 * @description 对应后端 TaktMenuSortDto
 */
export interface MenuSort {
  /**
   * MenuID
   */
  menuId: string;

  /**
   * 排序号（同级菜单排序）
   */
  sortOrder: number;

}

/**
 * Menu 导入模板行 DTO
 * 对应前端 MenuTemplate
 * @description 对应后端 TaktMenuTemplateDto
 */
export interface MenuTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  relatedPlant?: string;

  /**
   * 菜单编码（唯一索引：租户内唯一，见 ix_menu_code_unique）
   */
  menuCode?: string;

  /**
   * 菜单名称
   */
  menuName?: string;

  /**
   * 国际化键（用于多语言支持）
   */
  i18nKey?: string;

  /**
   * 菜单图标
   */
  icon?: string;

  /**
   * 父菜单ID（0表示根菜单）
   */
  parentId?: string;

  /**
   * 菜单路径（如：/100/1000/1001/，用于快速查询子菜单）
   */
  menuPath?: string;

  /**
   * 菜单类型（与 TaktMenuType 一致：0=目录，1=页面菜单，2=按钮）
   */
  menuType?: number;

  /**
   * 权限标识（格式：module:resource:action）
   */
  permission?: string;

  /**
   * 路由地址（前端路由）
   */
  routePath?: string;

  /**
   * 组件路径（前端组件路径）
   */
  componentPath?: string;

  /**
   * 是否外部链接
   */
  isExternal?: number;

  /**
   * 外部链接地址
   */
  externalUrl?: string;

  /**
   * 是否缓存（前端keep-alive）
   */
  isCached?: number;

  /**
   * 是否显示（0=隐藏，1=显示）
   */
  isVisible?: number;

  /**
   * 状态（1=启用，0=禁用）
   */
  menuStatus?: number;

  /**
   * 内置（1=是，0=否） 种子菜单为内置，不允许删除
   */
  isBuiltIn?: number;

  /**
   * 菜单描述
   */
  menuDescription?: string;

  /**
   * 拥有该菜单权限的角色 ID 列表（RBAC 反向合并，分配走 ITaktRbacService）
   */
  roleIds?: any;

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
 * Menu 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 MenuImport
 * @description 对应后端 TaktMenuImportDto
 */
export interface MenuImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  relatedPlant?: string;

  /**
   * 菜单编码（唯一索引：租户内唯一，见 ix_menu_code_unique）
   */
  menuCode?: string;

  /**
   * 菜单名称
   */
  menuName?: string;

  /**
   * 国际化键（用于多语言支持）
   */
  i18nKey?: string;

  /**
   * 菜单图标
   */
  icon?: string;

  /**
   * 父菜单ID（0表示根菜单）
   */
  parentId?: string;

  /**
   * 菜单路径（如：/100/1000/1001/，用于快速查询子菜单）
   */
  menuPath?: string;

  /**
   * 菜单类型（与 TaktMenuType 一致：0=目录，1=页面菜单，2=按钮）
   */
  menuType?: number;

  /**
   * 权限标识（格式：module:resource:action）
   */
  permission?: string;

  /**
   * 路由地址（前端路由）
   */
  routePath?: string;

  /**
   * 组件路径（前端组件路径）
   */
  componentPath?: string;

  /**
   * 是否外部链接
   */
  isExternal?: number;

  /**
   * 外部链接地址
   */
  externalUrl?: string;

  /**
   * 是否缓存（前端keep-alive）
   */
  isCached?: number;

  /**
   * 是否显示（0=隐藏，1=显示）
   */
  isVisible?: number;

  /**
   * 状态（1=启用，0=禁用）
   */
  menuStatus?: number;

  /**
   * 内置（1=是，0=否） 种子菜单为内置，不允许删除
   */
  isBuiltIn?: number;

  /**
   * 菜单描述
   */
  menuDescription?: string;

  /**
   * 拥有该菜单权限的角色 ID 列表（RBAC 反向合并，分配走 ITaktRbacService）
   */
  roleIds?: any;

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
 * Menu 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MenuExport
 * @description 对应后端 TaktMenuExportDto
 */
export interface MenuExport {
  /**
   * MenuID
   */
  menuId: string;

  /**
   * 菜单编码（唯一索引：租户内唯一，见 ix_menu_code_unique）
   */
  menuCode: string;

  /**
   * 菜单名称
   */
  menuName: string;

  /**
   * 国际化键（用于多语言支持）
   */
  i18nKey: string;

  /**
   * 菜单图标
   */
  icon: string;

  /**
   * 父菜单ID（0表示根菜单）
   */
  parentId: string;

  /**
   * 层级（1=一级菜单，2=二级菜单，以此类推）
   */
  level: number;

  /**
   * 菜单路径（如：/100/1000/1001/，用于快速查询子菜单）
   */
  menuPath: string;

  /**
   * 是否叶子节点（0=否，1=是）
   */
  isLeaf: number;

  /**
   * 菜单类型（与 TaktMenuType 一致：0=目录，1=页面菜单，2=按钮）
   */
  menuType: number;

  /**
   * 权限标识（格式：module:resource:action）
   */
  permission: string;

  /**
   * 路由地址（前端路由）
   */
  routePath: string;

  /**
   * 组件路径（前端组件路径）
   */
  componentPath: string;

  /**
   * 排序号（同级菜单排序）
   */
  sortOrder: number;

  /**
   * 是否外部链接
   */
  isExternal: number;

  /**
   * 外部链接地址
   */
  externalUrl: string;

  /**
   * 是否缓存（前端keep-alive）
   */
  isCached: number;

  /**
   * 是否显示（0=隐藏，1=显示）
   */
  isVisible: number;

  /**
   * 状态（1=启用，0=禁用）
   */
  menuStatus: number;

  /**
   * 内置（1=是，0=否） 种子菜单为内置，不允许删除
   */
  isBuiltIn: number;

  /**
   * 菜单描述
   */
  menuDescription: string;

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

