// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/statistics/logging
// 文件名称：operLog.types.ts
// 创建时间：2026-06-12
// 创建人：Takt365(Auto Generated)
// 功能描述：statistics/logging 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 操作日志实体
 * 对应前端 TaktOperLogDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 OperLog
 * @description 对应后端 TaktOperLogDto
 */
export interface OperLog extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 用户名（登录账号）
   */
  userName: string;

  /**
   * 操作模块（如：用户管理、部门管理）
   */
  operModule?: string;

  /**
   * 操作类型（HTTP 审计推导）
   */
  operType: string;

  /**
   * 操作方法（如：TaktUserService.CreateUserAsync）
   */
  operMethod?: string;

  /**
   * 请求方式（GET、POST、PUT、DELETE 等）
   */
  requestMethod?: string;

  /**
   * 操作 URL（含查询字符串）
   */
  operUrl?: string;

  /**
   * 请求参数 JSON（当前操作入参/操作值完整快照；写入方须脱敏密码、Token 等）
   */
  requestParam?: string;

  /**
   * 返回结果 JSON（当前操作出参/响应摘要）
   */
  jsonResult?: string;

  /**
   * 操作状态（0=失败，1=成功）
   */
  operStatus: number;

  /**
   * 错误消息（失败时）
   */
  errorMsg?: string;

  /**
   * 操作 IP
   */
  operIp?: string;

  /**
   * 操作地点（由 OperIp 解析，如：中国-广东省-深圳市）
   */
  operLocation?: string;

  /**
   * 操作时间（业务操作发生时刻）
   */
  operTime: string;

  /**
   * 执行耗时（毫秒）
   */
  elapsedTime: number;

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
 * 更新OperLog DTO
 * 继承 TaktOperLogCreateDto，添加 OperLogId 字段
 * 对应前端 OperLogUpdate
 * @description 对应后端 TaktOperLogUpdateDto
 */
export interface OperLogUpdate extends OperLogCreate {
  /**
   * OperLogID（标识要更新的实体）
   */
  operLogId: string;

}

/**
 * OperLog 状态更新 DTO
 * 对应前端 OperLogStatus
 * @description 对应后端 TaktOperLogStatusDto
 */
export interface OperLogStatus {
  /**
   * OperLogID
   */
  operLogId: string;

  /**
   * 操作状态（0=失败，1=成功）
   */
  operStatus: number;

}

/**
 * OperLog 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 OperLogExport
 * @description 对应后端 TaktOperLogExportDto
 */
export interface OperLogExport {
  /**
   * OperLogID
   */
  operLogId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 用户名（登录账号）
   */
  userName: string;

  /**
   * 操作模块（如：用户管理、部门管理）
   */
  operModule?: string;

  /**
   * 操作类型（HTTP 审计推导）
   */
  operType: string;

  /**
   * 操作方法（如：TaktUserService.CreateUserAsync）
   */
  operMethod?: string;

  /**
   * 请求方式（GET、POST、PUT、DELETE 等）
   */
  requestMethod?: string;

  /**
   * 操作 URL（含查询字符串）
   */
  operUrl?: string;

  /**
   * 请求参数 JSON（当前操作入参/操作值完整快照；写入方须脱敏密码、Token 等）
   */
  requestParam?: string;

  /**
   * 返回结果 JSON（当前操作出参/响应摘要）
   */
  jsonResult?: string;

  /**
   * 操作状态（0=失败，1=成功）
   */
  operStatus: number;

  /**
   * 错误消息（失败时）
   */
  errorMsg?: string;

  /**
   * 操作 IP
   */
  operIp?: string;

  /**
   * 操作地点（由 OperIp 解析，如：中国-广东省-深圳市）
   */
  operLocation?: string;

  /**
   * 操作时间（业务操作发生时刻）
   */
  operTime: string;

  /**
   * 执行耗时（毫秒）
   */
  elapsedTime: number;

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

