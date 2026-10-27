// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：scripts/sync
// 文件名称：sync-all.cjs
// 创建时间：2026-07-07
// 创建人：Takt365(Cursor AI)
// 功能描述：一键串联源数据同步；顺序固定为 matplt→mdl→so→st→ec（工厂物料→机种→工单→工时→变更）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const path = require('path');
const { spawnSync } = require('child_process');

const SYNC_DIR = __dirname;
const REPO_ROOT = path.resolve(__dirname, '..', '..');

/**
 * 源数据同步流水线（顺序不可打乱；`order` 为唯一权威序号）
 * @type {Array<{ order: number, key: string, label: string, script: string, dependsOn: string[], reason: string }>}
 */
const PIPELINE = [
  {
    order: 1,
    key: 'matplt',
    label: '源数据同步：工厂物料',
    script: 'matplt_sync.cjs',
    dependsOn: [],
    reason: 'PP_SapMaterial → material_plant；工单/工时/EC 均引用 material_code，须最先落地',
  },
  {
    order: 2,
    key: 'mdl',
    label: '源数据同步：机种目的地',
    script: 'mdl_sync.cjs',
    dependsOn: ['matplt'],
    reason: 'PP_SapModelDest → model_destination；机种与工厂物料同属物料域，紧随 matplt',
  },
  {
    order: 3,
    key: 'so',
    label: '源数据同步：生产工单/工作中心/序列号',
    script: 'so_sync.cjs',
    dependsOn: ['matplt'],
    reason: 'PP_SapOrders → production_order，并从 Manhour/Serial 回填 work_center、serial_no；组立日报 plant 随工单',
  },
  {
    order: 4,
    key: 'st',
    label: '源数据同步：标准工时',
    script: 'st_sync.cjs',
    dependsOn: ['matplt', 'so'],
    reason: 'PP_SapManhour → standard_operation_time；与 so 同源工时但独立落表；须在工单之后供组立 stdMinutes',
  },
  {
    order: 5,
    key: 'ec',
    label: '源数据同步：工程变更主表+明细',
    script: 'ec_sync.cjs',
    dependsOn: ['matplt'],
    reason: 'PP_SapEcn/EcnSub → source_ec + detail；变更明细含料号，放在主数据与计划/工时之后',
  },
];

/** 权威顺序键列表（勿手写排序，统一从此派生） */
const ORDERED_KEYS = PIPELINE.slice().sort((a, b) => a.order - b.order).map((s) => s.key);

/**
 * 按 pipeline.order 排序步骤（--only 乱序入参时仍保证执行顺序）
 * @param {typeof PIPELINE} steps
 * @returns {typeof PIPELINE}
 */
function sortStepsByOrder(steps) {
  return steps.slice().sort((a, b) => a.order - b.order);
}

/**
 * 执行子同步脚本
 * @param {string} scriptName 脚本文件名
 * @returns {number} 进程退出码
 */
function runChildScript(scriptName) {
  const scriptPath = path.join(SYNC_DIR, scriptName);
  const result = spawnSync(process.execPath, [scriptPath], {
    cwd: REPO_ROOT,
    stdio: 'inherit',
    env: process.env,
  });
  if (result.error) {
    throw result.error;
  }
  return result.status ?? 1;
}

/**
 * 解析 CLI：--only matplt,so 或 --from st
 * @returns {{ help?: boolean, onlyKeys: Set<string>|null, fromKey: string|null }}
 */
function parseCliArgs() {
  const argv = process.argv.slice(2);
  let onlyKeys = null;
  let fromKey = null;
  for (let i = 0; i < argv.length; i++) {
    const arg = argv[i];
    if (arg === '--help' || arg === '-h') {
      return { help: true };
    }
    if (arg === '--only' && argv[i + 1]) {
      onlyKeys = new Set(
        argv[i + 1]
          .split(',')
          .map((s) => s.trim().toLowerCase())
          .filter(Boolean)
      );
      i += 1;
      continue;
    }
    if (arg.startsWith('--only=')) {
      onlyKeys = new Set(
        arg
          .slice('--only='.length)
          .split(',')
          .map((s) => s.trim().toLowerCase())
          .filter(Boolean)
      );
      continue;
    }
    if (arg === '--from' && argv[i + 1]) {
      fromKey = argv[i + 1].trim().toLowerCase();
      i += 1;
      continue;
    }
    if (arg.startsWith('--from=')) {
      fromKey = arg.slice('--from='.length).trim().toLowerCase();
    }
  }
  return { onlyKeys, fromKey };
}

/**
 * 按 CLI 过滤流水线步骤（结果始终按 order 升序）
 * @param {typeof PIPELINE} pipeline
 * @param {{ onlyKeys: Set<string>|null, fromKey: string|null }} options
 * @returns {typeof PIPELINE}
 */
function resolvePipeline(pipeline, options) {
  const validKeys = new Set(pipeline.map((s) => s.key));
  if (options.onlyKeys) {
    for (const key of options.onlyKeys) {
      if (!validKeys.has(key)) {
        throw new Error(`未知步骤 key: ${key}，可选: ${ORDERED_KEYS.join(', ')}`);
      }
    }
    return sortStepsByOrder(pipeline.filter((s) => options.onlyKeys.has(s.key)));
  }
  if (options.fromKey) {
    if (!validKeys.has(options.fromKey)) {
      throw new Error(`未知 --from key: ${options.fromKey}，可选: ${ORDERED_KEYS.join(', ')}`);
    }
    const startOrder = pipeline.find((s) => s.key === options.fromKey).order;
    return sortStepsByOrder(pipeline.filter((s) => s.order >= startOrder));
  }
  return sortStepsByOrder(pipeline);
}

