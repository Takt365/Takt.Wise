// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/quality/cost/cost-report
// 文件名称：en-US.ts
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：Quality cost report page copy; keys logistics.quality.cost.cost-report.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：This software uses MIT License; the author assumes no liability for use.
// ========================================

export default {
  page: {
    title: 'Quality cost report',
    periodRange: 'Period',
    currencyCode: 'Currency',
    dimension: {
      assurance: 'Record group',
      issue: 'Model',
      incident: 'Scrap group',
    },
    dimensionName: {
      assurance: 'Record group name',
      issue: 'Model',
      incident: 'Scrap group name',
    },
    quantity: {
      issue: 'Total minutes',
      incident: 'Scrap qty',
    },
    selectPlantRequired: 'Please select a plant',
    selectPeriodRequired: 'Please select a period',
    exportSuccess: 'Export succeeded',
    exportFailed: 'Export failed',
    exportEmpty: 'No data to export. Run a query first.',
    tabs: {
      assurance: 'Quality assurance',
      issue: 'Quality issue',
      incident: 'Quality incident',
    },
    columns: {
      plantCode: 'Plant',
      currencyCode: 'Currency',
      orderCount: 'Documents',
      amount: 'Amount',
    },
  },
};
