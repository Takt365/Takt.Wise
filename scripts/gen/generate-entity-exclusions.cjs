// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：scripts
// 文件名称：generate-entity-exclusions.cjs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：代码生成架构约束（RBAC 八表、独立服务、）；单实体模式（--Entity）下不再维护「防 --all 覆盖」手工 CRUD 排除表
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const path = require('path');
const { entityShortFromControllerClassName } = require('./generate-script-common.cjs');

/**
 * RBAC 八张关联表：仅 TaktRbacService / TaktRbacsController，禁止脚本生成独立 CRUD
 */
const RBAC_ASSOCIATION_ENTITY_SHORT_NAMES = new Set([
  'UserRole',
  'UserTenant',
  'UserCompany',
  'RoleMenu',
  'RoleCompany',
  'RoleDept',
  'EmployeeDept',
  'EmployeePost',
]);

/**
 * 无实体、手工维护的独立应用服务（跳过服务/控制器脚本扫描）
 */
const MANUAL_STANDALONE_SERVICE_ENTITY_NAMES = new Set([
  'TaktAuth',
  'TaktRbac',
  'TaktFlowEngine',
  'TaktFileUploadEngine',
  'TaktProductionOrderFormFill',
  'TaktCustomerServiceStat',
]);

/**
 * 手工维护 DTO 的实体（含脚本无法生成的附加 DTO 类，禁止 generate-dtos-from-entity 覆盖）
 * TaktHoliday：TaktHolidayThemeDto（登录前假日主题预览，与实体字段并列但非实体映射）
 * TableArchive / DatabaseBackup：数据归档与备份编排（CRUD 外另有 preview/execute/run/schedule，禁止流水线覆盖）
 * QuartzTask：CRUD 外挂接 ITaktQuartzSchedulerManager（Schedule/Remove/Start/Pause/RunNow），禁止流水线覆盖
 */
const MANUAL_DTO_ENTITY_SHORT_NAMES = new Set([
  'Holiday',
  'TableArchive',
  'DatabaseBackup',
  'QuartzTask',
]);

/**
 * OneToMany 从实体、但业务上为「独立菜单 CRUD」的明细（非主表内嵌级联）
 * - Vue：不因 masterDetailChildRegistry 跳过；主表 master-detail 规划会 filterStandaloneMenuChildren
 * - DTO/服务：主表 Create/Update/Fill/Save **不**再级联该子表（子表自行 generate-all）
 * - 权限：buildPermissionBase 不继承主表前缀（各子表独立 Permission）
 * 例：QuartzLog；ManufacturerMaterial（Vendor+Supplier）；SellerMaterial（Customer+Client）；
 *     NewsCenter 六张从表（NewsComment / NewsCommentLike / NewsFavorite / NewsLike / NewsRead / NewsShare；无附件实体）
 *     — DTO 不级联；Vue 子导航主子见 generate-vue-common（masterPascal==='News'，同 Employee）
 * ⚠️ HumanResource/Personnel 的 EmployeeAddress 等为真正主子表，由 TaktEmployee 级联；
 *    Vue 特例见 generate-vue-common（masterPascal==='Employee'），不得列入本 STANDALONE 集合
 */
const STANDALONE_CHILD_VUE_ENTITY_SHORT_NAMES = new Set([
  'QuartzLog',
  'CustomerServiceRequest',
  'CustomerServiceOrder',
  'CustomerServiceTicket',
  'ManufacturerMaterial',
  'SellerMaterial',
  'NewsComment',
  'NewsCommentLike',
  'NewsFavorite',
  'NewsLike',
  'NewsRead',
  'NewsShare',
]);

/**
 * 是否为看板/分析统计短名（*Stat / *Trend / *Explosion）
 * 须独立 DTO+Service+Controller，禁止挂在 CRUD 上，禁止流水线按标准聚合 DTO 覆盖
 * @param {string} entityShort
 * @returns {boolean}
 */
function isAnalysisStatEntity(entityShort) {
  return /(?:Stat|Trend|Explosion)$/.test(entityShort || '');
}

