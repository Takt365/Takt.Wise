// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：sourceOfSupply.types.ts
// 创建时间：2026-07-21
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/procurement 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * Takt货源清单实体（公司级；工厂+物料+供货商维度的有效货源清单记录）
 * 对应前端 TaktSourceOfSupplyDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SourceOfSupply
 * @description 对应后端 TaktSourceOfSupplyDto
 */
export interface SourceOfSupply extends CompanyDtoBase {

  /**
   * 货源清单编码（租户+公司内唯一；业务单据号）
   */
  sourceOfSupplyCode?: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 供货商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 采购组（选项 TaktPurchaseGroups/options，DictValue=PurchaseGroupCode）
   */
  purchaseGroup?: string;

  /**
   * 固定（字典 sys_yes_no；1=是，0=否；固定货源清单，MRP/寻源优先选用）
   */
  isFixed?: number;

  /**
   * 冻结（字典 sys_yes_no；1=是，0=否；冻结后禁止新建采购订单引用）
   */
  isBlocked?: number;

  /**
   * 采购单位（字典 logistics_materials_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
   */
  purchaseUnit?: string;

  /**
 * 最小起订量（采购单位数量，整数）
   */
  minOrderQuantity?: number;

  /**
 * 舍入值（基本单位数量，用于数量舍入，整数）
   */
  roundingValue?: number;

  /**
 * 计划交货时间（天数，整数）
   */
  plannedDeliveryTimeDays?: number;

  /**
   * 框架协议号（采购合同/协议编码，可选）
   */
  agreementNumber?: string;

  /**
   * 协议行号
   */
  agreementLineNumber?: number;

  /**
   * 生效日期
   */
  validFrom?: string;

  /**
   * 失效日期
   */
  validTo?: string;

  /**
   * 货源清单状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  sourceStatus?: number;

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
 * SourceOfSupply 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SourceOfSupplyExport
 * @description 对应后端 TaktSourceOfSupplyExportDto
 */
export interface SourceOfSupplyExport {
  /**
   * SourceOfSupplyID
   */
  sourceOfSupplyId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 货源清单编码（租户+公司内唯一；业务单据号）
   */
  sourceOfSupplyCode: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode: string;

  /**
   * 供货商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）
   */
  supplierCode: string;

  /**
   * 采购组（选项 TaktPurchaseGroups/options，DictValue=PurchaseGroupCode）
   */
  purchaseGroup?: string;

  /**
   * 固定（字典 sys_yes_no；1=是，0=否；固定货源清单，MRP/寻源优先选用）
   */
  isFixed: number;

  /**
   * 冻结（字典 sys_yes_no；1=是，0=否；冻结后禁止新建采购订单引用）
   */
  isBlocked: number;

  /**
   * 采购单位（字典 logistics_materials_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
   */
  purchaseUnit: string;

  /**
 * 最小起订量（采购单位数量，整数）
   */
  minOrderQuantity: number;

  /**
 * 舍入值（基本单位数量，用于数量舍入，整数）
   */
  roundingValue: number;

  /**
 * 计划交货时间（天数，整数）
   */
  plannedDeliveryTimeDays: number;

  /**
   * 框架协议号（采购合同/协议编码，可选）
   */
  agreementNumber?: string;

  /**
   * 协议行号
   */
  agreementLineNumber?: number;

  /**
   * 生效日期
   */
  validFrom: string;

  /**
   * 失效日期
   */
  validTo: string;

  /**
   * 排序号（越小越靠前；同物料多货源清单时的优先级）
   */
  sortOrder: number;

  /**
   * 货源清单状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  sourceStatus: number;

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

