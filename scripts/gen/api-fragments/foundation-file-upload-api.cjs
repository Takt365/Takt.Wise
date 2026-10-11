// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：scripts/api-fragments
// 文件名称：foundation-file-upload-api.cjs
// 创建时间：2026-06-14
// 创建人：Takt365(Cursor AI)
// 功能描述：TaktFiles 上传/分片 API 片段（generate-from-backend 合并进 foundation/file.ts）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const UPLOAD_TYPE_NAMES = `  FileChunkCancel,
  FileChunkCheck,
  FileChunkCheckResult,
  FileChunkList,
  FileChunkListResult,
  FileChunkMerge,
  FileChunkUpload,
  FileUploadMeta,
  FileUploadPolicy,
  FileUploadResult,`;

/**
 * 上传/分片 API 段（使用 FILE_API_BASE 常量名）
 * @returns {string}
 */
function buildFoundationFileUploadApiBlock() {
  return `
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
    url: \`\${FILE_API_BASE}/upload-policy\`,
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
    url: \`\${FILE_API_BASE}/upload\`,
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
    url: \`\${FILE_API_BASE}/chunks/check\`,
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
    url: \`\${FILE_API_BASE}/chunks/list\`,
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
    url: \`\${FILE_API_BASE}/chunks\`,
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
    url: \`\${FILE_API_BASE}/chunks/merge\`,
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
    url: \`\${FILE_API_BASE}/chunks/cancel\`,
    method: 'delete',
    params: { identifier: dto.identifier },
  });
}
`;
}

/**
 * 将上传 API 合并进 generate-from-backend 产出的 foundation/file.ts
 * @param {string} content 已生成的 API 文件内容
 * @returns {string}
 */
function mergeFoundationFileUploadApi(content) {
  if (content.includes('getFileUploadPolicy')) {
    return content;
  }
  let merged = content;
  if (!merged.includes('FileUploadPolicy')) {
    merged = merged.replace(
      "} from '@/types/foundation/file.types';",
      `,\n${UPLOAD_TYPE_NAMES}\n} from '@/types/foundation/file.types';`
    );
  }
  const optionsMarker = '// ========================================\n// 选项';
  if (merged.includes(optionsMarker)) {
    merged = merged.replace(optionsMarker, `${buildFoundationFileUploadApiBlock()}\n${optionsMarker}`);
  } else {
    merged += buildFoundationFileUploadApiBlock();
  }
  return merged;
}

module.exports = {
  mergeFoundationFileUploadApi,
};
