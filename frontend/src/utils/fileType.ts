// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/utils/fileType
// 文件名称：fileType.ts
// 创建时间：2025-01-26
// 创建人：Takt365(Cursor AI)
// 功能描述：文件类型工具函数，与后端逻辑保持一致
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 获取文件扩展名
 * @param fileName 文件名
 * @returns 文件扩展名（小写，不包含点）
 */
export function getFileExtension(fileName: string): string {
  const lastDot = fileName.lastIndexOf('.')
  return lastDot > -1 ? fileName.substring(lastDot + 1).toLowerCase() : ''
}

/**
 * 根据文件扩展名获取文件分类
 * 与后端 TaktFileHelper.GetFileCategory 逻辑保持一致
 * @param fileExtension 文件扩展名（不包含点）
 * @returns 文件分类（0=文档，1=图片，2=视频，3=音频，4=压缩包）
 */
export function getFileCategory(fileExtension: string): number {
  const ext = fileExtension.toLowerCase()
  
  // 图片（1）- 只允许：jpg, png, gif, tif, svg
  if (['jpg', 'jpeg', 'png', 'gif', 'tif', 'tiff', 'svg'].includes(ext)) {
    return 1
  }
  
  // 视频（2）
  if (['mp4', 'avi', 'mov', 'wmv', 'flv', 'mkv', 'webm'].includes(ext)) {
    return 2
  }
  
  // 音频（3）
  if (['mp3', 'wav', 'flac', 'aac', 'ogg', 'wma'].includes(ext)) {
    return 3
  }
  
  // 压缩包（4）
  if (['zip', 'rar', '7z', 'tar', 'gz', 'bz2'].includes(ext)) {
    return 4
  }
  
  // 文档（0）- 默认分类，包括所有其他文件类型
  return 0
}

/**
 * 根据文件名获取文件分类
 * @param fileName 文件名
 * @returns 文件分类（0=文档，1=图片，2=视频，3=音频，4=压缩包）
 */
export function getFileCategoryByFileName(fileName: string): number {
  const ext = getFileExtension(fileName)
  return getFileCategory(ext)
}

/**
 * 根据文件扩展名获取MIME类型
 * 与后端 TaktFileHelper.GetMimeType 逻辑保持一致
 * @param fileExtension 文件扩展名（不包含点）
 * @returns MIME类型
 */
export function getMimeType(fileExtension: string): string {
  const ext = fileExtension.toLowerCase()
  
  switch (ext) {
    case 'jpg':
    case 'jpeg':
      return 'image/jpeg'
    case 'png':
      return 'image/png'
    case 'gif':
      return 'image/gif'
    case 'tif':
    case 'tiff':
      return 'image/tiff'
    case 'svg':
      return 'image/svg+xml'
    case 'pdf':
      return 'application/pdf'
    case 'docx':
      return 'application/vnd.openxmlformats-officedocument.wordprocessingml.document'
    case 'xlsx':
      return 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
    case 'pptx':
      return 'application/vnd.openxmlformats-officedocument.presentationml.presentation'
    case 'zip':
      return 'application/zip'
    case 'rar':
      return 'application/x-rar-compressed'
    case '7z':
      return 'application/x-7z-compressed'
    case 'txt':
      return 'text/plain'
    case 'csv':
      return 'text/csv'
    case 'json':
      return 'application/json'
    case 'xml':
      return 'application/xml'
    case 'mp4':
      return 'video/mp4'
    case 'avi':
      return 'video/x-msvideo'
    case 'mov':
      return 'video/quicktime'
    case 'wmv':
      return 'video/x-ms-wmv'
    case 'flv':
      return 'video/x-flv'
    case 'mkv':
      return 'video/x-matroska'
    case 'webm':
      return 'video/webm'
    case 'mp3':
      return 'audio/mpeg'
    case 'wav':
      return 'audio/wav'
    case 'flac':
      return 'audio/flac'
    case 'aac':
      return 'audio/aac'
    case 'ogg':
      return 'audio/ogg'
    case 'wma':
      return 'audio/x-ms-wma'
    case 'tar':
      return 'application/x-tar'
    case 'gz':
      return 'application/gzip'
    case 'bz2':
      return 'application/x-bzip2'
    default:
      return 'application/octet-stream'
  }
}

/**
 * 根据文件名获取MIME类型
 * @param fileName 文件名
 * @returns MIME类型
 */
export function getMimeTypeByFileName(fileName: string): string {
  const ext = getFileExtension(fileName)
  return getMimeType(ext)
}
