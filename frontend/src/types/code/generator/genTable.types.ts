// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/code/generator
// 文件名称：genTable.types.ts
// 创建时间：2026-08-12
// 创建人：Takt365(Auto Generated)
// 功能描述：code/generator 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  TaktPagedQuery,
  TenantCoreDtoBase
} from '@/types/common.types';

/**
 * Takt代码生成表配置实体 特例：继承组合 4：无关联工厂、无语言（TaktTenantCoreEntityBase）
 * 对应前端 TaktGenTableDto
 * 继承 TaktTenantCoreDtoBase
 * 对应前端 GenTable
 * @description 对应后端 TaktGenTableDto
 */
export interface GenTable extends TenantCoreDtoBase {
  /**
   * GenTableID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  genTableId: string;

  /**
   * 数据源（选项 TaktDatabaseInfos/list；持久化 displayName:tenantCode）
   */
  dataSource: string;

  /**
   * 表名称（选项 TaktDatabaseInfos/tables；新建可手输；租户内与 DataSource 唯一）
   */
  tableName: string;

  /**
   * 表描述（表注释）
   */
  tableComment?: string;

  /**
   * 关联父表（选项 TaktDatabaseInfos/tables 同库其他表；sub 模板必填）
   */
  subTableName?: string;

  /**
   * 关联外键（选项本表 columnList.databaseColumnName；sub 模板必填）
   */
  subTableFkName?: string;

  /**
   * 树编码（选项本表 columnList.databaseColumnName；tree 模板必填）
   */
  treeCode?: string;

  /**
   * 树父编码（选项本表 columnList.databaseColumnName；tree 模板必填）
   */
  treeParentCode?: string;

  /**
   * 树名称（选项本表 columnList.databaseColumnName；tree 模板必填）
   */
  treeName?: string;

  /**
   * 库表标识（字典 sys_yes_no；0=否 1=是）
   */
  inDatabase: number;

  /**
   * 生成模板类型（字典 code_generator_template_type；crud/sub/tree）
   */
  genTemplateCategory: string;

  /**
   * 模块名（功能模块名称）
   */
  genModuleName?: string;

  /**
   * 业务名（用于类名，如 Company，与模块拼接为 Takt.模块+类名）
   */
  genBusinessName: string;

  /**
   * 功能名（用于接口与注释的中文名称，如：公司、部门）
   */
  genFunctionName?: string;

  /**
   * 权限前缀（与生成控制器/菜单/前端权限一致；对应库列 <c>perms_prefix</c>）。
   */
  permsPrefix: string;

  /**
   * 菜单权限组（字典 code_generator_button_category 多选逗号；仅用于生成 menu_and_translation.sql 按钮 INSERT，不参与控制器/前端代码生成）
   */
  menuButtonGroup?: string;

  /**
   * 命名空间前缀（用于生成类名、方法名等的前缀）
   */
  namePrefix?: string;

  /**
   * 实体命名空间（默认当前项目：Takt.Domain.Entities）
   */
  entityNamespace?: string;

  /**
   * 实体类名称（首字母大写，驼峰命名）
   */
  entityClassName: string;

  /**
   * 传输对象Dto命名空间（默认当前项目：Takt.Application.Dtos）
   */
  dtoNamespace?: string;

  /**
   * 传输对象 Dto 类名
   */
  dtoClassName?: string;

  /**
   * 服务命名空间（默认当前项目：Takt.Application.Services）
   */
  serviceNamespace?: string;

  /**
   * 服务接口类名称
   */
  iServiceClassName?: string;

  /**
   * 服务类名称
   */
  serviceClassName?: string;

  /**
   * 控制器命名空间（默认当前项目：Takt.WebApi.Controllers）
   */
  controllerNamespace?: string;

  /**
   * 控制器类名称
   */
  controllerClassName?: string;

  /**
   * 仓储层（字典 sys_yes_no；0=否 1=是）
   */
  isRepository: number;

  /**
   * 仓储接口命名空间（默认当前项目：Takt.Domain.Repositories）
   */
  repositoryInterfaceNamespace?: string;

  /**
   * 仓储接口类名称
   */
  iRepositoryClassName?: string;

  /**
   * 仓储命名空间（默认当前项目：Takt.Infrastructure.Repositories）
   */
  repositoryNamespace?: string;

  /**
   * 仓储类名称
   */
  repositoryClassName?: string;

  /**
   * 生成功能（字典 code_generator_function 多选逗号；亦支持 JSON/数组）。 核心设计：GenFunction 不仅决定生成哪些 Controller Actions 和 Service Methods，还决定生成哪些 DTO 类。功能与 DTO 的映射关系如下： Query → QueryDto；Create → CreateDto；Update → UpdateDto；Status → StatusDto；Sort → SortDto； Import → TemplateDto + ImportDto；Export → ExportDto；所有功能 → Dto（基础传输对象，包含所有字段）。
   */
  genFunction?: string;

