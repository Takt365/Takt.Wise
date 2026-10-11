// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/quality/operation
// 文件名称：qualityStatQuery.types.ts
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：质量检验统计查询（数据看板 inspection-stat）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 质量检验统计查询
 * @description 对应后端 TaktQualityStatQueryDto
 */
export interface QualityStatQuery {
  /** 检验日期（范围-开始） */
  inspectionDateStart?: string;
  /** 检验日期（范围-结束） */
  inspectionDateEnd?: string;
}
