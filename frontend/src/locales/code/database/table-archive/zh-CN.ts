// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/code/database/table-archive
// 文件名称：zh-CN.ts
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：code/database/table-archive 页面静态文案；引用键 code.database.table-archive.page.*（段内全小写）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '数据表归档',
    subtitle:
      '登记物理表与归档键；预建 {表名}_{年份} 年表以降低主表压力，亦可按年将基表历史数据迁入年表',
    archive: {
      title: '按年归档',
      year: '归档年份',
      yearrequired: '请选择归档年份',
      selectpolicies: '选择配置',
      preview: '预览行数',
      execute: '确认归档',
      runnow: '立即执行',
      schedule: '后台执行',
      scheduledat: '执行时间',
      previewtotal: '合计将迁移 {count} 行',
      success: '归档完成',
      failed: '归档失败',
      runsuccess: '已创建立即归档任务',
      schedulesuccess: '已创建后台归档任务',
      emptyselection: '请至少选择一条已启用配置',
      schedulerequired: '请选择执行时间',
      schedulefuture: '执行时间须晚于当前时间',
      kinddatetime: 'yyyyMMddHHmmss（例 …_20251010101000）',
      kindyearmonth: 'yyyyMM（例 …_202510）',
      kindyear: 'yyyy（例 …_2025）',
    },
    tip: {
      archivekeycolumn:
        '归档键列：表中用于判断「属于哪一年」的物理列（如 costing_date）。归档预览/执行会按此列过滤该年数据，并迁入按键类型命名的归档表。请选择与业务年份语义一致的列。',
      archivekeykind:
        '归档键类型（字典 sys_archive_key_kind）：标准日期格式。1=yyyyMMddHHmmss → …_20251010101000；2=yyyyMM → …_202510；3=yyyy（默认）→ …_2025。归档名称为物理表名_格式码（如 takt_xxx_yyyy）。选列后会按列类型自动建议，仍可手工改。',
      retainhotyears:
        '热库保留年数：固定为 1，不可改。按年份归档时，当前年数据必须留在主表（热库）；仅允许归档「当前年-1」及更早。例：2026 只能归档≤2025。',
    },
    ensureyears: {
      title: '预建年表',
      years: '年份范围',
      yearstart: '起始年',
      yearend: '截止年',
      yearshint: '将创建 {表名}_2026 等形式的年表（已存在则跳过结构克隆）；起止年含端点，单次最多 30 年',
      execute: '创建年表',
      success: '年表已就绪',
      failed: '创建年表失败',
      emptyyears: '请选择年份范围',
      spantoolarge: '单次最多创建 30 个年表，请缩小年份范围',
      result: '已就绪：{tables}',
    },
  },
};
