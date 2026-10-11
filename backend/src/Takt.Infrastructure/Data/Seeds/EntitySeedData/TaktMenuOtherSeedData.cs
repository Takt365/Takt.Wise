// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds
// 文件名称：TaktMenuOtherSeedData.cs
// 创建时间：2026-08-25
// 创建人：Takt365(Cursor AI)
// 功能描述：无独立页面菜单的附属能力权限种子（MenuType=2）。
//           挂在父级 L2 页面菜单下，写入嵌套 Permission（如 room:list、version:create），
//           供角色授权与 [TaktPermission] / 前端 v-permission 对齐。
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;
using System.Text;
using Takt.Domain.Entities.Identity;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 无独立页面菜单的附属能力权限种子。
/// <para>
/// 由 TaktMenuSeedData 在 TaktMenuButtonSeedData 之后调用；不直接注册为 ITaktSeedDataCoordinator。
/// 与「按页面模板批量生成 CRUD 按钮」不同：本类按业务清单显式挂载嵌套资源权限。
/// </para>
/// </summary>
public class TaktMenuOtherSeedData
{
    /// <summary>
    /// 标准 CRUD 操作后缀（与控制器 [TaktPermission] 末段对齐；含 list，因无独立页面菜单）。
    /// </summary>
    private static readonly string[] CrudActions =
    [
        "list", "query", "create", "update", "delete", "import", "export"
    ];

    /// <summary>
    /// 操作显示名（与 CrudActions 一一对应）。
    /// </summary>
    private static readonly string[] CrudActionNames =
    [
        "列表", "查询", "新增", "修改", "删除", "导入", "导出"
    ];

    /// <summary>
    /// 附属资源定义：父页面 MenuCode → 资源段（Permission 中间段）→ 中文资源名。
    /// Permission = 父页 Permission 去掉 :list + :{resource}:{action}。
    /// </summary>
    private static readonly (string ParentMenuCode, string ResourceSegment, string ResourceName)[] OtherResources =
    [
        ("ROUTINE_MEETING_CENTER_MEETING", "minutes", "会后纪要"),
        ("ROUTINE_MEETING_CENTER_MEETING", "attendee", "出席人"),
        ("ROUTINE_MEETING_CENTER_MEETING", "notification", "会议通知"),
        ("ROUTINE_DOCUMENT_CENTER", "version", "版本"),
        ("ROUTINE_NEWS_CENTER", "comment", "评论"),
        ("ROUTINE_NEWS_CENTER", "like", "点赞"),
        ("ROUTINE_NEWS_CENTER", "favorite", "收藏"),
        ("ROUTINE_NEWS_CENTER", "share", "分享"),
        ("ROUTINE_NEWS_CENTER", "read", "阅读"),
        ("ROUTINE_NEWS_CENTER", "comment:like", "评论点赞"),
    ];

    /// <summary>
    /// 初始化附属能力按钮权限。
    /// </summary>
    /// <param name="serviceProvider">服务提供者。</param>
    /// <param name="specifiedTenantCode">租户编码。</param>
    /// <returns>元组：(InsertCount, UpdateCount)。</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? specifiedTenantCode = null)
    {
        var menuRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktMenu>>();
        if (string.IsNullOrWhiteSpace(specifiedTenantCode))
        {
            TaktLogger.Warning("未指定租户编码,跳过附属菜单权限种子数据初始化");
            return (0, 0);
        }

        var tenantCode = specifiedTenantCode;
        int insertCount = 0;
        int updateCount = 0;
        var sortBase = 9000;

        foreach (var (parentMenuCode, resourceSegment, resourceName) in OtherResources)
        {
            var parent = await menuRepository.FirstAsync(m =>
                m.TenantCode == tenantCode
                && m.MenuCode == parentMenuCode
                && m.MenuType == 1
                && m.IsDeleted == 0);
            if (parent == null)
            {
                TaktLogger.Warning(
                    "附属权限种子跳过：未找到父页面菜单 {MenuCode}（租户 {TenantCode}）",
                    parentMenuCode,
                    tenantCode);
                continue;
            }

            if (string.IsNullOrWhiteSpace(parent.Permission)
                || !parent.Permission.EndsWith(":list", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    $"父页面菜单 {parentMenuCode} 的 Permission 必须非空且以 \":list\" 结尾，当前为 {parent.Permission}。");
            }

            var permissionPrefix = parent.Permission[..^":list".Length];
            for (var i = 0; i < CrudActions.Length; i++)
            {
                var action = CrudActions[i];
                var actionName = CrudActionNames[i];
                var permission = $"{permissionPrefix}:{resourceSegment}:{action}";
                var codeSuffix = $"{resourceSegment.Replace(':', '_')}_{action}".ToUpperInvariant();
                var buttonCode = BuildButtonCode(parent.MenuCode, codeSuffix);
                var buttonName = $"{resourceName}-{actionName}";
                var i18nKey = TaktCommonI18nKeys.MenuButton(action);
                var sortOrder = sortBase + i;
                var (insert, update) = await CreateOrUpdateButtonAsync(
                    menuRepository,
                    tenantCode,
                    parent.Id,
                    buttonCode,
                    buttonName,
                    permission,
                    i18nKey,
                    sortOrder);
                insertCount += insert;
                updateCount += update;
            }

            sortBase += 100;
        }

        return (insertCount, updateCount);
    }

