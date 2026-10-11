// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/routine/help-desk
// 文件名称：itAsset.types.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/help-desk 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 服务台 IT 设备保修扩展实体（与财务 TaktAsset 按 AssetCode 一对一扩展）
 * 对应前端 TaktItAssetDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ItAsset
 * @description 对应后端 TaktItAssetDto
 */
export interface ItAsset extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 资产号码
   */
  assetCode?: string;

  /**
   * 保修类型（见 TaktWarrantyType）
   */
  warrantyType?: number;

  /**
   * 保修开始日期
   */
  warrantyStartDate?: string;

  /**
   * 保修到期日
   */
  warrantyExpiryDate?: string;

  /**
   * 保修服务商/厂商
   */
  warrantyProvider?: string;

  /**
   * 保修合同编码
   */
  warrantyContractCode?: string;

  /**
   * 服务电话
   */
  serviceHotline?: string;

  /**
   * 服务邮箱
   */
  serviceEmail?: string;

  /**
   * 维保到期日
   */
  maintenanceExpiryDate?: string;

  /**
   * 上次维保日期
   */
  lastMaintenanceDate?: string;

  /**
   * 下次维保日期
   */
  nextMaintenanceDate?: string;

  /**
   * 保修/维保说明
   */
  warrantyRemark?: string;

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
 * ItAsset 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ItAssetExport
 * @description 对应后端 TaktItAssetExportDto
 */
export interface ItAssetExport {
  /**
   * ItAssetID
   */
  itAssetId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 资产号码
   */
  assetCode: string;

  /**
   * 保修类型（见 TaktWarrantyType）
   */
  warrantyType: number;

  /**
   * 保修开始日期
   */
  warrantyStartDate?: string;

  /**
   * 保修到期日
   */
  warrantyExpiryDate?: string;

  /**
   * 保修服务商/厂商
   */
  warrantyProvider?: string;

  /**
   * 保修合同编码
   */
  warrantyContractCode?: string;

  /**
   * 服务电话
   */
  serviceHotline?: string;

  /**
   * 服务邮箱
   */
  serviceEmail?: string;

  /**
   * 维保到期日
   */
  maintenanceExpiryDate?: string;

  /**
   * 上次维保日期
   */
  lastMaintenanceDate?: string;

  /**
   * 下次维保日期
   */
  nextMaintenanceDate?: string;

  /**
   * 保修/维保说明
   */
  warrantyRemark?: string;

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

