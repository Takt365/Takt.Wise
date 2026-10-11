<!-- ======================================== -->
<!-- 项目名称：TaktWise -->
<!-- 命名空间：@/components/business/TaktSelect -->
<!-- 文件名称：index.vue -->
<!-- 创建时间：2025-01-20 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：Takt 下拉；多选默认宽=基准×2 + maxTagCount responsive 溢出；≥3000 虚拟+远程搜索 -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <!-- 字典数量 3 个及以下且非多选模式下，使用 Radio 单选 -->
  <a-radio-group
    v-if="shouldUseRadio && !multiple"
    :value="effectiveModelValue"
    :disabled="disabled"
    :size="radioSize"
    @change="handleRadioChange"
  >
    <a-radio
      v-for="option in radioOptions"
      :key="String(option.value)"
      :value="option.value"
    >
      {{ option.label }}
    </a-radio>
  </a-radio-group>
  
  <!-- 其他情况使用 Select 下拉选择框 -->
  <a-select
    v-else
    :class="selectRootClass"
    :value="effectiveModelValue"
    :options="options"
    :loading="loading"
    :placeholder="placeholder ?? t('common.page.form.placeholder.selectonly')"
    :allow-clear="allowClear"
    :disabled="disabled"
    :mode="multiple ? 'multiple' : undefined"
    :size="size"
    :show-search="showSearch"
    :filter-option="effectiveFilterOption"
    :option-filter-prop="effectiveRemoteSearch ? undefined : 'label'"
    :virtual="shouldUseVirtual"
    :list-height="listHeight"
    v-bind="{
      ...$attrs,
      ...(multiple ? { maxTagCount: effectiveMaxTagCount } : {})
    }"
    @change="handleChange"
    @search="handleSearch"
  >
    <template
      v-if="$slots.default"
      #default
    >
      <slot />
    </template>
  </a-select>
</template>

<script setup lang="ts">
import type { SelectValue, DefaultOptionType } from 'ant-design-vue/es/select'
import type { TaktDictSelectFieldNames, TaktDictSelectOption, TaktSelectOption } from '@/types/common.types'
import request from '@/api/request'
import { createLogger } from '@/utils/logger'
import { TAKT_LARGE_DATA_AUTO_THRESHOLD } from '@/utils/taktLargeData'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { isEmptyFormFieldValue } from '@/utils/taktDictDefault'
import { translateLocaleMessage } from '@/utils/taktI18nMessage'
import { useI18n } from 'vue-i18n'

const selectLogger = createLogger('takt-select')

const { t } = useI18n()
type SelectOptionLike = { label?: string; value?: string | number; dictLabel?: string; dictValue?: string | number; extLabel?: string; extValue?: string | number } & Record<string, unknown>

interface Props {
  /** 绑定值 */
  modelValue?: string | number | (string | number)[] | undefined
  /** 字典类型编码(用于加载字典数据,优先级高于 apiUrl) */
  dictType?: string | undefined
  /** API 端点(可选,如果提供了 dictType 或 options 则不需要) */
  apiUrl?: string | undefined
  /** 选项数据(可选,如果提供了则直接使用,不再通过 dictType 或 apiUrl 加载)。支持标准格式 { label, value } 或 TaktSelectOption 格式 { dictLabel, dictValue } */
  options?: TaktSelectOption[] | Array<{ label: string; value: string | number } & Record<string, unknown>> | undefined
  /** 占位符 */
  placeholder?: string | undefined
  /** 是否显示清除按钮 */
  allowClear?: boolean
  /** 是否禁用 */
  disabled?: boolean
  /** 是否多选 */
  multiple?: boolean
  /** 尺寸 */
  size?: 'small' | 'middle' | 'large'
  /** 是否支持搜索 */
  showSearch?: boolean
  /** 自定义过滤函数 */
  filterOption?: boolean | ((input: string, option?: DefaultOptionType) => boolean)
  /**
   * 多选标签上限；未传时默认 responsive（按控件宽度溢出为 +N）。
   * 可传数字固定显示个数。表单 w-full 时仍用 responsive 按实际宽度溢出。
   */
  maxTagCount?: number | 'responsive' | undefined
  /** 是否开启虚拟滚动；未显式指定时，选项数 ≥ 3000 自动开启 */
  virtual?: boolean
  /** API 请求附加查询参数（与 apiUrl 联用，如 plantCode、keyword） */
  apiParams?: Record<string, string | number | boolean | undefined | null> | undefined
  /** 是否远程搜索；未显式指定时，apiUrl 且首包选项数 ≥ 3000 自动开启 */
  remoteSearch?: boolean
  /** 远程搜索时写入请求的查询参数字段名，默认 keyword */
  searchParamKey?: string
  /** 远程搜索防抖毫秒，默认 300 */
  searchDebounceMs?: number
  /** 虚拟滚动时列表高度(单位:px),默认 256px */
  listHeight?: number
  /** 字段映射配置(用于自定义 label 和 value 字段名) */
  fieldNames?: {
    label?: string
    value?: string
  }
  /** dict-type 模式下值未绑定时是否自动选中 IsDefault=1 项（默认 true；用户手动清空后不再回填） */
  applyDictDefault?: boolean
  /** 仅保留这些字典值（与 dict-type 联用；空则不过滤） */
  includeValues?: ReadonlyArray<string | number> | undefined
  /**
   * 业务区域文化（如 DefaultCulture）；dict-type 时仅显示 CultureCode=eo 或匹配项
   * 未传时按 Accept-Language + eo 过滤
   */
  cultureCode?: string | undefined
}

