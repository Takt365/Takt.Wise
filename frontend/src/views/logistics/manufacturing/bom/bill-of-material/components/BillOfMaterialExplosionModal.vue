<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/bill-of-material/components -->
<!-- 文件名称：BillOfMaterialExplosionModal.vue -->
<!-- 功能描述：BOM 多层递归展开清单弹窗（运行时计算，客户端分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <TaktModal
    v-model:open="visible"
    :title="t('logistics.manufacturing.bom.bill-of-material.page.explosion.title')"
    width="1200px"
    :use-viewport-size="true"
    :footer="null"
    @cancel="handleClose"
  >
    <a-form layout="inline" class="mb-4 flex flex-wrap gap-y-2">
      <a-form-item :label="t('logistics.manufacturing.bom.bill-of-material.page.explosion.quantity')">
        <a-input-number
          v-model:value="queryForm.quantity"
          :min="0.0001"
          :precision="4"
          class="w-32"
        />
      </a-form-item>
      <a-form-item :label="t('logistics.manufacturing.bom.bill-of-material.page.explosion.maxLevel')">
        <a-input-number
          v-model:value="queryForm.maxLevel"
          :min="0"
          :max="20"
          class="w-24"
        />
      </a-form-item>
      <a-form-item :label="t('logistics.manufacturing.bom.bill-of-material.page.explosion.includeLevelZero')">
        <a-switch v-model:checked="queryForm.includeLevelZero" />
      </a-form-item>
      <a-form-item>
        <a-button type="primary" :loading="loading" @click="loadExplosion">
          {{ t('common.page.button.query') }}
        </a-button>
      </a-form-item>
    </a-form>

    <div v-if="summary" class="mb-3 text-sm text-text-secondary">
      {{ t('logistics.manufacturing.bom.bill-of-material.page.explosion.summary', summary) }}
    </div>

    <a-table
      :columns="columns"
      :data-source="paginatedLines"
      :loading="loading"
      :pagination="false"
      :scroll="{ x: 1400, y: 480 }"
      row-key="rowKey"
      size="middle"
    />

    <div class="mt-4 flex justify-end">
      <TaktPagination
        v-model:current="currentPage"
        v-model:page-size="pageSize"
        :total="totalLines"
        @change="handlePaginationChange"
      />
    </div>
  </TaktModal>
</template>

<script setup lang="ts">
/**
 * BOM 多层展开清单弹窗
 * @module views/logistics/manufacturing/bom/bill-of-material/components/bill-of-material-explosion-modal
 */
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { getBillOfMaterialExplosion } from '@/api/logistics/manufacturing/bom/billOfMaterialExplosion.api'
import type { BillOfMaterial } from '@/types/logistics/manufacturing/bom/billOfMaterial.types'
import type { BillOfMaterialExplosionLine } from '@/types/logistics/manufacturing/bom/billOfMaterialExplosion.types'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import TaktDictTag from '@/components/common/TaktDictTag/index.vue'

const props = defineProps<{
  /** 是否打开弹窗 */
  open: boolean
  /** 展开根 BOM 主表行 */
  record?: BillOfMaterial | null
}>()

const emit = defineEmits<{
  (e: 'update:open', value: boolean): void
}>()

/** i18n */
const { t } = useI18n()

/** 弹窗可见（v-model） */
const visible = computed({
  get: () => props.open,
  set: (value: boolean) => emit('update:open', value),
})

/** 查询 loading */
const loading = ref(false)

/** 展开查询表单 */
const queryForm = ref({
  quantity: 1,
  maxLevel: 20,
  includeLevelZero: true,
})

/** 展开结果行（全量，客户端分页） */
const explosionLines = ref<(BillOfMaterialExplosionLine & { rowKey: string })[]>([])

/** 展开摘要 */
const summary = ref<{ bomCode: string; parentMaterialCode: string; parentMaterialDescription: string; quantity: number } | null>(null)

/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())

/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())

/** 总行数 */
const totalLines = computed(() => explosionLines.value.length)

/** 当前页数据 */
const paginatedLines = computed(() => {
  const start = (currentPage.value - 1) * pageSize.value
  return explosionLines.value.slice(start, start + pageSize.value)
})

