// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/sop
// 文件名称：sopStepMedia.types.ts
// 创建时间：2026-06-15
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
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 工步 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  stepId?: string;

  /**
   * 媒体类型（1=图片JPG/PNG，2=视频MP4，3=PDF，4=3D轻量化；字典 logistics_sop_media_type）
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
   * 工步 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  stepId: string;

  /**
   * 媒体类型（1=图片JPG/PNG，2=视频MP4，3=PDF，4=3D轻量化；字典 logistics_sop_media_type）
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

