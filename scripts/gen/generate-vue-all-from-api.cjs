// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：scripts
// 文件名称：generate-vue-all-from-api.cjs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：一键串联 Vue 三模板生成（CRUD + TREE + Master-Detail）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const path = require('path');
const { spawnSync } = require('child_process');
const { logGeneratedFileWritePolicy, buildSingleEntityChildArgs, REPO_ROOT, SCRIPTS_GEN_DIR } = require('./generate-script-common.cjs');
const { parseVueCliArgs } = require('./generate-vue-common.cjs');

const SCRIPTS_DIR = SCRIPTS_GEN_DIR;

const VUE_PIPELINE = [
  { script: 'generate-vue-crud-from-api.cjs', label: '单表 CRUD' },
  { script: 'generate-vue-tree-from-api.cjs', label: '树表 TREE' },
  { script: 'generate-vue-master-detail-from-api.cjs', label: '主子表 Master-Detail' },
];

/**
 * @param {string} scriptName
 * @param {string[]} args
 * @returns {number}
 */
function runChildScript(scriptName, args) {
  const scriptPath = path.join(SCRIPTS_DIR, scriptName);
  const result = spawnSync(process.execPath, [scriptPath, ...args], {
    cwd: REPO_ROOT,
    stdio: 'inherit',
    env: process.env,
  });
  if (result.error) {
    throw result.error;
  }
  return result.status ?? 1;
}


function printAllUsage() {
  console.log(`
用法: node scripts/generate-vue-all-from-api.cjs [参数]

按顺序执行三模板 Vue 生成（各脚本仅处理匹配模板，其余自动跳过）:
  1. generate-vue-crud-from-api.cjs     单表 CRUD
  2. generate-vue-tree-from-api.cjs     树表 TREE（仅 tree API，禁止 list 探测）
  3. generate-vue-master-detail-from-api.cjs  主子表 Master-Detail

参数:
  --<实体名>            如 --Plant、--CostCenter、--DictType
  --view-path <路径>    覆盖 views 输出目录
  --dry-run             仅预览

说明:
  - 已禁用 --all；每次必须指定一个实体

示例:
  node scripts/generate-vue-all-from-api.cjs --CostCenter
`);
}

if (require.main === module) {
  console.log('🚀 开始一键生成 Vue 视图（CRUD + TREE + Master-Detail）...\n');
  logGeneratedFileWritePolicy();
  const options = parseVueCliArgs(printAllUsage);
  const childArgs = buildSingleEntityChildArgs(options);
  let exitCode = 0;
  VUE_PIPELINE.forEach((step, index) => {
    console.log(`\n── 步骤 ${index + 1}/${VUE_PIPELINE.length}: ${step.label} (${step.script}) ──\n`);
    const code = runChildScript(step.script, childArgs);
    if (code !== 0) {
      exitCode = code;
    }
  });
  if (exitCode !== 0) {
    process.exit(exitCode);
  }
  console.log('\n✨ Vue 三模板流水线完成');
}

module.exports = {
  VUE_PIPELINE,
  runChildScript,
  buildSingleEntityChildArgs,
};
