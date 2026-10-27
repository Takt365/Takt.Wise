// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/quality/cost/cost-report
// 文件名称：zh-CN.ts
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：品质成本报告页静态文案；引用键 logistics.quality.cost.cost-report.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '品质成本报告',
    periodRange: '期间',
    currencyCode: '币种',
    dimension: {
      assurance: '记录组',
      issue: '机种',
      incident: '废弃组',
    },
    dimensionName: {
      assurance: '记录组名称',
      issue: '机种',
      incident: '废弃组名称',
    },
    quantity: {
      issue: '总时间',
      incident: '废弃数量',
    },
    selectPlantRequired: '请选择工厂',
    selectPeriodRequired: '请选择期间',
    exportSuccess: '清单导出成功',
    exportFailed: '清单导出失败',
    exportEmpty: '暂无数据可导出，请先查询',
    tabs: {
      assurance: '品质保证',
      issue: '品质问题',
      incident: '品质事故',
    },
    columns: {
      plantCode: '工厂',
      currencyCode: '币种',
      orderCount: '单据数',
      amount: '金额',
    },
  },
};
