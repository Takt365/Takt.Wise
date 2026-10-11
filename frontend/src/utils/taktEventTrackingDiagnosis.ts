// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：taktEventTrackingDiagnosis.ts
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：性能事件诊断纯工具（上报结果 / 问题定位 / 行动建议）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import {
  getApiSlowThresholdMs,
  getClsErrorThreshold,
  getClsWarnThreshold,
  getFcpWarnThresholdMs,
  getFpsWarnThreshold,
  getInpErrorThresholdMs,
  getInpWarnThresholdMs,
  getLcpErrorThresholdMs,
  getLcpWarnThresholdMs,
  TAKT_EVENT_TYPE_API_ERROR,
  TAKT_EVENT_TYPE_API_SLOW,
  TAKT_EVENT_TYPE_FPS,
  TAKT_EVENT_TYPE_FPS_DWELL,
  TAKT_EVENT_TYPE_LONG_TASK,
  TAKT_EVENT_TYPE_WEB_VITAL,
} from '@/config/eventTracking';

/** 性能事件诊断结果 */
export interface EventTrackingDiagnosis {
  /** 上报结果摘要 */
  reportResult: string;
  /** 问题定位 */
  problemLocation: string;
  /** 建议行动 */
  action: string;
  /** 请求窗口内是否存在 Long Task */
  hasLongTask: boolean;
  /** 窗口内是否存在慢接口 */
  hasApiSlow?: boolean;
  /** 当前 FPS（仅 fps 事件） */
  fps?: number;
}

/** 诊断上下文（关联 Long Task / API 时间窗） */
export interface EventTrackingDiagnosisContext {
  /** API 请求开始 performance.now() */
  apiStartMs?: number;
  /** API 请求结束 performance.now() */
  apiEndMs?: number;
  /** 窗口内是否存在 Long Task */
  hasLongTaskInWindow?: boolean;
  /** 窗口内是否存在慢接口 */
  hasApiSlowInWindow?: boolean;
}

/**
 * 诊断 API 慢请求
 * @param durationMs 接口耗时毫秒
 * @param hasLongTaskDuringRequest 请求窗口内是否有 Long Task
 * @param slowThresholdMs 慢请求阈值
 * @returns {EventTrackingDiagnosis} 诊断结果
 */
export function diagnoseApiSlowEvent(
  durationMs: number,
  hasLongTaskDuringRequest: boolean,
  slowThresholdMs: number = getApiSlowThresholdMs()
): EventTrackingDiagnosis {
  if (durationMs >= slowThresholdMs && !hasLongTaskDuringRequest) {
    return {
      reportResult: `duration > ${slowThresholdMs}ms, hasLongTask: false`,
      problemLocation: '后端慢',
      action: '优化 SQL / 接口缓存',
      hasLongTask: false,
      hasApiSlow: true,
    };
  }
  if (durationMs >= slowThresholdMs && hasLongTaskDuringRequest) {
    return {
      reportResult: `duration > ${slowThresholdMs}ms, hasLongTask: true`,
      problemLocation: '前端渲染卡',
      action: '虚拟列表 / 分页 / 懒加载',
      hasLongTask: true,
      hasApiSlow: true,
    };
  }
  return {
    reportResult: `duration ${durationMs}ms`,
    problemLocation: '接口耗时异常',
    action: '排查网络与服务端日志',
    hasLongTask: hasLongTaskDuringRequest,
    hasApiSlow: durationMs >= slowThresholdMs,
  };
}

/**
 * 诊断页面停留 FPS（体验补充，非告警）
 * @param summary 汇总载荷
 * @returns {EventTrackingDiagnosis} 诊断结果
 */
export function diagnoseFpsDwellEvent(summary: {
  fpsP50: number;
  fpsP75: number;
  dwellMs: number;
  routePath: string;
  endReason: string;
}): EventTrackingDiagnosis {
  return {
    reportResult: `dwell ${summary.dwellMs}ms p50=${summary.fpsP50} p75=${summary.fpsP75} @${summary.routePath}`,
    problemLocation: '页面停留流畅度（体验指标）',
    action: '线上聚合 p50/p75，结合跳出率与关键交互完成率解读；不作性能告警',
    hasLongTask: false,
    fps: summary.fpsP75,
  };
}

/**
 * 诊断 FPS 掉帧
 * @param fps 当前帧率
 * @param hasApiSlowInWindow 采样窗口内是否有慢接口
 * @param threshold FPS 阈值
 * @returns {EventTrackingDiagnosis} 诊断结果
 */
