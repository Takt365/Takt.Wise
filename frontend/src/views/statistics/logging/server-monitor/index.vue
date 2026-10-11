<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/statistics/logging/server-monitor -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：服务监控页面，展示应用状态与硬件信息；权限 statistics:logging:server:monitor:list -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="statistics-logging-server-monitor p-4 flex flex-col gap-4">
    <!-- 页面标题 -->
    <h2 class="text-xl font-semibold m-0">{{ t('statistics.logging.server-monitor.page.title') }}</h2>

    <!-- 工具栏 -->
    <TaktToolsBar
      :show-create="false"
      :show-update="false"
      :show-delete="false"
      :show-import="false"
      :show-export="false"
      :show-advanced-query="false"
      :show-column-setting="false"
      :show-expand="false"
      :show-fullscreen="true"
      :show-refresh="true"
      :refresh-loading="loading"
      @refresh="loadData"
    />

    <a-spin :spinning="loading">
      <a-tabs v-model:active-key="activeTab" type="card">
        <!-- 应用状态 -->
        <a-tab-pane
          key="app"
          :tab="t('statistics.logging.server-monitor.page.tabs.app')"
        >
          <a-descriptions bordered :column="2" size="small" class="pt-2">
            <a-descriptions-item :label="t('statistics.logging.server-monitor.page.field.application.name')">
              {{ appStatus.applicationName }}
            </a-descriptions-item>
            <a-descriptions-item :label="t('statistics.logging.server-monitor.page.field.application.version')">
              <a-tag color="blue">{{ appStatus.applicationVersion }}</a-tag>
            </a-descriptions-item>
            <a-descriptions-item :label="t('statistics.logging.server-monitor.page.field.environment')">
              <a-tag :color="envColor">{{ appStatus.environment }}</a-tag>
            </a-descriptions-item>
            <a-descriptions-item :label="t('statistics.logging.server-monitor.page.field.machine.name')">
              {{ appStatus.machineName }}
            </a-descriptions-item>
            <a-descriptions-item :label="t('statistics.logging.server-monitor.page.field.dot.net.version')">
              {{ appStatus.dotNetVersion }}
            </a-descriptions-item>
            <a-descriptions-item :label="t('statistics.logging.server-monitor.page.field.process.architecture')">
              {{ appStatus.processArchitecture || '-' }}
            </a-descriptions-item>
            <a-descriptions-item :label="t('statistics.logging.server-monitor.page.field.processor.count')">
              {{ appStatus.processorCount }} {{ t('statistics.logging.server-monitor.page.unit.core') }}
            </a-descriptions-item>
            <a-descriptions-item :label="t('statistics.logging.server-monitor.page.field.start.time')">
              {{ formatDateTime(appStatus.startTime) }}
            </a-descriptions-item>
            <a-descriptions-item :label="t('statistics.logging.server-monitor.page.field.uptime')">
              {{ formatUptime(appStatus.uptime) }}
            </a-descriptions-item>
            <a-descriptions-item :label="t('statistics.logging.server-monitor.page.field.working.set')">
              {{ formatBytes(Number(appStatus.workingSet) || 0) }}
            </a-descriptions-item>
          </a-descriptions>
        </a-tab-pane>

        <!-- 操作系统与语言 -->
        <a-tab-pane
          key="system"
          :tab="t('statistics.logging.server-monitor.page.tabs.system')"
        >
          <a-descriptions bordered :column="2" size="small" class="pt-2">
            <a-descriptions-item :label="t('statistics.logging.server-monitor.page.field.operating.system')" :span="2">
              {{ hardwareInfo.operatingSystem }}
            </a-descriptions-item>
            <a-descriptions-item :label="t('statistics.logging.server-monitor.page.field.os.version')">
              {{ hardwareInfo.operatingSystemLanguage.osVersion || '-' }}
            </a-descriptions-item>
            <a-descriptions-item :label="t('statistics.logging.server-monitor.page.field.current.culture')">
              {{ hardwareInfo.operatingSystemLanguage.currentCultureDisplayName }}
            </a-descriptions-item>
            <a-descriptions-item :label="t('statistics.logging.server-monitor.page.field.current.ui.culture')">
              {{ hardwareInfo.operatingSystemLanguage.currentUICultureDisplayName }}
            </a-descriptions-item>
            <a-descriptions-item :label="t('statistics.logging.server-monitor.page.field.system.type')">
              {{ hardwareInfo.osArchitecture || '-' }}
            </a-descriptions-item>
          </a-descriptions>
          <a-divider class="my-3" />
          <a-descriptions bordered :column="2" size="small" :title="t('statistics.logging.server-monitor.page.section.motherboard')">
            <a-descriptions-item :label="t('statistics.logging.server-monitor.page.field.motherboard.manufacturer')">
              {{ hardwareInfo.motherboard.manufacturer || '-' }}
            </a-descriptions-item>
            <a-descriptions-item :label="t('statistics.logging.server-monitor.page.field.motherboard.product')">
              {{ hardwareInfo.motherboard.product || '-' }}
            </a-descriptions-item>
            <a-descriptions-item :label="t('statistics.logging.server-monitor.page.field.motherboard.serial.number')">
              {{ hardwareInfo.motherboard.serialNumber || '-' }}
            </a-descriptions-item>
            <a-descriptions-item :label="t('statistics.logging.server-monitor.page.field.motherboard.version')">
              {{ hardwareInfo.motherboard.version || '-' }}
            </a-descriptions-item>
            <a-descriptions-item :label="t('statistics.logging.server-monitor.page.field.motherboard.uuid')" :span="2">
              {{ hardwareInfo.motherboard.uuid || '-' }}
            </a-descriptions-item>
          </a-descriptions>
        </a-tab-pane>

        <!-- CPU -->
        <a-tab-pane
          key="cpu"
          :tab="t('statistics.logging.server-monitor.page.tabs.cpu')"
        >
          <div class="pt-2">
            <div class="flex flex-col items-center gap-2 pb-2 max-w-[280px] mx-auto">
              <span class="text-sm text-text-secondary">
                {{ t('statistics.logging.server-monitor.page.field.cpu.usage.percent') }}
              </span>
              <div ref="cpuUsageGaugeRef" class="h-[260px] w-full max-w-[320px]" />
            </div>
            <a-divider />
            <a-table
              v-model:expanded-row-keys="expandedCpuRowKeys"
              :columns="cpuColumns"
              :data-source="hardwareInfo.cpuList"
              :pagination="false"
              :row-key="getCpuRowKey"
              size="small"
            >
              <template #bodyCell="{ column, record }">
                <template v-if="column.key === 'numberOfCores'">
                  {{ record.numberOfCores }} {{ t('statistics.logging.server-monitor.page.unit.core') }}
                </template>
                <template v-if="column.key === 'numberOfLogicalProcessors'">
                  {{ record.numberOfLogicalProcessors }} {{ t('statistics.logging.server-monitor.page.unit.thread') }}
                </template>
                <template v-if="column.key === 'socketDesignation'">
                  {{ record.socketDesignation || '-' }}
                </template>
                <template v-if="column.key === 'usagePercent'">
                  <a-progress
                    :percent="clampUsagePercent(Number(record.usagePercent) || 0)"
                    size="small"
                    :stroke-color="getCpuUsageColor(Number(record.usagePercent) || 0)"
                  />
                </template>
              </template>
              <template #expandedRowRender="{ record }">
                <a-table
                  :columns="cpuCoreColumns"
                  :data-source="record.coreList ?? []"
                  :pagination="false"
                  :row-key="getCpuCoreRowKey"
                  size="small"
                >
                  <template #bodyCell="{ column, record: coreRecord }">
                    <template v-if="column.key === 'usagePercent'">
                      <a-progress
                        :percent="clampUsagePercent(Number(coreRecord.usagePercent) || 0)"
                        size="small"
                        :stroke-color="getCpuUsageColor(Number(coreRecord.usagePercent) || 0)"
                      />
                    </template>
                  </template>
                </a-table>
              </template>
            </a-table>
          </div>
        </a-tab-pane>

        <!-- 内存 -->
        <a-tab-pane
          key="memory"
          :tab="t('statistics.logging.server-monitor.page.tabs.memory')"
        >
          <div class="pt-2">
            <div class="flex flex-col items-center gap-2 pb-2 max-w-[280px] mx-auto">
              <span class="text-sm text-text-secondary">
                {{ t('statistics.logging.server-monitor.page.field.memory.usage.percent') }}
              </span>
              <div ref="memoryUsageGaugeRef" class="h-[260px] w-full max-w-[320px]" />
            </div>
            <a-divider />
            <a-table
              v-model:expanded-row-keys="expandedMemoryRowKeys"
              :columns="memoryColumns"
              :data-source="memorySummaryList"
              :expandable="memoryTableExpandable"
              :pagination="false"
              row-key="key"
              size="small"
            >
              <template #bodyCell="{ column, record }">
                <template v-if="column.key === 'totalSize'">
                  {{ formatBytes(toByteNumber(record.totalSize)) }}
                </template>
                <template v-if="column.key === 'usedSpace'">
                  {{ formatBytes(toByteNumber(record.usedSpace)) }}
                </template>
                <template v-if="column.key === 'freeSpace'">
                  {{ formatBytes(toByteNumber(record.freeSpace)) }}
                </template>
                <template v-if="column.key === 'usagePercent'">
                  <a-progress
                    :percent="clampUsagePercent(Number(record.usagePercent) || 0)"
                    size="small"
                    :stroke-color="getMemoryUsageColor(Number(record.usagePercent) || 0)"
                  />
                </template>
              </template>
              <template #expandedRowRender>
                <a-table
                  :columns="memoryModuleColumns"
                  :data-source="hardwareInfo.memoryModuleList"
                  :pagination="false"
                  :row-key="getMemoryModuleRowKey"
                  size="small"
                >
                  <template #bodyCell="{ column, record }">
                    <template v-if="column.key === 'capacity'">
                      {{ formatBytes(toByteNumber(record.capacity)) }}
                    </template>
                    <template v-if="column.key === 'speed'">
                      {{ formatMemoryModuleSpeed(record.speed) }}
                    </template>
                    <template v-if="column.key === 'manufacturer'">
                      {{ record.manufacturer || '-' }}
                    </template>
                    <template v-if="column.key === 'partNumber'">
                      {{ record.partNumber || '-' }}
                    </template>
                    <template v-if="column.key === 'serialNumber'">
                      {{ record.serialNumber || '-' }}
                    </template>
                  </template>
                </a-table>
              </template>
            </a-table>
          </div>
        </a-tab-pane>

        <!-- 显卡 -->
        <a-tab-pane
          key="gpu"
          :tab="t('statistics.logging.server-monitor.page.tabs.gpu')"
        >
          <a-table
            class="pt-2"
            :columns="gpuColumns"
            :data-source="hardwareInfo.gpuList"
            :pagination="false"
            size="small"
            :row-key="getGpuRowKey"
          >
            <template #bodyCell="{ column, record }">
              <template v-if="column.key === 'adapterRam'">
                {{ formatGpuAdapterRam(record.adapterRam) }}
              </template>
              <template v-if="column.key === 'driverVersion'">
                {{ record.driverVersion?.trim() || '-' }}
              </template>
              <template v-if="column.key === 'manufacturer'">
                {{ record.manufacturer?.trim() || '-' }}
              </template>
            </template>
          </a-table>
        </a-tab-pane>

        <!-- 磁盘 -->
        <a-tab-pane
          key="drive"
          :tab="t('statistics.logging.server-monitor.page.tabs.drive')"
        >
          <a-table
            class="pt-2"
            :columns="driveColumns"
            :data-source="hardwareInfo.driveList"
            :pagination="false"
            size="small"
            row-key="name"
          >
            <template #bodyCell="{ column, record }">
              <template v-if="column.key === 'totalSize'">
                {{ formatBytes(toByteNumber(record.totalSize)) }}
              </template>
              <template v-if="column.key === 'freeSpace'">
                {{ formatBytes(toByteNumber(record.freeSpace)) }}
              </template>
              <template v-if="column.key === 'usedSpace'">
                {{ formatBytes(toByteNumber(record.usedSpace)) }}
              </template>
              <template v-if="column.key === 'usagePercent'">
                <a-progress
                  :percent="calcUsagePercent(toByteNumber(record.usedSpace), toByteNumber(record.totalSize))"
                  size="small"
                  :stroke-color="getDriveUsageColor(calcUsagePercent(toByteNumber(record.usedSpace), toByteNumber(record.totalSize)))"
                />
              </template>
            </template>
          </a-table>
        </a-tab-pane>

        <!-- 网络 -->
        <a-tab-pane
          key="network"
          :tab="t('statistics.logging.server-monitor.page.tabs.network')"
        >
          <a-table
            class="pt-2"
            :columns="networkColumns"
            :data-source="hardwareInfo.networkAdapterList"
            :pagination="false"
            size="small"
            row-key="name"
          >
            <template #bodyCell="{ column, record }">
              <template v-if="column.key === 'name'">
                <div class="flex flex-col gap-0.5">
                  <span>{{ formatNetworkAdapterPrimary(record.name, record.description) }}</span>
                  <span
                    v-if="shouldShowNetworkDescription(record.name, record.description)"
                    class="text-xs text-text-secondary"
                  >
                    {{ record.description?.trim() }}
                  </span>
                </div>
              </template>
              <template v-if="column.key === 'macAddress'">
                {{ record.macAddress || '-' }}
              </template>
              <template v-if="column.key === 'ipAddress'">
                {{ record.ipAddress || '-' }}
              </template>
              <template v-if="column.key === 'speed'">
                {{ formatNetworkSpeed(record.speed) }}
              </template>
              <template v-if="column.key === 'status'">
                <a-tag :color="getNetworkStatusColor(record.status)">
                  {{ formatNetworkStatus(record.status) }}
                </a-tag>
              </template>
            </template>
          </a-table>
        </a-tab-pane>
      </a-tabs>
    </a-spin>
  </div>
