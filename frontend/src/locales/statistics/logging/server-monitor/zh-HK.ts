// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/statistics/logging/server-monitor
// 文件名称：zh-HK.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：statistics/logging/server-monitor 页面静态文案；引用键 statistics.logging.server-monitor.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: "服務監控",
    description: "查看應用運行狀態與伺服器硬件資訊（CPU、記憶體、顯示卡、磁碟、網絡）",
    tabs: {
      app: "應用",
      system: "系統",
      cpu: "CPU",
      memory: "記憶體",
      gpu: "顯示卡",
      drive: "磁碟",
      network: "網絡",
    },
    section: {
      app: {
        status: "應用狀態",
      },
      os: {
        language: "作業系統與語言",
      },
      motherboard: "主機板",
      cpu: "CPU 資訊",
      memory: "記憶體資訊",
      gpu: "顯示卡資訊",
      drive: "磁碟資訊",
      network: "網絡介面卡",
    },
    field: {
      application: {
        name: "應用名稱",
        version: "應用版本",
      },
      environment: "運行環境",
      machine: {
        name: "機器名",
      },
      dot: {
        net: {
          version: ".NET 版本",
        },
      },
      process: {
        architecture: "進程架構",
      },
      processor: {
        count: "處理器數量",
      },
      start: {
        time: "啟動時間",
      },
      uptime: "運行時長",
      working: {
        set: "工作集記憶體",
      },
      operating: {
        system: "作業系統",
      },
      os: {
        version: "系統版本",
      },
      current: {
        culture: "目前區域文化",
        ui: {
          culture: "目前 UI 文化",
        },
      },
      system: {
        type: "系統架構",
        type32: "32 位元",
        type64: "64 位元",
      },
      motherboard: {
        manufacturer: "主機板製造商",
        product: "主機板型號 / ID",
        serial: {
          number: "主機板序列號",
        },
        version: "主機板版本",
        uuid: "機器 UUID",
      },
      cpu: {
        name: "名稱",
        manufacturer: "製造商",
        cores: "核心數",
        logical: {
          processors: "邏輯處理器",
          core: {
            name: "邏輯核心",
          },
        },
        usage: {
          percent: "CPU 使用率",
        },
        model: "CPU 型號",
        socket: "插槽",
        processor: {
          id: "處理器 ID",
        },
      },
      usage: {
        used: "已用",
        idle: "空閒",
      },
      memory: {
        total: {
          physical: "實體記憶體總量",
        },
        used: {
          physical: "已用實體記憶體",
        },
        available: "可用",
        usage: {
          percent: "記憶體使用率",
        },
        type: {
          physical: "實體記憶體",
          virtual: "虛擬記憶體",
        },
        bank: {
          label: "插槽",
        },
        manufacturer: "製造商",
        capacity: "容量",
        speed: "頻率",
        part: {
          number: "部件號",
        },
        serial: {
          number: "序列號",
        },
      },
      gpu: {
        name: "名稱",
        manufacturer: "製造商",
        adapter: {
          ram: "顯示記憶體",
        },
        driver: {
          version: "驅動版本",
        },
      },
      drive: {
        name: "磁碟機",
        type: "類型",
        file: {
          system: "檔案系統",
        },
        total: {
          size: "總容量",
        },
        free: {
          space: "可用空間",
        },
        used: {
          space: "已用空間",
        },
        usage: {
          percent: "使用率",
        },
      },
      network: {
        name: "名稱",
        description: "描述",
        mac: {
          address: "MAC 位址",
        },
        ip: {
          address: "IP 位址",
        },
        speed: "速度",
        status: {
          online: "已聯網",
          no: {
            internet: "無外網",
          },
          dns: {
            fault: "DNS 異常",
          },
          up: "已連線",
          down: "已中斷",
          enabled: "已啟用",
          disabled: "已停用",
          unknown: "未知",
        },
      },
    },
    unit: {
      core: "核",
      thread: "執行緒",
      day: "天",
      hour: "小時",
      minute: "分鐘",
    },
    button: {
      refresh: {
        cache: "刷新硬件緩存",
      },
    },
    message: {
      load: {
        fail: "載入監控數據失敗",
      },
      refresh: {
        success: "硬件資訊緩存已刷新",
      },
    },
  },
};
