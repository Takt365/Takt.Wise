<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/foundation/ip-geolocation -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：IP归属（ip2region；权限 foundation:ip:geolocation:list；expose 无） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="foundation-ip-geolocation p-4 flex flex-col gap-4">
    <!-- 页面标题 -->
    <div>
      <h2 class="text-xl font-semibold mb-1 m-0">{{ t('foundation.ip-geolocation.page.title') }}</h2>
      <p class="text-sm text-text-secondary m-0">{{ t('foundation.ip-geolocation.page.description') }}</p>
    </div>

    <!-- 查询 -->
    <a-card :title="t('foundation.ip-geolocation.page.section.query')" :bordered="false" class="bg-container">
      <a-form layout="inline" :model="queryForm" @finish="handleSearch">
        <a-form-item
          :label="t('foundation.ip-geolocation.page.field.ip')"
          name="ip"
          :rules="ipFormRules"
        >
          <a-input
            v-model:value="queryForm.ip"
            class="w-80"
            :placeholder="t('foundation.ip-geolocation.page.placeholder.ip')"
            allow-clear
            :maxlength="45"
            show-count
          />
        </a-form-item>
        <a-form-item>
          <a-space>
            <a-button
              v-permission="PERMISSION_LIST"
              type="primary"
              html-type="submit"
              :loading="searchLoading"
            >
              {{ t('foundation.ip-geolocation.page.button.search') }}
            </a-button>
            <a-button
              v-permission="PERMISSION_LIST"
              :loading="clientLoading"
              @click="handleSearchClient"
            >
              {{ t('foundation.ip-geolocation.page.button.client') }}
            </a-button>
          </a-space>
        </a-form-item>
      </a-form>
    </a-card>

    <!-- 结果 -->
    <a-card :title="t('foundation.ip-geolocation.page.section.result')" :bordered="false" class="bg-container">
      <a-spin :spinning="searchLoading || clientLoading">
        <template v-if="result">
          <a-alert
            class="mb-4"
            :type="result.found ? 'success' : 'warning'"
            :message="result.found ? t('foundation.ip-geolocation.page.alert.found') : t('foundation.ip-geolocation.page.alert.not.found')"
            show-icon
          />
          <a-descriptions bordered :column="1" size="small">
            <a-descriptions-item :label="t('foundation.ip-geolocation.page.field.ip')">
              {{ displayValue(result.ip) }}
            </a-descriptions-item>
            <a-descriptions-item :label="t('foundation.ip-geolocation.page.field.country')">
              {{ displayValue(result.country) }}
            </a-descriptions-item>
            <a-descriptions-item :label="t('foundation.ip-geolocation.page.field.region')">
              {{ displayValue(result.region) }}
            </a-descriptions-item>
            <a-descriptions-item :label="t('foundation.ip-geolocation.page.field.province')">
              {{ displayValue(result.province) }}
            </a-descriptions-item>
            <a-descriptions-item :label="t('foundation.ip-geolocation.page.field.city')">
              {{ displayValue(result.city) }}
            </a-descriptions-item>
            <a-descriptions-item :label="t('foundation.ip-geolocation.page.field.isp')">
              {{ displayValue(result.isp) }}
            </a-descriptions-item>
            <a-descriptions-item :label="t('foundation.ip-geolocation.page.field.formatted.address')">
              {{ displayValue(result.formattedAddress) }}
            </a-descriptions-item>
            <a-descriptions-item :label="t('foundation.ip-geolocation.page.field.full.address')">
              {{ displayValue(result.fullAddress) }}
            </a-descriptions-item>
          </a-descriptions>
        </template>
        <a-empty v-else :description="t('foundation.ip-geolocation.page.placeholder.ip')" />
      </a-spin>
    </a-card>
  </div>
</template>

<script setup lang="ts">
/**
 * IP 归属查询页面
 * 调用 TaktIpGeolocations /search、/client，底层为 TaktLocationHelper + ip2region
 */
import { message } from 'ant-design-vue'
import type { Rule } from 'ant-design-vue/es/form'
import { useI18n } from 'vue-i18n'
import {
  searchClientIpGeolocation,
  searchIpGeolocation,
} from '@/api/foundation/ipGeolocation.api'
import type { IpGeolocation } from '@/types/foundation/ipGeolocation.types'
import { usePermissionStore } from '@/stores/identity/usePermissionStore'

const { t } = useI18n()

/** 权限 Store */
const permissionStore = usePermissionStore()

/** 列表权限（与菜单、控制器 foundation:ip:geolocation:list 一致） */
const PERMISSION_LIST = 'foundation:ip:geolocation:list'

/** 查询表单 */
const queryForm = ref({ ip: '' })

/** 按 IP 查询 loading */
const searchLoading = ref(false)

/** 客户端 IP 查询 loading */
const clientLoading = ref(false)

/** 最近一次查询结果 */
const result = ref<IpGeolocation | null>(null)

/** IP 表单校验 */
const ipFormRules = computed<Rule[]>(() => [
  { required: true, message: t('foundation.ip-geolocation.page.rule.ip.required') },
])

/**
 * 空值显示为占位符
 * @param value 字段值
 * @returns 显示文案
 */
function displayValue(value: string | undefined | null): string {
  if (value == null || String(value).trim() === '') {
    return '-'
  }
  return String(value)
}

/**
 * 按输入 IP 查询归属
 */
async function handleSearch() {
  if (!permissionStore.hasPermission(PERMISSION_LIST)) {
    return
  }
  const ip = queryForm.value.ip?.trim()
  if (!ip) {
    return
  }
  searchLoading.value = true
  try {
    result.value = await searchIpGeolocation(ip)
  } catch {
    result.value = null
    message.error(t('foundation.ip-geolocation.page.message.search.fail'))
  } finally {
    searchLoading.value = false
  }
}

/**
 * 查询当前请求客户端 IP 归属，并回填输入框
 */
async function handleSearchClient() {
  if (!permissionStore.hasPermission(PERMISSION_LIST)) {
    return
  }
  clientLoading.value = true
  try {
    const data = await searchClientIpGeolocation()
    result.value = data
    if (data.ip) {
      queryForm.value.ip = data.ip
    }
  } catch {
    result.value = null
    message.error(t('foundation.ip-geolocation.page.message.client.fail'))
  } finally {
    clientLoading.value = false
  }
}
</script>
