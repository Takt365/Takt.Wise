// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/bill-of-material
// 文件名称：ja-JP.ts
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
        title: "BOM明細（ヘッダ行選択後に編集）",
      },
    },
    select: {
      master: {
        first: "先に部品表ヘッダ行を選択してください",
      },
    },
    modal: {
      cascade: {
        hint: "ダイアログ内の明細/変更履歴タブはヘッダと一括保存できます。下部パネルで明細を個別に編集できます。",
      },
    },
    explosion: {
      title: "多階層 BOM 展開一覧",
      quantity: "必要数量",
      maxLevel: "最大階層",
      includeLevelZero: "親行を含む",
      summary: "BOM {bomCode} · 親 {parentMaterialCode} {parentMaterialDescription} · 数量 {quantity}",
      column: {
        level: "階層",
        immediateParent: "直接親",
        cumulativeQuantity: "累計必要量",
        hasChildBom: "下位 BOM あり",
        isCircular: "循環参照",
      },
      action: "展開一覧",
    },
  },
};
