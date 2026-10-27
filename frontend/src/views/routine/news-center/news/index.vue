<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/routine/news-center/news -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：新闻管理主从页（左新闻右互动：评论/点赞/收藏/分享），含查询、增删改；互动明细按选中新闻分页 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4 flex flex-col min-h-0 h-full">
    <!-- 左主右从 -->
    <TaktMasterDetailTableLr
      v-model:master-current="currentPage"
      v-model:master-page-size="pageSize"
      v-model:selecmed-master-key="selecmedMasterKey"
      class="min-h-0 flex-1"
      :master-columns="columns"
      :master-data-source="dataSource"
      :master-loading="loading"
      :master-row-key="getNewsId"
      :master-row-selection="rowSelection"
      master-id-column-key="newsId"
      :master-visible-column-keys="visibleColumnKeys"
      master-table-mode="masterDetailMaster"
      master-scroll-layout="masterDetailLr"
      :master-total="total"
      master-entity-scope="approval"
      @master-change="handleTableChange"
      @master-resize-column="handleResizeColumn"
      @master-pagination-change="handleMasterPaginationChange"
      @master-select="handleMasterSelect"
    >
      <template #master-toolbar>
        <TaktQueryBar
          v-model="queryKeyword"
          :placeholder="searchPlaceholder"
          :loading="loading"
          @search="handleSearch"
          @reset="handleReset"
        />
        <TaktToolsBar
      create-permission="routine:news:center:create"
      update-permission="routine:news:center:update"
      delete-permission="routine:news:center:delete"
      import-permission="routine:news:center:import"
      export-permission="routine:news:center:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-import="true"
      :show-export="true"
      :show-expand="false"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :show-refresh="true"
      :create-disabled="false"
      :create-loading="loading"
      :update-disabled="updateDisabled"
      :update-loading="loading"
      :delete-disabled="deleteDisabled"
      :delete-loading="loading"
      :refresh-loading="loading"
      @create="handleCreate"
      @update="handleUpdate"
      @delete="handleDelete"
      @import="handleImport"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      @refresh="handleRefresh"
        />
      </template>
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'newsCategory'">
          <TaktDictTag
            :value="getNewsDictValue(record, 'newsCategory')"
            dict-type="sys_news_type"
          />
        </template>
        <template v-else-if="column.key === 'newsIsTop'">
          <TaktDictTag
            :value="getNewsDictValue(record, 'newsIsTop')"
            dict-type="sys_yes_no"
          />
        </template>
        <template v-else-if="column.key === 'newsIsRecommended'">
          <TaktDictTag
            :value="getNewsDictValue(record, 'newsIsRecommended')"
            dict-type="sys_yes_no"
          />
        </template>
        <template v-else-if="column.key === 'targetScope'">
          <TaktDictTag
            :value="getNewsDictValue(record, 'targetScope')"
            dict-type="sys_publish_scope"
          />
        </template>
        <template v-else-if="column.key === 'newsStatus'">
          <TaktDictTag
            :value="getNewsDictValue(record, 'newsStatus')"
            dict-type="sys_publish_status"
          />
        </template>
      </template>
      <template #detail>
        <div class="flex h-full min-h-0 flex-1 flex-col overflow-hidden">
          <a-tabs
            v-model:active-key="detailActiveTab"
            class="news-engagement-tabs flex h-full min-h-0 flex-1 flex-col overflow-hidden"
            size="small"
          >
            <a-tab-pane
              key="comment"
              :tab="t('routine.news.center.news.page.tabs.comment')"
              class="h-full"
            >
              <NewsCommentPanel
                ref="newsCommentPanelRef"
                class="h-full min-h-0 flex-1"
              />
            </a-tab-pane>
            <a-tab-pane
              key="like"
              :tab="t('routine.news.center.news.page.tabs.like')"
              class="h-full"
            >
              <NewsEngagementPanel
                ref="newsLikePanelRef"
                engagement="like"
                class="h-full min-h-0 flex-1"
              />
            </a-tab-pane>
            <a-tab-pane
              key="favorite"
              :tab="t('routine.news.center.news.page.tabs.favorite')"
              class="h-full"
            >
              <NewsEngagementPanel
                ref="newsFavoritePanelRef"
                engagement="favorite"
                class="h-full min-h-0 flex-1"
              />
            </a-tab-pane>
            <a-tab-pane
              key="share"
              :tab="t('routine.news.center.news.page.tabs.share')"
              class="h-full"
            >
              <NewsEngagementPanel
                ref="newsSharePanelRef"
                engagement="share"
                class="h-full min-h-0 flex-1"
              />
            </a-tab-pane>
          </a-tabs>
        </div>
      </template>
    </TaktMasterDetailTableLr>

    <!-- 新增/编辑对话框 -->
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      :width="formModalWidthPx"
      wrap-class-name="takt-form-modal-resizable"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <NewsForm
        :key="formData?.newsId ?? 'create'"
        ref="formRef"
        :form-data="formData"
        :loading="formLoading"
      />
    </TaktModal>
    <!-- 高级查询抽屉 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      :storage-key="'takt-query-fields-routine-news-center-news'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('cultureCode')">
      <a-form-item :label="pi.queryLabel('cultureCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.cultureCode"
          dict-type="sys_culture_code"
          :placeholder="pi.queryPh('cultureCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('newsCode')">
      <a-form-item :label="pi.queryLabel('newsCode')">
        <a-input
          v-model:value="advancedQueryForm.newsCode"
          :placeholder="pi.queryPh('newsCode', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('newsCategory')">
      <a-form-item :label="pi.queryLabel('newsCategory')">
        <TaktSelect
          v-model:value="advancedQueryForm.newsCategory"
          dict-type="sys_news_type"
          :placeholder="pi.queryPh('newsCategory', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('newsTitle')">
      <a-form-item :label="pi.queryLabel('newsTitle')">
        <a-input
          v-model:value="advancedQueryForm.newsTitle"
          :placeholder="pi.queryPh('newsTitle', 'required')"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('newsSummary')">
      <a-form-item :label="pi.queryLabel('newsSummary')">
        <a-input
          v-model:value="advancedQueryForm.newsSummary"
          :placeholder="pi.queryPh('newsSummary', 'required')"
          show-count
          :maxlength="2000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('newsTags')">
      <a-form-item :label="pi.queryLabel('newsTags')">
        <a-input
          v-model:value="advancedQueryForm.newsTags"
          :placeholder="pi.queryPh('newsTags', 'required')"
          show-count
          :maxlength="500"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('newsContent')">
      <a-form-item :label="pi.queryLabel('newsContent')">
        <a-textarea
          v-model:value="advancedQueryForm.newsContent"
          :placeholder="pi.queryPh('newsContent', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('newsCoverImage')">
      <a-form-item :label="pi.queryLabel('newsCoverImage')">
        <a-input
          v-model:value="advancedQueryForm.newsCoverImage"
          :placeholder="pi.queryPh('newsCoverImage', 'required')"
          show-count
          :maxlength="500"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('newsIsTop')">
      <a-form-item :label="pi.queryLabel('newsIsTop')">
        <TaktSelect
          v-model:value="advancedQueryForm.newsIsTop"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('newsIsTop', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('newsIsRecommended')">
      <a-form-item :label="pi.queryLabel('newsIsRecommended')">
        <TaktSelect
          v-model:value="advancedQueryForm.newsIsRecommended"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('newsIsRecommended', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('newsEffectiveTimeStart')">
      <a-form-item :label="pi.queryLabel('newsEffectiveTimeStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.newsEffectiveTimeStart"
          :placeholder="pi.queryPh('newsEffectiveTimeStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('newsEffectiveTimeEnd')">
      <a-form-item :label="pi.queryLabel('newsEffectiveTimeEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.newsEffectiveTimeEnd"
          :placeholder="pi.queryPh('newsEffectiveTimeEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('newsExpireTimeStart')">
      <a-form-item :label="pi.queryLabel('newsExpireTimeStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.newsExpireTimeStart"
          :placeholder="pi.queryPh('newsExpireTimeStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('newsExpireTimeEnd')">
      <a-form-item :label="pi.queryLabel('newsExpireTimeEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.newsExpireTimeEnd"
          :placeholder="pi.queryPh('newsExpireTimeEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('newsReadCount')">
      <a-form-item :label="pi.queryLabel('newsReadCount')">
        <a-input-number
          v-model:value="advancedQueryForm.newsReadCount"
          :placeholder="pi.queryPh('newsReadCount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('newsLikeCount')">
      <a-form-item :label="pi.queryLabel('newsLikeCount')">
        <a-input-number
          v-model:value="advancedQueryForm.newsLikeCount"
          :placeholder="pi.queryPh('newsLikeCount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('newsCommentCount')">
      <a-form-item :label="pi.queryLabel('newsCommentCount')">
        <a-input-number
          v-model:value="advancedQueryForm.newsCommentCount"
          :placeholder="pi.queryPh('newsCommentCount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('newsFavoriteCount')">
      <a-form-item :label="pi.queryLabel('newsFavoriteCount')">
        <a-input-number
          v-model:value="advancedQueryForm.newsFavoriteCount"
          :placeholder="pi.queryPh('newsFavoriteCount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('newsShareCount')">
      <a-form-item :label="pi.queryLabel('newsShareCount')">
        <a-input-number
          v-model:value="advancedQueryForm.newsShareCount"
          :placeholder="pi.queryPh('newsShareCount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deptId')">
      <a-form-item :label="pi.queryLabel('deptId')">
        <TaktSelect
          v-model:value="advancedQueryForm.deptId"
          api-url="TaktDepts/tree-options"
          :placeholder="pi.queryPh('deptId', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deptName')">
      <a-form-item :label="pi.queryLabel('deptName')">
        <a-input
          v-model:value="advancedQueryForm.deptName"
          :placeholder="pi.queryPh('deptName', 'required')"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('publisherId')">
      <a-form-item :label="pi.queryLabel('publisherId')">
        <TaktSelect
          v-model:value="advancedQueryForm.publisherId"
          api-url="TaktUsers/options"
          :placeholder="pi.queryPh('publisherId', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('publisherName')">
      <a-form-item :label="pi.queryLabel('publisherName')">
        <a-input
          v-model:value="advancedQueryForm.publisherName"
          :placeholder="pi.queryPh('publisherName', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('newsPublishTimeStart')">
      <a-form-item :label="pi.queryLabel('newsPublishTimeStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.newsPublishTimeStart"
          :placeholder="pi.queryPh('newsPublishTimeStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('newsPublishTimeEnd')">
      <a-form-item :label="pi.queryLabel('newsPublishTimeEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.newsPublishTimeEnd"
          :placeholder="pi.queryPh('newsPublishTimeEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('targetScope')">
      <a-form-item :label="pi.queryLabel('targetScope')">
        <TaktSelect
          v-model:value="advancedQueryForm.targetScope"
          dict-type="sys_publish_scope"
          :placeholder="pi.queryPh('targetScope', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('targetDepartments')">
      <a-form-item :label="pi.queryLabel('targetDepartments')">
        <a-input
          v-model:value="advancedQueryForm.targetDepartments"
          :placeholder="pi.queryPh('targetDepartments', 'required')"
          show-count
          :maxlength="1000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('targetUsers')">
      <a-form-item :label="pi.queryLabel('targetUsers')">
        <a-input
          v-model:value="advancedQueryForm.targetUsers"
          :placeholder="pi.queryPh('targetUsers', 'required')"
          show-count
          :maxlength="2000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('newsStatus')">
      <a-form-item :label="pi.queryLabel('newsStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.newsStatus"
          dict-type="sys_publish_status"
          :placeholder="pi.queryPh('newsStatus', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvalStatus')">
      <a-form-item :label="pi.queryLabel('approvalStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.approvalStatus"
          dict-type="sys_approval_status"
          :placeholder="pi.queryPh('approvalStatus', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatorId')">
      <a-form-item :label="pi.queryLabel('initiatorId')">
        <a-input
          v-model:value="advancedQueryForm.initiatorId"
          :placeholder="pi.queryPh('initiatorId', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtStart')">
      <a-form-item :label="pi.queryLabel('initiatedAtStart')">
        <a-input
          v-model:value="advancedQueryForm.initiatedAtStart"
          :placeholder="pi.queryPh('initiatedAtStart', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtEnd')">
      <a-form-item :label="pi.queryLabel('initiatedAtEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.initiatedAtEnd"
          :placeholder="pi.queryPh('initiatedAtEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedBy')">
      <a-form-item :label="pi.queryLabel('approvedBy')">
        <a-input
          v-model:value="advancedQueryForm.approvedBy"
          :placeholder="pi.queryPh('approvedBy', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtStart')">
      <a-form-item :label="pi.queryLabel('approvedAtStart')">
        <a-input
          v-model:value="advancedQueryForm.approvedAtStart"
          :placeholder="pi.queryPh('approvedAtStart', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtEnd')">
      <a-form-item :label="pi.queryLabel('approvedAtEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.approvedAtEnd"
          :placeholder="pi.queryPh('approvedAtEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('flowInstanceId')">
      <a-form-item :label="pi.queryLabel('flowInstanceId')">
        <a-input
          v-model:value="advancedQueryForm.flowInstanceId"
          :placeholder="pi.queryPh('flowInstanceId', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtStart')">
      <a-form-item :label="pi.queryLabel('createdAtStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtStart"
          :placeholder="pi.queryPh('createdAtStart', 'select')"
          value-format="YYYY-MM-DD HH:mm:ss"
            show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtEnd')">
      <a-form-item :label="pi.queryLabel('createdAtEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtEnd"
          :placeholder="pi.queryPh('createdAtEnd', 'select')"
          value-format="YYYY-MM-DD HH:mm:ss"
            show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('extField')">
      <a-form-item
        name="extField"
        class="takt-form-item-ext-field"
        :label-col="{ style: { width: 'auto', maxWidth: 'none', flex: '0 0 auto' } }"
        :wrapper-col="{ style: { flex: '1 1 0', minWidth: 0 } }"
      >
        <template #label>
          <span class="takt-form-ext-field-label">
            <a-tooltip
              :title="t('common.page.entity.extfieldhint')"
              placement="top"
            >
              <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
            </a-tooltip>
            <span>{{ pi.queryLabel('extField') }}</span>
          </span>
        </template>
        <a-textarea
          v-model:value="advancedQueryForm.extField"
          :placeholder="t('common.page.form.placeholder.extfield')"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('remark')">
      <a-form-item :label="pi.queryLabel('remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="pi.queryPh('remark', 'optional')"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
        />
      </a-form-item>
      </div>
      </template>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: pi.self() })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        v-if="importVisible"
        :entity-i18n-key="NEWS_SELF_I18N_KEY"
        file-type="xlsx"
        :sheet-name="excelNames.sheet"
        :template-file-name="excelNames.fileBase"
        :download-template="handleDownloadTemplate"
        :import-file="handleImportFile"
        :max-size="10"
        :max-rows="1000"
        @success="handleImportSuccess"
      />
    </TaktModal>
    <!-- 列设置抽屉 -->
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'newsId'"
      :action-column-key="'action'"
      entity-scope="approval"
      table-mode="masterDetailMaster"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 新闻管理主从页（左新闻 + 右评论/点赞/收藏/分享）
 * @module views/routine/news-center/news
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import NewsForm from './components/NewsForm.vue'
import NewsCommentPanel from './components/NewsCommentPanel.vue'
import NewsEngagementPanel from './components/NewsEngagementPanel.vue'
import { provideNewsMasterContext, type NewsRowRecord } from './composables/useNewsMasterContext'
import { getNewsList, getNewsById, createNews, updateNews, deleteNewsById, deleteNewsBatch, getNewsTemplate, importNews, exportNews, updateNewsStatus } from '@/api/routine/news-center/news.api'
import type { News, NewsQuery } from '@/types/routine/news-center/news.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { normalizeImportResult, type TaktImportResult } from '@/utils/taktImportResult'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

