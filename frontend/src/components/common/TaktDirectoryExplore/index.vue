<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/components/common/TaktDirectoryExplore -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：通用目录浏览（服务器目录 / 文件服务器 / FTP）；浏览与新建走 API -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="takt-directory-explore flex flex-col gap-2">
    <!-- 文件服务器凭据 -->
    <a-form v-if="method === 'fileserver' && showAuthForm" layout="vertical" class="mb-0">
      <a-row :gutter="12">
        <a-col :span="12">
          <a-form-item :label="t('components.common.page.directoryexplore.userName')">
            <a-input v-model:value="authState.userName" allow-clear />
          </a-form-item>
        </a-col>
        <a-col :span="12">
          <a-form-item :label="t('components.common.page.directoryexplore.password')">
            <a-input-password
              v-model:value="authState.password"
              :placeholder="passwordPlaceholder"
              allow-clear
            />
          </a-form-item>
        </a-col>
      </a-row>
    </a-form>

    <!-- FTP 连接 -->
    <a-form v-if="method === 'ftp' && showAuthForm" layout="vertical" class="mb-0">
      <a-row :gutter="12">
        <a-col :span="16">
          <a-form-item :label="t('components.common.page.directoryexplore.host')" required>
            <a-input v-model:value="ftpState.host" allow-clear />
          </a-form-item>
        </a-col>
        <a-col :span="8">
          <a-form-item :label="t('components.common.page.directoryexplore.port')">
            <a-input-number v-model:value="ftpState.port" :min="1" :max="65535" class="w-full" />
          </a-form-item>
        </a-col>
      </a-row>
      <a-row :gutter="12">
        <a-col :span="12">
          <a-form-item :label="t('components.common.page.directoryexplore.userName')" required>
            <a-input v-model:value="ftpState.userName" allow-clear />
          </a-form-item>
        </a-col>
        <a-col :span="12">
          <a-form-item :label="t('components.common.page.directoryexplore.password')" required>
            <a-input-password
              v-model:value="ftpState.password"
              :placeholder="passwordPlaceholder"
              allow-clear
            />
          </a-form-item>
        </a-col>
      </a-row>
    </a-form>

    <!-- 新建文件夹已并入资源管理器工具栏 -->

    <TaktFolderExplorer
      :mode="explorerMode"
      :loading="loading"
      :current-path="currentPath"
      :parent-path="parentPath"
      :items="items"
      :selecmed-path="selecmedPath"
      :empty-hint="emptyHint"
      :address-placeholder="addressPlaceholder"
      :panel-height="panelHeight"
      :table-scroll-y="tableScrollY"
      :show-nav-tree="showNavTree"
      :allow-create="allowCreate"
      :can-create="canCreateHere"
      @go="onNavigate"
      @navigate="onNavigate"
      @enter="onNavigate"
      @up="goParent"
      @refresh="refresh"
      @select="onSelectPath"
      @create-folder="onCreateFolder"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 通用目录浏览：三种方法 server / fileserver / ftp，经 API 浏览与新建。
 */
import { computed, reactive, ref, watch } from 'vue'
import { useI18n } from 'vue-i18n'
import { message } from 'ant-design-vue'
import {
  browseDirectory,
  createDirectory,
} from '@/api/common/directoryExplore.api'
import type {
  TaktDirectoryExploreItem,
  TaktDirectoryExploreMethod,
} from '@/types/components/directoryExplore.types'
import type { TaktFolderExplorerPathMode } from '@/types/components/folderExplorer.types'
import {
  isFolderExplorerLocalRoot,
  resolveFolderExplorerConfirmPath,
  TAKT_FOLDER_EXPLORER_LOCAL_ROOT,
} from '@/utils/taktFolderExplorerPath'

/** i18n */
const { t } = useI18n()

