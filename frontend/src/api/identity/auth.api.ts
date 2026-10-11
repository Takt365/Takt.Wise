// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/identity
// 文件名称：auth.api.ts
// 创建时间：2026-05-27
// 创建人：Takt365(Auto Generated)
// 功能描述：identity 认证会话 API（对应后端 TaktAuthsController；文件名单数 auth.api.ts）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type { TaktSelectOption } from '@/types/common.types';
import type { TaktCaptchaChallengeDto } from '@/types/identity/captcha.types';
import type {
  LoginPreviewLocale,
  LoginPublicKeyResponse,
  LoginRequest,
  SessionVerifyPasswordRequest,
  SessionVerifyPasswordResponse,
  UserInfoResponse,
} from '@/types/identity/login.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktAuths
 */
const AUTH_API_BASE = 'TaktAuths';

// ========================================
// 会话
// ========================================

/**
 * 建立 Cookie 登录会话
 * @param {LoginRequest} dto 登录请求
 * @returns {Promise<boolean>} 是否成功
 */
export function signInSession(dto: LoginRequest): Promise<boolean> {
  return request<boolean>({
    url: `${AUTH_API_BASE}/session/signin`,
    method: 'post',
    data: dto,
    skipTokenRefresh: true,
    skipLoginAuthError: true,
    skipErrorNotification: true,
  });
}

/**
 * 清除 Cookie 登录会话
 * @returns {Promise<boolean>} 是否成功
 */
export function signOutSession(): Promise<boolean> {
  return request<boolean>({
    url: `${AUTH_API_BASE}/session/signout`,
    method: 'post',
    skipTokenRefresh: true,
    skipLoginAuthError: true,
    skipErrorNotification: true,
  });
}

/**
 * 生成登录验证码挑战（匿名）
 * @returns {Promise<TaktCaptchaChallengeDto>} 验证码挑战
 */
export function getSessionCaptcha(): Promise<TaktCaptchaChallengeDto> {
  return request<TaktCaptchaChallengeDto>({
    url: `${AUTH_API_BASE}/session/captcha`,
    method: 'get',
    skipTokenRefresh: true,
    skipLoginAuthError: true,
    skipErrorNotification: true,
  });
}

/**
 * 获取当前登录用户资料（权限、菜单、路由）
 * @returns {Promise<UserInfoResponse>} 用户资料
 */
export function getCurrentUser(): Promise<UserInfoResponse> {
  return request<UserInfoResponse>({
    url: `${AUTH_API_BASE}/me`,
    method: 'get',
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取登录页租户选项（匿名；返回全部租户，登录时按用户校验租户权限）
 * @returns {Promise<TaktSelectOption[]>} 租户下拉选项
 */
export function getSessionTenantOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${AUTH_API_BASE}/session/tenant-options`,
    method: 'get',
    skipTokenRefresh: true,
    skipLoginAuthError: true,
  });
}

/**
 * 校验登录页输入的租户编码是否存在且启用（匿名；查询 TaktTenant）
 * @param {string} tenantCode 租户编码
 * @returns {Promise<boolean>} 存在且启用为 true
 */
export function validateSessionTenantCode(tenantCode: string): Promise<boolean> {
  return request<boolean>({
    url: `${AUTH_API_BASE}/session/tenant-validate`,
    method: 'get',
    params: { tenantCode },
    skipTokenRefresh: true,
    skipLoginAuthError: true,
    skipErrorNotification: true,
  });
}

/**
 * 获取登录页语言切换选项（匿名；未传 tenantCode 时合并全部配置租户）
 * @param {string} [tenantCode] 租户编码（可选）
 * @returns {Promise<TaktSelectOption[]>} 语言下拉选项
 */
export function getSessionCultureOptions(tenantCode?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${AUTH_API_BASE}/session/culture-options`,
    method: 'get',
    params: tenantCode?.trim() ? { tenantCode: tenantCode.trim() } : undefined,
    skipTokenRefresh: true,
    skipLoginAuthError: true,
  });
}

/**
 * 登录前预览：解析用户默认公司、用户 DefaultCulture（须 X-Tenant-Code，与假日无关）
 * @param {string} tenantCode 租户编码
 * @param {string} userName 登录用户名
 * @returns {Promise<LoginPreviewLocale>} 公司编码、用户/公司默认语言
 */
export function getLoginPreviewLocale(tenantCode: string, userName: string): Promise<LoginPreviewLocale> {
  return request<LoginPreviewLocale>({
    url: `${AUTH_API_BASE}/session/login-preview-locale`,
    method: 'get',
    params: {
      tenantCode,
      userName,
    },
    skipTokenRefresh: true,
    skipLoginAuthError: true,
  });
}

/**
 * 获取当前用户可切换的公司选项（已登录，按权限过滤）
 * @returns {Promise<TaktSelectOption[]>} 公司下拉选项
 */
export function getUserCompanyOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${AUTH_API_BASE}/me/company-options`,
    method: 'get',
  });
}

// ========================================
// 密码与解锁
// ========================================

/**
 * 获取登录密码 RSA 公钥（匿名）
 * @returns {Promise<LoginPublicKeyResponse>} 公钥与算法
 */
export function getSessionLoginPublicKey(): Promise<LoginPublicKeyResponse> {
  return request<LoginPublicKeyResponse>({
    url: `${AUTH_API_BASE}/session/login-public-key`,
    method: 'get',
    skipTokenRefresh: true,
  });
}

/**
 * 登录预检：租户用户权限（不验密）→ 验密 → 返回是否需验证码
 * @param {SessionVerifyPasswordRequest} dto 校验请求
 * @returns {Promise<SessionVerifyPasswordResponse>} 校验结果
 */
export function verifySessionPassword(
  dto: SessionVerifyPasswordRequest,
): Promise<SessionVerifyPasswordResponse> {
  return request<SessionVerifyPasswordResponse>({
    url: `${AUTH_API_BASE}/session/verify-password`,
    method: 'post',
    data: dto,
    skipTokenRefresh: true,
    skipLoginAuthError: true,
    skipErrorNotification: true,
  });
}