/**
 * --only 子集须包含各步骤 dependsOn 中的前置 key
 * @param {typeof PIPELINE} steps
 */
function validateOnlyDependencies(steps) {
  const selecmed = new Set(steps.map((s) => s.key));
  const errors = [];
  for (const step of steps) {
    const missing = (step.dependsOn || []).filter((dep) => !selecmed.has(dep));
    if (missing.length > 0) {
      errors.push(
        `  · ${step.key} 依赖 [${missing.join(', ')}]，--only 须包含前置步骤，或改用 --from ${missing[0]}`
      );
    }
  }
  if (errors.length > 0) {
    throw new Error(`依赖校验失败（执行顺序: ${ORDERED_KEYS.join(' → ')}）\n${errors.join('\n')}`);
  }
}

/**
 * 启动前打印权威顺序与本次将执行的步骤
 * @param {typeof PIPELINE} steps
 */
function printPipelinePlan(steps) {
  console.log('权威顺序（不可打乱）:');
  for (const step of PIPELINE) {
    const mark = steps.some((s) => s.key === step.key) ? '▶' : ' ';
    const deps = step.dependsOn.length ? ` ← 依赖 ${step.dependsOn.join(', ')}` : '';
    console.log(`  ${mark} ${step.order}. ${step.key}  ${step.label}${deps}`);
    console.log(`       ${step.reason}`);
  }
  console.log('');
  console.log(`本次执行 ${steps.length} 步: ${steps.map((s) => s.key).join(' → ')}`);
}

function printUsage() {
  console.log(`
用法:
  node scripts/sync/sync-all.cjs
  node scripts/sync/sync-all.cjs --only matplt,so,st
  node scripts/sync/sync-all.cjs --from so

参数:
  --only <keys>   仅执行指定步骤（逗号分隔，自动按权威顺序排序；须满足 dependsOn）
  --from <key>    从指定步骤起执行至末尾（含该步）
  -h, --help      显示帮助

权威顺序（固定）:
  1. matplt  工厂物料     PP_SapMaterial → material_plant
  2. mdl     机种目的地   PP_SapModelDest → model_destination（依赖 matplt）
  3. so      生产工单     PP_SapOrders + Manhour/Serial 回填（依赖 matplt）
  4. st      标准工时     PP_SapManhour → standard_operation_time（依赖 matplt, so）
  5. ec      工程变更     PP_SapEcn/EcnSub → source_ec（依赖 matplt）

说明:
  - 全量同步必须按 1→5 顺序；--only 子集须包含各步 dependsOn
  - 任一步失败立即中止，不执行后续步骤

示例:
  node scripts/sync/sync-all.cjs
  node scripts/sync/sync-all.cjs --only matplt,mdl,so
  node scripts/sync/sync-all.cjs --from st
`);
}

function main() {
  const cli = parseCliArgs();
  if (cli.help) {
    printUsage();
    return;
  }
  let steps;
  try {
    steps = resolvePipeline(PIPELINE, cli);
    if (cli.onlyKeys) {
      validateOnlyDependencies(steps);
    }
  } catch (err) {
    console.error(err.message || err);
    printUsage();
    process.exit(1);
    return;
  }
  if (steps.length === 0) {
    console.error('没有可执行的同步步骤。');
    process.exit(1);
    return;
  }
  const startedAt = Date.now();
  console.log('==========================================');
  console.log('  Takt 源数据全量同步（sync-all）');
  console.log('==========================================');
  printPipelinePlan(steps);
  console.log('==========================================');
  const results = [];
  for (let i = 0; i < steps.length; i++) {
    const step = steps[i];
    console.log('');
    console.log(`>>> [${i + 1}/${steps.length}] #${step.order} ${step.key}: ${step.label}`);
    console.log(`>>> 脚本: scripts/sync/${step.script}`);
    const stepStart = Date.now();
    const exitCode = runChildScript(step.script);
    const elapsedSec = Math.round((Date.now() - stepStart) / 1000);
    results.push({ key: step.key, label: step.label, exitCode, elapsedSec });
    if (exitCode !== 0) {
      console.error('');
      console.error(`✗ 步骤 ${step.key} 失败，退出码 ${exitCode}，已中止后续同步。`);
      printSummary(results, startedAt, false);
      process.exit(exitCode);
      return;
    }
    console.log(`✓ 步骤 ${step.key} 完成（${elapsedSec}s）`);
  }
  printSummary(results, startedAt, true);
}

/**
 * 打印执行汇总
 * @param {Array<{ key: string, label: string, exitCode: number, elapsedSec: number }>} results
 * @param {number} startedAt
 * @param {boolean} allOk
 */
function printSummary(results, startedAt, allOk) {
  const totalSec = Math.round((Date.now() - startedAt) / 1000);
  console.log('');
  console.log('==========================================');
  console.log(allOk ? '  全部同步完成' : '  同步中止');
  console.log('------------------------------------------');
  for (const r of results) {
    const status = r.exitCode === 0 ? 'OK' : `FAIL(${r.exitCode})`;
    console.log(`  [${status}] ${r.key} - ${r.label} (${r.elapsedSec}s)`);
  }
  console.log('------------------------------------------');
  console.log(`  总耗时: ${totalSec}s`);
  console.log('==========================================');
}

main();
