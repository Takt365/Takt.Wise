// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/output
// 文件名称：assyOutputDetail.types.ts
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/output 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 组立日报明细（产出子表）实体
 * 对应前端 TaktAssyOutputDetailDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 AssyOutputDetail
 * @description 对应后端 TaktAssyOutputDetailDto
 */
export interface AssyOutputDetail extends CompanyDtoBase {
  /**
   * AssyOutputDetailID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  assyOutputDetailId: string;

  /**
   * 组立日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  assyOutputId: string;

  /**
   * 组立日报名称（填充字段）
   */
  assyOutputName?: string;

  /**
   * 工单号（冗余字段,便于查询）
   */
  prodOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 生产时段（固定值）
   */
  timePeriod: string;

  /**
   * 标准产能（冗余字段：默认快照主表 StdCapacity；有报工工时时按报工工时÷标准工时×稼动率重算该行）
   */
  stdCapacity: number;

  /**
   * 实际生产数量
   */
  prodActualQty: number;

  /**
   * 停线时间(分钟)
   */
  downtimeMinutes: number;

  /**
   * 停线原因（多选 DictLabel 逗号分隔；UI 提交由前端 dict-type 转换）
   */
  downtimeReason?: string;

  /**
   * 停线说明
   */
  downtimeDescription?: string;

  /**
   * 未达成原因（多选 DictLabel 逗号分隔；UI 提交由前端 dict-type 转换）
   */
  unachievedReason?: string;

  /**
   * 未达成说明
   */
  unachievedDescription?: string;

  /**
   * 投入工时(分钟)（计算结果：无产量且无报工时为 0；报工工时大于 0 时等于报工工时，否则为人数×60）
   */
  inputMinutes: number;

  /**
   * 实际工时(分钟)（计算结果：无产量且无报工时为 0；报工工时大于 0 时为报工工时减停线时间，否则为投入工时减停线时间；有产量时不小于 0）
   */
  actualMinutes: number;

  /**
   * 间接工时(分钟)（计算结果：无产量且无报工时为 0；否则为间接人数×向下取整(实际工时÷直接人数)）
   */
  indirectMinutes: number;

  /**
   * 报工工时(分钟)（填写场景：1 同一时段混合生产；2 清机；3 无产出、欠料、仪设、切换机种等需记录损失时间）
   */
  confirmMinutes: number;

  /**
   * 混合生产（0=非混合；N≥2 表示同班组同日期同生产时段内共有 N 笔有产量/报工）
   */
  mixedProd: number;

  /**
   * 达成率(%)（计算结果：实际生产数量÷StdCapacity×100%；标准产能为0时取0）
   */
  achievementRate: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 组立日报（主表） （主表：TaktAssyOutput）
   */
  assyOutput?: AssyOutput;

}


/**
 * AssyOutputDetail 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 AssyOutputDetailQuery
 * @description 对应后端 TaktAssyOutputDetailQueryDto
 */
export interface AssyOutputDetailQuery extends TaktPagedQuery {
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
   * 组立日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  assyOutputId?: string;

  /**
   * 工单号（冗余字段,便于查询）
   */
  prodOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 生产时段（固定值）
   */
  timePeriod?: string;

  /**
   * 标准产能（冗余字段：默认快照主表 StdCapacity；有报工工时时按报工工时÷标准工时×稼动率重算该行）
   */
  stdCapacity?: number;

  /**
   * 实际生产数量
   */
  prodActualQty?: number;

  /**
   * 停线时间(分钟)
   */
  downtimeMinutes?: number;

  /**
   * 停线原因（多选 DictLabel 逗号分隔；UI 提交由前端 dict-type 转换）
   */
  downtimeReason?: string;

  /**
   * 停线说明
   */
  downtimeDescription?: string;

  /**
   * 未达成原因（多选 DictLabel 逗号分隔；UI 提交由前端 dict-type 转换）
   */
  unachievedReason?: string;

  /**
   * 未达成说明
   */
  unachievedDescription?: string;

  /**
   * 投入工时(分钟)（计算结果：无产量且无报工时为 0；报工工时大于 0 时等于报工工时，否则为人数×60）
   */
  inputMinutes?: number;

  /**
   * 实际工时(分钟)（计算结果：无产量且无报工时为 0；报工工时大于 0 时为报工工时减停线时间，否则为投入工时减停线时间；有产量时不小于 0）
   */
  actualMinutes?: number;

  /**
   * 间接工时(分钟)（计算结果：无产量且无报工时为 0；否则为间接人数×向下取整(实际工时÷直接人数)）
   */
  indirectMinutes?: number;

  /**
   * 报工工时(分钟)（填写场景：1 同一时段混合生产；2 清机；3 无产出、欠料、仪设、切换机种等需记录损失时间）
   */
  confirmMinutes?: number;

  /**
   * 混合生产（0=非混合；N≥2 表示同班组同日期同生产时段内共有 N 笔有产量/报工）
   */
  mixedProd?: number;

