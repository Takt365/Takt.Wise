// ========================================
// 项目名称：节拍智控·TaktWise 
// 命名空间：@/utils/regex
// 文件名称：regex.ts
// 创建时间：2025-01-21
// 创建人：Takt365(Cursor AI)
// 功能描述：正则表达式工具类，提供常用正则表达式和验证方法
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 常用正则表达式模式
 */
export const RegexPatterns = {
  /** 邮箱地址 */
  /** 邮箱地址（RFC 5322 常用子集，长度 6-100 在 isValidEmail 中校验） */
  EMAIL: /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)+$/,
  
  /** 手机号（中国大陆） */
  PHONE_CN: /^1[3-9]\d{9}$/,

  /** 手机号（中国台湾） */
  PHONE_TW: /^09\d{8}$/,

  /** 手机号（中国香港） */
  PHONE_HK: /^(5|6|8|9)\d{7}$/,

  /** 手机号（美国 10 位） */
  PHONE_US: /^[2-9]\d{2}[2-9]\d{6}$/,

  /** 手机号（日本 11 位） */
  PHONE_JP: /^0\d{10}$/,
  
  /** 固定电话（中国大陆） */
  TEL_CN: /^0\d{2,3}-?\d{7,8}$/,
  
  /** 身份证号（18位） */
  ID_CARD_18: /^[1-9]\d{5}(18|19|20)\d{2}(0[1-9]|1[0-2])(0[1-9]|[12]\d|3[01])\d{3}[\dXx]$/,
  
  /** 身份证号（15位） */
  ID_CARD_15: /^[1-9]\d{5}\d{2}(0[1-9]|1[0-2])(0[1-9]|[12]\d|3[01])\d{3}$/,

  /** 身份证号（中国台湾） */
  ID_CARD_TW: /^[A-Z][12]\d{8}$/,

  /** 身份证号（中国香港） */
  ID_CARD_HK: /^[A-Z]{1,2}\d{6}\([0-9A]\)$/,

  /** 身份证号（美国，SSN） */
  ID_CARD_US: /^(?!000|666|9\d\d)\d{3}[- ]?(?!00)\d{2}[- ]?(?!0000)\d{4}$/,

  /** 身份证号（日本，个人番号12位） */
  ID_CARD_JP: /^\d{12}$/,
  
  /** 统一社会信用代码 */
  UNIFIED_SOCIAL_CREDIT_CODE: /^[0-9A-HJ-NPQRTUWXY]{2}\d{6}[0-9A-HJ-NPQRTUWXY]{10}$/,
  
  /** 邮政编码（中国大陆） */
  POSTAL_CODE_CN: /^[1-9]\d{5}$/,
  
  /** URL地址 */
  URL: /^https?:\/\/(www\.)?[-a-zA-Z0-9@:%._+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_+.~#?&//=]*)$/,
  
  /** IP地址（IPv4） */
  IPV4: /^((25[0-5]|2[0-4]\d|[01]?\d\d?)\.){3}(25[0-5]|2[0-4]\d|[01]?\d\d?)$/,
  
  /** IP地址（IPv6） */
  IPV6: /^([0-9a-fA-F]{1,4}:){7}[0-9a-fA-F]{1,4}$/,
  
  /** 用户名（小写字母开头，允许小写字母和数字，不允许特殊符号，4-20位） */
  userName: /^[a-z][a-z0-9]{3,19}$/,

  /** 登录账号（小写字母开头，字母与数字，4-20 位，与 UserName / 种子用户 demo 一致） */
  LOGIN_USER_NAME: /^[a-z][a-z0-9]{3,19}$/,

  /** 租户编码（3 位数字，与 Database:TenantCodes 一致） */
  TENANT_CODE: /^\d{3}$/,
  
  /** 真实姓名（中文姓名，2-50个汉字，支持复姓） */
  REAL_NAME: /^[\u4e00-\u9fa5]{2,50}$/,
  
  /** 全名（中文、英文、数字、空格、点、横线，2-100位） */
  FULL_NAME: /^[\u4e00-\u9fa5a-zA-Z0-9\s.-]{2,100}$/,
  
  /** 昵称（中文、英文、数字、下划线、横线、点，2-20 位） */
  NICK_NAME: /^[\u4e00-\u9fa5a-zA-Z0-9_.-]{2,20}$/,

  /** 员工姓名（中文、英文、数字、空格、点、横线，2-80 位；与 TaktEmployee.EmployeeName 对齐） */
  EMPLOYEE_NAME: /^[\u4e00-\u9fa5a-zA-Z0-9\s.-]{2,80}$/,
  
  /** 英文名（英文字母、数字、空格、横线、点、单引号，2-100位，必须以字母开头和结尾） */
  ENGLISH_NAME: /^[a-zA-Z]([a-zA-Z0-9\s.\-']{0,98}[a-zA-Z])?$/,

  /** 姓/名（仅英文与数字，首字母必须大写，数字不能在首位，1-100位） */
  NAME_EN: /^[A-Z][A-Za-z0-9]{0,99}$/,

  /** 姓（last_name，与后端 TaktRegexHelper.LastName 对齐） */
  LAST_NAME: /^(?:[\u4e00-\u9fa5]{1,20}|[A-Za-z][A-Za-z\s'\-]{0,49})$/,

  /** 名（first_name，与后端 TaktRegexHelper.FirstName 对齐） */
  FIRST_NAME: /^(?:[\u4e00-\u9fa5]{1,30}|[A-Za-z][A-Za-z\s'\-]{0,49})$/,

  /** 密码（强密码，8-20位，必须包含大小写字母、数字和特殊字符） */
  PASSWORD: /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,20}$/,
  
  /** 强密码（与 PASSWORD 一致，兼容既有调用） */
  STRONG_PASSWORD: /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,20}$/,
  
  /** 中文字符 */
  CHINESE: /^[\u4e00-\u9fa5]+$/,
  
  /** 英文字母 */
  ENGLISH: /^[a-zA-Z]+$/,
  
  /** 数字 */
  NUMBER: /^\d+$/,
  
  /** 整数（包括负数） */
  INTEGER: /^-?\d+$/,
  
  /** 正整数 */
  POSITIVE_INTEGER: /^[1-9]\d*$/,
  
  /** 非负整数（包括0） */
  NON_NEGATIVE_INTEGER: /^\d+$/,
  
  /** 浮点数 */
  FLOAT: /^-?\d+\.\d+$/,
  
  /** 正浮点数 */
  POSITIVE_FLOAT: /^[1-9]\d*\.\d+|0\.\d*[1-9]\d*$/,
  
  /** 非负浮点数（包括0） */
  NON_NEGATIVE_FLOAT: /^\d+\.\d+|0$/,
  
  /** 日期（YYYY-MM-DD） */
  DATE: /^\d{4}-\d{2}-\d{2}$/,
  
  /** 时间（HH:mm:ss） */
  TIME: /^([01]\d|2[0-3]):([0-5]\d):([0-5]\d)$/,
  
  /** 日期时间（YYYY-MM-DD HH:mm:ss） */
  DATETIME: /^\d{4}-\d{2}-\d{2}\s+([01]\d|2[0-3]):([0-5]\d):([0-5]\d)$/,
  
  /** 银行卡号（16-19位数字） */
  BANK_CARD: /^\d{16,19}$/,
  
  /** QQ号（5-11位数字） */
  QQ: /^[1-9]\d{4,10}$/,
  
  /** 微信号（6-20位，字母、数字、下划线、减号） */
  WECHAT: /^[a-zA-Z0-9_-]{6,20}$/,
  
  /** 车牌号（中国大陆） */
  LICENSE_PLATE_CN: /^[京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领][A-Z][A-HJ-NP-Z0-9]{4,5}[A-HJ-NP-Z0-9挂学警港澳]$/,

  /** 车牌号（美国，常见格式：1-7位字母数字，可含连接符） */
  LICENSE_PLATE_US: /^[A-Z0-9]{1,7}(?:[-\s][A-Z0-9]{1,7})?$/,

  /** 车牌号（日本，常见简化格式） */
  LICENSE_PLATE_JP: /^[0-9]{2,3}[ぁ-んァ-ヶ][0-9]{4}$|^[A-Z0-9]{2,4}[-\s]?[A-Z0-9]{2,4}$/,

  /** 车牌号（中国台湾，常见格式） */
  LICENSE_PLATE_TW: /^[A-Z]{2,3}[-\s]?[0-9]{3,4}$|^[0-9]{3,4}[-\s]?[A-Z]{2,3}$/,

  /** 车牌号（中国香港，常见格式） */
  LICENSE_PLATE_HK: /^[A-Z]{1,2}\d{1,4}$/,
  
  /** MAC地址 */
  MAC_ADDRESS: /^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$/,
  
  /** 颜色值（十六进制） */
  HEX_COLOR: /^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$/,
  
  /** 版本号（如：1.0.0） */
  VERSION: /^\d+\.\d+\.\d+$/,
  
  /** 文件扩展名 */
  FILE_EXTENSION: /\.([a-zA-Z0-9]+)$/,
  
  /** 通用编码（字母、数字、横线、下划线，3-50位） */
  CODE: /^[a-zA-Z0-9_-]{3,50}$/,
  
  /** 物料编码（字母、数字、横线、下划线、点，3-50位） */
  MATERIAL_CODE: /^[a-zA-Z0-9_.-]{3,50}$/,
  
  /** 文档编码（字母、数字、横线、下划线，3-50位） */
  DOCUMENT_CODE: /^[a-zA-Z0-9_-]{3,50}$/,
  
  /** 订单编码（字母、数字、横线，通常以字母开头，3-50位） */
  ORDER_CODE: /^[a-zA-Z][a-zA-Z0-9-]{2,49}$/,
  
  /** 批次编码（字母、数字、横线，通常以字母开头，3-50位） */
  BATCH_CODE: /^[a-zA-Z][a-zA-Z0-9-]{2,49}$/,
  
  /** 序列号（字母、数字组合，3-50位） */
  SERIAL_NUMBER: /^[a-zA-Z0-9]{3,50}$/,
  
  /** 编码（带日期格式：YYYYMMDD + 序号，如：20250121001） */
  CODE_WITH_DATE: /^\d{8}[a-zA-Z0-9]{1,20}$/,
  
  /** 编码（带前缀：前缀-序号，如：MAT-001、DOC-20250121-001） */
  CODE_WITH_PREFIX: /^[a-zA-Z]{2,10}-[a-zA-Z0-9]{1,30}$/,
  
  /** 编码（带分隔符：多个部分用横线或下划线分隔） */
  CODE_WITH_SEPARATOR: /^[a-zA-Z0-9]+([_-][a-zA-Z0-9]+){1,9}$/,
  
  /** 数字编码（纯数字，3-20位） */
  NUMERIC_CODE: /^\d{3,20}$/,
  
  /** 字母编码（纯字母，3-20位） */
  ALPHABETIC_CODE: /^[a-zA-Z]{3,20}$/,
  
  /** 大写字母编码（纯大写字母，3-20位） */
  UPPERCASE_CODE: /^[A-Z]{3,20}$/,
  
  /** 小写字母编码（纯小写字母，3-20位） */
  LOWERCASE_CODE: /^[a-z]{3,20}$/,
  
  /** 菜单编码（字母开头，允许字母、数字、下划线、横线，3-200位） */
  MENU_CODE: /^[a-zA-Z][a-zA-Z0-9_-]{2,199}$/,
  
  /** 国际化键（小写字母、数字、点号，格式如：menu.user.management，必须以字母或数字结尾，3-200位） */
  L10N_KEY: /^[a-z][a-z0-9.]{1,198}[a-z0-9]$/,
  
  /** 角色编码（字母开头，允许字母、数字、下划线、横线，3-50位） */
  ROLE_CODE: /^[a-zA-Z][a-zA-Z0-9_-]{2,49}$/,
  
  /** 部门编码（字母开头，允许字母、数字、下划线、横线，3-50位） */
  DEPT_CODE: /^[a-zA-Z][a-zA-Z0-9_-]{2,49}$/,
  
  /** 岗位编码（字母开头，允许字母、数字、下划线、横线，3-50位） */
  POST_CODE: /^[a-zA-Z][a-zA-Z0-9_-]{2,49}$/,

  /** 工厂代码（4位，与后端 TaktRegexHelper.PlantCode 对齐） */
  PLANT_CODE: /^[A-Z0-9]\d00$/,

  /** 公司代码（4位，与后端 TaktRegexHelper.CompanyCode 对齐） */
  COMPANY_CODE: /^\d{2}00$/,

  /** 利润中心编码（数值型，最大8位） */
  PROFIT_CENTER_CODE: /^\d{1,8}$/,

  /** 成本要素编码（数值型，最大8位） */
  COST_ELEMENT_CODE: /^\d{1,8}$/,

  /** 成本中心编码（数值型，最大8位） */
  COST_CENTER_CODE: /^\d{1,8}$/,

  /** 会计科目编码（数值型，最大8位） */
  TITLE_CODE: /^\d{1,8}$/,

  /** 资产编码（数值型，最大8位） */
  ASSET_CODE: /^\d{1,8}$/,

  /** 权限标识（模块:资源:操作，格式如：identity:user:create，小写字母、数字、冒号，3-100位） */
  PERMISSION: /^[a-z][a-z0-9]*:[a-z0-9]+:[a-z0-9]+$/,
  
  /** 空白字符 */
  WHITESPACE: /^\s*$/,
  
  /** 非空白字符 */
  NON_WHITESPACE: /\S/,
} as const

/** 登录页用户名最小长度（与种子用户 demo 及 UserName 规则 4-20 一致） */
export const LOGIN_USER_NAME_MIN_LENGTH = 4;

/** 登录页用户名最大长度（与 TaktUser.UserName varchar(20) 一致） */
export const LOGIN_USER_NAME_MAX_LENGTH = 20;

/** 登录页密码最小长度（与 PasswordPolicy:MinLength 一致） */
export const LOGIN_PASSWORD_MIN_LENGTH = 8;

/** 登录页密码最大长度（与密码策略 8-20 位一致） */
export const LOGIN_PASSWORD_MAX_LENGTH = 20;

/** 邮箱最小长度（RFC 5322 最短合法地址如 a@b.co） */
export const EMAIL_MIN_LENGTH = 6;

/** 邮箱最大长度（与 TaktEmployee.email varchar(100) 一致） */
export const EMAIL_MAX_LENGTH = 100;

/** 用户昵称最小长度 */
export const USER_NICK_NAME_MIN_LENGTH = 2;

/** 用户昵称最大长度（与 TaktUser.NickName nvarchar(20) 一致） */
export const USER_NICK_NAME_MAX_LENGTH = 20;

/** 员工姓名最小长度 */
export const EMPLOYEE_NAME_MIN_LENGTH = 2;

/** 员工姓名最大长度（与 TaktEmployee.EmployeeName nvarchar(80) 一致） */
export const EMPLOYEE_NAME_MAX_LENGTH = 80;

/** 中国大陆手机号位数 */
export const PHONE_CN_LENGTH = 11;

/** 中国香港手机号位数 */
export const PHONE_HK_LENGTH = 8;

/** 美国手机号位数 */
export const PHONE_US_LENGTH = 10;

/** 日本手机号位数 */
export const PHONE_JP_LENGTH = 11;

/** 公司默认文化 BCP47（与 TaktCultureSeedData 一致） */
export type TaktPhoneCultureCode = 'zh-CN' | 'zh-HK' | 'en-US' | 'ja-JP';

/** 公司默认文化 → 手机号正则键（与后端 TaktRegexHelper 对齐） */
export const PHONE_PATTERN_BY_CULTURE: Record<TaktPhoneCultureCode, keyof typeof RegexPatterns> = {
  'zh-CN': 'PHONE_CN',
  'zh-HK': 'PHONE_HK',
  'en-US': 'PHONE_US',
  'ja-JP': 'PHONE_JP',
};

/** 公司默认文化 → 手机号最大长度 */
export const PHONE_MAX_LENGTH_BY_CULTURE: Record<TaktPhoneCultureCode, number> = {
  'zh-CN': PHONE_CN_LENGTH,
  'zh-HK': PHONE_HK_LENGTH,
  'en-US': PHONE_US_LENGTH,
  'ja-JP': PHONE_JP_LENGTH,
};

/** 公司默认文化 → 手机号格式说明 I18n 键（common.tip.phone.*，表单提示） */
export const PHONE_TIP_I18N_BY_CULTURE: Record<TaktPhoneCultureCode, string> = {
  'zh-CN': 'common.tip.phone.cn',
  'zh-HK': 'common.tip.phone.hk',
  'en-US': 'common.tip.phone.us',
  'ja-JP': 'common.tip.phone.jp',
};

/**
 * 按公司默认文化 BCP47 解析手机号校验文化（未知时回退 en-US）
 * @param cultureCode 区域文化编码
 * @returns 规范后的文化编码
 */
export function resolvePhoneCultureCode(cultureCode?: string | null): TaktPhoneCultureCode {
  const normalized = String(cultureCode ?? '').trim();
  if (normalized === 'zh-CN' || normalized === 'zh-HK' || normalized === 'en-US' || normalized === 'ja-JP') {
    return normalized;
  }
  return 'en-US';
}

/**
 * 按公司默认文化返回手机号格式说明 I18n 键
 * @param cultureCode 区域文化编码
 * @returns common.tip.phone.* 键
 */
export function getPhoneTipI18nKeyByCulture(cultureCode?: string | null): string {
  return PHONE_TIP_I18N_BY_CULTURE[resolvePhoneCultureCode(cultureCode)];
}

/**
 * 按公司默认文化返回手机号最大长度
 * @param cultureCode 区域文化编码
 * @returns 最大位数
 */
export function getPhoneMaxLengthByCulture(cultureCode?: string | null): number {
  return PHONE_MAX_LENGTH_BY_CULTURE[resolvePhoneCultureCode(cultureCode)];
}

/**
 * 正则表达式工具类
 */
export class RegexHelper {
  /**
   * 测试字符串是否匹配正则表达式
   * @param pattern 正则表达式或模式名称
   * @param text 要测试的文本
   * @param flags 正则表达式标志（可选）
   * @returns 是否匹配
   */
  static test(pattern: RegExp | keyof typeof RegexPatterns, text: string, flags?: string): boolean {
    if (!text) {
      return false
    }

    let regex: RegExp
    if (pattern instanceof RegExp) {
      regex = pattern
    } else {
      const patternValue = RegexPatterns[pattern]
      if (!patternValue) {
        throw new Error(`未知的正则表达式模式: ${pattern}`)
      }
      regex = patternValue
    }

    if (flags) {
      regex = new RegExp(regex.source, flags)
    }

    return regex.test(text)
  }

  /**
   * 匹配字符串，返回匹配结果
   * @param pattern 正则表达式或模式名称
   * @param text 要匹配的文本
   * @param flags 正则表达式标志（可选）
   * @returns 匹配结果数组，如果没有匹配则返回null
   */
  static match(pattern: RegExp | keyof typeof RegexPatterns, text: string, flags?: string): RegExpMatchArray | null {
    if (!text) {
      return null
    }

    let regex: RegExp
    if (pattern instanceof RegExp) {
      regex = pattern
    } else {
      const patternValue = RegexPatterns[pattern]
      if (!patternValue) {
        throw new Error(`未知的正则表达式模式: ${pattern}`)
      }
      regex = patternValue
    }

    if (flags) {
      regex = new RegExp(regex.source, flags)
    }

    return text.match(regex)
  }

  /**
   * 匹配所有结果
   * @param pattern 正则表达式或模式名称
   * @param text 要匹配的文本
   * @param flags 正则表达式标志（可选，必须包含'g'标志才能匹配所有）
   * @returns 所有匹配结果的数组
   */
  static matchAll(pattern: RegExp | keyof typeof RegexPatterns, text: string, flags?: string): RegExpMatchArray[] {
    if (!text) {
      return []
    }

    let regex: RegExp
    if (pattern instanceof RegExp) {
      regex = pattern
    } else {
      const patternValue = RegexPatterns[pattern]
      if (!patternValue) {
        throw new Error(`未知的正则表达式模式: ${pattern}`)
      }
      regex = patternValue
    }

    const finalFlags = flags || (regex.global ? '' : 'g')
    const finalRegex = new RegExp(regex.source, finalFlags)

    const matches: RegExpMatchArray[] = []
    let match: RegExpMatchArray | null
    while ((match = finalRegex.exec(text)) !== null) {
      matches.push(match)
      if (!finalRegex.global) {
        break
      }
    }

    return matches
  }

  /**
   * 替换字符串
   * @param pattern 正则表达式或模式名称
   * @param text 要替换的文本
   * @param replacement 替换内容（可以是字符串或函数）
   * @param flags 正则表达式标志（可选）
   * @returns 替换后的字符串
   */
  static replace(
    pattern: RegExp | keyof typeof RegexPatterns,
    text: string,
    replacement: string | ((match: string, ...args: unknown[]) => string),
    flags?: string
  ): string {
    if (!text) {
      return text
    }

    let regex: RegExp
    if (pattern instanceof RegExp) {
      regex = pattern
    } else {
      const patternValue = RegexPatterns[pattern]
      if (!patternValue) {
        throw new Error(`未知的正则表达式模式: ${pattern}`)
      }
      regex = patternValue
    }

    if (flags) {
      regex = new RegExp(regex.source, flags)
    }

    if (typeof replacement === 'function') {
      return text.replace(regex, replacement)
    }

    return text.replace(regex, replacement)
  }

  /**
   * 提取匹配的组
   * @param pattern 正则表达式或模式名称
   * @param text 要匹配的文本
   * @param flags 正则表达式标志（可选）
   * @returns 匹配的组数组，如果没有匹配则返回null
   */
  static extractGroups(pattern: RegExp | keyof typeof RegexPatterns, text: string, flags?: string): string[] | null {
    const match = this.match(pattern, text, flags)
    if (!match) {
      return null
    }

    return match.slice(1) // 返回除完整匹配外的所有组
  }

  /**
   * 验证邮箱地址
   * @param email 邮箱地址
   * @returns 是否有效
   */
  static isValidEmail(email: string): boolean {
    const v = String(email ?? '').trim()
    if (v.length < EMAIL_MIN_LENGTH || v.length > EMAIL_MAX_LENGTH) {
      return false
    }
    return this.test('EMAIL', v)
  }

  /**
   * 验证手机号（按公司默认文化；未传文化时回退 en-US）
   * @param phone 手机号
   * @param cultureCode 公司 default_culture
   * @returns 是否有效
   */
  static isValidPhone(phone: string, cultureCode?: string | null): boolean {
    return this.isValidPhoneByCulture(phone, cultureCode)
  }

  /**
   * 按公司默认文化校验手机号（zh-CN 11 / zh-HK 8 / en-US 10 / ja-JP 11）
   * @param phone 手机号
   * @param cultureCode 公司 default_culture
   * @returns 是否有效
   */
  static isValidPhoneByCulture(phone: string, cultureCode?: string | null): boolean {
    const culture = resolvePhoneCultureCode(cultureCode)
    const v = String(phone ?? '').trim()
    if (!v) {
      return false
    }
    const maxLength = PHONE_MAX_LENGTH_BY_CULTURE[culture]
    if (v.length !== maxLength) {
      return false
    }
    const patternKey = PHONE_PATTERN_BY_CULTURE[culture]
    return this.test(patternKey, v)
  }

  /**
   * 验证身份证号
   * @param idCard 身份证号
   * @returns 是否有效
   */
  static isValidIdCard(idCard: string): boolean {
    return this.test('ID_CARD_18', idCard)
      || this.test('ID_CARD_15', idCard)
      || this.test('ID_CARD_TW', idCard)
      || this.test('ID_CARD_HK', idCard)
      || this.test('ID_CARD_US', idCard)
      || this.test('ID_CARD_JP', idCard)
  }

  /**
   * 验证手机号（中国大陆）
   */
  static isValidPhoneCn(phone: string): boolean {
    const v = String(phone ?? '').trim()
    return v.length === PHONE_CN_LENGTH && this.test('PHONE_CN', v)
  }

  /**
   * 验证手机号（中国香港 8 位）
   */
  static isValidPhoneHk(phone: string): boolean {
    const v = String(phone ?? '').trim()
    return v.length === PHONE_HK_LENGTH && this.test('PHONE_HK', v)
  }

  /**
   * 验证手机号（中国台湾）
   */
  static isValidPhoneTw(phone: string): boolean {
    return this.test('PHONE_TW', phone)
  }

  /**
   * 验证手机号（美国）
   */
  static isValidPhoneUs(phone: string): boolean {
    const v = String(phone ?? '').trim()
    return v.length === PHONE_US_LENGTH && this.test('PHONE_US', v)
  }

  /**
   * 验证手机号（日本）
   */
  static isValidPhoneJp(phone: string): boolean {
    const v = String(phone ?? '').trim()
    return v.length === PHONE_JP_LENGTH && this.test('PHONE_JP', v)
  }

  /**
   * 验证身份证号（中国大陆）
   */
  static isValidIdCardCn(idCard: string): boolean {
    return this.test('ID_CARD_18', idCard) || this.test('ID_CARD_15', idCard)
  }

  /**
   * 验证身份证号（中国台湾）
   */
  static isValidIdCardTw(idCard: string): boolean {
    return this.test('ID_CARD_TW', idCard)
  }

  /**
   * 验证身份证号（中国香港）
   */
  static isValidIdCardHk(idCard: string): boolean {
    return this.test('ID_CARD_HK', idCard)
  }

  /**
   * 验证身份证号（美国）
   */
  static isValidIdCardUs(idCard: string): boolean {
    return this.test('ID_CARD_US', idCard)
  }

  /**
   * 验证身份证号（日本）
   */
  static isValidIdCardJp(idCard: string): boolean {
    return this.test('ID_CARD_JP', idCard)
  }

  /**
   * 验证URL地址
   * @param url URL地址
   * @returns 是否有效
   */
  static isValidUrl(url: string): boolean {
    return this.test('URL', url)
  }

  /**
   * 验证IP地址
   * @param ip IP地址
   * @returns 是否有效（支持IPv4和IPv6）
   */
  static isValidIp(ip: string): boolean {
    return this.test('IPV4', ip) || this.test('IPV6', ip)
  }

  /**
   * 验证用户名
   * @param userName 用户名
   * @returns 是否有效
   */
  static isValidUserName(userName: string): boolean {
    return this.test('userName', userName)
  }

  /**
   * 验证登录账号（4-20 位小写字母开头）
   * @param userName 登录账号
   * @returns 是否有效
   */
  static isValidLoginUserName(userName: string): boolean {
    return this.test('LOGIN_USER_NAME', userName)
  }

  /**
   * 验证租户编码（3 位数字）
   * @param tenantCode 租户编码
   * @returns 是否有效
   */
  static isValidTenantCode(tenantCode: string): boolean {
    return this.test('TENANT_CODE', tenantCode)
  }

  /**
   * 验证真实姓名
   * @param realName 真实姓名
   * @returns 是否有效
   */
  static isValidRealName(realName: string): boolean {
    return this.test('REAL_NAME', realName)
  }

  /**
   * 检查字符串是否符合帕斯卡命名规范（仅英文部分）
   * @param text 要检查的文本
   * @returns 是否符合帕斯卡命名规范
   */
  private static isPascalCase(text: string): boolean {
    if (!text) return false
    
    // 如果包含中文字符，不需要验证帕斯卡命名
    if (/[\u4e00-\u9fa5]/.test(text)) {
      return true
    }
    
    // 按点、横线、空格、单引号、下划线分割成单词部分
    const parts = text.split(/[.\s\-'_]+/).filter(part => part.length > 0)
    
    if (parts.length === 0) return false
    
    // 检查每个单词部分的首字母是否大写
    for (const part of parts) {
      // 找到第一个字母的位置
      const firstLetterIndex = part.search(/[a-zA-Z]/)
      if (firstLetterIndex === -1) {
        // 如果没有字母（只有数字），跳过
        continue
      }
      const firstLetter = part.charAt(firstLetterIndex)
      // 首字母必须是大写
      if (firstLetter !== firstLetter.toUpperCase()) {
        return false
      }
    }
    
    return true
  }

  /**
   * 将字符串转换为帕斯卡命名（PascalCase）
   * @param text 要转换的文本
   * @returns 转换后的帕斯卡命名文本
   */
  static toPascalCase(text: string): string {
    if (!text) return ''
    
    // 如果包含中文字符，不转换
    if (/[\u4e00-\u9fa5]/.test(text)) {
      return text
    }
    
    // 按点、横线、空格、单引号、下划线分割，但保留分隔符
    const separatorPattern = /([.\s\-'_]+)/
    const parts = text.split(separatorPattern)
    const result: string[] = []
    
    for (let i = 0; i < parts.length; i++) {
      const part = parts[i]
      if (!part) continue
      
      // 如果是分隔符，直接添加
      if (separatorPattern.test(part)) {
        result.push(part)
        continue
      }
      
      // 处理单词部分
      const firstLetterIndex = part.search(/[a-zA-Z]/)
      if (firstLetterIndex === -1) {
        // 如果没有字母（只有数字），直接添加
        result.push(part)
        continue
      }
      
      // 将首字母转为大写，其余转为小写
      const prefix = part.substring(0, firstLetterIndex)
      const firstLetter = part.charAt(firstLetterIndex).toUpperCase()
      const rest = part.substring(firstLetterIndex + 1).toLowerCase()
      result.push(prefix + firstLetter + rest)
    }
    
    return result.join('')
  }

  /**
   * 验证全名
   * @param fullName 全名
   * @returns 是否有效
   */
  static isValidFullName(fullName: string): boolean {
    if (!this.test('FULL_NAME', fullName)) {
      return false
    }
    // 如果是英文，必须符合帕斯卡命名规范
    return this.isPascalCase(fullName)
  }

  /**
   * 验证昵称
   * @param nickName 昵称
   * @returns 是否有效
   */
  static isValidNickName(nickName: string): boolean {
    if (!this.test('NICK_NAME', nickName)) {
      return false
    }
    // 如果是英文，必须符合帕斯卡命名规范
    return this.isPascalCase(nickName)
  }

  /**
   * 用户表昵称（与后端 `TaktRegexHelper.nickName` 一致）：可为空；非空时仅校验 `NICK_NAME` 模式，不要求帕斯卡。
   */
  static isValidUserNickName(nickName: string): boolean {
    const v = (nickName ?? '').trim()
    if (!v) return true
    return this.test('NICK_NAME', v)
  }

  /**
   * 员工姓名（与后端 `TaktRegexHelper.EmployeeName` 一致）：2–80 位。
   * @param employeeName 员工姓名
   * @returns 是否有效
   */
  static isValidEmployeeName(employeeName: string): boolean {
    return this.test('EMPLOYEE_NAME', (employeeName ?? '').trim())
  }

  /**
   * 验证姓/名（仅英文与数字，首字母必须大写，数字不能在首位，1-100位）
   * @param name 姓或名
   * @param allowEmpty 是否允许为空（姓选填为 true，名必填为 false）
   * @returns 是否有效
   */
  static isValidFirstOrLastName(name: string, allowEmpty: boolean): boolean {
    if (!name || !name.trim()) {
      return allowEmpty
    }
    return name.length >= 1 && name.length <= 100 && this.test('NAME_EN', name)
  }

  /**
   * 验证英文名
   * @param englishName 英文名
   * @returns 是否有效
   */
  static isValidEnglishName(englishName: string): boolean {
    if (!englishName || englishName.length < 2 || englishName.length > 30) {
      return false
    }
    if (!this.test('ENGLISH_NAME', englishName)) {
      return false
    }
    // 英文名必须符合帕斯卡命名规范
    return this.isPascalCase(englishName)
  }

  /**
   * 验证密码强度
   * @param password 密码
   * @param strong 兼容参数，当前始终按强密码校验
   * @returns 是否有效
   */
  static isValidPassword(password: string, strong: boolean = true): boolean {
    void strong
    return this.test('PASSWORD', password)
  }

  /**
   * 验证日期格式
   * @param date 日期字符串
   * @returns 是否有效
   */
  static isValidDate(date: string): boolean {
    if (!this.test('DATE', date)) {
      return false
    }

    // 进一步验证日期是否真实存在
    const dateObj = new Date(date)
    return dateObj instanceof Date && !isNaN(dateObj.getTime())
  }

  /**
   * 验证时间格式
   * @param time 时间字符串
   * @returns 是否有效
   */
  static isValidTime(time: string): boolean {
    return this.test('TIME', time)
  }

  /**
   * 验证日期时间格式
   * @param datetime 日期时间字符串
   * @returns 是否有效
   */
  static isValidDateTime(datetime: string): boolean {
    if (!this.test('DATETIME', datetime)) {
      return false
    }

    // 进一步验证日期时间是否真实存在
    const dateObj = new Date(datetime.replace(/\s+/, 'T'))
    return dateObj instanceof Date && !isNaN(dateObj.getTime())
  }

  /**
   * 验证银行卡号
   * @param cardNumber 银行卡号
   * @returns 是否有效
   */
  static isValidBankCard(cardNumber: string): boolean {
    return this.test('BANK_CARD', cardNumber)
  }

  /**
   * 验证车牌号（中国大陆）
   * @param plate 车牌号
   * @returns 是否有效
   */
  static isValidLicensePlate(plate: string): boolean {
    return this.test('LICENSE_PLATE_CN', plate)
      || this.test('LICENSE_PLATE_US', plate)
      || this.test('LICENSE_PLATE_JP', plate)
      || this.test('LICENSE_PLATE_TW', plate)
      || this.test('LICENSE_PLATE_HK', plate)
  }

  /**
   * 验证车牌号（中国大陆）
   */
  static isValidLicensePlateCn(plate: string): boolean {
    return this.test('LICENSE_PLATE_CN', plate)
  }

  /**
   * 验证车牌号（美国）
   */
  static isValidLicensePlateUs(plate: string): boolean {
    return this.test('LICENSE_PLATE_US', plate)
  }

  /**
   * 验证车牌号（日本）
   */
  static isValidLicensePlateJp(plate: string): boolean {
    return this.test('LICENSE_PLATE_JP', plate)
  }

  /**
   * 验证车牌号（中国台湾）
   */
  static isValidLicensePlateTw(plate: string): boolean {
    return this.test('LICENSE_PLATE_TW', plate)
  }

  /**
   * 验证车牌号（中国香港）
   */
  static isValidLicensePlateHk(plate: string): boolean {
    return this.test('LICENSE_PLATE_HK', plate)
  }

  /**
   * 提取邮箱地址
   * @param text 文本
   * @returns 所有邮箱地址数组
   */
  static extractEmails(text: string): string[] {
    const matches = this.matchAll('EMAIL', text, 'g')
    return matches.map(match => match[0])
  }

  /**
   * 提取手机号
   * @param text 文本
   * @returns 所有手机号数组
   */
  static extractPhones(text: string): string[] {
    const matches = this.matchAll('PHONE_CN', text, 'g')
    return matches.map(match => match[0])
  }

  /**
   * 提取URL地址
   * @param text 文本
   * @returns 所有URL地址数组
   */
  static extractUrls(text: string): string[] {
    const matches = this.matchAll('URL', text, 'g')
    return matches.map(match => match[0])
  }

  /**
   * 提取IP地址
   * @param text 文本
   * @returns 所有IP地址数组
   */
  static extractIps(text: string): string[] {
    const ipv4Matches = this.matchAll('IPV4', text, 'g')
    const ipv6Matches = this.matchAll('IPV6', text, 'g')
    return [
      ...ipv4Matches.map(match => match[0]),
      ...ipv6Matches.map(match => match[0])
    ]
  }

  /**
   * 移除所有空白字符
   * @param text 文本
   * @returns 移除空白字符后的文本
   */
  static removeWhitespace(text: string): string {
    return this.replace(/\s+/g, text, '')
  }

  /**
   * 移除首尾空白字符
   * @param text 文本
   * @returns 移除首尾空白字符后的文本
   */
  static trim(text: string): string {
    return text.trim()
  }

  /**
   * 转义正则表达式特殊字符
   * @param text 文本
   * @returns 转义后的文本
   */
  static escape(text: string): string {
    return text.replace(/[.*+?^${}()|[\]\\]/g, '\\$&')
  }

  /**
   * 验证统一社会信用代码
   * @param code 统一社会信用代码
   * @returns 是否有效
   */
  static isValidUnifiedSocialCreditCode(code: string): boolean {
    return this.test('UNIFIED_SOCIAL_CREDIT_CODE', code)
  }

  /**
   * 验证QQ号
   * @param qq QQ号
   * @returns 是否有效
   */
  static isValidQQ(qq: string): boolean {
    return this.test('QQ', qq)
  }

  /**
   * 验证微信号
   * @param wechat 微信号
   * @returns 是否有效
   */
  static isValidWechat(wechat: string): boolean {
    return this.test('WECHAT', wechat)
  }

  /**
   * 验证MAC地址
   * @param mac MAC地址
   * @returns 是否有效
   */
  static isValidMacAddress(mac: string): boolean {
    return this.test('MAC_ADDRESS', mac)
  }

  /**
   * 验证颜色值（十六进制）
   * @param color 颜色值
   * @returns 是否有效
   */
  static isValidHexColor(color: string): boolean {
    return this.test('HEX_COLOR', color)
  }

  /**
   * 验证版本号
   * @param version 版本号
   * @returns 是否有效
   */
  static isValidVersion(version: string): boolean {
    return this.test('VERSION', version)
  }

  /**
   * 验证通用编码
   * @param code 编码
   * @returns 是否有效
   */
  static isValidCode(code: string): boolean {
    return this.test('CODE', code)
  }

  /**
   * 验证物料编码
   * @param code 物料编码
   * @returns 是否有效
   */
  static isValidMaterialCode(code: string): boolean {
    return this.test('MATERIAL_CODE', code)
  }

  /**
   * 验证文档编码
   * @param code 文档编码
   * @returns 是否有效
   */
  static isValidDocumentCode(code: string): boolean {
    return this.test('DOCUMENT_CODE', code)
  }

  /**
   * 验证订单编码
   * @param code 订单编码
   * @returns 是否有效
   */
  static isValidOrderCode(code: string): boolean {
    return this.test('ORDER_CODE', code)
  }

  /**
   * 验证批次编码
   * @param code 批次编码
   * @returns 是否有效
   */
  static isValidBatchCode(code: string): boolean {
    return this.test('BATCH_CODE', code)
  }

  /**
   * 验证序列号
   * @param serialNumber 序列号
   * @returns 是否有效
   */
  static isValidSerialNumber(serialNumber: string): boolean {
    return this.test('SERIAL_NUMBER', serialNumber)
  }

  /**
   * 验证带日期的编码
   * @param code 编码（格式：YYYYMMDD + 序号）
   * @returns 是否有效
   */
  static isValidCodeWithDate(code: string): boolean {
    return this.test('CODE_WITH_DATE', code)
  }

  /**
   * 验证带前缀的编码
   * @param code 编码（格式：前缀-序号）
   * @returns 是否有效
   */
  static isValidCodeWithPrefix(code: string): boolean {
    return this.test('CODE_WITH_PREFIX', code)
  }

  /**
   * 验证带分隔符的编码
   * @param code 编码（多个部分用横线或下划线分隔）
   * @returns 是否有效
   */
  static isValidCodeWithSeparator(code: string): boolean {
    return this.test('CODE_WITH_SEPARATOR', code)
  }

  /**
   * 验证数字编码
   * @param code 数字编码
   * @returns 是否有效
   */
  static isValidNumericCode(code: string): boolean {
    return this.test('NUMERIC_CODE', code)
  }

  /**
   * 验证字母编码
   * @param code 字母编码
   * @returns 是否有效
   */
  static isValidAlphabeticCode(code: string): boolean {
    return this.test('ALPHABETIC_CODE', code)
  }

  /**
   * 验证大写字母编码
   * @param code 大写字母编码
   * @returns 是否有效
   */
  static isValidUppercaseCode(code: string): boolean {
    return this.test('UPPERCASE_CODE', code)
  }

  /**
   * 验证小写字母编码
   * @param code 小写字母编码
   * @returns 是否有效
   */
  static isValidLowercaseCode(code: string): boolean {
    return this.test('LOWERCASE_CODE', code)
  }

  /**
   * 验证菜单编码
   * @param code 菜单编码
   * @returns 是否有效
   */
  static isValidMenuCode(code: string): boolean {
    return this.test('MENU_CODE', code)
  }

  /**
   * 验证国际化键
   * @param key 国际化键
   * @returns 是否有效
   */
  static isValidL10nKey(key: string): boolean {
    return this.test('L10N_KEY', key)
  }

  /**
   * 验证角色编码
   * @param code 角色编码
   * @returns 是否有效
   */
  static isValidRoleCode(code: string): boolean {
    return this.test('ROLE_CODE', code)
  }

  /**
   * 验证部门编码
   * @param code 部门编码
   * @returns 是否有效
   */
  static isValidDeptCode(code: string): boolean {
    return this.test('DEPT_CODE', code)
  }

  /**
   * 验证岗位编码
   * @param code 岗位编码
   * @returns 是否有效
   */
  static isValidPostCode(code: string): boolean {
    return this.test('POST_CODE', code)
  }

  /**
   * 验证权限标识
   * @param permission 权限标识（格式：模块:资源:操作）
   * @returns 是否有效
   */
  static isValidPermission(permission: string): boolean {
    if (!permission || permission.length < 3 || permission.length > 100) {
      return false
    }
    return this.test('PERMISSION', permission)
  }
}

/**
 * 便捷导出：直接使用函数形式
 * 注意：这些是静态方法的引用，可以直接调用
 */
export const regexTest = RegexHelper.test.bind(RegexHelper)
export const regexMatch = RegexHelper.match.bind(RegexHelper)
export const regexMatchAll = RegexHelper.matchAll.bind(RegexHelper)
export const regexReplace = RegexHelper.replace.bind(RegexHelper)
export const regexExtractGroups = RegexHelper.extractGroups.bind(RegexHelper)
export const isValidEmail = RegexHelper.isValidEmail.bind(RegexHelper)
export const isValidPhone = RegexHelper.isValidPhone.bind(RegexHelper)
export const isValidPhoneByCulture = RegexHelper.isValidPhoneByCulture.bind(RegexHelper)
export const isValidIdCard = RegexHelper.isValidIdCard.bind(RegexHelper)
export const isValidPhoneCn = RegexHelper.isValidPhoneCn.bind(RegexHelper)
export const isValidPhoneTw = RegexHelper.isValidPhoneTw.bind(RegexHelper)
export const isValidPhoneHk = RegexHelper.isValidPhoneHk.bind(RegexHelper)
export const isValidPhoneUs = RegexHelper.isValidPhoneUs.bind(RegexHelper)
export const isValidPhoneJp = RegexHelper.isValidPhoneJp.bind(RegexHelper)
export const isValidIdCardCn = RegexHelper.isValidIdCardCn.bind(RegexHelper)
export const isValidIdCardTw = RegexHelper.isValidIdCardTw.bind(RegexHelper)
export const isValidIdCardHk = RegexHelper.isValidIdCardHk.bind(RegexHelper)
export const isValidIdCardUs = RegexHelper.isValidIdCardUs.bind(RegexHelper)
export const isValidIdCardJp = RegexHelper.isValidIdCardJp.bind(RegexHelper)
export const isValidUrl = RegexHelper.isValidUrl.bind(RegexHelper)
export const isValidIp = RegexHelper.isValidIp.bind(RegexHelper)
export const isValidUserName = RegexHelper.isValidUserName.bind(RegexHelper)
export const isValidLoginUserName = RegexHelper.isValidLoginUserName.bind(RegexHelper)
export const isValidTenantCode = RegexHelper.isValidTenantCode.bind(RegexHelper)
export const isValidRealName = RegexHelper.isValidRealName.bind(RegexHelper)
export const isValidFullName = RegexHelper.isValidFullName.bind(RegexHelper)
export const isValidNickName = RegexHelper.isValidNickName.bind(RegexHelper)
export const isValidUserNickName = RegexHelper.isValidUserNickName.bind(RegexHelper)
export const isValidEmployeeName = RegexHelper.isValidEmployeeName.bind(RegexHelper)
export const isValidFirstOrLastName = RegexHelper.isValidFirstOrLastName.bind(RegexHelper)
export const isValidEnglishName = RegexHelper.isValidEnglishName.bind(RegexHelper)
export const isValidPassword = RegexHelper.isValidPassword.bind(RegexHelper)
export const isValidDate = RegexHelper.isValidDate.bind(RegexHelper)
export const toPascalCase = RegexHelper.toPascalCase.bind(RegexHelper)
export const isValidTime = RegexHelper.isValidTime.bind(RegexHelper)
export const isValidDateTime = RegexHelper.isValidDateTime.bind(RegexHelper)
export const isValidBankCard = RegexHelper.isValidBankCard.bind(RegexHelper)
export const isValidLicensePlate = RegexHelper.isValidLicensePlate.bind(RegexHelper)
export const isValidLicensePlateCn = RegexHelper.isValidLicensePlateCn.bind(RegexHelper)
export const isValidLicensePlateUs = RegexHelper.isValidLicensePlateUs.bind(RegexHelper)
export const isValidLicensePlateJp = RegexHelper.isValidLicensePlateJp.bind(RegexHelper)
export const isValidLicensePlateTw = RegexHelper.isValidLicensePlateTw.bind(RegexHelper)
export const isValidLicensePlateHk = RegexHelper.isValidLicensePlateHk.bind(RegexHelper)
export const extractEmails = RegexHelper.extractEmails.bind(RegexHelper)
export const extractPhones = RegexHelper.extractPhones.bind(RegexHelper)
export const extractUrls = RegexHelper.extractUrls.bind(RegexHelper)
export const extractIps = RegexHelper.extractIps.bind(RegexHelper)
export const removeWhitespace = RegexHelper.removeWhitespace.bind(RegexHelper)
export const regexTrim = RegexHelper.trim.bind(RegexHelper)
export const regexEscape = RegexHelper.escape.bind(RegexHelper)
export const isValidUnifiedSocialCreditCode = RegexHelper.isValidUnifiedSocialCreditCode.bind(RegexHelper)
export const isValidQQ = RegexHelper.isValidQQ.bind(RegexHelper)
export const isValidWechat = RegexHelper.isValidWechat.bind(RegexHelper)
export const isValidMacAddress = RegexHelper.isValidMacAddress.bind(RegexHelper)
export const isValidHexColor = RegexHelper.isValidHexColor.bind(RegexHelper)
export const isValidVersion = RegexHelper.isValidVersion.bind(RegexHelper)
export const isValidCode = RegexHelper.isValidCode.bind(RegexHelper)
export const isValidMaterialCode = RegexHelper.isValidMaterialCode.bind(RegexHelper)
export const isValidDocumentCode = RegexHelper.isValidDocumentCode.bind(RegexHelper)
export const isValidOrderCode = RegexHelper.isValidOrderCode.bind(RegexHelper)
export const isValidBatchCode = RegexHelper.isValidBatchCode.bind(RegexHelper)
export const isValidSerialNumber = RegexHelper.isValidSerialNumber.bind(RegexHelper)
export const isValidCodeWithDate = RegexHelper.isValidCodeWithDate.bind(RegexHelper)
export const isValidCodeWithPrefix = RegexHelper.isValidCodeWithPrefix.bind(RegexHelper)
export const isValidCodeWithSeparator = RegexHelper.isValidCodeWithSeparator.bind(RegexHelper)
export const isValidNumericCode = RegexHelper.isValidNumericCode.bind(RegexHelper)
export const isValidAlphabeticCode = RegexHelper.isValidAlphabeticCode.bind(RegexHelper)
export const isValidUppercaseCode = RegexHelper.isValidUppercaseCode.bind(RegexHelper)
export const isValidLowercaseCode = RegexHelper.isValidLowercaseCode.bind(RegexHelper)
export const isValidMenuCode = RegexHelper.isValidMenuCode.bind(RegexHelper)
export const isValidL10nKey = RegexHelper.isValidL10nKey.bind(RegexHelper)
export const isValidRoleCode = RegexHelper.isValidRoleCode.bind(RegexHelper)
export const isValidDeptCode = RegexHelper.isValidDeptCode.bind(RegexHelper)
export const isValidPostCode = RegexHelper.isValidPostCode.bind(RegexHelper)
export const isValidPermission = RegexHelper.isValidPermission.bind(RegexHelper)