/** 表格列 */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('logistics.manufacturing.bom.bill-of-material.page.explosion.column.level'),
    dataIndex: 'hierarchyLevel',
    key: 'hierarchyLevel',
    width: 72,
    fixed: 'left',
    customRender: ({ record }: { record: BillOfMaterialExplosionLine }) =>
      `${record.levelPrefix || ''}${record.hierarchyLevel}`,
  },
  {
    title: t('entity.billofmaterialitem.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 140,
    ellipsis: true,
  },
  {
    title: t('entity.billofmaterial.parentmaterialdescription'),
    dataIndex: 'materialDescription',
    key: 'materialDescription',
    width: 160,
    ellipsis: true,
  },
  {
    title: t('logistics.manufacturing.bom.bill-of-material.page.explosion.column.immediateParent'),
    dataIndex: 'immediateParentMaterialCode',
    key: 'immediateParentMaterialCode',
    width: 140,
    ellipsis: true,
  },
  {
    title: t('entity.billofmaterialitem.usagequantity'),
    dataIndex: 'usageQuantity',
    key: 'usageQuantity',
    width: 100,
  },
  {
    title: t('logistics.manufacturing.bom.bill-of-material.page.explosion.column.cumulativeQuantity'),
    dataIndex: 'cumulativeQuantity',
    key: 'cumulativeQuantity',
    width: 120,
  },
  {
    title: t('entity.billofmaterialitem.materialunit'),
    dataIndex: 'materialUnit',
    key: 'materialUnit',
    width: 80,
    customRender: ({ record }: { record: BillOfMaterialExplosionLine }) =>
      h(TaktDictTag, { dictType: 'logistics_materials_unit_of_measure_code', value: record.materialUnit }),
  },
  {
    title: t('entity.billofmaterialitem.scraprate'),
    dataIndex: 'scrapRate',
    key: 'scrapRate',
    width: 80,
  },
  {
    title: t('entity.billofmaterialitem.operationseq'),
    dataIndex: 'operationSeq',
    key: 'operationSeq',
    width: 80,
  },
  {
    title: t('entity.billofmaterialitem.isphantom'),
    dataIndex: 'isPhantom',
    key: 'isPhantom',
    width: 80,
    customRender: ({ record }: { record: BillOfMaterialExplosionLine }) =>
      h(TaktDictTag, { dictType: 'sys_yes_no', value: record.isPhantom }),
  },
  {
    title: t('logistics.manufacturing.bom.bill-of-material.page.explosion.column.hasChildBom'),
    dataIndex: 'hasChildBom',
    key: 'hasChildBom',
    width: 90,
    customRender: ({ record }: { record: BillOfMaterialExplosionLine }) =>
      h(TaktDictTag, { dictType: 'sys_yes_no', value: record.hasChildBom }),
  },
  {
    title: t('logistics.manufacturing.bom.bill-of-material.page.explosion.column.isCircular'),
    dataIndex: 'isCircular',
    key: 'isCircular',
    width: 90,
    customRender: ({ record }: { record: BillOfMaterialExplosionLine }) =>
      h(TaktDictTag, { dictType: 'sys_yes_no', value: record.isCircular }),
  }])

/**
 * 加载 BOM 展开清单
 */
async function loadExplosion() {
  const id = props.record?.billOfMaterialId
  if (!id) {
    message.warning(t('logistics.manufacturing.bom.bill-of-material.page.select.master.first'))
    return
  }
  loading.value = true
  try {
    const result = await getBillOfMaterialExplosion({
      billOfMaterialId: id,
      quantity: queryForm.value.quantity,
      maxLevel: queryForm.value.maxLevel,
      includeLevelZero: queryForm.value.includeLevelZero,
    })
    summary.value = {
      bomCode: result.bomCode,
      parentMaterialCode: result.parentMaterialCode,
      parentMaterialDescription: result.parentMaterialDescription,
      quantity: result.quantity,
    }
    explosionLines.value = (result.lines ?? []).map((line, index) => ({
      ...line,
      rowKey: `${line.hierarchyLevel}-${line.lineNumber}-${line.materialCode}-${index}`,
    }))
    currentPage.value = getTaktDefaultPageIndex()
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.load.data.failed'))
    explosionLines.value = []
    summary.value = null
  } finally {
    loading.value = false
  }
}

/**
 * 分页变更
 * @param page 页码
 * @param size 每页条数
 */
function handlePaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
}

/** 关闭弹窗 */
function handleClose() {
  visible.value = false
}

watch(
  () => props.open,
  async (open) => {
    if (open) {
      await ensureTaktPaginationConfigAsync()
      queryForm.value = { quantity: 1, maxLevel: 20, includeLevelZero: true }
      explosionLines.value = []
      summary.value = null
      currentPage.value = getTaktDefaultPageIndex()
      pageSize.value = getTaktDefaultPageSize()
      await loadExplosion()
    }
  }
)
</script>
