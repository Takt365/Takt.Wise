// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Foundation
// 文件名称：TaktFileDtos.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：File 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktFile 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Foundation;

// ========================================
// File 响应 DTO
// ========================================

/// <summary>
/// 文件实体 公司级实体：文件元数据按租户+公司隔离；字段与前端 entity.file.* 及业务附件 JSON 结构对齐
/// 对应前端 TaktFileDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktFileDto : TaktCompanyDtoBase
{
    /// <summary>
    /// FileID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileId { get; set; }

    /// <summary>
    /// 文件编码（唯一索引：租户+公司内唯一，见 ix_file_code_unique）
    /// </summary>
    public string FileCode { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（字典 sys_storage_naming；0=原文件+哈希值 1=自动生成 2=自定义）
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// 文件原始名称（上传时的原始文件名）
    /// </summary>
    public string FileOriginalName { get; set; } = string.Empty;

    /// <summary>
    /// 文件路径（关联一级菜单 uploadPath，选项 useMenuUploadPath）
    /// </summary>
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileSize { get; set; }

    /// <summary>
    /// 文件 MIME 类型
    /// </summary>
    public string FileType { get; set; } = string.Empty;

    /// <summary>
    /// 文件扩展名
    /// </summary>
    public string FileExtension { get; set; } = string.Empty;

    /// <summary>
    /// 文件哈希值（MD5 或 SHA256，用于去重与校验）
    /// </summary>
    public string FileHash { get; set; } = string.Empty;

    /// <summary>
    /// 文件分类（根据 FileType/MIME 自动推断：0=文档，1=图片，2=视频，3=音频，4=压缩包，5=其他）
    /// </summary>
    public int FileCategory { get; set; } = 0;

    /// <summary>
    /// 存储方式（字典 sys_storage_type；0=本地存储 1=OSS对象存储 2=FTP）
    /// </summary>
    public int StorageType { get; set; } = 0;

    /// <summary>
    /// 存储配置（JSON，OSS/FTP 等扩展配置）
    /// </summary>
    public string? StorageConfig { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件 URL）
    /// </summary>
    public string AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 下载次数
    /// </summary>
    public int DownloadCount { get; set; } = 0;

    /// <summary>
    /// 最后下载时间
    /// </summary>
    public DateTime? LastDownloadTime { get; set; }

    /// <summary>
    /// 公开（字典 sys_public_type；0=公开同公司可见，1=私有仅创建人可见/可改/可下载）
    /// </summary>
    public int IsPublic { get; set; } = 0;

    /// <summary>
    /// 文件描述
    /// </summary>
    public string FileDescription { get; set; } = string.Empty;

    /// <summary>
    /// 文件标签（多个标签用逗号分隔）
    /// </summary>
    public string FileTags { get; set; } = string.Empty;

    /// <summary>
    /// IP 地址（上传或访问来源）
    /// </summary>
    public string IpAddress { get; set; } = string.Empty;

    /// <summary>
    /// 位置（IP 对应地理位置）
    /// </summary>
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int FileStatus { get; set; } = 0;

}

// ========================================
// File 查询 DTO
// ========================================

/// <summary>
/// File 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktFileQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 文件编码（唯一索引：租户+公司内唯一，见 ix_file_code_unique）
    /// </summary>
    public string? FileCode { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（字典 sys_storage_naming；0=原文件+哈希值 1=自动生成 2=自定义）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 文件原始名称（上传时的原始文件名）
    /// </summary>
    public string? FileOriginalName { get; set; } = string.Empty;

    /// <summary>
    /// 文件路径（关联一级菜单 uploadPath，选项 useMenuUploadPath）
    /// </summary>
    public string? FilePath { get; set; } = string.Empty;

    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FileSize { get; set; }

    /// <summary>
    /// 文件 MIME 类型
    /// </summary>
    public string? FileType { get; set; } = string.Empty;

    /// <summary>
    /// 文件扩展名
    /// </summary>
    public string? FileExtension { get; set; } = string.Empty;

    /// <summary>
    /// 文件哈希值（MD5 或 SHA256，用于去重与校验）
    /// </summary>
    public string? FileHash { get; set; } = string.Empty;

    /// <summary>
    /// 文件分类（根据 FileType/MIME 自动推断：0=文档，1=图片，2=视频，3=音频，4=压缩包，5=其他）
    /// </summary>
    public int? FileCategory { get; set; }

    /// <summary>
    /// 存储方式（字典 sys_storage_type；0=本地存储 1=OSS对象存储 2=FTP）
    /// </summary>
    public int? StorageType { get; set; }

    /// <summary>
    /// 存储配置（JSON，OSS/FTP 等扩展配置）
    /// </summary>
    public string? StorageConfig { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件 URL）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 下载次数
    /// </summary>
    public int? DownloadCount { get; set; }

    /// <summary>
    /// 最后下载时间（范围查询-开始）
    /// </summary>
    public DateTime? LastDownloadTimeStart { get; set; }

    /// <summary>
    /// 最后下载时间（范围查询-结束）
    /// </summary>
    public DateTime? LastDownloadTimeEnd { get; set; }

    /// <summary>
    /// 公开（字典 sys_public_type；0=公开同公司可见，1=私有仅创建人可见/可改/可下载）
    /// </summary>
    public int? IsPublic { get; set; }

    /// <summary>
    /// 文件描述
    /// </summary>
    public string? FileDescription { get; set; } = string.Empty;

    /// <summary>
    /// 文件标签（多个标签用逗号分隔）
    /// </summary>
    public string? FileTags { get; set; } = string.Empty;

    /// <summary>
    /// IP 地址（上传或访问来源）
    /// </summary>
    public string? IpAddress { get; set; } = string.Empty;

    /// <summary>
    /// 位置（IP 对应地理位置）
    /// </summary>
    public string? Location { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? FileStatus { get; set; }

    /// <summary>
    /// 创建时间（范围查询-开始）
    /// </summary>
    public DateTime? CreatedAtStart { get; set; }

    /// <summary>
    /// 创建时间（范围查询-结束）
    /// </summary>
    public DateTime? CreatedAtEnd { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建File DTO
// ========================================

/// <summary>
/// 创建File DTO
/// </summary>
public class TaktFileCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;



    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 文件编码（唯一索引：租户+公司内唯一，见 ix_file_code_unique）
    /// </summary>
    [Required(ErrorMessage = "文件编码（唯一索引：租户+公司内唯一，见 ix_file_code_unique）不能为空")]
    public string FileCode { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（字典 sys_storage_naming；0=原文件+哈希值 1=自动生成 2=自定义）
    /// </summary>
    [Required(ErrorMessage = "文件名称（字典 sys_storage_naming；0=原文件+哈希值 1=自动生成 2=自定义）不能为空")]
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// 文件原始名称（上传时的原始文件名）
    /// </summary>
    [Required(ErrorMessage = "文件原始名称（上传时的原始文件名）不能为空")]
    public string FileOriginalName { get; set; } = string.Empty;

    /// <summary>
    /// 文件路径（关联一级菜单 uploadPath，选项 useMenuUploadPath）
    /// </summary>
    [Required(ErrorMessage = "文件路径（关联一级菜单 uploadPath，选项 useMenuUploadPath）不能为空")]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileSize { get; set; }

    /// <summary>
    /// 文件 MIME 类型
    /// </summary>
    [Required(ErrorMessage = "文件 MIME 类型不能为空")]
    public string FileType { get; set; } = string.Empty;

    /// <summary>
    /// 文件扩展名
    /// </summary>
    [Required(ErrorMessage = "文件扩展名不能为空")]
    public string FileExtension { get; set; } = string.Empty;

    /// <summary>
    /// 文件哈希值（MD5 或 SHA256，用于去重与校验）
    /// </summary>
    public string FileHash { get; set; } = string.Empty;

    /// <summary>
    /// 文件分类（根据 FileType/MIME 自动推断：0=文档，1=图片，2=视频，3=音频，4=压缩包，5=其他）
    /// </summary>
    public int FileCategory { get; set; } = 0;

    /// <summary>
    /// 存储方式（字典 sys_storage_type；0=本地存储 1=OSS对象存储 2=FTP）
    /// </summary>
    public int StorageType { get; set; } = 0;

    /// <summary>
    /// 存储配置（JSON，OSS/FTP 等扩展配置）
    /// </summary>
    public string? StorageConfig { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件 URL）
    /// </summary>
    [Required(ErrorMessage = "访问地址（文件 URL）不能为空")]
    public string AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 下载次数
    /// </summary>
    public int DownloadCount { get; set; } = 0;

    /// <summary>
    /// 最后下载时间
    /// </summary>
    public DateTime? LastDownloadTime { get; set; }

    /// <summary>
    /// 公开（字典 sys_public_type；0=公开同公司可见，1=私有仅创建人可见/可改/可下载）
    /// </summary>
    public int IsPublic { get; set; } = 0;

    /// <summary>
    /// 文件描述
    /// </summary>
    [Required(ErrorMessage = "文件描述不能为空")]
    public string FileDescription { get; set; } = string.Empty;

    /// <summary>
    /// 文件标签（多个标签用逗号分隔）
    /// </summary>
    [Required(ErrorMessage = "文件标签（多个标签用逗号分隔）不能为空")]
    public string FileTags { get; set; } = string.Empty;

    /// <summary>
    /// IP 地址（上传或访问来源）
    /// </summary>
    [Required(ErrorMessage = "IP 地址（上传或访问来源）不能为空")]
    public string IpAddress { get; set; } = string.Empty;

    /// <summary>
    /// 位置（IP 对应地理位置）
    /// </summary>
    [Required(ErrorMessage = "位置（IP 对应地理位置）不能为空")]
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int FileStatus { get; set; } = 0;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新File DTO
// ========================================

/// <summary>
/// 更新File DTO
/// 继承 TaktFileCreateDto，添加 FileId 字段
/// </summary>
public class TaktFileUpdateDto : TaktFileCreateDto
{
    /// <summary>
    /// FileID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileId { get; set; }

}

// ========================================
// File 状态 DTO
// ========================================

/// <summary>
/// File 状态更新 DTO
/// </summary>
public class TaktFileStatusDto
{
    /// <summary>
    /// FileID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [Required(ErrorMessage = "状态（字典 sys_normal_disable；1=启用，0=禁用）不能为空")]
    public int FileStatus { get; set; } = 0;
}

// ========================================
// File 公开范围 DTO
// ========================================

/// <summary>
/// File 是否公开更新 DTO
/// </summary>
public class TaktFilePublicDto
{
    /// <summary>
    /// FileID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileId { get; set; }

    /// <summary>
    /// 是否公开（字典 sys_public_type；0=公开，1=私有）
    /// </summary>
    [Required(ErrorMessage = "是否公开不能为空")]
    public int IsPublic { get; set; } = 0;
}

// ========================================
// 文件上传 DTO
// ========================================

/// <summary>
/// 整文件/分片合并上传附带业务元数据
/// </summary>
public class TaktFileUploadMetaDto
{
    /// <summary>
    /// 文件描述
    /// </summary>
    public string? FileDescription { get; set; }

    /// <summary>
    /// 文件标签（多个标签用逗号分隔）
    /// </summary>
    public string? FileTags { get; set; }

    /// <summary>
    /// 是否公开（字典 sys_public_type；0=公开，1=私有）
    /// </summary>
    public int IsPublic { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? FileStatus { get; set; }

    /// <summary>
    /// 上传业务类型（TaktFileUploadType 枚举值）
    /// </summary>
    public int? FileUploadType { get; set; }

    /// <summary>
    /// 目标存储文件名（可选）
    /// </summary>
    public string? TargetFileName { get; set; }

    /// <summary>
    /// 分类路径（相对目录）
    /// </summary>
    public string? CategoryPath { get; set; }

    /// <summary>
    /// 存储方式（字典 sys_storage_type；0=本地存储 1=OSS对象存储 2=FTP）
    /// </summary>
    public int? StorageType { get; set; }

    /// <summary>
    /// 存储命名策略（0=默认）
    /// </summary>
    public int? StorageNaming { get; set; }

    /// <summary>
    /// 存储配置（JSON）
    /// </summary>
    public string? StorageConfig { get; set; }
}

/// <summary>
/// 文件上传完成结果 DTO
/// </summary>
public class TaktFileUploadResultDto
{
    /// <summary>
    /// 文件 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileId { get; set; }

    /// <summary>
    /// 文件编码
    /// </summary>
    public string FileCode { get; set; } = string.Empty;

    /// <summary>
    /// 存储文件名
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// 原始文件名
    /// </summary>
    public string FileOriginalName { get; set; } = string.Empty;

    /// <summary>
    /// 文件路径（关联一级菜单 uploadPath，选项 useMenuUploadPath）
    /// </summary>
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileSize { get; set; }

    /// <summary>
    /// 文件 MIME 类型
    /// </summary>
    public string FileType { get; set; } = string.Empty;

    /// <summary>
    /// 文件扩展名
    /// </summary>
    public string FileExtension { get; set; } = string.Empty;

    /// <summary>
    /// 文件哈希值
    /// </summary>
    public string FileHash { get; set; } = string.Empty;

    /// <summary>
    /// 文件分类
    /// </summary>
    public int FileCategory { get; set; } = 0;

    /// <summary>
    /// 存储方式（字典 sys_storage_type；0=本地存储 1=OSS对象存储 2=FTP）
    /// </summary>
    public int StorageType { get; set; } = 0;

    /// <summary>
    /// 存储配置
    /// </summary>
    public string? StorageConfig { get; set; }

    /// <summary>
    /// 访问地址
    /// </summary>
    public string AccessUrl { get; set; } = string.Empty;
}

/// <summary>
/// 分片合并请求 DTO（含业务元数据）
/// </summary>
public class TaktFileChunkMergeDto
{
    /// <summary>
    /// 上传会话标识
    /// </summary>
    [Required(ErrorMessage = "上传会话标识不能为空")]
    public string Identifier { get; set; } = string.Empty;

    /// <summary>
    /// 原始文件名
    /// </summary>
    [Required(ErrorMessage = "文件名不能为空")]
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// 总分片数
    /// </summary>
    public int TotalChunks { get; set; }

    /// <summary>
    /// 文件总大小（字节）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TotalSize { get; set; }

    /// <summary>
    /// 文件描述
    /// </summary>
    public string? FileDescription { get; set; }

    /// <summary>
    /// 文件标签
    /// </summary>
    public string? FileTags { get; set; }

    /// <summary>
    /// 是否公开
    /// </summary>
    public int IsPublic { get; set; } = 0;

    /// <summary>
    /// 状态
    /// </summary>
    public int? FileStatus { get; set; }

    /// <summary>
    /// 上传业务类型
    /// </summary>
    public int? FileUploadType { get; set; }

    /// <summary>
    /// 目标存储文件名
    /// </summary>
    public string? TargetFileName { get; set; }

    /// <summary>
    /// 分类路径
    /// </summary>
    public string? CategoryPath { get; set; }

    /// <summary>
    /// 存储方式（字典 sys_storage_type；0=本地存储 1=OSS对象存储 2=FTP）
    /// </summary>
    public int? StorageType { get; set; }

    /// <summary>
    /// 存储命名策略
    /// </summary>
    public int? StorageNaming { get; set; }

    /// <summary>
    /// 存储配置
    /// </summary>
    public string? StorageConfig { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// File 导入模板行 DTO
/// </summary>
public class TaktFileTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 文件编码（唯一索引：租户+公司内唯一，见 ix_file_code_unique）
    /// </summary>
    public string? FileCode { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（字典 sys_storage_naming；0=原文件+哈希值 1=自动生成 2=自定义）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 文件原始名称（上传时的原始文件名）
    /// </summary>
    public string? FileOriginalName { get; set; } = string.Empty;

    /// <summary>
    /// 文件路径（关联一级菜单 uploadPath，选项 useMenuUploadPath）
    /// </summary>
    public string? FilePath { get; set; } = string.Empty;

    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FileSize { get; set; }

    /// <summary>
    /// 文件 MIME 类型
    /// </summary>
    public string? FileType { get; set; } = string.Empty;

    /// <summary>
    /// 文件扩展名
    /// </summary>
    public string? FileExtension { get; set; } = string.Empty;

    /// <summary>
    /// 文件哈希值（MD5 或 SHA256，用于去重与校验）
    /// </summary>
    public string? FileHash { get; set; } = string.Empty;

    /// <summary>
    /// 文件分类（根据 FileType/MIME 自动推断：0=文档，1=图片，2=视频，3=音频，4=压缩包，5=其他）
    /// </summary>
    public int? FileCategory { get; set; }

    /// <summary>
    /// 存储方式（字典 sys_storage_type；0=本地存储 1=OSS对象存储 2=FTP）
    /// </summary>
    public int? StorageType { get; set; }

    /// <summary>
    /// 存储配置（JSON，OSS/FTP 等扩展配置）
    /// </summary>
    public string? StorageConfig { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件 URL）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 下载次数
    /// </summary>
    public int? DownloadCount { get; set; }

    /// <summary>
    /// 最后下载时间
    /// </summary>
    public DateTime? LastDownloadTime { get; set; }

    /// <summary>
    /// 公开（字典 sys_public_type；0=公开同公司可见，1=私有仅创建人可见/可改/可下载）
    /// </summary>
    public int? IsPublic { get; set; }

    /// <summary>
    /// 文件描述
    /// </summary>
    public string? FileDescription { get; set; } = string.Empty;

    /// <summary>
    /// 文件标签（多个标签用逗号分隔）
    /// </summary>
    public string? FileTags { get; set; } = string.Empty;

    /// <summary>
    /// IP 地址（上传或访问来源）
    /// </summary>
    public string? IpAddress { get; set; } = string.Empty;

    /// <summary>
    /// 位置（IP 对应地理位置）
    /// </summary>
    public string? Location { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? FileStatus { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// File 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktFileImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;



    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 文件编码（唯一索引：租户+公司内唯一，见 ix_file_code_unique）
    /// </summary>
    public string? FileCode { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（字典 sys_storage_naming；0=原文件+哈希值 1=自动生成 2=自定义）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 文件原始名称（上传时的原始文件名）
    /// </summary>
    public string? FileOriginalName { get; set; } = string.Empty;

    /// <summary>
    /// 文件路径（关联一级菜单 uploadPath，选项 useMenuUploadPath）
    /// </summary>
    public string? FilePath { get; set; } = string.Empty;

    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FileSize { get; set; }

    /// <summary>
    /// 文件 MIME 类型
    /// </summary>
    public string? FileType { get; set; } = string.Empty;

    /// <summary>
    /// 文件扩展名
    /// </summary>
    public string? FileExtension { get; set; } = string.Empty;

    /// <summary>
    /// 文件哈希值（MD5 或 SHA256，用于去重与校验）
    /// </summary>
    public string? FileHash { get; set; } = string.Empty;

    /// <summary>
    /// 文件分类（根据 FileType/MIME 自动推断：0=文档，1=图片，2=视频，3=音频，4=压缩包，5=其他）
    /// </summary>
    public int? FileCategory { get; set; }

    /// <summary>
    /// 存储方式（字典 sys_storage_type；0=本地存储 1=OSS对象存储 2=FTP）
    /// </summary>
    public int? StorageType { get; set; }

    /// <summary>
    /// 存储配置（JSON，OSS/FTP 等扩展配置）
    /// </summary>
    public string? StorageConfig { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件 URL）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 下载次数
    /// </summary>
    public int? DownloadCount { get; set; }

    /// <summary>
    /// 最后下载时间
    /// </summary>
    public DateTime? LastDownloadTime { get; set; }

    /// <summary>
    /// 公开（字典 sys_public_type；0=公开同公司可见，1=私有仅创建人可见/可改/可下载）
    /// </summary>
    public int? IsPublic { get; set; }

    /// <summary>
    /// 文件描述
    /// </summary>
    public string? FileDescription { get; set; } = string.Empty;

    /// <summary>
    /// 文件标签（多个标签用逗号分隔）
    /// </summary>
    public string? FileTags { get; set; } = string.Empty;

    /// <summary>
    /// IP 地址（上传或访问来源）
    /// </summary>
    public string? IpAddress { get; set; } = string.Empty;

    /// <summary>
    /// 位置（IP 对应地理位置）
    /// </summary>
    public string? Location { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? FileStatus { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// File 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktFileExportDto
{
    /// <summary>
    /// FileID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 文件编码（唯一索引：租户+公司内唯一，见 ix_file_code_unique）
    /// </summary>
    public string FileCode { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（字典 sys_storage_naming；0=原文件+哈希值 1=自动生成 2=自定义）
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// 文件原始名称（上传时的原始文件名）
    /// </summary>
    public string FileOriginalName { get; set; } = string.Empty;

    /// <summary>
    /// 文件路径（关联一级菜单 uploadPath，选项 useMenuUploadPath）
    /// </summary>
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileSize { get; set; }

    /// <summary>
    /// 文件 MIME 类型
    /// </summary>
    public string FileType { get; set; } = string.Empty;

    /// <summary>
    /// 文件扩展名
    /// </summary>
    public string FileExtension { get; set; } = string.Empty;

    /// <summary>
    /// 文件哈希值（MD5 或 SHA256，用于去重与校验）
    /// </summary>
    public string FileHash { get; set; } = string.Empty;

    /// <summary>
    /// 文件分类（根据 FileType/MIME 自动推断：0=文档，1=图片，2=视频，3=音频，4=压缩包，5=其他）
    /// </summary>
    public int FileCategory { get; set; } = 0;

    /// <summary>
    /// 存储方式（字典 sys_storage_type；0=本地存储 1=OSS对象存储 2=FTP）
    /// </summary>
    public int StorageType { get; set; } = 0;

    /// <summary>
    /// 存储配置（JSON，OSS/FTP 等扩展配置）
    /// </summary>
    public string? StorageConfig { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件 URL）
    /// </summary>
    public string AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 下载次数
    /// </summary>
    public int DownloadCount { get; set; } = 0;

    /// <summary>
    /// 最后下载时间
    /// </summary>
    public DateTime? LastDownloadTime { get; set; }

    /// <summary>
    /// 公开（字典 sys_public_type；0=公开同公司可见，1=私有仅创建人可见/可改/可下载）
    /// </summary>
    public int IsPublic { get; set; } = 0;

    /// <summary>
    /// 文件描述
    /// </summary>
    public string FileDescription { get; set; } = string.Empty;

    /// <summary>
    /// 文件标签（多个标签用逗号分隔）
    /// </summary>
    public string FileTags { get; set; } = string.Empty;

    /// <summary>
    /// IP 地址（上传或访问来源）
    /// </summary>
    public string IpAddress { get; set; } = string.Empty;

    /// <summary>
    /// 位置（IP 对应地理位置）
    /// </summary>
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int FileStatus { get; set; } = 0;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