  /**
   * 达成率(%)（计算结果：实际生产数量÷StdCapacity×100%；标准产能为0时取0）
   */
  achievementRate?: number;

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
 * 创建AssyOutputDetail DTO
 * 对应前端 AssyOutputDetailCreate
 * @description 对应后端 TaktAssyOutputDetailCreateDto
 */
export interface AssyOutputDetailCreate {
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
   * 组立日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  assyOutputId: string;

  /**
   * 工单号（冗余字段,便于查询）
   */
  prodOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 生产时段（固定值）
   */
  timePeriod: string;

  /**
   * 标准产能（冗余字段：默认快照主表 StdCapacity；有报工工时时按报工工时÷标准工时×稼动率重算该行）
   */
  stdCapacity: number;

  /**
   * 实际生产数量
   */
  prodActualQty: number;

  /**
   * 停线时间(分钟)
   */
  downtimeMinutes: number;

  /**
   * 停线原因（多选 DictLabel 逗号分隔；UI 提交由前端 dict-type 转换）
   */
  downtimeReason?: string;

  /**
   * 停线说明
   */
  downtimeDescription?: string;

  /**
   * 未达成原因（多选 DictLabel 逗号分隔；UI 提交由前端 dict-type 转换）
   */
  unachievedReason?: string;

  /**
   * 未达成说明
   */
  unachievedDescription?: string;

  /**
   * 投入工时(分钟)（计算结果：无产量且无报工时为 0；报工工时大于 0 时等于报工工时，否则为人数×60）
   */
  inputMinutes: number;

  /**
   * 实际工时(分钟)（计算结果：无产量且无报工时为 0；报工工时大于 0 时为报工工时减停线时间，否则为投入工时减停线时间；有产量时不小于 0）
   */
  actualMinutes: number;

  /**
   * 间接工时(分钟)（计算结果：无产量且无报工时为 0；否则为间接人数×向下取整(实际工时÷直接人数)）
   */
  indirectMinutes: number;

  /**
   * 报工工时(分钟)（填写场景：1 同一时段混合生产；2 清机；3 无产出、欠料、仪设、切换机种等需记录损失时间）
   */
  confirmMinutes: number;

  /**
   * 混合生产（0=非混合；N≥2 表示同班组同日期同生产时段内共有 N 笔有产量/报工）
   */
  mixedProd: number;

  /**
   * 达成率(%)（计算结果：实际生产数量÷StdCapacity×100%；标准产能为0时取0）
   */
  achievementRate: number;

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
 * 更新AssyOutputDetail DTO
 * 继承 TaktAssyOutputDetailCreateDto，添加 AssyOutputDetailId 字段
 * 对应前端 AssyOutputDetailUpdate
 * @description 对应后端 TaktAssyOutputDetailUpdateDto
 */
export interface AssyOutputDetailUpdate extends AssyOutputDetailCreate {
  /**
   * AssyOutputDetailID（标识要更新的实体）
   */
  assyOutputDetailId: string;

}


/**
 * AssyOutputDetail 作废/撤销作废 DTO
 * 对应前端 AssyOutputDetailObsolete
 * @description 对应后端 TaktAssyOutputDetailObsoleteDto
 */
export interface AssyOutputDetailObsolete {
  /**
   * AssyOutputDetailID
   */
  assyOutputDetailId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * AssyOutputDetail 导入模板行 DTO
 * 对应前端 AssyOutputDetailTemplate
 * @description 对应后端 TaktAssyOutputDetailTemplateDto
 */
export interface AssyOutputDetailTemplate {
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
   * 组立日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  assyOutputId?: string;

  /**
   * 工单号（冗余字段,便于查询）
   */
  prodOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 生产时段（固定值）
   */
  timePeriod?: string;

  /**
   * 标准产能（冗余字段：默认快照主表 StdCapacity；有报工工时时按报工工时÷标准工时×稼动率重算该行）
   */
  stdCapacity?: number;

  /**
   * 实际生产数量
   */
  prodActualQty?: number;

  /**
   * 停线时间(分钟)
   */
  downtimeMinutes?: number;

  /**
   * 停线原因（多选 DictLabel 逗号分隔；UI 提交由前端 dict-type 转换）
   */
  downtimeReason?: string;

  /**
   * 停线说明
   */
  downtimeDescription?: string;

  /**
   * 未达成原因（多选 DictLabel 逗号分隔；UI 提交由前端 dict-type 转换）
   */
  unachievedReason?: string;

  /**
   * 未达成说明
   */
  unachievedDescription?: string;

  /**
   * 投入工时(分钟)（计算结果：无产量且无报工时为 0；报工工时大于 0 时等于报工工时，否则为人数×60）
   */
  inputMinutes?: number;

  /**
   * 实际工时(分钟)（计算结果：无产量且无报工时为 0；报工工时大于 0 时为报工工时减停线时间，否则为投入工时减停线时间；有产量时不小于 0）
   */
  actualMinutes?: number;

  /**
   * 间接工时(分钟)（计算结果：无产量且无报工时为 0；否则为间接人数×向下取整(实际工时÷直接人数)）
   */
  indirectMinutes?: number;

  /**
   * 报工工时(分钟)（填写场景：1 同一时段混合生产；2 清机；3 无产出、欠料、仪设、切换机种等需记录损失时间）
   */
  confirmMinutes?: number;

