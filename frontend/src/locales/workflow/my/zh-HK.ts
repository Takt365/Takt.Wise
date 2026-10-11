// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/workflow/my
// 文件名称：zh-HK.ts
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
        draft: "從草稿啟動",
      },
      flow: {
        form: {
          template: {
            list: {
              title: "選擇流程模板",
            },
          },
          process: {
            placeholder: "請選擇要發起的流程",
            required: "請選擇流程",
          },
          fill: {
            approval: {
              content: "填寫申請內容",
            },
          },
          applicant: {
            label: "申請人",
            placeholder: "請輸入申請人",
            required: "請填寫申請人",
          },
          title: {
            placeholder: "請輸入申請標題",
          },
          form: {
            data: {
              label: "表單數據",
            },
          },
          step3: {
            flow: {
              chart: "流程圖預覽",
            },
          },
          flow: {
            chart: {
              empty: "暫無流程圖",
            },
          },
          save: {
            draft: {
              label: "保存草稿",
              success: "草稿已保存，單號：{code}",
            },
          },
          submit: {
            label: "提交",
            success: "發起成功，單號：{code}",
          },
        },
      },
    },
    confirm: {
      start: {
        from: {
          draft: "確定從草稿啟動流程「{name}」嗎？",
        },
      },
    },
  },
};
