// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/aps
// 文件名称：changeoverMatrix.types.ts
// 创建时间：2026-07-13
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/aps 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 换型矩阵（工作中心 + 前产品 → 后产品的换型时间）
 * 对应前端 TaktChangeoverMatrixDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ChangeoverMatrix
 * @description 对应后端 TaktChangeoverMatrixDto
 */
export interface ChangeoverMatrix extends CompanyDtoBase {

  /**
   * 工作中心编码（关联 TaktWorkCenter.WorkCenterCode，选项 TaktWorkCenters/options，DictValue=WorkCenterCode）
   */
  workCenterCode?: string;

  /**
   * 换型前物料编码（关联 TaktGeneralMaterial.MaterialCode，选项 TaktGeneralMaterials/options）
   */
  fromMaterialCode?: string;

  /**
   * 换型后物料编码（关联 TaktGeneralMaterial.MaterialCode，选项 TaktGeneralMaterials/options）
   */
  toMaterialCode?: string;

  /**
   * 换型时间（分钟）
   */
  changeoverMinutes?: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  matrixStatus?: number;

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
 * ChangeoverMatrix 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ChangeoverMatrixExport
 * @description 对应后端 TaktChangeoverMatrixExportDto
 */
export interface ChangeoverMatrixExport {
  /**
   * ChangeoverMatrixID
   */
  changeoverMatrixId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 工作中心编码（关联 TaktWorkCenter.WorkCenterCode，选项 TaktWorkCenters/options，DictValue=WorkCenterCode）
   */
  workCenterCode: string;

  /**
   * 换型前物料编码（关联 TaktGeneralMaterial.MaterialCode，选项 TaktGeneralMaterials/options）
   */
  fromMaterialCode: string;

  /**
   * 换型后物料编码（关联 TaktGeneralMaterial.MaterialCode，选项 TaktGeneralMaterials/options）
   */
  toMaterialCode: string;

  /**
   * 换型时间（分钟）
   */
  changeoverMinutes: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  matrixStatus: number;

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

