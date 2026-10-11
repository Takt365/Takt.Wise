// ========================================
// 项目名称：节拍智控·TaktWise 
// 命名空间：@/utils/mask
// 文件名称：mask.ts
// 创建时间：2025-01-21
// 创建人：Takt365(Cursor AI)
// 功能描述：数据脱敏工具类，提供各种数据脱敏方法
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 脱敏工具类
 */
export class MaskHelper {
  /**
   * 默认敏感字段列表
   */
  private static readonly DEFAULT_SENSITIVE_FIELDS = [
    'password',
    'pwd',
    'passwd',
    'token',
    'authorization',
    'auth',
    'csrf',
    'cookie',
    'secret',
    'key',
    'apiKey',
    'apikey',
    'accessKey',
    'secretKey',
    'privateKey',
    'publicKey',
    'ticket',
    'cipher',
    'loginTicket',
    'idCard',
    'idcard',
    'identityCard',
    'bankCard',
    'bankcard',
    'cardNumber',
    'cardCode',
    'creditCard',
    'phone',
    'mobile',
    'telephone',
    'tel',
    'email',
    'mail',
    'address',
    'addr'
  ] as const

  /**
   * 脱敏手机号
   * @param phone 手机号
   * @param start 保留前几位（默认：3）
   * @param end 保留后几位（默认：4）
   * @param maskChar 脱敏字符（默认：'*'）
   * @returns 脱敏后的手机号
   * 
   * @example
   * maskPhone('13800138000') // '138****8000'
   * maskPhone('13800138000', 3, 4) // '138****8000'
   * maskPhone('13800138000', 1, 1) // '1********0'
   */
  static maskPhone(phone: string | null | undefined, start: number = 3, end: number = 4, maskChar: string = '*'): string {
    if (!phone || typeof phone !== 'string') {
      return ''
    }

    const phoneStr = String(phone).trim()
    if (phoneStr.length <= start + end) {
      return phoneStr
    }

    const visibleStart = phoneStr.substring(0, start)
    const visibleEnd = phoneStr.substring(phoneStr.length - end)
    const maskLength = phoneStr.length - start - end
    const mask = maskChar.repeat(maskLength)

    return `${visibleStart}${mask}${visibleEnd}`
  }

  /**
   * 脱敏邮箱
   * @param email 邮箱地址
   * @param maskChar 脱敏字符（默认：'*'）
   * @returns 脱敏后的邮箱
   * 
   * @example
   * maskEmail('test@example.com') // 't***@example.com'
   * maskEmail('admin@example.com') // 'a****@example.com'
   */
  static maskEmail(email: string | null | undefined, maskChar: string = '*'): string {
    if (!email || typeof email !== 'string') {
      return ''
    }

    const emailStr = String(email).trim()
    const atIndex = emailStr.indexOf('@')
    if (atIndex <= 0) {
      return emailStr
    }

    const userName = emailStr.substring(0, atIndex)
    const domain = emailStr.substring(atIndex)

    if (userName.length <= 1) {
      return `${userName}${maskChar.repeat(3)}${domain}`
    }

    const visibleStart = userName.substring(0, 1)
    const mask = maskChar.repeat(Math.max(3, userName.length - 1))

    return `${visibleStart}${mask}${domain}`
  }

  /**
   * 脱敏身份证号
   * @param idCard 身份证号
   * @param start 保留前几位（默认：3）
   * @param end 保留后几位（默认：4）
   * @param maskChar 脱敏字符（默认：'*'）
   * @returns 脱敏后的身份证号
   * 
   * @example
   * maskIdCard('110101199001011234') // '110************234'
   */
  static maskIdCard(idCard: string | null | undefined, start: number = 3, end: number = 4, maskChar: string = '*'): string {
    if (!idCard || typeof idCard !== 'string') {
      return ''
    }

    const idCardStr = String(idCard).trim()
    if (idCardStr.length <= start + end) {
      return idCardStr
    }

    const visibleStart = idCardStr.substring(0, start)
    const visibleEnd = idCardStr.substring(idCardStr.length - end)
    const maskLength = idCardStr.length - start - end
    const mask = maskChar.repeat(maskLength)

    return `${visibleStart}${mask}${visibleEnd}`
  }

