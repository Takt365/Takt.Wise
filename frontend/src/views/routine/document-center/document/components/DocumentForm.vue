<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/routine/document-center/document/components -->
<!-- 文件名称：DocumentForm.vue -->
<!-- 功能描述：文管中心主实体 支持制度、流程、模板等文档的分类、版本与权限控制维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form document-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="document-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plantCode')"
                name="plantCode"
              >
                <TaktSelect
                  v-model:value="formState.plantCode"
                  api-url="TaktPlants/options"
                  :placeholder="pi.ph('plantCode')"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('cultureCode')"
                name="cultureCode"
              >
                <TaktSelect
                  v-model:value="formState.cultureCode"
                  dict-type="sys_culture_code"
                  :placeholder="pi.ph('cultureCode')"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.form.numberingRule')"
                name="numberingRuleCode"
              >
                <TaktSelect
                  v-model:value="formState.numberingRuleCode"
                  api-url="TaktNumberings/options"
                  :api-params="{ documentType: '文管中心' }"
                  :placeholder="t('common.page.form.placeholder.selectonly')"
                  :disabled="!!formData?.documentId || loading"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('documentCode')"
                name="documentCode"
              >
                <a-input
                  v-model:value="formState.documentCode"
                  :placeholder="t('common.page.form.numberingCodePreview')"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('numberingRuleCode')"
                name="numberingRuleCode"
              >
                <a-input
                  v-model:value="formState.numberingRuleCode"
                  :placeholder="pi.ph('numberingRuleCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.documentId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('documentTitle')"
                name="documentTitle"
              >
                <a-input
                  v-model:value="formState.documentTitle"
                  :placeholder="pi.ph('documentTitle')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('documentCategory')"
                name="documentCategory"
              >
                <TaktSelect
                  v-model:value="formState.documentCategory"
                  dict-type="routine_document_center_category"
                  :placeholder="pi.ph('documentCategory')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('confidentialLevel')"
                name="confidentialLevel"
              >
                <TaktSelect
                  v-model:value="formState.confidentialLevel"
                  dict-type="routine_document_center_confidential_level"
                  :placeholder="pi.ph('confidentialLevel')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('version')"
                name="version"
              >
                <a-input-number
                  v-model:value="formState.version"
                  :placeholder="pi.ph('version')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('documentContent')"
                name="documentContent"
              >
                <TaktRichEditor
                  v-model:value="formState.documentContent"
                  :placeholder="pi.ph('documentContent')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('documentSummary')"
                name="documentSummary"
              >
                <a-input
                  v-model:value="formState.documentSummary"
                  :placeholder="pi.ph('documentSummary')"
                  show-count
                  :maxlength="2000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('documentTags')"
                name="documentTags"
              >
                <a-input
                  v-model:value="formState.documentTags"
                  :placeholder="pi.ph('documentTags')"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('fileName')"
                name="fileName"
              >
                <a-input
                  v-model:value="formState.fileName"
                  :placeholder="pi.ph('fileName')"
                  show-count
                  :maxlength="200"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('accessUrl')"
                name="accessUrl"
              >
                <TaktUploadFile
                  tabs-type="files"
                  :files-auto-upload="true"
                  :files-multiple="false"
                  :files-max-count="1"
                  :files-disabled="!!loading || fileUploading"
                  :files-max-size="taktFileMaxSizeMb"
                  :files-accept="taktFileAccept"
                  :files-hint="t('foundation.file.page.upload.hint', { max: taktFileMaxSizeMb })"
                  :files-custom-request="handleFilesCustomRequest"
                  v-model:files-file-list="filesFileList"
                  @files:remove="handleFileRemove"
                />
                <a-input
                  v-if="formState.accessUrl"
                  v-model:value="formState.accessUrl"
                  class="mt-2"
                  :placeholder="pi.ph('accessUrl')"
                  show-count
                  :maxlength="1000"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('documentEffectiveTime')"
                name="documentEffectiveTime"
              >
                <a-date-picker
                  v-model:value="formState.documentEffectiveTime"
                  :placeholder="pi.ph('documentEffectiveTime')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('documentExpireTime')"
                name="documentExpireTime"
              >
                <a-date-picker
                  v-model:value="formState.documentExpireTime"
                  :placeholder="pi.ph('documentExpireTime')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('documentPublishTime')"
                name="documentPublishTime"
              >
                <a-date-picker
                  v-model:value="formState.documentPublishTime"
                  :placeholder="pi.ph('documentPublishTime')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('publisherId')"
                name="publisherId"
              >
                <TaktSelect
                  v-model:value="formState.publisherId"
                  api-url="TaktUsers/options"
                  :placeholder="pi.ph('publisherId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('publisherName')"
                name="publisherName"
              >
                <a-input
                  v-model:value="formState.publisherName"
                  :placeholder="pi.ph('publisherName')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('deptId')"
                name="deptId"
              >
                <TaktSelect
                  v-model:value="formState.deptId"
                  api-url="TaktDepts/tree-options"
                  :placeholder="pi.ph('deptId')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('deptName')"
                name="deptName"
              >
                <a-input
                  v-model:value="formState.deptName"
                  :placeholder="pi.ph('deptName')"
                  show-count
                  :maxlength="100"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('documentIsTop')"
                name="documentIsTop"
              >
                <TaktSelect
                  v-model:value="formState.documentIsTop"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('documentIsTop')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('documentViewCount')"
                name="documentViewCount"
              >
                <a-input-number
                  v-model:value="formState.documentViewCount"
                  :placeholder="pi.ph('documentViewCount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('targetScope')"
                name="targetScope"
              >
                <TaktSelect
                  v-model:value="formState.targetScope"
                  dict-type="sys_publish_scope"
                  :placeholder="pi.ph('targetScope')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('targetDepartments')"
                name="targetDepartments"
              >
                <a-input
                  v-model:value="formState.targetDepartments"
                  :placeholder="pi.ph('targetDepartments')"
                  show-count
                  :maxlength="1000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('targetUsers')"
                name="targetUsers"
              >
                <a-input
                  v-model:value="formState.targetUsers"
                  :placeholder="pi.ph('targetUsers')"
                  show-count
                  :maxlength="2000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('documentStatus')"
                name="documentStatus"
              >
                <TaktSelect
                  v-model:value="formState.documentStatus"
                  dict-type="sys_publish_status"
                  :placeholder="pi.ph('documentStatus')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-3"
        :tab="t('common.page.form.tabs.basicinfo') + ' (4/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('tenantCode')"
                name="tenantCode"
              >
                <a-input
                  v-model:value="formState.tenantCode"
                  :placeholder="pi.ph('tenantCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('companyCode')"
                name="companyCode"
              >
                <TaktSelect
                  v-model:value="formState.companyCode"
                  api-url="TaktCompanies/options"
                  :placeholder="pi.ph('companyCode')"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                name="extField"
                class="takt-form-item-ext-field"
              >
                <template #label>
                  <span class="takt-form-ext-field-label">
                    <a-tooltip
                      :title="t('common.page.entity.extfieldhint')"
                      placement="top"
                    >
                      <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
                    </a-tooltip>
                    <span>{{ pi.label('extField') }}</span>
                  </span>
                </template>
                <a-textarea
                  v-model:value="formState.extField"
                  :placeholder="t('common.page.form.placeholder.extfield')"
                  :rows="4"
                  show-count
                  :maxlength="400"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="pi.ph('remark')"
                  :rows="4"
                  show-count
                  :maxlength="400"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
    </a-tabs>
    <!-- 下：子表 versions -->
    <TaktEditableTable
      ref="documentVersionTableRef"
      v-model="childDocumentVersionRows"
      :columns="documentVersionFormColumns"
      :title="documentVersionPi.self()"
      :add-button-entity="documentVersionPi.self()"
      id-field="documentVersionId"
      :default-row="createDefaultDocumentVersionRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-fileId="{ record }">
        <TaktSelect
          v-model:value="record.fileId"
          api-url="TaktFiles/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="documentVersionPi.queryPh('fileId', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-revisedBy="{ record }">
        <TaktSelect
          v-model:value="record.revisedBy"
          api-url="TaktUsers/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="documentVersionPi.queryPh('revisedBy', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-isObsolete="{ record }">
        <TaktSelect
          v-model:value="record.isObsolete"
          dict-type="sys_yes_no"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="documentVersionPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 文管中心主实体 支持制度、流程、模板等文档的分类、版本与权限控制维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/routine/document-center/document/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useDocumentI18n } from '../composables/useDocumentI18n'

/** 实体字段 i18n */
const pi = useDocumentI18n()

import type { DocumentCreate } from '@/types/routine/document-center/document.types'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { message } from 'ant-design-vue'
import type { UploadFile, UploadProps } from 'ant-design-vue'
import { getFileById } from '@/api/foundation/file.api'
import { uploadTaktFileSmart } from '@/utils/taktFileChunkUpload'
import {
  buildTaktFileAcceptAttribute,
  loadTaktFileUploadBasePolicy,
  resolveTaktFileMaxSizeMb,
} from '@/utils/taktFileUploadPolicy'
import { useTaktFormNumbering } from '@/composables/useTaktFormNumbering'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useUserStore } from '@/stores/identity/useUserStore'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文（当前公司 CultureCode 注入源） */
const userStore = useUserStore()

/**
 * 上下文隔离字段：租户 / 公司 / CultureCode / PlantCode（登录或公司切换注入；工厂可选改）
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或上下文切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (force || !target.tenantCode) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (force || !target.companyCode) {
    target.companyCode = tenantStore.companyCode
  }
  if (force || !target.cultureCode) {
    target.cultureCode = userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? ''
  }
  if (force || !target.plantCode) {
    const nextPlant = tenantStore.currentCompanyRelatedPlant || ''
    if (nextPlant) {
      target.plantCode = nextPlant
    }
  }
}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","numberingRuleCode","documentCode","numberingRuleCode","documentTitle","documentCategory","confidentialLevel","version","documentContent","documentSummary","documentTags","fileName","accessUrl","documentEffectiveTime","documentExpireTime","documentPublishTime","publisherId","publisherName","deptId","deptName","documentIsTop","documentViewCount","targetScope","targetDepartments","targetUsers","documentStatus","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'
import { resolveNextDetailLineNumber } from '@/utils/taktSequence'
import { useDocumentVersionI18n } from '../composables/useDocumentVersionI18n'

const documentVersionPi = useDocumentVersionI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childDocumentVersionRows = ref<Record<string, unknown>[]>([])
const documentVersionTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedDocumentVersionRow(row: Record<string, unknown>): boolean {
  const id = row.documentVersionId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextDocumentVersionLineNumber(): number {
  const rows = documentVersionTableRef.value?.getRows?.() ?? childDocumentVersionRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 documentVersion 可编辑列 */
const documentVersionFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: documentVersionPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'versionNo',
    title: documentVersionPi.label('versionNo'),
    width: 140,
  },
  {
    key: 'versionNote',
    title: documentVersionPi.label('versionNote'),
    editor: 'textarea',
    rows: 1,
    placeholder: documentVersionPi.ph('versionNote'),
    width: 180,
  },
  {
    key: 'fileId',
    title: documentVersionPi.label('fileId'),
    width: 140,
  },
  {
    key: 'revisedBy',
    title: documentVersionPi.label('revisedBy'),
    width: 140,
  },
  {
    key: 'revisedAt',
    title: documentVersionPi.label('revisedAt'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'isObsolete',
    title: documentVersionPi.label('isObsolete'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<DocumentCreate & { documentId?: string }> | null | undefined) {
  const rows_documentVersion = ((val as any)?.versions ?? []) as Record<string, unknown>[]
  childDocumentVersionRows.value = rows_documentVersion
}

function createDefaultDocumentVersionRow(): Record<string, unknown> {
  return {
    lineNumber: allocateNextDocumentVersionLineNumber(),
    versionNo: 0,
    versionNote: '',
    fileId: '',
    revisedBy: '',
    revisedAt: '',
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.documentId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    versions: documentVersionTableRef.value?.getRows?.() ?? childDocumentVersionRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        documentId: masterId,
      }
      if (isUpdate && isPersistedDocumentVersionRow(row)) {
        normalized.documentVersionId = row.documentVersionId
      } else {
        delete normalized.documentVersionId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<DocumentCreate & { documentId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})
/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  documentCategory: 0,
  confidentialLevel: 0,
  documentStatus: 0
}

/** 写入表单默认值（新增 / resetFields / 弹窗再次打开时） */
function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

/** 文件上传中 */
const fileUploading = ref(false)
/** takt-upload-file 文件列表 */
const filesFileList = ref<UploadFile[]>([])
/** 上传 accept */
const taktFileAccept = ref('')
/** 上传体积上限 MB */
const taktFileMaxSizeMb = ref(500)

/**
 * 按 fileName / accessUrl 同步上传列表展示
 */
function syncFilesFileListFromFormState() {
  const url = String(formState.accessUrl ?? '').trim()
  if (!url) {
    filesFileList.value = []
    return
  }
  filesFileList.value = [{
    uid: '-1',
    name: String(formState.fileName ?? url.split('/').pop() ?? 'file'),
    status: 'done',
    url,
  }]
}

/**
 * 将 TaktFile 上传结果回填至表单（文件名由上传结果回填，禁止手输）
 * @param file 本地文件
 * @param result 上传结果
 */
async function applyUploadResultToForm(file: globalThis.File, result: Awaited<ReturnType<typeof uploadTaktFileSmart>>) {
  let accessUrl = result.accessUrl?.trim() ?? ''
  if (!accessUrl && result.fileId) {
    const detail = await getFileById(result.fileId)
    accessUrl = detail.accessUrl?.trim() ?? ''
  }
  if (!accessUrl) {
    throw new Error('accessUrl empty')
  }
  formState.accessUrl = accessUrl
  formState.fileName = result.fileOriginalName?.trim()
    || result.fileName?.trim()
    || file.name
  syncFilesFileListFromFormState()
  formRef.value?.validateFields(['accessUrl', 'fileName']).catch(() => undefined)
}

/** takt-upload-file 自定义上传：落库 TaktFile 后回写 accessUrl / fileName */
const handleFilesCustomRequest: UploadProps['customRequest'] = (options) => {
  if (props.loading || fileUploading.value) {
    options.onError?.(new Error('upload disabled'))
    return
  }
  const originFile = options.file as globalThis.File
  fileUploading.value = true
  void (async () => {
    try {
      const result = await uploadTaktFileSmart(originFile)
      await applyUploadResultToForm(originFile, result)
      options.onSuccess?.(result)
    } catch (error: unknown) {
      const err = error instanceof Error ? error : new Error(String(error))
      message.error(t('common.feedback.failed'))
      options.onError?.(err)
    } finally {
      fileUploading.value = false
    }
  })()
}

/** 移除已上传文件 */
function handleFileRemove() {
  formState.accessUrl = ''
  formState.fileName = ''
  filesFileList.value = []
}

watch(
  () => [formState.fileName, formState.accessUrl],
  () => {
    syncFilesFileListFromFormState()
  },
)

/** 挂载后加载后端上传策略（accept / maxSize） */
onMounted(() => {
  void (async () => {
    try {
      const policy = await loadTaktFileUploadBasePolicy()
      taktFileAccept.value = buildTaktFileAcceptAttribute(policy.allowedExtensions ?? [])
      taktFileMaxSizeMb.value = resolveTaktFileMaxSizeMb(policy)
    } catch {
      // 回退默认值；实际上传校验仍由后端 API 返回
    }
  })()
})

/** 是否编辑态（编码规则取号仅新增） */
const isEditMode = computed(() => !!props.formData?.documentId)

useTaktFormNumbering({
  formState,
  isEdit: isEditMode,
  businessCodeField: 'documentCode',
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 documentId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.documentId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).versions
      applyScopeDefaults(next)
      Object.assign(formState, next)
    syncChildRowsFromFormData(val)
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        Object.assign(formState, val)
      }
      applyFormDefaults(formState)
      applyScopeDefaults(formState as Record<string, unknown>, true)
      formRef.value?.clearValidate()
    }
  },
  { immediate: true }
)

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture, tenantStore.currentCompanyRelatedPlant] as const,
  () => {
    if (!props.formData?.documentId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  numberingRuleCode: [{
    validator: async (_rule, value) => {
      if (isEditMode.value) {
        return Promise.resolve()
      }
      if (!String(value ?? '').trim()) {
        return Promise.reject(t('common.page.form.numberingRuleRequired'))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],
  documentCode: [{
    validator: async (_rule, value) => {
      if (isEditMode.value) {
        return Promise.resolve()
      }
      if (!String(value ?? '').trim()) {
        return Promise.reject(t('common.page.form.numberingCodePreview'))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],
  documentTitle: [
    {
      required: true,
      message: pi.ph('documentTitle'),
      trigger: 'blur'
    }
  ],
  documentCategory: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('documentCategory'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('documentCategory'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  confidentialLevel: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('confidentialLevel'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('confidentialLevel'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  version: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('version'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('version'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  publisherId: [
    {
      required: true,
      message: pi.ph('publisherId'),
      trigger: 'change'
    }
  ],
  documentIsTop: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('documentIsTop'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('documentIsTop'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  documentViewCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('documentViewCount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('documentViewCount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  targetScope: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('targetScope'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('targetScope'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  documentStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('documentStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('documentStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await documentVersionTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('documentCategory' in payload) {
    const rawdocumentCategory = payload.documentCategory
    if (rawdocumentCategory === undefined || rawdocumentCategory === null || rawdocumentCategory === '') {
      delete payload.documentCategory
    } else {
      const numdocumentCategory = typeof rawdocumentCategory === 'number' ? rawdocumentCategory : Number(rawdocumentCategory)
      if (Number.isFinite(numdocumentCategory)) payload.documentCategory = numdocumentCategory
      else delete payload.documentCategory
    }
  }
  if ('confidentialLevel' in payload) {
    const rawconfidentialLevel = payload.confidentialLevel
    if (rawconfidentialLevel === undefined || rawconfidentialLevel === null || rawconfidentialLevel === '') {
      delete payload.confidentialLevel
    } else {
      const numconfidentialLevel = typeof rawconfidentialLevel === 'number' ? rawconfidentialLevel : Number(rawconfidentialLevel)
      if (Number.isFinite(numconfidentialLevel)) payload.confidentialLevel = numconfidentialLevel
      else delete payload.confidentialLevel
    }
  }
  if ('version' in payload) {
    const rawversion = payload.version
    if (rawversion === undefined || rawversion === null || rawversion === '') {
      delete payload.version
    } else {
      const numversion = typeof rawversion === 'number' ? rawversion : Number(rawversion)
      if (Number.isFinite(numversion)) payload.version = numversion
      else delete payload.version
    }
  }
  if ('documentIsTop' in payload) {
    const rawdocumentIsTop = payload.documentIsTop
    if (rawdocumentIsTop === undefined || rawdocumentIsTop === null || rawdocumentIsTop === '') {
      delete payload.documentIsTop
    } else {
      const numdocumentIsTop = typeof rawdocumentIsTop === 'number' ? rawdocumentIsTop : Number(rawdocumentIsTop)
      if (Number.isFinite(numdocumentIsTop)) payload.documentIsTop = numdocumentIsTop
      else delete payload.documentIsTop
    }
  }
  if ('documentViewCount' in payload) {
    const rawdocumentViewCount = payload.documentViewCount
    if (rawdocumentViewCount === undefined || rawdocumentViewCount === null || rawdocumentViewCount === '') {
      delete payload.documentViewCount
    } else {
      const numdocumentViewCount = typeof rawdocumentViewCount === 'number' ? rawdocumentViewCount : Number(rawdocumentViewCount)
      if (Number.isFinite(numdocumentViewCount)) payload.documentViewCount = numdocumentViewCount
      else delete payload.documentViewCount
    }
  }
  if ('targetScope' in payload) {
    const rawtargetScope = payload.targetScope
    if (rawtargetScope === undefined || rawtargetScope === null || rawtargetScope === '') {
      delete payload.targetScope
    } else {
      const numtargetScope = typeof rawtargetScope === 'number' ? rawtargetScope : Number(rawtargetScope)
      if (Number.isFinite(numtargetScope)) payload.targetScope = numtargetScope
      else delete payload.targetScope
    }
  }
  if ('documentStatus' in payload) {
    const rawdocumentStatus = payload.documentStatus
    if (rawdocumentStatus === undefined || rawdocumentStatus === null || rawdocumentStatus === '') {
      delete payload.documentStatus
    } else {
      const numdocumentStatus = typeof rawdocumentStatus === 'number' ? rawdocumentStatus : Number(rawdocumentStatus)
      if (Number.isFinite(numdocumentStatus)) payload.documentStatus = numdocumentStatus
      else delete payload.documentStatus
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }

  if (props.formData?.documentId) {
    payload.documentId = props.formData.documentId
    delete payload.numberingRuleCode
  }
  return payload
}

/** 重置表单与子表行（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.documentId)
  childDocumentVersionRows.value = []
  documentVersionTableRef.value?.resetRows?.()
  activeTab.value = 'tab-0'
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>

<style scoped lang="css">
:deep(.ant-tabs-content-holder) {
  min-height: 50vh;
}

:deep(.ant-tabs-tabpane) {
  min-height: 50vh;
}
</style>