/**
 * @param {string} entityShort 实体短名（全字）
 * @returns {boolean}
 */
function isRbacJunctionEntity(entityShort) {
  return RBAC_ASSOCIATION_ENTITY_SHORT_NAMES.has(entityShort);
}

/**
 * 是否为手工维护 DTO 的实体（禁止 DTO/前端 types/i18n 流水脚本覆盖）
 * @param {string} entityShort
 * @returns {boolean}
 */
function isManualDtoEntity(entityShort) {
  return MANUAL_DTO_ENTITY_SHORT_NAMES.has(entityShort);
}

/**
 * 单实体 CLI：禁止对手工维护实体运行任何代码生成流水脚本
 * @param {string} entityShort
 */
function assertNotManualDtoEntityCli(entityShort) {
  if (!isManualDtoEntity(entityShort)) {
    return;
  }
  console.error(
    `❌ 实体 ${entityShort} 为手工维护模块（含附加 DTO/业务逻辑），禁止代码生成流水脚本（generate-all / generate-dtos / generate-from-backend / generate-services / generate-controllers / generate-vue-* 等）。`,
  );
  console.error('   例：TaktHoliday 含 TaktHolidayThemeDto 与主题预览 API，须手工同步实体、DTO、服务、i18n 种子与前端 types/api/views。');
  process.exit(1);
}

/**
 * 单实体 CLI：禁止对 RBAC 关联表生成独立 DTO/服务/控制器
 * @param {string} entityShort
 */
function assertNotRbacJunctionEntityCli(entityShort) {
  if (!isRbacJunctionEntity(entityShort)) {
    return;
  }
  console.error(
    `❌ 实体 ${entityShort} 为 RBAC 关联表，由 ITaktRbacService 统一维护，禁止本脚本生成独立 CRUD。`,
  );
  console.error(`   关联表: ${[...RBAC_ASSOCIATION_ENTITY_SHORT_NAMES].join('、')}`);
  process.exit(1);
}

/**
 * DTO 文件名 → 实体短名
 * @param {string} dtoFileName 如 TaktUserRoleDtos.cs
 * @returns {string|null}
 */
function entityShortFromDtoFileName(dtoFileName) {
  const base = path.basename(dtoFileName, '.cs');
  if (!base.endsWith('Dtos') || !base.startsWith('Takt')) {
    return null;
  }
  return base.slice(4, -'Dtos'.length);
}

/**
 * 含大量手工商业务方法的应用服务实体（禁止 generate-services / generate-controllers 覆盖）
 * 扫描阶段仍须能定位 ITaktXxxService；跳过逻辑在 process 阶段（与 generate-services-from-dtos 一致）
 * 附加 API 放同目录 *Extra*.cs 或本服务手工商文件；流水线仅生成标准 CRUD 时请勿列入本表
 * BillOfMaterial：BOM 展开见独立 Explosion；BomMaterialCostAnalysis：月推移见独立 AnalysisTrend；
 * DictData：CreateDictSnapshotAsync / GetDataDictAllAsync；
 * Configurable：运行时查询；EcGijutsu / AssyOutput：来源导入扩展；FlowInstance：实例统计
 * 约定：凡 *Trend / *Explosion / *Stat 分析模块须独立 DTO+Service+Controller+前端 API，禁止挂在 CRUD 服务上。
 * 短名以 Stat/Trend/Explosion 结尾时由 isAnalysisStatEntity 统一跳过流水线覆盖。
 */
const MANUAL_SERVICE_ENTITY_SHORT_NAMES = new Set([
  'BillOfMaterial',
  'BomMaterialCostAnalysis',
  'GenTable',
  'User',
  'DictData',
  'Configurable',
  'EcGijutsu',
  'AssyOutput',
  'FlowInstance',
]);

/**
 * @param {string} entityName
 * @returns {boolean}
 */
