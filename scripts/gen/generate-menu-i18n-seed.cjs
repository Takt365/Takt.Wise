// ========================================
// 项目名称：节拍智控·TaktWise
// 脚本名称：generate-menu-i18n-seed.cjs
// 功能描述：从 TaktMenuLevel1~4SeedData.cs 全量生成 TaktMenuI18nSeedData.cs（menu.* 翻译键）
// TranslationText：MenuName + 语言后缀（en-US/_us、ja-JP/_jp、zh-CN 无后缀、zh-HK/_hk）
// 用法: node scripts/generate-menu-i18n-seed.cjs [--all|-all]
// ========================================

const fs = require('fs');
const path = require('path');
const {
  parseAllOnlyGenerateArgs,
  TRANSLATION_RESOURCE_TYPE_FRONTEND,
} = require('./generate-script-common.cjs');

function printUsage() {
  console.log(`
用法:
  node scripts/generate-menu-i18n-seed.cjs [--all|-all]

说明:
  - 固定从 TaktMenuLevel1~4SeedData.cs 全量生成 menu.* 导航翻译种子
  - 仅支持无参或 --all / -all，不支持其他参数

示例:
  node scripts/generate-menu-i18n-seed.cjs
  node scripts/generate-menu-i18n-seed.cjs --all
`);
}

parseAllOnlyGenerateArgs(printUsage);
console.log('🚀 全量生成菜单 i18n 种子（--all）...\n');

const root = path.join(__dirname, '../..');
const menuSeedFiles = [
  'TaktMenuLevel1SeedData.cs',
  'TaktMenuLevel2SeedData.cs',
  'TaktMenuLevel3SeedData.cs',
  'TaktMenuLevel4SeedData.cs',
].map((name) =>
  path.join(root, 'backend/src/Takt.Infrastructure/Data/Seeds/EntitySeedData', name),
);

const outPath = path.join(
  root,
  'backend/src/Takt.Infrastructure/Data/Seeds/I18nSeedData/TaktMenuI18nSeedData.cs',
);

