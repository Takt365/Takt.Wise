// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/mds
// 文件名称：salesForecast.types.ts
// 创建时间：2026-11-06
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
 * Takt销售预测实体（公司级；客户发给我方的销售预测，可进 MDS；同编码多版靠接收版本号）
 * 对应前端 TaktSalesForecastDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 SalesForecast
 * @description 对应后端 TaktSalesForecastDto
 */
export interface SalesForecast extends ApprovalDtoBase {
  /**
   * SalesForecastID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  salesForecastId: string;

  /**
   * 销售预测编码（租户+公司+工厂内与接收版本号组合业务唯一；前端表单选择编码规则后自动通过 TaktNumbering 销售预测编码规则生成并展示，非手输；单据类型菜单：销售预测）
   */
  salesForecastCode: string;

  /**
   * 计划编制日期（客户侧业务计划日；与接收日期分离）
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
   * 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  materialDescription: string;

  /**
   * 计划批次（转入 MDS 后沿计划链下传，正式工单写入 ProdBatch）
   */
  plannedBatch?: string;

  /**
   * 客户编码（选项 TaktCustomers/options；汇总计划时可为空，DictValue=Id）
   */
  customerCode?: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1?: string;

  /**
   * 计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）
   */
  plannerGroupCode?: string;

  /**
   * 计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）
   */
  plannerGroupName?: string;

  /**
   * 计划总数量（各月最新版本数量之和）
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
   * 销售预测明细列表（主子表；一行=财年×月×版本，版本号 1～20；维度在主表） （子表：TaktSalesForecastItem）
   */
  items?: SalesForecastItem[];

}


/**
 * SalesForecast 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SalesForecastQuery
 * @description 对应后端 TaktSalesForecastQueryDto
 */
export interface SalesForecastQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 销售预测编码（租户+公司+工厂内与接收版本号组合业务唯一；前端表单选择编码规则后自动通过 TaktNumbering 销售预测编码规则生成并展示，非手输；单据类型菜单：销售预测）
   */
  salesForecastCode?: string;

  /**
   * 计划编制日期（客户侧业务计划日；与接收日期分离）（范围查询-开始）
   */
  planDateStart?: string;

  /**
   * 计划编制日期（客户侧业务计划日；与接收日期分离）（范围查询-结束）
   */
  planDateEnd?: string;

  /**
   * 接收日期（我方收到该版客户销售预测的日期）（范围查询-开始）
   */
  receiveDateStart?: string;

  /**
   * 接收日期（我方收到该版客户销售预测的日期）（范围查询-结束）
   */
  receiveDateEnd?: string;

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
   * 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  materialDescription?: string;

  /**
   * 计划批次（转入 MDS 后沿计划链下传，正式工单写入 ProdBatch）
   */
  plannedBatch?: string;

  /**
   * 客户编码（选项 TaktCustomers/options；汇总计划时可为空，DictValue=Id）
   */
  customerCode?: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1?: string;

  /**
   * 计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）
   */
  plannerGroupCode?: string;

  /**
   * 计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）
   */
  plannerGroupName?: string;

  /**
   * 计划总数量（各月最新版本数量之和）
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
   * 审批状态（字典 sys_approval_status；与 TaktApprovalEntityBase.ApprovalStatus 一致）
   */
  approvalStatus?: number;

  /**
   * 发起人ID
   */
  initiatorId?: string;

  /**
   * 发起时间（范围查询-开始）
   */
  initiatedAtStart?: string;

  /**
   * 发起时间（范围查询-结束）
   */
  initiatedAtEnd?: string;

  /**
   * 最终审批人ID
   */
  approvedBy?: string;

  /**
   * 最终审批时间（范围查询-开始）
   */
  approvedAtStart?: string;

  /**
   * 最终审批时间（范围查询-结束）
   */
  approvedAtEnd?: string;

  /**
   * 流程实例 ID
   */
  flowInstanceId?: string;

  /**
   * 创建时间（范围查询-开始）
   */
  createdAtStart?: string;

  /**
   * 创建时间（范围查询-结束）
   */
  createdAtEnd?: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建SalesForecast DTO
 * 对应前端 SalesForecastCreate
 * @description 对应后端 TaktSalesForecastCreateDto
 */
