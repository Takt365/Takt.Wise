// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/sop
// 文件名称：execScan.types.ts
// 创建时间：2026-08-12
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/sop 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * SOP 物料扫码记录实体
 * 对应前端 TaktSopExecScanDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SopExecScan
 * @description 对应后端 TaktSopExecScanDto
 */
export interface SopExecScan extends CompanyDtoBase {
  /**
   * SopExecScanID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  sopExecScanId: string;

  /**
   * 执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）
   */
  execId: string;

  /**
   * 执行追溯 名称（填充字段）
   */
  execName?: string;

  /**
   * 工步执行明细 ID（选项 TaktSopExecSteps/options；DictValue=Id）
   */
  execStepId?: string;

  /**
   * 工步执行明细 名称（填充字段）
   */
  execStepName?: string;

  /**
   * 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
   */
  stepId: string;

  /**
   * 工步 名称（填充字段）
   */
  stepName?: string;

  /**
   * 扫描条码
   */
  scannedBarcode: string;

  /**
   * 期望物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  expectedMaterialCode?: string;

  /**
   * 扫码结果（字典 logistics_manufacturing_sop_scan_result；1=PASS，2=NG）
   */
  scanResult: number;

  /**
   * 比对说明
   */
  matchMessage?: string;

  /**
   * 扫描时间
   */
  scannedAt: string;

  /**
   * 执行追溯 （主表：TaktSopExec）
   */
  exec?: SopExec;

}


/**
 * SopExecScan 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SopExecScanQuery
 * @description 对应后端 TaktSopExecScanQueryDto
 */
export interface SopExecScanQuery extends TaktPagedQuery {
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
   * 执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）
   */
  execId?: string;

  /**
   * 工步执行明细 ID（选项 TaktSopExecSteps/options；DictValue=Id）
   */
  execStepId?: string;

  /**
   * 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
   */
  stepId?: string;

  /**
   * 扫描条码
   */
  scannedBarcode?: string;

  /**
   * 期望物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  expectedMaterialCode?: string;

  /**
   * 扫码结果（字典 logistics_manufacturing_sop_scan_result；1=PASS，2=NG）
   */
  scanResult?: number;

  /**
   * 比对说明
   */
  matchMessage?: string;

  /**
   * 扫描时间（范围查询-开始）
   */
  scannedAtStart?: string;

  /**
   * 扫描时间（范围查询-结束）
   */
  scannedAtEnd?: string;

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
 * 创建SopExecScan DTO
 * 对应前端 SopExecScanCreate
 * @description 对应后端 TaktSopExecScanCreateDto
 */
export interface SopExecScanCreate {
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
   * 执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）
   */
  execId: string;

  /**
   * 工步执行明细 ID（选项 TaktSopExecSteps/options；DictValue=Id）
   */
  execStepId?: string;

  /**
   * 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
   */
  stepId: string;

  /**
   * 扫描条码
   */
  scannedBarcode: string;

  /**
   * 期望物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  expectedMaterialCode?: string;

  /**
   * 扫码结果（字典 logistics_manufacturing_sop_scan_result；1=PASS，2=NG）
   */
  scanResult: number;

  /**
   * 比对说明
   */
  matchMessage?: string;

  /**
   * 扫描时间
   */
  scannedAt: string;

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
 * 更新SopExecScan DTO
 * 继承 TaktSopExecScanCreateDto，添加 SopExecScanId 字段
 * 对应前端 SopExecScanUpdate
 * @description 对应后端 TaktSopExecScanUpdateDto
 */
export interface SopExecScanUpdate extends SopExecScanCreate {
  /**
   * SopExecScanID（标识要更新的实体）
   */
  sopExecScanId: string;

}


/**
 * SopExecScan 导入模板行 DTO
 * 对应前端 SopExecScanTemplate
 * @description 对应后端 TaktSopExecScanTemplateDto
 */
export interface SopExecScanTemplate {
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
   * 执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）
   */
  execId?: string;

  /**
   * 工步执行明细 ID（选项 TaktSopExecSteps/options；DictValue=Id）
   */
  execStepId?: string;

  /**
   * 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
   */
  stepId?: string;

  /**
   * 扫描条码
   */
  scannedBarcode?: string;

  /**
   * 期望物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  expectedMaterialCode?: string;

  /**
   * 扫码结果（字典 logistics_manufacturing_sop_scan_result；1=PASS，2=NG）
   */
  scanResult?: number;

  /**
   * 比对说明
   */
  matchMessage?: string;

  /**
   * 扫描时间
   */
  scannedAt?: string;

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
 * SopExecScan 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SopExecScanImport
 * @description 对应后端 TaktSopExecScanImportDto
 */
export interface SopExecScanImport {
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
   * 执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）
   */
  execId?: string;

  /**
   * 工步执行明细 ID（选项 TaktSopExecSteps/options；DictValue=Id）
   */
  execStepId?: string;

  /**
   * 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
   */
  stepId?: string;

  /**
   * 扫描条码
   */
  scannedBarcode?: string;

  /**
   * 期望物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  expectedMaterialCode?: string;

  /**
   * 扫码结果（字典 logistics_manufacturing_sop_scan_result；1=PASS，2=NG）
   */
  scanResult?: number;

  /**
   * 比对说明
   */
  matchMessage?: string;

  /**
   * 扫描时间
   */
  scannedAt?: string;

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
 * SopExecScan 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SopExecScanExport
 * @description 对应后端 TaktSopExecScanExportDto
 */
export interface SopExecScanExport {
  /**
   * SopExecScanID
   */
  sopExecScanId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）
   */
  execId: string;

  /**
   * 工步执行明细 ID（选项 TaktSopExecSteps/options；DictValue=Id）
   */
  execStepId?: string;

  /**
   * 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
   */
  stepId: string;

  /**
   * 扫描条码
   */
  scannedBarcode: string;

  /**
   * 期望物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  expectedMaterialCode?: string;

  /**
   * 扫码结果（字典 logistics_manufacturing_sop_scan_result；1=PASS，2=NG）
   */
  scanResult: number;

  /**
   * 比对说明
   */
  matchMessage?: string;

  /**
   * 扫描时间
   */
  scannedAt: string;

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

