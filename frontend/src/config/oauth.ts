// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/config
// 文件名称：oauth.ts
// 创建时间：2026-05-23
// 创建人：Takt365(Cursor AI)
// 功能描述：OAuth2 Authorization Code + PKCE 客户端配置（全部来自环境变量）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { getAppOrigin, joinOriginPath, requireViteEnv } from '@/config/viteEnv';
import type { TaktOAuthConfig } from '@/types/common.types';

export type { TaktOAuthConfig };

/** OAuth 配置单例缓存，避免重复解析环境变量 */
let oauthConfigCache: TaktOAuthConfig | null = null;

/**
 * 读取 OAuth 配置（延迟到首次使用，避免模块加载时 import.meta.env 未就绪）
 * @returns {TaktOAuthConfig} OAuth 配置
 */
export function getOAuthConfig(): TaktOAuthConfig {
  // 已解析过则直接返回缓存
  if (oauthConfigCache) {
    return oauthConfigCache;
  }

  /** 当前 SPA 站点 Origin（issuer 与端点拼接基准） */
  const appOrigin = getAppOrigin();
  // 组装 PKCE 客户端配置并写入缓存
  oauthConfigCache = {
    issuer: appOrigin,
    clientId: requireViteEnv('VITE_OAUTH_CLIENT_ID'),
    redirectUri: requireViteEnv('VITE_OAUTH_REDIRECT_URI'),
    scope: requireViteEnv('VITE_OAUTH_SCOPE'),
    authorizationEndpoint: joinOriginPath(appOrigin, requireViteEnv('VITE_OAUTH_AUTHORIZE_PATH')),
    tokenEndpoint: joinOriginPath(appOrigin, requireViteEnv('VITE_OAUTH_TOKEN_PATH')),
  };

  return oauthConfigCache;
}
