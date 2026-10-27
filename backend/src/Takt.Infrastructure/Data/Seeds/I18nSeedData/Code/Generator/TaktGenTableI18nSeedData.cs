// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Code.Generator
// 文件名称：TaktGenTableI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktGenTable 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktGenTable 实体国际化翻译种子（键前缀 entity.gentable.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktGenTableI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktGenTable 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 gentable 实体翻译...", tenantCode);

        foreach (var item in GetGenTableTranslations())
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

        TaktLogger.Information("TaktGenTable 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktGenTable 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.gentable._self / entity.gentable.{{field}}；ResourceGroup=Generator；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetGenTableTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.gentable._self
            new TranslationSeedItem("entity.gentable._self", "en-US", "Gen Table Information_us", "实体名称"),
            // entity.gentable._self
            new TranslationSeedItem("entity.gentable._self", "ja-JP", "Takt代码生成表配置信息_jp", "实体名称"),
            // entity.gentable._self
            new TranslationSeedItem("entity.gentable._self", "zh-CN", "Takt代码生成表配置信息", "实体名称"),
            // entity.gentable._self
            new TranslationSeedItem("entity.gentable._self", "zh-HK", "Takt代码生成表配置信息_hk", "实体名称"),

            // entity.gentable.datasource
            new TranslationSeedItem("entity.gentable.datasource", "en-US", "数据源_us", "数据源（选项 TaktDatabaseInfos/list；持久化 displayName:tenantCode）"),
            // entity.gentable.datasource
            new TranslationSeedItem("entity.gentable.datasource", "ja-JP", "数据源_jp", "数据源（选项 TaktDatabaseInfos/list；持久化 displayName:tenantCode）"),
            // entity.gentable.datasource
            new TranslationSeedItem("entity.gentable.datasource", "zh-CN", "数据源", "数据源（选项 TaktDatabaseInfos/list；持久化 displayName:tenantCode）"),
            // entity.gentable.datasource
            new TranslationSeedItem("entity.gentable.datasource", "zh-HK", "数据源_hk", "数据源（选项 TaktDatabaseInfos/list；持久化 displayName:tenantCode）"),

            // entity.gentable.tablename
            new TranslationSeedItem("entity.gentable.tablename", "en-US", "表名称_us", "表名称（选项 TaktDatabaseInfos/tables；新建可手输；租户内与 DataSource 唯一）"),
            // entity.gentable.tablename
            new TranslationSeedItem("entity.gentable.tablename", "ja-JP", "表名称_jp", "表名称（选项 TaktDatabaseInfos/tables；新建可手输；租户内与 DataSource 唯一）"),
            // entity.gentable.tablename
            new TranslationSeedItem("entity.gentable.tablename", "zh-CN", "表名称", "表名称（选项 TaktDatabaseInfos/tables；新建可手输；租户内与 DataSource 唯一）"),
            // entity.gentable.tablename
            new TranslationSeedItem("entity.gentable.tablename", "zh-HK", "表名称_hk", "表名称（选项 TaktDatabaseInfos/tables；新建可手输；租户内与 DataSource 唯一）"),

            // entity.gentable.tablecomment
            new TranslationSeedItem("entity.gentable.tablecomment", "en-US", "表描述_us", "表描述（表注释）"),
            // entity.gentable.tablecomment
            new TranslationSeedItem("entity.gentable.tablecomment", "ja-JP", "表描述_jp", "表描述（表注释）"),
            // entity.gentable.tablecomment
            new TranslationSeedItem("entity.gentable.tablecomment", "zh-CN", "表描述", "表描述（表注释）"),
            // entity.gentable.tablecomment
            new TranslationSeedItem("entity.gentable.tablecomment", "zh-HK", "表描述_hk", "表描述（表注释）"),

            // entity.gentable.subtablename
            new TranslationSeedItem("entity.gentable.subtablename", "en-US", "关联父表_us", "关联父表（选项 TaktDatabaseInfos/tables 同库其他表；sub 模板必填）"),
            // entity.gentable.subtablename
            new TranslationSeedItem("entity.gentable.subtablename", "ja-JP", "关联父表_jp", "关联父表（选项 TaktDatabaseInfos/tables 同库其他表；sub 模板必填）"),
            // entity.gentable.subtablename
            new TranslationSeedItem("entity.gentable.subtablename", "zh-CN", "关联父表", "关联父表（选项 TaktDatabaseInfos/tables 同库其他表；sub 模板必填）"),
            // entity.gentable.subtablename
            new TranslationSeedItem("entity.gentable.subtablename", "zh-HK", "关联父表_hk", "关联父表（选项 TaktDatabaseInfos/tables 同库其他表；sub 模板必填）"),

            // entity.gentable.subtablefkname
            new TranslationSeedItem("entity.gentable.subtablefkname", "en-US", "关联外键_us", "关联外键（选项本表 columnList.databaseColumnName；sub 模板必填）"),
            // entity.gentable.subtablefkname
            new TranslationSeedItem("entity.gentable.subtablefkname", "ja-JP", "关联外键_jp", "关联外键（选项本表 columnList.databaseColumnName；sub 模板必填）"),
            // entity.gentable.subtablefkname
            new TranslationSeedItem("entity.gentable.subtablefkname", "zh-CN", "关联外键", "关联外键（选项本表 columnList.databaseColumnName；sub 模板必填）"),
            // entity.gentable.subtablefkname
            new TranslationSeedItem("entity.gentable.subtablefkname", "zh-HK", "关联外键_hk", "关联外键（选项本表 columnList.databaseColumnName；sub 模板必填）"),

            // entity.gentable.treecode
            new TranslationSeedItem("entity.gentable.treecode", "en-US", "树编码_us", "树编码（选项本表 columnList.databaseColumnName；tree 模板必填）"),
            // entity.gentable.treecode
            new TranslationSeedItem("entity.gentable.treecode", "ja-JP", "树编码_jp", "树编码（选项本表 columnList.databaseColumnName；tree 模板必填）"),
            // entity.gentable.treecode
            new TranslationSeedItem("entity.gentable.treecode", "zh-CN", "树编码", "树编码（选项本表 columnList.databaseColumnName；tree 模板必填）"),
            // entity.gentable.treecode
            new TranslationSeedItem("entity.gentable.treecode", "zh-HK", "树编码_hk", "树编码（选项本表 columnList.databaseColumnName；tree 模板必填）"),

            // entity.gentable.treeparentcode
            new TranslationSeedItem("entity.gentable.treeparentcode", "en-US", "树父编码_us", "树父编码（选项本表 columnList.databaseColumnName；tree 模板必填）"),
            // entity.gentable.treeparentcode
            new TranslationSeedItem("entity.gentable.treeparentcode", "ja-JP", "树父编码_jp", "树父编码（选项本表 columnList.databaseColumnName；tree 模板必填）"),
            // entity.gentable.treeparentcode
            new TranslationSeedItem("entity.gentable.treeparentcode", "zh-CN", "树父编码", "树父编码（选项本表 columnList.databaseColumnName；tree 模板必填）"),
            // entity.gentable.treeparentcode
            new TranslationSeedItem("entity.gentable.treeparentcode", "zh-HK", "树父编码_hk", "树父编码（选项本表 columnList.databaseColumnName；tree 模板必填）"),

            // entity.gentable.treename
            new TranslationSeedItem("entity.gentable.treename", "en-US", "树名称_us", "树名称（选项本表 columnList.databaseColumnName；tree 模板必填）"),
            // entity.gentable.treename
            new TranslationSeedItem("entity.gentable.treename", "ja-JP", "树名称_jp", "树名称（选项本表 columnList.databaseColumnName；tree 模板必填）"),
            // entity.gentable.treename
            new TranslationSeedItem("entity.gentable.treename", "zh-CN", "树名称", "树名称（选项本表 columnList.databaseColumnName；tree 模板必填）"),
            // entity.gentable.treename
            new TranslationSeedItem("entity.gentable.treename", "zh-HK", "树名称_hk", "树名称（选项本表 columnList.databaseColumnName；tree 模板必填）"),

            // entity.gentable.indatabase
            new TranslationSeedItem("entity.gentable.indatabase", "en-US", "库表标识_us", "库表标识（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentable.indatabase
            new TranslationSeedItem("entity.gentable.indatabase", "ja-JP", "库表标识_jp", "库表标识（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentable.indatabase
            new TranslationSeedItem("entity.gentable.indatabase", "zh-CN", "库表标识", "库表标识（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentable.indatabase
            new TranslationSeedItem("entity.gentable.indatabase", "zh-HK", "库表标识_hk", "库表标识（字典 sys_yes_no；0=否 1=是）"),

            // entity.gentable.gentemplatecategory
            new TranslationSeedItem("entity.gentable.gentemplatecategory", "en-US", "生成模板类型_us", "生成模板类型（字典 code_generator_template_type；crud/sub/tree）"),
            // entity.gentable.gentemplatecategory
            new TranslationSeedItem("entity.gentable.gentemplatecategory", "ja-JP", "生成模板类型_jp", "生成模板类型（字典 code_generator_template_type；crud/sub/tree）"),
            // entity.gentable.gentemplatecategory
            new TranslationSeedItem("entity.gentable.gentemplatecategory", "zh-CN", "生成模板类型", "生成模板类型（字典 code_generator_template_type；crud/sub/tree）"),
            // entity.gentable.gentemplatecategory
            new TranslationSeedItem("entity.gentable.gentemplatecategory", "zh-HK", "生成模板类型_hk", "生成模板类型（字典 code_generator_template_type；crud/sub/tree）"),

            // entity.gentable.genmodulename
            new TranslationSeedItem("entity.gentable.genmodulename", "en-US", "模块名_us", "模块名（功能模块名称）"),
            // entity.gentable.genmodulename
            new TranslationSeedItem("entity.gentable.genmodulename", "ja-JP", "模块名_jp", "模块名（功能模块名称）"),
            // entity.gentable.genmodulename
            new TranslationSeedItem("entity.gentable.genmodulename", "zh-CN", "模块名", "模块名（功能模块名称）"),
            // entity.gentable.genmodulename
            new TranslationSeedItem("entity.gentable.genmodulename", "zh-HK", "模块名_hk", "模块名（功能模块名称）"),

            // entity.gentable.genbusinessname
            new TranslationSeedItem("entity.gentable.genbusinessname", "en-US", "业务名_us", "业务名（用于类名，如 Company，与模块拼接为 Takt.模块+类名）"),
            // entity.gentable.genbusinessname
            new TranslationSeedItem("entity.gentable.genbusinessname", "ja-JP", "业务名_jp", "业务名（用于类名，如 Company，与模块拼接为 Takt.模块+类名）"),
            // entity.gentable.genbusinessname
            new TranslationSeedItem("entity.gentable.genbusinessname", "zh-CN", "业务名", "业务名（用于类名，如 Company，与模块拼接为 Takt.模块+类名）"),
            // entity.gentable.genbusinessname
            new TranslationSeedItem("entity.gentable.genbusinessname", "zh-HK", "业务名_hk", "业务名（用于类名，如 Company，与模块拼接为 Takt.模块+类名）"),

            // entity.gentable.genfunctionname
            new TranslationSeedItem("entity.gentable.genfunctionname", "en-US", "功能名_us", "功能名（用于接口与注释的中文名称，如：公司、部门）"),
            // entity.gentable.genfunctionname
            new TranslationSeedItem("entity.gentable.genfunctionname", "ja-JP", "功能名_jp", "功能名（用于接口与注释的中文名称，如：公司、部门）"),
            // entity.gentable.genfunctionname
            new TranslationSeedItem("entity.gentable.genfunctionname", "zh-CN", "功能名", "功能名（用于接口与注释的中文名称，如：公司、部门）"),
            // entity.gentable.genfunctionname
            new TranslationSeedItem("entity.gentable.genfunctionname", "zh-HK", "功能名_hk", "功能名（用于接口与注释的中文名称，如：公司、部门）"),

            // entity.gentable.permsprefix
            new TranslationSeedItem("entity.gentable.permsprefix", "en-US", "权限前缀_us", "权限前缀（与生成控制器/菜单/前端权限一致；对应库列 <c>perms_prefix</c>）。"),
            // entity.gentable.permsprefix
            new TranslationSeedItem("entity.gentable.permsprefix", "ja-JP", "权限前缀_jp", "权限前缀（与生成控制器/菜单/前端权限一致；对应库列 <c>perms_prefix</c>）。"),
            // entity.gentable.permsprefix
            new TranslationSeedItem("entity.gentable.permsprefix", "zh-CN", "权限前缀", "权限前缀（与生成控制器/菜单/前端权限一致；对应库列 <c>perms_prefix</c>）。"),
            // entity.gentable.permsprefix
            new TranslationSeedItem("entity.gentable.permsprefix", "zh-HK", "权限前缀_hk", "权限前缀（与生成控制器/菜单/前端权限一致；对应库列 <c>perms_prefix</c>）。"),

            // entity.gentable.menubuttongroup
            new TranslationSeedItem("entity.gentable.menubuttongroup", "en-US", "菜单权限组_us", "菜单权限组（字典 code_generator_button_category 多选逗号；仅用于生成 menu_and_translation.sql 按钮 INSERT，不参与控制器/前端代码生成）"),
            // entity.gentable.menubuttongroup
            new TranslationSeedItem("entity.gentable.menubuttongroup", "ja-JP", "菜单权限组_jp", "菜单权限组（字典 code_generator_button_category 多选逗号；仅用于生成 menu_and_translation.sql 按钮 INSERT，不参与控制器/前端代码生成）"),
            // entity.gentable.menubuttongroup
            new TranslationSeedItem("entity.gentable.menubuttongroup", "zh-CN", "菜单权限组", "菜单权限组（字典 code_generator_button_category 多选逗号；仅用于生成 menu_and_translation.sql 按钮 INSERT，不参与控制器/前端代码生成）"),
            // entity.gentable.menubuttongroup
            new TranslationSeedItem("entity.gentable.menubuttongroup", "zh-HK", "菜单权限组_hk", "菜单权限组（字典 code_generator_button_category 多选逗号；仅用于生成 menu_and_translation.sql 按钮 INSERT，不参与控制器/前端代码生成）"),

            // entity.gentable.nameprefix
            new TranslationSeedItem("entity.gentable.nameprefix", "en-US", "命名空间前缀_us", "命名空间前缀（用于生成类名、方法名等的前缀）"),
            // entity.gentable.nameprefix
            new TranslationSeedItem("entity.gentable.nameprefix", "ja-JP", "命名空间前缀_jp", "命名空间前缀（用于生成类名、方法名等的前缀）"),
            // entity.gentable.nameprefix
            new TranslationSeedItem("entity.gentable.nameprefix", "zh-CN", "命名空间前缀", "命名空间前缀（用于生成类名、方法名等的前缀）"),
            // entity.gentable.nameprefix
            new TranslationSeedItem("entity.gentable.nameprefix", "zh-HK", "命名空间前缀_hk", "命名空间前缀（用于生成类名、方法名等的前缀）"),

            // entity.gentable.entitynamespace
            new TranslationSeedItem("entity.gentable.entitynamespace", "en-US", "实体命名空间_us", "实体命名空间（默认当前项目：Takt.Domain.Entities）"),
            // entity.gentable.entitynamespace
            new TranslationSeedItem("entity.gentable.entitynamespace", "ja-JP", "实体命名空间_jp", "实体命名空间（默认当前项目：Takt.Domain.Entities）"),
            // entity.gentable.entitynamespace
            new TranslationSeedItem("entity.gentable.entitynamespace", "zh-CN", "实体命名空间", "实体命名空间（默认当前项目：Takt.Domain.Entities）"),
            // entity.gentable.entitynamespace
            new TranslationSeedItem("entity.gentable.entitynamespace", "zh-HK", "实体命名空间_hk", "实体命名空间（默认当前项目：Takt.Domain.Entities）"),

            // entity.gentable.entityclassname
            new TranslationSeedItem("entity.gentable.entityclassname", "en-US", "实体类名称_us", "实体类名称（首字母大写，驼峰命名）"),
            // entity.gentable.entityclassname
            new TranslationSeedItem("entity.gentable.entityclassname", "ja-JP", "实体类名称_jp", "实体类名称（首字母大写，驼峰命名）"),
            // entity.gentable.entityclassname
            new TranslationSeedItem("entity.gentable.entityclassname", "zh-CN", "实体类名称", "实体类名称（首字母大写，驼峰命名）"),
            // entity.gentable.entityclassname
            new TranslationSeedItem("entity.gentable.entityclassname", "zh-HK", "实体类名称_hk", "实体类名称（首字母大写，驼峰命名）"),

            // entity.gentable.dtonamespace
            new TranslationSeedItem("entity.gentable.dtonamespace", "en-US", "传输对象Dto命名空间_us", "传输对象Dto命名空间（默认当前项目：Takt.Application.Dtos）"),
            // entity.gentable.dtonamespace
            new TranslationSeedItem("entity.gentable.dtonamespace", "ja-JP", "传输对象Dto命名空间_jp", "传输对象Dto命名空间（默认当前项目：Takt.Application.Dtos）"),
            // entity.gentable.dtonamespace
            new TranslationSeedItem("entity.gentable.dtonamespace", "zh-CN", "传输对象Dto命名空间", "传输对象Dto命名空间（默认当前项目：Takt.Application.Dtos）"),
            // entity.gentable.dtonamespace
            new TranslationSeedItem("entity.gentable.dtonamespace", "zh-HK", "传输对象Dto命名空间_hk", "传输对象Dto命名空间（默认当前项目：Takt.Application.Dtos）"),

            // entity.gentable.dtoclassname
            new TranslationSeedItem("entity.gentable.dtoclassname", "en-US", "传输对象Dto类名_us", "传输对象 Dto 类名"),
            // entity.gentable.dtoclassname
            new TranslationSeedItem("entity.gentable.dtoclassname", "ja-JP", "传输对象Dto类名_jp", "传输对象 Dto 类名"),
            // entity.gentable.dtoclassname
            new TranslationSeedItem("entity.gentable.dtoclassname", "zh-CN", "传输对象Dto类名", "传输对象 Dto 类名"),
            // entity.gentable.dtoclassname
            new TranslationSeedItem("entity.gentable.dtoclassname", "zh-HK", "传输对象Dto类名_hk", "传输对象 Dto 类名"),

            // entity.gentable.servicenamespace
            new TranslationSeedItem("entity.gentable.servicenamespace", "en-US", "服务命名空间_us", "服务命名空间（默认当前项目：Takt.Application.Services）"),
            // entity.gentable.servicenamespace
            new TranslationSeedItem("entity.gentable.servicenamespace", "ja-JP", "服务命名空间_jp", "服务命名空间（默认当前项目：Takt.Application.Services）"),
            // entity.gentable.servicenamespace
            new TranslationSeedItem("entity.gentable.servicenamespace", "zh-CN", "服务命名空间", "服务命名空间（默认当前项目：Takt.Application.Services）"),
            // entity.gentable.servicenamespace
            new TranslationSeedItem("entity.gentable.servicenamespace", "zh-HK", "服务命名空间_hk", "服务命名空间（默认当前项目：Takt.Application.Services）"),

            // entity.gentable.iserviceclassname
            new TranslationSeedItem("entity.gentable.iserviceclassname", "en-US", "服务接口类名称_us", "服务接口类名称"),
            // entity.gentable.iserviceclassname
            new TranslationSeedItem("entity.gentable.iserviceclassname", "ja-JP", "服务接口类名称_jp", "服务接口类名称"),
            // entity.gentable.iserviceclassname
            new TranslationSeedItem("entity.gentable.iserviceclassname", "zh-CN", "服务接口类名称", "服务接口类名称"),
            // entity.gentable.iserviceclassname
            new TranslationSeedItem("entity.gentable.iserviceclassname", "zh-HK", "服务接口类名称_hk", "服务接口类名称"),

            // entity.gentable.serviceclassname
            new TranslationSeedItem("entity.gentable.serviceclassname", "en-US", "服务类名称_us", "服务类名称"),
            // entity.gentable.serviceclassname
            new TranslationSeedItem("entity.gentable.serviceclassname", "ja-JP", "服务类名称_jp", "服务类名称"),
            // entity.gentable.serviceclassname
            new TranslationSeedItem("entity.gentable.serviceclassname", "zh-CN", "服务类名称", "服务类名称"),
            // entity.gentable.serviceclassname
            new TranslationSeedItem("entity.gentable.serviceclassname", "zh-HK", "服务类名称_hk", "服务类名称"),

            // entity.gentable.controllernamespace
            new TranslationSeedItem("entity.gentable.controllernamespace", "en-US", "控制器命名空间_us", "控制器命名空间（默认当前项目：Takt.WebApi.Controllers）"),
            // entity.gentable.controllernamespace
            new TranslationSeedItem("entity.gentable.controllernamespace", "ja-JP", "控制器命名空间_jp", "控制器命名空间（默认当前项目：Takt.WebApi.Controllers）"),
            // entity.gentable.controllernamespace
            new TranslationSeedItem("entity.gentable.controllernamespace", "zh-CN", "控制器命名空间", "控制器命名空间（默认当前项目：Takt.WebApi.Controllers）"),
            // entity.gentable.controllernamespace
            new TranslationSeedItem("entity.gentable.controllernamespace", "zh-HK", "控制器命名空间_hk", "控制器命名空间（默认当前项目：Takt.WebApi.Controllers）"),

            // entity.gentable.controllerclassname
            new TranslationSeedItem("entity.gentable.controllerclassname", "en-US", "控制器类名称_us", "控制器类名称"),
            // entity.gentable.controllerclassname
            new TranslationSeedItem("entity.gentable.controllerclassname", "ja-JP", "控制器类名称_jp", "控制器类名称"),
            // entity.gentable.controllerclassname
            new TranslationSeedItem("entity.gentable.controllerclassname", "zh-CN", "控制器类名称", "控制器类名称"),
            // entity.gentable.controllerclassname
            new TranslationSeedItem("entity.gentable.controllerclassname", "zh-HK", "控制器类名称_hk", "控制器类名称"),

            // entity.gentable.isrepository
            new TranslationSeedItem("entity.gentable.isrepository", "en-US", "仓储层_us", "仓储层（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentable.isrepository
            new TranslationSeedItem("entity.gentable.isrepository", "ja-JP", "仓储层_jp", "仓储层（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentable.isrepository
            new TranslationSeedItem("entity.gentable.isrepository", "zh-CN", "仓储层", "仓储层（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentable.isrepository
            new TranslationSeedItem("entity.gentable.isrepository", "zh-HK", "仓储层_hk", "仓储层（字典 sys_yes_no；0=否 1=是）"),

            // entity.gentable.repositoryinterfacenamespace
            new TranslationSeedItem("entity.gentable.repositoryinterfacenamespace", "en-US", "仓储接口命名空间_us", "仓储接口命名空间（默认当前项目：Takt.Domain.Repositories）"),
            // entity.gentable.repositoryinterfacenamespace
            new TranslationSeedItem("entity.gentable.repositoryinterfacenamespace", "ja-JP", "仓储接口命名空间_jp", "仓储接口命名空间（默认当前项目：Takt.Domain.Repositories）"),
            // entity.gentable.repositoryinterfacenamespace
            new TranslationSeedItem("entity.gentable.repositoryinterfacenamespace", "zh-CN", "仓储接口命名空间", "仓储接口命名空间（默认当前项目：Takt.Domain.Repositories）"),
            // entity.gentable.repositoryinterfacenamespace
            new TranslationSeedItem("entity.gentable.repositoryinterfacenamespace", "zh-HK", "仓储接口命名空间_hk", "仓储接口命名空间（默认当前项目：Takt.Domain.Repositories）"),

            // entity.gentable.irepositoryclassname
            new TranslationSeedItem("entity.gentable.irepositoryclassname", "en-US", "仓储接口类名称_us", "仓储接口类名称"),
            // entity.gentable.irepositoryclassname
            new TranslationSeedItem("entity.gentable.irepositoryclassname", "ja-JP", "仓储接口类名称_jp", "仓储接口类名称"),
            // entity.gentable.irepositoryclassname
            new TranslationSeedItem("entity.gentable.irepositoryclassname", "zh-CN", "仓储接口类名称", "仓储接口类名称"),
            // entity.gentable.irepositoryclassname
            new TranslationSeedItem("entity.gentable.irepositoryclassname", "zh-HK", "仓储接口类名称_hk", "仓储接口类名称"),

            // entity.gentable.repositorynamespace
            new TranslationSeedItem("entity.gentable.repositorynamespace", "en-US", "仓储命名空间_us", "仓储命名空间（默认当前项目：Takt.Infrastructure.Repositories）"),
            // entity.gentable.repositorynamespace
            new TranslationSeedItem("entity.gentable.repositorynamespace", "ja-JP", "仓储命名空间_jp", "仓储命名空间（默认当前项目：Takt.Infrastructure.Repositories）"),
            // entity.gentable.repositorynamespace
            new TranslationSeedItem("entity.gentable.repositorynamespace", "zh-CN", "仓储命名空间", "仓储命名空间（默认当前项目：Takt.Infrastructure.Repositories）"),
            // entity.gentable.repositorynamespace
            new TranslationSeedItem("entity.gentable.repositorynamespace", "zh-HK", "仓储命名空间_hk", "仓储命名空间（默认当前项目：Takt.Infrastructure.Repositories）"),

            // entity.gentable.repositoryclassname
            new TranslationSeedItem("entity.gentable.repositoryclassname", "en-US", "仓储类名称_us", "仓储类名称"),
            // entity.gentable.repositoryclassname
            new TranslationSeedItem("entity.gentable.repositoryclassname", "ja-JP", "仓储类名称_jp", "仓储类名称"),
            // entity.gentable.repositoryclassname
            new TranslationSeedItem("entity.gentable.repositoryclassname", "zh-CN", "仓储类名称", "仓储类名称"),
            // entity.gentable.repositoryclassname
            new TranslationSeedItem("entity.gentable.repositoryclassname", "zh-HK", "仓储类名称_hk", "仓储类名称"),

            // entity.gentable.genfunction
            new TranslationSeedItem("entity.gentable.genfunction", "en-US", "生成功能_us", "生成功能（字典 code_generator_function 多选逗号；亦支持 JSON/数组）。 核心设计：GenFunction 不仅决定生成哪些 Controller Actions 和 Service Methods，还决定生成哪些 DTO 类。功能与 DTO 的映射关系如下： Query → QueryDto；Create → CreateDto；Update → UpdateDto；Status → StatusDto；Sort → SortDto； Import → TemplateDto + ImportDto；Export → ExportDto；所有功能 → Dto（基础传输对象，包含所有字段）。"),
            // entity.gentable.genfunction
            new TranslationSeedItem("entity.gentable.genfunction", "ja-JP", "生成功能_jp", "生成功能（字典 code_generator_function 多选逗号；亦支持 JSON/数组）。 核心设计：GenFunction 不仅决定生成哪些 Controller Actions 和 Service Methods，还决定生成哪些 DTO 类。功能与 DTO 的映射关系如下： Query → QueryDto；Create → CreateDto；Update → UpdateDto；Status → StatusDto；Sort → SortDto； Import → TemplateDto + ImportDto；Export → ExportDto；所有功能 → Dto（基础传输对象，包含所有字段）。"),
            // entity.gentable.genfunction
            new TranslationSeedItem("entity.gentable.genfunction", "zh-CN", "生成功能", "生成功能（字典 code_generator_function 多选逗号；亦支持 JSON/数组）。 核心设计：GenFunction 不仅决定生成哪些 Controller Actions 和 Service Methods，还决定生成哪些 DTO 类。功能与 DTO 的映射关系如下： Query → QueryDto；Create → CreateDto；Update → UpdateDto；Status → StatusDto；Sort → SortDto； Import → TemplateDto + ImportDto；Export → ExportDto；所有功能 → Dto（基础传输对象，包含所有字段）。"),
            // entity.gentable.genfunction
            new TranslationSeedItem("entity.gentable.genfunction", "zh-HK", "生成功能_hk", "生成功能（字典 code_generator_function 多选逗号；亦支持 JSON/数组）。 核心设计：GenFunction 不仅决定生成哪些 Controller Actions 和 Service Methods，还决定生成哪些 DTO 类。功能与 DTO 的映射关系如下： Query → QueryDto；Create → CreateDto；Update → UpdateDto；Status → StatusDto；Sort → SortDto； Import → TemplateDto + ImportDto；Export → ExportDto；所有功能 → Dto（基础传输对象，包含所有字段）。"),

            // entity.gentable.genmethod
            new TranslationSeedItem("entity.gentable.genmethod", "en-US", "生成方式_us", "生成方式（字典 code_generator_method；0=zip 1=自定义路径 2=当前项目）"),
            // entity.gentable.genmethod
            new TranslationSeedItem("entity.gentable.genmethod", "ja-JP", "生成方式_jp", "生成方式（字典 code_generator_method；0=zip 1=自定义路径 2=当前项目）"),
            // entity.gentable.genmethod
            new TranslationSeedItem("entity.gentable.genmethod", "zh-CN", "生成方式", "生成方式（字典 code_generator_method；0=zip 1=自定义路径 2=当前项目）"),
            // entity.gentable.genmethod
            new TranslationSeedItem("entity.gentable.genmethod", "zh-HK", "生成方式_hk", "生成方式（字典 code_generator_method；0=zip 1=自定义路径 2=当前项目）"),

            // entity.gentable.genpath
            new TranslationSeedItem("entity.gentable.genpath", "en-US", "生成路径_us", "生成路径（字典 code_generator_path_type；GenMethod=1 时选择；0 默认 /；2 由 GenMethod 解析）"),
            // entity.gentable.genpath
            new TranslationSeedItem("entity.gentable.genpath", "ja-JP", "生成路径_jp", "生成路径（字典 code_generator_path_type；GenMethod=1 时选择；0 默认 /；2 由 GenMethod 解析）"),
            // entity.gentable.genpath
            new TranslationSeedItem("entity.gentable.genpath", "zh-CN", "生成路径", "生成路径（字典 code_generator_path_type；GenMethod=1 时选择；0 默认 /；2 由 GenMethod 解析）"),
            // entity.gentable.genpath
            new TranslationSeedItem("entity.gentable.genpath", "zh-HK", "生成路径_hk", "生成路径（字典 code_generator_path_type；GenMethod=1 时选择；0 默认 /；2 由 GenMethod 解析）"),

            // entity.gentable.isgenmenu
            new TranslationSeedItem("entity.gentable.isgenmenu", "en-US", "生成菜单_us", "生成菜单（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentable.isgenmenu
            new TranslationSeedItem("entity.gentable.isgenmenu", "ja-JP", "生成菜单_jp", "生成菜单（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentable.isgenmenu
            new TranslationSeedItem("entity.gentable.isgenmenu", "zh-CN", "生成菜单", "生成菜单（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentable.isgenmenu
            new TranslationSeedItem("entity.gentable.isgenmenu", "zh-HK", "生成菜单_hk", "生成菜单（字典 sys_yes_no；0=否 1=是）"),

            // entity.gentable.parentmenuid
            new TranslationSeedItem("entity.gentable.parentmenuid", "en-US", "上级菜单ID_us", "上级菜单（关联 TaktMenu.Id，选项 TaktMenus/tree-options）"),
            // entity.gentable.parentmenuid
            new TranslationSeedItem("entity.gentable.parentmenuid", "ja-JP", "上级菜单ID_jp", "上级菜单（关联 TaktMenu.Id，选项 TaktMenus/tree-options）"),
            // entity.gentable.parentmenuid
            new TranslationSeedItem("entity.gentable.parentmenuid", "zh-CN", "上级菜单ID", "上级菜单（关联 TaktMenu.Id，选项 TaktMenus/tree-options）"),
            // entity.gentable.parentmenuid
            new TranslationSeedItem("entity.gentable.parentmenuid", "zh-HK", "上级菜单ID_hk", "上级菜单（关联 TaktMenu.Id，选项 TaktMenus/tree-options）"),

            // entity.gentable.isgentranslation
            new TranslationSeedItem("entity.gentable.isgentranslation", "en-US", "生成翻译_us", "生成翻译（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentable.isgentranslation
            new TranslationSeedItem("entity.gentable.isgentranslation", "ja-JP", "生成翻译_jp", "生成翻译（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentable.isgentranslation
            new TranslationSeedItem("entity.gentable.isgentranslation", "zh-CN", "生成翻译", "生成翻译（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentable.isgentranslation
            new TranslationSeedItem("entity.gentable.isgentranslation", "zh-HK", "生成翻译_hk", "生成翻译（字典 sys_yes_no；0=否 1=是）"),

            // entity.gentable.sortfield
            new TranslationSeedItem("entity.gentable.sortfield", "en-US", "排序字段_us", "排序字段（选项本表 columnList.databaseColumnName）"),
            // entity.gentable.sortfield
            new TranslationSeedItem("entity.gentable.sortfield", "ja-JP", "排序字段_jp", "排序字段（选项本表 columnList.databaseColumnName）"),
            // entity.gentable.sortfield
            new TranslationSeedItem("entity.gentable.sortfield", "zh-CN", "排序字段", "排序字段（选项本表 columnList.databaseColumnName）"),
            // entity.gentable.sortfield
            new TranslationSeedItem("entity.gentable.sortfield", "zh-HK", "排序字段_hk", "排序字段（选项本表 columnList.databaseColumnName）"),

            // entity.gentable.sorttype
            new TranslationSeedItem("entity.gentable.sorttype", "en-US", "排序方向_us", "排序方向（字典 sys_sort_type；ASC=升序 DESC=降序）"),
            // entity.gentable.sorttype
            new TranslationSeedItem("entity.gentable.sorttype", "ja-JP", "排序方向_jp", "排序方向（字典 sys_sort_type；ASC=升序 DESC=降序）"),
            // entity.gentable.sorttype
            new TranslationSeedItem("entity.gentable.sorttype", "zh-CN", "排序方向", "排序方向（字典 sys_sort_type；ASC=升序 DESC=降序）"),
            // entity.gentable.sorttype
            new TranslationSeedItem("entity.gentable.sorttype", "zh-HK", "排序方向_hk", "排序方向（字典 sys_sort_type；ASC=升序 DESC=降序）"),

            // entity.gentable.frontui
            new TranslationSeedItem("entity.gentable.frontui", "en-US", "前端UI框架_us", "前端UI框架（字典 code_generator_frontend_ui_type；1=element plus 2=ant design vue）"),
            // entity.gentable.frontui
            new TranslationSeedItem("entity.gentable.frontui", "ja-JP", "前端UI框架_jp", "前端UI框架（字典 code_generator_frontend_ui_type；1=element plus 2=ant design vue）"),
            // entity.gentable.frontui
            new TranslationSeedItem("entity.gentable.frontui", "zh-CN", "前端UI框架", "前端UI框架（字典 code_generator_frontend_ui_type；1=element plus 2=ant design vue）"),
            // entity.gentable.frontui
            new TranslationSeedItem("entity.gentable.frontui", "zh-HK", "前端UI框架_hk", "前端UI框架（字典 code_generator_frontend_ui_type；1=element plus 2=ant design vue）"),

            // entity.gentable.frontformlayout
            new TranslationSeedItem("entity.gentable.frontformlayout", "en-US", "前端表单布局_us", "前端表单布局（字典 code_generator_frontend_form_layout；12=一行一列 24=一行两列）"),
            // entity.gentable.frontformlayout
            new TranslationSeedItem("entity.gentable.frontformlayout", "ja-JP", "前端表单布局_jp", "前端表单布局（字典 code_generator_frontend_form_layout；12=一行一列 24=一行两列）"),
            // entity.gentable.frontformlayout
            new TranslationSeedItem("entity.gentable.frontformlayout", "zh-CN", "前端表单布局", "前端表单布局（字典 code_generator_frontend_form_layout；12=一行一列 24=一行两列）"),
            // entity.gentable.frontformlayout
            new TranslationSeedItem("entity.gentable.frontformlayout", "zh-HK", "前端表单布局_hk", "前端表单布局（字典 code_generator_frontend_form_layout；12=一行一列 24=一行两列）"),

            // entity.gentable.frontbtnstyle
            new TranslationSeedItem("entity.gentable.frontbtnstyle", "en-US", "前端按钮样式_us", "前端按钮样式（字典 code_generator_button_style；0=文本 1=标准）"),
            // entity.gentable.frontbtnstyle
            new TranslationSeedItem("entity.gentable.frontbtnstyle", "ja-JP", "前端按钮样式_jp", "前端按钮样式（字典 code_generator_button_style；0=文本 1=标准）"),
            // entity.gentable.frontbtnstyle
            new TranslationSeedItem("entity.gentable.frontbtnstyle", "zh-CN", "前端按钮样式", "前端按钮样式（字典 code_generator_button_style；0=文本 1=标准）"),
            // entity.gentable.frontbtnstyle
            new TranslationSeedItem("entity.gentable.frontbtnstyle", "zh-HK", "前端按钮样式_hk", "前端按钮样式（字典 code_generator_button_style；0=文本 1=标准）"),

            // entity.gentable.isgencode
            new TranslationSeedItem("entity.gentable.isgencode", "en-US", "是否生成_us", "是否生成（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentable.isgencode
            new TranslationSeedItem("entity.gentable.isgencode", "ja-JP", "是否生成_jp", "是否生成（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentable.isgencode
            new TranslationSeedItem("entity.gentable.isgencode", "zh-CN", "是否生成", "是否生成（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentable.isgencode
            new TranslationSeedItem("entity.gentable.isgencode", "zh-HK", "是否生成_hk", "是否生成（字典 sys_yes_no；0=否 1=是）"),

            // entity.gentable.gencodecount
            new TranslationSeedItem("entity.gentable.gencodecount", "en-US", "代码生成次数_us", "代码生成次数（每次生成成功后自增）"),
            // entity.gentable.gencodecount
            new TranslationSeedItem("entity.gentable.gencodecount", "ja-JP", "代码生成次数_jp", "代码生成次数（每次生成成功后自增）"),
            // entity.gentable.gencodecount
            new TranslationSeedItem("entity.gentable.gencodecount", "zh-CN", "代码生成次数", "代码生成次数（每次生成成功后自增）"),
            // entity.gentable.gencodecount
            new TranslationSeedItem("entity.gentable.gencodecount", "zh-HK", "代码生成次数_hk", "代码生成次数（每次生成成功后自增）"),

            // entity.gentable.isusetabs
            new TranslationSeedItem("entity.gentable.isusetabs", "en-US", "使用tabs_us", "使用tabs（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentable.isusetabs
            new TranslationSeedItem("entity.gentable.isusetabs", "ja-JP", "使用tabs_jp", "使用tabs（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentable.isusetabs
            new TranslationSeedItem("entity.gentable.isusetabs", "zh-CN", "使用tabs", "使用tabs（字典 sys_yes_no；0=否 1=是）"),
            // entity.gentable.isusetabs
            new TranslationSeedItem("entity.gentable.isusetabs", "zh-HK", "使用tabs_hk", "使用tabs（字典 sys_yes_no；0=否 1=是）"),

            // entity.gentable.tabsfieldcount
            new TranslationSeedItem("entity.gentable.tabsfieldcount", "en-US", "tabs标签字段_us", "tabs标签中字段的数量"),
            // entity.gentable.tabsfieldcount
            new TranslationSeedItem("entity.gentable.tabsfieldcount", "ja-JP", "tabs标签字段_jp", "tabs标签中字段的数量"),
            // entity.gentable.tabsfieldcount
            new TranslationSeedItem("entity.gentable.tabsfieldcount", "zh-CN", "tabs标签字段", "tabs标签中字段的数量"),
            // entity.gentable.tabsfieldcount
            new TranslationSeedItem("entity.gentable.tabsfieldcount", "zh-HK", "tabs标签字段_hk", "tabs标签中字段的数量"),

            // entity.gentable.genauthor
            new TranslationSeedItem("entity.gentable.genauthor", "en-US", "作者_us", "作者"),
            // entity.gentable.genauthor
            new TranslationSeedItem("entity.gentable.genauthor", "ja-JP", "作者_jp", "作者"),
            // entity.gentable.genauthor
            new TranslationSeedItem("entity.gentable.genauthor", "zh-CN", "作者", "作者"),
            // entity.gentable.genauthor
            new TranslationSeedItem("entity.gentable.genauthor", "zh-HK", "作者_hk", "作者"),

            // entity.gentable.othergenoptions
            new TranslationSeedItem("entity.gentable.othergenoptions", "en-US", "其他生成选项_us", "其他生成选项（JSON格式，存储其他生成配置）"),
            // entity.gentable.othergenoptions
            new TranslationSeedItem("entity.gentable.othergenoptions", "ja-JP", "其他生成选项_jp", "其他生成选项（JSON格式，存储其他生成配置）"),
            // entity.gentable.othergenoptions
            new TranslationSeedItem("entity.gentable.othergenoptions", "zh-CN", "其他生成选项", "其他生成选项（JSON格式，存储其他生成配置）"),
            // entity.gentable.othergenoptions
            new TranslationSeedItem("entity.gentable.othergenoptions", "zh-HK", "其他生成选项_hk", "其他生成选项（JSON格式，存储其他生成配置）"),

            // entity.gentable.columns
            new TranslationSeedItem("entity.gentable.columns", "en-US", "字段配置列表_us", "字段配置列表（子表，外键：TaktGenTableColumn.GenTableId 关联本表 Id）"),
            // entity.gentable.columns
            new TranslationSeedItem("entity.gentable.columns", "ja-JP", "字段配置列表_jp", "字段配置列表（子表，外键：TaktGenTableColumn.GenTableId 关联本表 Id）"),
            // entity.gentable.columns
            new TranslationSeedItem("entity.gentable.columns", "zh-CN", "字段配置列表", "字段配置列表（子表，外键：TaktGenTableColumn.GenTableId 关联本表 Id）"),
            // entity.gentable.columns
            new TranslationSeedItem("entity.gentable.columns", "zh-HK", "字段配置列表_hk", "字段配置列表（子表，外键：TaktGenTableColumn.GenTableId 关联本表 Id）"),
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
