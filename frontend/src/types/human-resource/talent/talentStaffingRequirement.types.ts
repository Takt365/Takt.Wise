// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/talent
// 文件名称：talentStaffingRequirement.types.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/talent 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

import type {
  Dept
} from '@/types/human-resource/organization/dept.types';
import type {
  Post
} from '@/types/human-resource/organization/post.types';
import type {
  Employee
} from '@/types/human-resource/personnel/employee.types';

/**
 * 用人需求（审批单；状态见 TaktApprovalEntityBase.ApprovalStatus）
 * 对应前端 TaktTalentStaffingRequirementDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 TalentStaffingRequirement
 * @description 对应后端 TaktTalentStaffingRequirementDto
 */
export interface TalentStaffingRequirement extends ApprovalDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 需求单号（ReqCode，租户+公司内唯一；自动生成，如 PR-2026-00123）
   */
  reqCode?: string;

  /**
   * 申请部门ID（DeptID，FK→TaktDept）
   */
  deptId?: string;

  /**
   * 申请岗位ID（PositionID，FK→TaktPost）
   */
  postId?: string;

  /**
   * 职级（JobGrade/Rank，如专员/主任/工程师）
   */
  jobGrade?: string;

  /**
   * 需求人数（RequestQty，默认 1）
   */
  requestQty?: number;

  /**
   * 编制类型（HeadcountType：正式/派遣/实习生/临时）
   */
  headcountType?: string;

  /**
   * 需求原因（ReasonCode：新增编制/离职补充/业务扩大/替岗）
   */
  reasonCode?: string;

  /**
   * 替补员工ID（ReplaceEmpID，离职补充时填原员工，FK→TaktEmployee，可空）
   */
  replaceEmployeeId?: string;

  /**
   * 合同类型（ContractType：固定期/无固定/实习协议）
   */
  contractType?: string;

  /**
   * 工作地点（WorkLocation，如工厂/分公司）
   */
  workLocation?: string;

  /**
   * 岗位职责（JobDesc）
   */
  jobDesc?: string;

  /**
   * 任职要求（Qualification，学历/经验/技能）
   */
  qualification?: string;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

}

/**
 * TalentStaffingRequirement 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 TalentStaffingRequirementExport
 * @description 对应后端 TaktTalentStaffingRequirementExportDto
 */
export interface TalentStaffingRequirementExport {
  /**
   * TalentStaffingRequirementID
   */
  talentStaffingRequirementId: string;

  /**
   * 需求单号（ReqCode，租户+公司内唯一；自动生成，如 PR-2026-00123）
   */
  reqCode: string;

  /**
   * 申请部门ID（DeptID，FK→TaktDept）
   */
  deptId: string;

  /**
   * 申请岗位ID（PositionID，FK→TaktPost）
   */
  postId: string;

  /**
   * 职级（JobGrade/Rank，如专员/主任/工程师）
   */
  jobGrade?: string;

  /**
   * 需求人数（RequestQty，默认 1）
   */
  requestQty: number;

  /**
   * 编制类型（HeadcountType：正式/派遣/实习生/临时）
   */
  headcountType: string;

  /**
   * 需求原因（ReasonCode：新增编制/离职补充/业务扩大/替岗）
   */
  reasonCode: string;

  /**
   * 替补员工ID（ReplaceEmpID，离职补充时填原员工，FK→TaktEmployee，可空）
   */
  replaceEmployeeId?: string;

  /**
   * 期望入职日（ExpectedOnboardDate）
   */
  expectedOnboardDate?: string;

  /**
   * 合同类型（ContractType：固定期/无固定/实习协议）
   */
  contractType?: string;

  /**
   * 工作地点（WorkLocation，如工厂/分公司）
   */
  workLocation?: string;

  /**
   * 岗位职责（JobDesc）
   */
  jobDesc?: string;

  /**
   * 任职要求（Qualification，学历/经验/技能）
   */
  qualification?: string;

  /**
   * 预算年度（BudgetYear，用于 headcount 控制）
   */
  budgetYear?: string;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

