// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：purchaseForecast.types.ts
// 创建时间：2026-08-06
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/procurement 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * Takt采购预测实体（公司级；我方发给供应商的需求预测，结构对齐 TaktSalesForecast；同编码多版靠发出版本号；不进入我方 MDS/MRP 采购计划）
 * 对应前端 TaktPurchaseForecastDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 PurchaseForecast
 * @description 对应后端 TaktPurchaseForecastDto
 */
export interface PurchaseForecast extends ApprovalDtoBase {

  /**
   * 采购预测编码（租户+公司+工厂内与发出版本号组合业务唯一）
   */
  purchaseForecastCode?: string;

  /**
   * 计划编制日期（业务计划日；与发出日期分离）
   */
  planDate?: string;

  /**
   * 发出日期（我方将该版采购预测发给供应商的日期；对应销售预测的接收日期）
   */
  sendDate?: string;

  /**
   * 发出版本号（同工厂+预测编码下递增；从 1 起；对应销售预测的接收版本号）
   */
  sendVersionNo?: number;

  /**
   * 产品（四阶第 1 层；仅允许固定字面量 Product，长度固定 7；服务层写入强制覆盖）
   */
  salesProduct?: string;

  /**
   * 产品类别（字典 logistics_manufacturing_mds_product_category；DictValue=CAD/ISD/PAD；四阶第 2 层）
   */
  productCategoryCode?: string;

  /**
   * 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode；四阶第 3 层）
   */
  profitCenterCode?: string;

  /**
   * 机种编码（关联 TaktModelDestination.ModelCode；四阶第 4 层）
   */
  modelCode?: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode；具体 SKU）
   */
  materialCode?: string;

  /**
   * 物料描述（回填：随物料）
   */
  materialDescription?: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options；汇总计划时可为空，DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
   */
  supplierName1?: string;

  /**
   * 计划人员工ID（选项 TaktEmployees/options；DictValue=Id）
   */
  plannerGroupCode?: string;

  /**
   * 计划人（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  planBy?: string;

  /**
   * 计划总数量（基本单位数量；通常汇总版本 002）
   */
  totalQuantity?: number;

  /**
   * 计划总金额
   */
  totalAmount?: number;

  /**
   * 已转采购数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 已转采购金额
   */
  convertedAmount?: number;

  /**
   * 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
   */
  planStatus?: number;

  /**
   * 转换状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus?: number;

  /**
   * 计划说明
   */
  planDescription?: string;

  /**
   * 采购预测明细列表（主子表；一行=财年×月计划量 001/002/增减；维度在主表）（子表，级联保存）
   */
  items?: PurchaseForecastItemCreate[];

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
 * PurchaseForecast 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PurchaseForecastExport
 * @description 对应后端 TaktPurchaseForecastExportDto
 */
export interface PurchaseForecastExport {
  /**
   * PurchaseForecastID
   */
  purchaseForecastId: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 采购预测编码（租户+公司+工厂内与发出版本号组合业务唯一）
   */
  purchaseForecastCode: string;

  /**
   * 计划编制日期（业务计划日；与发出日期分离）
   */
  planDate: string;

  /**
   * 发出日期（我方将该版采购预测发给供应商的日期；对应销售预测的接收日期）
   */
  sendDate: string;

  /**
   * 发出版本号（同工厂+预测编码下递增；从 1 起；对应销售预测的接收版本号）
   */
  sendVersionNo: number;

  /**
   * 产品（四阶第 1 层；仅允许固定字面量 Product，长度固定 7；服务层写入强制覆盖）
   */
  salesProduct: string;

  /**
   * 产品类别（字典 logistics_manufacturing_mds_product_category；DictValue=CAD/ISD/PAD；四阶第 2 层）
   */
  productCategoryCode: string;

  /**
   * 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode；四阶第 3 层）
   */
  profitCenterCode?: string;

  /**
   * 机种编码（关联 TaktModelDestination.ModelCode；四阶第 4 层）
   */
  modelCode?: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode；具体 SKU）
   */
  materialCode: string;

  /**
   * 物料描述（回填：随物料）
   */
  materialDescription: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options；汇总计划时可为空，DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 供应商名称1（冗余，与 TaktSupplier.SupplierName1 对齐）
   */
  supplierName1?: string;

  /**
   * 计划人员工ID（选项 TaktEmployees/options；DictValue=Id）
   */
  plannerGroupCode?: string;

  /**
   * 计划人（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  planBy: string;

  /**
   * 计划总数量（基本单位数量；通常汇总版本 002）
   */
  totalQuantity: number;

  /**
   * 计划总金额
   */
  totalAmount: number;

  /**
   * 已转采购数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 已转采购金额
   */
  convertedAmount: number;

  /**
   * 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
   */
  planStatus: number;

  /**
   * 转换状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus: number;

  /**
   * 计划说明
   */
  planDescription?: string;

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

