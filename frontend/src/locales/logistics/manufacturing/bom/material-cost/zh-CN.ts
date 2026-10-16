// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/material-cost
// 文件名称：zh-CN.ts
// 创建时间：2026-07-14
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 物料成本三层浏览静态文案；引用键 logistics.manufacturing.bom.material-cost.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    masterpanetitle: 'BOM物料成本（机种汇总）',
    detailpanetitle: '产品 / 明细',
    productpanetitle: '产品月成本',
    itempanetitle: 'BOM 明细',
    selectmasterfirst: '请先选择一条机种汇总行',
    selectproductfirst: '请先选择一条产品行以查看 BOM 明细',
    periodRange: '核算年月',
    selectPlantRequired: '请选择工厂代码',
    selectModelRequired: '请选择机种',
    itemFilterHint: '默认：生产相关=X、PCB SECT 标识空、采购类型=F（可清空查看全部）',
    productRowCount: '产品数',
    modalmasterhint: '左机种 → 中产品 → 右明细（不拆实体）；请导入明细后合计或重算成本。',
    costSum: '成本合计',
    costRecalculate: '重算成本',
    backfillPurchaseInfo: '回填采购价格',
    refreshModelFields: '回填机种价格',
    costingMonth: '核算月份',
    costingMonthPlaceholder: '请选择核算月份',
    processRecordCount: '处理记录数',
    processRecordCountHint: '按工厂+产品组计数；0 表示全部，默认 5000',
    processRecordCountInvalid: '处理记录数须为大于等于 0 的整数',
    costNeedMonth: '请选择核算月份',
    costSumSubmitted: '已提交 {month} 后台合计，完成后将通知您',
    costRecalculateSubmitted: '已提交 {month} 后台重算（先归零再汇总），完成后将通知您',
    costRecalculateCompleted: '{month} 处理完成（耗时 {duration}，刷新 {refreshed} 组，跳过 {skipped} 组）',
    costRecalculateFailed: '成本处理失败',
    costRecalculateConfirmTitle: '确认重算成本？',
    costRecalculateConfirmContent: '将先归零再按明细重算该核算月成本，完成后刷新汇总。',
    backfillPurchaseInfoSuccess: '{month} 回填完成：更新 {updated} 行，跳过 {skipped} 行（扫描 {scanned} 行）',
    backfillPurchaseInfoFailed: '回填采购价格失败',
    refreshModelFieldsSuccess:
      '{month} 回填机种价格完成：扫描 {scanned} 行，机种更新 {modelUpdated}，平均更新 {averageUpdated}（{groups} 组）',
    refreshModelFieldsFailed: '回填机种价格失败',
  },
};
