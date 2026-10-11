// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/code/generator
// 文件名称：genEngine.api.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：code/generator 模块 API（自动生成，请勿手改路由常量）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  GenTable
} from '@/types/code/generator/genTable.types';
import type {
  CodeGenGenerateResult,
  CodeGenPreviewResult,
  GenerateCodeRequest,
  ImportTableFromDatabaseRequest,
  InitializeTableFromEntityRequest,
  PreviewCodeRequest,
} from '@/types/code/generator/genEngine.types';
import type { TaktBinaryDownload } from '@/types/common.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktGenEngines
 */
const GEN_ENGINE_API_BASE = 'TaktGenEngines';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取可用于「按实体初始化表」的实体类型全名列表
 * @returns {Promise<unknown>} 实体类型全名列表
 */
export function getAvailableEntityTypes(): Promise<unknown> {
  return request({
    url: `${GEN_ENGINE_API_BASE}/entities`,
    method: 'get',
  });
}

/**
 * 根据实体类型初始化数据表（无表流程：代码生成后手动建表）
 * @param {InitializeTableFromEntityRequest} dto 初始化请求（租户编码、实体类型全名）
 * @returns {Promise<unknown>} 操作结果
 */
export function initializeTableFromEntity(dto: InitializeTableFromEntityRequest): Promise<unknown> {
  return request({
    url: `${GEN_ENGINE_API_BASE}/entities/initialize`,
    method: 'post',
    data: dto,
  });
}

/**
 * 根据表配置和模板生成代码（GenMethod=0 返回 zip；1/2 返回落盘 JSON）
 */
export function generateCode(
  tableId: string,
  dto: GenerateCodeRequest,
): Promise<TaktBinaryDownload | CodeGenGenerateResult> {
  const genMethod = dto.genMethod != null ? Number(dto.genMethod) : 0;
  if (genMethod === 0) {
    return request<TaktBinaryDownload>({
      url: `${GEN_ENGINE_API_BASE}/generate/${tableId}`,
      method: 'post',
      data: dto,
      responseType: 'blob',
      returnBinaryMeta: true,
    });
  }
  return request<CodeGenGenerateResult>({
    url: `${GEN_ENGINE_API_BASE}/generate/${tableId}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 预览生成的代码文件（不落盘，仅用于模板校验）
 * @param {string} tableId 代码生成表配置 ID
 * @param {PreviewCodeRequest} dto 预览请求（模板可空；PathMappings 可覆盖内置路径解析）
 * @returns {Promise<CodeGenPreviewResult>} 预览结果（文件相对路径 + 内容 + 是否已存在）
 */
export function previewCode(tableId: string, dto: PreviewCodeRequest): Promise<CodeGenPreviewResult> {
  return request<CodeGenPreviewResult>({
    url: `${GEN_ENGINE_API_BASE}/preview/${tableId}`,
    method: 'post',
    data: dto,
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 从数据库导入表结构到代码生成配置（有表导入）
 * @param {ImportTableFromDatabaseRequest} dto 导入请求（租户编码、表名、可选表配置覆盖）
 * @returns {Promise<GenTable>} 导入后的表配置信息
 */
export function importTableFromDatabase(dto: ImportTableFromDatabaseRequest): Promise<GenTable> {
  return request<GenTable>({
    url: `${GEN_ENGINE_API_BASE}/database/import`,
    method: 'post',
    data: dto,
  });
}

/**
 * 从数据库同步指定表的列元数据到代码生成配置（增量更新，保留用户生成配置）
 * @param {string} tableId 代码生成表配置 ID
 * @returns {Promise<GenTable>} 同步后的表配置信息（含列列表）
 */
export function syncTableColumnsFromDatabase(tableId: string): Promise<GenTable> {
  return request<GenTable>({
    url: `${GEN_ENGINE_API_BASE}/database/sync/${tableId}`,
    method: 'post',
  });
}
