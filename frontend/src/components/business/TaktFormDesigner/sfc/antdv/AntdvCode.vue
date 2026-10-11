<template>
  <div class="antdv-code">
    <div class="antdv-code__preview takt-code-preview-panel">
      <pre class="antdv-code__pre"><code
        ref="codeRef"
        class="hljs language-xml"
      /></pre>
    </div>
    <a-button
      v-if="showDownload"
      type="primary"
      class="antdv-code__download"
      @click="onDownload"
    >
      下载 .vue
    </a-button>
  </div>
</template>

<script setup lang="ts">
import hljs from 'highlight.js'

const props = withDefaults(
  defineProps<{
    content: string
    showDownload?: boolean
    downloadFilename?: string
    rows?: number
  }>(),
  { showDownload: true, downloadFilename: 'FormComponent.antd.vue', rows: 16 }
)

const codeRef = ref<HTMLElement | null>(null)
const highlightedContent = computed(() => {
  const code = props.content ?? ''
  return code.trim() ? code : ''
})

function renderHighlightedCode() {
  const element = codeRef.value
  if (!element) return
  element.textContent = highlightedContent.value
  hljs.highlightElement(element)
}

watch(highlightedContent, () => {
  nextTick(() => renderHighlightedCode())
}, { immediate: true })

function onDownload() {
  const blob = new Blob([props.content], { type: 'text/plain;charset=utf-8' })
  const url = URL.createObjectURL(blob)
  const a = document.createElement('a')
  a.href = url
  a.download = props.downloadFilename
  a.click()
  URL.revokeObjectURL(url)
}
</script>

<style scoped>
.antdv-code {
  display: flex;
  flex-direction: column;
  flex: 1;
  min-width: 0;
  .antdv-code__preview {
    flex: 1;
    min-height: 200px;
    overflow: auto;
    border: 1px solid var(--ant-color-border);
    border-radius: var(--ant-border-radius);
  }
  .antdv-code__pre {
    margin: 0;
    padding: 0;
    font-size: 12px;
    line-height: 1.5;
  }
  .antdv-code__pre code {
    font-family: ui-monospace, 'SF Mono', Monaco, 'Cascadia Code', monospace;
  }
  .antdv-code__download {
    margin-top: 8px;
  }
}
</style>

