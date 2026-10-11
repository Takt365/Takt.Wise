// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/procurement/po-price-pir-deviation
// 文件名称：zh-CN.ts
// 创建时间：2026-09-20
// 创建人：Takt365(Cursor AI)
// 功能描述：PO价偏离PIR页静态文案；引用键 logistics.procurement.po-price-pir-deviation.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'PO价偏离PIR',
    periodRange: '订单期间',
    selectPlantRequired: '请选择工厂代码',
    selectPeriodRequired: '请选择订单期间',
    summary: '命中 {count} 行（无PIR {noPir} · 偏离适用价 {absent}）',
    export: '清单导出',
    exportSuccess: '清单导出成功',
    exportFailed: '清单导出失败',
    exportEmpty: '暂无数据可导出，请先查询',
    columns: {
      plantCode: '工厂',
      purchaseOrderCode: '采购订单',
      lineNumber: '行号',
      orderDate: '订单日期',
      supplierCode: '供应商',
      materialCode: '物料编码',
      materialDescription: '物料描述',
      orderQuantity: '数量',
      poUnitPrice: 'PO单价',
      pirValidPeriod: 'PIR生效周期',
      pirPricesText: 'PIR价格',
      deviationAmount: '价差',
      deviationPercent: '价差比',
    },
    reason: {
      noPir: '同周期无PIR',
      priceAbsent: '偏离适用PIR价',
    },
    filter: {
      all: '全部',
      changed: '有涨跌',
    },
    trend: {
      up: '偏高',
      down: '偏低',
    },
  },
};
