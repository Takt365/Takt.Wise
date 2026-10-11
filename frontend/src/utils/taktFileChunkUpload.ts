// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：taktFileChunkUpload.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：Foundation 文件分片上传/断点续传（策略与分片计划由 api/TaktFiles/upload-policy 驱动）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import {
  cancelFileChunks,
  checkFileChunk,
  getFileUploadPolicy,
  listFileChunks,
  mergeFileChunks,
  uploadFile,
  uploadFileChunk,
} from '@/api/foundation/file.api';
import type {
  FileChunkMerge,
  FileUploadMeta,
  FileUploadPolicy,
  FileUploadResult,
} from '@/types/foundation/file.types';
import { generateFileIdentifier } from '@/utils/upload';

/** 默认最大并发分片数（前端传输辅助参数） */
export const TAKT_FILE_DEFAULT_CONCURRENCY = 3;

/** 默认分片重试次数（前端传输辅助参数） */
export const TAKT_FILE_DEFAULT_MAX_RETRIES = 3;

/**
 * 分片上传状态
 */
export enum TaktFileChunkUploadStatus {
  /** 等待中 */
  Waiting = 'waiting',
  /** 计算 MD5 */
  Hashing = 'hashing',
  /** 上传中 */
  Uploading = 'uploading',
  /** 合并中 */
  Merging = 'merging',
  /** 已暂停 */
  Paused = 'paused',
  /** 成功 */
  Success = 'success',
  /** 失败 */
  Error = 'error',
  /** 已取消 */
  Cancelled = 'cancelled',
}

/**
 * 分片上传进度
 */
export interface TaktFileChunkUploadProgress {
  /** 状态 */
  status: TaktFileChunkUploadStatus;
  /** 总进度 0-100 */
  percent: number;
  /** 已上传分片数 */
  uploadedChunks: number;
  /** 总分片数 */
  totalChunks: number;
  /** 文件 MD5 标识 */
  identifier: string;
}

/**
 * 分片上传选项
 */
export interface TaktFileChunkUploadOptions {
  /** 并发数 */
  concurrency?: number;
  /** 分片最大重试次数 */
  maxRetries?: number;
  /** 业务元数据（合并时写入） */
  meta?: FileUploadMeta;
  /** 后端返回的分片计划（未传则在 start 时拉取 upload-policy） */
  uploadPolicy?: FileUploadPolicy;
  /** 进度回调 */
  onProgress?: (progress: TaktFileChunkUploadProgress) => void;
}

interface InternalChunkState {
  index: number;
  blob: Blob;
  uploaded: boolean;
  uploading: boolean;
  retries: number;
  abortController?: AbortController;
}

/**
 * 是否应使用分片上传（以后端 upload-policy 为准）
 * @param fileSize 文件大小（字节）
 * @returns 是否分片
 */
export async function shouldUseTaktFileChunkUpload(fileSize: number): Promise<boolean> {
  if (!Number.isFinite(fileSize) || fileSize <= 0) {
    return false;
  }
  const policy = await getFileUploadPolicy(fileSize);
  return policy.useChunkUpload === true;
}

/** 分片上传暂停信号（非错误，用于 UI 保持弹窗） */
export class TaktFileChunkUploadPausedError extends Error {
  /**
   * @param message 说明
   */
  constructor(message = 'PAUSED') {
    super(message);
    this.name = 'TaktFileChunkUploadPausedError';
  }
}

/**
 * Foundation 文件分片上传器（支持断点续传、暂停/恢复、取消）
 */
export class TaktFileChunkUploader {
  private readonly file: globalThis.File;
  private readonly options: Required<Pick<TaktFileChunkUploadOptions, 'concurrency' | 'maxRetries'>> &
    Pick<TaktFileChunkUploadOptions, 'meta' | 'onProgress' | 'uploadPolicy'>;
  private chunks: InternalChunkState[] = [];
  private uploadPolicy: FileUploadPolicy | null = null;
  private identifier = '';
  private status: TaktFileChunkUploadStatus = TaktFileChunkUploadStatus.Waiting;
  private paused = false;
  private cancelled = false;
  private uploadPromise: Promise<FileUploadResult> | null = null;

