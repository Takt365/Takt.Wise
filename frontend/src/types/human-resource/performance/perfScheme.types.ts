// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/performance
// 文件名称：perfScheme.types.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/performance 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 绩效方案指标（方案维度 + 指标维度合一，每行表示某方案下的一条指标）
 * 对应前端 TaktPerfSchemeDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PerfScheme
 * @description 对应后端 TaktPerfSchemeDto
 */
export interface PerfScheme extends CompanyDtoBase {

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
 * PerfScheme 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PerfSchemeExport
 * @description 对应后端 TaktPerfSchemeExportDto
 */
export interface PerfSchemeExport {
  /**
   * PerfSchemeID
   */
  perfSchemeId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 方案编码
   */
  schemeCode: string;

  /**
   * 方案名称
   */
  schemeName: string;

  /**
   * 适用部门
   */
  applicableDepartment: string;

  /**
   * 考核周期类型（月度/季度/半年度/年度）
   */
  cycleType: string;

  /**
   * 评分标准（百分制/五分制/等级制）
   */
  scoringStandard: string;

  /**
   * 自评权重（%）
   */
  selfEvaluationWeight: number;

  /**
   * 主管评分权重（%）
   */
  supervisorWeight: number;

  /**
   * 指标编码
   */
  metricCode: string;

  /**
   * 指标名称
   */
  metricName: string;

  /**
   * 指标类别（业绩/能力/态度/管理/创新/质量/效率/安全）
   */
  category: string;

  /**
   * 指标类型（定量/定性）
   */
  metricType: string;

  /**
   * 评分标准说明
   */
  scoringCriteria: string;

  /**
   * 标准权重（%）
   */
  standardWeight: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 状态（0=启用 1=停用）
   */
  schemeMetricStatus: number;

  /**
   * 关联工厂
   */
  plantCode?: string;

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

