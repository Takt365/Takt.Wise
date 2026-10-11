// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/utils/taktTicketPriority
// 文件名称：taktTicketPriority.ts
// 功能描述：ITSM 紧急度×影响范围 → 优先级矩阵，与后端 TaktTicketPriorityHelper 对齐
// ========================================

/** ITSM 3×3 优先级矩阵 [urgency-1][impact-1] → sys_priority_level */
const PRIORITY_MATRIX: readonly (readonly number[])[] = [
  [1, 2, 3],
  [2, 3, 4],
  [2, 3, 4],
] as const

/**
 * 将紧急度/影响范围规范为 1～3；非法或 0 视为 3（低）。
 * @param level 原始等级
 * @returns 1、2 或 3
 */
export function normalizeTicketLevel(level: number | null | undefined): number {
  if (level == null || level < 1 || level > 3) {
    return 3
  }
  return level
}

/**
 * 根据 ITSM 3×3 矩阵计算优先级（字典 sys_priority_level）。
 * @param urgency 紧急度 1～3
 * @param impact 影响范围 1～3
 * @returns 优先级 1～4
 */
export function resolveTicketPriority(
  urgency: number | null | undefined,
  impact: number | null | undefined,
): number {
  const u = normalizeTicketLevel(urgency)
  const i = normalizeTicketLevel(impact)
  return PRIORITY_MATRIX[u - 1]![i - 1]!
}
