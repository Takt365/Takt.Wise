// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：taktWebVitalsInsight.ts
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：Web Vitals 可观测性纯工具（Navigation/Resource/LongTask 归因与控制台摘要）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { EventTrackingDiagnosis } from '@/utils/taktEventTrackingDiagnosis';
import { isLongTaskApiSupported } from '@/utils/taktLongTask';
import type { TaktWebVitalMetric } from '@/utils/taktWebVitals';

/** CWV 等级 */
export type TaktWebVitalGrade = 'good' | 'needs-improvement' | 'poor';

/** Navigation Timing 摘要 */
export interface TaktNavigationTimingInsight {
  ttfbMs: number;
  dnsMs: number;
  tcpMs: number;
  responseMs: number;
  domInteractiveMs: number;
  domContentLoadedMs: number;
  loadEventEndMs: number;
}

/** 慢资源摘要 */
export interface TaktSlowResourceInsight {
  name: string;
  durationMs: number;
  initiatorType: string;
  transferSize: number;
}

/** FCP 前 Long Task 摘要 */
export interface TaktLongTaskBeforeInsight {
  startMs: number;
  durationMs: number;
}

/** Web Vital 性能归因 */
export interface TaktWebVitalPerformanceInsight {
  routePath: string;
  isDev: boolean;
  grade: TaktWebVitalGrade;
  excessMs: number;
  navigation: TaktNavigationTimingInsight | null;
  slowResources: TaktSlowResourceInsight[];
  longTasksBefore: TaktLongTaskBeforeInsight[];
  longTaskTotalMsBefore: number;
  likelyCause: string;
  suggestions: string[];
}

/**
 * 解析 CWV 等级
 * @param rawValue 指标值（毫秒或 CLS）
 * @param warnThreshold Warning 阈值
 * @param errorThreshold Error / Poor 阈值
 * @returns {TaktWebVitalGrade} 等级
 */
export function resolveWebVitalGrade(
  rawValue: number,
  warnThreshold: number,
  errorThreshold: number
): TaktWebVitalGrade {
  if (rawValue >= errorThreshold) {
    return 'poor';
  }
  if (rawValue >= warnThreshold) {
    return 'needs-improvement';
  }
  return 'good';
}

/**
 * 缩短资源 URL 便于日志阅读
 * @param url 完整 URL
 * @returns {string} 路径末尾片段
 */
export function shortenResourceUrlForLog(url: string): string {
  if (!url?.trim()) {
    return '';
  }
  try {
    const parsed = new URL(url, typeof window !== 'undefined' ? window.location.origin : undefined);
    const segments = parsed.pathname.split('/').filter(Boolean);
    const tail = segments.slice(-2).join('/');
    return tail || parsed.pathname || url;
  } catch {
    const parts = url.split('/').filter(Boolean);
    return parts.slice(-2).join('/') || url;
  }
}

/**
 * 读取 Navigation Timing 摘要
 * @returns {TaktNavigationTimingInsight | null} 无 navigation 条目时为 null
 */
export function readNavigationTimingInsight(): TaktNavigationTimingInsight | null {
  if (typeof performance === 'undefined' || !('getEntriesByType' in performance)) {
    return null;
  }
  const nav = performance.getEntriesByType('navigation')[0] as PerformanceNavigationTiming | undefined;
  if (!nav) {
    return null;
  }
  return {
    ttfbMs: Math.max(0, Math.round(nav.responseStart - nav.requestStart)),
    dnsMs: Math.max(0, Math.round(nav.domainLookupEnd - nav.domainLookupStart)),
    tcpMs: Math.max(0, Math.round(nav.connectEnd - nav.connectStart)),
    responseMs: Math.max(0, Math.round(nav.responseEnd - nav.responseStart)),
    domInteractiveMs: Math.max(0, Math.round(nav.domInteractive - nav.fetchStart)),
    domContentLoadedMs: Math.max(0, Math.round(nav.domContentLoadedEventEnd - nav.fetchStart)),
    loadEventEndMs: Math.max(0, Math.round(nav.loadEventEnd - nav.fetchStart)),
  };
}

