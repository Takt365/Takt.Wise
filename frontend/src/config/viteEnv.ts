// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/config
// 文件名称：viteEnv.ts
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：读取 VITE_ 环境变量（必填、无静态兜底）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 读取 Vite 注入的环境变量对象
 * @description 必须使用 `import.meta.env` 字面量；`import.meta?.env` 不会被 Vite 静态替换
 * @returns {ImportMetaEnv} 环境变量
 */
export function readViteEnv(): ImportMetaEnv {
  // 字面量访问以触发 Vite 构建期注入
  return import.meta.env;
}

/**
 * 是否处于 Vite 客户端运行时（MODE 已由构建注入）
 * @returns {boolean} MODE 为非空字符串时为 true
 */
export function isViteEnvInjected(): boolean {
  // 通过 MODE 是否存在判断是否在 Vite 客户端环境
  return typeof import.meta.env.MODE === 'string' && import.meta.env.MODE.length > 0;
}

/**
 * 读取必填 Vite 环境变量
 * @param {keyof ImportMetaEnv} key 变量名
 * @returns {string} 去首尾空格后的值
 * @throws {Error} 缺失或为空时抛出
 */
export function requireViteEnv(key: keyof ImportMetaEnv): string {
  /** 环境变量原始值 */
  const raw = import.meta.env[key];

  // 非字符串或空白视为未配置
  if (typeof raw !== 'string' || raw.trim() === '') {
    throw new Error(`[env] 缺少环境变量 ${String(key)}，请配置 frontend/.env、.env.development 或 .env.production`);
  }

  return raw.trim();
}

/**
 * 去掉末尾斜杠
 * @param {string} origin 根地址
 * @returns {string} 无尾斜杠的 origin
 */
export function normalizeOrigin(origin: string): string {
  // 统一去掉尾部 /，避免拼接双斜杠
  return origin.replace(/\/$/, '');
}

/**
 * 拼接根地址与路径
 * @param {string} origin 根地址
 * @param {string} path 以 / 开头的路径
 * @returns {string} 完整 URL
 */
export function joinOriginPath(origin: string, path: string): string {
  /** 规范化后的 origin */
  const base = normalizeOrigin(origin);
  // 路径缺省前导 / 时自动补上
  const suffix = path.startsWith('/') ? path : `/${path}`;
  return `${base}${suffix}`;
}

/**
 * 浏览器访问后端根地址（OAuth、SignalR 等与页面/API 代理同源策略一致）
 * @returns {string} VITE_APP_ORIGIN 规范化后的值
 */
export function getAppOrigin(): string {
  // 必填环境变量，缺失时 requireViteEnv 抛错
  return normalizeOrigin(requireViteEnv('VITE_APP_ORIGIN'));
}