</template>

<script setup lang="ts">
/**
 * 服务监控页面
 * 展示应用运行状态与服务器硬件信息
 */
import { message } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import dayjs from 'dayjs'
import duration from 'dayjs/plugin/duration'
import * as echarts from 'echarts/core'
import { GaugeChart } from 'echarts/charts'
import { TooltipComponent } from 'echarts/components'
import { CanvasRenderer } from 'echarts/renderers'
import type { ECharts } from 'echarts/core'
import { useTaktComponentLocale } from '@/composables/useTaktComponentLocale'
import { usePermissionStore } from '@/stores/identity/usePermissionStore'
import {
  getAppStatus,
  getServerHardware,
} from '@/api/statistics/logging/serverMonitor.api'
import type { AppStatus, CpuCoreInfo, CpuInfo, GpuInfo, MemoryModule, ServerHardware } from '@/types/statistics/logging/serverMonitor.types'

dayjs.extend(duration)

echarts.use([GaugeChart, TooltipComponent, CanvasRenderer])

const { t, locale } = useI18n()
/** ECharts 语言键（随 vue-i18n 同步） */
const { echartsLocale } = useTaktComponentLocale()
/** 权限 Store */
const permissionStore = usePermissionStore()

/** 列表/query 权限（GET hardware、GET app-status） */
const PERMISSION_LIST = 'statistics:logging:server:monitor:list'

