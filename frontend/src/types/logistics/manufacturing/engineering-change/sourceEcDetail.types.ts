// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：sourceEcDetail.types.ts
// 创建时间：2026-08-26
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/engineering-change 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 设变来源子表
 * 对应前端 TaktSourceEcDetailDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SourceEcDetail
 * @description 对应后端 TaktSourceEcDetailDto
 */
export interface SourceEcDetail extends CompanyDtoBase {
  /**
   * SourceEcDetailID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  sourceEcDetailId: string;

  /**
   * 主ID（选项 TaktSourceEcs/options；DictValue=Id）
   */
  sourceEcId: string;

  /**
   * 主名称（填充字段）
   */
  sourceEcName?: string;

  /**
   * 设变号码（冗余字段，便于查询）
   */
  sourceEcCode: string;

  /**
   * 行号（固定步长=10）
   */
  lineNumber: number;

  /**
   * 根物料编码
   */
  sourceRootMaterialCode: string;

  /**
   * 上阶物料编码
   */
  sourceParentMaterialCode: string;

  /**
   * 旧物料编码
   */
  sourceOldMaterialCode?: string;

  /**
   * 旧物料描述
   */
  sourceOldMaterialDescription?: string;

  /**
   * 旧物料用量
   */
  sourceOldUsageQuantity?: number;

  /**
   * 旧物料安装位置
   */
  sourceOldItemPosition?: string;

  /**
   * 新物料编码
   */
  sourceNewMaterialCode?: string;

  /**
   * 新物料描述
   */
  sourceNewMaterialDescription?: string;

  /**
   * 新物料用量
   */
  sourceNewUsageQuantity?: number;

  /**
   * 新物料安装位置
   */
  sourceNewItemPosition?: string;

  /**
   * BOM番号
   */
  sourceBomCode?: string;

  /**
   * 兼容性（两位码第1位 A=有 B=→ C=← D=无；第2位 1～9=同时变更 *=无同时变更）
   */
  sourceCompatibility?: string;

  /**
   * 第二供应商（字典 logistics_manufacturing_ec_2nd_vendor；1=有，2=优先，3=无）
   */
  source2ndVendor?: string;

  /**
   * 安排指示（字典 logistics_manufacturing_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
   */
  sourceInstruction?: string;

  /**
   * 旧物料处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
   */
  sourceOldPartDisposition?: string;

  /**
   * BOM生效日期
   */
  sourceBomEffectiveDate?: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 设变来源主表 （主表：TaktSourceEc）
   */
  sourceEc?: SourceEc;

}


/**
 * SourceEcDetail 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SourceEcDetailQuery
 * @description 对应后端 TaktSourceEcDetailQueryDto
 */
export interface SourceEcDetailQuery extends TaktPagedQuery {
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
   * 主ID（选项 TaktSourceEcs/options；DictValue=Id）
   */
  sourceEcId?: string;

  /**
   * 设变号码（冗余字段，便于查询）
   */
  sourceEcCode?: string;

  /**
   * 行号（固定步长=10）
   */
  lineNumber?: number;

  /**
   * 根物料编码
   */
  sourceRootMaterialCode?: string;

  /**
   * 上阶物料编码
   */
  sourceParentMaterialCode?: string;

  /**
   * 旧物料编码
   */
  sourceOldMaterialCode?: string;

  /**
   * 旧物料描述
   */
  sourceOldMaterialDescription?: string;

  /**
   * 旧物料用量
   */
  sourceOldUsageQuantity?: number;

  /**
   * 旧物料安装位置
   */
  sourceOldItemPosition?: string;

  /**
   * 新物料编码
   */
  sourceNewMaterialCode?: string;

  /**
   * 新物料描述
   */
  sourceNewMaterialDescription?: string;

  /**
   * 新物料用量
   */
  sourceNewUsageQuantity?: number;

  /**
   * 新物料安装位置
   */
  sourceNewItemPosition?: string;

  /**
   * BOM番号
   */
  sourceBomCode?: string;

  /**
   * 兼容性（两位码第1位 A=有 B=→ C=← D=无；第2位 1～9=同时变更 *=无同时变更）
   */
  sourceCompatibility?: string;

  /**
   * 第二供应商（字典 logistics_manufacturing_ec_2nd_vendor；1=有，2=优先，3=无）
   */
  source2ndVendor?: string;

  /**
   * 安排指示（字典 logistics_manufacturing_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
   */
  sourceInstruction?: string;

  /**
   * 旧物料处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
   */
  sourceOldPartDisposition?: string;

  /**
   * BOM生效日期（范围查询-开始）
   */
  sourceBomEffectiveDateStart?: string;

