// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcDetailI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEcDetail 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// TaktEcDetail 实体国际化翻译种子（键前缀 entity.ecdetail.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEcDetailI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEcDetail 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ecdetail 实体翻译...", tenantCode);

        foreach (var item in GetEcDetailTranslations())
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

        TaktLogger.Information("TaktEcDetail 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEcDetail 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.ecdetail._self / entity.ecdetail.{{field}}；ResourceGroup=EngineeringChange；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEcDetailTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ecdetail._self
            new TranslationSeedItem("entity.ecdetail._self", "en-US", "Ec Detail Information_us", "实体名称"),
            // entity.ecdetail._self
            new TranslationSeedItem("entity.ecdetail._self", "ja-JP", "设变明细信息_jp", "实体名称"),
            // entity.ecdetail._self
            new TranslationSeedItem("entity.ecdetail._self", "zh-CN", "设变明细信息", "实体名称"),
            // entity.ecdetail._self
            new TranslationSeedItem("entity.ecdetail._self", "zh-HK", "设变明细信息_hk", "实体名称"),

            // entity.ecdetail.linenumber
            new TranslationSeedItem("entity.ecdetail.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.ecdetail.linenumber
            new TranslationSeedItem("entity.ecdetail.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.ecdetail.linenumber
            new TranslationSeedItem("entity.ecdetail.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.ecdetail.linenumber
            new TranslationSeedItem("entity.ecdetail.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.ecdetail.eccode
            new TranslationSeedItem("entity.ecdetail.eccode", "en-US", "设变单号_us", "设变单号（冗余字段,便于查询）"),
            // entity.ecdetail.eccode
            new TranslationSeedItem("entity.ecdetail.eccode", "ja-JP", "设变单号_jp", "设变单号（冗余字段,便于查询）"),
            // entity.ecdetail.eccode
            new TranslationSeedItem("entity.ecdetail.eccode", "zh-CN", "设变单号", "设变单号（冗余字段,便于查询）"),
            // entity.ecdetail.eccode
            new TranslationSeedItem("entity.ecdetail.eccode", "zh-HK", "设变单号_hk", "设变单号（冗余字段,便于查询）"),

            // entity.ecdetail.ecbomlinecode
            new TranslationSeedItem("entity.ecdetail.ecbomlinecode", "en-US", "BOM行号_us", "BOM行号"),
            // entity.ecdetail.ecbomlinecode
            new TranslationSeedItem("entity.ecdetail.ecbomlinecode", "ja-JP", "BOM行号_jp", "BOM行号"),
            // entity.ecdetail.ecbomlinecode
            new TranslationSeedItem("entity.ecdetail.ecbomlinecode", "zh-CN", "BOM行号", "BOM行号"),
            // entity.ecdetail.ecbomlinecode
            new TranslationSeedItem("entity.ecdetail.ecbomlinecode", "zh-HK", "BOM行号_hk", "BOM行号"),

            // entity.ecdetail.ecmodelcode
            new TranslationSeedItem("entity.ecdetail.ecmodelcode", "en-US", "机种编码_us", "机种编码（按根物料编码 EcRootMaterialCode 查询型号目的地回填）"),
            // entity.ecdetail.ecmodelcode
            new TranslationSeedItem("entity.ecdetail.ecmodelcode", "ja-JP", "机种编码_jp", "机种编码（按根物料编码 EcRootMaterialCode 查询型号目的地回填）"),
            // entity.ecdetail.ecmodelcode
            new TranslationSeedItem("entity.ecdetail.ecmodelcode", "zh-CN", "机种编码", "机种编码（按根物料编码 EcRootMaterialCode 查询型号目的地回填）"),
            // entity.ecdetail.ecmodelcode
            new TranslationSeedItem("entity.ecdetail.ecmodelcode", "zh-HK", "机种编码_hk", "机种编码（按根物料编码 EcRootMaterialCode 查询型号目的地回填）"),

            // entity.ecdetail.ecrootmaterialtype
            new TranslationSeedItem("entity.ecdetail.ecrootmaterialtype", "en-US", "根物料类型_us", "根物料类型（字典 logistics_materials_material_type；按根物料编码查询工厂物料回填。执行内容直接读本字段）"),
            // entity.ecdetail.ecrootmaterialtype
            new TranslationSeedItem("entity.ecdetail.ecrootmaterialtype", "ja-JP", "根物料类型_jp", "根物料类型（字典 logistics_materials_material_type；按根物料编码查询工厂物料回填。执行内容直接读本字段）"),
            // entity.ecdetail.ecrootmaterialtype
            new TranslationSeedItem("entity.ecdetail.ecrootmaterialtype", "zh-CN", "根物料类型", "根物料类型（字典 logistics_materials_material_type；按根物料编码查询工厂物料回填。执行内容直接读本字段）"),
            // entity.ecdetail.ecrootmaterialtype
            new TranslationSeedItem("entity.ecdetail.ecrootmaterialtype", "zh-HK", "根物料类型_hk", "根物料类型（字典 logistics_materials_material_type；按根物料编码查询工厂物料回填。执行内容直接读本字段）"),

            // entity.ecdetail.ecrootmaterialcode
            new TranslationSeedItem("entity.ecdetail.ecrootmaterialcode", "en-US", "根物料编码_us", "根物料编码"),
            // entity.ecdetail.ecrootmaterialcode
            new TranslationSeedItem("entity.ecdetail.ecrootmaterialcode", "ja-JP", "根物料编码_jp", "根物料编码"),
            // entity.ecdetail.ecrootmaterialcode
            new TranslationSeedItem("entity.ecdetail.ecrootmaterialcode", "zh-CN", "根物料编码", "根物料编码"),
            // entity.ecdetail.ecrootmaterialcode
            new TranslationSeedItem("entity.ecdetail.ecrootmaterialcode", "zh-HK", "根物料编码_hk", "根物料编码"),

            // entity.ecdetail.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecdetail.ecrootmaterialdescription", "en-US", "根物料描述_us", "根物料描述（按根物料编码 EcRootMaterialCode 查询工厂物料回填）"),
            // entity.ecdetail.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecdetail.ecrootmaterialdescription", "ja-JP", "根物料描述_jp", "根物料描述（按根物料编码 EcRootMaterialCode 查询工厂物料回填）"),
            // entity.ecdetail.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecdetail.ecrootmaterialdescription", "zh-CN", "根物料描述", "根物料描述（按根物料编码 EcRootMaterialCode 查询工厂物料回填）"),
            // entity.ecdetail.ecrootmaterialdescription
            new TranslationSeedItem("entity.ecdetail.ecrootmaterialdescription", "zh-HK", "根物料描述_hk", "根物料描述（按根物料编码 EcRootMaterialCode 查询工厂物料回填）"),

            // entity.ecdetail.discontinuedstatus
            new TranslationSeedItem("entity.ecdetail.discontinuedstatus", "en-US", "停产状态_us", "停产状态（按根物料编码 EcRootMaterialCode 查询工厂物料回填）"),
            // entity.ecdetail.discontinuedstatus
            new TranslationSeedItem("entity.ecdetail.discontinuedstatus", "ja-JP", "停产状态_jp", "停产状态（按根物料编码 EcRootMaterialCode 查询工厂物料回填）"),
            // entity.ecdetail.discontinuedstatus
            new TranslationSeedItem("entity.ecdetail.discontinuedstatus", "zh-CN", "停产状态", "停产状态（按根物料编码 EcRootMaterialCode 查询工厂物料回填）"),
            // entity.ecdetail.discontinuedstatus
            new TranslationSeedItem("entity.ecdetail.discontinuedstatus", "zh-HK", "停产状态_hk", "停产状态（按根物料编码 EcRootMaterialCode 查询工厂物料回填）"),

            // entity.ecdetail.ecparentmaterialcode
            new TranslationSeedItem("entity.ecdetail.ecparentmaterialcode", "en-US", "上阶物料编码_us", "上阶物料编码"),
            // entity.ecdetail.ecparentmaterialcode
            new TranslationSeedItem("entity.ecdetail.ecparentmaterialcode", "ja-JP", "上阶物料编码_jp", "上阶物料编码"),
            // entity.ecdetail.ecparentmaterialcode
            new TranslationSeedItem("entity.ecdetail.ecparentmaterialcode", "zh-CN", "上阶物料编码", "上阶物料编码"),
            // entity.ecdetail.ecparentmaterialcode
            new TranslationSeedItem("entity.ecdetail.ecparentmaterialcode", "zh-HK", "上阶物料编码_hk", "上阶物料编码"),

            // entity.ecdetail.ecparentmaterialdescription
            new TranslationSeedItem("entity.ecdetail.ecparentmaterialdescription", "en-US", "上阶物料描述_us", "上阶物料描述（按上阶物料编码 EcParentMaterialCode 查询工厂物料回填）"),
            // entity.ecdetail.ecparentmaterialdescription
            new TranslationSeedItem("entity.ecdetail.ecparentmaterialdescription", "ja-JP", "上阶物料描述_jp", "上阶物料描述（按上阶物料编码 EcParentMaterialCode 查询工厂物料回填）"),
            // entity.ecdetail.ecparentmaterialdescription
            new TranslationSeedItem("entity.ecdetail.ecparentmaterialdescription", "zh-CN", "上阶物料描述", "上阶物料描述（按上阶物料编码 EcParentMaterialCode 查询工厂物料回填）"),
            // entity.ecdetail.ecparentmaterialdescription
            new TranslationSeedItem("entity.ecdetail.ecparentmaterialdescription", "zh-HK", "上阶物料描述_hk", "上阶物料描述（按上阶物料编码 EcParentMaterialCode 查询工厂物料回填）"),

            // entity.ecdetail.ecoldmaterialcode
            new TranslationSeedItem("entity.ecdetail.ecoldmaterialcode", "en-US", "旧物料编码_us", "旧物料编码"),
            // entity.ecdetail.ecoldmaterialcode
            new TranslationSeedItem("entity.ecdetail.ecoldmaterialcode", "ja-JP", "旧物料编码_jp", "旧物料编码"),
            // entity.ecdetail.ecoldmaterialcode
            new TranslationSeedItem("entity.ecdetail.ecoldmaterialcode", "zh-CN", "旧物料编码", "旧物料编码"),
            // entity.ecdetail.ecoldmaterialcode
            new TranslationSeedItem("entity.ecdetail.ecoldmaterialcode", "zh-HK", "旧物料编码_hk", "旧物料编码"),

            // entity.ecdetail.ecoldmaterialdescription
            new TranslationSeedItem("entity.ecdetail.ecoldmaterialdescription", "en-US", "旧物料描述_us", "旧物料描述"),
            // entity.ecdetail.ecoldmaterialdescription
            new TranslationSeedItem("entity.ecdetail.ecoldmaterialdescription", "ja-JP", "旧物料描述_jp", "旧物料描述"),
            // entity.ecdetail.ecoldmaterialdescription
            new TranslationSeedItem("entity.ecdetail.ecoldmaterialdescription", "zh-CN", "旧物料描述", "旧物料描述"),
            // entity.ecdetail.ecoldmaterialdescription
            new TranslationSeedItem("entity.ecdetail.ecoldmaterialdescription", "zh-HK", "旧物料描述_hk", "旧物料描述"),

            // entity.ecdetail.ecoldusagequantity
            new TranslationSeedItem("entity.ecdetail.ecoldusagequantity", "en-US", "旧用量_us", "旧用量"),
            // entity.ecdetail.ecoldusagequantity
            new TranslationSeedItem("entity.ecdetail.ecoldusagequantity", "ja-JP", "旧用量_jp", "旧用量"),
            // entity.ecdetail.ecoldusagequantity
            new TranslationSeedItem("entity.ecdetail.ecoldusagequantity", "zh-CN", "旧用量", "旧用量"),
            // entity.ecdetail.ecoldusagequantity
            new TranslationSeedItem("entity.ecdetail.ecoldusagequantity", "zh-HK", "旧用量_hk", "旧用量"),

            // entity.ecdetail.ecolditemposition
            new TranslationSeedItem("entity.ecdetail.ecolditemposition", "en-US", "旧位置_us", "旧位置"),
            // entity.ecdetail.ecolditemposition
            new TranslationSeedItem("entity.ecdetail.ecolditemposition", "ja-JP", "旧位置_jp", "旧位置"),
            // entity.ecdetail.ecolditemposition
            new TranslationSeedItem("entity.ecdetail.ecolditemposition", "zh-CN", "旧位置", "旧位置"),
            // entity.ecdetail.ecolditemposition
            new TranslationSeedItem("entity.ecdetail.ecolditemposition", "zh-HK", "旧位置_hk", "旧位置"),

            // entity.ecdetail.ecoldstock
            new TranslationSeedItem("entity.ecdetail.ecoldstock", "en-US", "旧品在库_us", "旧品在库（按旧物料编码 EcOldMaterialCode 查询工厂物料回填）"),
            // entity.ecdetail.ecoldstock
            new TranslationSeedItem("entity.ecdetail.ecoldstock", "ja-JP", "旧品在库_jp", "旧品在库（按旧物料编码 EcOldMaterialCode 查询工厂物料回填）"),
            // entity.ecdetail.ecoldstock
            new TranslationSeedItem("entity.ecdetail.ecoldstock", "zh-CN", "旧品在库", "旧品在库（按旧物料编码 EcOldMaterialCode 查询工厂物料回填）"),
            // entity.ecdetail.ecoldstock
            new TranslationSeedItem("entity.ecdetail.ecoldstock", "zh-HK", "旧品在库_hk", "旧品在库（按旧物料编码 EcOldMaterialCode 查询工厂物料回填）"),

            // entity.ecdetail.ecoldwarehouse
            new TranslationSeedItem("entity.ecdetail.ecoldwarehouse", "en-US", "旧品仓库_us", "旧品仓库（按旧物料编码 EcOldMaterialCode 查询工厂物料回填）"),
            // entity.ecdetail.ecoldwarehouse
            new TranslationSeedItem("entity.ecdetail.ecoldwarehouse", "ja-JP", "旧品仓库_jp", "旧品仓库（按旧物料编码 EcOldMaterialCode 查询工厂物料回填）"),
            // entity.ecdetail.ecoldwarehouse
            new TranslationSeedItem("entity.ecdetail.ecoldwarehouse", "zh-CN", "旧品仓库", "旧品仓库（按旧物料编码 EcOldMaterialCode 查询工厂物料回填）"),
            // entity.ecdetail.ecoldwarehouse
            new TranslationSeedItem("entity.ecdetail.ecoldwarehouse", "zh-HK", "旧品仓库_hk", "旧品仓库（按旧物料编码 EcOldMaterialCode 查询工厂物料回填）"),

            // entity.ecdetail.ecoldpurchasetype
            new TranslationSeedItem("entity.ecdetail.ecoldpurchasetype", "en-US", "旧采购类型_us", "旧采购类型（按旧物料编码 EcOldMaterialCode 查询工厂物料回填）"),
            // entity.ecdetail.ecoldpurchasetype
            new TranslationSeedItem("entity.ecdetail.ecoldpurchasetype", "ja-JP", "旧采购类型_jp", "旧采购类型（按旧物料编码 EcOldMaterialCode 查询工厂物料回填）"),
            // entity.ecdetail.ecoldpurchasetype
            new TranslationSeedItem("entity.ecdetail.ecoldpurchasetype", "zh-CN", "旧采购类型", "旧采购类型（按旧物料编码 EcOldMaterialCode 查询工厂物料回填）"),
            // entity.ecdetail.ecoldpurchasetype
            new TranslationSeedItem("entity.ecdetail.ecoldpurchasetype", "zh-HK", "旧采购类型_hk", "旧采购类型（按旧物料编码 EcOldMaterialCode 查询工厂物料回填）"),

            // entity.ecdetail.ecoldrequiresinspection
            new TranslationSeedItem("entity.ecdetail.ecoldrequiresinspection", "en-US", "旧品检验_us", "旧品检验（按旧物料编码 EcOldMaterialCode 查询工厂物料回填）"),
            // entity.ecdetail.ecoldrequiresinspection
            new TranslationSeedItem("entity.ecdetail.ecoldrequiresinspection", "ja-JP", "旧品检验_jp", "旧品检验（按旧物料编码 EcOldMaterialCode 查询工厂物料回填）"),
            // entity.ecdetail.ecoldrequiresinspection
            new TranslationSeedItem("entity.ecdetail.ecoldrequiresinspection", "zh-CN", "旧品检验", "旧品检验（按旧物料编码 EcOldMaterialCode 查询工厂物料回填）"),
            // entity.ecdetail.ecoldrequiresinspection
            new TranslationSeedItem("entity.ecdetail.ecoldrequiresinspection", "zh-HK", "旧品检验_hk", "旧品检验（按旧物料编码 EcOldMaterialCode 查询工厂物料回填）"),

            // entity.ecdetail.ecnewmaterialcode
            new TranslationSeedItem("entity.ecdetail.ecnewmaterialcode", "en-US", "新物料编码_us", "新物料编码"),
            // entity.ecdetail.ecnewmaterialcode
            new TranslationSeedItem("entity.ecdetail.ecnewmaterialcode", "ja-JP", "新物料编码_jp", "新物料编码"),
            // entity.ecdetail.ecnewmaterialcode
            new TranslationSeedItem("entity.ecdetail.ecnewmaterialcode", "zh-CN", "新物料编码", "新物料编码"),
            // entity.ecdetail.ecnewmaterialcode
            new TranslationSeedItem("entity.ecdetail.ecnewmaterialcode", "zh-HK", "新物料编码_hk", "新物料编码"),

            // entity.ecdetail.ecnewmaterialdescription
            new TranslationSeedItem("entity.ecdetail.ecnewmaterialdescription", "en-US", "新物料描述_us", "新物料描述"),
            // entity.ecdetail.ecnewmaterialdescription
            new TranslationSeedItem("entity.ecdetail.ecnewmaterialdescription", "ja-JP", "新物料描述_jp", "新物料描述"),
            // entity.ecdetail.ecnewmaterialdescription
            new TranslationSeedItem("entity.ecdetail.ecnewmaterialdescription", "zh-CN", "新物料描述", "新物料描述"),
            // entity.ecdetail.ecnewmaterialdescription
            new TranslationSeedItem("entity.ecdetail.ecnewmaterialdescription", "zh-HK", "新物料描述_hk", "新物料描述"),

            // entity.ecdetail.ecnewusagequantity
            new TranslationSeedItem("entity.ecdetail.ecnewusagequantity", "en-US", "新用量_us", "新用量"),
            // entity.ecdetail.ecnewusagequantity
            new TranslationSeedItem("entity.ecdetail.ecnewusagequantity", "ja-JP", "新用量_jp", "新用量"),
            // entity.ecdetail.ecnewusagequantity
            new TranslationSeedItem("entity.ecdetail.ecnewusagequantity", "zh-CN", "新用量", "新用量"),
            // entity.ecdetail.ecnewusagequantity
            new TranslationSeedItem("entity.ecdetail.ecnewusagequantity", "zh-HK", "新用量_hk", "新用量"),

            // entity.ecdetail.ecnewitemposition
            new TranslationSeedItem("entity.ecdetail.ecnewitemposition", "en-US", "新位置_us", "新位置"),
            // entity.ecdetail.ecnewitemposition
            new TranslationSeedItem("entity.ecdetail.ecnewitemposition", "ja-JP", "新位置_jp", "新位置"),
            // entity.ecdetail.ecnewitemposition
            new TranslationSeedItem("entity.ecdetail.ecnewitemposition", "zh-CN", "新位置", "新位置"),
            // entity.ecdetail.ecnewitemposition
            new TranslationSeedItem("entity.ecdetail.ecnewitemposition", "zh-HK", "新位置_hk", "新位置"),

            // entity.ecdetail.ecnewstock
            new TranslationSeedItem("entity.ecdetail.ecnewstock", "en-US", "新品在库_us", "新品在库（按新物料编码 EcNewMaterialCode 查询工厂物料回填）"),
            // entity.ecdetail.ecnewstock
            new TranslationSeedItem("entity.ecdetail.ecnewstock", "ja-JP", "新品在库_jp", "新品在库（按新物料编码 EcNewMaterialCode 查询工厂物料回填）"),
            // entity.ecdetail.ecnewstock
            new TranslationSeedItem("entity.ecdetail.ecnewstock", "zh-CN", "新品在库", "新品在库（按新物料编码 EcNewMaterialCode 查询工厂物料回填）"),
            // entity.ecdetail.ecnewstock
            new TranslationSeedItem("entity.ecdetail.ecnewstock", "zh-HK", "新品在库_hk", "新品在库（按新物料编码 EcNewMaterialCode 查询工厂物料回填）"),

            // entity.ecdetail.ecnewwarehouse
            new TranslationSeedItem("entity.ecdetail.ecnewwarehouse", "en-US", "新品仓库_us", "新品仓库（按新物料编码 EcNewMaterialCode 查询工厂物料回填）"),
            // entity.ecdetail.ecnewwarehouse
            new TranslationSeedItem("entity.ecdetail.ecnewwarehouse", "ja-JP", "新品仓库_jp", "新品仓库（按新物料编码 EcNewMaterialCode 查询工厂物料回填）"),
            // entity.ecdetail.ecnewwarehouse
            new TranslationSeedItem("entity.ecdetail.ecnewwarehouse", "zh-CN", "新品仓库", "新品仓库（按新物料编码 EcNewMaterialCode 查询工厂物料回填）"),
            // entity.ecdetail.ecnewwarehouse
            new TranslationSeedItem("entity.ecdetail.ecnewwarehouse", "zh-HK", "新品仓库_hk", "新品仓库（按新物料编码 EcNewMaterialCode 查询工厂物料回填）"),

            // entity.ecdetail.ecnewpurchasetype
            new TranslationSeedItem("entity.ecdetail.ecnewpurchasetype", "en-US", "新采购类型_us", "新采购类型（按新物料编码 EcNewMaterialCode 查询工厂物料回填）"),
            // entity.ecdetail.ecnewpurchasetype
            new TranslationSeedItem("entity.ecdetail.ecnewpurchasetype", "ja-JP", "新采购类型_jp", "新采购类型（按新物料编码 EcNewMaterialCode 查询工厂物料回填）"),
            // entity.ecdetail.ecnewpurchasetype
            new TranslationSeedItem("entity.ecdetail.ecnewpurchasetype", "zh-CN", "新采购类型", "新采购类型（按新物料编码 EcNewMaterialCode 查询工厂物料回填）"),
            // entity.ecdetail.ecnewpurchasetype
            new TranslationSeedItem("entity.ecdetail.ecnewpurchasetype", "zh-HK", "新采购类型_hk", "新采购类型（按新物料编码 EcNewMaterialCode 查询工厂物料回填）"),

            // entity.ecdetail.ecnewrequiresinspection
            new TranslationSeedItem("entity.ecdetail.ecnewrequiresinspection", "en-US", "新品检验_us", "新品检验（按新物料编码 EcNewMaterialCode 查询工厂物料回填）"),
            // entity.ecdetail.ecnewrequiresinspection
            new TranslationSeedItem("entity.ecdetail.ecnewrequiresinspection", "ja-JP", "新品检验_jp", "新品检验（按新物料编码 EcNewMaterialCode 查询工厂物料回填）"),
            // entity.ecdetail.ecnewrequiresinspection
            new TranslationSeedItem("entity.ecdetail.ecnewrequiresinspection", "zh-CN", "新品检验", "新品检验（按新物料编码 EcNewMaterialCode 查询工厂物料回填）"),
            // entity.ecdetail.ecnewrequiresinspection
            new TranslationSeedItem("entity.ecdetail.ecnewrequiresinspection", "zh-HK", "新品检验_hk", "新品检验（按新物料编码 EcNewMaterialCode 查询工厂物料回填）"),

            // entity.ecdetail.ecbomdate
            new TranslationSeedItem("entity.ecdetail.ecbomdate", "en-US", "BOM生效日期_us", "BOM生效日期"),
            // entity.ecdetail.ecbomdate
            new TranslationSeedItem("entity.ecdetail.ecbomdate", "ja-JP", "BOM生效日期_jp", "BOM生效日期"),
            // entity.ecdetail.ecbomdate
            new TranslationSeedItem("entity.ecdetail.ecbomdate", "zh-CN", "BOM生效日期", "BOM生效日期"),
            // entity.ecdetail.ecbomdate
            new TranslationSeedItem("entity.ecdetail.ecbomdate", "zh-HK", "BOM生效日期_hk", "BOM生效日期"),

            // entity.ecdetail.eciscompatible
            new TranslationSeedItem("entity.ecdetail.eciscompatible", "en-US", "兼容性_us", "兼容性"),
            // entity.ecdetail.eciscompatible
            new TranslationSeedItem("entity.ecdetail.eciscompatible", "ja-JP", "兼容性_jp", "兼容性"),
            // entity.ecdetail.eciscompatible
            new TranslationSeedItem("entity.ecdetail.eciscompatible", "zh-CN", "兼容性", "兼容性"),
            // entity.ecdetail.eciscompatible
            new TranslationSeedItem("entity.ecdetail.eciscompatible", "zh-HK", "兼容性_hk", "兼容性"),

            // entity.ecdetail.ec2ndvendor
            new TranslationSeedItem("entity.ecdetail.ec2ndvendor", "en-US", "第二供应商_us", "第二供应商（字典 logistics_manufacturing_ec_2nd_vendor；1=有 2=优先 3=无）"),
            // entity.ecdetail.ec2ndvendor
            new TranslationSeedItem("entity.ecdetail.ec2ndvendor", "ja-JP", "第二供应商_jp", "第二供应商（字典 logistics_manufacturing_ec_2nd_vendor；1=有 2=优先 3=无）"),
            // entity.ecdetail.ec2ndvendor
            new TranslationSeedItem("entity.ecdetail.ec2ndvendor", "zh-CN", "第二供应商", "第二供应商（字典 logistics_manufacturing_ec_2nd_vendor；1=有 2=优先 3=无）"),
            // entity.ecdetail.ec2ndvendor
            new TranslationSeedItem("entity.ecdetail.ec2ndvendor", "zh-HK", "第二供应商_hk", "第二供应商（字典 logistics_manufacturing_ec_2nd_vendor；1=有 2=优先 3=无）"),

            // entity.ecdetail.ecinstruction
            new TranslationSeedItem("entity.ecdetail.ecinstruction", "en-US", "生产指令_us", "生产指令"),
            // entity.ecdetail.ecinstruction
            new TranslationSeedItem("entity.ecdetail.ecinstruction", "ja-JP", "生产指令_jp", "生产指令"),
            // entity.ecdetail.ecinstruction
            new TranslationSeedItem("entity.ecdetail.ecinstruction", "zh-CN", "生产指令", "生产指令"),
            // entity.ecdetail.ecinstruction
            new TranslationSeedItem("entity.ecdetail.ecinstruction", "zh-HK", "生产指令_hk", "生产指令"),

            // entity.ecdetail.ecoldpartdisposition
            new TranslationSeedItem("entity.ecdetail.ecoldpartdisposition", "en-US", "旧品处理_us", "旧品处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用 2=废弃 3=返工 4=消耗 5=无处理 9=未定）"),
            // entity.ecdetail.ecoldpartdisposition
            new TranslationSeedItem("entity.ecdetail.ecoldpartdisposition", "ja-JP", "旧品处理_jp", "旧品处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用 2=废弃 3=返工 4=消耗 5=无处理 9=未定）"),
            // entity.ecdetail.ecoldpartdisposition
            new TranslationSeedItem("entity.ecdetail.ecoldpartdisposition", "zh-CN", "旧品处理", "旧品处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用 2=废弃 3=返工 4=消耗 5=无处理 9=未定）"),
            // entity.ecdetail.ecoldpartdisposition
            new TranslationSeedItem("entity.ecdetail.ecoldpartdisposition", "zh-HK", "旧品处理_hk", "旧品处理（字典 logistics_manufacturing_ec_old_part_disposition；1=转用 2=废弃 3=返工 4=消耗 5=无处理 9=未定）"),

            // entity.ecdetail.ecimplscope
            new TranslationSeedItem("entity.ecdetail.ecimplscope", "en-US", "实施范围_us", "实施范围（冗余：来自 TaktEcTe.EcImplScope）"),
            // entity.ecdetail.ecimplscope
            new TranslationSeedItem("entity.ecdetail.ecimplscope", "ja-JP", "实施范围_jp", "实施范围（冗余：来自 TaktEcTe.EcImplScope）"),
            // entity.ecdetail.ecimplscope
            new TranslationSeedItem("entity.ecdetail.ecimplscope", "zh-CN", "实施范围", "实施范围（冗余：来自 TaktEcTe.EcImplScope）"),
            // entity.ecdetail.ecimplscope
            new TranslationSeedItem("entity.ecdetail.ecimplscope", "zh-HK", "实施范围_hk", "实施范围（冗余：来自 TaktEcTe.EcImplScope）"),

            // entity.ecdetail.ecteid
            new TranslationSeedItem("entity.ecdetail.ecteid", "en-US", "技术课主表ID_us", "技术课主表 ID（TaktEcTe 主键；序列化为 string 避免 Javascript 精度问题）"),
            // entity.ecdetail.ecteid
            new TranslationSeedItem("entity.ecdetail.ecteid", "ja-JP", "技术课主表ID_jp", "技术课主表 ID（TaktEcTe 主键；序列化为 string 避免 Javascript 精度问题）"),
            // entity.ecdetail.ecteid
            new TranslationSeedItem("entity.ecdetail.ecteid", "zh-CN", "技术课主表ID", "技术课主表 ID（TaktEcTe 主键；序列化为 string 避免 Javascript 精度问题）"),
            // entity.ecdetail.ecteid
            new TranslationSeedItem("entity.ecdetail.ecteid", "zh-HK", "技术课主表ID_hk", "技术课主表 ID（TaktEcTe 主键；序列化为 string 避免 Javascript 精度问题）"),

            // entity.ecdetail.isobsolete
            new TranslationSeedItem("entity.ecdetail.isobsolete", "en-US", "是否作废_us", "是否作废"),
            // entity.ecdetail.isobsolete
            new TranslationSeedItem("entity.ecdetail.isobsolete", "ja-JP", "是否作废_jp", "是否作废"),
            // entity.ecdetail.isobsolete
            new TranslationSeedItem("entity.ecdetail.isobsolete", "zh-CN", "是否作废", "是否作废"),
            // entity.ecdetail.isobsolete
            new TranslationSeedItem("entity.ecdetail.isobsolete", "zh-HK", "是否作废_hk", "是否作废"),
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
        translation.ResourceGroup = "EngineeringChange";
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
