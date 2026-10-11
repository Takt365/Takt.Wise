// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/identity
// 文件名称：login.types.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：identity 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================


/**
 * 登录密码 RSA 公钥响应
 * 对应前端 LoginPublicKeyResponse
 * @description 对应后端 TaktLoginPublicKeyResponseDto
 */
export interface LoginPublicKeyResponse {
  /**
   * 算法标识（RSA-PKCS1）
   */
  algorithm: string;

  /**
   * RSA 公钥 PEM
   */
  publicKeyPem: string;

}


/**
 * 用户登录请求 DTO
 * 对应前端 LoginRequest
 * @description 对应后端 TaktLoginRequestDto
 */
export interface LoginRequest {
  /**
   * 用户名（8位）
   */
  userName: string;

  /**
   * 密码（RSA PKCS#1 密文 Base64；signin 有 LoginTicket 时可省略）
   */
  password: string;

  /**
   * 租户编码（用于多租户登录）
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
   * 验证码（如果启用）
   */
  captchaCode?: string;

  /**
   * 验证码ID（如果启用）
   */
  captchaId?: string;

  /**
   * 记住我（延长Token有效期）
   */
  rememberMe: boolean;

  /**
   * 登录票据（由 session/verify-password 签发；signin 凭此跳过重复验密）
   */
  loginTicket?: string;

}


/**
 * 登录预检请求 DTO（点登录：先租户用户权限，再密码；通过后决定是否弹验证码）
 * 对应前端 SessionVerifyPasswordRequest
 * @description 对应后端 TaktSessionVerifyPasswordRequestDto
 */
export interface SessionVerifyPasswordRequest {
  /**
   * 用户名
   */
  userName: string;

  /**
   * 密码（RSA PKCS#1 密文 Base64）
   */
  password: string;

  /**
   * 租户编码
   */
  tenantCode: string;

  /**
   * 验证码 ID（启用验证码时必填）
   */
  captchaId?: string;

  /**
   * 验证码载荷（启用验证码时必填）
   */
  captchaCode?: string;

}


/**
 * 登录预检响应 DTO（密码已通过；captchaRequired 为 true 时前端弹验证码后再 signin）
 * 对应前端 SessionVerifyPasswordResponse
 * @description 对应后端 TaktSessionVerifyPasswordResponseDto
 */
export interface SessionVerifyPasswordResponse {
  /**
   * 密码是否通过
   */
  passwordValid: boolean;

  /**
   * 是否需要弹出验证码
   */
  captchaRequired: boolean;

  /**
   * 登录票据（短时有效，供 session/signin 使用）
   */
  loginTicket: string;

}


/**
 * 用户登录响应 DTO
 * 对应前端 LoginResponse
 * @description 对应后端 TaktLoginResponseDto
 */
export interface LoginResponse {
  /**
   * 访问令牌（Access Token）
   */
  accessToken: string;

  /**
   * 刷新令牌（Refresh Token）
   */
  refreshToken: string;

  /**
   * 令牌类型（通常为 Bearer）
   */
  tokenType: string;

  /**
   * 过期时间（秒）
   */
  expiresIn: number;

  /**
   * 用户ID
   */
  userId: string;

  /**
   * 用户名
   */
  userName: string;

  /**
   * 昵称
   */
  nickName?: string;

  /**
   * 租户编码
   */
  tenantCode: string;

  /**
   * 公司编码
   */
  companyCode: string;

  /**
   * 用户类型
   */
  userType: number;

  /**
   * 角色列表
   */
  roles: string[];

  /**
   * 权限列表
   */
  permissions: string[];

  /**
   * 是否需要修改密码（首次登录或密码过期）
   */
  mustChangePassword: boolean;

  /**
   * 最后登录时间
   */
  lastLoginAt?: string;

  /**
   * 登录时间
   */
  loginAt: string;

}


/**
 * 刷新令牌请求 DTO
 * 对应前端 RefreshTokenRequest
 * @description 对应后端 TaktRefreshTokenRequestDto
 */
export interface RefreshTokenRequest {
  /**
   * 刷新令牌
   */
  refreshToken: string;

}


/**
 * 用户信息响应 DTO
 * 用于获取当前登录用户详细信息
 * 对应前端 UserInfoResponse
 * @description 对应后端 TaktUserInfoResponseDto
 */
export interface UserInfoResponse {
  /**
   * 用户ID
   */
  userId: string;

  /**
   * 用户名
   */
  userName: string;

  /**
   * 昵称
   */
  nickName?: string;

  /**
   * 用户类型
   */
  userType: number;

  /**
   * 用户类型名称
   */
  switch: any;

  /**
   * 关联的员工 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  employeeId: string;

  /**
   * 员工姓名（从员工表 name 关联）
   */
  employeeName?: string;

  /**
   * 员工性别（0=未知，1=男，2=女）
   */
  gender: number;

  /**
   * 员工手机号码
   */
  mobile?: string;

  /**
   * 员工电子邮箱
   */
  email?: string;

  /**
   * 用户头像 URL（来自员工档案 avatar）
   */
  avatar?: string;

  /**
   * 租户编码
   */
  tenantCode: string;

  /**
   * 租户名称（TaktTenant.TenantName）
   */
  tenantName?: string;

  /**
   * 公司编码
   */
  companyCode: string;

  /**
   * 当前公司关联工厂（takt_company.related_plant）
   */
  relatedPlant: string;

  /**
   * 用户区域文化 BCP47（takt_identity_user.culture_code，UI 偏好）
   */
  defaultCulture: string;

  /**
   * 用户区域文化本地化名称（TaktCulture.NativeName）
   */
  cultureName?: string;

  /**
   * 当前公司区域文化 BCP47（takt_company.default_culture）
   */
  companyDefaultCulture: string;

  /**
   * 公司名称（从公司表关联）
   */
  companyName?: string;

  /**
   * 当前公司下所属部门显示名（多部门顿号拼接）
   */
  deptName?: string;

  /**
   * 当前公司下所属部门编码（与 deptName 对应，供 org.dept.*）
   */
  deptCodes?: string[];

  /**
   * 状态（1=启用，0=禁用）
   */
  userStatus: number;

  /**
   * 角色编码列表（鉴权）
   */
  roles: string[];

  /**
   * 角色显示名（个人中心；与 roles 对应）
   */
  roleNames?: string[];

  /**
   * 权限列表
   */
  permissions: string[];

  /**
   * 可访问的菜单树（目录与菜单）
   */
  menus: MenuTree[];

  /**
   * 可访问的前端路由路径列表
   */
  routePaths: string[];

  /**
   * 可访问的公司列表
   */
  accessibleCompanies: string[];

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
   * 密码过期天数（0=永不过期）
   */
  passwordExpireDays: number;

  /**
   * 密码是否即将过期（7天内）
   */
  isPasswordExpiringSoon: boolean;

  /**
   * 创建时间
   */
  createdAt: string;

  /**
   * 更新时间
   */
  updatedAt: string;

}


/**
 * 登录前预览：用户默认公司与用户 CultureCode
 * 对应前端 LoginPreviewLocale
 * @description 对应后端 TaktLoginPreviewLocaleResponseDto
 */
export interface LoginPreviewLocale {
  /**
   * 用户默认登录公司代码（takt_identity_user_company.is_default=Yes）
   */
  companyCode: string;

  /**
   * 用户区域文化代码（takt_identity_user.culture_code；登录页 UI 语言仅用此字段）
   */
  cultureCode: string;
}

