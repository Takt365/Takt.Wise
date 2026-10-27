// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/engineering-change/ec-stat
// 文件名称：zh-HK.ts
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：设变统计页静态文案；引用键 logistics.manufacturing.engineering-change.ec-stat.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '設變統計',
    issuePeriodRange: '發行期間',
    entryPeriodRange: '錄入期間',
    ecCode: '設變單號',
    selectPlantRequired: '請選擇工廠',
    selectPeriodRequired: '請選擇發行期間或錄入期間',
    exportSuccess: '清單導出成功',
    exportFailed: '清單導出失敗',
    exportEmpty: '暫無數據可導出，請先查詢',
    tabs: {
      pur: '採購課',
      pc: '生管課',
      iqc: '受檢課',
      mc: '部管課',
      mtl: '物料',
      mfg2: '製造二課',
      mfg1: '製造一課',
      qc: '品管課',
      me: '製造技術課',
    },
    status: {
      all: '全部',
      done: '已實施',
      open: '未實施',
      me: {
        done: '已更新',
        open: '未更新',
      },
    },
    columns: {
      plantCode: '工廠',
      ecCode: '設變單號',
      lineCount: '行數',
      implementedCount: '已實施',
      pendingCount: '未實施',
      ratePercent: '實施率%',
      meImplementedCount: '已更新',
      tePendingCount: '未更新',
      teRatePercent: '更新率%',
    },
  },
};
