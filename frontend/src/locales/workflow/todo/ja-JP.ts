// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/workflow/todo
// 文件名称：ja-JP.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：workflow/todo 页面静态文案；引用键 workflow.todo.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    approve: {
      result: "承認結果",
    },
    node: {
      reject: {
        step: {
          label: "差し戻し先ノード",
          placeholder: "ノード ID（任意）",
        },
      },
    },
    transfer: {
      to: {
        user: {
          label: "転送先",
          placeholder: "転送先を選択",
        },
      },
    },
    add: {
      sign: {
        approvers: {
          label: "加签者",
          placeholder: "承認者を選択",
        },
        type: {
          sequential: "順次加签",
          all: "全会签",
          one: "或签",
        },
      },
    },
    task: {
      approve: {
        action: "承認操作",
      },
    },
    cashier: {
      payout: {
        method: "支払方法",
        required: "支払方法を選択してください",
        bank: "銀行振込",
        cash: "現金",
        repay: "借入相殺",
      },
    },
  },
};
