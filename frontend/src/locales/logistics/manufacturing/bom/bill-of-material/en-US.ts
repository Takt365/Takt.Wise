// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/bill-of-material
// 文件名称：en-US.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：logistics/manufacturing/bom/bill-of-material page static copy; keys logistics.manufacturing.bom.bill-of-material.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    detail: {
      panel: {
        title: "BOM items (maintain after selecting a header row)",
      },
    },
    select: {
      master: {
        first: "Please select a bill of material header row first",
      },
    },
    modal: {
      cascade: {
        hint: "Item / change log tabs in this dialog can be saved with the header; the bottom panel supports independent item CRUD.",
      },
    },
    explosion: {
      title: "Multi-level BOM explosion",
      quantity: "Demand quantity",
      maxLevel: "Max level",
      includeLevelZero: "Include parent row",
      summary: "BOM {bomCode} · Parent {parentMaterialCode} {parentMaterialDescription} · Qty {quantity}",
      column: {
        level: "Level",
        immediateParent: "Immediate parent",
        cumulativeQuantity: "Cumulative qty",
        hasChildBom: "Has child BOM",
        isCircular: "Circular",
      },
      action: "Explode BOM",
    },
  },
};
