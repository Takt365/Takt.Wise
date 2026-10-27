<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/code/database/databaseBackup.types/components -->
<!-- 文件名称：BackupClientFolderDialog.vue -->
<!-- 功能描述：客户端本机目录浏览器：授权盘符后浏览，自动回填完整绝对路径 -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <TaktModal
    v-model:open="openProxy"
    :title="t('code.database.database-backup.page.dialog.clienttitle')"
    :confirm-loading="confirmLoading"
    :use-viewport-size="false"
    width="960px"
    @ok="handleOk"
  >
    <div class="flex flex-col gap-2">
      <a-typography-text type="secondary">
        {{ t('code.database.database-backup.page.dialog.clientnativehint') }}
      </a-typography-text>
      <TaktFolderExplorer
        mode="local"
        :loading="loading"
        :current-path="currentPath"
        :parent-path="parentPath"
        :items="items"
        :selecmed-path="selecmedPath"
        :empty-hint="t('code.database.database-backup.page.dialog.clientemptyhint')"
        :panel-height="480"
        :table-scroll-y="340"
        :show-nav-tree="true"
        :allow-create="canCreateHere"
        :can-create="canCreateHere"
        @go="onNavigate"
        @up="onGoUp"
        @refresh="onRefresh"
        @select="onSelectPath"
        @create-folder="onCreateFolder"
      />
    </div>
  </TaktModal>
</template>

<script setup lang="ts">
/**
 * 客户端备份目录：盘符授权 + 资源管理器浏览，确认时自动回填完整绝对路径
 */
import { useI18n } from 'vue-i18n'
import { message } from 'ant-design-vue'
import type { TaktFolderExplorerItem } from '@/types/components/folderExplorer.types'
import {
  isClientAbsolutePath,
  isFolderExplorerLocalRoot,
  isLocalAbsolutePath,
  normalizeClientAbsolutePath,
  TAKT_FOLDER_EXPLORER_LOCAL_ROOT,
} from '@/utils/taktFolderExplorerPath'

/** File System Access 目录句柄 */
type FsDirHandle = FileSystemDirectoryHandle

/** 已授权盘符根 handle（key=D:） */
const driveRoots = new Map<string, FsDirHandle>()
/** 绝对路径 → 目录句柄 */
const handleByPath = new Map<string, FsDirHandle>()

/** i18n */
const { t } = useI18n()

const props = defineProps<{
  open: boolean
  initialPath?: string
}>()

const emit = defineEmits<{
  'update:open': [value: boolean]
  confirm: [path: string]
}>()

const openProxy = computed({
  get: () => props.open,
  set: (v: boolean) => emit('update:open', v),
})

/** 浏览 loading */
const loading = ref(false)
/** 确认 loading */
const confirmLoading = ref(false)
/** 当前路径 */
const currentPath = ref(TAKT_FOLDER_EXPLORER_LOCAL_ROOT)
/** 上级路径 */
const parentPath = ref<string | null>(null)
/** 列表项 */
const items = ref<TaktFolderExplorerItem[]>([])
/** 选中路径 */
const selecmedPath = ref('')

/** 盘符字母 */
const DRIVE_LETTERS = 'CDEFGHIJKLMNOPQRSTUVWXYZ'.split('')

const canCreateHere = computed(() => isLocalAbsolutePath(currentPath.value))

/**
 * @param path 路径
 * @returns 盘符键如 D:
 */
function driveKeyOf(path: string): string {
  const m = String(path || '').trim().match(/^([A-Za-z]):/)
  return m ? `${m[1]!.toUpperCase()}:` : ''
}

/**
 * @param path 路径
 * @returns 规范化绝对路径
 */
function toAbs(path: string): string {
  return normalizeClientAbsolutePath(path)
}

/**
 * 是否盘符根路径（D:\）
 * @param path 路径
 */
function isDriveRootPath(path: string): boolean {
  return /^[A-Za-z]:\\$/.test(toAbs(path))
}

/**
 * 「此电脑」盘符列表
 */
function buildDriveItems(): TaktFolderExplorerItem[] {
  return DRIVE_LETTERS.map((letter) => ({
    name: `${letter}:`,
    fullPath: `${letter}:\\`,
    isDirectory: true,
    modifiedTime: null,
  }))
}

/**
 * 列出子目录并缓存句柄
 * @param handle 目录句柄
 * @param basePath 绝对路径
 */
