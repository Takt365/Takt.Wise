// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/foundation/file
// 文件名称：ja-JP.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：foundation/file 页面静态文案；引用键 foundation.file.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    tags: {
      input: {
        placeholder: "タグを入力して Enter（最大 {max} 件）例：出勤、退勤",
      },
      max: {
        limit: "タグは最大 {max} 件まで追加できます",
      },
      add: "タグを追加",
    },
    custom: {
      filename: {
        placeholder: "保存ファイル名を入力（拡張子省略可・アップロードファイルから補完）",
      },
    },
    upload: {
      size: {
        exceeded: "ファイルサイズが上限を超えています。システムは最大500MBまで対応しています。",
      },
      unsupported: "このファイルはサポートされていません。管理者に連絡するか、別のファイルをアップロードしてください。",
      hint: "圧縮・画像・音声/動画・Office・pdf/txt/xml/json 等に対応；1ファイル最大 {max}MB",
    },
    oss: {
      provider: "OSSプロバイダ",
    },
    ftp: {
      provider: "FTPプロバイダ",
    },
  },
};