/** CPU 汇总使用率仪表盘容器 */
const cpuUsageGaugeRef = ref<HTMLDivElement | null>(null)
/** CPU 仪表盘容器 ResizeObserver */
let cpuUsageGaugeResizeObserver: ResizeObserver | null = null
/** 内存汇总使用率仪表盘容器 */
const memoryUsageGaugeRef = ref<HTMLDivElement | null>(null)
/** 内存仪表盘容器 ResizeObserver */
let memoryUsageGaugeResizeObserver: ResizeObserver | null = null
/** CPU 仪表盘实例 */
const cpuUsageGaugeChartRef = { chart: null as ECharts | null }
/** 内存仪表盘实例 */
const memoryUsageGaugeChartRef = { chart: null as ECharts | null }

/** 数据加载状态 */
const loading = ref(false)
/** 当前 Tab（app / system / cpu / memory / drive / network） */
const activeTab = ref('app')
/** 已展开的物理 CPU 行（展示逻辑核心子表） */
const expandedCpuRowKeys = ref<string[]>([])
/** 已展开的物理内存行（展示内存条子表） */
const expandedMemoryRowKeys = ref<string[]>([])

/** 服务器硬件信息 */
const hardwareInfo = ref<ServerHardware>({
  hostSerialNumber: '',
  driveSerialNumber: '',
  macAddress: '',
  cpuModel: '',
  cpuUsagePercent: 0,
  osArchitecture: '',
  operatingSystem: '',
  operatingSystemLanguage: {
    currentCulture: '',
    currentCultureDisplayName: '',
    currentCultureNativeName: '',
    currentUICulture: '',
    currentUICultureDisplayName: '',
    currentUICultureNativeName: '',
    systemDefaultLanguage: '',
    osVersion: '',
    installedLanguages: [],
  },
  motherboard: {
    manufacturer: '',
    product: '',
    serialNumber: '',
    version: '',
    uuid: '',
  },
  bios: {
    manufacturer: '',
    version: '',
    releaseDate: '',
    serialNumber: '',
  },
  cpuList: [],
  gpuList: [],
  memory: {
    totalPhysicalMemory: 0,
    availablePhysicalMemory: 0,
    usedPhysicalMemory: 0,
    totalVirtualMemory: 0,
    availableVirtualMemory: 0,
    usedVirtualMemory: 0,
  },
  memoryModuleList: [],
  driveList: [],
  networkAdapterList: [],
  computerSystemList: [],
})

