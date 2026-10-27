// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/engineering-change/ec-te
// 文件名称：ja-JP.ts
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：設変主画面静的文案；参照キー logistics.manufacturing.engineering.change.ec.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    sourceEcInput: {
      title: 'ソース設変取込',
      openButton: 'ソース取込',
      searchPlaceholder: '設変番号 / タイトル / 機種',
      detailCount: '明細行数',
      selectRequired: 'ソース設変を1件選択してください',
      loadToForm: 'フォームへ読込',
      plantFromCompany: '会社 {company} → 工場 {plant}',
      companyRequired: '会社を選択してからソース設変を取込んでください',
      formTitle: 'ソース設変取込（設変担当・実施範囲・添付を入力して保存）',
      attachmentRequired: '添付を1件以上追加し、ファイルをアップロードしてください',
      attachmentUploadRequired: '{row} 行目：添付ファイルが未アップロードです',
      importSelecmed: '選択取込（{count}）',
      importSuccess: '{count} 件の設変を取込しました',
      importPartial: '取込完了：成功 {success} 件、失敗 {fail} 件',
      detailsDeferred:
        '明細は {count} 行あり、サーバー側で登録します（ブラウザからは送信しません）。設変担当・実施範囲を入力して送信してください。',
    },
    attachment: {
      docCode: {
        formatInvalid: '文書コード形式が正しくありません（{hint}）',
        duplicate: '文書コード「{code}」は既に存在します',
        hint: {
          empty: '文書コードを入力してください',
          ec: '設変番号と一致させる',
          eppFpp: 'P-xxxx（P- + 数字4桁、例 P-0001）',
          tl: 'DTS-xxxx（DTS- + 数字4桁、例 DTS-0001）',
          quadDash: 'xxxx-xxxx（各数字4桁、例 1234-5678）',
        },
      },
      fileName: {
        duplicate: 'ファイル名「{name}」は既に存在します',
      },
      upload: {
        hint: 'PDFのみ、1回1ファイル、最大 {max}MB',
        pdfOnly: 'PDFファイルのみアップロードできます',
      },
    },
    tabs: {
      oldNewMaterial: '旧新部品',
      scope: '実施範囲',
    },
    scopeDepts: {
      master: '制御範囲',
      autoFill: '自動記入',
      pc: '生管',
      pur: '購買',
      iqc: '受検',
      mc: '部管',
      mtl: '物料',
      mfg2: '製二',
      mfg1: '製一',
      qc: '品管',
      me: '製技',
      nonFert: '非FERT',
      discontinued: '生産終了',
    },
    persist: {
      submitted: '設変 {ecCode} のバックグラウンド{action}を受付しました（明細 {detailCount} 行・部門実行派生含む）。完了後に通知します',
      completed: '設変 {ecCode} のバックグラウンド{action}が完了しました（明細 {detailCount} 行、所要 {duration}）',
      failed: '設変 {ecCode} のバックグラウンド{action}に失敗しました（明細 {detailCount} 行）',
      actionCreate: '新規',
      actionUpdate: '更新',
    },
  },
};
