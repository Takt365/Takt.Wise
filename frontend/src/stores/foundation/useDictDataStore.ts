// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/stores/foundation
// 文件名称：useDictDataStore.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：字典数据 Pinia 缓存（按 dictTypeCode 分组，供 takt-dict-tag 等使用）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import { getDataDictAll } from '@/api/foundation/dictData.api';
import type { TaktDictSelectFieldNames, TaktDictSelectOption, TaktSelectOption } from '@/types/common.types';
import { useTenantStore } from '@/stores/identity/useTenantStore';
import { resolveRequestLocale } from '@/stores/foundation/useLocaleStore';
import { groupDictItemsByTypeCode } from '@/utils/taktDictGroup';
import {
  applyDictFieldDefaults as applyDictFieldDefaultsToTarget,
  resolveDictDefaultOption,
  resolveDictDefaultValue,
} from '@/utils/taktDictDefault';
import { createLogger } from '@/utils/logger';

const dictLogger = createLogger('dict-data');

/**
 * 将字典 DTO 转为下拉选项
 * @param dto 字典数据 DTO
 * @returns 下拉选项
 */
function mapDictDataToOption(dto: TaktSelectOption): TaktSelectOption {
  return {
    dictLabel: dto.dictLabel,
    dictValue: dto.dictValue,
    i18nKey: dto.i18nKey,
    dictTypeCode: dto.dictTypeCode,
    cultureCode: dto.cultureCode,
    extLabel: dto.extLabel,
    extValue: dto.extValue,
    cssClass: dto.cssClass,
    listClass: dto.listClass,
    sortOrder: dto.sortOrder,
    isDefault: dto.isDefault,
  };
}

/**
 * 解析字典缓存键（租户 + 公司 + Accept-Language；与后端 GetDataDictAll 过滤一致）
 * @returns 缓存键
 */
function resolveDictCacheKey(): string {
  const tenantStore = useTenantStore();
  const tenantCode = tenantStore.tenantCode?.trim() ?? '';
  const companyCode = tenantStore.companyCode?.trim() ?? '';
  return `${tenantCode}|${companyCode}|${resolveRequestLocale()}`;
}

/**
 * 字典数据状态管理
 */
