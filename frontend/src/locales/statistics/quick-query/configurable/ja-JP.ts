// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/statistics/quick-query/configurable
// 文件名称：ja-JP.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：statistics/quick-query/configurable 页面静态文案；引用键 statistics.quickquery.configurable.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    run: "カスタム帳票実行",
    selectionscreen: "選択条件",
    query: "照会",
    exportdata: "データ出力",
    valueto: "終了値",
    enablenullcheck: "条件を適用",
    resulttitle: "照会結果",
    maxrowshint: "最大 {max} 行",
    rowlimit: "最大ヒット数",
    runpage: {
      backtolist: "一覧に戻る",
      resultempty: "選択条件を入力して照会してください",
      col: {
        label: "ラベル",
        operator: "条件",
        input: "入力値",
      },
    },
    field: {
      tenant: "テナント",
      database: "データベース",
      table: "テーブル",
    },
    operator: {
      eq: "等しい",
      ne: "等しくない",
      gt: "より大きい",
      gte: "以上",
      lt: "より小さい",
      lte: "以下",
      like: "含む",
      between: "範囲",
      in: "リスト内",
      isnull: "NULL",
      isnotnull: "NOT NULL",
    },
    designer: {
      design: "レポート設計",
      createstep: "定制报表 {code}：基本情報",
      tablestep: "データ表と結合",
      quickview: "コード",
      titlelabel: "タイトル",
      section: {
        datasource: "データソース",
        tabledata: "テーブル/DBビューのデータ",
      },
      sourcetype: {
        table: "テーブル",
        join: "テーブル結合",
        clearedjoin: "単一テーブルモードに切り替えました。結合設定をクリアしました",
        locked: "次のステップに進むと「テーブル / テーブル結合」は変更できません",
      },
      tableview: "テーブル/ビュー",
      mode: {
        basic: "基本モード",
        layout: "レイアウトモード",
      },
      confirm: "確定",
      datasource: "データソース（主テーブル）",
      outputfields: "出力フィールド",
      applyprimary: "主テーブルに設定",
      importcolumns: "全列をインポート",
      quickdept: "例：部門テーブル",
      addselection: "フィルタを追加",
      output: "出力",
      technical: "技術名",
      importsuccess: "全列をインポートしました",
      novisiblefield: "出力フィールドを1つ以上選択してください",
      prevstep: "前へ",
      nextstep: "次へ",
      execute: "実行 (F8)",
      steps: {
        basicinfo: "基本情報",
        selecttables: "データ表選択",
        selecttable: "データ表選択",
        selectjoin: "テーブル結合設定",
        joindesign: "テーブル結合設計",
        datalist: "データ列一覧",
        fieldpick: "フィールド選択",
        outputfields: "出力フィールド",
        selection: "選択条件",
        advanced: "詳細設定",
      },
      fieldtree: {
        root: "テーブル/ビュー/構造",
        datafield: "フィールド一覧（説明）",
        outputlist: "一覧フィールド",
        selectionfield: "選択フィールド",
        fieldname: "フィールド名",
        empty: "前のステップでデータ表を選択してください",
        sourcenotready: "データソースを書き込めません。テナントとデータ表を確認してください。",
      },
      jointype: {
        inner: "内部結合",
        left: "左外部結合",
        right: "右外部結合",
        full: "完全外部結合",
      },
      join: {
        primarytable: "主テーブル",
        jointable: "結合テーブル",
        condition: "結合条件",
        apply: "テーブル結合を適用",
        importcolumns: "両テーブルの全列をインポート",
        importsuccess: "両テーブルの全列をインポートしました",
        incomplete: "主テーブルと結合テーブルを選択してください",
        conditionrequired: "結合条件列を選択してください",
        samealias: "主テーブルと結合テーブルの別名は異なる必要があります",
        summaryreadonly: "結合は「テーブル結合設計」ステップで設定済み。ここは読み取り専用プレビューです",
      },
    },
  },
};
