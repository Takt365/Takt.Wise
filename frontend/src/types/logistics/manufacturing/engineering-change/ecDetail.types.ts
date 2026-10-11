// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ecDetail.types.ts
// 创建时间：2026-09-08
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
 * 设变明细（技术阶段一 ③，外键 EcGijutsuId 隶属 TaktEcGijutsu；含各部门执行视图外键 EcXxxId）。对应前端 TaktEcDetailDto。
 * 对应前端 EcDetail
 * @description 对应后端 TaktEcDetailDto
 */
export interface EcDetail extends CompanyDtoBase {
  /**
   * EcDetailID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  ecDetailId: string;

  /**
   * 设变主表ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  ecGijutsuId: string;

  /**
   * 设变主表名称（填充字段）
   */
  ecName?: string;

  /**
   * 设变单号（冗余字段,便于查询）
   */
  ecCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * BOM行号
   */
  ecBomLineCode?: string;

  /**
   * 机种
   */
  ecModelCode: string;

  /**
   * 根物料编码
   */
  ecRootMaterialCode?: string;

  /**
   * 根物料编码物料描述
   */
  ecRootMaterialDescription?: string;

  /**
   * 上阶物料编码
   */
  ecParentMaterialCode?: string;

  /**
   * 上阶物料描述（冗余：按 EcParentMaterialCode 取 TaktMaterialPlant.MaterialDescription 联动）
   */
  ecParentMaterialDescription?: string;

  /**
   * 停产状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料）
   */
  discontinuedStatus: string;
  /**
   * 实施范围（冗余：来自 TaktEcGijutsu.EcScope）
   */
  ecScope: number;

  /**
   * 旧物料编码
   */
  ecOldMaterialCode?: string;

  /**
   * 旧物料描述
   */
  ecOldMaterialDescription?: string;

  /**
   * 旧用量
   */
  ecOldUsageQuantity?: number;

  /**
   * 旧位置
   */
  ecOldItemPosition?: string;

  /**
   * 旧品在库
   */
  ecOldStock?: number;

  /**
   * 旧品仓库
   */
  ecOldWarehouse?: string;

  /**
   * 旧采购类型（F=外部采购，E=自制生产；与 BOM 采购类型口径一致）
   */
  ecOldPurchaseType?: string;

  /**
   * 旧品检验（字典 sys_yes_no；0=否 1=是）
   */
  ecOldRequiresInspection: number;

  /**
   * 新物料编码
   */
  ecNewMaterialCode?: string;

  /**
   * 新物料描述
   */
  ecNewMaterialDescription?: string;

  /**
   * 新用量
   */
  ecNewUsageQuantity?: number;

  /**
   * 新位置
   */
  ecNewItemPosition?: string;

  /**
   * 新品在库
   */
  ecNewStock?: number;

  /**
   * 新品仓库
   */
  ecNewWarehouse?: string;

  /**
   * 新采购类型（F=外部采购，E=自制生产；与 BOM 采购类型口径一致）
   */
  ecNewPurchaseType?: string;

  /**
   * 新品检验（字典 sys_yes_no；0=否 1=是）
   */
  ecNewRequiresInspection: number;

  /**
   * BOM生效日期
   */
  ecBomDate: string;

  /**
   * 兼容性（两位码第1位 A=有 B=→ C=← D=无；第2位 1～9=同时变更 *=无同时变更）
   */
  ecIsCompatible?: string;

  /**
   * 第二供应商（字典 logistics_manufacturing_ec_2nd_vendor；1=有，2=优先，3=无）
   */
  ec2ndVendor?: string;

  /**
   * 生产指令（字典 logistics_manufacturing_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
   */
  ecInstruction?: string;

  /**
   * 旧品处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
   */
  ecOldPartDisposition?: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * EcDetail 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EcDetailQuery
 * @description 对应后端 TaktEcDetailQueryDto
 */
export interface EcDetailQuery extends TaktPagedQuery {
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
   * 设变主表ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  ecGijutsuId?: string;

  /**
   * 设变单号（冗余字段,便于查询）
   */
  ecCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * BOM行号
   */
  ecBomLineCode?: string;

