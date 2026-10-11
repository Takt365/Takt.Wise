<template>
  <div class="takt-error-page">
    <div v-if="code" class="takt-error-code">{{ code }}</div>
    <a-result :status="status" :title="resolvedTitle" :sub-title="t(subtitleKey)">
      <template #extra>
        <a-space wrap>
          <a-button v-if="showReload" @click="handleReload">
            {{ t('error.page.common.reload') }}
          </a-button>
          <a-button v-if="showBack" @click="handleBack">
            {{ t('error.page.common.goback') }}
          </a-button>
          <a-button v-if="showLoginPrimary" type="primary" @click="handleLogin">
            {{ t('error.page.common.gologin') }}
          </a-button>
          <a-button
            v-if="showHome"
            :type="showLoginPrimary ? 'default' : 'primary'"
            @click="handleHome"
          >
            {{ t('error.page.common.gohome') }}
          </a-button>
        </a-space>
      </template>
    </a-result>
  </div>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import { useRouter } from 'vue-router'

/** 与 Ant Design Vue `a-result` 的 `status` 一致 */
type ErrorResultStatus = 'success' | 'error' | 'info' | 'warning' | '404' | '403' | '500'

const props = withDefaults(
  defineProps<{
    status: ErrorResultStatus
    code?: string
    /** 文案键；与 title 二选一，title 优先 */
    titleKey?: string
    /** 直接标题（如菜单占位页取自 route.meta.titleKey） */
    title?: string
    subtitleKey: string
    showBack?: boolean
    showHome?: boolean
    showLoginPrimary?: boolean
    showReload?: boolean
  }>(),
  {
    showBack: false,
    showHome: true,
    showLoginPrimary: false,
    showReload: false
  }
)

const { t } = useI18n()
const router = useRouter()

/** 结果页主标题 */
const resolvedTitle = computed(() => {
  if (props.title?.trim()) {
    return props.title.trim()
  }
  if (props.titleKey?.trim()) {
    return t(props.titleKey)
  }
  return ''
})

function handleHome() {
  router.push('/dashboard/workspace')
}

function handleLogin() {
  router.push('/login')
}

function handleBack() {
  if (typeof window !== 'undefined' && window.history.length > 1) {
    router.back()
  } else {
    router.push('/dashboard/workspace')
  }
}

function handleReload() {
  if (typeof window !== 'undefined') {
    window.location.reload()
  }
}
</script>

<style scoped lang="css">
.takt-error-page {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 100%;
  padding: 24px;
}

.takt-error-code {
  margin-bottom: 8px;
  font-size: 42px;
  font-weight: 800;
  line-height: 1;
  color: var(--ant-color-error);
}
</style>
