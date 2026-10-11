// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/identity
// 文件名称：user.types.ts
// 创建时间：2026-09-11
// 创建人：Takt365(Auto Generated)
// 功能描述：identity 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  TaktPagedQuery,
  TenantCultureDtoBase
} from '@/types/common.types';

/**
 * 用户实体 代表系统登录账号（身份认证域） 注意：用户与员工档案分离，用户仅用于认证和权限控制
 * 对应前端 TaktUserDto
 * 继承 TaktTenantCultureDtoBase
 * 对应前端 User
 * @description 对应后端 TaktUserDto
 */
export interface User extends TenantCultureDtoBase {
  /**
   * UserID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  userId: string;

  /**
   * 用户名（唯一索引：租户内唯一，见 ix_user_name_unique；登录账号，4–20 位）
   */
  userName: string;

  /**
   * 昵称（显示名称，2–20 位）
   */
  nickName: string;

  /**
   * 用户类型（字典 sys_user_type）
   */
  userType: number;

  /**
   * 密码哈希值（bcrypt加密）
   */
  passwordHash: string;

  /**
   * 关联的员工ID（必须关联人事档案）
   */
  employeeId: string;

  /**
   * 关联的员工名称（填充字段）
   */
  employeeName?: string;

  /**
   * 内置（字典 sys_yes_no；种子用户 admin/guest/demo 为内置，不允许删除）
   */
  isBuiltIn: number;

  /**
   * 最后登录时间
   */
  lastLoginAt?: string;

  /**
   * 最后登录IP
   */
  lastLoginIp?: string;

  /**
   * 登录次数
   */
  loginCount: number;

  /**
   * 密码过期天数（0=永不过期，30=30天后过期）
   */
  passwordExpireDays: number;

  /**
   * 失败登录次数
   */
  loginFailCount: number;

  /**
   * 锁定时间（登录失败过多时锁定）
   */
  lockedUntil?: string;

  /**
   * 状态（字典 sys_normal_disable）
   */
  userStatus: number;

  /**
   * 用户角色关联（RBAC，表 takt_identity_user_role） （子表：TaktUserRole）
   */
  userRoles?: UserRole[];

  /**
   * 用户可访问租户关联（RBAC，表 takt_identity_user_tenant） （子表：TaktUserTenant）
   */
  userTenants?: UserTenant[];

  /**
   * 用户可访问公司关联（RBAC，表 takt_identity_user_company） （子表：TaktUserCompany）
   */
  userCompanies?: UserCompany[];

  /**
   * 角色 ID 列表（填充字段）
   */
  roleIds?: any;

  /**
   * 角色名称列表（填充字段）
   */
  roleNames?: any;

  /**
   * 可访问公司编码列表（填充字段）
   */
  companyCodes?: any;

}


/**
 * User 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 UserQuery
 * @description 对应后端 TaktUserQueryDto
 */
export interface UserQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 用户名（唯一索引：租户内唯一，见 ix_user_name_unique；登录账号，4–20 位）
   */
  userName?: string;

  /**
   * 昵称（显示名称，2–20 位）
   */
  nickName: string;

  /**
   * 用户类型（字典 sys_user_type）
   */
  userType?: number;

  /**
   * 密码哈希值（bcrypt加密）
   */
  passwordHash?: string;

  /**
   * 关联的员工ID（必须关联人事档案）
   */
  employeeId?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 内置（字典 sys_yes_no；种子用户 admin/guest/demo 为内置，不允许删除）
   */
  isBuiltIn?: number;

  /**
   * 最后登录时间（范围查询-开始）
   */
  lastLoginAtStart?: string;

  /**
   * 最后登录时间（范围查询-结束）
   */
  lastLoginAtEnd?: string;

  /**
   * 最后登录IP
   */
  lastLoginIp?: string;

  /**
   * 登录次数
   */
  loginCount?: number;

  /**
   * 密码过期天数（0=永不过期，30=30天后过期）
   */
  passwordExpireDays?: number;

  /**
   * 失败登录次数
   */
  loginFailCount?: number;

  /**
   * 锁定时间（登录失败过多时锁定）（范围查询-开始）
   */
  lockedUntilStart?: string;

  /**
   * 锁定时间（登录失败过多时锁定）（范围查询-结束）
   */
  lockedUntilEnd?: string;

  /**
   * 状态（字典 sys_normal_disable）
   */
  userStatus?: number;

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

  /**
   * 创建人 ID
   */
  createdBy?: string;

}


/**
 * 创建User DTO
 * 对应前端 UserCreate
 * @description 对应后端 TaktUserCreateDto
 */