const props = withDefaults(defineProps<Props>(), {
  modelValue: undefined,
  dictType: undefined,
  apiUrl: undefined,
  options: undefined,
  placeholder: undefined,
  allowClear: true,
  disabled: false,
  multiple: false,
  size: 'middle',
  showSearch: true,
  filterOption: true,
  maxTagCount: undefined,
  virtual: undefined,
  apiParams: undefined,
  remoteSearch: undefined,
  searchParamKey: 'keyword',
  searchDebounceMs: 300,
  listHeight: 256,
  fieldNames: () => ({
    label: 'label',
    value: 'value'
  }),
  applyDictDefault: true,
  cultureCode: undefined,
  includeValues: undefined,
})

/**
 * 默认客户端过滤：按 label / value / dictLabel / dictValue 模糊匹配
 * @param input 搜索输入
 * @param option 选项
 * @returns 是否保留
 */
function defaultFilterOption(input: string, option?: DefaultOptionType): boolean {
  const needle = (input || '').trim().toLowerCase()
  if (!needle) {
    return true
  }
  const opt = option as SelectOptionLike | undefined
  const nested = (option as { option?: SelectOptionLike } | undefined)?.option
  const label = String(opt?.label ?? nested?.label ?? opt?.dictLabel ?? nested?.dictLabel ?? '')
  const value = String(opt?.value ?? nested?.value ?? opt?.dictValue ?? nested?.dictValue ?? '')
  const extLabel = String(opt?.extLabel ?? nested?.extLabel ?? '')
  return `${label} ${value} ${extLabel}`.toLowerCase().includes(needle)
}

const emit = defineEmits<{
  'update:modelValue': [value: string | number | (string | number)[] | undefined]
  /** 与模板 v-model:value 对齐（父级不走 modelValue） */
  'update:value': [value: string | number | (string | number)[] | undefined]
  'change': [value: string | number | (string | number)[] | undefined, option: SelectOptionLike | SelectOptionLike[] | null]
  'search': [value: string]
}>()

const attrs = useAttrs()

/**
 * 当前绑定值。v-model 走 modelValue；v-model:value 落在 attrs.value（未声明为 prop）
 * @returns 绑定值；未赋值时为 undefined
 */
function resolveBoundValue(): string | number | (string | number)[] | undefined {
  if (!isEmptyFormFieldValue(props.modelValue)) {
    return props.modelValue
  }
  const fromValueModel = attrs.value as string | number | (string | number)[] | undefined
  if (!isEmptyFormFieldValue(fromValueModel)) {
    return fromValueModel
  }
  return undefined
}

/**
 * 同步两种 v-model 事件，避免只写 modelValue 时 v-model:value 收不到默认值
 * @param value 新绑定值
 */
function emitBoundValue(value: string | number | (string | number)[] | undefined): void {
  emit('update:modelValue', value)
  emit('update:value', value)
}

/**
 * 是否已由调用方指定占满宽度（勿再套多选自动 2× 宽）
 * @returns {boolean} 含 w-full 等则为 true
 */
function hasExplicitFullWidthClass(): boolean {
  const cls = attrs.class
  const text = Array.isArray(cls) ? cls.map(String).join(' ') : String(cls ?? '')
  return /\bw-full\b/.test(text) || /\bmin-w-full\b/.test(text)
}

