<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：@/views/code/generator/components -->
<!-- 文件名称：CodePreview.vue -->
<!-- 功能描述：代码生成结果预览弹窗（TaktModal）；按 backend/frontend/script 分 Tab，左侧分组文件树、右侧 highlight.js 高亮；展示 validationIssues；由 index.vue v-model 传入 files/loading。 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <TaktModal
    v-model:open="open"
    :title="t('common.dialog.title.preview', { entity: t('entity.gentable._self') })"
    width="80%"
    :centered="true"
    :body-style="{ height: '75vh', overflow: 'auto' }"
    :footer="null"
    destroy-on-close
    @cancel="handleCancel"
  >
    <div
      v-if="loading"
      class="takt-code-preview flex min-h-[280px] items-center justify-center"
    >
      <a-spin :tip="t('code.generator.page.preview.loading')" />
    </div>
    <div
      v-else-if="!files || files.length === 0"
      class="takt-code-preview py-6 text-center"
    >
      <a-empty :description="t('code.generator.page.preview.empty')">
        <template #description>
          <span>{{ t('code.generator.page.preview.emptyhint') }}</span>
        </template>
      </a-empty>
    </div>
    <div
      v-else
      class="takt-code-preview h-full"
    >
      <a-alert
        v-if="(validationIssues?.length ?? 0) > 0"
        type="warning"
        show-icon
        :message="t('code.generator.page.preview.validationissuetitle', { count: validationIssues.length })"
        class="mb-2"
      >
        <template #description>
          <div class="max-h-40 overflow-auto">
            <div
              v-for="(issue, idx) in validationIssues"
              :key="`${issue.templateKey}_${idx}`"
              class="mb-2 border-b border-dashed border-border pb-2 last:mb-0 last:border-0"
            >
              <div class="break-all font-semibold text-text">
                {{ issue.templateKey }}
              </div>
              <div
                v-if="issue.targetPath"
                class="break-all text-xs text-text-secondary"
              >
                {{ issue.targetPath }}
              </div>
              <div class="break-all text-[var(--ant-color-error)]">
                {{ issue.message }}
              </div>
            </div>
          </div>
        </template>
      </a-alert>
      <a-tabs
        v-model:active-key="activeTab"
        class="mb-2"
      >
        <a-tab-pane
          key="backend"
          :tab="t('code.generator.page.preview.tab.backend')"
        />
        <a-tab-pane
          key="frontend"
          :tab="t('code.generator.page.preview.tab.frontend')"
        />
        <a-tab-pane
          key="script"
          :tab="t('code.generator.page.preview.tab.script')"
        />
      </a-tabs>
      <div class="flex h-full min-h-[300px] overflow-hidden rounded border border-border bg-container">
        <div class="w-80 shrink-0 overflow-y-auto border-r border-border bg-container">
          <template
            v-for="group in visibleCategoryGroups"
            :key="group.key"
          >
            <div class="border-b border-dashed border-border px-3 py-2 text-xs text-text-secondary">
              {{ group.label }}
            </div>
            <div
              v-for="f in group.files"
              :key="f.name"
              class="flex cursor-pointer items-center gap-1.5 break-all px-3 py-2 text-xs hover:bg-container"
              :class="selecmedFileName === f.name ? 'bg-primary/10 text-primary' : ''"
              @click="selecmedFileName = f.name"
            >
              <span class="min-w-0 flex-1">{{ f.name }}</span>
              <a-tag
                v-if="f.isExisting"
                color="orange"
              >
                {{ t('code.generator.page.preview.exists') }}
              </a-tag>
            </div>
          </template>
        </div>
        <div class="takt-code-preview-panel flex-1 overflow-auto p-3">
          <pre class="m-0 overflow-auto whitespace-pre text-xs leading-normal"><code
            class="hljs"
            v-html="highlightedHtml"
          /></pre>
        </div>
      </div>
    </div>
  </TaktModal>
</template>

<script setup lang="ts">
/**
 * 代码生成预览弹窗：接收 previewCode 返回的文件列表与校验问题，按路径归类展示并语法高亮。
 * 导出 PreviewFile、PreviewValidationIssue 供父页 index.vue 复用。
 */
import { useI18n } from 'vue-i18n'
import hljs from 'highlight.js'
import TaktModal from '@/components/business/TaktModal/index.vue'

/** highlight.js 自动检测语言子集（生成器常见后缀） */
const PREVIEW_HIGHLIGHT_AUTO_SUBSET = [
  'csharp',
  'typescript',
  'javascript',
  'xml',
  'json',
  'sql',
  'less',
  'css',
  'scss',
  'yaml',
  'markdown',
  'bash',
  'plaintext'
] as const

/**
 * HTML 转义（高亮失败回退纯文本）
 * @param text 原始代码文本
 * @returns {string} 转义后的 HTML 片段
 */
