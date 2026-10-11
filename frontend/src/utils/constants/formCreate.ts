// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils/constants
// 文件名称：formCreate.ts
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：@form-create/ant-design-vue 全局默认 option（预览/只读场景复用）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * form-create 默认 option：隐藏提交/重置，水平布局与 Ant Design Vue 表单一致
 */
export const FORM_CREATE_DEFAULT_OPTION: Readonly<Record<string, unknown>> = Object.freeze({
  form: {
    layout: 'horizontal',
    labelAlign: 'right',
    labelCol: { span: 6 },
    wrapperCol: { span: 18 }
  },
  submitBtn: false,
  resetBtn: false
});
