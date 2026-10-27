// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/dedicated-material
// 文件名称：zh-CN.ts
// 创建时间：2026-10-21
// 创建人：Takt365(Cursor AI)
// 功能描述：专用物料页静态文案；引用键 logistics.manufacturing.bom.dedicated-material.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '专用物料',
    costingMonth: '核算年月',
    costingMonthPlaceholder: 'yyyy-MM',
    material: '物料',
    materialOptional: '物料（可空）',
    modelOptional: '机种（可空）',
    hint: '先取该年月采购类型 F、生产相关 X、PCB SECT 标识为空的物料，再按成本头机种编码归类。只出现在一个机种的列为专用物料。',
    selectPlantRequired: '请选择工厂代码',
    selectPeriodRequired: '请选择核算年月',
    summary: '{plant} / {period} / 机种 {model} / 物料 {material} — 专用物料 {total} 行',
    exportSuccess: '专用物料导出成功',
    exportFailed: '专用物料导出失败',
    queryFailed: '专用物料查询失败',
    columns: {
      period: '年月',
      model: '机种',
      component: '物料',
      description: '物料描述',
      productCount: '产品数',
      usage: '用量清单',
      lineCost: '行成本',
    },
  },
}
