// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/materials/outbound-actual-trend
// 文件名称：zh-CN.ts
// 创建时间：2026-09-16
// 创建人：Takt365(Cursor AI)
// 功能描述：物料出库推移页静态文案；引用键 logistics.materials.outbound-actual-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '物料出库推移',
    periodRange: '过账年月',
    materialCode: '物料编码',
    customerCode: '客户编码',
    selectPlantRequired: '请选择工厂代码',
    selectPeriodRequired: '请选择过账年月',
    monthColumn: '月份',
    amountColumn: '出库份额（万CNY）',
    impactColumn: '涨跌金额（万CNY，单价差×数量）',
    summary: '共 {count} 个月（出库份额为当月合计万CNY；涨跌=(本月单价−上月单价)×本月数量，涨跌拆开互不冲抵）',
    footerTotals: '合计 {amount}    涨价合计 {up}    跌价合计 {down}',
    adjacentVs: '相对 {period}：单价差 × 当月数量',
    unitPrice: '单价',
    qty: '数量',
    month: '{month}月',
    monthYear: '{year}年{month}月',
    total: '合计',
    upTotal: '涨价合计',
    downTotal: '跌价合计',
    detailSummary: '物料行 {count} 条（每月一行：金额/单价/数量/月份/涨跌；涨跌=(本月单价−上月单价)×本月数量；期间两列可在列设置中显隐，导出随当前可见列）',
    chartSummary: '共 {count} 个月（正轴=涨、负轴=跌，单位万CNY；涨跌互不冲抵；子标题为选中期间）',
    tabs: {
      chart: '柱形图',
      trend: '推移表',
      detail: '明细',
    },
    chart: {
      title: '月度涨跌',
      subtext: '期间：{period}，基准月{baseMonth}',
      seriesUp: '涨',
      seriesDown: '跌',
      seriesFlat: '持平',
      unitWanYuan: '万CNY',
      net: '净值',
      markMax: '最大值',
      markMin: '最小值',
    },
    columns: {
      amount: '金额',
      unitPrice: '单价',
      quantity: '数量',
      month: '月份',
      adjacent: '单价涨跌%',
      trend: '期间涨跌',
      varianceAmount: '期间单价影响金额',
    },
    export: '清单导出',
    exportSuccess: '清单导出成功',
    exportFailed: '清单导出失败',
    exportEmpty: '暂无数据可导出，请先查询',
    filter: {
      all: '全部',
      changed: '仅涨跌',
    },
    trend: {
      none: '—',
      up: '涨',
      down: '跌',
      flat: '平',
    },
  },
};
