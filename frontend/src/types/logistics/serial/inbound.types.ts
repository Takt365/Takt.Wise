// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/serial
// 文件名称：inbound.types.ts
// 创建时间：2026-07-02
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/serial 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 序列号入库主表实体
 * 对应前端 TaktSerialInboundDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SerialInbound
 * @description 对应后端 TaktSerialInboundDto
 */
export interface SerialInbound extends CompanyDtoBase {
  /**
   * SerialInboundID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  serialInboundId: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 入库单号（租户+公司+工厂内唯一）
   */
  inboundCode: string;

  /**
   * 入库日期
   */
  inboundDate: string;

  /**
   * 入库类型（字典 logistics_materials_inbound_type；0=采购入库 1=生产入库 2=退货入库 3=调拨入库 4=序列号入库 5=其他）
   */
  inboundType: number;

  /**
   * 仓库编码（关联 TaktWarehouse.WarehouseCode，选项 TaktWarehouses/options）
   */
  warehouseCode: string;

  /**
   * 库位编码（关联 TaktStorageLocation.LocationCode，选项 TaktStorageLocations/options）
   */
  locationCode: string;

  /**
   * 总数量
   */
  totalQuantity: number;

  /**
   * 序列号入库明细列表（主子表关系） （子表：TaktSerialInboundItem）
   */
  items?: SerialInboundItem[];

}


/**
 * SerialInbound 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SerialInboundQuery
 * @description 对应后端 TaktSerialInboundQueryDto
 */
export interface SerialInboundQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 入库单号（租户+公司+工厂内唯一）
   */
  inboundCode?: string;

  /**
   * 入库日期（范围查询-开始）
   */
  inboundDateStart?: string;

  /**
   * 入库日期（范围查询-结束）
   */
  inboundDateEnd?: string;

  /**
   * 入库类型（字典 logistics_materials_inbound_type；0=采购入库 1=生产入库 2=退货入库 3=调拨入库 4=序列号入库 5=其他）
   */
  inboundType?: number;

  /**
   * 仓库编码（关联 TaktWarehouse.WarehouseCode，选项 TaktWarehouses/options）
   */
  warehouseCode?: string;

  /**
   * 库位编码（关联 TaktStorageLocation.LocationCode，选项 TaktStorageLocations/options）
   */
  locationCode?: string;

  /**
   * 总数量
   */
  totalQuantity?: number;

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
 * 创建SerialInbound DTO
 * 对应前端 SerialInboundCreate
 * @description 对应后端 TaktSerialInboundCreateDto
 */
export interface SerialInboundCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 入库单号（租户+公司+工厂内唯一）
   */
  inboundCode: string;

  /**
   * 入库日期
   */
  inboundDate: string;

  /**
   * 入库类型（字典 logistics_materials_inbound_type；0=采购入库 1=生产入库 2=退货入库 3=调拨入库 4=序列号入库 5=其他）
   */
  inboundType: number;

  /**
   * 仓库编码（关联 TaktWarehouse.WarehouseCode，选项 TaktWarehouses/options）
   */
  warehouseCode: string;

  /**
   * 库位编码（关联 TaktStorageLocation.LocationCode，选项 TaktStorageLocations/options）
   */
  locationCode: string;

  /**
   * 总数量
   */
  totalQuantity: number;

  /**
   * 序列号入库明细列表（主子表关系）（子表，级联保存）
   */
  items?: SerialInboundItemCreate[];

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
 * 更新SerialInbound DTO
 * 继承 TaktSerialInboundCreateDto，添加 SerialInboundId 字段
 * 对应前端 SerialInboundUpdate
 * @description 对应后端 TaktSerialInboundUpdateDto
 */
export interface SerialInboundUpdate extends SerialInboundCreate {
  /**
   * SerialInboundID（标识要更新的实体）
   */
  serialInboundId: string;

}


/**
 * SerialInbound 导入模板行 DTO
 * 对应前端 SerialInboundTemplate
 * @description 对应后端 TaktSerialInboundTemplateDto
 */
export interface SerialInboundTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 入库单号（租户+公司+工厂内唯一）
   */
  inboundCode?: string;

  /**
   * 入库日期
   */
  inboundDate?: string;

  /**
   * 入库类型（字典 logistics_materials_inbound_type；0=采购入库 1=生产入库 2=退货入库 3=调拨入库 4=序列号入库 5=其他）
   */
  inboundType?: number;

  /**
   * 仓库编码（关联 TaktWarehouse.WarehouseCode，选项 TaktWarehouses/options）
   */
  warehouseCode?: string;

  /**
   * 库位编码（关联 TaktStorageLocation.LocationCode，选项 TaktStorageLocations/options）
   */
  locationCode?: string;

  /**
   * 总数量
   */
  totalQuantity?: number;

  /**
   * 序列号入库明细列表（主子表关系）（子表，级联保存）
   */
  items?: SerialInboundItemCreate[];

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
 * SerialInbound 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SerialInboundImport
 * @description 对应后端 TaktSerialInboundImportDto
 */
export interface SerialInboundImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 入库单号（租户+公司+工厂内唯一）
   */
  inboundCode?: string;

  /**
   * 入库日期
   */
  inboundDate?: string;

  /**
   * 入库类型（字典 logistics_materials_inbound_type；0=采购入库 1=生产入库 2=退货入库 3=调拨入库 4=序列号入库 5=其他）
   */
  inboundType?: number;

  /**
   * 仓库编码（关联 TaktWarehouse.WarehouseCode，选项 TaktWarehouses/options）
   */
  warehouseCode?: string;

  /**
   * 库位编码（关联 TaktStorageLocation.LocationCode，选项 TaktStorageLocations/options）
   */
  locationCode?: string;

  /**
   * 总数量
   */
  totalQuantity?: number;

  /**
   * 序列号入库明细列表（主子表关系）（子表，级联保存）
   */
  items?: SerialInboundItemCreate[];

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
 * SerialInbound 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SerialInboundExport
 * @description 对应后端 TaktSerialInboundExportDto
 */
export interface SerialInboundExport {
  /**
   * SerialInboundID
   */
  serialInboundId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 入库单号（租户+公司+工厂内唯一）
   */
  inboundCode: string;

  /**
   * 入库日期
   */
  inboundDate: string;

  /**
   * 入库类型（字典 logistics_materials_inbound_type；0=采购入库 1=生产入库 2=退货入库 3=调拨入库 4=序列号入库 5=其他）
   */
  inboundType: number;

  /**
   * 仓库编码（关联 TaktWarehouse.WarehouseCode，选项 TaktWarehouses/options）
   */
  warehouseCode: string;

  /**
   * 库位编码（关联 TaktStorageLocation.LocationCode，选项 TaktStorageLocations/options）
   */
  locationCode: string;

  /**
   * 总数量
   */
  totalQuantity: number;

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