  /**
   * 机种
   */
  ecModelCode?: string;

  /**
   * 根物料编码
   */
  ecRootMaterialCode?: string;

  /**
   * 根物料编码物料描述
   */
  ecRootMaterialDescription?: string;

  /**
   * 上阶物料编码
   */
  ecParentMaterialCode?: string;

  /**
   * 上阶物料描述（冗余：按 EcParentMaterialCode 取 TaktMaterialPlant.MaterialDescription 联动）
   */
  ecParentMaterialDescription?: string;

  /**
   * 停产状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料）
   */
  discontinuedStatus?: string;
  /**
   * 实施范围（冗余：来自 TaktEcGijutsu.EcScope）
   */
  ecScope?: number;

  /**
   * 旧物料编码
   */
  ecOldMaterialCode?: string;

  /**
   * 旧物料描述
   */
  ecOldMaterialDescription?: string;

  /**
   * 旧用量
   */
  ecOldUsageQuantity?: number;

  /**
   * 旧位置
   */
  ecOldItemPosition?: string;

  /**
   * 旧品在库
   */
  ecOldStock?: number;

  /**
   * 旧品仓库
   */
  ecOldWarehouse?: string;

  /**
   * 旧采购类型（F=外部采购，E=自制生产；与 BOM 采购类型口径一致）
   */
  ecOldPurchaseType?: string;

  /**
   * 旧品检验（字典 sys_yes_no；0=否 1=是）
   */
  ecOldRequiresInspection?: number;

  /**
   * 新物料编码
   */
  ecNewMaterialCode?: string;

  /**
   * 新物料描述
   */
  ecNewMaterialDescription?: string;

  /**
   * 新用量
   */
  ecNewUsageQuantity?: number;

  /**
   * 新位置
   */
  ecNewItemPosition?: string;

  /**
   * 新品在库
   */
  ecNewStock?: number;

  /**
   * 新品仓库
   */
  ecNewWarehouse?: string;

  /**
   * 新采购类型（F=外部采购，E=自制生产；与 BOM 采购类型口径一致）
   */
  ecNewPurchaseType?: string;

  /**
   * 新品检验（字典 sys_yes_no；0=否 1=是）
   */
  ecNewRequiresInspection?: number;

  /**
   * BOM生效日期（范围查询-开始）
   */
  ecBomDateStart?: string;

  /**
   * BOM生效日期（范围查询-结束）
   */
  ecBomDateEnd?: string;

  /**
   * 兼容性（两位码第1位 A=有 B=→ C=← D=无；第2位 1～9=同时变更 *=无同时变更）
   */
  ecIsCompatible?: string;

  /**
   * 第二供应商（字典 logistics_manufacturing_ec_2nd_vendor；1=有，2=优先，3=无）
   */
  ec2ndVendor?: string;

  /**
   * 生产指令（字典 logistics_manufacturing_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
   */
  ecInstruction?: string;

  /**
   * 旧品处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
   */
  ecOldPartDisposition?: string;

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
 * 创建EcDetail DTO
 * 对应前端 EcDetailCreate
 * @description 对应后端 TaktEcDetailCreateDto
 */
export interface EcDetailCreate {
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
   * 设变主表ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  ecGijutsuId: string;

  /**
   * 设变单号（冗余字段,便于查询）
   */
  ecCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * BOM行号
   */
  ecBomLineCode?: string;

  /**
   * 机种
   */
  ecModelCode: string;

  /**
   * 根物料编码
   */
  ecRootMaterialCode?: string;

  /**
   * 根物料编码物料描述
   */
  ecRootMaterialDescription?: string;

  /**
   * 上阶物料编码
   */
  ecParentMaterialCode?: string;

  /**
   * 上阶物料描述（冗余：按 EcParentMaterialCode 取 TaktMaterialPlant.MaterialDescription 联动）
   */
  ecParentMaterialDescription?: string;

  /**
   * 停产状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料）
   */
  discontinuedStatus: string;
  /**
   * 实施范围（冗余：来自 TaktEcGijutsu.EcScope）
   */
  ecScope: number;

  /**
   * 旧物料编码
   */
  ecOldMaterialCode?: string;