import {
  useNewsI18n,
  NEWS_LIST_FIELDS,
  NEWS_QUERY_STRING_FIELDS,
  NEWS_QUERY_FIELDS,
  NEWS_SELF_I18N_KEY,
} from './composables/useNewsI18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useNewsI18n()

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktNews')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<News[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selecmedRow = ref<NewsRowRecord | null>(null)
/** 表格多选行 */
const selecmedRows = ref<NewsRowRecord[]>([])
/** 表格多选 row-key 集合 */
const selecmedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<News> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/**
 * 是否存在任一业务查询条件（分页除外）；无参时不请求列表/导出
 * @returns {boolean}
 */
function hasAnyListQueryFilter(): boolean {
  const kw = (queryKeyword.value ?? '').trim()
  if (kw.length > 0) {
    return true
  }
  const form = advancedQueryForm.value
  for (const key of NEWS_QUERY_STRING_FIELDS) {
    if (String(form[key] ?? '').trim().length > 0) {
      return true
    }
  }
  if (form.newsCategory !== undefined && form.newsCategory !== null) {
    return true
  }
  if (form.newsIsTop !== undefined && form.newsIsTop !== null) {
    return true
  }
  if (form.newsIsRecommended !== undefined && form.newsIsRecommended !== null) {
    return true
  }
  if (form.newsReadCount !== undefined && form.newsReadCount !== null) {
    return true
  }
  if (form.newsLikeCount !== undefined && form.newsLikeCount !== null) {
    return true
  }
  if (form.newsCommentCount !== undefined && form.newsCommentCount !== null) {
    return true
  }
  if (form.newsFavoriteCount !== undefined && form.newsFavoriteCount !== null) {
    return true
  }
  if (form.newsShareCount !== undefined && form.newsShareCount !== null) {
    return true
  }
  if (form.targetScope !== undefined && form.targetScope !== null) {
    return true
  }
  if (form.newsStatus !== undefined && form.newsStatus !== null) {
    return true
  }
  if (form.approvalStatus !== undefined && form.approvalStatus !== null) {
    return true
  }
  return false
}

