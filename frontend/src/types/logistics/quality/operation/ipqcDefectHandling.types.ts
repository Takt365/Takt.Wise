// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/quality/operation
// 文件名称：ipqcDefectHandling.types.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/operation 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * IPQC制程检验不良处理记录实体
 * 对应前端 TaktIpqcDefectHandlingDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 IpqcDefectHandling
 * @description 对应后端 TaktIpqcDefectHandlingDto
 */
export interface IpqcDefectHandling extends CompanyDtoBase {
  /**
   * IpqcDefectHandlingID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  ipqcDefectHandlingId: string;

  /**
   * IPQC不良处理编码
   */
  ipqcDefectHandlingCode: string;

  /**
   * IPQC检验单明细 ID（关联 TaktIpqcOrderItem.Id，选项 TaktIpqcOrderItems/options）
   */
  ipqcOrderItemId: string;

  /**
   * IPQC检验单明细 名称（填充字段）
   */
  ipqcOrderItemName?: string;

  /**
   * IPQC检验单编码（冗余字段，便于查询）
   */
  ipqcOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 不良类型（0=轻微，1=一般，2=严重，3=致命）
   */
  defectType: number;

  /**
   * 不良现象编码
   */
  defectCode: string;

  /**
   * 不良现象描述
   */
  defectDescription: string;

  /**
   * 不良数量
   */
  defectQuantity: number;

  /**
   * 处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）
   */
  handlingMethod: number;

  /**
   * 处理说明
   */
  handlingDescription?: string;

  /**
   * 责任部门
   */
  responsibleDept?: string;

  /**
   * 责任组编码（选项 TaktQualityGroups/options?qualityGroupCategory=3；DictValue=QualityGroupCode）
   */
  responsibleGroupCode?: string;

  /**
   * 责任组名称（冗余：按 ResponsibleGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
   */
  responsibleGroupName?: string;

  /**
   * 处理组编码（选项 TaktQualityGroups/options?qualityGroupCategory=4；DictValue=QualityGroupCode）
   */
  handlerGroupCode?: string;

  /**
   * 处理组名称（冗余：按 HandlerGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
   */
  handlerGroupName?: string;

  /**
   * 处理时间
   */
  handlingAt?: string;

  /**
   * 验证部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  verifyDeptId?: string;

  /**
   * 验证部门名称（冗余：按 VerifyDeptId 取 TaktDept.DeptName1 联动）
   */
  verifyDeptName?: string;

  /**
   * 质量负责人组编码（选项 TaktQualityGroups/options?qualityGroupCategory=5；DictValue=QualityGroupCode）
   */
  qualityOwnerGroupCode?: string;

  /**
   * 质量负责人组名称（冗余：按 QualityOwnerGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
   */
  qualityOwnerGroupName?: string;

  /**
   * 验证时间
   */
  verifyAt?: string;

  /**
   * 验证结果（字典 logistics_quality_defect_verify_status；0=待验证 1=合格 2=不合格）
   */
  verifyStatus?: number;

  /**
   * 预防措施/纠正措施
   */
  correctiveAction?: string;

  /**
   * 不良图片（JSON格式，存储不良图片URL列表）
   */
  defectImages?: string;

  /**
   * 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
   */
  attachments?: string;

  /**
   * 处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）
   */
  handlingStatus: number;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * IPQC检验单明细（主表） （主表：TaktIpqcOrderItem）
   */
  orderItem?: IpqcOrderItem;

}


/**
 * IpqcDefectHandling 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 IpqcDefectHandlingQuery
 * @description 对应后端 TaktIpqcDefectHandlingQueryDto
 */
export interface IpqcDefectHandlingQuery extends TaktPagedQuery {
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
   * IPQC不良处理编码
   */
  ipqcDefectHandlingCode?: string;

  /**
   * IPQC检验单明细 ID（关联 TaktIpqcOrderItem.Id，选项 TaktIpqcOrderItems/options）
   */
  ipqcOrderItemId?: string;

