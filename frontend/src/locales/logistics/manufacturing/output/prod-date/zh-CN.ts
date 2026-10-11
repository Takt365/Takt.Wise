// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/output/prod-date
// 文件名称：zh-CN.ts
// 功能描述：制造产出生产日期规则静态文案；引用键 logistics.manufacturing.output.prod-date.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    proddatelocked: '生产日期 {prodDate} 已锁定（次月 {cutoffDay} 日之后不可新增或修改）',
    proddateoutofrange: '生产日期超出可选范围（每月 {cutoffDay} 日之后仅可选当月1日至今日；不可选择今天之后的日期）',
  },
};
