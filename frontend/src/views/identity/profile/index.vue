<!-- ========================================
项目名称:TaktWise
命名空间:@/views/identity/profile
文件名称:index.vue
功能描述:个人中心三 Tab；资料 + 本用户操作/登录日志面板（QueryBar/ToolsBar）
版权信息:Copyright (c) 2025 Takt  All rights reserved.
免责声明:此软件使用 MIT License,作者不承担任何使用风险。
======================================== -->
<template>
  <div class="p-4">
    <a-card :bordered="false">
      <a-tabs v-model:active-key="activeTab" size="small">
        <a-tab-pane key="profile">
          <template #tab>
            <span class="inline-flex items-center gap-1">
              <RiUserLine class="takt-remix-icon-sm" />
              {{ t('identity.profile.page.tabs.profile') }}
            </span>
          </template>
          <div class="mb-4 flex flex-row items-center gap-4">
            <a-dropdown
              :trigger="['click']"
              placement="bottomLeft"
            >
              <button
                type="button"
                class="group relative inline-flex h-32 w-32 shrink-0 cursor-pointer items-center justify-center overflow-hidden rounded-full border-0 bg-transparent p-0 outline-none focus-visible:ring-2 focus-visible:ring-primary"
                :aria-label="t('identity.profile.page.avatar.menuAria')"
              >
                <a-avatar
                  v-if="avatarDisplayUrl"
                  :size="AVATAR_PAGE_SIZE"
                  :src="avatarDisplayUrl"
                  class="border-2 border-border transition group-hover:opacity-0 group-focus-visible:opacity-0"
                />
                <span
                  v-else
                  class="inline-flex items-center justify-center overflow-hidden rounded-full border-2 border-border bg-primary font-medium leading-none text-white transition group-hover:opacity-0 group-focus-visible:opacity-0"
                  :style="{
                    width: `${AVATAR_PAGE_SIZE}px`,
                    height: `${AVATAR_PAGE_SIZE}px`,
                    fontSize: `${AVATAR_PAGE_SIZE / 2}px`,
                  }"
                >{{ avatarInitial }}</span>
                <span
                  class="pointer-events-none absolute inset-0 flex items-center justify-center rounded-full bg-[var(--ant-color-primary-bg)] text-primary opacity-0 transition group-hover:opacity-100 group-focus-visible:opacity-100"
                  aria-hidden="true"
                >
                  <RiCameraLine :size="AVATAR_PAGE_SIZE / 2" />
                </span>
              </button>
              <template #overlay>
                <a-menu @click="handleAvatarMenuClick">
                  <a-menu-item key="view">
                    <span class="inline-flex items-center gap-2">
                      <RiEyeLine class="takt-remix-icon" />
                      {{ t('identity.profile.page.avatar.view') }}
                    </span>
                  </a-menu-item>
                  <a-menu-item
                    key="choose"
                    :disabled="!hasEmployeeBinding"
                  >
                    <span class="inline-flex items-center gap-2">
                      <RiImageAddLine class="takt-remix-icon" />
                      {{ t('identity.profile.page.avatar.choose') }}
                    </span>
                  </a-menu-item>
                </a-menu>
              </template>
            </a-dropdown>

            <a-popover
              v-model:open="nameCardOpen"
              trigger="click"
              placement="bottomLeft"
            >
              <template #title>
                <span class="font-medium text-text">{{ displayName }}</span>
              </template>
              <template #content>
                <div class="max-w-xs text-sm text-text-secondary">
                  {{ memberSinceText }}
                </div>
              </template>
              <button
                type="button"
                class="min-w-0 flex-1 border-0 bg-transparent p-0 text-left outline-none hover:opacity-90 focus-visible:ring-2 focus-visible:ring-primary"
                :aria-label="t('identity.profile.page.nameCard.aria')"
              >
                <div class="truncate text-lg font-medium text-text">
                  {{ displayName }}
                </div>
                <div class="truncate text-sm font-normal text-text-secondary">
                  {{ organizationLine }}
                </div>
              </button>
            </a-popover>
          </div>

          <!-- 查看头像 -->
          <TaktModal
            v-model:open="avatarViewOpen"
            :title="t('identity.profile.page.avatar.viewTitle')"
            :width="360"
            :use-viewport-size="false"
            hide-footer
          >
            <div class="flex items-center justify-center py-4">
              <a-avatar
                v-if="avatarDisplayUrl"
                :size="AVATAR_PREVIEW_SIZE"
                :src="avatarDisplayUrl"
                class="border border-border"
              />
              <span
                v-else
                class="inline-flex items-center justify-center overflow-hidden rounded-full border border-border bg-primary font-medium leading-none text-white"
                :style="{
                  width: `${AVATAR_PREVIEW_SIZE}px`,
                  height: `${AVATAR_PREVIEW_SIZE}px`,
                  fontSize: `${AVATAR_PREVIEW_SIZE / 2}px`,
                }"
              >{{ avatarInitial }}</span>
            </div>
          </TaktModal>

          <!-- 选择新头像 -->
          <TaktModal
            v-model:open="avatarChooseOpen"
            :title="t('identity.profile.page.avatar.chooseTitle')"
            :width="480"
            :use-viewport-size="false"
            hide-footer
          >
            <div class="flex flex-col items-center justify-center gap-6 py-2">
              <div class="flex flex-col items-center gap-2">
                <div class="text-sm text-text-secondary">
                  {{ t('identity.profile.page.avatar.current') }}
                </div>
                <a-avatar
                  v-if="avatarDisplayUrl"
                  :size="AVATAR_PREVIEW_SIZE"
                  :src="avatarDisplayUrl"
                  class="border border-border"
                />
                <span
                  v-else
                  class="inline-flex items-center justify-center overflow-hidden rounded-full border border-border bg-primary font-medium leading-none text-white"
                  :style="{
                    width: `${AVATAR_PREVIEW_SIZE}px`,
                    height: `${AVATAR_PREVIEW_SIZE}px`,
                    fontSize: `${AVATAR_PREVIEW_SIZE / 2}px`,
                  }"
                >{{ avatarInitial }}</span>
              </div>
              <div class="flex w-full max-w-sm flex-col items-center">
                <div class="mb-3 w-full text-center text-sm text-text-secondary">
                  {{ t('identity.profile.page.avatar.chooseHint') }}
                </div>
                <div class="flex w-full justify-center">
                  <TaktUploadFile
                    tabs-type="avatar"
                    v-model:avatar-url="avatarUploadUrl"
                    :avatar-show-preview="false"
                    :avatar-size="AVATAR_PREVIEW_SIZE"
                    :avatar-crop-size="AVATAR_PREVIEW_SIZE"
                    :avatar-canvas-size="AVATAR_CANVAS_SIZE"
                    :avatar-min-pixel-size="AVATAR_PAGE_SIZE"
                    :avatar-max-pixel-size="AVATAR_CANVAS_SIZE"
                    :avatar-max-size="2"
                    :avatar-custom-request="handleAvatarCustomRequest"
                    @avatar:success="handleAvatarChooseSuccess"
                    @avatar:remove="handleAvatarRemove"
                  />
                </div>
              </div>
            </div>
          </TaktModal>

          <div class="mb-3 flex flex-wrap items-center justify-end gap-2">
            <template v-if="!profileEditing">
              <a-button
                type="primary"
                @click="handleStartEditProfile"
              >
                {{ t('common.page.button.edit') }}
              </a-button>
            </template>
            <template v-else>
              <a-button
                :disabled="profileSaving"
                @click="handleCancelEditProfile"
              >
                {{ t('common.page.button.cancel') }}
              </a-button>
              <a-button
                type="primary"
                :loading="profileSaving"
                @click="handleSaveProfile"
              >
                {{ t('common.page.button.update') }}
              </a-button>
            </template>
          </div>

          <a-descriptions
            class="mb-4"
            :column="{ xs: 1, sm: 2, md: 2 }"
            bordered
            size="middle"
          >
            <a-descriptions-item :label="t('identity.profile.page.field.tenantCode')">
              {{ tenantDisplay }}
            </a-descriptions-item>
            <a-descriptions-item :label="t('identity.profile.page.field.companyCode')">
              {{ companyDisplay }}
            </a-descriptions-item>
            <a-descriptions-item :label="t('identity.profile.page.field.defaultCulture')">
              {{ cultureDisplay }}
            </a-descriptions-item>
            <a-descriptions-item :label="t('identity.profile.page.field.roles')">
              {{ rolesText }}
            </a-descriptions-item>
            <a-descriptions-item :label="t('identity.profile.page.field.employeeName')">
              {{ userInfo?.employeeName?.trim() || '—' }}
            </a-descriptions-item>
            <a-descriptions-item :label="t('entity.employee.gender')">
              <TaktDictTag
                v-if="userInfo?.gender != null"
                dict-type="sys_user_gender"
                :value="userInfo.gender"
              />
              <template v-else>—</template>
            </a-descriptions-item>
          </a-descriptions>

          <ProfileForm
            v-if="profileEditing"
            ref="profileFormRef"
            :form-data="userInfo"
            :loading="profileSaving"
          />

          <a-descriptions
            v-else
            :column="1"
            bordered
            size="middle"
          >
            <a-descriptions-item :label="t('entity.user.nickname')">
              {{ userInfo?.nickName?.trim() || '—' }}
            </a-descriptions-item>
            <a-descriptions-item :label="t('identity.profile.page.field.mobile')">
              {{ userInfo?.mobile?.trim() || '—' }}
            </a-descriptions-item>
            <a-descriptions-item :label="t('identity.profile.page.field.email')">
              {{ userInfo?.email?.trim() || '—' }}
            </a-descriptions-item>
          </a-descriptions>
        </a-tab-pane>

        <a-tab-pane key="operLog">
          <template #tab>
            <span class="inline-flex items-center gap-1">
              <RiFileList3Line class="takt-remix-icon-sm" />
              {{ t('identity.profile.page.tabs.operLog') }}
            </span>
          </template>
          <ProfileOperLogPanel v-if="activeTab === 'operLog'" />
        </a-tab-pane>

        <a-tab-pane key="loginLog">
          <template #tab>
            <span class="inline-flex items-center gap-1">
              <RiLoginBoxLine class="takt-remix-icon-sm" />
              {{ t('identity.profile.page.tabs.loginLog') }}
            </span>
          </template>
          <ProfileLoginLogPanel v-if="activeTab === 'loginLog'" />
        </a-tab-pane>
      </a-tabs>
    </a-card>
  </div>
