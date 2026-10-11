// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：theme.ts
// 创建时间：2026-05-29
// 创建人：Takt365(Cursor AI)
// 功能描述：主题模式 DOM、主题色预设、Ant Design CSS 变量同步
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { GlobalToken } from 'ant-design-vue/es/theme/interface';
import type { AppSetting, ThemeColorConfig } from '@/types/setting.types';
import { TAKT_THEME_COLOR_STORAGE_KEY, TAKT_THEME_STORAGE_KEY } from '@/utils/common';

export { TAKT_THEME_COLOR_STORAGE_KEY, TAKT_THEME_STORAGE_KEY };

/** 主题模式：浅色 / 深色 / 跟随系统 */
export type TaktThemeMode = 'light' | 'dark' | 'system';

/** 实际生效的主题 */
export type TaktResolvedTheme = 'light' | 'dark';

/** 主题色预设键名（与 color-base.css 著名色彩 --takt-* 一一对应） */
export type TaktThemeColorPreset =
  | 'mars-green'
  | 'tiffany-blue'
  | 'chinese-red'
  | 'titian-red'
  | 'burgundy-red'
  | 'bordeaux-red'
  | 'klein-blue'
  | 'vandyke-brown'
  | 'prussian-blue'
  | 'sennelier-yellow'
  | 'memorial-gray';

/**
 * 预设顺序（与 color-base.css「著名色彩」声明顺序一致，勿改顺序）
 * @see frontend/src/styles/color-base.css
 */
export const themeColorPresetKeys: readonly TaktThemeColorPreset[] = [
  'mars-green',
  'tiffany-blue',
  'chinese-red',
  'titian-red',
  'burgundy-red',
  'bordeaux-red',
  'klein-blue',
  'vandyke-brown',
  'prussian-blue',
  'sennelier-yellow',
  'memorial-gray',
] as const;

/** 预设主题色映射（色值与 color-base.css --takt-* 相同） */
export const themeColorMap: Record<TaktThemeColorPreset, string> = {
  'mars-green': '#008C8C',// 马尔斯绿
  'tiffany-blue': '#81D8D0',// 蒂芙尼蓝 #A5D3D4
  'chinese-red': '#DE2910',// 中国红 #E60000
  'titian-red': '#B05923',// 提香红#BD5620
  'burgundy-red': '#800020',// 勃艮第红
  'bordeaux-red': '#7B1B38',// 波尔多红
  'klein-blue': '#002fa7',// 克莱因蓝
  'vandyke-brown': '#492D22',// 凡戴克棕 #664228
  'prussian-blue': '#003153',// 普鲁士蓝
  'sennelier-yellow': '#FBD26A',// 申布伦黄
  'memorial-gray': '#808080',// 纪念灰
};

/** color-base.css「著名色彩」CSS 变量（与 themeColorPresetKeys 顺序一致） */
export const themeColorCssVars: readonly string[] = themeColorPresetKeys.map(
  (preset) => `var(--takt-${preset})`,
);

/**
 * 按序号取著名色 CSS 变量（循环）
 * @param index 序号（0 起）
 * @returns var(--takt-*) 字符串
 */
export function resolveThemeColorCssVar(index: number): string {
  if (!Number.isFinite(index)) {
    return themeColorCssVars[0] as string;
  }
  return themeColorCssVars[Math.abs(Math.trunc(index)) % themeColorCssVars.length] as string;
}

/**
 * 主题色预设（存储/CSS 用连字符 slug）→ common.page.color.* 翻译键后缀（仅小写点分段，禁止连字符）
 */
export const themeColorPresetI18nKeyMap: Record<TaktThemeColorPreset, string> = {
  'mars-green': 'mars.green',
  'tiffany-blue': 'tiffany.blue',
  'chinese-red': 'chinese.red',
  'titian-red': 'titian.red',
  'burgundy-red': 'burgundy.red',
  'bordeaux-red': 'bordeaux.red',
  'klein-blue': 'klein.blue',
  'vandyke-brown': 'vandyke.brown',
  'prussian-blue': 'prussian.blue',
  'sennelier-yellow': 'sennelier.yellow',
  'memorial-gray': 'memorial.gray',
};

/** 默认预设（列表首项） */
export const defaultThemeColorPreset: TaktThemeColorPreset = themeColorPresetKeys[0];

/** 系统默认主题色（与 defaultSetting.themeColor 一致） */
export const systemDefaultThemeColorConfig: ThemeColorConfig = { type: 'blue' };

/** 系统默认明暗（与 defaultSetting.theme 一致） */
export const systemDefaultThemeMode: TaktResolvedTheme = 'dark';

/** 假日主题 DTO 中影响主题色解析的字段 */
export interface TaktHolidayThemeHint {
  isHolidayToday?: boolean;
  holidayTheme?: string | null;
}

/**
 * AppSetting.themeColor.type 短键 → TaktThemeColorPreset（与 types/setting.types ThemeColor 一致）
 */
