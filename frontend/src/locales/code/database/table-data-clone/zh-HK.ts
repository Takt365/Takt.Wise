// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/code/database/table-data-clone
// 文件名称：zh-HK.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：code/database/table-data-clone 页面静态文案；引用键 code.database.table-data-clone.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: "跨租戶整表克隆",
    subtitle: "僅支持跨租戶；一次 1~5 張表；克隆前先備份再 TRUNCATE 清空目標整表",
    section: {
      scope: "租戶與數據庫",
      tables: "表映射（1~5 張）",
      options: "克隆選項",
      result: "執行結果",
    },
    field: {
      sourcetenant: "源租戶",
      targettenant: "目標租戶",
      sourcedatabase: "源數據庫",
      targetdatabase: "目標數據庫",
      sourcetable: "源表",
      targettable: "目標表",
      preserveidentity: "保留自增列原值（IDENTITY_INSERT）",
    },
    tablemapping: {
      addrow: "添加表",
      removerow: "刪除",
      maxhint: "一次最多 5 張表",
      actioncolumn: "操作",
    },
    crosstenantrequired: "整表克隆僅支持跨租戶，源租戶與目標租戶不能相同",
    tablerequired: "請至少配置一張表映射",
    action: {
      startclone: "開始克隆",
      confirmexecute: "確認並執行克隆",
    },
    backupmodaltitle: "備份窗口 — 請先確認再執行",
    clonesuccess: "克隆成功",
    clonefailed: "克隆失敗",
    previewrequired: "請先打開備份窗口並完成確認",
    preview: {
      confirmhint: "我已閱讀備份窗口提示，確認目標表將先全量備份再 TRUNCATE 清空",
      summary: "共 {count} 張目標表將在克隆前先全量備份，再 TRUNCATE 清空全部數據。",
      warning: "警告：克隆前將先備份再清空目標表 {tableName} 的全部數據，此操作不可撤銷，請確認後繼續。",
      backupwithrows: "步驟 1：將目標表 {tableName} 的全部 {rowCount} 行備份到 {backupTable}",
      backupempty: "步驟 1：目標表 {tableName} 當前無數據，將創建空結構備份表 {backupTable}",
      cleartruncate: "步驟 2：TRUNCATE 清空目標表 {tableName} 中的全部數據（共 {rowCount} 行）",
    },
    result: {
      backuptable: "備份表",
      backeduprows: "備份行數",
      clearedrows: "清空行數",
      sourcerows: "源表行數",
      clonedrows: "克隆行數",
      commoncolumns: "同名列數",
      summary: "備份與清空摘要",
      targetrows: "目標現有行數",
      plannedbackuptable: "計劃備份表",
      totalsourcerows: "源表行數合計",
      totalclonedrows: "克隆行數合計",
      tablecount: "克隆表數量",
    },
  },
};
