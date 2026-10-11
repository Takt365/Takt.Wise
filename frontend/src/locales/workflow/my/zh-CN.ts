// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/workflow/my
// 文件名称：zh-CN.ts
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
        draft: "从草稿启动",
      },
      flow: {
        form: {
          template: {
            list: {
              title: "选择流程模板",
            },
          },
          process: {
            placeholder: "请选择要发起的流程",
            required: "请选择流程",
          },
          fill: {
            approval: {
              content: "填写申请内容",
            },
          },
          applicant: {
            label: "申请人",
            placeholder: "请输入申请人",
            required: "请填写申请人",
          },
          title: {
            placeholder: "请输入申请标题",
          },
          form: {
            data: {
              label: "表单数据",
            },
          },
          step3: {
            flow: {
              chart: "流程图预览",
            },
          },
          flow: {
            chart: {
              empty: "暂无流程图",
            },
          },
          save: {
            draft: {
              label: "保存草稿",
              success: "草稿已保存，单号：{code}",
            },
          },
          submit: {
            label: "提交",
            success: "发起成功，单号：{code}",
          },
        },
      },
    },
    confirm: {
      start: {
        from: {
          draft: "确定从草稿启动流程「{name}」吗？",
        },
      },
    },
  },
};
