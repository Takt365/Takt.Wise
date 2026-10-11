// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/accounting/controlling
// 文件名称：costCenter.types.ts
// 创建时间：2026-08-31
// 创建人：Takt365(Auto Generated)
// 功能描述：accounting/controlling 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 成本中心实体
 * 对应前端 TaktCostCenterDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 CostCenter
 * @description 对应后端 TaktCostCenterDto
 */
export interface CostCenter extends CompanyDtoBase {
  /**
   * CostCenterID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  costCenterId: string;

  /**
   * 成本中心编码（6 位，与部门编码同长；租户+公司内唯一）
   */
  costCenterCode: string;

  /**
   * 成本中心名称
   */
  costCenterName: string;

  /**
   * 父级 ID（0 表示根节点）
   */
  parentId: string;

  /**
   * 成本中心类型（字典 accounting_controlling_cost_center_type；DictValue=F/G/H/L/S）
   */
  costCenterType: string;

  /**
   * 负责人用户 ID（选项 TaktUsers/options，DictValue=Id）
   */
  managerId?: string;

  /**
   * 负责人姓名（冗余：按 ManagerId 取 TaktUser.NickName联动）
   */
  managerName?: string;

  /**
   * 所属部门（选项 TaktDepts/tree-options,DictValue=Id）
   */
  deptId?: string;

  /**
   * 所属部门名称（冗余：按 DeptId 取 TaktDept.DeptName联动）
   */
  deptName?: string;

  /**
   * 成本中心层级
   */
  costCenterLevel: number;

  /**
   * 生效日期
   */
  validFrom: string;

  /**
   * 失效日期
   */
  validTo: string;

  /**
   * 排序号（回填）
   */
  sortOrder: number;

  /**
   * 成本中心状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  costCenterStatus: number;

}


/**
 * CostCenter 树形列表/树选择 DTO（含子节点）
 * 对应 GetCostCenterTreeAsync 等接口
 * 对应前端 CostCenterTree
 * @description 对应后端 TaktCostCenterTreeDto
 */
export interface CostCenterTree extends CostCenter {
  /**
   * 子节点（懒加载树接口返回 null，表示尚未加载；勿用空 List 冒充已加载）
   */
  children?: CostCenterTree[];

}


/**
 * CostCenter 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 CostCenterQuery
 * @description 对应后端 TaktCostCenterQueryDto
 */
export interface CostCenterQuery extends TaktPagedQuery {
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
   * 成本中心编码（6 位，与部门编码同长；租户+公司内唯一）
   */
  costCenterCode?: string;

  /**
   * 成本中心名称
   */
  costCenterName?: string;

  /**
   * 父级 ID（0 表示根节点）
   */
  parentId?: string;

  /**
   * 成本中心类型（字典 accounting_controlling_cost_center_type；DictValue=F/G/H/L/S）
   */
  costCenterType?: string;

  /**
   * 负责人用户 ID（选项 TaktUsers/options，DictValue=Id）
   */
  managerId?: string;

  /**
   * 负责人姓名（冗余：按 ManagerId 取 TaktUser.NickName联动）
   */
  managerName?: string;

  /**
   * 所属部门（选项 TaktDepts/tree-options,DictValue=Id）
   */
  deptId?: string;

  /**
   * 所属部门名称（冗余：按 DeptId 取 TaktDept.DeptName联动）
   */
  deptName?: string;

  /**
   * 成本中心层级
   */
  costCenterLevel?: number;

  /**
   * 生效日期（范围查询-开始）
   */
  validFromStart?: string;

  /**
   * 生效日期（范围查询-结束）
   */
  validFromEnd?: string;

  /**
   * 失效日期（范围查询-开始）
   */
  validToStart?: string;

  /**
   * 失效日期（范围查询-结束）
   */
  validToEnd?: string;

  /**
   * 排序号（回填）
   */
  sortOrder?: number;

  /**
   * 成本中心状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  costCenterStatus?: number;

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
 * 创建CostCenter DTO
 * 对应前端 CostCenterCreate
 * @description 对应后端 TaktCostCenterCreateDto
 */
export interface CostCenterCreate {
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
   * 成本中心编码（6 位，与部门编码同长；租户+公司内唯一）
   */
  costCenterCode: string;

  /**
   * 成本中心名称
   */
  costCenterName: string;

  /**
   * 父级 ID（0 表示根节点）
   */
  parentId: string;

  /**
   * 成本中心类型（字典 accounting_controlling_cost_center_type；DictValue=F/G/H/L/S）
   */
  costCenterType: string;

  /**
   * 负责人用户 ID（选项 TaktUsers/options，DictValue=Id）
   */
  managerId?: string;

  /**
   * 负责人姓名（冗余：按 ManagerId 取 TaktUser.NickName联动）
   */
  managerName?: string;

  /**
   * 所属部门（选项 TaktDepts/tree-options,DictValue=Id）
   */
  deptId?: string;

  /**
   * 所属部门名称（冗余：按 DeptId 取 TaktDept.DeptName联动）
   */
  deptName?: string;

  /**
   * 成本中心层级
   */
  costCenterLevel: number;

  /**
   * 生效日期
   */
  validFrom: string;

  /**
   * 失效日期
   */
  validTo: string;

