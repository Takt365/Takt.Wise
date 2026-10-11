// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：storageLocation.types.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/materials 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * Takt库位主数据实体（公司级；从属于 TaktWarehouse）
 * 对应前端 TaktStorageLocationDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 StorageLocation
 * @description 对应后端 TaktStorageLocationDto
 */
export interface StorageLocation extends CompanyDtoBase {

  /**
   * 仓库编码（冗余字段，便于查询；关联 TaktWarehouse.WarehouseCode）
   */
  warehouseCode?: string;

  /**
   * 库位编码（租户+公司+工厂+仓库内唯一；序列号入出库等业务表存此编码）
   */
  locationCode?: string;

  /**
   * 库位名称
   */
  locationName?: string;

  /**
   * 库位类型（0=存储区，1=拣货区，2=暂存区，3=不良品区，4=其他）
   */
  locationType?: number;

  /**
   * 库位状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  locationStatus?: number;

  /**
   * 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
   */
  isBuiltIn?: number;

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
 * StorageLocation 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 StorageLocationExport
 * @description 对应后端 TaktStorageLocationExportDto
 */
export interface StorageLocationExport {
  /**
   * StorageLocationID
   */
  storageLocationId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 仓库ID（主子表关系，关联 TaktWarehouse 主键）
   */
  warehouseId: string;

  /**
   * 工厂代码（冗余字段，便于查询；关联 TaktPlant.PlantCode）
   */
  plantCode: string;

  /**
   * 仓库编码（冗余字段，便于查询；关联 TaktWarehouse.WarehouseCode）
   */
  warehouseCode: string;

  /**
   * 库位编码（租户+公司+工厂+仓库内唯一；序列号入出库等业务表存此编码）
   */
  locationCode: string;

  /**
   * 库位名称
   */
  locationName: string;

  /**
   * 库位类型（0=存储区，1=拣货区，2=暂存区，3=不良品区，4=其他）
   */
  locationType: number;

  /**
   * 库位状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  locationStatus: number;

  /**
   * 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
   */
  isBuiltIn: number;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

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

