// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：publicIp.ts
// 创建时间：2026-10-27
// 创建人：Takt365(Cursor AI)
// 功能描述：浏览器出口公网 IP 探测（I/O 网关；经公网 echo，非 HTTP RemoteIp/::1）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** 公网 echo 源（浏览器直连，反映客户端 NAT 出口） */
const PUBLIC_IP_ECHO_URLS = [
  'https://api.ipify.org?format=json',
  'https://api64.ipify.org?format=json',
] as const

/**
 * 从 JSON 文本解析 ip 字段
 * @param text 响应正文
 * @returns IP 或 null
 */
function tryParseIpifyJson(text: string): string | null {
  try {
    const data = JSON.parse(text) as { ip?: unknown }
    const ip = typeof data.ip === 'string' ? data.ip.trim() : ''
    return ip || null
  } catch {
    return null
  }
}

/**
 * 探测当前浏览器出口公网 IP（客户端 NAT 对外地址；与本机网卡/::1 无关）
 * @param timeoutMs 单源超时毫秒
 * @returns {Promise<string>} 公网 IP
 * @throws 全部 echo 失败时抛出 Error
 */
export async function resolveBrowserPublicIp(timeoutMs = 6000): Promise<string> {
  const errors: string[] = []
  for (const url of PUBLIC_IP_ECHO_URLS) {
    const controller = new AbortController()
    const timer = window.setTimeout(() => controller.abort(), timeoutMs)
    try {
      const response = await fetch(url, {
        method: 'GET',
        signal: controller.signal,
        cache: 'no-store',
      })
      if (!response.ok) {
        errors.push(`${url} HTTP ${response.status}`)
        continue
      }
      const ip = tryParseIpifyJson(await response.text())
      if (ip) {
        return ip
      }
      errors.push(`${url} empty ip`)
    } catch (error) {
      const message = error instanceof Error ? error.message : String(error)
      errors.push(`${url} ${message}`)
    } finally {
      window.clearTimeout(timer)
    }
  }

  throw new Error(errors.length ? errors.join('; ') : 'public ip echo failed')
}