export const useDictDataStore = defineStore('dict-data', () => {
  const dictMap = ref<Record<string, TaktSelectOption[]>>({});
  const loading = ref(false);
  const loaded = ref(false);
  /** 上次成功加载时的缓存键 */
  const loadedCacheKey = ref('');

  let loadingPromise: Promise<void> | null = null;
  /** 进行中的加载所对应的缓存键 */
  let pendingCacheKey = '';

  /**
   * 是否已加载完成
   */
  const isLoaded = computed(() => loaded.value);

  /**
   * 当前缓存是否与租户/公司/Accept-Language 一致
   */
  const isCacheFresh = computed(() => loaded.value && loadedCacheKey.value === resolveDictCacheKey());

  /**
   * 从缓存获取字典项
   * @param value 字典值或扩展标签
   * @param dictTypeCode 字典类型编码
   * @param useExtLabel 是否按 extLabel 匹配
   * @returns 匹配项或 undefined
   */
  function getDictOption(
    value: string | number,
    dictTypeCode: string,
    useExtLabel = false
  ): TaktSelectOption | undefined {
    const options = dictMap.value[dictTypeCode] ?? [];
    const valueText = String(value);

    if (useExtLabel) {
      return options.find((item) => String(item.extLabel) === valueText);
    }

    return (
      options.find((item) => String(item.dictValue) === valueText)
      ?? options.find((item) => String(item.dictLabel) === valueText)
    );
  }

  /**
   * 按区域文化过滤字典项：指定 culture 时保留 eo + 该区域；未指定时保留 eo + Accept-Language
   * @param options 字典项
   * @param cultureCode 业务区域文化（如表单 DefaultCulture）
   * @returns 过滤后的选项
   */
  function filterDictOptionsByCulture(
    options: readonly TaktSelectOption[],
    cultureCode?: string | null,
  ): TaktSelectOption[] {
    const target = (cultureCode ?? '').trim().toLowerCase();
    const uiLocale = resolveRequestLocale().trim().toLowerCase();
    const matchCulture = target || uiLocale;
    return options.filter((item) => {
      const code = (item.cultureCode ?? '').trim().toLowerCase() || 'mul';
      return code === 'mul' || code === matchCulture;
    });
  }

  /**
   * 按字典类型获取下拉选项（供 takt-select 等组件使用）
   * @param dictTypeCode 字典类型编码
   * @param fieldNames label / value 字段映射
   * @param cultureCode 可选：业务区域文化（税码等按 DefaultCulture 过滤）
   * @returns 含 label、value 的选项列表
   */
  function getDictOptionsForSelect(
    dictTypeCode: string,
    fieldNames: TaktDictSelectFieldNames,
    cultureCode?: string | null,
  ): TaktDictSelectOption[] {
    const options = filterDictOptionsByCulture(dictMap.value[dictTypeCode] ?? [], cultureCode);
    const { labelField, valueField } = fieldNames;

    return options.map((item) => {
      const label = String(item[labelField] ?? item.dictLabel ?? '');
      const rawValue = item[valueField] ?? item.dictValue ?? '';
      const value =
        typeof rawValue === 'string' || typeof rawValue === 'number' ? rawValue : String(rawValue);

      return {
        ...item,
        label,
        value,
      };
    });
  }

  /**
   * 获取字典类型下 IsDefault=1 的默认项
   * @param dictTypeCode 字典类型编码
   * @param cultureCode 可选区域文化
   * @returns 默认字典项
   */
  function getDictDefaultOption(
    dictTypeCode: string,
    cultureCode?: string | null,
  ): TaktSelectOption | undefined {
    return resolveDictDefaultOption(
      filterDictOptionsByCulture(dictMap.value[dictTypeCode] ?? [], cultureCode),
    );
  }

  /**
   * 获取字典类型下 IsDefault=1 的默认绑定值
   * @param dictTypeCode 字典类型编码
   * @param valueField 值字段（默认 dictValue）
   * @param cultureCode 可选区域文化
   * @returns 默认值
   */
  function getDictDefaultValue(
    dictTypeCode: string,
    valueField: TaktDictSelectFieldNames['valueField'] = 'dictValue',
    cultureCode?: string | null,
  ): string | number | undefined {
    return resolveDictDefaultValue(
      filterDictOptionsByCulture(dictMap.value[dictTypeCode] ?? [], cultureCode),
      { valueField },
    );
  }

  /**
   * 按字段→字典类型映射写入 IsDefault 默认值（仅空字段；调用前须已 loadAllDictDataAsync）
   * @param target 表单模型
   * @param fieldDictMap 表单字段名 → dictTypeCode
   * @param valueFieldByField 可选：字段级 valueField 覆盖
   */
  function applyDictFieldDefaults(
    target: Record<string, unknown>,
    fieldDictMap: Readonly<Record<string, string>>,
    valueFieldByField?: Readonly<Partial<Record<string, TaktDictSelectFieldNames['valueField']>>>,
  ): void {
    applyDictFieldDefaultsToTarget(
      target,
      fieldDictMap,
      (dictTypeCode, valueField) => getDictDefaultValue(dictTypeCode, valueField ?? 'dictValue'),
      valueFieldByField,
    );
  }

  /**
   * 加载全部字典数据（GET TaktDictDatas/all；含各 CultureCode，下拉再按区域/UI 语言过滤）
   * @param options.force 为 true 时忽略已加载缓存并强制刷新
   */
  async function loadAllDictDataAsync(options?: { force?: boolean }): Promise<void> {
    const cacheKey = resolveDictCacheKey();

    if (!options?.force && loaded.value && loadedCacheKey.value === cacheKey) {
      return;
    }

    if (!options?.force && loadingPromise && pendingCacheKey === cacheKey) {
      return loadingPromise;
    }

    loading.value = true;
    pendingCacheKey = cacheKey;

    loadingPromise = (async () => {
      try {
        const requestLocale = resolveRequestLocale();
        const result = await getDataDictAll();
        const items = (result?.items ?? []).map(mapDictDataToOption);
        dictMap.value = groupDictItemsByTypeCode(items, requestLocale);
        loaded.value = true;
        loadedCacheKey.value = cacheKey;
      } catch (error) {
        dictLogger.warn('加载字典数据失败', { action: 'loadAllDictData', cacheKey }, error);
        throw error;
      } finally {
        loading.value = false;
        if (pendingCacheKey === cacheKey) {
          loadingPromise = null;
          pendingCacheKey = '';
        }
      }
    })();

    return loadingPromise;
  }

  /**
   * 强制重载字典（语言/租户/公司切换后调用）
   */
  async function reloadAllDictDataAsync(): Promise<void> {
    loaded.value = false;
    loadedCacheKey.value = '';
    loadingPromise = null;
    pendingCacheKey = '';
    return loadAllDictDataAsync({ force: true });
  }

  /**
   * 重置缓存（登出等场景）
   */
  function resetDictData(): void {
    dictMap.value = {};
    loaded.value = false;
    loadedCacheKey.value = '';
    loading.value = false;
    loadingPromise = null;
    pendingCacheKey = '';
  }

  return {
    dictMap,
    loading,
    loaded,
    isLoaded,
    isCacheFresh,
    getDictOption,
    getDictOptionsForSelect,
    getDictDefaultOption,
    getDictDefaultValue,
    applyDictFieldDefaults,
    loadAllDictDataAsync,
    reloadAllDictDataAsync,
    resetDictData,
  };
});