function escapeHtmlPlain(text: string): string {
  return text
    .replace(/&/g, '&amp;')
    .replace(/</g, '&lt;')
    .replace(/>/g, '&gt;')
    .replace(/"/g, '&quot;')
}

/**
 * 按文件扩展名解析 highlight.js 语言 id
 * @param fileName 相对路径或文件名
 * @returns {string | null} 语言 id，无法识别时 null
 */
function resolveHighlightLanguage(fileName: string): string | null {
  const base = fileName.split(/[/\\]/).pop() ?? fileName
  const dot = base.lastIndexOf('.')
  if (dot < 0) return null
  const ext = base.slice(dot + 1).toLowerCase()
  const byExt: Record<string, string> = {
    cs: 'csharp',
    ts: 'typescript',
    tsx: 'typescript',
    js: 'javascript',
    jsx: 'javascript',
    mjs: 'javascript',
    cjs: 'javascript',
    vue: 'xml',
    json: 'json',
    sql: 'sql',
    less: 'less',
    css: 'css',
    scss: 'scss',
    sass: 'scss',
    md: 'markdown',
    markdown: 'markdown',
    xml: 'xml',
    html: 'xml',
    htm: 'xml',
    yaml: 'yaml',
    yml: 'yaml',
    csproj: 'xml',
    props: 'xml',
    targets: 'xml',
    sln: 'plaintext'
  }
  return byExt[ext] ?? null
}

/**
 * 对预览代码做语法高亮
 * @param code 文件内容
 * @param fileName 用于推断语言
 * @returns {string} 可 v-html 的高亮 HTML
 */
function highlightPreviewCode(code: string, fileName: string): string {
  if (!code) return ''
  const lang = resolveHighlightLanguage(fileName)
  if (lang && hljs.getLanguage(lang)) {
    try {
      return hljs.highlight(code, { language: lang, ignoreIllegals: true }).value
    } catch {
      /* 指定语言高亮失败时回退自动检测 */
    }
  }
  try {
    return hljs.highlightAuto(code, [...PREVIEW_HIGHLIGHT_AUTO_SUBSET]).value
  } catch {
    return escapeHtmlPlain(code)
  }
}

/** 预览单文件（相对路径 + 内容） */
export interface PreviewFile {
  /** 生成目标相对路径 */
  name: string
  /** 文件正文 */
  content: string
  /** 磁盘上是否已存在同名文件 */
  isExisting?: boolean
}

/** 预览前校验问题（模板/路径/说明） */
export interface PreviewValidationIssue {
  templateKey: string
  targetPath?: string
  message: string
}

/** 顶层 Tab：后端 / 前端 / 脚本 SQL */
type PreviewTab = 'backend' | 'frontend' | 'script'

/** 左侧文件树分组键 */
type PreviewCategory =
  | 'backendEntity'
  | 'backendDto'
  | 'backendService'
  | 'backendController'
  | 'backendValidators'
  | 'backendOther'
  | 'frontendApi'
  | 'frontendType'
  | 'frontendView'
  | 'frontendComponent'
  | 'frontendOther'
  | 'scriptTranslationSql'
  | 'scriptMenuSql'
  | 'scriptOther'

/** 各 Tab 下分组展示顺序（与代码生成模板产出顺序一致） */
const TAB_CATEGORY_ORDER: Record<PreviewTab, PreviewCategory[]> = {
  backend: [
    'backendEntity',
    'backendDto',
    'backendService',
    'backendController',
    'backendValidators',
    'backendOther'],
  frontend: [
    'frontendApi',
    'frontendType',
    'frontendView',
    'frontendComponent',
    'frontendOther'],
  script: [
    'scriptTranslationSql',
    'scriptMenuSql',
    'scriptOther'],
}

/** 组件入参 */
const props = withDefaults(
  defineProps<{
    /** 弹窗 v-model 开关 */
    modelValue?: boolean
    /** 预览文件列表 */
    files?: PreviewFile[]
    /** 预览请求加载中 */
    loading?: boolean
    /** 生成前校验问题（非空时顶部 Alert） */
    validationIssues?: PreviewValidationIssue[]
  }>(),
  { modelValue: false, files: () => [], loading: false, validationIssues: () => [] }
)

/** v-model 更新事件 */
const emit = defineEmits<{
  (e: 'update:modelValue', value: boolean): void
}>()

/** i18n 翻译函数 */
const { t } = useI18n()

/** 弹窗 open，双向绑定 modelValue */
const open = computed({
  get: () => props.modelValue,
  set: (v) => emit('update:modelValue', v),
})

/** 当前 Tab：backend | frontend | script */
const activeTab = ref<PreviewTab>('backend')

/** 左侧选中的文件相对路径 */
const selecmedFileName = ref('')

/** 左侧分组（分类标题 + 文件列表） */
interface PreviewCategoryGroup {
  key: PreviewCategory
  label: string
  files: PreviewFile[]
}

/**
 * 路径统一为小写正斜杠
 * @param path 原始路径
 * @returns {string} 规范化路径
 */
function normalizePath(path: string): string {
  return path.replace(/\\/g, '/').toLowerCase()
}

/**
 * 解析文件所属顶层 Tab
 * @param path 相对路径
 * @returns {PreviewTab} backend | frontend | script
 */
function resolveFileTab(path: string): PreviewTab {
  const normalized = normalizePath(path)
  if (normalized.startsWith('frontend/')) return 'frontend'
  if (normalized.endsWith('.sql') || normalized.startsWith('backend/sql/')) return 'script'
  return 'backend'
}

/**
 * 解析文件在左侧树中的细分类别
 * @param path 相对路径
 * @returns {PreviewCategory} 分组键
 */
function resolveFileCategory(path: string): PreviewCategory {
  const normalized = normalizePath(path)
  if (normalized.startsWith('frontend/')) {
    if (normalized.includes('/src/api/')) return 'frontendApi'
    if (normalized.includes('/src/types/')) return 'frontendType'
    if (normalized.includes('/src/views/') && normalized.includes('/components/')) return 'frontendComponent'
    if (normalized.includes('/src/views/')) return 'frontendView'
    return 'frontendOther'
  }

  if (normalized.endsWith('.sql') || normalized.startsWith('backend/sql/')) {
    if (normalized.includes('translation')) return 'scriptTranslationSql'
    if (normalized.includes('menu')) return 'scriptMenuSql'
    return 'scriptOther'
  }

  if (normalized.startsWith('backend/src/')) {
    if (normalized.includes('/entities/')) return 'backendEntity'
    if (normalized.includes('/dtos/')) return 'backendDto'
    if (normalized.includes('/services/')) return 'backendService'
    if (normalized.includes('/controllers/')) return 'backendController'
    if (normalized.includes('/validators/') || normalized.endsWith('validators.cs')) return 'backendValidators'
  }
  return 'backendOther'
}

/**
 * 分类 i18n 标题
 * @param category 分组键
 * @returns {string} 展示文案
 */
function categoryLabel(category: PreviewCategory): string {
  switch (category) {
    case 'backendEntity':
      return t('code.generator.page.preview.category.backend.entity')
    case 'backendDto':
      return t('code.generator.page.preview.category.backend.dto')
    case 'backendService':
      return t('code.generator.page.preview.category.backend.service')
    case 'backendController':
      return t('code.generator.page.preview.category.backend.controller')
    case 'backendValidators':
      return t('code.generator.page.preview.category.backend.validators')
    case 'backendOther':
      return t('code.generator.page.preview.category.backend.other')
    case 'frontendApi':
      return t('code.generator.page.preview.category.frontend.api')
    case 'frontendType':
      return t('code.generator.page.preview.category.frontend.type')
    case 'frontendView':
      return t('code.generator.page.preview.category.frontend.view')
    case 'frontendComponent':
      return t('code.generator.page.preview.category.frontend.component')
    case 'frontendOther':
      return t('code.generator.page.preview.category.frontend.other')
    case 'scriptTranslationSql':
      return t('code.generator.page.preview.category.script.translationsql')
    case 'scriptMenuSql':
      return t('code.generator.page.preview.category.script.menusql')
    case 'scriptOther':
      return t('code.generator.page.preview.category.script.other')
  }
}

/** 按 Tab 分组的文件列表 */
const tabFilesMap = computed<Record<PreviewTab, PreviewFile[]>>(() => {
  const map: Record<PreviewTab, PreviewFile[]> = { backend: [], frontend: [], script: [] }
  for (const file of props.files ?? []) {
    map[resolveFileTab(file.name)].push(file)
  }
  return map
})

/** 当前 Tab 下按固定顺序分组的可见文件树 */
const visibleCategoryGroups = computed<PreviewCategoryGroup[]>(() => {
  const groups = new Map<PreviewCategory, PreviewFile[]>()
  for (const file of tabFilesMap.value[activeTab.value]) {
    const category = resolveFileCategory(file.name)
    const list = groups.get(category) ?? []
    list.push(file)
    groups.set(category, list)
  }
  return TAB_CATEGORY_ORDER[activeTab.value]
    .filter((key) => (groups.get(key)?.length ?? 0) > 0)
    .map((key) => ({
      key,
      label: categoryLabel(key),
      files: [...(groups.get(key) ?? [])].sort((a, b) => a.name.localeCompare(b.name)),
    }))
})

/** files 变化时重置 Tab 与默认选中文件 */
watch(
  () => props.files,
  (list) => {
    const files = list ?? []
    const [first] = files
    activeTab.value = first ? resolveFileTab(first.name) : 'backend'
    selecmedFileName.value = first?.name ?? ''
  },
  { immediate: true }
)

/** 切换 Tab 时若当前选中不在本 Tab，则选中第一项 */
watch(
  [activeTab, tabFilesMap],
  () => {
    const currentFiles = tabFilesMap.value[activeTab.value]
    if (currentFiles.some((f) => f.name === selecmedFileName.value)) return
    const [head] = currentFiles
    selecmedFileName.value = head?.name ?? ''
  },
  { immediate: true }
)

/** 当前选中文件正文 */
const selecmedContent = computed(() => {
  const f = (props.files ?? []).find((item) => item.name === selecmedFileName.value)
  return f ? f.content : ''
})

/** 右侧代码区高亮 HTML */
const highlightedHtml = computed(() =>
  highlightPreviewCode(selecmedContent.value, selecmedFileName.value)
)

/**
 * 关闭弹窗
 */
function handleCancel() {
  emit('update:modelValue', false)
}
</script>
