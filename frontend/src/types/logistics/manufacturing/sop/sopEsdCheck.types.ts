// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/sop
// 文件名称：sopEsdCheck.types.ts
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
 * SOP ESD 检查实体
 * 对应前端 TaktSopEsdCheckDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SopEsdCheck
 * @description 对应后端 TaktSopEsdCheckDto
 */
export interface SopEsdCheck extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 工位 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  workstationId?: string;

  /**
   * 执行追溯 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  execId?: string;

  /**
   * 员工 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  employeeId?: string;

  /**
   * 监测设备编码
   */
  deviceCode?: string;

  /**
   * 达标（字典 sys_yes_no，0=否，1=是）
   */
  isCompliant?: number;

  /**
   * 锁屏（字典 sys_yes_no，0=否，1=是）
   */
  lockScreenTriggered?: number;

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
 * SopEsdCheck 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SopEsdCheckExport
 * @description 对应后端 TaktSopEsdCheckExportDto
 */
export interface SopEsdCheckExport {
  /**
   * SopEsdCheckID
   */
  sopEsdCheckId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工位 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  workstationId: string;

  /**
   * 执行追溯 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  execId?: string;

  /**
   * 员工 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  employeeId: string;

  /**
   * 监测设备编码
   */
  deviceCode?: string;

  /**
   * 阻值（兆欧）
   */
  resistanceValue?: number;

  /**
   * 达标（字典 sys_yes_no，0=否，1=是）
   */
  isCompliant: number;

  /**
   * 锁屏（字典 sys_yes_no，0=否，1=是）
   */
  lockScreenTriggered: number;

  /**
   * 检查时间
   */
  checkedAt: string;

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

