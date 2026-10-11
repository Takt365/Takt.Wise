// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/quality/operation
// 文件名称：fqcOrder.types.ts
// 创建时间：2026-07-23
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
 * FQC出货检验单实体
 * 对应前端 TaktFqcOrderDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 FqcOrder
 * @description 对应后端 TaktFqcOrderDto
 */
export interface FqcOrder extends CompanyDtoBase {

  /**
   * 来源单号（选项 TaktSalesOrders/options；DictValue=SalesOrderCode）
   */
  sourceCode?: string;

  /**
   * 检验日期
   */
  inspectionDate?: string;

  /**
   * FQC检验单编码（唯一索引，根据来源单号自动生成）
   */
  fqcOrderCode?: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 总入库数
   */
  totalWarehouseQuantity?: number;

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
   * 判定人（选项 TaktEmployees/options；DictValue=EmployeeCode）
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
   * 判定状态（字典 logistics_quality_judge_status）
   */
  judgeStatus?: number;

  /**
   * FQC检验单明细列表（主子表关系）（子表，级联保存）
   */
  items?: FqcOrderItemCreate[];

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
 * FqcOrder 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 FqcOrderExport
 * @description 对应后端 TaktFqcOrderExportDto
 */
export interface FqcOrderExport {
  /**
   * FqcOrderID
   */
  fqcOrderId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 来源单号（选项 TaktSalesOrders/options；DictValue=SalesOrderCode）
   */
  sourceCode: string;

  /**
   * 检验日期
   */
  inspectionDate?: string;

  /**
   * FQC检验单编码（唯一索引，根据来源单号自动生成）
   */
  fqcOrderCode: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 总入库数
   */
  totalWarehouseQuantity: number;

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
   * 判定人（选项 TaktEmployees/options；DictValue=EmployeeCode）
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
   * 判定状态（字典 logistics_quality_judge_status）
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

