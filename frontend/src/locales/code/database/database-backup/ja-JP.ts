// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/code/database/databaseBackup.types
// 文件名称：ja-JP.ts
// 作成時間：2026-07-19
// 作成人：Takt365(Cursor AI)
// 機能記述：データベースバックアップ静的文言；キー code.database.database-backup.page.*（段内全小文字）
//
// 著作権：Copyright (c) 2026 Takt  All rights reserved.
// 免責：MIT License。
// ========================================

export default {
  page: {
    title: 'データベースバックアップ',
    subtitle:
      'テナントDBの Full/Delta Sync。保存先はローカル（サーバ側）・クライアント・ファイルサーバ・FTP。即時/スケジュールは Quartz ワンショットを作成します。',
    section: {
      form: 'バックアップ設定',
      history: 'バックアップ履歴',
    },
    field: {
      tenant: '対象テナント',
      database: '対象データベース',
      backuptype: 'バックアップ種別',
      backuppath: 'バックアップパス',
      scheduledat: '実行時刻',
      remark: '備考',
      file: 'バックアップファイル',
      status: '状態',
    },
    pathtype: {
      local: 'ローカル（サーバ側）',
      client: 'クライアント',
      network: 'ファイルサーバ',
      ftp: 'FTP サーバ',
    },
    dialog: {
      localtitle: 'ローカルフォルダ選択（サーバ側）',
      clienttitle: 'クライアントフォルダ選択',
      networktitle: 'ファイルサーバ',
      ftptitle: 'FTP サーバ',
      localpathplaceholder: 'API ホストのパス（例 D:\\Backup\\2026）',
      localpickbutton: 'フォルダを選択',
      localnativehint: 'API（browse/local）のフォルダ選択。新規作成可。固定ホワイトリストなし。',
      localnoabsolutepath: '絶対パスを選択または入力してください',
      clientnativehint: 'ドライブをダブルクリックし、対象フォルダを選択すると完全絶対パスが自動入力されます（例 D:\\SQLRecovery）。',
      clientemptyhint: 'ドライブをダブルクリックして参照を開始',
      clientpathplaceholder: '本機の完全絶対パス（例 D:\\Backup\\2026）',
      clientpathrequired: 'クライアントフォルダを選択してください',
      clientneeddrivefirst: '先にドライブを承認してください',
      clientgrantdrivehint: 'ダイアログで {drive} 配下のフォルダを選択してください。完全パスは自動で入ります',
      clientgrantfailed: 'ローカルフォルダの承認に失敗しました',
      clientpickerunsupported: 'このブラウザはローカルフォルダ選択に非対応です。Chrome / Edge をご利用ください',
      clientabsoluterequired: '完全絶対パスを選択してください（例 D:\\Backup）',
      localneedabsolute: 'ドライブまたはフォルダを開いてから確定（例 D:\\Backup）',
      localpickedname: '選択：{name}',
      createdirectory: '新しいフォルダ',
      createdirectorysuccess: 'フォルダを作成しました',
      createdirectoryfailed: 'フォルダ作成に失敗しました',
      newfolderplaceholder: '新しいフォルダ名',
      newfolderrequired: 'フォルダ名を入力してください',
      ftppathplaceholder: 'リモートパスを入力して移動（例 /backup）',
      uncplaceholder: '\\\\server\\share\\folder',
      passwordkeep: '空欄は保存済みパスワードを維持',
      reselect: '再選択',
      notselecmed: 'パス未選択',
    },
    backuptype: {
      full: 'Full Sync（完全）',
      delta: 'Delta Sync（差分）',
    },
    status: {
      pending: '待機',
      running: '実行中',
      success: '成功',
      failed: '失敗',
      scheduled: 'スケジュール済',
    },
    executemode: {
      immediate: '即時',
      background: 'バックグラウンド',
    },
    button: {
      runnow: '即時実行',
      schedule: 'スケジュール',
      refresh: '更新',
    },
    tip: {
      path: 'ローカル=API ホスト、クライアント=本機選択、ファイルサーバ=UNC、FTP=リモート',
      delta: '差分バックアップには事前の完全バックアップが必要です',
      schedule: '即時/スケジュールとも Quartz タスクを作成します',
    },
    message: {
      runsuccess: '即時バックアップタスクを作成しました',
      schedulesuccess: 'バックグラウンドバックアップタスクを作成しました',
      pathrequired: 'バックアップパスを選択してください',
      schedulerequired: '実行時刻を選択してください',
      schedulefuture: '実行時刻は現在より後である必要があります',
      browsefailed: 'ディレクトリ参照に失敗しました',
      ftprequired: 'FTP のホスト・ユーザー・パスワードを入力してください',
    },
  },
};
