// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：taktLocaleMerge.ts
// 创建时间：2026-05-27
// 创建人：Takt365(Cursor AI)
// 功能描述：locales 静态语言包按目录路径嵌套合并（文件内顶级节点为 page，引用为 业务目录.page.*）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktLocaleMessageTree } from '@/utils/taktI18nMessage';

export type { TaktLocaleMessageTree };

/** locale messages 树合并最大深度（07-overflow-vue） */
const TAKT_MAX_LOCALE_MERGE_DEPTH = 32;

/**
 * 从 glob 文件路径解析命名空间段（如 ./dashboard/workspace/zh-CN.ts → dashboard.workspace）
 * @param filePath import.meta.glob 相对路径
 * @param localeFilePattern 语言文件名正则（用于剥离 zh-CN.ts 等）
 */
export function resolveLocaleNamespaceSegments(
  filePath: string,
  localeFilePattern: RegExp
): string[] {
  const relative = filePath.replace(localeFilePattern, '').replace(/^\.\//, '').replace(/\/$/, '');

  return relative.split('/').filter(Boolean);
}

/**
 * 将语言包载荷嵌套到命名空间路径下
 * @param segments 目录段（dashboard、workspace）
 * @param payload 文件 default 导出对象
 */
export function nestLocalePayloadUnderNamespace(
  segments: string[],
  payload: TaktLocaleMessageTree
): TaktLocaleMessageTree {
  if (segments.length === 0) {
    return payload;
  }

  return segments.reduceRight<TaktLocaleMessageTree>(
    (acc, segment) => ({ [segment]: acc }),
    payload
  );
}

/**
 * 深度合并 vue-i18n messages（后者覆盖同名叶节点）
 * @param target 目标树
 * @param source 待合并树
 */
export function deepMergeLocaleMessages(
  target: TaktLocaleMessageTree,
  source: TaktLocaleMessageTree,
  depth = 0
): TaktLocaleMessageTree {
  if (depth > TAKT_MAX_LOCALE_MERGE_DEPTH) {
    return target;
  }
  const output: TaktLocaleMessageTree = { ...target };

  Object.entries(source).forEach(([key, value]) => {
    const existing = output[key];

    if (
      value !== null &&
      typeof value === 'object' &&
      !Array.isArray(value) &&
      existing !== null &&
      typeof existing === 'object' &&
      !Array.isArray(existing)
    ) {
      output[key] = deepMergeLocaleMessages(
        existing as TaktLocaleMessageTree,
        value as TaktLocaleMessageTree,
        depth + 1
      );
      return;
    }

    output[key] = value;
  });

  return output;
}
