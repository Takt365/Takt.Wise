// ========================================
// 项目名称：节拍智控·TaktWise (TDF)
// 命名空间：frontend/src/locales
// 文件名称：index.ts
// 创建时间：2026-05-22
// 创建人：Takt365(Cursor AI)
// 功能描述：国际化配置（vue-i18n）
//           · 本地静态：import.meta.glob 按目录路径嵌套（如 locales/dashboard/workspace → dashboard.workspace.page.*）
//           · 语言文件内顶级节点必须为 page（引用键：业务目录.page.*，勿重复 common/login 等目录名）
//           · 后端动态：登录后或登录页租户校验通过后由 useTranslationStore 合并
//           · 租户启用语言以 GetCultureOptions 为准；本目录 glob 仅决定静态包有哪些 locale 键
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { createI18n } from 'vue-i18n';
import {
  buildNestedLocaleMessages,
  registerTaktI18n,
  resolveLocaleMessageFromTree,
} from '@/utils/taktI18nMessage';
import { TAKT_DEFAULT_LOCALE, TAKT_LOCALE_STORAGE_KEY } from '@/utils/common';
import {
  deepMergeLocaleMessages,
  nestLocalePayloadUnderNamespace,
  resolveLocaleNamespaceSegments,
  type TaktLocaleMessageTree,
} from '@/utils/taktLocaleMerge';

/** 单语言包消息结构 */
type TaktLocaleMessages = TaktLocaleMessageTree;

/** 语言文件后缀（如 common/zh-CN.ts） */
const LOCALE_FILE_SUFFIX_PATTERN = /\/([a-z]{2}-[A-Z]{2})\.ts$/;

/** 构建期收集 locales 目录下全部静态翻译模块（排除本文件） */
const localeModules = import.meta.glob<{ default: TaktLocaleMessages }>('./**/*.ts', {
  eager: true,
});

/**
 * 由 glob 合并本地静态消息（locale 键由文件名扫描得出）
 * @returns vue-i18n 静态 messages 表
 */
function buildStaticLocaleMessages(): Record<string, TaktLocaleMessages> {
  const messages: Record<string, TaktLocaleMessages> = {};

  Object.keys(localeModules)
    .sort()
    .forEach((filePath) => {
      if (filePath === './index.ts') {
        return;
      }

      const localeMatch = filePath.match(LOCALE_FILE_SUFFIX_PATTERN);
      if (!localeMatch) {
        return;
      }

      const locale = localeMatch[1];
      if (!messages[locale]) {
        messages[locale] = {};
      }

      const payload = localeModules[filePath]?.default;
      if (payload && typeof payload === 'object') {
        const segments = resolveLocaleNamespaceSegments(filePath, LOCALE_FILE_SUFFIX_PATTERN);
        const nested = nestLocalePayloadUnderNamespace(segments, payload);
        messages[locale] = deepMergeLocaleMessages(messages[locale], nested);
      }
    });

  return messages;
}

/**
 * 将后端扁平翻译键合并到 vue-i18n（供 useTranslationStore 调用；同名键覆盖静态文案）
 * @param cultureCode 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
 * @param flatMessages 扁平键值（如 entity.user.name）
 */
export function mergeDynamicLocaleMessages(
  cultureCode: string,
  flatMessages: Record<string, string>
): void {
  if (Object.keys(flatMessages).length === 0) {
    return;
  }

  const nestedMessages = buildNestedLocaleMessages(flatMessages);
  i18n.global.mergeLocaleMessage(cultureCode, nestedMessages);
}

const staticMessages = buildStaticLocaleMessages();

/**
 * 读取 localStorage 中的语言偏好（仅用于 i18n 初始 locale，勿依赖 takt-locale-sync 以免循环引用）
 * @returns 区域文化编码
 */
function readInitialI18nLocale(): string {
  const stored = localStorage.getItem(TAKT_LOCALE_STORAGE_KEY)?.trim();
  return stored || TAKT_DEFAULT_LOCALE;
}

/**
 * 创建 i18n 实例（仅含本地静态包；动态包在登录后由 TranslationStore 增量合并）
 */
const i18n = createI18n({
  legacy: false,
  locale: readInitialI18nLocale(),
  /** 禁用语言回退：当前 locale 无翻译时由 missing 返回资源键 */
  fallbackLocale: false,
  fallbackWarn: false,
  missingWarn: false,
  missing: (locale, key) => {
    const messages = (i18n.global as { messages: { value: Record<string, TaktLocaleMessageTree> } }).messages.value;
    const tree = messages[locale];
    if (tree) {
      const text = resolveLocaleMessageFromTree(tree, key);
      if (text !== undefined) {
        return text;
      }
    }
    return key;
  },
  // glob 合并结果为运行时对象，与 vue-i18n 泛型 LocaleMessage 对齐
  messages: staticMessages as never,
});

registerTaktI18n(i18n);

export default i18n;