  /**
   * 生成方式（字典 code_generator_method；0=zip 1=自定义路径 2=当前项目）
   */
  genMethod: number;

  /**
   * 生成路径（字典 code_generator_path_type；GenMethod=1 时选择；0 默认 /；2 由 GenMethod 解析）
   */
  genPath: string;

  /**
   * 生成菜单（字典 sys_yes_no；0=否 1=是）
   */
  isGenMenu: number;

  /**
   * 上级菜单（关联 TaktMenu.Id，选项 TaktMenus/tree-options）
   */
  parentMenuId: string;

  /**
   * 上级菜单（关联 TaktMenu.Id，选项 TaktMenus/tree-options）
   */
  parentMenuName?: string;

  /**
   * 生成翻译（字典 sys_yes_no；0=否 1=是）
   */
  isGenTranslation: number;

  /**
   * 排序字段（选项本表 columnList.databaseColumnName）
   */
  sortField: string;

  /**
   * 排序方向（字典 sys_sort_type；ASC=升序 DESC=降序）
   */
  sortType: string;

  /**
   * 前端UI框架（字典 code_generator_frontend_ui_type；1=element plus 2=ant design vue）
   */
  frontUi: number;

  /**
   * 前端表单布局（字典 code_generator_frontend_form_layout；12=一行一列 24=一行两列）
   */
  frontFormLayout: number;

  /**
   * 前端按钮样式（字典 code_generator_button_style；0=文本 1=标准）
   */
  frontBtnStyle: number;

  /**
   * 是否生成（字典 sys_yes_no；0=否 1=是）
   */
  isGenCode: number;

  /**
   * 代码生成次数（每次生成成功后自增）
   */
  genCodeCount: number;

  /**
   * 使用tabs（字典 sys_yes_no；0=否 1=是）
   */
  isUseTabs: number;

  /**
   * tabs标签中字段的数量
   */
  tabsFieldCount: number;

  /**
   * 作者
   */
  genAuthor: string;

  /**
   * 其他生成选项（JSON格式，存储其他生成配置）
   */
  otherGenOptions?: string;

  /**
   * 字段配置列表（子表，外键：TaktGenTableColumn.GenTableId 关联本表 Id） （子表：TaktGenTableColumn）
   */
  columns?: GenTableColumn[];

}


/**
 * GenTable 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 GenTableQuery
 * @description 对应后端 TaktGenTableQueryDto
 */