</template>

<script setup lang="ts">
/**
 * 个人中心：资料（脸书风格头像菜单）+ 当前用户操作日志 / 登录日志（me/list 面板）
 */
import { useI18n } from 'vue-i18n'
import { message } from 'ant-design-vue'
import type { MenuProps, UploadProps } from 'ant-design-vue'
import { RiCameraLine, RiEyeLine, RiFileList3Line, RiImageAddLine, RiLoginBoxLine, RiUserLine } from '@remixicon/vue'
import { storeToRefs } from 'pinia'
import { useUserStore } from '@/stores/identity/useUserStore'
import { updateMyAvatar, updateMyProfile } from '@/api/identity/user.api'
import type { UpdateMyProfileResult } from '@/types/identity/user.types'
import { getFileById } from '@/api/foundation/file.api'
import { uploadTaktFileSmart } from '@/utils/taktFileChunkUpload'
import { resolveTaktAvatarInitial, resolveTaktAvatarUrl } from '@/utils/taktAvatarUrl'
import { formatDate, formatDateCN } from '@/utils/datetime'
import { createLogger } from '@/utils/logger'
import ProfileForm from './components/ProfileForm.vue'
import ProfileOperLogPanel from './components/ProfileOperLogPanel.vue'
import ProfileLoginLogPanel from './components/ProfileLoginLogPanel.vue'

