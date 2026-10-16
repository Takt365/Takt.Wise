// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData
// 文件名称：TaktDeptI18nSeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：TaktDept 实体国际化翻译种子（键前缀 org.dept.*；含 2300/DTA 与 1000/日本本社）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData;

/// <summary>
/// 部门国际化翻译种子（键前缀 org.dept.*）
/// 幂等性：存在则更新，不存在则创建
/// TranslationText 为部门名称；ContextNote 为 TaktDeptSeedData.DeptDescription
/// </summary>
public class TaktDeptI18nSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（在部门种子之后）
    /// </summary>
    public int Order => 51;

    /// <summary>
    /// 初始化部门国际化翻译种子
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（可选，用于租户级实体种子数据）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化部门国际化翻译种子...");

        // 验证租户编码
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过部门国际化翻译种子初始化");
            return (0, 0);    // 返回插入和更新的记录数（插入数, 更新数）
        }

        var repository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktTranslation>>();
        var cultureRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCulture>>();
        var cultureIdByCode = (await cultureRepository.GetListAsync(c => c.TenantCode == tenantCode))
            .ToDictionary(c => c.CultureCode, c => c.Id);
        int insertCount = 0;
        int updateCount = 0;

        TaktLogger.Information("正在为租户 {TenantCode} 初始化部门国际化翻译种子...", tenantCode);

        foreach (var row in GetStandardDeptTranslations())
        {
            if (!cultureIdByCode.TryGetValue(row.CultureCode, out var cultureId))
            {
                TaktLogger.Warning("未找到区域 {CultureCode}，跳过国际化翻译 {I18nKey}", row.CultureCode, row.I18nKey);
                continue;
            }

            var item = new TranslationSeedItem(row.I18nKey, row.CultureCode, row.TranslationText, row.ContextNote);
            var (_, i, u) = await CreateOrUpdateTranslationAsync(repository, tenantCode, cultureId, item);
            insertCount += i;
            updateCount += u;
        }

        TaktLogger.Information("部门国际化翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);

        return (insertCount, updateCount);
    }

    /// <summary>
    /// 获取标准部门翻译列表
    /// 包含 TaktDeptSeedData.cs 组织的部门国际化翻译
    /// 包含公司 DTA 组织的部门国际化翻译
    /// </summary>
    private static List<(string I18nKey, string CultureCode, string TranslationText, string? ContextNote)> GetStandardDeptTranslations()
    {
        return new List<(string, string, string, string?)>
        {
            // ========================================
            // 集团骨架：0000 集团 → 1000 总公司 / 2300 TEAC东莞工厂 / 2400 TEAC香港
            // ========================================
            ("org.dept.0000", "zh-CN", "TEAC", "ティアック株式会社"),
            ("org.dept.0000", "en-US", "TEAC", "TEAC CORPORATION"),
            ("org.dept.0000", "ja-JP", "TEAC", "ティアック株式会社"),
            ("org.dept.0000", "zh-HK", "TEAC", "ティアック株式会社"),

            ("org.dept.1000", "zh-CN", "TCJ", "TCJ"),
            ("org.dept.1000", "en-US", "TCJ", "TCJ"),
            ("org.dept.1000", "ja-JP", "TCJ", "TCJ"),
            ("org.dept.1000", "zh-HK", "TCJ", "TCJ"),

            ("org.dept.2300", "zh-CN", "DTA", "DTA"),
            ("org.dept.2300", "en-US", "DTA", "DTA"),
            ("org.dept.2300", "ja-JP", "DTA", "DTA"),
            ("org.dept.2300", "zh-HK", "DTA", "DTA"),

            ("org.dept.2400", "zh-CN", "TAC", "TAC"),
            ("org.dept.2400", "en-US", "TAC", "TAC"),
            ("org.dept.2400", "ja-JP", "TAC", "TAC"),
            ("org.dept.2400", "zh-HK", "TAC", "TAC"),

            ("org.dept.t000", "zh-CN", "总经理室", "TAC"),
            ("org.dept.t000", "en-US", "General Manager Office", "TAC"),
            ("org.dept.t000", "ja-JP", "総経理室", "TAC"),
            ("org.dept.t000", "zh-HK", "總經理室", "TAC"),
            ("org.dept.t100", "zh-CN", "财务部", "TAC"),
            ("org.dept.t100", "en-US", "Finance Department", "TAC"),
            ("org.dept.t100", "ja-JP", "財務部", "TAC"),
            ("org.dept.t100", "zh-HK", "財務部", "TAC"),
            ("org.dept.t200", "zh-CN", "资材物流部", "TAC"),
            ("org.dept.t200", "en-US", "Materials & Logistics Department", "TAC"),
            ("org.dept.t200", "ja-JP", "資材物流部", "TAC"),
            ("org.dept.t200", "zh-HK", "資材物流部", "TAC"),

            // ========================================
            // DTA 一级部门：D1000～D0900
            // ========================================
            ("org.dept.d1000", "zh-CN", "总经理室", "一级部门"),
            ("org.dept.d1000", "en-US", "General Manager Office", "Level 1 Dept"),
            ("org.dept.d1000", "ja-JP", "総経理室", "一級部門"),
            ("org.dept.d1000", "zh-HK", "総経理室", "一級部門"),
            
            ("org.dept.d0100", "zh-CN", "总务部", "一级部门"),
            ("org.dept.d0100", "en-US", "General Affairs Dept", "Level 1 Dept"),
            ("org.dept.d0100", "ja-JP", "総務部", "一級部門"),
            ("org.dept.d0100", "zh-HK", "総務部", "一級部門"),
            
            ("org.dept.d0200", "zh-CN", "财务部", "一级部门"),
            ("org.dept.d0200", "en-US", "Finance Dept", "Level 1 Dept"),
            ("org.dept.d0200", "ja-JP", "財務部", "一級部門"),
            ("org.dept.d0200", "zh-HK", "財務部", "一級部門"),
            
            ("org.dept.d0300", "zh-CN", "IT部", "一级部门"),
            ("org.dept.d0300", "en-US", "IT Dept", "Level 1 Dept"),
            ("org.dept.d0300", "ja-JP", "IT部", "一級部門"),
            ("org.dept.d0300", "zh-HK", "IT部", "一級部門"),
            
            ("org.dept.d0400", "zh-CN", "管理部", "一级部门"),
            ("org.dept.d0400", "en-US", "Management Dept", "Level 1 Dept"),
            ("org.dept.d0400", "ja-JP", "管理部", "一級部門"),
            ("org.dept.d0400", "zh-HK", "管理部", "一級部門"),
            
            ("org.dept.d0500", "zh-CN", "资材部", "一级部门"),
            ("org.dept.d0500", "en-US", "Materials Dept", "Level 1 Dept"),
            ("org.dept.d0500", "ja-JP", "資材部", "一級部門"),
            ("org.dept.d0500", "zh-HK", "資材部", "一級部門"),
            
            ("org.dept.d0600", "zh-CN", "生产部", "一级部门"),
            ("org.dept.d0600", "en-US", "Production Dept", "Level 1 Dept"),
            ("org.dept.d0600", "ja-JP", "生産部", "一級部門"),
            ("org.dept.d0600", "zh-HK", "生産部", "一級部門"),
            
            ("org.dept.d0700", "zh-CN", "技术部", "一级部门"),
            ("org.dept.d0700", "en-US", "Technology Dept", "Level 1 Dept"),
            ("org.dept.d0700", "ja-JP", "技術部", "一級部門"),
            ("org.dept.d0700", "zh-HK", "技術部", "一級部門"),
            
            ("org.dept.d0800", "zh-CN", "品保部", "一级部门"),
            ("org.dept.d0800", "en-US", "Quality Assurance Dept", "Level 1 Dept"),
            ("org.dept.d0800", "ja-JP", "品保部", "一級部門"),
            ("org.dept.d0800", "zh-HK", "品保部", "一級部門"),
            
            ("org.dept.d0900", "zh-CN", "OEM部", "一级部门"),
            ("org.dept.d0900", "en-US", "OEM Dept", "Level 1 Dept"),
            ("org.dept.d0900", "ja-JP", "OEM部", "一級部門"),
            ("org.dept.d0900", "zh-HK", "OEM部", "一級部門"),

            // ========================================
            // 总务部二级部门：D0110～D0130
            // ========================================
            ("org.dept.d0110", "zh-CN", "总务课", "二级部门"),
            ("org.dept.d0110", "en-US", "General Affairs Section", "Level 2 Dept"),
            ("org.dept.d0110", "ja-JP", "総務課", "二級部門"),
            ("org.dept.d0110", "zh-HK", "総務課", "二級部門"),

            // ========================================
            // 财务部二级部门：D0210～D0230
            // ========================================
            ("org.dept.d0210", "zh-CN", "财务课", "二级部门"),
            ("org.dept.d0210", "en-US", "Finance Section", "Level 2 Dept"),
            ("org.dept.d0210", "ja-JP", "財務課", "二級部門"),
            ("org.dept.d0210", "zh-HK", "財務課", "二級部門"),

            // ========================================
            // IT部二级部门：D0310～D0330
            // ========================================
            ("org.dept.d0310", "zh-CN", "电脑课", "二级部门"),
            ("org.dept.d0310", "en-US", "Computer Section", "Level 2 Dept"),
            ("org.dept.d0310", "ja-JP", "電腦課", "二級部門"),
            ("org.dept.d0310", "zh-HK", "電腦課", "二級部門"),

            // ========================================
            // 管理部二级部门：D0410～D0430
            // ========================================
            ("org.dept.d0410", "zh-CN", "报关课", "二级部门"),
            ("org.dept.d0410", "en-US", "Customs Section", "Level 2 Dept"),
            ("org.dept.d0410", "ja-JP", "報關課", "二級部門"),
            ("org.dept.d0410", "zh-HK", "報關課", "二級部門"),
            
            ("org.dept.d0420", "zh-CN", "生管课", "二级部门"),
            ("org.dept.d0420", "en-US", "Production Control Section", "Level 2 Dept"),
            ("org.dept.d0420", "ja-JP", "生管課", "二級部門"),
            ("org.dept.d0420", "zh-HK", "生管課", "二級部門"),
            
            ("org.dept.d0430", "zh-CN", "部管课", "二级部门"),
            ("org.dept.d0430", "en-US", "Department Management Section", "Level 2 Dept"),
            ("org.dept.d0430", "ja-JP", "部管課", "二級部門"),
            ("org.dept.d0430", "zh-HK", "部管課", "二級部門"),

            // ========================================
            // 资材部二级部门：D0510～D0530
            // ========================================
            ("org.dept.d0510", "zh-CN", "采购课", "二级部门"),
            ("org.dept.d0510", "en-US", "Purchasing Section", "Level 2 Dept"),
            ("org.dept.d0510", "ja-JP", "採購課", "二級部門"),
            ("org.dept.d0510", "zh-HK", "採購課", "二級部門"),

            // ========================================
            // 生产部二级部门：D0610～D0630
            // ========================================
            ("org.dept.d0610", "zh-CN", "制造1课", "二级部门"),
            ("org.dept.d0610", "en-US", "Manufacturing Section 1", "Level 2 Dept"),
            ("org.dept.d0610", "ja-JP", "製造1課", "二級部門"),
            ("org.dept.d0610", "zh-HK", "製造1課", "二級部門"),
            
            ("org.dept.d0620", "zh-CN", "制造2课", "二级部门"),
            ("org.dept.d0620", "en-US", "Manufacturing Section 2", "Level 2 Dept"),
            ("org.dept.d0620", "ja-JP", "製造2課", "二級部門"),
            ("org.dept.d0620", "zh-HK", "製造2課", "二級部門"),
            
            ("org.dept.d0630", "zh-CN", "制造技术课", "二级部门"),
            ("org.dept.d0630", "en-US", "Manufacturing Technology Section", "Level 2 Dept"),
            ("org.dept.d0630", "ja-JP", "製造技術課", "二級部門"),
            ("org.dept.d0630", "zh-HK", "製造技術課", "二級部門"),

            // ========================================
            // 制造2课二级部门：D0621～D0626
            // ========================================
            ("org.dept.d0621", "zh-CN", "SMT", "三级部门"),
            ("org.dept.d0621", "en-US", "SMT", "Level 3 Dept"),
            ("org.dept.d0621", "ja-JP", "SMT", "三級部門"),
            ("org.dept.d0621", "zh-HK", "SMT", "三級部門"),
            
            ("org.dept.d0622", "zh-CN", "自插", "三级部门"),
            ("org.dept.d0622", "en-US", "Auto Insertion", "Level 3 Dept"),
            ("org.dept.d0622", "ja-JP", "自插", "三級部門"),
            ("org.dept.d0622", "zh-HK", "自插", "三級部門"),
            
            ("org.dept.d0623", "zh-CN", "修正", "三级部门"),
            ("org.dept.d0623", "en-US", "Rework", "Level 3 Dept"),
            ("org.dept.d0623", "ja-JP", "修正", "三級部門"),
            ("org.dept.d0623", "zh-HK", "修正", "三級部門"),
            
            ("org.dept.d0624", "zh-CN", "手插", "三级部门"),
            ("org.dept.d0624", "en-US", "Manual Insertion", "Level 3 Dept"),
            ("org.dept.d0624", "ja-JP", "手插", "三級部門"),
            ("org.dept.d0624", "zh-HK", "手插", "三級部門"),
            
            ("org.dept.d0625", "zh-CN", "电子物料", "三级部门"),
            ("org.dept.d0625", "en-US", "Materials", "Level 3 Dept"),
            ("org.dept.d0625", "ja-JP", "电子物料", "三級部門"),
            ("org.dept.d0625", "zh-HK", "电子物料", "三級部門"),
            
            ("org.dept.d0626", "zh-CN", "制造2课-间接", "三级部门"),
            ("org.dept.d0626", "en-US", "Manufacturing Section 2 - Indirect", "Level 3 Dept"),
            ("org.dept.d0626", "ja-JP", "製造2課-間接", "三級部門"),
            ("org.dept.d0626", "zh-HK", "製造2課-間接", "三級部門"),

            // ========================================
            // 技术部二级部门：D0710～D0730
            // ========================================
            ("org.dept.d0710", "zh-CN", "技术课", "二级部门"),
            ("org.dept.d0710", "en-US", "Technology Section", "Level 2 Dept"),
            ("org.dept.d0710", "ja-JP", "技術課", "二級部門"),
            ("org.dept.d0710", "zh-HK", "技術課", "二級部門"),

            // ========================================
            // 品保部二级部门：D0810～D0820
            // ========================================
            ("org.dept.d0810", "zh-CN", "受检课", "二级部门"),
            ("org.dept.d0810", "en-US", "Incoming Inspection Section", "Level 2 Dept"),
            ("org.dept.d0810", "ja-JP", "受检課", "二級部門"),
            ("org.dept.d0810", "zh-HK", "受检課", "二級部門"),
            
            ("org.dept.d0820", "zh-CN", "品管课", "二级部门"),
            ("org.dept.d0820", "en-US", "Quality Control Section", "Level 2 Dept"),
            ("org.dept.d0820", "ja-JP", "品管課", "二級部門"),
            ("org.dept.d0820", "zh-HK", "品管課", "二級部門"),

            // ========================================
            // OEM部二级部门：D0910～D0920
            // ========================================
            ("org.dept.d0910", "zh-CN", "OEM QA课", "二级部门"),
            ("org.dept.d0910", "en-US", "OEM QA Section", "Level 2 Dept"),
            ("org.dept.d0910", "ja-JP", "OEM QA課", "二級部門"),
            ("org.dept.d0910", "zh-HK", "OEM QA課", "二級部門"),
            
            ("org.dept.d0920", "zh-CN", "OEM管理课", "二级部门"),
            ("org.dept.d0920", "en-US", "OEM Management Section", "Level 2 Dept"),
            ("org.dept.d0920", "ja-JP", "OEM管理課", "二級部門"),
            ("org.dept.d0920", "zh-HK", "OEM管理課", "二級部門"),

            // ========================================
            // 公司 1000 / J100 日本本社组织（org.dept.{deptcode}）
            // ========================================
            ("org.dept.1100", "zh-CN", "总务人事部", "日本本社"),
            ("org.dept.1100", "en-US", "General Affairs and Personnel Department", "Japan HQ"),
            ("org.dept.1100", "ja-JP", "総務人事部", "日本本社"),
            ("org.dept.1100", "zh-HK", "總務人事部", "日本本社"),
            ("org.dept.1110", "zh-CN", "人事劳政课", "日本本社"),
            ("org.dept.1110", "en-US", "Human Resources and Employee Relations Section", "Japan HQ"),
            ("org.dept.1110", "ja-JP", "人事労政課", "日本本社"),
            ("org.dept.1110", "zh-HK", "人事勞政課", "日本本社"),
            ("org.dept.1120", "zh-CN", "总务课", "日本本社"),
            ("org.dept.1120", "en-US", "General Affairs Section", "Japan HQ"),
            ("org.dept.1120", "ja-JP", "総務課", "日本本社"),
            ("org.dept.1120", "zh-HK", "總務課", "日本本社"),
            ("org.dept.1170", "zh-CN", "秘书课", "日本本社"),
            ("org.dept.1170", "en-US", "Secretarial Section", "Japan HQ"),
            ("org.dept.1170", "ja-JP", "秘書課", "日本本社"),
            ("org.dept.1170", "zh-HK", "秘書課", "日本本社"),
            ("org.dept.1510", "zh-CN", "法务课", "日本本社"),
            ("org.dept.1510", "en-US", "Legal Affairs Section", "Japan HQ"),
            ("org.dept.1510", "ja-JP", "法務課", "日本本社"),
            ("org.dept.1510", "zh-HK", "法務課", "日本本社"),
            ("org.dept.1520", "zh-CN", "知识产权课", "日本本社"),
            ("org.dept.1520", "en-US", "Intellectual Property Section", "Japan HQ"),
            ("org.dept.1520", "ja-JP", "知的財産課", "日本本社"),
            ("org.dept.1520", "zh-HK", "知識產權課", "日本本社"),
            ("org.dept.1149", "zh-CN", "部付TEAC工会专从", "日本本社"),
            ("org.dept.1149", "en-US", "TEAC WORKER'S UNION", "Japan HQ"),
            ("org.dept.1149", "ja-JP", "部付ティアック労働組合専従", "日本本社"),
            ("org.dept.1149", "zh-HK", "部付TEAC工會專從", "日本本社"),
            ("org.dept.1160", "zh-CN", "部付其他", "日本本社"),
            ("org.dept.1160", "en-US", "General Affairs and Personnel Department (Others)", "Japan HQ"),
            ("org.dept.1160", "ja-JP", "部付その他", "日本本社"),
            ("org.dept.1160", "zh-HK", "部付其他", "日本本社"),
            ("org.dept.1200", "zh-CN", "财务企划部", "日本本社"),
            ("org.dept.1200", "en-US", "Corporate Finance and Planning Department", "Japan HQ"),
            ("org.dept.1200", "ja-JP", "財務企画部", "日本本社"),
            ("org.dept.1200", "zh-HK", "財務企劃部", "日本本社"),
            ("org.dept.1210", "zh-CN", "财务课", "日本本社"),
            ("org.dept.1210", "en-US", "Credit Section", "Japan HQ"),
            ("org.dept.1210", "ja-JP", "財務課", "日本本社"),
            ("org.dept.1210", "zh-HK", "財務課", "日本本社"),
            ("org.dept.1220", "zh-CN", "经理课", "日本本社"),
            ("org.dept.1220", "en-US", "Accounting Section", "Japan HQ"),
            ("org.dept.1220", "ja-JP", "経理課", "日本本社"),
            ("org.dept.1220", "zh-HK", "經理課", "日本本社"),
            ("org.dept.1430", "zh-CN", "经营信息课", "日本本社"),
            ("org.dept.1430", "en-US", "Business Intelligence Section", "Japan HQ"),
            ("org.dept.1430", "ja-JP", "経営情報課", "日本本社"),
            ("org.dept.1430", "zh-HK", "經營資訊課", "日本本社"),
            ("org.dept.9800", "zh-CN", "内部监察室", "日本本社"),
            ("org.dept.9800", "en-US", "Internal Audit Department", "Japan HQ"),
            ("org.dept.9800", "ja-JP", "内部監査室", "日本本社"),
            ("org.dept.9800", "zh-HK", "內部監察室", "日本本社"),
            ("org.dept.9200", "zh-CN", "SCM本部", "日本本社"),
            ("org.dept.9200", "en-US", "Supply-Chain Management Headquarters", "Japan HQ"),
            ("org.dept.9200", "ja-JP", "SCM本部", "日本本社"),
            ("org.dept.9200", "zh-HK", "SCM本部", "日本本社"),
            ("org.dept.1800", "zh-CN", "管理・采购部", "日本本社"),
            ("org.dept.1800", "en-US", "Administration and Purchasing Department", "Japan HQ"),
            ("org.dept.1800", "ja-JP", "管理・購買部", "日本本社"),
            ("org.dept.1800", "zh-HK", "管理・採購部", "日本本社"),
            ("org.dept.1820", "zh-CN", "采购课", "日本本社"),
            ("org.dept.1820", "en-US", "Purchasing Section", "Japan HQ"),
            ("org.dept.1820", "ja-JP", "購買課", "日本本社"),
            ("org.dept.1820", "zh-HK", "採購課", "日本本社"),
            ("org.dept.1810", "zh-CN", "管理课", "日本本社"),
            ("org.dept.1810", "en-US", "Operations Section", "Japan HQ"),
            ("org.dept.1810", "ja-JP", "管理課", "日本本社"),
            ("org.dept.1810", "zh-HK", "管理課", "日本本社"),
            ("org.dept.1250", "zh-CN", "贸易课", "日本本社"),
            ("org.dept.1250", "en-US", "Trade Operation Section", "Japan HQ"),
            ("org.dept.1250", "ja-JP", "貿易課", "日本本社"),
            ("org.dept.1250", "zh-HK", "貿易課", "日本本社"),
            ("org.dept.1900", "zh-CN", "品质保证・技术部", "日本本社"),
            ("org.dept.1900", "en-US", "Quality Assurance and Engineering Department", "Japan HQ"),
            ("org.dept.1900", "ja-JP", "品質保証・技術部", "日本本社"),
            ("org.dept.1900", "zh-HK", "品質保證・技術部", "日本本社"),
            ("org.dept.1920", "zh-CN", "品质保证课", "日本本社"),
            ("org.dept.1920", "en-US", "Quality Assurance Section", "Japan HQ"),
            ("org.dept.1920", "ja-JP", "品質保証課", "日本本社"),
            ("org.dept.1920", "zh-HK", "品質保證課", "日本本社"),
            ("org.dept.1940", "zh-CN", "安全规格・环境管理课", "日本本社"),
            ("org.dept.1940", "en-US", "Product Safety Standards and Environment Control Section", "Japan HQ"),
            ("org.dept.1940", "ja-JP", "安全規格・環境管理課", "日本本社"),
            ("org.dept.1940", "zh-HK", "安全規格・環境管理課", "日本本社"),
            ("org.dept.1720", "zh-CN", "生产技术课", "日本本社"),
            ("org.dept.1720", "en-US", "Engineering Section", "Japan HQ"),
            ("org.dept.1720", "ja-JP", "生産技術課", "日本本社"),
            ("org.dept.1720", "zh-HK", "生產技術課", "日本本社"),
            ("org.dept.9701", "zh-CN", "高级音频事业部", "日本本社"),
            ("org.dept.9701", "en-US", "Premium Audio Division", "Japan HQ"),
            ("org.dept.9701", "ja-JP", "プレミアムオーディオ事業部", "日本本社"),
            ("org.dept.9701", "zh-HK", "高級音頻事業部", "日本本社"),
            ("org.dept.3100", "zh-CN", "国内营业部", "日本本社"),
            ("org.dept.3100", "en-US", "Domestic Sales Department, Premium Audio Division", "Japan HQ"),
            ("org.dept.3100", "ja-JP", "国内営業部", "日本本社"),
            ("org.dept.3100", "zh-HK", "國內營業部", "日本本社"),
            ("org.dept.3110", "zh-CN", "营业1课", "日本本社"),
            ("org.dept.3110", "en-US", "Sales Section 1", "Japan HQ"),
            ("org.dept.3110", "ja-JP", "営業1課", "日本本社"),
            ("org.dept.3110", "zh-HK", "營業1課", "日本本社"),
            ("org.dept.3120", "zh-CN", "营业2课", "日本本社"),
            ("org.dept.3120", "en-US", "Sales Section 2", "Japan HQ"),
            ("org.dept.3120", "ja-JP", "営業2課", "日本本社"),
            ("org.dept.3120", "zh-HK", "營業2課", "日本本社"),
            ("org.dept.3200", "zh-CN", "海外营业・销售促进部", "日本本社"),
            ("org.dept.3200", "en-US", "International Sales and Marketing Department", "Japan HQ"),
            ("org.dept.3200", "ja-JP", "海外営業・販売促進部", "日本本社"),
            ("org.dept.3200", "zh-HK", "海外營業・銷售促進部", "日本本社"),
            ("org.dept.3530", "zh-CN", "海外营业课", "日本本社"),
            ("org.dept.3530", "en-US", "International Sales Section", "Japan HQ"),
            ("org.dept.3530", "ja-JP", "海外営業課", "日本本社"),
            ("org.dept.3530", "zh-HK", "海外營業課", "日本本社"),
            ("org.dept.3540", "zh-CN", "销售促进课", "日本本社"),
            ("org.dept.3540", "en-US", "Marketing and Communications Section", "Japan HQ"),
            ("org.dept.3540", "ja-JP", "販売促進課", "日本本社"),
            ("org.dept.3540", "zh-HK", "銷售促進課", "日本本社"),
            ("org.dept.3560", "zh-CN", "产品课", "日本本社"),
            ("org.dept.3560", "en-US", "Product Section", "Japan HQ"),
            ("org.dept.3560", "ja-JP", "プロダクト課", "日本本社"),
            ("org.dept.3560", "zh-HK", "產品課", "日本本社"),
            ("org.dept.2100", "zh-CN", "业务推进部", "日本本社"),
            ("org.dept.2100", "en-US", "Promotion Division Department", "Japan HQ"),
            ("org.dept.2100", "ja-JP", "業務推進部", "日本本社"),
            ("org.dept.2100", "zh-HK", "業務推進部", "日本本社"),
            ("org.dept.2160", "zh-CN", "流通管理课", "日本本社"),
            ("org.dept.2160", "en-US", "Distribution Management Section", "Japan HQ"),
            ("org.dept.2160", "ja-JP", "流通管理課", "日本本社"),
            ("org.dept.2160", "zh-HK", "流通管理課", "日本本社"),
            ("org.dept.2170", "zh-CN", "电子商务课", "日本本社"),
            ("org.dept.2170", "en-US", "Electronic Commerce Section", "Japan HQ"),
            ("org.dept.2170", "ja-JP", "イーコマース課", "日本本社"),
            ("org.dept.2170", "zh-HK", "電子商務課", "日本本社"),
            ("org.dept.2200", "zh-CN", "开发部", "日本本社"),
            ("org.dept.2200", "en-US", "Research and Development Department", "Japan HQ"),
            ("org.dept.2200", "ja-JP", "開発部", "日本本社"),
            ("org.dept.2200", "zh-HK", "開發部", "日本本社"),
            ("org.dept.2230", "zh-CN", "电气设计课", "日本本社"),
            ("org.dept.2230", "en-US", "Electrical Designing Section", "Japan HQ"),
            ("org.dept.2230", "ja-JP", "電気設計課", "日本本社"),
            ("org.dept.2230", "zh-HK", "電氣設計課", "日本本社"),
            ("org.dept.2240", "zh-CN", "机构设计课", "日本本社"),
            ("org.dept.2240", "en-US", "Mechanical Designing Section", "Japan HQ"),
            ("org.dept.2240", "ja-JP", "機構設計課", "日本本社"),
            ("org.dept.2240", "zh-HK", "機構設計課", "日本本社"),
            ("org.dept.2210", "zh-CN", "内容制作课", "日本本社"),
            ("org.dept.2210", "en-US", "Content Production Section", "Japan HQ"),
            ("org.dept.2210", "ja-JP", "コンテンツ制作課", "日本本社"),
            ("org.dept.2210", "zh-HK", "內容製作課", "日本本社"),
            ("org.dept.9703", "zh-CN", "TASCAM事业部", "日本本社"),
            ("org.dept.9703", "en-US", "TASCAM Division", "Japan HQ"),
            ("org.dept.9703", "ja-JP", "タスカム事業部", "日本本社"),
            ("org.dept.9703", "zh-HK", "TASCAM事業部", "日本本社"),
            ("org.dept.3400", "zh-CN", "国内营业部", "日本本社"),
            ("org.dept.3400", "en-US", "Domestic Sales Department, TASCAM Division", "Japan HQ"),
            ("org.dept.3400", "ja-JP", "国内営業部", "日本本社"),
            ("org.dept.3400", "zh-HK", "國內營業部", "日本本社"),
            ("org.dept.3460", "zh-CN", "国内销售课", "日本本社"),
            ("org.dept.3460", "en-US", "Domestic Sales Section", "Japan HQ"),
            ("org.dept.3460", "ja-JP", "国内販売課", "日本本社"),
            ("org.dept.3460", "zh-HK", "國內銷售課", "日本本社"),
            ("org.dept.3440", "zh-CN", "客户支持课", "日本本社"),
            ("org.dept.3440", "en-US", "Customer Support Section", "Japan HQ"),
            ("org.dept.3440", "ja-JP", "カスタマーサポート課", "日本本社"),
            ("org.dept.3440", "zh-HK", "客戶支援課", "日本本社"),
            ("org.dept.3450", "zh-CN", "放送营业课", "日本本社"),
            ("org.dept.3450", "en-US", "Broadcast Sales Section", "Japan HQ"),
            ("org.dept.3450", "ja-JP", "放送営業課", "日本本社"),
            ("org.dept.3450", "zh-HK", "放送營業課", "日本本社"),
            ("org.dept.3470", "zh-CN", "PA/SR课", "日本本社"),
            ("org.dept.3470", "en-US", "PA/SR Section", "Japan HQ"),
            ("org.dept.3470", "ja-JP", "PA/SR課", "日本本社"),
            ("org.dept.3470", "zh-HK", "PA/SR課", "日本本社"),
            ("org.dept.3700", "zh-CN", "营业企划部", "日本本社"),
            ("org.dept.3700", "en-US", "Sales and Marketing Department", "Japan HQ"),
            ("org.dept.3700", "ja-JP", "営業企画部", "日本本社"),
            ("org.dept.3700", "zh-HK", "營業企劃部", "日本本社"),
            ("org.dept.3490", "zh-CN", "海外销售课", "日本本社"),
            ("org.dept.3490", "en-US", "International Sales Section", "Japan HQ"),
            ("org.dept.3490", "ja-JP", "海外販売課", "日本本社"),
            ("org.dept.3490", "zh-HK", "海外銷售課", "日本本社"),
            ("org.dept.3310", "zh-CN", "销售促进课", "日本本社"),
            ("org.dept.3310", "en-US", "Sales Planning Section", "Japan HQ"),
            ("org.dept.3310", "ja-JP", "販売促進課", "日本本社"),
            ("org.dept.3310", "zh-HK", "銷售促進課", "日本本社"),
            ("org.dept.3480", "zh-CN", "流通管理课", "日本本社"),
            ("org.dept.3480", "en-US", "Distribution Management Section", "Japan HQ"),
            ("org.dept.3480", "ja-JP", "流通管理課", "日本本社"),
            ("org.dept.3480", "zh-HK", "流通管理課", "日本本社"),
            ("org.dept.9910", "zh-CN", "开发统括本部", "日本本社"),
            ("org.dept.9910", "en-US", "Research and Development Headquarters", "Japan HQ"),
            ("org.dept.9910", "ja-JP", "開発統括本部", "日本本社"),
            ("org.dept.9910", "zh-HK", "開發統括本部", "日本本社"),
            ("org.dept.36ta", "zh-CN", "TASCAM开发部", "日本本社"),
            ("org.dept.36ta", "en-US", "TASCAM Research and Development Department", "Japan HQ"),
            ("org.dept.36ta", "ja-JP", "タスカム開発部", "日本本社"),
            ("org.dept.36ta", "zh-HK", "TASCAM開發部", "日本本社"),
            ("org.dept.3670", "zh-CN", "硬件开发课", "日本本社"),
            ("org.dept.3670", "en-US", "Hardware Designing Section", "Japan HQ"),
            ("org.dept.3670", "ja-JP", "ハードウェア開発課", "日本本社"),
            ("org.dept.3670", "zh-HK", "硬件開發課", "日本本社"),
            ("org.dept.3630", "zh-CN", "固件开发课", "日本本社"),
            ("org.dept.3630", "en-US", "Firmware Designing Section", "Japan HQ"),
            ("org.dept.3630", "ja-JP", "ファームウェア開発課", "日本本社"),
            ("org.dept.3630", "zh-HK", "固件開發課", "日本本社"),
            ("org.dept.3610", "zh-CN", "企划开发课", "日本本社"),
            ("org.dept.3610", "en-US", "Product Planning Section", "Japan HQ"),
            ("org.dept.3610", "ja-JP", "企画開発課", "日本本社"),
            ("org.dept.3610", "zh-HK", "企劃開發課", "日本本社"),
            ("org.dept.3603", "zh-CN", "外包课", "日本本社"),
            ("org.dept.3603", "en-US", "Outsourcing Section", "Japan HQ"),
            ("org.dept.3603", "ja-JP", "アウトソーシング課", "日本本社"),
            ("org.dept.3603", "zh-HK", "外包課", "日本本社"),
            ("org.dept.36ip", "zh-CN", "信息设备开发部", "日本本社"),
            ("org.dept.36ip", "en-US", "Information Products Research and Development Department", "Japan HQ"),
            ("org.dept.36ip", "ja-JP", "情報機器開発部", "日本本社"),
            ("org.dept.36ip", "zh-HK", "資訊設備開發部", "日本本社"),
            ("org.dept.4650", "zh-CN", "硬件开发课", "日本本社"),
            ("org.dept.4650", "en-US", "Hardware Designing Section", "Japan HQ"),
            ("org.dept.4650", "ja-JP", "ハードウェア開発課", "日本本社"),
            ("org.dept.4650", "zh-HK", "硬件開發課", "日本本社"),
            ("org.dept.4660", "zh-CN", "固件开发课", "日本本社"),
            ("org.dept.4660", "en-US", "Firmware Designing Section", "Japan HQ"),
            ("org.dept.4660", "ja-JP", "ファームウェア開発課", "日本本社"),
            ("org.dept.4660", "zh-HK", "固件開發課", "日本本社"),
            ("org.dept.36pi", "zh-CN", "产品集成部", "日本本社"),
            ("org.dept.36pi", "en-US", "Product Integration Department", "Japan HQ"),
            ("org.dept.36pi", "ja-JP", "プロダクトインテグレーション部", "日本本社"),
            ("org.dept.36pi", "zh-HK", "產品整合部", "日本本社"),
            ("org.dept.3690", "zh-CN", "应用与集成开发课", "日本本社"),
            ("org.dept.3690", "en-US", "Application and Integration Designing Section", "Japan HQ"),
            ("org.dept.3690", "ja-JP", "アプリ＆インテグレーション開発課", "日本本社"),
            ("org.dept.3690", "zh-HK", "應用與整合開發課", "日本本社"),
            ("org.dept.3620", "zh-CN", "机构开发课", "日本本社"),
            ("org.dept.3620", "en-US", "Mechanical Designing Section", "Japan HQ"),
            ("org.dept.3620", "ja-JP", "機構開発課", "日本本社"),
            ("org.dept.3620", "zh-HK", "機構開發課", "日本本社"),
            ("org.dept.1710", "zh-CN", "开发业务课", "日本本社"),
            ("org.dept.1710", "en-US", "Development Coordination Section", "Japan HQ"),
            ("org.dept.1710", "ja-JP", "開発業務課", "日本本社"),
            ("org.dept.1710", "zh-HK", "開發業務課", "日本本社"),
            ("org.dept.3330", "zh-CN", "设计课", "日本本社"),
            ("org.dept.3330", "en-US", "Design Work Section", "Japan HQ"),
            ("org.dept.3330", "ja-JP", "デザイン課", "日本本社"),
            ("org.dept.3330", "zh-HK", "設計課", "日本本社"),
            ("org.dept.9400", "zh-CN", "信息设备事业部", "日本本社"),
            ("org.dept.9400", "en-US", "Information Products Division", "Japan HQ"),
            ("org.dept.9400", "ja-JP", "情報機器事業部", "日本本社"),
            ("org.dept.9400", "zh-HK", "資訊設備事業部", "日本本社"),
            ("org.dept.4500", "zh-CN", "事业推进部", "日本本社"),
            ("org.dept.4500", "en-US", "Business Operation Department", "Japan HQ"),
            ("org.dept.4500", "ja-JP", "事業推進部", "日本本社"),
            ("org.dept.4500", "zh-HK", "事業推進部", "日本本社"),
            ("org.dept.4520", "zh-CN", "销售促进课", "日本本社"),
            ("org.dept.4520", "en-US", "Sales Promotion Section", "Japan HQ"),
            ("org.dept.4520", "ja-JP", "販売促進課", "日本本社"),
            ("org.dept.4520", "zh-HK", "銷售促進課", "日本本社"),
            ("org.dept.4530", "zh-CN", "事业计划课", "日本本社"),
            ("org.dept.4530", "en-US", "Business Planning Section", "Japan HQ"),
            ("org.dept.4530", "ja-JP", "事業計画課", "日本本社"),
            ("org.dept.4530", "zh-HK", "事業計劃課", "日本本社"),
            ("org.dept.4410", "zh-CN", "客户支持课", "日本本社"),
            ("org.dept.4410", "en-US", "Customer Support Section", "Japan HQ"),
            ("org.dept.4410", "ja-JP", "カスタマーサポート課", "日本本社"),
            ("org.dept.4410", "zh-HK", "客戶支援課", "日本本社"),
            ("org.dept.4430", "zh-CN", "TR技术解决方案课", "日本本社"),
            ("org.dept.4430", "en-US", "TR Technical Solution Section", "Japan HQ"),
            ("org.dept.4430", "ja-JP", "TRテクニカルソリューション課", "日本本社"),
            ("org.dept.4430", "zh-HK", "TR技術解決方案課", "日本本社"),
            ("org.dept.4800", "zh-CN", "测量产品营业部", "日本本社"),
            ("org.dept.4800", "en-US", "Measurement Products Sales Department", "Japan HQ"),
            ("org.dept.4800", "ja-JP", "メジャメントプロダクト営業部", "日本本社"),
            ("org.dept.4800", "zh-HK", "測量產品營業部", "日本本社"),
            ("org.dept.4810", "zh-CN", "国内营业课", "日本本社"),
            ("org.dept.4810", "en-US", "Domestic Sales Section", "Japan HQ"),
            ("org.dept.4810", "ja-JP", "国内営業課", "日本本社"),
            ("org.dept.4810", "zh-HK", "國內營業課", "日本本社"),
            ("org.dept.4830", "zh-CN", "海外营业课", "日本本社"),
            ("org.dept.4830", "en-US", "International Sales Section", "Japan HQ"),
            ("org.dept.4830", "ja-JP", "海外営業課", "日本本社"),
            ("org.dept.4830", "zh-HK", "海外營業課", "日本本社"),
            ("org.dept.4900", "zh-CN", "影像系统解决方案营业部", "日本本社"),
            ("org.dept.4900", "en-US", "Imaging System Solutions Sales Department", "Japan HQ"),
            ("org.dept.4900", "ja-JP", "イメージングシステムソリューション営業部", "日本本社"),
            ("org.dept.4900", "zh-HK", "影像系統解決方案營業部", "日本本社"),
            ("org.dept.4910", "zh-CN", "医疗系统国内营业课", "日本本社"),
            ("org.dept.4910", "en-US", "Medical System Domestic Sales Section", "Japan HQ"),
            ("org.dept.4910", "ja-JP", "メディカルシステム国内営業課", "日本本社"),
            ("org.dept.4910", "zh-HK", "醫療系統國內營業課", "日本本社"),
            ("org.dept.4920", "zh-CN", "医疗系统海外营业课", "日本本社"),
            ("org.dept.4920", "en-US", "Medical System International Sales Section", "Japan HQ"),
            ("org.dept.4920", "ja-JP", "メディカルシステム海外営業課", "日本本社"),
            ("org.dept.4920", "zh-HK", "醫療系統海外營業課", "日本本社"),
        };
    }

    /// <summary>
    /// 更新 TaktTranslation 所有字段（CultureId 为 SeedAsync 参数）
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
        translation.ResourceGroup = "Foundation";
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
    /// 记录翻译项（对应 TaktTranslation 所有字段，CultureId 为 SeedAsync 参数）
    /// </summary>
    private sealed record TranslationSeedItem(
        string I18nKey,
        string CultureCode,
        string TranslationText,
        string? ContextNote);
}
