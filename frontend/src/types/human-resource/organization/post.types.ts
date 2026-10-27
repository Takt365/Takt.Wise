// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/organization
// 文件名称：post.types.ts
// 创建时间：2026-08-21
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/organization 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 岗位实体 代表组织架构中的岗位/职位
 * 对应前端 TaktPostDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Post
 * @description 对应后端 TaktPostDto
 */
export interface Post extends CompanyDtoBase {
  /**
   * PostID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  postId: string;

  /**
   * 岗位编码（唯一索引：租户+公司内唯一，见 ix_post_code_unique）
   */
  postCode: string;

  /**
   * 岗位名称
   */
  postName: string;

  /**
   * 所属部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  deptId: string;

  /**
   * 所属部门名称（冗余：按 DeptId 取 TaktDept.DeptName联动）
   */
  deptName: string;

  /**
   * 岗位类别（字典 sys_post_category；列存 DictValue：MGT=管理岗 PRO=专业岗 ENG=技术岗 SUP=支持岗 OPS=操作岗）
   */
  postCategory: string;

  /**
   * 岗位职级（字典 sys_post_level；列存 DictValue：P1~P4 专业序列 M1~M5 管理序列）
   */
  postLevel: string;

  /**
   * 编制人数
   */
  headcount: number;

  /**
   * 当前在职人数
   */
  currentCount: number;

  /**
   * 岗位职责
   */
  responsibilities: string;

  /**
   * 任职要求
   */
  requirements: string;

  /**
   * 学历要求（字典 humanresource_personnel_education_level；1=高中及以下 2=大专 3=本科 4=硕士 5=博士）
   */
  educationRequired: number;

  /**
   * 工作经验要求（年）
   */
  experienceYears: number;

  /**
   * 薪资范围（最低）
   */
  salaryMin?: number;

  /**
   * 薪资范围（最高）
   */
  salaryMax?: number;

  /**
   * 内置（字典 sys_yes_no；0=否 1=是；种子岗位为内置，不允许删除）
   */
  isBuiltIn: number;

  /**
   * 岗位描述
   */
  postDescription?: string;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
   */
  postStatus: number;

}


/**
 * Post 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PostQuery
 * @description 对应后端 TaktPostQueryDto
 */
export interface PostQuery extends TaktPagedQuery {
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
   * 岗位编码（唯一索引：租户+公司内唯一，见 ix_post_code_unique）
   */
  postCode?: string;

  /**
   * 岗位名称
   */
  postName?: string;

  /**
   * 所属部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  deptId?: string;

  /**
   * 所属部门名称（冗余：按 DeptId 取 TaktDept.DeptName联动）
   */
  deptName?: string;

  /**
   * 岗位类别（字典 sys_post_category；列存 DictValue：MGT=管理岗 PRO=专业岗 ENG=技术岗 SUP=支持岗 OPS=操作岗）
   */
  postCategory?: string;

  /**
   * 岗位职级（字典 sys_post_level；列存 DictValue：P1~P4 专业序列 M1~M5 管理序列）
   */
  postLevel?: string;

  /**
   * 编制人数
   */
  headcount?: number;

  /**
   * 当前在职人数
   */
  currentCount?: number;

  /**
   * 岗位职责
   */
  responsibilities?: string;

  /**
   * 任职要求
   */
  requirements?: string;

  /**
   * 学历要求（字典 humanresource_personnel_education_level；1=高中及以下 2=大专 3=本科 4=硕士 5=博士）
   */
  educationRequired?: number;

  /**
   * 工作经验要求（年）
   */
  experienceYears?: number;

  /**
   * 薪资范围（最低）
   */
  salaryMin?: number;

  /**
   * 薪资范围（最高）
   */
  salaryMax?: number;

  /**
   * 内置（字典 sys_yes_no；0=否 1=是；种子岗位为内置，不允许删除）
   */
  isBuiltIn?: number;

  /**
   * 岗位描述
   */
  postDescription?: string;

  /**
   * 排序号
   */
  sortOrder?: number;

  /**
   * 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
   */
  postStatus?: number;

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
 * 创建Post DTO
 * 对应前端 PostCreate
 * @description 对应后端 TaktPostCreateDto
 */
export interface PostCreate {
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
   * 岗位编码（唯一索引：租户+公司内唯一，见 ix_post_code_unique）
   */
  postCode: string;

