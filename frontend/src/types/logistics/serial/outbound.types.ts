// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/serial
// 文件名称：outbound.types.ts
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
 * 序列号出库主表实体
 * 对应前端 TaktSerialOutboundDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SerialOutbound
 * @description 对应后端 TaktSerialOutboundDto
 */
export interface SerialOutbound extends CompanyDtoBase {
  /**
   * SerialOutboundID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  serialOutboundId: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 出库单号（租户+公司+工厂内唯一）
   */
  outboundCode: string;

  /**
   * 发货单号
   */
  shippingInvoiceCode: string;

  /**
   * 装车日期
   */
  outboundDate: string;

  /**
   * 仕向地（选项 TaktModelDestinations/options，DictValue=DestinationCode）
   */
  destination: string;

  /**
   * 目的地港（字典 logistics_serial_destination_port_code；DictValue 为港口/运输编码，如 ACE_AIR、VIE）
   */
  destinationPort: string;

  /**
   * 出库类型（字典 logistics_materials_outbound_type；0=销售出库 1=生产领料 2=退货出库 3=调拨出库 4=报废出库 5=序列号出库 6=其他）
   */
  outboundType: number;

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
   * 序列号出库明细列表（主子表关系） （子表：TaktSerialOutboundItem）
   */
  items?: SerialOutboundItem[];

}


/**
 * SerialOutbound 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SerialOutboundQuery
 * @description 对应后端 TaktSerialOutboundQueryDto
 */
export interface SerialOutboundQuery extends TaktPagedQuery {
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
   * 出库单号（租户+公司+工厂内唯一）
   */
  outboundCode?: string;

  /**
   * 发货单号
   */
  shippingInvoiceCode?: string;

  /**
   * 装车日期（范围查询-开始）
   */
  outboundDateStart?: string;

  /**
   * 装车日期（范围查询-结束）
   */
  outboundDateEnd?: string;

  /**
   * 仕向地（选项 TaktModelDestinations/options，DictValue=DestinationCode）
   */
  destination?: string;

  /**
   * 目的地港（字典 logistics_serial_destination_port_code；DictValue 为港口/运输编码，如 ACE_AIR、VIE）
   */
  destinationPort?: string;

  /**
   * 出库类型（字典 logistics_materials_outbound_type；0=销售出库 1=生产领料 2=退货出库 3=调拨出库 4=报废出库 5=序列号出库 6=其他）
   */
  outboundType?: number;

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
 * 创建SerialOutbound DTO
 * 对应前端 SerialOutboundCreate
 * @description 对应后端 TaktSerialOutboundCreateDto
 */
export interface SerialOutboundCreate {
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
   * 出库单号（租户+公司+工厂内唯一）
   */
  outboundCode: string;

  /**
   * 发货单号
   */
  shippingInvoiceCode: string;

  /**
   * 装车日期
   */
  outboundDate: string;

  /**
   * 仕向地（选项 TaktModelDestinations/options，DictValue=DestinationCode）
   */
  destination: string;

  /**
   * 目的地港（字典 logistics_serial_destination_port_code；DictValue 为港口/运输编码，如 ACE_AIR、VIE）
   */
  destinationPort: string;

  /**
   * 出库类型（字典 logistics_materials_outbound_type；0=销售出库 1=生产领料 2=退货出库 3=调拨出库 4=报废出库 5=序列号出库 6=其他）
   */
  outboundType: number;

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
   * 序列号出库明细列表（主子表关系）（子表，级联保存）
   */
  items?: SerialOutboundItemCreate[];

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
 * 更新SerialOutbound DTO
 * 继承 TaktSerialOutboundCreateDto，添加 SerialOutboundId 字段
 * 对应前端 SerialOutboundUpdate
 * @description 对应后端 TaktSerialOutboundUpdateDto
 */
export interface SerialOutboundUpdate extends SerialOutboundCreate {
  /**
   * SerialOutboundID（标识要更新的实体）
   */
  serialOutboundId: string;

}


/**
 * SerialOutbound 导入模板行 DTO
 * 对应前端 SerialOutboundTemplate
 * @description 对应后端 TaktSerialOutboundTemplateDto
 */
export interface SerialOutboundTemplate {
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
   * 出库单号（租户+公司+工厂内唯一）
   */
  outboundCode?: string;

  /**
   * 发货单号
   */
  shippingInvoiceCode?: string;

  /**
   * 装车日期
   */
  outboundDate?: string;

  /**
   * 仕向地（选项 TaktModelDestinations/options，DictValue=DestinationCode）
   */
  destination?: string;

  /**
   * 目的地港（字典 logistics_serial_destination_port_code；DictValue 为港口/运输编码，如 ACE_AIR、VIE）
   */
  destinationPort?: string;

  /**
   * 出库类型（字典 logistics_materials_outbound_type；0=销售出库 1=生产领料 2=退货出库 3=调拨出库 4=报废出库 5=序列号出库 6=其他）
   */
  outboundType?: number;

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
   * 序列号出库明细列表（主子表关系）（子表，级联保存）
   */
  items?: SerialOutboundItemCreate[];

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
 * SerialOutbound 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SerialOutboundImport
 * @description 对应后端 TaktSerialOutboundImportDto
 */
export interface SerialOutboundImport {
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
   * 出库单号（租户+公司+工厂内唯一）
   */
  outboundCode?: string;

  /**
   * 发货单号
   */
  shippingInvoiceCode?: string;

  /**
   * 装车日期
   */
  outboundDate?: string;

  /**
   * 仕向地（选项 TaktModelDestinations/options，DictValue=DestinationCode）
   */
  destination?: string;

  /**
   * 目的地港（字典 logistics_serial_destination_port_code；DictValue 为港口/运输编码，如 ACE_AIR、VIE）
   */
  destinationPort?: string;

  /**
   * 出库类型（字典 logistics_materials_outbound_type；0=销售出库 1=生产领料 2=退货出库 3=调拨出库 4=报废出库 5=序列号出库 6=其他）
   */
  outboundType?: number;

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
   * 序列号出库明细列表（主子表关系）（子表，级联保存）
   */
  items?: SerialOutboundItemCreate[];

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
 * SerialOutbound 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SerialOutboundExport
 * @description 对应后端 TaktSerialOutboundExportDto
 */
export interface SerialOutboundExport {
  /**
   * SerialOutboundID
   */
  serialOutboundId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 出库单号（租户+公司+工厂内唯一）
   */
  outboundCode: string;

  /**
   * 发货单号
   */
  shippingInvoiceCode: string;

  /**
   * 装车日期
   */
  outboundDate: string;

  /**
   * 仕向地（选项 TaktModelDestinations/options，DictValue=DestinationCode）
   */
  destination: string;

  /**
   * 目的地港（字典 logistics_serial_destination_port_code；DictValue 为港口/运输编码，如 ACE_AIR、VIE）
   */
  destinationPort: string;

  /**
   * 出库类型（字典 logistics_materials_outbound_type；0=销售出库 1=生产领料 2=退货出库 3=调拨出库 4=报废出库 5=序列号出库 6=其他）
   */
  outboundType: number;

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

