// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/output/prod-date
// 文件名称：zh-HK.ts
// 功能描述：製造產出生產日期規則靜態文案；引用鍵 logistics.manufacturing.output.prod-date.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    proddatelocked: '生產日期 {prodDate} 已鎖定（次月 {cutoffDay} 日之後不可新增或修改）。',
    proddateoutofrange: '生產日期超出可選範圍（每月 {cutoffDay} 日之後僅可選當月1日至今日；不可選擇今天之後的日期）。',
  },
};
