// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/sop
// 文件名称：sopExecScan.types.ts
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
 * SOP 物料扫码记录实体
 * 对应前端 TaktSopExecScanDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SopExecScan
 * @description 对应后端 TaktSopExecScanDto
 */
export interface SopExecScan extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 执行追溯 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  execId?: string;

  /**
   * 工步执行明细 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  execStepId?: string;

  /**
   * 工步 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  stepId?: string;

  /**
   * 扫描条码
   */
  scannedBarcode?: string;

  /**
   * 期望物料编码
   */
  expectedMaterialCode?: string;

  /**
   * 扫码结果（1=PASS，2=NG；字典 logistics_manufacturing_sop_scan_result）
   */
  scanResult?: number;

  /**
   * 比对说明
   */
  matchMessage?: string;

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
 * SopExecScan 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SopExecScanExport
 * @description 对应后端 TaktSopExecScanExportDto
 */
export interface SopExecScanExport {
  /**
   * SopExecScanID
   */
  sopExecScanId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 执行追溯 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  execId: string;

  /**
   * 工步执行明细 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  execStepId?: string;

  /**
   * 工步 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  stepId: string;

  /**
   * 扫描条码
   */
  scannedBarcode: string;

  /**
   * 期望物料编码
   */
  expectedMaterialCode?: string;

  /**
   * 扫码结果（1=PASS，2=NG；字典 logistics_manufacturing_sop_scan_result）
   */
  scanResult: number;

  /**
   * 比对说明
   */
  matchMessage?: string;

  /**
   * 扫描时间
   */
  scannedAt: string;

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

