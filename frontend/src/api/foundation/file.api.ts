// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/foundation
// 文件名称：file.api.ts
// 创建时间：2026-06-13
// 创建人：Takt365(Auto Generated)
// 功能描述：Foundation 文件模块 API（CRUD + 上传/分片；路由常量由 generate-from-backend 生成，上传段见 api-fragments）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  TaktPagedResult,
  TaktSelectOption
} from '@/types/common.types';
import type {
  File as TaktFile,
  FileChunkCancel,
  FileChunkCheck,
  FileChunkCheckResult,
  FileChunkList,
  FileChunkListResult,
  FileChunkMerge,
  FileChunkUpload,
  FileCreate,
  FilePublic,
  FileStatus,
  FileUpdate,
  FileUploadMeta,
  FileUploadPolicy,
  FileUploadResult,
} from '@/types/foundation/file.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktFiles
 */
const FILE_API_BASE = 'TaktFiles';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取文件列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<TaktFile>>} 分页结果
 */
export function getFileList(queryDto: any): Promise<TaktPagedResult<TaktFile>> {
  return request<TaktPagedResult<TaktFile>>({
    url: `${FILE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取文件
 * @param {string} id 文件ID
 * @returns {Promise<TaktFile>} 文件DTO
 */
export function getFileById(id: string): Promise<TaktFile> {
  return request<TaktFile>({
    url: `${FILE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 按文件ID下载物理文件
 * @param {string} id 文件ID
 * @returns {Promise<Blob>} 文件二进制
 */
export function downloadFileById(id: string): Promise<Blob> {
  return request<Blob>({
    url: `${FILE_API_BASE}/${id}/download`,
    method: 'get',
    responseType: 'blob',
  });
}

/**
 * 创建文件
 * @param {FileCreate} dto 创建DTO
 * @returns {Promise<TaktFile>} 文件DTO
 */
export function createFile(dto: FileCreate): Promise<TaktFile> {
  return request<TaktFile>({
    url: `${FILE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新文件
 * @param {string} id 文件ID
 * @param {FileUpdate} dto 更新DTO
 * @returns {Promise<TaktFile>} 文件DTO
 */
export function updateFile(id: string, dto: FileUpdate): Promise<TaktFile> {
  return request<TaktFile>({
    url: `${FILE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除文件
 * @param {string} id 文件ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteFileById(id: string): Promise<void> {
  return request({
    url: `${FILE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除文件
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteFileBatch(ids: string[]): Promise<void> {
  return request({
    url: `${FILE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新文件状态
 * @param {FileStatus} dto 状态 DTO
 * @returns {Promise<TaktFile>} 文件DTO
 */
export function updateFileStatus(dto: FileStatus): Promise<TaktFile> {
  return request<TaktFile>({
    url: `${FILE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新文件公开
 * @param {FilePublic} dto 公开范围 DTO
 * @returns {Promise<TaktFile>} 文件DTO
 */
export function updateFilePublic(dto: FilePublic): Promise<TaktFile> {
  return request<TaktFile>({
    url: `${FILE_API_BASE}/public`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 上传与分片
// ========================================

/**
 * 将上传元数据写入 FormData（camelCase 字段名）
 * @param formData 表单数据
 * @param meta 业务元数据
 */
function appendFileUploadMeta(formData: FormData, meta?: FileUploadMeta): void {
  if (!meta) {
    return;
  }
  const entries: Array<[string, string | number | undefined]> = [
    ['fileDescription', meta.fileDescription],
    ['fileTags', meta.fileTags],
    ['isPublic', meta.isPublic],
    ['fileStatus', meta.fileStatus],
    ['fileUploadType', meta.fileUploadType],
    ['targetFileName', meta.targetFileName],
    ['categoryPath', meta.categoryPath],
    ['storageType', meta.storageType],
    ['storageNaming', meta.storageNaming],
    ['storageConfig', meta.storageConfig],
  ];
  for (const [key, value] of entries) {
    if (value !== undefined && value !== null && value !== '') {
      formData.append(key, String(value));
    }
  }
}

/**
 * 获取上传策略（可选 totalSizeBytes 计算分片计划）
 * @param totalSizeBytes 文件总大小（字节）
 * @returns 上传策略
 */
export function getFileUploadPolicy(totalSizeBytes?: number): Promise<FileUploadPolicy> {
  return request<FileUploadPolicy>({
    url: `${FILE_API_BASE}/upload-policy`,
    method: 'get',
    params: totalSizeBytes != null && totalSizeBytes > 0 ? { totalSizeBytes } : undefined,
  });
}

/**
 * 整文件上传
 * @param file 浏览器 File 对象
 * @param meta 业务元数据
 * @returns 上传结果
 */
export function uploadFile(file: globalThis.File, meta?: FileUploadMeta): Promise<FileUploadResult> {
  const formData = new FormData();
  formData.append('file', file);
  appendFileUploadMeta(formData, meta);
  return request<FileUploadResult>({
    url: `${FILE_API_BASE}/upload`,
    method: 'post',
    data: formData,
    headers: {
      'Content-Type': 'multipart/form-data',
    },
  });
}

/**
 * 检查分片是否已上传
 * @param dto 检查参数
 * @returns 是否存在
 */
export function checkFileChunk(dto: FileChunkCheck): Promise<FileChunkCheckResult> {
  return request<FileChunkCheckResult>({
    url: `${FILE_API_BASE}/chunks/check`,
    method: 'post',
    data: dto,
  });
}

/**
 * 列出已上传分片序号
 * @param dto 查询参数
 * @returns 已上传分片序号列表
 */
export function listFileChunks(dto: FileChunkList): Promise<FileChunkListResult> {
  return request<FileChunkListResult>({
    url: `${FILE_API_BASE}/chunks/list`,
    method: 'post',
    data: dto,
  });
}

/**
 * 上传单个分片
 * @param chunkFile 分片文件
 * @param dto 分片元数据
 * @returns 操作结果
 */
export function uploadFileChunk(chunkFile: globalThis.File, dto: FileChunkUpload): Promise<void> {
  const formData = new FormData();
  formData.append('file', chunkFile);
  formData.append('identifier', dto.identifier);
  formData.append('chunkNumber', String(dto.chunkNumber));
  formData.append('totalChunks', String(dto.totalChunks));
  formData.append('chunkSize', String(dto.chunkSize));
  formData.append('totalSize', String(dto.totalSize));
  formData.append('fileName', dto.fileName);
  return request({
    url: `${FILE_API_BASE}/chunks`,
    method: 'post',
    data: formData,
    headers: {
      'Content-Type': 'multipart/form-data',
    },
  });
}

/**
 * 合并分片并完成上传
 * @param dto 合并参数
 * @returns 上传结果
 */
export function mergeFileChunks(dto: FileChunkMerge): Promise<FileUploadResult> {
  return request<FileUploadResult>({
    url: `${FILE_API_BASE}/chunks/merge`,
    method: 'post',
    data: dto,
  });
}

/**
 * 取消分片上传并清理临时分片
 * @param dto 取消参数
 * @returns 操作结果
 */
export function cancelFileChunks(dto: FileChunkCancel): Promise<void> {
  return request({
    url: `${FILE_API_BASE}/chunks/cancel`,
    method: 'delete',
    params: { identifier: dto.identifier },
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取文件选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getFileOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${FILE_API_BASE}/options`,
    method: 'get',
    params: {
      
      ...(plant ? { plantCode: plant } : {}),
      ...(kw ? { keyword: kw } : {}),
    },
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 获取导入模板
 * @param {string} sheetName sheetName
 * @param {string} templateName templateName
 * @returns {Promise<Blob>} Excel文件
 */
export function getFileTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${FILE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入文件
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importFile(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  return request({
    url: `${FILE_API_BASE}/import`,
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
 * 导出文件
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportFile(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${FILE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}