// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/workflow/scheme
// 文件名称：ja-JP.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：workflow/scheme 页面静态文案；引用键 workflow.scheme.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    link: {
      form: {
        title: "関連フォーム",
        option: {
          link: "既存フォームを関連",
          new: "新規フォーム",
        },
        required: "関連するフォームを選択してください",
      },
    },
    select: {
      form: {
        placeholder: "フォームを選択",
      },
    },
    no: {
      form: {
        hint: "未選択の場合は後でフォーム管理から設定できます",
      },
    },
    new: {
      form: {
        code: {
          label: "フォームコード",
          placeholder: "例 trip_form",
        },
        name: {
          label: "フォーム名",
          placeholder: "例 出張申請",
        },
        required: "新規フォームのコードと名称を入力してください",
      },
    },
    form: {
      config: {
        required: "フォーム内容を設計してください",
      },
    },
    publish: {
      success: "方案を公開しました",
    },
    disable: {
      success: "方案を無効化しました",
    },
    suspension: {
      active: "有効",
      suspended: "保留",
    },
    designer: {
      label: {
        create: "プロセス設計",
        edit: "プロセス設計を編集",
      },
    },
    invalid: {
      process: {
        content: "プロセス設計が無効です",
      },
    },
    load: {
      detail: {
        failed: "方案詳細の読み込みに失敗しました",
      },
    },
    step: {
      step1: {
        flow: {
          info: "プロセス情報",
        },
      },
      step2: {
        select: {
          form: "フォーム選択",
        },
      },
      step3: {
        flow: {
          design: "プロセス設計",
        },
      },
      prev: "前へ",
      next: "次へ",
      done: "完了",
      validate: {
        fail: "ステップ {step} の検証に失敗しました",
      },
      complete: {
        required: "保存前に全ステップを完了してください",
      },
    },
  },
};
