// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/workflow/scheme
// 文件名称：en-US.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：workflow/scheme page static copy; keys workflow.scheme.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    link: {
      form: {
        title: "Linked form",
        option: {
          link: "Link existing form",
          new: "Create new form",
        },
        required: "Select a form to link",
      },
    },
    select: {
      form: {
        placeholder: "Select a form",
      },
    },
    no: {
      form: {
        hint: "You can skip and configure the form later",
      },
    },
    new: {
      form: {
        code: {
          label: "Form code",
          placeholder: "e.g. trip_form",
        },
        name: {
          label: "Form name",
          placeholder: "e.g. Trip request",
        },
        required: "Enter code and name for the new form",
      },
    },
    form: {
      config: {
        required: "Design the linked form content",
      },
    },
    publish: {
      success: "Scheme published",
    },
    disable: {
      success: "Scheme disabled",
    },
    suspension: {
      active: "Active",
      suspended: "Suspended",
    },
    designer: {
      label: {
        create: "Process design",
        edit: "Edit process design",
      },
    },
    invalid: {
      process: {
        content: "Invalid process design. Check nodes and edges.",
      },
    },
    load: {
      detail: {
        failed: "Failed to load scheme detail",
      },
    },
    step: {
      step1: {
        flow: {
          info: "Process info",
        },
      },
      step2: {
        select: {
          form: "Select form",
        },
      },
      step3: {
        flow: {
          design: "Process design",
        },
      },
      prev: "Previous",
      next: "Next",
      done: "Done",
      validate: {
        fail: "Step {step} validation failed",
      },
      complete: {
        required: "Complete all steps before saving",
      },
    },
  },
};
