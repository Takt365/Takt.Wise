// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/sales/so-price-scr-deviation
// 文件名称：zh-CN.ts
// 创建时间：2026-09-20
// 创建人：Takt365(Cursor AI)
// 功能描述：SO价偏离SCR页静态文案；引用键 logistics.sales.so-price-scr-deviation.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'SO价偏离SCR',
    periodRange: '订单期间',
    selectPlantRequired: '请选择工厂代码',
    selectPeriodRequired: '请选择订单期间',
    summary: '命中 {count} 行（无SCR {noScr} · 单价不在价目 {absent}；同生效周期含条件行/数量等级/价值等级）',
    export: '清单导出',
    exportSuccess: '清单导出成功',
    exportFailed: '清单导出失败',
    exportEmpty: '暂无数据可导出，请先查询',
    columns: {
      plantCode: '工厂',
      salesOrderCode: '销售订单',
      lineNumber: '行号',
      orderDate: '订单日期',
      pricingDate: '价格日期',
      customerCode: '客户',
      materialCode: '物料',
      currencyCode: '币种',
      orderQuantity: '数量',
      soNormalizedUnitPrice: 'SO折算单价',
      matchReason: '原因',
      scrValidFrom: 'SCR有效起',
      scrValidTo: 'SCR有效止',
      scrPricesText: '周期内销售条件',
      closestScrNormalizedUnitPrice: '最近SCR折算单价',
      deviationAmount: '偏离金额',
      deviationTrend: '方向',
    },
    reason: {
      noScr: '同周期无SCR',
      priceAbsent: '单价不在价目中',
    },
    trend: {
      up: '偏高',
      down: '偏低',
    },
  },
};
