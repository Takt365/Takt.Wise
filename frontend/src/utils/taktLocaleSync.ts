// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：taktLocaleSync.ts
// 创建时间：2026-05-29
// 创建人：Takt365(Cursor AI)
// 功能描述：第三方组件库语言同步（Ant Design Vue / dayjs / ECharts，与 vue-i18n 对齐；运行时网关：模块级 locale 状态）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import dayjs from 'dayjs';
import 'dayjs/locale/en';
import 'dayjs/locale/zh-cn';
import 'dayjs/locale/zh-hk';
import 'dayjs/locale/ja';
import { registerLocale } from 'echarts/core';
import echartsEnUS from 'echarts/i18n/langEN-obj';
import echartsZhCN from 'echarts/i18n/langZH-obj';
import echartsZhHK from 'echarts/i18n/langZH-obj';
import echartsJaJP from 'echarts/i18n/langJA-obj';
import type { Locale as AntdLocale } from 'ant-design-vue/es/locale';
import antdZhCN from 'ant-design-vue/es/locale/zh_CN';
import antdZhHK from 'ant-design-vue/es/locale/zh_HK';
import antdEnUS from 'ant-design-vue/es/locale/en_US';
import antdJaJP from 'ant-design-vue/es/locale/ja_JP';
import {
  TAKT_DEFAULT_LOCALE,
  TAKT_LOCALE_STORAGE_KEY,
} from '@/utils/common';
import type { TaktCultureCode } from '@/types/common.types';

/** ECharts 语言包类型（与官方 lang*-obj 默认导出一致） */
type EchartsLocale = typeof echartsEnUS;

/** 已内置 Ant Design Vue / dayjs / ECharts 语言包的 CultureCode */
type TaktComponentLocalePack = 'en-US' | 'ja-JP' | 'zh-CN' | 'zh-HK';

const COMPONENT_LOCALE_PACKS: readonly TaktComponentLocalePack[] = [
  'en-US',
  'ja-JP',
  'zh-CN',
  'zh-HK',
];

/** 与 vue-i18n 同步的 ECharts registerLocale 键（模块级，由 syncTaktComponentLocales 更新） */
let currentEchartsLocale: TaktCultureCode = TAKT_DEFAULT_LOCALE;

/** 租户运行时可选 CultureCode（由 localeStore.loadCultureOptionsAsync 同步） */
let runtimeSupportedCultureCodes: readonly string[] = [];

/**
 * 同步租户可选 CultureCode（GetCultureOptions / SessionCultureOptions）
 * @param codes 租户启用语言 dictValue 列表
 */
export function setRuntimeSupportedCultureCodes(codes: readonly string[]): void {
  const unique: string[] = [];
  const seen = new Set<string>();

  for (const raw of codes) {
    const code = String(raw).trim();
    if (!code || seen.has(code)) {
      continue;
    }

    seen.add(code);
    unique.push(code);
  }

  runtimeSupportedCultureCodes = unique;
}

/**
 * 读取租户运行时可选 CultureCode
 * @returns 与 cultureOptions 一致的编码列表
 */
export function getRuntimeSupportedCultureCodes(): readonly string[] {
  return runtimeSupportedCultureCodes;
}

/**
 * 读取当前已同步的 ECharts locale 键
 * @returns 区域文化编码
 */
export function getCurrentTaktEchartsLocale(): TaktCultureCode {
  return currentEchartsLocale;
}

/** Ant Design Vue ConfigProvider locale（按内置组件语言包） */
const antdLocaleMap: Record<TaktComponentLocalePack, AntdLocale> = {
  'zh-CN': antdZhCN,
  'zh-HK': antdZhHK,
  'en-US': antdEnUS,
  'ja-JP': antdJaJP,
};

/** dayjs 全局 locale 名 */
const dayjsLocaleMap: Record<TaktComponentLocalePack, string> = {
  'zh-CN': 'zh-cn',
  'zh-HK': 'zh-hk',
  'en-US': 'en',
  'ja-JP': 'ja',
};

/** ECharts 语言包 */
const echartsLocaleMap: Record<TaktComponentLocalePack, EchartsLocale> = {
  'zh-CN': echartsZhCN,
  'zh-HK': echartsZhHK,
  'en-US': echartsEnUS,
  'ja-JP': echartsJaJP,
};

let echartsBasePacksRegistered = false;

/** 已 registerLocale 的 ECharts 键（含租户 CultureCode） */
const registeredEchartsLocaleKeys = new Set<string>();

/**
 * 读取用户已选语言（localStorage）；无有效记录时返回默认 en-US
 * @returns 区域文化编码
 */
export function readStoredLocale(): string {
  const stored = localStorage.getItem(TAKT_LOCALE_STORAGE_KEY)?.trim();
  return stored || TAKT_DEFAULT_LOCALE;
}

