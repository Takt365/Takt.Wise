// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/engineering-change/ec-stat
// 文件名称：zh-CN.ts
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：设变统计页静态文案；引用键 logistics.manufacturing.engineering-change.ec-stat.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '设变统计',
    issuePeriodRange: '发行期间',
    entryPeriodRange: '录入期间',
    ecCode: '设变单号',
    selectPlantRequired: '请选择工厂',
    selectPeriodRequired: '请选择发行期间或录入期间',
    exportSuccess: '清单导出成功',
    exportFailed: '清单导出失败',
    exportEmpty: '暂无数据可导出，请先查询',
    tabs: {
      pur: '采购课',
      pc: '生管课',
      iqc: '受检课',
      mc: '部管课',
      mtl: '物料',
      mfg2: '制造二课',
      mfg1: '制造一课',
      qc: '品管课',
      me: '制造技术课',
    },
    status: {
      all: '全部',
      done: '已实施',
      open: '未实施',
      me: {
        done: '已更新',
        open: '未更新',
      },
    },
    columns: {
      plantCode: '工厂',
      ecCode: '设变单号',
      lineCount: '行数',
      implementedCount: '已实施',
      pendingCount: '未实施',
      ratePercent: '实施率%',
      meImplementedCount: '已更新',
      tePendingCount: '未更新',
      teRatePercent: '更新率%',
    },
  },
};
