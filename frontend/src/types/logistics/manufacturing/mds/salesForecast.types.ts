// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/mds
// 文件名称：salesForecast.types.ts
// 创建时间：2026-07-29
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/mds 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * Takt销售预测实体（公司级；MDS 独立需求源头；一单一物料，四阶维度在主表；财年在明细）
 * 对应前端 TaktSalesForecastDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 SalesForecast
 * @description 对应后端 TaktSalesForecastDto
 */
export interface SalesForecast extends ApprovalDtoBase {

  /**
   * 销售预测编码（租户+公司+工厂内业务唯一）
   */
  salesForecastCode?: string;

  /**
   * 计划编制日期
   */
  planDate?: string;

  /**
   * 接收日期（我方收到该版客户销售预测的日期）
   */
  receiveDate?: string;

  /**
   * 接收版本号（同工厂+预测编码下递增；从 1 起）
   */
  receiveVersionNo?: number;

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
   * 客户编码（选项 TaktCustomers/options；汇总计划时可为空，DictValue=Id）
   */
  customerCode?: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1?: string;

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
   * 已转生产/销售数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 已转生产/销售金额
   */
  convertedAmount?: number;

  /**
   * 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
   */
  planStatus?: number;

  /**
   * 转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus?: number;

  /**
   * 计划说明
   */
  planDescription?: string;

  /**
   * 销售预测明细列表（主子表；一行=财年×月计划量 001/002/增减；维度在主表）（子表，级联保存）
   */
  items?: SalesForecastItemCreate[];

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
 * SalesForecast 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SalesForecastExport
 * @description 对应后端 TaktSalesForecastExportDto
 */
export interface SalesForecastExport {
  /**
   * SalesForecastID
   */
  salesForecastId: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 销售预测编码（租户+公司+工厂内业务唯一）
   */
  salesForecastCode: string;

  /**
   * 计划编制日期
   */
  planDate: string;

  /**
   * 接收日期（我方收到该版客户销售预测的日期）
   */
  receiveDate: string;

  /**
   * 接收版本号（同工厂+预测编码下递增；从 1 起）
   */
  receiveVersionNo: number;

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
   * 客户编码（选项 TaktCustomers/options；汇总计划时可为空，DictValue=Id）
   */
  customerCode?: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1?: string;

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
   * 已转生产/销售数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 已转生产/销售金额
   */
  convertedAmount: number;

  /**
   * 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
   */
  planStatus: number;

  /**
   * 转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
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

