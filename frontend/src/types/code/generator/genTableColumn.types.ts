// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/code/generator
// 文件名称：genTableColumn.types.ts
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
 * Takt代码生成字段配置实体 特例：继承组合 4：无关联工厂、无语言（TaktTenantCoreEntityBase）
 * 对应前端 TaktGenTableColumnDto
 * 继承 TaktTenantCoreDtoBase
 * 对应前端 GenTableColumn
 * @description 对应后端 TaktGenTableColumnDto
 */
export interface GenTableColumn extends TenantCoreDtoBase {
  /**
   * GenTableColumnID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  genTableColumnId: string;

  /**
   * 生成表ID（关联代码生成表配置，序列化为string以避免Javascript精度问题）
   */
  genTableId: string;

  /**
   * 生成表名称（填充字段）
   */
  genTableName?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 数据库列名称（唯一索引：租户内生成表+列名唯一，见 ix_gen_table_column_column_unique；snake_case，如 column_name）
   */
  databaseColumnName: string;

  /**
   * 列描述（字段注释）
   */
  columnComment?: string;

  /**
   * 数据类型（字典 sys_db_data_type；nvarchar/varchar/int/datetime/decimal 等）
   */
  databaseDataType: string;

  /**
   * C#类型（字典 code_generator_csharp_data_type；string/int/long/datetime/decimal/bool/guid 等）
   */
  csharpDataType: string;

  /**
   * C#列名（C#属性名，首字母大写，帕斯卡命名法）
   */
  csharpColumnName: string;

  /**
   * C#长度（字符串长度、数值类型的整数位数）
   */
  length: number;

  /**
   * C#小数位数（decimal等数值类型的小数位数）
   */
  decimalDigits: number;

  /**
   * 主键（字典 sys_yes_no；0=否 1=是）
   */
  isPk: number;

  /**
   * 自增（字典 sys_yes_no；0=否 1=是）
   */
  isIncrement: number;

  /**
   * 必填（字典 sys_yes_no；0=否 1=是）
   */
  isRequired: number;

  /**
   * 新增（字典 sys_yes_no；0=否 1=是）
   */
  isCreate: number;

  /**
   * 更新（字典 sys_yes_no；0=否 1=是）
   */
  isUpdate: number;

  /**
   * 查重（字典 sys_yes_no；0=否 1=是）
   */
  isUnique: number;

  /**
   * 列表（字典 sys_yes_no；0=否 1=是）
   */
  isList: number;

  /**
   * 导出（字典 sys_yes_no；0=否 1=是）
   */
  isExport: number;

  /**
   * 可排序（字典 sys_yes_no；0=否 1=是）。控制前端表格列是否显示 sortable 排序图标，与 TaktGenTable.SortField/SortType（默认排序规则）互补。
   */
  isSort: number;

  /**
   * 查询（字典 sys_yes_no；0=否 1=是）
   */
  isQuery: number;

  /**
   * 查询方式（字典 code_generator_query_type：eq/ne/gt/gte/lt/lte/like/between）。IsQuery=0 时必须为空串；IsQuery=1 时必填，字符串默认 like、其他类型默认 eq
   */
  queryType: string;

  /**
   * 显示类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）
   */
  htmlType: string;

  /**
   * 字典类型（选项 TaktDictTypes/options；DictValue=Id）
   */
  dictType?: string;

  /**
   * 所属表配置（主表，本表 GenTableId 关联 TaktGenTable.Id） （主表：TaktGenTable）
   */
  table?: GenTable;

}


/**
 * GenTableColumn 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 GenTableColumnQuery
 * @description 对应后端 TaktGenTableColumnQueryDto
 */
