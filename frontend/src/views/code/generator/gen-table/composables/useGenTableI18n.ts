// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/code/generator/gen-table/composables
// 文件名称：useGenTableI18n.ts
// 功能描述：Takt代码生成表配置实体 特例：继承组合 4：无关联工厂、无语言字段清单 + useGenTableI18n（字段名映射一次，文案由 entity.gentable.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { GenTableQuery } from '@/types/code/generator/genTable.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktGenTableI18nSeedData 一致的实体 slug */
export const GENTABLE_ENTITY_SLUG = 'gentable'

/** entity.gentable._self 静态属性（导入组件 entity-i18n-key 等） */
export const GENTABLE_SELF_I18N_KEY = buildEntitySelfI18nKey(GENTABLE_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const GENTABLE_LIST_FIELDS = [
  'dataSource',
  'tableName',
  'tableComment',
  'subTableName',
  'subTableFkName',
  'treeCode',
  'treeParentCode',
  'treeName',
  'inDatabase',
  'genTemplateCategory',
  'genModuleName',
  'genBusinessName',
  'genFunctionName',
  'permsPrefix',
  'menuButtonGroup',
  'namePrefix',
  'entityNamespace',
  'entityClassName',
  'dtoNamespace',
  'dtoClassName',
  'serviceNamespace',
  'iServiceClassName',
  'serviceClassName',
  'controllerNamespace',
  'controllerClassName',
  'isRepository',
  'repositoryInterfaceNamespace',
  'iRepositoryClassName',
  'repositoryNamespace',
  'repositoryClassName',
  'genFunction',
  'genMethod',
  'genPath',
  'isGenMenu',
  'parentMenuId',
  'parentMenuName',
  'isGenTranslation',
  'sortField',
  'sortType',
  'frontUi',
  'frontFormLayout',
  'frontBtnStyle',
  'isGenCode',
  'genCodeCount',
  'isUseTabs',
  'tabsFieldCount',
  'genAuthor',
  'otherGenOptions',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const GENTABLE_PLACEHOLDER = {
  tenantCode: 'optional',
  dataSource: 'required',
  tableName: 'required',
  tableComment: 'optional',
  subTableName: 'optional',
  subTableFkName: 'optional',
  treeCode: 'optional',
  treeParentCode: 'optional',
  treeName: 'optional',
  inDatabase: 'select',
  genTemplateCategory: 'select',
  genModuleName: 'optional',
  genBusinessName: 'required',
  genFunctionName: 'optional',
  permsPrefix: 'required',
  menuButtonGroup: 'optional',
  namePrefix: 'optional',
  entityNamespace: 'optional',
  entityClassName: 'required',
  dtoNamespace: 'optional',
  dtoClassName: 'optional',
  serviceNamespace: 'optional',
  iServiceClassName: 'optional',
  serviceClassName: 'optional',
  controllerNamespace: 'optional',
  controllerClassName: 'optional',
  isRepository: 'select',
  repositoryInterfaceNamespace: 'optional',
  iRepositoryClassName: 'optional',
  repositoryNamespace: 'optional',
  repositoryClassName: 'optional',
  genFunction: 'optional',
  genMethod: 'select',
  genPath: 'select',
  isGenMenu: 'select',
  parentMenuId: 'select',
  isGenTranslation: 'select',
  sortField: 'required',
  sortType: 'select',
  frontUi: 'select',
  frontFormLayout: 'select',
  frontBtnStyle: 'select',
  isGenCode: 'select',
  genCodeCount: 'select',
  isUseTabs: 'select',
  tabsFieldCount: 'select',
  genAuthor: 'required',
  otherGenOptions: 'optional',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type GenTableField = keyof typeof GENTABLE_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const GENTABLE_QUERY_STRING_FIELDS = [
  'dataSource',
  'tableName',
  'tableComment',
  'subTableName',
  'subTableFkName',
  'treeCode',
  'treeParentCode',
  'treeName',
  'genTemplateCategory',
  'genModuleName',
  'genBusinessName',
  'genFunctionName',
  'permsPrefix',
  'menuButtonGroup',
  'namePrefix',
  'entityNamespace',
  'entityClassName',
  'dtoNamespace',
  'dtoClassName',
  'serviceNamespace',
  'iServiceClassName',
  'serviceClassName',
  'controllerNamespace',
  'controllerClassName',
  'repositoryInterfaceNamespace',
  'iRepositoryClassName',
  'repositoryNamespace',
  'repositoryClassName',
  'genFunction',
  'genPath',
  'parentMenuId',
  'sortField',
  'sortType',
  'genAuthor',
  'otherGenOptions',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof GenTableQuery)[]

export type GenTableQueryField =
  | (typeof GENTABLE_QUERY_STRING_FIELDS)[number]
  | 'inDatabase' | 'isRepository' | 'genMethod' | 'isGenMenu' | 'isGenTranslation' | 'frontUi' | 'frontFormLayout' | 'frontBtnStyle' | 'isGenCode' | 'genCodeCount' | 'isUseTabs' | 'tabsFieldCount'

/** 高级查询抽屉全部字段（含数值） */
export const GENTABLE_QUERY_FIELDS: readonly GenTableQueryField[] = [
  ...GENTABLE_QUERY_STRING_FIELDS,
  'inDatabase',
  'isRepository',
  'genMethod',
  'isGenMenu',
  'isGenTranslation',
  'frontUi',
  'frontFormLayout',
  'frontBtnStyle',
  'isGenCode',
  'genCodeCount',
  'isUseTabs',
  'tabsFieldCount',
]

/**
 * Takt代码生成表配置实体 特例：继承组合 4：无关联工厂、无语言字段 i18n：index / gen-table-form 统一入口
 */
export function useGenTableI18n() {
  const ef = useEntityFieldI18n(GENTABLE_ENTITY_SLUG)

  function ph(field: GenTableField): string {
    return ef.placeholder(field, GENTABLE_PLACEHOLDER[field])
  }

  function queryPh(field: GenTableQueryField, kind: EntityFieldPlaceholderKind): string {
    return ef.queryPlaceholder(field, kind)
  }

  return {
    t: ef.t,
    label: ef.label,
    queryLabel: ef.queryLabel,
    queryPh,
    self: ef.self,
    ph,
  }
}
