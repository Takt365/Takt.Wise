// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/code/database/table-data-clone
// 文件名称：ja-JP.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：code/database/table-data-clone 页面静态文案；引用键 code.database.table-data-clone.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: "テナント間テーブルクローン",
    subtitle: "テナント間のみ；1 回 1～5 表；クローン前にターゲット表を全量バックアップして TRUNCATE します",
    section: {
      scope: "テナントとデータベース",
      tables: "テーブルマッピング（1～5）",
      options: "クローンオプション",
      result: "実行結果",
    },
    field: {
      sourcetenant: "ソーステナント",
      targettenant: "ターゲットテナント",
      sourcedatabase: "ソース DB",
      targetdatabase: "ターゲット DB",
      sourcetable: "ソース表",
      targettable: "ターゲット表",
      preserveidentity: "IDENTITY 列の値を保持（IDENTITY_INSERT）",
    },
    tablemapping: {
      addrow: "表を追加",
      removerow: "削除",
      maxhint: "1 回最大 5 表",
      actioncolumn: "操作",
    },
    crosstenantrequired: "テーブルクローンはテナント間のみ。ソースとターゲットのテナントは同一にできません",
    tablerequired: "少なくとも 1 つの表マッピングを設定してください",
    action: {
      startclone: "クローン開始",
      confirmexecute: "確認して実行",
    },
    backupmodaltitle: "バックアップウィンドウ — 実行前に確認",
    clonesuccess: "クローン成功",
    clonefailed: "クローン失敗",
    previewrequired: "バックアップウィンドウで確認してから実行してください",
    preview: {
      confirmhint: "バックアップウィンドウの注意を確認し、ターゲット表を全量バックアップ後に TRUNCATE することに同意します",
      summary: "クローン前に {count} 件のターゲット表を全量バックアップし、その後 TRUNCATE で全データを削除します。",
      warning: "警告：クローン前にターゲット表 {tableName} の全データをバックアップしてから削除します。取り消せません。",
      backupwithrows: "手順 1：ターゲット表 {tableName} の全 {rowCount} 行を {backupTable} にバックアップ",
      backupempty: "手順 1：ターゲット表 {tableName} にデータなし。空構造のバックアップ表 {backupTable} を作成",
      cleartruncate: "手順 2：ターゲット表 {tableName} の全データを TRUNCATE（{rowCount} 行）",
    },
    result: {
      backuptable: "バックアップ表",
      backeduprows: "バックアップ行数",
      clearedrows: "削除行数",
      sourcerows: "ソース行数",
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