export interface GenTableQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 数据源（选项 TaktDatabaseInfos/list；持久化 displayName:tenantCode）
   */
  dataSource?: string;

  /**
   * 表名称（选项 TaktDatabaseInfos/tables；新建可手输；租户内与 DataSource 唯一）
   */
  tableName?: string;

  /**
   * 表描述（表注释）
   */
  tableComment?: string;

  /**
   * 关联父表（选项 TaktDatabaseInfos/tables 同库其他表；sub 模板必填）
   */
  subTableName?: string;

  /**
   * 关联外键（选项本表 columnList.databaseColumnName；sub 模板必填）
   */
  subTableFkName?: string;

  /**
   * 树编码（选项本表 columnList.databaseColumnName；tree 模板必填）
   */
  treeCode?: string;

  /**
   * 树父编码（选项本表 columnList.databaseColumnName；tree 模板必填）
   */
  treeParentCode?: string;

  /**
   * 树名称（选项本表 columnList.databaseColumnName；tree 模板必填）
   */
  treeName?: string;

  /**
   * 库表标识（字典 sys_yes_no；0=否 1=是）
   */
  inDatabase?: number;

  /**
   * 生成模板类型（字典 code_generator_template_type；crud/sub/tree）
   */
  genTemplateCategory?: string;

  /**
   * 模块名（功能模块名称）
   */
  genModuleName?: string;

  /**
   * 业务名（用于类名，如 Company，与模块拼接为 Takt.模块+类名）
   */
  genBusinessName?: string;

  /**
   * 功能名（用于接口与注释的中文名称，如：公司、部门）
   */
  genFunctionName?: string;

  /**
   * 权限前缀（与生成控制器/菜单/前端权限一致；对应库列 <c>perms_prefix</c>）。
   */
  permsPrefix?: string;

  /**
   * 菜单权限组（字典 code_generator_button_category 多选逗号；仅用于生成 menu_and_translation.sql 按钮 INSERT，不参与控制器/前端代码生成）
   */
  menuButtonGroup?: string;

  /**
   * 命名空间前缀（用于生成类名、方法名等的前缀）
   */
  namePrefix?: string;

  /**
   * 实体命名空间（默认当前项目：Takt.Domain.Entities）
   */
  entityNamespace?: string;

  /**
   * 实体类名称（首字母大写，驼峰命名）
   */
  entityClassName?: string;

  /**
   * 传输对象Dto命名空间（默认当前项目：Takt.Application.Dtos）
   */
  dtoNamespace?: string;

  /**
   * 传输对象 Dto 类名
   */
  dtoClassName?: string;

  /**
   * 服务命名空间（默认当前项目：Takt.Application.Services）
   */
  serviceNamespace?: string;

  /**
   * 服务接口类名称
   */
  iServiceClassName?: string;

  /**
   * 服务类名称
   */
  serviceClassName?: string;

  /**
   * 控制器命名空间（默认当前项目：Takt.WebApi.Controllers）
   */
  controllerNamespace?: string;

  /**
   * 控制器类名称
   */
  controllerClassName?: string;

  /**
   * 仓储层（字典 sys_yes_no；0=否 1=是）
   */
  isRepository?: number;

  /**
   * 仓储接口命名空间（默认当前项目：Takt.Domain.Repositories）
   */
  repositoryInterfaceNamespace?: string;

  /**
   * 仓储接口类名称
   */
  iRepositoryClassName?: string;

  /**
   * 仓储命名空间（默认当前项目：Takt.Infrastructure.Repositories）
   */
  repositoryNamespace?: string;

  /**
   * 仓储类名称
   */
  repositoryClassName?: string;

  /**
   * 生成功能（字典 code_generator_function 多选逗号；亦支持 JSON/数组）。 核心设计：GenFunction 不仅决定生成哪些 Controller Actions 和 Service Methods，还决定生成哪些 DTO 类。功能与 DTO 的映射关系如下： Query → QueryDto；Create → CreateDto；Update → UpdateDto；Status → StatusDto；Sort → SortDto； Import → TemplateDto + ImportDto；Export → ExportDto；所有功能 → Dto（基础传输对象，包含所有字段）。
   */
  genFunction?: string;

  /**
   * 生成方式（字典 code_generator_method；0=zip 1=自定义路径 2=当前项目）
   */
  genMethod?: number;

  /**
   * 生成路径（字典 code_generator_path_type；GenMethod=1 时选择；0 默认 /；2 由 GenMethod 解析）
   */
  genPath?: string;

  /**
   * 生成菜单（字典 sys_yes_no；0=否 1=是）
   */
  isGenMenu?: number;

  /**
   * 上级菜单（关联 TaktMenu.Id，选项 TaktMenus/tree-options）
   */
  parentMenuId?: string;

  /**
   * 生成翻译（字典 sys_yes_no；0=否 1=是）
   */
  isGenTranslation?: number;

  /**
   * 排序字段（选项本表 columnList.databaseColumnName）
   */
  sortField?: string;

  /**
   * 排序方向（字典 sys_sort_type；ASC=升序 DESC=降序）
   */
  sortType?: string;

  /**
   * 前端UI框架（字典 code_generator_frontend_ui_type；1=element plus 2=ant design vue）
   */
  frontUi?: number;

  /**
   * 前端表单布局（字典 code_generator_frontend_form_layout；12=一行一列 24=一行两列）
   */
  frontFormLayout?: number;

  /**
   * 前端按钮样式（字典 code_generator_button_style；0=文本 1=标准）
   */
  frontBtnStyle?: number;

  /**
   * 是否生成（字典 sys_yes_no；0=否 1=是）
   */
  isGenCode?: number;

  /**
   * 代码生成次数（每次生成成功后自增）
   */
  genCodeCount?: number;

  /**
   * 使用tabs（字典 sys_yes_no；0=否 1=是）
   */
  isUseTabs?: number;

  /**
   * tabs标签中字段的数量
   */
  tabsFieldCount?: number;

  /**
   * 作者
   */
  genAuthor?: string;

  /**
   * 其他生成选项（JSON格式，存储其他生成配置）
   */
  otherGenOptions?: string;

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
 * 创建GenTable DTO
 * 对应前端 GenTableCreate
 * @description 对应后端 TaktGenTableCreateDto
 */
