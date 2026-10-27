// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：scripts
// 文件名称：generate-vue-script-docs.cjs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：Vue CRUD 生成脚本共享 JSDoc（index.vue / *Form.vue script setup 段）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 树表 index.vue：ref/computed 状态块（左右均为树表：超阈值懒加载+virtual，无分页）
 * @param {string} entityPascal
 * @param {object} options
 */
function buildTreeIndexStateRefs(entityPascal, options) {
  const {
    hasForm,
    hasImport,
    idField,
    titleField,
    queryInit,
    queryFactoryBlock,
    searchFieldLabelExprs,
    needsUserStore,
    needsExcelNames,
    entityClassName,
    entityPascal: entityPascalOpt,
  } = options;
  const i18nPrefix = (entityPascalOpt || entityPascal).toUpperCase();
  const formBlock = hasForm ? `
/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<${entityPascal}> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()
` : '';
  const importLine = hasImport ? `/** 导入对话框是否打开 */
const importVisible = ref(false)
` : '';
  return `/** i18n 翻译函数 */
const { t } = useI18n()
${needsUserStore ? `/** 用户上下文（companyDefaultCulture 等） */
const userStore = useUserStore()
` : ''}${needsExcelNames ? `/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('${entityClassName}')
` : ''}/** 右侧树表快捷查询占位文案 */
const tableSearchPlaceholder = computed(() =>
  t('common.page.form.placeholder.search', {
    keyword: [${searchFieldLabelExprs}].join(' / '),
  })
)

/** 左侧树关键字（仅过滤已加载节点，不重复请求 API） */
const treeQueryKeyword = ref('')
/** 右侧树表快捷查询关键字 */
const queryKeyword = ref('')
/** 左侧树工具栏「展开/收缩」状态（仅已加载层） */
const treeExpanded = ref(false)
/** 左侧树当前展开的节点 key 列表 */
const treeExpandedKeys = ref<(string | number)[]>([])
/** 右侧树表工具栏「全部展开/收缩」 */
const tableExpanded = ref(false)
/** 右侧 a-table 树表当前展开行 key */
const tableExpandedRowKeys = ref<(string | number)[]>([])
/** 达到阈值后左右树均按 parentId 一层懒加载 */
const useLazyTree = ref(true)
/** 左侧树 loading */
const loading = ref(false)
/** 右侧树 loading */
const listLoading = ref(false)
/** 左侧 a-tree 数据（懒加载仅已展开路径；低于阈值时为全量树） */
const entityTreeData = ref<TaktLazyTreeNode[]>([])
/** 右侧树表数据源（带 children / _hasChildren，组件内拍平 virtual） */
const tableTreeData = ref<Record<string, unknown>[]>([])
/** 左侧树当前选中的节点 key 列表 */
const selecmedTreeKeys = ref<(string | number)[]>([])
/** 工具栏单选时当前行（编辑/删除） */
const selecmedRow = ref<${entityPascal}RowRecord | null>(null)
/** 表格多选行 */
const selecmedRows = ref<${entityPascal}RowRecord[]>([])
/** 表格多选 row-key 集合 */
const selecmedRowKeys = ref<(string | number)[]>([])
${formBlock}
/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
${queryFactoryBlock ? `${queryFactoryBlock}\n` : ''}/** 高级查询表单模型 */
const advancedQueryForm = ref(${queryFactoryBlock ? 'createEmptyAdvancedQueryForm()' : `{
${queryInit}
}`})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  ${i18nPrefix}_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
)
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
${importLine}/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = '${idField}'
/** 树节点标题字段名（左侧树 title） */
const treeTitleField = '${titleField}'
`;
}

/**
 * 单表/主子表 index.vue：ref/computed 状态块（完整 JSDoc）
 * @param {string} entityPascal
 * @param {object} options
 */
function buildSingleIndexStateRefs(entityPascal, options) {
  const {
    hasForm,
    hasImport,
    hasUpdate,
    hasDelete,
    queryInit,
    queryFactoryBlock,
    entityPascal: entityPascalOpt,
    entityIdName,
    excelConst,
    defaultVisibleColumnKeysLiteral,
  } = options;
  const i18nPrefix = (entityPascalOpt || entityPascal).toUpperCase();
  const formBlock = hasForm ? `
/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<${entityPascal}> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()
` : '';
  const importLine = hasImport ? `/** 导入对话框是否打开 */
const importVisible = ref(false)
` : '';
  const updateDisabled = hasUpdate ? `/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selecmedRows.value.length !== 1)
` : '';
  const deleteDisabled = hasDelete ? `/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selecmedRows.value.length === 0)
` : '';
  const visibleColumnKeysBlock = defaultVisibleColumnKeysLiteral
    ? `/** 默认可见列（id + 前 8 个业务列 + 操作列，与 CRUD 单表一致） */
const DEFAULT_VISIBLE_COLUMN_KEYS: string[] = ${defaultVisibleColumnKeysLiteral}

/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([...DEFAULT_VISIBLE_COLUMN_KEYS])
`
    : `/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
`;
  return `/** i18n 翻译函数 */
const { t } = useI18n()
${excelConst}/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<${entityPascal}[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selecmedRow = ref<${entityPascal}RowRecord | null>(null)
/** 表格多选行 */
const selecmedRows = ref<${entityPascal}RowRecord[]>([])
/** 表格多选 row-key 集合 */
const selecmedRowKeys = ref<(string | number)[]>([])
${formBlock}
/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
${queryFactoryBlock ? `${queryFactoryBlock}\n` : ''}/** 高级查询表单模型 */
const advancedQueryForm = ref(${queryFactoryBlock ? 'createEmptyAdvancedQueryForm()' : `{
${queryInit}
}`})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  ${i18nPrefix}_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
)
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
${importLine}${visibleColumnKeysBlock}/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = '${entityIdName}'
${updateDisabled}${deleteDisabled}`;
}

/**
 * *Form.vue script 段：表单状态与 props（完整 JSDoc）
 * @param {object} options
 */
function buildFormScriptStateBlock(options) {
  const {
    formContentClassExpr,
    formFieldsJson,
    mdScript,
    scopeStoreScript,
    useFormTabs = true,
    omitFormFieldsArray = false,
  } = options;
  const formContentScript = useFormTabs
    ? `/** 表单内容区高度 class${omitFormFieldsArray ? '（多 Tab 大表单固定 10 行高度）' : '（字段多时 tab-10 行）'} */
const formContentClass = ${formContentClassExpr}
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
`
    : '';
  const formFieldsBlock = omitFormFieldsArray
    ? ''
    : `/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ${formFieldsJson}

`;
  return `/** i18n 翻译函数 */
const { t } = useI18n()
${scopeStoreScript || ''}${formContentScript}${formFieldsBlock}${mdScript}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<${options.entityPascal}Create & { ${options.entityIdField}?: string }> | null
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
const formState = reactive<Record<string, any>>({})`;
}

module.exports = {
  buildTreeIndexStateRefs,
  buildSingleIndexStateRefs,
  buildFormScriptStateBlock,
};
