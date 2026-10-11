// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/utils/taktDictDefault
// 文件名称：taktDictDefault.ts
// 功能描述：字典 IsDefault 默认项解析（与 TaktDictData.is_default / sys_yes_no 对齐）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktDictSelectFieldNames, TaktSelectOption } from '@/types/common.types';

/**
 * 是否字典默认项（IsDefault=1）
 * @param option 字典选项
 * @returns 是否为默认项
 */
export function resolveDictDataIsDefault(option: Pick<TaktSelectOption, 'isDefault'>): boolean {
  return option.isDefault === 1;
}

/**
 * 表单字段是否尚未赋值（undefined / null / 空串）
 * @param value 字段当前值
 * @returns 是否为空
 */
export function isEmptyFormFieldValue(value: unknown): boolean {
  return value === undefined || value === null || value === '';
}

/**
 * 从字典项列表解析 IsDefault=1 的默认项（多项时取 sortOrder 最小）
 * @param options 字典项列表
 * @returns 默认项或 undefined
 */
export function resolveDictDefaultOption(
  options: readonly TaktSelectOption[],
): TaktSelectOption | undefined {
  if (!options?.length) {
    return undefined;
  }

  const marked = options.filter(resolveDictDataIsDefault);
  if (marked.length === 0) {
    return undefined;
  }

  if (marked.length === 1) {
    return marked[0];
  }

  return [...marked].sort((a, b) => {
    const orderDiff = (a.sortOrder ?? 0) - (b.sortOrder ?? 0);
    if (orderDiff !== 0) {
      return orderDiff;
    }
    return String(a.dictValue ?? '').localeCompare(String(b.dictValue ?? ''));
  })[0];
}

/**
 * 解析字典默认绑定值（按 valueField 取 DictValue / ExtLabel 等）
 * @param options 字典项列表
 * @param fieldNames 值字段映射
 * @returns 默认值；无默认项时 undefined
 */
export function resolveDictDefaultValue(
  options: readonly TaktSelectOption[],
  fieldNames?: Pick<TaktDictSelectFieldNames, 'valueField'>,
): string | number | undefined {
  const option = resolveDictDefaultOption(options);
  if (!option) {
    return undefined;
  }

  const valueField = fieldNames?.valueField ?? 'dictValue';
  const raw = option[valueField] ?? option.dictValue;
  if (raw === undefined || raw === null || raw === '') {
    return undefined;
  }

  return raw;
}

/**
 * 按字段→字典类型映射写入 IsDefault 默认值（仅空字段）
 * @param target 表单模型
 * @param fieldDictMap 表单字段名 → dictTypeCode
 * @param resolveDefault 按 dictTypeCode 解析默认值
 * @param valueFieldByField 可选：字段级 valueField 覆盖
 */
export function applyDictFieldDefaults(
  target: Record<string, unknown>,
  fieldDictMap: Readonly<Record<string, string>>,
  resolveDefault: (dictTypeCode: string, valueField?: TaktDictSelectFieldNames['valueField']) => string | number | undefined,
  valueFieldByField?: Readonly<Partial<Record<string, TaktDictSelectFieldNames['valueField']>>>,
): void {
  for (const [field, dictTypeCode] of Object.entries(fieldDictMap)) {
    if (!isEmptyFormFieldValue(target[field])) {
      continue;
    }

    const valueField = valueFieldByField?.[field];
    const defaultValue = resolveDefault(dictTypeCode, valueField);
    if (defaultValue !== undefined) {
      target[field] = defaultValue;
    }
  }
}
