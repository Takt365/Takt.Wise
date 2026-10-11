// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/statistics/logging
// 文件名称：loginLog.types.ts
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
 * 登录日志实体
 * 对应前端 TaktLoginLogDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 LoginLog
 * @description 对应后端 TaktLoginLogDto
 */
export interface LoginLog extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 用户名（登录账号）
   */
  userName: string;

  /**
   * 登录方式
   */
  loginType?: string;

  /**
   * 浏览器
   */
  browser?: string;

  /**
   * 操作系统（TaktConstants.OperatingSystem）
   */
  os?: string;

  /**
   * 用户代理（User-Agent）
   */
  userAgent?: string;

  /**
   * 登录结果
   */
  loginResult: string;

  /**
   * 登录结果消息
   */
  loginMessage?: string;

  /**
   * 登录IP地址
   */
  loginIp?: string;

  /**
   * 登录地点（IP解析，如：中国-广东省-深圳市）
   */
  loginLocation?: string;

  /**
   * 登出时间
   */
  logoutAt?: string;

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
 * 更新LoginLog DTO
 * 继承 TaktLoginLogCreateDto，添加 LoginLogId 字段
 * 对应前端 LoginLogUpdate
 * @description 对应后端 TaktLoginLogUpdateDto
 */
export interface LoginLogUpdate extends LoginLogCreate {
  /**
   * LoginLogID（标识要更新的实体）
   */
  loginLogId: string;

}

/**
 * LoginLog 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 LoginLogExport
 * @description 对应后端 TaktLoginLogExportDto
 */
export interface LoginLogExport {
  /**
   * LoginLogID
   */
  loginLogId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 用户名（登录账号）
   */
  userName: string;

  /**
   * 登录方式
   */
  loginType?: string;

  /**
   * 浏览器
   */
  browser?: string;

  /**
   * 操作系统（TaktConstants.OperatingSystem）
   */
  os?: string;

  /**
   * 用户代理（User-Agent）
   */
  userAgent?: string;

  /**
   * 登录结果
   */
  loginResult: string;

  /**
   * 登录结果消息
   */
  loginMessage?: string;

  /**
   * 登录IP地址
   */
  loginIp?: string;

  /**
   * 登录地点（IP解析，如：中国-广东省-深圳市）
   */
  loginLocation?: string;

  /**
   * 登出时间
   */
  logoutAt?: string;

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

