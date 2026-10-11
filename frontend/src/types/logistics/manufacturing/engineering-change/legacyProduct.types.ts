// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：legacyProduct.types.ts
// 创建时间：2026-06-22
// 功能描述：设变旧品管制类型（对齐 TaktEcLegacyProductDto / QueryDto / UpdateDto）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { CompanyDtoBase, TaktPagedQuery } from '@/types/common.types';

/**
 * 旧品管制视图行
 * @description 对应后端 TaktEcLegacyProductDto
 */
export interface EcLegacyProduct extends CompanyDtoBase {
  /**
   * 视图主键（与设变明细 Id 相同，序列化为 string）
   */
  ecLegacyProductId: string;

  /**
   * 设变明细 ID
   */
  ecDetailId: string;

  /**
   * 设变单号
   */
  ecCode: string;

  /**
   * 行号
   */
  lineNumber: number;

  /**
   * 机种编码
   */
  ecModelCode: string;

  /**
   * 旧物料编码
   */
  ecOldMaterialCode?: string;

  /**
   * 旧物料描述
   */
  ecOldMaterialDescription?: string;

  /**
   * 旧用量
   */
  ecOldUsageQuantity?: number;

  /**
   * 兼容性（两位码第1位 A=有 B=→ C=← D=无；第2位 1～9=同时变更 *=无同时变更）
   */
  ecIsCompatible?: string;

  /**
   * 第二供应商（字典 logistics_manufacturing_ec_2nd_vendor）
   */
  ec2ndVendor?: string;

  /**
   * 生产指令（字典 logistics_manufacturing_ec_source_instruction）
   */
  ecInstruction?: string;

  /**
   * 旧品处理（字典 logistics_manufacturing_ec_old_part_disposition）
   */
  ecOldPartDisposition?: string;

  /**
   * 新物料编码
   */
  ecNewMaterialCode?: string;

  /**
   * 生管旧品处理（TaktEcSeikan.EcOldPartDisposition；字典 logistics_manufacturing_ec_old_part_disposition）
   */
  oldProductHandling?: string;

  /**
   * 停产状态（字典 logistics_materials_material_discontinued_status）
   */
  discontinuedStatus: string;
}

/**
 * 旧品管制查询 DTO
 * @description 对应后端 TaktEcLegacyProductQueryDto
 */
export interface EcLegacyProductQuery extends TaktPagedQuery {
  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 区域文化编码
   */
  cultureCode?: string;

  /**
   * 设变单号
   */
  ecCode?: string;

  /**
   * 机种编码
   */
  ecModelCode?: string;

  /**
   * 旧物料编码
   */
  ecOldMaterialCode?: string;
}

/**
 * 旧品管制更新 DTO
 * @description 对应后端 TaktEcLegacyProductUpdateDto
 */
export interface EcLegacyProductUpdate {
  /**
   * 设变明细 ID
   */
  ecDetailId: string;

  /**
   * 生管旧品处理（TaktEcSeikan.EcOldPartDisposition；字典 logistics_manufacturing_ec_old_part_disposition）
   */
  oldProductHandling?: string;

  /**
   * 停产状态（字典 logistics_materials_material_discontinued_status）
   */
  discontinuedStatus: string;

  /**
   * 备注
   */
  remark?: string;
}
