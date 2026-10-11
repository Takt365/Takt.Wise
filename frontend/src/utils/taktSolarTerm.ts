// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：taktSolarTerm.ts
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：二十四节气近似日期与 i18n 末段键（对齐 common.page.quote.{节气}）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 二十四节气键（拼音小写，与种子 I18nKey 末段一致）
 */
export const TAKT_SOLAR_TERM_KEYS = [
  'lichun',
  'yushui',
  'jingzhe',
  'chunfen',
  'qingming',
  'guyu',
  'lixia',
  'xiaoman',
  'mangzhong',
  'xiazhi',
  'xiaoshu',
  'dashu',
  'liqiu',
  'chushu',
  'bailu',
  'qiufen',
  'hanlu',
  'shuangjiang',
  'lidong',
  'xiaoxue',
  'daxue',
  'dongzhi',
  'xiaohan',
  'dahan',
] as const;

/** 节气键类型 */
export type TaktSolarTermKey = (typeof TAKT_SOLAR_TERM_KEYS)[number];

/**
 * 节气起始近似公历月日（北半球常用民用近似；用于 UI 轮换，非天文精密推算）
 * 顺序：立春→…→大寒（跨年以小寒/大寒在 1 月）
 */
const SOLAR_TERM_STARTS: ReadonlyArray<{ key: TaktSolarTermKey; month: number; day: number }> = [
  { key: 'xiaohan', month: 1, day: 5 },
  { key: 'dahan', month: 1, day: 20 },
  { key: 'lichun', month: 2, day: 4 },
  { key: 'yushui', month: 2, day: 19 },
  { key: 'jingzhe', month: 3, day: 6 },
  { key: 'chunfen', month: 3, day: 21 },
  { key: 'qingming', month: 4, day: 5 },
  { key: 'guyu', month: 4, day: 20 },
  { key: 'lixia', month: 5, day: 6 },
  { key: 'xiaoman', month: 5, day: 21 },
  { key: 'mangzhong', month: 6, day: 6 },
  { key: 'xiazhi', month: 6, day: 21 },
  { key: 'xiaoshu', month: 7, day: 7 },
  { key: 'dashu', month: 7, day: 23 },
  { key: 'liqiu', month: 8, day: 8 },
  { key: 'chushu', month: 8, day: 23 },
  { key: 'bailu', month: 9, day: 8 },
  { key: 'qiufen', month: 9, day: 23 },
  { key: 'hanlu', month: 10, day: 8 },
  { key: 'shuangjiang', month: 10, day: 23 },
  { key: 'lidong', month: 11, day: 7 },
  { key: 'xiaoxue', month: 11, day: 22 },
  { key: 'daxue', month: 12, day: 7 },
  { key: 'dongzhi', month: 12, day: 22 },
];

/**
 * 按公历日期解析当前二十四节气键（民用近似）
 * @param date 参考日期；非法时按当天
 * @returns {TaktSolarTermKey} 节气拼音键
 */
export function resolveTaktSolarTermKey(date?: Date | null): TaktSolarTermKey {
  const d = date instanceof Date && !Number.isNaN(date.getTime()) ? date : new Date();
  const month = d.getMonth() + 1;
  const day = d.getDate();
  const md = month * 100 + day;
  let current: TaktSolarTermKey = 'dahan';
  for (const term of SOLAR_TERM_STARTS) {
    const termMd = term.month * 100 + term.day;
    if (md >= termMd) {
      current = term.key;
    }
  }
  // 1/1～小寒前仍属上年冬至之后 → 大雪后至小寒前为冬至段；上环已覆盖
  if (md < SOLAR_TERM_STARTS[0].month * 100 + SOLAR_TERM_STARTS[0].day) {
    return 'dongzhi';
  }
  return current;
}

/**
 * 工作台引用区 i18n 完整键
 * @param date 参考日期
 * @returns {string} common.page.quote.{节气}
 */
export function resolveTaktSolarTermQuoteI18nKey(date?: Date | null): string {
  return `common.page.quote.${resolveTaktSolarTermKey(date)}`;
}