export const appSettingThemeColorToPreset = {
  green: 'mars-green',
  cyan: 'tiffany-blue',
  red: 'chinese-red',
  orange: 'titian-red',
  purple: 'burgundy-red',
  pink: 'bordeaux-red',
  blue: 'klein-blue',
  brown: 'vandyke-brown',
  indigo: 'prussian-blue',
  yellow: 'sennelier-yellow',
  gray: 'memorial-gray',
} as const satisfies Record<string, TaktThemeColorPreset>;

/** AppSetting 主题色短键（不含 custom） */
export type AppSettingThemeColorKey = keyof typeof appSettingThemeColorToPreset;

/**
 * AppSetting.themeColor.type 短键 → TaktThemeColorPreset（custom 回退 klein-blue）
 * @param type 主题色类型
 * @returns 主题色预设
 */
export function mapAppSettingThemeColorToPreset(type: ThemeColorConfig['type']): TaktThemeColorPreset {
  if (type === 'custom') {
    return appSettingThemeColorToPreset.blue;
  }
  return appSettingThemeColorToPreset[type];
}

/** TaktThemeColorPreset → AppSetting.themeColor.type 短键 */
export const presetToAppSettingThemeColor: Record<TaktThemeColorPreset, AppSettingThemeColorKey> =
  Object.fromEntries(
    Object.entries(appSettingThemeColorToPreset).map(([shortKey, preset]) => [preset, shortKey])
  ) as Record<TaktThemeColorPreset, AppSettingThemeColorKey>;

/** app-setting localStorage 键名（与 @/setting STORAGE_KEY 一致，此处只读避免循环依赖） */
const APP_SETTING_STORAGE_KEY = 'app-setting';

/**
 * 从 app-setting 读取主题模式（light / dark）
 * @returns 模式或 null
 */
export function readAppSettingThemeMode(): 'light' | 'dark' | null {
  if (typeof window === 'undefined' || !localStorage) {
    return null;
  }
  try {
    const raw = localStorage.getItem(APP_SETTING_STORAGE_KEY);
    if (!raw) {
      return null;
    }
    const parsed = JSON.parse(raw) as { theme?: unknown };
    if (parsed.theme === 'light' || parsed.theme === 'dark') {
      return parsed.theme;
    }
  } catch {
    return null;
  }
  return null;
}

/**
 * 从 app-setting 读取主题色预设
 * @returns 预设或 null
 */
export function readAppSettingThemeColorPreset(): TaktThemeColorPreset | null {
  if (typeof window === 'undefined' || !localStorage) {
    return null;
  }
  try {
    const raw = localStorage.getItem(APP_SETTING_STORAGE_KEY);
    if (!raw) {
      return null;
    }
    const parsed = JSON.parse(raw) as { themeColor?: { type?: unknown } };
    const type = parsed.themeColor?.type;
    if (typeof type !== 'string' || type === 'custom') {
      return null;
    }
    const preset = appSettingThemeColorToPreset[type as AppSettingThemeColorKey];
    return preset ?? null;
  } catch {
    return null;
  }
}

/**
 * 从 app-setting 读取用户是否手动改过外观
 * @returns 是否用户锁定
 */
export function readAppSettingAppearanceUserOverride(): boolean {
  if (typeof window === 'undefined' || !localStorage) {
    return false;
  }
  try {
    const raw = localStorage.getItem(APP_SETTING_STORAGE_KEY);
    if (!raw) {
      return false;
    }
    const parsed = JSON.parse(raw) as { appearanceUserOverride?: unknown };
    return parsed.appearanceUserOverride === true;
  } catch {
    return false;
  }
}

/**
 * 将 camelCase 转为 kebab-case
 * @param key 原始键名
 * @returns kebab-case 键名
 */
function camelToKebab(key: string): string {
  return key.replace(/([A-Z])/g, '-$1').toLowerCase();
}

/**
 * 读取已持久化的主题模式
 * @returns 主题模式
 */
export function readStoredThemeMode(): TaktThemeMode {
  const stored = localStorage.getItem(TAKT_THEME_STORAGE_KEY);
  if (stored === 'light' || stored === 'dark' || stored === 'system') {
    return stored;
  }
  const fromAppSetting = readAppSettingThemeMode();
  if (fromAppSetting) {
    return fromAppSetting;
  }
  return systemDefaultThemeMode;
}

/**
 * 根据模式与系统偏好解析实际主题
 * @param mode 主题模式
 * @returns 实际生效的主题
 */
export function resolveThemeMode(mode: TaktThemeMode): TaktResolvedTheme {
  if (mode === 'system') {
    return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
  }
  return mode;
}

/**
 * 将实际主题写入 html 根节点
 * @param resolved 实际生效的主题
 */
export function applyThemeDom(resolved: TaktResolvedTheme): void {
  document.documentElement.dataset.theme = resolved;
  document.documentElement.style.colorScheme = resolved;
}

/**
 * 应用启动前初始化主题 DOM，避免首屏闪烁
 * 未手动选外观：系统默认 dark；已选手动：TAKT_THEME / app-setting
 */
export function initTaktThemeDom(): void {
  if (!readAppSettingAppearanceUserOverride()) {
    applyThemeDom(systemDefaultThemeMode);
    return;
  }
  applyThemeDom(resolveThemeMode(readStoredThemeMode()));
}