/** 根 class：多选自动宽=基准×2（见 select-base.css） */
const selectRootClass = computed(() => {
  const multiple = !!props.multiple
  return [
    'takt-select',
    multiple ? 'takt-select--multiple' : 'takt-select--single',
    multiple && !hasExplicitFullWidthClass() ? 'takt-select--multiple-auto-width' : null,
  ]
})

const loading = ref(false)
const rawData = ref<TaktSelectOption[]>([])
const remoteSearchKeyword = ref('')
let remoteSearchTimer: ReturnType<typeof setTimeout> | undefined
/** apiUrl 首包（无 keyword）选项数 ≥ 阈值后自动远程搜索 */
const autoLargeRemoteSearch = ref(false)
const dictDataStore = useDictDataStore()
/** 当前 dictType 是否已自动写入过 IsDefault（用户清空后不再自动回填） */
const dictDefaultApplied = ref(false)

/**
 * 是否远程搜索：显式 true 强制开；显式 false 强制关；未指定时按大数据自动开
 */
const effectiveRemoteSearch = computed(
  () => props.remoteSearch === true || (props.remoteSearch !== false && autoLargeRemoteSearch.value),
)

const effectiveFilterOption = computed(() => {
  // 仅显式 remote-search：纯远程，关闭客户端过滤（后端须支持 keyword）
  if (props.remoteSearch === true && props.apiUrl) {
    return false
  }
  if (typeof props.filterOption === 'function') {
    const customFilter = props.filterOption
    return (input: string, option?: DefaultOptionType) => customFilter(input, option)
  }
  if (props.filterOption === true) {
    return defaultFilterOption
  }
  return props.filterOption
})

/**
 * 解析 dict-type 模式下 valueField 映射
 * @returns getDictDefaultValue 使用的 valueField
 */
function resolveDictValueField(): TaktDictSelectFieldNames['valueField'] {
  const valueFieldKey = props.fieldNames?.value
  if (valueFieldKey === 'extLabel') {
    return 'extLabel'
  }
  if (valueFieldKey === 'extValue') {
    return 'extValue'
  }
  if (valueFieldKey === 'sortOrder') {
    return 'sortOrder'
  }
  return 'dictValue'
}

/**
 * dict-type 且绑定值为空时，按 IsDefault=1 自动选中（每 dictType 仅一次）
 */
function tryApplyDictDefault(): void {
  if (!props.dictType || props.applyDictDefault === false || props.multiple || dictDefaultApplied.value) {
    return
  }
  if (!isEmptyFormFieldValue(resolveBoundValue())) {
    return
  }

  const dictValueField = resolveDictValueField()
  const defaultValue = dictDataStore.getDictDefaultValue(props.dictType, dictValueField, props.cultureCode)
  if (defaultValue === undefined) {
    return
  }

  const boundValue = resolveBoundValue()
  let expectedValueType = inferValueType(boundValue)
  if (expectedValueType === 'string' && boundValue == null) {
    const dictOptions = dictDataStore.getDictOptionsForSelect(props.dictType, {
      valueField: dictValueField,
      labelField: props.fieldNames?.label === 'extLabel' ? 'extLabel' : 'dictLabel',
    }, props.cultureCode)
    if (dictOptions.every((option) => isNumericValue(option.value))) {
      expectedValueType = 'number'
    }
  }

  const rawValue = typeof defaultValue === 'number' ? defaultValue : String(defaultValue)
  const convertedValue = convertValueType(rawValue, expectedValueType, props.dictType)
  dictDefaultApplied.value = true
  emit('update:modelValue', convertedValue)
}

/**
 * 推断期望的值类型（根据 modelValue 的类型）
 * @param modelValue 绑定值
 * @returns 期望的值类型：'number' | 'string'
 */
function inferValueType(modelValue: string | number | (string | number)[] | undefined): 'number' | 'string' {
  if (modelValue === undefined || modelValue === null) {
    // 无法推断，默认返回 string
    return 'string'
  }
  
  // 如果是数组，检查第一个元素的类型
  if (Array.isArray(modelValue)) {
    if (modelValue.length === 0) {
      return 'string'
    }
    return typeof modelValue[0] === 'number' ? 'number' : 'string'
  }
  
  // 单个值，直接检查类型
  return typeof modelValue === 'number' ? 'number' : 'string'
}

