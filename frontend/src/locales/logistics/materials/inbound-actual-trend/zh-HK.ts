// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/materials/inbound-actual-trend
// 文件名称：zh-HK.ts
// 创建时间：2026-09-16
// 创建人：Takt365(Cursor AI)
// 功能描述：採購實績推移頁靜態文案；引用鍵 logistics.materials.inbound-actual-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '物料入庫推移',
    periodRange: '過賬年月',
    materialCode: '物料編碼',
    supplierCode: '供應商編碼',
    selectPlantRequired: '請選擇工廠代碼',
    selectPeriodRequired: '請選擇過賬年月',
    monthColumn: '月份',
    amountColumn: '採購份額（萬CNY）',
    impactColumn: '漲跌金額（萬CNY，單價差×數量）',
    summary: '共 {count} 個月（採購份額為當月合計萬CNY；漲跌=(本月單價−上月單價)×本月數量，漲跌拆開互不沖抵）',
    footerTotals: '合計 {amount}    漲價合計 {up}    跌價合計 {down}',
    adjacentVs: '相對 {period}：單價差 × 當月數量',
    unitPrice: '單價',
    qty: '數量',
    month: '{month}月',
    monthYear: '{year}年{month}月',
    total: '合計',
    upTotal: '漲價合計',
    downTotal: '跌價合計',
    detailSummary: '物料行 {count} 條（每月一行：金額/單價/數量/月份/漲跌；漲跌=(本月單價−上月單價)×本月數量；期間兩列可在列設置中顯隱，匯出隨當前可見列）',
    chartSummary: '共 {count} 個月（正軸=漲、負軸=跌，單位萬CNY；漲跌互不沖抵；子標題為選中期間）',
    tabs: {
      chart: '柱形圖',
      trend: '推移表',
      detail: '明細',
    },
    chart: {
      title: '月度漲跌',
      subtext: '期間：{period}，基準月{baseMonth}',
      seriesUp: '漲',
      seriesDown: '跌',
      seriesFlat: '持平',
      unitWanYuan: '萬CNY',
      net: '淨值',
      markMax: '最大值',
      markMin: '最小值',
    },
    columns: {
      amount: '金額',
      unitPrice: '單價',
      quantity: '數量',
      month: '月份',
      adjacent: '單價漲跌%',
      trend: '期間漲跌',
      varianceAmount: '期間單價影響金額',
    },
    export: '清單匯出',
    exportSuccess: '清單匯出成功',
    exportFailed: '清單匯出失敗',
    exportEmpty: '暫無數據可匯出，請先查詢',
    filter: {
      all: '全部',
      changed: '僅漲跌',
    },
    trend: {
      none: '—',
      up: '漲',
      down: '跌',
      flat: '平',
    },
  },
};
