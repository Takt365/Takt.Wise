// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/engineering-change/composables
// 文件名称：useEcDeptViewFormRules.ts
// 创建时间：2026-09-09
// 创建人：Takt365(Cursor AI)
// 功能描述：执行部门更新表单：课别填报字段前端必填（后端可空；不含 remark）；生管/采购旧品结余随「是否处理」条件必填
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { Ref } from 'vue'
import type { Rule } from 'ant-design-vue/es/form'
import { useI18n } from 'vue-i18n'
import { getEcDeptExecFillRequiredFields } from '@/constants/logistics/ecDeptExecLineFields.constants'
import {
  isValidEcIqcExecContent,
  isValidEcPcOldPartBalance,
  isValidEcPurExecContent,
} from '@/utils/regex'

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
 * @param needOldPartHandling 生管/采购：旧品是否结余（1=是 0=否）；为是时旧品结余必填
 * @returns {{ rules: ComputedRef<Record<string, Rule[]>> }} Ant Design Form rules
 */
export function useEcDeptViewFormRules(
  deptSlug: string,
  label: (field: string) => string,
  needOldPartHandling?: Ref<number>,
) {
  const { t } = useI18n()

  /** 更新态填报必填规则；生管/采购旧品结余须「旧物料-采购单:序号:数量;结余;处理方式;」且受是否处理控制 */
  const rules = computed<Record<string, Rule[]>>(() => {
    const result: Record<string, Rule[]> = {}
    for (const field of getEcDeptExecFillRequiredFields(deptSlug)) {
      if (field === 'ecOldPartBalance') {
        continue
      }
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
    if (deptSlug === 'ecpc' || deptSlug === 'ecpur') {
      const oldPartLabel = label('ecOldPartBalance')
      const needHandling = needOldPartHandling?.value !== 0
      result.ecOldPartBalance = [
        {
          required: needHandling,
          validator: async (_rule, value) => {
            if (!needHandling) {
              return Promise.resolve()
            }
            if (isEcDeptFillEmpty(value)) {
              return Promise.reject(
                t('common.page.form.placeholder.required', { field: oldPartLabel }),
              )
            }
            if (!isValidEcPcOldPartBalance(String(value))) {
              return Promise.reject(
                t('common.validation.format.pc.oldpart.invalid', { field: oldPartLabel }),
              )
            }
            return Promise.resolve()
          },
          trigger: ['change', 'blur'],
        },
      ]
    }
    if (deptSlug === 'ecpur') {
      const fieldLabel = label('execContent')
      const execRules = result.execContent ?? []
      execRules.push({
        validator: async (_rule, value) => {
          if (isEcDeptFillEmpty(value)) {
            return Promise.resolve()
          }
          if (!isValidEcPurExecContent(String(value))) {
            return Promise.reject(t('common.validation.format.pur.exec.invalid', { field: fieldLabel }))
          }
          return Promise.resolve()
        },
        trigger: ['change', 'blur'],
      })
      result.execContent = execRules
    }
    if (deptSlug === 'eciqc') {
      const fieldLabel = label('execContent')
      const execRules = result.execContent ?? []
      execRules.push({
        validator: async (_rule, value) => {
          if (isEcDeptFillEmpty(value)) {
            return Promise.resolve()
          }
          if (!isValidEcIqcExecContent(String(value))) {
            return Promise.reject(t('common.validation.format.iqc.exec.invalid', { field: fieldLabel }))
          }
          return Promise.resolve()
        },
        trigger: ['change', 'blur'],
      })
      result.execContent = execRules
    }
    return result
  })

  return { rules }
}
