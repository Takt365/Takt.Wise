// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/utils
// 文件名称：taktEcAttachmentDocCode.ts
// 创建时间：2026-08-26
// 创建人：Takt365(Cursor AI)
// 功能描述：设变附件 DocCode 格式校验、PDF 扩展名判定，以及由 DocCode 生成存储/展示文件名（与后端 TaktEcAttachmentDocCodeHelper 对齐）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** 文件类别 DictValue（logistics_manufacturing_ec_attachment_type） */
export const EcAttachmentType = {
  /** 联络 */
  TL: 'TL',
  /** EPP */
  EPP: 'EPP',
  /** FPP */
  FPP: 'FPP',
  /** 外部联络 */
  EL: 'EL',
  /** TCJ */
  TCJ: 'TCJ',
  /** 设变 */
  EC: 'EC',
} as const

/** EPP/FPP：P- + 4 位数字 */
const EPP_FPP_PATTERN = /^P-\d{4}$/
/** 联络 TL：DTS- + 4 位数字 */
const TL_PATTERN = /^DTS-\d{4}$/
/** TCJ / EL：xxxx-xxxx */
const QUAD_DASH_QUAD_PATTERN = /^\d{4}-\d{4}$/

/**
 * 按文件类别校验文件编码格式
 * @param attachmentType 文件类别 DictValue
 * @param docCode 文件编码
 * @param ecCode 设变单号（类型 EC 时须一致）
 * @returns 是否合法
 */
export function isValidEcAttachmentDocCode(
  attachmentType: string | null | undefined,
  docCode: string | null | undefined,
  ecCode?: string | null,
): boolean {
  const type = String(attachmentType ?? '').trim()
  const code = String(docCode ?? '').trim()
  if (!type || !code) {
    return false
  }
  switch (type) {
    case EcAttachmentType.EC:
      return code === String(ecCode ?? '').trim()
    case EcAttachmentType.EPP:
    case EcAttachmentType.FPP:
      return EPP_FPP_PATTERN.test(code)
    case EcAttachmentType.TL:
      return TL_PATTERN.test(code)
    case EcAttachmentType.TCJ:
    case EcAttachmentType.EL:
      return QUAD_DASH_QUAD_PATTERN.test(code)
    default:
      return true
  }
}

/**
 * 文件编码占位/格式提示键后缀（配合 locales page.attachment.docCode.*）
 * @param attachmentType 文件类别
 * @returns 提示键末段，无规则时返回 empty
 */
export function getEcAttachmentDocCodeHintKey(
  attachmentType: string | null | undefined,
): 'ec' | 'eppFpp' | 'tl' | 'quadDash' | 'empty' {
  const type = String(attachmentType ?? '').trim()
  switch (type) {
    case EcAttachmentType.EC:
      return 'ec'
    case EcAttachmentType.EPP:
    case EcAttachmentType.FPP:
      return 'eppFpp'
    case EcAttachmentType.TL:
      return 'tl'
    case EcAttachmentType.TCJ:
    case EcAttachmentType.EL:
      return 'quadDash'
    default:
      return 'empty'
  }
}

/**
 * 类型为 EC 时是否应将文件编码锁定为设变单号
 * @param attachmentType 文件类别
 * @returns 是否锁定
 */
export function isEcAttachmentDocCodeLockedToEcCode(
  attachmentType: string | null | undefined,
): boolean {
  return String(attachmentType ?? '').trim() === EcAttachmentType.EC
}

/**
 * 是否为 PDF 附件文件名（扩展名 .pdf，大小写不敏感；与后端 TaktEcAttachmentDocCodeHelper.IsPdfFileName 对齐）
 * @param fileName 源文件名或存储文件名
 * @returns 扩展名为 .pdf 时 true
 */
export function isEcAttachmentPdfFileName(fileName?: string | null): boolean {
  return extractFileExtension(fileName).toLowerCase() === '.pdf'
}

/**
 * 由文件编码生成存储/展示文件名（与后端 TaktEcAttachmentDocCodeHelper.BuildFileNameFromDocCode 对齐）
 * @param docCode 文件编码
 * @param sourceFileName 源文件名或当前 fileName
 * @param accessUrl 访问地址（源文件名无扩展名时回退）
 * @returns DocCode + 扩展名；docCode 为空时返回空串
 */
export function buildEcAttachmentFileName(
  docCode: string | null | undefined,
  sourceFileName?: string | null,
  accessUrl?: string | null,
): string {
  const code = String(docCode ?? '').trim()
  if (!code) {
    return ''
  }
  const ext = extractFileExtension(sourceFileName) || extractFileExtension(accessUrl)
  return ext ? `${code}${ext}` : code
}

/**
 * 从文件名或 URL 提取扩展名（含点）
 * @param source 文件名或 URL
 * @returns 如 .pdf；无法解析时返回空串
 */
function extractFileExtension(source?: string | null): string {
  const raw = String(source ?? '').trim()
  if (!raw) {
    return ''
  }
  const withoutQuery = raw.split('?', 2)[0].replace(/\\/g, '/')
  const fileName = withoutQuery.split('/').pop() ?? ''
  const dot = fileName.lastIndexOf('.')
  if (dot <= 0 || dot === fileName.length - 1) {
    return ''
  }
  return fileName.slice(dot)
}
