// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/sop
// 文件名称：sopRevision.types.ts
// 创建时间：2026-06-15
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
 * SOP 版本实体
 * 对应前端 TaktSopRevisionDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SopRevision
 * @description 对应后端 TaktSopRevisionDto
 */
export interface SopRevision extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * SOP 文档头 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  sopId?: string;

  /**
   * 版本号（主版本.次版本，如 1.0、A.01）
   */
  revision?: string;

  /**
   * 受控 PDF URL
   */
  fileUrl?: string;

  /**
   * 变更说明
   */
  changeDesc?: string;

  /**
   * 关联 ECN 主表 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  ecnId?: string;

  /**
   * 是否锁定（ECN 后旧版锁定；字典 sys_yes_no，0=否，1=是）
   */
  isLocked?: number;

  /**
   * 是否强制班组长确认（新版本弹窗；字典 sys_yes_no，0=否，1=是）
   */
  forceLeaderAck?: number;

  /**
   * 版本状态（字典 sys_lifecycle_status）
   */
  revisionStatus?: number;

  /**
   * 生效规则（1=立即生效，2=按工单生效；字典 logistics_sop_effective_rule）
   */
  effectiveRule?: number;

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
 * SopRevision 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SopRevisionExport
 * @description 对应后端 TaktSopRevisionExportDto
 */
export interface SopRevisionExport {
  /**
   * SopRevisionID
   */
  sopRevisionId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * SOP 文档头 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  sopId: string;

  /**
   * 版本号（主版本.次版本，如 1.0、A.01）
   */
  revision: string;

  /**
   * 受控 PDF URL
   */
  fileUrl?: string;

  /**
   * 变更说明
   */
  changeDesc?: string;

  /**
   * 关联 ECN 主表 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  ecnId?: string;

  /**
   * 是否锁定（ECN 后旧版锁定；字典 sys_yes_no，0=否，1=是）
   */
  isLocked: number;

  /**
   * 是否强制班组长确认（新版本弹窗；字典 sys_yes_no，0=否，1=是）
   */
  forceLeaderAck: number;

  /**
   * 版本状态（字典 sys_lifecycle_status）
   */
  revisionStatus: number;

  /**
   * 生效规则（1=立即生效，2=按工单生效；字典 logistics_sop_effective_rule）
   */
  effectiveRule: number;

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

