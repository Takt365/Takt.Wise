// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/bom
// 文件名称：routing.types.ts
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/bom 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 工艺路线主表实体
 * 对应前端 TaktRoutingDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 Routing
 * @description 对应后端 TaktRoutingDto
 */
export interface Routing extends ApprovalDtoBase {

  /**
   * 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode，ExtValue=PlantCode）
   */
  workCenter: string;

  /**
   * 工艺路线编码
   */
  routingCode: string;

  /**
   * 工艺路线名称
   */
  routingName: string;

  /**
   * 用途（字典 logistics_manufacturing_routing_purpose：1=生产，2=工程/设计，3=万能，4=工厂维护）
   */
  purpose: number;

  /**
   * 适用物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode: string;

  /**
   * 版本号
   */
  version: string;

  /**
   * 状态（字典 logistics_manufacturing_routing_status：1=生成的，2=对订单下达，3=对成本核算下达，4=下达的）
   */
  routingStatus: number;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 失效日期
   */
  expiryDate?: string;

  /**
   * 工艺路线说明
   */
  routingDescription?: string;

  /**
   * 工艺路线明细列表（主子表关系） （子表：TaktRoutingItem）
   */
  items?: RoutingItem[];

}


/**
 * Routing 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 RoutingQuery
 * @description 对应后端 TaktRoutingQueryDto
 */
export interface RoutingQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
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
   * 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode，ExtValue=PlantCode）
   */
  workCenter?: string;

  /**
   * 工艺路线编码
   */
  routingCode?: string;

  /**
   * 工艺路线名称
   */
  routingName?: string;

  /**
   * 工艺路线组编码（选项 TaktBomGroups/options?bomGroupCategory=2；DictValue=BomGroupCode）
   */
  routingGroupCode?: string;

  /**
   * 工艺路线组名称（冗余：按 RoutingGroupCode 取 TaktBomGroups.BomGroupName 联动）
   */
  routingGroupName?: string;

  /**
   * 用途（字典 logistics_manufacturing_routing_purpose：1=生产，2=工程/设计，3=万能，4=工厂维护）
   */
  purpose?: number;

  /**
   * 适用物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 版本号
   */
  version?: string;

  /**
   * 状态（字典 logistics_manufacturing_routing_status：1=生成的，2=对订单下达，3=对成本核算下达，4=下达的）
   */
  routingStatus?: number;

  /**
   * 生效日期（范围查询-开始）
   */
  effectiveDateStart?: string;

  /**
   * 生效日期（范围查询-结束）
   */
  effectiveDateEnd?: string;

  /**
   * 失效日期（范围查询-开始）
   */
  expiryDateStart?: string;

  /**
   * 失效日期（范围查询-结束）
   */
  expiryDateEnd?: string;

  /**
   * 工艺路线说明
   */
  routingDescription?: string;

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
 * 创建Routing DTO
 * 对应前端 RoutingCreate
 * @description 对应后端 TaktRoutingCreateDto
 */
export interface RoutingCreate {
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
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode，ExtValue=PlantCode）
   */
  workCenter: string;

  /**
   * 工艺路线编码
   */
  routingCode: string;

  /**
   * 工艺路线名称
   */
  routingName: string;

  /**
   * 用途（字典 logistics_manufacturing_routing_purpose：1=生产，2=工程/设计，3=万能，4=工厂维护）
   */
  purpose: number;

  /**
   * 适用物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode: string;

  /**
   * 版本号
   */
  version: string;

  /**
   * 状态（字典 logistics_manufacturing_routing_status：1=生成的，2=对订单下达，3=对成本核算下达，4=下达的）
   */
  routingStatus: number;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 失效日期
   */
  expiryDate?: string;

  /**
   * 工艺路线说明
   */
  routingDescription?: string;

  /**
   * 工艺路线明细列表（主子表关系）（子表，级联保存）
   */
  items?: RoutingItemCreate[];

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
 * 更新Routing DTO
 * 继承 TaktRoutingCreateDto，添加 RoutingId 字段
 * 对应前端 RoutingUpdate
 * @description 对应后端 TaktRoutingUpdateDto
 */
export interface RoutingUpdate extends RoutingCreate {
  /**
   * RoutingID（标识要更新的实体）
   */
  routingId: string;

