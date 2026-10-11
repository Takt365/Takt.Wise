// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/defect
// 文件名称：pcbaInspectionDetail.types.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/defect 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * PCBA检查明细实体
 * 对应前端 TaktPcbaInspectionDetailDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PcbaInspectionDetail
 * @description 对应后端 TaktPcbaInspectionDetailDto
 */
export interface PcbaInspectionDetail extends CompanyDtoBase {
  /**
   * PcbaInspectionDetailID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  pcbaInspectionDetailId: string;

  /**
   * PCBA检查日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  pcbaInspectionId: string;

  /**
   * PCBA检查日报名称（填充字段）
   */
  pcbaInspectionName?: string;

  /**
   * 工单号（冗余字段,便于查询）
   */
  prodOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * PCBA板别（字典 logistics_manufacturing_pcba_function，存 DictValue）
   */
  pcbaBoardType?: string;

  /**
   * 目视线别（字典 logistics_manufacturing_visual_inspection_line_category，存 DictValue）
   */
  visualInspectionLine?: string;

  /**
   * AOI线别（字典 logistics_manufacturing_aoi_inspection_line_category，存 DictValue）
   */
  aoiLine?: string;

  /**
   * B面实装日期
   */
  bSideAssemblyDate?: string;

  /**
   * T面实装日期
   */
  tSideAssemblyDate?: string;

  /**
   * 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo: number;

  /**
   * 检查员（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  inspectorName?: string;

  /**
   * 当日完成数量
   */
  dailyCompletedQty: number;

  /**
   * 检查数量
   */
  inspectionQty: number;

  /**
   * 检查状态（字典 logistics_manufacturing_pcba_inspection_status；1=检查中 2=测试中 3=检查完成 4=测试完成）
   */
  inspectionStatus: number;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
   */
  TeamCode?: string;

  /**
   * 检查工数
   */
  inspectionWorkHours: number;

  /**
   * AOI工数
   */
  aoiWorkHours: number;

  /**
   * 不良数量
   */
  defectQty: number;

  /**
   * 手贴
   */
  handPlacement?: string;

  /**
   * 流水号
   */
  serialNumber?: string;

  /**
   * 内容
   */
  content?: string;

  /**
   * 不良个所（字典 logistics_manufacturing_pcb_location_category，存 DictValue）
   */
  defectLocation?: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * PCBA检查日报（主表） （主表：TaktPcbaInspection）
   */
  pcbaInspection?: PcbaInspection;

}


/**
 * PcbaInspectionDetail 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PcbaInspectionDetailQuery
 * @description 对应后端 TaktPcbaInspectionDetailQueryDto
 */
export interface PcbaInspectionDetailQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
   */
  plantCode?: string;

  /**
   * PCBA检查日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  pcbaInspectionId?: string;

  /**
   * 工单号（冗余字段,便于查询）
   */
  prodOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * PCBA板别（字典 logistics_manufacturing_pcba_function，存 DictValue）
   */
  pcbaBoardType?: string;

  /**
   * 目视线别（字典 logistics_manufacturing_visual_inspection_line_category，存 DictValue）
   */
  visualInspectionLine?: string;

  /**
   * AOI线别（字典 logistics_manufacturing_aoi_inspection_line_category，存 DictValue）
   */
  aoiLine?: string;

  /**
   * B面实装日期（范围查询-开始）
   */
  bSideAssemblyDateStart?: string;

  /**
   * B面实装日期（范围查询-结束）
   */
  bSideAssemblyDateEnd?: string;

  /**
   * T面实装日期（范围查询-开始）
   */
  tSideAssemblyDateStart?: string;

  /**
   * T面实装日期（范围查询-结束）
   */
  tSideAssemblyDateEnd?: string;

  /**
   * 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo?: number;

  /**
   * 检查员（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  inspectorName?: string;

  /**
   * 当日完成数量
   */
  dailyCompletedQty?: number;

  /**
   * 检查数量
   */
  inspectionQty?: number;

  /**
   * 检查状态（字典 logistics_manufacturing_pcba_inspection_status；1=检查中 2=测试中 3=检查完成 4=测试完成）
   */
  inspectionStatus?: number;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
   */
  TeamCode?: string;

  /**
   * 检查工数
   */
  inspectionWorkHours?: number;

  /**
   * AOI工数
   */
  aoiWorkHours?: number;

  /**
   * 不良数量
   */
  defectQty?: number;

  /**
   * 手贴
   */
  handPlacement?: string;

  /**
   * 流水号
   */
  serialNumber?: string;

  /**
   * 内容
   */
  content?: string;

  /**
   * 不良个所（字典 logistics_manufacturing_pcb_location_category，存 DictValue）
   */
  defectLocation?: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
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
 * 创建PcbaInspectionDetail DTO
 * 对应前端 PcbaInspectionDetailCreate
 * @description 对应后端 TaktPcbaInspectionDetailCreateDto
 */
