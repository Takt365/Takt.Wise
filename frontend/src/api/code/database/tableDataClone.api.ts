// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/code/database
// 文件名称：tableDataClone.api.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：跨租户整表克隆 API（预览 + 执行）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  TableClonePreview,
  TableCloneRequest,
  TableCloneResult,
} from '@/types/code/database/tableDataClone.types';

/** API 路径前缀（对应 TaktTableClonesController） */
const TABLE_CLONE_API_BASE = 'TaktTableClones';

/**
 * 获取跨租户整表克隆备份预览（备份窗口）
 * @param dto 克隆请求
 * @returns {Promise<TableClonePreview>} 各目标表备份预览
 */
export function getTableClonePreview(dto: TableCloneRequest): Promise<TableClonePreview> {
  return request<TableClonePreview>({
    url: `${TABLE_CLONE_API_BASE}/preview`,
    method: 'post',
    data: dto,
  });
}

/**
 * 跨租户批量克隆表数据（须 confirmTargetBackupAndClear=true）
 * @param dto 克隆请求
 * @returns {Promise<TableCloneResult>} 批量克隆结果
 */
export function cloneTable(dto: TableCloneRequest): Promise<TableCloneResult> {
  return request<TableCloneResult>({
    url: `${TABLE_CLONE_API_BASE}/clone`,
    method: 'post',
    data: dto,
  });
}
