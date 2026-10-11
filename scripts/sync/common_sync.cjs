// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：scripts/sync
// 文件名称：common_sync.cjs
// 创建时间：2026-07-07
// 创建人：Takt365(Cursor AI)
// 功能描述：源数据同步脚本公共配置与 sqlcmd 工具（供 matplt/mdl/so/st/ec_sync 引用）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const { execFileSync, spawn } = require('child_process');
const fs = require('fs');
const path = require('path');

/** SQL Server 主机（可用环境变量 TAKT_SYNC_SQL_SERVER 覆盖） */
const SQL_SERVER = process.env.TAKT_SYNC_SQL_SERVER || 'fs03';
/** SQL 登录用户 */
const SQL_USER = process.env.TAKT_SYNC_SQL_USER || 'sa';
/** SQL 登录密码 */
const SQL_PWD = process.env.TAKT_SYNC_SQL_PWD || 'Tac26901333.';
/** Takt 业务库 */
const DB = process.env.TAKT_SYNC_DB || 'zTakt_000_Dev';
/** 批大小；0=全量（可用 TAKT_SYNC_BATCH_SIZE 覆盖） */
const BATCH_SIZE = Number(process.env.TAKT_SYNC_BATCH_SIZE ?? 0);
/** sqlcmd 可执行路径 */
const SQLCMD_PATH = process.env.TAKT_SYNC_SQLCMD_PATH
  || 'C:\\Program Files\\Microsoft SQL Server\\Client SDK\\ODBC\\170\\Tools\\Binn\\SQLCMD.EXE';

/** 同步写入默认租户码 */
const TENANT_CODE = process.env.TAKT_SYNC_TENANT_CODE || '000';
/** 同步写入默认公司码 */
const COMPANY_CODE = process.env.TAKT_SYNC_COMPANY_CODE || '2300';
/** 同步系统用户 Id（created_by / updated_by） */
const SYNC_USER_ID = Number(process.env.TAKT_SYNC_USER_ID || 900001);

/** oper_log 用户名 */
const SAP_SYNC_USER_NAME = 'SYSTEM_SAP_SYNC';
/** oper_log 操作类型 */
const SAP_SYNC_OPER_TYPE = 'SAP_SYNC';

/** 大脚本执行超时（毫秒） */
const SQL_EXEC_TIMEOUT_MS = 1_800_000;
/** sqlcmd 登录超时（秒） */
const SQL_LOGIN_TIMEOUT_SEC = '180';
/** 标量查询超时（毫秒） */
const SQL_VALUE_TIMEOUT_MS = 60_000;

/**
 * @returns {number} 写入 T-SQL @batch_size 的值
 */
function resolveBatchSizeSqlValue() {
  return BATCH_SIZE > 0 ? BATCH_SIZE : 0;
}

/**
 * @returns {string} 批大小日志文案
 */
function formatBatchSizeLabel() {
  return BATCH_SIZE > 0 ? `${BATCH_SIZE} 条/批` : '全部（正式环境）';
}

/**
 * 构建 sqlcmd 连接参数前缀
 * @returns {string[]}
 */
function buildSqlCmdConnectionArgs() {
  return ['-S', SQL_SERVER, '-U', SQL_USER, '-P', SQL_PWD, '-d', DB, '-f', '65001'];
}

/**
 * 异步执行 sqlcmd，带心跳与超时
 * @param {string[]} args sqlcmd 参数
 * @param {string} label 日志标签
 * @param {number} [timeoutMs] 超时毫秒
 * @returns {Promise<string>} stdout
 */
function runSqlCmdWithProgress(args, label, timeoutMs = SQL_EXEC_TIMEOUT_MS) {
  return new Promise((resolve, reject) => {
    const start = Date.now();
    let stdout = '';
    let stderr = '';
    let finished = false;
    const child = spawn(SQLCMD_PATH, args, { stdio: ['ignore', 'pipe', 'pipe'] });

    const heartbeat = setInterval(() => {
      const elapsed = Math.round((Date.now() - start) / 1000);
      console.log(`   [${label}] 仍在执行... 已耗时 ${elapsed}s`);
    }, 10000);

    const timeoutHandle = setTimeout(() => {
      if (!finished) {
        finished = true;
        clearInterval(heartbeat);
        try { child.kill('SIGTERM'); } catch (_) {}
        reject(new Error(`执行超时 (${Math.round(timeoutMs / 1000)}s)`));
      }
    }, timeoutMs);

    child.stdout.on('data', (chunk) => { stdout += chunk.toString(); });
    child.stderr.on('data', (chunk) => { stderr += chunk.toString(); });
    child.on('error', (err) => {
      if (!finished) {
        finished = true;
        clearInterval(heartbeat);
        clearTimeout(timeoutHandle);
        reject(err);
      }
    });
    child.on('close', (code) => {
      if (!finished) {
        finished = true;
        clearInterval(heartbeat);
        clearTimeout(timeoutHandle);
        if (code === 0) resolve(stdout);
        else reject(new Error(stderr || stdout || `sqlcmd exit code ${code}`));
      }
    });
  });
}

