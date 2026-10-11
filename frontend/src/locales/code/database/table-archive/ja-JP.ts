// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/code/database/table-archive
// 文件名称：ja-JP.ts
// 作成時間：2026-07-19
// 作成人：Takt365(Cursor AI)
// 機能記述：code/database/table-archive 静的文言；キー code.database.table-archive.page.*（段内全小文字）
//
// 著作権：Copyright (c) 2026 Takt  All rights reserved.
// 免責：MIT License。
// ========================================

export default {
  page: {
    title: 'テーブルアーカイブ',
    subtitle:
      '物理テーブルとアーカイブキーを登録し、{表}_{年} 年表で基表負荷を下げ、必要に応じて年単位で履歴データを年表へ移行します',
    archive: {
      title: '年次アーカイブ',
      year: 'アーカイブ年',
      yearrequired: 'アーカイブ年を選択してください',
      selectpolicies: '設定を選択',
      preview: '行数プレビュー',
      execute: 'アーカイブ実行',
      runnow: '即時実行',
      schedule: 'バックグラウンド実行',
      scheduledat: '実行時刻',
      previewtotal: '移行予定合計 {count} 行',
      success: 'アーカイブ完了',
      failed: 'アーカイブ失敗',
      runsuccess: '即時アーカイブタスクを作成しました',
      schedulesuccess: 'バックグラウンドアーカイブタスクを作成しました',
      emptyselection: '有効な設定を1件以上選択してください',
      schedulerequired: '実行時刻を選択してください',
      schedulefuture: '実行時刻は現在より後である必要があります',
      kinddatetime: 'yyyyMMddHHmmss（例 …_20251010101000）',
      kindyearmonth: 'yyyyMM（例 …_202510）',
      kindyear: 'yyyy（例 …_2025）',
    },
    tip: {
      archivekeycolumn:
        'アーカイブキー列：行がどの年に属するか判定する物理列（例：costing_date）。プレビュー/アーカイブはこの列で年を絞り、キー種別で命名したアーカイブ表へ移行します。',
      archivekeykind:
        'アーカイブキータイプ（辞書 sys_archive_key_kind）：標準日付形式。1=yyyyMMddHHmmss → …_20251010101000；2=yyyyMM → …_202510；3=yyyy（既定）→ …_2025。アーカイブ名は {表}_{形式コード}。列選択後に型から自動提案し、手動変更も可能。',
      retainhotyears:
        'ホット保持年数：固定 1（変更不可）。当年データは基表に残し、当年−1 以前のみアーカイブ可（例：2026年なら ≤2025）。',
    },
    ensureyears: {
      title: '年表を作成',
      years: '年の範囲',
      yearstart: '開始年',
      yearend: '終了年',
      yearshint: '{表}_2026 形式の年表を作成（既存なら構造クローンをスキップ）。両端含む。1回最大30年',
      execute: '年表作成',
      success: '年表の準備完了',
      failed: '年表の作成に失敗',
      emptyyears: '年の範囲を選択してください',
      spantoolarge: '1回あたり最大30年分です。範囲を狭めてください',
      result: '準備完了：{tables}',
    },
  },
};
