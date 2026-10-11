// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/utils/taktDictGroup
// 文件名称：taktDictGroup.ts
// 功能描述：字典扁平列表按 dictTypeCode 分组；同 DictValue 去重（Accept-Language 区域项优先于 mul）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktSelectOption } from '@/types/common.types';
import { resolveDictDataIsDefault } from '@/utils/taktDictDefault';

/** 多种语言内容 CultureCode（IETF BCP 47 / ISO 639-3 mul） */
const GLOBAL_CULTURE_CODE = 'mul';

/**
 * 计算 CultureCode 去重优先级（越高越优先保留）
 * @param cultureCode 字典项区域编码
 * @param requestLocale 当前 Accept-Language
 * @returns 优先级
 */
function resolveCultureDedupePriority(cultureCode: string, requestLocale: string): number {
  if (cultureCode === requestLocale) {
    return 2;
  }
  if (cultureCode === GLOBAL_CULTURE_CODE) {
    return 0;
  }
  return 1;
}

/**
 * 规范化字典项 CultureCode（空串视为 mul，兼容历史数据）
 * @param cultureCode 原始区域编码
 * @returns 规范化编码
 */
function normalizeDictCultureCode(cultureCode: string | null | undefined): string {
  const trimmed = cultureCode?.trim();
  return trimmed ? trimmed : GLOBAL_CULTURE_CODE;
}

/**
 * 将 GetDataDictAll 扁平列表分组为 dictTypeCode → 选项列表
 * 同 dictTypeCode + dictValue 多条时保留 Accept-Language 匹配项，其次其它区域项，最后 eo
 * @param items 后端返回的字典项
 * @param requestLocale 当前 Accept-Language（与 resolveRequestLocale 一致）
 * @returns 按 dictTypeCode 分组的选项（组内按 sortOrder 升序）
 */
export function groupDictItemsByTypeCode(
  items: readonly TaktSelectOption[],
  requestLocale: string,
): Record<string, TaktSelectOption[]> {
  if (!items?.length) {
    return {};
  }

  const locale = requestLocale.trim() || 'zh-CN';
  const grouped: Record<string, TaktSelectOption[]> = {};
  const dedupeIndex = new Map<string, { typeCode: string; index: number; priority: number }>();

  for (const item of items) {
    const typeCode = item.dictTypeCode?.trim();
    if (!typeCode) {
      continue;
    }

    const cultureCode = normalizeDictCultureCode(item.cultureCode);
    const option: TaktSelectOption = {
      ...item,
      cultureCode,
    };
    const valueKey = String(option.dictValue ?? '');
    const dedupeKey = `${typeCode}\0${valueKey}`;
    const priority = resolveCultureDedupePriority(cultureCode, locale);

    if (!grouped[typeCode]) {
      grouped[typeCode] = [];
    }

    const existing = dedupeIndex.get(dedupeKey);
    if (!existing) {
      dedupeIndex.set(dedupeKey, {
        typeCode,
        index: grouped[typeCode].length,
        priority,
      });
      grouped[typeCode].push(option);
      continue;
    }

    if (priority < existing.priority) {
      continue;
    }
    if (priority === existing.priority) {
      const existingOption = grouped[typeCode][existing.index];
      if (resolveDictDataIsDefault(existingOption) && !resolveDictDataIsDefault(option)) {
        continue;
      }
      if (!resolveDictDataIsDefault(existingOption) && resolveDictDataIsDefault(option)) {
        grouped[typeCode][existing.index] = option;
      }
      continue;
    }

    grouped[typeCode][existing.index] = option;
    dedupeIndex.set(dedupeKey, { typeCode, index: existing.index, priority });
  }

  for (const typeCode of Object.keys(grouped)) {
    grouped[typeCode].sort((a, b) => {
      const orderDiff = (a.sortOrder ?? 0) - (b.sortOrder ?? 0);
      if (orderDiff !== 0) {
        return orderDiff;
      }
      return String(a.dictValue ?? '').localeCompare(String(b.dictValue ?? ''));
    });
  }

  return grouped;
}
