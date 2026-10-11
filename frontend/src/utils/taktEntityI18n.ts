// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/utils/taktEntityI18n
// 文件名称：taktEntityI18n.ts
// 功能描述：实体字段 → entity.*/common.page.entity.* 翻译键解析（与 generate-script-common.cjs / 后端种子对齐）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** 通用实体字段 → common.page.entity.*（与 TaktCommonI18nSeedData / ENTITY_BASE_FIELDS 一致；PlantCode 等不进 entity.{slug}.*） */
export const COMMON_ENTITY_FIELD_I18N_KEYS: Readonly<Record<string, string>> = {
  remark: 'common.page.entity.remark',
  extField: 'common.page.entity.extfield',
  tenantCode: 'common.page.entity.tenantcode',
  companyCode: 'common.page.entity.companycode',
  cultureCode: 'common.page.entity.culturecode',
  plantCode: 'common.page.entity.plantcode',
  relatedPlant: 'common.page.entity.relatedplant',
  companyDefaultCulture: 'common.page.entity.companydefaultculture',
  createdAtStart: 'common.page.entity.createdatstart',
  createdAtEnd: 'common.page.entity.createdatend',
  approvalStatus: 'common.page.entity.approvalstatus',
  initiatorId: 'common.page.entity.initiatorid',
  initiatedAt: 'common.page.entity.initiatedat',
  approvalOpinion: 'common.page.entity.approvalopinion',
  approvedBy: 'common.page.entity.approvedby',
  approvedAt: 'common.page.entity.approvedat',
  flowInstanceId: 'common.page.entity.flowinstanceid',
}

/** 全局属性 camelCase → I18nKey 末段覆盖 */
const ENTITY_FIELD_I18N_SEGMENT: Readonly<Record<string, string>> = {
  passwordHash: 'password',
  employeeId: 'employeeid',
  dictCode: 'code',
  typeCode: 'code',
  themeCode: 'code',
  // *GroupCategory 去 slug 后会落成 category；种子统一用 groupcategory
  defectGroupCategory: 'groupcategory',
  ecGroupCategory: 'groupcategory',
  qualityGroupCategory: 'groupcategory',
  purchaseGroupCategory: 'groupcategory',
  salesGroupCategory: 'groupcategory',
}

/** 按实体 slug 覆盖末段 */
const ENTITY_PROPERTY_I18N_SEGMENT_BY_SLUG: Readonly<Record<string, Readonly<Record<string, string>>>> = {
  menu: {
    i18nKey: 'l10nkey',
    componentPath: 'component',
    externalUrl: 'linkurl',
  },
}

/**
 * 业务自声明 CultureCode 的实体 slug（组合 4；≠ 基类隔离 CultureCode）
 * - culture：去重后 entity.culture.code（cultureCode strip → code）
 * - translation：无相邻重复 → entity.translation.culturecode
 * 均不走 common.page.entity.culturecode
 */
const ENTITY_OWN_CULTURE_CODE_SLUGS = new Set(['culture', 'translation'])

/**
 * 去掉属性名中与实体 slug 重复的前缀（plantCode + plant → code）
 * @param camelName 属性 camelCase
 * @param entitySlug 实体 slug（全小写）
 */
function stripEntitySlugPrefixFromCamel(camelName: string, entitySlug: string): string {
  if (!entitySlug) {
    return camelName
  }
  const prefix = entitySlug.toLowerCase()
  const lower = camelName.toLowerCase()
  if (!lower.startsWith(prefix) || camelName.length <= prefix.length) {
    return camelName
  }
  const rest = camelName.slice(prefix.length)
  return rest.charAt(0).toLowerCase() + rest.slice(1)
}

/**
 * 将属性 camelCase 解析为 I18nKey 末段（全小写 a-z0-9）
 * @param camelName _self 或属性 camelCase
 * @param entitySlug 实体 slug（全小写）
 */
export function resolveEntityFieldI18nSegment(camelName: string, entitySlug?: string): string {
  if (camelName === '_self') {
    return '_self'
  }
  const slug = entitySlug?.toLowerCase() ?? ''
  const slugOverrides = slug ? ENTITY_PROPERTY_I18N_SEGMENT_BY_SLUG[slug] : undefined
  let segment =
    slugOverrides?.[camelName] ??
    ENTITY_FIELD_I18N_SEGMENT[camelName] ??
    stripEntitySlugPrefixFromCamel(camelName, slug)
  segment = String(segment).toLowerCase()
  if (!/^[a-z0-9]+$/.test(segment)) {
    throw new Error(`I18n 键末段非法（须全小写 a-z0-9）：${camelName} → ${segment}`)
  }
  return segment
}

/**
 * 生成 entity.{slug}.{segment} 完整翻译键
 * @param entitySlug 实体 slug（全小写，如 plant）
 * @param fieldName 属性 camelCase 或 _self
 */
export function buildEntityI18nKey(entitySlug: string, fieldName: string): string {
  const normalizedSlug = entitySlug.toLowerCase()
  if (!/^[a-z0-9]+$/.test(normalizedSlug)) {
    throw new Error(`I18n 实体 slug 非法（须全小写 a-z0-9）：${entitySlug}`)
  }
  return `entity.${normalizedSlug}.${resolveEntityFieldI18nSegment(fieldName, normalizedSlug)}`
}

/**
 * 解析字段完整 i18n 键（remark/extField 等走 common.page.entity.*，其余走 entity.{slug}.*）
 * @param entitySlug 实体 slug（全小写）
 * @param fieldName 属性 camelCase
 */
export function resolveEntityFieldI18nKey(entitySlug: string, fieldName: string): string {
  const slug = entitySlug.toLowerCase()
  // Culture / Translation：业务 CultureCode，走 entity.*（culture→code 去重；translation→culturecode）
  if (fieldName === 'cultureCode' && ENTITY_OWN_CULTURE_CODE_SLUGS.has(slug)) {
    return buildEntityI18nKey(slug, fieldName)
  }
  const common = COMMON_ENTITY_FIELD_I18N_KEYS[fieldName]
  if (common) {
    return common
  }
  return buildEntityI18nKey(entitySlug, fieldName)
}

/**
 * entity.{slug}._self 键
 * @param entitySlug 实体 slug（全小写）
 */
export function buildEntitySelfI18nKey(entitySlug: string): string {
  return buildEntityI18nKey(entitySlug, '_self')
}

/**
 * 高级查询区间字段展示标签（*DateStart/*DateEnd、*TimeStart/*TimeEnd）
 * @param entitySlug 实体 slug（全小写）
 * @param fieldName 查询字段 camelCase
 * @param t vue-i18n 翻译函数
 */
export function resolveQueryRangeFieldLabel(
  entitySlug: string,
  fieldName: string,
  t: (key: string) => string,
): string {
  const rangeMatch = fieldName.match(/^(.+?)(Start|End)$/)
  if (rangeMatch) {
    const baseName = rangeMatch[1]
    const baseLower = baseName.toLowerCase()
    // time/date/*At（startedAt/logoutAt/finishedAt 等）均按「创建时间（起/止）」模板替换基字段名
    if (
      baseLower.includes('time') ||
      baseLower.includes('date') ||
      baseLower.endsWith('at')
    ) {
      const commonKey =
        rangeMatch[2] === 'Start' ? 'common.page.entity.createdatstart' : 'common.page.entity.createdatend'
      const baseLabel = t(resolveEntityFieldI18nKey(entitySlug, baseName))
      return t(commonKey).replace(t('common.page.entity.createdat'), baseLabel)
    }
  }
  return t(resolveEntityFieldI18nKey(entitySlug, fieldName))
}