/** 应用运行状态 */
const appStatus = ref<AppStatus>({
  applicationName: '',
  applicationVersion: '',
  environment: '',
  machineName: '',
  startTime: '',
  uptime: '',
  dotNetVersion: '',
  workingSet: '',
  processorCount: 0,
  processArchitecture: '',
})

/** 物理内存使用率（%）；饼图与汇总列表共用 */
const memoryUsagePercent = computed(() =>
  calcUsagePercent(hardwareInfo.value.memory.usedPhysicalMemory, hardwareInfo.value.memory.totalPhysicalMemory),
)

/** 内存汇总列表（物理 / 虚拟） */
const memorySummaryList = computed(() => {
  const memory = hardwareInfo.value.memory
  return [
    {
      key: 'physical',
      memoryType: t('statistics.logging.server-monitor.page.field.memory.type.physical'),
      totalSize: memory.totalPhysicalMemory,
      usedSpace: memory.usedPhysicalMemory,
      freeSpace: memory.availablePhysicalMemory,
      usagePercent: calcUsagePercent(memory.usedPhysicalMemory, memory.totalPhysicalMemory),
    },
    {
      key: 'virtual',
      memoryType: t('statistics.logging.server-monitor.page.field.memory.type.virtual'),
      totalSize: memory.totalVirtualMemory,
      usedSpace: memory.usedVirtualMemory,
      freeSpace: memory.availableVirtualMemory,
      usagePercent: calcUsagePercent(memory.usedVirtualMemory, memory.totalVirtualMemory),
    },
  ]
})

/** 内存汇总表：仅物理内存行可展开内存条子表 */
const memoryTableExpandable = computed(() => ({
  rowExpandable: (record: { key: string }) =>
    record.key === 'physical' && (hardwareInfo.value.memoryModuleList?.length ?? 0) > 0,
}))

/** CPU 平均使用率（%） */
const cpuUsagePercent = computed(() => Number(hardwareInfo.value.cpuUsagePercent) || 0)

/** 环境标签颜色 */
const envColor = computed(() => {
  switch (appStatus.value.environment) {
    case 'Development':
      return 'green'
    case 'Staging':
      return 'orange'
    case 'Production':
      return 'red'
    default:
      return 'default'
  }
})

/** CPU 表格列（每行 = 一个物理 CPU / Socket） */
const cpuColumns = computed(() => [
  { title: t('statistics.logging.server-monitor.page.field.cpu.name'), dataIndex: 'name', key: 'name' },
  { title: t('statistics.logging.server-monitor.page.field.cpu.manufacturer'), dataIndex: 'manufacturer', key: 'manufacturer' },
  { title: t('statistics.logging.server-monitor.page.field.cpu.cores'), dataIndex: 'numberOfCores', key: 'numberOfCores' },
  { title: t('statistics.logging.server-monitor.page.field.cpu.logical.processors'), dataIndex: 'numberOfLogicalProcessors', key: 'numberOfLogicalProcessors' },
  { title: t('statistics.logging.server-monitor.page.field.cpu.socket'), dataIndex: 'socketDesignation', key: 'socketDesignation' },
  { title: t('statistics.logging.server-monitor.page.field.cpu.usage.percent'), dataIndex: 'usagePercent', key: 'usagePercent', width: 180 },
  { title: t('statistics.logging.server-monitor.page.field.cpu.processor.id'), dataIndex: 'processorId', key: 'processorId' },
])

/** 逻辑核心子表列（CpuCoreList） */
const cpuCoreColumns = computed(() => [
  { title: t('statistics.logging.server-monitor.page.field.cpu.logical.core.name'), dataIndex: 'name', key: 'name', width: 120 },
  { title: t('statistics.logging.server-monitor.page.field.cpu.usage.percent'), dataIndex: 'usagePercent', key: 'usagePercent', width: 180 },
])

