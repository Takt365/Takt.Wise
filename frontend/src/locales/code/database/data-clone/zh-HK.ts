// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/code/database/data-clone
// 文件名称：zh-HK.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：code/database/data-clone 页面静态文案；引用键 code.database.data-clone.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: "公司級數據克隆",
    subtitle: "一次一個源公司、一張表 → 一個目標公司、一張表；克隆前先備份再清空目標公司數據",
    section: {
      source: "源範圍",
      target: "目標範圍",
      options: "克隆選項",
      result: "執行結果",
    },
    field: {
      tenant: "租戶",
      database: "數據庫",
      table: "數據表",
      company: "公司編碼",
      preserveidentity: "保留自增列原值（IDENTITY_INSERT）",
    },
    action: {
      startclone: "開始克隆",
      confirmexecute: "確認並執行克隆",
    },
    backupmodaltitle: "備份窗口 — 請先確認再執行",
    backupsummary: "備份與清空說明",
    samescopeerror: "源與目標的租戶、數據庫、數據表、公司編碼不能完全相同",
    previewrequired: "請先打開備份窗口並完成確認",
    clonesuccess: "克隆成功",
    clonefailed: "克隆失敗",
    preview: {
      confirmhint: "我已閱讀備份窗口提示，確認目標公司將先備份再清空",
      warning: "警告：克隆前將先備份再清空目標表 {tableName} 中公司 {companyCode} 的數據，此操作不可撤銷，請確認後繼續。",
      backupwithrows: "步驟 1：將目標表 {tableName} 中公司 {companyCode} 的全部 {rowCount} 行備份到 {backupTable}",
      backupempty: "步驟 1：目標表 {tableName} 中公司 {companyCode} 當前無數據，將創建空結構備份表 {backupTable}",
      cleardelete: "步驟 2：刪除目標表 {tableName} 中公司 {companyCode} 的全部數據（共 {rowCount} 行）",
      resultbackupwithrows: "已備份目標表 {tableName} 中公司 {companyCode} 的 {rowCount} 行到 {backupTable}",
      resultbackupempty: "目標表 {tableName} 中公司 {companyCode} 無數據，已創建空結構備份表 {backupTable}",
      resultdeleted: "已刪除目標表 {tableName} 中公司 {companyCode} 全部 {rowCount} 行",
      resultsummary: "{backupPart}；{clearPart}",
    },
    result: {
      backuptable: "備份表",
      backeduprows: "備份行數",
      clearedrows: "清空行數",
      sourcerows: "源公司行數",
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
