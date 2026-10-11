// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/defect
// 文件名称：pcbaRepairDetail.types.ts
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
 * PCBA改修明细实体
 * 对应前端 TaktPcbaRepairDetailDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PcbaRepairDetail
 * @description 对应后端 TaktPcbaRepairDetailDto
 */
export interface PcbaRepairDetail extends CompanyDtoBase {
  /**
   * PcbaRepairDetailID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  pcbaRepairDetailId: string;

  /**
   * PCBA改修日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  pcbaRepairId: string;

  /**
   * PCBA改修日报名称（填充字段）
   */
  pcbaRepairName?: string;

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
   * 生产实绩
   */
  prodActualQty: number;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
   */
  TeamCode?: string;

  /**
   * 卡号
   */
  cardCode?: string;

  /**
   * 不良症状
   */
  defectSymptom?: string;

  /**
   * 检出工程（字典 logistics_manufacturing_defect_category，存 DictValue，与组立不良区分共用）
   */
  defectEngineering?: string;

  /**
   * 不良原因
   */
  defectReason?: string;

  /**
   * 不良数量
   */
  defectQty: number;

  /**
   * 责任归属（字典 logistics_manufacturing_defect_responsibility_category，存 DictValue）
   */
  defectResponsibility?: string;

  /**
   * 不良性质（字典 logistics_manufacturing_defect_nature，存 DictValue）
   */
  defectNature?: string;

  /**
   * 修理员（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  repairOperator?: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * PCBA改修日报（主表） （主表：TaktPcbaRepair）
   */
  pcbaRepair?: PcbaRepair;

}


/**
 * PcbaRepairDetail 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PcbaRepairDetailQuery
 * @description 对应后端 TaktPcbaRepairDetailQueryDto
 */
export interface PcbaRepairDetailQuery extends TaktPagedQuery {
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
   * PCBA改修日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  pcbaRepairId?: string;

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
   * 生产实绩
   */
  prodActualQty?: number;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
   */
  TeamCode?: string;

  /**
   * 卡号
   */
  cardCode?: string;

  /**
   * 不良症状
   */
  defectSymptom?: string;

  /**
   * 检出工程（字典 logistics_manufacturing_defect_category，存 DictValue，与组立不良区分共用）
   */
  defectEngineering?: string;

  /**
   * 不良原因
   */
  defectReason?: string;

  /**
   * 不良数量
   */
  defectQty?: number;

  /**
   * 责任归属（字典 logistics_manufacturing_defect_responsibility_category，存 DictValue）
   */
  defectResponsibility?: string;

  /**
   * 不良性质（字典 logistics_manufacturing_defect_nature，存 DictValue）
   */
  defectNature?: string;

  /**
   * 修理员（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  repairOperator?: string;

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
 * 创建PcbaRepairDetail DTO
 * 对应前端 PcbaRepairDetailCreate
 * @description 对应后端 TaktPcbaRepairDetailCreateDto
 */
export interface PcbaRepairDetailCreate {
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
   * PCBA改修日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  pcbaRepairId: string;

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
   * 生产实绩
   */
  prodActualQty: number;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
   */
  TeamCode?: string;

  /**
   * 卡号
   */
  cardCode?: string;

  /**
   * 不良症状
   */
  defectSymptom?: string;

  /**
   * 检出工程（字典 logistics_manufacturing_defect_category，存 DictValue，与组立不良区分共用）
   */
  defectEngineering?: string;

  /**
   * 不良原因
   */
  defectReason?: string;

  /**
   * 不良数量
   */
  defectQty: number;

  /**
   * 责任归属（字典 logistics_manufacturing_defect_responsibility_category，存 DictValue）
   */
  defectResponsibility?: string;

  /**
   * 不良性质（字典 logistics_manufacturing_defect_nature，存 DictValue）
   */
  defectNature?: string;