const profileLogger = createLogger('identity-profile')
const { t, locale } = useI18n()
const userStore = useUserStore()
const { userInfo } = storeToRefs(userStore)

/** 个人中心页头像展示边长 */
const AVATAR_PAGE_SIZE = 128
/** 查看 / 选择预览 / 裁剪框与输出边长 */
const AVATAR_PREVIEW_SIZE = 256
/** 裁剪画布边长 */
const AVATAR_CANVAS_SIZE = 512

/** 当前 Tab */
const activeTab = ref('profile')

/** 查看头像弹窗 */
const avatarViewOpen = ref(false)

/** 选择头像弹窗 */
const avatarChooseOpen = ref(false)

/** 名称资料卡片 */
const nameCardOpen = ref(false)

/** 个人资料编辑态 */
const profileEditing = ref(false)

/** 个人资料保存中 */
const profileSaving = ref(false)

/** 个人资料表单 */
const profileFormRef = ref<{
  validate: () => Promise<void>
  getValues: () => import('@/types/identity/user.types').UpdateMyProfile
  resetFields: () => void
} | null>(null)

/** 是否已绑定员工（头像落库依赖员工 Avatar） */
const hasEmployeeBinding = computed(() => {
  const id = userInfo.value?.employeeId
  if (id == null || id === '') {
    return false
  }
  const n = Number(id)
  return Number.isFinite(n) && n > 0
})

