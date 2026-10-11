// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：scripts
// 文件名称：xml-cref-strip.cjs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：see cref 转纯文本（供 remove-xml-cref 与代码生成 sanitize 共用）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 将 cref 目标转为纯文本
 * @param {string} raw
 * @returns {string}
 */
function crefToPlainText(raw) {
  let cref = String(raw).trim();
  cref = cref.replace(/^[TMFPN]:/, '');
  cref = cref.replace(/\(.*\)$/, '');

  if (cref === 'HttpContext.Items') {
    return 'HttpContext.Items';
  }

  const parts = cref.split('.').filter(Boolean);
  if (parts.length === 0) {
    return cref;
  }

  const last = parts[parts.length - 1];
  const secondLast = parts.length >= 2 ? parts[parts.length - 2] : '';

  const isTaktType = (name) => /^I?Takt[A-Z]/.test(name);
  const isSimpleMember = (name) =>
    name === 'Id'
    || name === 'TenantCode'
    || name === 'CompanyCode'
    || name === 'Permission'
    || name === 'Items'
    || name === 'Value';

  if (parts.length >= 2) {
    if (isTaktType(secondLast) || isSimpleMember(last)) {
      return `${secondLast}.${last}`;
    }
    if (isTaktType(last)) {
      return last;
    }
    return `${secondLast}.${last}`;
  }

  return last;
}

/**
 * 替换内容中全部 see cref
 * @param {string} content
 * @returns {string}
 */
function stripSeeCref(content) {
  return content.replace(
    /<see\s+cref=(?:"([^"]+)"|'([^']+)'|&quot;([^&]+)&quot;)\s*\/?>/gi,
    (_, a, b, c) => crefToPlainText(a || b || c),
  ).replace(
    /<see\s+cref=\\"([^\\"]+)\\"\s*\/?>/gi,
    (_, inner) => crefToPlainText(inner),
  );
}

module.exports = {
  crefToPlainText,
  stripSeeCref,
};
