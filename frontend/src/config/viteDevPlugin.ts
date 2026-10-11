// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/config
// 文件名称：viteDevPlugin.ts
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：Vite 开发服务器插件（代理请求日志 + 客户端日志落盘 frontend/logs）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { dirname, resolve } from 'node:path';
import { fileURLToPath } from 'node:url';
import type { IncomingMessage, ServerResponse } from 'node:http';
import type { Plugin, ViteDevServer } from 'vite';
import type { LogReportPayload } from '../types/logger.types.ts';
import {
  TAKT_CLIENT_LOG_INGEST_PATH,
  writeClientLogEntriesToFiles,
} from './viteLogFileWriter.ts';

/** 开发代理路径前缀（与 vite.config 代理段一致） */
const DEV_LOG_PATH_PREFIXES = ['/api', '/connect', '/hubs'] as const;

const pluginDir = dirname(fileURLToPath(import.meta.url));
/** 前端本地日志目录（与 backend WebApi/logs 同级概念） */
const FRONTEND_LOGS_DIR = resolve(pluginDir, '../../logs');

/**
 * 读取 POST 请求体
 * @param {IncomingMessage} req 请求
 * @returns {Promise<string>} UTF-8 文本
 */
function readRequestBody(req: IncomingMessage): Promise<string> {
  return new Promise((resolveBody, reject) => {
    const chunks: Buffer[] = [];
    req.on('data', (chunk: Buffer) => {
      chunks.push(chunk);
    });
    req.on('end', () => {
      resolveBody(Buffer.concat(chunks).toString('utf8'));
    });
    req.on('error', reject);
  });
}

/**
 * 处理客户端日志落盘请求
 * @param {IncomingMessage} req 请求
 * @param {ServerResponse} res 响应
 */
async function handleClientLogIngest(req: IncomingMessage, res: ServerResponse): Promise<void> {
  try {
    const body = await readRequestBody(req);
    const payload = JSON.parse(body) as LogReportPayload;
    if (!Array.isArray(payload.entries)) {
      res.statusCode = 400;
      res.end('Bad Request');
      return;
    }
    writeClientLogEntriesToFiles(FRONTEND_LOGS_DIR, payload.entries);
    res.statusCode = 204;
    res.end();
  } catch {
    res.statusCode = 400;
    res.end('Bad Request');
  }
}

/**
 * 开发服务器 API / OAuth / SignalR 请求日志 + 客户端日志落盘（仅 serve）
 * @returns {Plugin} Vite 插件
 */
export function vitePluginLogger(): Plugin {
  return {
    name: 'takt-vite-dev-logger',
    apply: 'serve',
    configureServer(server: ViteDevServer) {
      server.middlewares.use((req, res, next) => {
        const url = req.url?.split('?')[0] ?? '';
        if (url === TAKT_CLIENT_LOG_INGEST_PATH && req.method === 'POST') {
          void handleClientLogIngest(req, res);
          return;
        }
        const fullUrl = req.url ?? '';
        const shouldLog = DEV_LOG_PATH_PREFIXES.some((prefix) => fullUrl.startsWith(prefix));
        if (shouldLog) {
          // eslint-disable-next-line no-console -- 开发环境终端调试
          console.info(`[Vite Dev] ${req.method ?? 'GET'} ${fullUrl}`);
        }
        next();
      });
    },
  };
}