  /**
   * @param file 待上传文件
   * @param options 上传选项
   */
  constructor(file: globalThis.File, options: TaktFileChunkUploadOptions = {}) {
    if (!file || file.size <= 0) {
      throw new Error('文件不能为空');
    }
    this.file = file;
    this.options = {
      concurrency: options.concurrency ?? TAKT_FILE_DEFAULT_CONCURRENCY,
      maxRetries: options.maxRetries ?? TAKT_FILE_DEFAULT_MAX_RETRIES,
      meta: options.meta,
      uploadPolicy: options.uploadPolicy,
      onProgress: options.onProgress,
    };
    this.uploadPolicy = options.uploadPolicy ?? null;
  }

  /**
   * 开始上传（可 await）
   * @returns 上传结果
   */
  start(): Promise<FileUploadResult> {
    if (this.uploadPromise) {
      return this.uploadPromise;
    }
    this.uploadPromise = this.runUpload();
    return this.uploadPromise;
  }

  /**
   * 暂停上传
   */
  pause(): void {
    if (this.status !== TaktFileChunkUploadStatus.Uploading) {
      return;
    }
    this.paused = true;
    this.chunks.forEach((chunk) => {
      chunk.abortController?.abort();
      chunk.uploading = false;
    });
    this.setStatus(TaktFileChunkUploadStatus.Paused);
  }

  /**
   * 恢复上传
   */
  resume(): Promise<FileUploadResult> {
    if (this.status !== TaktFileChunkUploadStatus.Paused && this.status !== TaktFileChunkUploadStatus.Error) {
      return this.start();
    }
    this.paused = false;
    this.cancelled = false;
    this.uploadPromise = this.runUpload();
    return this.uploadPromise;
  }

  /**
   * 取消上传并清理服务端临时分片
   */
  async cancel(): Promise<void> {
    this.cancelled = true;
    this.paused = false;
    this.chunks.forEach((chunk) => {
      chunk.abortController?.abort();
      chunk.uploading = false;
    });
    this.setStatus(TaktFileChunkUploadStatus.Cancelled);
    if (this.identifier) {
      try {
        await cancelFileChunks({ identifier: this.identifier });
      } catch {
        // 取消清理失败不阻断 UI
      }
    }
  }

  /**
   * 拉取并缓存后端分片计划
   * @returns 分片计划
   */
  private async ensureUploadPolicy(): Promise<FileUploadPolicy> {
    if (this.uploadPolicy) {
      return this.uploadPolicy;
    }
    const policy = await getFileUploadPolicy(this.file.size);
    if (!policy.useChunkUpload || !policy.chunkSizeBytes || !policy.totalChunks) {
      throw new Error('当前文件不需要或不允许分片上传');
    }
    this.uploadPolicy = policy;
    this.chunks = this.createChunksFromPolicy(policy);
    return policy;
  }

