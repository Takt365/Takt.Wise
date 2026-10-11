// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/code/database/data-clone
// 文件名称：zh-CN.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：code/database/data-clone 页面静态文案；引用键 code.database.data-clone.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: "公司级数据克隆",
    subtitle: "一次一个源公司、一张表 → 一个目标公司、一张表；克隆前先备份再清空目标公司数据",
    section: {
      source: "源范围",
      target: "目标范围",
      options: "克隆选项",
      result: "执行结果",
    },
    field: {
      tenant: "租户",
      database: "数据库",
      table: "数据表",
      company: "公司编码",
      preserveidentity: "保留自增列原值（IDENTITY_INSERT）",
    },
    action: {
      startclone: "开始克隆",
      confirmexecute: "确认并执行克隆",
    },
    backupmodaltitle: "备份窗口 — 请先确认再执行",
    backupsummary: "备份与清空说明",
    samescopeerror: "源与目标的租户、数据库、数据表、公司编码不能完全相同",
    previewrequired: "请先打开备份窗口并完成确认",
    clonesuccess: "克隆成功",
    clonefailed: "克隆失败",
    preview: {
      confirmhint: "我已阅读备份窗口提示，确认目标公司将先备份再清空",
      warning: "警告：克隆前将先备份再清空目标表 {tableName} 中公司 {companyCode} 的数据，此操作不可撤销，请确认后继续。",
      backupwithrows: "步骤 1：将目标表 {tableName} 中公司 {companyCode} 的全部 {rowCount} 行备份到 {backupTable}",
      backupempty: "步骤 1：目标表 {tableName} 中公司 {companyCode} 当前无数据，将创建空结构备份表 {backupTable}",
      cleardelete: "步骤 2：删除目标表 {tableName} 中公司 {companyCode} 的全部数据（共 {rowCount} 行）",
      resultbackupwithrows: "已备份目标表 {tableName} 中公司 {companyCode} 的 {rowCount} 行到 {backupTable}",
      resultbackupempty: "目标表 {tableName} 中公司 {companyCode} 无数据，已创建空结构备份表 {backupTable}",
      resultdeleted: "已删除目标表 {tableName} 中公司 {companyCode} 全部 {rowCount} 行",
      resultsummary: "{backupPart}；{clearPart}",
    },
    result: {
      backuptable: "备份表",
      backeduprows: "备份行数",
      clearedrows: "清空行数",
      sourcerows: "源公司行数",
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