  /**
   * 岗位名称
   */
  postName: string;

  /**
   * 所属部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  deptId: string;

  /**
   * 所属部门名称（冗余：按 DeptId 取 TaktDept.DeptName联动）
   */
  deptName: string;

  /**
   * 岗位类别（字典 sys_post_category；列存 DictValue：MGT=管理岗 PRO=专业岗 ENG=技术岗 SUP=支持岗 OPS=操作岗）
   */
  postCategory: string;

  /**
   * 岗位职级（字典 sys_post_level；列存 DictValue：P1~P4 专业序列 M1~M5 管理序列）
   */
  postLevel: string;

  /**
   * 编制人数
   */
  headcount: number;

  /**
   * 当前在职人数
   */
  currentCount: number;

  /**
   * 岗位职责
   */
  responsibilities: string;

  /**
   * 任职要求
   */
  requirements: string;

  /**
   * 学历要求（字典 humanresource_personnel_education_level；1=高中及以下 2=大专 3=本科 4=硕士 5=博士）
   */
  educationRequired: number;

  /**
   * 工作经验要求（年）
   */
  experienceYears: number;

  /**
   * 薪资范围（最低）
   */
  salaryMin?: number;

  /**
   * 薪资范围（最高）
   */
  salaryMax?: number;

  /**
   * 内置（字典 sys_yes_no；0=否 1=是；种子岗位为内置，不允许删除）
   */
  isBuiltIn: number;

  /**
   * 岗位描述
   */
  postDescription?: string;

  /**
   * 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
   */
  postStatus: number;

  /**
   * 关联该岗位的员工 ID 列表（RBAC 反向合并）
   */
  employeeIds?: any;

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
 * 更新Post DTO
 * 继承 TaktPostCreateDto，添加 PostId 字段
 * 对应前端 PostUpdate
 * @description 对应后端 TaktPostUpdateDto
 */
export interface PostUpdate extends PostCreate {
  /**
   * PostID（标识要更新的实体）
   */
  postId: string;

}


/**
 * Post 状态更新 DTO
 * 对应前端 PostStatus
 * @description 对应后端 TaktPostStatusDto
 */
export interface PostStatus {
  /**
   * PostID
   */
  postId: string;

  /**
   * 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
   */
  postStatus: number;

}


/**
 * Post 排序更新 DTO
 * 对应前端 PostSort
 * @description 对应后端 TaktPostSortDto
 */
export interface PostSort {
  /**
   * PostID
   */
  postId: string;

  /**
   * 排序号
   */
  sortOrder: number;

}


/**
 * Post 导入模板行 DTO
 * 对应前端 PostTemplate
 * @description 对应后端 TaktPostTemplateDto
 */
export interface PostTemplate {
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
   * 岗位编码（唯一索引：租户+公司内唯一，见 ix_post_code_unique）
   */
  postCode?: string;

  /**
   * 岗位名称
   */
  postName?: string;

  /**
   * 所属部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  deptId?: string;

  /**
   * 所属部门名称（冗余：按 DeptId 取 TaktDept.DeptName联动）
   */
  deptName?: string;

  /**
   * 岗位类别（字典 sys_post_category；列存 DictValue：MGT=管理岗 PRO=专业岗 ENG=技术岗 SUP=支持岗 OPS=操作岗）
   */
  postCategory?: string;

  /**
   * 岗位职级（字典 sys_post_level；列存 DictValue：P1~P4 专业序列 M1~M5 管理序列）
   */
  postLevel?: string;

  /**
   * 编制人数
   */
  headcount?: number;

  /**
   * 当前在职人数
   */
  currentCount?: number;

  /**
   * 岗位职责
   */
  responsibilities?: string;

  /**
   * 任职要求
   */
  requirements?: string;

  /**
   * 学历要求（字典 humanresource_personnel_education_level；1=高中及以下 2=大专 3=本科 4=硕士 5=博士）
   */
  educationRequired?: number;

  /**
   * 工作经验要求（年）
   */
  experienceYears?: number;

  /**
   * 薪资范围（最低）
   */
  salaryMin?: number;