export function diagnoseFpsDropEvent(
  fps: number,
  hasApiSlowInWindow: boolean,
  threshold: number = getFpsWarnThreshold()
): EventTrackingDiagnosis {
  if (!hasApiSlowInWindow) {
    return {
      reportResult: `fps < ${threshold}, 无接口慢请求`,
      problemLocation: '动画/渲染问题',
      action: '检查 CSS / 大量 DOM',
      hasLongTask: false,
      hasApiSlow: false,
      fps,
    };
  }
  return {
    reportResult: `fps < ${threshold}, 伴随接口慢`,
    problemLocation: '前后端叠加',
    action: '先优化接口，再检查渲染与 DOM',
    hasLongTask: false,
    hasApiSlow: true,
    fps,
  };
}

/**
 * 诊断 Long Task
 * @param durationMs 阻塞毫秒
 * @returns {EventTrackingDiagnosis} 诊断结果
 */
export function diagnoseLongTaskEvent(durationMs: number): EventTrackingDiagnosis {
  return {
    reportResult: `longtask ${durationMs}ms`,
    problemLocation: '主线程阻塞',
    action: '拆分长任务 / 使用 Web Worker',
    hasLongTask: true,
  };
}

/**
 * 诊断 Web Vital（FCP / LCP / INP / CLS）
 * @param metric 指标名
 * @param rawValue 原始值（毫秒或 CLS 分数）
 * @param warnThreshold Warning 阈值
 * @param errorThreshold Error 阈值
 * @param likelyCause 可选主因（来自 Performance API 归因）
 * @param suggestions 可选建议列表
 * @returns {EventTrackingDiagnosis} 诊断结果
 */
export function diagnoseWebVitalEvent(
  metric: 'fcp' | 'lcp' | 'inp' | 'cls',
  rawValue: number,
  warnThreshold: number,
  errorThreshold: number,
  likelyCause?: string,
  suggestions?: string[]
): EventTrackingDiagnosis {
  const label = metric.toUpperCase();
  const valueText = metric === 'cls' ? rawValue.toFixed(3) : `${Math.round(rawValue)}ms`;
  const excess = Math.max(0, metric === 'cls' ? rawValue - warnThreshold : rawValue - warnThreshold);
  const excessText = metric === 'cls' ? excess.toFixed(3) : `${Math.round(excess)}ms`;
  const defaultAction = suggestions?.[0]
    ?? (metric === 'inp'
      ? '拆分长任务 / 减少主线程阻塞'
      : metric === 'cls'
        ? '预留尺寸 / 避免动态插入内容'
        : '优化静态资源 / 预加载 / 分包');
  if (rawValue >= errorThreshold) {
    return {
      reportResult: `${label} ${valueText} 超出 ${excessText} (Poor ≥${errorThreshold}${metric === 'cls' ? '' : 'ms'})`,
      problemLocation: likelyCause
        ?? (metric === 'inp' ? '交互响应慢' : metric === 'cls' ? '布局偏移过大' : '首屏加载慢'),
      action: defaultAction,
      hasLongTask: metric === 'inp' || metric === 'fcp' || metric === 'lcp',
    };
  }
  if (rawValue >= warnThreshold) {
    return {
      reportResult: `${label} ${valueText} 超出 ${excessText} (Warning ≥${warnThreshold}${metric === 'cls' ? '' : 'ms'})`,
      problemLocation: likelyCause
        ?? (metric === 'inp' ? '交互响应需优化' : metric === 'cls' ? '布局稳定性需优化' : '首屏加载需优化'),
      action: defaultAction,
      hasLongTask: false,
    };
  }
  return {
    reportResult: `${label} ${valueText}`,
    problemLocation: '指标正常',
    action: '持续观察 Core Web Vitals',
    hasLongTask: false,
  };
}

/**
 * 诊断 API HTTP 错误
 * @param status HTTP 状态码
 * @param durationMs 耗时毫秒
 * @returns {EventTrackingDiagnosis} 诊断结果
 */
export function diagnoseApiErrorEvent(status: number, durationMs: number): EventTrackingDiagnosis {
  return {
    reportResult: `HTTP ${status || 'network-error'}, duration ${durationMs}ms`,
    problemLocation: '接口错误',
    action: '检查后端日志 / 权限 / 参数校验',
    hasLongTask: false,
    hasApiSlow: durationMs >= getApiSlowThresholdMs(),
  };
}