/**
 * 判断值是否是数值类型（number 或可转换为 number 的 string）
 */
function isNumericValue(value: string | number | (string | number)[] | undefined): boolean {
  if (value == null) return false
  
  if (Array.isArray(value)) {
    if (value.length === 0) return false
    const first = value[0]
    return typeof first === 'number' || (typeof first === 'string' && first !== '' && !isNaN(Number(first)))
  }
  
  return typeof value === 'number' || (typeof value === 'string' && value !== '' && !isNaN(Number(value)))
}

/**
 * 判断字符串是否是纯数字字符串（用于字典值的类型转换）
 */
function isNumericString(str: string): boolean {
  if (!str || str.trim() === '') return false
  const trimmed = String(str).trim()
  if (!/^-?\d+(\.\d+)?$/.test(trimmed) || Number.isNaN(Number(trimmed))) return false
  return Number.isSafeInteger(Number(trimmed))
}

/**
 * 转换值类型（根据期望的类型转换字典数据的值）
 */
function convertValueType(value: string | number, expectedType: 'number' | 'string', dictType: string): string | number {
  if (typeof value === expectedType) {
    return value
  }
  
  if (expectedType === 'number' && typeof value === 'string') {
    if (!value || value.trim() === '') {
      selectLogger.warn('字典值为空，无法转换为 number，返回 0', {
        action: 'convertValueType',
        dictType,
        value,
      })
      return 0
    }
    
    if (!isNumericString(value)) {
      selectLogger.error('字典值不是数值字符串，无法转换为 number', {
        action: 'convertValueType',
        dictType,
        value,
      })
      throw new Error(`字典数据值类型转换失败：字典类型 ${dictType} 的值 "${value}" 不是数值字符串，无法转换为 number 类型`)
    }
    
    const numValue = Number(value)
    if (isNaN(numValue)) {
      selectLogger.error('字典值无法转换为 number', {
        action: 'convertValueType',
        dictType,
        value,
      })
      throw new Error(`字典数据值类型转换失败：字典类型 ${dictType} 的值 "${value}" 无法转换为 number 类型`)
    }
    
    return numValue
  }
  
  if (expectedType === 'string' && typeof value === 'number') {
    return String(value)
  }
  
  return value
}

function normalizeValue(value: unknown): string | number {
  if (typeof value === 'string' || typeof value === 'number') {
    return value
  }
  return ''
}

/**
 * 多选模式绑定值规范化：未选/空/无效 DictValue 视为 undefined，并与 options.value 对齐
 * @param modelValue 绑定值
 * @param optionList 当前下拉选项
 * @returns a-select 可识别的多选值；未选时为 undefined
 */
function normalizeMultipleSelectValue(
  modelValue: string | number | (string | number)[] | undefined | null,
  optionList: ReadonlyArray<{ value?: string | number }>,
): (string | number)[] | undefined {
  if (modelValue == null || modelValue === '') {
    return undefined
  }
  let candidates: (string | number)[]
  if (Array.isArray(modelValue)) {
    candidates = [...modelValue]
  } else if (typeof modelValue === 'number' && modelValue === 0) {
    return undefined
  } else if (modelValue === '0') {
    return undefined
  } else {
    candidates = [modelValue]
  }
  const filtered = candidates.filter((item) => {
    if (item == null) {
      return false
    }
    const text = String(item).trim()
    return text !== '' && text !== '0'
  })
  if (filtered.length === 0) {
    return undefined
  }
  if (optionList.length === 0) {
    return undefined
  }
  const aligned = filtered
    .map((item) => optionList.find((opt) => String(opt.value) === String(item))?.value)
    .filter((item): item is string | number => item != null && item !== '')
  return aligned.length > 0 ? aligned : undefined
}

/** a-select 实际绑定值（单选/多选均与 options 值类型对齐，避免 string/number 错配显示占位符） */
const effectiveModelValue = computed(() => {
  const boundValue = resolveBoundValue()
  if (props.multiple) {
    return normalizeMultipleSelectValue(
      boundValue,
      options.value as ReadonlyArray<{ value?: string | number }>,
    )
  }
  if (boundValue == null || boundValue === '') {
    return boundValue
  }
  const optionList = options.value as ReadonlyArray<{ value?: string | number }>
  if (!optionList.length) {
    return boundValue
  }
  const matched = optionList.find((opt) => String(opt.value) === String(boundValue))
  return matched != null ? matched.value : boundValue
})

