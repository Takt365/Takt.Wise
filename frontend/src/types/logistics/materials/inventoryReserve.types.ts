// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：inventoryReserve.types.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/materials 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 存货跌价准备实体（CAS 存货准则 / IAS 2） 计量原则：资产负债表日按「成本与可变现净值孰低」；成本高于可变现净值时计提跌价准备； 可变现净值回升时，在原已计提金额内转回（CAS/IFRS 允许；与 US GAAP ASC 330 一般禁止转回不同）。 唯一键：租户 + 公司 + 工厂 + 期间 + 物料 + 评估类别（期间存当月首日表示年月）
 * 对应前端 TaktInventoryReserveDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 InventoryReserve
 * @description 对应后端 TaktInventoryReserveDto
 */
export interface InventoryReserve extends CompanyDtoBase {

  /**
   * 期间（资产负债表日所属会计期间；业务存当月首日，表示年月，如 2026-07-01 → 2026年7月）
   */
  periodDate?: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 物料描述（冗余展示）
   */
  materialDescription?: string;

  /**
   * 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
   */
  valuation?: string;

  /**
   * 计提范围（字典 logistics_materials_inventory_reserve_scope；1=按单个存货项目，2=按存货类别；CAS 优先单个项目，数量繁多单价较低时可按类别）
   */
  provisionScope?: number;

  /**
   * 库存数量（基本单位，4 位小数）
   */
  stockQuantity?: number;

  /**
   * 单位成本（存货取得/结存单位成本；与币种一致，5 位小数）
   */
  unitCost?: number;

  /**
   * 存货成本合计（CAS/IAS 2 之 Cost；通常≈库存数量×单位成本；跌价前账面余额）
   */
  inventoryCost?: number;

  /**
   * 估计售价合计（CAS/IAS 2：预计售价；普通销售过程中的估计售价）
   */
  estimatedSellingPrice?: number;

  /**
   * 至完工估计将要发生的成本合计（在产品/半成品完工尚需成本；产成品一般为 0）
   */
  estimatedCompletionCost?: number;

  /**
   * 销售估计费用及税费合计（CAS/IAS 2：销售所需估计费用；含相关税费）
   */
  estimatedSellingCost?: number;

  /**
   * 可变现净值（NRV = 估计售价 − 至完工估计成本 − 估计销售费用及税费；不得为负时业务层可钳制为 0）
   */
  netRealizableValue?: number;

  /**
   * 单位可变现净值（便于与单位成本比较；可为 0 表示未单独维护）
   */
  unitNetRealizableValue?: number;

  /**
   * 期初跌价准备余额（存货跌价准备科目期初贷方余额）
   */
  openingProvision?: number;

  /**
   * 本期计提金额（成本高于可变现净值时新增计提；计入资产减值损失/营业成本等，按公司会计政策）
   */
  provisionAmount?: number;

  /**
   * 本期转回金额（可变现净值回升时，在原已计提跌价准备金额内转回；CAS/IAS 2 允许）
   */
  reversalAmount?: number;

  /**
   * 期末跌价准备余额（= 期初 + 本期计提 − 本期转回；不得低于 0，且不应使账面价值低于可变现净值）
   */
  closingProvision?: number;

  /**
   * 本期净损益影响（= 本期计提 − 本期转回；正数为净计提，负数为净转回）
   */
  impairmentLoss?: number;

  /**
   * 账面价值（Carrying amount = 存货成本 − 期末跌价准备；报表列示金额，应 ≤ 可变现净值当成本更高时取孰低）
   */
  carryingAmount?: number;

  /**
   * 币种（字典 accounting_financial_currency_code，DictValue=CNY/USD 等）
   */
  currencyCode?: string;

  /**
   * 跌价原因说明（业务备注：滞销、毁损、市价下跌等）
   */
  impairmentReason?: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=停用）
   */
  provisionStatus?: number;

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
 * InventoryReserve 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 InventoryReserveExport
 * @description 对应后端 TaktInventoryReserveExportDto
 */
export interface InventoryReserveExport {
  /**
   * InventoryReserveID
   */
  inventoryReserveId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 期间（资产负债表日所属会计期间；业务存当月首日，表示年月，如 2026-07-01 → 2026年7月）
   */
  periodDate: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode: string;

  /**
   * 物料描述（冗余展示）
   */
  materialDescription?: string;

  /**
   * 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
   */
  valuation: string;

  /**
   * 计提范围（字典 logistics_materials_inventory_reserve_scope；1=按单个存货项目，2=按存货类别；CAS 优先单个项目，数量繁多单价较低时可按类别）
   */
  provisionScope: number;

  /**
   * 库存数量（基本单位，4 位小数）
   */
  stockQuantity: number;

  /**
   * 单位成本（存货取得/结存单位成本；与币种一致，5 位小数）
   */
  unitCost: number;

  /**
   * 存货成本合计（CAS/IAS 2 之 Cost；通常≈库存数量×单位成本；跌价前账面余额）
   */
  inventoryCost: number;

  /**
   * 估计售价合计（CAS/IAS 2：预计售价；普通销售过程中的估计售价）
   */
  estimatedSellingPrice: number;

  /**
   * 至完工估计将要发生的成本合计（在产品/半成品完工尚需成本；产成品一般为 0）
   */
  estimatedCompletionCost: number;

  /**
   * 销售估计费用及税费合计（CAS/IAS 2：销售所需估计费用；含相关税费）
   */
  estimatedSellingCost: number;

  /**
   * 可变现净值（NRV = 估计售价 − 至完工估计成本 − 估计销售费用及税费；不得为负时业务层可钳制为 0）
   */
  netRealizableValue: number;

  /**
   * 单位可变现净值（便于与单位成本比较；可为 0 表示未单独维护）
   */
  unitNetRealizableValue: number;

  /**
   * 期初跌价准备余额（存货跌价准备科目期初贷方余额）
   */
  openingProvision: number;

  /**
   * 本期计提金额（成本高于可变现净值时新增计提；计入资产减值损失/营业成本等，按公司会计政策）
   */
  provisionAmount: number;

  /**
   * 本期转回金额（可变现净值回升时，在原已计提跌价准备金额内转回；CAS/IAS 2 允许）
   */
  reversalAmount: number;

  /**
   * 期末跌价准备余额（= 期初 + 本期计提 − 本期转回；不得低于 0，且不应使账面价值低于可变现净值）
   */
  closingProvision: number;

  /**
   * 本期净损益影响（= 本期计提 − 本期转回；正数为净计提，负数为净转回）
   */
  impairmentLoss: number;

  /**
   * 账面价值（Carrying amount = 存货成本 − 期末跌价准备；报表列示金额，应 ≤ 可变现净值当成本更高时取孰低）
   */
  carryingAmount: number;

  /**
   * 币种（字典 accounting_financial_currency_code，DictValue=CNY/USD 等）
   */
  currencyCode: string;

  /**
   * 跌价原因说明（业务备注：滞销、毁损、市价下跌等）
   */
  impairmentReason?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=停用）
   */
  provisionStatus: number;

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

