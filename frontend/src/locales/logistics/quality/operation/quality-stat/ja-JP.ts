// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/quality/operation/quality-stat
// 文件名称：ja-JP.ts
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：品質統計ページ文言；キー logistics.quality.operation.quality-stat.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '品質統計',
    periodRange: '期間',
    dimension: {
      iqc: '仕入先',
      ipqc: '工程',
      fqc: '顧客',
    },
    dimensionName: {
      iqc: '仕入先名',
      ipqc: '工程名',
      fqc: '顧客名',
    },
    selectPlantRequired: '工場を選択してください',
    selectPeriodRequired: '期間を選択してください',
    exportSuccess: '一覧出力に成功しました',
    exportFailed: '一覧出力に失敗しました',
    exportEmpty: '出力するデータがありません。先に検索してください',
    tabs: {
      iqc: 'IQC',
      ipqc: 'IPQC',
      fqc: 'FQC',
    },
    columns: {
      plantCode: '工場',
      orderCount: '検査件数',
      sampleQty: '抜取数',
      qualifiedQty: '合格数',
      unqualifiedQty: '不合格数',
      returnQty: '返品数',
      defectRatePercent: '不良率%',
      qualifiedRatePercent: '合格率%',
    },
  },
};
