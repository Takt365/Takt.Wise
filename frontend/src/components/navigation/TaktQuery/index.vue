<!-- ========================================
项目名称:TaktWise
命名空间:@/components/navigation/TaktQuery
文件名称:index.vue
创建时间:2025-01-20
创建人:Takt365(Cursor AI)
功能描述:菜单搜索组件,用于快速查找并跳转菜单

版权信息:Copyright (c) 2025 Takt  All rights reserved.
免责声明:此软件使用 MIT License,作者不承担任何使用风险。
======================================== -->
<template>
  <a-input-search
    v-model:value="query"
    :placeholder="searchPlaceholder"
    class="takt-query"
    allow-clear
    @search="handleSearch"
    @change="handleChange"
  >
    <template #prefix>
      <RiSearchLine class="takt-remix-icon" />
    </template>
  </a-input-search>
  <a-dropdown
    v-model:open="dropdownVisible"
    :trigger="['click']"
    placement="bottomLeft"
    class="takt-query-dropdown"
  >
    <template #overlay>
      <a-menu v-if="searchResults.length > 0">
        <a-menu-item
          v-for="item in searchResults"
          :key="item.path"
          @click="handleSelect(item.path)"
        >
          <div class="search-result-item">
            <span class="result-title">{{ item.title }}</span>
            <span class="result-path">{{ item.path }}</span>
          </div>
        </a-menu-item>
      </a-menu>
      <div
        v-else
        class="no-results"
      >
        {{ $t('common.status.search.empty') }}
      </div>
    </template>
  </a-dropdown>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import { useMenuStore } from '@/stores/identity/useMenuStore'
import { RiSearchLine } from '@remixicon/vue'

const { t } = useI18n()

/** 菜单搜索占位：参数化 search + entity.menu（非 common 抽象类） */
const searchPlaceholder = computed(() =>
  t('common.page.form.placeholder.search', {
    keyword: t('entity.menu.name') + t('common.tip.or') + t('entity.menu.code'),
  }),
)

interface SearchResult {
  title: string
  path: string
}
type MenuSearchItem = {
  title?: string
  label?: string
  key?: string
  children?: MenuSearchItem[]
}
function isMenuSearchItem(value: unknown): value is MenuSearchItem {
  return !!value && typeof value === 'object'
}

const router = useRouter()
const menuStore = useMenuStore()

const query = ref('')
const dropdownVisible = ref(false)

const searchResults = computed(() => {
  if (!query.value.trim()) {
    return []
  }
  
  const results: SearchResult[] = []
  const searchText = query.value.toLowerCase()
  
  const searchInMenu = (items: unknown[]) => {
    items.forEach(item => {
      if (!isMenuSearchItem(item)) return
      const title = item.title || item.label || ''
      const path = item.key || ''
      
      if (title.toLowerCase().includes(searchText) || path.toLowerCase().includes(searchText)) {
        results.push({
          title,
          path
        })
      }
      
      if (item.children && item.children.length > 0) {
        searchInMenu(item.children)
      }
    })
  }
  
  searchInMenu(menuStore.menuItems ?? [])
  
  return results.slice(0, 10) // 限制最多显示10个结果
})

watch(searchResults, (results) => {
  dropdownVisible.value = results.length > 0 && query.value.trim() !== ''
})

const handleSearch = (_value: string) => {
  const firstResult = searchResults.value[0]
  if (firstResult) {
    handleSelect(firstResult.path)
  }
}

const handleChange = () => {
  // 输入变化时自动显示下拉
}

const handleSelect = (path: string) => {
  router.push(path)
  query.value = ''
  dropdownVisible.value = false
}
</script>

<style scoped>
.takt-query {
  width: 300px;
}

.takt-query-dropdown {
  .search-result-item {
    display: flex;
    flex-direction: column;
    
    .result-title {
      font-weight: 500;
    }
    
    .result-path {
      font-size: 12px;
      color: rgba(0, 0, 0, 0.45);
      margin-top: 2px;
    }
  }
  
  .no-results {
    padding: 12px;
    text-align: center;
    color: rgba(0, 0, 0, 0.45);
  }
}
</style>
