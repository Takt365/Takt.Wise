// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：appMeta.ts
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：应用元信息（package.json 版本、构建时间；供日志/事件/PWA 等共用）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { readViteEnv } from '@/config/viteEnv';
import appInfoJson from 'virtual:app-info';

/**
 * package.json 摘要
 */
export interface AppPackageInfo {
  /**
   * npm 包名
   */
  name: string;

  /**
   * 语义化版本
   */
  version: string;

  /**
   * 生产依赖
   */
  dependencies: Record<string, string>;

  /**
   * 开发依赖
   */
  devDependencies: Record<string, string>;
}

/**
 * 构建时注入的应用信息（virtual:app-info）
 */
export interface AppInfo {
  /**
   * package.json 摘要
   */
  pkg: AppPackageInfo;

  /**
   * 最近一次构建时间（ISO8601）
   */
  lastBuildTime: string;
}

/**
 * 校验 virtual:app-info 载荷（无效则抛错，不做静态兜底）
 * @param {unknown} raw 插件注入数据
 * @returns {AppInfo} 应用信息
 */
function assertAppInfo(raw: unknown): AppInfo {
  const parsed = (typeof raw === 'string' ? JSON.parse(raw) : raw) as AppInfo;

  if (!parsed?.pkg) {
    throw new Error('[appMeta] virtual:app-info 缺少 pkg');
  }

  const name = parsed.pkg.name?.trim();
  const version = parsed.pkg.version?.trim();
  const lastBuildTime = parsed.lastBuildTime?.trim();

  if (!name) {
    throw new Error('[appMeta] package.json name 未注入');
  }

  if (!version) {
    throw new Error('[appMeta] package.json version 未注入');
  }

  if (!lastBuildTime) {
    throw new Error('[appMeta] lastBuildTime 未注入');
  }

  return {
    pkg: {
      name,
      version,
      dependencies: parsed.pkg.dependencies ?? {},
      devDependencies: parsed.pkg.devDependencies ?? {},
    },
    lastBuildTime,
  };
}

/** 构建时注入的完整应用信息 */
export const appInfo: AppInfo = assertAppInfo(appInfoJson);

/** npm 包版本（package.json version） */
export const appPackageVersion = appInfo.pkg.version;

/** npm 包名 */
export const appPackageName = appInfo.pkg.name;

/**
 * 解析运行时应用版本（VITE_APP_VERSION 优先，否则 package.json version）
 * @returns {string} 应用版本
 */
function resolveAppVersion(): string {
  const fromEnv = import.meta.env.VITE_APP_VERSION?.trim();
  if (fromEnv) {
    return fromEnv;
  }

  return appPackageVersion;
}

/** 运行时对外展示的应用版本 */
export const appVersion = resolveAppVersion();

/** 生产依赖版本表 */
export const appDependencies = appInfo.pkg.dependencies;

/** 开发依赖版本表 */
export const appDevDependencies = appInfo.pkg.devDependencies;

/** 最近一次构建时间 */
export const appLastBuildTime = appInfo.lastBuildTime;

/**
 * 日志 / 事件总线共用的应用元信息快照
 * @returns {{ appName: string; appVersion: string; environment: string }} 元信息
 */
export function createAppMeta(): { appName: string; appVersion: string; environment: string } {
  const env = readViteEnv();
  const appName = typeof env.VITE_APP_TITLE === 'string' ? env.VITE_APP_TITLE.trim() : '';
  const environment = typeof env.MODE === 'string' ? env.MODE.trim() : '';

  if (!appName) {
    throw new Error('[appMeta] 缺少 VITE_APP_TITLE，请配置 frontend/.env*');
  }

  if (!environment) {
    throw new Error('[appMeta] 缺少 MODE');
  }

  return {
    appName,
    appVersion: resolveAppVersion(),
    environment,
  };
}
