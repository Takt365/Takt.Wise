// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/serial
// 文件名称：outboundItem.types.ts
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
 * 序列号出库明细实体
 * 对应前端 TaktSerialOutboundItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SerialOutboundItem
 * @description 对应后端 TaktSerialOutboundItemDto
 */
export interface SerialOutboundItem extends CompanyDtoBase {
  /**
   * SerialOutboundItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  serialOutboundItemId: string;

  /**
   * 出库主表 ID（关联 TaktSerialOutbound.Id，选项 TaktSerialOutbounds/options）
   */
  outboundId: string;

  /**
   * 出库主表 名称（填充字段）
   */
  outboundName?: string;

  /**
   * 出库单号（冗余字段，便于查询）
   */
  outboundCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 出库序列号（租户+公司内唯一）
   */
  outboundSerialCode: string;

  /**
   * 关联入库主表 ID（关联 TaktSerialInbound.Id，选项 TaktSerialInbounds/options）
   */
  referenceInboundId: string;

  /**
   * 关联入库主表 名称（填充字段）
   */
  referenceInboundName?: string;

  /**
   * 关联入库单号（选项 TaktSerialInbounds/options，DictValue=InboundCode）
   */
  referenceInboundCode: string;

  /**
   * 关联入库行号（对应 TaktSerialInboundItem.LineNumber）
   */
  referenceInboundLineNumber: number;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 出库主表 （主表：TaktSerialOutbound）
   */
  outbound?: SerialOutbound;

}


/**
 * SerialOutboundItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SerialOutboundItemQuery
 * @description 对应后端 TaktSerialOutboundItemQueryDto
 */
export interface SerialOutboundItemQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 出库主表 ID（关联 TaktSerialOutbound.Id，选项 TaktSerialOutbounds/options）
   */
  outboundId?: string;

  /**
   * 出库单号（冗余字段，便于查询）
   */
  outboundCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 出库序列号（租户+公司内唯一）
   */
  outboundSerialCode?: string;

  /**
   * 关联入库主表 ID（关联 TaktSerialInbound.Id，选项 TaktSerialInbounds/options）
   */
  referenceInboundId?: string;

  /**
   * 关联入库单号（选项 TaktSerialInbounds/options，DictValue=InboundCode）
   */
  referenceInboundCode?: string;

  /**
   * 关联入库行号（对应 TaktSerialInboundItem.LineNumber）
   */
  referenceInboundLineNumber?: number;

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
 * 创建SerialOutboundItem DTO
 * 对应前端 SerialOutboundItemCreate
 * @description 对应后端 TaktSerialOutboundItemCreateDto
 */
export interface SerialOutboundItemCreate {
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
   * 出库主表 ID（关联 TaktSerialOutbound.Id，选项 TaktSerialOutbounds/options）
   */
  outboundId: string;

  /**
   * 出库单号（冗余字段，便于查询）
   */
  outboundCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 出库序列号（租户+公司内唯一）
   */
  outboundSerialCode: string;

  /**
   * 关联入库主表 ID（关联 TaktSerialInbound.Id，选项 TaktSerialInbounds/options）
   */
  referenceInboundId: string;

  /**
   * 关联入库单号（选项 TaktSerialInbounds/options，DictValue=InboundCode）
   */
  referenceInboundCode: string;

  /**
   * 关联入库行号（对应 TaktSerialInboundItem.LineNumber）
   */
  referenceInboundLineNumber: number;

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
 * 更新SerialOutboundItem DTO
 * 继承 TaktSerialOutboundItemCreateDto，添加 SerialOutboundItemId 字段
 * 对应前端 SerialOutboundItemUpdate
 * @description 对应后端 TaktSerialOutboundItemUpdateDto
 */
export interface SerialOutboundItemUpdate extends SerialOutboundItemCreate {
  /**
   * SerialOutboundItemID（标识要更新的实体）
   */
  serialOutboundItemId: string;

}


/**
 * SerialOutboundItem 作废/撤销作废 DTO
 * 对应前端 SerialOutboundItemObsolete
 * @description 对应后端 TaktSerialOutboundItemObsoleteDto
 */
export interface SerialOutboundItemObsolete {
  /**
   * SerialOutboundItemID
   */
  serialOutboundItemId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * SerialOutboundItem 导入模板行 DTO
 * 对应前端 SerialOutboundItemTemplate
 * @description 对应后端 TaktSerialOutboundItemTemplateDto
 */
export interface SerialOutboundItemTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 出库主表 ID（关联 TaktSerialOutbound.Id，选项 TaktSerialOutbounds/options）
   */
  outboundId?: string;

  /**
   * 出库单号（冗余字段，便于查询）
   */
  outboundCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 出库序列号（租户+公司内唯一）
   */
  outboundSerialCode?: string;

  /**
   * 关联入库主表 ID（关联 TaktSerialInbound.Id，选项 TaktSerialInbounds/options）
   */
  referenceInboundId?: string;

  /**
   * 关联入库单号（选项 TaktSerialInbounds/options，DictValue=InboundCode）
   */
  referenceInboundCode?: string;

  /**
   * 关联入库行号（对应 TaktSerialInboundItem.LineNumber）
   */
  referenceInboundLineNumber?: number;

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
 * SerialOutboundItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SerialOutboundItemImport
 * @description 对应后端 TaktSerialOutboundItemImportDto
 */
export interface SerialOutboundItemImport {
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
   * 出库主表 ID（关联 TaktSerialOutbound.Id，选项 TaktSerialOutbounds/options）
   */
  outboundId?: string;

  /**
   * 出库单号（冗余字段，便于查询）
   */
  outboundCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 出库序列号（租户+公司内唯一）
   */
  outboundSerialCode?: string;

  /**
   * 关联入库主表 ID（关联 TaktSerialInbound.Id，选项 TaktSerialInbounds/options）
   */
  referenceInboundId?: string;

  /**
   * 关联入库单号（选项 TaktSerialInbounds/options，DictValue=InboundCode）
   */
  referenceInboundCode?: string;

  /**
   * 关联入库行号（对应 TaktSerialInboundItem.LineNumber）
   */
  referenceInboundLineNumber?: number;

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
 * SerialOutboundItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SerialOutboundItemExport
 * @description 对应后端 TaktSerialOutboundItemExportDto
 */
export interface SerialOutboundItemExport {
  /**
   * SerialOutboundItemID
   */
  serialOutboundItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 出库主表 ID（关联 TaktSerialOutbound.Id，选项 TaktSerialOutbounds/options）
   */
  outboundId: string;

  /**
   * 出库单号（冗余字段，便于查询）
   */
  outboundCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 出库序列号（租户+公司内唯一）
   */
  outboundSerialCode: string;

  /**
   * 关联入库主表 ID（关联 TaktSerialInbound.Id，选项 TaktSerialInbounds/options）
   */
  referenceInboundId: string;

  /**
   * 关联入库单号（选项 TaktSerialInbounds/options，DictValue=InboundCode）
   */
  referenceInboundCode: string;

  /**
   * 关联入库行号（对应 TaktSerialInboundItem.LineNumber）
   */
  referenceInboundLineNumber: number;

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

