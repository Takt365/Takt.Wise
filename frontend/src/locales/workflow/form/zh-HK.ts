// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/workflow/form
// 文件名称：zh-HK.ts
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
      business: "業務表單",
      system: "系統表單",
      general: "通用表單",
    },
    type: {
      static: "靜態表單",
      custom: "自定義表單",
      dynamic: "動態表單",
    },
    version: {
      placeholder: "請輸入版本號",
    },
    data: {
      source: {
        placeholder: "請輸入數據源標識",
      },
      table: {
        placeholder: "請選擇數據表",
      },
    },
    entity: {
      table: {
        hint: "實體表字段將用於表單設計器",
      },
    },
    is: {
      datasource: {
        label: "綁定業務數據源",
        hint: "按物理表映射字段，並支持審批回寫",
      },
    },
    business: {
      binding: {
        title: "業務狀態與提交規則",
      },
      status: {
        column: {
          label: "業務狀態列",
          placeholder: "選擇或輸入蛇形列名（如 trip_status）",
        },
      },
    },
    status: {
      in: {
        progress: "審批中狀態值",
      },
      approved: "已通過狀態值",
      rejected: "已駁回狀態值",
      cancelled: "已撤銷狀態值",
    },
    submit: {
      allowed: {
        statuses: {
          label: "允許提交的狀態",
          placeholder: "輸入狀態值後回車（如 0、3）",
        },
      },
    },
    require: {
      data: {
        table: "請選擇數據表並加載字段",
      },
      form: {
        config: "請完成表單設計",
      },
    },
    publish: {
      success: "表單已發布",
    },
    disable: {
      success: "表單已停用",
    },
    load: {
      detail: {
        failed: "加載表單詳情失敗",
      },
      form: {
        config: {
          failed: "獲取表單配置失敗",
        },
      },
    },
    step: {
      form: {
        info: "表單信息",
        design: "表單設計",
      },
      data: {
        source: "數據源",
        table: {
          list: "數據表",
          loaded: "已獲取所有數據列項，下一步可還原表單",
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