  /**
   * 成本中心状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  costCenterStatus: number;

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
 * 更新CostCenter DTO
 * 继承 TaktCostCenterCreateDto，添加 CostCenterId 字段
 * 对应前端 CostCenterUpdate
 * @description 对应后端 TaktCostCenterUpdateDto
 */
export interface CostCenterUpdate extends CostCenterCreate {
  /**
   * CostCenterID（标识要更新的实体）
   */
  costCenterId: string;

}


/**
 * CostCenter 状态更新 DTO
 * 对应前端 CostCenterStatus
 * @description 对应后端 TaktCostCenterStatusDto
 */
export interface CostCenterStatus {
  /**
   * CostCenterID
   */
  costCenterId: string;

  /**
   * 成本中心状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  costCenterStatus: number;

}


/**
 * CostCenter 排序更新 DTO
 * 对应前端 CostCenterSort
 * @description 对应后端 TaktCostCenterSortDto
 */
export interface CostCenterSort {
  /**
   * CostCenterID
   */
  costCenterId: string;

  /**
   * 排序号（回填）
   */
  sortOrder: number;

}


/**
 * CostCenter 导入模板行 DTO
 * 对应前端 CostCenterTemplate
 * @description 对应后端 TaktCostCenterTemplateDto
 */
export interface CostCenterTemplate {
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
   * 成本中心编码（6 位，与部门编码同长；租户+公司内唯一）
   */
  costCenterCode?: string;

  /**
   * 成本中心名称
   */
  costCenterName?: string;

  /**
   * 父级 ID（0 表示根节点）
   */
  parentId?: string;

  /**
   * 成本中心类型（字典 accounting_controlling_cost_center_type；DictValue=F/G/H/L/S）
   */
  costCenterType?: string;

  /**
   * 负责人用户 ID（选项 TaktUsers/options，DictValue=Id）
   */
  managerId?: string;

  /**
   * 负责人姓名（冗余：按 ManagerId 取 TaktUser.NickName联动）
   */
  managerName?: string;

  /**
   * 所属部门（选项 TaktDepts/tree-options,DictValue=Id）
   */
  deptId?: string;

  /**
   * 所属部门名称（冗余：按 DeptId 取 TaktDept.DeptName联动）
   */
  deptName?: string;

  /**
   * 成本中心层级
   */
  costCenterLevel?: number;

  /**
   * 生效日期
   */
  validFrom?: string;

  /**
   * 失效日期
   */
  validTo?: string;

  /**
   * 成本中心状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  costCenterStatus?: number;

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
 * CostCenter 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 CostCenterImport
 * @description 对应后端 TaktCostCenterImportDto
 */
export interface CostCenterImport {
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
   * 成本中心编码（6 位，与部门编码同长；租户+公司内唯一）
   */
  costCenterCode?: string;

  /**
   * 成本中心名称
   */
  costCenterName?: string;

  /**
   * 父级 ID（0 表示根节点）
   */
  parentId?: string;

  /**
   * 成本中心类型（字典 accounting_controlling_cost_center_type；DictValue=F/G/H/L/S）
   */
  costCenterType?: string;

  /**
   * 负责人用户 ID（选项 TaktUsers/options，DictValue=Id）
   */
  managerId?: string;

  /**
   * 负责人姓名（冗余：按 ManagerId 取 TaktUser.NickName联动）
   */
  managerName?: string;

  /**
   * 所属部门（选项 TaktDepts/tree-options,DictValue=Id）
   */
  deptId?: string;

  /**
   * 所属部门名称（冗余：按 DeptId 取 TaktDept.DeptName联动）
   */
  deptName?: string;

  /**
   * 成本中心层级
   */
  costCenterLevel?: number;

  /**
   * 生效日期
   */
  validFrom?: string;

  /**
   * 失效日期
   */
  validTo?: string;

  /**
   * 成本中心状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  costCenterStatus?: number;

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
 * CostCenter 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 CostCenterExport
 * @description 对应后端 TaktCostCenterExportDto
 */
export interface CostCenterExport {
  /**
   * CostCenterID
   */
  costCenterId: string;

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
   * 成本中心编码（6 位，与部门编码同长；租户+公司内唯一）
   */
  costCenterCode: string;

  /**
   * 成本中心名称
   */
  costCenterName: string;

  /**
   * 父级 ID（0 表示根节点）
   */
  parentId: string;

  /**
   * 成本中心类型（字典 accounting_controlling_cost_center_type；DictValue=F/G/H/L/S）
   */
  costCenterType: string;

  /**
   * 负责人用户 ID（选项 TaktUsers/options，DictValue=Id）
   */
  managerId?: string;

  /**
   * 负责人姓名（冗余：按 ManagerId 取 TaktUser.NickName联动）
   */
  managerName?: string;

  /**
   * 所属部门（选项 TaktDepts/tree-options,DictValue=Id）
   */
  deptId?: string;

  /**
   * 所属部门名称（冗余：按 DeptId 取 TaktDept.DeptName联动）
   */
  deptName?: string;

  /**
   * 成本中心层级
   */
  costCenterLevel: number;

  /**
   * 生效日期
   */
  validFrom: string;

  /**
   * 失效日期
   */
  validTo: string;

  /**
   * 排序号（回填）
   */
  sortOrder: number;

  /**
   * 成本中心状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  costCenterStatus: number;

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

