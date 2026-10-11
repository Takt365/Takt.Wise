// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/workflow/todo
// 文件名称：en-US.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：workflow/todo page static copy; keys workflow.todo.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    approve: {
      result: "Approval result",
    },
    node: {
      reject: {
        step: {
          label: "Reject to node",
          placeholder: "Target node ID (optional)",
        },
      },
    },
    transfer: {
      to: {
        user: {
          label: "Transfer to",
          placeholder: "Select assignee",
        },
      },
    },
    add: {
      sign: {
        approvers: {
          label: "Additional approvers",
          placeholder: "Select approvers",
        },
        type: {
          sequential: "Sequential",
          all: "All must approve",
          one: "Any one approves",
        },
      },
    },
    task: {
      approve: {
        action: "Approval actions",
      },
    },
    cashier: {
      payout: {
        method: "Payout method",
        required: "Please select payout method",
        bank: "Bank transfer",
        cash: "Cash",
        repay: "Offset loan",
      },
    },
  },
};
