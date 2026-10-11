// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/code/database
// 文件名称：dataClone.api.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：code/database 模块 API（自动生成，请勿手改路由常量）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  DataClone,
  DataClonePreview,
  DataCloneResult
} from '@/types/code/database/dataClone.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktDataClones
 */
const DATA_CLONE_API_BASE = 'TaktDataClones';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取公司级数据克隆备份预览（备份窗口）
 * @param {DataClone} dto 克隆请求
 * @returns {Promise<DataClonePreview>} 备份与清空预览
 */
export function getDataClonePreview(dto: DataClone): Promise<DataClonePreview> {
  return request<DataClonePreview>({
    url: `${DATA_CLONE_API_BASE}/preview`,
    method: 'post',
    data: dto,
  });
}

/**
 * 按公司范围克隆数据（一次一个源公司、一张表；须先确认备份窗口）
 * @param {DataClone} dto 克隆请求
 * @returns {Promise<DataCloneResult>} 克隆结果
 */
export function cloneData(dto: DataClone): Promise<DataCloneResult> {
  return request<DataCloneResult>({
    url: `${DATA_CLONE_API_BASE}/clone`,
    method: 'post',
    data: dto,
  });
}