// 将后端数据转换为 Select 组件需要的格式
const options = computed(() => {
  const boundValue = resolveBoundValue()
  const expectedValueType = props.apiUrl ? 'string' as const : inferValueType(boundValue)
  
  // 如果直接提供了 options，需要转换字段名和值类型
  if (props.options?.length) {
    const labelField = props.fieldNames?.label ?? 'label'
    const valueField = props.fieldNames?.value ?? 'value'
    
    return props.options.map(item => {
      const itemObj = item as SelectOptionLike
      const rawValueSource = 'value' in item ? item.value : (itemObj[valueField] ?? itemObj.dictValue ?? itemObj.extLabel ?? itemObj.extValue ?? '')
      const label = 'label' in item ? item.label : (itemObj[labelField] ?? itemObj.dictLabel ?? itemObj.extLabel ?? '')
      const rawValue = normalizeValue(rawValueSource)
      const convertedValue = convertValueType(rawValue, expectedValueType, props.dictType || 'custom')
      
      return {
        ...itemObj,
        [labelField]: label,
        [valueField]: convertedValue,
        label,
        value: convertedValue
      }
    })
  }
  
  // 如果提供了 dictType，从字典 store 加载数据
  if (props.dictType) {
    const labelField = props.fieldNames?.label
    const valueField = props.fieldNames?.value
    
    // 转换 fieldNames 为 getDictOptions 需要的格式
    const dictLabelField: 'dictLabel' | 'extLabel' = (labelField === 'extLabel' ? 'extLabel' : 'dictLabel')
    const valueFieldKey = props.fieldNames?.value
    const dictValueField: TaktDictSelectFieldNames['valueField'] =
      valueFieldKey === 'extLabel'
        ? 'extLabel'
        : valueFieldKey === 'extValue'
          ? 'extValue'
          : valueFieldKey === 'sortOrder'
            ? 'sortOrder'
            : 'dictValue'

    const dictOptions = dictDataStore.getDictOptionsForSelect(props.dictType, {
      valueField: dictValueField,
      labelField: dictLabelField
    }, props.cultureCode)

    // 仅当绑定值已是 number 时选项转 number；空值保持 string，避免先 number 后 string「请选择」
    const expectedDictValueType: 'number' | 'string' =
      typeof boundValue === 'number'
      || (Array.isArray(boundValue) && typeof boundValue[0] === 'number')
        ? 'number'
        : 'string'
    
    const includeValues = props.includeValues
    return dictOptions.map((option: TaktDictSelectOption) => {
      // sys_culture_code：DictLabel 即 NativeName；其余 i18nKey 须走树解析（避免 t('….1') 被 vue-i18n 当数组下标）
      let resolvedLabel = String(option.label ?? option.dictLabel ?? '')
      if (props.dictType === 'sys_culture_code') {
        resolvedLabel = String(option.dictLabel ?? option.label ?? '')
      } else if (option.i18nKey?.trim()) {
        const translated = translateLocaleMessage(option.i18nKey.trim())
        if (translated && translated !== option.i18nKey.trim()) {
          resolvedLabel = translated
        }
      }
      return {
        ...option,
        label: resolvedLabel,
        value: convertValueType(option.value, expectedDictValueType, props.dictType || '')
      }
    }).filter((option) => {
      if (!includeValues?.length) {
        return true
      }
      return includeValues.some((item) => String(item) === String(option.value))
    })
  }
  
  // 否则使用从 API 加载的数据
  const labelField = props.fieldNames?.label ?? 'label'
  const valueField = props.fieldNames?.value ?? 'value'
  
  return rawData.value.map(item => {
    const itemAny = item as SelectOptionLike
    const label = labelField === 'extLabel' 
      ? String(itemAny.extLabel ?? itemAny.dictLabel ?? '')
      : (itemAny.dictLabel ?? '')
    
    const rawValue = valueField === 'extLabel'
      ? (itemAny.extLabel ?? itemAny.dictValue ?? '')
      : valueField === 'extValue'
      ? (itemAny.extValue ?? itemAny.dictValue ?? '')
      : (itemAny.dictValue ?? '')
    
    // apiUrl：后端 DTO 主键已为 string（ValueToStringConverter），统一转 string 选项值
    const convertedValue = props.apiUrl
      ? convertValueType(normalizeValue(rawValue), 'string', 'api')
      : convertValueType(rawValue, expectedValueType, props.dictType || 'api')
    
    return {
      ...item,
      [labelField]: label,
      [valueField]: convertedValue,
      label,
      value: convertedValue
    }
  })
})

