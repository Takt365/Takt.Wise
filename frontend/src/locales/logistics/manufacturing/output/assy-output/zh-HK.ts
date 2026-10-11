// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/output/assy-output
// 文件名称：zh-HK.ts
// 功能描述：logistics/manufacturing/output/assy-output 頁面靜態文案；引用鍵 logistics.manufacturing.output.assy-output.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    stdcapacityhint: '小時標準产能 = 直接人員 × 60 ÷ 標準工時(分鐘) × 標準生產稼動率(%)，由系統根據主表數據自動計算。',
    detailstdcapacityhint: '默認快照表頭小時標準产能；有報工工時時按「報工工時÷標準工時×稼動率」重算該行。',
    confirmminuteshint: '填寫場景：1. 同一時段混合生產；2. 清機；3. 無產出但需記錄損失時間（欠料、儀設、切換機種等）。',
    proddatelocked: '生產日期 {prodDate} 已鎖定（次月 {cutoffDay} 日之後不可新增或修改）。',
    proddateoutofrange: '生產日期超出可選範圍（每月 {cutoffDay} 日之後僅可選當月1日至今日；不可選擇今天之後的日期）。',
  },
};
