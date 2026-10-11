// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/composables
// 文件名称：useCompanyRelatedPlant.ts
// 创建时间：2026-07-16
// 创建人：Takt365(Cursor AI)
// 功能描述：解析当前登录公司关联工厂（TaktCompany.RelatedPlant，经公司选项 ExtValue）；供业务页默认选中工厂
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { computed } from 'vue'
import { useTenantStore } from '@/stores/identity/useTenantStore'

/**
 * 当前公司关联工厂编码（确保公司选项已加载）
 * @returns {Promise<string>} PlantCode；未配置时为空串
 */
export async function resolveCurrentCompanyRelatedPlantCode(): Promise<string> {
  const tenantStore = useTenantStore()
  const hasOptions = tenantStore.companyOptions.length > 0
  const currentExt = String(tenantStore.currentCompanyOption?.extValue ?? '').trim()
  if (!hasOptions || (tenantStore.companyCode && !currentExt)) {
    await tenantStore.loadCompanyOptionsAsync()
  }
  return String(tenantStore.currentCompanyRelatedPlant ?? '').trim()
}

/**
 * 组合式：公司切换时同步关联工厂编码
 * @returns relatedPlantCode 与 ensure/apply 辅助
 */
export function useCompanyRelatedPlant() {
  const tenantStore = useTenantStore()

  /** 当前公司关联工厂（响应式） */
  const relatedPlantCode = computed(() => tenantStore.currentCompanyRelatedPlant)

  /**
   * 确保选项就绪并返回关联工厂
   * @returns {Promise<string>} PlantCode
   */
  async function ensureRelatedPlantCode(): Promise<string> {
    return resolveCurrentCompanyRelatedPlantCode()
  }

  return {
    relatedPlantCode,
    ensureRelatedPlantCode,
  }
}