async function listDirEntries(handle: FsDirHandle, basePath: string): Promise<TaktFolderExplorerItem[]> {
  const base = toAbs(basePath).replace(/\\+$/, '')
  const prefix = `${base}\\`
  const list: TaktFolderExplorerItem[] = []
  // File System Access：values() 为异步迭代器
  const dir = handle as FsDirHandle & {
    values?: () => AsyncIterable<FileSystemHandle>
  }
  if (typeof dir.values !== 'function') {
    return list
  }
  for await (const entry of dir.values()) {
    if (entry.kind !== 'directory') {
      continue
    }
    const fullPath = toAbs(`${prefix}${entry.name}`)
    handleByPath.set(fullPath.toLowerCase(), entry as FsDirHandle)
    list.push({
      name: entry.name,
      fullPath,
      isDirectory: true,
      modifiedTime: null,
    })
  }
  list.sort((a, b) => a.name.localeCompare(b.name, undefined, { sensitivity: 'base' }))
  return list
}

/**
 * 解析句柄对应绝对路径（结合用户点击的盘符）
 * @param driveKey 盘符 D:
 * @param handle 授权得到的句柄
 * @returns 绝对路径
 */
function resolveGrantedAbsolutePath(driveKey: string, handle: FsDirHandle): string {
  const letter = driveKey.replace(/:$/, '')
  const name = String(handle.name || '').trim()
  // 选中盘符根时 name 常为空或为盘符名
  if (!name || /^[A-Za-z]:?$/.test(name)) {
    return toAbs(`${letter}:\\`)
  }
  // 选中盘符下某个文件夹：自动拼完整绝对路径（无需用户手补）
  return toAbs(`${letter}:\\${name}`)
}

/**
 * 授权盘符并进入目录（系统自动回填绝对路径）
 * @param driveKey 如 D:
 * @returns 是否成功
 */
async function grantDriveRoot(driveKey: string): Promise<boolean> {
  const w = window as Window & {
    showDirectoryPicker?: (opts?: { mode?: string }) => Promise<FsDirHandle>
  }
  if (typeof w.showDirectoryPicker !== 'function') {
    message.error(t('code.database.database-backup.page.dialog.clientpickerunsupported'))
    return false
  }
  const letter = driveKey.replace(/:$/, '')
  message.info(t('code.database.database-backup.page.dialog.clientgrantdrivehint', { drive: `${letter}:` }))
  try {
    const handle = await w.showDirectoryPicker({ mode: 'readwrite' })
    const abs = resolveGrantedAbsolutePath(driveKey, handle)
    const rootPath = toAbs(`${letter}:\\`)
    driveRoots.set(driveKey, handle)
    handleByPath.set(abs.toLowerCase(), handle)
    if (isDriveRootPath(abs)) {
      handleByPath.set(rootPath.toLowerCase(), handle)
    }
    currentPath.value = abs
    selecmedPath.value = abs
    parentPath.value = isDriveRootPath(abs) ? TAKT_FOLDER_EXPLORER_LOCAL_ROOT : rootPath
    items.value = await listDirEntries(handle, abs)
    message.success(t('code.database.database-backup.page.dialog.localpickedname', { name: abs }))
    return true
  } catch (error) {
    if (error instanceof DOMException && error.name === 'AbortError') {
      return false
    }
    logger.error('[BackupClientDialog] grant drive failed', { error })
    message.error(t('code.database.database-backup.page.dialog.clientgrantfailed'))
    return false
  }
}

/**
 * @param abs 绝对路径
 * @returns 缓存句柄
 */
function findHandle(abs: string): FsDirHandle | undefined {
  const key = toAbs(abs).toLowerCase()
  const hit = handleByPath.get(key)
  if (hit) {
    return hit
  }
  if (isDriveRootPath(abs)) {
    return driveRoots.get(driveKeyOf(abs))
  }
  return undefined
}

/**
 * 加载目录
 * @param path 目标
 */
