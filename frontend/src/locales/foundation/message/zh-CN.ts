// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/foundation/message
// 文件名称：zh-CN.ts
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
        unread: "我的未读",
        read: "我的已读",
      },
    },
    recipient: {
      all: "当前公司全部用户",
      list: {
        label: "指定用户",
        select: "接收用户",
        placeholder: "请选择接收用户（最多 5 人）",
        required: "请至少选择一位接收用户",
        max: "最多只能选择 {max} 位接收用户",
      },
      send: {
        to: {
          all: {
            forbidden: "全员发送仅超级管理员可用",
          },
        },
      },
      broadcast: {
        success: "广播已发送",
      },
    },
    upload: {
      select: "选择文件",
      image: {
        hint: "支持常见图片格式，上传后自动填入附件链接",
      },
      file: {
        hint: "上传附件文件，大文件将自动分片上传",
      },
      multimedia: {
        hint: "支持图片、文件、视频、语音等格式，大文件将自动分片上传",
      },
      video: {
        hint: "上传视频文件",
      },
      voice: {
        hint: "上传语音文件",
      },
      success: "附件上传成功",
      failed: "附件上传失败",
      required: "请先上传附件",
      content: {
        optional: "可填写文字说明（选填）",
      },
    },
  },
};
