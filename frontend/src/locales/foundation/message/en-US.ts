// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/foundation/message
// 文件名称：en-US.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：foundation/message page static copy; keys foundation.message.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    list: {
      scope: {
        all: "All Messages",
        unread: "My Unread",
        read: "My Read",
      },
    },
    recipient: {
      all: "All users in current company",
      list: {
        label: "Selected users",
        select: "Recipients",
        placeholder: "Select recipients (up to 5)",
        required: "Select at least one recipient",
        max: "You can select at most {max} recipients",
      },
      send: {
        to: {
          all: {
            forbidden: "Broadcast is available to super administrators only",
          },
        },
      },
      broadcast: {
        success: "Broadcast sent",
      },
    },
    upload: {
      select: "Select file",
      image: {
        hint: "Upload an image; the attachment URL will be filled automatically",
      },
      file: {
        hint: "Upload a file; large files use chunked upload",
      },
      multimedia: {
        hint: "Images, files, video, or audio; large files use chunked upload",
      },
      video: {
        hint: "Upload a video file",
      },
      voice: {
        hint: "Upload an audio file",
      },
      success: "Attachment uploaded",
      failed: "Attachment upload failed",
      required: "Please upload an attachment first",
      content: {
        optional: "Optional text description",
      },
    },
  },
};
