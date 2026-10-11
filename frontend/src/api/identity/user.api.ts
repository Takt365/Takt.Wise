// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/identity
// 文件名称：user.api.ts
// 创建时间：2026-09-11
// 创建人：Takt365(Auto Generated)
// 功能描述：identity 模块 API（自动生成，请勿手改路由常量）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  TaktBinaryDownload,
  TaktPagedResult,
  TaktSelectOption
} from '@/types/common.types';
import type {
  ChangePassword,
  CreateUser,
  ForgotPassword,
  ForgotPasswordResult,
  ResetPassword,
  UpdateMyAvatar,
  UpdateMyProfile,
  UpdateMyProfileResult,
  UpdateUser,
  User,
  UserStatus,
  UserUnlock
} from '@/types/identity/user.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktUsers
 */
const USER_API_BASE = 'TaktUsers';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取用户列表（分页）
 * @param {any} query 查询DTO
 * @returns {Promise<TaktPagedResult<User>>} 分页结果
 */
export function getUserList(query: any): Promise<TaktPagedResult<User>> {
  return request<TaktPagedResult<User>>({
    url: `${USER_API_BASE}/list`,
    method: 'get',
    params: query,
  });
}

/**
 * 根据ID获取用户
 * @param {string} id 用户ID
 * @returns {Promise<User>} 用户DTO
 */
export function getUserById(id: string): Promise<User> {
  return request<User>({
    url: `${USER_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建用户
 * @param {CreateUser} dto 创建用户DTO
 * @returns {Promise<User>} 用户DTO
 */
export function createUser(dto: CreateUser): Promise<User> {
  return request<User>({
    url: `${USER_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新用户
 * @param {string} id 用户ID
 * @param {UpdateUser} dto 更新用户DTO
 * @returns {Promise<User>} 用户DTO
 */
export function updateUser(id: string, dto: UpdateUser): Promise<User> {
  return request<User>({
    url: `${USER_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除用户
 * @param {string} id 用户ID
 * @returns {Promise<void>} 任务
 */
export function deleteUserById(id: string): Promise<void> {
  return request({
    url: `${USER_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除用户
 * @param {string[]} ids 用户ID列表
 * @returns {Promise<void>} 任务
 */
export function deleteUserBatch(ids: string[]): Promise<void> {
  return request({
    url: `${USER_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新用户状态
 * @param {string} id 用户ID
 * @param {UserStatus} dto 状态DTO
 * @returns {Promise<User>} 用户DTO
 */
export function updateUserStatus(id: string, dto: UserStatus): Promise<User> {
  return request<User>({
    url: `${USER_API_BASE}/${id}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取用户选项列表（用于下拉框等）
 * @param {string} plantCode plantCode
 * @param {string} keyword keyword
 * @returns {Promise<TaktSelectOption[]>} 用户选项列表
 */
export function getUserOptions(plantCode?: string, keyword?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${USER_API_BASE}/options`,
    method: 'get',
    params: {
      plantCode,
      keyword
    },
  });
}

// ========================================
// 密码与解锁
// ========================================

/**
 * 重置用户密码（管理员操作）
 * @param {string} id 用户ID
 * @param {ResetPassword} dto 重置密码DTO
 * @returns {Promise<void>} 任务
 */
export function resetUserPassword(id: string, dto: ResetPassword): Promise<void> {
  return request({
    url: `${USER_API_BASE}/${id}/reset-password`,
    method: 'put',
    data: dto,
  });
}

/**
 * 重置密码
 * @param {ResetPassword} dto 重置密码DTO
 * @returns {Promise<void>} 任务
 */
export function resetPassword(dto: ResetPassword): Promise<void> {
  return request({
    url: `${USER_API_BASE}/reset-password`,
    method: 'put',
    data: dto,
  });
}

/**
 * 修改密码（用户自己操作）
 * @param {ChangePassword} dto 修改密码DTO
 * @returns {Promise<void>} 任务
 */
export function changePassword(dto: ChangePassword): Promise<void> {
  return request({
    url: `${USER_API_BASE}/change-password`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新当前登录用户头像（个人中心；写入关联员工 Avatar）
 * @param dto 头像 DTO（空串清除）
 * @returns {Promise<string | null>} 更新后的头像 URL
 */
export function updateMyAvatar(dto: UpdateMyAvatar): Promise<string | null> {
  return request<string | null>({
    url: `${USER_API_BASE}/me/avatar`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新当前登录用户个人资料（昵称→用户表；手机/邮箱→员工表）
 * @param dto 个人资料 DTO
 * @returns {Promise<UpdateMyProfileResult>} 更新后的可展示字段
 */
export function updateMyProfile(dto: UpdateMyProfile): Promise<UpdateMyProfileResult> {
  return request<UpdateMyProfileResult>({
    url: `${USER_API_BASE}/me/profile`,
    method: 'put',
    data: dto,
  });
}

/**
 * 忘记密码（发送密码重置邮件）
 * @param {ForgotPassword} dto 忘记密码DTO
 * @returns {Promise<ForgotPasswordResult>} 结果
 */
export function forgotPassword(dto: ForgotPassword): Promise<ForgotPasswordResult> {
  return request<ForgotPasswordResult>({
    url: `${USER_API_BASE}/forgot-password`,
    method: 'post',
    data: dto,
  });
}

/**
 * 解锁用户
 * @param {UserUnlock} dto 解锁用户DTO
 * @returns {Promise<UserUnlock>} 用户DTO
 */
export function unlock(dto: UserUnlock): Promise<UserUnlock> {
  return request<UserUnlock>({
    url: `${USER_API_BASE}/unlock`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 统计
// ========================================

/**
 * 统计用户总数
 * @returns {Promise<number>} 用户总数
 */
export function getUserCount(): Promise<number> {
  return request<number>({
    url: `${USER_API_BASE}/count`,
    method: 'get',
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 获取导入模板
 * @param {string} sheetName 工作表名称
 * @param {string} templateName templateName
 * @returns {Promise<Blob>} Excel文件
 */
export function getUserTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${USER_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入用户
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName 工作表名称
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importUserData(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${USER_API_BASE}/import`,
    method: 'post',
    data: formData,
    headers: {
      'Content-Type': 'multipart/form-data',
    },
    params: {
      sheetName
    },
  });
}

/**
 * 导出用户
 * @param {any} query 查询DTO
 * @param {string} sheetName 工作表名称
 * @param {string} exportName exportName
 * @returns {Promise<TaktBinaryDownload>} Excel/ZIP 文件及响应头
 */
export function exportUserData(
  query: any,
  sheetName?: string,
  exportName?: string
): Promise<TaktBinaryDownload> {
  return request<TaktBinaryDownload>({
    url: `${USER_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
    returnBinaryMeta: true,
  });
}
