// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：taktMessageDisplay.ts
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：在线消息展示文案纯函数（发送者昵称+登录名、通知正文）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 格式化消息发送者展示名：有昵称时为 nickName&lt;userName&gt;，否则为登录名
 * @param nickName 发送者昵称
 * @param userName 发送者登录名
 * @returns 展示用发送者标识
 */
export function formatMessageSenderDisplay(
  nickName: string | null | undefined,
  userName: string | null | undefined,
): string {
  const loginName = (userName ?? '').trim() || '?';
  const nick = (nickName ?? '').trim();
  if (nick) {
    return `${nick}<${loginName}>`;
  }
  return loginName;
}

/**
 * 组装通知中心私信正文：{发送者}: {消息内容}
 * @param nickName 发送者昵称
 * @param userName 发送者登录名
 * @param messageContent 消息正文
 * @returns 通知正文
 */
export function formatPrivateMessageNotificationContent(
  nickName: string | null | undefined,
  userName: string | null | undefined,
  messageContent: string | null | undefined,
): string {
  const sender = formatMessageSenderDisplay(nickName, userName);
  const body = (messageContent ?? '').trim();
  return body ? `${sender}: ${body}` : sender;
}