/**
 * 按事件类型生成诊断
 * @param eventTrackingType 事件类型
 * @param payload 事件载荷
 * @param context 关联上下文
 * @returns {EventTrackingDiagnosis | null} 诊断结果
 */
export function buildEventTrackingDiagnosis(
  eventTrackingType: string,
  payload: {
    durationMs: number;
    performanceStartMs?: number;
    entryName?: string;
    containerName?: string;
  },
  context: EventTrackingDiagnosisContext = {}
): EventTrackingDiagnosis | null {
  const type = eventTrackingType.trim().toLowerCase();
  switch (type) {
    case TAKT_EVENT_TYPE_API_SLOW:
      return diagnoseApiSlowEvent(
        payload.durationMs,
        context.hasLongTaskInWindow ?? false
      );
    case TAKT_EVENT_TYPE_FPS:
      return diagnoseFpsDropEvent(
        payload.performanceStartMs ?? 0,
        context.hasApiSlowInWindow ?? false
      );
    case TAKT_EVENT_TYPE_FPS_DWELL:
      return diagnoseFpsDwellEvent({
        fpsP50: (payload.performanceStartMs ?? 0) / 10,
        fpsP75: Number(payload.containerId?.replace(/^p75-/, '')) / 10 || 0,
        dwellMs: payload.durationMs,
        routePath: payload.containerSrc ?? '',
        endReason: payload.containerName ?? '',
      });
    case TAKT_EVENT_TYPE_LONG_TASK:
      return diagnoseLongTaskEvent(payload.durationMs);
    case TAKT_EVENT_TYPE_API_ERROR:
      return diagnoseApiErrorEvent(Number(payload.containerName) || 0, payload.durationMs);
    case TAKT_EVENT_TYPE_WEB_VITAL: {
      const entryName = payload.entryName?.trim().toLowerCase() ?? 'fcp';
      if (entryName === 'lcp') {
        return diagnoseWebVitalEvent(
          'lcp',
          payload.durationMs,
          getLcpWarnThresholdMs(),
          getLcpErrorThresholdMs()
        );
      }
      if (entryName === 'inp') {
        return diagnoseWebVitalEvent(
          'inp',
          payload.durationMs,
          getInpWarnThresholdMs(),
          getInpErrorThresholdMs()
        );
      }
      if (entryName === 'cls') {
        const clsScore = payload.durationMs / 1000;
        return diagnoseWebVitalEvent(
          'cls',
          clsScore,
          getClsWarnThreshold(),
          getClsErrorThreshold()
        );
      }
      return diagnoseWebVitalEvent('fcp', payload.durationMs, getFcpWarnThresholdMs(), getFcpWarnThresholdMs());
    }
    default:
      return null;
  }
}

/**
 * 合并诊断到 attributionJson
 * @param attributionJson 原始 JSON 字符串
 * @param diagnosis 诊断结果
 * @returns {string} 合并后的 JSON
 */
export function mergeDiagnosisIntoAttributionJson(
  attributionJson: string | undefined,
  diagnosis: EventTrackingDiagnosis
): string {
  let base: Record<string, unknown> = {};
  if (attributionJson?.trim()) {
    try {
      const parsed = JSON.parse(attributionJson) as unknown;
      if (parsed && typeof parsed === 'object' && !Array.isArray(parsed)) {
        base = parsed as Record<string, unknown>;
      } else if (parsed != null) {
        base = { raw: parsed };
      }
    } catch {
      base = { raw: attributionJson };
    }
  }
  return JSON.stringify({ ...base, diagnosis });
}

/**
 * 从 attributionJson 解析诊断结果
 * @param attributionJson attribution JSON
 * @returns {EventTrackingDiagnosis | null} 诊断或 null
 */
export function parseEventTrackingDiagnosis(
  attributionJson: string | null | undefined
): EventTrackingDiagnosis | null {
  if (!attributionJson?.trim()) {
    return null;
  }
  try {
    const parsed = JSON.parse(attributionJson) as { diagnosis?: EventTrackingDiagnosis };
    if (parsed?.diagnosis?.reportResult && parsed.diagnosis.problemLocation && parsed.diagnosis.action) {
      return parsed.diagnosis;
    }
  } catch {
    return null;
  }
  return null;
}
