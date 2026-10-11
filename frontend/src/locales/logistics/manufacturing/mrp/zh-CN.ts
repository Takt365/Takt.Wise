// ========================================
// 项目名称：节拍智控·TaktWise
// 文件名称：zh-CN.ts
// 功能描述：制造 MRP 页面静态文案；引用键 logistics.manufacturing.mrp.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// ========================================

export default {
  page: {
    wizard: {
      title: 'MRP 运算向导',
      step: {
        mps: '选择 MPS',
        options: '运算参数',
        run: '执行运算',
        preview: '预览明细',
        publish: '发布',
      },
      mpsHint: '请确认 MRP 已关联主生产计划（MPS），下一步可配置 BOM 展开与在途抵扣参数。',
      bomType: 'BOM 类型',
      maxBomLevel: '最大展开层级',
      includePo: '计入开放采购订单',
      includePlanned: '计入计划订单',
      runReady: '点击「运行」开始 MRP 运算（BOM 展开 + 净需求）。',
      publishHint: '发布后将生成自制计划订单/采购计划，MRP 状态变为已发布。',
      runSuccess: 'MRP 运算完成',
      publishSuccess: 'MRP 已发布',
    },
    flow: {
      wizard: 'MRP 向导',
      trace: '供需追溯',
    },
    mpsFromMds: {
      title: '从 MDS 生成 MPS',
      mds: '主需求计划 MDS',
      success: 'MPS 已生成',
    },
    apsSchedule: {
      pickOrders: '选择 APS 订单',
      scheduleSuccess: 'APS 排程完成',
      releaseSuccess: '生产工单已发布',
    },
    purchasePlan: {
      convertSuccess: '已转采购申请',
    },
    plannedOrder: {
      releaseSuccess: '已释放到 APS',
    },
  },
};