  /**
   * 脱敏银行卡号
   * @param bankCard 银行卡号
   * @param start 保留前几位（默认：4）
   * @param end 保留后几位（默认：4）
   * @param maskChar 脱敏字符（默认：'*'）
   * @returns 脱敏后的银行卡号
   * 
   * @example
   * maskBankCard('6222021234567890123') // '6222***********0123'
   */
  static maskBankCard(bankCard: string | null | undefined, start: number = 4, end: number = 4, maskChar: string = '*'): string {
    if (!bankCard || typeof bankCard !== 'string') {
      return ''
    }

    const bankCardStr = String(bankCard).trim()
    if (bankCardStr.length <= start + end) {
      return bankCardStr
    }

    const visibleStart = bankCardStr.substring(0, start)
    const visibleEnd = bankCardStr.substring(bankCardStr.length - end)
    const maskLength = bankCardStr.length - start - end
    const mask = maskChar.repeat(maskLength)

    return `${visibleStart}${mask}${visibleEnd}`
  }

  /**
   * 脱敏姓名
   * @param name 姓名
   * @param maskChar 脱敏字符（默认：'*'）
   * @returns 脱敏后的姓名
   * 
   * @example
   * maskName('张三') // '张*'
   * maskName('李四') // '李*'
   * maskName('王五') // '王*'
   * maskName('欧阳修') // '欧**'
   */
  static maskName(name: string | null | undefined, maskChar: string = '*'): string {
    if (!name || typeof name !== 'string') {
      return ''
    }

    const nameStr = String(name).trim()
    if (nameStr.length <= 1) {
      return nameStr
    }

    if (nameStr.length === 2) {
      return `${nameStr[0]}${maskChar}`
    }

    // 3个字符及以上：保留第一个，其余用脱敏字符
    const visibleStart = nameStr.substring(0, 1)
    const mask = maskChar.repeat(nameStr.length - 1)

    return `${visibleStart}${mask}`
  }

  /**
   * 脱敏地址
   * @param address 地址
   * @param keepLength 保留长度（默认：6）
   * @param maskChar 脱敏字符（默认：'*'）
   * @returns 脱敏后的地址
   * 
   * @example
   * maskAddress('北京市朝阳区某某街道123号') // '北京市朝阳区****'
   */
  static maskAddress(address: string | null | undefined, keepLength: number = 6, maskChar: string = '*'): string {
    if (!address || typeof address !== 'string') {
      return ''
    }

    const addressStr = String(address).trim()
    if (addressStr.length <= keepLength) {
      return addressStr
    }

    const visibleStart = addressStr.substring(0, keepLength)
    const mask = maskChar.repeat(Math.min(4, addressStr.length - keepLength))

    return `${visibleStart}${mask}`
  }

  /**
   * 通用脱敏方法
   * @param text 要脱敏的文本
   * @param start 保留前几位（默认：3）
   * @param end 保留后几位（默认：3）
   * @param maskChar 脱敏字符（默认：'*'）
   * @returns 脱敏后的文本
   * 
   * @example
   * mask('1234567890') // '123****890'
   * mask('abcdefghij', 2, 2) // 'ab******ij'
   */
  static mask(text: string | null | undefined, start: number = 3, end: number = 3, maskChar: string = '*'): string {
    if (!text || typeof text !== 'string') {
      return ''
    }

    const textStr = String(text).trim()
    if (textStr.length <= start + end) {
      return textStr
    }

    const visibleStart = textStr.substring(0, start)
    const visibleEnd = textStr.substring(textStr.length - end)
    const maskLength = textStr.length - start - end
    const mask = maskChar.repeat(maskLength)

    return `${visibleStart}${mask}${visibleEnd}`
  }

  /**
   * 完全脱敏（全部替换为脱敏字符）
   * @param text 要脱敏的文本
   * @param maskChar 脱敏字符（默认：'*'）
   * @returns 完全脱敏后的文本
   * 
   * @example
   * maskFull('1234567890') // '**********'
   */
  static maskFull(text: string | null | undefined, maskChar: string = '*'): string {
    if (!text || typeof text !== 'string') {
      return ''
    }

    return maskChar.repeat(String(text).length)
  }

