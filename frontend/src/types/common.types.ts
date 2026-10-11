// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types
// 文件名称：common.types.ts
// 创建时间：2026-05-22
// 创建人：Takt365(Cursor AI)
// 功能描述：通用类型定义（API 响应、分页、实体基类等；实体基类字段序 id → relatedPlant|plantCode → 隔离/审计，对齐 Domain）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { FormInstance } from 'ant-design-vue';
import type { Ref } from 'vue';
import type { SignalRMessage } from '@/types/foundation/signalR.types';
import type {
  TAKT_CAPTCHA_DISABLED_HINTS,
  TAKT_IDLE_ACTIVITY_EVENTS,
  TaktMenuType,
  TaktResultCode,
  TAKT_REMIX_ICON_CLASS,
  TAKT_REMIX_ICON_LG_CLASS,
  TAKT_REMIX_ICON_SM_CLASS,
  TAKT_REMIX_ICON_XL_CLASS,
} from '@/utils/common';

/**
 * Remix 图标 CSS 类名联合类型（与 {@link TAKT_REMIX_ICON_CLASS} 等常量一致）
 */
export type TaktRemixIconClass =
  | typeof TAKT_REMIX_ICON_CLASS
  | typeof TAKT_REMIX_ICON_SM_CLASS
  | typeof TAKT_REMIX_ICON_LG_CLASS
  | typeof TAKT_REMIX_ICON_XL_CLASS;

/**
 * 菜单类型数值（与 {@link TaktMenuType} 一致）
 */
export type TaktMenuTypeValue = TaktMenuType;

/**
 * 字典转下拉选项时的字段映射
 */
export interface TaktDictSelectFieldNames {
  /** 作为 label 的字典字段 */
  labelField: 'dictLabel' | 'extLabel';
  /** 作为 value 的字典字段 */
  valueField: 'dictValue' | 'extLabel' | 'extValue' | 'sortOrder';
}

/**
 * 供 a-select 使用的字典选项（含 label / value，并保留原始字典字段）
 */
export type TaktDictSelectOption = TaktSelectOption & {
  label: string;
  value: string | number;
};

/**
 * SignalR 消息（含可选 messageId，供 store 去重）
 */
export type SignalRMessageWithId = SignalRMessage & { messageId?: string | number };

/**
 * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
 */
export type TaktCultureCode = string;

/**
 * 验证码未启用错误文案片段
 */
export type TaktCaptchaDisabledHint = (typeof TAKT_CAPTCHA_DISABLED_HINTS)[number];

/**
 * 空闲会话活动事件名
 */
export type TaktIdleActivityEvent = (typeof TAKT_IDLE_ACTIVITY_EVENTS)[number];

/**
 * v-permission 指令绑定值
 */
export type TaktPermissionBindingValue = string | string[];

/**
 * 登录表单位置
 */
export type TaktLoginLayoutPosition = 'left' | 'center' | 'right';

/**
 * OAuth 客户端配置
 */
export interface TaktOAuthConfig {
  /** 颁发者 / 根地址 */
  issuer: string;
  /** 客户端 ID */
  clientId: string;
  /** 回调地址 */
  redirectUri: string;
  /** 授权范围 */
  scope: string;
  /** 授权端点 */
  authorizationEndpoint: string;
  /** 令牌端点 */
  tokenEndpoint: string;
}

/**
 * useLoginFieldSync 配置
 */
export interface UseLoginFieldSyncOptions {
  /** 读取租户编码 */
  getTenantCode: () => string;
  /** 写回规范化租户编码 */
  setTenantCode: (code: string) => void;
  /** 读取用户名 */
  getUserName: () => string;
  /** 写回规范化用户名 */
  setUserName: (userName: string) => void;
  /** 登录表单实例 */
  formRef: Ref<FormInstance | undefined>;
}

/**
 * 验证码子组件（Slider / Behavior）通过 defineExpose 暴露的实例形状
 */
export interface TaktCaptchaPanelExpose {
  buildCaptchaCode: () => string;
  canSubmit: { value: boolean };
}

/**
 * useTaktLoginCaptcha 可选配置
 */
export interface UseTaktLoginCaptchaOptions {
  /** 拼图/滑轨验证通过后自动继续（无需点击确认） */
  onVerified?: () => void | Promise<void>;
  /** 验证码未启用时静默跳过（不提示用户） */
  onCaptchaSkipped?: () => void | Promise<void>;
}

/**
 * 二进制下载（Excel/ZIP 等），含响应头中的文件名与 MIME（供 resolveExportDownloadFileName）
 */
export interface TaktBinaryDownload {
  /**
   * 文件二进制
   */
  blob: Blob;

  /**
   * 响应头 Content-Disposition（原始字符串）
   */
  contentDisposition: string | null;