/**
 * 规范化为租户可选或构建期静态包中的 CultureCode
 * @param cultureCode 区域文化编码
 * @param availableCultureCodes 租户可选列表；省略时用运行时 cultureOptions
 * @returns 规范后的编码
 */
export function resolveTaktCultureCode(
  cultureCode: string,
  availableCultureCodes?: readonly string[],
): TaktCultureCode {
  const trimmed = cultureCode.trim();
  if (!trimmed) {
    return resolveTaktCultureCode(TAKT_DEFAULT_LOCALE, availableCultureCodes);
  }

  const available =
    availableCultureCodes && availableCultureCodes.length > 0
      ? availableCultureCodes
      : runtimeSupportedCultureCodes;

  if (available.length > 0) {
    const inList = findCultureCodeInAvailableOrder(trimmed, available);
    if (inList) {
      return inList;
    }

    const defaultCode = pickDefaultCultureCode(available);
    if (defaultCode) {
      return defaultCode;
    }
  }

  return trimmed;
}

/**
 * 租户可选 CultureCode 集合与顺序（顺序与 GetCultureOptions / 会话文化列表一致）
 */
interface AvailableCultureIndex {
  /** 去重集合 */
  set: Set<string>;
  /** 保留 API 原始顺序与大小写 */
  order: string[];
}

/**
 * 构建租户可选 CultureCode 索引
 * @param availableCultureCodes 租户会话可选 CultureCode 列表（来自 cultureOptions / GetCultureOptionsAsync）
 */
function buildAvailableCultureIndex(availableCultureCodes: readonly string[]): AvailableCultureIndex {
  const order: string[] = [];
  const seen = new Set<string>();

  for (const raw of availableCultureCodes) {
    const code = String(raw).trim();
    if (!code || seen.has(code)) {
      continue;
    }

    seen.add(code);
    order.push(code);
  }

  return { set: seen, order };
}

/**
 * 在租户列表中按大小写不敏感查找 CultureCode（返回 API 原始编码）
 * @param code 浏览器或 BCP47 标签
 * @param availableOrder 租户可选列表（有序）
 */
function findCultureCodeInAvailableOrder(
  code: string,
  availableOrder: readonly string[],
): string | null {
  const lower = code.trim().toLowerCase();
  if (!lower) {
    return null;
  }

  for (const item of availableOrder) {
    if (item.toLowerCase() === lower) {
      return item;
    }
  }

  return null;
}

/**
 * 在租户列表中按谓词查找首个 CultureCode
 * @param availableOrder 租户可选列表（有序）
 * @param predicate 匹配条件
 */
function findCultureCodeInAvailableByPredicate(
  availableOrder: readonly string[],
  predicate: (code: string) => boolean,
): string | null {
  for (const item of availableOrder) {
    if (predicate(item)) {
      return item;
    }
  }

  return null;
}

/**
 * 判断租户 CultureCode 是否表示繁体中文（非简体 zh-CN / zh-SG）
 * @param code CultureCode
 */
function isTraditionalChineseCultureCode(code: string): boolean {
  const lower = code.trim().toLowerCase();
  if (!lower.startsWith('zh')) {
    return false;
  }

  if (lower === 'zh-cn' || lower === 'zh-sg') {
    return false;
  }

  if (lower.endsWith('-cn') || lower.endsWith('-sg')) {
    return false;
  }

  if (lower.includes('hans')) {
    return false;
  }

  return true;
}

/**
 * 将租户 CultureCode 映射到内置第三方组件语言包
 * @param cultureCode 区域文化编码
 */
function resolveComponentLocalePack(cultureCode: string): TaktComponentLocalePack {
  const lower = cultureCode.trim().toLowerCase();

  if (lower === 'en-us' || lower.startsWith('en-')) {
    return 'en-US';
  }

  if (lower.startsWith('ja')) {
    return 'ja-JP';
  }

  if (lower === 'zh-cn' || lower === 'zh-sg' || lower.includes('hans')) {
    return 'zh-CN';
  }

  if (lower === 'zh-hk' || lower === 'zh-mo' || lower.includes('hk') || lower.includes('mo')) {
    return 'zh-HK';
  }

  if (isTraditionalChineseCultureCode(cultureCode)) {
    return 'zh-HK';
  }

  if (lower.startsWith('zh')) {
    return 'zh-CN';
  }

  return 'en-US';
}

/**
 * 在租户列表中选取默认语言（优先系统默认 en-US，否则首项）
 * @param availableOrder 租户可选列表（有序）
 */
function pickDefaultCultureCode(availableOrder: readonly string[]): string | null {
  if (availableOrder.length === 0) {
    return null;
  }

  const systemDefault = findCultureCodeInAvailableOrder(TAKT_DEFAULT_LOCALE, availableOrder);
  if (systemDefault) {
    return systemDefault;
  }

  return availableOrder[0];
}