/** 内存汇总表格列 */
const memoryColumns = computed(() => [
  { title: t('statistics.logging.server-monitor.page.field.memory.type'), dataIndex: 'memoryType', key: 'memoryType' },
  { title: t('statistics.logging.server-monitor.page.field.memory.total.physical'), dataIndex: 'totalSize', key: 'totalSize' },
  { title: t('statistics.logging.server-monitor.page.field.memory.used.physical'), dataIndex: 'usedSpace', key: 'usedSpace' },
  { title: t('statistics.logging.server-monitor.page.field.memory.available'), dataIndex: 'freeSpace', key: 'freeSpace' },
  { title: t('statistics.logging.server-monitor.page.field.memory.usage.percent'), dataIndex: 'usagePercent', key: 'usagePercent', width: 180 },
])

/** 物理内存条子表列 */
const memoryModuleColumns = computed(() => [
  { title: t('statistics.logging.server-monitor.page.field.memory.bank.label'), dataIndex: 'bankLabel', key: 'bankLabel' },
  { title: t('statistics.logging.server-monitor.page.field.memory.manufacturer'), dataIndex: 'manufacturer', key: 'manufacturer' },
  { title: t('statistics.logging.server-monitor.page.field.memory.capacity'), dataIndex: 'capacity', key: 'capacity' },
  { title: t('statistics.logging.server-monitor.page.field.memory.speed'), dataIndex: 'speed', key: 'speed' },
  { title: t('statistics.logging.server-monitor.page.field.memory.part.number'), dataIndex: 'partNumber', key: 'partNumber' },
  { title: t('statistics.logging.server-monitor.page.field.memory.serial.number'), dataIndex: 'serialNumber', key: 'serialNumber' },
])

/**
 * 物理 CPU 表格 row-key（仅依据 record 字段，不使用 index）
 * @param record CPU 行
 * @returns 唯一键
 */
function getCpuRowKey(record: CpuInfo): string {
  const processorId = String(record.processorId ?? '').trim()
  if (processorId) {
    return processorId
  }
  const composite = [
    String(record.socketDesignation ?? '').trim(),
    String(record.name ?? '').trim(),
    String(record.manufacturer ?? '').trim(),
  ].filter(Boolean).join('|')
  return composite || 'cpu-unknown'
}

/**
 * 逻辑核心子表 row-key
 * @param core 逻辑核心行
 * @returns 唯一键
 */
function getCpuCoreRowKey(core: CpuCoreInfo): string {
  return String(core.name ?? '').trim() || 'core-unknown'
}

/**
 * 内存条子表 row-key（仅依据 record 字段，不使用 index）
 * @param module 内存条行
 * @returns 唯一键
 */
function getMemoryModuleRowKey(module: MemoryModule): string {
  const bankLabel = String(module.bankLabel ?? '').trim()
  if (bankLabel) {
    return bankLabel
  }
  const serialNumber = String(module.serialNumber ?? '').trim()
  if (serialNumber) {
    return serialNumber
  }
  const composite = [
    String(module.manufacturer ?? '').trim(),
    String(module.partNumber ?? '').trim(),
    String(module.capacity ?? ''),
    String(module.speed ?? ''),
  ].filter(Boolean).join('|')
  return composite || 'memory-module-unknown'
}

/**
 * 显卡表格 row-key（仅依据 record 字段，不使用 index）
 * @param record 显卡行
 * @returns 唯一键
 */
function getGpuRowKey(record: GpuInfo): string {
  const composite = [
    String(record.name ?? '').trim(),
    String(record.manufacturer ?? '').trim(),
    String(record.driverVersion ?? '').trim(),
    String(record.adapterRam ?? ''),
  ].filter(Boolean).join('|')
  return composite || 'gpu-unknown'
}

/**
 * 格式化显存容量；WMI 无效值（如 0 或 4GB 上限）时显示「-」
 * @param adapterRam 显存字节数
 * @returns 展示文案
 */
function formatGpuAdapterRam(adapterRam: unknown): string {
  const bytes = toByteNumber(adapterRam)
  if (bytes <= 0) {
    return '-'
  }
  return formatBytes(bytes)
}

/**
 * 格式化内存条频率（MHz）
 * @param speedMHz 频率
 * @returns 展示文案
 */
function formatMemoryModuleSpeed(speedMHz: unknown): string {
  const speed = Number(speedMHz) || 0
  if (speed <= 0) {
    return '-'
  }
  return `${speed} MHz`
}

/** 磁盘表格列 */
const driveColumns = computed(() => [
  { title: t('statistics.logging.server-monitor.page.field.drive.name'), dataIndex: 'name', key: 'name' },
  { title: t('statistics.logging.server-monitor.page.field.drive.type'), dataIndex: 'driveType', key: 'driveType' },
  { title: t('statistics.logging.server-monitor.page.field.drive.file.system'), dataIndex: 'fileSystem', key: 'fileSystem' },
  { title: t('statistics.logging.server-monitor.page.field.drive.total.size'), dataIndex: 'totalSize', key: 'totalSize' },
  { title: t('statistics.logging.server-monitor.page.field.drive.free.space'), dataIndex: 'freeSpace', key: 'freeSpace' },
  { title: t('statistics.logging.server-monitor.page.field.drive.used.space'), dataIndex: 'usedSpace', key: 'usedSpace' },
  { title: t('statistics.logging.server-monitor.page.field.drive.usage.percent'), dataIndex: 'usagePercent', key: 'usagePercent', width: 200 },
])