export interface PcbaInspectionDetailCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
   */
  plantCode: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * PCBA检查日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  pcbaInspectionId: string;

  /**
   * 工单号（冗余字段,便于查询）
   */
  prodOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * PCBA板别（字典 logistics_manufacturing_pcba_function，存 DictValue）
   */
  pcbaBoardType?: string;

  /**
   * 目视线别（字典 logistics_manufacturing_visual_inspection_line_category，存 DictValue）
   */
  visualInspectionLine?: string;

  /**
   * AOI线别（字典 logistics_manufacturing_aoi_inspection_line_category，存 DictValue）
   */
  aoiLine?: string;

  /**
   * B面实装日期
   */
  bSideAssemblyDate?: string;

  /**
   * T面实装日期
   */
  tSideAssemblyDate?: string;

  /**
   * 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo: number;

  /**
   * 检查员（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  inspectorName?: string;

  /**
   * 当日完成数量
   */
  dailyCompletedQty: number;

  /**
   * 检查数量
   */
  inspectionQty: number;

  /**
   * 检查状态（字典 logistics_manufacturing_pcba_inspection_status；1=检查中 2=测试中 3=检查完成 4=测试完成）
   */
  inspectionStatus: number;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
   */
  TeamCode?: string;

  /**
   * 检查工数
   */
  inspectionWorkHours: number;

  /**
   * AOI工数
   */
  aoiWorkHours: number;

  /**
   * 不良数量
   */
  defectQty: number;

  /**
   * 手贴
   */
  handPlacement?: string;

  /**
   * 流水号
   */
  serialNumber?: string;

  /**
   * 内容
   */
  content?: string;

  /**
   * 不良个所（字典 logistics_manufacturing_pcb_location_category，存 DictValue）
   */
  defectLocation?: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
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
 * 更新PcbaInspectionDetail DTO
 * 继承 TaktPcbaInspectionDetailCreateDto，添加 PcbaInspectionDetailId 字段
 * 对应前端 PcbaInspectionDetailUpdate
 * @description 对应后端 TaktPcbaInspectionDetailUpdateDto
 */
export interface PcbaInspectionDetailUpdate extends PcbaInspectionDetailCreate {
  /**
   * PcbaInspectionDetailID（标识要更新的实体）
   */
  pcbaInspectionDetailId: string;

}


/**
 * PcbaInspectionDetail 状态更新 DTO
 * 对应前端 PcbaInspectionDetailStatus
 * @description 对应后端 TaktPcbaInspectionDetailStatusDto
 */
export interface PcbaInspectionDetailStatus {
  /**
   * PcbaInspectionDetailID
   */
  pcbaInspectionDetailId: string;

  /**
   * 检查状态（字典 logistics_manufacturing_pcba_inspection_status；1=检查中 2=测试中 3=检查完成 4=测试完成）
   */
  inspectionStatus: number;

}


/**
 * PcbaInspectionDetail 作废/撤销作废 DTO
 * 对应前端 PcbaInspectionDetailObsolete
 * @description 对应后端 TaktPcbaInspectionDetailObsoleteDto
 */
export interface PcbaInspectionDetailObsolete {
  /**
   * PcbaInspectionDetailID
   */
  pcbaInspectionDetailId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * PcbaInspectionDetail 导入模板行 DTO
 * 对应前端 PcbaInspectionDetailTemplate
 * @description 对应后端 TaktPcbaInspectionDetailTemplateDto
 */
export interface PcbaInspectionDetailTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
   */
  plantCode?: string;

  /**
   * PCBA检查日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  pcbaInspectionId?: string;

  /**
   * 工单号（冗余字段,便于查询）
   */
  prodOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * PCBA板别（字典 logistics_manufacturing_pcba_function，存 DictValue）
   */
  pcbaBoardType?: string;

  /**
   * 目视线别（字典 logistics_manufacturing_visual_inspection_line_category，存 DictValue）
   */
  visualInspectionLine?: string;

  /**
   * AOI线别（字典 logistics_manufacturing_aoi_inspection_line_category，存 DictValue）
   */
  aoiLine?: string;

  /**
   * B面实装日期
   */
  bSideAssemblyDate?: string;

  /**
   * T面实装日期
   */
  tSideAssemblyDate?: string;

  /**
   * 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo?: number;

  /**
   * 检查员（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  inspectorName?: string;

  /**
   * 当日完成数量
   */
  dailyCompletedQty?: number;

  /**
   * 检查数量
   */
  inspectionQty?: number;

