// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：conditionRecordScaleValue.types.ts
// 创建时间：2026-09-21
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/sales 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * Takt销售阶梯价格实体（主子表：TaktSalesConditionRecordItem → ScaleValues；与数量等级仅差 ScaleValue↔ScaleQuantity）
 * 对应前端 TaktSalesConditionRecordScaleValueDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SalesConditionRecordScaleValue
 * @description 对应后端 TaktSalesConditionRecordScaleValueDto
 */
export interface SalesConditionRecordScaleValue extends CompanyDtoBase {
  /**
   * SalesConditionRecordScaleValueID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  salesConditionRecordScaleValueId: string;

  /**
   * 销售条件记录行 ID（主子表关系；选项 TaktSalesConditionRecordItems/options，DictValue=Id）
   */
  salesConditionRecordItemId: string;

  /**
   * 销售条件记录行 名称（填充字段）
   */
  salesConditionRecordItemName?: string;

  /**
   * 定价记录号（冗余：与明细 SalesConditionRecordCode 一致）
   */
  salesConditionRecordCode: string;

  /**
   * 定价序号（冗余：与明细 SalesConditionRecordSeq 一致，固定步长=10）
   */
  salesConditionRecordSeq: number;

  /**
   * 等级序号（回填：同一明细内阶梯序号，固定步长=10）
   */
  salesScaleSeq: number;

  /**
   * 等级值（价值等级门槛；对应数量等级表的 ScaleQuantity）
   */
  scaleValue: number;

  /**
   * 价格
   */
  price: number;

  /**
   * 未税价格（冗余；可由 Price 与税码推算后回写）
   */
  untaxedPrice: number;

  /**
   * 含税价格（冗余；可由 Price 与税码推算后回写）
   */
  taxIncludedPrice: number;

  /**
   * 税费（冗余；含税−未税，打印用）
   */
  taxAmount: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * SalesConditionRecordScaleValue 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SalesConditionRecordScaleValueQuery
 * @description 对应后端 TaktSalesConditionRecordScaleValueQueryDto
 */
export interface SalesConditionRecordScaleValueQuery extends TaktPagedQuery {
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
   * 销售条件记录行 ID（主子表关系；选项 TaktSalesConditionRecordItems/options，DictValue=Id）
   */
  salesConditionRecordItemId?: string;

  /**
   * 定价记录号（冗余：与明细 SalesConditionRecordCode 一致）
   */
  salesConditionRecordCode?: string;

  /**
   * 定价序号（冗余：与明细 SalesConditionRecordSeq 一致，固定步长=10）
   */
  salesConditionRecordSeq?: number;

  /**
   * 等级序号（回填：同一明细内阶梯序号，固定步长=10）
   */
  salesScaleSeq?: number;

  /**
   * 等级值（价值等级门槛；对应数量等级表的 ScaleQuantity）
   */
  scaleValue?: number;

  /**
   * 价格
   */
  price?: number;

  /**
   * 未税价格（冗余；可由 Price 与税码推算后回写）
   */
  untaxedPrice?: number;

  /**
   * 含税价格（冗余；可由 Price 与税码推算后回写）
   */
  taxIncludedPrice?: number;

  /**
   * 税费（冗余；含税−未税，打印用）
   */
  taxAmount?: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
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
 * 创建SalesConditionRecordScaleValue DTO
 * 对应前端 SalesConditionRecordScaleValueCreate
 * @description 对应后端 TaktSalesConditionRecordScaleValueCreateDto
 */
export interface SalesConditionRecordScaleValueCreate {
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
   * 销售条件记录行 ID（主子表关系；选项 TaktSalesConditionRecordItems/options，DictValue=Id）
   */
  salesConditionRecordItemId: string;

  /**
   * 定价记录号（冗余：与明细 SalesConditionRecordCode 一致）
   */
  salesConditionRecordCode: string;

  /**
   * 定价序号（冗余：与明细 SalesConditionRecordSeq 一致，固定步长=10）
   */
  salesConditionRecordSeq: number;

  /**
   * 等级序号（回填：同一明细内阶梯序号，固定步长=10）
   */
  salesScaleSeq: number;

  /**
   * 等级值（价值等级门槛；对应数量等级表的 ScaleQuantity）
   */
  scaleValue: number;

  /**
   * 价格
   */
  price: number;

  /**
   * 未税价格（冗余；可由 Price 与税码推算后回写）
   */
  untaxedPrice: number;

  /**
   * 含税价格（冗余；可由 Price 与税码推算后回写）
   */
  taxIncludedPrice: number;

