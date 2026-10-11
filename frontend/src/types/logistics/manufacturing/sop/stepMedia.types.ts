// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/sop
// 文件名称：stepMedia.types.ts
// 创建时间：2026-08-12
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/sop 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * SOP 工步多媒体实体
 * 对应前端 TaktSopStepMediaDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SopStepMedia
 * @description 对应后端 TaktSopStepMediaDto
 */
export interface SopStepMedia extends CompanyDtoBase {
  /**
   * SopStepMediaID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  sopStepMediaId: string;

  /**
   * 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
   */
  stepId: string;

  /**
   * 工步 名称（填充字段）
   */
  stepName?: string;

  /**
   * 媒体类型（字典 logistics_sop_media_type；1=图片JPG/PNG，2=视频MP4，3=PDF，4=3D轻量化）
   */
  mediaType: number;

  /**
   * 文件 URL
   */
  fileUrl: string;

  /**
   * 文件扩展名（jpg/png/mp4/pdf/glb 等）
   */
  fileExt?: string;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 工步 （主表：TaktSopStep）
   */
  step?: SopStep;

}


/**
 * SopStepMedia 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SopStepMediaQuery
 * @description 对应后端 TaktSopStepMediaQueryDto
 */
export interface SopStepMediaQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
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
   * 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
   */
  stepId?: string;

  /**
   * 媒体类型（字典 logistics_sop_media_type；1=图片JPG/PNG，2=视频MP4，3=PDF，4=3D轻量化）
   */
  mediaType?: number;

  /**
   * 文件 URL
   */
  fileUrl?: string;

  /**
   * 文件扩展名（jpg/png/mp4/pdf/glb 等）
   */
  fileExt?: string;

  /**
   * 排序号
   */
  sortOrder?: number;

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
 * 创建SopStepMedia DTO
 * 对应前端 SopStepMediaCreate
 * @description 对应后端 TaktSopStepMediaCreateDto
 */
export interface SopStepMediaCreate {
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
   * 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
   */
  stepId: string;

  /**
   * 媒体类型（字典 logistics_sop_media_type；1=图片JPG/PNG，2=视频MP4，3=PDF，4=3D轻量化）
   */
  mediaType: number;

  /**
   * 文件 URL
   */
  fileUrl: string;

  /**
   * 文件扩展名（jpg/png/mp4/pdf/glb 等）
   */
  fileExt?: string;

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
 * 更新SopStepMedia DTO
 * 继承 TaktSopStepMediaCreateDto，添加 SopStepMediaId 字段
 * 对应前端 SopStepMediaUpdate
 * @description 对应后端 TaktSopStepMediaUpdateDto
 */
export interface SopStepMediaUpdate extends SopStepMediaCreate {
  /**
   * SopStepMediaID（标识要更新的实体）
   */
  sopStepMediaId: string;

}


/**
 * SopStepMedia 排序更新 DTO
 * 对应前端 SopStepMediaSort
 * @description 对应后端 TaktSopStepMediaSortDto
 */
export interface SopStepMediaSort {
  /**
   * SopStepMediaID
   */
  sopStepMediaId: string;

  /**
   * 排序号
   */
  sortOrder: number;

}


/**
 * SopStepMedia 导入模板行 DTO
 * 对应前端 SopStepMediaTemplate
 * @description 对应后端 TaktSopStepMediaTemplateDto
 */
export interface SopStepMediaTemplate {
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
   * 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
   */
  stepId?: string;

  /**
   * 媒体类型（字典 logistics_sop_media_type；1=图片JPG/PNG，2=视频MP4，3=PDF，4=3D轻量化）
   */
  mediaType?: number;

  /**
   * 文件 URL
   */
  fileUrl?: string;

  /**
   * 文件扩展名（jpg/png/mp4/pdf/glb 等）
   */
  fileExt?: string;

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
 * SopStepMedia 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SopStepMediaImport
 * @description 对应后端 TaktSopStepMediaImportDto
 */
export interface SopStepMediaImport {
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
   * 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
   */
  stepId?: string;

  /**
   * 媒体类型（字典 logistics_sop_media_type；1=图片JPG/PNG，2=视频MP4，3=PDF，4=3D轻量化）
   */
  mediaType?: number;

  /**
   * 文件 URL
   */
  fileUrl?: string;

  /**
   * 文件扩展名（jpg/png/mp4/pdf/glb 等）
   */
  fileExt?: string;

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
 * SopStepMedia 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SopStepMediaExport
 * @description 对应后端 TaktSopStepMediaExportDto
 */
export interface SopStepMediaExport {
  /**
   * SopStepMediaID
   */
  sopStepMediaId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
   */
  stepId: string;

  /**
   * 媒体类型（字典 logistics_sop_media_type；1=图片JPG/PNG，2=视频MP4，3=PDF，4=3D轻量化）
   */
  mediaType: number;

  /**
   * 文件 URL
   */
  fileUrl: string;

  /**
   * 文件扩展名（jpg/png/mp4/pdf/glb 等）
   */
  fileExt?: string;

  /**
   * 排序号
   */
  sortOrder: number;

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

