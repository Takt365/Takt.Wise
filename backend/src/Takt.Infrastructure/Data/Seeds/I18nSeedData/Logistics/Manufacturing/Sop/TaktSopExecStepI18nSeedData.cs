// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Sop
// 文件名称：TaktSopExecStepI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSopExecStep 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktSopExecStep 实体国际化翻译种子（键前缀 entity.sopexecstep.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSopExecStepI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSopExecStep 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 sopexecstep 实体翻译...", tenantCode);

        foreach (var item in GetSopExecStepTranslations())
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

        TaktLogger.Information("TaktSopExecStep 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSopExecStep 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.sopexecstep._self / entity.sopexecstep.{{field}}；ResourceGroup=Sop；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSopExecStepTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.sopexecstep._self
            new TranslationSeedItem("entity.sopexecstep._self", "en-US", "Sop Exec Step Information_us", "实体名称"),
            // entity.sopexecstep._self
            new TranslationSeedItem("entity.sopexecstep._self", "ja-JP", "SOP 工步执行明细信息_jp", "实体名称"),
            // entity.sopexecstep._self
            new TranslationSeedItem("entity.sopexecstep._self", "zh-CN", "SOP 工步执行明细信息", "实体名称"),
            // entity.sopexecstep._self
            new TranslationSeedItem("entity.sopexecstep._self", "zh-HK", "SOP 工步执行明细信息_hk", "实体名称"),

            // entity.sopexecstep.execid
            new TranslationSeedItem("entity.sopexecstep.execid", "en-US", "执行追溯ID_us", "执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）"),
            // entity.sopexecstep.execid
            new TranslationSeedItem("entity.sopexecstep.execid", "ja-JP", "执行追溯ID_jp", "执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）"),
            // entity.sopexecstep.execid
            new TranslationSeedItem("entity.sopexecstep.execid", "zh-CN", "执行追溯ID", "执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）"),
            // entity.sopexecstep.execid
            new TranslationSeedItem("entity.sopexecstep.execid", "zh-HK", "执行追溯ID_hk", "执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）"),

            // entity.sopexecstep.stepid
            new TranslationSeedItem("entity.sopexecstep.stepid", "en-US", "工步ID_us", "工步 ID（选项 TaktSopSteps/options；DictValue=Id）"),
            // entity.sopexecstep.stepid
            new TranslationSeedItem("entity.sopexecstep.stepid", "ja-JP", "工步ID_jp", "工步 ID（选项 TaktSopSteps/options；DictValue=Id）"),
            // entity.sopexecstep.stepid
            new TranslationSeedItem("entity.sopexecstep.stepid", "zh-CN", "工步ID", "工步 ID（选项 TaktSopSteps/options；DictValue=Id）"),
            // entity.sopexecstep.stepid
            new TranslationSeedItem("entity.sopexecstep.stepid", "zh-HK", "工步ID_hk", "工步 ID（选项 TaktSopSteps/options；DictValue=Id）"),

            // entity.sopexecstep.stepno
            new TranslationSeedItem("entity.sopexecstep.stepno", "en-US", "工步序号_us", "工步序号快照"),
            // entity.sopexecstep.stepno
            new TranslationSeedItem("entity.sopexecstep.stepno", "ja-JP", "工步序号_jp", "工步序号快照"),
            // entity.sopexecstep.stepno
            new TranslationSeedItem("entity.sopexecstep.stepno", "zh-CN", "工步序号", "工步序号快照"),
            // entity.sopexecstep.stepno
            new TranslationSeedItem("entity.sopexecstep.stepno", "zh-HK", "工步序号_hk", "工步序号快照"),

            // entity.sopexecstep.startedat
            new TranslationSeedItem("entity.sopexecstep.startedat", "en-US", "开始时间_us", "开始时间"),
            // entity.sopexecstep.startedat
            new TranslationSeedItem("entity.sopexecstep.startedat", "ja-JP", "开始时间_jp", "开始时间"),
            // entity.sopexecstep.startedat
            new TranslationSeedItem("entity.sopexecstep.startedat", "zh-CN", "开始时间", "开始时间"),
            // entity.sopexecstep.startedat
            new TranslationSeedItem("entity.sopexecstep.startedat", "zh-HK", "开始时间_hk", "开始时间"),

            // entity.sopexecstep.endedat
            new TranslationSeedItem("entity.sopexecstep.endedat", "en-US", "结束时间_us", "结束时间"),
            // entity.sopexecstep.endedat
            new TranslationSeedItem("entity.sopexecstep.endedat", "ja-JP", "结束时间_jp", "结束时间"),
            // entity.sopexecstep.endedat
            new TranslationSeedItem("entity.sopexecstep.endedat", "zh-CN", "结束时间", "结束时间"),
            // entity.sopexecstep.endedat
            new TranslationSeedItem("entity.sopexecstep.endedat", "zh-HK", "结束时间_hk", "结束时间"),

            // entity.sopexecstep.stepresult
            new TranslationSeedItem("entity.sopexecstep.stepresult", "en-US", "工步结果_us", "工步结果（字典 logistics_manufacturing_sop_check_result；1=合格，2=不合格，3=不适用/跳过）"),
            // entity.sopexecstep.stepresult
            new TranslationSeedItem("entity.sopexecstep.stepresult", "ja-JP", "工步结果_jp", "工步结果（字典 logistics_manufacturing_sop_check_result；1=合格，2=不合格，3=不适用/跳过）"),
            // entity.sopexecstep.stepresult
            new TranslationSeedItem("entity.sopexecstep.stepresult", "zh-CN", "工步结果", "工步结果（字典 logistics_manufacturing_sop_check_result；1=合格，2=不合格，3=不适用/跳过）"),
            // entity.sopexecstep.stepresult
            new TranslationSeedItem("entity.sopexecstep.stepresult", "zh-HK", "工步结果_hk", "工步结果（字典 logistics_manufacturing_sop_check_result；1=合格，2=不合格，3=不适用/跳过）"),

            // entity.sopexecstep.confirmedby
            new TranslationSeedItem("entity.sopexecstep.confirmedby", "en-US", "确认人ID_us", "确认人 ID（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.sopexecstep.confirmedby
            new TranslationSeedItem("entity.sopexecstep.confirmedby", "ja-JP", "确认人ID_jp", "确认人 ID（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.sopexecstep.confirmedby
            new TranslationSeedItem("entity.sopexecstep.confirmedby", "zh-CN", "确认人ID", "确认人 ID（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.sopexecstep.confirmedby
            new TranslationSeedItem("entity.sopexecstep.confirmedby", "zh-HK", "确认人ID_hk", "确认人 ID（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.sopexecstep.confirmedbyname
            new TranslationSeedItem("entity.sopexecstep.confirmedbyname", "en-US", "确认人名称_us", "确认人名称（冗余：按 ConfirmedBy 取 TaktEmployee.EmployeeName 联动）"),
            // entity.sopexecstep.confirmedbyname
            new TranslationSeedItem("entity.sopexecstep.confirmedbyname", "ja-JP", "确认人名称_jp", "确认人名称（冗余：按 ConfirmedBy 取 TaktEmployee.EmployeeName 联动）"),
            // entity.sopexecstep.confirmedbyname
            new TranslationSeedItem("entity.sopexecstep.confirmedbyname", "zh-CN", "确认人名称", "确认人名称（冗余：按 ConfirmedBy 取 TaktEmployee.EmployeeName 联动）"),
            // entity.sopexecstep.confirmedbyname
            new TranslationSeedItem("entity.sopexecstep.confirmedbyname", "zh-HK", "确认人名称_hk", "确认人名称（冗余：按 ConfirmedBy 取 TaktEmployee.EmployeeName 联动）"),

            // entity.sopexecstep.confirmedat
            new TranslationSeedItem("entity.sopexecstep.confirmedat", "en-US", "确认时间_us", "确认时间"),
            // entity.sopexecstep.confirmedat
            new TranslationSeedItem("entity.sopexecstep.confirmedat", "ja-JP", "确认时间_jp", "确认时间"),
            // entity.sopexecstep.confirmedat
            new TranslationSeedItem("entity.sopexecstep.confirmedat", "zh-CN", "确认时间", "确认时间"),
            // entity.sopexecstep.confirmedat
            new TranslationSeedItem("entity.sopexecstep.confirmedat", "zh-HK", "确认时间_hk", "确认时间"),

            // entity.sopexecstep.blocknextstep
            new TranslationSeedItem("entity.sopexecstep.blocknextstep", "en-US", "是否禁止下一步_us", "是否禁止下一步（字典 sys_yes_no；0=否，1=是）"),
            // entity.sopexecstep.blocknextstep
            new TranslationSeedItem("entity.sopexecstep.blocknextstep", "ja-JP", "是否禁止下一步_jp", "是否禁止下一步（字典 sys_yes_no；0=否，1=是）"),
            // entity.sopexecstep.blocknextstep
            new TranslationSeedItem("entity.sopexecstep.blocknextstep", "zh-CN", "是否禁止下一步", "是否禁止下一步（字典 sys_yes_no；0=否，1=是）"),
            // entity.sopexecstep.blocknextstep
            new TranslationSeedItem("entity.sopexecstep.blocknextstep", "zh-HK", "是否禁止下一步_hk", "是否禁止下一步（字典 sys_yes_no；0=否，1=是）"),

            // entity.sopexecstep.exec
            new TranslationSeedItem("entity.sopexecstep.exec", "en-US", "执行追溯_us", "执行追溯"),
            // entity.sopexecstep.exec
            new TranslationSeedItem("entity.sopexecstep.exec", "ja-JP", "执行追溯_jp", "执行追溯"),
            // entity.sopexecstep.exec
            new TranslationSeedItem("entity.sopexecstep.exec", "zh-CN", "执行追溯", "执行追溯"),
            // entity.sopexecstep.exec
            new TranslationSeedItem("entity.sopexecstep.exec", "zh-HK", "执行追溯_hk", "执行追溯"),
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