  /**
   * 旧物料描述
   */
  ecOldMaterialDescription?: string;

  /**
   * 旧用量
   */
  ecOldUsageQuantity?: number;

  /**
   * 旧位置
   */
  ecOldItemPosition?: string;

  /**
   * 旧品在库
   */
  ecOldStock?: number;

  /**
   * 旧品仓库
   */
  ecOldWarehouse?: string;

  /**
   * 旧采购类型（F=外部采购，E=自制生产；与 BOM 采购类型口径一致）
   */
  ecOldPurchaseType?: string;

  /**
   * 旧品检验（字典 sys_yes_no；0=否 1=是）
   */
  ecOldRequiresInspection: number;

  /**
   * 新物料编码
   */
  ecNewMaterialCode?: string;

  /**
   * 新物料描述
   */
  ecNewMaterialDescription?: string;

  /**
   * 新用量
   */
  ecNewUsageQuantity?: number;

  /**
   * 新位置
   */
  ecNewItemPosition?: string;

  /**
   * 新品在库
   */
  ecNewStock?: number;

  /**
   * 新品仓库
   */
  ecNewWarehouse?: string;

  /**
   * 新采购类型（F=外部采购，E=自制生产；与 BOM 采购类型口径一致）
   */
  ecNewPurchaseType?: string;

  /**
   * 新品检验（字典 sys_yes_no；0=否 1=是）
   */
  ecNewRequiresInspection: number;

  /**
   * BOM生效日期
   */
  ecBomDate: string;

  /**
   * 兼容性（两位码第1位 A=有 B=→ C=← D=无；第2位 1～9=同时变更 *=无同时变更）
   */
  ecIsCompatible?: string;

  /**
   * 第二供应商（字典 logistics_manufacturing_ec_2nd_vendor；1=有，2=优先，3=无）
   */
  ec2ndVendor?: string;

  /**
   * 生产指令（字典 logistics_manufacturing_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
   */
  ecInstruction?: string;

  /**
   * 旧品处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
   */
  ecOldPartDisposition?: string;

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
 * 更新EcDetail DTO
 * 继承 TaktEcDetailCreateDto，添加 EcDetailId 字段
 * 对应前端 EcDetailUpdate
 * @description 对应后端 TaktEcDetailUpdateDto
 */
export interface EcDetailUpdate extends EcDetailCreate {
  /**
   * EcDetailID（标识要更新的实体）
   */
  ecDetailId: string;

}


/**
 * EcDetail 作废/撤销作废 DTO
 * 对应前端 EcDetailObsolete
 * @description 对应后端 TaktEcDetailObsoleteDto
 */
export interface EcDetailObsolete {
  /**
   * EcDetailID
   */
  ecDetailId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * EcDetail 导入模板行 DTO
 * 对应前端 EcDetailTemplate
 * @description 对应后端 TaktEcDetailTemplateDto
 */
export interface EcDetailTemplate {
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
   * 设变主表ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  ecGijutsuId?: string;

  /**
   * 设变单号（冗余字段,便于查询）
   */
  ecCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * BOM行号
   */
  ecBomLineCode?: string;

  /**
   * 机种
   */
  ecModelCode?: string;

  /**
   * 根物料编码
   */
  ecRootMaterialCode?: string;

  /**
   * 根物料编码物料描述
   */
  ecRootMaterialDescription?: string;

  /**
   * 上阶物料编码
   */
  ecParentMaterialCode?: string;

  /**
   * 上阶物料描述（冗余：按 EcParentMaterialCode 取 TaktMaterialPlant.MaterialDescription 联动）
   */
  ecParentMaterialDescription?: string;

  /**
   * 停产状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料）
   */
  discontinuedStatus?: string;
  /**
   * 实施范围（冗余：来自 TaktEcGijutsu.EcScope）
   */
  ecScope?: number;

  /**
   * 旧物料编码
   */
  ecOldMaterialCode?: string;

  /**
   * 旧物料描述
   */
  ecOldMaterialDescription?: string;

  /**
   * 旧用量
   */
  ecOldUsageQuantity?: number;

  /**
   * 旧位置
   */
  ecOldItemPosition?: string;

