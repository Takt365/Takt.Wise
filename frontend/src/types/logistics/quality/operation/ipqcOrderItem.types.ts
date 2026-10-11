// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/quality/operation
// 文件名称：ipqcOrderItem.types.ts
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
 * IPQC制程检验单明细实体
 * 对应前端 TaktIpqcOrderItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 IpqcOrderItem
 * @description 对应后端 TaktIpqcOrderItemDto
 */
export interface IpqcOrderItem extends CompanyDtoBase {
  /**
   * IpqcOrderItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  ipqcOrderItemId: string;

  /**
   * IPQC检验单 ID（关联 TaktIpqcOrder.Id，选项 TaktIpqcOrders/options）
   */
  ipqcOrderId: string;

  /**
   * IPQC检验单 名称（填充字段）
   */
  ipqcOrderName?: string;

  /**
   * IPQC检验单编码（冗余字段，便于查询）
   */
  ipqcOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 物料编码（选项 TaktGeneralMaterials/options，DictValue=MaterialCode）
   */
  materialCode: string;

  /**
   * 物料描述
   */
  materialDescription: string;

  /**
   * 批次号
   */
  batchCode?: string;

  /**
   * 生产数量
   */
  productionQuantity: number;

  /**
   * 检验标准编码（选项 TaktInspectionStandards/options，DictValue=StandardCode）
   */
  standardCode: string;

  /**
   * 抽样方案编码（选项 TaktSamplingSchemes/options，DictValue=SamplingSchemeCode）
   */
  samplingSchemeCode: string;

  /**
   * 检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）
   */
  inspectionMethod: number;

  /**
   * 抽样数量
   */
  sampleQuantity: number;

  /**
   * 合格数量
   */
  qualifiedQuantity: number;

  /**
   * 不合格数量
   */
  unqualifiedQuantity: number;

  /**
   * 验退数量
   */
  inspectionReturnQuantity: number;

  /**
   * 抽检序列号
   */
  sampleSerialCode?: string;

  /**
   * 检验说明
   */
  inspectionDescription?: string;

  /**
   * 检验员（人员代码）
   */
  inspectorBy: string;

  /**
   * 检验日期
   */
  inspectionDate: string;

  /**
   * 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）
   */
  judgeStatus: number;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * IPQC检验单（主表） （主表：TaktIpqcOrder）
   */
  order?: IpqcOrder;

  /**
   * 不良处理记录列表（主子表关系） （子表：TaktIpqcDefectHandling）
   */
  defectHandlings?: IpqcDefectHandling[];

}


/**
 * IpqcOrderItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 IpqcOrderItemQuery
 * @description 对应后端 TaktIpqcOrderItemQueryDto
 */
export interface IpqcOrderItemQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
   */
  plantCode?: string;

  /**
   * IPQC检验单 ID（关联 TaktIpqcOrder.Id，选项 TaktIpqcOrders/options）
   */
  ipqcOrderId?: string;

  /**
   * IPQC检验单编码（冗余字段，便于查询）
   */
  ipqcOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 物料编码（选项 TaktGeneralMaterials/options，DictValue=MaterialCode）
   */
  materialCode?: string;

  /**
   * 物料描述
   */
  materialDescription?: string;

  /**
   * 批次号
   */
  batchCode?: string;

  /**
   * 生产数量
   */
  productionQuantity?: number;

  /**
   * 检验标准编码（选项 TaktInspectionStandards/options，DictValue=StandardCode）
   */
  standardCode?: string;

  /**
   * 抽样方案编码（选项 TaktSamplingSchemes/options，DictValue=SamplingSchemeCode）
   */
  samplingSchemeCode?: string;

  /**
   * 检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）
   */
  inspectionMethod?: number;

  /**
   * 抽样数量
   */
  sampleQuantity?: number;

  /**
   * 合格数量
   */
  qualifiedQuantity?: number;

  /**
   * 不合格数量
   */
  unqualifiedQuantity?: number;

  /**
   * 验退数量
   */
  inspectionReturnQuantity?: number;

  /**
   * 抽检序列号
   */
  sampleSerialCode?: string;

  /**
   * 检验说明
   */
  inspectionDescription?: string;

  /**
   * 检验员（人员代码）
   */
  inspectorBy?: string;

  /**
   * 检验日期（范围查询-开始）
   */
  inspectionDateStart?: string;

  /**
   * 检验日期（范围查询-结束）
   */
  inspectionDateEnd?: string;

  /**
   * 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）
   */
  judgeStatus?: number;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * 创建IpqcOrderItem DTO
 * 对应前端 IpqcOrderItemCreate
 * @description 对应后端 TaktIpqcOrderItemCreateDto
 */