export interface GenTableCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 数据源（选项 TaktDatabaseInfos/list；持久化 displayName:tenantCode）
   */
  dataSource: string;

  /**
   * 表名称（选项 TaktDatabaseInfos/tables；新建可手输；租户内与 DataSource 唯一）
   */
  tableName: string;

  /**
   * 表描述（表注释）
   */
  tableComment?: string;

  /**
   * 关联父表（选项 TaktDatabaseInfos/tables 同库其他表；sub 模板必填）
   */
  subTableName?: string;

  /**
   * 关联外键（选项本表 columnList.databaseColumnName；sub 模板必填）
   */
  subTableFkName?: string;

  /**
   * 树编码（选项本表 columnList.databaseColumnName；tree 模板必填）
   */
  treeCode?: string;

  /**
   * 树父编码（选项本表 columnList.databaseColumnName；tree 模板必填）
   */
  treeParentCode?: string;

  /**
   * 树名称（选项本表 columnList.databaseColumnName；tree 模板必填）
   */
  treeName?: string;

  /**
   * 库表标识（字典 sys_yes_no；0=否 1=是）
   */
  inDatabase: number;

  /**
   * 生成模板类型（字典 code_generator_template_type；crud/sub/tree）
   */
  genTemplateCategory: string;

  /**
   * 模块名（功能模块名称）
   */
  genModuleName?: string;

  /**
   * 业务名（用于类名，如 Company，与模块拼接为 Takt.模块+类名）
   */
  genBusinessName: string;

  /**
   * 功能名（用于接口与注释的中文名称，如：公司、部门）
   */
  genFunctionName?: string;

  /**
   * 权限前缀（与生成控制器/菜单/前端权限一致；对应库列 <c>perms_prefix</c>）。
   */
  permsPrefix: string;

  /**
   * 菜单权限组（字典 code_generator_button_category 多选逗号；仅用于生成 menu_and_translation.sql 按钮 INSERT，不参与控制器/前端代码生成）
   */
  menuButtonGroup?: string;

  /**
   * 命名空间前缀（用于生成类名、方法名等的前缀）
   */
  namePrefix?: string;

  /**
   * 实体命名空间（默认当前项目：Takt.Domain.Entities）
   */
  entityNamespace?: string;

  /**
   * 实体类名称（首字母大写，驼峰命名）
   */
  entityClassName: string;

  /**
   * 传输对象Dto命名空间（默认当前项目：Takt.Application.Dtos）
   */
  dtoNamespace?: string;

  /**
   * 传输对象 Dto 类名
   */
  dtoClassName?: string;

  /**
   * 服务命名空间（默认当前项目：Takt.Application.Services）
   */
  serviceNamespace?: string;

  /**
   * 服务接口类名称
   */
  iServiceClassName?: string;

  /**
   * 服务类名称
   */
  serviceClassName?: string;

  /**
   * 控制器命名空间（默认当前项目：Takt.WebApi.Controllers）
   */
  controllerNamespace?: string;

  /**
   * 控制器类名称
   */
  controllerClassName?: string;

  /**
   * 仓储层（字典 sys_yes_no；0=否 1=是）
   */
  isRepository: number;

  /**
   * 仓储接口命名空间（默认当前项目：Takt.Domain.Repositories）
   */
  repositoryInterfaceNamespace?: string;

  /**
   * 仓储接口类名称
   */
  iRepositoryClassName?: string;

  /**
   * 仓储命名空间（默认当前项目：Takt.Infrastructure.Repositories）
   */
  repositoryNamespace?: string;

  /**
   * 仓储类名称
   */
  repositoryClassName?: string;

  /**
   * 生成功能（字典 code_generator_function 多选逗号；亦支持 JSON/数组）。 核心设计：GenFunction 不仅决定生成哪些 Controller Actions 和 Service Methods，还决定生成哪些 DTO 类。功能与 DTO 的映射关系如下： Query → QueryDto；Create → CreateDto；Update → UpdateDto；Status → StatusDto；Sort → SortDto； Import → TemplateDto + ImportDto；Export → ExportDto；所有功能 → Dto（基础传输对象，包含所有字段）。
   */
  genFunction?: string;

  /**
   * 生成方式（字典 code_generator_method；0=zip 1=自定义路径 2=当前项目）
   */
  genMethod: number;

  /**
   * 生成路径（字典 code_generator_path_type；GenMethod=1 时选择；0 默认 /；2 由 GenMethod 解析）
   */
  genPath: string;

  /**
   * 生成菜单（字典 sys_yes_no；0=否 1=是）
   */
  isGenMenu: number;

  /**
   * 上级菜单（关联 TaktMenu.Id，选项 TaktMenus/tree-options）
   */
  parentMenuId: string;

  /**
   * 生成翻译（字典 sys_yes_no；0=否 1=是）
   */
  isGenTranslation: number;

  /**
   * 排序字段（选项本表 columnList.databaseColumnName）
   */
  sortField: string;

  /**
   * 排序方向（字典 sys_sort_type；ASC=升序 DESC=降序）
   */
  sortType: string;

  /**
   * 前端UI框架（字典 code_generator_frontend_ui_type；1=element plus 2=ant design vue）
   */
  frontUi: number;

  /**
   * 前端表单布局（字典 code_generator_frontend_form_layout；12=一行一列 24=一行两列）
   */
  frontFormLayout: number;

  /**
   * 前端按钮样式（字典 code_generator_button_style；0=文本 1=标准）
   */
  frontBtnStyle: number;

  /**
   * 是否生成（字典 sys_yes_no；0=否 1=是）
   */
  isGenCode: number;

  /**
   * 代码生成次数（每次生成成功后自增）
   */
  genCodeCount: number;

  /**
   * 使用tabs（字典 sys_yes_no；0=否 1=是）
   */
  isUseTabs: number;

  /**
   * tabs标签中字段的数量
   */
  tabsFieldCount: number;

  /**
   * 作者
   */
  genAuthor: string;

  /**
   * 其他生成选项（JSON格式，存储其他生成配置）
   */
  otherGenOptions?: string;

  /**
   * 字段配置列表（子表，外键：TaktGenTableColumn.GenTableId 关联本表 Id）（子表，级联保存）
   */
  columns?: GenTableColumnCreate[];

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
 * 更新GenTable DTO
 * 继承 TaktGenTableCreateDto，添加 GenTableId 字段
 * 对应前端 GenTableUpdate
 * @description 对应后端 TaktGenTableUpdateDto
 */
