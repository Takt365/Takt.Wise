// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/foundation/message
// 文件名称：ja-JP.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：foundation/message 页面静态文案；引用键 foundation.message.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    list: {
      scope: {
        all: "すべてのメッセージ",
        unread: "未読（自分）",
        read: "既読（自分）",
      },
    },
    recipient: {
      all: "現在の会社の全ユーザー",
      list: {
        label: "指定ユーザー",
        select: "受信ユーザー",
        placeholder: "受信ユーザーを選択（最大5人）",
        required: "受信ユーザーを1人以上選択してください",
        max: "受信ユーザーは最大 {max} 人まで選択できます",
      },
      send: {
        to: {
          all: {
            forbidden: "全員送信はスーパー管理者のみ利用できます",
          },
        },
      },
      broadcast: {
        success: "ブロードキャストを送信しました",
      },
    },
    upload: {
      select: "ファイルを選択",
      image: {
        hint: "画像をアップロードすると添付リンクが自動入力されます",
      },
      file: {
        hint: "ファイルをアップロード（大容量は分割アップロード）",
      },
      multimedia: {
        hint: "画像・ファイル・動画・音声などに対応（大容量は分割アップロード）",
      },
      video: {
        hint: "動画ファイルをアップロード",
      },
      voice: {
        hint: "音声ファイルをアップロード",
      },
      success: "添付のアップロードに成功しました",
      failed: "添付のアップロードに失敗しました",
      required: "先に添付をアップロードしてください",
      content: {
        optional: "テキスト説明（任意）",
      },
    },
  },
};
