// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/output/prod-date
// 文件名称：en-US.ts
// 功能描述：Manufacturing output prod date rule copy; keys logistics.manufacturing.output.prod-date.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    proddatelocked: 'Production date {prodDate} is locked (cannot create or edit after day {cutoffDay} of the following month).',
    proddateoutofrange: 'Production date is out of range (after day {cutoffDay} of each month, only dates from the 1st of the current month through today are selectable).',
  },
};