/**
 * 将 SQL 写入临时文件并通过 sqlcmd 执行
 * @param {string} sql T-SQL 脚本
 * @param {string} label 日志标签
 * @param {{ filePrefix?: string }} [options]
 * @returns {Promise<string>} stdout；失败时返回空串
 */
async function execSQL(sql, label, options = {}) {
  const prefix = options.filePrefix || 'sap_sync';
  const tmpFile = path.join(process.env.TEMP, `${prefix}_${Date.now()}.sql`);
  fs.writeFileSync(tmpFile, sql, 'utf8');
  const args = [
    ...buildSqlCmdConnectionArgs(),
    '-l', SQL_LOGIN_TIMEOUT_SEC,
    '-i', tmpFile,
    '-b',
    '-C',
  ];
  try {
    console.log(`▶ [${label}] 开始执行...`);
    const stdout = await runSqlCmdWithProgress(args, label, SQL_EXEC_TIMEOUT_MS);
    if (stdout && stdout.trim()) {
      console.log(stdout.trim());
    }
    console.log(`   [${label}] ✅ 完成`);
    return stdout || '';
  } catch (e) {
    const detail = (e && (e.stderr || e.stdout || e.message || '')).toString().slice(0, 1000);
    console.error(`❌ [${label}] ${detail}`);
    return '';
  } finally {
    try { fs.unlinkSync(tmpFile); } catch (_) {}
  }
}

/**
 * 执行标量查询并返回首行文本
 * @param {string} sql 须返回单值
 * @param {{ filePrefix?: string }} [options]
 * @returns {string}
 */
function execSQLValue(sql, options = {}) {
  const prefix = options.filePrefix || 'sap_val';
  const tmpFile = path.join(process.env.TEMP, `${prefix}_${Date.now()}.sql`);
  fs.writeFileSync(tmpFile, sql, 'utf8');
  const outFile = path.join(process.env.TEMP, `${prefix}_out_${Date.now()}.txt`);
  const args = [
    ...buildSqlCmdConnectionArgs(),
    '-i', tmpFile,
    '-o', outFile,
    '-h', '-1',
    '-W',
    '-r', '0',
    '-b',
    '-C',
  ];
  try {
    execFileSync(SQLCMD_PATH, args, { encoding: 'utf8', timeout: SQL_VALUE_TIMEOUT_MS, stdio: 'pipe' });
    let text = fs.readFileSync(outFile, 'utf8').trim();
    text = text.split('\n')
      .filter((l) => !/^\s*\(\d+\s+rows?\s+affected\)\s*$/i.test(l))
      .join('\n')
      .trim();
    return text || '0';
  } catch (e) {
    return '0';
  } finally {
    try { fs.unlinkSync(tmpFile); } catch (_) {}
    try { fs.unlinkSync(outFile); } catch (_) {}
  }
}

/**
 * 从 sqlcmd 输出解析 INS/UPD 汇总行
 * @param {string} text sqlcmd stdout
 * @returns {{ mainInsert: number, mainUpdate: number }}
 */
function parseInsUpdSummaryCounts(text) {
  const insertMatch = text.match(/INS\s+(\d+)/i);
  const updateMatch = text.match(/UPD\s+(\d+)/i);
  return {
    mainInsert: insertMatch ? Number(insertMatch[1]) : 0,
    mainUpdate: updateMatch ? Number(updateMatch[1]) : 0,
  };
}

module.exports = {
  SQL_SERVER,
  SQL_USER,
  SQL_PWD,
  DB,
  BATCH_SIZE,
  SQLCMD_PATH,
  TENANT_CODE,
  COMPANY_CODE,
  SYNC_USER_ID,
  SAP_SYNC_USER_NAME,
  SAP_SYNC_OPER_TYPE,
  SQL_EXEC_TIMEOUT_MS,
  SQL_LOGIN_TIMEOUT_SEC,
  SQL_VALUE_TIMEOUT_MS,
  resolveBatchSizeSqlValue,
  formatBatchSizeLabel,
  buildSqlCmdConnectionArgs,
  runSqlCmdWithProgress,
  execSQL,
  execSQLValue,
  parseInsUpdSummaryCounts,
};
