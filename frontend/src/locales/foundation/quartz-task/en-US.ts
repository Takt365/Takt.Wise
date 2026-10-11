// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/foundation/quartz-task
// 文件名称：en-US.ts
// 创建时间：2026-06-28
// 创建人：Takt365(Cursor AI)
// 功能描述：foundation/quartz-task cron modal static copy; keys foundation.quartz-task.page.cron.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件 use MIT License.
// ========================================

export default {
  page: {
    executeSubmitted: 'Submitted for background execution; you will be notified when finished',
    executeMonth: {
      modalTitle: 'Select costing month and target database',
      costingMonth: 'Costing month',
      costingMonthPlaceholder: 'Select costing month',
      costingMonthRequired: 'Please select a costing month',
      hint: 'Target database and costing month are both required. Cron without params uses the current month and task ExecuteParams for the database.',
    },
    executeDb: {
      modalTitle: 'Select sync databases',
      sourceDatabase: 'Source database',
      targetDatabase: 'Target database',
      sourceRequired: 'Please select a source database',
      targetRequired: 'Please select a target database',
      hintTargetOnly: 'Source is fixed (or in-DB backfill). Choose the target tenant database.',
      hintSourceTarget: 'Choose staging source and target tenant DB. Cron tasks need the same keys in ExecuteParams.',
    },
    signalr: {
      executeSucceeded: 'Task {code} succeeded ({duration}ms)',
      executeFailed: 'Task {code} failed ({duration}ms)',
    },
    cron: {
      modalTitle: 'Cron Expression',
      inputPlaceholder: 'Click to configure cron expression',
      expressionTitle: 'Time Expression',
      fullExpression: 'Full Cron Expression',
      ok: 'OK',
      wildcard: '{unit}, allowed wildcards [, - * /]',
      rangeFrom: 'Cycle from',
      rangeTo: 'to',
      intervalFrom: 'From',
      intervalEvery: ', every',
      intervalExecute: ' execute once',
      specify: 'Specify',
      nextRuns: 'Next 5 Run Times',
      noNextRuns: 'Unable to parse current expression',
      sourceExpression: 'Original expression',
      sourceMeaning: 'Meaning',
      describe: {
        atTime: 'Every day at {h}:{m}:{s}',
        intervalSeconds: 'From second {start}, every {step} second(s)',
        intervalMinutes: 'From minute {start}, every {step} minute(s)',
        intervalHours: 'From hour {start}, every {step} hour(s)',
        specificSeconds: 'Seconds: {values}',
        specificMinutes: 'Minutes: {values}',
        specificHours: 'Hours: {values}',
        specificDays: 'Days of month: {values}',
        specificMonths: 'Months: {values}',
        specificWeeks: 'Weekdays: {values}',
        unknown: 'Cannot auto-explain this expression; edit by field below',
        join: '; ',
      },
      tab: {
        second: 'Second',
        minute: 'Minute',
        hour: 'Hour',
        day: 'Day',
        month: 'Month',
        week: 'Week',
        year: 'Year',
      },
      field: {
        second: 'Sec',
        minute: 'Min',
        hour: 'Hour',
        day: 'Day',
        month: 'Month',
        week: 'Week',
        year: 'Year',
      },
      everySecond: 'Every second',
      everyMinute: 'Every minute',
      everyHour: 'Every hour',
      everyDay: 'Every day',
      everyMonth: 'Every month',
      everyYear: 'Every year',
      everyInterval: 'Every {step} {unit}(s), starting at {start}',
      specificMulti: 'Specific {unit}(s)',
      range: 'From {start} to {end} {unit}(s)',
      everyWeekInterval: 'Every {step} week(s), starting at week {start}',
      specificWeek: 'Specific weekday(s)',
      specificDay: 'Specific day(s) of month',
      lastDayOfMonth: 'Last day of month',
      lastWorkdayOfMonth: 'Last weekday of month',
      lastWeekdayOfMonth: 'Last {weekday} of month',
      daysBeforeMonthEnd: '{days} day(s) before month end',
      nearestWeekday: 'Nearest weekday to day {day} of month',
      nthWeekday: 'The {nth} {weekday} of month',
      monthRange: 'Every month from {start} to {end}',
      yearRange: 'Every year from {start} to {end}',
      selectPlaceholder: 'Select',
      weekday: {
        mon: 'Monday',
        tue: 'Tuesday',
        wed: 'Wednesday',
        thu: 'Thursday',
        fri: 'Friday',
        sat: 'Saturday',
        sun: 'Sunday',
      },
    },
  },
}
