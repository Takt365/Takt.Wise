// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/workflow/form
// 文件名称：ja-JP.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：workflow/form 页面静态文案；引用键 workflow.form.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    category: {
      business: "業務フォーム",
      system: "システムフォーム",
      general: "汎用フォーム",
    },
    type: {
      static: "静的フォーム",
      custom: "カスタムフォーム",
      dynamic: "動的フォーム",
    },
    version: {
      placeholder: "バージョンを入力",
    },
    data: {
      source: {
        placeholder: "データソースを入力",
      },
      table: {
        placeholder: "データテーブルを選択",
      },
    },
    entity: {
      table: {
        hint: "エンティティ列はフォーム設計で使用します",
      },
    },
    is: {
      datasource: {
        label: "業務データソースをバインド",
        hint: "物理テーブル列をマップし承認書き戻しを有効化",
      },
    },
    business: {
      binding: {
        title: "業務状態と提出ルール",
      },
      status: {
        column: {
          label: "業務状態列",
          placeholder: "蛇形列名を選択または入力（例 trip_status）",
        },
      },
    },
    status: {
      in: {
        progress: "承認中の状態値",
      },
      approved: "承認済み状態値",
      rejected: "却下状態値",
      cancelled: "取消状態値",
    },
    submit: {
      allowed: {
        statuses: {
          label: "提出を許可する状態",
          placeholder: "状態値を入力して Enter（例 0、3）",
        },
      },
    },
    require: {
      data: {
        table: "データテーブルを選択して列を読み込んでください",
      },
      form: {
        config: "フォーム設計を完了してください",
      },
    },
    publish: {
      success: "フォームを公開しました",
    },
    disable: {
      success: "フォームを無効化しました",
    },
    load: {
      detail: {
        failed: "フォーム詳細の読み込みに失敗しました",
      },
      form: {
        config: {
          failed: "フォーム設定の取得に失敗しました",
        },
      },
    },
    step: {
      form: {
        info: "フォーム情報",
        design: "フォーム設計",
      },
      data: {
        source: "データソース",
        table: {
          list: "データテーブル",
          loaded: "列項目を取得しました。次のステップでフォームを復元できます",
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
