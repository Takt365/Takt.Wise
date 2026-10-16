// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/material-cost
// 文件名称：en-US.ts
// 创建时间：2026-07-14
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM material cost 3-level browse static copy; keys logistics.manufacturing.bom.material-cost.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    masterpanetitle: 'BOM material cost (model summary)',
    detailpanetitle: 'Product / Lines',
    productpanetitle: 'Product monthly cost',
    itempanetitle: 'BOM lines',
    selectmasterfirst: 'Please select a model summary row first',
    selectproductfirst: 'Please select a product row to view BOM lines',
    periodRange: 'Costing period',
    selectPlantRequired: 'Please select a plant',
    selectModelRequired: 'Please select a model',
    itemFilterHint: 'Default: ProductionRelated=X, PcbSectIndicator empty, PurchaseType=F (clear to show all)',
    productRowCount: 'Products',
    modalmasterhint: 'Left model → center product → right lines (no entity split). Import lines, then sum or recalculate cost.',
    costSum: 'Cost sum',
    costRecalculate: 'Recalculate cost',
    backfillPurchaseInfo: 'Backfill purchase price',
    refreshModelFields: 'Backfill model price',
    costingMonth: 'Costing month',
    costingMonthPlaceholder: 'Select costing month',
    processRecordCount: 'Records to process',
    processRecordCountHint: 'Counted by plant + product group; 0 = all, default 5000',
    processRecordCountInvalid: 'Records to process must be an integer >= 0',
    costNeedMonth: 'Please select a costing month',
    costSumSubmitted: 'Sum for {month} submitted; you will be notified when done',
    costRecalculateSubmitted: 'Recalculation for {month} submitted (zero then rebuild); you will be notified when done',
    costRecalculateCompleted: '{month} completed ({duration}; refreshed {refreshed}, skipped {skipped})',
    costRecalculateFailed: 'Cost processing failed',
    costRecalculateConfirmTitle: 'Recalculate cost?',
    costRecalculateConfirmContent: 'Zero existing totals for this costing month, then rebuild from item lines and refresh the summary.',
    backfillPurchaseInfoSuccess:
      '{month} backfill done: updated {updated}, skipped {skipped} (scanned {scanned})',
    backfillPurchaseInfoFailed: 'Failed to backfill purchase price',
    refreshModelFieldsSuccess:
      '{month} model price backfill done: scanned {scanned}, model updated {modelUpdated}, average updated {averageUpdated} ({groups} groups)',
    refreshModelFieldsFailed: 'Failed to backfill model price',
  },
};