export interface GenTableColumnQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 生成表ID（关联代码生成表配置，序列化为string以避免Javascript精度问题）
   */
  genTableId?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 数据库列名称（唯一索引：租户内生成表+列名唯一，见 ix_gen_table_column_column_unique；snake_case，如 column_name）
   */
  databaseColumnName?: string;

  /**
   * 列描述（字段注释）
   */
  columnComment?: string;

  /**
   * 数据类型（字典 sys_db_data_type；nvarchar/varchar/int/datetime/decimal 等）
   */
  databaseDataType?: string;

  /**
   * C#类型（字典 code_generator_csharp_data_type；string/int/long/datetime/decimal/bool/guid 等）
   */
  csharpDataType?: string;

  /**
   * C#列名（C#属性名，首字母大写，帕斯卡命名法）
   */
  csharpColumnName?: string;

  /**
   * C#长度（字符串长度、数值类型的整数位数）
   */
  length?: number;

  /**
   * C#小数位数（decimal等数值类型的小数位数）
   */
  decimalDigits?: number;

  /**
   * 主键（字典 sys_yes_no；0=否 1=是）
   */
  isPk?: number;

  /**
   * 自增（字典 sys_yes_no；0=否 1=是）
   */
  isIncrement?: number;

  /**
   * 必填（字典 sys_yes_no；0=否 1=是）
   */
  isRequired?: number;

  /**
   * 新增（字典 sys_yes_no；0=否 1=是）
   */
  isCreate?: number;

  /**
   * 更新（字典 sys_yes_no；0=否 1=是）
   */
  isUpdate?: number;

  /**
   * 查重（字典 sys_yes_no；0=否 1=是）
   */
  isUnique?: number;

  /**
   * 列表（字典 sys_yes_no；0=否 1=是）
   */
  isList?: number;

  /**
   * 导出（字典 sys_yes_no；0=否 1=是）
   */
  isExport?: number;

  /**
   * 可排序（字典 sys_yes_no；0=否 1=是）。控制前端表格列是否显示 sortable 排序图标，与 TaktGenTable.SortField/SortType（默认排序规则）互补。
   */
  isSort?: number;

  /**
   * 查询（字典 sys_yes_no；0=否 1=是）
   */
  isQuery?: number;

  /**
   * 查询方式（字典 code_generator_query_type：eq/ne/gt/gte/lt/lte/like/between）。IsQuery=0 时必须为空串；IsQuery=1 时必填，字符串默认 like、其他类型默认 eq
   */
  queryType?: string;

  /**
   * 显示类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）
   */
  htmlType?: string;

  /**
   * 字典类型（选项 TaktDictTypes/options；DictValue=Id）
   */
  dictType?: string;

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
 * 创建GenTableColumn DTO
 * 对应前端 GenTableColumnCreate
 * @description 对应后端 TaktGenTableColumnCreateDto
 */
export interface GenTableColumnCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 生成表ID（关联代码生成表配置，序列化为string以避免Javascript精度问题）
   */
  genTableId: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 数据库列名称（唯一索引：租户内生成表+列名唯一，见 ix_gen_table_column_column_unique；snake_case，如 column_name）
   */
  databaseColumnName: string;

  /**
   * 列描述（字段注释）
   */
  columnComment?: string;

  /**
   * 数据类型（字典 sys_db_data_type；nvarchar/varchar/int/datetime/decimal 等）
   */
  databaseDataType: string;

  /**
   * C#类型（字典 code_generator_csharp_data_type；string/int/long/datetime/decimal/bool/guid 等）
   */
  csharpDataType: string;

  /**
   * C#列名（C#属性名，首字母大写，帕斯卡命名法）
   */
  csharpColumnName: string;

  /**
   * C#长度（字符串长度、数值类型的整数位数）
   */
  length: number;

  /**
   * C#小数位数（decimal等数值类型的小数位数）
   */
  decimalDigits: number;

  /**
   * 主键（字典 sys_yes_no；0=否 1=是）
   */
  isPk: number;

  /**
   * 自增（字典 sys_yes_no；0=否 1=是）
   */
  isIncrement: number;

  /**
   * 必填（字典 sys_yes_no；0=否 1=是）
   */
  isRequired: number;

  /**
   * 新增（字典 sys_yes_no；0=否 1=是）
   */
  isCreate: number;

  /**
   * 更新（字典 sys_yes_no；0=否 1=是）
   */
  isUpdate: number;

  /**
   * 查重（字典 sys_yes_no；0=否 1=是）
   */
  isUnique: number;

  /**
   * 列表（字典 sys_yes_no；0=否 1=是）
   */
  isList: number;

  /**
   * 导出（字典 sys_yes_no；0=否 1=是）
   */
  isExport: number;

  /**
   * 可排序（字典 sys_yes_no；0=否 1=是）。控制前端表格列是否显示 sortable 排序图标，与 TaktGenTable.SortField/SortType（默认排序规则）互补。
   */
  isSort: number;

  /**
   * 查询（字典 sys_yes_no；0=否 1=是）
   */
  isQuery: number;

  /**
   * 查询方式（字典 code_generator_query_type：eq/ne/gt/gte/lt/lte/like/between）。IsQuery=0 时必须为空串；IsQuery=1 时必填，字符串默认 like、其他类型默认 eq
   */
  queryType: string;

  /**
   * 显示类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）
   */
  htmlType: string;

  /**
   * 字典类型（选项 TaktDictTypes/options；DictValue=Id）
   */
  dictType?: string;

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
 * 更新GenTableColumn DTO
 * 继承 TaktGenTableColumnCreateDto，添加 GenTableColumnId 字段
 * 对应前端 GenTableColumnUpdate
 * @description 对应后端 TaktGenTableColumnUpdateDto
 */
