// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/workflow/instance
// 文件名称：zh-CN.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：workflow/instance 页面静态文案；引用键 workflow.instance.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    status: {
      '0': "运行中",
      '1': "已完成",
      '2': "已驳回",
      '3': "已挂起",
      '4': "已终止",
      '5': "草稿",
      unknown: "未知状态",
    },
    no: {
      history: "暂无流转记录",
    },
    task: {
      form: {
        content: "表单内容",
      },
    },
    form: {
      data: {
        empty: "（空）",
      },
    },
    suspend: {
      reason: {
        label: "挂起原因",
        placeholder: "请输入挂起原因（选填）",
      },
    },
    terminate: {
      reason: {
        placeholder: "请输入终止原因（选填）",
      },
    },
    confirm: {
      resume: "确定恢复流程「{name}」吗？",
      revoke: "确定撤回流程「{name}」吗？",
    },
    msg: {
      suspend: {
        success: "挂起成功",
      },
      resume: {
        success: "恢复成功",
      },
      terminate: {
        success: "终止成功",
      },
      reduce: {
        sign: {
          success: "减签成功",
          fail: "减签失败",
        },
      },
    },
  },
};