export interface UserCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 用户名（唯一索引：租户内唯一，见 ix_user_name_unique；登录账号，4–20 位）
   */
  userName: string;

  /**
   * 昵称（显示名称，2–20 位）
   */
  nickName: string;

  /**
   * 用户类型（字典 sys_user_type）
   */
  userType: number;

  /**
   * 密码哈希值（bcrypt加密）
   */
  passwordHash: string;

  /**
   * 关联的员工ID（必须关联人事档案）
   */
  employeeId: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 内置（字典 sys_yes_no；种子用户 admin/guest/demo 为内置，不允许删除）
   */
  isBuiltIn: number;

  /**
   * 密码过期天数（0=永不过期，30=30天后过期）
   */
  passwordExpireDays: number;

  /**
   * 状态（字典 sys_normal_disable）
   */
  userStatus: number;

  /**
   * 用户角色关联（RBAC 全量覆盖，分配走 ITaktRbacService）
   */
  roleIds?: any;

  /**
   * 用户可访问租户关联（RBAC 全量覆盖，分配走 ITaktRbacService）
   */
  tenantCodes?: any;

  /**
   * 用户可访问公司关联（RBAC 全量覆盖，分配走 ITaktRbacService）
   */
  companyCodes?: any;

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
 * 更新User DTO
 * 继承 TaktUserCreateDto，添加 UserId 字段
 * 对应前端 UserUpdate
 * @description 对应后端 TaktUserUpdateDto
 */
export interface UserUpdate extends UserCreate {
  /**
   * UserID（标识要更新的实体）
   */
  userId: string;

}


/**
 * User 状态更新 DTO
 * 对应前端 UserStatus
 * @description 对应后端 TaktUserStatusDto
 */
export interface UserStatus {
  /**
   * UserID
   */
  userId: string;

  /**
   * 状态（字典 sys_normal_disable）
   */
  userStatus: number;

}


/**
 * User 导入模板行 DTO
 * 对应前端 UserTemplate
 * @description 对应后端 TaktUserTemplateDto
 */
export interface UserTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 用户名（唯一索引：租户内唯一，见 ix_user_name_unique；登录账号，4–20 位）
   */
  userName?: string;

  /**
   * 昵称（显示名称，2–20 位）
   */
  nickName: string;

  /**
   * 用户类型（字典 sys_user_type）
   */
  userType?: number;

  /**
   * 密码哈希值（bcrypt加密）
   */
  passwordHash?: string;

  /**
   * 关联的员工ID（必须关联人事档案）
   */
  employeeId?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 内置（字典 sys_yes_no；种子用户 admin/guest/demo 为内置，不允许删除）
   */
  isBuiltIn?: number;

  /**
   * 密码过期天数（0=永不过期，30=30天后过期）
   */
  passwordExpireDays?: number;

  /**
   * 状态（字典 sys_normal_disable）
   */
  userStatus?: number;

  /**
   * 用户角色关联（RBAC 全量覆盖，分配走 ITaktRbacService）
   */
  roleIds?: any;

  /**
   * 用户可访问租户关联（RBAC 全量覆盖，分配走 ITaktRbacService）
   */
  tenantCodes?: any;

  /**
   * 用户可访问公司关联（RBAC 全量覆盖，分配走 ITaktRbacService）
   */
  companyCodes?: any;

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
 * User 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 UserImport
 * @description 对应后端 TaktUserImportDto
 */
export interface UserImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 用户名（唯一索引：租户内唯一，见 ix_user_name_unique；登录账号，4–20 位）
   */
  userName?: string;

  /**
   * 昵称（显示名称，2–20 位）
   */
  nickName: string;

  /**
   * 用户类型（字典 sys_user_type）
   */
  userType?: number;

  /**
   * 密码哈希值（bcrypt加密）
   */
  passwordHash?: string;

  /**
   * 关联的员工ID（必须关联人事档案）
   */
  employeeId?: string;

  /**
   * 员工编码（导入时按编码匹配人事档案）
   */
  employeeCode?: string;

  /**
   * 用户类型名称（导入 Excel 字典标签）
   */
  userTypeName?: string;

  /**
   * 状态名称（导入 Excel 字典标签）
   */
  statusName?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 内置（字典 sys_yes_no；种子用户 admin/guest/demo 为内置，不允许删除）
   */
  isBuiltIn?: number;

  /**
   * 密码过期天数（0=永不过期，30=30天后过期）
   */
  passwordExpireDays?: number;

  /**
   * 状态（字典 sys_normal_disable）
   */
  userStatus?: number;

  /**
   * 用户角色关联（RBAC 全量覆盖，分配走 ITaktRbacService）
   */
  roleIds?: any;

  /**
   * 用户可访问租户关联（RBAC 全量覆盖，分配走 ITaktRbacService）
   */
  tenantCodes?: any;

  /**
   * 用户可访问公司关联（RBAC 全量覆盖，分配走 ITaktRbacService）
   */
  companyCodes?: any;

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
 * User 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 UserExport
 * @description 对应后端 TaktUserExportDto
 */
export interface UserExport {
  /**
   * UserID
   */
  userId: string;

