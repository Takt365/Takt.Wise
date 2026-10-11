// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/constants
// 文件名称：takt.constants.ts
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：系统内置常量与展示文案（与 backend TaktConstants 对齐；非字典项）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** 内置常量类别 */
export type TaktConstCategory =
  | 'loginType'
  | 'loginResult'
  | 'operType'
  | 'quartzTaskType'
  | 'deviceType'
  | 'browserType'
  | 'operatingSystem'

/** 登录方式（TaktConstants.LoginType） */
export const TaktLoginType = {
  Unknown: 'unknown',
  Password: 'password',
  RefreshToken: 'refreshtoken',
  ClientCredentials: 'clientcredentials',
  AuthorizationCode: 'authorizationcode',
  OAuthAuthorize: 'oauthauthorize',
  VerifyPassword: 'verifypassword',
  SignOut: 'signout',
} as const

/** 登录结果（TaktConstants.LoginResult） */
export const TaktLoginResult = {
  Success: 'success',
  PasswordError: 'passworderror',
  UserNotFound: 'usernotfound',
  UserDisabled: 'userdisabled',
  UserLocked: 'userlocked',
  CaptchaError: 'captchaerror',
} as const

/** 操作类型（TaktConstants.OperType） */
export const TaktOperType = {
  Unknown: 'unknown',
  Create: 'create',
  Update: 'update',
  Delete: 'delete',
  Query: 'query',
  Export: 'export',
  Import: 'import',
  Grant: 'grant',
  ForceOut: 'forceout',
  CodeGen: 'codegen',
  ClearData: 'cleardata',
} as const

/** Quartz 任务类型（TaktConstants.QuartzTaskType） */
export const TaktQuartzTaskType = {
  Assembly: 'assembly',
  Http: 'http',
  Sql: 'sql',
} as const

/** Quartz 触发器类型（对齐 TaktQuartzSchedulerManager：0=Simple 1=Cron） */
export const TaktQuartzTriggerType = {
  Simple: 0,
  Cron: 1,
} as const

/** Quartz Misfire 策略（对齐 TaktQuartzSchedulerManager：0=默认 1=忽略 2=立即触发 3=不触发） */
export const TaktQuartzMisfirePolicy = {
  Default: 0,
  Ignore: 1,
  FireAndProceed: 2,
  DoNothing: 3,
} as const

/** 登录设备（TaktConstants.DeviceType） */
export const TaktDeviceType = {
  Unknown: 'unknown',
  Pc: 'pc',
  Mobile: 'mobile',
  Tablet: 'tablet',
} as const

/** 浏览器（TaktConstants.BrowserType） */
export const TaktBrowserType = {
  Unknown: 'unknown',
  Chrome: 'chrome',
  Firefox: 'firefox',
  Safari: 'safari',
  Edge: 'edge',
} as const

/** 操作系统（TaktConstants.OperatingSystem） */
export const TaktOperatingSystem = {
  Unknown: 'unknown',
  Windows: 'windows',
  MacOs: 'macos',
  Linux: 'linux',
  Android: 'android',
  Ios: 'ios',
} as const

const LABELS: Record<TaktConstCategory, Record<string, string>> = {
  loginType: {
    [TaktLoginType.Unknown]: '未知',
    [TaktLoginType.Password]: '账号密码',
    [TaktLoginType.RefreshToken]: '刷新令牌',
    [TaktLoginType.ClientCredentials]: '客户端凭证',
    [TaktLoginType.AuthorizationCode]: '授权码换令牌',
    [TaktLoginType.OAuthAuthorize]: 'OAuth授权登录',
    [TaktLoginType.VerifyPassword]: '预检验密',
    [TaktLoginType.SignOut]: '注销会话',
  },
  loginResult: {
    [TaktLoginResult.Success]: '登录成功',
    [TaktLoginResult.PasswordError]: '密码错误',
    [TaktLoginResult.UserNotFound]: '用户不存在',
    [TaktLoginResult.UserDisabled]: '用户已禁用',
    [TaktLoginResult.UserLocked]: '用户已锁定',
    [TaktLoginResult.CaptchaError]: '验证码错误',
  },
  operType: {
    [TaktOperType.Unknown]: '未知',
    [TaktOperType.Create]: '新增',
    [TaktOperType.Update]: '修改',
    [TaktOperType.Delete]: '删除',
    [TaktOperType.Query]: '查询',
    [TaktOperType.Export]: '导出',
    [TaktOperType.Import]: '导入',
    [TaktOperType.Grant]: '授权',
    [TaktOperType.ForceOut]: '强退',
    [TaktOperType.CodeGen]: '生成代码',
    [TaktOperType.ClearData]: '清空数据',
  },
  quartzTaskType: {
    [TaktQuartzTaskType.Assembly]: '程序集',
    [TaktQuartzTaskType.Http]: '网络请求',
    [TaktQuartzTaskType.Sql]: 'SQL语句',
  },
  deviceType: {
    [TaktDeviceType.Unknown]: '未知',
    [TaktDeviceType.Pc]: 'PC',
    [TaktDeviceType.Mobile]: '手机',
    [TaktDeviceType.Tablet]: '平板',
  },
  browserType: {
    [TaktBrowserType.Unknown]: '未知',
    [TaktBrowserType.Chrome]: 'Chrome',
    [TaktBrowserType.Firefox]: 'Firefox',
    [TaktBrowserType.Safari]: 'Safari',
    [TaktBrowserType.Edge]: 'Edge',
  },
  operatingSystem: {
    [TaktOperatingSystem.Unknown]: '未知',
    [TaktOperatingSystem.Windows]: 'Windows',
    [TaktOperatingSystem.MacOs]: 'macOS',
    [TaktOperatingSystem.Linux]: 'Linux',
    [TaktOperatingSystem.Android]: 'Android',
    [TaktOperatingSystem.Ios]: 'iOS',
  },
}

/**
 * 解析内置常量展示文案
 * @param category 常量类别
 * @param value 落库值
 * @returns 展示文案；未知则回退原值或「-」
 */
export function formatTaktConstLabel(
  category: TaktConstCategory,
  value?: string | null,
): string {
  if (value == null || value === '') {
    return '-'
  }
  const trimmed = String(value).trim()
  return LABELS[category][trimmed] ?? trimmed
}

/**
 * 内置常量下拉选项
 * @param category 常量类别
 * @returns label/value 选项列表
 */
export function getTaktConstOptions(category: TaktConstCategory): Array<{ label: string; value: string }> {
  const map = LABELS[category]
  return Object.entries(map).map(([value, label]) => ({ label, value }))
}

export const loginTypeOptions = getTaktConstOptions('loginType')
export const loginResultOptions = getTaktConstOptions('loginResult')
export const operTypeOptions = getTaktConstOptions('operType')
export const quartzTaskTypeOptions = getTaktConstOptions('quartzTaskType')
export const browserTypeOptions = getTaktConstOptions('browserType')
export const operatingSystemOptions = getTaktConstOptions('operatingSystem')
