// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/statistics/logging/server-monitor
// 文件名称：ja-JP.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：statistics/logging/server-monitor 页面静态文案；引用键 statistics.logging.server-monitor.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: "サービス監視",
    description: "アプリケーション稼働状態とサーバーハードウェア（CPU、メモリ、GPU、ディスク、ネットワーク）を表示",
    tabs: {
      app: "アプリ",
      system: "システム",
      cpu: "CPU",
      memory: "メモリ",
      gpu: "GPU",
      drive: "ディスク",
      network: "ネットワーク",
    },
    section: {
      app: {
        status: "アプリケーション状態",
      },
      os: {
        language: "OS と言語",
      },
      motherboard: "マザーボード",
      cpu: "CPU 情報",
      memory: "メモリ情報",
      gpu: "GPU 情報",
      drive: "ディスク情報",
      network: "ネットワークアダプター",
    },
    field: {
      application: {
        name: "アプリケーション名",
        version: "バージョン",
      },
      environment: "実行環境",
      machine: {
        name: "マシン名",
      },
      dot: {
        net: {
          version: ".NET バージョン",
        },
      },
      process: {
        architecture: "プロセスアーキテクチャ",
      },
      processor: {
        count: "プロセッサ数",
      },
      start: {
        time: "起動時刻",
      },
      uptime: "稼働時間",
      working: {
        set: "ワーキングセット",
      },
      operating: {
        system: "オペレーティングシステム",
      },
      os: {
        version: "OS バージョン",
      },
      current: {
        culture: "現在のカルチャ",
        ui: {
          culture: "現在の UI カルチャ",
        },
      },
      system: {
        type: "OS アーキテクチャ",
        type32: "32 ビット",
        type64: "64 ビット",
      },
      motherboard: {
        manufacturer: "マザーボードメーカー",
        product: "マザーボード型番 / ID",
        serial: {
          number: "マザーボードシリアル",
        },
        version: "マザーボードバージョン",
        uuid: "マシン UUID",
      },
      cpu: {
        name: "名称",
        manufacturer: "メーカー",
        cores: "コア数",
        logical: {
          processors: "論理プロセッサ",
          core: {
            name: "論理コア",
          },
        },
        usage: {
          percent: "CPU 使用率",
        },
        model: "CPU モデル",
        socket: "ソケット",
        processor: {
          id: "プロセッサ ID",
        },
      },
      usage: {
        used: "使用中",
        idle: "空き",
      },
      memory: {
        total: {
          physical: "物理メモリ合計",
        },
        used: {
          physical: "使用物理メモリ",
        },
        available: "空き",
        usage: {
          percent: "メモリ使用率",
        },
        type: {
          physical: "物理メモリ",
          virtual: "仮想メモリ",
        },
        bank: {
          label: "スロット",
        },
        manufacturer: "メーカー",
        capacity: "容量",
        speed: "周波数",
        part: {
          number: "部品番号",
        },
        serial: {
          number: "シリアル番号",
        },
      },
      gpu: {
        name: "名称",
        manufacturer: "メーカー",
        adapter: {
          ram: "ビデオメモリ",
        },
        driver: {
          version: "ドライバーバージョン",
        },
      },
      drive: {
        name: "ドライブ",
        type: "種類",
        file: {
          system: "ファイルシステム",
        },
        total: {
          size: "総容量",
        },
        free: {
          space: "空き容量",
        },
        used: {
          space: "使用容量",
        },
        usage: {
          percent: "使用率",
        },
      },
      network: {
        name: "名称",
        description: "説明",
        mac: {
          address: "MAC アドレス",
        },
        ip: {
          address: "IP アドレス",
        },
        speed: "速度",
        status: {
          online: "オンライン",
          no: {
            internet: "インターネット未接続",
          },
          dns: {
            fault: "DNS 異常",
          },
          up: "接続中",
          down: "切断",
          enabled: "有効",
          disabled: "無効",
          unknown: "不明",
        },
      },
    },
    unit: {
      core: "コア",
      thread: "スレッド",
      day: "日",
      hour: "時間",
      minute: "分",
    },
    button: {
      refresh: {
        cache: "ハードウェアキャッシュを更新",
      },
    },
    message: {
      load: {
        fail: "監視データの読み込みに失敗しました",
      },
      refresh: {
        success: "ハードウェアキャッシュを更新しました",
      },
    },
  },
};
