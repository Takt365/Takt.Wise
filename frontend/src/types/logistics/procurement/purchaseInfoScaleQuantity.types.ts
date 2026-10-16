// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：purchaseInfoScaleQuantity.types.ts
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
 * Takt采购阶梯数量实体（主子表：TaktPurchaseInfoItem → ScaleQuantities；与价值等级仅差 ScaleQuantity↔ScaleValue）
 * 对应前端 TaktPurchaseInfoScaleQuantityDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PurchaseInfoScaleQuantity
 * @description 对应后端 TaktPurchaseInfoScaleQuantityDto
 */
export interface PurchaseInfoScaleQuantity extends CompanyDtoBase {
  /**
   * PurchaseInfoScaleQuantityID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  purchaseInfoScaleQuantityId: string;

  /**
   * 采购信息行 ID（主子表关系；选项 TaktPurchaseInfoItems/options，DictValue=Id）
   */
  purchaseInfoItemId: string;

  /**
   * 采购信息行 名称（填充字段）
   */
  purchaseInfoItemName?: string;

  /**
   * 定价记录号（冗余：与明细 PurchaseInfoCode 一致）
   */
  purchaseInfoCode: string;

  /**
   * 定价序号（冗余：与明细 PurchaseInfoSeq 一致，固定步长=10）
   */
  purchaseInfoSeq: number;

  /**
   * 等级序号（回填：同一明细内阶梯序号，固定步长=10）
   */
  purchaseScaleSeq: number;

  /**
   * 等级数量（数量等级门槛；对应价值等级表的 ScaleValue）
   */
  scaleQuantity: number;

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
 * PurchaseInfoScaleQuantity 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PurchaseInfoScaleQuantityQuery
 * @description 对应后端 TaktPurchaseInfoScaleQuantityQueryDto
 */
export interface PurchaseInfoScaleQuantityQuery extends TaktPagedQuery {
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
   * 采购信息行 ID（主子表关系；选项 TaktPurchaseInfoItems/options，DictValue=Id）
   */
  purchaseInfoItemId?: string;

  /**
   * 定价记录号（冗余：与明细 PurchaseInfoCode 一致）
   */
  purchaseInfoCode?: string;

  /**
   * 定价序号（冗余：与明细 PurchaseInfoSeq 一致，固定步长=10）
   */
  purchaseInfoSeq?: number;

  /**
   * 等级序号（回填：同一明细内阶梯序号，固定步长=10）
   */
  purchaseScaleSeq?: number;

  /**
   * 等级数量（数量等级门槛；对应价值等级表的 ScaleValue）
   */
  scaleQuantity?: number;

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
 * 创建PurchaseInfoScaleQuantity DTO
 * 对应前端 PurchaseInfoScaleQuantityCreate
 * @description 对应后端 TaktPurchaseInfoScaleQuantityCreateDto
 */
export interface PurchaseInfoScaleQuantityCreate {
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
   * 采购信息行 ID（主子表关系；选项 TaktPurchaseInfoItems/options，DictValue=Id）
   */
  purchaseInfoItemId: string;

  /**
   * 定价记录号（冗余：与明细 PurchaseInfoCode 一致）
   */
  purchaseInfoCode: string;

  /**
   * 定价序号（冗余：与明细 PurchaseInfoSeq 一致，固定步长=10）
   */
  purchaseInfoSeq: number;

  /**
   * 等级序号（回填：同一明细内阶梯序号，固定步长=10）
   */
  purchaseScaleSeq: number;