/** 显卡表格列 */
const gpuColumns = computed(() => [
  { title: t('statistics.logging.server-monitor.page.field.gpu.name'), dataIndex: 'name', key: 'name', ellipsis: true },
  { title: t('statistics.logging.server-monitor.page.field.gpu.manufacturer'), dataIndex: 'manufacturer', key: 'manufacturer' },
  { title: t('statistics.logging.server-monitor.page.field.gpu.adapter.ram'), dataIndex: 'adapterRam', key: 'adapterRam' },
  { title: t('statistics.logging.server-monitor.page.field.gpu.driver.version'), dataIndex: 'driverVersion', key: 'driverVersion' },
])

/** 网络表格列 */
const networkColumns = computed(() => [
  { title: t('statistics.logging.server-monitor.page.field.network.name'), dataIndex: 'name', key: 'name', ellipsis: true },
  { title: t('statistics.logging.server-monitor.page.field.network.mac.address'), dataIndex: 'macAddress', key: 'macAddress' },
  { title: t('statistics.logging.server-monitor.page.field.network.ip.address'), dataIndex: 'ipAddress', key: 'ipAddress' },
  { title: t('statistics.logging.server-monitor.page.field.network.speed'), dataIndex: 'speed', key: 'speed' },
  { title: t('statistics.logging.server-monitor.page.field.network.status'), dataIndex: 'status', key: 'status' },
])

/**
 * 判断网卡描述是否与名称重复
 * @param name 适配器名称
 * @param description 适配器描述
 * @returns 描述与名称不同且非空时返回 true
 */
function shouldShowNetworkDescription(name?: string, description?: string): boolean {
  const normalizedName = (name ?? '').trim()
  const normalizedDescription = (description ?? '').trim()
  if (!normalizedName || !normalizedDescription) {
    return false
  }
  return normalizedDescription.localeCompare(normalizedName, undefined, { sensitivity: 'accent' }) !== 0
}

/**
 * 网卡名称主文案（名称与描述相同时只取一处）
 * @param name 适配器名称
 * @param description 适配器描述
 * @returns 展示用主标题
 */
function formatNetworkAdapterPrimary(name?: string, description?: string): string {
  const normalizedName = (name ?? '').trim()
  const normalizedDescription = (description ?? '').trim()
  if (normalizedName) {
    return normalizedName
  }
  if (normalizedDescription) {
    return normalizedDescription
  }
  return '-'
}

/**
 * 将 API 数值转为字节数
 * @param value 字节数（number 或 string）
 * @returns 字节数
 */
function toByteNumber(value: unknown): number {
  if (value == null || value === '') {
    return 0
  }
  const num = Number(value)
  return Number.isFinite(num) ? num : 0
}

/**
 * 计算使用率（%）
 * @param used 已用量
 * @param total 总量
 * @returns 使用率百分比
 */
function calcUsagePercent(used: unknown, total: unknown): number {
  const totalNum = toByteNumber(total)
  const usedNum = toByteNumber(used)
  if (!totalNum) {
    return 0
  }
  return Math.round((usedNum / totalNum) * 10000) / 100
}

/**
 * 格式化字节数
 * @param bytes 字节数
 * @returns 格式化字符串
 */
function formatBytes(bytes: number): string {
  if (!bytes) {
    return '0 B'
  }
  const k = 1024
  const sizes = ['B', 'KB', 'MB', 'GB', 'TB']
  const i = Math.floor(Math.log(bytes) / Math.log(k))
  return `${parseFloat((bytes / Math.pow(k, i)).toFixed(2))} ${sizes[i]}`
}

/**
 * 格式化网卡链路速度（比特/秒）
 * @param bitsPerSecond 比特/秒
 * @returns 格式化字符串
 */
function formatNetworkSpeed(bitsPerSecond: unknown): string {
  const bps = toByteNumber(bitsPerSecond)
  if (!bps) {
    return '-'
  }
  if (bps >= 1_000_000_000) {
    return `${(bps / 1_000_000_000).toFixed(2)} Gbps`
  }
  if (bps >= 1_000_000) {
    return `${(bps / 1_000_000).toFixed(2)} Mbps`
  }
  if (bps >= 1_000) {
    return `${(bps / 1_000).toFixed(2)} Kbps`
  }
  return `${bps} bps`
}

/**
 * 网卡状态 Tag 颜色
 * @param status 后端联网状态
 * @returns Ant Design Tag color
 */
function getNetworkStatusColor(status: string | undefined): string {
  const normalized = (status ?? '').trim().toLowerCase()
  switch (normalized) {
    case 'online':
    case 'up':
    case 'enabled':
      return 'green'
    case 'nointernet':
    case 'dnsfault':
      return 'orange'
    case 'down':
    case 'disabled':
      return 'red'
    default:
      return 'default'
  }
}

/**
 * 格式化网卡状态文案
 * @param status 后端联网状态
 * @returns 本地化状态
 */