  /**
   * BOM生效日期（范围查询-结束）
   */
  sourceBomEffectiveDateEnd?: string;

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
 * 创建SourceEcDetail DTO
 * 对应前端 SourceEcDetailCreate
 * @description 对应后端 TaktSourceEcDetailCreateDto
 */
export interface SourceEcDetailCreate {
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
   * 主ID（选项 TaktSourceEcs/options；DictValue=Id）
   */
  sourceEcId: string;

  /**
   * 设变号码（冗余字段，便于查询）
   */
  sourceEcCode: string;

  /**
   * 行号（固定步长=10）
   */
  lineNumber: number;

  /**
   * 根物料编码
   */
  sourceRootMaterialCode: string;

  /**
   * 上阶物料编码
   */
  sourceParentMaterialCode: string;

  /**
   * 旧物料编码
   */
  sourceOldMaterialCode?: string;

  /**
   * 旧物料描述
   */
  sourceOldMaterialDescription?: string;

  /**
   * 旧物料用量
   */
  sourceOldUsageQuantity?: number;

  /**
   * 旧物料安装位置
   */
  sourceOldItemPosition?: string;

  /**
   * 新物料编码
   */
  sourceNewMaterialCode?: string;

  /**
   * 新物料描述
   */
  sourceNewMaterialDescription?: string;

  /**
   * 新物料用量
   */
  sourceNewUsageQuantity?: number;

  /**
   * 新物料安装位置
   */
  sourceNewItemPosition?: string;

  /**
   * BOM番号
   */
  sourceBomCode?: string;

  /**
   * 兼容性（两位码第1位 A=有 B=→ C=← D=无；第2位 1～9=同时变更 *=无同时变更）
   */
  sourceCompatibility?: string;

  /**
   * 第二供应商（字典 logistics_manufacturing_ec_2nd_vendor；1=有，2=优先，3=无）
   */
  source2ndVendor?: string;

  /**
   * 安排指示（字典 logistics_manufacturing_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
   */
  sourceInstruction?: string;

  /**
   * 旧物料处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
   */
  sourceOldPartDisposition?: string;

  /**
   * BOM生效日期
   */
  sourceBomEffectiveDate?: string;

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
 * 更新SourceEcDetail DTO
 * 继承 TaktSourceEcDetailCreateDto，添加 SourceEcDetailId 字段
 * 对应前端 SourceEcDetailUpdate
 * @description 对应后端 TaktSourceEcDetailUpdateDto
 */
export interface SourceEcDetailUpdate extends SourceEcDetailCreate {
  /**
   * SourceEcDetailID（标识要更新的实体）
   */
  sourceEcDetailId: string;

}


/**
 * SourceEcDetail 作废/撤销作废 DTO
 * 对应前端 SourceEcDetailObsolete
 * @description 对应后端 TaktSourceEcDetailObsoleteDto
 */
export interface SourceEcDetailObsolete {
  /**
   * SourceEcDetailID
   */
  sourceEcDetailId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * SourceEcDetail 导入模板行 DTO
 * 对应前端 SourceEcDetailTemplate
 * @description 对应后端 TaktSourceEcDetailTemplateDto
 */
export interface SourceEcDetailTemplate {
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
   * 主ID（选项 TaktSourceEcs/options；DictValue=Id）
   */
  sourceEcId?: string;

  /**
   * 设变号码（冗余字段，便于查询）
   */
  sourceEcCode?: string;

  /**
   * 行号（固定步长=10）
   */
  lineNumber?: number;

  /**
   * 根物料编码
   */
  sourceRootMaterialCode?: string;

  /**
   * 上阶物料编码
   */
  sourceParentMaterialCode?: string;

  /**
   * 旧物料编码
   */
  sourceOldMaterialCode?: string;

  /**
   * 旧物料描述
   */
  sourceOldMaterialDescription?: string;

  /**
   * 旧物料用量
   */
  sourceOldUsageQuantity?: number;

  /**
   * 旧物料安装位置
   */
  sourceOldItemPosition?: string;

  /**
   * 新物料编码
   */
  sourceNewMaterialCode?: string;

  /**
   * 新物料描述
   */
  sourceNewMaterialDescription?: string;

  /**
   * 新物料用量
   */
  sourceNewUsageQuantity?: number;

  /**
   * 新物料安装位置
   */
  sourceNewItemPosition?: string;

  /**
   * BOM番号
   */
  sourceBomCode?: string;

  /**
   * 兼容性（两位码第1位 A=有 B=→ C=← D=无；第2位 1～9=同时变更 *=无同时变更）
   */
  sourceCompatibility?: string;