/**
 * 创建空的高级查询表单（无默认填充；无参时列表保持空）
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(NEWS_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof NEWS_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    newsCategory: undefined as number | undefined,
    newsIsTop: undefined as number | undefined,
    newsIsRecommended: undefined as number | undefined,
    newsReadCount: undefined as number | undefined,
    newsLikeCount: undefined as number | undefined,
    newsCommentCount: undefined as number | undefined,
    newsFavoriteCount: undefined as number | undefined,
    newsShareCount: undefined as number | undefined,
    targetScope: undefined as number | undefined,
    newsStatus: undefined as number | undefined,
    approvalStatus: undefined as number | undefined,  }
}
/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  NEWS_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
)
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 导入对话框是否打开 */
const importVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = 'newsId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selecmedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selecmedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 主表选中行上下文（右侧明细面板读取） */
const { selecmedMasterRow } = provideNewsMasterContext()
/** 右侧互动 Tab：评论 / 点赞 / 收藏 / 分享 */
const detailActiveTab = ref<'comment' | 'like' | 'favorite' | 'share'>('comment')
const newsCommentPanelRef = ref<InstanceType<typeof NewsCommentPanel> | null>(null)
const newsLikePanelRef = ref<InstanceType<typeof NewsEngagementPanel> | null>(null)
const newsFavoritePanelRef = ref<InstanceType<typeof NewsEngagementPanel> | null>(null)
const newsSharePanelRef = ref<InstanceType<typeof NewsEngagementPanel> | null>(null)

