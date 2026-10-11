// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/foundation
// 文件名称：numbering.types.ts
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：foundation 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 编码规则实体 定义系统中各类业务单据的编码生成规则，如：订单号、合同号、发票号等 支持灵活的前缀、日期格式、流水号组合 编码顺序：单据类型-公司-部门-前缀-日期-流水号 示例：order-1000-DEPT01-SO-20250120-000001
 * 对应前端 TaktNumberingDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Numbering
 * @description 对应后端 TaktNumberingDto
 */
export interface Numbering extends CompanyDtoBase {
  /**
   * NumberingID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  numberingId: string;

  /**
   * 规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）
   */
  ruleCode: string;

  /**
   * 规则名称（如：销售订单号、采购订单号）
   */
  ruleName: string;

  /**
   * 单据类型（关联 TaktMenu.MenuName，选项 TaktMenus/tree-options?valueBy=name）
   */
  documentType: string;

  /**
   * 部门编码（关联 TaktDept.IsoCode，选项 TaktDepts/iso-tree-options）
   */
  deptCode: string;

  /**
   * 前缀编码（如：PUR、SORD、ANN）
   */
  prefixCode?: string;

  /**
   * 日期格式（字典 sys_numbering_date_format；none/空=不使用日期；yyyy、yyyyMM、yyyyMMdd、yyyyMMddHH；须与 reset_period 粒度匹配）
   */
  dateFormat?: string;

  /**
   * 流水位数（3=001, 4=0001, 5=00001, 6=000001）
   */
  sequenceLength: number;

  /**
   * 流水步长（每次递增的数值，默认1）
   */
  sequenceStep: number;

  /**
   * 后缀编码（可选，最多 4 位）
   */
  suffixCode?: string;

  /**
   * 重置周期（字典 sys_reset_period；DictValue=None|Annually|Monthly|Daily；须与 date_format 粒度匹配）
   */
  resetPeriod: string;

  /**
   * 当前流水（用于记录下一个流水号值）
   */
  currentSequence: number;

  /**
   * 起始编码（新增时必填；完整业务编码样例，末段为当前流水号） 如：SO-20250120-000001；生成编码后会更新为最近一次产出编码
   */
  exampleCode: string;

  /**
   * 分隔符（空=段直接拼接；-=连字符分隔，默认 -）
   */
  separator?: string;

  /**
   * 内置（字典 sys_yes_no；0=否 1=是）
   */
  isBuiltIn: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  numberingStatus: number;

  /**
   * 描述说明；可选配置编码段顺序，格式：segments:CompanyCode,DeptCode,PrefixCode,DateSequence（段名为实体属性名）
   */
  numberingDescription?: string;

}


/**
 * Numbering 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 NumberingQuery
 * @description 对应后端 TaktNumberingQueryDto
 */
export interface NumberingQuery extends TaktPagedQuery {
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
   * 规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）
   */
  ruleCode?: string;

  /**
   * 规则名称（如：销售订单号、采购订单号）
   */
  ruleName?: string;

  /**
   * 单据类型（关联 TaktMenu.MenuName，选项 TaktMenus/tree-options?valueBy=name）
   */
  documentType?: string;

  /**
   * 部门编码（关联 TaktDept.IsoCode，选项 TaktDepts/iso-tree-options）
   */
  deptCode?: string;

  /**
   * 前缀编码（如：PUR、SORD、ANN）
   */
  prefixCode?: string;

  /**
   * 日期格式（字典 sys_numbering_date_format；none/空=不使用日期；yyyy、yyyyMM、yyyyMMdd、yyyyMMddHH；须与 reset_period 粒度匹配）
   */
  dateFormat?: string;

  /**
   * 流水位数（3=001, 4=0001, 5=00001, 6=000001）
   */
  sequenceLength?: number;

  /**
   * 流水步长（每次递增的数值，默认1）
   */
  sequenceStep?: number;

  /**
   * 后缀编码（可选，最多 4 位）
   */
  suffixCode?: string;

  /**
   * 重置周期（字典 sys_reset_period；DictValue=None|Annually|Monthly|Daily；须与 date_format 粒度匹配）
   */
  resetPeriod?: string;

  /**
   * 当前流水（用于记录下一个流水号值）
   */
  currentSequence?: number;

  /**
   * 起始编码（新增时必填；完整业务编码样例，末段为当前流水号） 如：SO-20250120-000001；生成编码后会更新为最近一次产出编码
   */
  exampleCode?: string;

  /**
   * 分隔符（空=段直接拼接；-=连字符分隔，默认 -）
   */
  separator?: string;

  /**
   * 内置（字典 sys_yes_no；0=否 1=是）
   */
  isBuiltIn?: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  numberingStatus?: number;