// 虚拟滚动：显式 true，或选项数 ≥ 3000（含静态 options / 已加载 rawData）
const shouldUseVirtual = computed(() => {
  if (props.virtual === true) {
    return true
  }
  if (props.virtual === false) {
    return false
  }
  const count = Math.max(options.value.length, rawData.value.length, props.options?.length ?? 0)
  return count >= TAKT_LARGE_DATA_AUTO_THRESHOLD || autoLargeRemoteSearch.value
})

// 判断是否应该使用 Radio 单选（字典数量 3 个及以下且非多选模式，且值必须是数值类型）
const shouldUseRadio = computed(() => {
  if (!props.dictType || props.multiple || options.value.length === 0 || options.value.length > 3) {
    return false
  }
  
  const allOptionsAreNumeric = options.value.every((option: unknown) => isNumericValue((option as SelectOptionLike).value))
  if (!allOptionsAreNumeric) {
    return false
  }
  
  const boundValue = resolveBoundValue()
  return boundValue == null || isNumericValue(boundValue)
})

// Radio 组件的尺寸（RadioGroup 不支持 'middle'，只支持 'small' | 'large' | 'default'）
const radioSize = computed(() => {
  return props.size === 'middle' ? 'default' : (props.size === 'small' ? 'small' : 'large')
})

// Radio 选项数据（options.value 已包含正确转换后的 label 和 value）
const radioOptions = computed(() => {
  return options.value.map((option: unknown) => {
    const item = option as SelectOptionLike
    return {
      label: item.label ?? item.dictLabel ?? '',
      value: item.value ?? ''
    }
  })
})

// 多选：未指定 maxTagCount 时默认 responsive（按宽度正确溢出为 +N）
const effectiveMaxTagCount = computed<number | 'responsive'>(() => {
  const val = props.maxTagCount
  if (val !== undefined && val !== null) {
    return val
  }
  return 'responsive'
})

/**
 * apiParams 已传入时，是否满足发起请求（各级联参数字段均非空）
 * @returns 是否可请求
 */
function canLoadApiOptions(): boolean {
  if (props.disabled) {
    return false
  }
  if (!props.apiParams) {
    return true
  }
  const entries = Object.entries(props.apiParams)
  if (entries.length === 0) {
    return true
  }
  return entries.every(([, val]) => val !== undefined && val !== null && String(val).trim() !== '')
}

// 加载数据
const loadData = async () => {
  // 如果提供了 options，直接使用，不需要加载
  if (props.options?.length) {
    return
  }

  // dict-type：预热 Pinia 字典缓存，避免选项空白（登录 bootstrap 未完成时弹窗已打开）
  if (props.dictType) {
    try {
      loading.value = true
      await dictDataStore.loadAllDictDataAsync()
      tryApplyDictDefault()
    } catch (error) {
      selectLogger.warn('加载字典数据失败', { action: 'loadData', dictType: props.dictType }, error)
    } finally {
      loading.value = false
    }
    return
  }

  // 如果提供了 apiUrl，通过 API 加载数据
  if (props.apiUrl) {
    if (!canLoadApiOptions()) {
      rawData.value = []
      return
    }
    try {
      loading.value = true
      const params: Record<string, string | number | boolean> = {}
      if (props.apiParams) {
        for (const [key, val] of Object.entries(props.apiParams)) {
          if (val !== undefined && val !== null && val !== '') {
            params[key] = val
          }
        }
      }
      if (effectiveRemoteSearch.value) {
        const keyword = remoteSearchKeyword.value.trim()
        if (keyword) {
          params[props.searchParamKey] = keyword
        }
      }
      const data = await request<TaktSelectOption[]>({
        url: props.apiUrl,
        method: 'get',
        params: Object.keys(params).length > 0 ? params : undefined,
      })
      rawData.value = Array.isArray(data) ? data : []
      // 首包无关键字且条数达阈值 → 后续输入走远程搜索；❌ 禁止前端截断
      const keyword = remoteSearchKeyword.value.trim()
      if (
        !keyword
        && props.remoteSearch !== false
        && rawData.value.length >= TAKT_LARGE_DATA_AUTO_THRESHOLD
      ) {
        autoLargeRemoteSearch.value = true
      }
    } catch (error) {
      selectLogger.error('加载选项数据失败', { action: 'loadData', apiUrl: props.apiUrl }, error)
      rawData.value = []
    } finally {
      loading.value = false
    }
    return
  }

  // 如果 dictType、apiUrl 和 options 都未提供，才发出警告
  selectLogger.warn('dictType、apiUrl 和 options 都未提供，无法加载数据', { action: 'loadData' })
}

