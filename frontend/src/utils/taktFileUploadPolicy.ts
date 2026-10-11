// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/utils
// 文件名称：taktFileUploadPolicy.ts
// 创建时间：2026-06-12
// 创建人：Takt
// 功能描述：文件上传 UI 辅助（accept/提示；策略与校验以 api/TaktFiles/upload-policy 为准）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { getFileUploadPolicy } from '@/api/foundation/file.api';
import type { FileUploadPolicy } from '@/types/foundation/file.types';

/** 全局基础上传策略缓存（不含 totalSize 分片计划） */
let cachedBasePolicyPromise: Promise<FileUploadPolicy> | null = null;

/**
 * 加载后端上传基础策略（MaxFileSize、AllowedExtensions 等）
 * @returns 上传策略
 */
export async function loadTaktFileUploadBasePolicy(): Promise<FileUploadPolicy> {
  if (!cachedBasePolicyPromise) {
    cachedBasePolicyPromise = getFileUploadPolicy();
  }
  return cachedBasePolicyPromise;
}

/**
 * 解析单文件最大体积（MB，用于 UI 展示）
 * @param policy 上传策略
 * @returns MB 整数
 */
export function resolveTaktFileMaxSizeMb(policy: FileUploadPolicy): number {
  const bytes = Number(policy.maxFileSizeBytes);
  if (!Number.isFinite(bytes) || bytes <= 0) {
    return 500;
  }
  return Math.max(1, Math.floor(bytes / 1024 / 1024));
}

/**
 * 构建 a-upload accept 属性
 * @param allowedExtensions 允许扩展名（小写、不含点）
 * @returns accept 字符串
 */
export function buildTaktFileAcceptAttribute(allowedExtensions: readonly string[]): string {
  if (!allowedExtensions?.length) {
    return '';
  }
  return allowedExtensions.map((ext) => `.${ext}`).join(',');
}

/**
 * 从文件名解析扩展名（小写、不含点）
 * @param fileName 文件名
 * @returns 扩展名；无扩展名时返回空串
 */
export function getTaktFileExtension(fileName: string): string {
  if (!fileName?.trim()) {
    return '';
  }
  const lastDot = fileName.lastIndexOf('.');
  if (lastDot < 0 || lastDot === fileName.length - 1) {
    return '';
  }
  return fileName.slice(lastDot + 1).toLowerCase();
}