  /**
   * 旧品在库
   */
  ecOldStock?: number;

  /**
   * 旧品仓库
   */
  ecOldWarehouse?: string;

  /**
   * 旧采购类型（F=外部采购，E=自制生产；与 BOM 采购类型口径一致）
   */
  ecOldPurchaseType?: string;

  /**
   * 旧品检验（字典 sys_yes_no；0=否 1=是）
   */
  ecOldRequiresInspection?: number;

  /**
   * 新物料编码
   */
  ecNewMaterialCode?: string;

  /**
   * 新物料描述
   */
  ecNewMaterialDescription?: string;

  /**
   * 新用量
   */
  ecNewUsageQuantity?: number;

  /**
   * 新位置
   */
  ecNewItemPosition?: string;

  /**
   * 新品在库
   */
  ecNewStock?: number;

  /**
   * 新品仓库
   */
  ecNewWarehouse?: string;

  /**
   * 新采购类型（F=外部采购，E=自制生产；与 BOM 采购类型口径一致）
   */
  ecNewPurchaseType?: string;

  /**
   * 新品检验（字典 sys_yes_no；0=否 1=是）
   */
  ecNewRequiresInspection?: number;

  /**
   * BOM生效日期
   */
  ecBomDate?: string;

  /**
   * 兼容性（两位码第1位 A=有 B=→ C=← D=无；第2位 1～9=同时变更 *=无同时变更）
   */
  ecIsCompatible?: string;

  /**
   * 第二供应商（字典 logistics_manufacturing_ec_2nd_vendor；1=有，2=优先，3=无）
   */
  ec2ndVendor?: string;

  /**
   * 生产指令（字典 logistics_manufacturing_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
   */
  ecInstruction?: string;

  /**
   * 旧品处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
   */
  ecOldPartDisposition?: string;

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
 * EcDetail 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EcDetailImport
 * @description 对应后端 TaktEcDetailImportDto
 */
export interface EcDetailImport {
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
   * 设变主表ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  ecGijutsuId?: string;

  /**
   * 设变单号（冗余字段,便于查询）
   */
  ecCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * BOM行号
   */
  ecBomLineCode?: string;

  /**
   * 机种
   */
  ecModelCode?: string;

  /**
   * 根物料编码
   */
  ecRootMaterialCode?: string;

  /**
   * 根物料编码物料描述
   */
  ecRootMaterialDescription?: string;

  /**
   * 上阶物料编码
   */
  ecParentMaterialCode?: string;

  /**
   * 上阶物料描述（冗余：按 EcParentMaterialCode 取 TaktMaterialPlant.MaterialDescription 联动）
   */
  ecParentMaterialDescription?: string;

  /**
   * 停产状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料）
   */
  discontinuedStatus?: string;
  /**
   * 实施范围（冗余：来自 TaktEcGijutsu.EcScope）
   */
  ecScope?: number;

  /**
   * 旧物料编码
   */
  ecOldMaterialCode?: string;

  /**
   * 旧物料描述
   */
  ecOldMaterialDescription?: string;

  /**
   * 旧用量
   */
  ecOldUsageQuantity?: number;

  /**
   * 旧位置
   */
  ecOldItemPosition?: string;

  /**
   * 旧品在库
   */
  ecOldStock?: number;

  /**
   * 旧品仓库
   */
  ecOldWarehouse?: string;

  /**
   * 旧采购类型（F=外部采购，E=自制生产；与 BOM 采购类型口径一致）
   */
  ecOldPurchaseType?: string;

  /**
   * 旧品检验（字典 sys_yes_no；0=否 1=是）
   */
  ecOldRequiresInspection?: number;

  /**
   * 新物料编码
   */
  ecNewMaterialCode?: string;

  /**
   * 新物料描述
   */
  ecNewMaterialDescription?: string;

  /**
   * 新用量
   */
  ecNewUsageQuantity?: number;

  /**
   * 新位置
   */
  ecNewItemPosition?: string;

  /**
   * 新品在库
   */
  ecNewStock?: number;

  /**
   * 新品仓库
   */
  ecNewWarehouse?: string;