// 辅助函数：从 SelectValue 中提取原始值
const extractRawValue = (value: SelectValue): string | number | (string | number)[] | undefined => {
  if (value === undefined || value === null) {
    return undefined
  }
  
  // 如果是数组
  if (Array.isArray(value)) {
    return value.map(v => {
      if (typeof v === 'object' && v !== null && 'value' in v) {
        return (v).value
      }
      return v as string | number
    })
  }
  
  // 如果是对象（LabeledValue）
  if (typeof value === 'object' && 'value' in value) {
    return (value).value
  }
  
  // 原始值
  return value as string | number
}

// 处理 Radio 值变化
const handleRadioChange = (event: unknown) => {
  const eventValue = typeof event === 'object' && event !== null && 'target' in event
    ? (event as { target?: { value?: string | number } }).target?.value
    : undefined
  const value = eventValue ?? (event as string | number | null | undefined)
  if (value == null) return
  
  emitBoundValue(value)
  const option = radioOptions.value.find((opt: { label: string; value: string | number }) => opt.value === value)
  emit('change', value, option ?? null)
}

// 处理 Select 值变化
const handleChange = (value: SelectValue, option: DefaultOptionType | DefaultOptionType[]) => {
  const rawValue = extractRawValue(value)
  emitBoundValue(rawValue)
  const normalizedOption = option as SelectOptionLike | SelectOptionLike[] | null
  emit('change', rawValue, normalizedOption)
}

// 处理搜索
const handleSearch = (value: string) => {
  emit('search', value)
  if (!effectiveRemoteSearch.value || !props.apiUrl) {
    return
  }
  if (remoteSearchTimer) {
    clearTimeout(remoteSearchTimer)
  }
  remoteSearchTimer = setTimeout(() => {
    remoteSearchKeyword.value = value
    void loadData()
  }, props.searchDebounceMs)
}

watch(() => props.dictType, () => {
  dictDefaultApplied.value = false
})

watch(() => props.cultureCode, () => {
  dictDefaultApplied.value = false
  if (props.dictType) {
    void loadData()
  }
})

// 监听 dictType、API URL、options、apiParams 与 disabled 变化
watch(() => [props.dictType, props.apiUrl, props.options, props.apiParams, props.disabled, props.cultureCode], () => {
  autoLargeRemoteSearch.value = props.remoteSearch === true
  remoteSearchKeyword.value = ''
  if (props.options?.length) {
    if (
      props.remoteSearch !== false
      && (props.options?.length ?? 0) >= TAKT_LARGE_DATA_AUTO_THRESHOLD
    ) {
      // 静态 options 大数据：仅自动虚拟滚动（无 apiUrl 时无法远程）
      autoLargeRemoteSearch.value = false
    }
    return
  }
  if (props.dictType || props.apiUrl) {
    void loadData()
  }
}, { deep: true })

watch(() => props.modelValue, (val) => {
  if (!effectiveRemoteSearch.value || !props.apiUrl || val == null || val === '') {
    return
  }
  const text = String(Array.isArray(val) ? val[0] : val).trim()
  if (!text || remoteSearchKeyword.value === text) {
    return
  }
  remoteSearchKeyword.value = text
  void loadData()
})

onMounted(() => {
  // 使用 nextTick 确保 props 已经完全初始化（特别是在条件渲染的场景下）
  nextTick(() => {
    if (props.remoteSearch === true) {
      autoLargeRemoteSearch.value = true
    }
    if (effectiveRemoteSearch.value && props.apiUrl && props.modelValue != null && props.modelValue !== '') {
      remoteSearchKeyword.value = String(
        Array.isArray(props.modelValue) ? props.modelValue[0] : props.modelValue,
      ).trim()
    }
    if (props.dictType || (props.apiUrl && !props.options?.length)) {
      void loadData()
    }
  })
})
</script>

<style scoped>
/* 组件样式 */
</style>
