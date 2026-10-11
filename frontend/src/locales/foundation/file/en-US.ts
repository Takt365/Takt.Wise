// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/foundation/file
// 文件名称：en-US.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：foundation/file page static copy; keys foundation.file.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    tags: {
      input: {
        placeholder: "Type a tag and press Enter (max {max}), e.g. work, off-duty",
      },
      max: {
        limit: "You can add at most {max} tags",
      },
      add: "Add tag",
    },
    custom: {
      filename: {
        placeholder: "Enter the stored file name; extension is optional and will be inferred from the upload",
      },
    },
    upload: {
      size: {
        exceeded: "The file exceeds the size limit. The system supports up to 500MB only.",
      },
      unsupported: "This file type is not supported. Contact an administrator or upload a different file.",
      hint: "Supports common archives, images, audio/video, Office formats, pdf/txt/xml/json; max {max}MB per file",
    },
    oss: {
      provider: "OSS provider",
    },
    ftp: {
      provider: "FTP provider",
    },
  },
};