  /**
   * 修理员（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  repairOperator?: string;

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
 * 更新PcbaRepairDetail DTO
 * 继承 TaktPcbaRepairDetailCreateDto，添加 PcbaRepairDetailId 字段
 * 对应前端 PcbaRepairDetailUpdate
 * @description 对应后端 TaktPcbaRepairDetailUpdateDto
 */
export interface PcbaRepairDetailUpdate extends PcbaRepairDetailCreate {
  /**
   * PcbaRepairDetailID（标识要更新的实体）
   */
  pcbaRepairDetailId: string;

}


/**
 * PcbaRepairDetail 作废/撤销作废 DTO
 * 对应前端 PcbaRepairDetailObsolete
 * @description 对应后端 TaktPcbaRepairDetailObsoleteDto
 */
export interface PcbaRepairDetailObsolete {
  /**
   * PcbaRepairDetailID
   */
  pcbaRepairDetailId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * PcbaRepairDetail 导入模板行 DTO
 * 对应前端 PcbaRepairDetailTemplate
 * @description 对应后端 TaktPcbaRepairDetailTemplateDto
 */
export interface PcbaRepairDetailTemplate {
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
   * PCBA改修日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  pcbaRepairId?: string;

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
   * 生产实绩
   */
  prodActualQty?: number;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
   */
  TeamCode?: string;

  /**
   * 卡号
   */
  cardCode?: string;

  /**
   * 不良症状
   */
  defectSymptom?: string;

  /**
   * 检出工程（字典 logistics_manufacturing_defect_category，存 DictValue，与组立不良区分共用）
   */
  defectEngineering?: string;

  /**
   * 不良原因
   */
  defectReason?: string;

  /**
   * 不良数量
   */
  defectQty?: number;

  /**
   * 责任归属（字典 logistics_manufacturing_defect_responsibility_category，存 DictValue）
   */
  defectResponsibility?: string;

  /**
   * 不良性质（字典 logistics_manufacturing_defect_nature，存 DictValue）
   */
  defectNature?: string;

  /**
   * 修理员（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  repairOperator?: string;

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
 * PcbaRepairDetail 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PcbaRepairDetailImport
 * @description 对应后端 TaktPcbaRepairDetailImportDto
 */
export interface PcbaRepairDetailImport {
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
   * PCBA改修日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  pcbaRepairId?: string;

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
   * 生产实绩
   */
  prodActualQty?: number;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
   */
  TeamCode?: string;

  /**
   * 卡号
   */
  cardCode?: string;

  /**
   * 不良症状
   */
  defectSymptom?: string;

  /**
   * 检出工程（字典 logistics_manufacturing_defect_category，存 DictValue，与组立不良区分共用）
   */
  defectEngineering?: string;

  /**
   * 不良原因
   */
  defectReason?: string;

  /**
   * 不良数量
   */
  defectQty?: number;

  /**
   * 责任归属（字典 logistics_manufacturing_defect_responsibility_category，存 DictValue）
   */
  defectResponsibility?: string;

  /**
   * 不良性质（字典 logistics_manufacturing_defect_nature，存 DictValue）
   */
  defectNature?: string;

  /**
   * 修理员（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  repairOperator?: string;

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
 * PcbaRepairDetail 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PcbaRepairDetailExport
 * @description 对应后端 TaktPcbaRepairDetailExportDto
 */
export interface PcbaRepairDetailExport {
  /**
   * PcbaRepairDetailID
   */
  pcbaRepairDetailId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * PCBA改修日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  pcbaRepairId: string;

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
   * 生产实绩
   */
  prodActualQty: number;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
   */
  TeamCode?: string;

  /**
   * 卡号
   */
  cardCode?: string;

  /**
   * 不良症状
   */
  defectSymptom?: string;

  /**
   * 检出工程（字典 logistics_manufacturing_defect_category，存 DictValue，与组立不良区分共用）
   */
  defectEngineering?: string;

  /**
   * 不良原因
   */
  defectReason?: string;

  /**
   * 不良数量
   */
  defectQty: number;

  /**
   * 责任归属（字典 logistics_manufacturing_defect_responsibility_category，存 DictValue）
   */
  defectResponsibility?: string;

  /**
   * 不良性质（字典 logistics_manufacturing_defect_nature，存 DictValue）
   */
  defectNature?: string;

  /**
   * 修理员（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  repairOperator?: string;

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

