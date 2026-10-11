// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/talent
// 文件名称：jobPosting.types.ts
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/talent 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 职位发布（业务发布单，非审批单；状态见 posting_status）
 * 对应前端 TaktTalentJobPostingDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 TalentJobPosting
 * @description 对应后端 TaktTalentJobPostingDto
 */
export interface TalentJobPosting extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 用人需求ID
   */
  staffingRequirementId?: string;

  /**
   * 发布编码（租户+公司内唯一）
   */
  postingCode?: string;

  /**
   * 职位标题
   */
  talentJobPostingTitle?: string;

  /**
   * 发布状态（0=草稿，1=招聘中，2=已暂停，3=已关闭）
   */
  postingStatus?: number;

  /**
   * 职位发布日期
   */
  publishDate?: string;

  /**
   * 招聘开放日期
   */
  openDate?: string;

  /**
   * 招聘关闭日期
   */
  closeDate?: string;

  /**
   * 发布渠道（0=官网，1=招聘网站，2=内推，3=校园，9=其他）
   */
  publishChannel?: number;

  /**
   * 发布说明
   */
  reason?: string;

  /**
   * 录用信息（子表，级联保存）
   */
  talentOffers?: TalentOfferCreate[];

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
 * TalentJobPosting 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 TalentJobPostingExport
 * @description 对应后端 TaktTalentJobPostingExportDto
 */
export interface TalentJobPostingExport {
  /**
   * TalentJobPostingID
   */
  talentJobPostingId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 用人需求ID
   */
  staffingRequirementId: string;

  /**
   * 发布编码（租户+公司内唯一）
   */
  postingCode: string;

  /**
   * 职位标题
   */
  talentJobPostingTitle: string;

  /**
   * 发布状态（0=草稿，1=招聘中，2=已暂停，3=已关闭）
   */
  postingStatus: number;

  /**
   * 职位发布日期
   */
  publishDate: string;

  /**
   * 招聘开放日期
   */
  openDate: string;

  /**
   * 招聘关闭日期
   */
  closeDate?: string;

  /**
   * 发布渠道（0=官网，1=招聘网站，2=内推，3=校园，9=其他）
   */
  publishChannel: number;

  /**
   * 发布说明
   */
  reason?: string;

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

