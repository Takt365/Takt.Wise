<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/views/dashboard/workspace/modules -->
<!-- 文件名称：WelcomeModule.vue -->
<!-- 功能描述：工作台欢迎问候模块（时段问候 / 假日问候与引用，依赖公开假日主题 API） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="min-h-32 py-4">
    <!-- 问候行 -->
    <a-row
      class="mb-3"
      :gutter="16"
    >
      <a-col
        :span="12"
        class="text-lg font-medium text-text"
      >
        <span>{{ greetingText }}</span>
        <a-tag
          v-if="activeHoliday?.isHolidayToday && activeHoliday.holidayName"
          class="ml-2 align-middle"
          color="processing"
        >
          {{ activeHoliday.holidayName }}
        </a-tag>
      </a-col>
      <a-col
        :span="12"
        class="text-right text-sm text-text-secondary"
      >
        <a-tooltip :title="dateTooltip">
          <span>{{ t('dashboard.workspace.page.currenttimelabel') }} {{ dateText }}</span>
        </a-tooltip>
      </a-col>
    </a-row>

    <!-- 引用区 -->
    <div class="flex items-center gap-2 text-xl font-bold leading-relaxed text-text-secondary md:text-[22px]">
      <span class="inline-flex shrink-0 items-center text-xl md:text-[22px]">
        <RiLightbulbLine />
      </span>
      <span>{{ quoteText }}</span>
    </div>
  </div>
</template>

<script setup lang="ts">
/**
 * 工作台欢迎问候模块：展示当前时刻、时段/假日问候语与每日引用
 */
import { useI18n } from 'vue-i18n'
import dayjs from 'dayjs'
import localizedFormat from 'dayjs/plugin/localizedFormat'
import utc from 'dayjs/plugin/utc'
import timezone from 'dayjs/plugin/timezone'
import dayOfYear from 'dayjs/plugin/dayOfYear'
import weekOfYear from 'dayjs/plugin/weekOfYear'
import quarterOfYear from 'dayjs/plugin/quarterOfYear'
import { RiLightbulbLine } from '@remixicon/vue'
import { storeToRefs } from 'pinia'
import { getCurrentUser } from '@/api/identity/auth.api'
import { useUserStore } from '@/stores/identity/useUserStore'
import { useLocaleStore } from '@/stores/foundation/useLocaleStore'
import type { HolidayTheme } from '@/types/human-resource/attendance/holiday.types'
import { normalizeUserInfoProfile } from '@/utils/taktUserProfileNormalize'
import { resolveTaktSolarTermQuoteI18nKey } from '@/utils/taktSolarTerm'

dayjs.extend(localizedFormat)
dayjs.extend(utc)
dayjs.extend(timezone)
dayjs.extend(dayOfYear)
dayjs.extend(weekOfYear)
dayjs.extend(quarterOfYear)

const { t } = useI18n()
const userStore = useUserStore()
const { holidayFromToken, userName, isLoggedIn } = storeToRefs(userStore)
const localeStore = useLocaleStore()
const { currentLocale } = storeToRefs(localeStore)

/** 当前时刻（每秒刷新，用于时钟与时段问候） */
const now = ref(new Date())

/** 用户展示名（昵称 → 员工姓名 → 用户名） */
const displayName = ref('')

/** 时钟定时器句柄 */
let timer: number | null = null

/** 区域与时区映射（CultureCode → IANA 时区） */
const localeTimeZoneMap: Record<string, string> = {
  'zh-CN': 'Asia/Shanghai',
  'zh-HK': 'Asia/Hong_Kong',
  'en-US': 'America/New_York',
  'ja-JP': 'Asia/Tokyo',
}

/**
 * 去除字符串首尾空白；非字符串返回空串
 * @param value 原始值
 * @returns {string} 修剪后的字符串
 */
function safeTrim(value: unknown): string {
  return typeof value === 'string' ? value.trim() : ''
}

/**
 * 当前区域对应的 IANA 时区
 */
const timeZone = computed(() => localeTimeZoneMap[currentLocale.value] || dayjs.tz.guess())

/**
 * 当前假日主题（与 HolidayTheme 对齐，来自 userStore.holidayFromToken）
 */
const activeHoliday = computed<HolidayTheme | null>(() => holidayFromToken.value)

/**
 * 格式化后的当前时刻文本
 */
const dateText = computed(() =>
  dayjs(now.value).tz(timeZone.value).format('YYYY-MM-DD HH:mm:ss'),
)

/**
 * 日期悬停提示（本地化长日期、星期、季度、周、年内第几天）
 */
const dateTooltip = computed(() => {
  const d = dayjs(now.value).tz(timeZone.value)
  const dayOfYearLabel = t('dashboard.workspace.page.dayofyearlabel', { n: d.dayOfYear() })
  return `${d.format('LL')} · ${d.format('dddd')} · Q${d.quarter()} · W${d.week()} · ${dayOfYearLabel}`
})

/**
 * 解析用户展示名（已登录时从 /me 拉取，否则为空）
 * @returns {Promise<void>}
 */
async function resolveDisplayName(): Promise<void> {
  if (!isLoggedIn.value) {
    displayName.value = ''
    return
  }

  try {
    if (!userStore.profileLoaded) {
      await userStore.loadUserProfile()
    }
    const profile = normalizeUserInfoProfile(await getCurrentUser())
    displayName.value =
      safeTrim(profile.nickName) ||
      safeTrim(profile.employeeName) ||
      safeTrim(profile.userName) ||
      safeTrim(userName.value)
  } catch {
    displayName.value = safeTrim(userName.value)
  }
}

/**
 * 按当前会话租户与公司同步当日假日主题（不改动界面语言）
 * @returns {Promise<void>}
 */
async function syncHolidayTheme(): Promise<void> {
  await userStore.loadHolidayThemeForCurrentSession()
}

/**
 * 刷新当前时刻
 */
function updateNow(): void {
  now.value = new Date()
}

/**
 * 问候语：假日且 isHolidayToday 时使用 holidayGreeting，否则时段问候 + 展示名
 */
const greetingText = computed(() => {
  void currentLocale.value
  const holiday = activeHoliday.value
  const name = displayName.value

  if (holiday?.isHolidayToday && safeTrim(holiday.holidayGreeting)) {
    const greeting = holiday.holidayGreeting.trim()
    return name ? `${greeting}，${name}` : greeting
  }

  const hour = now.value.getHours()
  let key: string
  if (hour < 9) key = 'common.page.greeting.morning'
  else if (hour < 12) key = 'common.page.greeting.forenoon'
  else if (hour < 14) key = 'common.page.greeting.noon'
  else if (hour < 18) key = 'common.page.greeting.afternoon'
  else key = 'common.page.greeting.night'

  const greeting = t(key)
  return name ? `${greeting}${name}` : greeting
})

/**
 * 引用区：假日且 isHolidayToday 时使用 holidayQuote，否则按当前二十四节气取 common.page.quote.*
 */
const quoteText = computed(() => {
  void currentLocale.value
  const holiday = activeHoliday.value
  if (holiday?.isHolidayToday && safeTrim(holiday.holidayQuote)) {
    return holiday.holidayQuote.trim()
  }
  return t(resolveTaktSolarTermQuoteI18nKey(now.value))
})

onMounted(() => {
  updateNow()
  timer = window.setInterval(updateNow, 1000)
  void syncHolidayTheme()
  void resolveDisplayName()
})

watch(currentLocale, () => {
  void syncHolidayTheme()
})

onBeforeUnmount(() => {
  if (timer !== null) {
    clearInterval(timer)
    timer = null
  }
})
</script>
