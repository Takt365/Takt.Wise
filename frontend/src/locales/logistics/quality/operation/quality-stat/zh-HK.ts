// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/quality/operation/quality-stat
// 文件名称：zh-HK.ts
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：質量統計頁靜態文案；引用鍵 logistics.quality.operation.quality-stat.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '質量統計',
    periodRange: '期間',
    dimension: {
      iqc: '供應商',
      ipqc: '工序',
      fqc: '客戶',
    },
    dimensionName: {
      iqc: '供應商名稱',
      ipqc: '工序名稱',
      fqc: '客戶名稱',
    },
    selectPlantRequired: '請選擇工廠',
    selectPeriodRequired: '請選擇期間',
    exportSuccess: '清單匯出成功',
    exportFailed: '清單匯出失敗',
    exportEmpty: '暫無數據可匯出，請先查詢',
    tabs: {
      iqc: 'IQC',
      ipqc: 'IPQC',
      fqc: 'FQC',
    },
    columns: {
      plantCode: '工廠',
      orderCount: '檢驗單數',
      sampleQty: '抽樣數量',
      qualifiedQty: '合格數量',
      unqualifiedQty: '不合格數量',
      returnQty: '驗退數量',
      defectRatePercent: '不良率%',
      qualifiedRatePercent: '合格率%',
    },
  },
};