  /**
   * 等级数量（数量等级门槛；对应价值等级表的 ScaleValue）
   */
  scaleQuantity: number;

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
 * 更新PurchaseInfoScaleQuantity DTO
 * 继承 TaktPurchaseInfoScaleQuantityCreateDto，添加 PurchaseInfoScaleQuantityId 字段
 * 对应前端 PurchaseInfoScaleQuantityUpdate
 * @description 对应后端 TaktPurchaseInfoScaleQuantityUpdateDto
 */
export interface PurchaseInfoScaleQuantityUpdate extends PurchaseInfoScaleQuantityCreate {
  /**
   * PurchaseInfoScaleQuantityID（标识要更新的实体）
   */
  purchaseInfoScaleQuantityId: string;

}


/**
 * PurchaseInfoScaleQuantity 作废/撤销作废 DTO
 * 对应前端 PurchaseInfoScaleQuantityObsolete
 * @description 对应后端 TaktPurchaseInfoScaleQuantityObsoleteDto
 */
export interface PurchaseInfoScaleQuantityObsolete {
  /**
   * PurchaseInfoScaleQuantityID
   */
  purchaseInfoScaleQuantityId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * PurchaseInfoScaleQuantity 导入模板行 DTO
 * 对应前端 PurchaseInfoScaleQuantityTemplate
 * @description 对应后端 TaktPurchaseInfoScaleQuantityTemplateDto
 */
export interface PurchaseInfoScaleQuantityTemplate {
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
   * 采购信息行 ID（主子表关系；选项 TaktPurchaseInfoItems/options，DictValue=Id）
   */
  purchaseInfoItemId?: string;

  /**
   * 定价记录号（冗余：与明细 PurchaseInfoCode 一致）
   */
  purchaseInfoCode?: string;

  /**
   * 定价序号（冗余：与明细 PurchaseInfoSeq 一致，固定步长=10）
   */
  purchaseInfoSeq?: number;

  /**
   * 等级序号（回填：同一明细内阶梯序号，固定步长=10）
   */
  purchaseScaleSeq?: number;

  /**
   * 等级数量（数量等级门槛；对应价值等级表的 ScaleValue）
   */
  scaleQuantity?: number;

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
 * PurchaseInfoScaleQuantity 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PurchaseInfoScaleQuantityImport
 * @description 对应后端 TaktPurchaseInfoScaleQuantityImportDto
 */
export interface PurchaseInfoScaleQuantityImport {
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
   * 采购信息行 ID（主子表关系；选项 TaktPurchaseInfoItems/options，DictValue=Id）
   */
  purchaseInfoItemId?: string;

  /**
   * 定价记录号（冗余：与明细 PurchaseInfoCode 一致）
   */
  purchaseInfoCode?: string;

  /**
   * 定价序号（冗余：与明细 PurchaseInfoSeq 一致，固定步长=10）
   */
  purchaseInfoSeq?: number;

  /**
   * 等级序号（回填：同一明细内阶梯序号，固定步长=10）
   */
  purchaseScaleSeq?: number;

  /**
   * 等级数量（数量等级门槛；对应价值等级表的 ScaleValue）
   */
  scaleQuantity?: number;

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
 * PurchaseInfoScaleQuantity 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PurchaseInfoScaleQuantityExport
 * @description 对应后端 TaktPurchaseInfoScaleQuantityExportDto
 */
export interface PurchaseInfoScaleQuantityExport {
  /**
   * PurchaseInfoScaleQuantityID
   */
  purchaseInfoScaleQuantityId: string;

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
   * 采购信息行 ID（主子表关系；选项 TaktPurchaseInfoItems/options，DictValue=Id）
   */
  purchaseInfoItemId: string;

  /**
   * 定价记录号（冗余：与明细 PurchaseInfoCode 一致）
   */
  purchaseInfoCode: string;

  /**
   * 定价序号（冗余：与明细 PurchaseInfoSeq 一致，固定步长=10）
   */
  purchaseInfoSeq: number;

  /**
   * 等级序号（回填：同一明细内阶梯序号，固定步长=10）
   */
  purchaseScaleSeq: number;

  /**
   * 等级数量（数量等级门槛；对应价值等级表的 ScaleValue）
   */
  scaleQuantity: number;

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