async function load(path?: string | null) {
  const target = String(path ?? currentPath.value ?? '').trim()
  loading.value = true
  try {
    if (!target || isFolderExplorerLocalRoot(target)) {
      currentPath.value = TAKT_FOLDER_EXPLORER_LOCAL_ROOT
      parentPath.value = null
      selecmedPath.value = ''
      items.value = buildDriveItems()
      return
    }
    const abs = toAbs(target)
    if (!isLocalAbsolutePath(abs)) {
      message.warning(t('code.database.database-backup.page.dialog.localneedabsolute'))
      return
    }
    const driveKey = driveKeyOf(abs)
    let handle = findHandle(abs)
    if (!handle) {
      const ok = await grantDriveRoot(driveKey)
      if (!ok) {
        await load(TAKT_FOLDER_EXPLORER_LOCAL_ROOT)
      }
      return
    }
    currentPath.value = abs
    selecmedPath.value = abs
    if (isDriveRootPath(abs)) {
      parentPath.value = TAKT_FOLDER_EXPLORER_LOCAL_ROOT
    } else {
      const parent = abs.replace(/\\[^\\]+$/, '')
      parentPath.value = /^[A-Za-z]:$/.test(parent) ? `${parent}\\` : parent
    }
    items.value = await listDirEntries(handle, abs)
  } catch (error) {
    logger.error('[BackupClientDialog] load failed', { error })
    message.error(t('code.database.database-backup.page.message.browsefailed'))
  } finally {
    loading.value = false
  }
}

/**
 * @param path 导航目标
 */
async function onNavigate(path: string) {
  const target = String(path || '').trim()
  if (!target || isFolderExplorerLocalRoot(target)) {
    await load(TAKT_FOLDER_EXPLORER_LOCAL_ROOT)
    return
  }
  const abs = toAbs(target)
  const driveKey = driveKeyOf(abs)
  if (isDriveRootPath(abs) && !findHandle(abs) && !driveRoots.has(driveKey)) {
    loading.value = true
    try {
      await grantDriveRoot(driveKey)
    } finally {
      loading.value = false
    }
    return
  }
  await load(abs)
}

/** 上级 */
async function onGoUp() {
  if (parentPath.value == null) {
    return
  }
  await load(parentPath.value)
}

/** 刷新 */
async function onRefresh() {
  await load(currentPath.value)
}

/**
 * @param path 选中路径
 */
function onSelectPath(path: string) {
  selecmedPath.value = String(path || '').trim()
}

/**
 * @param name 新文件夹名
 */
async function onCreateFolder(name: string) {
  const folderName = String(name || '').trim()
  if (!folderName || /[\\/:*?"<>|]/.test(folderName)) {
    message.warning(t('code.database.database-backup.page.dialog.newfolderrequired'))
    return
  }
  const base = toAbs(currentPath.value)
  if (!isLocalAbsolutePath(base)) {
    message.warning(t('code.database.database-backup.page.dialog.clientneeddrivefirst'))
    return
  }
  const handle = findHandle(base)
  if (!handle) {
    message.warning(t('code.database.database-backup.page.dialog.clientneeddrivefirst'))
    return
  }
  loading.value = true
  try {
    const created = await handle.getDirectoryHandle(folderName, { create: true })
    const fullPath = toAbs(`${base.replace(/\\+$/, '')}\\${folderName}`)
    handleByPath.set(fullPath.toLowerCase(), created)
    await load(base)
    selecmedPath.value = fullPath
    message.success(t('code.database.database-backup.page.dialog.createdirectorysuccess'))
  } catch (error) {
    logger.error('[BackupClientDialog] mkdir failed', { error })
    message.error(t('code.database.database-backup.page.dialog.createdirectoryfailed'))
  } finally {
    loading.value = false
  }
}

/** 确认回填完整绝对路径 */
async function handleOk() {
  const candidate = toAbs(selecmedPath.value || currentPath.value)
  if (!isClientAbsolutePath(candidate) || isFolderExplorerLocalRoot(candidate)) {
    message.warning(t('code.database.database-backup.page.dialog.localneedabsolute'))
    return
  }
  confirmLoading.value = true
  try {
    emit('confirm', candidate)
    openProxy.value = false
  } finally {
    confirmLoading.value = false
  }
}

watch(
  () => props.open,
  async (open) => {
    if (!open) {
      return
    }
    confirmLoading.value = false
    const initial = toAbs(props.initialPath || '')
    if (isLocalAbsolutePath(initial)) {
      selecmedPath.value = initial
      await load(initial)
    } else {
      selecmedPath.value = ''
      await load(TAKT_FOLDER_EXPLORER_LOCAL_ROOT)
    }
  },
)
</script>
