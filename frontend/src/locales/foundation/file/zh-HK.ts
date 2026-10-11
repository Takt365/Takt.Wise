// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/foundation/file
// 文件名称：zh-HK.ts
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
        placeholder: "輸入標籤後回車添加，最多 {max} 個，如：上班、下班",
      },
      max: {
        limit: "最多添加 {max} 個標籤",
      },
      add: "添加標籤",
    },
    custom: {
      filename: {
        placeholder: "請輸入磁碟存儲檔案名，可不填副檔名（將按上傳檔案自動補全）",
      },
    },
    upload: {
      size: {
        exceeded: "檔案大小超出了限制，系統僅支持500Mb",
      },
      unsupported: "上傳的檔案不被支持。請聯繫管理員或重新上傳其他檔案",
      hint: "支持常見壓縮包、圖片、音視頻、Office 新格式及 pdf/txt/xml/json；單檔不超過 {max}MB",
    },
    oss: {
      provider: "OSS 提供商",
    },
    ftp: {
      provider: "FTP 提供商",
    },
  },
};
