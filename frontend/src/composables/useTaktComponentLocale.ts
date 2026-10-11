// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/composables
// 文件名称：useTaktComponentLocale.ts
// 创建时间：2026-05-29
// 创建人：Takt365(Cursor AI)
// 功能描述：组件库语言 Composable（Ant Design Vue / ECharts 随 vue-i18n locale 同步）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { computed } from 'vue';
import { useI18n } from 'vue-i18n';
import { getAntDesignVueLocale, getEchartsLocaleCode } from '@/utils/taktLocaleSync';

/**
 * 组件库语言（随 vue-i18n locale 变化，供 ConfigProvider / ECharts 使用）
 * @returns {object} antDesignVueLocale、echartsLocale 响应式引用
 */
export function useTaktComponentLocale() {
  /** vue-i18n 实例（含当前 locale） */
  const { locale } = useI18n();

  /** Ant Design Vue ConfigProvider 语言包 */
  const antDesignVueLocale = computed(() => {
    /** 当前语言编码字符串 */
    const localeCode = String(locale.value);
    // 映射为 Ant Design Vue locale 对象
    return getAntDesignVueLocale(localeCode);
  });

  /** ECharts registerLocale 键（响应式，与 vue-i18n locale 同步） */
  const echartsLocale = computed(() => {
    /** 当前语言编码字符串 */
    const localeCode = String(locale.value);
    // 映射为 ECharts 注册用语言键
    return getEchartsLocaleCode(localeCode);
  });

  return {
    antDesignVueLocale,
    echartsLocale,
  };
}