/** 头像 v-model（与顶栏 userInfo.avatar 同步；可为种子相对路径或上传 URL） */
const avatarModel = ref(userInfo.value?.avatar?.trim() || '')

watch(
  () => userInfo.value?.avatar,
  (value) => {
    avatarModel.value = value?.trim() || ''
  },
)

/** 头像展示 URL（admin 种子 / 上传地址） */
const avatarDisplayUrl = computed(() => resolveTaktAvatarUrl(avatarModel.value))

/** 无头像占位：用户名首字母 */
const avatarInitial = computed(() =>
  resolveTaktAvatarInitial(userInfo.value?.userName, userInfo.value?.nickName),
)

/** 上传组件预览双向绑定：展示解析后 URL，写回原始/上传地址 */
const avatarUploadUrl = computed({
  get: () => resolveTaktAvatarUrl(avatarModel.value) || '',
  set: (value: string) => {
    avatarModel.value = value?.trim() || ''
  },
})

/** 展示名：仅当前登录用户昵称 */
const displayName = computed(() => {
  return userInfo.value?.nickName?.trim() || '—'
})

/**
 * 注册日起始文案（用户 CreatedAt）
 */
const memberSinceText = computed(() => {
  const raw = userInfo.value?.createdAt?.trim()
  if (!raw) {
    return t('identity.profile.page.nameCard.startedUsingUnknown')
  }
  const lang = String(locale.value || '')
  const date =
    lang.startsWith('zh') || lang.startsWith('ja')
      ? formatDateCN(raw)
      : formatDate(raw)
  return t('identity.profile.page.nameCard.startedUsing', { date })
})

/**
 * 解析部门显示名（优先 org.dept.{code}，否则回退接口 DeptName）
 */
function resolveDeptDisplayNames(): string {
  const codes = userInfo.value?.deptCodes ?? []
  const fallback = (userInfo.value?.deptName ?? '').trim()
  if (codes.length === 0) {
    return fallback
  }
  const labels = codes.map((code, index) => {
    const key = `org.dept.${code.trim().toLowerCase()}`
    const translated = t(key)
    if (translated && translated !== key) {
      return translated
    }
    const parts = fallback ? fallback.split(/[、,，]/) : []
    return parts[index]?.trim() || code
  })
  return labels.filter(Boolean).join('、')
}

/**
 * 第二行：当前登录公司 / 所属部门（TaktUserCompany 会话公司 + TaktEmployeeDept）
 */
const organizationLine = computed(() => {
  const company =
    userInfo.value?.companyName?.trim() ||
    userInfo.value?.companyCode?.trim() ||
    ''
  const dept = resolveDeptDisplayNames()
  if (company && dept) {
    return `${company} / ${dept}`
  }
  return company || dept || '—'
})

/** 编码 + 名称展示（名称优先，附带编码） */
function formatNamedCode(name?: string | null, code?: string | null): string {
  const n = name?.trim() || ''
  const c = code?.trim() || ''
  if (n && c) {
    return `${n}（${c}）`
  }
  return n || c || '—'
}

/** 租户：TaktTenant.TenantName + TenantCode */
const tenantDisplay = computed(() =>
  formatNamedCode(userInfo.value?.tenantName, userInfo.value?.tenantCode),
)

/** 公司：CompanyName + CompanyCode */
const companyDisplay = computed(() =>
  formatNamedCode(userInfo.value?.companyName, userInfo.value?.companyCode),
)

/** 默认语言：TaktCulture.NativeName + CultureCode */
const cultureDisplay = computed(() =>
  formatNamedCode(userInfo.value?.cultureName, userInfo.value?.defaultCulture),
)

/** 角色列表文案（优先 RoleName） */
const rolesText = computed(() => {
  const names = userInfo.value?.roleNames?.filter((n) => !!n?.trim()) ?? []
  if (names.length > 0) {
    return names.join('、')
  }
  const roles = userInfo.value?.roles
  if (!roles?.length) {
    return '—'
  }
  return roles.join('、')
})

/**
 * 头像下拉菜单点击
 * @param info 菜单项
 */
const handleAvatarMenuClick: MenuProps['onClick'] = (info) => {
  const key = String(info.key)
  if (key === 'view') {
    avatarViewOpen.value = true
    return
  }
  if (key === 'choose') {
    if (!hasEmployeeBinding.value) {
      message.warning(t('identity.profile.page.avatar.noEmployee'))
      return
    }
    avatarChooseOpen.value = true
  }
}

