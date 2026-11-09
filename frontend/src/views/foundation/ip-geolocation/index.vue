<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/foundation/ip-geolocation -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：IP归属（进页自动公网IP；ip2region；权限 foundation:ip:geolocation:list） -->
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

    <!-- 自动公网（ip138 风格） -->
    <a-card :bordered="false" class="bg-container">
      <a-spin :spinning="visitorLoading">
        <a-alert
          v-if="visitorLine"
          type="info"
          show-icon
          :message="visitorLine"
        />
        <a-alert
          v-else-if="visitorError"
          type="warning"
          show-icon
          :message="visitorError"
        />
        <a-empty
          v-else
          :description="t('foundation.ip-geolocation.page.visitor.loading')"
        />
      </a-spin>
    </a-card>

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
      <div v-if="activeHostIps.length > 0" class="mt-3 flex flex-wrap items-center gap-2">
        <span class="text-sm text-text-secondary">{{ t('foundation.ip-geolocation.page.field.active.host.ips') }}</span>
        <a-tag
          v-for="hostIp in activeHostIps"
          :key="hostIp"
          class="cursor-pointer"
          :color="hostIp === queryForm.ip ? 'processing' : undefined"
          @click="handleSelectActiveHostIp(hostIp)"
        >
          {{ hostIp }}
        </a-tag>
      </div>
    </a-card>

    <!-- 结果 -->
    <a-card :title="t('foundation.ip-geolocation.page.section.result')" :bordered="false" class="bg-container">
      <a-spin :spinning="searchLoading || clientLoading || visitorLoading">
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
 * 进页自动探测浏览器出口公网 IP + ip2region；本机网卡为辅
 */
import { message } from 'ant-design-vue'
import type { Rule } from 'ant-design-vue/es/form'
import { useI18n } from 'vue-i18n'
import {
  searchClientIpGeolocation,
  searchIpGeolocation,
  searchVisitorPublicIpGeolocation,
} from '@/api/foundation/ipGeolocation.api'
import type { IpGeolocation } from '@/types/foundation/ipGeolocation.types'
import { usePermissionStore } from '@/stores/identity/usePermissionStore'
import { resolveBrowserPublicIp } from '@/utils/publicIp'

const { t } = useI18n()

/** 权限 Store */
const permissionStore = usePermissionStore()

/** 列表权限（与菜单、控制器 foundation:ip:geolocation:list 一致） */
const PERMISSION_LIST = 'foundation:ip:geolocation:list'

/** 查询表单 */
const queryForm = ref({ ip: '' })

/** 按 IP 查询 loading */
const searchLoading = ref(false)

/** 本机网卡查询 loading */
const clientLoading = ref(false)

/** 进页公网自动探测 loading */
const visitorLoading = ref(false)

/** 最近一次查询结果（库内原文原样展示） */
const result = ref<IpGeolocation | null>(null)

/** 本机已激活网卡 IP */
const activeHostIps = ref<string[]>([])

/** 是否已写入公网摘要（切换语言时壳文案随 t() 重算） */
const visitorLineActive = ref(false)

/** 公网自动探测失败文案 */
const visitorError = ref('')

/** IP 表单校验 */
const ipFormRules = computed<Rule[]>(() => [
  { required: true, message: t('foundation.ip-geolocation.page.rule.ip.required') },
])

/**
 * 公网摘要行：您的IP：{ip},位置：{location}（壳文案 i18n，地点库原文）
 */
const visitorLine = computed(() => {
  if (!visitorLineActive.value || !result.value) return ''
  const ip = result.value.ip?.trim() || ''
  const location = result.value.formattedAddress?.trim()
    || result.value.fullAddress?.trim()
    || t('foundation.ip-geolocation.page.visitor.unknown.location')
  return t('foundation.ip-geolocation.page.visitor.line', { ip, location })
})

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
 * 应用归属结果到表单与详情区
 * @param data 归属结果（原文展示）
 * @param asVisitor 是否同时刷新公网摘要行
 */
function applyGeolocationResult(data: IpGeolocation, asVisitor = false) {
  result.value = data
  if (data.ip) {
    queryForm.value.ip = data.ip
  }
  if (asVisitor) {
    visitorLineActive.value = true
    visitorError.value = ''
  }
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
    applyGeolocationResult(await searchIpGeolocation(ip))
  } catch {
    result.value = null
    message.error(t('foundation.ip-geolocation.page.message.search.fail'))
  } finally {
    searchLoading.value = false
  }
}

/**
 * 是否为回环 IP（禁止当作「已激活网卡」展示）
 * @param ip IP 文本
 * @returns 回环则为 true
 */
function isLoopbackIp(ip: string | undefined | null): boolean {
  const text = (ip ?? '').trim().toLowerCase()
  return !text || text === '::1' || text === 'localhost' || text.startsWith('127.')
}

/**
 * 查询本机已激活网卡 IP 归属（仅真实网卡；过滤 ::1）
 */
async function handleSearchClient() {
  if (!permissionStore.hasPermission(PERMISSION_LIST)) {
    return
  }
  clientLoading.value = true
  try {
    const data = await searchClientIpGeolocation()
    const hosts = (data.activeHostIps ?? []).map((x) => x.trim()).filter((x) => x && !isLoopbackIp(x))
    activeHostIps.value = hosts
    if (hosts.length > 0) {
      queryForm.value.ip = hosts[0]
      applyGeolocationResult({ ...data, ip: hosts[0], activeHostIps: hosts })
    } else if (data.ip && !isLoopbackIp(data.ip)) {
      applyGeolocationResult(data)
    } else {
      result.value = null
      message.warning(t('foundation.ip-geolocation.page.message.client.fail'))
    }
  } catch {
    result.value = null
    activeHostIps.value = []
    message.error(t('foundation.ip-geolocation.page.message.client.fail'))
  } finally {
    clientLoading.value = false
  }
}

/**
 * 点选已激活网卡 IP 并查询归属
 * @param hostIp 激活网卡 IP
 */
async function handleSelectActiveHostIp(hostIp: string) {
  if (!hostIp?.trim()) {
    return
  }
  queryForm.value.ip = hostIp.trim()
  await handleSearch()
}

/**
 * 进页自动获取客户端出口公网 IP 并查归属（优先浏览器 echo，失败回退后端 /public）
 */
async function loadVisitorPublicIp() {
  if (!permissionStore.hasPermission(PERMISSION_LIST)) {
    return
  }
  visitorLoading.value = true
  visitorError.value = ''
  try {
    let publicIp = ''
    try {
      publicIp = await resolveBrowserPublicIp()
    } catch {
      const fallback = await searchVisitorPublicIpGeolocation()
      applyGeolocationResult(fallback, true)
      return
    }
    const data = await searchIpGeolocation(publicIp)
    applyGeolocationResult(data, true)
  } catch {
    visitorLineActive.value = false
    visitorError.value = t('foundation.ip-geolocation.page.message.visitor.fail')
  } finally {
    visitorLoading.value = false
  }
}

onMounted(() => {
  void loadVisitorPublicIp()
})
</script>