/**
 * 是否为 Vite 开发态 ESM 模块请求（非生产 bundle，duration 多为编译排队）
 * @param resourceName PerformanceResourceTiming.name
 * @returns {boolean} 是 dev 模块时为 true
 */
export function isViteDevModuleResource(resourceName: string): boolean {
  if (!resourceName) {
    return false;
  }
  return resourceName.includes('/@vite/') || resourceName.includes('/@fs/')
    || /\.tsx?(?:\?|$)/.test(resourceName) || resourceName.includes('.vue?');
}

/**
 * 读取截止时刻前的慢资源（按 duration 降序；生产环境优先 .js/.css bundle）
 * @param cutoffMs 截止毫秒（如 FCP）
 * @param limit 最多条数
 * @returns {TaktSlowResourceInsight[]} 慢资源列表
 */
export function readSlowResourcesBeforeMs(cutoffMs: number, limit = 5): TaktSlowResourceInsight[] {
  if (typeof performance === 'undefined' || !('getEntriesByType' in performance) || cutoffMs <= 0) {
    return [];
  }
  const isDev = typeof import.meta !== 'undefined' && !!import.meta.env?.DEV;
  const resources = performance.getEntriesByType('resource') as PerformanceResourceTiming[];
  const mapped = resources
    .filter((entry) => entry.responseEnd > 0 && entry.responseEnd <= cutoffMs + 100)
    .map((entry) => ({
      name: shortenResourceUrlForLog(entry.name),
      durationMs: Math.max(0, Math.round(entry.duration)),
      initiatorType: entry.initiatorType ?? '',
      transferSize: entry.transferSize ?? 0,
      isDevModule: isViteDevModuleResource(entry.name),
    }));
  const productionLike = mapped.filter((item) => !item.isDevModule);
  if (isDev) {
    if (productionLike.length === 0) {
      return [];
    }
    return productionLike
      .sort((a, b) => b.durationMs - a.durationMs)
      .slice(0, Math.max(1, limit))
      .map(({ isDevModule: _ignored, ...item }) => item);
  }
  const pool = productionLike.length > 0 ? productionLike : mapped;
  return pool
    .sort((a, b) => b.durationMs - a.durationMs)
    .slice(0, Math.max(1, limit))
    .map(({ isDevModule: _ignored, ...item }) => item);
}

/**
 * 读取截止时刻前的 Long Task
 * @param cutoffMs 截止毫秒
 * @param limit 最多条数
 * @returns {TaktLongTaskBeforeInsight[]} Long Task 列表
 */
export function readLongTasksBeforeMs(cutoffMs: number, limit = 5): TaktLongTaskBeforeInsight[] {
  if (!isLongTaskApiSupported() || cutoffMs <= 0) {
    return [];
  }
  return performance.getEntriesByType('longtask')
    .filter((entry) => entry.startTime + entry.duration <= cutoffMs + 50)
    .map((entry) => ({
      startMs: Math.max(0, Math.round(entry.startTime)),
      durationMs: Math.max(0, Math.round(entry.duration)),
    }))
    .sort((a, b) => b.durationMs - a.durationMs)
    .slice(0, Math.max(1, limit));
}

/**
 * 推断 FCP/LCP 类指标主因与建议
 * @param metric 指标
 * @param rawValue 原始值
 * @param navigation Navigation 摘要
 * @param slowResources 慢资源
 * @param longTaskTotalMs Long Task 累计毫秒
 * @param isDev 是否开发环境
 * @returns {{ likelyCause: string; suggestions: string[] }} 主因与建议
 */
