// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/material-zero-price
// 文件名称：ja-JP.ts
// 创建时间：2026-08-13
// 创建人：Takt365(Cursor AI)
// 功能描述：参照キー logistics.manufacturing.bom.material-zero-price.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    plantCode: '工場',
    selectPlantRequired: '工場を選択してください',
    costingMonth: '原価計算月',
    costingMonthPlaceholder: '原価計算月を選択',
    costNeedMonth: '原価計算月を選択してください',
    modelCode: '機種',
    modelCodesOptional: '機種（任意・複数可；空=全機種）',
    modelCodePlaceholder: '任意；空＝全機種',
    hint: '{month} · 製品 {productCount} · ゼロ価格部品 {componentCount}（FERT のみ · IsDeleted=0 · ProductionRelated=X · PcbSectIndicator 空 · PurchaseType=F · 移動平均価格=0；推奨代替＝末尾文字逆順例 E02597400C→B→A、移動価格表の同期間または直近有価÷PriceUnit）',
    productCodes: '共用製品',
    productCount: '製品数',
    suggestedComponentCode: '推奨代替部品',
    suggestedMovingPrice: '推奨移動価格',
    exportSuccess: 'Bom0価格のエクスポートに成功しました',
    exportFailed: 'Bom0価格のエクスポートに失敗しました',
    costSum: 'コスト計算',
    costRecalculate: '再計算コスト',
    costAverage: '平均コスト計算',
    purchaseInfoBackfill: '購買価格補完',
    purchaseInfoBackfillSuccess:
      '{month} 購買価格補完完了：スキャン {scanned}、更新 {updated}、価格なし {skipped}、変更なし {unchanged}',
    purchaseInfoBackfillFailed: '購買価格補完に失敗しました',
    movingPriceBackfill: '移動価格を補完',
    movingPriceBackfillBatch: '移動価格を一括補完',
    movingPriceBackfillRow: '移動価格を補完',
    movingPriceBackfillNoSuggested: '推奨代替部品がないため移動価格を補完できません',
    movingPriceBackfillConfirmTitle: '移動価格を補完しますか？',
    movingPriceBackfillConfirmContent:
      '現在の工場・原価月で部品 {component} のゼロ価格明細を推奨代替 {suggested} の移動平均価格／単位／通貨で補完し、ExtField 履歴を書き込み、各機種の製品月次原価と機種月次原価を更新します。',
    movingPriceBackfillBatchConfirmTitle: '移動価格を一括補完しますか？',
    movingPriceBackfillBatchConfirmContent:
      '現在の工場・原価月 {month}（および機種条件）で、推奨代替がある全ゼロ価格部品の移動平均価格／単位／通貨を補完し、ExtField 履歴と製品／機種月次原価を更新します。',
    movingPriceBackfillSuccess:
      '{month} 移動価格補完完了：明細スキャン {scanned}、更新 {updated}、価格なし {skipped}、変更なし {unchanged}；製品月次原価 {productCost}、機種月次原価 {modelAverage}；{priceInfo}',
    movingPriceBackfillBatchSuccess:
      '{month} 移動価格一括補完完了：部品 {components}、明細スキャン {scanned}、更新 {updated}、価格なし {skipped}、変更なし {unchanged}；製品月次原価 {productCost}、機種月次原価 {modelAverage}',
    movingPriceBackfillFailed: '移動価格補完に失敗しました',
    movingPriceManualRow: '手動で価格更新',
    movingPriceManualTitle: '移動価格を手動置換',
    movingPriceManualHint:
      '新部品の移動価格・単位・通貨を、工場＋原価月の当該部品の全製品明細へ回填し、関連する全機種の製品／機種月次原価を更新、ExtField に完全履歴を記録します。',
    movingPriceManualOriginal: '原部品',
    movingPriceManualReplace: '置換',
    movingPriceManualSourceComponent: '新部品',
    movingPriceManualSourceRequired: '置換する新部品コードを入力してください',
    movingPriceManualPrice: '移動価格',
    movingPriceManualPriceRequired: '0 より大きい移動価格を入力してください',
    movingPriceManualUnit: '価格単位',
    movingPriceManualCurrency: '通貨',
    movingPriceManualSuccess:
      '{month} 手動置換完了：原部品 {component} ← 新部品 {source}、スキャン {scanned}、更新 {updated}、変更なし {unchanged}；製品月次原価 {productCost}、機種月次原価 {modelAverage}；{priceInfo}',
    movingPriceManualFailed: '移動価格の手動置換に失敗しました',
    latestPurchaseCost: '最近購買原価を計算',
    latestPurchaseCostSuccess:
      '{month} 最近購買原価完了：スキャン {scanned}、更新 {refreshed}、スキップ {skipped}',
    latestPurchaseCostFailed: '最近購買原価の計算に失敗しました',
    costSumSubmitted: '{month} のバックグラウンド計算（全品目タイプ）を受付ました。完了後に通知します',
    costRecalculateSubmitted: '{month} の再計算（全品目タイプ；ゼロクリア後に再集計）を受付ました。完了後に通知します',
    costRecalculateCompleted: '{month} 処理完了（所要 {duration}、更新 {refreshed}、スキップ {skipped}）',
    costRecalculateFailed: 'コスト処理に失敗しました',
    costRecalculateConfirmTitle: 'コストを再計算しますか？',
    costRecalculateConfirmContent: '旧コストを拡張項目へ保存したうえで、当該原価月の全品目タイプを再集計します。',
    costAverageSuccess:
      '{month} 平均完了：スキャン {scanned}（製品月次原価>0は {positiveCostRows}）、機種更新 {modelUpdated}、タイプ更新 {typeUpdated}、平均更新 {averageUpdated}（原価あり {groupsWithCost}/{groups}、なし {groupsNoCost}）',
    costAverageFailed: '平均コスト計算に失敗しました',
    pcbSectMark: 'PCB SECT を標記',
    pcbSectMarkConfirmTitle: 'PCB SECT ツリー全体を標記しますか？',
    pcbSectMarkConfirmContent:
      '工場・原価月 {month}（および機種条件）で、説明に「PCB SECT」を含むノードとその子孫ツリーに pcb_sect_indicator=X を書き込みます（既存はスキップ）。',
    pcbSectMarkSuccess:
      '{month} PCB SECT 標記完了：スキャン {scanned}、ツリー {pcbSect}、新規 {updated}、既存 {unchanged}',
    pcbSectMarkFailed: 'PCB SECT 標記に失敗しました',
    stripLeadingZeros: '先頭0を除去',
    stripLeadingZerosConfirmTitle: '先頭の 0 を除去しますか？',
    stripLeadingZerosConfirmContent:
      '工場 {plant} の原価ヘッダの製品コード、および明細（年次表を含む）の製品コード・部品コードのうち、数字のみで先頭が 0 の値から先頭 0 を除きます。例：000000001918610130 → 1918610130。0919MH8G93 のように文字を含むコードは変更しません。一意キーが衝突する行はスキップします。',
    stripLeadingZerosSuccess:
      '先頭 0 の除去完了：ヘッダ製品 {headerProduct}、明細製品 {itemProduct}、明細部品 {itemComponent}、衝突スキップ {skipped}',
    stripLeadingZerosFailed: '先頭 0 の除去に失敗しました',
  },
}
