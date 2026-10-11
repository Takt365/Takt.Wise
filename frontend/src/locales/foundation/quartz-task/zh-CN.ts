// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/foundation/quartz-task
// 文件名称：zh-CN.ts
// 创建时间：2026-06-28
// 创建人：Takt365(Cursor AI)
// 功能描述：foundation/quartz-task Cron 弹窗静态文案；引用键 foundation.quartz-task.page.cron.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    executeSubmitted: '已提交后台执行，完成后将通知并刷新列表',
    executeMonth: {
      modalTitle: '选择核算月份与目标库',
      costingMonth: '核算月份',
      costingMonthPlaceholder: '请选择核算月份',
      costingMonthRequired: '请选择核算月份',
      hint: '须同时选择目标库与核算月；Cron 未传参时核算月默认当月、库取任务 ExecuteParams。',
    },
    executeDb: {
      modalTitle: '选择同步数据库',
      sourceDatabase: '源表数据库',
      targetDatabase: '目标表数据库',
      sourceRequired: '请选择源表数据库',
      targetRequired: '请选择目标表数据库',
      hintTargetOnly: '源库固定（或本库回填）；请选择写入的目标租户库。',
      hintSourceTarget: '请选择暂存源库与写入目标租户库；Cron 任务须在 ExecuteParams 配置同名字段。',
    },
    signalr: {
      executeSucceeded: '任务 {code} 执行成功（{duration}ms）',
      executeFailed: '任务 {code} 执行失败（{duration}ms）',
    },
    cron: {
      modalTitle: 'Cron 表达式',
      inputPlaceholder: '点击输入框配置 Cron 表达式',
      expressionTitle: '时间表达式',
      fullExpression: 'crontab完整表达式',
      ok: '确定',
      wildcard: '{unit}，允许的通配符[, - * /]',
      rangeFrom: '周期从',
      rangeTo: '到',
      intervalFrom: '从',
      intervalEvery: '开始，每',
      intervalExecute: '执行一次',
      specify: '指定',
      nextRuns: '最近 5 次运行时间',
      noNextRuns: '无法解析当前表达式',
      sourceExpression: '原表达式',
      sourceMeaning: '含义说明',
      describe: {
        atTime: '每天 {h}:{m}:{s} 执行',
        intervalSeconds: '从第 {start} 秒起，每隔 {step} 秒',
        intervalMinutes: '从第 {start} 分起，每隔 {step} 分钟',
        intervalHours: '从 {start} 点起，每隔 {step} 小时',
        specificSeconds: '指定秒：{values}',
        specificMinutes: '指定分：{values}',
        specificHours: '指定时：{values}',
        specificDays: '指定日：{values}',
        specificMonths: '指定月：{values}',
        specificWeeks: '指定星期：{values}',
        unknown: '无法自动解释该表达式，请对照下方分段编辑',
        join: '；',
      },
      tab: {
        second: '秒',
        minute: '分钟',
        hour: '小时',
        day: '日',
        month: '月',
        week: '周',
        year: '年',
      },
      field: {
        second: '秒',
        minute: '分钟',
        hour: '小时',
        day: '日',
        month: '月',
        week: '周',
        year: '年',
      },
      everySecond: '每一秒钟',
      everyMinute: '每一分钟',
      everyHour: '每一小时',
      everyDay: '每一天',
      everyMonth: '每一月',
      everyYear: '每一年',
      everyInterval: '每隔 {step} {unit} 执行，从 {start} {unit} 开始',
      specificMulti: '具体{unit}（可多选）',
      range: '周期从 {start} 到 {end} {unit}',
      everyWeekInterval: '每隔 {step} 周执行，从 {start} 开始',
      specificWeek: '具体星期（可多选）',
      specificDay: '具体天数（可多选）',
      lastDayOfMonth: '在这个月的最后一天',
      lastWorkdayOfMonth: '在这个月的最后一个工作日',
      lastWeekdayOfMonth: '在这个月的最后一个 {weekday}',
      daysBeforeMonthEnd: '在本月底前 {days} 天',
      nearestWeekday: '最近的工作日至本月 {day} 日',
      nthWeekday: '在这个月的第 {nth} 个 {weekday}',
      monthRange: '从 {start} 到 {end} 月之间的每个月',
      yearRange: '从 {start} 到 {end} 年之间的每一年',
      selectPlaceholder: '请选择',
      weekday: {
        mon: '星期一',
        tue: '星期二',
        wed: '星期三',
        thu: '星期四',
        fri: '星期五',
        sat: '星期六',
        sun: '星期日',
      },
    },
  },
}
