// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/utils/crypto
// 文件名称：crypto.ts
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：登录密码 RSA 传输加密（crypto-js 编码 + JSEncrypt PKCS#1）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import CryptoJS from 'crypto-js';
import { JSEncrypt } from 'jsencrypt';

/**
 * 使用 RSA 公钥加密登录密码（PKCS#1 v1.5，输出 Base64 密文）
 * @param {string} plainPassword 表单明文密码
 * @param {string} publicKeyPem 服务端下发的 RSA 公钥 PEM
 * @returns {string} Base64 密文；加密失败时返回空字符串
 */
export function encryptLoginPassword(plainPassword: string, publicKeyPem: string): string {
  // 明文或公钥缺失时无法加密，返回空串由调用方处理
  if (!plainPassword || !publicKeyPem) {
    return '';
  }

  /** JSEncrypt 实例，用于 PKCS#1 加密 */
  const encryptor = new JSEncrypt();
  // 注入服务端 RSA 公钥
  encryptor.setPublicKey(publicKeyPem);

  /** UTF-8 规范化后的明文字符串 */
  const utf8Plain = CryptoJS.enc.Utf8.stringify(CryptoJS.enc.Utf8.parse(plainPassword));
  // 执行 RSA 加密
  const cipher = encryptor.encrypt(utf8Plain);

  // JSEncrypt 失败时返回 false，统一转为空串
  return typeof cipher === 'string' ? cipher : '';
}