  /**
   * IPQC检验单编码（冗余字段，便于查询）
   */
  ipqcOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 不良类型（0=轻微，1=一般，2=严重，3=致命）
   */
  defectType?: number;

  /**
   * 不良现象编码
   */
  defectCode?: string;

  /**
   * 不良现象描述
   */
  defectDescription?: string;

  /**
   * 不良数量
   */
  defectQuantity?: number;

  /**
   * 处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）
   */
  handlingMethod?: number;

  /**
   * 处理说明
   */
  handlingDescription?: string;

  /**
   * 责任部门
   */
  responsibleDept?: string;

  /**
   * 责任组编码（选项 TaktQualityGroups/options?qualityGroupCategory=3；DictValue=QualityGroupCode）
   */
  responsibleGroupCode?: string;

  /**
   * 责任组名称（冗余：按 ResponsibleGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
   */
  responsibleGroupName?: string;

  /**
   * 处理组编码（选项 TaktQualityGroups/options?qualityGroupCategory=4；DictValue=QualityGroupCode）
   */
  handlerGroupCode?: string;

  /**
   * 处理组名称（冗余：按 HandlerGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
   */
  handlerGroupName?: string;

  /**
   * 处理时间（范围查询-开始）
   */
  handlingAtStart?: string;

  /**
   * 处理时间（范围查询-结束）
   */
  handlingAtEnd?: string;

  /**
   * 验证部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  verifyDeptId?: string;

  /**
   * 验证部门名称（冗余：按 VerifyDeptId 取 TaktDept.DeptName1 联动）
   */
  verifyDeptName?: string;

  /**
   * 质量负责人组编码（选项 TaktQualityGroups/options?qualityGroupCategory=5；DictValue=QualityGroupCode）
   */
  qualityOwnerGroupCode?: string;

  /**
   * 质量负责人组名称（冗余：按 QualityOwnerGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
   */
  qualityOwnerGroupName?: string;

  /**
   * 验证时间（范围查询-开始）
   */
  verifyAtStart?: string;

  /**
   * 验证时间（范围查询-结束）
   */
  verifyAtEnd?: string;

  /**
   * 验证结果（字典 logistics_quality_defect_verify_status；0=待验证 1=合格 2=不合格）
   */
  verifyStatus?: number;

  /**
   * 预防措施/纠正措施
   */
  correctiveAction?: string;

  /**
   * 不良图片（JSON格式，存储不良图片URL列表）
   */
  defectImages?: string;

  /**
   * 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
   */
  attachments?: string;

  /**
   * 处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）
   */
  handlingStatus?: number;

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
 * 创建IpqcDefectHandling DTO
 * 对应前端 IpqcDefectHandlingCreate
 * @description 对应后端 TaktIpqcDefectHandlingCreateDto
 */
export interface IpqcDefectHandlingCreate {
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
   * IPQC不良处理编码
   */
  ipqcDefectHandlingCode: string;

  /**
   * IPQC检验单明细 ID（关联 TaktIpqcOrderItem.Id，选项 TaktIpqcOrderItems/options）
   */
  ipqcOrderItemId: string;

  /**
   * IPQC检验单编码（冗余字段，便于查询）
   */
  ipqcOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 不良类型（0=轻微，1=一般，2=严重，3=致命）
   */
  defectType: number;

  /**
   * 不良现象编码
   */
  defectCode: string;

  /**
   * 不良现象描述
   */
  defectDescription: string;

  /**
   * 不良数量
   */
  defectQuantity: number;

  /**
   * 处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）
   */
  handlingMethod: number;

  /**
   * 处理说明
   */
  handlingDescription?: string;

  /**
   * 责任部门
   */
  responsibleDept?: string;

