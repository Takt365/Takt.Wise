// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/accounting/controlling
// 文件名称：profitCenter.types.ts
// 创建时间：2026-07-09
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
 * 利润中心实体
 * 对应前端 TaktProfitCenterDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ProfitCenter
 * @description 对应后端 TaktProfitCenterDto
 */
export interface ProfitCenter extends CompanyDtoBase {
  /**
   * ProfitCenterID
   */
  profitCenterId: string;
  /**
   * 利润中心编码（4位，租户+公司内唯一）
   */
  profitCenterCode: string;
  /**
   * 利润中心名称
   */
  profitCenterName: string;
  /**
   * 父级 ID
   */
  parentId: string;
  /**
   * 负责人用户 ID
   */
  managerId?: string;
  /**
   * 负责人姓名
   */
  managerName?: string;
  /**
   * 所属部门 ID
   */
  deptId?: string;
  /**
   * 所属部门名称
   */
  deptName?: string;
  /**
   * 利润中心层级
   */
  profitCenterLevel: number;
  /**
   * 生效日期
   */
  validFrom: string;
  /**
   * 失效日期
   */
  validTo: string;
  /**
   * 排序号
   */
  sortOrder: number;
  /**
   * 利润中心状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  profitCenterStatus: number;
}


/**
 * ProfitCenter 树形列表/树选择 DTO（含子节点）
 * 对应 GetProfitCenterTreeAsync 等接口
 * 对应前端 ProfitCenterTree
 * @description 对应后端 TaktProfitCenterTreeDto
 */
export interface ProfitCenterTree extends ProfitCenter {
  /**
   * 子节点
   */
  children: ProfitCenterTree[];

}


/**
 * ProfitCenter 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ProfitCenterQuery
 * @description 对应后端 TaktProfitCenterQueryDto
 */
export interface ProfitCenterQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 利润中心编码（4位，租户+公司内唯一）
   */
  profitCenterCode?: string;

  /**
   * 利润中心名称
   */
  profitCenterName?: string;

  /**
   * 父级 ID
   */
  parentId?: string;

  /**
   * 负责人用户 ID
   */
  managerId?: string;

  /**
   * 负责人姓名
   */
  managerName?: string;

  /**
   * 所属部门 ID
   */
  deptId?: string;

  /**
   * 所属部门名称
   */
  deptName?: string;

  /**
   * 利润中心层级
   */
  profitCenterLevel?: number;

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
   * 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode?: string;

  /**
   * 排序号
   */
  sortOrder?: number;

  /**
   * 利润中心状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  profitCenterStatus?: number;

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
 * 创建ProfitCenter DTO
 * 对应前端 ProfitCenterCreate
 * @description 对应后端 TaktProfitCenterCreateDto
 */
export interface ProfitCenterCreate {
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
   * 利润中心编码（4位，租户+公司内唯一）
   */
  profitCenterCode: string;

  /**
   * 利润中心名称
   */
  profitCenterName: string;

  /**
   * 父级 ID
   */
  parentId: string;

  /**
   * 负责人用户 ID
   */
  managerId?: string;

  /**
   * 负责人姓名
   */
  managerName?: string;

  /**
   * 所属部门 ID
   */
  deptId?: string;

  /**
   * 所属部门名称
   */
  deptName?: string;

  /**
   * 利润中心层级
   */
  profitCenterLevel: number;

  /**
   * 生效日期
   */
  validFrom: string;

  /**
   * 失效日期
   */
  validTo: string;

  /**
   * 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode: string;

  /**
   * 利润中心状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  profitCenterStatus: number;

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
 * 更新ProfitCenter DTO
 * 继承 TaktProfitCenterCreateDto，添加 ProfitCenterId 字段
 * 对应前端 ProfitCenterUpdate
 * @description 对应后端 TaktProfitCenterUpdateDto
 */
export interface ProfitCenterUpdate extends ProfitCenterCreate {
  /**
   * ProfitCenterID（标识要更新的实体）
   */
  profitCenterId: string;

}


/**
 * ProfitCenter 状态更新 DTO
 * 对应前端 ProfitCenterStatus
 * @description 对应后端 TaktProfitCenterStatusDto
 */
export interface ProfitCenterStatus {
  /**
   * ProfitCenterID
   */
  profitCenterId: string;

  /**
   * 利润中心状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  profitCenterStatus: number;

}


/**
 * ProfitCenter 排序更新 DTO
 * 对应前端 ProfitCenterSort
 * @description 对应后端 TaktProfitCenterSortDto
 */
export interface ProfitCenterSort {
  /**
   * ProfitCenterID
   */
  profitCenterId: string;

  /**
   * 排序号
   */
  sortOrder: number;

}


/**
 * ProfitCenter 导入模板行 DTO
 * 对应前端 ProfitCenterTemplate
 * @description 对应后端 TaktProfitCenterTemplateDto
 */
export interface ProfitCenterTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 利润中心编码（4位，租户+公司内唯一）
   */
  profitCenterCode?: string;

  /**
   * 利润中心名称
   */
  profitCenterName?: string;

  /**
   * 父级 ID
   */
  parentId?: string;

  /**
   * 负责人用户 ID
   */
  managerId?: string;

  /**
   * 负责人姓名
   */
  managerName?: string;

  /**
   * 所属部门 ID
   */
  deptId?: string;

  /**
   * 所属部门名称
   */
  deptName?: string;

  /**
   * 利润中心层级
   */
  profitCenterLevel?: number;

  /**
   * 生效日期
   */
  validFrom?: string;

  /**
   * 失效日期
   */
  validTo?: string;

  /**
   * 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode?: string;

  /**
   * 利润中心状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  profitCenterStatus?: number;

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
 * ProfitCenter 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ProfitCenterImport
 * @description 对应后端 TaktProfitCenterImportDto
 */
export interface ProfitCenterImport {
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
   * 利润中心编码（4位，租户+公司内唯一）
   */
  profitCenterCode?: string;

  /**
   * 利润中心名称
   */
  profitCenterName?: string;

  /**
   * 父级 ID
   */
  parentId?: string;

  /**
   * 负责人用户 ID
   */
  managerId?: string;

  /**
   * 负责人姓名
   */
  managerName?: string;

  /**
   * 所属部门 ID
   */
  deptId?: string;

  /**
   * 所属部门名称
   */
  deptName?: string;

  /**
   * 利润中心层级
   */
  profitCenterLevel?: number;

  /**
   * 生效日期
   */
  validFrom?: string;

  /**
   * 失效日期
   */
  validTo?: string;

  /**
   * 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode?: string;

  /**
   * 利润中心状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  profitCenterStatus?: number;

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
 * ProfitCenter 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ProfitCenterExport
 * @description 对应后端 TaktProfitCenterExportDto
 */
export interface ProfitCenterExport {
  /**
   * ProfitCenterID
   */
  profitCenterId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 利润中心编码（4位，租户+公司内唯一）
   */
  profitCenterCode: string;

  /**
   * 利润中心名称
   */
  profitCenterName: string;

  /**
   * 父级 ID
   */
  parentId: string;

  /**
   * 负责人用户 ID
   */
  managerId?: string;

  /**
   * 负责人姓名
   */
  managerName?: string;

  /**
   * 所属部门 ID
   */
  deptId?: string;

  /**
   * 所属部门名称
   */
  deptName?: string;

  /**
   * 利润中心层级
   */
  profitCenterLevel: number;

  /**
   * 生效日期
   */
  validFrom: string;

  /**
   * 失效日期
   */
  validTo: string;

  /**
   * 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode: string;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 利润中心状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  profitCenterStatus: number;

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

