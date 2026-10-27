// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/common-material
// 文件名称：zh-HK.ts
// 创建时间：2026-10-21
// 创建人：Takt365(Cursor AI)
// 功能描述：通用物料页静态文案；引用键 logistics.manufacturing.bom.common-material.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '通用物料',
    costingMonth: '核算年月',
    costingMonthPlaceholder: 'yyyy-MM',
    material: '物料',
    materialOptional: '物料（可空）',
    modelOptional: '機種（可空）',
    hint: '先取該年月採購類型 F、生產相關 X、PCB SECT 標識為空的物料，再按成本頭機種編碼歸類。出現在兩個及以上機種的列為通用物料。',
    selectPlantRequired: '請選擇工廠代碼',
    selectPeriodRequired: '請選擇核算年月',
    summary: '{plant} / {period} / 機種 {model} / 物料 {material} — 通用物料 {total} 行',
    exportSuccess: '通用物料導出成功',
    exportFailed: '通用物料導出失敗',
    queryFailed: '通用物料查詢失敗',
    columns: {
      period: '年月',
      model: '機種',
      component: '物料',
      description: '物料描述',
      productCount: '產品數',
      usage: '用量清單',
      lineCost: '行成本',
    },
  },
}
