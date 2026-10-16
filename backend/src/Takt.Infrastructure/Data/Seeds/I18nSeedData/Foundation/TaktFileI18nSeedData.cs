// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Foundation
// 文件名称：TaktFileI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktFile 实体字段国际化种子（已对齐前端 locales：src/locales/foundation/file）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Foundation;

/// <summary>
/// TaktFile 实体国际化翻译种子（键前缀 entity.file.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktFileI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktFile 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 file 实体翻译...", tenantCode);

        foreach (var item in GetFileTranslations())
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

        TaktLogger.Information("TaktFile 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktFile 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.file._self / entity.file.{{field}}；ResourceGroup=Foundation；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetFileTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.file._self
            new TranslationSeedItem("entity.file._self", "en-US", "File Information_us", "实体名称"),
            // entity.file._self
            new TranslationSeedItem("entity.file._self", "ja-JP", "文件信息_jp", "实体名称"),
            // entity.file._self
            new TranslationSeedItem("entity.file._self", "zh-CN", "文件信息", "实体名称"),
            // entity.file._self
            new TranslationSeedItem("entity.file._self", "zh-HK", "文件信息_hk", "实体名称"),

            // entity.file.code
            new TranslationSeedItem("entity.file.code", "en-US", "文件编码_us", "文件编码（唯一索引：租户+公司内唯一，见 ix_file_code_unique；根据 MIME 类型自动通过 TaktNumbering 文件编码规则生成，非表单手选）"),
            // entity.file.code
            new TranslationSeedItem("entity.file.code", "ja-JP", "文件编码_jp", "文件编码（唯一索引：租户+公司内唯一，见 ix_file_code_unique；根据 MIME 类型自动通过 TaktNumbering 文件编码规则生成，非表单手选）"),
            // entity.file.code
            new TranslationSeedItem("entity.file.code", "zh-CN", "文件编码", "文件编码（唯一索引：租户+公司内唯一，见 ix_file_code_unique；根据 MIME 类型自动通过 TaktNumbering 文件编码规则生成，非表单手选）"),
            // entity.file.code
            new TranslationSeedItem("entity.file.code", "zh-HK", "文件编码_hk", "文件编码（唯一索引：租户+公司内唯一，见 ix_file_code_unique；根据 MIME 类型自动通过 TaktNumbering 文件编码规则生成，非表单手选）"),

            // entity.file.name
            new TranslationSeedItem("entity.file.name", "en-US", "文件名称_us", "文件名称（字典 sys_storage_naming；0=原文件+哈希值 1=自动生成 2=自定义）"),
            // entity.file.name
            new TranslationSeedItem("entity.file.name", "ja-JP", "文件名称_jp", "文件名称（字典 sys_storage_naming；0=原文件+哈希值 1=自动生成 2=自定义）"),
            // entity.file.name
            new TranslationSeedItem("entity.file.name", "zh-CN", "文件名称", "文件名称（字典 sys_storage_naming；0=原文件+哈希值 1=自动生成 2=自定义）"),
            // entity.file.name
            new TranslationSeedItem("entity.file.name", "zh-HK", "文件名称_hk", "文件名称（字典 sys_storage_naming；0=原文件+哈希值 1=自动生成 2=自定义）"),

            // entity.file.originalname
            new TranslationSeedItem("entity.file.originalname", "en-US", "原始名称_us", "文件原始名称（上传时的原始文件名）"),
            // entity.file.originalname
            new TranslationSeedItem("entity.file.originalname", "ja-JP", "原始名称_jp", "文件原始名称（上传时的原始文件名）"),
            // entity.file.originalname
            new TranslationSeedItem("entity.file.originalname", "zh-CN", "原始名称", "文件原始名称（上传时的原始文件名）"),
            // entity.file.originalname
            new TranslationSeedItem("entity.file.originalname", "zh-HK", "原始名称_hk", "文件原始名称（上传时的原始文件名）"),

            // entity.file.path
            new TranslationSeedItem("entity.file.path", "en-US", "文件路径_us", "文件路径（关联一级菜单 uploadPath，选项 useMenuUploadPath）"),
            // entity.file.path
            new TranslationSeedItem("entity.file.path", "ja-JP", "文件路径_jp", "文件路径（关联一级菜单 uploadPath，选项 useMenuUploadPath）"),
            // entity.file.path
            new TranslationSeedItem("entity.file.path", "zh-CN", "文件路径", "文件路径（关联一级菜单 uploadPath，选项 useMenuUploadPath）"),
            // entity.file.path
            new TranslationSeedItem("entity.file.path", "zh-HK", "文件路径_hk", "文件路径（关联一级菜单 uploadPath，选项 useMenuUploadPath）"),

            // entity.file.size
            new TranslationSeedItem("entity.file.size", "en-US", "文件大小_us", "文件大小（字节）"),
            // entity.file.size
            new TranslationSeedItem("entity.file.size", "ja-JP", "文件大小_jp", "文件大小（字节）"),
            // entity.file.size
            new TranslationSeedItem("entity.file.size", "zh-CN", "文件大小", "文件大小（字节）"),
            // entity.file.size
            new TranslationSeedItem("entity.file.size", "zh-HK", "文件大小_hk", "文件大小（字节）"),

            // entity.file.type
            new TranslationSeedItem("entity.file.type", "en-US", "MIME类型_us", "文件 MIME 类型"),
            // entity.file.type
            new TranslationSeedItem("entity.file.type", "ja-JP", "MIME类型_jp", "文件 MIME 类型"),
            // entity.file.type
            new TranslationSeedItem("entity.file.type", "zh-CN", "MIME类型", "文件 MIME 类型"),
            // entity.file.type
            new TranslationSeedItem("entity.file.type", "zh-HK", "MIME类型_hk", "文件 MIME 类型"),

            // entity.file.extension
            new TranslationSeedItem("entity.file.extension", "en-US", "扩展名_us", "文件扩展名"),
            // entity.file.extension
            new TranslationSeedItem("entity.file.extension", "ja-JP", "扩展名_jp", "文件扩展名"),
            // entity.file.extension
            new TranslationSeedItem("entity.file.extension", "zh-CN", "扩展名", "文件扩展名"),
            // entity.file.extension
            new TranslationSeedItem("entity.file.extension", "zh-HK", "扩展名_hk", "文件扩展名"),

            // entity.file.hash
            new TranslationSeedItem("entity.file.hash", "en-US", "哈希值_us", "文件哈希值（MD5 或 SHA256，用于去重与校验）"),
            // entity.file.hash
            new TranslationSeedItem("entity.file.hash", "ja-JP", "哈希值_jp", "文件哈希值（MD5 或 SHA256，用于去重与校验）"),
            // entity.file.hash
            new TranslationSeedItem("entity.file.hash", "zh-CN", "哈希值", "文件哈希值（MD5 或 SHA256，用于去重与校验）"),
            // entity.file.hash
            new TranslationSeedItem("entity.file.hash", "zh-HK", "哈希值_hk", "文件哈希值（MD5 或 SHA256，用于去重与校验）"),

            // entity.file.category
            new TranslationSeedItem("entity.file.category", "en-US", "文件分类_us", "文件分类（根据 FileType/MIME 自动推断：0=文档，1=图片，2=视频，3=音频，4=压缩包，5=其他）"),
            // entity.file.category
            new TranslationSeedItem("entity.file.category", "ja-JP", "文件分类_jp", "文件分类（根据 FileType/MIME 自动推断：0=文档，1=图片，2=视频，3=音频，4=压缩包，5=其他）"),
            // entity.file.category
            new TranslationSeedItem("entity.file.category", "zh-CN", "文件分类", "文件分类（根据 FileType/MIME 自动推断：0=文档，1=图片，2=视频，3=音频，4=压缩包，5=其他）"),
            // entity.file.category
            new TranslationSeedItem("entity.file.category", "zh-HK", "文件分类_hk", "文件分类（根据 FileType/MIME 自动推断：0=文档，1=图片，2=视频，3=音频，4=压缩包，5=其他）"),

            // entity.file.storagetype
            new TranslationSeedItem("entity.file.storagetype", "en-US", "存储方式_us", "存储方式（字典 sys_storage_type；0=本地存储 1=OSS对象存储 2=FTP）"),
            // entity.file.storagetype
            new TranslationSeedItem("entity.file.storagetype", "ja-JP", "存储方式_jp", "存储方式（字典 sys_storage_type；0=本地存储 1=OSS对象存储 2=FTP）"),
            // entity.file.storagetype
            new TranslationSeedItem("entity.file.storagetype", "zh-CN", "存储方式", "存储方式（字典 sys_storage_type；0=本地存储 1=OSS对象存储 2=FTP）"),
            // entity.file.storagetype
            new TranslationSeedItem("entity.file.storagetype", "zh-HK", "存储方式_hk", "存储方式（字典 sys_storage_type；0=本地存储 1=OSS对象存储 2=FTP）"),

            // entity.file.storageconfig
            new TranslationSeedItem("entity.file.storageconfig", "en-US", "存储配置_us", "存储配置（JSON，OSS/FTP 等扩展配置）"),
            // entity.file.storageconfig
            new TranslationSeedItem("entity.file.storageconfig", "ja-JP", "存储配置_jp", "存储配置（JSON，OSS/FTP 等扩展配置）"),
            // entity.file.storageconfig
            new TranslationSeedItem("entity.file.storageconfig", "zh-CN", "存储配置", "存储配置（JSON，OSS/FTP 等扩展配置）"),
            // entity.file.storageconfig
            new TranslationSeedItem("entity.file.storageconfig", "zh-HK", "存储配置_hk", "存储配置（JSON，OSS/FTP 等扩展配置）"),

            // entity.file.accessurl
            new TranslationSeedItem("entity.file.accessurl", "en-US", "访问地址_us", "访问地址（文件 URL）"),
            // entity.file.accessurl
            new TranslationSeedItem("entity.file.accessurl", "ja-JP", "访问地址_jp", "访问地址（文件 URL）"),
            // entity.file.accessurl
            new TranslationSeedItem("entity.file.accessurl", "zh-CN", "访问地址", "访问地址（文件 URL）"),
            // entity.file.accessurl
            new TranslationSeedItem("entity.file.accessurl", "zh-HK", "访问地址_hk", "访问地址（文件 URL）"),

            // entity.file.downloadcount
            new TranslationSeedItem("entity.file.downloadcount", "en-US", "下载次数_us", "下载次数"),
            // entity.file.downloadcount
            new TranslationSeedItem("entity.file.downloadcount", "ja-JP", "下载次数_jp", "下载次数"),
            // entity.file.downloadcount
            new TranslationSeedItem("entity.file.downloadcount", "zh-CN", "下载次数", "下载次数"),
            // entity.file.downloadcount
            new TranslationSeedItem("entity.file.downloadcount", "zh-HK", "下载次数_hk", "下载次数"),

            // entity.file.lastdownloadtime
            new TranslationSeedItem("entity.file.lastdownloadtime", "en-US", "最后下载_us", "最后下载时间"),
            // entity.file.lastdownloadtime
            new TranslationSeedItem("entity.file.lastdownloadtime", "ja-JP", "最后下载_jp", "最后下载时间"),
            // entity.file.lastdownloadtime
            new TranslationSeedItem("entity.file.lastdownloadtime", "zh-CN", "最后下载", "最后下载时间"),
            // entity.file.lastdownloadtime
            new TranslationSeedItem("entity.file.lastdownloadtime", "zh-HK", "最后下载_hk", "最后下载时间"),

            // entity.file.ispublic
            new TranslationSeedItem("entity.file.ispublic", "en-US", "公开_us", "公开（字典 sys_public_type；0=公开同公司可见，1=私有仅创建人可见/可改/可下载）"),
            // entity.file.ispublic
            new TranslationSeedItem("entity.file.ispublic", "ja-JP", "公开_jp", "公开（字典 sys_public_type；0=公开同公司可见，1=私有仅创建人可见/可改/可下载）"),
            // entity.file.ispublic
            new TranslationSeedItem("entity.file.ispublic", "zh-CN", "公开", "公开（字典 sys_public_type；0=公开同公司可见，1=私有仅创建人可见/可改/可下载）"),
            // entity.file.ispublic
            new TranslationSeedItem("entity.file.ispublic", "zh-HK", "公开_hk", "公开（字典 sys_public_type；0=公开同公司可见，1=私有仅创建人可见/可改/可下载）"),

            // entity.file.description
            new TranslationSeedItem("entity.file.description", "en-US", "文件描述_us", "文件描述"),
            // entity.file.description
            new TranslationSeedItem("entity.file.description", "ja-JP", "文件描述_jp", "文件描述"),
            // entity.file.description
            new TranslationSeedItem("entity.file.description", "zh-CN", "文件描述", "文件描述"),
            // entity.file.description
            new TranslationSeedItem("entity.file.description", "zh-HK", "文件描述_hk", "文件描述"),

            // entity.file.tags
            new TranslationSeedItem("entity.file.tags", "en-US", "文件标签_us", "文件标签（多个标签用逗号分隔）"),
            // entity.file.tags
            new TranslationSeedItem("entity.file.tags", "ja-JP", "文件标签_jp", "文件标签（多个标签用逗号分隔）"),
            // entity.file.tags
            new TranslationSeedItem("entity.file.tags", "zh-CN", "文件标签", "文件标签（多个标签用逗号分隔）"),
            // entity.file.tags
            new TranslationSeedItem("entity.file.tags", "zh-HK", "文件标签_hk", "文件标签（多个标签用逗号分隔）"),

            // entity.file.ipaddress
            new TranslationSeedItem("entity.file.ipaddress", "en-US", "IP地址_us", "IP 地址（上传或访问来源）"),
            // entity.file.ipaddress
            new TranslationSeedItem("entity.file.ipaddress", "ja-JP", "IP地址_jp", "IP 地址（上传或访问来源）"),
            // entity.file.ipaddress
            new TranslationSeedItem("entity.file.ipaddress", "zh-CN", "IP地址", "IP 地址（上传或访问来源）"),
            // entity.file.ipaddress
            new TranslationSeedItem("entity.file.ipaddress", "zh-HK", "IP地址_hk", "IP 地址（上传或访问来源）"),

            // entity.file.location
            new TranslationSeedItem("entity.file.location", "en-US", "位置_us", "位置（IP 对应地理位置）"),
            // entity.file.location
            new TranslationSeedItem("entity.file.location", "ja-JP", "位置_jp", "位置（IP 对应地理位置）"),
            // entity.file.location
            new TranslationSeedItem("entity.file.location", "zh-CN", "位置", "位置（IP 对应地理位置）"),
            // entity.file.location
            new TranslationSeedItem("entity.file.location", "zh-HK", "位置_hk", "位置（IP 对应地理位置）"),

            // entity.file.status
            new TranslationSeedItem("entity.file.status", "en-US", "状态_us", "状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.file.status
            new TranslationSeedItem("entity.file.status", "ja-JP", "状态_jp", "状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.file.status
            new TranslationSeedItem("entity.file.status", "zh-CN", "状态", "状态（字典 sys_normal_disable；1=启用，0=禁用）"),
            // entity.file.status
            new TranslationSeedItem("entity.file.status", "zh-HK", "状态_hk", "状态（字典 sys_normal_disable；1=启用，0=禁用）"),
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
    /// 翻译种子项（对应 TaktTranslation 全部可写字段，CultureId 由 SeedAsync 解析）
    /// </summary>
    private sealed record TranslationSeedItem(
        string I18nKey,
        string CultureCode,
        string TranslationText,
        string? ContextNote);
}
