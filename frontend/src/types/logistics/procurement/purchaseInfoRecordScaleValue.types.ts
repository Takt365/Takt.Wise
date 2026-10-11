// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：purchaseInfoRecordScaleValue.types.ts
// 创建时间：2026-09-21
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
 * Takt采购阶梯价格实体（主子表：TaktPurchaseInfoRecordItem → ScaleValues；与数量等级仅差 ScaleValue↔ScaleQuantity）
 * 对应前端 TaktPurchaseInfoRecordScaleValueDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PurchaseInfoRecordScaleValue
 * @description 对应后端 TaktPurchaseInfoRecordScaleValueDto
 */
export interface PurchaseInfoRecordScaleValue extends CompanyDtoBase {
  /**
   * PurchaseInfoRecordScaleValueID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  purchaseInfoRecordScaleValueId: string;

  /**
   * 采购信息记录行 ID（主子表关系；选项 TaktPurchaseInfoRecordItems/options，DictValue=Id）
   */
  purchaseInfoRecordItemId: string;

  /**
   * 采购信息记录行 名称（填充字段）
   */
  purchaseInfoRecordItemName?: string;

  /**
   * 定价记录号（冗余：与明细 PurchaseInfoRecordCode 一致）
   */
  purchaseInfoRecordCode: string;

  /**
   * 定价序号（冗余：与明细 PurchaseInfoRecordSeq 一致，固定步长=10）
   */
  purchaseInfoRecordSeq: number;

  /**
   * 等级序号（回填：同一明细内阶梯序号，固定步长=10）
   */
  purchaseScaleSeq: number;

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
 * PurchaseInfoRecordScaleValue 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PurchaseInfoRecordScaleValueQuery
 * @description 对应后端 TaktPurchaseInfoRecordScaleValueQueryDto
 */
export interface PurchaseInfoRecordScaleValueQuery extends TaktPagedQuery {
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
   * 采购信息记录行 ID（主子表关系；选项 TaktPurchaseInfoRecordItems/options，DictValue=Id）
   */
  purchaseInfoRecordItemId?: string;

  /**
   * 定价记录号（冗余：与明细 PurchaseInfoRecordCode 一致）
   */
  purchaseInfoRecordCode?: string;

  /**
   * 定价序号（冗余：与明细 PurchaseInfoRecordSeq 一致，固定步长=10）
   */
  purchaseInfoRecordSeq?: number;

  /**
   * 等级序号（回填：同一明细内阶梯序号，固定步长=10）
   */
  purchaseScaleSeq?: number;

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
 * 创建PurchaseInfoRecordScaleValue DTO
 * 对应前端 PurchaseInfoRecordScaleValueCreate
 * @description 对应后端 TaktPurchaseInfoRecordScaleValueCreateDto
 */
export interface PurchaseInfoRecordScaleValueCreate {
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
   * 采购信息记录行 ID（主子表关系；选项 TaktPurchaseInfoRecordItems/options，DictValue=Id）
   */
  purchaseInfoRecordItemId: string;

  /**
   * 定价记录号（冗余：与明细 PurchaseInfoRecordCode 一致）
   */
  purchaseInfoRecordCode: string;

  /**
   * 定价序号（冗余：与明细 PurchaseInfoRecordSeq 一致，固定步长=10）
   */
  purchaseInfoRecordSeq: number;

  /**
   * 等级序号（回填：同一明细内阶梯序号，固定步长=10）
   */
  purchaseScaleSeq: number;

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
 * 更新PurchaseInfoRecordScaleValue DTO
 * 继承 TaktPurchaseInfoRecordScaleValueCreateDto，添加 PurchaseInfoRecordScaleValueId 字段
 * 对应前端 PurchaseInfoRecordScaleValueUpdate
 * @description 对应后端 TaktPurchaseInfoRecordScaleValueUpdateDto
 */
export interface PurchaseInfoRecordScaleValueUpdate extends PurchaseInfoRecordScaleValueCreate {
  /**
   * PurchaseInfoRecordScaleValueID（标识要更新的实体）
   */
  purchaseInfoRecordScaleValueId: string;

}


/**
 * PurchaseInfoRecordScaleValue 作废/撤销作废 DTO
 * 对应前端 PurchaseInfoRecordScaleValueObsolete
 * @description 对应后端 TaktPurchaseInfoRecordScaleValueObsoleteDto
 */
export interface PurchaseInfoRecordScaleValueObsolete {
  /**
   * PurchaseInfoRecordScaleValueID
   */
  purchaseInfoRecordScaleValueId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * PurchaseInfoRecordScaleValue 导入模板行 DTO
 * 对应前端 PurchaseInfoRecordScaleValueTemplate
 * @description 对应后端 TaktPurchaseInfoRecordScaleValueTemplateDto
 */
export interface PurchaseInfoRecordScaleValueTemplate {
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
   * 采购信息记录行 ID（主子表关系；选项 TaktPurchaseInfoRecordItems/options，DictValue=Id）
   */
  purchaseInfoRecordItemId?: string;

  /**
   * 定价记录号（冗余：与明细 PurchaseInfoRecordCode 一致）
   */
  purchaseInfoRecordCode?: string;

  /**
   * 定价序号（冗余：与明细 PurchaseInfoRecordSeq 一致，固定步长=10）
   */
  purchaseInfoRecordSeq?: number;

  /**
   * 等级序号（回填：同一明细内阶梯序号，固定步长=10）
   */
  purchaseScaleSeq?: number;

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
 * PurchaseInfoRecordScaleValue 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PurchaseInfoRecordScaleValueImport
 * @description 对应后端 TaktPurchaseInfoRecordScaleValueImportDto
 */
export interface PurchaseInfoRecordScaleValueImport {
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
   * 采购信息记录行 ID（主子表关系；选项 TaktPurchaseInfoRecordItems/options，DictValue=Id）
   */
  purchaseInfoRecordItemId?: string;

  /**
   * 定价记录号（冗余：与明细 PurchaseInfoRecordCode 一致）
   */
  purchaseInfoRecordCode?: string;

  /**
   * 定价序号（冗余：与明细 PurchaseInfoRecordSeq 一致，固定步长=10）
   */
  purchaseInfoRecordSeq?: number;

  /**
   * 等级序号（回填：同一明细内阶梯序号，固定步长=10）
   */
  purchaseScaleSeq?: number;

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
 * PurchaseInfoRecordScaleValue 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PurchaseInfoRecordScaleValueExport
 * @description 对应后端 TaktPurchaseInfoRecordScaleValueExportDto
 */
export interface PurchaseInfoRecordScaleValueExport {
  /**
   * PurchaseInfoRecordScaleValueID
   */
  purchaseInfoRecordScaleValueId: string;

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
   * 采购信息记录行 ID（主子表关系；选项 TaktPurchaseInfoRecordItems/options，DictValue=Id）
   */
  purchaseInfoRecordItemId: string;

  /**
   * 定价记录号（冗余：与明细 PurchaseInfoRecordCode 一致）
   */
  purchaseInfoRecordCode: string;

  /**
   * 定价序号（冗余：与明细 PurchaseInfoRecordSeq 一致，固定步长=10）
   */
  purchaseInfoRecordSeq: number;

  /**
   * 等级序号（回填：同一明细内阶梯序号，固定步长=10）
   */
  purchaseScaleSeq: number;

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

