// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/bom
// 文件名称：billOfMaterialExplosion.types.ts
// 创建时间：2026-08-28
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/bom 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================


/**
 * BOM 递归展开查询参数
 * 对应前端 BillOfMaterialExplosionQuery
 * @description 对应后端 TaktBillOfMaterialExplosionQueryDto
 */
export interface BillOfMaterialExplosionQuery {
  /**
   * 展开根 BOM ID
   */
  billOfMaterialId: string;

  /**
   * 需求数量（父件数量，默认 1）
   */
  quantity: number;

  /**
   * 最大展开层级（0=仅父件，1=仅直接子件；默认 20）
   */
  maxLevel: number;

  /**
   * 是否包含层级 0 父件行
   */
  includeLevelZero: boolean;

}


/**
 * BOM 递归展开结果
 * 对应前端 BillOfMaterialExplosion
 * @description 对应后端 TaktBillOfMaterialExplosionDto
 */
export interface BillOfMaterialExplosion {
  /**
   * 根 BOM ID
   */
  billOfMaterialId: string;

  /**
   * BOM 编码
   */
  bomCode: string;

  /**
   * 父件物料编码
   */
  parentMaterialCode: string;

  /**
   * 父件物料名称
   */
  parentMaterialName: string;

  /**
   * 需求数量
   */
  quantity: number;

  /**
   * 展开行列表（按层级、行号排序）
   */
  lines: BillOfMaterialExplosionLine[];

  /**
   * 父件物料描述
   */
  parentMaterialDescription?: string;

}


/**
 * BOM 展开行（运行时计算，不落库）
 * 对应前端 BillOfMaterialExplosionLine
 * @description 对应后端 TaktBillOfMaterialExplosionLineDto
 */
export interface BillOfMaterialExplosionLine {
  /**
   * 层级（0=父件，1=直接子件，依次递增）
   */
  hierarchyLevel: number;

  /**
   * 层级显示前缀（如 . / .. / ...）
   */
  levelPrefix: string;

  /**
   * 来源 BOM ID
   */
  sourceBillOfMaterialId: string;

  /**
   * 来源 BOM 明细行 ID（层级 0 为空）
   */
  sourceBillOfMaterialItemId?: string;

  /**
   * 行号（层级 0 为 0）
   */
  lineNumber: number;

  /**
   * 子项物料 ID（层级 0 为父件 ID）
   */
  materialId: string;

  /**
   * 子项物料编码
   */
  materialCode: string;

  /**
   * 子项物料名称
   */
  materialName?: string;

  /**
   * 直接父件物料编码（展开路径上的上一级）
   */
  immediateParentMaterialCode: string;

  /**
   * 单位用量（对直接父件基本数量）
   */
  usageQuantity: number;

  /**
   * 单位
   */
  materialUnit: string;

  /**
   * 损耗率
   */
  scrapRate: number;

  /**
   * 累计需求量（考虑上层数量传递）
   */
  cumulativeQuantity: number;

  /**
   * 工序号
   */
  operationSeq: number;

  /**
   * 工作中心
   */
  workCenter?: string;

  /**
   * 位号
   */
  position?: string;

  /**
   * 是否虚拟件
   */
  isPhantom: number;

  /**
   * 是否可选件
   */
  isOptional: number;

  /**
   * 替代组号
   */
  substituteGroup?: string;

  /**
   * 是否存在下级 BOM
   */
  hasChildBom: number;

  /**
   * 是否循环引用（检测到环时标记，不再下钻）
   */
  isCircular: number;

  /**
   * 子项物料描述
   */
  materialDescription: string;

}