  /**
   * 响应头 Content-Type
   */
  contentType: string | null;
}

/**
 * Takt API 统一返回结果（与后端 Takt.Shared.Models.TaktApiResult&lt;T&gt; 一致，JSON 为 camelCase）
 */
export interface TaktApiResult<T = unknown> {
  /**
   * 结果代码（成功为 TaktResultCode.Success = 200）
   */
  code: TaktResultCode;

  /**
   * 提示消息
   */
  message: string;

  /**
   * 业务数据（失败时常为 null）
   */
  data: T | null;

  /**
   * 是否成功（后端计算属性，code === 200 时为 true）
   */
  success?: boolean;
}

/**
 * 分页全局配置（来源 appsettings Paged，由 GET TaktWise/pagination 下发）
 */
export interface TaktPaginationConfig {
  /** 默认页码（从 1 开始） */
  defaultPageIndex: number;
  /** 默认每页条数 */
  defaultPageSize: number;
  /** 列表 pageSize 上限 */
  maxPageSize: number;
  /** TaktPagination 可选每页条数 */
  pageSizeOptions: string[];
}

/**
 * 分页查询基类
 * 对应后端 TaktPagedQuery（默认值来自 appsettings Paged）
 */
export interface TaktPagedQuery {
  /**
   * 当前页码（从1开始）
   */
  pageIndex: number;

  /**
   * 每页大小（默认见 appsettings Paged:DefaultPageSize）
   */
  pageSize: number;

  /**
   * 关键词（用于模糊查询，在多个字段中搜索）
   */
  keyWords?: string;
}

/**
 * 分页结果
 * 对应后端 TaktPagedResult<T>
 */
export interface TaktPagedResult<T> {
  /**
   * 数据列表
   */
  data: T[];

  /**
   * 总记录数
   */
  total: number;

  /**
   * 当前页码（从1开始）
   */
  pageIndex: number;

  /**
   * 每页大小
   */
  pageSize: number;

  /**
   * 总页数
   */
  totalPages: number;

  /**
   * 是否有上一页
   */
  hasPreviousPage: boolean;

  /**
   * 是否有下一页
   */
  hasNextPage: boolean;
}

/**
 * Takt 下拉选择框选项（对应后端 TaktSelectOption，与 TaktDictData 一致）
 */
export interface TaktSelectOption {
  /**
   * 字典标签
   */
  dictLabel: string;

  /**
   * 字典键值
   */
  dictValue: string | number;

  /**
   * 国际化键（用于多语言翻译，与 TaktDictData.i18nKey 一致）
   */
  i18nKey?: string;

  /**
   * 字典类型编码（用于批量加载时前端分组，单个查询时通常为空）
   */
  dictTypeCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 扩展标签
   */
  extLabel?: string;

  /**
   * 扩展键值
   */
  extValue?: string | number;

  /**
   * CSS类名
   */
  cssClass?: number;

  /**
   * 列表类名
   */
  listClass?: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 是否默认项（1=是，0=否；与 TaktDictData.IsDefault / sys_yes_no 一致）
   */
  isDefault?: number;
}

/**
 * Takt 树形下拉选择框选项（对应后端 TaktTreeSelectOption，通用树形结构，适用于部门、会计科目、菜单等）
 */
export interface TaktTreeSelectOption extends TaktSelectOption {
  /**
   * 子节点列表（懒加载：非叶子保持 undefined，勿传空数组）
   */
  children?: TaktTreeSelectOption[];

  /**
   * 是否叶子（懒加载树；true 不可展开）
   */
  isLeaf?: boolean;
}

/**
 * 租户级实体基类
 * 对应后端 TaktTenantEntityBase / TaktTenantDtoBase
 * 字段序对齐 Domain CodeFirst：id → relatedPlant → cultureCode → tenantCode …
 * 仅租户隔离 + RelatedPlant + CultureCode（不含公司隔离）
 * 适用于用户、角色、菜单等跨公司共享的实体
 */
export interface TaktTenantEntityBase {
  /**
   * 主键ID（对应后端long型，前端用string）
   */
  id: string;

  /**
   * 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  relatedPlant: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 租户编码
   */
  tenantCode: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建人ID
   */
  createdBy: string;

  /**
   * 创建时间
   */
  createdAt: string;

  /**
   * 更新人ID
   */
  updatedBy?: string;

  /**
   * 更新时间
   */
  updatedAt?: string;

  /**
   * 是否删除(0=未删除,1=已删除)
   */
  isDeleted: number;

  /**
   * 删除人ID
   */
  deletedBy?: string;

  /**
   * 删除时间
   */
  deletedAt?: string;
}

/**
 * 公司级实体基类
 * 对应后端 TaktCompanyEntityBase / TaktCompanyDtoBase
 * 字段序对齐 Domain CodeFirst：id → plantCode → tenantCode → companyCode → cultureCode …
 * 适用于部门、岗位、员工等业务实体
 */