  /**
   * 描述说明；可选配置编码段顺序，格式：segments:CompanyCode,DeptCode,PrefixCode,DateSequence（段名为实体属性名）
   */
  numberingDescription?: string;

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
 * 创建Numbering DTO
 * 对应前端 NumberingCreate
 * @description 对应后端 TaktNumberingCreateDto
 */
export interface NumberingCreate {
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
   * 规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）
   */
  ruleCode: string;

  /**
   * 规则名称（如：销售订单号、采购订单号）
   */
  ruleName: string;

  /**
   * 单据类型（关联 TaktMenu.MenuName，选项 TaktMenus/tree-options?valueBy=name）
   */
  documentType: string;

  /**
   * 部门编码（关联 TaktDept.IsoCode，选项 TaktDepts/iso-tree-options）
   */
  deptCode: string;

  /**
   * 前缀编码（如：PUR、SORD、ANN）
   */
  prefixCode?: string;

  /**
   * 日期格式（字典 sys_numbering_date_format；none/空=不使用日期；yyyy、yyyyMM、yyyyMMdd、yyyyMMddHH；须与 reset_period 粒度匹配）
   */
  dateFormat?: string;

  /**
   * 流水位数（3=001, 4=0001, 5=00001, 6=000001）
   */
  sequenceLength: number;

  /**
   * 流水步长（每次递增的数值，默认1）
   */
  sequenceStep: number;

  /**
   * 后缀编码（可选，最多 4 位）
   */
  suffixCode?: string;

  /**
   * 重置周期（字典 sys_reset_period；DictValue=None|Annually|Monthly|Daily；须与 date_format 粒度匹配）
   */
  resetPeriod: string;

  /**
   * 当前流水（用于记录下一个流水号值）
   */
  currentSequence: number;

  /**
   * 起始编码（新增时必填；完整业务编码样例，末段为当前流水号） 如：SO-20250120-000001；生成编码后会更新为最近一次产出编码
   */
  exampleCode: string;

  /**
   * 分隔符（空=段直接拼接；-=连字符分隔，默认 -）
   */
  separator?: string;

  /**
   * 内置（字典 sys_yes_no；0=否 1=是）
   */
  isBuiltIn: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  numberingStatus: number;

  /**
   * 描述说明；可选配置编码段顺序，格式：segments:CompanyCode,DeptCode,PrefixCode,DateSequence（段名为实体属性名）
   */
  numberingDescription?: string;

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
 * 更新Numbering DTO
 * 继承 TaktNumberingCreateDto，添加 NumberingId 字段
 * 对应前端 NumberingUpdate
 * @description 对应后端 TaktNumberingUpdateDto
 */
export interface NumberingUpdate extends NumberingCreate {
  /**
   * NumberingID（标识要更新的实体）
   */
  numberingId: string;

}


/**
 * Numbering 状态更新 DTO
 * 对应前端 NumberingStatus
 * @description 对应后端 TaktNumberingStatusDto
 */
export interface NumberingStatus {
  /**
   * NumberingID
   */
  numberingId: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  numberingStatus: number;

}


/**
 * Numbering 导入模板行 DTO
 * 对应前端 NumberingTemplate
 * @description 对应后端 TaktNumberingTemplateDto
 */
export interface NumberingTemplate {
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
   * 规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）
   */
  ruleCode?: string;

  /**
   * 规则名称（如：销售订单号、采购订单号）
   */
  ruleName?: string;

  /**
   * 单据类型（关联 TaktMenu.MenuName，选项 TaktMenus/tree-options?valueBy=name）
   */
  documentType?: string;

  /**
   * 部门编码（关联 TaktDept.IsoCode，选项 TaktDepts/iso-tree-options）
   */
  deptCode?: string;

  /**
   * 前缀编码（如：PUR、SORD、ANN）
   */
  prefixCode?: string;

  /**
   * 日期格式（字典 sys_numbering_date_format；none/空=不使用日期；yyyy、yyyyMM、yyyyMMdd、yyyyMMddHH；须与 reset_period 粒度匹配）
   */
  dateFormat?: string;

  /**
   * 流水位数（3=001, 4=0001, 5=00001, 6=000001）
   */
  sequenceLength?: number;

  /**
   * 流水步长（每次递增的数值，默认1）
   */
  sequenceStep?: number;

  /**
   * 后缀编码（可选，最多 4 位）
   */
  suffixCode?: string;

  /**
   * 重置周期（字典 sys_reset_period；DictValue=None|Annually|Monthly|Daily；须与 date_format 粒度匹配）
   */
  resetPeriod?: string;

  /**
   * 当前流水（用于记录下一个流水号值）
   */
  currentSequence?: number;

  /**
   * 起始编码（新增时必填；完整业务编码样例，末段为当前流水号） 如：SO-20250120-000001；生成编码后会更新为最近一次产出编码
   */
  exampleCode?: string;

  /**
   * 分隔符（空=段直接拼接；-=连字符分隔，默认 -）
   */
  separator?: string;

