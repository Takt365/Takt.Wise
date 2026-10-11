// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/code/database
// 文件名称：tableClone.api.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：code/database 模块 API（自动生成，请勿手改路由常量）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  TableClone,
  TableClonePreview,
  TableCloneResult
} from '@/types/code/database/tableClone.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktTableClones
 */
const TABLE_CLONE_API_BASE = 'TaktTableClones';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取跨租户整表克隆备份预览（备份窗口）
 * @param {TableClone} dto 克隆请求
 * @returns {Promise<TableClonePreview>} 备份与清空预览
 */
export function getTableClonePreview(dto: TableClone): Promise<TableClonePreview> {
  return request<TableClonePreview>({
    url: `${TABLE_CLONE_API_BASE}/preview`,
    method: 'post',
    data: dto,
  });
}

/**
 * 跨租户批量克隆源表数据到目标表（一次 1~5 张表；须先确认备份窗口）
 * @param {TableClone} dto 克隆请求
 * @returns {Promise<TableCloneResult>} 克隆结果
 */
export function cloneTable(dto: TableClone): Promise<TableCloneResult> {
  return request<TableCloneResult>({
    url: `${TABLE_CLONE_API_BASE}/clone`,
    method: 'post',
    data: dto,
  });
}
