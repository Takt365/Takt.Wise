// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/utils/taktAvatarUrl
// 文件名称：taktAvatarUrl.ts
// 功能描述：解析用户头像 URL（仅 admin 种子 gif）与无头像时的用户名首字母
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import taktAdminAvatar from '@/assets/images/avatar/takt_admin.gif'

/** 管理员种子头像路径（与 TaktEmployeeSeedData.AdminAvatarPath 一致） */
export const TAKT_SEED_AVATAR_ADMIN = 'assets/images/avatar/takt_admin.gif'

/** 种子相对路径 → Vite 解析后的 URL（仅 admin） */
const SEED_AVATAR_URL_BY_PATH: Readonly<Record<string, string>> = {
  [TAKT_SEED_AVATAR_ADMIN]: taktAdminAvatar,
  [`@/${TAKT_SEED_AVATAR_ADMIN}`]: taktAdminAvatar,
}

/**
 * 将资料/种子中的头像字段解析为可给 img/a-avatar 使用的 URL
 * @param avatar 员工 Avatar 或用户资料 avatar（可为 admin 种子路径、上传 AccessUrl、绝对 URL）
 * @returns 可展示的 URL；空则 undefined（由首字母占位）
 */
export function resolveTaktAvatarUrl(avatar: string | null | undefined): string | undefined {
  const raw = typeof avatar === 'string' ? avatar.trim() : ''
  if (!raw) {
    return undefined
  }
  const seeded = SEED_AVATAR_URL_BY_PATH[raw]
  if (seeded) {
    return seeded
  }
  const withoutAt = raw.startsWith('@/') ? raw.slice(2) : raw
  const seededWithoutAt = SEED_AVATAR_URL_BY_PATH[withoutAt]
  if (seededWithoutAt) {
    return seededWithoutAt
  }
  return raw
}

/**
 * 无头像时用用户名（优先）或昵称首字符大写展示
 * @param userName 登录用户名
 * @param nickName 昵称（用户名为空时回退）
 * @returns 单个大写字符；皆空则为 ?
 */
export function resolveTaktAvatarInitial(
  userName?: string | null,
  nickName?: string | null,
): string {
  const source = (userName?.trim() || nickName?.trim() || '').trim()
  if (!source) {
    return '?'
  }
  const first = [...source][0] ?? '?'
  return first.toLocaleUpperCase()
}