  /**
   * 内置（字典 sys_yes_no；0=否 1=是）
   */
  isBuiltIn?: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  numberingStatus?: number;

  /**
   * 描述说明；可选配置编码段顺序，格式：segments:CompanyCode,DeptCode,PrefixCode,DateSequence（段名为实体属性名）
   */
  numberingDescription?: string;

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
 * Numbering 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 NumberingImport
 * @description 对应后端 TaktNumberingImportDto
 */
export interface NumberingImport {
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
   * 规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）
   */
  ruleCode?: string;

  /**
   * 规则名称（如：销售订单号、采购订单号）
   */
  ruleName?: string;

  /**
   * 单据类型（关联 TaktMenu.MenuName，选项 TaktMenus/tree-options?valueBy=name）
   */
  documentType?: string;

  /**
   * 部门编码（关联 TaktDept.IsoCode，选项 TaktDepts/iso-tree-options）
   */
  deptCode?: string;

  /**
   * 前缀编码（如：PUR、SORD、ANN）
   */
  prefixCode?: string;

  /**
   * 日期格式（字典 sys_numbering_date_format；none/空=不使用日期；yyyy、yyyyMM、yyyyMMdd、yyyyMMddHH；须与 reset_period 粒度匹配）
   */
  dateFormat?: string;

  /**
   * 流水位数（3=001, 4=0001, 5=00001, 6=000001）
   */
  sequenceLength?: number;

  /**
   * 流水步长（每次递增的数值，默认1）
   */
  sequenceStep?: number;

  /**
   * 后缀编码（可选，最多 4 位）
   */
  suffixCode?: string;

  /**
   * 重置周期（字典 sys_reset_period；DictValue=None|Annually|Monthly|Daily；须与 date_format 粒度匹配）
   */
  resetPeriod?: string;

  /**
   * 当前流水（用于记录下一个流水号值）
   */
  currentSequence?: number;

  /**
   * 起始编码（新增时必填；完整业务编码样例，末段为当前流水号） 如：SO-20250120-000001；生成编码后会更新为最近一次产出编码
   */
  exampleCode?: string;

  /**
   * 分隔符（空=段直接拼接；-=连字符分隔，默认 -）
   */
  separator?: string;

  /**
   * 内置（字典 sys_yes_no；0=否 1=是）
   */
  isBuiltIn?: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  numberingStatus?: number;

  /**
   * 描述说明；可选配置编码段顺序，格式：segments:CompanyCode,DeptCode,PrefixCode,DateSequence（段名为实体属性名）
   */
  numberingDescription?: string;

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
 * Numbering 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 NumberingExport
 * @description 对应后端 TaktNumberingExportDto
 */
export interface NumberingExport {
  /**
   * NumberingID
   */
  numberingId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）
   */
  ruleCode: string;

  /**
   * 规则名称（如：销售订单号、采购订单号）
   */
  ruleName: string;

  /**
   * 单据类型（关联 TaktMenu.MenuName，选项 TaktMenus/tree-options?valueBy=name）
   */
  documentType: string;

  /**
   * 部门编码（关联 TaktDept.IsoCode，选项 TaktDepts/iso-tree-options）
   */
  deptCode: string;

  /**
   * 前缀编码（如：PUR、SORD、ANN）
   */
  prefixCode?: string;

  /**
   * 日期格式（字典 sys_numbering_date_format；none/空=不使用日期；yyyy、yyyyMM、yyyyMMdd、yyyyMMddHH；须与 reset_period 粒度匹配）
   */
  dateFormat?: string;

  /**
   * 流水位数（3=001, 4=0001, 5=00001, 6=000001）
   */
  sequenceLength: number;

  /**
   * 流水步长（每次递增的数值，默认1）
   */
  sequenceStep: number;

  /**
   * 后缀编码（可选，最多 4 位）
   */
  suffixCode?: string;

  /**
   * 重置周期（字典 sys_reset_period；DictValue=None|Annually|Monthly|Daily；须与 date_format 粒度匹配）
   */
  resetPeriod: string;

  /**
   * 当前流水（用于记录下一个流水号值）
   */
  currentSequence: number;

  /**
   * 起始编码（新增时必填；完整业务编码样例，末段为当前流水号） 如：SO-20250120-000001；生成编码后会更新为最近一次产出编码
   */
  exampleCode: string;

  /**
   * 分隔符（空=段直接拼接；-=连字符分隔，默认 -）
   */
  separator?: string;

  /**
   * 内置（字典 sys_yes_no；0=否 1=是）
   */
  isBuiltIn: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  numberingStatus: number;

  /**
   * 描述说明；可选配置编码段顺序，格式：segments:CompanyCode,DeptCode,PrefixCode,DateSequence（段名为实体属性名）
   */
  numberingDescription?: string;

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

