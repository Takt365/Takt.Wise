// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Personnel
// 文件名称：TaktEmployeeI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEmployee 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktEmployee 实体国际化翻译种子（键前缀 entity.employee.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEmployeeI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEmployee 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 employee 实体翻译...", tenantCode);

        foreach (var item in GetEmployeeTranslations())
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

        TaktLogger.Information("TaktEmployee 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEmployee 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.employee._self / entity.employee.{{field}}；ResourceGroup=Personnel；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEmployeeTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.employee._self
            new TranslationSeedItem("entity.employee._self", "en-US", "Employee Information_us", "实体名称"),
            // entity.employee._self
            new TranslationSeedItem("entity.employee._self", "ja-JP", "员工信息_jp", "实体名称"),
            // entity.employee._self
            new TranslationSeedItem("entity.employee._self", "zh-CN", "员工信息", "实体名称"),
            // entity.employee._self
            new TranslationSeedItem("entity.employee._self", "zh-HK", "员工信息_hk", "实体名称"),

            // entity.employee.code
            new TranslationSeedItem("entity.employee.code", "en-US", "员工编码_us", "员工编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 员工编码规则生成并展示，非手输；单据类型菜单：员工档案）"),
            // entity.employee.code
            new TranslationSeedItem("entity.employee.code", "ja-JP", "员工编码_jp", "员工编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 员工编码规则生成并展示，非手输；单据类型菜单：员工档案）"),
            // entity.employee.code
            new TranslationSeedItem("entity.employee.code", "zh-CN", "员工编码", "员工编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 员工编码规则生成并展示，非手输；单据类型菜单：员工档案）"),
            // entity.employee.code
            new TranslationSeedItem("entity.employee.code", "zh-HK", "员工编码_hk", "员工编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 员工编码规则生成并展示，非手输；单据类型菜单：员工档案）"),

            // entity.employee.name
            new TranslationSeedItem("entity.employee.name", "en-US", "姓名_us", "姓名（2–80 位）"),
            // entity.employee.name
            new TranslationSeedItem("entity.employee.name", "ja-JP", "姓名_jp", "姓名（2–80 位）"),
            // entity.employee.name
            new TranslationSeedItem("entity.employee.name", "zh-CN", "姓名", "姓名（2–80 位）"),
            // entity.employee.name
            new TranslationSeedItem("entity.employee.name", "zh-HK", "姓名_hk", "姓名（2–80 位）"),

            // entity.employee.gender
            new TranslationSeedItem("entity.employee.gender", "en-US", "性别_us", "性别（字典 sys_user_gender；0=未知 1=男 2=女）"),
            // entity.employee.gender
            new TranslationSeedItem("entity.employee.gender", "ja-JP", "性别_jp", "性别（字典 sys_user_gender；0=未知 1=男 2=女）"),
            // entity.employee.gender
            new TranslationSeedItem("entity.employee.gender", "zh-CN", "性别", "性别（字典 sys_user_gender；0=未知 1=男 2=女）"),
            // entity.employee.gender
            new TranslationSeedItem("entity.employee.gender", "zh-HK", "性别_hk", "性别（字典 sys_user_gender；0=未知 1=男 2=女）"),

            // entity.employee.birthdate
            new TranslationSeedItem("entity.employee.birthdate", "en-US", "出生日期_us", "出生日期（人事档案必填）"),
            // entity.employee.birthdate
            new TranslationSeedItem("entity.employee.birthdate", "ja-JP", "出生日期_jp", "出生日期（人事档案必填）"),
            // entity.employee.birthdate
            new TranslationSeedItem("entity.employee.birthdate", "zh-CN", "出生日期", "出生日期（人事档案必填）"),
            // entity.employee.birthdate
            new TranslationSeedItem("entity.employee.birthdate", "zh-HK", "出生日期_hk", "出生日期（人事档案必填）"),

            // entity.employee.idcardcode
            new TranslationSeedItem("entity.employee.idcardcode", "en-US", "身份证号_us", "身份证号（人事档案必填）"),
            // entity.employee.idcardcode
            new TranslationSeedItem("entity.employee.idcardcode", "ja-JP", "身份证号_jp", "身份证号（人事档案必填）"),
            // entity.employee.idcardcode
            new TranslationSeedItem("entity.employee.idcardcode", "zh-CN", "身份证号", "身份证号（人事档案必填）"),
            // entity.employee.idcardcode
            new TranslationSeedItem("entity.employee.idcardcode", "zh-HK", "身份证号_hk", "身份证号（人事档案必填）"),

            // entity.employee.mobile
            new TranslationSeedItem("entity.employee.mobile", "en-US", "手机号码_us", "手机号码（人事档案必填）"),
            // entity.employee.mobile
            new TranslationSeedItem("entity.employee.mobile", "ja-JP", "手机号码_jp", "手机号码（人事档案必填）"),
            // entity.employee.mobile
            new TranslationSeedItem("entity.employee.mobile", "zh-CN", "手机号码", "手机号码（人事档案必填）"),
            // entity.employee.mobile
            new TranslationSeedItem("entity.employee.mobile", "zh-HK", "手机号码_hk", "手机号码（人事档案必填）"),

            // entity.employee.email
            new TranslationSeedItem("entity.employee.email", "en-US", "电子邮箱_us", "电子邮箱"),
            // entity.employee.email
            new TranslationSeedItem("entity.employee.email", "ja-JP", "电子邮箱_jp", "电子邮箱"),
            // entity.employee.email
            new TranslationSeedItem("entity.employee.email", "zh-CN", "电子邮箱", "电子邮箱"),
            // entity.employee.email
            new TranslationSeedItem("entity.employee.email", "zh-HK", "电子邮箱_hk", "电子邮箱"),

            // entity.employee.nativeplace
            new TranslationSeedItem("entity.employee.nativeplace", "en-US", "籍贯_us", "籍贯（字典 humanresource_personnel_native_place_code；列存 6 位 GB 行政区划代码，人事档案必填；与住址子表无关）"),
            // entity.employee.nativeplace
            new TranslationSeedItem("entity.employee.nativeplace", "ja-JP", "籍贯_jp", "籍贯（字典 humanresource_personnel_native_place_code；列存 6 位 GB 行政区划代码，人事档案必填；与住址子表无关）"),
            // entity.employee.nativeplace
            new TranslationSeedItem("entity.employee.nativeplace", "zh-CN", "籍贯", "籍贯（字典 humanresource_personnel_native_place_code；列存 6 位 GB 行政区划代码，人事档案必填；与住址子表无关）"),
            // entity.employee.nativeplace
            new TranslationSeedItem("entity.employee.nativeplace", "zh-HK", "籍贯_hk", "籍贯（字典 humanresource_personnel_native_place_code；列存 6 位 GB 行政区划代码，人事档案必填；与住址子表无关）"),

            // entity.employee.ethnicity
            new TranslationSeedItem("entity.employee.ethnicity", "en-US", "民族_us", "民族（字典 humanresource_personnel_ethnic_code；DictValue 1～56）"),
            // entity.employee.ethnicity
            new TranslationSeedItem("entity.employee.ethnicity", "ja-JP", "民族_jp", "民族（字典 humanresource_personnel_ethnic_code；DictValue 1～56）"),
            // entity.employee.ethnicity
            new TranslationSeedItem("entity.employee.ethnicity", "zh-CN", "民族", "民族（字典 humanresource_personnel_ethnic_code；DictValue 1～56）"),
            // entity.employee.ethnicity
            new TranslationSeedItem("entity.employee.ethnicity", "zh-HK", "民族_hk", "民族（字典 humanresource_personnel_ethnic_code；DictValue 1～56）"),

            // entity.employee.politicalaffiliation
            new TranslationSeedItem("entity.employee.politicalaffiliation", "en-US", "政治面貌_us", "政治面貌（字典 humanresource_personnel_political_affiliation；0～12；人事档案必填）"),
            // entity.employee.politicalaffiliation
            new TranslationSeedItem("entity.employee.politicalaffiliation", "ja-JP", "政治面貌_jp", "政治面貌（字典 humanresource_personnel_political_affiliation；0～12；人事档案必填）"),
            // entity.employee.politicalaffiliation
            new TranslationSeedItem("entity.employee.politicalaffiliation", "zh-CN", "政治面貌", "政治面貌（字典 humanresource_personnel_political_affiliation；0～12；人事档案必填）"),
            // entity.employee.politicalaffiliation
            new TranslationSeedItem("entity.employee.politicalaffiliation", "zh-HK", "政治面貌_hk", "政治面貌（字典 humanresource_personnel_political_affiliation；0～12；人事档案必填）"),

            // entity.employee.maritalstatus
            new TranslationSeedItem("entity.employee.maritalstatus", "en-US", "婚姻状况_us", "婚姻状况（字典 humanresource_personnel_marital_status；0=未婚 1=已婚 2=离异 3=丧偶；人事档案必填）"),
            // entity.employee.maritalstatus
            new TranslationSeedItem("entity.employee.maritalstatus", "ja-JP", "婚姻状况_jp", "婚姻状况（字典 humanresource_personnel_marital_status；0=未婚 1=已婚 2=离异 3=丧偶；人事档案必填）"),
            // entity.employee.maritalstatus
            new TranslationSeedItem("entity.employee.maritalstatus", "zh-CN", "婚姻状况", "婚姻状况（字典 humanresource_personnel_marital_status；0=未婚 1=已婚 2=离异 3=丧偶；人事档案必填）"),
            // entity.employee.maritalstatus
            new TranslationSeedItem("entity.employee.maritalstatus", "zh-HK", "婚姻状况_hk", "婚姻状况（字典 humanresource_personnel_marital_status；0=未婚 1=已婚 2=离异 3=丧偶；人事档案必填）"),

            // entity.employee.status
            new TranslationSeedItem("entity.employee.status", "en-US", "员工状态_us", "员工状态（字典 humanresource_personnel_employee_status；1=试用期 2=正式 3=离职 4=退休）"),
            // entity.employee.status
            new TranslationSeedItem("entity.employee.status", "ja-JP", "员工状态_jp", "员工状态（字典 humanresource_personnel_employee_status；1=试用期 2=正式 3=离职 4=退休）"),
            // entity.employee.status
            new TranslationSeedItem("entity.employee.status", "zh-CN", "员工状态", "员工状态（字典 humanresource_personnel_employee_status；1=试用期 2=正式 3=离职 4=退休）"),
            // entity.employee.status
            new TranslationSeedItem("entity.employee.status", "zh-HK", "员工状态_hk", "员工状态（字典 humanresource_personnel_employee_status；1=试用期 2=正式 3=离职 4=退休）"),

            // entity.employee.isbuiltin
            new TranslationSeedItem("entity.employee.isbuiltin", "en-US", "内置_us", "内置（字典 sys_yes_no；0=否 1=是；种子员工不可删）"),
            // entity.employee.isbuiltin
            new TranslationSeedItem("entity.employee.isbuiltin", "ja-JP", "内置_jp", "内置（字典 sys_yes_no；0=否 1=是；种子员工不可删）"),
            // entity.employee.isbuiltin
            new TranslationSeedItem("entity.employee.isbuiltin", "zh-CN", "内置", "内置（字典 sys_yes_no；0=否 1=是；种子员工不可删）"),
            // entity.employee.isbuiltin
            new TranslationSeedItem("entity.employee.isbuiltin", "zh-HK", "内置_hk", "内置（字典 sys_yes_no；0=否 1=是；种子员工不可删）"),

            // entity.employee.avatar
            new TranslationSeedItem("entity.employee.avatar", "en-US", "头像URL_us", "头像URL（展示用；档案附件明细见 EmployeeAttachments）"),
            // entity.employee.avatar
            new TranslationSeedItem("entity.employee.avatar", "ja-JP", "头像URL_jp", "头像URL（展示用；档案附件明细见 EmployeeAttachments）"),
            // entity.employee.avatar
            new TranslationSeedItem("entity.employee.avatar", "zh-CN", "头像URL", "头像URL（展示用；档案附件明细见 EmployeeAttachments）"),
            // entity.employee.avatar
            new TranslationSeedItem("entity.employee.avatar", "zh-HK", "头像URL_hk", "头像URL（展示用；档案附件明细见 EmployeeAttachments）"),

            // entity.employee.depts
            new TranslationSeedItem("entity.employee.depts", "en-US", "员工部门关联_us", "员工部门关联（RBAC，表 takt_human_resource_organization_employeedept）"),
            // entity.employee.depts
            new TranslationSeedItem("entity.employee.depts", "ja-JP", "员工部门关联_jp", "员工部门关联（RBAC，表 takt_human_resource_organization_employeedept）"),
            // entity.employee.depts
            new TranslationSeedItem("entity.employee.depts", "zh-CN", "员工部门关联", "员工部门关联（RBAC，表 takt_human_resource_organization_employeedept）"),
            // entity.employee.depts
            new TranslationSeedItem("entity.employee.depts", "zh-HK", "员工部门关联_hk", "员工部门关联（RBAC，表 takt_human_resource_organization_employeedept）"),

            // entity.employee.posts
            new TranslationSeedItem("entity.employee.posts", "en-US", "员工岗位关联_us", "员工岗位关联（RBAC，表 takt_human_resource_organization_employeepost）"),
            // entity.employee.posts
            new TranslationSeedItem("entity.employee.posts", "ja-JP", "员工岗位关联_jp", "员工岗位关联（RBAC，表 takt_human_resource_organization_employeepost）"),
            // entity.employee.posts
            new TranslationSeedItem("entity.employee.posts", "zh-CN", "员工岗位关联", "员工岗位关联（RBAC，表 takt_human_resource_organization_employeepost）"),
            // entity.employee.posts
            new TranslationSeedItem("entity.employee.posts", "zh-HK", "员工岗位关联_hk", "员工岗位关联（RBAC，表 takt_human_resource_organization_employeepost）"),

            // entity.employee.addresses
            new TranslationSeedItem("entity.employee.addresses", "en-US", "员工地址_us", "员工地址（家庭/工作/常住）"),
            // entity.employee.addresses
            new TranslationSeedItem("entity.employee.addresses", "ja-JP", "员工地址_jp", "员工地址（家庭/工作/常住）"),
            // entity.employee.addresses
            new TranslationSeedItem("entity.employee.addresses", "zh-CN", "员工地址", "员工地址（家庭/工作/常住）"),
            // entity.employee.addresses
            new TranslationSeedItem("entity.employee.addresses", "zh-HK", "员工地址_hk", "员工地址（家庭/工作/常住）"),

            // entity.employee.educations
            new TranslationSeedItem("entity.employee.educations", "en-US", "教育经历_us", "教育经历（含最高学历 IsHighest）"),
            // entity.employee.educations
            new TranslationSeedItem("entity.employee.educations", "ja-JP", "教育经历_jp", "教育经历（含最高学历 IsHighest）"),
            // entity.employee.educations
            new TranslationSeedItem("entity.employee.educations", "zh-CN", "教育经历", "教育经历（含最高学历 IsHighest）"),
            // entity.employee.educations
            new TranslationSeedItem("entity.employee.educations", "zh-HK", "教育经历_hk", "教育经历（含最高学历 IsHighest）"),

            // entity.employee.families
            new TranslationSeedItem("entity.employee.families", "en-US", "家庭成员_us", "家庭成员（含紧急联系人 IsEmergencyContact）"),
            // entity.employee.families
            new TranslationSeedItem("entity.employee.families", "ja-JP", "家庭成员_jp", "家庭成员（含紧急联系人 IsEmergencyContact）"),
            // entity.employee.families
            new TranslationSeedItem("entity.employee.families", "zh-CN", "家庭成员", "家庭成员（含紧急联系人 IsEmergencyContact）"),
            // entity.employee.families
            new TranslationSeedItem("entity.employee.families", "zh-HK", "家庭成员_hk", "家庭成员（含紧急联系人 IsEmergencyContact）"),

            // entity.employee.experiences
            new TranslationSeedItem("entity.employee.experiences", "en-US", "外部工作经历_us", "外部工作经历"),
            // entity.employee.experiences
            new TranslationSeedItem("entity.employee.experiences", "ja-JP", "外部工作经历_jp", "外部工作经历"),
            // entity.employee.experiences
            new TranslationSeedItem("entity.employee.experiences", "zh-CN", "外部工作经历", "外部工作经历"),
            // entity.employee.experiences
            new TranslationSeedItem("entity.employee.experiences", "zh-HK", "外部工作经历_hk", "外部工作经历"),

            // entity.employee.skills
            new TranslationSeedItem("entity.employee.skills", "en-US", "技能与证书_us", "技能与证书"),
            // entity.employee.skills
            new TranslationSeedItem("entity.employee.skills", "ja-JP", "技能与证书_jp", "技能与证书"),
            // entity.employee.skills
            new TranslationSeedItem("entity.employee.skills", "zh-CN", "技能与证书", "技能与证书"),
            // entity.employee.skills
            new TranslationSeedItem("entity.employee.skills", "zh-HK", "技能与证书_hk", "技能与证书"),

            // entity.employee.contracts
            new TranslationSeedItem("entity.employee.contracts", "en-US", "劳动合同_us", "劳动合同"),
            // entity.employee.contracts
            new TranslationSeedItem("entity.employee.contracts", "ja-JP", "劳动合同_jp", "劳动合同"),
            // entity.employee.contracts
            new TranslationSeedItem("entity.employee.contracts", "zh-CN", "劳动合同", "劳动合同"),
            // entity.employee.contracts
            new TranslationSeedItem("entity.employee.contracts", "zh-HK", "劳动合同_hk", "劳动合同"),

            // entity.employee.joineds
            new TranslationSeedItem("entity.employee.joineds", "en-US", "入职上岗办理_us", "入职上岗办理（实际上岗日/试用/转正/部门岗位）"),
            // entity.employee.joineds
            new TranslationSeedItem("entity.employee.joineds", "ja-JP", "入职上岗办理_jp", "入职上岗办理（实际上岗日/试用/转正/部门岗位）"),
            // entity.employee.joineds
            new TranslationSeedItem("entity.employee.joineds", "zh-CN", "入职上岗办理", "入职上岗办理（实际上岗日/试用/转正/部门岗位）"),
            // entity.employee.joineds
            new TranslationSeedItem("entity.employee.joineds", "zh-HK", "入职上岗办理_hk", "入职上岗办理（实际上岗日/试用/转正/部门岗位）"),

            // entity.employee.onboardings
            new TranslationSeedItem("entity.employee.onboardings", "en-US", "入职待办_us", "入职待办"),
            // entity.employee.onboardings
            new TranslationSeedItem("entity.employee.onboardings", "ja-JP", "入职待办_jp", "入职待办"),
            // entity.employee.onboardings
            new TranslationSeedItem("entity.employee.onboardings", "zh-CN", "入职待办", "入职待办"),
            // entity.employee.onboardings
            new TranslationSeedItem("entity.employee.onboardings", "zh-HK", "入职待办_hk", "入职待办"),

            // entity.employee.reassignments
            new TranslationSeedItem("entity.employee.reassignments", "en-US", "调动记录_us", "调动记录"),
            // entity.employee.reassignments
            new TranslationSeedItem("entity.employee.reassignments", "ja-JP", "调动记录_jp", "调动记录"),
            // entity.employee.reassignments
            new TranslationSeedItem("entity.employee.reassignments", "zh-CN", "调动记录", "调动记录"),
            // entity.employee.reassignments
            new TranslationSeedItem("entity.employee.reassignments", "zh-HK", "调动记录_hk", "调动记录"),

            // entity.employee.resignations
            new TranslationSeedItem("entity.employee.resignations", "en-US", "离职办理_us", "离职办理"),
            // entity.employee.resignations
            new TranslationSeedItem("entity.employee.resignations", "ja-JP", "离职办理_jp", "离职办理"),
            // entity.employee.resignations
            new TranslationSeedItem("entity.employee.resignations", "zh-CN", "离职办理", "离职办理"),
            // entity.employee.resignations
            new TranslationSeedItem("entity.employee.resignations", "zh-HK", "离职办理_hk", "离职办理"),

            // entity.employee.attachments
            new TranslationSeedItem("entity.employee.attachments", "en-US", "档案附件_us", "档案附件"),
            // entity.employee.attachments
            new TranslationSeedItem("entity.employee.attachments", "ja-JP", "档案附件_jp", "档案附件"),
            // entity.employee.attachments
            new TranslationSeedItem("entity.employee.attachments", "zh-CN", "档案附件", "档案附件"),
            // entity.employee.attachments
            new TranslationSeedItem("entity.employee.attachments", "zh-HK", "档案附件_hk", "档案附件"),

            // entity.employee.delegations
            new TranslationSeedItem("entity.employee.delegations", "en-US", "员工代理_us", "员工代理（被代理人视角；外键 OriginalEmployeeId）"),
            // entity.employee.delegations
            new TranslationSeedItem("entity.employee.delegations", "ja-JP", "员工代理_jp", "员工代理（被代理人视角；外键 OriginalEmployeeId）"),
            // entity.employee.delegations
            new TranslationSeedItem("entity.employee.delegations", "zh-CN", "员工代理", "员工代理（被代理人视角；外键 OriginalEmployeeId）"),
            // entity.employee.delegations
            new TranslationSeedItem("entity.employee.delegations", "zh-HK", "员工代理_hk", "员工代理（被代理人视角；外键 OriginalEmployeeId）"),
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