export interface IpqcOrderItemCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
   */
  plantCode: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * IPQC检验单 ID（关联 TaktIpqcOrder.Id，选项 TaktIpqcOrders/options）
   */
  ipqcOrderId: string;

  /**
   * IPQC检验单编码（冗余字段，便于查询）
   */
  ipqcOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 物料编码（选项 TaktGeneralMaterials/options，DictValue=MaterialCode）
   */
  materialCode: string;

  /**
   * 物料描述
   */
  materialDescription: string;

  /**
   * 批次号
   */
  batchCode?: string;

  /**
   * 生产数量
   */
  productionQuantity: number;

  /**
   * 检验标准编码（选项 TaktInspectionStandards/options，DictValue=StandardCode）
   */
  standardCode: string;

  /**
   * 抽样方案编码（选项 TaktSamplingSchemes/options，DictValue=SamplingSchemeCode）
   */
  samplingSchemeCode: string;

  /**
   * 检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）
   */
  inspectionMethod: number;

  /**
   * 抽样数量
   */
  sampleQuantity: number;

  /**
   * 合格数量
   */
  qualifiedQuantity: number;

  /**
   * 不合格数量
   */
  unqualifiedQuantity: number;

  /**
   * 验退数量
   */
  inspectionReturnQuantity: number;

  /**
   * 抽检序列号
   */
  sampleSerialCode?: string;

  /**
   * 检验说明
   */
  inspectionDescription?: string;

  /**
   * 检验员（人员代码）
   */
  inspectorBy: string;

  /**
   * 检验日期
   */
  inspectionDate: string;

  /**
   * 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）
   */
  judgeStatus: number;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 不良处理记录列表（主子表关系）（子表，级联保存）
   */
  defectHandlings?: IpqcDefectHandlingUpdate[];

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
 * 更新IpqcOrderItem DTO
 * 继承 TaktIpqcOrderItemCreateDto，添加 IpqcOrderItemId 字段
 * 对应前端 IpqcOrderItemUpdate
 * @description 对应后端 TaktIpqcOrderItemUpdateDto
 */
export interface IpqcOrderItemUpdate extends IpqcOrderItemCreate {
  /**
   * IpqcOrderItemID（标识要更新的实体）
   */
  ipqcOrderItemId: string;

}


/**
 * IpqcOrderItem 状态更新 DTO
 * 对应前端 IpqcOrderItemStatus
 * @description 对应后端 TaktIpqcOrderItemStatusDto
 */
export interface IpqcOrderItemStatus {
  /**
   * IpqcOrderItemID
   */
  ipqcOrderItemId: string;

  /**
   * 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）
   */
  judgeStatus: number;

}


/**
 * IpqcOrderItem 作废/撤销作废 DTO
 * 对应前端 IpqcOrderItemObsolete
 * @description 对应后端 TaktIpqcOrderItemObsoleteDto
 */
export interface IpqcOrderItemObsolete {
  /**
   * IpqcOrderItemID
   */
  ipqcOrderItemId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * IpqcOrderItem 导入模板行 DTO
 * 对应前端 IpqcOrderItemTemplate
 * @description 对应后端 TaktIpqcOrderItemTemplateDto
 */
export interface IpqcOrderItemTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
   */
  plantCode?: string;

  /**
   * IPQC检验单 ID（关联 TaktIpqcOrder.Id，选项 TaktIpqcOrders/options）
   */
  ipqcOrderId?: string;

  /**
   * IPQC检验单编码（冗余字段，便于查询）
   */
  ipqcOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 物料编码（选项 TaktGeneralMaterials/options，DictValue=MaterialCode）
   */
  materialCode?: string;

  /**
   * 物料描述
   */
  materialDescription?: string;

  /**
   * 批次号
   */
  batchCode?: string;

  /**
   * 生产数量
   */
  productionQuantity?: number;

  /**
   * 检验标准编码（选项 TaktInspectionStandards/options，DictValue=StandardCode）
   */
  standardCode?: string;

  /**
   * 抽样方案编码（选项 TaktSamplingSchemes/options，DictValue=SamplingSchemeCode）
   */
  samplingSchemeCode?: string;

  /**
   * 检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）
   */
  inspectionMethod?: number;

  /**
   * 抽样数量
   */
  sampleQuantity?: number;

  /**
   * 合格数量
   */
  qualifiedQuantity?: number;

