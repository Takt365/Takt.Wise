// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/foundation
// 文件名称：file.types.ts
// 创建时间：2026-06-13
// 创建人：Takt365(Auto Generated)
// 功能描述：foundation 模块类型定义（自动生成 CRUD + 文件上传/分片协议类型；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 文件实体 公司级实体：文件元数据按租户+公司隔离；字段与前端 entity.file.* 及业务附件 JSON 结构对齐
 * 对应前端 TaktFileDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 File
 * @description 对应后端 TaktFileDto
 */
export interface File extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 文件编码（唯一索引：租户+公司内唯一，见 ix_file_code_unique）
   */
  fileCode?: string;

  /**
   * 文件名称（字典 sys_storage_naming；0=原文件+哈希值 1=自动生成 2=自定义）
   */
  fileName?: string;

  /**
   * 文件原始名称（上传时的原始文件名）
   */
  fileOriginalName?: string;

  /**
   * 文件路径（关联一级菜单 uploadPath，选项 useMenuUploadPath）
   */
  filePath?: string;

  /**
   * 文件大小（字节）
   */
  fileSize?: string;

  /**
   * 文件 MIME 类型
   */
  fileType?: string;

  /**
   * 文件扩展名
   */
  fileExtension?: string;

  /**
   * 文件哈希值（MD5 或 SHA256，用于去重与校验）
   */
  fileHash?: string;

  /**
   * 文件分类（根据 FileType/MIME 自动推断：0=文档，1=图片，2=视频，3=音频，4=压缩包，5=其他）
   */
  fileCategory?: number;

  /**
   * 存储方式（字典 sys_storage_type；0=本地存储 1=OSS对象存储 2=FTP）
   */
  storageType?: number;

  /**
   * 存储配置（JSON，OSS/FTP 等扩展配置）
   */
  storageConfig?: string;

  /**
   * 访问地址（文件 URL）
   */
  accessUrl?: string;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

}

/**
 * File 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 FileExport
 * @description 对应后端 TaktFileExportDto
 */
export interface FileExport {
  /**
   * FileID
   */
  fileId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 文件编码（唯一索引：租户+公司内唯一，见 ix_file_code_unique）
   */
  fileCode: string;

  /**
   * 文件名称（字典 sys_storage_naming；0=原文件+哈希值 1=自动生成 2=自定义）
   */
  fileName: string;

  /**
   * 文件原始名称（上传时的原始文件名）
   */
  fileOriginalName: string;

  /**
   * 文件路径（关联一级菜单 uploadPath，选项 useMenuUploadPath）
   */
  filePath: string;

  /**
   * 文件大小（字节）
   */
  fileSize: string;

  /**
   * 文件 MIME 类型
   */
  fileType: string;

  /**
   * 文件扩展名
   */
  fileExtension: string;

  /**
   * 文件哈希值（MD5 或 SHA256，用于去重与校验）
   */
  fileHash: string;

  /**
   * 文件分类（根据 FileType/MIME 自动推断：0=文档，1=图片，2=视频，3=音频，4=压缩包，5=其他）
   */
  fileCategory: number;

  /**
   * 存储方式（字典 sys_storage_type；0=本地存储 1=OSS对象存储 2=FTP）
   */
  storageType: number;

  /**
   * 存储配置（JSON，OSS/FTP 等扩展配置）
   */
  storageConfig?: string;

  /**
   * 访问地址（文件 URL）
   */
  accessUrl: string;

  /**
   * 下载次数
   */
  downloadCount: number;

  /**
   * 最后下载时间
   */
  lastDownloadTime?: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  fileStatus: number;

  /**
   * 是否公开（字典 sys_public_type；0=公开同公司可见，1=私有仅创建人可见/可改/可下载）
   */
  isPublic: number;

  /**
   * 文件描述
   */
  fileDescription: string;

  /**
   * 文件标签（多个标签用逗号分隔）
   */
  fileTags: string;

  /**
   * IP 地址（上传或访问来源）
   */
  ipAddress: string;

  /**
   * 位置（IP 对应地理位置）
   */
  location: string;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

/**
 * 上传策略（GET TaktFiles/upload-policy）
 */
export interface FileUploadPolicy {
  /** 单文件最大字节数 */
  maxFileSizeBytes: string | number;
  /** 最大分片数 */
  maxChunkCount: number;
  /** 默认分片大小（字节） */
  defaultChunkSizeBytes: string | number;
  /** 分片上传阈值（字节） */
  chunkThresholdBytes: string | number;
  /** 分片临时目录（相对 wwwroot） */
  chunkRelativePath: string;
  /** 允许扩展名（小写、不含点） */
  allowedExtensions: string[];
  /** 禁止扩展名（小写、不含点） */
  deniedExtensions: string[];
  /** 传入 totalSize 时：是否应分片上传 */
  useChunkUpload?: boolean;
  /** 传入 totalSize 时：分片大小（字节） */
  chunkSizeBytes?: string | number;
  /** 传入 totalSize 时：总分片数 */
  totalChunks?: number;
  /** 传入 totalSize 时：文件总大小（字节） */
  totalSizeBytes?: string | number;
}

/**
 * 整文件/合并上传附带业务元数据
 */
export interface FileUploadMeta {
  fileDescription?: string;
  fileTags?: string;
  isPublic?: number;
  fileStatus?: number;
  fileUploadType?: number;
  targetFileName?: string;
  categoryPath?: string;
  storageType?: number;
  storageNaming?: number;
  storageConfig?: string;
}

/**
 * 上传完成结果（存储层 + 可选业务 fileId）
 */
export interface FileUploadResult {
  fileId?: string;
  fileCode?: string;
  fileName?: string;
  fileOriginalName?: string;
  filePath?: string;
  fileSize?: string | number;
  fileType?: string;
  fileExtension?: string;
  fileHash?: string;
  fileCategory?: number;
  storageType?: number;
  storageConfig?: string;
  accessUrl?: string;
}

/**
 * 分片存在性检查请求
 */
export interface FileChunkCheck {
  identifier: string;
  chunkNumber: number;
  chunkSize: string | number;
  totalSize: string | number;
  totalChunks?: number;
  fileName?: string;
}

/**
 * 分片存在性检查结果
 */
export interface FileChunkCheckResult {
  exists: boolean;
}

/**
 * 已上传分片列表查询
 */
export interface FileChunkList {
  identifier: string;
  totalChunks?: number;
  totalSize: string | number;
}

/**
 * 已上传分片列表结果
 */
export interface FileChunkListResult {
  uploadedChunkNumbers: number[];
}

/**
 * 分片上传表单元数据
 */
export interface FileChunkUpload {
  identifier: string;
  chunkNumber: number;
  totalChunks: number;
  chunkSize: string | number;
  totalSize: string | number;
  fileName: string;
}

/**
 * 分片合并请求（含业务元数据）
 */
export interface FileChunkMerge {
  identifier: string;
  fileName: string;
  totalChunks: number;
  totalSize: string | number;
  fileDescription?: string;
  fileTags?: string;
  isPublic?: number;
  fileUploadType?: number;
  targetFileName?: string;
  categoryPath?: string;
  storageType?: number;
  storageConfig?: string;
  storageNaming?: number;
  fileStatus?: number;
}

/**
 * 取消分片上传
 */
export interface FileChunkCancel {
  identifier: string;
}