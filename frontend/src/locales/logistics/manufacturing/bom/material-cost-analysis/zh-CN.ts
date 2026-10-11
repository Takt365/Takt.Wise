// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/material-cost-analysis
// 文件名称：zh-CN.ts
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 物料成本分析页静态文案；引用键 logistics.manufacturing.bom.material-cost-analysis.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'BOM成本分析',
    periodRange: '核算期间',
    selectPlantRequired: '请选择工厂代码',
    selectMaterialTypeRequired: '请选择物料类型',
    selectPeriodRequired: '请选择核算期间',
    queryFailed: 'BOM成本分析查询失败',
    exportSuccess: 'BOM成本分析导出成功',
    exportFailed: 'BOM成本分析导出失败',
    filter: {
      all: '全部',
      changed: '仅涨跌',
    },
    sort: {
      productCode: '产品编码（全表）',
      trend: '涨跌优先（全表）',
      varianceDesc: '差额绝对值降序（全表）',
    },
    columns: {
      trend: '涨跌',
      varianceAmount: '环比差额',
      variancePercent: '环比%',
    },
    trend: {
      none: '—',
      up: '涨',
      down: '跌',
      flat: '平',
    },
  },
};
