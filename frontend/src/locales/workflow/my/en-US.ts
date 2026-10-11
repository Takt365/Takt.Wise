// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/workflow/my
// 文件名称：en-US.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：workflow/my page static copy; keys workflow.my.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    start: {
      from: {
        draft: "Start from draft",
      },
      flow: {
        form: {
          template: {
            list: {
              title: "Select process template",
            },
          },
          process: {
            placeholder: "Select a process to start",
            required: "Please select a process",
          },
          fill: {
            approval: {
              content: "Fill application",
            },
          },
          applicant: {
            label: "Applicant",
            placeholder: "Enter applicant",
            required: "Please enter applicant",
          },
          title: {
            placeholder: "Enter application title",
          },
          form: {
            data: {
              label: "Form data",
            },
          },
          step3: {
            flow: {
              chart: "Flow chart preview",
            },
          },
          flow: {
            chart: {
              empty: "No flow chart",
            },
          },
          save: {
            draft: {
              label: "Save draft",
              success: "Draft saved. Code: {code}",
            },
          },
          submit: {
            label: "Submit",
            success: "Started successfully. Code: {code}",
          },
        },
      },
    },
    confirm: {
      start: {
        from: {
          draft: "Start process \"{name}\" from draft?",
        },
      },
    },
  },
};
