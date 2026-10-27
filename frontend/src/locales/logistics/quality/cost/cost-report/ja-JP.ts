// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/quality/cost/cost-report
// 文件名称：ja-JP.ts
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：品質コスト報告ページ文言；キー logistics.quality.cost.cost-report.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '品質コスト報告',
    periodRange: '期間',
    currencyCode: '通貨',
    dimension: {
      assurance: '記録グループ',
      issue: '機種',
      incident: '廃棄グループ',
    },
    dimensionName: {
      assurance: '記録グループ名',
      issue: '機種',
      incident: '廃棄グループ名',
    },
    quantity: {
      issue: '合計時間',
      incident: '廃棄数量',
    },
    selectPlantRequired: '工場を選択してください',
    selectPeriodRequired: '期間を選択してください',
    exportSuccess: '一覧出力に成功しました',
    exportFailed: '一覧出力に失敗しました',
    exportEmpty: '出力するデータがありません。先に検索してください',
    tabs: {
      assurance: '品質保証',
      issue: '品質問題',
      incident: '品質事故',
    },
    columns: {
      plantCode: '工場',
      currencyCode: '通貨',
      orderCount: '件数',
      amount: '金額',
    },
  },
};