/**
 * 将头像写回 userInfo（同步顶栏展示，避免整份 profile 重载）
 * @param url 头像 URL；空则清除
 */
function syncAvatarToUserStore(url: string): void {
  const current = userInfo.value
  if (!current) {
    return
  }
  userInfo.value = {
    ...current,
    avatar: url || undefined,
  }
}

/**
 * 将个人资料写回 userInfo（昵称 + 员工联系字段）
 * @param result 更新结果
 */
function syncProfileToUserStore(result: UpdateMyProfileResult): void {
  const current = userInfo.value
  if (!current) {
    return
  }
  userInfo.value = {
    ...current,
    nickName: result.nickName,
    mobile: result.mobile ?? current.mobile,
    email: result.email ?? current.email,
  }
}

/**
 * 进入个人资料编辑
 */
function handleStartEditProfile(): void {
  profileEditing.value = true
}

/**
 * 取消个人资料编辑
 */
function handleCancelEditProfile(): void {
  profileFormRef.value?.resetFields()
  profileEditing.value = false
}

/**
 * 保存个人资料（NickName→用户表；姓名/手机/邮箱/性别→员工表）
 */
async function handleSaveProfile(): Promise<void> {
  try {
    await profileFormRef.value?.validate()
    const dto = profileFormRef.value?.getValues()
    if (!dto) {
      return
    }
    profileSaving.value = true
    const result = await updateMyProfile(dto)
    syncProfileToUserStore(result)
    profileEditing.value = false
    message.success(t('identity.profile.page.saveSuccess'))
  } catch (error) {
    if (error && typeof error === 'object' && 'errorFields' in error) {
      return
    }
    profileLogger.error('保存个人资料失败', error)
  } finally {
    profileSaving.value = false
  }
}

/**
 * 解析上传结果中的访问 URL
 * @param result 文件上传结果
 */
async function resolveUploadedAccessUrl(
  result: Awaited<ReturnType<typeof uploadTaktFileSmart>>,
): Promise<string> {
  let accessUrl = result.accessUrl?.trim() ?? ''
  if (!accessUrl && result.fileId) {
    const detail = await getFileById(result.fileId)
    accessUrl = detail.accessUrl?.trim() ?? ''
  }
  if (!accessUrl) {
    throw new Error('accessUrl empty')
  }
  return accessUrl
}

/**
 * 头像裁剪后上传：TaktFile(categoryPath=avatar) → 更新员工 Avatar → 同步资料
 */
const handleAvatarCustomRequest: UploadProps['customRequest'] = (options) => {
  if (!hasEmployeeBinding.value) {
    options.onError?.(new Error(t('identity.profile.page.avatar.noEmployee')))
    message.warning(t('identity.profile.page.avatar.noEmployee'))
    return
  }
  const originFile = options.file as globalThis.File
  void (async () => {
    try {
      const result = await uploadTaktFileSmart(originFile, { categoryPath: 'avatar' })
      const accessUrl = await resolveUploadedAccessUrl(result)
      await updateMyAvatar({ avatar: accessUrl })
      avatarModel.value = accessUrl
      syncAvatarToUserStore(accessUrl)
      options.onSuccess?.({ url: accessUrl })
    } catch (error: unknown) {
      profileLogger.error('update avatar failed', { action: 'handleAvatarCustomRequest' }, error)
      const err = error instanceof Error ? error : new Error(String(error))
      message.error(err.message || t('common.feedback.failed'))
      options.onError?.(err)
    }
  })()
}

/**
 * 选择头像上传成功后关闭弹窗
 */
function handleAvatarChooseSuccess(): void {
  avatarChooseOpen.value = false
}

/**
 * 删除头像并落库
 */
async function handleAvatarRemove(): Promise<void> {
  if (!hasEmployeeBinding.value) {
    message.warning(t('identity.profile.page.avatar.noEmployee'))
    avatarModel.value = userInfo.value?.avatar?.trim() || ''
    return
  }
  try {
    await updateMyAvatar({ avatar: '' })
    avatarModel.value = ''
    syncAvatarToUserStore('')
  } catch (error: unknown) {
    profileLogger.error('remove avatar failed', { action: 'handleAvatarRemove' }, error)
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.failed'))
    avatarModel.value = userInfo.value?.avatar?.trim() || ''
  }
}
</script>