export interface GenTableColumnUpdate extends GenTableColumnCreate {
  /**
   * GenTableColumnID（标识要更新的实体）
   */
  genTableColumnId: string;

}


/**
 * GenTableColumn 导入模板行 DTO
 * 对应前端 GenTableColumnTemplate
 * @description 对应后端 TaktGenTableColumnTemplateDto
 */
export interface GenTableColumnTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 生成表ID（关联代码生成表配置，序列化为string以避免Javascript精度问题）
   */
  genTableId?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 数据库列名称（唯一索引：租户内生成表+列名唯一，见 ix_gen_table_column_column_unique；snake_case，如 column_name）
   */
  databaseColumnName?: string;

  /**
   * 列描述（字段注释）
   */
  columnComment?: string;

  /**
   * 数据类型（字典 sys_db_data_type；nvarchar/varchar/int/datetime/decimal 等）
   */
  databaseDataType?: string;

  /**
   * C#类型（字典 code_generator_csharp_data_type；string/int/long/datetime/decimal/bool/guid 等）
   */
  csharpDataType?: string;

  /**
   * C#列名（C#属性名，首字母大写，帕斯卡命名法）
   */
  csharpColumnName?: string;

  /**
   * C#长度（字符串长度、数值类型的整数位数）
   */
  length?: number;

  /**
   * C#小数位数（decimal等数值类型的小数位数）
   */
  decimalDigits?: number;

  /**
   * 主键（字典 sys_yes_no；0=否 1=是）
   */
  isPk?: number;

  /**
   * 自增（字典 sys_yes_no；0=否 1=是）
   */
  isIncrement?: number;

  /**
   * 必填（字典 sys_yes_no；0=否 1=是）
   */
  isRequired?: number;

  /**
   * 新增（字典 sys_yes_no；0=否 1=是）
   */
  isCreate?: number;

  /**
   * 更新（字典 sys_yes_no；0=否 1=是）
   */
  isUpdate?: number;

  /**
   * 查重（字典 sys_yes_no；0=否 1=是）
   */
  isUnique?: number;

  /**
   * 列表（字典 sys_yes_no；0=否 1=是）
   */
  isList?: number;

  /**
   * 导出（字典 sys_yes_no；0=否 1=是）
   */
  isExport?: number;

  /**
   * 可排序（字典 sys_yes_no；0=否 1=是）。控制前端表格列是否显示 sortable 排序图标，与 TaktGenTable.SortField/SortType（默认排序规则）互补。
   */
  isSort?: number;

  /**
   * 查询（字典 sys_yes_no；0=否 1=是）
   */
  isQuery?: number;

  /**
   * 查询方式（字典 code_generator_query_type：eq/ne/gt/gte/lt/lte/like/between）。IsQuery=0 时必须为空串；IsQuery=1 时必填，字符串默认 like、其他类型默认 eq
   */
  queryType?: string;

  /**
   * 显示类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）
   */
  htmlType?: string;

  /**
   * 字典类型（选项 TaktDictTypes/options；DictValue=Id）
   */
  dictType?: string;

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
 * GenTableColumn 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 GenTableColumnImport
 * @description 对应后端 TaktGenTableColumnImportDto
 */
