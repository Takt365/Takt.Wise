// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/engineering-change/composables
// 文件名称：useEcDeptViewFormRules.ts
// 创建时间：2026-09-09
// 创建人：Takt365(Cursor AI)
// 功能描述：执行部门更新表单：课别填报字段前端必填（后端可空；不含 remark）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { Rule } from 'ant-design-vue/es/form'
import { useI18n } from 'vue-i18n'
import { getEcDeptExecFillRequiredFields } from '@/constants/logistics/ecDeptExecLineFields.constants'

/**
 * 值是否视为空（字符串 trim；数字 0 合法）
 * @param value 表单值
 * @returns {boolean} 是否空
 */
function isEcDeptFillEmpty(value: unknown): boolean {
  if (value === null || value === undefined) {
    return true
  }
  if (typeof value === 'string') {
    return value.trim() === ''
  }
  return false
}

/**
 * 执行部门更新表单校验规则（填报字段全部必填）
 * @param deptSlug 部门实体 slug
 * @param label 字段标签解析（pi.label）
 * @returns {ComputedRef<Record<string, Rule[]>>} Ant Design Form rules
 */
export function useEcDeptViewFormRules(
  deptSlug: string,
  label: (field: string) => string,
) {
  const { t } = useI18n()

  /** 更新态填报必填规则 */
  const rules = computed<Record<string, Rule[]>>(() => {
    const result: Record<string, Rule[]> = {}
    for (const field of getEcDeptExecFillRequiredFields(deptSlug)) {
      const fieldLabel = label(field)
      result[field] = [
        {
          required: true,
          validator: async (_rule, value) => {
            if (isEcDeptFillEmpty(value)) {
              return Promise.reject(
                t('common.page.form.placeholder.required', { field: fieldLabel }),
              )
            }
            return Promise.resolve()
          },
          trigger: ['change', 'blur'],
        },
      ]
    }
    return result
  })

  return { rules }
}
