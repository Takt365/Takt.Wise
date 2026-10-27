// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/foundation
// 文件名称：numbering.types.ts
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：foundation 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

  /**
   * 起始编码（新增时必填；完整业务编码样例，末段为当前流水号） 如：SO-20250120-000001；生成编码后会更新为最近一次产出编码
   */
  startingNumber: string;

  /**
   * 分隔符（空=段直接拼接；-=连字符分隔，默认 -）
   */
  separator?: string;

  /**
   * 内置（字典 sys_yes_no；0=否 1=是）
   */
  isBuiltIn: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  numberingStatus: number;

  /**
   * 描述说明；可选配置编码段顺序，格式：segments:CompanyCode,DeptCode,PrefixCode,DateSequence（段名为实体属性名）
   */
  numberingDescription?: string;

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

