// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/quality/operation/quality-stat
// 文件名称：zh-CN.ts
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：质量统计页静态文案；引用键 logistics.quality.operation.quality-stat.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '质量统计',
    periodRange: '期间',
    dimension: {
      iqc: '供应商',
      ipqc: '工序',
      fqc: '客户',
    },
    dimensionName: {
      iqc: '供应商名称',
      ipqc: '工序名称',
      fqc: '客户名称',
    },
    selectPlantRequired: '请选择工厂',
    selectPeriodRequired: '请选择期间',
    exportSuccess: '清单导出成功',
    exportFailed: '清单导出失败',
    exportEmpty: '暂无数据可导出，请先查询',
    tabs: {
      iqc: 'IQC',
      ipqc: 'IPQC',
      fqc: 'FQC',
    },
    columns: {
      plantCode: '工厂',
      orderCount: '检验单数',
      sampleQty: '抽样数量',
      qualifiedQty: '合格数量',
      unqualifiedQty: '不合格数量',
      returnQty: '验退数量',
      defectRatePercent: '不良率%',
      qualifiedRatePercent: '合格率%',
    },
  },
};