  /**
   * 执行上传主流程
   * @returns 上传结果
   */
  private async runUpload(): Promise<FileUploadResult> {
    try {
      this.setStatus(TaktFileChunkUploadStatus.Hashing);
      const policy = await this.ensureUploadPolicy();
      if (!this.identifier) {
        this.identifier = await generateFileIdentifier(this.file);
      }
      await this.restoreUploadedChunks(policy);
      this.setStatus(TaktFileChunkUploadStatus.Uploading);
      const allUploaded = await this.uploadAllChunks(policy);
      if (this.cancelled) {
        throw new Error('上传已取消');
      }
      if (this.paused || !allUploaded) {
        throw new TaktFileChunkUploadPausedError();
      }
      this.setStatus(TaktFileChunkUploadStatus.Merging);
      const mergeDto: FileChunkMerge = {
        identifier: this.identifier,
        fileName: this.file.name,
        totalChunks: policy.totalChunks!,
        totalSize: String(this.file.size),
        fileDescription: this.options.meta?.fileDescription,
        fileTags: this.options.meta?.fileTags,
        isPublic: this.options.meta?.isPublic,
        fileUploadType: this.options.meta?.fileUploadType,
        targetFileName: this.options.meta?.targetFileName,
        categoryPath: this.options.meta?.categoryPath,
        storageType: this.options.meta?.storageType,
        storageConfig: this.options.meta?.storageConfig,
        storageNaming: this.options.meta?.storageNaming,
        fileStatus: this.options.meta?.fileStatus,
      };
      const result = await mergeFileChunks(mergeDto);
      this.setStatus(TaktFileChunkUploadStatus.Success);
      this.emitProgress(100);
      return result;
    } catch (error) {
      if (error instanceof TaktFileChunkUploadPausedError) {
        this.setStatus(TaktFileChunkUploadStatus.Paused);
        throw error;
      }
      if (this.cancelled) {
        this.setStatus(TaktFileChunkUploadStatus.Cancelled);
      } else if (this.paused) {
        this.setStatus(TaktFileChunkUploadStatus.Paused);
      } else {
        this.setStatus(TaktFileChunkUploadStatus.Error);
      }
      throw error;
    }
  }

  /**
   * 从服务端恢复已上传分片（断点续传）
   * @param policy 分片计划
   */
  private async restoreUploadedChunks(policy: FileUploadPolicy): Promise<void> {
    const listResult = await listFileChunks({
      identifier: this.identifier,
      totalChunks: policy.totalChunks,
      totalSize: String(this.file.size),
    });
    const uploadedSet = new Set(listResult.uploadedChunkNumbers ?? []);
    this.chunks.forEach((chunk) => {
      const chunkNumber = chunk.index + 1;
      if (uploadedSet.has(chunkNumber)) {
        chunk.uploaded = true;
      }
    });
    this.emitProgress(this.calcPercent());
  }

  /**
   * 并发上传未完成分片
   * @param policy 分片计划
   * @returns 是否全部分片已上传
   */
  private async uploadAllChunks(policy: FileUploadPolicy): Promise<boolean> {
    const pending = (): InternalChunkState | undefined =>
      this.chunks.find((c) => !c.uploaded && !c.uploading && !this.paused && !this.cancelled);

    const workers: Promise<void>[] = [];
    const worker = async (): Promise<void> => {
      while (!this.paused && !this.cancelled) {
        const chunk = pending();
        if (!chunk) {
          break;
        }
        await this.uploadSingleChunk(chunk, policy);
      }
    };
    const concurrency = Math.max(1, Math.min(this.options.concurrency, this.chunks.length));
    for (let i = 0; i < concurrency; i++) {
      workers.push(worker());
    }
    await Promise.all(workers);
    if (this.paused || this.cancelled) {
      return false;
    }
    if (this.chunks.some((c) => !c.uploaded)) {
      throw new Error('部分分片上传失败');
    }
    return true;
  }

