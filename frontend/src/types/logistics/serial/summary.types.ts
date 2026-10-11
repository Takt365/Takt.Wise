// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/serial
// 文件名称：summary.types.ts
// 创建时间：2026-07-20
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/serial 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 序列号汇总（公司级；一行对应一笔入库序列及其可选出库对照）
 * 对应前端 TaktSerialSummaryDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SerialSummary
 * @description 对应后端 TaktSerialSummaryDto
 */
export interface SerialSummary extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 入库单号
   */
  inboundCode?: string;

  /**
   * 入库日期
   */
  inboundDate?: string;

  /**
   * 产品物料（选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 入库序列号（计算后的业务序号；租户+公司+工厂内唯一）
   */
  inboundSerialCode?: string;

  /**
   * 入库数量
   */
  inboundQuantity?: number;

  /**
   * 产品入库序列号（原始扫描号码）
   */
  productInboundSerialCode?: string;

  /**
   * 出库单号（未出库时为空）
   */
  outboundCode?: string;

  /**
   * 发货单号（未出库时为空）
   */
  shippingInvoiceCode?: string;

  /**
   * 装车日期（未装车时为空）
   */
  loadingDate?: string;

  /**
   * 仕向地（选项 TaktModelDestinations/options，DictValue=DestinationCode）
   */
  destination?: string;

  /**
   * 目的地港（字典 logistics_serial_destination_port_code；DictValue 为港口/运输编码，如 ACE_AIR、VIE）
   */
  destinationPort?: string;

  /**
   * 出库日期（未出库时为空）
   */
  outboundDate?: string;

  /**
   * 出库序列号（计算后的业务序号；未出库时为空）
   */
  outboundSerialCode?: string;

  /**
   * 出库数量
   */
  outboundQuantity?: number;

  /**
   * 产品出库序列号（原始扫描号码；未出库时为空）
   */
  productOutboundSerialCode?: string;

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
 * SerialSummary 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SerialSummaryExport
 * @description 对应后端 TaktSerialSummaryExportDto
 */
export interface SerialSummaryExport {
  /**
   * SerialSummaryID
   */
  serialSummaryId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 入库单号
   */
  inboundCode: string;

  /**
   * 入库日期
   */
  inboundDate: string;

  /**
   * 产品物料（选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode: string;

  /**
   * 入库序列号（计算后的业务序号；租户+公司+工厂内唯一）
   */
  inboundSerialCode: string;

  /**
   * 入库数量
   */
  inboundQuantity: number;

  /**
   * 产品入库序列号（原始扫描号码）
   */
  productInboundSerialCode: string;

  /**
   * 出库单号（未出库时为空）
   */
  outboundCode: string;

  /**
   * 发货单号（未出库时为空）
   */
  shippingInvoiceCode: string;

  /**
   * 装车日期（未装车时为空）
   */
  loadingDate?: string;

  /**
   * 仕向地（选项 TaktModelDestinations/options，DictValue=DestinationCode）
   */
  destination: string;

  /**
   * 目的地港（字典 logistics_serial_destination_port_code；DictValue 为港口/运输编码，如 ACE_AIR、VIE）
   */
  destinationPort: string;

  /**
   * 出库日期（未出库时为空）
   */
  outboundDate?: string;

  /**
   * 出库序列号（计算后的业务序号；未出库时为空）
   */
  outboundSerialCode: string;

  /**
   * 出库数量
   */
  outboundQuantity: number;

  /**
   * 产品出库序列号（原始扫描号码；未出库时为空）
   */
  productOutboundSerialCode: string;

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