const props = withDefaults(defineProps<{
  /**
   * 浏览方式：server=服务器目录；fileserver=文件服务器；ftp=FTP
   */
  method: TaktDirectoryExploreMethod
  /** 初始路径 */
  initialPath?: string
  /** 文件服务器用户名 */
  initialUserName?: string
  /** FTP 主机 */
  initialHost?: string
  /** FTP 端口 */
  initialPort?: number | null
  /** 是否已有保存密码（占位提示） */
  hasPassword?: boolean
  /** 已存配置 Id（解密密码） */
  configId?: string
  /** 是否显示凭据表单 */
  showAuthForm?: boolean
  /** 是否允许新建文件夹 */
  allowCreate?: boolean
  /** 是否显示左树 */
  showNavTree?: boolean
  /** 面板高度 */
  panelHeight?: number
  /** 表格滚动高度 */
  tableScrollY?: number
}>(), {
  showAuthForm: true,
  allowCreate: true,
  showNavTree: true,
  panelHeight: 460,
  tableScrollY: 300,
})

const emit = defineEmits<{
  /** 选中路径变化 */
  select: [path: string]
  /** 当前路径变化 */
  'update:path': [path: string]
}>()

const loading = ref(false)
const creating = ref(false)
const currentPath = ref('')
const parentPath = ref<string | null>(null)
const items = ref<TaktDirectoryExploreItem[]>([])
const selecmedPath = ref('')

const authState = reactive({
  userName: '',
  password: '',
})

const ftpState = reactive({
  host: '',
  port: 21 as number,
  userName: '',
  password: '',
})

const explorerMode = computed<TaktFolderExplorerPathMode>(() => {
  if (props.method === 'server') {
    return 'local'
  }
  if (props.method === 'fileserver') {
    return 'unc'
  }
  return 'ftp'
})

const passwordPlaceholder = computed(() =>
  props.hasPassword
    ? t('components.common.page.directoryexplore.passwordkeep')
    : t('components.common.page.directoryexplore.passwordoptional'),
)

const emptyHint = computed(() => {
  if (props.method === 'server') {
    return t('components.business.page.folderexplorer.computer')
  }
  return t('components.common.page.directoryexplore.notconnected')
})

const addressPlaceholder = computed(() => {
  if (props.method === 'server') {
    return t('components.common.page.directoryexplore.serverpathplaceholder')
  }
  if (props.method === 'fileserver') {
    return t('components.common.page.directoryexplore.uncplaceholder')
  }
  return t('components.common.page.directoryexplore.ftppathplaceholder')
})

const canCreateHere = computed(() => {
  if (props.method === 'server') {
    // 已进入盘符/文件夹，或在「此电脑」选中了盘符
    const cur = String(currentPath.value || '').trim()
    if (cur && !isFolderExplorerLocalRoot(cur) && /^[A-Za-z]:/.test(cur)) {
      return true
    }
    const sel = String(selecmedPath.value || '').trim()
    return !!sel && /^[A-Za-z]:[\\/]?$/.test(sel)
  }
  if (props.method === 'fileserver') {
    return String(currentPath.value || '').startsWith('\\\\')
  }
  return !!String(currentPath.value || '').trim()
})

/**
 * 解析新建文件夹的父目录
 * @returns 父路径；无法新建返回空
 */
function resolveCreateParent(): string {
  if (props.method === 'server') {
    const cur = String(currentPath.value || '').trim()
    if (cur && !isFolderExplorerLocalRoot(cur) && /^[A-Za-z]:/.test(cur)) {
      return cur.replace(/\\+$/, '') || cur
    }
    const sel = String(selecmedPath.value || '').trim()
    if (/^[A-Za-z]:[\\/]?$/.test(sel)) {
      return sel.replace(/[\\/]+$/, '') || sel
    }
    return ''
  }
  if (props.method === 'fileserver') {
    return String(currentPath.value || '').trim().replace(/\\+$/, '')
  }
  const base = String(currentPath.value || '/').trim().replace(/\/+$/, '') || ''
  return base === '' ? '/' : base
}

/**
 * 组装 fileserver / ftp 认证
 */
function buildAuth() {
  if (props.method === 'fileserver') {
    return {
      fileServer: {
        userName: authState.userName || undefined,
        password: authState.password || undefined,
        configId: props.configId,
      },
    }
  }
  if (props.method === 'ftp') {
    return {
      ftp: {
        host: ftpState.host.trim(),
        port: ftpState.port || 21,
        userName: ftpState.userName.trim(),
        password: ftpState.password || undefined,
        configId: props.configId,
      },
    }
  }
  return {}
}

/**
 * 校验 FTP 必填
 * @returns 是否通过
 */
