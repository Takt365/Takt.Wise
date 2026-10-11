// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/foundation
// 文件名称：dictData.types.ts
// 创建时间：2026-07-20
// 创建人：Takt365(Auto Generated)
// 功能描述：foundation 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  TaktPagedQuery,
  TenantCultureDtoBase
} from '@/types/common.types';

/**
 * 字典数据实体 字典类型的具体数据项
 * 对应前端 TaktDictDataDto
 * 继承 TaktTenantCultureDtoBase（组合 2：无关联工厂、有语言）
 * 对应前端 DictData
 * @description 对应后端 TaktDictDataDto
 */
export interface DictData extends TenantCultureDtoBase {
  /**
   * DictDataID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  dictDataId: string;
  /**
   * 字典类型（选项 TaktDictTypes/options；DictValue=Id）
   */
  dictTypeId: string;
  /**
   * 字典类型编码（冗余，与 TaktDictType.DictTypeCode 对齐；Length=140）
   */
  dictTypeCode: string;
  /**
   * 字典项标签（Length=40）
   */
  dictLabel: string;
  /**
   * 字典项值（Length=40）
   */
  dictValue: string;
  /**
   * 国际化键（Length=140）
   */
  i18nKey: string;
  /**
   * 扩展标签（Length=140）
   */
  extLabel?: string;
  /**
   * 扩展值（Length=140）
   */
  extValue?: string;
  /**
   * 列表样式类
   */
  listClass?: number;
  /**
   * CSS 类名
   */
  cssClass?: number;
  /**
   * 是否默认项（1=是，0=否）
   */
  isDefault?: number;
  /**
   * 排序号
   */
  sortOrder?: number;
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
 * 创建 DictData DTO（组合 2：TenantCode + CultureCode，无关联工厂；CultureCode 默认 mul）
 */
export interface DictDataCreate {
  tenantCode: string;
  cultureCode: string;
  dictTypeId: string;
  dictTypeCode: string;
  dictLabel: string;
  dictValue: string;
  i18nKey: string;
  extLabel?: string;
  extValue?: string;
  listClass?: number;
  cssClass?: number;
  isDefault?: number;
  sortOrder?: number;
  extField?: string;
  remark?: string;
}

/**
 * 更新 DictData DTO
 */
export interface DictDataUpdate extends DictDataCreate {
  dictDataId: string;
}

/**
 * DictData 分页查询 DTO
 */
export interface DictDataQuery extends TaktPagedQuery {
  tenantCode?: string;
  cultureCode?: string;
  dictTypeId?: string;
  dictTypeCode?: string;
  dictLabel?: string;
  dictValue?: string;
  i18nKey?: string;
}

/**
 * DictData 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 DictDataExport
 * @description 对应后端 TaktDictDataExportDto
 */
export interface DictDataExport {
  /**
   * DictDataID
   */
  dictDataId: string;

  /**
   * 字典类型ID（关联 TaktDictType.Id；唯一索引：租户内 DictTypeId+CultureCode+DictLabel+I18nKey 唯一）
   */
  dictTypeId: string;

  /**
   * 字典类型编码（冗余，与 TaktDictType.DictTypeCode 对齐；Length=140）
   */
  dictTypeCode: string;

  /**
   * 字典项标签（唯一索引：租户内 DictTypeId+CultureCode+DictLabel+I18nKey 唯一；sys_culture_code 等区域文化项用本族语，同语言多地区才加括号，如 English (US)、中文 (简体)）
   */
  dictLabel: string;

  /**
   * 字典项值（实际存储值，如：0, 1, 2）
   */
  dictValue: string;

  /**
   * 国际化键（与 DictTypeCode 段对应，如 dict.accounting.controlling.cost.center.type.f）
   */
  i18nKey: string;

  /**
   * 扩展标签（用于存储额外的显示文本，如：副标题、简短描述等）
   */
  extLabel?: string;

  /**
   * 扩展值（用于存储额外的业务数据，如：编码、标识符等）
   */
  extValue?: string;

  /**
   * 列表样式类（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于下拉列表选项中显示的颜色标识
   */
  listClass: number;

  /**
   * CSS 类名（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于数据表格中字典值显示的颜色标签
   */
  cssClass: number;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 是否默认项（1=是，0=否）
   */
  isDefault: number;

  /**
   * 排序号
   */
  sortOrder: number;

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

/**
 * 当前 Accept-Language 下全部字典数据响应 DTO（含 CultureCode mul 的多语言通用项）
 * 对应前端 DataDictAll；Items 为扁平列表，含 DictTypeCode 供前端分组
 * 对应前端 DataDictAll
 * @description 对应后端 TaktDataDictAllDto
 */
export interface DataDictAll {
  /**
   * 字典项列表（已按 DictTypeCode、SortOrder 排序）
   */
  items: TaktSelectOption[];

}

