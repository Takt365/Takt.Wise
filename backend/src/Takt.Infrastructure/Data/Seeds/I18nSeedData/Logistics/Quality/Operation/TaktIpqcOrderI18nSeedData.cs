// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Operation
// 文件名称：TaktIpqcOrderI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktIpqcOrder 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Operation;

/// <summary>
/// TaktIpqcOrder 实体国际化翻译种子（键前缀 entity.ipqcorder.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktIpqcOrderI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktIpqcOrder 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ipqcorder 实体翻译...", tenantCode);

        foreach (var item in GetIpqcOrderTranslations())
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

        TaktLogger.Information("TaktIpqcOrder 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktIpqcOrder 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.ipqcorder._self / entity.ipqcorder.{{field}}；ResourceGroup=Operation；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetIpqcOrderTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ipqcorder._self
            new TranslationSeedItem("entity.ipqcorder._self", "en-US", "Ipqc Order Information_us", "实体名称"),
            // entity.ipqcorder._self
            new TranslationSeedItem("entity.ipqcorder._self", "ja-JP", "IPQC制程检验单信息_jp", "实体名称"),
            // entity.ipqcorder._self
            new TranslationSeedItem("entity.ipqcorder._self", "zh-CN", "IPQC制程检验单信息", "实体名称"),
            // entity.ipqcorder._self
            new TranslationSeedItem("entity.ipqcorder._self", "zh-HK", "IPQC制程检验单信息_hk", "实体名称"),

            // entity.ipqcorder.sourcecode
            new TranslationSeedItem("entity.ipqcorder.sourcecode", "en-US", "来源单号_us", "来源单号（选项 TaktProductionOrders/options；DictValue=ProdOrderCode）"),
            // entity.ipqcorder.sourcecode
            new TranslationSeedItem("entity.ipqcorder.sourcecode", "ja-JP", "来源单号_jp", "来源单号（选项 TaktProductionOrders/options；DictValue=ProdOrderCode）"),
            // entity.ipqcorder.sourcecode
            new TranslationSeedItem("entity.ipqcorder.sourcecode", "zh-CN", "来源单号", "来源单号（选项 TaktProductionOrders/options；DictValue=ProdOrderCode）"),
            // entity.ipqcorder.sourcecode
            new TranslationSeedItem("entity.ipqcorder.sourcecode", "zh-HK", "来源单号_hk", "来源单号（选项 TaktProductionOrders/options；DictValue=ProdOrderCode）"),

            // entity.ipqcorder.inspectiondate
            new TranslationSeedItem("entity.ipqcorder.inspectiondate", "en-US", "检验日期_us", "检验日期"),
            // entity.ipqcorder.inspectiondate
            new TranslationSeedItem("entity.ipqcorder.inspectiondate", "ja-JP", "检验日期_jp", "检验日期"),
            // entity.ipqcorder.inspectiondate
            new TranslationSeedItem("entity.ipqcorder.inspectiondate", "zh-CN", "检验日期", "检验日期"),
            // entity.ipqcorder.inspectiondate
            new TranslationSeedItem("entity.ipqcorder.inspectiondate", "zh-HK", "检验日期_hk", "检验日期"),

            // entity.ipqcorder.code
            new TranslationSeedItem("entity.ipqcorder.code", "en-US", "IPQC检验单编码_us", "IPQC检验单编码（唯一索引，根据来源单号自动生成）"),
            // entity.ipqcorder.code
            new TranslationSeedItem("entity.ipqcorder.code", "ja-JP", "IPQC检验单编码_jp", "IPQC检验单编码（唯一索引，根据来源单号自动生成）"),
            // entity.ipqcorder.code
            new TranslationSeedItem("entity.ipqcorder.code", "zh-CN", "IPQC检验单编码", "IPQC检验单编码（唯一索引，根据来源单号自动生成）"),
            // entity.ipqcorder.code
            new TranslationSeedItem("entity.ipqcorder.code", "zh-HK", "IPQC检验单编码_hk", "IPQC检验单编码（唯一索引，根据来源单号自动生成）"),

            // entity.ipqcorder.processcode
            new TranslationSeedItem("entity.ipqcorder.processcode", "en-US", "工序编码_us", "工序编码"),
            // entity.ipqcorder.processcode
            new TranslationSeedItem("entity.ipqcorder.processcode", "ja-JP", "工序编码_jp", "工序编码"),
            // entity.ipqcorder.processcode
            new TranslationSeedItem("entity.ipqcorder.processcode", "zh-CN", "工序编码", "工序编码"),
            // entity.ipqcorder.processcode
            new TranslationSeedItem("entity.ipqcorder.processcode", "zh-HK", "工序编码_hk", "工序编码"),

            // entity.ipqcorder.processname
            new TranslationSeedItem("entity.ipqcorder.processname", "en-US", "工序名称_us", "工序名称"),
            // entity.ipqcorder.processname
            new TranslationSeedItem("entity.ipqcorder.processname", "ja-JP", "工序名称_jp", "工序名称"),
            // entity.ipqcorder.processname
            new TranslationSeedItem("entity.ipqcorder.processname", "zh-CN", "工序名称", "工序名称"),
            // entity.ipqcorder.processname
            new TranslationSeedItem("entity.ipqcorder.processname", "zh-HK", "工序名称_hk", "工序名称"),

            // entity.ipqcorder.totalproductionquantity
            new TranslationSeedItem("entity.ipqcorder.totalproductionquantity", "en-US", "生产总数_us", "生产总数"),
            // entity.ipqcorder.totalproductionquantity
            new TranslationSeedItem("entity.ipqcorder.totalproductionquantity", "ja-JP", "生产总数_jp", "生产总数"),
            // entity.ipqcorder.totalproductionquantity
            new TranslationSeedItem("entity.ipqcorder.totalproductionquantity", "zh-CN", "生产总数", "生产总数"),
            // entity.ipqcorder.totalproductionquantity
            new TranslationSeedItem("entity.ipqcorder.totalproductionquantity", "zh-HK", "生产总数_hk", "生产总数"),

            // entity.ipqcorder.totalsamplequantity
            new TranslationSeedItem("entity.ipqcorder.totalsamplequantity", "en-US", "总抽样数量_us", "总抽样数量（自动计算 = 各明细抽样数量合计）"),
            // entity.ipqcorder.totalsamplequantity
            new TranslationSeedItem("entity.ipqcorder.totalsamplequantity", "ja-JP", "总抽样数量_jp", "总抽样数量（自动计算 = 各明细抽样数量合计）"),
            // entity.ipqcorder.totalsamplequantity
            new TranslationSeedItem("entity.ipqcorder.totalsamplequantity", "zh-CN", "总抽样数量", "总抽样数量（自动计算 = 各明细抽样数量合计）"),
            // entity.ipqcorder.totalsamplequantity
            new TranslationSeedItem("entity.ipqcorder.totalsamplequantity", "zh-HK", "总抽样数量_hk", "总抽样数量（自动计算 = 各明细抽样数量合计）"),

            // entity.ipqcorder.totalqualifiedquantity
            new TranslationSeedItem("entity.ipqcorder.totalqualifiedquantity", "en-US", "总合格数量_us", "总合格数量（自动计算 = 各明细合格数量合计）"),
            // entity.ipqcorder.totalqualifiedquantity
            new TranslationSeedItem("entity.ipqcorder.totalqualifiedquantity", "ja-JP", "总合格数量_jp", "总合格数量（自动计算 = 各明细合格数量合计）"),
            // entity.ipqcorder.totalqualifiedquantity
            new TranslationSeedItem("entity.ipqcorder.totalqualifiedquantity", "zh-CN", "总合格数量", "总合格数量（自动计算 = 各明细合格数量合计）"),
            // entity.ipqcorder.totalqualifiedquantity
            new TranslationSeedItem("entity.ipqcorder.totalqualifiedquantity", "zh-HK", "总合格数量_hk", "总合格数量（自动计算 = 各明细合格数量合计）"),

            // entity.ipqcorder.totalunqualifiedquantity
            new TranslationSeedItem("entity.ipqcorder.totalunqualifiedquantity", "en-US", "总不合格数量_us", "总不合格数量（自动计算 = 各明细不合格数量合计）"),
            // entity.ipqcorder.totalunqualifiedquantity
            new TranslationSeedItem("entity.ipqcorder.totalunqualifiedquantity", "ja-JP", "总不合格数量_jp", "总不合格数量（自动计算 = 各明细不合格数量合计）"),
            // entity.ipqcorder.totalunqualifiedquantity
            new TranslationSeedItem("entity.ipqcorder.totalunqualifiedquantity", "zh-CN", "总不合格数量", "总不合格数量（自动计算 = 各明细不合格数量合计）"),
            // entity.ipqcorder.totalunqualifiedquantity
            new TranslationSeedItem("entity.ipqcorder.totalunqualifiedquantity", "zh-HK", "总不合格数量_hk", "总不合格数量（自动计算 = 各明细不合格数量合计）"),

            // entity.ipqcorder.totalinspectionreturnquantity
            new TranslationSeedItem("entity.ipqcorder.totalinspectionreturnquantity", "en-US", "总验退数量_us", "总验退数量（自动计算 = 各明细验退数量合计）"),
            // entity.ipqcorder.totalinspectionreturnquantity
            new TranslationSeedItem("entity.ipqcorder.totalinspectionreturnquantity", "ja-JP", "总验退数量_jp", "总验退数量（自动计算 = 各明细验退数量合计）"),
            // entity.ipqcorder.totalinspectionreturnquantity
            new TranslationSeedItem("entity.ipqcorder.totalinspectionreturnquantity", "zh-CN", "总验退数量", "总验退数量（自动计算 = 各明细验退数量合计）"),
            // entity.ipqcorder.totalinspectionreturnquantity
            new TranslationSeedItem("entity.ipqcorder.totalinspectionreturnquantity", "zh-HK", "总验退数量_hk", "总验退数量（自动计算 = 各明细验退数量合计）"),

            // entity.ipqcorder.judgebyemployeeid
            new TranslationSeedItem("entity.ipqcorder.judgebyemployeeid", "en-US", "判定人ID_us", "判定人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.ipqcorder.judgebyemployeeid
            new TranslationSeedItem("entity.ipqcorder.judgebyemployeeid", "ja-JP", "判定人ID_jp", "判定人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.ipqcorder.judgebyemployeeid
            new TranslationSeedItem("entity.ipqcorder.judgebyemployeeid", "zh-CN", "判定人ID", "判定人（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.ipqcorder.judgebyemployeeid
            new TranslationSeedItem("entity.ipqcorder.judgebyemployeeid", "zh-HK", "判定人ID_hk", "判定人（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.ipqcorder.judgebyemployeename
            new TranslationSeedItem("entity.ipqcorder.judgebyemployeename", "en-US", "判定人名称_us", "判定人名称（冗余：按 JudgeByEmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.ipqcorder.judgebyemployeename
            new TranslationSeedItem("entity.ipqcorder.judgebyemployeename", "ja-JP", "判定人名称_jp", "判定人名称（冗余：按 JudgeByEmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.ipqcorder.judgebyemployeename
            new TranslationSeedItem("entity.ipqcorder.judgebyemployeename", "zh-CN", "判定人名称", "判定人名称（冗余：按 JudgeByEmployeeId 取 TaktEmployee.EmployeeName 联动）"),
            // entity.ipqcorder.judgebyemployeename
            new TranslationSeedItem("entity.ipqcorder.judgebyemployeename", "zh-HK", "判定人名称_hk", "判定人名称（冗余：按 JudgeByEmployeeId 取 TaktEmployee.EmployeeName 联动）"),

            // entity.ipqcorder.judgedate
            new TranslationSeedItem("entity.ipqcorder.judgedate", "en-US", "判定日期_us", "判定日期"),
            // entity.ipqcorder.judgedate
            new TranslationSeedItem("entity.ipqcorder.judgedate", "ja-JP", "判定日期_jp", "判定日期"),
            // entity.ipqcorder.judgedate
            new TranslationSeedItem("entity.ipqcorder.judgedate", "zh-CN", "判定日期", "判定日期"),
            // entity.ipqcorder.judgedate
            new TranslationSeedItem("entity.ipqcorder.judgedate", "zh-HK", "判定日期_hk", "判定日期"),

            // entity.ipqcorder.judgedescription
            new TranslationSeedItem("entity.ipqcorder.judgedescription", "en-US", "判定说明_us", "判定说明"),
            // entity.ipqcorder.judgedescription
            new TranslationSeedItem("entity.ipqcorder.judgedescription", "ja-JP", "判定说明_jp", "判定说明"),
            // entity.ipqcorder.judgedescription
            new TranslationSeedItem("entity.ipqcorder.judgedescription", "zh-CN", "判定说明", "判定说明"),
            // entity.ipqcorder.judgedescription
            new TranslationSeedItem("entity.ipqcorder.judgedescription", "zh-HK", "判定说明_hk", "判定说明"),

            // entity.ipqcorder.judgestatus
            new TranslationSeedItem("entity.ipqcorder.judgestatus", "en-US", "判定状态_us", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）"),
            // entity.ipqcorder.judgestatus
            new TranslationSeedItem("entity.ipqcorder.judgestatus", "ja-JP", "判定状态_jp", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）"),
            // entity.ipqcorder.judgestatus
            new TranslationSeedItem("entity.ipqcorder.judgestatus", "zh-CN", "判定状态", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）"),
            // entity.ipqcorder.judgestatus
            new TranslationSeedItem("entity.ipqcorder.judgestatus", "zh-HK", "判定状态_hk", "判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）"),

            // entity.ipqcorder.items
            new TranslationSeedItem("entity.ipqcorder.items", "en-US", "IPQC检验单明细列表_us", "IPQC检验单明细列表（主子表关系）"),
            // entity.ipqcorder.items
            new TranslationSeedItem("entity.ipqcorder.items", "ja-JP", "IPQC检验单明细列表_jp", "IPQC检验单明细列表（主子表关系）"),
            // entity.ipqcorder.items
            new TranslationSeedItem("entity.ipqcorder.items", "zh-CN", "IPQC检验单明细列表", "IPQC检验单明细列表（主子表关系）"),
            // entity.ipqcorder.items
            new TranslationSeedItem("entity.ipqcorder.items", "zh-HK", "IPQC检验单明细列表_hk", "IPQC检验单明细列表（主子表关系）"),
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
        translation.ResourceGroup = "Operation";
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
