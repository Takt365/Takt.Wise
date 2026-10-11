// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/code/database/table-archive
// 文件名称：en-US.ts
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：code/database/table-archive static copy; keys code.database.table-archive.page.* (lowercase segments)
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'Table Archive',
    subtitle:
      'Register tables and archive keys; ensure {table}_{year} year tables to ease the hot table; optionally move historical rows by year',
    archive: {
      title: 'Archive by year',
      year: 'Archive year',
      yearrequired: 'Select an archive year',
      selectpolicies: 'Select configs',
      preview: 'Preview row count',
      execute: 'Confirm archive',
      runnow: 'Run now',
      schedule: 'Schedule',
      scheduledat: 'Run at',
      previewtotal: 'Total rows to move: {count}',
      success: 'Archive completed',
      failed: 'Archive failed',
      runsuccess: 'Immediate archive task created',
      schedulesuccess: 'Background archive task created',
      emptyselection: 'Select at least one enabled config',
      schedulerequired: 'Select a run time',
      schedulefuture: 'Run time must be in the future',
      kinddatetime: 'yyyyMMddHHmmss (e.g. …_20251010101000)',
      kindyearmonth: 'yyyyMM (e.g. …_202510)',
      kindyear: 'yyyy (e.g. …_2025)',
    },
    tip: {
      archivekeycolumn:
        'Archive key column: the physical column that decides which year a row belongs to (e.g. costing_date). Preview/archive filters by this column and moves matching rows into archive tables named by key kind. Pick a column that matches your business year.',
      archivekeykind:
        'Archive key kind (dict sys_archive_key_kind): standard date formats. 1=yyyyMMddHHmmss → …_20251010101000; 2=yyyyMM → …_202510; 3=yyyy (default) → …_2025. Archive name is {table}_{formatCode}. Suggested from column type after you pick a column; you may override.',
      retainhotyears:
        'Hot retain years: fixed to 1 (not editable). Current-year data stays in the hot table; only years ≤ currentYear−1 may be archived (e.g. in 2026, only ≤2025).',
    },
    ensureyears: {
      title: 'Ensure year tables',
      years: 'Year range',
      yearstart: 'Start year',
      yearend: 'End year',
      yearshint: 'Creates tables like {table}_2026 (clone structure if missing); inclusive range, max 30 years per run',
      execute: 'Create tables',
      success: 'Year tables ready',
      failed: 'Failed to create year tables',
      emptyyears: 'Select a year range',
      spantoolarge: 'At most 30 year tables per run; narrow the range',
      result: 'Ready: {tables}',
    },
  },
};
