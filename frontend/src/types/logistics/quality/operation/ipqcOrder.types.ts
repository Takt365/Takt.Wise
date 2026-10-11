// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/quality/operation
// 文件名称：ipqcOrder.types.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/operation 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * IPQC制程检验单实体
 * 对应前端 TaktIpqcOrderDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 IpqcOrder
 * @description 对应后端 TaktIpqcOrderDto
 */
export interface IpqcOrder extends CompanyDtoBase {

  /**
   * 来源单号（选项 TaktProductionOrders/options，DictValue=ProdOrderCode）
   */
  sourceCode?: string;

  /**
   * 检验日期
   */
  inspectionDate?: string;

  /**
   * IPQC检验单编码（唯一索引，根据来源单号自动生成）
   */
  ipqcOrderCode?: string;

  /**
   * 工序编码
   */
  processCode?: string;

  /**
   * 工序名称
   */
  processName?: string;

  /**
   * 生产总数
   */
  totalProductionQuantity?: number;

  /**
   * 总抽样数量（自动计算 = 各明细抽样数量合计）
   */
  totalSampleQuantity?: number;

  /**
   * 总合格数量（自动计算 = 各明细合格数量合计）
   */
  totalQualifiedQuantity?: number;

  /**
   * 总不合格数量（自动计算 = 各明细不合格数量合计）
   */
  totalUnqualifiedQuantity?: number;

  /**
   * 总验退数量（自动计算 = 各明细验退数量合计）
   */
  totalInspectionReturnQuantity?: number;

  /**
   * 判定人（人员代码）
   */
  judgeBy?: string;

  /**
   * 判定日期
   */
  judgeDate?: string;

  /**
   * 判定说明
   */
  judgeDescription?: string;

  /**
   * 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）
   */
  judgeStatus?: number;

  /**
   * IPQC检验单明细列表（主子表关系）（子表，级联保存）
   */
  items?: IpqcOrderItemCreate[];

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
 * IpqcOrder 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 IpqcOrderExport
 * @description 对应后端 TaktIpqcOrderExportDto
 */
export interface IpqcOrderExport {
  /**
   * IpqcOrderID
   */
  ipqcOrderId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 来源单号（选项 TaktProductionOrders/options，DictValue=ProdOrderCode）
   */
  sourceCode: string;

  /**
   * 检验日期
   */
  inspectionDate?: string;

  /**
   * IPQC检验单编码（唯一索引，根据来源单号自动生成）
   */
  ipqcOrderCode: string;

  /**
   * 工序编码
   */
  processCode: string;

  /**
   * 工序名称
   */
  processName: string;

  /**
   * 生产总数
   */
  totalProductionQuantity: number;

  /**
   * 总抽样数量（自动计算 = 各明细抽样数量合计）
   */
  totalSampleQuantity: number;

  /**
   * 总合格数量（自动计算 = 各明细合格数量合计）
   */
  totalQualifiedQuantity: number;

  /**
   * 总不合格数量（自动计算 = 各明细不合格数量合计）
   */
  totalUnqualifiedQuantity: number;

  /**
   * 总验退数量（自动计算 = 各明细验退数量合计）
   */
  totalInspectionReturnQuantity: number;

  /**
   * 判定人（人员代码）
   */
  judgeBy?: string;

  /**
   * 判定日期
   */
  judgeDate?: string;

  /**
   * 判定说明
   */
  judgeDescription?: string;

  /**
   * 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）
   */
  judgeStatus: number;

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

