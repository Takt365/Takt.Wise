// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/material-zero-price
// 文件名称：en-US.ts
// 创建时间：2026-08-13
// 创建人：Takt365(Cursor AI)
// 功能描述：keys logistics.manufacturing.bom.material-zero-price.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    plantCode: 'Plant',
    selectPlantRequired: 'Please select a plant',
    costingMonth: 'Costing month',
    costingMonthPlaceholder: 'Select costing month',
    costNeedMonth: 'Please select a costing month',
    modelCode: 'Model',
    modelCodesOptional: 'Models (optional, multi-select; empty = all)',
    modelCodePlaceholder: 'Optional; empty = all models',
    hint: '{month} · {productCount} products · {componentCount} zero-price components (FERT only · IsDeleted=0 · ProductionRelated=X · PcbSectIndicator empty · PurchaseType=F · moving avg = 0; suggested substitute = reverse trailing letter e.g. E02597400C→B→A from material_moving_price same/nearest period ÷ PriceUnit)',
    productCodes: 'Shared products',
    productCount: 'Products',
    suggestedComponentCode: 'Suggested component',
    suggestedMovingPrice: 'Suggested moving price',
    exportSuccess: 'Bom0 price exported',
    exportFailed: 'Failed to export Bom0 price',
    costSum: 'Calculate cost',
    costRecalculate: 'Recalculate cost',
    costAverage: 'Calculate average cost',
    purchaseInfoRecordBackfill: 'Backfill purchase price',
    purchaseInfoRecordBackfillSuccess:
      '{month} purchase-price backfill done: scanned {scanned}, updated {updated}, no price {skipped}, unchanged {unchanged}',
    purchaseInfoRecordBackfillFailed: 'Failed to backfill purchase price',
    movingPriceBackfill: 'Backfill moving price',
    movingPriceBackfillBatch: 'Batch backfill moving price',
    movingPriceBackfillRow: 'Backfill moving price',
    movingPriceBackfillNoSuggested: 'No suggested component; cannot backfill moving price',
    movingPriceBackfillConfirmTitle: 'Backfill moving price?',
    movingPriceBackfillConfirmContent:
      'For current plant/month, backfill zero-price lines of {component} from suggested {suggested} (moving avg / unit / currency), write ExtField history, and refresh product/model monthly costs.',
    movingPriceBackfillBatchConfirmTitle: 'Batch backfill moving price?',
    movingPriceBackfillBatchConfirmContent:
      'For current plant/month {month} (and model filter), backfill all zero-price components that have a suggested substitute, write ExtField history, and refresh product/model monthly costs.',
    movingPriceBackfillSuccess:
      '{month} moving-price backfill done: items scanned {scanned}, updated {updated}, no price {skipped}, unchanged {unchanged}; product monthly cost {productCost}, model monthly cost {modelAverage}; {priceInfo}',
    movingPriceBackfillBatchSuccess:
      '{month} batch moving-price backfill done: components {components}, items scanned {scanned}, updated {updated}, no price {skipped}, unchanged {unchanged}; product monthly cost {productCost}, model monthly cost {modelAverage}',
    movingPriceBackfillFailed: 'Failed to backfill moving price',
    movingPriceManualRow: 'Manual update price',
    movingPriceManualTitle: 'Manual replace moving price',
    movingPriceManualHint:
      'Copy the new component moving price/unit/currency onto all original item rows for this component in the plant+month, refresh product/model monthly costs on all related model headers, and write full ExtField history.',
    movingPriceManualOriginal: 'Original',
    movingPriceManualReplace: 'Replace with',
    movingPriceManualSourceComponent: 'New component',
    movingPriceManualSourceRequired: 'Enter the replacement component code',
    movingPriceManualPrice: 'Moving price',
    movingPriceManualPriceRequired: 'Enter a moving price greater than 0',
    movingPriceManualUnit: 'Price unit',
    movingPriceManualCurrency: 'Currency',
    movingPriceManualSuccess:
      '{month} manual replace done: {component} ← {source}, scanned {scanned}, updated {updated}, unchanged {unchanged}; product monthly cost {productCost}, model monthly cost {modelAverage}; {priceInfo}',
    movingPriceManualFailed: 'Failed to manually replace moving price',
    latestPurchaseCost: 'Calculate latest purchase cost',
    latestPurchaseCostSuccess:
      '{month} latest purchase cost done: scanned {scanned}, refreshed {refreshed}, skipped {skipped}',
    latestPurchaseCostFailed: 'Failed to calculate latest purchase cost',
    costSumSubmitted: 'Cost calculation for {month} submitted (all material types); you will be notified when done',
    costRecalculateSubmitted: 'Recalculation for {month} submitted (all material types; zero then rebuild); you will be notified when done',
    costRecalculateCompleted: '{month} completed ({duration}; refreshed {refreshed}, skipped {skipped})',
    costRecalculateFailed: 'Cost processing failed',
    costRecalculateConfirmTitle: 'Recalculate cost?',
    costRecalculateConfirmContent: 'Archive old cost to ExtField, then rebuild this costing month for all material types.',
    costAverageSuccess:
      '{month} average done: scanned {scanned} (product cost>0: {positiveCostRows}), model updated {modelUpdated}, type updated {typeUpdated}, average updated {averageUpdated} (with cost {groupsWithCost}/{groups}, no cost {groupsNoCost})',
    costAverageFailed: 'Failed to calculate average cost',
    pcbSectMark: 'Mark PCB SECT',
    pcbSectMarkConfirmTitle: 'Mark entire PCB SECT trees?',
    pcbSectMarkConfirmContent:
      'For plant/month {month} (and model filter), find nodes whose description contains "PCB SECT" and all descendants, then write pcb_sect_indicator=X (skip if already marked).',
    pcbSectMarkSuccess:
      '{month} PCB SECT mark done: scanned {scanned}, tree rows {pcbSect}, newly marked {updated}, already marked {unchanged}',
    pcbSectMarkFailed: 'Failed to mark PCB SECT',
    stripLeadingZeros: 'Strip leading zeros',
    stripLeadingZerosConfirmTitle: 'Strip leading zeros?',
    stripLeadingZerosConfirmContent:
      'For plant {plant}, strip leading zeros from pure-numeric product codes on the cost header, and from product and component codes on cost items (including yearly tables). 000000001918610130 becomes 1918610130. Codes with letters, such as 0919MH8G93, stay unchanged. Rows that would collide on a unique key are skipped.',
    stripLeadingZerosSuccess:
      'Leading zeros stripped: header products {headerProduct}, item products {itemProduct}, item components {itemComponent}, skipped conflicts {skipped}',
    stripLeadingZerosFailed: 'Failed to strip leading zeros',
  },
}
