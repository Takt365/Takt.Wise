// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/foundation/quartz-task
// 文件名称：ja-JP.ts
// 创建时间：2026-06-28
// 创建人：Takt365(Cursor AI)
// 功能描述：foundation/quartz-task Cron モーダル静的文案；キー foundation.quartz-task.page.cron.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件 use MIT License.
// ========================================

export default {
  page: {
    executeSubmitted: 'バックグラウンド実行を受け付けました。完了後に通知します',
    executeMonth: {
      modalTitle: '原価計算月とターゲット DB を選択',
      costingMonth: '原価計算月',
      costingMonthPlaceholder: '原価計算月を選択',
      costingMonthRequired: '原価計算月を選択してください',
      hint: 'ターゲット DB と原価計算月の両方が必須です。パラメータなしの Cron は当月、DB はタスク ExecuteParams を使用します。',
    },
    executeDb: {
      modalTitle: '同期データベースを選択',
      sourceDatabase: 'ソースデータベース',
      targetDatabase: 'ターゲットデータベース',
      sourceRequired: 'ソースデータベースを選択してください',
      targetRequired: 'ターゲットデータベースを選択してください',
      hintTargetOnly: 'ソースは固定（または同一 DB 回填）です。書き込み先のテナント DB を選択してください。',
      hintSourceTarget: 'ステージング元と書き込み先テナント DB を選択してください。Cron は ExecuteParams に同キーが必要です。',
    },
    signalr: {
      executeSucceeded: 'タスク {code} 実行成功（{duration}ms）',
      executeFailed: 'タスク {code} 実行失敗（{duration}ms）',
    },
    cron: {
      modalTitle: 'Cron 式',
      inputPlaceholder: 'クリックして Cron 式を設定',
      expressionTitle: '時間式',
      fullExpression: 'Cron 完全式',
      ok: '確定',
      wildcard: '{unit}、ワイルドカード [, - * /] 可',
      rangeFrom: '周期',
      rangeTo: 'から',
      intervalFrom: '',
      intervalEvery: ' から、',
      intervalExecute: ' ごとに実行',
      specify: '指定',
      nextRuns: '直近 5 回の実行時刻',
      noNextRuns: '現在の式を解析できません',
      sourceExpression: '元の式',
      sourceMeaning: '意味の説明',
      describe: {
        atTime: '毎日 {h}:{m}:{s} に実行',
        intervalSeconds: '{start} 秒から {step} 秒ごと',
        intervalMinutes: '{start} 分から {step} 分ごと',
        intervalHours: '{start} 時から {step} 時間ごと',
        specificSeconds: '指定秒：{values}',
        specificMinutes: '指定分：{values}',
        specificHours: '指定時：{values}',
        specificDays: '指定日：{values}',
        specificMonths: '指定月：{values}',
        specificWeeks: '指定曜日：{values}',
        unknown: 'この式を自動説明できません。下の項目で編集してください',
        join: '；',
      },
      tab: {
        second: '秒',
        minute: '分',
        hour: '時',
        day: '日',
        month: '月',
        week: '週',
        year: '年',
      },
      field: {
        second: '秒',
        minute: '分',
        hour: '時',
        day: '日',
        month: '月',
        week: '週',
        year: '年',
      },
      everySecond: '毎秒',
      everyMinute: '毎分',
      everyHour: '毎時',
      everyDay: '毎日',
      everyMonth: '毎月',
      everyYear: '毎年',
      everyInterval: '{start} {unit} から {step} {unit} ごと',
      specificMulti: '特定の{unit}（複数可）',
      range: '{start} から {end} {unit}',
      everyWeekInterval: '{start} 週目から {step} 週ごと',
      specificWeek: '特定の曜日（複数可）',
      specificDay: '特定の日（複数可）',
      lastDayOfMonth: '月末最終日',
      lastWorkdayOfMonth: '月末最終平日',
      lastWeekdayOfMonth: '月末最終 {weekday}',
      daysBeforeMonthEnd: '月末 {days} 日前',
      nearestWeekday: '本月 {day} 日に最も近い平日',
      nthWeekday: '本月第 {nth} {weekday}',
      monthRange: '{start} 月から {end} 月まで毎月',
      yearRange: '{start} 年から {end} 年まで毎年',
      selectPlaceholder: '選択',
      weekday: {
        mon: '月曜日',
        tue: '火曜日',
        wed: '水曜日',
        thu: '木曜日',
        fri: '金曜日',
        sat: '土曜日',
        sun: '日曜日',
      },
    },
  },
}
