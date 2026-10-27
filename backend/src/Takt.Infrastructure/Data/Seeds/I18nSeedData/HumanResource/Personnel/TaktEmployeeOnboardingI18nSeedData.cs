// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Personnel
// 文件名称：TaktEmployeeOnboardingI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEmployeeOnboarding 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Personnel;

/// <summary>
/// TaktEmployeeOnboarding 实体国际化翻译种子（键前缀 entity.employeeonboarding.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEmployeeOnboardingI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEmployeeOnboarding 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 employeeonboarding 实体翻译...", tenantCode);

        foreach (var item in GetEmployeeOnboardingTranslations())
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

        TaktLogger.Information("TaktEmployeeOnboarding 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEmployeeOnboarding 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.employeeonboarding._self / entity.employeeonboarding.{{field}}；ResourceGroup=Personnel；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEmployeeOnboardingTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.employeeonboarding._self
            new TranslationSeedItem("entity.employeeonboarding._self", "en-US", "Employee Onboarding Information_us", "实体名称"),
            // entity.employeeonboarding._self
            new TranslationSeedItem("entity.employeeonboarding._self", "ja-JP", "入职待办信息_jp", "实体名称"),
            // entity.employeeonboarding._self
            new TranslationSeedItem("entity.employeeonboarding._self", "zh-CN", "入职待办信息", "实体名称"),
            // entity.employeeonboarding._self
            new TranslationSeedItem("entity.employeeonboarding._self", "zh-HK", "入职待办信息_hk", "实体名称"),

            // entity.employeeonboarding.offerid
            new TranslationSeedItem("entity.employeeonboarding.offerid", "en-US", "录用信息ID_us", "录用信息（选项 TaktTalentOffers/options；DictValue=Id）"),
            // entity.employeeonboarding.offerid
            new TranslationSeedItem("entity.employeeonboarding.offerid", "ja-JP", "录用信息ID_jp", "录用信息（选项 TaktTalentOffers/options；DictValue=Id）"),
            // entity.employeeonboarding.offerid
            new TranslationSeedItem("entity.employeeonboarding.offerid", "zh-CN", "录用信息ID", "录用信息（选项 TaktTalentOffers/options；DictValue=Id）"),
            // entity.employeeonboarding.offerid
            new TranslationSeedItem("entity.employeeonboarding.offerid", "zh-HK", "录用信息ID_hk", "录用信息（选项 TaktTalentOffers/options；DictValue=Id）"),

            // entity.employeeonboarding.todocode
            new TranslationSeedItem("entity.employeeonboarding.todocode", "en-US", "待办单号_us", "待办单号（租户+公司内业务编码）"),
            // entity.employeeonboarding.todocode
            new TranslationSeedItem("entity.employeeonboarding.todocode", "ja-JP", "待办单号_jp", "待办单号（租户+公司内业务编码）"),
            // entity.employeeonboarding.todocode
            new TranslationSeedItem("entity.employeeonboarding.todocode", "zh-CN", "待办单号", "待办单号（租户+公司内业务编码）"),
            // entity.employeeonboarding.todocode
            new TranslationSeedItem("entity.employeeonboarding.todocode", "zh-HK", "待办单号_hk", "待办单号（租户+公司内业务编码）"),

            // entity.employeeonboarding.plannedjoineddate
            new TranslationSeedItem("entity.employeeonboarding.plannedjoineddate", "en-US", "计划上岗日期_us", "计划上岗日期（JoinedDate 计划值）"),
            // entity.employeeonboarding.plannedjoineddate
            new TranslationSeedItem("entity.employeeonboarding.plannedjoineddate", "ja-JP", "计划上岗日期_jp", "计划上岗日期（JoinedDate 计划值）"),
            // entity.employeeonboarding.plannedjoineddate
            new TranslationSeedItem("entity.employeeonboarding.plannedjoineddate", "zh-CN", "计划上岗日期", "计划上岗日期（JoinedDate 计划值）"),
            // entity.employeeonboarding.plannedjoineddate
            new TranslationSeedItem("entity.employeeonboarding.plannedjoineddate", "zh-HK", "计划上岗日期_hk", "计划上岗日期（JoinedDate 计划值）"),

            // entity.employeeonboarding.candidatename
            new TranslationSeedItem("entity.employeeonboarding.candidatename", "en-US", "候选人姓名_us", "候选人姓名（快照）"),
            // entity.employeeonboarding.candidatename
            new TranslationSeedItem("entity.employeeonboarding.candidatename", "ja-JP", "候选人姓名_jp", "候选人姓名（快照）"),
            // entity.employeeonboarding.candidatename
            new TranslationSeedItem("entity.employeeonboarding.candidatename", "zh-CN", "候选人姓名", "候选人姓名（快照）"),
            // entity.employeeonboarding.candidatename
            new TranslationSeedItem("entity.employeeonboarding.candidatename", "zh-HK", "候选人姓名_hk", "候选人姓名（快照）"),

            // entity.employeeonboarding.mobile
            new TranslationSeedItem("entity.employeeonboarding.mobile", "en-US", "候选人手机_us", "候选人手机（快照）"),
            // entity.employeeonboarding.mobile
            new TranslationSeedItem("entity.employeeonboarding.mobile", "ja-JP", "候选人手机_jp", "候选人手机（快照）"),
            // entity.employeeonboarding.mobile
            new TranslationSeedItem("entity.employeeonboarding.mobile", "zh-CN", "候选人手机", "候选人手机（快照）"),
            // entity.employeeonboarding.mobile
            new TranslationSeedItem("entity.employeeonboarding.mobile", "zh-HK", "候选人手机_hk", "候选人手机（快照）"),

            // entity.employeeonboarding.employeeid
            new TranslationSeedItem("entity.employeeonboarding.employeeid", "en-US", "关联员工ID_us", "关联员工（选项 TaktEmployees/options；建档后回填，可空，DictValue=Id）"),
            // entity.employeeonboarding.employeeid
            new TranslationSeedItem("entity.employeeonboarding.employeeid", "ja-JP", "关联员工ID_jp", "关联员工（选项 TaktEmployees/options；建档后回填，可空，DictValue=Id）"),
            // entity.employeeonboarding.employeeid
            new TranslationSeedItem("entity.employeeonboarding.employeeid", "zh-CN", "关联员工ID", "关联员工（选项 TaktEmployees/options；建档后回填，可空，DictValue=Id）"),
            // entity.employeeonboarding.employeeid
            new TranslationSeedItem("entity.employeeonboarding.employeeid", "zh-HK", "关联员工ID_hk", "关联员工（选项 TaktEmployees/options；建档后回填，可空，DictValue=Id）"),

            // entity.employeeonboarding.employeecode
            new TranslationSeedItem("entity.employeeonboarding.employeecode", "en-US", "员工编码_us", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐；建档回填后可写，建档前可空）"),
            // entity.employeeonboarding.employeecode
            new TranslationSeedItem("entity.employeeonboarding.employeecode", "ja-JP", "员工编码_jp", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐；建档回填后可写，建档前可空）"),
            // entity.employeeonboarding.employeecode
            new TranslationSeedItem("entity.employeeonboarding.employeecode", "zh-CN", "员工编码", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐；建档回填后可写，建档前可空）"),
            // entity.employeeonboarding.employeecode
            new TranslationSeedItem("entity.employeeonboarding.employeecode", "zh-HK", "员工编码_hk", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐；建档回填后可写，建档前可空）"),

            // entity.employeeonboarding.employeename
            new TranslationSeedItem("entity.employeeonboarding.employeename", "en-US", "员工姓名_us", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐；建档回填后可写，建档前可空）"),
            // entity.employeeonboarding.employeename
            new TranslationSeedItem("entity.employeeonboarding.employeename", "ja-JP", "员工姓名_jp", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐；建档回填后可写，建档前可空）"),
            // entity.employeeonboarding.employeename
            new TranslationSeedItem("entity.employeeonboarding.employeename", "zh-CN", "员工姓名", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐；建档回填后可写，建档前可空）"),
            // entity.employeeonboarding.employeename
            new TranslationSeedItem("entity.employeeonboarding.employeename", "zh-HK", "员工姓名_hk", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐；建档回填后可写，建档前可空）"),

            // entity.employeeonboarding.employeejoinedid
            new TranslationSeedItem("entity.employeeonboarding.employeejoinedid", "en-US", "入职上岗单ID_us", "入职上岗单（关联 TaktEmployeeJoined.Id；待办完成后回填，可空）"),
            // entity.employeeonboarding.employeejoinedid
            new TranslationSeedItem("entity.employeeonboarding.employeejoinedid", "ja-JP", "入职上岗单ID_jp", "入职上岗单（关联 TaktEmployeeJoined.Id；待办完成后回填，可空）"),
            // entity.employeeonboarding.employeejoinedid
            new TranslationSeedItem("entity.employeeonboarding.employeejoinedid", "zh-CN", "入职上岗单ID", "入职上岗单（关联 TaktEmployeeJoined.Id；待办完成后回填，可空）"),
            // entity.employeeonboarding.employeejoinedid
            new TranslationSeedItem("entity.employeeonboarding.employeejoinedid", "zh-HK", "入职上岗单ID_hk", "入职上岗单（关联 TaktEmployeeJoined.Id；待办完成后回填，可空）"),

            // entity.employeeonboarding.reason
            new TranslationSeedItem("entity.employeeonboarding.reason", "en-US", "待办说明_us", "待办说明"),
            // entity.employeeonboarding.reason
            new TranslationSeedItem("entity.employeeonboarding.reason", "ja-JP", "待办说明_jp", "待办说明"),
            // entity.employeeonboarding.reason
            new TranslationSeedItem("entity.employeeonboarding.reason", "zh-CN", "待办说明", "待办说明"),
            // entity.employeeonboarding.reason
            new TranslationSeedItem("entity.employeeonboarding.reason", "zh-HK", "待办说明_hk", "待办说明"),

            // entity.employeeonboarding.todostatus
            new TranslationSeedItem("entity.employeeonboarding.todostatus", "en-US", "待办状态_us", "待办状态（字典 humanresource_personnel_onboarding_status；0=待办理 1=办理中 2=已完成 3=已取消）"),
            // entity.employeeonboarding.todostatus
            new TranslationSeedItem("entity.employeeonboarding.todostatus", "ja-JP", "待办状态_jp", "待办状态（字典 humanresource_personnel_onboarding_status；0=待办理 1=办理中 2=已完成 3=已取消）"),
            // entity.employeeonboarding.todostatus
            new TranslationSeedItem("entity.employeeonboarding.todostatus", "zh-CN", "待办状态", "待办状态（字典 humanresource_personnel_onboarding_status；0=待办理 1=办理中 2=已完成 3=已取消）"),
            // entity.employeeonboarding.todostatus
            new TranslationSeedItem("entity.employeeonboarding.todostatus", "zh-HK", "待办状态_hk", "待办状态（字典 humanresource_personnel_onboarding_status；0=待办理 1=办理中 2=已完成 3=已取消）"),

            // entity.employeeonboarding.employee
            new TranslationSeedItem("entity.employeeonboarding.employee", "en-US", "员工主档_us", "员工主档（多对一；建档回填后可有值）"),
            // entity.employeeonboarding.employee
            new TranslationSeedItem("entity.employeeonboarding.employee", "ja-JP", "员工主档_jp", "员工主档（多对一；建档回填后可有值）"),
            // entity.employeeonboarding.employee
            new TranslationSeedItem("entity.employeeonboarding.employee", "zh-CN", "员工主档", "员工主档（多对一；建档回填后可有值）"),
            // entity.employeeonboarding.employee
            new TranslationSeedItem("entity.employeeonboarding.employee", "zh-HK", "员工主档_hk", "员工主档（多对一；建档回填后可有值）"),

            // entity.employeeonboarding.offer
            new TranslationSeedItem("entity.employeeonboarding.offer", "en-US", "录用信息_us", "录用信息"),
            // entity.employeeonboarding.offer
            new TranslationSeedItem("entity.employeeonboarding.offer", "ja-JP", "录用信息_jp", "录用信息"),
            // entity.employeeonboarding.offer
            new TranslationSeedItem("entity.employeeonboarding.offer", "zh-CN", "录用信息", "录用信息"),
            // entity.employeeonboarding.offer
            new TranslationSeedItem("entity.employeeonboarding.offer", "zh-HK", "录用信息_hk", "录用信息"),

            // entity.employeeonboarding.employeejoined
            new TranslationSeedItem("entity.employeeonboarding.employeejoined", "en-US", "入职上岗单_us", "入职上岗单"),
            // entity.employeeonboarding.employeejoined
            new TranslationSeedItem("entity.employeeonboarding.employeejoined", "ja-JP", "入职上岗单_jp", "入职上岗单"),
            // entity.employeeonboarding.employeejoined
            new TranslationSeedItem("entity.employeeonboarding.employeejoined", "zh-CN", "入职上岗单", "入职上岗单"),
            // entity.employeeonboarding.employeejoined
            new TranslationSeedItem("entity.employeeonboarding.employeejoined", "zh-HK", "入职上岗单_hk", "入职上岗单"),
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
        translation.ResourceGroup = "Personnel";
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