  /**
   * 薪资范围（最高）
   */
  salaryMax?: number;

  /**
   * 内置（字典 sys_yes_no；0=否 1=是；种子岗位为内置，不允许删除）
   */
  isBuiltIn?: number;

  /**
   * 岗位描述
   */
  postDescription?: string;

  /**
   * 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
   */
  postStatus?: number;

  /**
   * 关联该岗位的员工 ID 列表（RBAC 反向合并）
   */
  employeeIds?: any;

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
 * Post 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PostImport
 * @description 对应后端 TaktPostImportDto
 */
export interface PostImport {
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
   * 岗位编码（唯一索引：租户+公司内唯一，见 ix_post_code_unique）
   */
  postCode?: string;

  /**
   * 岗位名称
   */
  postName?: string;

  /**
   * 所属部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  deptId?: string;

  /**
   * 所属部门名称（冗余：按 DeptId 取 TaktDept.DeptName联动）
   */
  deptName?: string;

  /**
   * 岗位类别（字典 sys_post_category；列存 DictValue：MGT=管理岗 PRO=专业岗 ENG=技术岗 SUP=支持岗 OPS=操作岗）
   */
  postCategory?: string;

  /**
   * 岗位职级（字典 sys_post_level；列存 DictValue：P1~P4 专业序列 M1~M5 管理序列）
   */
  postLevel?: string;

  /**
   * 编制人数
   */
  headcount?: number;

  /**
   * 当前在职人数
   */
  currentCount?: number;

  /**
   * 岗位职责
   */
  responsibilities?: string;

  /**
   * 任职要求
   */
  requirements?: string;

  /**
   * 学历要求（字典 humanresource_personnel_education_level；1=高中及以下 2=大专 3=本科 4=硕士 5=博士）
   */
  educationRequired?: number;

  /**
   * 工作经验要求（年）
   */
  experienceYears?: number;

  /**
   * 薪资范围（最低）
   */
  salaryMin?: number;

  /**
   * 薪资范围（最高）
   */
  salaryMax?: number;

  /**
   * 内置（字典 sys_yes_no；0=否 1=是；种子岗位为内置，不允许删除）
   */
  isBuiltIn?: number;

  /**
   * 岗位描述
   */
  postDescription?: string;

  /**
   * 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
   */
  postStatus?: number;

  /**
   * 关联该岗位的员工 ID 列表（RBAC 反向合并）
   */
  employeeIds?: any;

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
 * Post 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PostExport
 * @description 对应后端 TaktPostExportDto
 */
export interface PostExport {
  /**
   * PostID
   */
  postId: string;

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
   * 岗位编码（唯一索引：租户+公司内唯一，见 ix_post_code_unique）
   */
  postCode: string;

  /**
   * 岗位名称
   */
  postName: string;

  /**
   * 所属部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  deptId: string;

  /**
   * 所属部门名称（冗余：按 DeptId 取 TaktDept.DeptName联动）
   */
  deptName: string;

  /**
   * 岗位类别（字典 sys_post_category；列存 DictValue：MGT=管理岗 PRO=专业岗 ENG=技术岗 SUP=支持岗 OPS=操作岗）
   */
  postCategory: string;

  /**
   * 岗位职级（字典 sys_post_level；列存 DictValue：P1~P4 专业序列 M1~M5 管理序列）
   */
  postLevel: string;

  /**
   * 编制人数
   */
  headcount: number;

  /**
   * 当前在职人数
   */
  currentCount: number;

  /**
   * 岗位职责
   */
  responsibilities: string;

  /**
   * 任职要求
   */
  requirements: string;

  /**
   * 学历要求（字典 humanresource_personnel_education_level；1=高中及以下 2=大专 3=本科 4=硕士 5=博士）
   */
  educationRequired: number;

  /**
   * 工作经验要求（年）
   */
  experienceYears: number;

  /**
   * 薪资范围（最低）
   */
  salaryMin?: number;

  /**
   * 薪资范围（最高）
   */
  salaryMax?: number;

  /**
   * 内置（字典 sys_yes_no；0=否 1=是；种子岗位为内置，不允许删除）
   */
  isBuiltIn: number;

  /**
   * 岗位描述
   */
  postDescription?: string;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
   */
  postStatus: number;

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

