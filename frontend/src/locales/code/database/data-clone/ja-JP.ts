// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/code/database/data-clone
// 文件名称：ja-JP.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：code/database/data-clone 页面静态文案；引用键 code.database.data-clone.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: "会社単位データクローン",
    subtitle: "1 ソース会社・1 表 → 1 ターゲット会社・1 表。クローン前にターゲット会社データをバックアップしてから削除します",
    section: {
      source: "ソース範囲",
      target: "ターゲット範囲",
      options: "クローンオプション",
      result: "実行結果",
    },
    field: {
      tenant: "テナント",
      database: "データベース",
      table: "テーブル",
      company: "会社コード",
      preserveidentity: "IDENTITY 列の値を保持（IDENTITY_INSERT）",
    },
    action: {
      startclone: "クローン開始",
      confirmexecute: "確認して実行",
    },
    backupmodaltitle: "バックアップウィンドウ — 実行前に確認",
    backupsummary: "バックアップと削除の手順",
    samescopeerror: "ソースとターゲットのテナント・DB・表・会社コードをすべて同一にできません",
    previewrequired: "バックアップウィンドウで確認してから実行してください",
    clonesuccess: "クローン成功",
    clonefailed: "クローン失敗",
    preview: {
      confirmhint: "バックアップウィンドウの注意を確認し、ターゲット会社データをバックアップ後に削除することに同意します",
      warning: "警告：クローン前にターゲット表 {tableName} の会社 {companyCode} のデータをバックアップしてから削除します。取り消せません。",
      backupwithrows: "手順 1：ターゲット表 {tableName} の会社 {companyCode} の全 {rowCount} 行を {backupTable} にバックアップ",
      backupempty: "手順 1：ターゲット表 {tableName} の会社 {companyCode} にデータなし。空構造のバックアップ表 {backupTable} を作成",
      cleardelete: "手順 2：ターゲット表 {tableName} の会社 {companyCode} の全データを削除（{rowCount} 行）",
      resultbackupwithrows: "ターゲット表 {tableName} の会社 {companyCode} の {rowCount} 行を {backupTable} にバックアップ済み",
      resultbackupempty: "ターゲット表 {tableName} の会社 {companyCode} にデータなし。空構造のバックアップ表 {backupTable} を作成済み",
      resultdeleted: "ターゲット表 {tableName} の会社 {companyCode} の全 {rowCount} 行を削除済み",
      resultsummary: "{backupPart}；{clearPart}",
    },
    result: {
      backuptable: "バックアップ表",
      backeduprows: "バックアップ行数",
      clearedrows: "削除行数",
      sourcerows: "ソース会社行数",
      clonedrows: "クローン行数",
      commoncolumns: "共通列数",
      summary: "バックアップ・削除サマリー",
      targetrows: "ターゲット既存行数",
      plannedbackuptable: "予定バックアップ表",
      totalsourcerows: "ソース行数合計",
      totalclonedrows: "クローン行数合計",
      tablecount: "クローン表数",
    },
  },
};
