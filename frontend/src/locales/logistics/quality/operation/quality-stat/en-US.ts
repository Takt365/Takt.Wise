// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/quality/operation/quality-stat
// 文件名称：en-US.ts
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：Quality statistics page copy; keys logistics.quality.operation.quality-stat.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：This software uses MIT License; the author assumes no liability for use.
// ========================================

export default {
  page: {
    title: 'Quality statistics',
    periodRange: 'Period',
    dimension: {
      iqc: 'Supplier',
      ipqc: 'Process',
      fqc: 'Customer',
    },
    dimensionName: {
      iqc: 'Supplier name',
      ipqc: 'Process name',
      fqc: 'Customer name',
    },
    selectPlantRequired: 'Please select a plant',
    selectPeriodRequired: 'Please select a period',
    exportSuccess: 'Export succeeded',
    exportFailed: 'Export failed',
    exportEmpty: 'No data to export. Run a query first.',
    tabs: {
      iqc: 'IQC',
      ipqc: 'IPQC',
      fqc: 'FQC',
    },
    columns: {
      plantCode: 'Plant',
      orderCount: 'Orders',
      sampleQty: 'Sample qty',
      qualifiedQty: 'Qualified qty',
      unqualifiedQty: 'Unqualified qty',
      returnQty: 'Return qty',
      defectRatePercent: 'Defect %',
      qualifiedRatePercent: 'Qualified %',
    },
  },
};
