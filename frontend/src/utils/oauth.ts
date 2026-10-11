// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：oauth.ts
// 创建时间：2026-05-29
// 创建人：Takt365(Cursor AI)
// 功能描述：OAuth2 PKCE、授权跳转、访问令牌无感刷新（运行时网关：Pinia / localStorage 副作用）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { refreshAccessToken, type TaktOAuthTokenResponse } from '@/api/identity/oauth.api';
import { getOAuthConfig } from '@/config/oauth';
import { useUserStore } from '@/stores/identity/useUserStore';

const PKCE_VERIFIER_KEY = 'takt.oauth.pkce_verifier';
const PKCE_STATE_KEY = 'takt.oauth.state';

/** 访问令牌过期前多久触发主动刷新（毫秒） */
export const TOKEN_REFRESH_BUFFER_MS = 60_000;

/** 正在进行中的刷新 Promise（单飞，避免并发重复请求 /connect/token） */
let refreshInFlight: Promise<boolean> | null = null;

/**
 * 生成 URL-safe 随机字符串
 * @param {number} length 字节长度
 * @returns {string} Base64URL 字符串
 */
function generateRandomString(length: number): string {
  const bytes = new Uint8Array(length);
  crypto.getRandomValues(bytes);
  return btoa(String.fromCharCode(...bytes))
    .replace(/\+/g, '-')
    .replace(/\//g, '_')
    .replace(/=+$/, '');
}

/**
 * 计算 code_challenge (S256)
 * @param {string} verifier code_verifier
 * @returns {Promise<string>} code_challenge
 */
async function sha256Base64Url(verifier: string): Promise<string> {
  const data = new TextEncoder().encode(verifier);
  const digest = await crypto.subtle.digest('SHA-256', data);
  const bytes = new Uint8Array(digest);
  return btoa(String.fromCharCode(...bytes))
    .replace(/\+/g, '-')
    .replace(/\//g, '_')
    .replace(/=+$/, '');
}

/**
 * 创建 PKCE 参数并写入 sessionStorage
 * @returns {Promise<{ verifier: string; challenge: string; state: string }>} PKCE 三元组
 */
export async function createPkcePair(): Promise<{
  verifier: string;
  challenge: string;
  state: string;
}> {
  const verifier = generateRandomString(32);
  const challenge = await sha256Base64Url(verifier);
  const state = generateRandomString(16);
  sessionStorage.setItem(PKCE_VERIFIER_KEY, verifier);
  sessionStorage.setItem(PKCE_STATE_KEY, state);
  return { verifier, challenge, state };
}

/**
 * 读取并清除 code_verifier
 * @returns {string | null} verifier
 */
export function consumePkceVerifier(): string | null {
  const verifier = sessionStorage.getItem(PKCE_VERIFIER_KEY);
  sessionStorage.removeItem(PKCE_VERIFIER_KEY);
  return verifier;
}

/**
 * 读取并清除 state
 * @returns {string | null} state
 */
export function consumeOAuthState(): string | null {
  const state = sessionStorage.getItem(PKCE_STATE_KEY);
  sessionStorage.removeItem(PKCE_STATE_KEY);
  return state;
}

/**
 * 跳转到 OpenIddict 授权端点（须已调用 signInSession 建立 Cookie）
 * @returns {Promise<void>}
 */
export async function redirectToAuthorize(): Promise<void> {
  const oauthConfig = getOAuthConfig();
  const { challenge, state } = await createPkcePair();
  const params = new URLSearchParams({
    client_id: oauthConfig.clientId,
    response_type: 'code',
    redirect_uri: oauthConfig.redirectUri,
    scope: oauthConfig.scope,
    code_challenge: challenge,
    code_challenge_method: 'S256',
    state,
  });
  const authorizeUrl = `${oauthConfig.authorizationEndpoint}?${params.toString()}`;
  window.location.href = authorizeUrl;
}

/**
 * 将令牌响应写入用户 Store
 * @param {TaktOAuthTokenResponse} token 令牌端点响应
 */
export function applyOAuthTokenResponse(token: TaktOAuthTokenResponse): void {
  const userStore = useUserStore();
  userStore.setOAuthTokens({
    accessToken: token.access_token,
    refreshToken: token.refresh_token,
    expiresIn: token.expires_in,
  });
}

/**
 * 访问令牌是否即将过期（或已过期）
 * @param {number} [bufferMs=TOKEN_REFRESH_BUFFER_MS] 提前刷新缓冲时间
 * @returns {boolean} 是否需要刷新
 */
export function isAccessTokenExpiringSoon(bufferMs: number = TOKEN_REFRESH_BUFFER_MS): boolean {
  const userStore = useUserStore();
  if (!userStore.token) {
    return false;
  }
  const expiresAt = userStore.tokenExpiresAt;
  if (!expiresAt || !Number.isFinite(expiresAt)) {
    return false;
  }
  return Date.now() >= expiresAt - bufferMs;
}

/**
 * 使用 refresh_token 刷新访问令牌（并发请求共享同一 Promise）
 * @returns {Promise<boolean>} 是否刷新成功
 */
export async function refreshOAuthTokens(): Promise<boolean> {
  if (refreshInFlight) {
    return refreshInFlight;
  }
  refreshInFlight = (async () => {
    const userStore = useUserStore();
    const currentRefreshToken = userStore.refreshToken;
    if (!currentRefreshToken) {
      return false;
    }
    try {
      const token = await refreshAccessToken(currentRefreshToken);
      applyOAuthTokenResponse(token);
      return true;
    } catch {
      return false;
    } finally {
      refreshInFlight = null;
    }
  })();
  return refreshInFlight;
}

/**
 * 请求前确保 access_token 有效（即将过期时主动刷新）
 * @returns {Promise<boolean>} 是否具备可用访问令牌
 */
export async function ensureValidAccessToken(): Promise<boolean> {
  const userStore = useUserStore();
  if (!userStore.token) {
    return false;
  }
  if (!isAccessTokenExpiringSoon()) {
    return true;
  }
  if (!userStore.refreshToken) {
    return false;
  }
  return refreshOAuthTokens();
}
