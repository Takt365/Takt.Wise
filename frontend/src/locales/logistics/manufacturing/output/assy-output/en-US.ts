// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/output/assy-output
// 文件名称：en-US.ts
// 功能描述：logistics/manufacturing/output/assy-output page static copy; keys logistics.manufacturing.output.assy-output.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    stdcapacityhint: 'Hourly standard capacity = direct labor × 60 ÷ standard minutes (min) × personnel operation rate (%); calculated automatically from master data.',
    detailstdcapacityhint: 'Defaults to a snapshot of master hourly standard capacity; when confirm minutes > 0, recalculated as confirm minutes ÷ standard minutes × operation rate for that row.',
    confirmminuteshint: 'Enter when: (1) mixed production in the same time period; (2) cleaning; (3) no output but loss time must be recorded (shortage, equipment, changeover, etc.).',
    proddatelocked: 'Production date {prodDate} is locked (cannot create or edit after day {cutoffDay} of the following month).',
    proddateoutofrange: 'Production date is out of range (after day {cutoffDay} of each month, only dates from the 1st of the current month through today are selectable).',
  },
};