function ensureFtpReady(): boolean {
  if (!ftpState.host?.trim() || !ftpState.userName?.trim()) {
    message.warning(t('components.common.page.directoryexplore.ftprequired'))
    return false
  }
  const canUseSaved = !!props.hasPassword && !!props.configId
  if (!ftpState.password && !canUseSaved) {
    message.warning(t('components.common.page.directoryexplore.ftprequired'))
    return false
  }
  return true
}

/**
 * 浏览指定路径
 * @param path 路径
 */
async function load(path?: string | null) {
  if (props.method === 'ftp' && !ensureFtpReady()) {
    return
  }
  if (props.method === 'fileserver') {
    const target = String(path ?? currentPath.value ?? '').trim()
    if (!target) {
      message.warning(t('components.common.page.directoryexplore.pathrequired'))
      return
    }
  }
  loading.value = true
  try {
    let browsePath = path === undefined || path === null ? '' : String(path).trim()
    if (props.method === 'server' && isFolderExplorerLocalRoot(browsePath)) {
      browsePath = ''
    }
    const res = await browseDirectory({
      method: props.method,
      path: browsePath || undefined,
      ...buildAuth(),
    })
    currentPath.value = res.currentPath || ''
    parentPath.value = res.parentPath === undefined ? null : res.parentPath
    items.value = [...(res.items ?? [])].sort((a, b) =>
      String(a.name ?? '').localeCompare(String(b.name ?? ''), undefined, { sensitivity: 'base' }),
    )
    // 进入目录后选中当前目录；回到「此电脑」清空选中
    if (props.method === 'server' && !res.currentPath) {
      selecmedPath.value = ''
    } else {
      selecmedPath.value = res.currentPath || ''
    }
    emit('update:path', currentPath.value)
  } catch (error) {
    logger.error('[TaktDirectoryExplore] browse failed', { error, method: props.method })
    message.error(
      (error instanceof Error && error.message)
        || t('components.common.page.directoryexplore.browsefailed'),
    )
  } finally {
    loading.value = false
  }
}

/**
 * @param path 导航目标
 */
function onNavigate(path: unknown) {
  if (typeof path !== 'string') {
    if (props.method === 'server') {
      void load(undefined)
    }
    return
  }
  if (props.method === 'server' && isFolderExplorerLocalRoot(path)) {
    void load(undefined)
    return
  }
  void load(path)
}

function goParent() {
  if (props.method === 'server') {
    if (parentPath.value === '' || parentPath.value === TAKT_FOLDER_EXPLORER_LOCAL_ROOT) {
      void load(undefined)
    } else if (parentPath.value) {
      void load(parentPath.value)
    } else {
      void load(undefined)
    }
    return
  }
  if (parentPath.value) {
    void load(parentPath.value)
  }
}

function refresh() {
  if (props.method === 'server') {
    void load(currentPath.value || undefined)
    return
  }
  void load(currentPath.value || props.initialPath || (props.method === 'ftp' ? '/' : ''))
}

/**
 * @param path 选中
 */
function onSelectPath(path: string) {
  selecmedPath.value = path
  emit('select', path)
}

/**
 * 资源管理器工具栏「新建文件夹」
 * @param name 文件夹名
 */
async function onCreateFolder(name: string) {
  const folderName = String(name || '').trim().replace(/[\\/]/g, '')
  if (!folderName) {
    message.warning(t('components.common.page.directoryexplore.newfolderrequired'))
    return
  }
  if (props.method === 'ftp' && !ensureFtpReady()) {
    return
  }
  const parent = resolveCreateParent()
  if (!parent) {
    message.warning(t('components.common.page.directoryexplore.needabsolutepath'))
    return
  }
  let target = ''
  let refreshPath = parent
  if (props.method === 'server') {
    const base = parent.replace(/\\+$/, '')
    target = `${base}\\${folderName}`
    refreshPath = /^[A-Za-z]:$/.test(base) ? `${base}\\` : base
  } else if (props.method === 'fileserver') {
    target = `${parent}\\${folderName}`
    refreshPath = parent
  } else {
    target = parent === '/' ? `/${folderName}` : `${parent}/${folderName}`
    refreshPath = parent
  }
  creating.value = true
  try {
    await createDirectory({
      method: props.method,
      path: target,
      ...buildAuth(),
    })
    message.success(t('components.common.page.directoryexplore.createfoldersuccess'))
    await load(refreshPath)
    selecmedPath.value = target
  } catch (error) {
    logger.error('[TaktDirectoryExplore] mkdir failed', { error })
    message.error(
      (error instanceof Error && error.message)
        || t('components.common.page.directoryexplore.createfolderfailed'),
    )
  } finally {
    creating.value = false
  }
}