  /**
   * 责任组编码（选项 TaktQualityGroups/options?qualityGroupCategory=3；DictValue=QualityGroupCode）
   */
  responsibleGroupCode?: string;

  /**
   * 责任组名称（冗余：按 ResponsibleGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
   */
  responsibleGroupName?: string;

  /**
   * 处理组编码（选项 TaktQualityGroups/options?qualityGroupCategory=4；DictValue=QualityGroupCode）
   */
  handlerGroupCode?: string;

  /**
   * 处理组名称（冗余：按 HandlerGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
   */
  handlerGroupName?: string;

  /**
   * 处理时间
   */
  handlingAt?: string;

  /**
   * 验证部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  verifyDeptId?: string;

  /**
   * 验证部门名称（冗余：按 VerifyDeptId 取 TaktDept.DeptName1 联动）
   */
  verifyDeptName?: string;

  /**
   * 质量负责人组编码（选项 TaktQualityGroups/options?qualityGroupCategory=5；DictValue=QualityGroupCode）
   */
  qualityOwnerGroupCode?: string;

  /**
   * 质量负责人组名称（冗余：按 QualityOwnerGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
   */
  qualityOwnerGroupName?: string;

  /**
   * 验证时间
   */
  verifyAt?: string;

  /**
   * 验证结果（字典 logistics_quality_defect_verify_status；0=待验证 1=合格 2=不合格）
   */
  verifyStatus?: number;

  /**
   * 预防措施/纠正措施
   */
  correctiveAction?: string;

  /**
   * 不良图片（JSON格式，存储不良图片URL列表）
   */
  defectImages?: string;

  /**
   * 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
   */
  attachments?: string;

  /**
   * 处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）
   */
  handlingStatus: number;

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
 * 更新IpqcDefectHandling DTO
 * 继承 TaktIpqcDefectHandlingCreateDto，添加 IpqcDefectHandlingId 字段
 * 对应前端 IpqcDefectHandlingUpdate
 * @description 对应后端 TaktIpqcDefectHandlingUpdateDto
 */
export interface IpqcDefectHandlingUpdate extends IpqcDefectHandlingCreate {
  /**
   * IpqcDefectHandlingID（标识要更新的实体）
   */
  ipqcDefectHandlingId: string;

}


/**
 * IpqcDefectHandling 状态更新 DTO
 * 对应前端 IpqcDefectHandlingStatus
 * @description 对应后端 TaktIpqcDefectHandlingStatusDto
 */
export interface IpqcDefectHandlingStatus {
  /**
   * IpqcDefectHandlingID
   */
  ipqcDefectHandlingId: string;

  /**
   * 处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）
   */
  handlingStatus: number;

}


/**
 * IpqcDefectHandling 作废/撤销作废 DTO
 * 对应前端 IpqcDefectHandlingObsolete
 * @description 对应后端 TaktIpqcDefectHandlingObsoleteDto
 */
export interface IpqcDefectHandlingObsolete {
  /**
   * IpqcDefectHandlingID
   */
  ipqcDefectHandlingId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * IpqcDefectHandling 导入模板行 DTO
 * 对应前端 IpqcDefectHandlingTemplate
 * @description 对应后端 TaktIpqcDefectHandlingTemplateDto
 */
export interface IpqcDefectHandlingTemplate {
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
   * IPQC不良处理编码
   */
  ipqcDefectHandlingCode?: string;

  /**
   * IPQC检验单明细 ID（关联 TaktIpqcOrderItem.Id，选项 TaktIpqcOrderItems/options）
   */
  ipqcOrderItemId?: string;

  /**
   * IPQC检验单编码（冗余字段，便于查询）
   */
  ipqcOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 不良类型（0=轻微，1=一般，2=严重，3=致命）
   */
  defectType?: number;

  /**
   * 不良现象编码
   */
  defectCode?: string;

  /**
   * 不良现象描述
   */
  defectDescription?: string;

  /**
   * 不良数量
   */
  defectQuantity?: number;

