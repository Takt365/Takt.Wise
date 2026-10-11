// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/output/prod-date
// 文件名称：ja-JP.ts
// 功能描述：製造产出 生産日ルール文案；引用键 logistics.manufacturing.output.prod-date.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    proddatelocked: '生産日 {prodDate} はロック済み（翌月 {cutoffDay} 日以降は新規・変更不可）。',
    proddateoutofrange: '生産日が選択可能範囲外です（毎月 {cutoffDay} 日以降は当月1日から今日までのみ選択可）。',
  },
};