/**
 * @deprecated 兼容旧调用；请用工具栏新建
 */
async function createFolder() {
  // no-op：由 explorer 弹窗触发 onCreateFolder
}

/**
 * 解析确认用完整路径；必要时确保目录存在
 * @param ensure 是否调用创建 API
 * @returns 路径；无效返回空
 */
async function resolveConfirmPath(ensure = true): Promise<string> {
  const mode = explorerMode.value
  let path = resolveFolderExplorerConfirmPath(mode, currentPath.value, selecmedPath.value)
  if (!path) {
    return ''
  }
  if (props.method === 'server') {
    if (isFolderExplorerLocalRoot(path) || path === '\\' || !/^[A-Za-z]:[\\/]/.test(path)) {
      return ''
    }
  }
  if (props.method === 'fileserver') {
    if (!/^\\\\[^\\]+\\[^\\]+/.test(path.replace(/\//g, '\\'))) {
      return ''
    }
  }
  if (props.method === 'ftp' && !path.startsWith('/')) {
    return ''
  }
  if (!ensure) {
    return path
  }
  if (props.method === 'ftp' && !ensureFtpReady()) {
    return ''
  }
  creating.value = true
  try {
    const created = await createDirectory({
      method: props.method,
      path,
      ...buildAuth(),
    })
    const resolved = String(created || '').trim() || path
    // mkdir 成功后仍须是完整路径，禁止回填残缺值
    if (props.method === 'server' && !/^[A-Za-z]:[\\/]/.test(resolved)) {
      return path
    }
    if (props.method === 'fileserver' && !resolved.startsWith('\\\\')) {
      return path
    }
    return resolved
  } finally {
    creating.value = false
  }
}

/**
 * 获取当前选中/路径与凭据快照
 * @returns 快照
 */
function getValues() {
  return {
    method: props.method,
    path: resolveFolderExplorerConfirmPath(explorerMode.value, currentPath.value, selecmedPath.value),
    userName: props.method === 'ftp' ? ftpState.userName : authState.userName,
    password: props.method === 'ftp' ? ftpState.password : authState.password,
    host: ftpState.host,
    port: ftpState.port,
  }
}

/**
 * 重置并加载
 * @param path 可选路径
 */
async function reload(path?: string) {
  await load(path === undefined ? (props.initialPath || undefined) : path)
}

watch(
  () => [props.method, props.initialPath, props.initialHost, props.initialUserName, props.initialPort] as const,
  () => {
    authState.userName = props.initialUserName || ''
    authState.password = ''
    ftpState.host = props.initialHost || ''
    ftpState.port = props.initialPort || 21
    ftpState.userName = props.initialUserName || ''
    ftpState.password = ''
    newFolderName.value = ''
    selecmedPath.value = props.initialPath || ''
    currentPath.value = props.initialPath || (props.method === 'ftp' ? '/' : '')
    items.value = []
    parentPath.value = null
    if (props.method === 'server') {
      void load(props.initialPath || undefined)
    } else if (props.method === 'fileserver' && props.initialPath) {
      void load(props.initialPath)
    } else if (props.method === 'ftp' && props.initialHost && props.initialUserName
      && (props.hasPassword && props.configId)) {
      void load(props.initialPath || '/')
    }
  },
  { immediate: true },
)

defineExpose({
  /** 浏览服务器目录 */
  browseServerDirectory: (path?: string) => load(path),
  /** 浏览文件服务器目录 */
  browseFileServerDirectory: (path: string) => load(path),
  /** 浏览 FTP 目录 */
  browseFtpDirectory: (path?: string) => load(path || '/'),
  reload,
  refresh,
  createFolder,
  resolveConfirmPath,
  getValues,
  getSelecmedPath: () => selecmedPath.value,
  getCurrentPath: () => currentPath.value,
})
</script>
