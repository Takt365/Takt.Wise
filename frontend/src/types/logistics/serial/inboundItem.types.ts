// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/serial
// 文件名称：inboundItem.types.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/serial 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 序列号入库明细实体
 * 对应前端 TaktSerialInboundItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SerialInboundItem
 * @description 对应后端 TaktSerialInboundItemDto
 */
export interface SerialInboundItem extends CompanyDtoBase {
  /**
   * SerialInboundItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  serialInboundItemId: string;

  /**
   * 入库主表 ID（关联 TaktSerialInbound.Id，选项 TaktSerialInbounds/options）
   */
  inboundId: string;

  /**
   * 入库主表 名称（填充字段）
   */
  inboundName?: string;

  /**
   * 入库单号（冗余字段，便于查询）
   */
  inboundCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 入库序列号（租户+公司内唯一）
   */
  inboundSerialCode: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 入库主表 （主表：TaktSerialInbound）
   */
  inbound?: SerialInbound;

}


/**
 * SerialInboundItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SerialInboundItemQuery
 * @description 对应后端 TaktSerialInboundItemQueryDto
 */
export interface SerialInboundItemQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 入库主表 ID（关联 TaktSerialInbound.Id，选项 TaktSerialInbounds/options）
   */
  inboundId?: string;

  /**
   * 入库单号（冗余字段，便于查询）
   */
  inboundCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 入库序列号（租户+公司内唯一）
   */
  inboundSerialCode?: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * 创建SerialInboundItem DTO
 * 对应前端 SerialInboundItemCreate
 * @description 对应后端 TaktSerialInboundItemCreateDto
 */
export interface SerialInboundItemCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 入库主表 ID（关联 TaktSerialInbound.Id，选项 TaktSerialInbounds/options）
   */
  inboundId: string;

  /**
   * 入库单号（冗余字段，便于查询）
   */
  inboundCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 入库序列号（租户+公司内唯一）
   */
  inboundSerialCode: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

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
 * 更新SerialInboundItem DTO
 * 继承 TaktSerialInboundItemCreateDto，添加 SerialInboundItemId 字段
 * 对应前端 SerialInboundItemUpdate
 * @description 对应后端 TaktSerialInboundItemUpdateDto
 */
export interface SerialInboundItemUpdate extends SerialInboundItemCreate {
  /**
   * SerialInboundItemID（标识要更新的实体）
   */
  serialInboundItemId: string;

}


/**
 * SerialInboundItem 作废/撤销作废 DTO
 * 对应前端 SerialInboundItemObsolete
 * @description 对应后端 TaktSerialInboundItemObsoleteDto
 */
export interface SerialInboundItemObsolete {
  /**
   * SerialInboundItemID
   */
  serialInboundItemId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * SerialInboundItem 导入模板行 DTO
 * 对应前端 SerialInboundItemTemplate
 * @description 对应后端 TaktSerialInboundItemTemplateDto
 */
export interface SerialInboundItemTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 入库主表 ID（关联 TaktSerialInbound.Id，选项 TaktSerialInbounds/options）
   */
  inboundId?: string;

  /**
   * 入库单号（冗余字段，便于查询）
   */
  inboundCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 入库序列号（租户+公司内唯一）
   */
  inboundSerialCode?: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * SerialInboundItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SerialInboundItemImport
 * @description 对应后端 TaktSerialInboundItemImportDto
 */
export interface SerialInboundItemImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 入库主表 ID（关联 TaktSerialInbound.Id，选项 TaktSerialInbounds/options）
   */
  inboundId?: string;

  /**
   * 入库单号（冗余字段，便于查询）
   */
  inboundCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 入库序列号（租户+公司内唯一）
   */
  inboundSerialCode?: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * SerialInboundItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SerialInboundItemExport
 * @description 对应后端 TaktSerialInboundItemExportDto
 */
export interface SerialInboundItemExport {
  /**
   * SerialInboundItemID
   */
  serialInboundItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 入库主表 ID（关联 TaktSerialInbound.Id，选项 TaktSerialInbounds/options）
   */
  inboundId: string;

  /**
   * 入库单号（冗余字段，便于查询）
   */
  inboundCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 入库序列号（租户+公司内唯一）
   */
  inboundSerialCode: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

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

