// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/output/assy-output
// 文件名称：zh-CN.ts
// 功能描述：logistics/manufacturing/output/assy-output 页面静态文案；引用键 logistics.manufacturing.output.assy-output.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    stdcapacityhint: '小时标准产能 = 直接人员 × 60 ÷ 标准工时(分钟) × 标准生产稼动率(%)，由系统根据主表数据自动计算。',
    detailstdcapacityhint: '默认快照表头小时标准产能；有报工工时时按「报工工时÷标准工时×稼动率」重算该行。',
    confirmminuteshint: '填写场景：1. 同一时段混合生产；2. 清机；3. 无产出但需记录损失时间（欠料、仪设、切换机种等）。',
    proddatelocked: '生产日期 {prodDate} 已锁定（次月 {cutoffDay} 日之后不可新增或修改）',
    proddateoutofrange: '生产日期超出可选范围（每月 {cutoffDay} 日之后仅可选当月1日至今日；不可选择今天之后的日期）',
  },
};
