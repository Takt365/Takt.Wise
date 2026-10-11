// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/workflow/form
// 文件名称：en-US.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：workflow/form page static copy; keys workflow.form.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    category: {
      business: "Business form",
      system: "System form",
      general: "General form",
    },
    type: {
      static: "Static form",
      custom: "Custom form",
      dynamic: "Dynamic form",
    },
    version: {
      placeholder: "Enter version",
    },
    data: {
      source: {
        placeholder: "Enter data source",
      },
      table: {
        placeholder: "Select data table",
      },
    },
    entity: {
      table: {
        hint: "Entity table columns are used in the form designer",
      },
    },
    is: {
      datasource: {
        label: "Bind business data source",
        hint: "Maps physical table columns and enables approval write-back",
      },
    },
    business: {
      binding: {
        title: "Business status & submit rules",
      },
      status: {
        column: {
          label: "Business status column",
          placeholder: "Select or enter snake_case column (e.g. trip_status)",
        },
      },
    },
    status: {
      in: {
        progress: "In-progress status value",
      },
      approved: "Approved status value",
      rejected: "Rejected status value",
      cancelled: "Cancelled status value",
    },
    submit: {
      allowed: {
        statuses: {
          label: "Allowed submit statuses",
          placeholder: "Type a value and press Enter (e.g. 0, 3)",
        },
      },
    },
    require: {
      data: {
        table: "Select a data table and load columns",
      },
      form: {
        config: "Complete the form designer",
      },
    },
    publish: {
      success: "Form published",
    },
    disable: {
      success: "Form disabled",
    },
    load: {
      detail: {
        failed: "Failed to load form detail",
      },
      form: {
        config: {
          failed: "Failed to load form configuration",
        },
      },
    },
    step: {
      form: {
        info: "Form info",
        design: "Form design",
      },
      data: {
        source: "Data source",
        table: {
          list: "Data tables",
          loaded: "Columns loaded. Continue to restore the form.",
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