  /**
   * 上传单个分片（含重试与断点检查）
   * @param chunk 分片状态
   * @param policy 分片计划
   */
  private async uploadSingleChunk(chunk: InternalChunkState, policy: FileUploadPolicy): Promise<void> {
    if (chunk.uploaded || chunk.uploading || this.paused || this.cancelled) {
      return;
    }
    /** 须在首个 await 前占用，避免并发 worker 重复上传同一分片 */
    chunk.uploading = true;
    const chunkNumber = chunk.index + 1;
    try {
      const checkResult = await checkFileChunk({
        identifier: this.identifier,
        chunkNumber,
        chunkSize: String(chunk.blob.size),
        totalSize: String(this.file.size),
        totalChunks: policy.totalChunks,
        fileName: this.file.name,
      });
      if (checkResult.exists) {
        chunk.uploaded = true;
        this.emitProgress(this.calcPercent());
        return;
      }
      chunk.abortController = new AbortController();
      const chunkFile = new File([chunk.blob], this.file.name, { type: this.file.type });
      await uploadFileChunk(chunkFile, {
        identifier: this.identifier,
        chunkNumber,
        totalChunks: policy.totalChunks!,
        chunkSize: String(chunk.blob.size),
        totalSize: String(this.file.size),
        fileName: this.file.name,
      });
      chunk.uploaded = true;
      chunk.retries = 0;
      this.emitProgress(this.calcPercent());
    } catch (error) {
      if (this.paused || this.cancelled) {
        return;
      }
      if (chunk.retries < this.options.maxRetries) {
        chunk.retries += 1;
        chunk.uploading = false;
        await this.uploadSingleChunk(chunk, policy);
        return;
      }
      throw error;
    } finally {
      chunk.uploading = false;
      chunk.abortController = undefined;
    }
  }

  /**
   * 按后端分片计划切分文件
   * @param policy 分片计划
   * @returns 分片状态列表
   */
  private createChunksFromPolicy(policy: FileUploadPolicy): InternalChunkState[] {
    const chunkSize = Number(policy.chunkSizeBytes);
    const totalChunks = policy.totalChunks ?? 0;
    if (!Number.isFinite(chunkSize) || chunkSize <= 0 || totalChunks <= 0) {
      throw new Error('分片计划无效');
    }
    const chunks: InternalChunkState[] = [];
    for (let i = 0; i < totalChunks; i++) {
      const start = i * chunkSize;
      const end = i === totalChunks - 1 ? this.file.size : Math.min(start + chunkSize, this.file.size);
      chunks.push({
        index: i,
        blob: this.file.slice(start, end),
        uploaded: false,
        uploading: false,
        retries: 0,
      });
    }
    return chunks;
  }

  /**
   * 计算总进度
   * @returns 0-100
   */
  private calcPercent(): number {
    if (this.chunks.length === 0) {
      return 0;
    }
    const uploaded = this.chunks.filter((c) => c.uploaded).length;
    return Math.round((uploaded * 100) / this.chunks.length);
  }

  /**
   * 更新状态并通知
   * @param status 新状态
   */
  private setStatus(status: TaktFileChunkUploadStatus): void {
    this.status = status;
    this.emitProgress(this.calcPercent());
  }

  /**
   * 触发进度回调
   * @param percent 进度
   */
  private emitProgress(percent: number): void {
    this.options.onProgress?.({
      status: this.status,
      percent,
      uploadedChunks: this.chunks.filter((c) => c.uploaded).length,
      totalChunks: this.chunks.length,
      identifier: this.identifier,
    });
  }
}

/**
 * 智能上传：小文件整传，大文件分片+断点续传（策略以后端为准）
 * @param file 文件
 * @param meta 业务元数据
 * @param options 分片选项
 * @returns 上传结果
 */
export async function uploadTaktFileSmart(
  file: globalThis.File,
  meta?: FileUploadMeta,
  options?: TaktFileChunkUploadOptions
): Promise<FileUploadResult> {
  const policy = await getFileUploadPolicy(file.size);
  if (!policy.useChunkUpload) {
    return uploadFile(file, meta);
  }
  const uploader = new TaktFileChunkUploader(file, { ...options, meta, uploadPolicy: policy });
  return uploader.start();
}

/**
 * 强制分片上传（含断点续传）
 * @param file 文件
 * @param meta 业务元数据
 * @param options 分片选项
 * @returns 上传结果
 */
export async function uploadTaktFileWithChunks(
  file: globalThis.File,
  meta?: FileUploadMeta,
  options?: TaktFileChunkUploadOptions
): Promise<FileUploadResult> {
  const policy = await getFileUploadPolicy(file.size);
  const uploader = new TaktFileChunkUploader(file, { ...options, meta, uploadPolicy: policy });
  return uploader.start();
}
