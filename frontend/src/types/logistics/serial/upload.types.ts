// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/serial
// 文件名称：upload.types.ts
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
 * 序列号上传（公司级；发货票维度的送货/装箱明细行）
 * 对应前端 TaktSerialUploadDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SerialUpload
 * @description 对应后端 TaktSerialUploadDto
 */
export interface SerialUpload extends CompanyDtoBase {
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
   * 出库日期
   */
  outboundDate?: string;

  /**
   * 发货单号（固定 9 位）
   */
  shippingInvoiceCode?: string;

  /**
   * 序号（同一工厂+发货单号内唯一）
   */
  sequenceCode?: number;

  /**
   * 产品物料（选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode；最长 20）
   */
  materialCode?: string;

  /**
   * 合计数量
   */
  totalQuantity?: number;

  /**
   * 序列号（固定 7 位）
   */
  serialCode?: string;

  /**
   * 装箱数量
   */
  packingQuantity?: number;

  /**
   * 运输方式（最长 20）
   */
  transportMode?: string;

  /**
   * 物料描述（最长 40）
   */
  materialText?: string;

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
 * SerialUpload 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SerialUploadExport
 * @description 对应后端 TaktSerialUploadExportDto
 */
export interface SerialUploadExport {
  /**
   * SerialUploadID
   */
  serialUploadId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 出库日期
   */
  outboundDate: string;

  /**
   * 发货单号（固定 9 位）
   */
  shippingInvoiceCode: string;

  /**
   * 序号（同一工厂+发货单号内唯一）
   */
  sequenceCode: number;

  /**
   * 产品物料（选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode；最长 20）
   */
  materialCode: string;

  /**
   * 合计数量
   */
  totalQuantity: number;

  /**
   * 序列号（固定 7 位）
   */
  serialCode: string;

  /**
   * 装箱数量
   */
  packingQuantity: number;

  /**
   * 运输方式（最长 20）
   */
  transportMode: string;

  /**
   * 物料描述（最长 40）
   */
  materialText: string;

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

