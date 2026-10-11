// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/workflow/instance
// 文件名称：ja-JP.ts
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
      '0': "実行中",
      '1': "完了",
      '2': "却下",
      '3': "保留",
      '4': "終了",
      '5': "下書き",
      unknown: "不明",
    },
    no: {
      history: "履歴がありません",
    },
    task: {
      form: {
        content: "フォーム内容",
      },
    },
    form: {
      data: {
        empty: "（空）",
      },
    },
    suspend: {
      reason: {
        label: "保留理由",
        placeholder: "保留理由を入力（任意）",
      },
    },
    terminate: {
      reason: {
        placeholder: "終了理由を入力（任意）",
      },
    },
    confirm: {
      resume: "プロセス「{name}」を再開しますか？",
      revoke: "プロセス「{name}」を取り消しますか？",
    },
    msg: {
      suspend: {
        success: "保留しました",
      },
      resume: {
        success: "再開しました",
      },
      terminate: {
        success: "終了しました",
      },
      reduce: {
        sign: {
          success: "減签しました",
          fail: "減签に失敗しました",
        },
      },
    },
  },
};
