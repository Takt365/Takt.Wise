<template>
  <div
    v-if="show"
    class="takt-pagination"
  >
    <a-pagination
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      :show-size-changer="showSizeChanger"
      :show-quick-jumper="computedShowQuickJumper"
      v-bind="showTotalAttr"
      :page-size-options="pageSizeOptions"
      :size="size"
      :simple="simple"
      @change="handleChange"
      @show-size-change="handleShowSizeChange"
    />
  </div>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import {
  getTaktDefaultPageIndex,
  getTaktDefaultPageSize,
  getTaktPageSizeOptions,
} from '@/utils/taktPaged'

const { t } = useI18n()

interface Props {
  /** 是否显示 */
  show?: boolean
  /** 当前页码 */
  current?: number
  /** 每页条数 */
  pageSize?: number
  /** 数据总数 */
  total?: number
  /** 是否显示每页条数选择器 */
  showSizeChanger?: boolean
  /** 是否显示总数 */
  showTotal?: boolean | ((total: number, range: [number, number]) => string)
  /** 是否显示跳转 */
  showJumper?: boolean
  /** 每页条数选项 */
  pageSizeOptions?: string[]
  /** 尺寸 */
  size?: 'default' | 'small'
  /** 简单模式 */
  simple?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  show: true,
  current: () => getTaktDefaultPageIndex(),
  pageSize: () => getTaktDefaultPageSize(),
  total: 0,
  showSizeChanger: true,
  showTotal: true,
  showJumper: true,
  pageSizeOptions: () => [...getTaktPageSizeOptions()],
  size: 'small',
  simple: false
})

const emit = defineEmits<{
  'update:current': [page: number]
  'update:pageSize': [pageSize: number]
  'change': [page: number, pageSize: number]
  'show-size-change': [current: number, size: number]
  'jump': [page: number]
}>()

const currentPage = ref(props.current)
const pageSize = ref(props.pageSize)

// 计算总页数
const totalPages = computed(() => {
  return Math.ceil(props.total / pageSize.value) || 1
})

// 计算 showQuickJumper：根据总页数自动显隐（少于等于5页不显示，6页及以上显示）
const computedShowQuickJumper = computed(() => {
  if (!props.showJumper) {
    return false
  }
  // 总页数 <= 5 时不显示，>= 6 时显示
  return totalPages.value >= 6
})

// 处理 showTotal：根据条件动态返回 show-total 属性（避免在 exactOptionalPropertyTypes 下传递 undefined）
const showTotalAttr = computed(() => {
  if (typeof props.showTotal === 'function') {
    return { 'show-total': props.showTotal }
  }
  if (props.showTotal === true) {
    return { 'show-total': (total: number, _range: [number, number]) => t('common.table.total', { total }) }
  }
  // 当 showTotal 为 false 时，不传递 show-total 属性
  return {}
})

// 监听 props 变化
watch(() => props.current, (val) => {
  currentPage.value = val
})

watch(() => props.pageSize, (val) => {
  pageSize.value = val
})

const handleChange = (page: number, size: number) => {
  currentPage.value = page
  pageSize.value = size
  emit('update:current', page)
  emit('update:pageSize', size)
  emit('change', page, size)
}

const handleShowSizeChange = (current: number, size: number) => {
  currentPage.value = current
  pageSize.value = size
  emit('update:current', current)
  emit('update:pageSize', size)
  emit('show-size-change', current, size)
}
</script>

<style scoped>
.takt-pagination {
  margin: 0 4px 4px 4px;
  display: flex;
  justify-content: flex-end;
}
</style>
