// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/output/assy-output
// 文件名称：ja-JP.ts
// 功能描述：logistics/manufacturing/output/assy-output ページ静的文案；引用键 logistics.manufacturing.output.assy-output.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    stdcapacityhint: '時間当たり標準产能 = 直接人员 × 60 ÷ 標準工時（分） × 人員標準稼働率（%）；主表データから自動計算。',
    detailstdcapacityhint: '既定で主表の時間当たり標準产能をスナップショット；報工工時>0 の場合はその行を報工工時÷標準工時×稼働率で再計算。',
    confirmminuteshint: '入力场景：(1) 同一時間帯の混合生産；(2) 清掃；(3) 产出なしで損失時間を記録（欠料・設備・機種切替等）。',
    proddatelocked: '生産日 {prodDate} はロック済み（翌月 {cutoffDay} 日以降は新規・変更不可）。',
    proddateoutofrange: '生産日が選択可能範囲外です（毎月 {cutoffDay} 日以降は当月1日から今日までのみ選択可）。',
  },
};
