<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：@/components/common/TaktRichEditor -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：通用富文本（Umo Editor）；v-model:value 输出 HTML，对齐 a-textarea -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div
    ref="hostRef"
    class="takt-rich-editor"
    :class="{ 'takt-rich-editor-disabled': disabled }"
    :style="{ height }"
  >
    <UmoEditor
      v-if="canMount"
      ref="editorRef"
      v-bind="editorOptions"
      @created="handleCreated"
      @changed="handleChanged"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 通用富文本编辑器（Umo Editor），表单绑定 HTML 字符串。
 * 弹窗/页签内须等容器有宽高再挂载，否则 Tiptap 会得到 0 尺寸而整片空白。
 */
import { useI18n } from 'vue-i18n';
import { UmoEditor, type UmoEditorInstance } from '@umoteam/editor';
import '@umoteam/editor/style';
import { getFileById } from '@/api/foundation/file.api';
import { useThemeStore } from '@/stores/common/useThemeStore';
import type { FileUploadResult } from '@/types/foundation/file.types';
import { uploadTaktFileSmart } from '@/utils/taktFileChunkUpload';

const { t, locale } = useI18n();
const themeStore = useThemeStore();

const props = withDefaults(defineProps<{
  /** HTML 正文（对齐 a-textarea 的 value） */
  value?: string;
  /** 占位文案 */
  placeholder?: string;
  /** 禁用编辑 */
  disabled?: boolean;
  /** 编辑器高度 */
  height?: string;
}>(), {
  value: '',
  placeholder: '',
  disabled: false,
  height: '28rem',
});

const emit = defineEmits<{
  'update:value': [html: string];
}>();

/** 外层容器，用于测量可见尺寸 */
const hostRef = ref<HTMLElement | null>(null);
/** 容器具备有效宽高后再挂载 Umo，避免 Modal/Tab 里 0 尺寸初始化 */
const canMount = ref(false);
/** Umo 实例 */
const editorRef = ref<UmoEditorInstance | null>(null);
/** 外部灌入内容时跳过 changed 回写 */
const applyingExternal = ref(false);
/** 实例键，避免同页多个编辑器互相覆盖本地状态 */
const editorKey = `takt-rich-${useId().replace(/:/g, '')}`;

/**
 * 将应用 locale 映射为 Umo 支持的语言
 * @param culture 当前 vue-i18n locale
 */
function mapUmoLocale(culture: string): 'zh-CN' | 'en-US' {
  return culture.toLowerCase().startsWith('en') ? 'en-US' : 'zh-CN';
}

/**
 * 规范化 HTML：空段落视为空串
 * @param html 原始 HTML
 */
function normalizeHtml(html: string): string {
  const trimmed = html.replace(/<p>\s*<\/p>/gi, '').trim();
  return trimmed === '' ? '' : html;
}

/**
 * 读取编辑器当前 HTML
 */
function readHtml(): string {
  const html = editorRef.value?.getHTML?.() ?? '';
  return normalizeHtml(html);
}

/**
 * 向父级回写 HTML（与当前 value 相同时跳过）
 * @param html 编辑器 HTML
 */
function emitHtml(html: string): void {
  const next = normalizeHtml(html);
  if (next === (props.value ?? '')) {
    return;
  }
  emit('update:value', next);
}

/**
 * 从上传结果解析可插入正文的 URL
 * @param result 上传结果
 */
async function resolveUploadedUrl(result: FileUploadResult): Promise<string> {
  const direct = result.accessUrl?.trim() ?? '';
  if (direct) {
    return direct;
  }
  if (!result.fileId) {
    return '';
  }
  const detail = await getFileById(result.fileId);
  return detail.accessUrl?.trim() ?? '';
}

/**
 * 解析 Umo 传入的文件对象
 * @param file 浏览器 File 或 Umo 包装对象
 */
function unwrapUploadFile(file: File | { file: File }): File {
  return file instanceof File ? file : file.file;
}

/**
 * 图片/附件上传：走现有文件服务，仅把 URL 交给编辑器
 * @param file 待上传文件
 */
