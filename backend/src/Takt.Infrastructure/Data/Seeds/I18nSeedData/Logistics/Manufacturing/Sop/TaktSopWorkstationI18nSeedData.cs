// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Sop
// 文件名称：TaktSopWorkstationI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSopWorkstation 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Sop;

/// <summary>
/// TaktSopWorkstation 实体国际化翻译种子（键前缀 entity.sopworkstation.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSopWorkstationI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSopWorkstation 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 sopworkstation 实体翻译...", tenantCode);

        foreach (var item in GetSopWorkstationTranslations())
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

        TaktLogger.Information("TaktSopWorkstation 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSopWorkstation 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.sopworkstation._self / entity.sopworkstation.{{field}}；ResourceGroup=Sop；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSopWorkstationTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.sopworkstation._self
            new TranslationSeedItem("entity.sopworkstation._self", "en-US", "Sop Workstation Information_us", "实体名称"),
            // entity.sopworkstation._self
            new TranslationSeedItem("entity.sopworkstation._self", "ja-JP", "SOP 工位主数据信息_jp", "实体名称"),
            // entity.sopworkstation._self
            new TranslationSeedItem("entity.sopworkstation._self", "zh-CN", "SOP 工位主数据信息", "实体名称"),
            // entity.sopworkstation._self
            new TranslationSeedItem("entity.sopworkstation._self", "zh-HK", "SOP 工位主数据信息_hk", "实体名称"),

            // entity.sopworkstation.workstationcode
            new TranslationSeedItem("entity.sopworkstation.workstationcode", "en-US", "工位编码_us", "工位编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 工位编码规则生成并展示，非手输；单据类型菜单：工位管理）"),
            // entity.sopworkstation.workstationcode
            new TranslationSeedItem("entity.sopworkstation.workstationcode", "ja-JP", "工位编码_jp", "工位编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 工位编码规则生成并展示，非手输；单据类型菜单：工位管理）"),
            // entity.sopworkstation.workstationcode
            new TranslationSeedItem("entity.sopworkstation.workstationcode", "zh-CN", "工位编码", "工位编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 工位编码规则生成并展示，非手输；单据类型菜单：工位管理）"),
            // entity.sopworkstation.workstationcode
            new TranslationSeedItem("entity.sopworkstation.workstationcode", "zh-HK", "工位编码_hk", "工位编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 工位编码规则生成并展示，非手输；单据类型菜单：工位管理）"),

            // entity.sopworkstation.workstationname
            new TranslationSeedItem("entity.sopworkstation.workstationname", "en-US", "工位名称_us", "工位名称"),
            // entity.sopworkstation.workstationname
            new TranslationSeedItem("entity.sopworkstation.workstationname", "ja-JP", "工位名称_jp", "工位名称"),
            // entity.sopworkstation.workstationname
            new TranslationSeedItem("entity.sopworkstation.workstationname", "zh-CN", "工位名称", "工位名称"),
            // entity.sopworkstation.workstationname
            new TranslationSeedItem("entity.sopworkstation.workstationname", "zh-HK", "工位名称_hk", "工位名称"),

            // entity.sopworkstation.workcenter
            new TranslationSeedItem("entity.sopworkstation.workcenter", "en-US", "工作中心_us", "工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）"),
            // entity.sopworkstation.workcenter
            new TranslationSeedItem("entity.sopworkstation.workcenter", "ja-JP", "工作中心_jp", "工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）"),
            // entity.sopworkstation.workcenter
            new TranslationSeedItem("entity.sopworkstation.workcenter", "zh-CN", "工作中心", "工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）"),
            // entity.sopworkstation.workcenter
            new TranslationSeedItem("entity.sopworkstation.workcenter", "zh-HK", "工作中心_hk", "工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）"),

            // entity.sopworkstation.productionline
            new TranslationSeedItem("entity.sopworkstation.productionline", "en-US", "生产班组_us", "生产班组"),
            // entity.sopworkstation.productionline
            new TranslationSeedItem("entity.sopworkstation.productionline", "ja-JP", "生产班组_jp", "生产班组"),
            // entity.sopworkstation.productionline
            new TranslationSeedItem("entity.sopworkstation.productionline", "zh-CN", "生产班组", "生产班组"),
            // entity.sopworkstation.productionline
            new TranslationSeedItem("entity.sopworkstation.productionline", "zh-HK", "生产班组_hk", "生产班组"),

            // entity.sopworkstation.workstationtype
            new TranslationSeedItem("entity.sopworkstation.workstationtype", "en-US", "工位类型_us", "工位类型（字典 sys_workstation_type；1=装配，2=检验，3=包装，4=测试，5=其他）"),
            // entity.sopworkstation.workstationtype
            new TranslationSeedItem("entity.sopworkstation.workstationtype", "ja-JP", "工位类型_jp", "工位类型（字典 sys_workstation_type；1=装配，2=检验，3=包装，4=测试，5=其他）"),
            // entity.sopworkstation.workstationtype
            new TranslationSeedItem("entity.sopworkstation.workstationtype", "zh-CN", "工位类型", "工位类型（字典 sys_workstation_type；1=装配，2=检验，3=包装，4=测试，5=其他）"),
            // entity.sopworkstation.workstationtype
            new TranslationSeedItem("entity.sopworkstation.workstationtype", "zh-HK", "工位类型_hk", "工位类型（字典 sys_workstation_type；1=装配，2=检验，3=包装，4=测试，5=其他）"),

            // entity.sopworkstation.processsegmenttype
            new TranslationSeedItem("entity.sopworkstation.processsegmenttype", "en-US", "工艺段类型_us", "工艺段类型（字典 logistics_manufacturing_process_segment_type；1=SMT，2=自插，3=手插，4=修正，5=总装）"),
            // entity.sopworkstation.processsegmenttype
            new TranslationSeedItem("entity.sopworkstation.processsegmenttype", "ja-JP", "工艺段类型_jp", "工艺段类型（字典 logistics_manufacturing_process_segment_type；1=SMT，2=自插，3=手插，4=修正，5=总装）"),
            // entity.sopworkstation.processsegmenttype
            new TranslationSeedItem("entity.sopworkstation.processsegmenttype", "zh-CN", "工艺段类型", "工艺段类型（字典 logistics_manufacturing_process_segment_type；1=SMT，2=自插，3=手插，4=修正，5=总装）"),
            // entity.sopworkstation.processsegmenttype
            new TranslationSeedItem("entity.sopworkstation.processsegmenttype", "zh-HK", "工艺段类型_hk", "工艺段类型（字典 logistics_manufacturing_process_segment_type；1=SMT，2=自插，3=手插，4=修正，5=总装）"),

            // entity.sopworkstation.workstationstatus
            new TranslationSeedItem("entity.sopworkstation.workstationstatus", "en-US", "启用状态_us", "启用状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）"),
            // entity.sopworkstation.workstationstatus
            new TranslationSeedItem("entity.sopworkstation.workstationstatus", "ja-JP", "启用状态_jp", "启用状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）"),
            // entity.sopworkstation.workstationstatus
            new TranslationSeedItem("entity.sopworkstation.workstationstatus", "zh-CN", "启用状态", "启用状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）"),
            // entity.sopworkstation.workstationstatus
            new TranslationSeedItem("entity.sopworkstation.workstationstatus", "zh-HK", "启用状态_hk", "启用状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）"),

            // entity.sopworkstation.sortorder
            new TranslationSeedItem("entity.sopworkstation.sortorder", "en-US", "排序号_us", "排序号（回填）"),
            // entity.sopworkstation.sortorder
            new TranslationSeedItem("entity.sopworkstation.sortorder", "ja-JP", "排序号_jp", "排序号（回填）"),
            // entity.sopworkstation.sortorder
            new TranslationSeedItem("entity.sopworkstation.sortorder", "zh-CN", "排序号", "排序号（回填）"),
            // entity.sopworkstation.sortorder
            new TranslationSeedItem("entity.sopworkstation.sortorder", "zh-HK", "排序号_hk", "排序号（回填）"),
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
        translation.ResourceGroup = "Sop";
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
