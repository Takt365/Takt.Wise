// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/foundation/file
// 文件名称：zh-CN.ts
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
        placeholder: "输入标签后回车添加，最多 {max} 个，如：上班、下班",
      },
      max: {
        limit: "最多添加 {max} 个标签",
      },
      add: "添加标签",
    },
    custom: {
      filename: {
        placeholder: "请输入磁盘存储文件名，可不含扩展名（将按上传文件自动补全）",
      },
    },
    upload: {
      size: {
        exceeded: "文件大小超出了限制，系统仅支持500Mb",
      },
      unsupported: "上传的文件不被支持。请联系管理员或重新上传其他文件",
      hint: "支持常见压缩包、图片、音视频、Office 新格式及 pdf/txt/xml/json；单文件不超过 {max}MB",
    },
    oss: {
      provider: "OSS 提供商",
    },
    ftp: {
      provider: "FTP 提供商",
    },
  },
};