  /**
   * 处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）
   */
  handlingMethod?: number;

  /**
   * 处理说明
   */
  handlingDescription?: string;

  /**
   * 责任部门
   */
  responsibleDept?: string;

  /**
   * 责任组编码（选项 TaktQualityGroups/options?qualityGroupCategory=3；DictValue=QualityGroupCode）
   */
  responsibleGroupCode?: string;

  /**
   * 责任组名称（冗余：按 ResponsibleGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
   */
  responsibleGroupName?: string;

  /**
   * 处理组编码（选项 TaktQualityGroups/options?qualityGroupCategory=4；DictValue=QualityGroupCode）
   */
  handlerGroupCode?: string;

  /**
   * 处理组名称（冗余：按 HandlerGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
   */
  handlerGroupName?: string;

  /**
   * 处理时间
   */
  handlingAt?: string;

  /**
   * 验证部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  verifyDeptId?: string;

  /**
   * 验证部门名称（冗余：按 VerifyDeptId 取 TaktDept.DeptName1 联动）
   */
  verifyDeptName?: string;

  /**
   * 质量负责人组编码（选项 TaktQualityGroups/options?qualityGroupCategory=5；DictValue=QualityGroupCode）
   */
  qualityOwnerGroupCode?: string;

  /**
   * 质量负责人组名称（冗余：按 QualityOwnerGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
   */
  qualityOwnerGroupName?: string;

  /**
   * 验证时间
   */
  verifyAt?: string;

  /**
   * 验证结果（字典 logistics_quality_defect_verify_status；0=待验证 1=合格 2=不合格）
   */
  verifyStatus?: number;

  /**
   * 预防措施/纠正措施
   */
  correctiveAction?: string;

  /**
   * 不良图片（JSON格式，存储不良图片URL列表）
   */
  defectImages?: string;

  /**
   * 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
   */
  attachments?: string;

  /**
   * 处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）
   */
  handlingStatus?: number;

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
 * IpqcDefectHandling 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 IpqcDefectHandlingImport
 * @description 对应后端 TaktIpqcDefectHandlingImportDto
 */
export interface IpqcDefectHandlingImport {
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
   * IPQC不良处理编码
   */
  ipqcDefectHandlingCode?: string;

  /**
   * IPQC检验单明细 ID（关联 TaktIpqcOrderItem.Id，选项 TaktIpqcOrderItems/options）
   */
  ipqcOrderItemId?: string;

  /**
   * IPQC检验单编码（冗余字段，便于查询）
   */
  ipqcOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 不良类型（0=轻微，1=一般，2=严重，3=致命）
   */
  defectType?: number;

  /**
   * 不良现象编码
   */
  defectCode?: string;

  /**
   * 不良现象描述
   */
  defectDescription?: string;

  /**
   * 不良数量
   */
  defectQuantity?: number;

  /**
   * 处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）
   */
  handlingMethod?: number;

  /**
   * 处理说明
   */
  handlingDescription?: string;

  /**
   * 责任部门
   */
  responsibleDept?: string;

  /**
   * 责任组编码（选项 TaktQualityGroups/options?qualityGroupCategory=3；DictValue=QualityGroupCode）
   */
  responsibleGroupCode?: string;

  /**
   * 责任组名称（冗余：按 ResponsibleGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
   */
  responsibleGroupName?: string;

  /**
   * 处理组编码（选项 TaktQualityGroups/options?qualityGroupCategory=4；DictValue=QualityGroupCode）
   */
  handlerGroupCode?: string;

  /**
   * 处理组名称（冗余：按 HandlerGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
   */
  handlerGroupName?: string;

  /**
   * 处理时间
   */
  handlingAt?: string;

  /**
   * 验证部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  verifyDeptId?: string;

  /**
   * 验证部门名称（冗余：按 VerifyDeptId 取 TaktDept.DeptName1 联动）
   */
  verifyDeptName?: string;

