// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/foundation/cache
// 文件名称：zh-HK.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：foundation/cache 页面静态文案；引用键 foundation.cache.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: "緩存管理",
    description: "查看運行時緩存配置與統計，並按鍵檢查或移除緩存項",
    section: {
      config: "緩存配置",
      statistics: "緩存統計",
      key: {
        ops: "按鍵操作",
      },
    },
    field: {
      provider: "緩存提供者",
      default: {
        expiration: {
          minutes: "預設過期（分鐘）",
        },
      },
      enable: {
        sliding: {
          expiration: "滑動過期",
        },
        multi: {
          level: {
            cache: "多級緩存",
          },
        },
      },
      redis: {
        instance: {
          name: "Redis 實例前綴",
        },
      },
      note: "說明",
      current: {
        entry: {
          count: "目前條目數",
        },
      },
      total: {
        hits: "命中次數",
        misses: "未命中次數",
      },
      hit: {
        rate: "命中率",
      },
      estimated: {
        size: {
          bytes: "估算佔用（位元組）",
        },
      },
      cache: {
        key: "緩存鍵",
      },
    },
    placeholder: {
      cache: {
        key: "請輸入完整緩存鍵",
      },
    },
    button: {
      check: {
        exists: "檢查存在",
      },
      remove: "移除鍵",
    },
    rule: {
      cache: {
        key: {
          required: "請輸入緩存鍵",
        },
      },
    },
    message: {
      load: {
        fail: "載入緩存資訊失敗",
      },
      check: {
        fail: "檢查緩存鍵失敗",
      },
      remove: {
        success: "緩存鍵已移除",
        fail: "移除緩存鍵失敗",
      },
      loading: {
        hint: "載入中…",
      },
    },
    alert: {
      key: {
        exists: "緩存鍵存在",
        not: {
          exists: "緩存鍵不存在",
        },
      },
    },
  },
};
