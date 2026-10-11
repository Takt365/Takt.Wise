// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/code/database/table-data-clone
// 文件名称：zh-CN.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：code/database/table-data-clone 页面静态文案；引用键 code.database.table-data-clone.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: "跨租户整表克隆",
    subtitle: "仅支持跨租户；一次 1~5 张表；克隆前先备份再 TRUNCATE 清空目标整表",
    section: {
      scope: "租户与数据库",
      tables: "表映射（1~5 张）",
      options: "克隆选项",
      result: "执行结果",
    },
    field: {
      sourcetenant: "源租户",
      targettenant: "目标租户",
      sourcedatabase: "源数据库",
      targetdatabase: "目标数据库",
      sourcetable: "源表",
      targettable: "目标表",
      preserveidentity: "保留自增列原值（IDENTITY_INSERT）",
    },
    tablemapping: {
      addrow: "添加表",
      removerow: "删除",
      maxhint: "一次最多 5 张表",
      actioncolumn: "操作",
    },
    crosstenantrequired: "整表克隆仅支持跨租户，源租户与目标租户不能相同",
    tablerequired: "请至少配置一张表映射",
    action: {
      startclone: "开始克隆",
      confirmexecute: "确认并执行克隆",
    },
    backupmodaltitle: "备份窗口 — 请先确认再执行",
    clonesuccess: "克隆成功",
    clonefailed: "克隆失败",
    previewrequired: "请先打开备份窗口并完成确认",
    preview: {
      confirmhint: "我已阅读备份窗口提示，确认目标表将先全量备份再 TRUNCATE 清空",
      summary: "共 {count} 张目标表将在克隆前先全量备份，再 TRUNCATE 清空全部数据。",
      warning: "警告：克隆前将先备份再清空目标表 {tableName} 的全部数据，此操作不可撤销，请确认后继续。",
      backupwithrows: "步骤 1：将目标表 {tableName} 的全部 {rowCount} 行备份到 {backupTable}",
      backupempty: "步骤 1：目标表 {tableName} 当前无数据，将创建空结构备份表 {backupTable}",
      cleartruncate: "步骤 2：TRUNCATE 清空目标表 {tableName} 中的全部数据（共 {rowCount} 行）",
    },
    result: {
      backuptable: "备份表",
      backeduprows: "备份行数",
      clearedrows: "清空行数",
      sourcerows: "源表行数",
      clonedrows: "克隆行数",
      commoncolumns: "同名列数",
      summary: "备份与清空摘要",
      targetrows: "目标现有行数",
      plannedbackuptable: "计划备份表",
      totalsourcerows: "源表行数合计",
      totalclonedrows: "克隆行数合计",
      tablecount: "克隆表数量",
    },
  },
};