function shouldExcludeStandaloneService(entityName) {
  if (MANUAL_STANDALONE_SERVICE_ENTITY_NAMES.has(entityName)) {
    return true;
  }
  const short = entityName?.startsWith('Takt') ? entityName.slice(4) : entityName;
  if (isAnalysisStatEntity(short)) {
    return true;
  }
  return MANUAL_SERVICE_ENTITY_SHORT_NAMES.has(short);
}

/**
 * 服务脚本：是否跳过 *Dtos.cs（RBAC 八表）
 * @param {string} dtoFile
 * @returns {boolean}
 */
function shouldExcludeDtoFile(dtoFile) {
  const entityShort = entityShortFromDtoFileName(path.basename(dtoFile));
  if (entityShort != null && isRbacJunctionEntity(entityShort)) {
    return true;
  }
  if (entityShort != null && isAnalysisStatEntity(entityShort)) {
    return true;
  }
  return entityShort != null && isManualDtoEntity(entityShort);
}

/**
 * 前端脚本：是否跳过 *Dtos.cs 源文件（RBAC 八表）
 * @param {string} sourceFileBase 如 TaktUserRoleDtos
 * @returns {boolean}
 */
function shouldExcludeDtoSourceBase(sourceFileBase) {
  const entityShort = entityShortFromDtoFileName(`${sourceFileBase}.cs`);
  if (entityShort != null && isRbacJunctionEntity(entityShort)) {
    return true;
  }
  if (entityShort != null && isAnalysisStatEntity(entityShort)) {
    return true;
  }
  return entityShort != null && isManualDtoEntity(entityShort);
}

/**
 * @param {string} controllerName
 * @returns {string}
 */
function entityShortFromControllerName(controllerName) {
  return entityShortFromControllerClassName(controllerName);
}

/**
 * 前端脚本：是否跳过控制器（RBAC 八表 + 独立服务 + 手工维护模块）
 * @param {string} controllerName
 * @returns {boolean}
 */
function shouldExcludeController(controllerName) {
  const entityShort = entityShortFromControllerName(controllerName);
  if (isRbacJunctionEntity(entityShort)) {
    return true;
  }
  if (isManualDtoEntity(entityShort)) {
    return true;
  }
  if (isAnalysisStatEntity(entityShort)) {
    return true;
  }
  const entityName = entityShort ? `Takt${entityShort}` : '';
  return MANUAL_STANDALONE_SERVICE_ENTITY_NAMES.has(entityName);
}


/**
 * 是否为「从实体但仍有独立 index/form」的 Vue 模块（覆盖 masterDetailChildRegistry 跳过）
 * @param {string} entityShort 实体短名（如 QuartzLog）
 * @returns {boolean}
 */
function isStandaloneChildVueEntity(entityShort) {
  return STANDALONE_CHILD_VUE_ENTITY_SHORT_NAMES.has(entityShort);
}

/**
 * Vue 视图/表单生成脚本：是否跳过该 API 模块
 * @param {string} _apiRelPath 相对 frontend/src/api 的路径（保留参数以兼容调用方）
 * @param {string} entityShort 实体短名（如 Menu、FlowScheme）
 * @returns {boolean}
 */
function shouldExcludeVueGeneration(_apiRelPath, entityShort) {
  return isManualDtoEntity(entityShort);
}

module.exports = {
  RBAC_ASSOCIATION_ENTITY_SHORT_NAMES,
  MANUAL_STANDALONE_SERVICE_ENTITY_NAMES,
  MANUAL_DTO_ENTITY_SHORT_NAMES,
  MANUAL_SERVICE_ENTITY_SHORT_NAMES,
  STANDALONE_CHILD_VUE_ENTITY_SHORT_NAMES,
  isRbacJunctionEntity,
  isManualDtoEntity,
  isAnalysisStatEntity,
  assertNotRbacJunctionEntityCli,
  assertNotManualDtoEntityCli,
  shouldExcludeStandaloneService,
  shouldExcludeDtoFile,
  shouldExcludeDtoSourceBase,
  entityShortFromControllerName,
  shouldExcludeController,
  shouldExcludeVueGeneration,
  isStandaloneChildVueEntity,
};
