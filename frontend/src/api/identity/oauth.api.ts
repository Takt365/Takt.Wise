// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/identity
// 文件名称：oauth.api.ts
// 创建时间：2026-05-23
// 创建人：Takt365(Cursor AI)
// 功能描述：OAuth2 令牌端点（Authorization Code + PKCE / RefreshToken）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { getOAuthConfig } from '@/config/oauth';
import { buildTaktClientProfileHeaders } from '@/utils/taktClientProfile';

/**
 * OpenIddict 令牌响应
 */
export interface TaktOAuthTokenResponse {
  access_token: string;
  refresh_token?: string;
  token_type: string;
  expires_in: number;
  scope?: string;
}

/**
 * 使用 authorization_code + PKCE 换取令牌
 * @param {string} code 授权码
 * @param {string} codeVerifier PKCE verifier
 * @returns {Promise<TaktOAuthTokenResponse>} 令牌
 */
export async function exchangeAuthorizationCode(
  code: string,
  codeVerifier: string
): Promise<TaktOAuthTokenResponse> {
  const oauthConfig = getOAuthConfig();
  const body = new URLSearchParams({
    grant_type: 'authorization_code',
    client_id: oauthConfig.clientId,
    code,
    redirect_uri: oauthConfig.redirectUri,
    code_verifier: codeVerifier,
  });

  const response = await fetch(oauthConfig.tokenEndpoint, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/x-www-form-urlencoded',
      ...buildTaktClientProfileHeaders(),
    },
    body,
  });

  if (!response.ok) {
    const text = await response.text();
    throw new Error(text || '授权码换令牌失败');
  }

  return (await response.json()) as TaktOAuthTokenResponse;
}

/**
 * 使用 refresh_token 刷新访问令牌
 * @param {string} refreshToken 刷新令牌
 * @returns {Promise<TaktOAuthTokenResponse>} 新令牌
 */
export async function refreshAccessToken(refreshToken: string): Promise<TaktOAuthTokenResponse> {
  const oauthConfig = getOAuthConfig();
  const body = new URLSearchParams({
    grant_type: 'refresh_token',
    client_id: oauthConfig.clientId,
    refresh_token: refreshToken,
  });

  const response = await fetch(oauthConfig.tokenEndpoint, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/x-www-form-urlencoded',
      ...buildTaktClientProfileHeaders(),
    },
    body,
  });

  if (!response.ok) {
    const text = await response.text();
    throw new Error(text || '刷新令牌失败');
  }

  return (await response.json()) as TaktOAuthTokenResponse;
}