  /**
   * 检查状态（字典 logistics_manufacturing_pcba_inspection_status；1=检查中 2=测试中 3=检查完成 4=测试完成）
   */
  inspectionStatus?: number;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
   */
  TeamCode?: string;

  /**
   * 检查工数
   */
  inspectionWorkHours?: number;

  /**
   * AOI工数
   */
  aoiWorkHours?: number;

  /**
   * 不良数量
   */
  defectQty?: number;

  /**
   * 手贴
   */
  handPlacement?: string;

  /**
   * 流水号
   */
  serialNumber?: string;

  /**
   * 内容
   */
  content?: string;

  /**
   * 不良个所（字典 logistics_manufacturing_pcb_location_category，存 DictValue）
   */
  defectLocation?: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
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
 * PcbaInspectionDetail 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PcbaInspectionDetailImport
 * @description 对应后端 TaktPcbaInspectionDetailImportDto
 */
export interface PcbaInspectionDetailImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
   */
  plantCode?: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * PCBA检查日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  pcbaInspectionId?: string;

  /**
   * 工单号（冗余字段,便于查询）
   */
  prodOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * PCBA板别（字典 logistics_manufacturing_pcba_function，存 DictValue）
   */
  pcbaBoardType?: string;

  /**
   * 目视线别（字典 logistics_manufacturing_visual_inspection_line_category，存 DictValue）
   */
  visualInspectionLine?: string;

  /**
   * AOI线别（字典 logistics_manufacturing_aoi_inspection_line_category，存 DictValue）
   */
  aoiLine?: string;

  /**
   * B面实装日期
   */
  bSideAssemblyDate?: string;

  /**
   * T面实装日期
   */
  tSideAssemblyDate?: string;

  /**
   * 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo?: number;

  /**
   * 检查员（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  inspectorName?: string;

  /**
   * 当日完成数量
   */
  dailyCompletedQty?: number;

  /**
   * 检查数量
   */
  inspectionQty?: number;

  /**
   * 检查状态（字典 logistics_manufacturing_pcba_inspection_status；1=检查中 2=测试中 3=检查完成 4=测试完成）
   */
  inspectionStatus?: number;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
   */
  TeamCode?: string;

  /**
   * 检查工数
   */
  inspectionWorkHours?: number;

  /**
   * AOI工数
   */
  aoiWorkHours?: number;

  /**
   * 不良数量
   */
  defectQty?: number;

  /**
   * 手贴
   */
  handPlacement?: string;

  /**
   * 流水号
   */
  serialNumber?: string;

  /**
   * 内容
   */
  content?: string;

  /**
   * 不良个所（字典 logistics_manufacturing_pcb_location_category，存 DictValue）
   */
  defectLocation?: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
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
 * PcbaInspectionDetail 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PcbaInspectionDetailExport
 * @description 对应后端 TaktPcbaInspectionDetailExportDto
 */
export interface PcbaInspectionDetailExport {
  /**
   * PcbaInspectionDetailID
   */
  pcbaInspectionDetailId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * PCBA检查日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  pcbaInspectionId: string;

  /**
   * 工单号（冗余字段,便于查询）
   */
  prodOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * PCBA板别（字典 logistics_manufacturing_pcba_function，存 DictValue）
   */
  pcbaBoardType?: string;

  /**
   * 目视线别（字典 logistics_manufacturing_visual_inspection_line_category，存 DictValue）
   */
  visualInspectionLine?: string;

  /**
   * AOI线别（字典 logistics_manufacturing_aoi_inspection_line_category，存 DictValue）
   */
  aoiLine?: string;

  /**
   * B面实装日期
   */
  bSideAssemblyDate?: string;

  /**
   * T面实装日期
   */
  tSideAssemblyDate?: string;

  /**
   * 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo: number;

  /**
   * 检查员（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  inspectorName?: string;

  /**
   * 当日完成数量
   */
  dailyCompletedQty: number;

  /**
   * 检查数量
   */
  inspectionQty: number;

  /**
   * 检查状态（字典 logistics_manufacturing_pcba_inspection_status；1=检查中 2=测试中 3=检查完成 4=测试完成）
   */
  inspectionStatus: number;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
   */
  TeamCode?: string;

  /**
   * 检查工数
   */
  inspectionWorkHours: number;

  /**
   * AOI工数
   */
  aoiWorkHours: number;

  /**
   * 不良数量
   */
  defectQty: number;

  /**
   * 手贴
   */
  handPlacement?: string;

  /**
   * 流水号
   */
  serialNumber?: string;

  /**
   * 内容
   */
  content?: string;

  /**
   * 不良个所（字典 logistics_manufacturing_pcb_location_category，存 DictValue）
   */
  defectLocation?: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
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

