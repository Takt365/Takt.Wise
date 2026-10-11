// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：naming.ts
// 创建时间：2026-05-22
// 创建人：Takt365(Cursor AI)
// 功能描述：与后端 TaktNamingHelper 对齐的 Excel 导入导出命名（sheet / fileName）、部门显示名 org.dept.* 键、推移导出 {工厂}_{报表名}
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 与后端 TaktNamingHelper.DefaultSheetNameEnglish 一致
 * @param {string} entityTypeName 领域实体类名，如 TaktUser
 * @returns {string} 去掉 Takt 前缀后的工作表英文名；空输入返回 ''
 */
export function taktDefaultExcelSheetName(entityTypeName: string): string {
  /** 去首尾空白后的实体类名 */
  const n = (entityTypeName || '').trim();
  // 空输入直接返回
  if (!n) {
    return n;
  }
  // 标准 Takt 前缀实体：剥离 Takt 保留英文资源名
  if (n.startsWith('Takt') && n.length > 4) {
    return n.slice(4);
  }
  // 非 Takt 前缀则原样作为 sheet 名
  return n;
}

/** Excel 导出 sheet 名与文件基名 */
export type TaktExcelEntityNames = { sheet: string; fileBase: string };

/**
 * 与后端 ResolveExcelImportExport 在「仅传实体类名」时的默认一致
 * @param {string} entityTypeName 领域实体类名
 * @param {string | null} [sheetEnglishOverride] 非标准列表导出等工作表名（如 FlowTodo）
 * @returns {TaktExcelEntityNames} sheet 与 fileBase（不含扩展名）
 */
export function taktExcelEntityNames(
  entityTypeName: string,
  sheetEnglishOverride?: string | null
): TaktExcelEntityNames {
  /** 导出文件名基名（保留完整实体类名） */
  const fileBase = (entityTypeName || '').trim();
  /** 工作表名：优先覆盖值，否则按 DefaultSheetNameEnglish 规则 */
  const sheet = (sheetEnglishOverride?.trim() || taktDefaultExcelSheetName(fileBase));
  return { sheet, fileBase };
}

/**
 * 部门显示名翻译键，与 TaktNamingHelper.OrgDeptResourceKey / TaktDeptI18nSeedData 一致
 * @param {string | null | undefined} deptCode TaktDept.DeptCode，如 D0420
 * @returns {string} org.dept.{编码小写}；空输入返回 ''
 */
export function taktOrgDeptI18nKey(deptCode: string | null | undefined): string {
  const code = (deptCode || '').trim().toLowerCase();
  if (!code) {
    return '';
  }
  if (!/^[a-z0-9]+$/.test(code)) {
    throw new Error(`部门编码只能包含字母和数字：${deptCode}`);
  }
  return `org.dept.${code}`;
}

/**
 * 与后端 TaktNamingHelper.PlantPrefixedTrendExportBase 一致：{工厂码}_{报表名}
 * @param {string} plantCode 选中工厂码，如 C100
 * @param {string} reportLabel 报表短名（已本地化），如 月销售推移 / Monthly Sales Trend
 * @returns {string} 导出基名（不含扩展名与时间戳）
 */
export function taktPlantPrefixedTrendExportBase(plantCode: string, reportLabel: string): string {
  const plant = (plantCode || '').trim();
  let label = (reportLabel || '').trim();
  if (!plant) {
    throw new Error('plantCode 不能为空');
  }
  if (label.toLowerCase().endsWith('.xlsx')) {
    label = label.slice(0, -5).trimEnd();
  }
  label = label.replace(/[/\\?%*:|"<>]/g, '_').replace(/\s+/g, ' ').trim();
  if (!label) {
    throw new Error('reportLabel 不能为空');
  }
  return `${plant}_${label}`;
}

/**
 * 推移导出：本地化报表名 → sheetName + 工厂前缀 exportName / fallbackBase
 * @param {string} plantCode 工厂码
 * @param {string} reportTitle 本地化报表名（如 t('….page.title')）
 * @returns {{ sheetName: string; exportName: string; fallbackBase: string }}
 */
export function taktTrendExportFileNames(
  plantCode: string,
  reportTitle: string,
): { sheetName: string; exportName: string; fallbackBase: string } {
  const sheetName = (reportTitle || '').trim().replace(/[/\\?%*:|"<>]/g, '_').replace(/\s+/g, ' ').trim();
  const exportName = taktPlantPrefixedTrendExportBase(plantCode, sheetName);
  return { sheetName, exportName, fallbackBase: exportName };
}