/**
 * 刷新当前选中新闻的右侧互动面板
 * @returns {void}
 */
function reloadEngagementPanels(): void {
  if (!selecmedMasterKey.value) {
    return
  }
  newsCommentPanelRef.value?.reload?.()
  newsLikePanelRef.value?.reload?.()
  newsFavoritePanelRef.value?.reload?.()
  newsSharePanelRef.value?.reload?.()
}

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400；无参不补默认）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {NewsQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<NewsQuery>): NewsQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: NewsQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof NewsQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of NEWS_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.newsCategory !== undefined && form.newsCategory !== null) {
    query.newsCategory = form.newsCategory
  }
  if (form.newsIsTop !== undefined && form.newsIsTop !== null) {
    query.newsIsTop = form.newsIsTop
  }
  if (form.newsIsRecommended !== undefined && form.newsIsRecommended !== null) {
    query.newsIsRecommended = form.newsIsRecommended
  }
  if (form.newsReadCount !== undefined && form.newsReadCount !== null) {
    query.newsReadCount = form.newsReadCount
  }
  if (form.newsLikeCount !== undefined && form.newsLikeCount !== null) {
    query.newsLikeCount = form.newsLikeCount
  }
  if (form.newsCommentCount !== undefined && form.newsCommentCount !== null) {
    query.newsCommentCount = form.newsCommentCount
  }
  if (form.newsFavoriteCount !== undefined && form.newsFavoriteCount !== null) {
    query.newsFavoriteCount = form.newsFavoriteCount
  }
  if (form.newsShareCount !== undefined && form.newsShareCount !== null) {
    query.newsShareCount = form.newsShareCount
  }
  if (form.targetScope !== undefined && form.targetScope !== null) {
    query.targetScope = form.targetScope
  }
  if (form.newsStatus !== undefined && form.newsStatus !== null) {
    query.newsStatus = form.newsStatus
  }
  if (form.approvalStatus !== undefined && form.approvalStatus !== null) {
    query.approvalStatus = form.approvalStatus
  }
  return query
}
/** 页面挂载：租户上下文就绪后加载分页配置；无查询条件时 loadData 保持空表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  void dictDataStore.loadAllDictDataAsync()
  loadData()
})

/** 主表行点击选中 key（左右主子表高亮） */
const selecmedMasterKey = ref('')