  /**
   * 质量负责人组编码（选项 TaktQualityGroups/options?qualityGroupCategory=5；DictValue=QualityGroupCode）
   */
  qualityOwnerGroupCode?: string;

  /**
   * 质量负责人组名称（冗余：按 QualityOwnerGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
   */
  qualityOwnerGroupName?: string;

  /**
   * 验证时间
   */
  verifyAt?: string;

  /**
   * 验证结果（字典 logistics_quality_defect_verify_status；0=待验证 1=合格 2=不合格）
   */
  verifyStatus?: number;

  /**
   * 预防措施/纠正措施
   */
  correctiveAction?: string;

  /**
   * 不良图片（JSON格式，存储不良图片URL列表）
   */
  defectImages?: string;

  /**
   * 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
   */
  attachments?: string;

  /**
   * 处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）
   */
  handlingStatus?: number;

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
 * IpqcDefectHandling 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 IpqcDefectHandlingExport
 * @description 对应后端 TaktIpqcDefectHandlingExportDto
 */
export interface IpqcDefectHandlingExport {
  /**
   * IpqcDefectHandlingID
   */
  ipqcDefectHandlingId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * IPQC不良处理编码
   */
  ipqcDefectHandlingCode: string;

  /**
   * IPQC检验单明细 ID（关联 TaktIpqcOrderItem.Id，选项 TaktIpqcOrderItems/options）
   */
  ipqcOrderItemId: string;

  /**
   * IPQC检验单编码（冗余字段，便于查询）
   */
  ipqcOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 不良类型（0=轻微，1=一般，2=严重，3=致命）
   */
  defectType: number;

  /**
   * 不良现象编码
   */
  defectCode: string;

  /**
   * 不良现象描述
   */
  defectDescription: string;

  /**
   * 不良数量
   */
  defectQuantity: number;

  /**
   * 处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）
   */
  handlingMethod: number;

  /**
   * 处理说明
   */
  handlingDescription?: string;

  /**
   * 责任部门
   */
  responsibleDept?: string;

  /**
   * 责任组编码（选项 TaktQualityGroups/options?qualityGroupCategory=3；DictValue=QualityGroupCode）
   */
  responsibleGroupCode?: string;

  /**
   * 责任组名称（冗余：按 ResponsibleGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
   */
  responsibleGroupName?: string;

  /**
   * 处理组编码（选项 TaktQualityGroups/options?qualityGroupCategory=4；DictValue=QualityGroupCode）
   */
  handlerGroupCode?: string;

  /**
   * 处理组名称（冗余：按 HandlerGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
   */
  handlerGroupName?: string;

  /**
   * 处理时间
   */
  handlingAt?: string;

  /**
   * 验证部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  verifyDeptId?: string;

  /**
   * 验证部门名称（冗余：按 VerifyDeptId 取 TaktDept.DeptName1 联动）
   */
  verifyDeptName?: string;

  /**
   * 质量负责人组编码（选项 TaktQualityGroups/options?qualityGroupCategory=5；DictValue=QualityGroupCode）
   */
  qualityOwnerGroupCode?: string;

  /**
   * 质量负责人组名称（冗余：按 QualityOwnerGroupCode 取 TaktQualityGroups.QualityGroupName 联动）
   */
  qualityOwnerGroupName?: string;

  /**
   * 验证时间
   */
  verifyAt?: string;

  /**
   * 验证结果（字典 logistics_quality_defect_verify_status；0=待验证 1=合格 2=不合格）
   */
  verifyStatus?: number;

  /**
   * 预防措施/纠正措施
   */
  correctiveAction?: string;

  /**
   * 不良图片（JSON格式，存储不良图片URL列表）
   */
  defectImages?: string;

  /**
   * 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
   */
  attachments?: string;

  /**
   * 处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）
   */
  handlingStatus: number;

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