function formatNetworkStatus(status: string | undefined): string {
  const normalized = (status ?? '').trim().toLowerCase()
  switch (normalized) {
    case 'online':
      return t('statistics.logging.server-monitor.page.field.network.status.online')
    case 'nointernet':
      return t('statistics.logging.server-monitor.page.field.network.status.no.internet')
    case 'dnsfault':
      return t('statistics.logging.server-monitor.page.field.network.status.dns.fault')
    case 'up':
      return t('statistics.logging.server-monitor.page.field.network.status.up')
    case 'down':
      return t('statistics.logging.server-monitor.page.field.network.status.down')
    case 'enabled':
      return t('statistics.logging.server-monitor.page.field.network.status.enabled')
    case 'disabled':
      return t('statistics.logging.server-monitor.page.field.network.status.disabled')
    default:
      return status?.trim() || t('statistics.logging.server-monitor.page.field.network.status.unknown')
  }
}

/**
 * 格式化日期时间
 * @param dateTime 日期时间字符串
 * @returns 格式化字符串
 */
function formatDateTime(dateTime: string): string {
  if (!dateTime) {
    return '-'
  }
  return dayjs(dateTime).format('YYYY-MM-DD HH:mm:ss')
}

/**
 * 格式化运行时长
 * @param uptime TimeSpan 字符串
 * @returns 本地化时长文案
 */
function formatUptime(uptime: string): string {
  if (!uptime) {
    return '-'
  }
  const parts = uptime.split('.')
  if (parts.length >= 2) {
    const days = parseInt(parts[0] || '0', 10)
    const time = parts[1]?.split(':')
    if (!time || time.length < 2) {
      return uptime
    }
    const hours = parseInt(time[0] || '0', 10)
    const minutes = parseInt(time[1] || '0', 10)
    const dayUnit = t('statistics.logging.server-monitor.page.unit.day')
    const hourUnit = t('statistics.logging.server-monitor.page.unit.hour')
    const minuteUnit = t('statistics.logging.server-monitor.page.unit.minute')
    if (days > 0) {
      return `${days} ${dayUnit} ${hours} ${hourUnit} ${minutes} ${minuteUnit}`
    }
    return `${hours} ${hourUnit} ${minutes} ${minuteUnit}`
  }
  return uptime
}

/**
 * 限制使用率百分比在 0~100
 * @param usage 使用率
 * @returns 0~100
 */
function clampUsagePercent(usage: number): number {
  const value = Number(usage) || 0
  return Math.min(Math.max(value, 0), 100)
}

/** 仪表盘刻度段颜色：0~30 青 / 30~70 蓝 / 70~100 红 */
const GAUGE_AXIS_COLORS: ReadonlyArray<[number, string]> = [
  [0.3, '#67e0e3'],
  [0.7, '#37a2da'],
  [1, '#fd666d'],
]

/**
 * 按使用率返回仪表盘刻度/数值强调色
 * @param usage 使用率 0~100
 * @returns 与刻度段一致的颜色
 */
function getGaugeAccentColor(usage: number): string {
  const value = clampUsagePercent(usage)
  if (value >= 70) {
    return '#fd666d'
  }
  if (value >= 30) {
    return '#37a2da'
  }
  return '#67e0e3'
}

/**
 * 渲染使用率仪表盘（半圆速度表风格，0~100%）
 * @param chartRef 图表实例引用
 * @param containerRef 容器 DOM
 * @param usedPercent 已用百分比
 */
function renderUsageGaugeChart(
  chartRef: { chart: ECharts | null },
  containerRef: HTMLDivElement | null,
  usedPercent: number,
): void {
  if (!containerRef) {
    return
  }
  if (!chartRef.chart) {
    chartRef.chart = echarts.init(containerRef, undefined, { locale: echartsLocale.value })
  }
  const used = clampUsagePercent(usedPercent)
  const accentColor = getGaugeAccentColor(used)
  const usedLabel = t('statistics.logging.server-monitor.page.field.usage.used')
  chartRef.chart.setOption(
    {
      tooltip: {
        formatter: `${usedLabel}: ${used.toFixed(2)}%`,
      },
      series: [
        {
          type: 'gauge',
          min: 0,
          max: 100,
          splitNumber: 10,
          startAngle: 225,
          endAngle: -45,
          radius: '92%',
          center: ['50%', '58%'],
          axisLine: {
            lineStyle: {
              width: 18,
              color: [...GAUGE_AXIS_COLORS],
            },
          },
          pointer: {
            length: '62%',
            width: 5,
            itemStyle: {
              color: accentColor,
            },
          },
          axisTick: {
            distance: -18,
            length: 6,
            lineStyle: {
              color: '#fff',
              width: 1,
            },
          },
          splitLine: {
            distance: -20,
            length: 16,
            lineStyle: {
              color: '#fff',
              width: 2,
            },
          },
          axisLabel: {
            distance: 22,
            fontSize: 11,
            color: (value: number) => getGaugeAccentColor(value),
          },
          detail: {
            valueAnimation: true,
            offsetCenter: [0, '72%'],
            fontSize: 24,
            fontWeight: 700,
            color: accentColor,
            formatter: (value: number) => `${(Number(value) || 0).toFixed(2)}%`,
          },
          data: [{ value: used, name: usedLabel }],
        },
      ],
    },
    { notMerge: true },
  )
}

/**
 * 渲染 CPU 汇总使用率仪表盘
 */
function renderCpuUsageGaugeChart(): void {
  renderUsageGaugeChart(
    cpuUsageGaugeChartRef,
    cpuUsageGaugeRef.value,
    cpuUsagePercent.value,
  )
}

/**
 * 渲染内存汇总使用率仪表盘
 */
function renderMemoryUsageGaugeChart(): void {
  renderUsageGaugeChart(
    memoryUsageGaugeChartRef,
    memoryUsageGaugeRef.value,
    memoryUsagePercent.value,
  )
}