export interface TaktCompanyEntityBase {
  /**
   * 主键ID（对应后端long型，前端用string）
   */
  id: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
   */
  plantCode: string;

  /**
   * 租户编码
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
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建人ID
   */
  createdBy: string;

  /**
   * 创建时间
   */
  createdAt: string;

  /**
   * 更新人ID
   */
  updatedBy?: string;

  /**
   * 更新时间
   */
  updatedAt?: string;

  /**
   * 是否删除(0=未删除,1=已删除)
   */
  isDeleted: number;

  /**
   * 删除人ID
   */
  deletedBy?: string;

  /**
   * 删除时间
   */
  deletedAt?: string;
}

/**
 * 审批级实体基类
 * 对应后端 TaktApprovalEntityBase / TaktApprovalDtoBase
 * 字段序对齐 Domain CodeFirst：id → plantCode → tenantCode → companyCode → cultureCode …
 * 适用于需要审批的业务实体，如：请假单、报销单、采购单、合同等
 */
export interface TaktApprovalEntityBase {
  /**
   * 主键ID（对应后端long型，前端用string）
   */
  id: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
   */
  plantCode: string;

  /**
   * 租户编码
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
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 审批状态（字典 sys_approval_status；0=待审批，1=审批中，2=已通过，3=已驳回，4=已撤回，5=已终止）
   */
  approvalStatus: number;

  /**
   * 发起人ID
   */
  initiatorId?: string;

  /**
   * 发起时间
   */
  initiatedAt?: string;

  /**
   * 审批意见（支持多级审批时多条意见，用JSON数组存储）
   */
  approvalOpinion?: string;

  /**
   * 最终审批人ID
   */
  approvedBy?: string;

  /**
   * 最终审批时间
   */
  approvedAt?: string;

  /**
   * 流程实例 ID（关联 takt_workflow_instance；StartFlowInstance 后由业务写入）
   */
  flowInstanceId?: string;

  /**
   * 创建人ID
   */
  createdBy: string;

  /**
   * 创建时间
   */
  createdAt: string;

  /**
   * 更新人ID
   */
  updatedBy?: string;

  /**
   * 更新时间
   */
  updatedAt?: string;

  /**
   * 是否删除(0=未删除,1=已删除)
   */
  isDeleted: number;

  /**
   * 删除人ID
   */
  deletedBy?: string;

  /**
   * 删除时间
   */
  deletedAt?: string;
}

/**
 * 租户组合 4 DTO 基类（对应后端 TaktTenantCoreDtoBase：无关联工厂、无语言）
 */
export type TenantCoreDtoBase = {
  /**
   * 租户编码
   */
  tenantCode: string;
} & Omit<TaktTenantEntityBase, 'id' | 'relatedPlant' | 'cultureCode' | 'tenantCode'>;

/**
 * 租户组合 2 DTO 基类（对应后端 TaktTenantCultureDtoBase：无关联工厂、有语言）
 */
export type TenantCultureDtoBase = {
  /**
   * 租户编码
   */
  tenantCode: string;
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;
} & Omit<TaktTenantEntityBase, 'id' | 'relatedPlant' | 'cultureCode' | 'tenantCode'>;

/**
 * 租户组合 3 DTO 基类（对应后端 TaktTenantPlantDtoBase：有关联工厂、无语言）
 */
export type TenantPlantDtoBase = {
  /**
   * 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  relatedPlant: string;
  /**
   * 租户编码
   */
  tenantCode: string;
} & Omit<TaktTenantEntityBase, 'id' | 'relatedPlant' | 'cultureCode' | 'tenantCode'>;

/**
 * 租户级 DTO 基类（对应后端 TaktTenantDtoBase，公共字段对齐 TaktTenantEntityBase，不含实体主键 id）
 * 无 id 时 relatedPlant 仍居首（对齐 Domain：主键后即关联工厂）
 */
export type TenantDtoBase = {
  /**
   * 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  relatedPlant: string;
  /**
   * 租户编码
   */
  tenantCode: string;
} & Omit<TaktTenantEntityBase, 'id' | 'relatedPlant' | 'tenantCode'>;

/**
 * 公司级 DTO 基类（对应后端 TaktCompanyDtoBase）
 * 无 id 时 plantCode 仍居首
 */
export type CompanyDtoBase = {
  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;
} & Omit<TaktCompanyEntityBase, 'id' | 'plantCode'>;

/**
 * 审批级 DTO 基类（对应后端 TaktApprovalDtoBase）
 * 无 id 时 plantCode 仍居首
 */
export type ApprovalDtoBase = {
  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;
} & Omit<TaktApprovalEntityBase, 'id' | 'plantCode'>;
