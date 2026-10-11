// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/foundation/cache
// 文件名称：ja-JP.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：foundation/cache 页面静态文案；引用键 foundation.cache.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: "キャッシュ管理",
    description: "実行時キャッシュの設定と統計を表示し、キー単位で存在確認・削除を行います",
    section: {
      config: "キャッシュ設定",
      statistics: "キャッシュ統計",
      key: {
        ops: "キー操作",
      },
    },
    field: {
      provider: "プロバイダー",
      default: {
        expiration: {
          minutes: "デフォルト有効期限（分）",
        },
      },
      enable: {
        sliding: {
          expiration: "スライディング有効期限",
        },
        multi: {
          level: {
            cache: "多段キャッシュ",
          },
        },
      },
      redis: {
        instance: {
          name: "Redis インスタンス接頭辞",
        },
      },
      note: "説明",
      current: {
        entry: {
          count: "現在のエントリ数",
        },
      },
      total: {
        hits: "ヒット数",
        misses: "ミス数",
      },
      hit: {
        rate: "ヒット率",
      },
      estimated: {
        size: {
          bytes: "推定サイズ（バイト）",
        },
      },
      cache: {
        key: "キャッシュキー",
      },
    },
    placeholder: {
      cache: {
        key: "キャッシュキーを入力",
      },
    },
    button: {
      check: {
        exists: "存在確認",
      },
      remove: "キー削除",
    },
    rule: {
      cache: {
        key: {
          required: "キャッシュキーを入力してください",
        },
      },
    },
    message: {
      load: {
        fail: "キャッシュ情報の読み込みに失敗しました",
      },
      check: {
        fail: "キャッシュキーの確認に失敗しました",
      },
      remove: {
        success: "キャッシュキーを削除しました",
        fail: "キャッシュキーの削除に失敗しました",
      },
      loading: {
        hint: "読み込み中…",
      },
    },
    alert: {
      key: {
        exists: "キャッシュキーは存在します",
        not: {
          exists: "キャッシュキーは存在しません",
        },
      },
    },
  },
};
