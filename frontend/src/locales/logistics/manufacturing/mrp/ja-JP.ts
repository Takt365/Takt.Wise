// ========================================
// 项目名称：节拍智控·TaktWise
// 文件名称：ja-JP.ts
// 功能描述：製造 MRP ページ静的文案；参照キー logistics.manufacturing.mrp.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// ========================================

export default {
  page: {
    wizard: {
      title: 'MRP 実行ウィザード',
      step: {
        mps: 'MPS 選択',
        options: '実行パラメータ',
        run: '実行',
        preview: '明細プレビュー',
        publish: '公開',
      },
      mpsHint: 'MRP ヘッダが MPS に紐づいていることを確認してください。',
      bomType: 'BOM タイプ',
      maxBomLevel: '最大展開レベル',
      includePo: '未完了 PO を含む',
      includePlanned: '計画オーダーを含む',
      runReady: '「実行」で MRP（BOM 展開＋ネッティング）を開始します。',
      publishHint: '公開後、計画オーダーと購買計画が生成されます。',
      runSuccess: 'MRP 実行完了',
      publishSuccess: 'MRP を公開しました',
    },
    flow: {
      wizard: 'MRP ウィザード',
      trace: '需給トレース',
    },
    mpsFromMds: {
      title: 'MDS から MPS 生成',
      mds: '主需要計画 MDS',
      success: 'MPS を生成しました',
    },
    apsSchedule: {
      pickOrders: 'APS オーダー選択',
      scheduleSuccess: 'APS スケジュール完了',
      releaseSuccess: '製造指図を発行しました',
    },
    purchasePlan: {
      convertSuccess: '購買依頼へ変換しました',
    },
    plannedOrder: {
      releaseSuccess: 'APS へ解放しました',
    },
  },
};