  /**
   * 第二供应商（字典 logistics_manufacturing_ec_2nd_vendor；1=有，2=优先，3=无）
   */
  source2ndVendor?: string;

  /**
   * 安排指示（字典 logistics_manufacturing_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
   */
  sourceInstruction?: string;

  /**
   * 旧物料处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
   */
  sourceOldPartDisposition?: string;

  /**
   * BOM生效日期
   */
  sourceBomEffectiveDate?: string;

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
 * SourceEcDetail 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SourceEcDetailImport
 * @description 对应后端 TaktSourceEcDetailImportDto
 */
export interface SourceEcDetailImport {
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
   * 主ID（选项 TaktSourceEcs/options；DictValue=Id）
   */
  sourceEcId?: string;

  /**
   * 设变号码（冗余字段，便于查询）
   */
  sourceEcCode?: string;

  /**
   * 行号（固定步长=10）
   */
  lineNumber?: number;

  /**
   * 根物料编码
   */
  sourceRootMaterialCode?: string;

  /**
   * 上阶物料编码
   */
  sourceParentMaterialCode?: string;

  /**
   * 旧物料编码
   */
  sourceOldMaterialCode?: string;

  /**
   * 旧物料描述
   */
  sourceOldMaterialDescription?: string;

  /**
   * 旧物料用量
   */
  sourceOldUsageQuantity?: number;

  /**
   * 旧物料安装位置
   */
  sourceOldItemPosition?: string;

  /**
   * 新物料编码
   */
  sourceNewMaterialCode?: string;

  /**
   * 新物料描述
   */
  sourceNewMaterialDescription?: string;

  /**
   * 新物料用量
   */
  sourceNewUsageQuantity?: number;

  /**
   * 新物料安装位置
   */
  sourceNewItemPosition?: string;

  /**
   * BOM番号
   */
  sourceBomCode?: string;

  /**
   * 兼容性（两位码第1位 A=有 B=→ C=← D=无；第2位 1～9=同时变更 *=无同时变更）
   */
  sourceCompatibility?: string;

  /**
   * 第二供应商（字典 logistics_manufacturing_ec_2nd_vendor；1=有，2=优先，3=无）
   */
  source2ndVendor?: string;

  /**
   * 安排指示（字典 logistics_manufacturing_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
   */
  sourceInstruction?: string;

  /**
   * 旧物料处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
   */
  sourceOldPartDisposition?: string;

  /**
   * BOM生效日期
   */
  sourceBomEffectiveDate?: string;

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
 * SourceEcDetail 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SourceEcDetailExport
 * @description 对应后端 TaktSourceEcDetailExportDto
 */
export interface SourceEcDetailExport {
  /**
   * SourceEcDetailID
   */
  sourceEcDetailId: string;

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
   * 主ID（选项 TaktSourceEcs/options；DictValue=Id）
   */
  sourceEcId: string;

  /**
   * 设变号码（冗余字段，便于查询）
   */
  sourceEcCode: string;

  /**
   * 行号（固定步长=10）
   */
  lineNumber: number;

  /**
   * 根物料编码
   */
  sourceRootMaterialCode: string;

  /**
   * 上阶物料编码
   */
  sourceParentMaterialCode: string;

  /**
   * 旧物料编码
   */
  sourceOldMaterialCode?: string;

  /**
   * 旧物料描述
   */
  sourceOldMaterialDescription?: string;

  /**
   * 旧物料用量
   */
  sourceOldUsageQuantity?: number;

  /**
   * 旧物料安装位置
   */
  sourceOldItemPosition?: string;

  /**
   * 新物料编码
   */
  sourceNewMaterialCode?: string;

  /**
   * 新物料描述
   */
  sourceNewMaterialDescription?: string;

  /**
   * 新物料用量
   */
  sourceNewUsageQuantity?: number;

  /**
   * 新物料安装位置
   */
  sourceNewItemPosition?: string;

  /**
   * BOM番号
   */
  sourceBomCode?: string;

  /**
   * 兼容性（两位码第1位 A=有 B=→ C=← D=无；第2位 1～9=同时变更 *=无同时变更）
   */
  sourceCompatibility?: string;

  /**
   * 第二供应商（字典 logistics_manufacturing_ec_2nd_vendor；1=有，2=优先，3=无）
   */
  source2ndVendor?: string;

  /**
   * 安排指示（字典 logistics_manufacturing_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
   */
  sourceInstruction?: string;

  /**
   * 旧物料处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
   */
  sourceOldPartDisposition?: string;

  /**
   * BOM生效日期
   */
  sourceBomEffectiveDate?: string;

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

