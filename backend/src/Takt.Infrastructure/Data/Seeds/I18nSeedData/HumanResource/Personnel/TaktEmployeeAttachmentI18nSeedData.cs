// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Personnel
// 文件名称：TaktEmployeeAttachmentI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEmployeeAttachment 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktEmployeeAttachment 实体国际化翻译种子（键前缀 entity.employeeattachment.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEmployeeAttachmentI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEmployeeAttachment 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 employeeattachment 实体翻译...", tenantCode);

        foreach (var item in GetEmployeeAttachmentTranslations())
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

        TaktLogger.Information("TaktEmployeeAttachment 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEmployeeAttachment 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.employeeattachment._self / entity.employeeattachment.{{field}}；ResourceGroup=Personnel；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEmployeeAttachmentTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.employeeattachment._self
            new TranslationSeedItem("entity.employeeattachment._self", "en-US", "Employee Attachment Information_us", "实体名称"),
            // entity.employeeattachment._self
            new TranslationSeedItem("entity.employeeattachment._self", "ja-JP", "员工档案附件信息_jp", "实体名称"),
            // entity.employeeattachment._self
            new TranslationSeedItem("entity.employeeattachment._self", "zh-CN", "员工档案附件信息", "实体名称"),
            // entity.employeeattachment._self
            new TranslationSeedItem("entity.employeeattachment._self", "zh-HK", "员工档案附件信息_hk", "实体名称"),

            // entity.employeeattachment.employeeid
            new TranslationSeedItem("entity.employeeattachment.employeeid", "en-US", "员工ID_us", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeeattachment.employeeid
            new TranslationSeedItem("entity.employeeattachment.employeeid", "ja-JP", "员工ID_jp", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeeattachment.employeeid
            new TranslationSeedItem("entity.employeeattachment.employeeid", "zh-CN", "员工ID", "员工（选项 TaktEmployees/options；DictValue=Id）"),
            // entity.employeeattachment.employeeid
            new TranslationSeedItem("entity.employeeattachment.employeeid", "zh-HK", "员工ID_hk", "员工（选项 TaktEmployees/options；DictValue=Id）"),

            // entity.employeeattachment.employeecode
            new TranslationSeedItem("entity.employeeattachment.employeecode", "en-US", "员工编码_us", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),
            // entity.employeeattachment.employeecode
            new TranslationSeedItem("entity.employeeattachment.employeecode", "ja-JP", "员工编码_jp", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),
            // entity.employeeattachment.employeecode
            new TranslationSeedItem("entity.employeeattachment.employeecode", "zh-CN", "员工编码", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),
            // entity.employeeattachment.employeecode
            new TranslationSeedItem("entity.employeeattachment.employeecode", "zh-HK", "员工编码_hk", "员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）"),

            // entity.employeeattachment.employeename
            new TranslationSeedItem("entity.employeeattachment.employeename", "en-US", "员工姓名_us", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),
            // entity.employeeattachment.employeename
            new TranslationSeedItem("entity.employeeattachment.employeename", "ja-JP", "员工姓名_jp", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),
            // entity.employeeattachment.employeename
            new TranslationSeedItem("entity.employeeattachment.employeename", "zh-CN", "员工姓名", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),
            // entity.employeeattachment.employeename
            new TranslationSeedItem("entity.employeeattachment.employeename", "zh-HK", "员工姓名_hk", "员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）"),

            // entity.employeeattachment.filename
            new TranslationSeedItem("entity.employeeattachment.filename", "en-US", "文件名称_us", "文件名称（原始文件名，长度对齐 TaktFile.FileName）"),
            // entity.employeeattachment.filename
            new TranslationSeedItem("entity.employeeattachment.filename", "ja-JP", "文件名称_jp", "文件名称（原始文件名，长度对齐 TaktFile.FileName）"),
            // entity.employeeattachment.filename
            new TranslationSeedItem("entity.employeeattachment.filename", "zh-CN", "文件名称", "文件名称（原始文件名，长度对齐 TaktFile.FileName）"),
            // entity.employeeattachment.filename
            new TranslationSeedItem("entity.employeeattachment.filename", "zh-HK", "文件名称_hk", "文件名称（原始文件名，长度对齐 TaktFile.FileName）"),

            // entity.employeeattachment.accessurl
            new TranslationSeedItem("entity.employeeattachment.accessurl", "en-US", "访问地址_us", "访问地址（关联 TaktFile.AccessUrl）"),
            // entity.employeeattachment.accessurl
            new TranslationSeedItem("entity.employeeattachment.accessurl", "ja-JP", "访问地址_jp", "访问地址（关联 TaktFile.AccessUrl）"),
            // entity.employeeattachment.accessurl
            new TranslationSeedItem("entity.employeeattachment.accessurl", "zh-CN", "访问地址", "访问地址（关联 TaktFile.AccessUrl）"),
            // entity.employeeattachment.accessurl
            new TranslationSeedItem("entity.employeeattachment.accessurl", "zh-HK", "访问地址_hk", "访问地址（关联 TaktFile.AccessUrl）"),

            // entity.employeeattachment.employee
            new TranslationSeedItem("entity.employeeattachment.employee", "en-US", "员工主档_us", "员工主档（多对一）"),
            // entity.employeeattachment.employee
            new TranslationSeedItem("entity.employeeattachment.employee", "ja-JP", "员工主档_jp", "员工主档（多对一）"),
            // entity.employeeattachment.employee
            new TranslationSeedItem("entity.employeeattachment.employee", "zh-CN", "员工主档", "员工主档（多对一）"),
            // entity.employeeattachment.employee
            new TranslationSeedItem("entity.employeeattachment.employee", "zh-HK", "员工主档_hk", "员工主档（多对一）"),
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
