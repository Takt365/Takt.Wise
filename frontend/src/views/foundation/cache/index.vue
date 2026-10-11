<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/foundation/cache -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：缓存管理页面，展示配置与统计，支持按键检查/移除（权限 foundation:cache:list） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="foundation-cache p-4 flex flex-col gap-4">
    <!-- 页面标题 -->
    <div>
      <h2 class="text-xl font-semibold mb-1 m-0">{{ t('foundation.cache.page.title') }}</h2>
      <p class="text-sm text-text-secondary m-0">{{ t('foundation.cache.page.description') }}</p>
    </div>

    <!-- 工具栏 -->
    <TaktToolsBar
      :show-create="false"
      :show-update="false"
      :show-delete="false"
      :show-import="false"
      :show-export="false"
      :show-advanced-query="false"
      :show-column-setting="false"
      :show-expand="false"
      :show-fullscreen="true"
      :show-refresh="true"
      :refresh-loading="loading"
      @refresh="loadCacheInfo"
    />

    <a-spin :spinning="loading">
      <!-- 缓存配置 -->
      <a-card :title="t('foundation.cache.page.section.config')" :bordered="false" class="bg-container">
        <a-descriptions bordered :column="1" size="small">
          <a-descriptions-item :label="t('foundation.cache.page.field.provider')">
            {{ cacheInfo?.provider ?? '-' }}
          </a-descriptions-item>
          <a-descriptions-item :label="t('foundation.cache.page.field.default.expiration.minutes')">
            {{ cacheInfo?.defaultExpirationMinutes ?? '-' }}
          </a-descriptions-item>
          <a-descriptions-item :label="t('foundation.cache.page.field.enable.sliding.expiration')">
            {{ formatYesNo(cacheInfo?.enableSlidingExpiration) }}
          </a-descriptions-item>
          <a-descriptions-item :label="t('foundation.cache.page.field.enable.multi.level.cache')">
            {{ formatYesNo(cacheInfo?.enableMultiLevelCache) }}
          </a-descriptions-item>
          <a-descriptions-item
            v-if="cacheInfo?.provider === 'Redis'"
            :label="t('foundation.cache.page.field.redis.instance.name')"
          >
            {{ cacheInfo?.redisInstanceName || '-' }}
          </a-descriptions-item>
        </a-descriptions>
      </a-card>

      <!-- 缓存统计 -->
      <a-card :title="t('foundation.cache.page.section.statistics')" :bordered="false" class="bg-container mt-4">
        <a-descriptions bordered :column="1" size="small">
          <template v-if="!statistics?.supported || statistics?.message">
            <a-descriptions-item :label="t('foundation.cache.page.field.note')">
              {{ statistics?.message ?? t('foundation.cache.page.message.loading.hint') }}
            </a-descriptions-item>
          </template>
          <template v-else>
            <a-descriptions-item :label="t('foundation.cache.page.field.current.entry.count')">
              {{ statistics?.currentEntryCount ?? '-' }}
            </a-descriptions-item>
            <a-descriptions-item :label="t('foundation.cache.page.field.total.hits')">
              {{ statistics?.totalHits ?? '-' }}
            </a-descriptions-item>
            <a-descriptions-item :label="t('foundation.cache.page.field.total.misses')">
              {{ statistics?.totalMisses ?? '-' }}
            </a-descriptions-item>
            <a-descriptions-item :label="t('foundation.cache.page.field.hit.rate')">
              {{ formatHitRate(statistics?.hitRate) }}
            </a-descriptions-item>
            <a-descriptions-item :label="t('foundation.cache.page.field.estimated.size.bytes')">
              {{ formatEstimatedBytes(statistics?.currentEstimatedSizeBytes) }}
            </a-descriptions-item>
          </template>
        </a-descriptions>
      </a-card>

      <!-- 按键操作 -->
      <a-card :title="t('foundation.cache.page.section.key.ops')" :bordered="false" class="bg-container mt-4">
        <a-form layout="inline" :model="keyForm" @finish="handleCheckExists">
          <a-form-item
            :label="t('foundation.cache.page.field.cache.key')"
            name="key"
            :rules="keyFormRules"
          >
            <a-input
              v-model:value="keyForm.key"
              class="w-72"
              :placeholder="t('foundation.cache.page.placeholder.cache.key')"
              allow-clear
            />
          </a-form-item>
          <a-form-item>
            <a-space>
              <a-button
                v-permission="PERMISSION_LIST"
                type="primary"
                html-type="submit"
                :loading="existsLoading"
              >
                {{ t('foundation.cache.page.button.check.exists') }}
              </a-button>
              <a-button
                v-permission="PERMISSION_LIST"
                danger
                :loading="removeLoading"
                :disabled="!keyForm.key?.trim()"
                @click="handleRemove"
              >
                {{ t('foundation.cache.page.button.remove') }}
              </a-button>
            </a-space>
          </a-form-item>
        </a-form>
        <a-alert
          v-if="existsResult !== null"
          class="mt-3"
          :type="existsResult ? 'success' : 'warning'"
          :message="existsResult ? t('foundation.cache.page.alert.key.exists') : t('foundation.cache.page.alert.key.not.exists')"
          show-icon
        />
      </a-card>
    </a-spin>
  </div>