const pairRegex =
  /menu\.MenuName\s*=\s*"([^"]+)";\s*\r?\n\s*menu\.MenuCode[^;]+;\s*\r?\n\s*menu\.I18nKey\s*=\s*"([^"]+)"/g;

/** @type {Array<{ menuName: string, i18nKey: string }>} */
const items = [];

for (const filePath of menuSeedFiles) {
  if (!fs.existsSync(filePath)) {
    console.error('Menu seed file not found:', filePath);
    process.exit(1);
  }
  const content = fs.readFileSync(filePath, 'utf8');
  let match;
  while ((match = pairRegex.exec(content)) !== null) {
    const menuName = match[1];
    const i18nKey = match[2];
    if (!i18nKey.startsWith('menu.')) {
      continue;
    }
    items.push({ menuName, i18nKey });
  }
}

const seen = new Set();
const unique = items.filter((item) => {
  if (seen.has(item.i18nKey)) {
    return false;
  }
  seen.add(item.i18nKey);
  return true;
});

if (unique.length === 0) {
  console.error('No menu.* entries parsed from menu level seed files');
  process.exit(1);
}

const cultures = ['en-US', 'ja-JP', 'zh-CN', 'zh-HK'];

/** 各语言 TranslationText 后缀（与 MenuName 基准文案拼接） */
const CULTURE_TRANSLATION_SUFFIX = {
  'en-US': '_us',
  'ja-JP': '_jp',
  'zh-CN': '',
  'zh-HK': '_hk',
};

/** @param {string} s */
function esc(s) {
  return (s || '').replace(/\\/g, '\\\\').replace(/"/g, '\\"');
}

/**
 * @param {string} text
 * @param {string} culture
 * @returns {string}
 */
function withCultureTranslationSuffix(text, culture) {
  const base = (text || '').trim();
  if (!base) {
    return base;
  }
  const suffix = CULTURE_TRANSLATION_SUFFIX[culture];
  if (!suffix || base.endsWith(suffix)) {
    return base;
  }
  return `${base}${suffix}`;
}

const today = new Date().toISOString().slice(0, 10);
const menuResourceGroup = 'Foundation';
const menuResourceType = TRANSLATION_RESOURCE_TYPE_FRONTEND;
const lines = [];

lines.push('// ========================================');
lines.push('// 项目名称：节拍智控·TaktWise');
lines.push('// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData');
lines.push('// 文件名称：TaktMenuI18nSeedData.cs');
lines.push(`// 创建时间：${today}`);
lines.push('// 创建人：Takt365(Cursor AI)');
lines.push('// 功能描述：菜单导航国际化翻译种子（menu.* 键，与 TaktMenuLevel1~4SeedData I18nKey 对齐）');
lines.push('// ');
lines.push('// 版权信息：Copyright (c) 2025 Takt  All rights reserved.');
lines.push('// 免责声明：此软件使用 MIT License，作者不承担任何使用风险.');
lines.push('// ========================================');
lines.push('');
lines.push('using System.Linq;');
lines.push('using Microsoft.Extensions.DependencyInjection;');
lines.push('using Takt.Domain.Entities.Foundation;');
lines.push('using Takt.Domain.Interfaces;');
lines.push('using Takt.Domain.Repositories;');
lines.push('using Takt.Shared.Helpers;');
lines.push('');
lines.push('namespace Takt.Infrastructure.Data.Seeds.I18nSeedData;');
lines.push('');
lines.push('/// <summary>');
lines.push('/// 菜单导航国际化翻译种子（键前缀 menu.*，与菜单种子 I18nKey 一致）');
lines.push('/// 幂等性：存在则更新，不存在则创建');
lines.push('/// TranslationText 为菜单显示名 MenuName；由 generate-menu-i18n-seed.cjs 从 Level1~4 种子生成');
lines.push('/// 与 I18nSeedData.Identity.TaktMenuI18nSeedData（entity.menu.* 实体字段）职责不同');
lines.push('/// </summary>');
lines.push('public class TaktMenuI18nSeedData : ITaktSeedDataCoordinator');
lines.push('{');
lines.push('    /// <summary>');
lines.push('    /// 执行顺序（在问候语翻译之前，菜单种子落库之后）');
lines.push('    /// </summary>');
lines.push('    public int Order => 47;');
lines.push('');
lines.push('    /// <summary>');
lines.push('    /// 初始化菜单导航国际化翻译种子');
lines.push('    /// </summary>');
lines.push('    /// <param name="serviceProvider">服务提供者</param>');
lines.push('    /// <param name="tenantCode">租户编码</param>');
lines.push('    /// <returns>插入数与更新数</returns>');
lines.push(
  '    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)',
);
lines.push('    {');
lines.push('        TaktLogger.Information("开始初始化菜单导航国际化翻译种子...");');
lines.push('');
lines.push('        if (string.IsNullOrEmpty(tenantCode))');
lines.push('        {');
lines.push('            TaktLogger.Warning("租户编码为空，跳过菜单导航国际化翻译种子");');
lines.push('            return (0, 0);');
lines.push('        }');
lines.push('');
lines.push(
  '        var repository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktTranslation>>();',
);
lines.push(
  '        var cultureRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCulture>>();',
);
lines.push(
  '        var cultureIdByCode = (await cultureRepository.GetListAsync(c => c.TenantCode == tenantCode))',
);
lines.push('            .ToDictionary(c => c.CultureCode, c => c.Id);');
lines.push('        int insertCount = 0;');
lines.push('        int updateCount = 0;');
lines.push('');
lines.push('        TaktLogger.Information("正在为租户 {TenantCode} 初始化 menu.* 翻译...", tenantCode);');
lines.push('');
lines.push('        foreach (var row in GetMenuTranslations())');
lines.push('        {');
lines.push('            if (!cultureIdByCode.TryGetValue(row.CultureCode, out var cultureId))');
lines.push('            {');
lines.push(
  '                TaktLogger.Warning("未找到区域文化 {CultureCode}，跳过翻译 {I18nKey}", row.CultureCode, row.I18nKey);',
);
lines.push('                continue;');
lines.push('            }');
lines.push('');
lines.push(
  '            var item = new TranslationSeedItem(row.I18nKey, row.CultureCode, row.TranslationText, row.ContextNote);',
);
lines.push(
  '            var (_, i, u) = await CreateOrUpdateTranslationAsync(repository, tenantCode, cultureId, item);',
);
lines.push('            insertCount += i;');
lines.push('            updateCount += u;');
lines.push('        }');
lines.push('');
lines.push(
  '        TaktLogger.Information("菜单导航国际化翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);',
);
lines.push('');
lines.push('        return (insertCount, updateCount);');
lines.push('    }');
lines.push('');
lines.push('    /// <summary>');
lines.push('    /// 菜单导航翻译列表（en-US / ja-JP / zh-CN / zh-HK）');
lines.push('    /// </summary>');
lines.push(
  '    private static List<(string I18nKey, string CultureCode, string TranslationText, string? ContextNote)> GetMenuTranslations()',
);
lines.push('    {');
lines.push('        return new List<(string, string, string, string?)>');
lines.push('        {');

let lastKey = '';
for (const item of unique) {
  if (lastKey && lastKey !== item.i18nKey) {
    lines.push('');
  }
  for (const culture of cultures) {
    const text = withCultureTranslationSuffix(item.menuName, culture);
    lines.push(`            // ${item.i18nKey}`);
    lines.push(
      `            ("${esc(item.i18nKey)}", "${culture}", "${esc(text)}", "菜单导航"),`,
    );
  }
  lastKey = item.i18nKey;
}

lines.push('        };');
lines.push('    }');
lines.push('');
lines.push('    /// <summary>');
lines.push('    /// 填充 TaktTranslation 全部业务字段（含租户基类字段）');
lines.push('    /// </summary>');
lines.push('    private static void ApplyTranslationFields(');
lines.push('        TaktTranslation translation,');
lines.push('        string tenantCode,');
lines.push('        long cultureId,');
lines.push('        TranslationSeedItem item)');
lines.push('    {');
lines.push('        translation.TenantCode = tenantCode;');
lines.push('        translation.CultureId = cultureId;');
lines.push('        translation.CultureCode = item.CultureCode;');
lines.push('        translation.I18nKey = item.I18nKey;');
lines.push('        translation.TranslationText = item.TranslationText;');
lines.push(`        translation.ResourceGroup = "${menuResourceGroup}";`);
lines.push(`        translation.ResourceType = "${menuResourceType}";`);
lines.push('        translation.ContextNote = item.ContextNote;');
lines.push('        translation.ExtField = null;');
lines.push('        translation.Remark = null;');
lines.push('        translation.IsDeleted = 0;');
lines.push('        translation.DeletedBy = null;');
lines.push('        translation.DeletedAt = null;');
lines.push('    }');
lines.push('');
lines.push(
  '    private static async Task<(TaktTranslation Translation, int InsertCount, int UpdateCount)> CreateOrUpdateTranslationAsync(',
);
lines.push('        ITaktTenantSeedRepository<TaktTranslation> repository,');
lines.push('        string tenantCode,');
lines.push('        long cultureId,');
lines.push('        TranslationSeedItem item)');
lines.push('    {');
lines.push('        var translation = await repository.FirstAsync(t =>');
lines.push('            t.TenantCode == tenantCode &&');
lines.push('            t.I18nKey == item.I18nKey &&');
lines.push('            t.CultureCode == item.CultureCode);');
lines.push('');
lines.push('        if (translation == null)');
lines.push('        {');
lines.push('            translation = new TaktTranslation();');
lines.push('            ApplyTranslationFields(translation, tenantCode, cultureId, item);');
lines.push('            translation = await repository.CreateAsync(translation);');
lines.push('            return (translation, 1, 0);');
lines.push('        }');
lines.push('');
lines.push('        ApplyTranslationFields(translation, tenantCode, cultureId, item);');
lines.push('        await repository.UpdateAsync(translation);');
lines.push('        return (translation, 0, 1);');
lines.push('    }');
lines.push('');
lines.push('    /// <summary>');
lines.push('    /// 翻译种子项（CultureId 由 SeedAsync 解析）');
lines.push('    /// </summary>');
lines.push('    private sealed record TranslationSeedItem(');
lines.push('        string I18nKey,');
lines.push('        string CultureCode,');
lines.push('        string TranslationText,');
lines.push('        string? ContextNote);');
lines.push('}');
lines.push('');

fs.writeFileSync(outPath, lines.join('\n'), 'utf8');
console.log(
  `Generated ${unique.length} keys, ${unique.length * cultures.length} rows -> ${outPath}`,
);