export interface GenTableUpdate extends GenTableCreate {
  /**
   * GenTableID（标识要更新的实体）
   */
  genTableId: string;

  /**
   * 字段配置列表（子表，外键：TaktGenTableColumn.GenTableId 关联本表 Id）（子表，级联保存）
   */
  columns?: any;

}


/**
 * GenTable 导入模板行 DTO
 * 对应前端 GenTableTemplate
 * @description 对应后端 TaktGenTableTemplateDto
 */
export interface GenTableTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 数据源（选项 TaktDatabaseInfos/list；持久化 displayName:tenantCode）
   */
  dataSource?: string;

  /**
   * 表名称（选项 TaktDatabaseInfos/tables；新建可手输；租户内与 DataSource 唯一）
   */
  tableName?: string;

  /**
   * 表描述（表注释）
   */
  tableComment?: string;

  /**
   * 关联父表（选项 TaktDatabaseInfos/tables 同库其他表；sub 模板必填）
   */
  subTableName?: string;

  /**
   * 关联外键（选项本表 columnList.databaseColumnName；sub 模板必填）
   */
  subTableFkName?: string;

  /**
   * 树编码（选项本表 columnList.databaseColumnName；tree 模板必填）
   */
  treeCode?: string;

  /**
   * 树父编码（选项本表 columnList.databaseColumnName；tree 模板必填）
   */
  treeParentCode?: string;

  /**
   * 树名称（选项本表 columnList.databaseColumnName；tree 模板必填）
   */
  treeName?: string;

  /**
   * 库表标识（字典 sys_yes_no；0=否 1=是）
   */
  inDatabase?: number;

  /**
   * 生成模板类型（字典 code_generator_template_type；crud/sub/tree）
   */
  genTemplateCategory?: string;

  /**
   * 模块名（功能模块名称）
   */
  genModuleName?: string;

  /**
   * 业务名（用于类名，如 Company，与模块拼接为 Takt.模块+类名）
   */
  genBusinessName?: string;

  /**
   * 功能名（用于接口与注释的中文名称，如：公司、部门）
   */
  genFunctionName?: string;

  /**
   * 权限前缀（与生成控制器/菜单/前端权限一致；对应库列 <c>perms_prefix</c>）。
   */
  permsPrefix?: string;

  /**
   * 菜单权限组（字典 code_generator_button_category 多选逗号；仅用于生成 menu_and_translation.sql 按钮 INSERT，不参与控制器/前端代码生成）
   */
  menuButtonGroup?: string;

  /**
   * 命名空间前缀（用于生成类名、方法名等的前缀）
   */
  namePrefix?: string;

  /**
   * 实体命名空间（默认当前项目：Takt.Domain.Entities）
   */
  entityNamespace?: string;

  /**
   * 实体类名称（首字母大写，驼峰命名）
   */
  entityClassName?: string;

  /**
   * 传输对象Dto命名空间（默认当前项目：Takt.Application.Dtos）
   */
  dtoNamespace?: string;

  /**
   * 传输对象 Dto 类名
   */
  dtoClassName?: string;

  /**
   * 服务命名空间（默认当前项目：Takt.Application.Services）
   */
  serviceNamespace?: string;

  /**
   * 服务接口类名称
   */
  iServiceClassName?: string;

  /**
   * 服务类名称
   */
  serviceClassName?: string;

  /**
   * 控制器命名空间（默认当前项目：Takt.WebApi.Controllers）
   */
  controllerNamespace?: string;

  /**
   * 控制器类名称
   */
  controllerClassName?: string;

  /**
   * 仓储层（字典 sys_yes_no；0=否 1=是）
   */
  isRepository?: number;

  /**
   * 仓储接口命名空间（默认当前项目：Takt.Domain.Repositories）
   */
  repositoryInterfaceNamespace?: string;

  /**
   * 仓储接口类名称
   */
  iRepositoryClassName?: string;

  /**
   * 仓储命名空间（默认当前项目：Takt.Infrastructure.Repositories）
   */
  repositoryNamespace?: string;

  /**
   * 仓储类名称
   */
  repositoryClassName?: string;

  /**
   * 生成功能（字典 code_generator_function 多选逗号；亦支持 JSON/数组）。 核心设计：GenFunction 不仅决定生成哪些 Controller Actions 和 Service Methods，还决定生成哪些 DTO 类。功能与 DTO 的映射关系如下： Query → QueryDto；Create → CreateDto；Update → UpdateDto；Status → StatusDto；Sort → SortDto； Import → TemplateDto + ImportDto；Export → ExportDto；所有功能 → Dto（基础传输对象，包含所有字段）。
   */
  genFunction?: string;

  /**
   * 生成方式（字典 code_generator_method；0=zip 1=自定义路径 2=当前项目）
   */
  genMethod?: number;

  /**
   * 生成路径（字典 code_generator_path_type；GenMethod=1 时选择；0 默认 /；2 由 GenMethod 解析）
   */
  genPath?: string;

  /**
   * 生成菜单（字典 sys_yes_no；0=否 1=是）
   */
  isGenMenu?: number;

  /**
   * 上级菜单（关联 TaktMenu.Id，选项 TaktMenus/tree-options）
   */
  parentMenuId?: string;

  /**
   * 生成翻译（字典 sys_yes_no；0=否 1=是）
   */
  isGenTranslation?: number;

  /**
   * 排序字段（选项本表 columnList.databaseColumnName）
   */
  sortField?: string;

  /**
   * 排序方向（字典 sys_sort_type；ASC=升序 DESC=降序）
   */
  sortType?: string;

  /**
   * 前端UI框架（字典 code_generator_frontend_ui_type；1=element plus 2=ant design vue）
   */
  frontUi?: number;

  /**
   * 前端表单布局（字典 code_generator_frontend_form_layout；12=一行一列 24=一行两列）
   */
  frontFormLayout?: number;

  /**
   * 前端按钮样式（字典 code_generator_button_style；0=文本 1=标准）
   */
  frontBtnStyle?: number;

  /**
   * 是否生成（字典 sys_yes_no；0=否 1=是）
   */
  isGenCode?: number;

  /**
   * 代码生成次数（每次生成成功后自增）
   */
  genCodeCount?: number;

  /**
   * 使用tabs（字典 sys_yes_no；0=否 1=是）
   */
  isUseTabs?: number;

  /**
   * tabs标签中字段的数量
   */
  tabsFieldCount?: number;

  /**
   * 作者
   */
  genAuthor?: string;

  /**
   * 其他生成选项（JSON格式，存储其他生成配置）
   */
  otherGenOptions?: string;

  /**
   * 字段配置列表（子表，外键：TaktGenTableColumn.GenTableId 关联本表 Id）（子表，级联保存）
   */
  columns?: GenTableColumnCreate[];

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
 * GenTable 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 GenTableImport
 * @description 对应后端 TaktGenTableImportDto
 */