  /**
   * 用户名（唯一索引：租户内唯一，见 ix_user_name_unique；登录账号，4–20 位）
   */
  userName: string;

  /**
   * 昵称（显示名称，2–20 位）
   */
  nickName: string;

  /**
   * 用户类型（字典 sys_user_type）
   */
  userType: number;

  /**
   * 密码哈希值（bcrypt加密）
   */
  passwordHash: string;

  /**
   * 关联的员工ID（必须关联人事档案）
   */
  employeeId: string;

  /**
   * 关联的员工名称（填充字段）
   */
  employeeName?: string;

  /**
   * 用户类型名称（导出字典标签）
   */
  userTypeName?: string;

  /**
   * 状态名称（导出字典标签）
   */
  statusName?: string;

  /**
   * 角色名称（导出填充，逗号分隔）
   */
  roleNames?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 内置（字典 sys_yes_no；种子用户 admin/guest/demo 为内置，不允许删除）
   */
  isBuiltIn: number;

  /**
   * 最后登录时间
   */
  lastLoginAt?: string;

  /**
   * 最后登录IP
   */
  lastLoginIp?: string;

  /**
   * 登录次数
   */
  loginCount: number;

  /**
   * 密码过期天数（0=永不过期，30=30天后过期）
   */
  passwordExpireDays: number;

  /**
   * 失败登录次数
   */
  loginFailCount: number;

  /**
   * 锁定时间（登录失败过多时锁定）
   */
  lockedUntil?: string;

  /**
   * 状态（字典 sys_normal_disable）
   */
  userStatus: number;

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


/**
 * 创建用户 DTO（与 TaktUserCreateDto 同义，供服务/控制器引用）
 * 对应前端 CreateUser
 * @description 对应后端 TaktCreateUserDto
 */
export interface CreateUser extends UserCreate {
}


/**
 * 更新用户 DTO（与 TaktUserUpdateDto 同义，供服务/控制器引用）
 * 对应前端 UpdateUser
 * @description 对应后端 TaktUpdateUserDto
 */
export interface UpdateUser extends UserUpdate {
}


/**
 * 重置密码 DTO（管理员按 UserId 重置）
 * 对应前端 ResetPassword
 * @description 对应后端 TaktResetPasswordDto
 */
export interface ResetPassword {
  /**
   * 用户 ID
   */
  userId: string;

  /**
   * 新密码
   */
  newPassword: string;

}


/**
 * 修改密码 DTO（当前登录用户修改自己的密码）
 * 对应前端 ChangePassword
 * @description 对应后端 TaktChangePasswordDto
 */
export interface ChangePassword {
  /**
   * 旧密码
   */
  oldPassword: string;

  /**
   * 新密码
   */
  newPassword: string;

  /**
   * 确认新密码
   */
  confirmPassword: string;

}

/**
 * 当前登录用户更新头像 DTO
 * @description 对应后端 TaktUpdateMyAvatarDto；空串清除头像
 */
export interface UpdateMyAvatar {
  /**
   * 头像访问 URL（空或空白表示清除）
   */
  avatar?: string | null;
}

/**
 * 当前登录用户更新个人资料 DTO
 * @description 对应后端 TaktUpdateMyProfileDto；仅 NickName→用户表，Mobile/Email→员工表
 */
export interface UpdateMyProfile {
  /**
   * 昵称（TaktUser.NickName）
   */
  nickName: string;

  /**
   * 手机号码（TaktEmployee.Mobile）
   */
  mobile?: string | null;

  /**
   * 电子邮箱（TaktEmployee.Email）
   */
  email?: string | null;
}

/**
 * 个人资料更新结果
 * @description 对应后端 TaktUpdateMyProfileResultDto
 */
export interface UpdateMyProfileResult {
  /**
   * 昵称
   */
  nickName: string;

  /**
   * 手机
   */
  mobile?: string | null;

  /**
   * 邮箱
   */
  email?: string | null;
}

/**
 * 忘记密码 DTO
 * 对应前端 ForgotPassword
 * @description 对应后端 TaktForgotPasswordDto
 */
export interface ForgotPassword {
  /**
   * 用户名或邮箱
   */
  userNameOrEmail: string;

}


/**
 * 忘记密码结果 DTO
 * 对应前端 ForgotPasswordResult
 * @description 对应后端 TaktForgotPasswordResultDto
 */
export interface ForgotPasswordResult {
  /**
   * 是否成功
   */
  success: boolean;

  /**
   * 错误码（Success 为 false 时有效）
   */
  code?: string;

  /**
   * 提示信息
   */
  message?: string;

}


/**
 * 解锁用户 DTO
 * 对应前端 UserUnlock
 * @description 对应后端 TaktUserUnlockDto
 */
export interface UserUnlock {
  /**
   * 用户 ID
   */
  userId: string;

  /**
   * 解锁原因
   */
  reason?: string;

}