/** 旧版 preset / 后端假日主题键 → 现行 preset（著名色统一 *-red 后缀） */
const legacyThemeColorPresetMap: Record<string, TaktThemeColorPreset> = {
  burgundy: 'burgundy-red',
  bordeaux: 'bordeaux-red',
  'van-dyke-brown': 'vandyke-brown',
  'senelier-yellow': 'sennelier-yellow',
};

/**
 * 解析主题色预设键（含 localStorage / 后端假日主题旧键兼容）
 * @param stored 原始键名
 * @returns 现行 preset，无法识别时返回 null
 */
export function resolveThemeColorPreset(stored: string | null | undefined): TaktThemeColorPreset | null {
  const key = stored?.trim();
  if (!key) {
    return null;
  }
  if (key in themeColorMap) {
    return key as TaktThemeColorPreset;
  }
  return legacyThemeColorPresetMap[key] ?? null;
}

/**
 * 读取已持久化的主题色预设
 * @returns 主题色预设
 */
export function readStoredThemeColorPreset(): TaktThemeColorPreset {
  const fromAppSetting = readAppSettingThemeColorPreset();
  if (fromAppSetting) {
    return fromAppSetting;
  }
  const fromStorage = resolveThemeColorPreset(localStorage.getItem(TAKT_THEME_COLOR_STORAGE_KEY));
  if (fromStorage) {
    return fromStorage;
  }
  return mapAppSettingThemeColorToPreset(systemDefaultThemeColorConfig.type);
}

/**
 * 获取预设对应的色值
 * @param preset 预设键名
 * @returns 十六进制色值
 */
export function getThemeColorValue(preset: TaktThemeColorPreset): string {
  return themeColorMap[preset];
}

/**
 * 将 AppSetting.themeColor 解析为 hex（不含假日优先级）
 * @param config 主题色配置
 * @returns 十六进制色值
 */
export function resolveThemeColorConfigHex(config: ThemeColorConfig): string {
  if (config.type === 'custom' && config.customColor) {
    return config.customColor;
  }
  return getThemeColorValue(mapAppSettingThemeColorToPreset(config.type));
}

/**
 * 解析生效主题色
 * 叠加顺序（由低到高）：系统默认 → 假日适配 → 用户自定义；解析时自顶向下判断
 * @param setting 当前 AppSetting
 * @param holiday 假日主题 DTO 片段
 * @param systemDefaultColor 系统默认主题色配置
 * @returns 十六进制主色
 */
export function resolveEffectiveColorPrimary(
  setting: AppSetting,
  holiday: TaktHolidayThemeHint | null | undefined,
  systemDefaultColor: ThemeColorConfig = systemDefaultThemeColorConfig,
): string {
  if (setting.appearanceUserOverride) {
    if (setting.themeColor.type === 'custom' && setting.themeColor.customColor) {
      return setting.themeColor.customColor;
    }
    return resolveThemeColorConfigHex(setting.themeColor);
  }
  if (holiday?.isHolidayToday) {
    const preset = resolveThemeColorPreset(holiday.holidayTheme);
    if (preset) {
      return getThemeColorValue(preset);
    }
  }
  return resolveThemeColorConfigHex(systemDefaultColor);
}

/**
 * 解析生效主题色 preset（用于色板选中态；custom 时返回 null）
 * @param setting 当前 AppSetting
 * @param holiday 假日主题 DTO 片段
 * @param storedPreset 持久化 preset
 * @returns preset 或 null
 */
export function resolveEffectiveColorPreset(
  setting: AppSetting,
  holiday: TaktHolidayThemeHint | null | undefined,
  storedPreset: TaktThemeColorPreset,
): TaktThemeColorPreset | null {
  if (setting.appearanceUserOverride) {
    if (setting.themeColor.type === 'custom') {
      return null;
    }
    return storedPreset;
  }
  if (holiday?.isHolidayToday) {
    const resolved = resolveThemeColorPreset(holiday.holidayTheme);
    if (resolved) {
      return resolved;
    }
  }
  return mapAppSettingThemeColorToPreset(systemDefaultThemeColorConfig.type);
}

/**
 * 将 Ant Design Vue token 中的 color* 字段写入 documentElement CSS 变量
 * @param token Ant Design Vue useToken 返回的 token
 * @param prefix CSS 变量前缀，默认 ant
 */
export function syncAntDesignCssVariables(token: GlobalToken, prefix = 'ant'): void {
  const root = document.documentElement;
  (Object.keys(token) as Array<keyof GlobalToken>).forEach((key) => {
    const keyName = String(key);
    if (!keyName.startsWith('color')) {
      return;
    }
    const value = token[key];
    if (typeof value !== 'string') {
      return;
    }
    const cssVar = `--${prefix}-${camelToKebab(keyName)}`;
    // 值未变则跳过，避免 ConfigProvider 刷新时整批重写 :root
    if (root.style.getPropertyValue(cssVar) === value) {
      return;
    }
    root.style.setProperty(cssVar, value);
  });
}
