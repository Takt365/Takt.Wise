// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/workflow/todo
// 文件名称：zh-HK.ts
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
      result: "審批結果",
    },
    node: {
      reject: {
        step: {
          label: "駁回到節點",
          placeholder: "請輸入目標節點 ID（可選）",
        },
      },
    },
    transfer: {
      to: {
        user: {
          label: "轉辦對象",
          placeholder: "請選擇轉辦對象",
        },
      },
    },
    add: {
      sign: {
        approvers: {
          label: "加簽人",
          placeholder: "請選擇加簽人",
        },
        type: {
          sequential: "順序加簽",
          all: "會簽（全部通過）",
          one: "或簽（一人通過）",
        },
      },
    },
    task: {
      approve: {
        action: "審批操作",
      },
    },
    cashier: {
      payout: {
        method: "付款方式",
        required: "請選擇付款方式",
        bank: "銀行轉賬",
        cash: "現金",
        repay: "沖抵借款",
      },
    },
  },
};