  /**
   * 混合生产（0=非混合；N≥2 表示同班组同日期同生产时段内共有 N 笔有产量/报工）
   */
  mixedProd?: number;

  /**
   * 达成率(%)（计算结果：实际生产数量÷StdCapacity×100%；标准产能为0时取0）
   */
  achievementRate?: number;

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
 * AssyOutputDetail 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 AssyOutputDetailImport
 * @description 对应后端 TaktAssyOutputDetailImportDto
 */
export interface AssyOutputDetailImport {
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
   * 组立日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  assyOutputId?: string;

  /**
   * 工单号（冗余字段,便于查询）
   */
  prodOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 生产时段（固定值）
   */
  timePeriod?: string;

  /**
   * 标准产能（冗余字段：默认快照主表 StdCapacity；有报工工时时按报工工时÷标准工时×稼动率重算该行）
   */
  stdCapacity?: number;

  /**
   * 实际生产数量
   */
  prodActualQty?: number;

  /**
   * 停线时间(分钟)
   */
  downtimeMinutes?: number;

  /**
   * 停线原因（多选 DictLabel 逗号分隔；UI 提交由前端 dict-type 转换）
   */
  downtimeReason?: string;

  /**
   * 停线说明
   */
  downtimeDescription?: string;

  /**
   * 未达成原因（多选 DictLabel 逗号分隔；UI 提交由前端 dict-type 转换）
   */
  unachievedReason?: string;

  /**
   * 未达成说明
   */
  unachievedDescription?: string;

  /**
   * 投入工时(分钟)（计算结果：无产量且无报工时为 0；报工工时大于 0 时等于报工工时，否则为人数×60）
   */
  inputMinutes?: number;

  /**
   * 实际工时(分钟)（计算结果：无产量且无报工时为 0；报工工时大于 0 时为报工工时减停线时间，否则为投入工时减停线时间；有产量时不小于 0）
   */
  actualMinutes?: number;

  /**
   * 间接工时(分钟)（计算结果：无产量且无报工时为 0；否则为间接人数×向下取整(实际工时÷直接人数)）
   */
  indirectMinutes?: number;

  /**
   * 报工工时(分钟)（填写场景：1 同一时段混合生产；2 清机；3 无产出、欠料、仪设、切换机种等需记录损失时间）
   */
  confirmMinutes?: number;

  /**
   * 混合生产（0=非混合；N≥2 表示同班组同日期同生产时段内共有 N 笔有产量/报工）
   */
  mixedProd?: number;

  /**
   * 达成率(%)（计算结果：实际生产数量÷StdCapacity×100%；标准产能为0时取0）
   */
  achievementRate?: number;

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
 * AssyOutputDetail 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 AssyOutputDetailExport
 * @description 对应后端 TaktAssyOutputDetailExportDto
 */
export interface AssyOutputDetailExport {
  /**
   * AssyOutputDetailID
   */
  assyOutputDetailId: string;

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
   * 组立日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  assyOutputId: string;

  /**
   * 工单号（冗余字段,便于查询）
   */
  prodOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 生产时段（固定值）
   */
  timePeriod: string;

  /**
   * 标准产能（冗余字段：默认快照主表 StdCapacity；有报工工时时按报工工时÷标准工时×稼动率重算该行）
   */
  stdCapacity: number;

  /**
   * 实际生产数量
   */
  prodActualQty: number;

  /**
   * 停线时间(分钟)
   */
  downtimeMinutes: number;

  /**
   * 停线原因（多选 DictLabel 逗号分隔；UI 提交由前端 dict-type 转换）
   */
  downtimeReason?: string;

  /**
   * 停线说明
   */
  downtimeDescription?: string;

  /**
   * 未达成原因（多选 DictLabel 逗号分隔；UI 提交由前端 dict-type 转换）
   */
  unachievedReason?: string;

  /**
   * 未达成说明
   */
  unachievedDescription?: string;

  /**
   * 投入工时(分钟)（计算结果：无产量且无报工时为 0；报工工时大于 0 时等于报工工时，否则为人数×60）
   */
  inputMinutes: number;

  /**
   * 实际工时(分钟)（计算结果：无产量且无报工时为 0；报工工时大于 0 时为报工工时减停线时间，否则为投入工时减停线时间；有产量时不小于 0）
   */
  actualMinutes: number;

  /**
   * 间接工时(分钟)（计算结果：无产量且无报工时为 0；否则为间接人数×向下取整(实际工时÷直接人数)）
   */
  indirectMinutes: number;

  /**
   * 报工工时(分钟)（填写场景：1 同一时段混合生产；2 清机；3 无产出、欠料、仪设、切换机种等需记录损失时间）
   */
  confirmMinutes: number;

  /**
   * 混合生产（0=非混合；N≥2 表示同班组同日期同生产时段内共有 N 笔有产量/报工）
   */
  mixedProd: number;

  /**
   * 达成率(%)（计算结果：实际生产数量÷StdCapacity×100%；标准产能为0时取0）
   */
  achievementRate: number;

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