/**
 * 从租户繁体项中按浏览器语义排序（Chrome「中文(繁体)」优先 TW）
 * @param codes 租户列表中的繁体 CultureCode
 * @param preferTwFirst 是否优先含 tw 的编码
 */
function orderTraditionalCultureCodes(codes: readonly string[], preferTwFirst: boolean): string[] {
  const rank = (cultureCode: string): number => {
    const lower = cultureCode.toLowerCase();
    if (lower.includes('tw')) {
      return preferTwFirst ? 0 : 1;
    }

    if (lower.includes('hk') || lower.includes('mo')) {
      return preferTwFirst ? 1 : 0;
    }

    return 2;
  };

  return [...codes].sort((a, b) => rank(a) - rank(b));
}

/**
 * 在租户可选列表中选取繁体中文（泛化 zh-Hant / zh-CHT 时使用）
 * @param availableOrder 租户可选列表（有序）
 * @param preferTwFirst 是否优先含 tw 的编码
 */
function pickTraditionalCultureFromAvailable(
  availableOrder: readonly string[],
  preferTwFirst: boolean,
): string | null {
  const traditional = availableOrder.filter((code) => isTraditionalChineseCultureCode(code));
  if (traditional.length === 0) {
    return null;
  }

  return orderTraditionalCultureCodes(traditional, preferTwFirst)[0] ?? null;
}

/**
 * 将浏览器 BCP47 标签匹配到租户可选 CultureCode（唯一权威：租户语言列表）
 * @param tag navigator.language / navigator.languages 项
 * @param availableOrder 租户可选列表（有序）
 */
function matchBrowserTagToAvailableCulture(
  tag: string,
  availableOrder: readonly string[],
): string | null {
  const trimmed = tag.trim();
  if (!trimmed || availableOrder.length === 0) {
    return null;
  }

  const exact = findCultureCodeInAvailableOrder(trimmed, availableOrder);
  if (exact) {
    return exact;
  }

  const parts = trimmed.toLowerCase().replace(/_/g, '-').split('-');
  const lang = parts[0];

  if (lang === 'zh') {
    const region = parts.find((part) => ['hk', 'mo', 'tw', 'cn', 'sg'].includes(part));
    const script = parts.find((part) => part === 'hant' || part === 'hans');

    if (region === 'hk' || region === 'mo') {
      return findCultureCodeInAvailableByPredicate(
        availableOrder,
        (code) => /hk|mo/i.test(code),
      );
    }

    if (region === 'tw') {
      return findCultureCodeInAvailableByPredicate(availableOrder, (code) => /tw/i.test(code));
    }

    if (region === 'cn' || region === 'sg' || script === 'hans') {
      return findCultureCodeInAvailableByPredicate(availableOrder, (code) => {
        const lower = code.toLowerCase();
        return lower === 'zh-cn' || lower === 'zh-sg' || /-(cn|sg)$/i.test(lower);
      });
    }

    if (script === 'hant' || parts.includes('cht')) {
      return pickTraditionalCultureFromAvailable(availableOrder, true);
    }

    return null;
  }

  if (lang === 'ja') {
    return findCultureCodeInAvailableByPredicate(availableOrder, (code) =>
      code.toLowerCase().startsWith('ja'),
    );
  }

  if (lang === 'en') {
    return findCultureCodeInAvailableByPredicate(availableOrder, (code) =>
      code.toLowerCase().startsWith('en'),
    );
  }

  return null;
}

/**
 * 读取浏览器 navigator 原始语言标签（按用户偏好顺序）
 * @returns BCP47 语言标签列表
 */
function readBrowserLanguageTags(): string[] {
  if (typeof navigator === 'undefined') {
    return [];
  }

  const rawTags =
    navigator.languages?.length > 0 ? navigator.languages : [navigator.language];

  return rawTags.map((tag) => String(tag ?? '').trim()).filter((tag) => tag.length > 0);
}

/**
 * 按浏览器语言偏好顺序，在租户可选列表中匹配首个 CultureCode
 * @param availableCultureCodes 租户会话可选 CultureCode 列表
 * @returns 匹配到的编码；无匹配时返回 null
 */
export function findBrowserCultureInAvailableList(
  availableCultureCodes: readonly string[],
): TaktCultureCode | null {
  const { order } = buildAvailableCultureIndex(availableCultureCodes);

  if (order.length === 0) {
    return null;
  }

  for (const tag of readBrowserLanguageTags()) {
    const matched = matchBrowserTagToAvailableCulture(tag, order);
    if (matched) {
      return matched;
    }
  }

  return null;
}