function inferLoadingLikelyCause(
  metric: TaktWebVitalMetric,
  rawValue: number,
  navigation: TaktNavigationTimingInsight | null,
  slowResources: TaktSlowResourceInsight[],
  longTaskTotalMs: number,
  isDev: boolean
): { likelyCause: string; suggestions: string[] } {
  const suggestions: string[] = [];
  if (isDev) {
    suggestions.unshift('生产构建复测（Vite dev 逐模块 .ts 请求会虚高 FCP，非真实 bundle）');
  }
  if (navigation && navigation.ttfbMs >= 600) {
    suggestions.push(`TTFB ${navigation.ttfbMs}ms 偏高：查后端接口/网关/502/冷启动`);
  }
  if (navigation && navigation.domContentLoadedMs >= rawValue * 0.6) {
    suggestions.push('DOM 解析偏慢：减少首屏同步脚本、延迟非关键组件');
  }
  const topScript = slowResources.find((item) => item.initiatorType === 'script' || item.name.endsWith('.js'));
  if (topScript && topScript.durationMs >= 300) {
    suggestions.push(`慢 JS「${topScript.name}」${topScript.durationMs}ms：路由懒加载/分包/Tree-shaking`);
  }
  const topCss = slowResources.find((item) => item.initiatorType === 'css' || item.name.endsWith('.css'));
  if (topCss && topCss.durationMs >= 200) {
    suggestions.push(`慢 CSS「${topCss.name}」${topCss.durationMs}ms：关键 CSS 内联或减体积`);
  }
  if (longTaskTotalMs >= 200) {
    suggestions.push(`FCP 前 Long Task 累计 ${longTaskTotalMs}ms：拆分主线程任务/Web Worker`);
  }
  if (suggestions.length === 0) {
    suggestions.push('打开 Performance 面板核对瀑布图与 Main 线程火焰图');
  }
  let likelyCause = '首屏资源加载链路过长';
  if (isDev) {
    likelyCause = 'Vite 开发态逐模块编译（.ts 请求耗时≠生产 JS bundle）';
  } else if (navigation && navigation.ttfbMs >= 600) {
    likelyCause = '网络/后端 TTFB 偏高';
  } else if (longTaskTotalMs >= 200) {
    likelyCause = 'FCP 前主线程 Long Task 阻塞渲染';
  } else if (topScript && topScript.durationMs >= 300) {
    likelyCause = '首屏 JS 下载或解析偏慢';
  } else if (metric === 'lcp') {
    likelyCause = '最大内容元素（图片/文本块）渲染偏晚';
  }
  return { likelyCause, suggestions: suggestions.slice(0, 4) };
}

/**
 * 构建 Web Vital 性能归因（FCP/LCP/INP/CLS）
 * @param metric 指标名
 * @param rawValue 原始值
 * @param warnThreshold Warning 阈值
 * @param errorThreshold Error 阈值
 * @returns {TaktWebVitalPerformanceInsight} 归因摘要
 */
export function buildWebVitalPerformanceInsight(
  metric: TaktWebVitalMetric,
  rawValue: number,
  warnThreshold: number,
  errorThreshold: number
): TaktWebVitalPerformanceInsight {
  const routePath = typeof window !== 'undefined' ? window.location.pathname : '';
  const isDev = typeof import.meta !== 'undefined' && !!import.meta.env?.DEV;
  const grade = resolveWebVitalGrade(rawValue, warnThreshold, errorThreshold);
  const excessMs = Math.max(0, Math.round(rawValue - warnThreshold));
  const navigation = metric === 'fcp' || metric === 'lcp' ? readNavigationTimingInsight() : null;
  const cutoffMs = metric === 'cls' ? performance.now() : rawValue;
  const slowResources = metric === 'fcp' || metric === 'lcp'
    ? readSlowResourcesBeforeMs(cutoffMs)
    : [];
  const longTasksBefore = metric === 'fcp' || metric === 'lcp' || metric === 'inp'
    ? readLongTasksBeforeMs(metric === 'inp' ? performance.now() : cutoffMs)
    : [];
  const longTaskTotalMsBefore = longTasksBefore.reduce((sum, item) => sum + item.durationMs, 0);
  let likelyCause = '指标正常';
  let suggestions = ['持续观察 Core Web Vitals'];
  if (grade !== 'good') {
    if (metric === 'inp') {
      likelyCause = longTaskTotalMsBefore >= 200 ? '交互时主线程 Long Task 阻塞' : '交互事件处理偏慢';
      suggestions = [
        '拆分 click/input 回调中的重计算',
        '列表页启用 virtual / 分页，避免一次渲染大量 DOM',
        '检查同步 Axios 拦截与 Pinia 大对象更新',
      ];
      if (isDev) {
        suggestions.unshift('开发模式复测生产构建');
      }
    } else if (metric === 'cls') {
      likelyCause = '布局在首屏后发生未预留空间的位移';
      suggestions = [
        '图片/广告位预留 width/height',
        '避免在现有内容上方动态插入 banner',
        '字体加载使用 font-display: optional 或 size-adjust',
      ];
    } else {
      const inferred = inferLoadingLikelyCause(
        metric,
        rawValue,
        navigation,
        slowResources,
        longTaskTotalMsBefore,
        isDev
      );
      likelyCause = inferred.likelyCause;
      suggestions = inferred.suggestions;
    }
  }
  return {
    routePath,
    isDev,
    grade,
    excessMs,
    navigation,
    slowResources,
    longTasksBefore,
    longTaskTotalMsBefore,
    likelyCause,
    suggestions,
  };
}