/**
 * 初始化使用率仪表盘与尺寸监听
 * @param containerRef 容器 DOM
 * @param renderChart 渲染回调
 * @param resizeObserverRef 当前 ResizeObserver
 * @param chartRef 图表实例引用
 * @returns 新的 ResizeObserver
 */
function initUsageGaugeChart(
  containerRef: HTMLDivElement | null,
  renderChart: () => void,
  resizeObserverRef: ResizeObserver | null,
  chartRef: { chart: ECharts | null },
): ResizeObserver | null {
  if (!containerRef) {
    return resizeObserverRef
  }
  renderChart()
  resizeObserverRef?.disconnect()
  const observer = new ResizeObserver(() => {
    chartRef.chart?.resize()
  })
  observer.observe(containerRef)
  return observer
}

/**
 * 初始化 CPU 汇总仪表盘与尺寸监听
 */
function initCpuUsageGaugeChart(): void {
  cpuUsageGaugeResizeObserver = initUsageGaugeChart(
    cpuUsageGaugeRef.value,
    renderCpuUsageGaugeChart,
    cpuUsageGaugeResizeObserver,
    cpuUsageGaugeChartRef,
  )
}

/**
 * 初始化内存汇总仪表盘与尺寸监听
 */
function initMemoryUsageGaugeChart(): void {
  memoryUsageGaugeResizeObserver = initUsageGaugeChart(
    memoryUsageGaugeRef.value,
    renderMemoryUsageGaugeChart,
    memoryUsageGaugeResizeObserver,
    memoryUsageGaugeChartRef,
  )
}

/**
 * 销毁 CPU 汇总仪表盘实例
 */
function disposeCpuUsageGaugeChart(): void {
  cpuUsageGaugeResizeObserver?.disconnect()
  cpuUsageGaugeResizeObserver = null
  cpuUsageGaugeChartRef.chart?.dispose()
  cpuUsageGaugeChartRef.chart = null
}

/**
 * 销毁内存汇总仪表盘实例
 */
function disposeMemoryUsageGaugeChart(): void {
  memoryUsageGaugeResizeObserver?.disconnect()
  memoryUsageGaugeResizeObserver = null
  memoryUsageGaugeChartRef.chart?.dispose()
  memoryUsageGaugeChartRef.chart = null
}

/**
 * 语言切换时重建仪表盘（ECharts locale 在 init 时注入）
 */
function reinitUsageGaugeChartsOnLocaleChange(): void {
  disposeCpuUsageGaugeChart()
  disposeMemoryUsageGaugeChart()
  nextTick(() => {
    if (activeTab.value === 'cpu') {
      initCpuUsageGaugeChart()
    }
    if (activeTab.value === 'memory') {
      initMemoryUsageGaugeChart()
    }
  })
}

/**
 * 获取 CPU 使用率颜色
 * @param usage 使用率百分比
 * @returns 颜色值
 */
function getCpuUsageColor(usage: number): string {
  return getMemoryUsageColor(usage)
}

/**
 * 获取内存使用率颜色
 * @param usage 使用率百分比
 * @returns 颜色值
 */
function getMemoryUsageColor(usage: number): string {
  if (usage >= 90) {
    return '#ff4d4f'
  }
  if (usage >= 70) {
    return '#faad14'
  }
  return '#52c41a'
}

/**
 * 获取磁盘使用率颜色
 * @param usage 使用率百分比
 * @returns 颜色值
 */
function getDriveUsageColor(usage: number): string {
  if (usage >= 90) {
    return '#ff4d4f'
  }
  if (usage >= 70) {
    return '#faad14'
  }
  return '#1677ff'
}

/**
 * 加载监控数据（需 statistics:logging:server:monitor:list）
 */
async function loadData() {
  if (!permissionStore.hasPermission(PERMISSION_LIST)) {
    return
  }
  loading.value = true
  try {
    const [hardware, status] = await Promise.all([getServerHardware(), getAppStatus()])
    hardwareInfo.value = hardware
    appStatus.value = status
    nextTick(() => {
      if (activeTab.value === 'cpu') {
        initCpuUsageGaugeChart()
      }
      if (activeTab.value === 'memory') {
        initMemoryUsageGaugeChart()
      }
    })
  } catch {
    message.error(t('statistics.logging.server-monitor.page.message.load.fail'))
  } finally {
    loading.value = false
  }
}

/** 租户/公司切换时刷新 */
useTableRefresh(loadData)

onMounted(() => {
  void loadData()
  nextTick(() => {
    initCpuUsageGaugeChart()
    initMemoryUsageGaugeChart()
  })
})

watch(cpuUsagePercent, () => {
  renderCpuUsageGaugeChart()
})

watch(memoryUsagePercent, () => {
  renderMemoryUsageGaugeChart()
})

watch([locale, echartsLocale], () => {
  reinitUsageGaugeChartsOnLocaleChange()
})

watch(activeTab, (key) => {
  if (key === 'cpu') {
    nextTick(() => {
      initCpuUsageGaugeChart()
      cpuUsageGaugeChartRef.chart?.resize()
    })
  }
  if (key === 'memory') {
    nextTick(() => {
      initMemoryUsageGaugeChart()
      memoryUsageGaugeChartRef.chart?.resize()
    })
  }
})

onUnmounted(() => {
  disposeCpuUsageGaugeChart()
  disposeMemoryUsageGaugeChart()
})
</script>