  /**
   * 工艺路线明细列表（主子表关系）（子表，级联保存）
   */
  items?: any;

}


/**
 * Routing 状态更新 DTO
 * 对应前端 RoutingStatus
 * @description 对应后端 TaktRoutingStatusDto
 */
export interface RoutingStatus {
  /**
   * RoutingID
   */
  routingId: string;

  /**
   * 状态（字典 logistics_manufacturing_routing_status：1=生成的，2=对订单下达，3=对成本核算下达，4=下达的）
   */
  routingStatus: number;

}


/**
 * Routing 导入模板行 DTO
 * 对应前端 RoutingTemplate
 * @description 对应后端 TaktRoutingTemplateDto
 */
export interface RoutingTemplate {
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
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode，ExtValue=PlantCode）
   */
  workCenter?: string;

  /**
   * 工艺路线编码
   */
  routingCode?: string;

  /**
   * 工艺路线名称
   */
  routingName?: string;

  /**
   * 工艺路线组编码（选项 TaktBomGroups/options?bomGroupCategory=2；DictValue=BomGroupCode）
   */
  routingGroupCode?: string;

  /**
   * 工艺路线组名称（冗余：按 RoutingGroupCode 取 TaktBomGroups.BomGroupName 联动）
   */
  routingGroupName?: string;

  /**
   * 用途（字典 logistics_manufacturing_routing_purpose：1=生产，2=工程/设计，3=万能，4=工厂维护）
   */
  purpose?: number;

  /**
   * 适用物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 版本号
   */
  version?: string;

  /**
   * 状态（字典 logistics_manufacturing_routing_status：1=生成的，2=对订单下达，3=对成本核算下达，4=下达的）
   */
  routingStatus?: number;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 失效日期
   */
  expiryDate?: string;

  /**
   * 工艺路线说明
   */
  routingDescription?: string;

  /**
   * 工艺路线明细列表（主子表关系）（子表，级联保存）
   */
  items?: RoutingItemCreate[];

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
 * Routing 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 RoutingImport
 * @description 对应后端 TaktRoutingImportDto
 */
export interface RoutingImport {
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
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode，ExtValue=PlantCode）
   */
  workCenter?: string;

  /**
   * 工艺路线编码
   */
  routingCode?: string;

  /**
   * 工艺路线名称
   */
  routingName?: string;

  /**
   * 工艺路线组编码（选项 TaktBomGroups/options?bomGroupCategory=2；DictValue=BomGroupCode）
   */
  routingGroupCode?: string;

  /**
   * 工艺路线组名称（冗余：按 RoutingGroupCode 取 TaktBomGroups.BomGroupName 联动）
   */
  routingGroupName?: string;

  /**
   * 用途（字典 logistics_manufacturing_routing_purpose：1=生产，2=工程/设计，3=万能，4=工厂维护）
   */
  purpose?: number;

  /**
   * 适用物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 版本号
   */
  version?: string;

  /**
   * 状态（字典 logistics_manufacturing_routing_status：1=生成的，2=对订单下达，3=对成本核算下达，4=下达的）
   */
  routingStatus?: number;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 失效日期
   */
  expiryDate?: string;

  /**
   * 工艺路线说明
   */
  routingDescription?: string;

  /**
   * 工艺路线明细列表（主子表关系）（子表，级联保存）
   */
  items?: RoutingItemCreate[];

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
 * Routing 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 RoutingExport
 * @description 对应后端 TaktRoutingExportDto
 */
export interface RoutingExport {
  /**
   * RoutingID
   */
  routingId: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode，ExtValue=PlantCode）
   */
  workCenter: string;

  /**
   * 工艺路线编码
   */
  routingCode: string;

  /**
   * 工艺路线名称
   */
  routingName: string;

  /**
   * 用途（字典 logistics_manufacturing_routing_purpose：1=生产，2=工程/设计，3=万能，4=工厂维护）
   */
  purpose: number;

  /**
   * 适用物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode: string;

  /**
   * 版本号
   */
  version: string;

  /**
   * 状态（字典 logistics_manufacturing_routing_status：1=生成的，2=对订单下达，3=对成本核算下达，4=下达的）
   */
  routingStatus: number;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 失效日期
   */
  expiryDate?: string;

  /**
   * 工艺路线说明
   */
  routingDescription?: string;

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