  /**
   * 新采购类型（F=外部采购，E=自制生产；与 BOM 采购类型口径一致）
   */
  ecNewPurchaseType?: string;

  /**
   * 新品检验（字典 sys_yes_no；0=否 1=是）
   */
  ecNewRequiresInspection?: number;

  /**
   * BOM生效日期
   */
  ecBomDate?: string;

  /**
   * 兼容性（两位码第1位 A=有 B=→ C=← D=无；第2位 1～9=同时变更 *=无同时变更）
   */
  ecIsCompatible?: string;

  /**
   * 第二供应商（字典 logistics_manufacturing_ec_2nd_vendor；1=有，2=优先，3=无）
   */
  ec2ndVendor?: string;

  /**
   * 生产指令（字典 logistics_manufacturing_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
   */
  ecInstruction?: string;

  /**
   * 旧品处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
   */
  ecOldPartDisposition?: string;

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
 * EcDetail 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EcDetailExport
 * @description 对应后端 TaktEcDetailExportDto
 */
export interface EcDetailExport {
  /**
   * EcDetailID
   */
  ecDetailId: string;

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
   * 设变主表ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  ecGijutsuId: string;

  /**
   * 设变单号（冗余字段,便于查询）
   */
  ecCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * BOM行号
   */
  ecBomLineCode?: string;

  /**
   * 机种
   */
  ecModelCode: string;

  /**
   * 根物料编码
   */
  ecRootMaterialCode?: string;

  /**
   * 根物料编码物料描述
   */
  ecRootMaterialDescription?: string;

  /**
   * 上阶物料编码
   */
  ecParentMaterialCode?: string;

  /**
   * 上阶物料描述（冗余：按 EcParentMaterialCode 取 TaktMaterialPlant.MaterialDescription 联动）
   */
  ecParentMaterialDescription?: string;

  /**
   * 停产状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料）
   */
  discontinuedStatus: string;
  /**
   * 实施范围（冗余：来自 TaktEcGijutsu.EcScope）
   */
  ecScope: number;

  /**
   * 旧物料编码
   */
  ecOldMaterialCode?: string;

  /**
   * 旧物料描述
   */
  ecOldMaterialDescription?: string;

  /**
   * 旧用量
   */
  ecOldUsageQuantity?: number;

  /**
   * 旧位置
   */
  ecOldItemPosition?: string;

  /**
   * 旧品在库
   */
  ecOldStock?: number;

  /**
   * 旧品仓库
   */
  ecOldWarehouse?: string;

  /**
   * 旧采购类型（F=外部采购，E=自制生产；与 BOM 采购类型口径一致）
   */
  ecOldPurchaseType?: string;

  /**
   * 旧品检验（字典 sys_yes_no；0=否 1=是）
   */
  ecOldRequiresInspection: number;

  /**
   * 新物料编码
   */
  ecNewMaterialCode?: string;

  /**
   * 新物料描述
   */
  ecNewMaterialDescription?: string;

  /**
   * 新用量
   */
  ecNewUsageQuantity?: number;

  /**
   * 新位置
   */
  ecNewItemPosition?: string;

  /**
   * 新品在库
   */
  ecNewStock?: number;

  /**
   * 新品仓库
   */
  ecNewWarehouse?: string;

  /**
   * 新采购类型（F=外部采购，E=自制生产；与 BOM 采购类型口径一致）
   */
  ecNewPurchaseType?: string;

  /**
   * 新品检验（字典 sys_yes_no；0=否 1=是）
   */
  ecNewRequiresInspection: number;

  /**
   * BOM生效日期
   */
  ecBomDate: string;

  /**
   * 兼容性（两位码第1位 A=有 B=→ C=← D=无；第2位 1～9=同时变更 *=无同时变更）
   */
  ecIsCompatible?: string;

  /**
   * 第二供应商（字典 logistics_manufacturing_ec_2nd_vendor；1=有，2=优先，3=无）
   */
  ec2ndVendor?: string;

  /**
   * 生产指令（字典 logistics_manufacturing_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
   */
  ecInstruction?: string;

  /**
   * 旧品处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
   */
  ecOldPartDisposition?: string;

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