export interface GenTableImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 数据源（选项 TaktDatabaseInfos/list；持久化 displayName:tenantCode）
   */
  dataSource?: string;

  /**
   * 表名称（选项 TaktDatabaseInfos/tables；新建可手输；租户内与 DataSource 唯一）
   */
  tableName?: string;

  /**
   * 表描述（表注释）
   */
  tableComment?: string;

  /**
   * 关联父表（选项 TaktDatabaseInfos/tables 同库其他表；sub 模板必填）
   */
  subTableName?: string;

  /**
   * 关联外键（选项本表 columnList.databaseColumnName；sub 模板必填）
   */
  subTableFkName?: string;

  /**
   * 树编码（选项本表 columnList.databaseColumnName；tree 模板必填）
   */
  treeCode?: string;

  /**
   * 树父编码（选项本表 columnList.databaseColumnName；tree 模板必填）
   */
  treeParentCode?: string;

  /**
   * 树名称（选项本表 columnList.databaseColumnName；tree 模板必填）
   */
  treeName?: string;

  /**
   * 库表标识（字典 sys_yes_no；0=否 1=是）
   */
  inDatabase?: number;

  /**
   * 生成模板类型（字典 code_generator_template_type；crud/sub/tree）
   */
  genTemplateCategory?: string;

  /**
   * 模块名（功能模块名称）
   */
  genModuleName?: string;

  /**
   * 业务名（用于类名，如 Company，与模块拼接为 Takt.模块+类名）
   */
  genBusinessName?: string;

  /**
   * 功能名（用于接口与注释的中文名称，如：公司、部门）
   */
  genFunctionName?: string;

  /**
   * 权限前缀（与生成控制器/菜单/前端权限一致；对应库列 <c>perms_prefix</c>）。
   */
  permsPrefix?: string;

  /**
   * 菜单权限组（字典 code_generator_button_category 多选逗号；仅用于生成 menu_and_translation.sql 按钮 INSERT，不参与控制器/前端代码生成）
   */
  menuButtonGroup?: string;

  /**
   * 命名空间前缀（用于生成类名、方法名等的前缀）
   */
  namePrefix?: string;

  /**
   * 实体命名空间（默认当前项目：Takt.Domain.Entities）
   */
  entityNamespace?: string;

  /**
   * 实体类名称（首字母大写，驼峰命名）
   */
  entityClassName?: string;

  /**
   * 传输对象Dto命名空间（默认当前项目：Takt.Application.Dtos）
   */
  dtoNamespace?: string;

  /**
   * 传输对象 Dto 类名
   */
  dtoClassName?: string;

  /**
   * 服务命名空间（默认当前项目：Takt.Application.Services）
   */
  serviceNamespace?: string;

  /**
   * 服务接口类名称
   */
  iServiceClassName?: string;

  /**
   * 服务类名称
   */
  serviceClassName?: string;

  /**
   * 控制器命名空间（默认当前项目：Takt.WebApi.Controllers）
   */
  controllerNamespace?: string;

  /**
   * 控制器类名称
   */
  controllerClassName?: string;

  /**
   * 仓储层（字典 sys_yes_no；0=否 1=是）
   */
  isRepository?: number;

  /**
   * 仓储接口命名空间（默认当前项目：Takt.Domain.Repositories）
   */
  repositoryInterfaceNamespace?: string;

  /**
   * 仓储接口类名称
   */
  iRepositoryClassName?: string;

  /**
   * 仓储命名空间（默认当前项目：Takt.Infrastructure.Repositories）
   */
  repositoryNamespace?: string;

  /**
   * 仓储类名称
   */
  repositoryClassName?: string;

  /**
   * 生成功能（字典 code_generator_function 多选逗号；亦支持 JSON/数组）。 核心设计：GenFunction 不仅决定生成哪些 Controller Actions 和 Service Methods，还决定生成哪些 DTO 类。功能与 DTO 的映射关系如下： Query → QueryDto；Create → CreateDto；Update → UpdateDto；Status → StatusDto；Sort → SortDto； Import → TemplateDto + ImportDto；Export → ExportDto；所有功能 → Dto（基础传输对象，包含所有字段）。
   */
  genFunction?: string;

  /**
   * 生成方式（字典 code_generator_method；0=zip 1=自定义路径 2=当前项目）
   */
  genMethod?: number;

  /**
   * 生成路径（字典 code_generator_path_type；GenMethod=1 时选择；0 默认 /；2 由 GenMethod 解析）
   */
  genPath?: string;

  /**
   * 生成菜单（字典 sys_yes_no；0=否 1=是）
   */
  isGenMenu?: number;

  /**
   * 上级菜单（关联 TaktMenu.Id，选项 TaktMenus/tree-options）
   */
  parentMenuId?: string;

  /**
   * 生成翻译（字典 sys_yes_no；0=否 1=是）
   */
  isGenTranslation?: number;

  /**
   * 排序字段（选项本表 columnList.databaseColumnName）
   */
  sortField?: string;

  /**
   * 排序方向（字典 sys_sort_type；ASC=升序 DESC=降序）
   */
  sortType?: string;

  /**
   * 前端UI框架（字典 code_generator_frontend_ui_type；1=element plus 2=ant design vue）
   */
  frontUi?: number;

  /**
   * 前端表单布局（字典 code_generator_frontend_form_layout；12=一行一列 24=一行两列）
   */
  frontFormLayout?: number;

  /**
   * 前端按钮样式（字典 code_generator_button_style；0=文本 1=标准）
   */
  frontBtnStyle?: number;

  /**
   * 是否生成（字典 sys_yes_no；0=否 1=是）
   */
  isGenCode?: number;

  /**
   * 代码生成次数（每次生成成功后自增）
   */
  genCodeCount?: number;

  /**
   * 使用tabs（字典 sys_yes_no；0=否 1=是）
   */
  isUseTabs?: number;

  /**
   * tabs标签中字段的数量
   */
  tabsFieldCount?: number;

  /**
   * 作者
   */
  genAuthor?: string;

  /**
   * 其他生成选项（JSON格式，存储其他生成配置）
   */
  otherGenOptions?: string;

  /**
   * 字段配置列表（子表，外键：TaktGenTableColumn.GenTableId 关联本表 Id）（子表，级联保存）
   */
  columns?: GenTableColumnCreate[];

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
 * GenTable 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 GenTableExport
 * @description 对应后端 TaktGenTableExportDto
 */