/**
 * 用诊断 + 归因生成控制台一行摘要（无需展开 context 即可读）
 * @param metric 指标名
 * @param rawValue 原始值
 * @param warnThreshold Warning 阈值
 * @param errorThreshold Error 阈值
 * @param diagnosis 诊断结果
 * @param insight 性能归因
 * @returns {string} 控制台主消息
 */
export function formatWebVitalConsoleMessage(
  metric: TaktWebVitalMetric,
  rawValue: number,
  warnThreshold: number,
  errorThreshold: number,
  diagnosis: EventTrackingDiagnosis,
  insight: TaktWebVitalPerformanceInsight
): string {
  const valueText = metric === 'cls' ? rawValue.toFixed(3) : `${Math.round(rawValue)}ms`;
  const thresholdText = metric === 'cls'
    ? `warn≥${warnThreshold} poor≥${errorThreshold}`
    : `warn≥${warnThreshold}ms poor≥${errorThreshold}ms`;
  const parts = [
    `${metric.toUpperCase()} ${valueText}`,
    `超出 ${insight.excessMs}${metric === 'cls' ? '' : 'ms'}`,
    `等级 ${insight.grade}`,
    `定位 ${diagnosis.problemLocation}`,
    `主因 ${insight.likelyCause}`,
    `建议 ${insight.suggestions[0] ?? diagnosis.action}`,
  ];
  if (insight.routePath) {
    parts.push(`路由 ${insight.routePath}`);
  }
  if (insight.navigation) {
    parts.push(`TTFB ${insight.navigation.ttfbMs}ms DCL ${insight.navigation.domContentLoadedMs}ms`);
  }
  if (insight.slowResources[0]) {
    const top = insight.slowResources[0];
    parts.push(`慢资源 ${top.name} ${top.durationMs}ms (${top.initiatorType})`);
  }
  if (insight.longTaskTotalMsBefore > 0) {
    parts.push(`LongTask累计 ${insight.longTaskTotalMsBefore}ms`);
  }
  if (insight.isDev) {
    parts.push('env=dev');
  }
  parts.push(`阈值 ${thresholdText}`);
  return parts.join(' | ');
}

/**
 * 构建写入 logger context 的结构化归因
 * @param metric 指标名
 * @param rawValue 原始值
 * @param warnThreshold Warning 阈值
 * @param errorThreshold Error 阈值
 * @param diagnosis 诊断结果
 * @param insight 性能归因
 * @returns {Record<string, unknown>} LogContext 扩展字段
 */
export function buildWebVitalLogContext(
  metric: TaktWebVitalMetric,
  rawValue: number,
  warnThreshold: number,
  errorThreshold: number,
  diagnosis: EventTrackingDiagnosis,
  insight: TaktWebVitalPerformanceInsight
): Record<string, unknown> {
  return {
    action: 'web-vital',
    metric,
    rawValue,
    warnThreshold,
    errorThreshold,
    grade: insight.grade,
    excessMs: insight.excessMs,
    routePath: insight.routePath,
    isDev: insight.isDev,
    problemLocation: diagnosis.problemLocation,
    actionHint: insight.suggestions.join('；'),
    reportResult: diagnosis.reportResult,
    likelyCause: insight.likelyCause,
    navigation: insight.navigation ?? undefined,
    slowResources: insight.slowResources,
    longTasksBefore: insight.longTasksBefore,
    longTaskTotalMsBefore: insight.longTaskTotalMsBefore,
  };
}
