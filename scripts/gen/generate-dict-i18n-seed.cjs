// ========================================
// 项目名称：节拍智控·TaktWise
// 脚本名称：generate-dict-i18n-seed.cjs
// 功能描述：从 TaktDictDataSeedData.cs 全量生成 TaktDictI18nSeedData.cs（dict.* 翻译键）
// TranslationText：DictLabel + 语言后缀（en-US/_us、ja-JP/_jp、zh-CN/_cn、zh-HK/_hk）
// 用法: node scripts/generate-dict-i18n-seed.cjs [--all|-all]
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
  node scripts/generate-dict-i18n-seed.cjs [--all|-all]

说明:
  - 固定从 TaktDictDataSeedData.cs 全量生成 dict.* 翻译种子
  - 仅支持无参或 --all / -all，不支持其他参数

示例:
  node scripts/generate-dict-i18n-seed.cjs
  node scripts/generate-dict-i18n-seed.cjs --all
`);
}

parseAllOnlyGenerateArgs(printUsage);
console.log('🚀 全量生成字典 i18n 种子（--all）...\n');

const root = path.join(__dirname, '../..');
const dictSeedPath = path.join(
  root,
  'backend/src/Takt.Infrastructure/Data/Seeds/EntitySeedData/TaktDictDataSeedData.cs',
);
const outPath = path.join(
  root,
  'backend/src/Takt.Infrastructure/Data/Seeds/I18nSeedData/TaktDictI18nSeedData.cs',
);

const content = fs.readFileSync(dictSeedPath, 'utf8');
// 元组：DictTypeCode, DictLabel, DictValue, I18nKey, ListClass, CssClass, IsDefault, Remark, SortOrder, CultureCode[, ExtLabel, ExtValue]
// 仅匹配至 CultureCode；扩展字段可选，不参与 i18n 解析
const lineRegex =
  /\("([^"]*)","([^"]*)","([^"]*)","([^"]*)",\d+,\d+,\d+,"([^"]*)",\d+,"[^"]*"/g;

const items = [];
let match;
while ((match = lineRegex.exec(content)) !== null) {
  const i18nKey = match[4];
  if (!i18nKey.startsWith('dict.')) {
    continue;
  }
  items.push({
    dictLabel: match[2],
    dictValue: match[3],
    i18nKey,
    remark: match[5],
  });
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
  console.error('No dict entries parsed from', dictSeedPath);
  process.exit(1);
}

const cultures = ['en-US', 'ja-JP', 'zh-CN', 'zh-HK'];

/** 各语言 TranslationText 后缀（与 DictLabel 基准文案拼接） */
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
 * 为 TranslationText 追加语言后缀（如 低 → 低_us）
 * @param {string} text 基准文案
 * @param {string} culture BCP47
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

/**
 * DictLabel 为基准；sys_culture_code / sys_country_code 的 TranslationText 与 DictLabel 相同
 * （本族语/本国语言全球统一展示，不做 UI 语言后缀）
 * @param {{ dictLabel: string, i18nKey: string }} item
 * @param {string} culture
 * @returns {string}
 */
function resolveTranslationText(item, culture) {
  if (
    item.i18nKey.startsWith('dict.sys.culture.code.')
    || item.i18nKey.startsWith('dict.sys.country.code.')
  ) {
    return (item.dictLabel || '').trim();
  }
  return withCultureTranslationSuffix(item.dictLabel, culture);
}

const today = new Date().toISOString().slice(0, 10);
const dictResourceGroup = 'Foundation';
const dictResourceType = TRANSLATION_RESOURCE_TYPE_FRONTEND;
const lines = [];

lines.push('// ========================================');
lines.push('// 项目名称：节拍智控·TaktWise');
lines.push('// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData');
lines.push('// 文件名称：TaktDictI18nSeedData.cs');
lines.push(`// 创建时间：${today}`);
lines.push('// 创建人：Takt365(Cursor AI)');
lines.push('// 功能描述：字典项国际化翻译种子（dict.* 键，与 TaktDictDataSeedData I18nKey 对齐）');
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
lines.push('/// 字典项国际化翻译种子（键前缀 dict.*）');
lines.push('/// 幂等性：存在则更新，不存在则创建');
lines.push('/// TranslationText 为字典显示标签；ContextNote 为 TaktDictDataSeedData.Remark');
lines.push('/// 部门类字典项 I18nKey 复用 org.dept.*，由 TaktDeptI18nSeedData 提供翻译，不在此重复');
lines.push('/// </summary>');
lines.push('public class TaktDictI18nSeedData : ITaktSeedDataCoordinator');
lines.push('{');
lines.push('    /// <summary>');
lines.push('    /// 执行顺序（在通用翻译之后、部门翻译之前）');
lines.push('    /// </summary>');
lines.push('    public int Order => 50;');
lines.push('');
lines.push('    /// <summary>');
lines.push('    /// 初始化字典项国际化翻译种子');
lines.push('    /// </summary>');
lines.push('    /// <param name="serviceProvider">服务提供者</param>');
lines.push('    /// <param name="tenantCode">租户编码</param>');
lines.push('    /// <returns>插入数与更新数</returns>');
lines.push(
  '    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)',
);
lines.push('    {');
lines.push('        TaktLogger.Information("开始初始化字典项国际化翻译种子...");');
lines.push('');
lines.push('        if (string.IsNullOrEmpty(tenantCode))');
lines.push('        {');
lines.push('            TaktLogger.Warning("租户编码为空，跳过字典项国际化翻译种子");');
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
lines.push('        TaktLogger.Information("正在为租户 {TenantCode} 初始化 dict.* 翻译...", tenantCode);');
lines.push('');
lines.push('        foreach (var row in GetDictTranslations())');
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
  '        TaktLogger.Information("字典项国际化翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);',
);
lines.push('');
lines.push('        return (insertCount, updateCount);');
lines.push('    }');
lines.push('');
lines.push('    /// <summary>');
lines.push('    /// 字典项翻译列表（en-US / ja-JP / zh-CN / zh-HK）');
lines.push('    /// </summary>');
lines.push(
  '    private static List<(string I18nKey, string CultureCode, string TranslationText, string? ContextNote)> GetDictTranslations()',
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
    const text = resolveTranslationText(item, culture);
    lines.push(`            // ${item.i18nKey}`);
    lines.push(
      `            ("${esc(item.i18nKey)}", "${culture}", "${esc(text)}", "${esc(item.remark)}"),`,
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
lines.push(`        translation.ResourceGroup = "${dictResourceGroup}";`);
lines.push(`        translation.ResourceType = "${dictResourceType}";`);
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
