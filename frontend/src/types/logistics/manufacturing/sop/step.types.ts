// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/sop
// 文件名称：step.types.ts
// 创建时间：2026-08-12
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/sop 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * SOP 工步实体
 * 对应前端 TaktSopStepDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SopStep
 * @description 对应后端 TaktSopStepDto
 */
export interface SopStep extends CompanyDtoBase {
  /**
   * SopStepID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  sopStepId: string;

  /**
   * 正文 ID（选项 TaktSopContents/options；DictValue=Id）
   */
  contentId: string;

  /**
   * 正文 名称（填充字段）
   */
  contentName?: string;

  /**
   * 工步序号
   */
  stepNo: number;

  /**
   * 工步标题
   */
  stepTitle: string;

  /**
   * 作业说明
   */
  stepDescription?: string;

  /**
   * 安全警示
   */
  safetyAlert?: string;

  /**
   * 弹窗（字典 sys_yes_no；0=否，1=是）
   */
  safetyPopupRequired: number;

  /**
   * 正文 （主表：TaktSopContent）
   */
  content?: SopContent;

  /**
   * 多媒体 （子表：TaktSopStepMedia）
   */
  mediaList?: SopStepMedia[];

  /**
   * 检验项目 （子表：TaktSopStepCheckItem）
   */
  checkItems?: SopStepCheckItem[];

}


/**
 * SopStep 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SopStepQuery
 * @description 对应后端 TaktSopStepQueryDto
 */
export interface SopStepQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
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
   * 正文 ID（选项 TaktSopContents/options；DictValue=Id）
   */
  contentId?: string;

  /**
   * 工步序号
   */
  stepNo?: number;

  /**
   * 工步标题
   */
  stepTitle?: string;

  /**
   * 作业说明
   */
  stepDescription?: string;

  /**
   * 安全警示
   */
  safetyAlert?: string;

  /**
   * 弹窗（字典 sys_yes_no；0=否，1=是）
   */
  safetyPopupRequired?: number;

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
 * 创建SopStep DTO
 * 对应前端 SopStepCreate
 * @description 对应后端 TaktSopStepCreateDto
 */
export interface SopStepCreate {
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
   * 正文 ID（选项 TaktSopContents/options；DictValue=Id）
   */
  contentId: string;

  /**
   * 工步序号
   */
  stepNo: number;

  /**
   * 工步标题
   */
  stepTitle: string;

  /**
   * 作业说明
   */
  stepDescription?: string;

  /**
   * 安全警示
   */
  safetyAlert?: string;

  /**
   * 弹窗（字典 sys_yes_no；0=否，1=是）
   */
  safetyPopupRequired: number;

  /**
   * 多媒体（子表，级联保存）
   */
  mediaList?: SopStepMediaCreate[];

  /**
   * 检验项目（子表，级联保存）
   */
  checkItems?: SopStepCheckItemCreate[];

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
 * 更新SopStep DTO
 * 继承 TaktSopStepCreateDto，添加 SopStepId 字段
 * 对应前端 SopStepUpdate
 * @description 对应后端 TaktSopStepUpdateDto
 */
export interface SopStepUpdate extends SopStepCreate {
  /**
   * SopStepID（标识要更新的实体）
   */
  sopStepId: string;

  /**
   * 多媒体（子表，级联保存）
   */
  mediaList?: any;

  /**
   * 检验项目（子表，级联保存）
   */
  checkItems?: any;

}


/**
 * SopStep 导入模板行 DTO
 * 对应前端 SopStepTemplate
 * @description 对应后端 TaktSopStepTemplateDto
 */
export interface SopStepTemplate {
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
   * 正文 ID（选项 TaktSopContents/options；DictValue=Id）
   */
  contentId?: string;

  /**
   * 工步序号
   */
  stepNo?: number;

  /**
   * 工步标题
   */
  stepTitle?: string;

  /**
   * 作业说明
   */
  stepDescription?: string;

  /**
   * 安全警示
   */
  safetyAlert?: string;

  /**
   * 弹窗（字典 sys_yes_no；0=否，1=是）
   */
  safetyPopupRequired?: number;

  /**
   * 多媒体（子表，级联保存）
   */
  mediaList?: SopStepMediaCreate[];

  /**
   * 检验项目（子表，级联保存）
   */
  checkItems?: SopStepCheckItemCreate[];

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
 * SopStep 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SopStepImport
 * @description 对应后端 TaktSopStepImportDto
 */
export interface SopStepImport {
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
   * 正文 ID（选项 TaktSopContents/options；DictValue=Id）
   */
  contentId?: string;

  /**
   * 工步序号
   */
  stepNo?: number;

  /**
   * 工步标题
   */
  stepTitle?: string;

  /**
   * 作业说明
   */
  stepDescription?: string;

  /**
   * 安全警示
   */
  safetyAlert?: string;

  /**
   * 弹窗（字典 sys_yes_no；0=否，1=是）
   */
  safetyPopupRequired?: number;

  /**
   * 多媒体（子表，级联保存）
   */
  mediaList?: SopStepMediaCreate[];

  /**
   * 检验项目（子表，级联保存）
   */
  checkItems?: SopStepCheckItemCreate[];

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
 * SopStep 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SopStepExport
 * @description 对应后端 TaktSopStepExportDto
 */
export interface SopStepExport {
  /**
   * SopStepID
   */
  sopStepId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 正文 ID（选项 TaktSopContents/options；DictValue=Id）
   */
  contentId: string;

  /**
   * 工步序号
   */
  stepNo: number;

  /**
   * 工步标题
   */
  stepTitle: string;

  /**
   * 作业说明
   */
  stepDescription?: string;

  /**
   * 安全警示
   */
  safetyAlert?: string;

  /**
   * 弹窗（字典 sys_yes_no；0=否，1=是）
   */
  safetyPopupRequired: number;

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

