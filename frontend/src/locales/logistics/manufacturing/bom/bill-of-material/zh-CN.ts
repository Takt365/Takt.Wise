// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/bill-of-material
// 文件名称：zh-CN.ts
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
        title: "BOM 明细（选中主表行后维护）",
      },
    },
    select: {
      master: {
        first: "请先选择一条物料清单主表数据",
      },
    },
    modal: {
      cascade: {
        hint: "弹窗内「物料清单明细 / BOM 变更记录」Tab 可与主表一次保存；底部面板可独立维护明细行。",
      },
    },
    explosion: {
      title: "BOM 多层展开清单",
      quantity: "需求数量",
      maxLevel: "最大层级",
      includeLevelZero: "含父件行",
      summary: "BOM {bomCode} · 父件 {parentMaterialCode} {parentMaterialDescription} · 需求 {quantity}",
      column: {
        level: "层级",
        immediateParent: "直接父件",
        cumulativeQuantity: "累计需求量",
        hasChildBom: "有下级 BOM",
        isCircular: "循环引用",
      },
      action: "展开清单",
    },
  },
};
