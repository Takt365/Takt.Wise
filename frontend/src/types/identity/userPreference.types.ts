// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/identity
// 文件名称：userPreference.types.ts
// 创建时间：2026-09-11
// 创建人：Takt365(Auto Generated)
// 功能描述：identity 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 用户个人偏好实体 公司级：租户+公司+UserId 唯一一行，PreferenceJson 存完整 AppSetting
 * 对应前端 TaktUserPreferenceDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 UserPreference
 * @description 对应后端 TaktUserPreferenceDto
 */
export interface UserPreference extends CompanyDtoBase {
  /**
   * UserPreferenceID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  userPreferenceId: string;

  /**
   * 用户 ID（关联 TaktUser.Id；唯一索引：租户+公司内唯一，见 ix_user_preference_user_id_unique）
   */
  userId: string;

  /**
   * 用户 名称（填充字段）
   */
  userName?: string;

  /**
   * 偏好 JSON（完整 AppSetting 序列化；超长配置用 ntext）
   */
  preferenceJson: string;

  /**
   * 用户布局（冗余热字段；如 side、top、mix、content）
   */
  userLayout: string;

  /**
   * 用户主题（冗余热字段；如 light、dark）
   */
  userTheme: string;

  /**
   * 用户颜色（冗余热字段；themeColor.type 如 blue、green、custom）
   */
  userColor: string;

  /**
   * 用户界面语言（冗余热字段；BCP47 如 zh-CN、en-US、ja-JP；≠ 基类 cultureCode）
   */
  userLocale: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  userPreferenceStatus: number;

}


/**
 * UserPreference 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 UserPreferenceQuery
 * @description 对应后端 TaktUserPreferenceQueryDto
 */
export interface UserPreferenceQuery extends TaktPagedQuery {
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
   * 用户 ID（关联 TaktUser.Id；唯一索引：租户+公司内唯一，见 ix_user_preference_user_id_unique）
   */
  userId?: string;

  /**
   * 偏好 JSON（完整 AppSetting 序列化；超长配置用 ntext）
   */
  preferenceJson?: string;

  /**
   * 用户布局（冗余热字段；如 side、top、mix、content）
   */
  userLayout?: string;

  /**
   * 用户主题（冗余热字段；如 light、dark）
   */
  userTheme?: string;

  /**
   * 用户颜色（冗余热字段；themeColor.type 如 blue、green、custom）
   */
  userColor?: string;

  /**
   * 用户界面语言（冗余热字段；BCP47 如 zh-CN、en-US、ja-JP；≠ 基类 cultureCode）
   */
  userLocale?: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  userPreferenceStatus?: number;

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
 * 创建UserPreference DTO
 * 对应前端 UserPreferenceCreate
 * @description 对应后端 TaktUserPreferenceCreateDto
 */
export interface UserPreferenceCreate {
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
   * 用户 ID（关联 TaktUser.Id；唯一索引：租户+公司内唯一，见 ix_user_preference_user_id_unique）
   */
  userId: string;

  /**
   * 偏好 JSON（完整 AppSetting 序列化；超长配置用 ntext）
   */
  preferenceJson: string;

  /**
   * 用户布局（冗余热字段；如 side、top、mix、content）
   */
  userLayout: string;

  /**
   * 用户主题（冗余热字段；如 light、dark）
   */
  userTheme: string;

  /**
   * 用户颜色（冗余热字段；themeColor.type 如 blue、green、custom）
   */
  userColor: string;

  /**
   * 用户界面语言（冗余热字段；BCP47 如 zh-CN、en-US、ja-JP；≠ 基类 cultureCode）
   */
  userLocale: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  userPreferenceStatus: number;

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
 * 更新UserPreference DTO
 * 继承 TaktUserPreferenceCreateDto，添加 UserPreferenceId 字段
 * 对应前端 UserPreferenceUpdate
 * @description 对应后端 TaktUserPreferenceUpdateDto
 */
export interface UserPreferenceUpdate extends UserPreferenceCreate {
  /**
   * UserPreferenceID（标识要更新的实体）
   */
  userPreferenceId: string;

}


/**
 * UserPreference 状态更新 DTO
 * 对应前端 UserPreferenceStatus
 * @description 对应后端 TaktUserPreferenceStatusDto
 */
export interface UserPreferenceStatus {
  /**
   * UserPreferenceID
   */
  userPreferenceId: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  userPreferenceStatus: number;

}


/**
 * UserPreference 导入模板行 DTO
 * 对应前端 UserPreferenceTemplate
 * @description 对应后端 TaktUserPreferenceTemplateDto
 */
export interface UserPreferenceTemplate {
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
   * 用户 ID（关联 TaktUser.Id；唯一索引：租户+公司内唯一，见 ix_user_preference_user_id_unique）
   */
  userId?: string;

  /**
   * 偏好 JSON（完整 AppSetting 序列化；超长配置用 ntext）
   */
  preferenceJson?: string;

  /**
   * 用户布局（冗余热字段；如 side、top、mix、content）
   */
  userLayout?: string;

  /**
   * 用户主题（冗余热字段；如 light、dark）
   */
  userTheme?: string;

  /**
   * 用户颜色（冗余热字段；themeColor.type 如 blue、green、custom）
   */
  userColor?: string;

  /**
   * 用户界面语言（冗余热字段；BCP47 如 zh-CN、en-US、ja-JP；≠ 基类 cultureCode）
   */
  userLocale?: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  userPreferenceStatus?: number;

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
 * UserPreference 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 UserPreferenceImport
 * @description 对应后端 TaktUserPreferenceImportDto
 */
export interface UserPreferenceImport {
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
   * 用户 ID（关联 TaktUser.Id；唯一索引：租户+公司内唯一，见 ix_user_preference_user_id_unique）
   */
  userId?: string;

  /**
   * 偏好 JSON（完整 AppSetting 序列化；超长配置用 ntext）
   */
  preferenceJson?: string;

  /**
   * 用户布局（冗余热字段；如 side、top、mix、content）
   */
  userLayout?: string;

  /**
   * 用户主题（冗余热字段；如 light、dark）
   */
  userTheme?: string;

  /**
   * 用户颜色（冗余热字段；themeColor.type 如 blue、green、custom）
   */
  userColor?: string;

  /**
   * 用户界面语言（冗余热字段；BCP47 如 zh-CN、en-US、ja-JP；≠ 基类 cultureCode）
   */
  userLocale?: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  userPreferenceStatus?: number;

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
 * UserPreference 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 UserPreferenceExport
 * @description 对应后端 TaktUserPreferenceExportDto
 */
export interface UserPreferenceExport {
  /**
   * UserPreferenceID
   */
  userPreferenceId: string;

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
   * 用户 ID（关联 TaktUser.Id；唯一索引：租户+公司内唯一，见 ix_user_preference_user_id_unique）
   */
  userId: string;

  /**
   * 偏好 JSON（完整 AppSetting 序列化；超长配置用 ntext）
   */
  preferenceJson: string;

  /**
   * 用户布局（冗余热字段；如 side、top、mix、content）
   */
  userLayout: string;

  /**
   * 用户主题（冗余热字段；如 light、dark）
   */
  userTheme: string;

  /**
   * 用户颜色（冗余热字段；themeColor.type 如 blue、green、custom）
   */
  userColor: string;

  /**
   * 用户界面语言（冗余热字段；BCP47 如 zh-CN、en-US、ja-JP；≠ 基类 cultureCode）
   */
  userLocale: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  userPreferenceStatus: number;

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

