// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/talent
// 文件名称：offer.types.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/talent 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 录用信息（审批单，状态见 TaktApprovalEntityBase.ApprovalStatus）
 * 对应前端 TaktTalentOfferDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 TalentOffer
 * @description 对应后端 TaktTalentOfferDto
 */
export interface TalentOffer extends ApprovalDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 职位发布ID（用人需求→职位发布→录用）
   */
  jobPostingId?: string;

  /**
   * 录用编码（租户+公司内业务编码）
   */
  offerCode?: string;

  /**
   * 录用日期（HireDate：确认录用/发 offer）
   */
  hireDate?: string;

  /**
   * 关联员工ID（录用通过并建档后回填，可空）
   */
  employeeId?: string;

  /**
   * 拟录用部门ID
   */
  deptId?: string;

  /**
   * 拟录用部门名称
   */
  deptName?: string;

  /**
   * 拟录用岗位ID
   */
  postId?: string;

  /**
   * 拟录用岗位名称
   */
  postName?: string;

  /**
   * 录用说明
   */
  reason?: string;

  /**
   * 入职待办（子表，级联保存）
   */
  employeeOnboardings?: EmployeeOnboardingCreate[];

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
 * TalentOffer 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 TalentOfferExport
 * @description 对应后端 TaktTalentOfferExportDto
 */
export interface TalentOfferExport {
  /**
   * TalentOfferID
   */
  talentOfferId: string;

  /**
   * 职位发布ID（用人需求→职位发布→录用）
   */
  jobPostingId: string;

  /**
   * 录用编码（租户+公司内业务编码）
   */
  offerCode: string;

  /**
   * 录用日期（HireDate：确认录用/发 offer）
   */
  hireDate: string;

  /**
   * 关联员工ID（录用通过并建档后回填，可空）
   */
  employeeId?: string;

  /**
   * 拟录用部门ID
   */
  deptId: string;

  /**
   * 拟录用部门名称
   */
  deptName: string;

  /**
   * 拟录用岗位ID
   */
  postId?: string;

  /**
   * 拟录用岗位名称
   */
  postName?: string;

  /**
   * 录用说明
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

