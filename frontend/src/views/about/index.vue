<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：@/views/about -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：关于页面，系统信息与依赖列表（表头分组） -->
<!-- ======================================== -->

<template>
  <div class="about">
    <a-descriptions
      :column="1"
      bordered
    >
      <a-descriptions-item :label="t('about.page.systemname')">
        {{ t('common.page.app.name') }}
      </a-descriptions-item>
      <a-descriptions-item :label="t('about.page.systemversion')">
        V{{ appInfo.pkg.version }}
      </a-descriptions-item>
      <a-descriptions-item :label="t('about.page.productcode')">
        {{ t('common.page.app.productcode') }}
      </a-descriptions-item>
      <a-descriptions-item :label="t('about.page.projectdoc')">
        <a
          :href="repoUrl"
          target="_blank"
          rel="noopener noreferrer"
          class="about-link"
        ><span class="about-link-inner"><RiFileTextLine />{{ t('about.page.doclink') }}</span></a>
        <a
          :href="licenseUrl"
          target="_blank"
          rel="noopener noreferrer"
          class="about-link"
        ><span class="about-link-inner"><RiCopyrightLine />{{ t('about.page.license') }}</span></a>
        <router-link
          to="/privacy"
          class="about-link"
        >
          <span class="about-link-inner"><RiShieldLine />{{ t('about.page.privacy') }}</span>
        </router-link>
        <router-link
          to="/terms"
          class="about-link"
        >
          <span class="about-link-inner"><RiFileList3Line />{{ t('about.page.terms') }}</span>
        </router-link>
      </a-descriptions-item>
    </a-descriptions>

    <a-table
      :columns="columns"
      :data-source="tableData"
      bordered
      size="small"
      :pagination="false"
      class="dep-table"
    />
  </div>
</template>

<script setup lang="ts">
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { RiFileTextLine, RiCopyrightLine, RiShieldLine, RiFileList3Line } from '@remixicon/vue'
import { appInfo } from '@/utils/appMeta'

const { t } = useI18n()

const repoUrl = 'https://github.com/Lean365/Takt.Net'
const licenseUrl = 'https://github.com/Lean365/Takt.Net/blob/master/LICENSE'

const columns = computed<TableColumnsType>(() => [
  {
    title: t('about.page.depgroup'),
    children: [
      { title: t('about.page.depname'), dataIndex: 'depName1', key: 'depName1', ellipsis: true },
      { title: t('about.page.depversion'), dataIndex: 'depVer1', key: 'depVer1', ellipsis: true },
      { title: t('about.page.depname'), dataIndex: 'depName2', key: 'depName2', ellipsis: true },
      { title: t('about.page.depversion'), dataIndex: 'depVer2', key: 'depVer2', ellipsis: true }
    ]
  },
  {
    title: t('about.page.devgroup'),
    children: [
      { title: t('about.page.depname'), dataIndex: 'devName1', key: 'devName1', ellipsis: true },
      { title: t('about.page.depversion'), dataIndex: 'devVer1', key: 'devVer1', ellipsis: true },
      { title: t('about.page.depname'), dataIndex: 'devName2', key: 'devName2', ellipsis: true },
      { title: t('about.page.depversion'), dataIndex: 'devVer2', key: 'devVer2', ellipsis: true }
    ]
  }
])

const toRows = (record: Record<string, string>) => {
  const list = Object.entries(record).map(([name, version]) => ({ name, version }))
  const rows: { name1: string; version1: string; name2: string; version2: string }[] = []
  for (let i = 0; i < list.length; i += 2) {
    const a = list[i]
    const b = list[i + 1]
    rows.push({
      name1: a?.name ?? '',
      version1: a?.version ?? '',
      name2: b?.name ?? '',
      version2: b?.version ?? ''
    })
  }
  return rows
}

const tableData = computed(() => {
  const depRows = toRows(appInfo.pkg.dependencies)
  const devRows = toRows(appInfo.pkg.devDependencies)
  const len = Math.max(depRows.length, devRows.length, 1)
  return Array.from({ length: len }, (_, i) => {
    const dep = depRows[i] ?? { name1: '', version1: '', name2: '', version2: '' }
    const dev = devRows[i] ?? { name1: '', version1: '', name2: '', version2: '' }
    return {
      key: i,
      depName1: dep.name1,
      depVer1: dep.version1,
      depName2: dep.name2,
      depVer2: dep.version2,
      devName1: dev.name1,
      devVer1: dev.version1,
      devName2: dev.name2,
      devVer2: dev.version2
    }
  })
})
</script>

<style scoped lang="css">
.about {
  padding: 24px;
}
.about-link-inner {
  display: inline-flex;
  align-items: center;
  gap: 8px;
}
.about :deep(.ant-descriptions-item-content) a + a {
  margin-left: 12px;
}
.dep-table {
  margin-top: 16px;
  :deep(.ant-table) {
    font-size: 12px;
  }
}
</style>
