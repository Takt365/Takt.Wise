// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/workflow/instance
// 文件名称：en-US.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：workflow/instance page static copy; keys workflow.instance.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    status: {
      '0': "Running",
      '1': "Completed",
      '2': "Rejected",
      '3': "Suspended",
      '4': "Terminated",
      '5': "Draft",
      unknown: "Unknown",
    },
    no: {
      history: "No transition history",
    },
    task: {
      form: {
        content: "Form content",
      },
    },
    form: {
      data: {
        empty: "(empty)",
      },
    },
    suspend: {
      reason: {
        label: "Suspend reason",
        placeholder: "Enter suspend reason (optional)",
      },
    },
    terminate: {
      reason: {
        placeholder: "Enter terminate reason (optional)",
      },
    },
    confirm: {
      resume: "Resume process \"{name}\"?",
      revoke: "Revoke process \"{name}\"?",
    },
    msg: {
      suspend: {
        success: "Suspended successfully",
      },
      resume: {
        success: "Resumed successfully",
      },
      terminate: {
        success: "Terminated successfully",
      },
      reduce: {
        sign: {
          success: "Sign-off reduced successfully",
          fail: "Failed to reduce sign-off",
        },
      },
    },
  },
};
