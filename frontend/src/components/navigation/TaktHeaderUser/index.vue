<!-- ========================================
项目名称:TaktWise
命名空间:@/components/navigation/TaktHeaderUser
文件名称:index.vue
创建时间:2025-01-20
创建人:Takt365(Cursor AI)
功能描述:用户菜单（个人中心 / 退出）；无头像时展示用户名首字母
版权信息:Copyright (c) 2025 Takt  All rights reserved.
免责声明:此软件使用 MIT License,作者不承担任何使用风险。
======================================== -->
<template>
  <a-dropdown
    :trigger="['click']"
    placement="bottomRight"
  >
    <a-button type="text">
      <template #icon>
        <a-avatar
          :size="20"
          v-bind="avatarUrl ? { src: avatarUrl } : {}"
          class="bg-primary text-[10px] font-medium text-white"
        >
          {{ avatarInitial }}
        </a-avatar>
      </template>
    </a-button>
    <template #overlay>
      <a-menu>
        <a-menu-item @click="handleProfile">
          <UserOutlined />
          {{ $t('common.page.button.profile') }}
        </a-menu-item>
        <a-menu-divider />
        <a-menu-item @click="handleLogout">
          <LogoutOutlined />
          {{ $t('common.page.button.logout') }}
        </a-menu-item>
      </a-menu>
    </template>
  </a-dropdown>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import { Modal } from 'ant-design-vue'
import {
  UserOutlined,
  LogoutOutlined
} from '@ant-design/icons-vue'
import { storeToRefs } from 'pinia'
import { useUserStore } from '@/stores/identity/useUserStore'
import { executeUserLogoutAsync } from '@/bootstrap/taktEventHandlers'
import { resolveTaktAvatarInitial, resolveTaktAvatarUrl } from '@/utils/taktAvatarUrl'

const emit = defineEmits<{
  'profile': []
  'logout': []
}>()

const router = useRouter()
const { userInfo } = storeToRefs(useUserStore())

/** 头像 URL：admin 种子 / 上传地址；空则用首字母 */
const avatarUrl = computed(() => resolveTaktAvatarUrl(userInfo.value?.avatar))

/** 无头像占位：用户名首字母 */
const avatarInitial = computed(() =>
  resolveTaktAvatarInitial(userInfo.value?.userName, userInfo.value?.nickName),
)

/**
 * 进入个人中心（Layout 附属路由，不依赖菜单权限）
 */
const handleProfile = () => {
  emit('profile')
  router.push('/profile')
}

const { t } = useI18n()
const handleLogout = () => {
  const logoutAction = t('common.page.button.logout')
  Modal.confirm({
    title: t('common.tip.confirm.title', { action: logoutAction }),
    content: t('common.tip.confirm.question', { action: logoutAction }),
    centered: true,
    okText: t('common.page.button.ok'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      emit('logout')
      await executeUserLogoutAsync()
    }
  })
}
</script>