    /// <summary>
    /// 生成按钮编码：MenuCode + _ + 后缀；超长时截断并附加哈希。
    /// </summary>
    /// <param name="menuCode">父菜单编码。</param>
    /// <param name="codeSuffix">资源_操作后缀（大写）。</param>
    /// <returns>按钮 MenuCode。</returns>
    private static string BuildButtonCode(string menuCode, string codeSuffix)
    {
        var code = $"{menuCode}_{codeSuffix}";
        if (code.Length <= 120)
        {
            return code;
        }

        var hash = ComputeStableHash(code);
        return code[..111] + "_" + hash;
    }

    /// <summary>
    /// 计算稳定哈希（8 位十六进制）。
    /// </summary>
    /// <param name="input">输入。</param>
    /// <returns>哈希串。</returns>
    private static string ComputeStableHash(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes)[..8];
    }

    /// <summary>
    /// 创建或更新单个按钮菜单（MenuType=2）。
    /// </summary>
    private static async Task<(int InsertCount, int UpdateCount)> CreateOrUpdateButtonAsync(
        ITaktTenantSeedRepository<TaktMenu> menuRepository,
        string tenantCode,
        long parentId,
        string buttonCode,
        string buttonName,
        string permission,
        string menuL10nKey,
        int sortOrder)
    {
        var button = await menuRepository.FirstAsync(m => m.TenantCode == tenantCode && m.MenuCode == buttonCode);
        if (button == null)
        {
            button = await menuRepository.FirstAsync(m =>
                m.TenantCode == tenantCode
                && m.ParentId == parentId
                && m.MenuType == 2
                && m.Permission == permission);
        }

        if (button == null)
        {
            button = new TaktMenu
            {
                TenantCode = tenantCode,
                MenuName = buttonName,
                MenuCode = buttonCode,
                I18nKey = menuL10nKey,
                ParentId = parentId,
                MenuType = 2,
                Permission = permission,
                MenuStatus = 1,
                IsVisible = 1,
                SortOrder = sortOrder,
                IsCached = 0,
                IsExternal = 0,
                Level = 0,
                IsLeaf = 1,
                IsBuiltIn = 1,
                CreatedBy = 900001,
                CreatedAt = DateTime.Now
            };
            button = await menuRepository.CreateAsync(button);
            if (button.ParentId > 0)
            {
                var parentMenu = await menuRepository.FirstAsync(m => m.TenantCode == tenantCode && m.Id == button.ParentId);
                if (parentMenu != null)
                {
                    button.MenuPath = $"{parentMenu.MenuPath}{button.Id}/";
                    button.Level = parentMenu.Level + 1;
                    if (parentMenu.IsLeaf == 1)
                    {
                        parentMenu.IsLeaf = 0;
                        await menuRepository.UpdateAsync(parentMenu);
                    }
                }
            }
            else
            {
                button.MenuPath = $"/{button.Id}/";
                button.Level = 1;
            }

            await menuRepository.UpdateAsync(button);
            return (1, 0);
        }

        button.MenuName = buttonName;
        button.MenuCode = buttonCode;
        button.I18nKey = menuL10nKey;
        button.Permission = permission;
        button.MenuStatus = 1;
        button.IsVisible = 1;
        button.SortOrder = sortOrder;
        button.IsBuiltIn = 1;
        if (button.ParentId != parentId)
        {
            button.ParentId = parentId;
            if (button.ParentId > 0)
            {
                var parentMenu = await menuRepository.FirstAsync(m => m.TenantCode == tenantCode && m.Id == button.ParentId);
                if (parentMenu != null)
                {
                    button.MenuPath = $"{parentMenu.MenuPath}{button.Id}/";
                    button.Level = parentMenu.Level + 1;
                    if (parentMenu.IsLeaf == 1)
                    {
                        parentMenu.IsLeaf = 0;
                        await menuRepository.UpdateAsync(parentMenu);
                    }
                }
            }
        }

        button.UpdatedBy = 900001;
        button.UpdatedAt = DateTime.Now;
        await menuRepository.UpdateAsync(button);
        return (0, 1);
    }
}
