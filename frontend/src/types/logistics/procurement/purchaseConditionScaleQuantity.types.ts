// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：purchaseConditionScaleQuantity.types.ts
// 创建时间：2026-10-30
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
 * Takt采购阶梯数量实体（主子表：TaktPurchaseConditionItem → ScaleQuantities；与价值等级仅差 ScaleQuantity↔ScaleValue）
 * 对应前端 TaktPurchaseConditionScaleQuantityDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PurchaseConditionScaleQuantity
 * @description 对应后端 TaktPurchaseConditionScaleQuantityDto
 */
export interface PurchaseConditionScaleQuantity extends CompanyDtoBase {
  /**
   * PurchaseConditionScaleQuantityID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  purchaseConditionScaleQuantityId: string;

  /**
   * 采购条件行 ID（主子表关系；选项 TaktPurchaseConditionItems/options，DictValue=Id）
   */
  purchaseConditionItemId: string;

  /**
   * 采购条件行 名称（填充字段）
   */
  purchaseConditionItemName?: string;

  /**
   * 定价记录号（冗余：与明细 PurchaseConditionCode 一致）
   */
  purchaseConditionCode: string;

  /**
   * 定价序号（冗余：与明细 PurchaseConditionSeq 一致，固定步长=10）
   */
  purchaseConditionSeq: number;

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
 * PurchaseConditionScaleQuantity 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PurchaseConditionScaleQuantityQuery
 * @description 对应后端 TaktPurchaseConditionScaleQuantityQueryDto
 */
export interface PurchaseConditionScaleQuantityQuery extends TaktPagedQuery {
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
   * 采购条件行 ID（主子表关系；选项 TaktPurchaseConditionItems/options，DictValue=Id）
   */
  purchaseConditionItemId?: string;

  /**
   * 定价记录号（冗余：与明细 PurchaseConditionCode 一致）
   */
  purchaseConditionCode?: string;

  /**
   * 定价序号（冗余：与明细 PurchaseConditionSeq 一致，固定步长=10）
   */
  purchaseConditionSeq?: number;

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
 * 创建PurchaseConditionScaleQuantity DTO
 * 对应前端 PurchaseConditionScaleQuantityCreate
 * @description 对应后端 TaktPurchaseConditionScaleQuantityCreateDto
 */
export interface PurchaseConditionScaleQuantityCreate {
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
   * 采购条件行 ID（主子表关系；选项 TaktPurchaseConditionItems/options，DictValue=Id）
   */
  purchaseConditionItemId: string;

  /**
   * 定价记录号（冗余：与明细 PurchaseConditionCode 一致）
   */
  purchaseConditionCode: string;

  /**
   * 定价序号（冗余：与明细 PurchaseConditionSeq 一致，固定步长=10）
   */
  purchaseConditionSeq: number;

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
 * 更新PurchaseConditionScaleQuantity DTO
 * 继承 TaktPurchaseConditionScaleQuantityCreateDto，添加 PurchaseConditionScaleQuantityId 字段
 * 对应前端 PurchaseConditionScaleQuantityUpdate
 * @description 对应后端 TaktPurchaseConditionScaleQuantityUpdateDto
 */
export interface PurchaseConditionScaleQuantityUpdate extends PurchaseConditionScaleQuantityCreate {
  /**
   * PurchaseConditionScaleQuantityID（标识要更新的实体）
   */
  purchaseConditionScaleQuantityId: string;

}


/**
 * PurchaseConditionScaleQuantity 作废/撤销作废 DTO
 * 对应前端 PurchaseConditionScaleQuantityObsolete
 * @description 对应后端 TaktPurchaseConditionScaleQuantityObsoleteDto
 */
export interface PurchaseConditionScaleQuantityObsolete {
  /**
   * PurchaseConditionScaleQuantityID
   */
  purchaseConditionScaleQuantityId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * PurchaseConditionScaleQuantity 导入模板行 DTO
 * 对应前端 PurchaseConditionScaleQuantityTemplate
 * @description 对应后端 TaktPurchaseConditionScaleQuantityTemplateDto
 */
export interface PurchaseConditionScaleQuantityTemplate {
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
   * 采购条件行 ID（主子表关系；选项 TaktPurchaseConditionItems/options，DictValue=Id）
   */
  purchaseConditionItemId?: string;

  /**
   * 定价记录号（冗余：与明细 PurchaseConditionCode 一致）
   */
  purchaseConditionCode?: string;

  /**
   * 定价序号（冗余：与明细 PurchaseConditionSeq 一致，固定步长=10）
   */
  purchaseConditionSeq?: number;

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
 * PurchaseConditionScaleQuantity 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PurchaseConditionScaleQuantityImport
 * @description 对应后端 TaktPurchaseConditionScaleQuantityImportDto
 */
export interface PurchaseConditionScaleQuantityImport {
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
   * 采购条件行 ID（主子表关系；选项 TaktPurchaseConditionItems/options，DictValue=Id）
   */
  purchaseConditionItemId?: string;

  /**
   * 定价记录号（冗余：与明细 PurchaseConditionCode 一致）
   */
  purchaseConditionCode?: string;

  /**
   * 定价序号（冗余：与明细 PurchaseConditionSeq 一致，固定步长=10）
   */
  purchaseConditionSeq?: number;

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
 * PurchaseConditionScaleQuantity 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PurchaseConditionScaleQuantityExport
 * @description 对应后端 TaktPurchaseConditionScaleQuantityExportDto
 */
export interface PurchaseConditionScaleQuantityExport {
  /**
   * PurchaseConditionScaleQuantityID
   */
  purchaseConditionScaleQuantityId: string;

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
   * 采购条件行 ID（主子表关系；选项 TaktPurchaseConditionItems/options，DictValue=Id）
   */
  purchaseConditionItemId: string;

  /**
   * 定价记录号（冗余：与明细 PurchaseConditionCode 一致）
   */
  purchaseConditionCode: string;

  /**
   * 定价序号（冗余：与明细 PurchaseConditionSeq 一致，固定步长=10）
   */
  purchaseConditionSeq: number;

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