  /**
   * 税费（冗余；含税−未税，打印用）
   */
  taxAmount: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
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

}


/**
 * 更新SalesConditionRecordScaleValue DTO
 * 继承 TaktSalesConditionRecordScaleValueCreateDto，添加 SalesConditionRecordScaleValueId 字段
 * 对应前端 SalesConditionRecordScaleValueUpdate
 * @description 对应后端 TaktSalesConditionRecordScaleValueUpdateDto
 */
export interface SalesConditionRecordScaleValueUpdate extends SalesConditionRecordScaleValueCreate {
  /**
   * SalesConditionRecordScaleValueID（标识要更新的实体）
   */
  salesConditionRecordScaleValueId: string;

}


/**
 * SalesConditionRecordScaleValue 作废/撤销作废 DTO
 * 对应前端 SalesConditionRecordScaleValueObsolete
 * @description 对应后端 TaktSalesConditionRecordScaleValueObsoleteDto
 */
export interface SalesConditionRecordScaleValueObsolete {
  /**
   * SalesConditionRecordScaleValueID
   */
  salesConditionRecordScaleValueId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * SalesConditionRecordScaleValue 导入模板行 DTO
 * 对应前端 SalesConditionRecordScaleValueTemplate
 * @description 对应后端 TaktSalesConditionRecordScaleValueTemplateDto
 */
export interface SalesConditionRecordScaleValueTemplate {
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
   * 销售条件记录行 ID（主子表关系；选项 TaktSalesConditionRecordItems/options，DictValue=Id）
   */
  salesConditionRecordItemId?: string;

  /**
   * 定价记录号（冗余：与明细 SalesConditionRecordCode 一致）
   */
  salesConditionRecordCode?: string;

  /**
   * 定价序号（冗余：与明细 SalesConditionRecordSeq 一致，固定步长=10）
   */
  salesConditionRecordSeq?: number;

  /**
   * 等级序号（回填：同一明细内阶梯序号，固定步长=10）
   */
  salesScaleSeq?: number;

  /**
   * 等级值（价值等级门槛；对应数量等级表的 ScaleQuantity）
   */
  scaleValue?: number;

  /**
   * 价格
   */
  price?: number;

  /**
   * 未税价格（冗余；可由 Price 与税码推算后回写）
   */
  untaxedPrice?: number;

  /**
   * 含税价格（冗余；可由 Price 与税码推算后回写）
   */
  taxIncludedPrice?: number;

  /**
   * 税费（冗余；含税−未税，打印用）
   */
  taxAmount?: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * SalesConditionRecordScaleValue 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SalesConditionRecordScaleValueImport
 * @description 对应后端 TaktSalesConditionRecordScaleValueImportDto
 */
export interface SalesConditionRecordScaleValueImport {
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
   * 销售条件记录行 ID（主子表关系；选项 TaktSalesConditionRecordItems/options，DictValue=Id）
   */
  salesConditionRecordItemId?: string;

  /**
   * 定价记录号（冗余：与明细 SalesConditionRecordCode 一致）
   */
  salesConditionRecordCode?: string;

  /**
   * 定价序号（冗余：与明细 SalesConditionRecordSeq 一致，固定步长=10）
   */
  salesConditionRecordSeq?: number;

  /**
   * 等级序号（回填：同一明细内阶梯序号，固定步长=10）
   */
  salesScaleSeq?: number;

  /**
   * 等级值（价值等级门槛；对应数量等级表的 ScaleQuantity）
   */
  scaleValue?: number;

  /**
   * 价格
   */
  price?: number;

  /**
   * 未税价格（冗余；可由 Price 与税码推算后回写）
   */
  untaxedPrice?: number;

  /**
   * 含税价格（冗余；可由 Price 与税码推算后回写）
   */
  taxIncludedPrice?: number;

  /**
   * 税费（冗余；含税−未税，打印用）
   */
  taxAmount?: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * SalesConditionRecordScaleValue 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SalesConditionRecordScaleValueExport
 * @description 对应后端 TaktSalesConditionRecordScaleValueExportDto
 */
export interface SalesConditionRecordScaleValueExport {
  /**
   * SalesConditionRecordScaleValueID
   */
  salesConditionRecordScaleValueId: string;

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
   * 销售条件记录行 ID（主子表关系；选项 TaktSalesConditionRecordItems/options，DictValue=Id）
   */
  salesConditionRecordItemId: string;

  /**
   * 定价记录号（冗余：与明细 SalesConditionRecordCode 一致）
   */
  salesConditionRecordCode: string;

  /**
   * 定价序号（冗余：与明细 SalesConditionRecordSeq 一致，固定步长=10）
   */
  salesConditionRecordSeq: number;

  /**
   * 等级序号（回填：同一明细内阶梯序号，固定步长=10）
   */
  salesScaleSeq: number;

  /**
   * 等级值（价值等级门槛；对应数量等级表的 ScaleQuantity）
   */
  scaleValue: number;

  /**
   * 价格
   */
  price: number;

  /**
   * 未税价格（冗余；可由 Price 与税码推算后回写）
   */
  untaxedPrice: number;

  /**
   * 含税价格（冗余；可由 Price 与税码推算后回写）
   */
  taxIncludedPrice: number;

  /**
   * 税费（冗余；含税−未税，打印用）
   */
  taxAmount: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
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

