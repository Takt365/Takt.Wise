<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/layouts -->
<!-- 文件名称：TaktCachedRouterView.vue -->
<!-- 功能描述：布局内页面出口。只缓存仍打开的标签；切换不卸载，关闭才销毁 -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <router-view v-slot="{ Component, route: currentRoute }">
    <keep-alive :include="includeNames">
      <component
        :is="Component ? resolveCachedPage(currentRoute.path, Component) : undefined"
        v-if="Component"
        :key="currentRoute.path"
      />
    </keep-alive>
  </router-view>
</template>

<script setup lang="ts">
/**
 * 缓存键是路由 path，不是 fullPath。include 只含仍打开的标签；关掉的页从名单移除后由 keep-alive 销毁。
 * 外层 :key 为租户+公司时，隔离域变化会整段丢弃这些实例。
 */
import type { Component } from 'vue'
import { storeToRefs } from 'pinia'
import { useOpenPageCacheStore } from '@/stores/navigation/useOpenPageCacheStore'

defineOptions({ name: 'TaktCachedRouterView' })

const route = useRoute()
const openPageCacheStore = useOpenPageCacheStore()
const { paths: openPaths } = storeToRefs(openPageCacheStore)
/** 每个 path 一份稳定包装，名字唯一，供 keep-alive include 命中 */
const pageWrappers = new Map<string, Component>()

/**
 * keep-alive 按组件名匹配；path 里的符号换成下划线
 * @param path 路由 path
 * @returns 包装组件名
 */
function tabCacheName(path: string): string {
  const safe = path.replace(/[^a-zA-Z0-9]/g, '_')
  return `TaktTab${safe}`
}

/**
 * 当前页加上标签栏仍打开的 path
 * @returns include 名单
 */
const includeNames = computed(() => {
  const names = new Set<string>()
  for (const path of openPaths.value) {
    names.add(tabCacheName(path))
  }
  if (route.path) {
    names.add(tabCacheName(route.path))
  }
  return [...names]
})

/**
 * 为该 path 取稳定包装。包装只创建一次，里面的页面实例由 keep-alive 保住。
 * @param path 路由 path
 * @param page 路由组件
 * @returns 带唯一 name 的包装
 */
function resolveCachedPage(path: string, page: Component): Component {
  const cached = pageWrappers.get(path)
  if (cached) {
    return cached
  }
  const inner = markRaw(page)
  const wrapped = markRaw(defineComponent({
    name: tabCacheName(path),
    setup() {
      return () => h(inner)
    },
  }))
  pageWrappers.set(path, wrapped)
  return wrapped
}
</script>
