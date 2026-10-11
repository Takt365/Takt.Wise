// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/workflow/form
// 文件名称：zh-CN.ts
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
      business: "业务表单",
      system: "系统表单",
      general: "通用表单",
    },
    type: {
      static: "静态表单",
      custom: "自定义表单",
      dynamic: "动态表单",
    },
    version: {
      placeholder: "请输入版本号",
    },
    data: {
      source: {
        placeholder: "请输入数据源标识",
      },
      table: {
        placeholder: "请选择数据表",
      },
    },
    entity: {
      table: {
        hint: "实体表字段将用于表单设计器",
      },
    },
    is: {
      datasource: {
        label: "绑定业务数据源",
        hint: "绑定后按物理表映射字段，并支持审批回写",
      },
    },
    business: {
      binding: {
        title: "业务状态与提交规则",
      },
      status: {
        column: {
          label: "业务状态列",
          placeholder: "选择或输入蛇形列名（如 trip_status）",
        },
      },
    },
    status: {
      in: {
        progress: "审批中状态值",
      },
      approved: "已通过状态值",
      rejected: "已驳回状态值",
      cancelled: "已撤销状态值",
    },
    submit: {
      allowed: {
        statuses: {
          label: "允许提交的状态",
          placeholder: "输入状态值后回车（如 0、3）",
        },
      },
    },
    require: {
      data: {
        table: "请选择数据表并加载字段",
      },
      form: {
        config: "请完成表单设计",
      },
    },
    publish: {
      success: "表单已发布",
    },
    disable: {
      success: "表单已停用",
    },
    load: {
      detail: {
        failed: "加载表单详情失败",
      },
      form: {
        config: {
          failed: "获取表单配置失败",
        },
      },
    },
    step: {
      form: {
        info: "表单信息",
        design: "表单设计",
      },
      data: {
        source: "数据源",
        table: {
          list: "数据表",
          loaded: "已获取所有数据列项，下一步可还原表单",
        },
      },
      prev: "上一步",
      next: "下一步",
      done: "完成",
      validate: {
        fail: "第 {step} 步校验未通过",
      },
      complete: {
        required: "请完成全部步骤后再保存",
      },
    },
  },
};
