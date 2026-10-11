// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/workflow/scheme
// 文件名称：zh-HK.ts
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
        title: "關聯表單",
        option: {
          link: "關聯已有表單",
          new: "新建表單",
        },
        required: "請選擇要關聯的表單",
      },
    },
    select: {
      form: {
        placeholder: "請選擇表單",
      },
    },
    no: {
      form: {
        hint: "未選擇表單時可跳過，稍後在表單管理中維護",
      },
    },
    new: {
      form: {
        code: {
          label: "表單編碼",
          placeholder: "如 trip_form",
        },
        name: {
          label: "表單名稱",
          placeholder: "如 出差申請表",
        },
        required: "新建表單須填寫編碼與名稱",
      },
    },
    form: {
      config: {
        required: "請設計關聯表單內容",
      },
    },
    publish: {
      success: "方案已發布",
    },
    disable: {
      success: "方案已停用",
    },
    suspension: {
      active: "激活",
      suspended: "掛起",
    },
    designer: {
      label: {
        create: "流程設計",
        edit: "編輯流程設計",
      },
    },
    invalid: {
      process: {
        content: "流程設計內容無效，請檢查節點與連線",
      },
    },
    load: {
      detail: {
        failed: "加載方案詳情失敗",
      },
    },
    step: {
      step1: {
        flow: {
          info: "流程信息",
        },
      },
      step2: {
        select: {
          form: "選擇表單",
        },
      },
      step3: {
        flow: {
          design: "流程設計",
        },
      },
      prev: "上一步",
      next: "下一步",
      done: "完成",
      validate: {
        fail: "第 {step} 步校驗未通過",
      },
      complete: {
        required: "請完成全部步驟後再保存",
      },
    },
  },
};