/**
 * 从浏览器 navigator 读取候选 CultureCode（须在租户可选列表内）
 * @param availableCultureCodes 租户会话可选 CultureCode 列表
 * @returns 已规范为 TaktCultureCode 的候选列表
 */
export function readBrowserCultureCandidates(
  availableCultureCodes: readonly string[],
): TaktCultureCode[] {
  const { order } = buildAvailableCultureIndex(availableCultureCodes);
  const seen = new Set<string>();
  const result: TaktCultureCode[] = [];

  if (order.length === 0) {
    return result;
  }

  for (const tag of readBrowserLanguageTags()) {
    const matched = matchBrowserTagToAvailableCulture(tag, order);
    if (!matched || seen.has(matched)) {
      continue;
    }

    seen.add(matched);
    result.push(matched);
  }

  return result;
}

/**
 * 登录预览语言：浏览器与用户默认不一致时优先浏览器（须在租户可选列表内）
 * @param userDefaultCulture 用户 DefaultCulture
 * @param availableCultureCodes 租户会话可选 CultureCode 列表
 * @returns 应应用的区域文化编码
 */
export function resolveLoginPreviewLocale(
  userDefaultCulture: string,
  availableCultureCodes: readonly string[],
): TaktCultureCode {
  const { set: available, order } = buildAvailableCultureIndex(availableCultureCodes);

  if (available.size === 0) {
    return resolveTaktCultureCode(userDefaultCulture.trim());
  }

  const userDefault =
    findCultureCodeInAvailableOrder(userDefaultCulture.trim(), order) ??
    pickDefaultCultureCode(order) ??
    TAKT_DEFAULT_LOCALE;

  const browserInList = findBrowserCultureInAvailableList(availableCultureCodes);

  if (browserInList && browserInList !== userDefault) {
    return browserInList;
  }

  if (available.has(userDefault)) {
    return userDefault;
  }

  return browserInList ?? userDefault;
}

/**
 * 注册内置 ECharts 语言包（仅执行一次）
 */
function registerEchartsBasePacksOnce(): void {
  if (echartsBasePacksRegistered) {
    return;
  }

  for (const pack of COMPONENT_LOCALE_PACKS) {
    registerLocale(pack, echartsLocaleMap[pack]);
    registeredEchartsLocaleKeys.add(pack);
  }

  echartsBasePacksRegistered = true;
}

/**
 * 按租户 CultureCode 注册 ECharts 语言包（键与 vue-i18n locale 一致）
 * @param cultureCode 区域文化编码
 */
function ensureEchartsLocaleRegistered(cultureCode: string): void {
  if (registeredEchartsLocaleKeys.has(cultureCode)) {
    return;
  }

  const pack = resolveComponentLocalePack(cultureCode);
  registerLocale(cultureCode, echartsLocaleMap[pack]);
  registeredEchartsLocaleKeys.add(cultureCode);
}

/**
 * 获取 Ant Design Vue ConfigProvider 语言包
 * @param cultureCode 区域文化编码
 * @returns Ant Design Vue locale
 */
export function getAntDesignVueLocale(cultureCode: string): AntdLocale {
  return antdLocaleMap[resolveComponentLocalePack(cultureCode)];
}

/**
 * 获取 ECharts 语言包
 * @param cultureCode 区域文化编码
 * @returns ECharts LocaleOption
 */
export function getEchartsLocale(cultureCode: string): EchartsLocale {
  const normalized = resolveTaktCultureCode(cultureCode);
  ensureEchartsLocaleRegistered(normalized);
  return echartsLocaleMap[resolveComponentLocalePack(normalized)];
}

/**
 * 获取 ECharts init 使用的 registerLocale 键（与 TaktCultureCode 一致）
 * @param cultureCode 区域文化编码
 * @returns 区域文化编码
 */
export function getEchartsLocaleCode(cultureCode: string): TaktCultureCode {
  return resolveTaktCultureCode(cultureCode);
}

/**
 * 获取 ECharts init 第三参数（与当前语言同步）
 * @param cultureCode 区域文化编码，默认取 getCurrentTaktEchartsLocale()
 * @returns ECharts init 选项片段
 */
export function getEchartsInitLocaleOption(cultureCode?: string): { locale: TaktCultureCode } {
  const code = cultureCode ? getEchartsLocaleCode(cultureCode) : currentEchartsLocale;

  return { locale: code };
}

/**
 * 同步第三方组件库语言（dayjs 全局 + ECharts 注册表）
 * @param cultureCode 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
 */
export function syncTaktComponentLocales(cultureCode: string): void {
  const normalized = resolveTaktCultureCode(cultureCode);
  const pack = resolveComponentLocalePack(normalized);

  dayjs.locale(dayjsLocaleMap[pack]);
  registerEchartsBasePacksOnce();
  ensureEchartsLocaleRegistered(normalized);
  currentEchartsLocale = normalized;
}
