// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/material-zero-price
// 文件名称：zh-HK.ts
// 创建时间：2026-08-13
// 创建人：Takt365(Cursor AI)
// 功能描述：引用鍵 logistics.manufacturing.bom.material-zero-price.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    plantCode: '工廠',
    selectPlantRequired: '請選擇工廠',
    costingMonth: '核算月',
    costingMonthPlaceholder: '選擇核算月',
    costNeedMonth: '請選擇核算月',
    modelCode: '機種',
    modelCodesOptional: '機種（可選，可多選；空=全部機種）',
    modelCodePlaceholder: '可選；空＝全部機種',
    hint: '{month} · 產品 {productCount} · 零價組件 {componentCount}（僅 FERT · IsDeleted=0 · ProductionRelated=X · PcbSectIndicator 空 · PurchaseType=F · 移動平均價=0；建議代替＝末尾字母逆推如 E02597400C→B→A，取移動價格表同期間或以前最近有價÷PriceUnit）',
    productCodes: '共用產品',
    productCount: '產品數',
    suggestedComponentCode: '建議代替組件',
    suggestedMovingPrice: '建議移動價格',
    exportSuccess: 'Bom0價格匯出成功',
    exportFailed: 'Bom0價格匯出失敗',
    costSum: '計算成本',
    costRecalculate: '重算成本',
    costAverage: '計算平均成本',
    purchaseInfoRecordBackfill: '回填採購價',
    purchaseInfoRecordBackfillSuccess:
      '{month} 回填採購價完成：掃描 {scanned} 行，更新 {updated}，無價格 {skipped}，未變化 {unchanged}',
    purchaseInfoRecordBackfillFailed: '回填採購價失敗',
    movingPriceBackfill: '回填移動價格',
    movingPriceBackfillBatch: '批量回填移動價格',
    movingPriceBackfillRow: '回填移動價格',
    movingPriceBackfillNoSuggested: '無建議代替組件，無法回填移動價格',
    movingPriceBackfillConfirmTitle: '確認回填移動價格？',
    movingPriceBackfillConfirmContent:
      '將按當前工廠與核算月，把組件 {component} 的零價明細回填為建議代替 {suggested} 的移動平均價／單位／貨幣，寫入 ExtField 履歷，並更新各機種產品月成本與機種月成本。',
    movingPriceBackfillBatchConfirmTitle: '確認批量回填移動價格？',
    movingPriceBackfillBatchConfirmContent:
      '將按當前工廠與核算月 {month}（及機種條件）對全部有建議代替的零價組件回填移動平均價／單位／貨幣，寫入 ExtField 履歷，並更新各機種產品月成本與機種月成本。',
    movingPriceBackfillSuccess:
      '{month} 回填移動價格完成：明細掃描 {scanned}、更新 {updated}、無價格 {skipped}、未變化 {unchanged}；產品月成本 {productCost}、機種月成本 {modelAverage}；{priceInfo}',
    movingPriceBackfillBatchSuccess:
      '{month} 批量回填移動價格完成：組件 {components}、明細掃描 {scanned}、更新 {updated}、無價格 {skipped}、未變化 {unchanged}；產品月成本 {productCost}、機種月成本 {modelAverage}',
    movingPriceBackfillFailed: '回填移動價格失敗',
    movingPriceManualRow: '手工更新價格',
    movingPriceManualTitle: '手工替換更新移動價格',
    movingPriceManualHint:
      '將新組件的移動價格、價格單位、幣種回填到原組件明細（工廠+核算月下該組件全部產品行），並同步更新各機種主表產品月成本與機種月成本，ExtField 記錄完整履歷。',
    movingPriceManualOriginal: '原組件',
    movingPriceManualReplace: '替換',
    movingPriceManualSourceComponent: '新組件',
    movingPriceManualSourceRequired: '請輸入替換新組件編碼',
    movingPriceManualPrice: '移動價格',
    movingPriceManualPriceRequired: '請輸入大於 0 的移動價格',
    movingPriceManualUnit: '價格單位',
    movingPriceManualCurrency: '幣種',
    movingPriceManualSuccess:
      '{month} 手工替換完成：原組件 {component} ← 新組件 {source}，明細掃描 {scanned}、更新 {updated}、未變化 {unchanged}；產品月成本 {productCost}、機種月成本 {modelAverage}；{priceInfo}',
    movingPriceManualFailed: '手工替換更新移動價格失敗',
    latestPurchaseCost: '計算最近採購成本',
    latestPurchaseCostSuccess:
      '{month} 計算最近採購成本完成：掃描 {scanned} 行，刷新 {refreshed} 組，跳過 {skipped} 組',
    latestPurchaseCostFailed: '計算最近採購成本失敗',
    costSumSubmitted: '已提交 {month} 後台計算成本（全部物料類型），完成後將通知您',
    costRecalculateSubmitted: '已提交 {month} 後台重算（全部物料類型；先歸零再匯總），完成後將通知您',
    costRecalculateCompleted: '{month} 處理完成（耗時 {duration}，刷新 {refreshed} 組，跳過 {skipped} 組）',
    costRecalculateFailed: '成本處理失敗',
    costRecalculateConfirmTitle: '確認重算成本？',
    costRecalculateConfirmContent: '將把舊成本寫入擴展欄位後，按該核算月全部物料類型重算成本。',
    costAverageSuccess:
      '{month} 計算平均成本完成：掃描 {scanned} 行（產品月成本>0共 {positiveCostRows}），機種更新 {modelUpdated}，類型更新 {typeUpdated}，月均更新 {averageUpdated}（有成本組 {groupsWithCost}/{groups}，無成本組 {groupsNoCost}）',
    costAverageFailed: '計算平均成本失敗',
    pcbSectMark: '標記 PCB SECT',
    pcbSectMarkConfirmTitle: '確認標記 PCB SECT 整樹？',
    pcbSectMarkConfirmContent:
      '將按當前工廠與核算月 {month}（及機種條件）識別組件描述含「PCB SECT」的節點及其子層級整樹，在明細 pcb_sect_indicator 寫入 X（已有標識跳過）。',
    pcbSectMarkSuccess:
      '{month} PCB SECT 打標完成：掃描 {scanned}，整樹 {pcbSect}，新標 {updated}，已有 {unchanged}',
    pcbSectMarkFailed: 'PCB SECT 打標失敗',
    stripLeadingZeros: '去前導0',
    stripLeadingZerosConfirmTitle: '確認去掉前導 0？',
    stripLeadingZerosConfirmContent:
      '將當前工廠 {plant} 的成本匯總表產品編碼，以及明細表（含各年分表）的產品編碼、組件編碼中，純數字且帶前導 0 的值去掉前導 0。例如 000000001918610130 變為 1918610130。含字母的編碼（如 0919MH8G93）不改。去掉後與已有唯一鍵衝突的行會跳過。',
    stripLeadingZerosSuccess:
      '去前導 0 完成：匯總產品 {headerProduct}，明細產品 {itemProduct}，明細組件 {itemComponent}，衝突跳過 {skipped}',
    stripLeadingZerosFailed: '去前導 0 失敗',
  },
}