  /**
   * 不合格数量
   */
  unqualifiedQuantity?: number;

  /**
   * 验退数量
   */
  inspectionReturnQuantity?: number;

  /**
   * 抽检序列号
   */
  sampleSerialCode?: string;

  /**
   * 检验说明
   */
  inspectionDescription?: string;

  /**
   * 检验员（人员代码）
   */
  inspectorBy?: string;

  /**
   * 检验日期
   */
  inspectionDate?: string;

  /**
   * 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）
   */
  judgeStatus?: number;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

  /**
   * 不良处理记录列表（主子表关系）（子表，级联保存）
   */
  defectHandlings?: IpqcDefectHandlingCreate[];

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
 * IpqcOrderItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 IpqcOrderItemImport
 * @description 对应后端 TaktIpqcOrderItemImportDto
 */
export interface IpqcOrderItemImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
   */
  plantCode?: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * IPQC检验单 ID（关联 TaktIpqcOrder.Id，选项 TaktIpqcOrders/options）
   */
  ipqcOrderId?: string;

  /**
   * IPQC检验单编码（冗余字段，便于查询）
   */
  ipqcOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 物料编码（选项 TaktGeneralMaterials/options，DictValue=MaterialCode）
   */
  materialCode?: string;

  /**
   * 物料描述
   */
  materialDescription?: string;

  /**
   * 批次号
   */
  batchCode?: string;

  /**
   * 生产数量
   */
  productionQuantity?: number;

  /**
   * 检验标准编码（选项 TaktInspectionStandards/options，DictValue=StandardCode）
   */
  standardCode?: string;

  /**
   * 抽样方案编码（选项 TaktSamplingSchemes/options，DictValue=SamplingSchemeCode）
   */
  samplingSchemeCode?: string;

  /**
   * 检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）
   */
  inspectionMethod?: number;

  /**
   * 抽样数量
   */
  sampleQuantity?: number;

  /**
   * 合格数量
   */
  qualifiedQuantity?: number;

  /**
   * 不合格数量
   */
  unqualifiedQuantity?: number;

  /**
   * 验退数量
   */
  inspectionReturnQuantity?: number;

  /**
   * 抽检序列号
   */
  sampleSerialCode?: string;

  /**
   * 检验说明
   */
  inspectionDescription?: string;

  /**
   * 检验员（人员代码）
   */
  inspectorBy?: string;

  /**
   * 检验日期
   */
  inspectionDate?: string;

  /**
   * 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）
   */
  judgeStatus?: number;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

  /**
   * 不良处理记录列表（主子表关系）（子表，级联保存）
   */
  defectHandlings?: IpqcDefectHandlingCreate[];

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
 * IpqcOrderItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 IpqcOrderItemExport
 * @description 对应后端 TaktIpqcOrderItemExportDto
 */
export interface IpqcOrderItemExport {
  /**
   * IpqcOrderItemID
   */
  ipqcOrderItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * IPQC检验单 ID（关联 TaktIpqcOrder.Id，选项 TaktIpqcOrders/options）
   */
  ipqcOrderId: string;

  /**
   * IPQC检验单编码（冗余字段，便于查询）
   */
  ipqcOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 物料编码（选项 TaktGeneralMaterials/options，DictValue=MaterialCode）
   */
  materialCode: string;

  /**
   * 物料描述
   */
  materialDescription: string;

  /**
   * 批次号
   */
  batchCode?: string;

  /**
   * 生产数量
   */
  productionQuantity: number;

  /**
   * 检验标准编码（选项 TaktInspectionStandards/options，DictValue=StandardCode）
   */
  standardCode: string;

  /**
   * 抽样方案编码（选项 TaktSamplingSchemes/options，DictValue=SamplingSchemeCode）
   */
  samplingSchemeCode: string;

  /**
   * 检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）
   */
  inspectionMethod: number;

  /**
   * 抽样数量
   */
  sampleQuantity: number;

  /**
   * 合格数量
   */
  qualifiedQuantity: number;

  /**
   * 不合格数量
   */
  unqualifiedQuantity: number;

  /**
   * 验退数量
   */
  inspectionReturnQuantity: number;

  /**
   * 抽检序列号
   */
  sampleSerialCode?: string;

  /**
   * 检验说明
   */
  inspectionDescription?: string;

  /**
   * 检验员（人员代码）
   */
  inspectorBy: string;

  /**
   * 检验日期
   */
  inspectionDate: string;

  /**
   * 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）
   */
  judgeStatus: number;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

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

