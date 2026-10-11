// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Foundation
// 文件名称：TaktFile.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：文件实体，定义上传文件的元数据与存储信息（与 entity.file.* / 请假证明附件 JSON 对齐）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Foundation;

/// <summary>
/// 文件实体
/// 公司级实体：文件元数据按租户+公司隔离；字段与前端 entity.file.* 及业务附件 JSON 结构对齐
/// </summary>
[SugarTable("takt_foundation_file", "文件表")]
[SugarIndex("ix_file_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_file_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_file_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(FileCode), OrderByType.Asc, true)]
[SugarIndex("ix_file_hash", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(FileHash), OrderByType.Asc, false)]
[SugarIndex("ix_file_category", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(FileCategory), OrderByType.Asc, false)]
[SugarIndex("ix_file_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(FileStatus), OrderByType.Asc, false)]
public class TaktFile : TaktCompanyEntityBase
{
    /// <summary>
    /// 文件编码（唯一索引：租户+公司内唯一，见 ix_file_code_unique；根据 MIME 类型自动通过 TaktNumbering 文件编码规则生成，非表单手选）
    /// </summary>
    [SugarColumn(ColumnName = "file_code", ColumnDescription = "文件编码", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
    public string FileCode { get; set; } = string.Empty;
    /// <summary>
    /// 文件名称（字典 sys_storage_naming；0=原文件+哈希值 1=自动生成 2=自定义）
    /// </summary>
    [SugarColumn(ColumnName = "file_name", ColumnDescription = "文件名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string FileName { get; set; } = string.Empty;
    /// <summary>
    /// 文件原始名称（上传时的原始文件名）
    /// </summary>
    [SugarColumn(ColumnName = "file_original_name", ColumnDescription = "原始名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string FileOriginalName { get; set; } = string.Empty;
    /// <summary>
    /// 文件路径（关联一级菜单 uploadPath，选项 useMenuUploadPath）
    /// </summary>
    [SugarColumn(ColumnName = "file_path", ColumnDescription = "文件路径", ColumnDataType = "nvarchar", Length = 500, IsNullable = false)]
    public string FilePath { get; set; } = string.Empty;
    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    [SugarColumn(ColumnName = "file_size", ColumnDescription = "文件大小", ColumnDataType = "bigint", IsNullable = false, DefaultValue = "0")]
    public long FileSize { get; set; } = 0;
    /// <summary>
    /// 文件 MIME 类型
    /// </summary>
    [SugarColumn(ColumnName = "file_type", ColumnDescription = "MIME类型", ColumnDataType = "nvarchar", Length = 100, IsNullable = false, DefaultValue = "''")]
    public string FileType { get; set; } = string.Empty;
    /// <summary>
    /// 文件扩展名
    /// </summary>
    [SugarColumn(ColumnName = "file_extension", ColumnDescription = "扩展名", ColumnDataType = "varchar", Length = 20, IsNullable = false, DefaultValue = "''")]
    public string FileExtension { get; set; } = string.Empty;
    /// <summary>
    /// 文件哈希值（MD5 或 SHA256，用于去重与校验）
    /// </summary>
    [SugarColumn(ColumnName = "file_hash", ColumnDescription = "哈希值", ColumnDataType = "varchar", Length = 64, IsNullable = false, DefaultValue = "''")]
    public string FileHash { get; set; } = string.Empty;
    /// <summary>
    /// 文件分类（根据 FileType/MIME 自动推断：0=文档，1=图片，2=视频，3=音频，4=压缩包，5=其他）
    /// </summary>
    [SugarColumn(ColumnName = "file_category", ColumnDescription = "文件分类", ColumnDataType = "int", IsNullable = false, DefaultValue = "5")]
    public int FileCategory { get; set; } = 5;
    /// <summary>
    /// 存储方式（字典 sys_storage_type；0=本地存储 1=OSS对象存储 2=FTP）
    /// </summary>
    [SugarColumn(ColumnName = "storage_type", ColumnDescription = "存储方式", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int StorageType { get; set; } = 0;
    /// <summary>
    /// 存储配置（JSON，OSS/FTP 等扩展配置）
    /// </summary>
    [SugarColumn(ColumnName = "storage_config", ColumnDescription = "存储配置", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
    public string? StorageConfig { get; set; }
    /// <summary>
    /// 访问地址（文件 URL）
    /// </summary>
    [SugarColumn(ColumnName = "access_url", ColumnDescription = "访问地址", ColumnDataType = "nvarchar", Length = 1000, IsNullable = false, DefaultValue = "''")]
    public string AccessUrl { get; set; } = string.Empty;
    /// <summary>
    /// 下载次数
    /// </summary>
    [SugarColumn(ColumnName = "download_count", ColumnDescription = "下载次数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int DownloadCount { get; set; } = 0;
    /// <summary>
    /// 最后下载时间
    /// </summary>
    [SugarColumn(ColumnName = "last_download_time", ColumnDescription = "最后下载", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? LastDownloadTime { get; set; }
    /// <summary>
    /// 公开（字典 sys_public_type；0=公开同公司可见，1=私有仅创建人可见/可改/可下载）
    /// </summary>
    [SugarColumn(ColumnName = "is_public", ColumnDescription = "公开", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsPublic { get; set; } = 0;
    /// <summary>
    /// 文件描述
    /// </summary>
    [SugarColumn(ColumnName = "file_description", ColumnDescription = "文件描述", ColumnDataType = "nvarchar", Length = 500, IsNullable = false, DefaultValue = "''")]
    public string FileDescription { get; set; } = string.Empty;
    /// <summary>
    /// 文件标签（多个标签用逗号分隔）
    /// </summary>
    [SugarColumn(ColumnName = "file_tags", ColumnDescription = "文件标签", ColumnDataType = "nvarchar", Length = 200, IsNullable = false, DefaultValue = "''")]
    public string FileTags { get; set; } = string.Empty;
    /// <summary>
    /// IP 地址（上传或访问来源）
    /// </summary>
    [SugarColumn(ColumnName = "ip_address", ColumnDescription = "IP地址", ColumnDataType = "varchar", Length = 50, IsNullable = false, DefaultValue = "''")]
    public string IpAddress { get; set; } = string.Empty;
    /// <summary>
    /// 位置（IP 对应地理位置）
    /// </summary>
    [SugarColumn(ColumnName = "location", ColumnDescription = "位置", ColumnDataType = "nvarchar", Length = 200, IsNullable = false, DefaultValue = "''")]
    public string Location { get; set; } = string.Empty;
    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "file_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int FileStatus { get; set; } = 1;
}
