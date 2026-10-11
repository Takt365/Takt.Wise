// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/workflow/instance
// 文件名称：zh-HK.ts
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
      '0': "運行中",
      '1': "已完成",
      '2': "已駁回",
      '3': "已掛起",
      '4': "已終止",
      '5': "草稿",
      unknown: "未知狀態",
    },
    no: {
      history: "暫無流轉記錄",
    },
    task: {
      form: {
        content: "表單內容",
      },
    },
    form: {
      data: {
        empty: "（空）",
      },
    },
    suspend: {
      reason: {
        label: "掛起原因",
        placeholder: "請輸入掛起原因（選填）",
      },
    },
    terminate: {
      reason: {
        placeholder: "請輸入終止原因（選填）",
      },
    },
    confirm: {
      resume: "確定恢復流程「{name}」嗎？",
      revoke: "確定撤回流程「{name}」嗎？",
    },
    msg: {
      suspend: {
        success: "掛起成功",
      },
      resume: {
        success: "恢復成功",
      },
      terminate: {
        success: "終止成功",
      },
      reduce: {
        sign: {
          success: "減簽成功",
          fail: "減簽失敗",
        },
      },
    },
  },
};