export interface SalesForecastCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode: string;

  /**
   * 销售预测编码（租户+公司+工厂内与接收版本号组合业务唯一；前端表单选择编码规则后自动通过 TaktNumbering 销售预测编码规则生成并展示，非手输；单据类型菜单：销售预测）
   */
  salesForecastCode: string;

  /**
   * 编码规则编码（前端表单从 TaktNumberings/options 选择；对应 TaktNumbering.RuleCode；不落库）
   */
  numberingRuleCode?: string;

  /**
   * 计划编制日期（客户侧业务计划日；与接收日期分离）
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
   * 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  materialDescription: string;

  /**
   * 计划批次（转入 MDS 后沿计划链下传，正式工单写入 ProdBatch）
   */
  plannedBatch?: string;

  /**
   * 客户编码（选项 TaktCustomers/options；汇总计划时可为空，DictValue=Id）
   */
  customerCode?: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1?: string;

  /**
   * 计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）
   */
  plannerGroupCode?: string;

  /**
   * 计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）
   */
  plannerGroupName?: string;

  /**
   * 计划总数量（各月最新版本数量之和）
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
   * 销售预测明细列表（主子表；一行=财年×月×版本，版本号 1～20；维度在主表）（子表，级联保存）
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
 * 更新SalesForecast DTO
 * 继承 TaktSalesForecastCreateDto，添加 SalesForecastId 字段
 * 对应前端 SalesForecastUpdate
 * @description 对应后端 TaktSalesForecastUpdateDto
 */
export interface SalesForecastUpdate extends SalesForecastCreate {
  /**
   * SalesForecastID（标识要更新的实体）
   */
  salesForecastId: string;

  /**
   * 销售预测明细列表（主子表；一行=财年×月×版本，版本号 1～20；维度在主表）（子表，级联保存）
   */
  items?: any;

}


/**
 * SalesForecast 状态更新 DTO
 * 对应前端 SalesForecastStatus
 * @description 对应后端 TaktSalesForecastStatusDto
 */
export interface SalesForecastStatus {
  /**
   * SalesForecastID
   */
  salesForecastId: string;

  /**
   * 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
   */
  planStatus: number;

}


/**
 * SalesForecast 导入模板行 DTO
 * 对应前端 SalesForecastTemplate
 * @description 对应后端 TaktSalesForecastTemplateDto
 */
export interface SalesForecastTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode?: string;

  /**
   * 销售预测编码（租户+公司+工厂内与接收版本号组合业务唯一；前端表单选择编码规则后自动通过 TaktNumbering 销售预测编码规则生成并展示，非手输；单据类型菜单：销售预测）
   */
  salesForecastCode?: string;

  /**
   * 计划编制日期（客户侧业务计划日；与接收日期分离）
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
   * 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  materialDescription?: string;

  /**
   * 计划批次（转入 MDS 后沿计划链下传，正式工单写入 ProdBatch）
   */
  plannedBatch?: string;

  /**
   * 客户编码（选项 TaktCustomers/options；汇总计划时可为空，DictValue=Id）
   */
  customerCode?: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1?: string;

  /**
   * 计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）
   */
  plannerGroupCode?: string;

  /**
   * 计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）
   */
  plannerGroupName?: string;

  /**
   * 计划总数量（各月最新版本数量之和）
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
   * 销售预测明细列表（主子表；一行=财年×月×版本，版本号 1～20；维度在主表）（子表，级联保存）
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
 * SalesForecast 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SalesForecastImport
 * @description 对应后端 TaktSalesForecastImportDto
 */
export interface SalesForecastImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode?: string;

  /**
   * 销售预测编码（租户+公司+工厂内与接收版本号组合业务唯一；前端表单选择编码规则后自动通过 TaktNumbering 销售预测编码规则生成并展示，非手输；单据类型菜单：销售预测）
   */
  salesForecastCode?: string;

  /**
   * 计划编制日期（客户侧业务计划日；与接收日期分离）
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
   * 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  materialDescription?: string;

  /**
   * 计划批次（转入 MDS 后沿计划链下传，正式工单写入 ProdBatch）
   */
  plannedBatch?: string;

  /**
   * 客户编码（选项 TaktCustomers/options；汇总计划时可为空，DictValue=Id）
   */
  customerCode?: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1?: string;

  /**
   * 计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）
   */
  plannerGroupCode?: string;

  /**
   * 计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）
   */
  plannerGroupName?: string;

  /**
   * 计划总数量（各月最新版本数量之和）
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
   * 销售预测明细列表（主子表；一行=财年×月×版本，版本号 1～20；维度在主表）（子表，级联保存）
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
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 销售预测编码（租户+公司+工厂内与接收版本号组合业务唯一；前端表单选择编码规则后自动通过 TaktNumbering 销售预测编码规则生成并展示，非手输；单据类型菜单：销售预测）
   */
  salesForecastCode: string;

  /**
   * 计划编制日期（客户侧业务计划日；与接收日期分离）
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
   * 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  materialDescription: string;

  /**
   * 计划批次（转入 MDS 后沿计划链下传，正式工单写入 ProdBatch）
   */
  plannedBatch?: string;

  /**
   * 客户编码（选项 TaktCustomers/options；汇总计划时可为空，DictValue=Id）
   */
  customerCode?: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1?: string;

  /**
   * 计划组编码（选项 TaktApsGroups/options?apsGroupCategory=1；DictValue=ApsGroupCode）
   */
  plannerGroupCode?: string;

  /**
   * 计划组名称（冗余：按 PlannerGroupCode 取 TaktApsGroups.ApsGroupName 联动）
   */
  plannerGroupName?: string;

  /**
   * 计划总数量（各月最新版本数量之和）
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

