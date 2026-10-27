// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/engineering-change/ec-stat
// 文件名称：ja-JP.ts
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：设变统计页静态文案；引用键 logistics.manufacturing.engineering-change.ec-stat.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '設変統計',
    issuePeriodRange: '発行期間',
    entryPeriodRange: '入力期間',
    ecCode: '設変番号',
    selectPlantRequired: '工場を選択してください',
    selectPeriodRequired: '発行期間または入力期間を選択してください',
    exportSuccess: 'エクスポートしました',
    exportFailed: 'エクスポートに失敗しました',
    exportEmpty: '先に照会してください',
    tabs: {
      pur: '購買課',
      pc: '生管課',
      iqc: '受入検査課',
      mc: '部品管理課',
      mtl: '電子材料',
      mfg2: '製造二課',
      mfg1: '製造一課',
      qc: '品管課',
      me: '製造技術課',
    },
    status: {
      all: 'すべて',
      done: '実施済',
      open: '未実施',
      me: {
        done: '更新済',
        open: '未更新',
      },
    },
    columns: {
      plantCode: '工場',
      ecCode: '設変番号',
      lineCount: '行数',
      implementedCount: '実施済',
      pendingCount: '未実施',
      ratePercent: '実施率%',
      meImplementedCount: '更新済',
      tePendingCount: '未更新',
      teRatePercent: '更新率%',
    },
  },
};
