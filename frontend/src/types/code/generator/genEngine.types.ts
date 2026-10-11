// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/code/generator
// 文件名称：genEngine.types.ts
// 创建时间：2026-06-02
// 创建人：Takt365(Cursor AI)
// 功能描述：代码生成引擎类型定义，对齐 TaktGenEngineDtos.cs
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { GenTableCreate } from '@/types/code/generator/genTable.types';

/**
 * 代码生成结果：文件名（或相对路径）与生成内容
 * 对应前端 CodeGenResult
 * @description 对应后端 TaktCodeGenResultDto
 */
export interface CodeGenResult {
  /**
   * 生成的文件名或相对路径（如 Entity.cs、Dto.cs）
   */
  fileName: string;
  /**
   * 生成后的代码/文本内容
   */
  content: string;
}

/**
 * 代码预览文件：目标路径、渲染内容、目标路径是否已存在
 * 对应前端 CodeGenPreviewFile
 * @description 对应后端 TaktCodeGenPreviewFileDto
 */
export interface CodeGenPreviewFile {
  /**
   * 目标相对路径（如 backend/src/Takt.Domain/Entities/Identity/TaktUser.cs）
   */
  path: string;
  /**
   * 渲染后的代码/文本内容
   */
  content: string;
  /**
   * 目标路径下文件是否已存在（仅 GenMethod=1/2 且路径可解析时有效）
   */
  isExisting: boolean;
}

/**
 * 预览模板校验问题：记录模板键、目标路径与错误信息
 * 对应前端 CodeGenPreviewValidationIssue
 * @description 对应后端 TaktCodeGenPreviewValidationIssueDto
 */
export interface CodeGenPreviewValidationIssue {
  /**
   * 模板键（如 Backend/Crud/Csharp/Dto.cs）
   */
  templateKey: string;
  /**
   * 解析后的目标相对路径（可能为空）
   */
  targetPath?: string;
  /**
   * 校验错误信息（模板解析失败、模板渲染失败等）
   */
  message: string;
}

/**
 * 预览渲染结果：包含可预览文件与模板校验问题
 * 对应前端 CodeGenPreviewResult
 * @description 对应后端 TaktCodeGenPreviewResultDto
 */
export interface CodeGenPreviewResult {
  /**
   * 预览渲染是否通过（无校验问题则为 true）
   */
  isValid: boolean;
  /**
   * 渲染成功的预览文件列表
   */
  previewFiles: CodeGenPreviewFile[];
  /**
   * 模板校验问题列表（按模板逐项记录）
   */
  validationIssues: CodeGenPreviewValidationIssue[];
}

/**
 * 从数据库导入表请求（有表导入）
 * 对应前端 ImportTableFromDatabaseRequest
 * @description 对应后端 TaktImportTableFromDatabaseRequestDto
 */
export interface ImportTableFromDatabaseRequest {
  /**
   * 租户编码（3 位，对应 appsettings Database:TenantCodes）
   */
  tenantCode: string;
  /**
   * 要导入的数据表名
   */
  tableName: string;
  /**
   * 表配置覆盖（可选，用于补充实体类名、业务名等）
   */
  tableOverrides?: GenTableCreate;
}

/**
 * 代码生成表是否生成代码
 * 对应前端 IsGenCode
 * @description 对应后端 TaktIsGenCodeDto
 */
export interface IsGenCode {
  /**
   * 代码生成表配置 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  genTableId: string;
  /**
   * 是否生成代码（0=否，1=是）
   */
  isGenCode: number;
}

/**
 * 从实体初始化表请求（无表流程）
 * 对应前端 InitializeTableFromEntityRequest
 * @description 对应后端 TaktInitializeTableFromEntityRequestDto
 */
export interface InitializeTableFromEntityRequest {
  /**
   * 租户编码（3 位，对应 appsettings Database:TenantCodes）
   */
  tenantCode: string;
  /**
   * 实体类型全名（如 Takt.Domain.Entities.Code.Generator.TaktGenTable）
   */
  entityTypeFullName: string;
}

/**
 * 代码生成交付结果（GenMethod=1/2 落盘；GenMethod=0 为 zip 二进制，不经此类型）
 * @description 对应后端 TaktCodeGenGenerateResultDto（JSON 部分）
 */
export interface CodeGenGenerateResult {
  genMethod: number;
  basePath?: string;
  writtenFilePaths?: string[];
  fileCount: number;
  zipFileName?: string;
}

/**
 * 代码生成请求
 * @description 对应后端 TaktGenerateCodeRequestDto
 */
export interface GenerateCodeRequest {
  templates?: Record<string, string>;
  /** 0=zip，1=自定义路径，2=当前项目；可空则取表配置 */
  genMethod?: number;
  /** GenMethod=1 时的目标根路径 */
  genPath?: string;
}

/**
 * 代码预览请求
 * 对应前端 PreviewCodeRequest
 * @description 对应后端 TaktPreviewCodeRequestDto
 */
export interface PreviewCodeRequest {
  /**
   * 模板字典：模板键 → Scriban 模板内容（可空，由后端加载）
   */
  templates?: Record<string, string>;
  /**
   * 路径映射：模板键 → 目标相对路径
   */
  pathMappings?: Record<string, string>;
  /**
   * 目标根路径（可空；为空时不检查是否已存在）
   */
  targetBasePath?: string;
}
