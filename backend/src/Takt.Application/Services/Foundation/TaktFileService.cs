// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：TaktFileService.cs
// 创建时间：2026-06-13
// 创建人：Takt365(Cursor AI)
// 功能描述：文件应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.IO;
using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Foundation;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 文件应用服务
/// </summary>
public class TaktFileService : TaktServiceBase, ITaktFileService
{
    private readonly ITaktCompanyRepository<TaktFile> _fileRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;
    private readonly ITaktFileUploadEngine _fileUploadEngine;
    private readonly ITaktNumberingGenerator _numberingGenerator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="fileRepository">文件仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="fileUploadEngine">文件上传引擎</param>
    /// <param name="numberingGenerator">编码生成器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktFileService(
        ITaktCompanyRepository<TaktFile> fileRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktFileUploadEngine fileUploadEngine,
        ITaktNumberingGenerator numberingGenerator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _fileRepository = fileRepository;
        _uniqueValidator = uniqueValidator;
        _fileUploadEngine = fileUploadEngine;
        _numberingGenerator = numberingGenerator;
    }

    /// <summary>
    /// 获取文件列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktFileDto>> GetFileListAsync(TaktFileQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _fileRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktFileDto>.Create(
            data.Adapt<List<TaktFileDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取文件
    /// </summary>
    /// <param name="id">文件ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktFileDto?> GetFileByIdAsync(long id)
    {
        var entity = await _fileRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktFileDto>();
    }

    /// <summary>
    /// 按文件ID下载物理文件流
    /// </summary>
    /// <param name="id">文件ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>可读流与下载文件名</returns>
    public async Task<TaktFileDownloadStreamResult> DownloadFileByIdAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        EnsureThreeLayerContext();
        var entity = await _fileRepository.GetByIdAsync(id);
        if (entity == null
            || entity.TenantCode != CurrentTenantCode
            || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("文件不存在");
        }

        EnsureFileDownloadAllowed(entity);

        var downloadName = !string.IsNullOrWhiteSpace(entity.FileOriginalName)
            ? entity.FileOriginalName
            : entity.FileName;
        var descriptor = new TaktFileStorageDescriptor
        {
            FilePath = entity.FilePath,
            StorageType = entity.StorageType,
            StorageConfig = entity.StorageConfig,
        };
        var result = await _fileUploadEngine.OpenReadAsync(
            descriptor,
            downloadName,
            entity.FileType,
            cancellationToken);

        entity.DownloadCount = checked(entity.DownloadCount + 1);
        entity.LastDownloadTime = DateTime.Now;
        await _fileRepository.UpdateAsync(entity);

        return result;
    }

    /// <summary>
    /// 按访问地址打开物理文件流
    /// </summary>
    /// <param name="accessUrl">TaktFile.AccessUrl</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>可读流与下载文件名</returns>
    public async Task<TaktFileDownloadStreamResult> DownloadFileByAccessUrlAsync(
        string accessUrl,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accessUrl);
        EnsureThreeLayerContext();
        var normalized = accessUrl.Trim();
        var files = await _fileRepository.GetListAsync(x =>
            x.AccessUrl == normalized
            && x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode);
        var entity = files.OrderByDescending(x => x.Id).FirstOrDefault();
        if (entity == null)
        {
            throw new TaktBusinessException("文件不存在");
        }
        return await DownloadFileByIdAsync(entity.Id, cancellationToken);
    }

    /// <summary>
    /// 校验当前用户是否允许下载该文件
    /// </summary>
    /// <param name="entity">文件实体</param>
    private void EnsureFileDownloadAllowed(TaktFile entity)
    {
        if (!TaktFileHelper.IsFileStatusEnabled(entity.FileStatus))
        {
            ThrowBusinessExceptionLocalized(TaktValidationI18nKeys.FileDownloadDisabled);
        }

        var currentUserId = CurrentUserId ?? 0;
        if (entity.IsPublic == 1 && entity.CreatedBy != currentUserId)
        {
            ThrowBusinessExceptionLocalized(TaktValidationI18nKeys.FileAccessDenied);
        }
    }

    /// <summary>
    /// 校验当前用户是否允许删除该文件（私有文件仅创建人可删）
    /// </summary>
    /// <param name="entity">文件实体</param>
    private void EnsureFileDeleteAllowed(TaktFile entity)
    {
        var currentUserId = CurrentUserId ?? 0;
        if (entity.IsPublic == 1 && entity.CreatedBy != currentUserId)
        {
            ThrowBusinessExceptionLocalized(TaktValidationI18nKeys.FileAccessDenied);
        }
    }

    /// <summary>
    /// 将物理文件重命名为带删除标记的文件名（xxx.ext → xxx.del.ext，经上传引擎调用 TaktFileHelper）
    /// </summary>
    /// <param name="entity">文件实体</param>
    private async Task MarkFilePhysicalDeletedAsync(TaktFile entity)
    {
        if (string.IsNullOrWhiteSpace(entity.FilePath))
        {
            return;
        }

        var fileName = Path.GetFileName(entity.FilePath.Replace('\\', '/'));
        if (TaktFileHelper.IsDeletedPhysicalFileName(fileName))
        {
            return;
        }

        var descriptor = new TaktFileStorageDescriptor
        {
            FilePath = entity.FilePath,
            StorageType = entity.StorageType,
            StorageConfig = entity.StorageConfig,
        };
        var deletedRelativePath = await _fileUploadEngine.MarkStoredFileDeletedAsync(descriptor);
        if (string.Equals(deletedRelativePath, entity.FilePath, StringComparison.Ordinal))
        {
            return;
        }

        entity.FilePath = deletedRelativePath;
        entity.FileName = Path.GetFileName(deletedRelativePath.Replace('\\', '/'));
        await _fileRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 获取文件选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetFileOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var currentUserId = CurrentUserId ?? 0;
        var list = await _fileRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.FileStatus == TaktFileHelper.FileStatusEnabled
                && (x.IsPublic == 0 || x.CreatedBy == currentUserId),
            x => x.FileName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.FileName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建文件
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFileDto> CreateFileAsync(TaktFileCreateDto dto)
    {
        var entity = dto.Adapt<TaktFile>();
        var isUnique_ix_file_code_unique = await _uniqueValidator.IsUniqueAsync(
            _fileRepository,
            x => x.FileCode == entity.FileCode);
        if (!isUnique_ix_file_code_unique)
        {
            throw new TaktBusinessException("文件的FileCode已存在");
        }
        entity = await _fileRepository.CreateAsync(entity);
        return await GetFileByIdAsync(entity.Id) ?? entity.Adapt<TaktFileDto>();
    }

    /// <summary>
    /// 更新文件
    /// </summary>
    /// <param name="id">文件ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFileDto> UpdateFileAsync(long id, TaktFileUpdateDto dto)
    {
        var entity = await _fileRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("文件不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_file_code_unique = await _uniqueValidator.IsUniqueAsync(
            _fileRepository,
            x => x.FileCode == entity.FileCode,
            id);
        if (!isUnique_ix_file_code_unique)
        {
            throw new TaktBusinessException("文件的FileCode已存在");
        }
        await _fileRepository.UpdateAsync(entity);
        return await GetFileByIdAsync(id) ?? throw new TaktBusinessException("文件不存在");
    }

    /// <summary>
    /// 删除文件（软删数据表；物理文件经 TaktFileHelper 重命名为 xxx.del.ext）
    /// </summary>
    /// <param name="id">文件ID</param>
    /// <returns>任务</returns>
    public async Task DeleteFileByIdAsync(long id)
    {
        EnsureThreeLayerContext();
        var entity = await _fileRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("文件不存在或已删除");
        }

        await DeleteFileEntityAsync(entity);
    }

    /// <summary>
    /// 批量删除文件（每条均软删数据表，并将物理文件重命名为 xxx.del.ext）
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteFileBatchAsync(IEnumerable<long> ids)
    {
        EnsureThreeLayerContext();
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }

        foreach (var id in idList)
        {
            var entity = await _fileRepository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new TaktBusinessException("文件不存在或已删除");
            }

            await DeleteFileEntityAsync(entity);
        }
    }

    /// <summary>
    /// 删除单条文件实体：权限校验、物理文件标记删除、数据表软删
    /// </summary>
    /// <param name="entity">已加载的文件实体</param>
    private async Task DeleteFileEntityAsync(TaktFile entity)
    {
        if (entity.TenantCode != CurrentTenantCode
            || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("文件不存在或已删除");
        }

        EnsureFileDeleteAllowed(entity);
        await MarkFilePhysicalDeletedAsync(entity);
        var deleted = await _fileRepository.DeleteAsync(entity.Id);
        if (!deleted)
        {
            throw new TaktBusinessException("文件不存在或已删除");
        }
    }

    /// <summary>
    /// 更新文件状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFileDto> UpdateFileStatusAsync(TaktFileStatusDto dto)
    {
        var entity = await _fileRepository.GetByIdAsync(dto.FileId);
        if (entity == null)
        {
            throw new TaktBusinessException("文件不存在");
        }
        entity.FileStatus = dto.FileStatus;
        await _fileRepository.UpdateAsync(entity);
        return await GetFileByIdAsync(dto.FileId) ?? throw new TaktBusinessException("文件不存在");
    }

    /// <summary>
    /// 更新文件公开
    /// </summary>
    /// <param name="dto">公开范围 DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFileDto> UpdateFilePublicAsync(TaktFilePublicDto dto)
    {
        var entity = await _fileRepository.GetByIdAsync(dto.FileId);
        if (entity == null)
        {
            throw new TaktBusinessException("文件不存在");
        }
        if (dto.IsPublic is not 0 and not 1)
        {
            throw new TaktBusinessException("公开必须为字典 sys_public_type 合法值（0=公开，1=私有）");
        }
        entity.IsPublic = dto.IsPublic;
        await _fileRepository.UpdateAsync(entity);
        return await GetFileByIdAsync(dto.FileId) ?? throw new TaktBusinessException("文件不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetFileTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktFileTemplateDto>(
            sheetName ?? "文件导入模板",
            fileName ?? "文件导入模板.xlsx");
    }

    /// <summary>
    /// 导入文件
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportFileAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktFileImportDto>(fileStream, sheetName ?? "文件导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktFile>();
                var importKey = $"{entity.FileCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（FileCode）");
                }
                var isUnique_ix_file_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _fileRepository,
                    x => x.FileCode == entity.FileCode);
                if (!isUnique_ix_file_code_unique)
                {
                    throw new TaktBusinessException("文件的FileCode已存在");
                }
                await _fileRepository.CreateAsync(entity);
                success += 1;
            }
            catch (Exception ex)
            {
                fail += 1;
                errors.Add($"第{i + 2}行: {ex.Message}");
            }
        }
        return (success, fail, errors);
    }

    /// <summary>
    /// 导出文件
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportFileAsync(TaktFileQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktFileQueryDto());
        var list = await _fileRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktFileExportDto>(),
                sheetName ?? "文件数据",
                fileName ?? "文件导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktFileExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "文件数据",
            fileName ?? "文件导出.xlsx");
    }

    // ========================================
    // 文件上传（整文件 / 分片）
    // ========================================

    /// <summary>
    /// 获取上传策略（可选 totalSizeBytes 计算分片计划）
    /// </summary>
    /// <param name="totalSizeBytes">文件总大小（字节）</param>
    /// <returns>上传策略</returns>
    public TaktFileUploadPolicyResult GetFileUploadPolicy(long? totalSizeBytes = null)
    {
        EnsureThreeLayerContext();
        return _fileUploadEngine.GetUploadPolicy(totalSizeBytes);
    }

    /// <summary>
    /// 整文件上传并落库
    /// </summary>
    /// <param name="fileStream">文件流</param>
    /// <param name="fileName">原始文件名</param>
    /// <param name="contentType">MIME 类型</param>
    /// <param name="meta">业务元数据</param>
    /// <param name="clientIp">上传来源客户端 IP（由 WebApi 经 TaktLocationHelper.ResolveClientIp 解析）</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>上传结果</returns>
    public async Task<TaktFileUploadResultDto> UploadFileAsync(
        Stream fileStream,
        string fileName,
        string? contentType,
        TaktFileUploadMetaDto? meta = null,
        string? clientIp = null,
        CancellationToken cancellationToken = default)
    {
        EnsureThreeLayerContext();
        ArgumentNullException.ThrowIfNull(fileStream);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        await EnsureUploadOriginalNameUniqueTodayAsync(fileName);
        var scope = BuildUploadScope(meta);
        var stored = await _fileUploadEngine.UploadFileAsync(
            fileStream,
            fileName,
            contentType,
            scope,
            cancellationToken);
        var entity = await PersistFileEntityAsync(stored, meta, clientIp);
        return MapUploadResult(entity);
    }

    /// <summary>
    /// 检查分片是否已上传
    /// </summary>
    /// <param name="request">检查参数</param>
    /// <returns>是否存在</returns>
    public async Task<TaktFileChunkCheckResult> CheckFileChunkAsync(TaktFileChunkCheckRequest request)
    {
        EnsureThreeLayerContext();
        ArgumentNullException.ThrowIfNull(request);
        return await _fileUploadEngine.CheckChunkAsync(request, BuildBaseUploadScope());
    }

    /// <summary>
    /// 列出已上传分片序号
    /// </summary>
    /// <param name="request">查询参数</param>
    /// <returns>已上传分片序号</returns>
    public async Task<TaktFileChunkListResult> ListFileChunksAsync(TaktFileChunkListRequest request)
    {
        EnsureThreeLayerContext();
        ArgumentNullException.ThrowIfNull(request);
        return await _fileUploadEngine.ListUploadedChunksAsync(request, BuildBaseUploadScope());
    }

    /// <summary>
    /// 上传单个分片
    /// </summary>
    /// <param name="chunkStream">分片流</param>
    /// <param name="request">分片元数据</param>
    /// <param name="cancellationToken">取消令牌</param>
    public async Task UploadFileChunkAsync(
        Stream chunkStream,
        TaktFileChunkUploadRequest request,
        CancellationToken cancellationToken = default)
    {
        EnsureThreeLayerContext();
        ArgumentNullException.ThrowIfNull(chunkStream);
        ArgumentNullException.ThrowIfNull(request);
        await _fileUploadEngine.UploadChunkAsync(
            chunkStream,
            request,
            BuildBaseUploadScope(),
            cancellationToken);
    }

    /// <summary>
    /// 合并分片并落库
    /// </summary>
    /// <param name="dto">合并参数</param>
    /// <param name="clientIp">上传来源客户端 IP（由 WebApi 经 TaktLocationHelper.ResolveClientIp 解析）</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>上传结果</returns>
    public async Task<TaktFileUploadResultDto> MergeFileChunksAsync(
        TaktFileChunkMergeDto dto,
        string? clientIp = null,
        CancellationToken cancellationToken = default)
    {
        EnsureThreeLayerContext();
        ArgumentNullException.ThrowIfNull(dto);
        ArgumentException.ThrowIfNullOrWhiteSpace(dto.FileName);
        await EnsureUploadOriginalNameUniqueTodayAsync(dto.FileName);
        var meta = MapMergeMeta(dto);
        var scope = BuildUploadScope(meta);
        var mergeRequest = new TaktFileChunkMergeRequest
        {
            Identifier = dto.Identifier,
            FileName = dto.FileName,
            TotalChunks = dto.TotalChunks,
            TotalSize = dto.TotalSize,
        };
        var stored = await _fileUploadEngine.MergeChunksAsync(
            mergeRequest,
            scope,
            cancellationToken);
        var entity = await PersistFileEntityAsync(stored, meta, clientIp);
        return MapUploadResult(entity);
    }

    /// <summary>
    /// 取消分片上传并清理临时分片
    /// </summary>
    /// <param name="identifier">上传会话标识</param>
    public async Task CancelFileChunksAsync(string identifier)
    {
        EnsureThreeLayerContext();
        ArgumentException.ThrowIfNullOrWhiteSpace(identifier);
        await _fileUploadEngine.CancelUploadedChunksAsync(identifier, BuildBaseUploadScope());
    }

    /// <summary>
    /// 构建租户+公司隔离范围（分片临时目录）
    /// </summary>
    /// <returns>上传范围</returns>
    private TaktFileUploadScope BuildBaseUploadScope()
    {
        EnsureThreeLayerContext();
        return new TaktFileUploadScope
        {
            TenantCode = CurrentTenantCode!,
            CompanyCode = CurrentCompanyCode!,
        };
    }

    /// <summary>
    /// 由业务元数据构建上传范围
    /// </summary>
    /// <param name="meta">业务元数据</param>
    /// <returns>上传范围</returns>
    private TaktFileUploadScope BuildUploadScope(TaktFileUploadMetaDto? meta)
    {
        var scope = BuildBaseUploadScope();
        if (meta == null)
        {
            return scope;
        }

        scope.CategoryPath = meta.CategoryPath;
        scope.TargetFileName = meta.TargetFileName;
        scope.StorageNaming = TaktFileHelper.NormalizeStorageNamingValue(meta.StorageNaming, 0);
        scope.StorageType = meta.StorageType ?? 0;
        scope.StorageConfig = meta.StorageConfig;
        if (meta.FileUploadType.HasValue
            && Enum.IsDefined(typeof(TaktFileUploadType), meta.FileUploadType.Value))
        {
            scope.FileUploadType = (TaktFileUploadType)meta.FileUploadType.Value;
        }

        return scope;
    }

    /// <summary>
    /// 分片合并 DTO 转业务元数据
    /// </summary>
    /// <param name="dto">合并 DTO</param>
    /// <returns>业务元数据</returns>
    private static TaktFileUploadMetaDto MapMergeMeta(TaktFileChunkMergeDto dto)
    {
        return new TaktFileUploadMetaDto
        {
            FileDescription = dto.FileDescription,
            FileTags = dto.FileTags,
            IsPublic = dto.IsPublic,
            FileStatus = dto.FileStatus,
            FileUploadType = dto.FileUploadType,
            TargetFileName = dto.TargetFileName,
            CategoryPath = dto.CategoryPath,
            StorageType = dto.StorageType,
            StorageNaming = dto.StorageNaming,
            StorageConfig = dto.StorageConfig,
        };
    }

    /// <summary>
    /// 校验当日同租户+公司下原始文件名是否已存在（与上传引擎 Path.GetFileName 归一化一致）
    /// </summary>
    /// <param name="originalFileName">上传原始文件名</param>
    private async Task EnsureUploadOriginalNameUniqueTodayAsync(string originalFileName)
    {
        var safeOriginalName = Path.GetFileName(originalFileName.Trim());
        if (string.IsNullOrWhiteSpace(safeOriginalName))
        {
            return;
        }

        var dayStart = DateTime.Today;
        var dayEnd = dayStart.AddDays(1);
        var duplicated = await _fileRepository.ExistsAsync(x =>
            x.FileOriginalName == safeOriginalName
            && x.CreatedAt >= dayStart
            && x.CreatedAt < dayEnd);
        if (duplicated)
        {
            ThrowLocalizedException(
                TaktValidationI18nKeys.FileUploadDuplicateOriginalNameToday,
                extraTokens: new Dictionary<string, string>(StringComparer.Ordinal)
                {
                    ["fileName"] = safeOriginalName,
                });
        }
    }

    /// <summary>
    /// 存储结果落库为 TaktFile 实体
    /// </summary>
    /// <param name="stored">引擎存储结果</param>
    /// <param name="meta">业务元数据</param>
    /// <param name="clientIp">上传来源客户端 IP</param>
    /// <returns>已持久化实体</returns>
    private async Task<TaktFile> PersistFileEntityAsync(
        TaktStoredFileResult stored,
        TaktFileUploadMetaDto? meta,
        string? clientIp = null)
    {
        var (ipAddress, location) = ResolveUploadIpAndLocation(clientIp);
        var entity = new TaktFile
        {
            FileCode = stored.FileCode,
            FileName = stored.FileName,
            FileOriginalName = stored.FileOriginalName,
            FilePath = stored.FilePath,
            FileSize = stored.FileSize,
            FileType = stored.FileType ?? string.Empty,
            FileExtension = stored.FileExtension ?? string.Empty,
            FileHash = stored.FileHash ?? string.Empty,
            FileCategory = stored.FileCategory,
            StorageType = stored.StorageType,
            StorageConfig = stored.StorageConfig,
            AccessUrl = stored.AccessUrl ?? string.Empty,
            FileDescription = ResolveUploadFileDescription(stored.FileCode, stored.FileName, meta),
            FileTags = meta?.FileTags ?? string.Empty,
            IsPublic = meta?.IsPublic ?? 0,
            FileStatus = TaktFileHelper.NormalizeFileStatusOrDefault(meta?.FileStatus),
            IpAddress = ipAddress,
            Location = location,
        };
        var isUnique_ix_file_code_unique = await _uniqueValidator.IsUniqueAsync(
            _fileRepository,
            x => x.FileCode == entity.FileCode);
        if (!isUnique_ix_file_code_unique)
        {
            var ruleCode = ResolveFileNumberingRuleCode(entity.FileType);
            var generated = await _numberingGenerator.GenerateNextAsync(ruleCode);
            if (string.IsNullOrWhiteSpace(generated.BusinessCode))
            {
                throw new TaktBusinessException("文件编码生成失败");
            }

            entity.FileCode = generated.BusinessCode;
            if (string.IsNullOrWhiteSpace(meta?.FileDescription))
            {
                entity.FileDescription = TaktFileHelper.BuildFileCodeNameDescription(entity.FileCode, entity.FileName);
            }
        }
        return await _fileRepository.CreateAsync(entity);
    }

    /// <summary>
    /// MIME → 文件编码规则码（与 TaktNumberingSeedData FD-F*、上传引擎一致）
    /// </summary>
    /// <param name="fileMimeType">MIME 类型</param>
    /// <returns>规则码</returns>
    private static string ResolveFileNumberingRuleCode(string fileMimeType)
    {
        return TaktFileHelper.GetFileCategoryFromMimeType(fileMimeType) switch
        {
            TaktFileHelper.FileCategory.Document => "FD-FDOC",
            TaktFileHelper.FileCategory.Image => "FD-FIMG",
            TaktFileHelper.FileCategory.Video => "FD-FVID",
            TaktFileHelper.FileCategory.Audio => "FD-FAUD",
            TaktFileHelper.FileCategory.Archive => "FD-FARC",
            _ => "FD-FOTH",
        };
    }

    /// <summary>
    /// 解析上传落库的 IP 与地理位置（经 TaktHttpAuditHelper.ResolveLocationFromIp）
    /// </summary>
    /// <param name="clientIp">客户端 IP</param>
    /// <returns>可落库的 IP 与位置</returns>
    private static (string IpAddress, string Location) ResolveUploadIpAndLocation(string? clientIp)
    {
        var location = TaktHttpAuditHelper.ResolveLocationFromIp(clientIp, null);
        return (clientIp ?? string.Empty, location);
    }

    /// <summary>
    /// 解析上传文件描述（未传入时默认由文件编码与存储文件名组成）
    /// </summary>
    /// <param name="fileCode">文件业务编码</param>
    /// <param name="fileName">存储文件名</param>
    /// <param name="meta">业务元数据</param>
    /// <returns>文件描述</returns>
    private static string ResolveUploadFileDescription(
        string fileCode,
        string fileName,
        TaktFileUploadMetaDto? meta)
    {
        if (!string.IsNullOrWhiteSpace(meta?.FileDescription))
        {
            return meta.FileDescription.Trim();
        }

        return TaktFileHelper.BuildFileCodeNameDescription(fileCode, fileName);
    }

    /// <summary>
    /// 实体映射为上传结果 DTO
    /// </summary>
    /// <param name="entity">文件实体</param>
    /// <returns>上传结果</returns>
    private static TaktFileUploadResultDto MapUploadResult(TaktFile entity)
    {
        return new TaktFileUploadResultDto
        {
            FileId = entity.Id,
            FileCode = entity.FileCode,
            FileName = entity.FileName,
            FileOriginalName = entity.FileOriginalName,
            FilePath = entity.FilePath,
            FileSize = entity.FileSize,
            FileType = entity.FileType,
            FileExtension = entity.FileExtension,
            FileHash = entity.FileHash,
            FileCategory = entity.FileCategory,
            StorageType = entity.StorageType,
            StorageConfig = entity.StorageConfig,
            AccessUrl = entity.AccessUrl,
        };
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建文件查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktFile, bool>> QueryExpression(TaktFileQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktFile>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.FileCode != null && x.FileCode.Contains(keywords))
                || (x.FileName != null && x.FileName.Contains(keywords))
                || (x.FileOriginalName != null && x.FileOriginalName.Contains(keywords))
                || (x.FilePath != null && x.FilePath.Contains(keywords))
                || SqlFunc.ToString(x.FileSize).Contains(keywords)
                || (x.FileType != null && x.FileType.Contains(keywords))
                || (x.FileExtension != null && x.FileExtension.Contains(keywords))
                || (x.FileHash != null && x.FileHash.Contains(keywords))
                || SqlFunc.ToString(x.FileCategory).Contains(keywords)
                || SqlFunc.ToString(x.StorageType).Contains(keywords)
                || (x.StorageConfig != null && x.StorageConfig.Contains(keywords))
                || (x.AccessUrl != null && x.AccessUrl.Contains(keywords))
                || SqlFunc.ToString(x.DownloadCount).Contains(keywords)
                || SqlFunc.ToString(x.FileStatus).Contains(keywords)
                || SqlFunc.ToString(x.IsPublic).Contains(keywords)
                || (x.FileDescription != null && x.FileDescription.Contains(keywords))
                || (x.FileTags != null && x.FileTags.Contains(keywords))
                || (x.IpAddress != null && x.IpAddress.Contains(keywords))
                || (x.Location != null && x.Location.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.LastDownloadTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.FileCode))
        {
            exp = exp.And(x => x.FileCode != null && x.FileCode.Contains(queryDto.FileCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.FileName))
        {
            exp = exp.And(x => x.FileName != null && x.FileName.Contains(queryDto.FileName));
        }

        if (!string.IsNullOrEmpty(queryDto?.FileOriginalName))
        {
            exp = exp.And(x => x.FileOriginalName != null && x.FileOriginalName.Contains(queryDto.FileOriginalName));
        }

        if (!string.IsNullOrEmpty(queryDto?.FilePath))
        {
            exp = exp.And(x => x.FilePath != null && x.FilePath.Contains(queryDto.FilePath));
        }

        if (queryDto?.FileSize.HasValue == true)
        {
            exp = exp.And(x => x.FileSize == queryDto.FileSize);
        }

        if (!string.IsNullOrEmpty(queryDto?.FileType))
        {
            exp = exp.And(x => x.FileType != null && x.FileType.Contains(queryDto.FileType));
        }

        if (!string.IsNullOrEmpty(queryDto?.FileExtension))
        {
            exp = exp.And(x => x.FileExtension != null && x.FileExtension.Contains(queryDto.FileExtension));
        }

        if (!string.IsNullOrEmpty(queryDto?.FileHash))
        {
            exp = exp.And(x => x.FileHash != null && x.FileHash.Contains(queryDto.FileHash));
        }

        if (queryDto?.FileCategory.HasValue == true)
        {
            exp = exp.And(x => x.FileCategory == queryDto.FileCategory);
        }

        if (queryDto?.StorageType.HasValue == true)
        {
            exp = exp.And(x => x.StorageType == queryDto.StorageType);
        }

        if (!string.IsNullOrEmpty(queryDto?.StorageConfig))
        {
            exp = exp.And(x => x.StorageConfig != null && x.StorageConfig.Contains(queryDto.StorageConfig));
        }

        if (!string.IsNullOrEmpty(queryDto?.AccessUrl))
        {
            exp = exp.And(x => x.AccessUrl != null && x.AccessUrl.Contains(queryDto.AccessUrl));
        }

        if (queryDto?.DownloadCount.HasValue == true)
        {
            exp = exp.And(x => x.DownloadCount == queryDto.DownloadCount);
        }

        if (queryDto?.FileStatus.HasValue == true)
        {
            exp = exp.And(x => x.FileStatus == queryDto.FileStatus);
        }

        if (queryDto?.IsPublic.HasValue == true)
        {
            exp = exp.And(x => x.IsPublic == queryDto.IsPublic);
        }

        if (!string.IsNullOrEmpty(queryDto?.FileDescription))
        {
            exp = exp.And(x => x.FileDescription != null && x.FileDescription.Contains(queryDto.FileDescription));
        }

        if (!string.IsNullOrEmpty(queryDto?.FileTags))
        {
            exp = exp.And(x => x.FileTags != null && x.FileTags.Contains(queryDto.FileTags));
        }

        if (!string.IsNullOrEmpty(queryDto?.IpAddress))
        {
            exp = exp.And(x => x.IpAddress != null && x.IpAddress.Contains(queryDto.IpAddress));
        }

        if (!string.IsNullOrEmpty(queryDto?.Location))
        {
            exp = exp.And(x => x.Location != null && x.Location.Contains(queryDto.Location));
        }

        if (!string.IsNullOrEmpty(queryDto?.CultureCode))
        {
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(queryDto.CultureCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.LastDownloadTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.LastDownloadTime >= queryDto.LastDownloadTimeStart);
        }

        if (queryDto?.LastDownloadTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.LastDownloadTime <= queryDto.LastDownloadTimeEnd);
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt >= queryDto.CreatedAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt <= queryDto.CreatedAtEnd);
        }
        if (!string.IsNullOrWhiteSpace(queryDto?.PlantCode))
        {
            var plantCode = queryDto.PlantCode;
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(plantCode));
        }


        return exp.ToExpression();
    }
}