export interface GenTableExport {
  /**
   * GenTableID
   */
  genTableId: string;

  /**
   * 数据源（选项 TaktDatabaseInfos/list；持久化 displayName:tenantCode）
   */
  dataSource: string;

  /**
   * 表名称（选项 TaktDatabaseInfos/tables；新建可手输；租户内与 DataSource 唯一）
   */
  tableName: string;

  /**
   * 表描述（表注释）
   */
  tableComment?: string;

  /**
   * 关联父表（选项 TaktDatabaseInfos/tables 同库其他表；sub 模板必填）
   */
  subTableName?: string;

  /**
   * 关联外键（选项本表 columnList.databaseColumnName；sub 模板必填）
   */
  subTableFkName?: string;

  /**
   * 树编码（选项本表 columnList.databaseColumnName；tree 模板必填）
   */
  treeCode?: string;

  /**
   * 树父编码（选项本表 columnList.databaseColumnName；tree 模板必填）
   */
  treeParentCode?: string;

  /**
   * 树名称（选项本表 columnList.databaseColumnName；tree 模板必填）
   */
  treeName?: string;

  /**
   * 库表标识（字典 sys_yes_no；0=否 1=是）
   */
  inDatabase: number;

  /**
   * 生成模板类型（字典 code_generator_template_type；crud/sub/tree）
   */
  genTemplateCategory: string;

