// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/workflow/my
// 文件名称：ja-JP.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：workflow/my 页面静态文案；引用键 workflow.my.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    start: {
      from: {
        draft: "下書きから開始",
      },
      flow: {
        form: {
          template: {
            list: {
              title: "テンプレートを選択",
            },
          },
          process: {
            placeholder: "開始するプロセスを選択",
            required: "プロセスを選択してください",
          },
          fill: {
            approval: {
              content: "申請内容を入力",
            },
          },
          applicant: {
            label: "申請者",
            placeholder: "申請者を入力",
            required: "申請者を入力してください",
          },
          title: {
            placeholder: "申請タイトルを入力",
          },
          form: {
            data: {
              label: "フォームデータ",
            },
          },
          step3: {
            flow: {
              chart: "フロー図プレビュー",
            },
          },
          flow: {
            chart: {
              empty: "フロー図がありません",
            },
          },
          save: {
            draft: {
              label: "下書き保存",
              success: "下書きを保存しました。番号：{code}",
            },
          },
          submit: {
            label: "提出",
            success: "開始しました。番号：{code}",
          },
        },
      },
    },
    confirm: {
      start: {
        from: {
          draft: "下書きからプロセス「{name}」を開始しますか？",
        },
      },
    },
  },
};
