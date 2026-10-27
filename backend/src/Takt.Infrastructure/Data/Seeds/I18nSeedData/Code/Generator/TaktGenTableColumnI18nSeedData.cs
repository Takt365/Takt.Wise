// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Code.Generator
// 文件名称：TaktGenTableColumnI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktGenTableColumn 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Code.Generator;

/// <summary>
/// TaktGenTableColumn 实体国际化翻译种子（键前缀 entity.gentablecolumn.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktGenTableColumnI18nSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（实体翻译种子，位于部门翻译之后）
    /// </summary>
    public int Order => 52;

    /// <summary>
    /// 初始化实体字段翻译种子
    /// </summary>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化 TaktGenTableColumn 实体国际化翻译种子...");

        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过实体国际化翻译种子初始化");
            return (0, 0);
        }

        var repository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktTranslation>>();
        var cultureRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCulture>>();
        var cultureIdByCode = (await cultureRepository.GetListAsync(c => c.TenantCode == tenantCode))
            .ToDictionary(c => c.CultureCode, c => c.Id);
        int insertCount = 0;
        int updateCount = 0;

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 gentablecolumn 实体翻译...", tenantCode);

        foreach (var item in GetGenTableColumnTranslations())
        {
            if (!cultureIdByCode.TryGetValue(item.CultureCode, out var cultureId))
            {
                TaktLogger.Warning("未找到区域文化 {CultureCode}，跳过翻译 {I18nKey}", item.CultureCode, item.I18nKey);
                continue;
            }

            var (translation, i, u) = await CreateOrUpdateTranslationAsync(
                repository,
                tenantCode,
                cultureId,
                item);
            insertCount += i;
            updateCount += u;
        }

        TaktLogger.Information("TaktGenTableColumn 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktGenTableColumn 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.gentablecolumn._self / entity.gentablecolumn.{{field}}；ResourceGroup=Generator；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetGenTableColumnTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.gentablecolumn._self
            new TranslationSeedItem("entity.gentablecolumn._self", "en-US", "Gen Table Column Information_us", "实体名称"),
            // entity.gentablecolumn._self
            new TranslationSeedItem("entity.gentablecolumn._self", "ja-JP", "Takt代码生成字段配置信息_jp", "实体名称"),
            // entity.gentablecolumn._self
            new TranslationSeedItem("entity.gentablecolumn._self", "zh-CN", "Takt代码生成字段配置信息", "实体名称"),
            // entity.gentablecolumn._self
            new TranslationSeedItem("entity.gentablecolumn._self", "zh-HK", "Takt代码生成字段配置信息_hk", "实体名称"),

            // entity.gentablecolumn.gentableid
            new TranslationSeedItem("entity.gentablecolumn.gentableid", "en-US", "生成表ID_us", "生成表ID（关联代码生成表配置，序列化为string以避免Javascript精度问题）"),
            // entity.gentablecolumn.gentableid
            new TranslationSeedItem("entity.gentablecolumn.gentableid", "ja-JP", "生成表ID_jp", "生成表ID（关联代码生成表配置，序列化为string以避免Javascript精度问题）"),
            // entity.gentablecolumn.gentableid
            new TranslationSeedItem("entity.gentablecolumn.gentableid", "zh-CN", "生成表ID", "生成表ID（关联代码生成表配置，序列化为string以避免Javascript精度问题）"),
            // entity.gentablecolumn.gentableid
            new TranslationSeedItem("entity.gentablecolumn.gentableid", "zh-HK", "生成表ID_hk", "生成表ID（关联代码生成表配置，序列化为string以避免Javascript精度问题）"),

            // entity.gentablecolumn.linenumber
            new TranslationSeedItem("entity.gentablecolumn.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.gentablecolumn.linenumber
            new TranslationSeedItem("entity.gentablecolumn.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.gentablecolumn.linenumber
            new TranslationSeedItem("entity.gentablecolumn.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.gentablecolumn.linenumber
            new TranslationSeedItem("entity.gentablecolumn.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.gentablecolumn.databasecolumnname
            new TranslationSeedItem("entity.gentablecolumn.databasecolumnname", "en-US", "列名称_us", "数据库列名称（唯一索引：租户内生成表+列名唯一，见 ix_gen_table_column_column_unique；snake_case，如 column_name）"),
            // entity.gentablecolumn.databasecolumnname
            new TranslationSeedItem("entity.gentablecolumn.databasecolumnname", "ja-JP", "列名称_jp", "数据库列名称（唯一索引：租户内生成表+列名唯一，见 ix_gen_table_column_column_unique；snake_case，如 column_name）"),
            // entity.gentablecolumn.databasecolumnname
            new TranslationSeedItem("entity.gentablecolumn.databasecolumnname", "zh-CN", "列名称", "数据库列名称（唯一索引：租户内生成表+列名唯一，见 ix_gen_table_column_column_unique；snake_case，如 column_name）"),
            // entity.gentablecolumn.databasecolumnname
            new TranslationSeedItem("entity.gentablecolumn.databasecolumnname", "zh-HK", "列名称_hk", "数据库列名称（唯一索引：租户内生成表+列名唯一，见 ix_gen_table_column_column_unique；snake_case，如 column_name）"),

            // entity.gentablecolumn.columncomment
            new TranslationSeedItem("entity.gentablecolumn.columncomment", "en-US", "列描述_us", "列描述（字段注释）"),
            // entity.gentablecolumn.columncomment
            new TranslationSeedItem("entity.gentablecolumn.columncomment", "ja-JP", "列描述_jp", "列描述（字段注释）"),
            // entity.gentablecolumn.columncomment
            new TranslationSeedItem("entity.gentablecolumn.columncomment", "zh-CN", "列描述", "列描述（字段注释）"),
            // entity.gentablecolumn.columncomment
            new TranslationSeedItem("entity.gentablecolumn.columncomment", "zh-HK", "列描述_hk", "列描述（字段注释）"),

            // entity.gentablecolumn.databasedatatype
            new TranslationSeedItem("entity.gentablecolumn.databasedatatype", "en-US", "数据类型_us", "数据类型（字典 sys_db_data_type；nvarchar/varchar/int/datetime/decimal 等）"),
            // entity.gentablecolumn.databasedatatype
            new TranslationSeedItem("entity.gentablecolumn.databasedatatype", "ja-JP", "数据类型_jp", "数据类型（字典 sys_db_data_type；nvarchar/varchar/int/datetime/decimal 等）"),
            // entity.gentablecolumn.databasedatatype
            new TranslationSeedItem("entity.gentablecolumn.databasedatatype", "zh-CN", "数据类型", "数据类型（字典 sys_db_data_type；nvarchar/varchar/int/datetime/decimal 等）"),
            // entity.gentablecolumn.databasedatatype
            new TranslationSeedItem("entity.gentablecolumn.databasedatatype", "zh-HK", "数据类型_hk", "数据类型（字典 sys_db_data_type；nvarchar/varchar/int/datetime/decimal 等）"),

            // entity.gentablecolumn.csharpdatatype
            new TranslationSeedItem("entity.gentablecolumn.csharpdatatype", "en-US", "C#类型_us", "C#类型（字典 code_generator_csharp_data_type；string/int/long/datetime/decimal/bool/guid 等）"),
            // entity.gentablecolumn.csharpdatatype
            new TranslationSeedItem("entity.gentablecolumn.csharpdatatype", "ja-JP", "C#类型_jp", "C#类型（字典 code_generator_csharp_data_type；string/int/long/datetime/decimal/bool/guid 等）"),
            // entity.gentablecolumn.csharpdatatype
            new TranslationSeedItem("entity.gentablecolumn.csharpdatatype", "zh-CN", "C#类型", "C#类型（字典 code_generator_csharp_data_type；string/int/long/datetime/decimal/bool/guid 等）"),
            // entity.gentablecolumn.csharpdatatype
            new TranslationSeedItem("entity.gentablecolumn.csharpdatatype", "zh-HK", "C#类型_hk", "C#类型（字典 code_generator_csharp_data_type；string/int/long/datetime/decimal/bool/guid 等）"),

            // entity.gentablecolumn.csharpcolumnname
            new TranslationSeedItem("entity.gentablecolumn.csharpcolumnname", "en-US", "C#列名_us", "C#列名（C#属性名，首字母大写，帕斯卡命名法）"),
            // entity.gentablecolumn.csharpcolumnname
            new TranslationSeedItem("entity.gentablecolumn.csharpcolumnname", "ja-JP", "C#列名_jp", "C#列名（C#属性名，首字母大写，帕斯卡命名法）"),
            // entity.gentablecolumn.csharpcolumnname
            new TranslationSeedItem("entity.gentablecolumn.csharpcolumnname", "zh-CN", "C#列名", "C#列名（C#属性名，首字母大写，帕斯卡命名法）"),
            // entity.gentablecolumn.csharpcolumnname
            new TranslationSeedItem("entity.gentablecolumn.csharpcolumnname", "zh-HK", "C#列名_hk", "C#列名（C#属性名，首字母大写，帕斯卡命名法）"),

            // entity.gentablecolumn.length
            new TranslationSeedItem("entity.gentablecolumn.length", "en-US", "长度_us", "C#长度（字符串长度、数值类型的整数位数）"),
            // entity.gentablecolumn.length
            new TranslationSeedItem("entity.gentablecolumn.length", "ja-JP", "长度_jp", "C#长度（字符串长度、数值类型的整数位数）"),
            // entity.gentablecolumn.length
            new TranslationSeedItem("entity.gentablecolumn.length", "zh-CN", "长度", "C#长度（字符串长度、数值类型的整数位数）"),
            // entity.gentablecolumn.length
            new TranslationSeedItem("entity.gentablecolumn.length", "zh-HK", "长度_hk", "C#长度（字符串长度、数值类型的整数位数）"),

            // entity.gentablecolumn.decimaldigits
            new TranslationSeedItem("entity.gentablecolumn.decimaldigits", "en-US", "精度_us", "C#小数位数（decimal等数值类型的小数位数）"),
            // entity.gentablecolumn.decimaldigits
            new TranslationSeedItem("entity.gentablecolumn.decimaldigits", "ja-JP", "精度_jp", "C#小数位数（decimal等数值类型的小数位数）"),
            // entity.gentablecolumn.decimaldigits
            new TranslationSeedItem("entity.gentablecolumn.decimaldigits", "zh-CN", "精度", "C#小数位数（decimal等数值类型的小数位数）"),
            // entity.gentablecolumn.decimaldigits
            new TranslationSeedItem("entity.gentablecolumn.decimaldigits", "zh-HK", "精度_hk", "C#小数位数（decimal等数值类型的小数位数）"),

            // entity.gentablecolumn.ispk
            new TranslationSeedItem("entity.gentablecolumn.ispk", "en-US", "主键_us", "主键（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentablecolumn.ispk
            new TranslationSeedItem("entity.gentablecolumn.ispk", "ja-JP", "主键_jp", "主键（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentablecolumn.ispk
            new TranslationSeedItem("entity.gentablecolumn.ispk", "zh-CN", "主键", "主键（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentablecolumn.ispk
            new TranslationSeedItem("entity.gentablecolumn.ispk", "zh-HK", "主键_hk", "主键（字典 sys_yes_no；0=否 1=是）"),

            // entity.gentablecolumn.isincrement
            new TranslationSeedItem("entity.gentablecolumn.isincrement", "en-US", "自增_us", "自增（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentablecolumn.isincrement
            new TranslationSeedItem("entity.gentablecolumn.isincrement", "ja-JP", "自增_jp", "自增（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentablecolumn.isincrement
            new TranslationSeedItem("entity.gentablecolumn.isincrement", "zh-CN", "自增", "自增（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentablecolumn.isincrement
            new TranslationSeedItem("entity.gentablecolumn.isincrement", "zh-HK", "自增_hk", "自增（字典 sys_yes_no；0=否 1=是）"),

            // entity.gentablecolumn.isrequired
            new TranslationSeedItem("entity.gentablecolumn.isrequired", "en-US", "必填_us", "必填（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentablecolumn.isrequired
            new TranslationSeedItem("entity.gentablecolumn.isrequired", "ja-JP", "必填_jp", "必填（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentablecolumn.isrequired
            new TranslationSeedItem("entity.gentablecolumn.isrequired", "zh-CN", "必填", "必填（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentablecolumn.isrequired
            new TranslationSeedItem("entity.gentablecolumn.isrequired", "zh-HK", "必填_hk", "必填（字典 sys_yes_no；0=否 1=是）"),

            // entity.gentablecolumn.iscreate
            new TranslationSeedItem("entity.gentablecolumn.iscreate", "en-US", "新增_us", "新增（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentablecolumn.iscreate
            new TranslationSeedItem("entity.gentablecolumn.iscreate", "ja-JP", "新增_jp", "新增（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentablecolumn.iscreate
            new TranslationSeedItem("entity.gentablecolumn.iscreate", "zh-CN", "新增", "新增（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentablecolumn.iscreate
            new TranslationSeedItem("entity.gentablecolumn.iscreate", "zh-HK", "新增_hk", "新增（字典 sys_yes_no；0=否 1=是）"),

            // entity.gentablecolumn.isupdate
            new TranslationSeedItem("entity.gentablecolumn.isupdate", "en-US", "更新_us", "更新（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentablecolumn.isupdate
            new TranslationSeedItem("entity.gentablecolumn.isupdate", "ja-JP", "更新_jp", "更新（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentablecolumn.isupdate
            new TranslationSeedItem("entity.gentablecolumn.isupdate", "zh-CN", "更新", "更新（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentablecolumn.isupdate
            new TranslationSeedItem("entity.gentablecolumn.isupdate", "zh-HK", "更新_hk", "更新（字典 sys_yes_no；0=否 1=是）"),

            // entity.gentablecolumn.isunique
            new TranslationSeedItem("entity.gentablecolumn.isunique", "en-US", "查重_us", "查重（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentablecolumn.isunique
            new TranslationSeedItem("entity.gentablecolumn.isunique", "ja-JP", "查重_jp", "查重（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentablecolumn.isunique
            new TranslationSeedItem("entity.gentablecolumn.isunique", "zh-CN", "查重", "查重（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentablecolumn.isunique
            new TranslationSeedItem("entity.gentablecolumn.isunique", "zh-HK", "查重_hk", "查重（字典 sys_yes_no；0=否 1=是）"),

            // entity.gentablecolumn.islist
            new TranslationSeedItem("entity.gentablecolumn.islist", "en-US", "列表_us", "列表（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentablecolumn.islist
            new TranslationSeedItem("entity.gentablecolumn.islist", "ja-JP", "列表_jp", "列表（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentablecolumn.islist
            new TranslationSeedItem("entity.gentablecolumn.islist", "zh-CN", "列表", "列表（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentablecolumn.islist
            new TranslationSeedItem("entity.gentablecolumn.islist", "zh-HK", "列表_hk", "列表（字典 sys_yes_no；0=否 1=是）"),

            // entity.gentablecolumn.isexport
            new TranslationSeedItem("entity.gentablecolumn.isexport", "en-US", "导出_us", "导出（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentablecolumn.isexport
            new TranslationSeedItem("entity.gentablecolumn.isexport", "ja-JP", "导出_jp", "导出（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentablecolumn.isexport
            new TranslationSeedItem("entity.gentablecolumn.isexport", "zh-CN", "导出", "导出（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentablecolumn.isexport
            new TranslationSeedItem("entity.gentablecolumn.isexport", "zh-HK", "导出_hk", "导出（字典 sys_yes_no；0=否 1=是）"),

            // entity.gentablecolumn.issort
            new TranslationSeedItem("entity.gentablecolumn.issort", "en-US", "排序_us", "可排序（字典 sys_yes_no；0=否 1=是）。控制前端表格列是否显示 sortable 排序图标，与 TaktGenTable.SortField/SortType（默认排序规则）互补。"),
            // entity.gentablecolumn.issort
            new TranslationSeedItem("entity.gentablecolumn.issort", "ja-JP", "排序_jp", "可排序（字典 sys_yes_no；0=否 1=是）。控制前端表格列是否显示 sortable 排序图标，与 TaktGenTable.SortField/SortType（默认排序规则）互补。"),
            // entity.gentablecolumn.issort
            new TranslationSeedItem("entity.gentablecolumn.issort", "zh-CN", "排序", "可排序（字典 sys_yes_no；0=否 1=是）。控制前端表格列是否显示 sortable 排序图标，与 TaktGenTable.SortField/SortType（默认排序规则）互补。"),
            // entity.gentablecolumn.issort
            new TranslationSeedItem("entity.gentablecolumn.issort", "zh-HK", "排序_hk", "可排序（字典 sys_yes_no；0=否 1=是）。控制前端表格列是否显示 sortable 排序图标，与 TaktGenTable.SortField/SortType（默认排序规则）互补。"),

            // entity.gentablecolumn.isquery
            new TranslationSeedItem("entity.gentablecolumn.isquery", "en-US", "查询_us", "查询（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentablecolumn.isquery
            new TranslationSeedItem("entity.gentablecolumn.isquery", "ja-JP", "查询_jp", "查询（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentablecolumn.isquery
            new TranslationSeedItem("entity.gentablecolumn.isquery", "zh-CN", "查询", "查询（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentablecolumn.isquery
            new TranslationSeedItem("entity.gentablecolumn.isquery", "zh-HK", "查询_hk", "查询（字典 sys_yes_no；0=否 1=是）"),

            // entity.gentablecolumn.querytype
            new TranslationSeedItem("entity.gentablecolumn.querytype", "en-US", "查询方式_us", "查询方式（字典 code_generator_query_type：eq/ne/gt/gte/lt/lte/like/between）。IsQuery=0 时必须为空串；IsQuery=1 时必填，字符串默认 like、其他类型默认 eq"),
            // entity.gentablecolumn.querytype
            new TranslationSeedItem("entity.gentablecolumn.querytype", "ja-JP", "查询方式_jp", "查询方式（字典 code_generator_query_type：eq/ne/gt/gte/lt/lte/like/between）。IsQuery=0 时必须为空串；IsQuery=1 时必填，字符串默认 like、其他类型默认 eq"),
            // entity.gentablecolumn.querytype
            new TranslationSeedItem("entity.gentablecolumn.querytype", "zh-CN", "查询方式", "查询方式（字典 code_generator_query_type：eq/ne/gt/gte/lt/lte/like/between）。IsQuery=0 时必须为空串；IsQuery=1 时必填，字符串默认 like、其他类型默认 eq"),
            // entity.gentablecolumn.querytype
            new TranslationSeedItem("entity.gentablecolumn.querytype", "zh-HK", "查询方式_hk", "查询方式（字典 code_generator_query_type：eq/ne/gt/gte/lt/lte/like/between）。IsQuery=0 时必须为空串；IsQuery=1 时必填，字符串默认 like、其他类型默认 eq"),

            // entity.gentablecolumn.htmltype
            new TranslationSeedItem("entity.gentablecolumn.htmltype", "en-US", "显示类型_us", "显示类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）"),
            // entity.gentablecolumn.htmltype
            new TranslationSeedItem("entity.gentablecolumn.htmltype", "ja-JP", "显示类型_jp", "显示类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）"),
            // entity.gentablecolumn.htmltype
            new TranslationSeedItem("entity.gentablecolumn.htmltype", "zh-CN", "显示类型", "显示类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）"),
            // entity.gentablecolumn.htmltype
            new TranslationSeedItem("entity.gentablecolumn.htmltype", "zh-HK", "显示类型_hk", "显示类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）"),

            // entity.gentablecolumn.dicttype
            new TranslationSeedItem("entity.gentablecolumn.dicttype", "en-US", "字典类型_us", "字典类型（选项 TaktDictTypes/options；DictValue=Id）"),
            // entity.gentablecolumn.dicttype
            new TranslationSeedItem("entity.gentablecolumn.dicttype", "ja-JP", "字典类型_jp", "字典类型（选项 TaktDictTypes/options；DictValue=Id）"),
            // entity.gentablecolumn.dicttype
            new TranslationSeedItem("entity.gentablecolumn.dicttype", "zh-CN", "字典类型", "字典类型（选项 TaktDictTypes/options；DictValue=Id）"),
            // entity.gentablecolumn.dicttype
            new TranslationSeedItem("entity.gentablecolumn.dicttype", "zh-HK", "字典类型_hk", "字典类型（选项 TaktDictTypes/options；DictValue=Id）"),

            // entity.gentablecolumn.table
            new TranslationSeedItem("entity.gentablecolumn.table", "en-US", "所属表配置_us", "所属表配置（主表，本表 GenTableId 关联 TaktGenTable.Id）"),
            // entity.gentablecolumn.table
            new TranslationSeedItem("entity.gentablecolumn.table", "ja-JP", "所属表配置_jp", "所属表配置（主表，本表 GenTableId 关联 TaktGenTable.Id）"),
            // entity.gentablecolumn.table
            new TranslationSeedItem("entity.gentablecolumn.table", "zh-CN", "所属表配置", "所属表配置（主表，本表 GenTableId 关联 TaktGenTable.Id）"),
            // entity.gentablecolumn.table
            new TranslationSeedItem("entity.gentablecolumn.table", "zh-HK", "所属表配置_hk", "所属表配置（主表，本表 GenTableId 关联 TaktGenTable.Id）"),
        };
    }

    /// <summary>
    /// 填充 TaktTranslation 全部业务字段（含租户基类字段）
    /// </summary>
    private static void ApplyTranslationFields(
        TaktTranslation translation,
        string tenantCode,
        long cultureId,
        TranslationSeedItem item)
    {
        translation.TenantCode = tenantCode;
        translation.CultureId = cultureId;
        translation.CultureCode = item.CultureCode;
        translation.I18nKey = item.I18nKey;
        translation.TranslationText = item.TranslationText;
        translation.ResourceGroup = "Generator";
        translation.ResourceType = "frontend";
        translation.ContextNote = item.ContextNote;
        translation.ExtField = null;
        translation.Remark = null;
        translation.IsDeleted = 0;
        translation.DeletedBy = null;
        translation.DeletedAt = null;
    }

    private static async Task<(TaktTranslation Translation, int InsertCount, int UpdateCount)> CreateOrUpdateTranslationAsync(
        ITaktTenantSeedRepository<TaktTranslation> repository,
        string tenantCode,
        long cultureId,
        TranslationSeedItem item)
    {
        var translation = await repository.FirstAsync(t =>
            t.TenantCode == tenantCode &&
            t.I18nKey == item.I18nKey &&
            t.CultureCode == item.CultureCode);

        if (translation == null)
        {
            translation = new TaktTranslation();
            ApplyTranslationFields(translation, tenantCode, cultureId, item);
            translation = await repository.CreateAsync(translation);
            return (translation, 1, 0);
        }

        ApplyTranslationFields(translation, tenantCode, cultureId, item);
        await repository.UpdateAsync(translation);
        return (translation, 0, 1);
    }

    /// <summary>
    /// 翻译种子项（对应 TaktTranslation 全部可写字段，CultureId 由 SeedAsync 解析）
    /// </summary>
    private sealed record TranslationSeedItem(
        string I18nKey,
        string CultureCode,
        string TranslationText,
        string? ContextNote);
}