/** 同步主表选中行到右侧明细（子表由 *-panel watch 自动 reload） */
function syncMasterSelection(record: NewsRowRecord | null) {
  selecmedMasterRow.value = record
  selecmedMasterKey.value = record ? getNewsId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as NewsRowRecord
  const key = getNewsId(row)
  selecmedRowKeys.value = [key]
  selecmedRows.value = [row]
  selecmedRow.value = row
  syncMasterSelection(row)
}

/**
 * 主表分页变更（v-model 已同步页码与 pageSize）
 * @param _page 页码
 * @param _pageSize 每页条数
 */
function handleMasterPaginationChange(_page: number, _pageSize: number) {
  loadData()
}

/** 加载主表详情并回填当前页 dataSource */
async function loadNewsDetail(record: NewsRowRecord): Promise<News | null> {
  const id = getNewsId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getNewsById(id)
    const index = dataSource.value.findIndex((row) => getNewsId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as News
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'newsId',
    key: 'newsId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getNewsField(record, 'newsId') ?? ''
  },
  {
    title: pi.label('newsCode'),
    dataIndex: 'newsCode',
    key: 'newsCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'newsCode') ?? ''
  },
  {
    title: pi.label('newsCategory'),
    dataIndex: 'newsCategory',
    key: 'newsCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('newsTitle'),
    dataIndex: 'newsTitle',
    key: 'newsTitle',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'newsTitle') ?? ''
  },
  {
    title: pi.label('newsSummary'),
    dataIndex: 'newsSummary',
    key: 'newsSummary',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'newsSummary') ?? ''
  },
  {
    title: pi.label('newsTags'),
    dataIndex: 'newsTags',
    key: 'newsTags',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'newsTags') ?? ''
  },
  {
    title: pi.label('newsContent'),
    dataIndex: 'newsContent',
    key: 'newsContent',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'newsContent') ?? ''
  },
  {
    title: pi.label('newsCoverImage'),
    dataIndex: 'newsCoverImage',
    key: 'newsCoverImage',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'newsCoverImage') ?? ''
  },
  {
    title: pi.label('newsIsTop'),
    dataIndex: 'newsIsTop',
    key: 'newsIsTop',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('newsIsRecommended'),
    dataIndex: 'newsIsRecommended',
    key: 'newsIsRecommended',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('newsEffectiveTime'),
    dataIndex: 'newsEffectiveTime',
    key: 'newsEffectiveTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'newsEffectiveTime') ?? ''
  },
  {
    title: pi.label('newsExpireTime'),
    dataIndex: 'newsExpireTime',
    key: 'newsExpireTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'newsExpireTime') ?? ''
  },
  {
    title: pi.label('newsReadCount'),
    dataIndex: 'newsReadCount',
    key: 'newsReadCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'newsReadCount') ?? ''
  },
  {
    title: pi.label('newsLikeCount'),
    dataIndex: 'newsLikeCount',
    key: 'newsLikeCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'newsLikeCount') ?? ''
  },
  {
    title: pi.label('newsCommentCount'),
    dataIndex: 'newsCommentCount',
    key: 'newsCommentCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'newsCommentCount') ?? ''
  },
  {
    title: pi.label('newsFavoriteCount'),
    dataIndex: 'newsFavoriteCount',
    key: 'newsFavoriteCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'newsFavoriteCount') ?? ''
  },
  {
    title: pi.label('newsShareCount'),
    dataIndex: 'newsShareCount',
    key: 'newsShareCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'newsShareCount') ?? ''
  },
  {
    title: pi.label('deptId'),
    dataIndex: 'deptId',
    key: 'deptId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'deptId') ?? ''
  },
  {
    title: pi.label('deptName'),
    dataIndex: 'deptName',
    key: 'deptName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'deptName') ?? ''
  },
  {
    title: pi.label('publisherId'),
    dataIndex: 'publisherId',
    key: 'publisherId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'publisherId') ?? ''
  },
  {
    title: pi.label('publisherName'),
    dataIndex: 'publisherName',
    key: 'publisherName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'publisherName') ?? ''
  },
  {
    title: pi.label('newsPublishTime'),
    dataIndex: 'newsPublishTime',
    key: 'newsPublishTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'newsPublishTime') ?? ''
  },
  {
    title: pi.label('targetScope'),
    dataIndex: 'targetScope',
    key: 'targetScope',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('targetDepartments'),
    dataIndex: 'targetDepartments',
    key: 'targetDepartments',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'targetDepartments') ?? ''
  },
  {
    title: pi.label('targetUsers'),
    dataIndex: 'targetUsers',
    key: 'targetUsers',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNewsField(record, 'targetUsers') ?? ''
  },
  {
    title: pi.label('newsStatus'),
    dataIndex: 'newsStatus',
    key: 'newsStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'routine:news:center:update',
        onClick: (record: NewsRowRecord) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'routine:news:center:delete',
        onClick: (record: NewsRowRecord) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getNewsId = (record: NewsRowRecord): string => {
  const id = (record as Record<string, unknown>)?.[entityIdName]
  return id != null ? String(id) : ''
}
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getNewsField = (record: any, field: string): any => record?.[field]
/**
 * 供 TaktDictTag 等组件使用的标量字典值
 * @param record 行数据
 * @param field 字段名
 */
const getNewsDictValue = (
  record: NewsRowRecord,
  field: string,
): string | number | undefined => {
  const value = (record as Record<string, unknown>)?.[field]
  if (value === null || value === undefined) return undefined
  if (typeof value === 'string' || typeof value === 'number') return value
  return String(value)
}

/** 行选择配置 */
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: NewsRowRecord[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: NewsRowRecord, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
      syncMasterSelection(record)
    } else if (selecmedRow.value && getNewsId(selecmedRow.value) === getNewsId(record)) {
      selecmedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: NewsRowRecord[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
    syncMasterSelection(selecmedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    if (!hasAnyListQueryFilter()) {
      dataSource.value = []
      total.value = 0
      return
    }
    const res = await getNewsList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[News] 加载数据失败', { error })
    message.error(error?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

/** 租户/公司切换时由 bootstrap 发出 table:refresh，自动重载列表 */
useTableRefresh(loadData)

/** 快捷查询 */
function handleSearch() {
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 重置查询条件并刷新列表 */
function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = {
  cultureCode: '',
  plantCode: '',
  newsCode: '',
  newsCategory: undefined as number | undefined,
  newsTitle: '',
  newsSummary: '',
  newsTags: '',
  newsContent: '',
  newsCoverImage: '',
  newsIsTop: undefined as number | undefined,
  newsIsRecommended: undefined as number | undefined,
  newsEffectiveTimeStart: '',
  newsEffectiveTimeEnd: '',
  newsExpireTimeStart: '',
  newsExpireTimeEnd: '',
  newsReadCount: undefined as number | undefined,
  newsLikeCount: undefined as number | undefined,
  newsCommentCount: undefined as number | undefined,
  newsFavoriteCount: undefined as number | undefined,
  newsShareCount: undefined as number | undefined,
  deptId: '',
  deptName: '',
  publisherId: '',
  publisherName: '',
  newsPublishTimeStart: '',
  newsPublishTimeEnd: '',
  targetScope: undefined as number | undefined,
  targetDepartments: '',
  targetUsers: '',
  newsStatus: undefined as number | undefined,
  approvalStatus: undefined as number | undefined,
  initiatorId: '',
  initiatedAtStart: '',
  initiatedAtEnd: '',
  approvedBy: '',
  approvedAtStart: '',
  approvedAtEnd: '',
  flowInstanceId: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
  }
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: pi.self() })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: NewsRowRecord) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await loadNewsDetail(record)
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
  } finally {
    formLoading.value = false
  }
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selecmedRow.value) {
    void handleEdit(selecmedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: pi.self() }))
  }
}
/** 提交新增/编辑表单 */
async function handleFormSubmit() {
  const refInst = formRef.value
  if (!refInst?.validate) return
  try {
    await refInst.validate()
  } catch {
    return
  }
  formLoading.value = true
  try {
    const payload = refInst.getValues?.() ?? { ...(formData.value as any) }
    const id = (formData.value as any)?.[entityIdName]
    if (id) {
      await updateNews(id, payload as any)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createNews(payload as any)
      message.success(t('common.feedback.created', { target: pi.self() }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selecmedMasterKey.value) {
      reloadEngagementPanels()
    }
    loadData()
  } finally {
    formLoading.value = false
  }
}

/** 关闭新增/编辑弹窗（不提交） */
function handleFormCancel() {
  formVisible.value = false
  formData.value = null
  nextTick(() => formRef.value?.resetFields())
}
/** 打开导入对话框 */
function handleImport() {
  importVisible.value = true
}

/** 下载导入模板 Excel */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getNewsTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importNews(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  loadData()
  reloadEngagementPanels()
  if (result.fail === 0 && result.success > 0) {
    setTimeout(() => { importVisible.value = false }, 2000)
  }
}

/** 关闭导入对话框 */
function handleImportCancel() {
  importVisible.value = false
}
/** 导出当前查询条件下的 Excel */
async function handleExport() {
  try {
    loading.value = true
    if (!hasAnyListQueryFilter()) {
      return
    }
    const exportMeta = await exportNews(
      buildListQuery({ pageIndex: 1, pageSize: 100000 }),
      excelNames.sheet,
      excelNames.fileBase
    )
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fallbackBase = `${excelNames.fileBase}_${ts.getFullYear()}${pad(ts.getMonth() + 1)}${pad(ts.getDate())}${pad(ts.getHours())}${pad(ts.getMinutes())}${pad(ts.getSeconds())}`
    const fileName = resolveExportDownloadFileName({
      contentDisposition: (exportMeta as any).contentDisposition ?? null,
      contentType: (exportMeta as any).contentType ?? null,
      fallbackBase
    })
    const blob = (exportMeta as any).blob ?? exportMeta
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
    message.success(t('common.feedback.export.success', { target: pi.self() }))
  } catch (error: any) {
    logger.error('[News] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: NewsRowRecord) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteNewsById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      selecmedRowKeys.value = []
      selecmedRows.value = []
      selecmedRow.value = null
      syncMasterSelection(null)
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selecmedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: pi.self() }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: pi.self(), count: selecmedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selecmedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteNewsBatch(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      selecmedRowKeys.value = []
      selecmedRows.value = []
      selecmedRow.value = null
      syncMasterSelection(null)
      loadData()
    }
  })
}
/** 打开高级查询抽屉 */
function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

/** 高级查询提交：关闭抽屉并重置分页 */
function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
  cultureCode: '',
  plantCode: '',
  newsCode: '',
  newsCategory: undefined as number | undefined,
  newsTitle: '',
  newsSummary: '',
  newsTags: '',
  newsContent: '',
  newsCoverImage: '',
  newsIsTop: undefined as number | undefined,
  newsIsRecommended: undefined as number | undefined,
  newsEffectiveTimeStart: '',
  newsEffectiveTimeEnd: '',
  newsExpireTimeStart: '',
  newsExpireTimeEnd: '',
  newsReadCount: undefined as number | undefined,
  newsLikeCount: undefined as number | undefined,
  newsCommentCount: undefined as number | undefined,
  newsFavoriteCount: undefined as number | undefined,
  newsShareCount: undefined as number | undefined,
  deptId: '',
  deptName: '',
  publisherId: '',
  publisherName: '',
  newsPublishTimeStart: '',
  newsPublishTimeEnd: '',
  targetScope: undefined as number | undefined,
  targetDepartments: '',
  targetUsers: '',
  newsStatus: undefined as number | undefined,
  approvalStatus: undefined as number | undefined,
  initiatorId: '',
  initiatedAtStart: '',
  initiatedAtEnd: '',
  approvedBy: '',
  approvedAtStart: '',
  approvedAtEnd: '',
  flowInstanceId: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
  }
}

/** 打开列设置抽屉 */
function handleColumnSetting() {
  columnSettingVisible.value = true
}

/** 列设置：更新可见列 key */
function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

/** 列设置：恢复默认可见列 */
function handleColumnSettingReset() {
  visibleColumnKeys.value = []
}

/** 刷新列表 */
function handleRefresh() {
  loadData()
}

/** 表格 change 占位 */
function handleTableChange() {}
/** 列宽拖拽回调占位 */
function handleResizeColumn() {}
</script>