async function handleFileUpload(file: File | { file: File }): Promise<{ id: string; url: string }> {
  const blob = unwrapUploadFile(file);
  const result = await uploadTaktFileSmart(blob);
  const url = await resolveUploadedUrl(result);
  if (!url) {
    throw new Error(t('components.common.page.richeditor.imagefail'));
  }
  return {
    id: result.fileId ?? blob.name,
    url,
  };
}

/**
 * 工具栏保存：表单仍由父级提交，此处只同步 HTML
 * @param content Umo 文档内容
 */
async function handleSave(content?: { html?: string } | string): Promise<boolean> {
  const html = typeof content === 'string' ? content : content?.html;
  emitHtml(html ?? readHtml());
  return true;
}

/** 挂载时占位，避免把 placeholder 放进响应式 options */
const initialPlaceholder = props.placeholder || t('components.common.page.richeditor.placeholder');

/**
 * Umo 配置。只传与默认结构兼容的字段；不要传残缺的 page，以免覆盖默认页边距后渲染崩溃。
 * document.content 只取创建时的值，之后靠 setContent 同步。
 */
const editorOptions = {
  editorKey,
  locale: mapUmoLocale(locale.value),
  theme: themeStore.resolvedTheme,
  height: '100%',
  fullscreenZIndex: 2000,
  toolbar: {
    showSaveLabel: false,
    defaultMode: 'classic',
    menus: ['base', 'insert', 'table', 'tools', 'view'],
  },
  document: {
    content: props.value || '',
    placeholder: {
      en_US: initialPlaceholder,
      zh_CN: initialPlaceholder,
    },
    readOnly: props.disabled,
    autofocus: false,
    autoSave: {
      enabled: false,
      interval: 300000,
    },
  },
  onSave: handleSave,
  onFileUpload: handleFileUpload,
  onFileDelete: () => undefined,
};

/**
 * 把外部 HTML 写入编辑器
 * @param html 父级 v-model
 */
function syncExternalContent(html: string): void {
  if (!editorRef.value?.setContent) {
    return;
  }
  const next = html ?? '';
  const current = readHtml();
  if (next === current) {
    return;
  }
  applyingExternal.value = true;
  editorRef.value.setContent(next || '', { emitUpdate: false });
  void nextTick(() => {
    applyingExternal.value = false;
  });
}

/**
 * 编辑器就绪后对齐禁用/语言/主题/工具栏
 */
function applyEditorState(): void {
  const editor = editorRef.value;
  if (!editor) {
    return;
  }
  editor.setToolbar?.({ mode: 'classic', show: true });
  editor.setReadOnly?.(props.disabled);
  editor.setLocale?.(mapUmoLocale(locale.value));
  editor.setTheme?.(themeStore.resolvedTheme);
}

/** 创建完成 */
function handleCreated(): void {
  applyEditorState();
  syncExternalContent(props.value ?? '');
}

/** 内容变化 */
function handleChanged(): void {
  if (applyingExternal.value) {
    return;
  }
  emitHtml(readHtml());
}

/**
 * 容器宽高大于阈值才允许挂载
 */
function syncCanMount(): void {
  const el = hostRef.value;
  canMount.value = Boolean(el && el.clientWidth > 8 && el.clientHeight > 8);
}

let resizeObserver: ResizeObserver | null = null;

onMounted(() => {
  syncCanMount();
  void nextTick(() => {
    syncCanMount();
  });
  if (!hostRef.value || typeof ResizeObserver === 'undefined') {
    canMount.value = true;
    return;
  }
  resizeObserver = new ResizeObserver(() => {
    syncCanMount();
  });
  resizeObserver.observe(hostRef.value);
});

onBeforeUnmount(() => {
  resizeObserver?.disconnect();
  resizeObserver = null;
});

watch(
  () => props.value,
  (html) => {
    syncExternalContent(html ?? '');
  },
);

watch(
  () => props.disabled,
  () => {
    editorRef.value?.setReadOnly?.(props.disabled);
  },
);

watch(
  () => locale.value,
  (culture) => {
    editorRef.value?.setLocale?.(mapUmoLocale(culture));
  },
);

watch(
  () => themeStore.resolvedTheme,
  (theme) => {
    editorRef.value?.setTheme?.(theme);
  },
);
</script>