</template>

<script setup lang="ts">
/**
 * 缓存管理页面
 * 展示运行时缓存配置、统计与按键操作
 */
import { message, Modal } from 'ant-design-vue'
import type { Rule } from 'ant-design-vue/es/form'
import { useI18n } from 'vue-i18n'
import {
  existsCacheKey,
  getCacheInfo,
  getCacheStatistics,
  removeCacheKey,
} from '@/api/foundation/cache.api'
import type { TaktCacheInfoDto, TaktCacheStatisticsDto } from '@/types/foundation/cache.types'
import { usePermissionStore } from '@/stores/identity/usePermissionStore'

const { t } = useI18n()
/** 权限 Store */
const permissionStore = usePermissionStore()

/** 列表/键操作权限（与 TaktCachesController、菜单 foundation:cache:list 一致） */
const PERMISSION_LIST = 'foundation:cache:list'

/** 页面加载状态 */
const loading = ref(false)
/** 缓存配置 */
const cacheInfo = ref<TaktCacheInfoDto | null>(null)
/** 缓存统计 */
const statistics = ref<TaktCacheStatisticsDto | null>(null)
/** 按键表单 */
const keyForm = ref({ key: '' })
/** 检查存在 loading */
const existsLoading = ref(false)
/** 移除键 loading */
const removeLoading = ref(false)
/** 键存在检查结果 */
const existsResult = ref<boolean | null>(null)

/** 缓存键表单校验 */
const keyFormRules = computed<Rule[]>(() => [
  { required: true, message: t('foundation.cache.page.rule.cache.key.required') }])

/**
 * 格式化是/否
 * @param value 布尔值
 * @returns 本地化文案
 */
function formatYesNo(value: boolean | undefined): string {
  if (value === undefined) {
    return '-'
  }
  return value ? t('common.status.yes') : t('common.status.no')
}

/**
 * 格式化命中率
 * @param hitRate 0~1
 * @returns 百分比或占位
 */
function formatHitRate(hitRate: number | undefined): string {
  if (hitRate == null) {
    return '-'
  }
  return `${Math.round(hitRate * 10000) / 100}%`
}

/**
 * 格式化估算字节数
 * @param bytes 字节数
 * @returns 数值或占位
 */
function formatEstimatedBytes(bytes: number | undefined): string {
  if (bytes == null) {
    return '-'
  }
  return String(bytes)
}

/**
 * 加载缓存配置与统计（需 foundation:cache:list）
 */
async function loadCacheInfo() {
  if (!permissionStore.hasPermission(PERMISSION_LIST)) {
    return
  }
  loading.value = true
  existsResult.value = null
  try {
    const [info, stats] = await Promise.all([getCacheInfo(), getCacheStatistics()])
    cacheInfo.value = info
    statistics.value = stats
  } catch {
    cacheInfo.value = null
    statistics.value = null
    message.error(t('foundation.cache.page.message.load.fail'))
  } finally {
    loading.value = false
  }
}

/** 租户/公司切换时刷新 */
useTableRefresh(loadCacheInfo)

/**
 * 检查缓存键是否存在
 */
async function handleCheckExists() {
  if (!permissionStore.hasPermission(PERMISSION_LIST)) {
    return
  }
  const key = keyForm.value.key?.trim()
  if (!key) {
    return
  }
  existsLoading.value = true
  existsResult.value = null
  try {
    const result = await existsCacheKey(key)
    existsResult.value = result.exists
  } catch {
    message.error(t('foundation.cache.page.message.check.fail'))
  } finally {
    existsLoading.value = false
  }
}

/**
 * 移除指定缓存键
 */
function handleRemove() {
  if (!permissionStore.hasPermission(PERMISSION_LIST)) {
    return
  }
  const key = keyForm.value.key?.trim()
  if (!key) {
    return
  }
  Modal.confirm({
    title: t('foundation.cache.page.button.remove'),
    content: key,
    okType: 'danger',
    onOk: async () => {
      removeLoading.value = true
      existsResult.value = null
      try {
        await removeCacheKey(key)
        message.success(t('foundation.cache.page.message.remove.success'))
        existsResult.value = false
        await loadCacheInfo()
      } catch {
        message.error(t('foundation.cache.page.message.remove.fail'))
      } finally {
        removeLoading.value = false
      }
    },
  })
}

onMounted(() => {
  void loadCacheInfo()
})
</script>