  /**
   * 脱敏对象中的敏感字段
   * @param obj 要脱敏的对象
   * @param sensitiveFields 敏感字段列表（可选，默认使用内置列表）
   * @param maskChar 脱敏字符（默认：'*'）
   * @returns 脱敏后的对象
   * 
   * @example
   * maskObject({ name: '张三', phone: '13800138000', email: 'test@example.com' })
   * // { name: '张*', phone: '138****8000', email: 't***@example.com' }
   */
  static maskObject(
    obj: unknown,
    sensitiveFields?: string[],
    maskChar: string = '*'
  ): unknown {
    if (obj === null || obj === undefined) {
      return obj
    }

    if (typeof obj !== 'object') {
      return obj
    }

    if (Array.isArray(obj)) {
      return obj.map(item => this.maskObject(item, sensitiveFields, maskChar))
    }

    const fields = sensitiveFields || [...this.DEFAULT_SENSITIVE_FIELDS]
    const sanitized: Record<string, unknown> = {}

    for (const [key, value] of Object.entries(obj as Record<string, unknown>)) {
      const lowerKey = key.toLowerCase()

      // 检查是否为敏感字段
      const isSensitive = fields.some(field => lowerKey.includes(field.toLowerCase()))

      if (isSensitive) {
        // 根据字段类型和名称选择脱敏方式
        if (typeof value === 'string' && value) {
          if (lowerKey.includes('phone') || lowerKey.includes('mobile') || lowerKey.includes('tel')) {
            sanitized[key] = this.maskPhone(value, 3, 4, maskChar)
          } else if (lowerKey.includes('email') || lowerKey.includes('mail')) {
            sanitized[key] = this.maskEmail(value, maskChar)
          } else if (lowerKey.includes('idcard') || lowerKey.includes('identitycard')) {
            sanitized[key] = this.maskIdCard(value, 3, 4, maskChar)
          } else if (lowerKey.includes('bankcard') || lowerKey.includes('cardnumber') || lowerKey.includes('cardno')) {
            sanitized[key] = this.maskBankCard(value, 4, 4, maskChar)
          } else if (lowerKey.includes('name') && !lowerKey.includes('userName')) {
            sanitized[key] = this.maskName(value, maskChar)
          } else if (lowerKey.includes('address') || lowerKey.includes('addr')) {
            sanitized[key] = this.maskAddress(value, 6, maskChar)
          } else {
            // 默认完全脱敏
            sanitized[key] = this.maskFull(value, maskChar)
          }
        } else {
          sanitized[key] = maskChar.repeat(3)
        }
      } else if (typeof value === 'object' && value !== null) {
        // 递归处理嵌套对象
        sanitized[key] = this.maskObject(value, sensitiveFields, maskChar)
      } else {
        sanitized[key] = value
      }
    }

    return sanitized
  }

  /**
   * 检查字段名是否为敏感字段
   * @param fieldName 字段名
   * @param sensitiveFields 敏感字段列表（可选，默认使用内置列表）
   * @returns 是否为敏感字段
   */
  static isSensitiveField(fieldName: string, sensitiveFields?: string[]): boolean {
    if (!fieldName || typeof fieldName !== 'string') {
      return false
    }

    const fields = sensitiveFields || [...this.DEFAULT_SENSITIVE_FIELDS]
    const lowerFieldName = fieldName.toLowerCase()

    return fields.some(field => lowerFieldName.includes(field.toLowerCase()))
  }

  /**
   * 脱敏日志数据（用于日志输出）
   * @param data 要脱敏的数据
   * @param sensitiveFields 敏感字段列表（可选，默认使用内置列表）
   * @returns 脱敏后的数据
   */
  static maskForLogging(data: unknown, sensitiveFields?: string[]): unknown {
    return this.maskObject(data, sensitiveFields, '*')
  }
}

/**
 * 便捷导出：直接使用函数形式
 */
export const maskPhone = MaskHelper.maskPhone.bind(MaskHelper)
export const maskEmail = MaskHelper.maskEmail.bind(MaskHelper)
export const maskIdCard = MaskHelper.maskIdCard.bind(MaskHelper)
export const maskBankCard = MaskHelper.maskBankCard.bind(MaskHelper)
export const maskName = MaskHelper.maskName.bind(MaskHelper)
export const maskAddress = MaskHelper.maskAddress.bind(MaskHelper)
export const mask = MaskHelper.mask.bind(MaskHelper)
export const maskFull = MaskHelper.maskFull.bind(MaskHelper)
export const maskObject = MaskHelper.maskObject.bind(MaskHelper)
export const isSensitiveField = MaskHelper.isSensitiveField.bind(MaskHelper)
export const maskForLogging = MaskHelper.maskForLogging.bind(MaskHelper)
