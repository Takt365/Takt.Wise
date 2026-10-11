// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/foundation/message
// 文件名称：zh-HK.ts
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
        all: "全部消息",
        unread: "我的未讀",
        read: "我的已讀",
      },
    },
    recipient: {
      all: "當前公司全部用戶",
      list: {
        label: "指定用戶",
        select: "接收用戶",
        placeholder: "請選擇接收用戶（最多 5 人）",
        required: "請至少選擇一位接收用戶",
        max: "最多只能選擇 {max} 位接收用戶",
      },
      send: {
        to: {
          all: {
            forbidden: "全員發送僅超級管理員可用",
          },
        },
      },
      broadcast: {
        success: "廣播已發送",
      },
    },
    upload: {
      select: "選擇文件",
      image: {
        hint: "支持常見圖片格式，上傳後自動填入附件連結",
      },
      file: {
        hint: "上傳附件文件，大文件將自動分片上傳",
      },
      multimedia: {
        hint: "支持圖片、文件、視頻、語音等格式，大文件將自動分片上傳",
      },
      video: {
        hint: "上傳視頻文件",
      },
      voice: {
        hint: "上傳語音文件",
      },
      success: "附件上傳成功",
      failed: "附件上傳失敗",
      required: "請先上傳附件",
      content: {
        optional: "可填寫文字說明（選填）",
      },
    },
  },
};
