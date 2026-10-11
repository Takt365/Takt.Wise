// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：exportDownloadName.ts
// 创建时间：2026-05-22
// 创建人：Takt365(Cursor AI)
// 功能描述：按服务端响应头还原下载文件名（zip/xlsx 由后端 Content-Type 决定）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 从 Content-Disposition 解析文件名（支持 filename、filename* RFC5987）
 * @param {string | undefined | null} contentDisposition 响应头 Content-Disposition
 * @returns {string | null} 解析到的文件名；无法解析时返回 null
 */
export function parseContentDispositionFileName(contentDisposition: string | undefined | null): string | null {
  // 非字符串或空则无法解析
  if (!contentDisposition || typeof contentDisposition !== 'string') {
    return null;
  }
  /** 去首尾空白后的头值 */
  const trimmed = contentDisposition.trim();
  if (!trimmed) {
    return null;
  }

  // 优先 RFC5987 filename*=UTF-8''...
  const star = /filename\*=(?:UTF-8''|utf-8'')([^;\s]+)/i.exec(trimmed);
  if (star?.[1]) {
    /** 去掉引号的编码片段 */
    const raw = star[1].replace(/^["']|["']$/g, '').trim();
    try {
      // URL 解码 UTF-8 文件名
      return decodeURIComponent(raw);
    } catch {
      // filename* 解码失败时回退原始片段（Try 语义，不抛异常）
      return raw;
    }
  }

  // 带双引号的 filename="..."
  const quoted = /filename\s*=\s*"([^"]+)"/i.exec(trimmed);
  if (quoted?.[1]) {
    return sanitizeExportFileSegment(quoted[1]);
  }

  // 无引号 filename=...
  const unquoted = /filename\s*=\s*([^;\s]+)/i.exec(trimmed);
  if (unquoted?.[1]) {
    return sanitizeExportFileSegment(unquoted[1].replace(/^["']|["']$/g, ''));
  }

  return null;
}

/**
 * 清理文件名非法路径字符
 * @param {string} name 原始片段
 * @returns {string} 可安全用于下载的文件名片段
 */
function sanitizeExportFileSegment(name: string): string {
  /** 去空白后的名称 */
  const s = name.trim();
  if (!s) {
    return s;
  }
  // 替换 Windows/URL 非法字符为下划线
  return s.replace(/[/\\?%*:|"<>]/g, '_');
}

/**
 * 去掉已知导出扩展名以便重新拼接
 * @param {string} base 文件基名
 * @returns {string} 无 .xlsx/.xls/.zip 后缀的基名
 */
function stripKnownExportExt(base: string): string {
  return base.replace(/\.(xlsx|xls|zip)$/i, '');
}

/**
 * 为基名确保指定扩展名
 * @param {string} base 文件基名
 * @param {string} ext 目标扩展名（含点）
 * @returns {string} 带扩展名的文件名
 */
function ensureExt(base: string, ext: string): string {
  return stripKnownExportExt(base) + ext;
}

/**
 * 优先使用服务端 Content-Disposition 中的文件名；无则按 Content-Type 为 fallbackBase 补扩展名
 * @param {object} options 解析选项
 * @param {string | null} [options.contentDisposition] Content-Disposition 响应头
 * @param {string | null} [options.contentType] Content-Type 响应头
 * @param {string} options.fallbackBase 无头时的文件基名
 * @returns {string} 最终下载文件名（含扩展名）
 */
export function resolveExportDownloadFileName(options: {
  contentDisposition?: string | null;
  contentType?: string | null;
  /** 不含扩展名或任意；方法内会去掉末尾 .xlsx/.zip 再拼接 */
  fallbackBase: string;
}): string {
  /** 从 Content-Disposition 解析的文件名 */
  const fromHeader = parseContentDispositionFileName(options.contentDisposition ?? undefined);
  if (fromHeader && fromHeader.length > 0) {
    return fromHeader;
  }

  /** 小写 Content-Type，用于推断扩展名 */
  const ct = (options.contentType || '').toLowerCase();
  // 后端分批导出 zip
  if (ct.includes('application/zip')) {
    return ensureExt(options.fallbackBase, '.zip');
  }
  // Excel OpenXML 或旧版 xls
  if (
    ct.includes('spreadsheetml') ||
    ct.includes('application/vnd.ms-excel') ||
    ct.includes('officedocument.spreadsheetml')
  ) {
    return ensureExt(options.fallbackBase, '.xlsx');
  }
  // 默认按 xlsx 兜底
  return ensureExt(options.fallbackBase, '.xlsx');
}
