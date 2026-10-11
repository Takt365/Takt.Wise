// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/workflow/scheme
// 文件名称：zh-CN.ts
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
        title: "关联表单",
        option: {
          link: "关联已有表单",
          new: "新建表单",
        },
        required: "请选择要关联的表单",
      },
    },
    select: {
      form: {
        placeholder: "请选择表单",
      },
    },
    no: {
      form: {
        hint: "未选择表单时可跳过，稍后在表单管理中维护",
      },
    },
    new: {
      form: {
        code: {
          label: "表单编码",
          placeholder: "如 trip_form",
        },
        name: {
          label: "表单名称",
          placeholder: "如 出差申请表",
        },
        required: "新建表单须填写编码与名称",
      },
    },
    form: {
      config: {
        required: "请设计关联表单内容",
      },
    },
    publish: {
      success: "方案已发布",
    },
    disable: {
      success: "方案已停用",
    },
    suspension: {
      active: "激活",
      suspended: "挂起",
    },
    designer: {
      label: {
        create: "流程设计",
        edit: "编辑流程设计",
      },
    },
    invalid: {
      process: {
        content: "流程设计内容无效，请检查节点与连线",
      },
    },
    load: {
      detail: {
        failed: "加载方案详情失败",
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
          form: "选择表单",
        },
      },
      step3: {
        flow: {
          design: "流程设计",
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