  /**
   * 模块名（功能模块名称）
   */
  genModuleName?: string;

  /**
   * 业务名（用于类名，如 Company，与模块拼接为 Takt.模块+类名）
   */
  genBusinessName: string;

  /**
   * 功能名（用于接口与注释的中文名称，如：公司、部门）
   */
  genFunctionName?: string;

  /**
   * 权限前缀（与生成控制器/菜单/前端权限一致；对应库列 <c>perms_prefix</c>）。
   */
  permsPrefix: string;

  /**
   * 菜单权限组（字典 code_generator_button_category 多选逗号；仅用于生成 menu_and_translation.sql 按钮 INSERT，不参与控制器/前端代码生成）
   */
  menuButtonGroup?: string;

  /**
   * 命名空间前缀（用于生成类名、方法名等的前缀）
   */
  namePrefix?: string;

  /**
   * 实体命名空间（默认当前项目：Takt.Domain.Entities）
   */
  entityNamespace?: string;

  /**
   * 实体类名称（首字母大写，驼峰命名）
   */
  entityClassName: string;

  /**
   * 传输对象Dto命名空间（默认当前项目：Takt.Application.Dtos）
   */
  dtoNamespace?: string;

  /**
   * 传输对象 Dto 类名
   */
  dtoClassName?: string;

  /**
   * 服务命名空间（默认当前项目：Takt.Application.Services）
   */
  serviceNamespace?: string;

  /**
   * 服务接口类名称
   */
  iServiceClassName?: string;

  /**
   * 服务类名称
   */
  serviceClassName?: string;

  /**
   * 控制器命名空间（默认当前项目：Takt.WebApi.Controllers）
   */
  controllerNamespace?: string;

  /**
   * 控制器类名称
   */
  controllerClassName?: string;

  /**
   * 仓储层（字典 sys_yes_no；0=否 1=是）
   */
  isRepository: number;

  /**
   * 仓储接口命名空间（默认当前项目：Takt.Domain.Repositories）
   */
  repositoryInterfaceNamespace?: string;

  /**
   * 仓储接口类名称
   */
  iRepositoryClassName?: string;

  /**
   * 仓储命名空间（默认当前项目：Takt.Infrastructure.Repositories）
   */
  repositoryNamespace?: string;

  /**
   * 仓储类名称
   */
  repositoryClassName?: string;

  /**
   * 生成功能（字典 code_generator_function 多选逗号；亦支持 JSON/数组）。 核心设计：GenFunction 不仅决定生成哪些 Controller Actions 和 Service Methods，还决定生成哪些 DTO 类。功能与 DTO 的映射关系如下： Query → QueryDto；Create → CreateDto；Update → UpdateDto；Status → StatusDto；Sort → SortDto； Import → TemplateDto + ImportDto；Export → ExportDto；所有功能 → Dto（基础传输对象，包含所有字段）。
   */
  genFunction?: string;

  /**
   * 生成方式（字典 code_generator_method；0=zip 1=自定义路径 2=当前项目）
   */
  genMethod: number;

  /**
   * 生成路径（字典 code_generator_path_type；GenMethod=1 时选择；0 默认 /；2 由 GenMethod 解析）
   */
  genPath: string;

  /**
   * 生成菜单（字典 sys_yes_no；0=否 1=是）
   */
  isGenMenu: number;

  /**
   * 上级菜单（关联 TaktMenu.Id，选项 TaktMenus/tree-options）
   */
  parentMenuId: string;

  /**
   * 生成翻译（字典 sys_yes_no；0=否 1=是）
   */
  isGenTranslation: number;

  /**
   * 排序字段（选项本表 columnList.databaseColumnName）
   */
  sortField: string;

  /**
   * 排序方向（字典 sys_sort_type；ASC=升序 DESC=降序）
   */
  sortType: string;

  /**
   * 前端UI框架（字典 code_generator_frontend_ui_type；1=element plus 2=ant design vue）
   */
  frontUi: number;

  /**
   * 前端表单布局（字典 code_generator_frontend_form_layout；12=一行一列 24=一行两列）
   */
  frontFormLayout: number;

  /**
   * 前端按钮样式（字典 code_generator_button_style；0=文本 1=标准）
   */
  frontBtnStyle: number;

  /**
   * 是否生成（字典 sys_yes_no；0=否 1=是）
   */
  isGenCode: number;

  /**
   * 代码生成次数（每次生成成功后自增）
   */
  genCodeCount: number;

  /**
   * 使用tabs（字典 sys_yes_no；0=否 1=是）
   */
  isUseTabs: number;

  /**
   * tabs标签中字段的数量
   */
  tabsFieldCount: number;

  /**
   * 作者
   */
  genAuthor: string;

  /**
   * 其他生成选项（JSON格式，存储其他生成配置）
   */
  otherGenOptions?: string;

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