export interface GenTableColumnImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 生成表ID（关联代码生成表配置，序列化为string以避免Javascript精度问题）
   */
  genTableId?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 数据库列名称（唯一索引：租户内生成表+列名唯一，见 ix_gen_table_column_column_unique；snake_case，如 column_name）
   */
  databaseColumnName?: string;

  /**
   * 列描述（字段注释）
   */
  columnComment?: string;

  /**
   * 数据类型（字典 sys_db_data_type；nvarchar/varchar/int/datetime/decimal 等）
   */
  databaseDataType?: string;

  /**
   * C#类型（字典 code_generator_csharp_data_type；string/int/long/datetime/decimal/bool/guid 等）
   */
  csharpDataType?: string;

  /**
   * C#列名（C#属性名，首字母大写，帕斯卡命名法）
   */
  csharpColumnName?: string;

  /**
   * C#长度（字符串长度、数值类型的整数位数）
   */
  length?: number;

  /**
   * C#小数位数（decimal等数值类型的小数位数）
   */
  decimalDigits?: number;

  /**
   * 主键（字典 sys_yes_no；0=否 1=是）
   */
  isPk?: number;

  /**
   * 自增（字典 sys_yes_no；0=否 1=是）
   */
  isIncrement?: number;

  /**
   * 必填（字典 sys_yes_no；0=否 1=是）
   */
  isRequired?: number;

  /**
   * 新增（字典 sys_yes_no；0=否 1=是）
   */
  isCreate?: number;

  /**
   * 更新（字典 sys_yes_no；0=否 1=是）
   */
  isUpdate?: number;

  /**
   * 查重（字典 sys_yes_no；0=否 1=是）
   */
  isUnique?: number;

  /**
   * 列表（字典 sys_yes_no；0=否 1=是）
   */
  isList?: number;

  /**
   * 导出（字典 sys_yes_no；0=否 1=是）
   */
  isExport?: number;

  /**
   * 可排序（字典 sys_yes_no；0=否 1=是）。控制前端表格列是否显示 sortable 排序图标，与 TaktGenTable.SortField/SortType（默认排序规则）互补。
   */
  isSort?: number;

  /**
   * 查询（字典 sys_yes_no；0=否 1=是）
   */
  isQuery?: number;

  /**
   * 查询方式（字典 code_generator_query_type：eq/ne/gt/gte/lt/lte/like/between）。IsQuery=0 时必须为空串；IsQuery=1 时必填，字符串默认 like、其他类型默认 eq
   */
  queryType?: string;

  /**
   * 显示类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）
   */
  htmlType?: string;

  /**
   * 字典类型（选项 TaktDictTypes/options；DictValue=Id）
   */
  dictType?: string;

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
 * GenTableColumn 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 GenTableColumnExport
 * @description 对应后端 TaktGenTableColumnExportDto
 */
export interface GenTableColumnExport {
  /**
   * GenTableColumnID
   */
  genTableColumnId: string;

  /**
   * 生成表ID（关联代码生成表配置，序列化为string以避免Javascript精度问题）
   */
  genTableId: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 数据库列名称（唯一索引：租户内生成表+列名唯一，见 ix_gen_table_column_column_unique；snake_case，如 column_name）
   */
  databaseColumnName: string;

  /**
   * 列描述（字段注释）
   */
  columnComment?: string;

  /**
   * 数据类型（字典 sys_db_data_type；nvarchar/varchar/int/datetime/decimal 等）
   */
  databaseDataType: string;

  /**
   * C#类型（字典 code_generator_csharp_data_type；string/int/long/datetime/decimal/bool/guid 等）
   */
  csharpDataType: string;

  /**
   * C#列名（C#属性名，首字母大写，帕斯卡命名法）
   */
  csharpColumnName: string;

  /**
   * C#长度（字符串长度、数值类型的整数位数）
   */
  length: number;

  /**
   * C#小数位数（decimal等数值类型的小数位数）
   */
  decimalDigits: number;

  /**
   * 主键（字典 sys_yes_no；0=否 1=是）
   */
  isPk: number;

  /**
   * 自增（字典 sys_yes_no；0=否 1=是）
   */
  isIncrement: number;

  /**
   * 必填（字典 sys_yes_no；0=否 1=是）
   */
  isRequired: number;

  /**
   * 新增（字典 sys_yes_no；0=否 1=是）
   */
  isCreate: number;

  /**
   * 更新（字典 sys_yes_no；0=否 1=是）
   */
  isUpdate: number;

  /**
   * 查重（字典 sys_yes_no；0=否 1=是）
   */
  isUnique: number;

  /**
   * 列表（字典 sys_yes_no；0=否 1=是）
   */
  isList: number;

  /**
   * 导出（字典 sys_yes_no；0=否 1=是）
   */
  isExport: number;

  /**
   * 可排序（字典 sys_yes_no；0=否 1=是）。控制前端表格列是否显示 sortable 排序图标，与 TaktGenTable.SortField/SortType（默认排序规则）互补。
   */
  isSort: number;

  /**
   * 查询（字典 sys_yes_no；0=否 1=是）
   */
  isQuery: number;

  /**
   * 查询方式（字典 code_generator_query_type：eq/ne/gt/gte/lt/lte/like/between）。IsQuery=0 时必须为空串；IsQuery=1 时必填，字符串默认 like、其他类型默认 eq
   */
  queryType: string;

  /**
   * 显示类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）
   */
  htmlType: string;

  /**
   * 字典类型（选项 TaktDictTypes/options；DictValue=Id）
   */
  dictType?: string;

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

