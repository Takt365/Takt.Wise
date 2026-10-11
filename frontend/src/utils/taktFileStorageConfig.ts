// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/utils
// 文件名称：taktFileStorageConfig.ts
// 创建时间：2025-06-12
// 创建人：Takt
// 功能描述：文件存储配置 JSON 解析与上传 CategoryPath 拼接（菜单模块路径 + 文件类型目录）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { getFileCategoryByFileName } from '@/utils/fileType';

/** 基础平台文件页默认上传路径（一级菜单 FOUNDATION → uploads/foundation） */
export const TAKT_FILE_DEFAULT_UPLOAD_PATH = 'uploads/foundation';

/** 后端 TaktOssHelper 当前已实现的上传 OSS 提供商（与 IsSupportedProvider 对齐） */
export const TAKT_SUPPORTED_OSS_PROVIDERS = ['aliyun'] as const;

/** 已实现的 OSS 提供商标识 */
export type TaktSupportedOssProvider = (typeof TAKT_SUPPORTED_OSS_PROVIDERS)[number];

/**
 * 判断 OSS 提供商是否已被后端实现
 * @param value 字典 sys_oss_provider 的 dictValue
 * @returns 是否支持
 */
export function isSupportedOssProvider(value: string | null | undefined): value is TaktSupportedOssProvider {
  if (!value?.trim()) {
    return false;
  }
  return (TAKT_SUPPORTED_OSS_PROVIDERS as readonly string[]).includes(value.trim().toLowerCase());
}

/**
 * 过滤 OSS 下拉选项，仅保留后端已实现的提供商
 * @param options takt-select 选项列表
 * @returns 过滤后的选项
 */
export function filterOssProviderSelectOptions<T extends { value?: string | number }>(
  options: readonly T[],
): T[] {
  return options.filter((item) => isSupportedOssProvider(String(item.value ?? '')));
}

/**
 * 规范化 OSS 提供商标识；未实现时回退 aliyun
 * @param value 表单或 storageConfig 中的值
 * @returns 有效 provider
 */
export function normalizeOssProvider(value: unknown): TaktSupportedOssProvider {
  const text = String(value ?? '').trim().toLowerCase();
  return isSupportedOssProvider(text) ? text : 'aliyun';
}

/** 存储配置 JSON 载荷（写入 TaktFile.StorageConfig） */
export interface TaktFileStorageConfigPayload {
  /** 上传路径（一级目录菜单 RoutePath 首段，如 uploads/human-resource） */
  uploadPath?: string;
  /** 存储命名规则（字典 sys_storage_naming：0/1/2） */
  storageNaming?: string | number;
  /** OSS 提供商标识（字典 sys_oss_provider；StorageType=1） */
  ossProvider?: string;
  /** FTP 提供商标识（字典 sys_ftp_provider；StorageType=2） */
  ftpProvider?: string;
}

/**
 * 从 StorageConfig JSON 解析存储字典字段
 * @param raw StorageConfig 原始字符串
 * @returns 解析后的字段；非法 JSON 返回空对象
 */
export function parseFileStorageConfig(raw?: string | null): TaktFileStorageConfigPayload {
  if (!raw?.trim()) {
    return {};
  }
  try {
    const parsed = JSON.parse(raw) as TaktFileStorageConfigPayload;
    if (parsed == null || typeof parsed !== 'object') {
      return {};
    }
    return parsed;
  } catch {
    return {};
  }
}

/**
 * 将存储字典字段序列化为 StorageConfig JSON
 * @param payload 表单字段
 * @returns JSON 字符串；全部为空时返回 null
 */
export function buildFileStorageConfigJson(payload: TaktFileStorageConfigPayload): string | null {
  const uploadPath = payload.uploadPath?.trim();
  const storageNaming = payload.storageNaming;
  const ossProvider = payload.ossProvider?.trim();
  const ftpProvider = payload.ftpProvider?.trim();
  const hasNaming = storageNaming !== undefined && storageNaming !== null && String(storageNaming).trim() !== '';
  if (!uploadPath && !hasNaming && !ossProvider && !ftpProvider) {
    return null;
  }
  const body: TaktFileStorageConfigPayload = {};
  if (uploadPath) {
    body.uploadPath = uploadPath;
  }
  if (hasNaming) {
    body.storageNaming = storageNaming;
  }
  if (ossProvider) {
    body.ossProvider = ossProvider;
  }
  if (ftpProvider) {
    body.ftpProvider = ftpProvider;
  }
  return JSON.stringify(body);
}

/**
 * 按文件分类返回存储子目录段（与后端 TaktFileHelper.GetStorageDirectorySegment 对齐）
 * @param fileCategory 文件分类 0~5
 * @returns 目录段，如 images、documents
 */
export function resolveStorageDirectoryFromFileCategory(fileCategory: number): string {
  switch (fileCategory) {
    case 1:
      return 'images';
    case 2:
      return 'videos';
    case 3:
      return 'audios';
    case 4:
      return 'archives';
    case 0:
      return 'documents';
    default:
      return 'default';
  }
}

/**
 * 根据上传文件推断存储子目录段
 * @param file 待上传文件
 * @returns 目录段
 */
export function resolveStorageDirectoryFromUploadFile(file: File): string {
  return resolveStorageDirectoryFromFileCategory(getFileCategoryByFileName(file.name));
}

/**
 * 由菜单模块路径与文件类型目录拼接引擎 CategoryPath（去掉 uploads/ 前缀；default 不追加）
 * @param uploadPath 上传路径（如 uploads/human-resource）
 * @param storageDirectorySegment 文件类型目录段（如 images）
 * @returns CategoryPath；无有效路径时返回 undefined
 */
export function resolveFileCategoryPath(
  uploadPath?: string | null,
  storageDirectorySegment?: string | null
): string | undefined {
  if (!uploadPath?.trim()) {
    return undefined;
  }
  let base = uploadPath.trim().replace(/^uploads\/?/i, '').replace(/^\/+|\/+$/g, '');
  const dir = storageDirectorySegment?.trim();
  if (dir && dir !== 'default') {
    base = base ? `${base}/${dir}` : dir;
  }
  return base || undefined;
}

/**
 * 根据菜单上传路径与上传文件生成 CategoryPath
 * @param uploadPath 菜单推导路径（如 uploads/human-resource）
 * @param file 待上传文件
 * @returns CategoryPath（如 human-resource/images）
 */
export function resolveFileCategoryPathForUpload(
  uploadPath?: string | null,
  file?: File | null
): string | undefined {
  if (!file) {
    return resolveFileCategoryPath(uploadPath);
  }
  return resolveFileCategoryPath(uploadPath, resolveStorageDirectoryFromUploadFile(file));
}

/**
 * 合并存储默认值（新增态或空字段）
 * @param target 目标对象
 * @param force 是否强制覆盖
 */
export function applyFileStorageDefaults(target: Record<string, unknown>, force = false): void {
  if (force || target.uploadPath == null || target.uploadPath === '') {
    target.uploadPath = TAKT_FILE_DEFAULT_UPLOAD_PATH;
  }
  if (force || target.storageNaming == null || target.storageNaming === '') {
    target.storageNaming = 0;
  }
  if (force || target.storageType == null || target.storageType === '') {
    target.storageType = 0;
  }
}
