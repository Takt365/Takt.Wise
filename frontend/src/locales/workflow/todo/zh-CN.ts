// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/workflow/todo
// 文件名称：zh-CN.ts
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
      result: "审批结果",
    },
    node: {
      reject: {
        step: {
          label: "驳回到节点",
          placeholder: "请输入目标节点 ID（可选）",
        },
      },
    },
    transfer: {
      to: {
        user: {
          label: "转办对象",
          placeholder: "请选择转办对象",
        },
      },
    },
    add: {
      sign: {
        approvers: {
          label: "加签人",
          placeholder: "请选择加签人",
        },
        type: {
          sequential: "顺序加签",
          all: "会签（全部通过）",
          one: "或签（一人通过）",
        },
      },
    },
    task: {
      approve: {
        action: "审批操作",
      },
    },
    cashier: {
      payout: {
        method: "付款方式",
        required: "请选择付款方式",
        bank: "银行转账",
        cash: "现金",
        repay: "冲抵借款",
      },
    },
  },
};
