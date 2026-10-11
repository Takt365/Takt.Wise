// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/routine/document-center
// 文件名称：documentVersion.types.ts
// 创建时间：2026-08-24
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/document-center 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 文管文档版本子实体
 * 对应前端 TaktDocumentVersionDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 DocumentVersion
 * @description 对应后端 TaktDocumentVersionDto
 */
export interface DocumentVersion extends CompanyDtoBase {
  /**
   * DocumentVersionID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  documentVersionId: string;

  /**
   * 文档 ID（选项 TaktDocuments/options；DictValue=Id）
   */
  documentId: string;

  /**
   * 文档 名称（填充字段）
   */
  documentName?: string;

  /**
   * 行号（固定步长=10）
   */
  lineNumber: number;

  /**
   * 版本号
   */
  versionNo: number;

  /**
   * 版本说明
   */
  versionNote?: string;

  /**
   * 文件 ID（选项 TaktFiles/options；DictValue=Id；上传下载元数据在 TaktFile）
   */
  fileId: string;

  /**
   * 文件 名称（填充字段）
   */
  fileName?: string;

  /**
   * 修订人 ID（选项 TaktUsers/options；DictValue=Id）
   */
  revisedBy: string;

  /**
   * 修订人姓名（冗余字段，便于查询）
   */
  revisedByName?: string;

  /**
   * 修订时间
   */
  revisedAt: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 文档（主表） （主表：TaktDocument）
   */
  document?: Document;

}


/**
 * DocumentVersion 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 DocumentVersionQuery
 * @description 对应后端 TaktDocumentVersionQueryDto
 */
export interface DocumentVersionQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 文档 ID（选项 TaktDocuments/options；DictValue=Id）
   */
  documentId?: string;

  /**
   * 行号（固定步长=10）
   */
  lineNumber?: number;

  /**
   * 版本号
   */
  versionNo?: number;

  /**
   * 版本说明
   */
  versionNote?: string;

  /**
   * 文件 ID（选项 TaktFiles/options；DictValue=Id；上传下载元数据在 TaktFile）
   */
  fileId?: string;

  /**
   * 修订人 ID（选项 TaktUsers/options；DictValue=Id）
   */
  revisedBy?: string;

  /**
   * 修订人姓名（冗余字段，便于查询）
   */
  revisedByName?: string;

  /**
   * 修订时间（范围查询-开始）
   */
  revisedAtStart?: string;

  /**
   * 修订时间（范围查询-结束）
   */
  revisedAtEnd?: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

  /**
   * 创建时间（范围查询-开始）
   */
  createdAtStart?: string;

  /**
   * 创建时间（范围查询-结束）
   */
  createdAtEnd?: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建DocumentVersion DTO
 * 对应前端 DocumentVersionCreate
 * @description 对应后端 TaktDocumentVersionCreateDto
 */
export interface DocumentVersionCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode: string;

  /**
   * 文档 ID（选项 TaktDocuments/options；DictValue=Id）
   */
  documentId: string;

  /**
   * 行号（固定步长=10）
   */
  lineNumber: number;

  /**
   * 版本号
   */
  versionNo: number;

  /**
   * 版本说明
   */
  versionNote?: string;

  /**
   * 文件 ID（选项 TaktFiles/options；DictValue=Id；上传下载元数据在 TaktFile）
   */
  fileId: string;

  /**
   * 修订人 ID（选项 TaktUsers/options；DictValue=Id）
   */
  revisedBy: string;

  /**
   * 修订人姓名（冗余字段，便于查询）
   */
  revisedByName?: string;

  /**
   * 修订时间
   */
  revisedAt: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * 更新DocumentVersion DTO
 * 继承 TaktDocumentVersionCreateDto，添加 DocumentVersionId 字段
 * 对应前端 DocumentVersionUpdate
 * @description 对应后端 TaktDocumentVersionUpdateDto
 */
export interface DocumentVersionUpdate extends DocumentVersionCreate {
  /**
   * DocumentVersionID（标识要更新的实体）
   */
  documentVersionId: string;

}


/**
 * DocumentVersion 作废/撤销作废 DTO
 * 对应前端 DocumentVersionObsolete
 * @description 对应后端 TaktDocumentVersionObsoleteDto
 */
export interface DocumentVersionObsolete {
  /**
   * DocumentVersionID
   */
  documentVersionId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * DocumentVersion 导入模板行 DTO
 * 对应前端 DocumentVersionTemplate
 * @description 对应后端 TaktDocumentVersionTemplateDto
 */
export interface DocumentVersionTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode?: string;

  /**
   * 文档 ID（选项 TaktDocuments/options；DictValue=Id）
   */
  documentId?: string;

  /**
   * 行号（固定步长=10）
   */
  lineNumber?: number;

  /**
   * 版本号
   */
  versionNo?: number;

  /**
   * 版本说明
   */
  versionNote?: string;

  /**
   * 文件 ID（选项 TaktFiles/options；DictValue=Id；上传下载元数据在 TaktFile）
   */
  fileId?: string;

  /**
   * 修订人 ID（选项 TaktUsers/options；DictValue=Id）
   */
  revisedBy?: string;

  /**
   * 修订人姓名（冗余字段，便于查询）
   */
  revisedByName?: string;

  /**
   * 修订时间
   */
  revisedAt?: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * DocumentVersion 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 DocumentVersionImport
 * @description 对应后端 TaktDocumentVersionImportDto
 */
export interface DocumentVersionImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode?: string;

  /**
   * 文档 ID（选项 TaktDocuments/options；DictValue=Id）
   */
  documentId?: string;

  /**
   * 行号（固定步长=10）
   */
  lineNumber?: number;

  /**
   * 版本号
   */
  versionNo?: number;

  /**
   * 版本说明
   */
  versionNote?: string;

  /**
   * 文件 ID（选项 TaktFiles/options；DictValue=Id；上传下载元数据在 TaktFile）
   */
  fileId?: string;

  /**
   * 修订人 ID（选项 TaktUsers/options；DictValue=Id）
   */
  revisedBy?: string;

  /**
   * 修订人姓名（冗余字段，便于查询）
   */
  revisedByName?: string;

  /**
   * 修订时间
   */
  revisedAt?: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * DocumentVersion 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 DocumentVersionExport
 * @description 对应后端 TaktDocumentVersionExportDto
 */
export interface DocumentVersionExport {
  /**
   * DocumentVersionID
   */
  documentVersionId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 文档 ID（选项 TaktDocuments/options；DictValue=Id）
   */
  documentId: string;

  /**
   * 行号（固定步长=10）
   */
  lineNumber: number;

  /**
   * 版本号
   */
  versionNo: number;

  /**
   * 版本说明
   */
  versionNote?: string;

  /**
   * 文件 ID（选项 TaktFiles/options；DictValue=Id；上传下载元数据在 TaktFile）
   */
  fileId: string;

  /**
   * 修订人 ID（选项 TaktUsers/options；DictValue=Id）
   */
  revisedBy: string;

  /**
   * 修订人姓名（冗余字段，便于查询）
   */
  revisedByName?: string;

  /**
   * 修订时间
   */
  revisedAt: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

