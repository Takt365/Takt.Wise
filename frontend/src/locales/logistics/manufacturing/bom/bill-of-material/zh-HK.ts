// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/bill-of-material
// 文件名称：zh-HK.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：logistics/manufacturing/bom/bill-of-material 页面静态文案；引用键 logistics.manufacturing.bom.bill-of-material.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    detail: {
      panel: {
        title: "BOM 明細（選中主表行後維護）",
      },
    },
    select: {
      master: {
        first: "請先選擇一條物料清單主表數據",
      },
    },
    modal: {
      cascade: {
        hint: "彈窗內「物料清單明細 / BOM 變更記錄」Tab 可與主表一次保存；底部面板可獨立維護明細行。",
      },
    },
    explosion: {
      title: "BOM 多層展開清單",
      quantity: "需求數量",
      maxLevel: "最大層級",
      includeLevelZero: "含父件行",
      summary: "BOM {bomCode} · 父件 {parentMaterialCode} {parentMaterialDescription} · 需求 {quantity}",
      column: {
        level: "層級",
        immediateParent: "直接父件",
        cumulativeQuantity: "累計需求量",
        hasChildBom: "有下級 BOM",
        isCircular: "循環引用",
      },
      action: "展開清單",
    },
  },
};
