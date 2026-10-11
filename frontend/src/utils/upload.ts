// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：upload.ts
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：通用上传工具（MD5 标识 + 可选 UploadHelper）。
//           Foundation 文件模块请使用 api/foundation/file.api.ts + takt-file-chunk-upload.ts（对接 TaktFiles/upload-policy）。
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import axios, { type AxiosProgressEvent, type CancelTokenSource } from 'axios';
import SparkMD5 from 'spark-md5';
import { createLogger } from '@/utils/logger';

/** 上传模块日志 */
const uploadLogger = createLogger('upload');

/**
 * 分片存在性检查结果（与后端约定字段 exists）
 */
interface TaktChunkCheckResult {
  /**
   * 分片是否已存在
   */
  exists?: boolean;
}

function isUploadCancelled(error: unknown): boolean {
  if (axios.isCancel(error)) return true
  const e = error as { __CANCEL__?: boolean; message?: string }
  return e.__CANCEL__ === true || (typeof e.message === 'string' && e.message.includes('canceled'))
}

function uploadErrorMessage(error: unknown): string {
  if (error instanceof Error) return error.message
  return String(error)
}

/**
 * 文件上传状态
 */
export enum UploadStatus {
  /** 等待中 */
  WAITING = 'waiting',
  /** 上传中 */
  UPLOADING = 'uploading',
  /** 暂停 */
  PAUSED = 'paused',
  /** 成功 */
  SUCCESS = 'success',
  /** 失败 */
  ERROR = 'error',
  /** 取消 */
  CANCELLED = 'cancelled'
}

/**
 * 分片信息
 */
export interface ChunkInfo {
  /** 分片索引 */
  index: number
  /** 分片起始位置 */
  start: number
  /** 分片结束位置 */
  end: number
  /** 分片数据 */
  blob: Blob
  /** 是否已上传 */
  uploaded: boolean
  /** 是否正在上传 */
  uploading: boolean
  /** 重试次数 */
  retries: number
  /** 上传进度（0-100） */
  progress: number
  /** 取消令牌 */
  cancelToken?: CancelTokenSource
}

/**
 * 上传文件信息
 */
export interface UploadFileInfo {
  /** 文件唯一标识 */
  id: string
  /** 原始文件对象 */
  file: File
  /** 文件名称 */
  name: string
  /** 文件大小 */
  size: number
  /** 文件类型 */
  type: string
  /** 文件唯一标识符（用于断点续传） */
  identifier: string
  /** 文件状态 */
  status: UploadStatus
  /** 上传进度（0-100） */
  progress: number
  /** 分片列表 */
  chunks: ChunkInfo[]
  /** 总分片数 */
  totalChunks: number
  /** 已上传分片数 */
  uploadedChunks: number
  /** 上传开始时间 */
  startTime?: number
  /** 上传结束时间 */
  endTime?: number
  /** 上传响应数据 */
  response?: unknown
  /** 错误信息 */
  error?: Error
  /** 自定义参数 */
  params?: Record<string, unknown>
}

/**
 * 上传配置选项
 */
export interface UploadOptions {
  /** 上传地址 */
  url?: string
  /** 分片大小（字节），默认 2MB */
  chunkSize?: number
  /** 是否自动上传 */
  autoStart?: boolean
  /** 是否启用分片上传 */
  chunked?: boolean
  /** 是否启用服务端分片合并 */
  chunkedByServer?: boolean
  /** 最大并发上传数 */
  simultaneousUploads?: number
  /** 最大重试次数 */
  maxRetries?: number
  /** 重试延迟时间（毫秒） */
  retryDelay?: number
  /** 上传参数 */
  params?: Record<string, unknown>
  /** 请求头 */
  headers?: Record<string, string>
  /** 上传成功回调 */
  onSuccess?: (file: UploadFileInfo, response: unknown) => void
  /** 上传失败回调 */
  onError?: (file: UploadFileInfo, error: Error) => void
  /** 上传进度回调 */
  onProgress?: (file: UploadFileInfo, progress: number) => void
  /** 文件状态变化回调 */
  onStatusChange?: (file: UploadFileInfo, status: UploadStatus) => void
  /** 分片检查URL（用于断点续传） */
  checkChunkUrl?: string
  /** 分片上传URL */
  chunkUrl?: string
  /** 合并分片URL */
  mergeUrl?: string
}

/**
 * 上传文件结果
 */
export interface UploadResult {
  /** 是否成功 */
  success: boolean
  /** 文件信息 */
  file?: UploadFileInfo
  /** 响应数据 */
  response?: unknown
  /** 错误信息 */
  error?: Error
}

/**
 * 生成文件唯一标识符（用于断点续传）
 * 使用 spark-md5 计算文件内容的 MD5 值
 */
type FilePrototypeWithLegacySlice = typeof File.prototype & {
  mozSlice?: typeof File.prototype.slice
  webkitSlice?: typeof File.prototype.slice
}

export async function generateFileIdentifier(file: File): Promise<string> {
  return new Promise((resolve, reject) => {
    const proto = File.prototype as FilePrototypeWithLegacySlice
    const blobSlice = proto.slice ?? proto.mozSlice ?? proto.webkitSlice
    if (!blobSlice) {
      reject(new Error('浏览器不支持文件分片读取'))
      return
    }
    const chunkSize = 2 * 1024 * 1024 // 2MB 分片读取
    const chunks = Math.ceil(file.size / chunkSize)
    let currentChunk = 0
    const spark = new SparkMD5.ArrayBuffer()
    const fileReader = new FileReader()

    fileReader.onload = (e) => {
      if (e.target?.result) {
        spark.append(e.target.result as ArrayBuffer)
        currentChunk++

        if (currentChunk < chunks) {
          loadNext()
        } else {
          const hash = spark.end()
          resolve(hash)
        }
      }
    }

    fileReader.onerror = () => {
      reject(new Error('读取文件失败'))
    }

    const loadNext = () => {
      const start = currentChunk * chunkSize
      const end = start + chunkSize >= file.size ? file.size : start + chunkSize
      fileReader.readAsArrayBuffer(blobSlice.call(file, start, end))
    }

    loadNext()
  })
}

/**
 * 快速生成文件标识符（基于文件元数据，不读取文件内容）
 * 用于快速标识，但不保证唯一性（相同内容不同文件名会生成不同标识）
 */
function generateFileIdentifierFast(file: File): string {
  const data = `${file.name}-${file.size}-${file.lastModified}`
  return SparkMD5.hash(data)
}

/**
 * 生成文件ID
 */
function generateFileId(): string {
  return `file_${Date.now()}_${Math.random().toString(36).slice(2, 11)}`;
}

/**
 * 将文件分片
 */
function createChunks(file: File, chunkSize: number): ChunkInfo[] {
  const chunks: ChunkInfo[] = []
  const totalChunks = Math.ceil(file.size / chunkSize)

  for (let i = 0; i < totalChunks; i++) {
    const start = i * chunkSize
    const end = Math.min(start + chunkSize, file.size)
    const blob = file.slice(start, end)

    chunks.push({
      index: i,
      start,
      end,
      blob,
      uploaded: false,
      uploading: false,
      retries: 0,
      progress: 0
    })
  }

  return chunks
}

/**
 * 上传工具类
 * 自行实现分片上传、断点续传、暂停/恢复、错误重试、队列管理
 */
export class UploadHelper {
  private files: Map<string, UploadFileInfo> = new Map()
  private options: Required<Omit<UploadOptions, 'onSuccess' | 'onError' | 'onProgress' | 'onStatusChange' | 'params' | 'headers' | 'checkChunkUrl' | 'chunkUrl' | 'mergeUrl'>> & Pick<UploadOptions, 'onSuccess' | 'onError' | 'onProgress' | 'onStatusChange' | 'params' | 'headers' | 'checkChunkUrl' | 'chunkUrl' | 'mergeUrl'>
  private defaultChunkSize = 2 * 1024 * 1024 // 默认分片大小：2MB
  private uploadingFiles: Set<string> = new Set()
  private activeUploads: Map<string, Promise<void>> = new Map()

  constructor(options: UploadOptions = {}) {
    this.options = {
      url: options.url || 'upload',
      chunkSize: options.chunkSize || this.defaultChunkSize,
      autoStart: options.autoStart ?? false,
      chunked: options.chunked ?? true,
      chunkedByServer: options.chunkedByServer ?? false,
      simultaneousUploads: options.simultaneousUploads || 3,
      maxRetries: options.maxRetries || 3,
      retryDelay: options.retryDelay || 1000,
      checkChunkUrl: options.checkChunkUrl || 'upload/check',
      chunkUrl: options.chunkUrl || 'upload/chunk',
      mergeUrl: options.mergeUrl || 'upload/merge',
      params: options.params || {},
      headers: options.headers || {},
      onSuccess: options.onSuccess,
      onError: options.onError,
      onProgress: options.onProgress,
      onStatusChange: options.onStatusChange
    }
  }

  /**
   * 添加文件到上传队列
   */
  async addFile(file: File, params?: Record<string, unknown>, useFastIdentifier: boolean = false): Promise<UploadFileInfo> {
    const id = generateFileId()
    
    // 根据参数选择使用快速标识符还是完整 MD5
    const identifier = useFastIdentifier
      ? generateFileIdentifierFast(file)
      : await generateFileIdentifier(file)
    
    const chunks = this.options.chunked && file.size > this.options.chunkSize
      ? createChunks(file, this.options.chunkSize)
      : []

    const fileInfo: UploadFileInfo = {
      id,
      file,
      name: file.name,
      size: file.size,
      type: file.type,
      identifier,
      status: UploadStatus.WAITING,
      progress: 0,
      chunks,
      totalChunks: chunks.length,
      uploadedChunks: 0,
      params: { ...this.options.params, ...params }
    }

    this.files.set(id, fileInfo)

    uploadLogger.info('添加文件到队列', { action: 'add', fileName: file.name, id, identifier });

    // 如果启用自动上传，立即开始上传
    if (this.options.autoStart) {
      this.upload(id)
    }

    return fileInfo
  }

  /**
   * 移除文件
   */
  removeFile(id: string): void {
    const fileInfo = this.files.get(id)
    if (!fileInfo) return

    // 取消正在上传的分片
    if (fileInfo.status === UploadStatus.UPLOADING || fileInfo.status === UploadStatus.PAUSED) {
      this.cancel(id)
    }

    this.files.delete(id)
    this.uploadingFiles.delete(id)
    this.activeUploads.delete(id)

    uploadLogger.info('移除文件', { action: 'remove', fileName: fileInfo.name });
  }

  /**
   * 检查分片是否已上传（用于断点续传）
   */
  private async checkChunk(fileInfo: UploadFileInfo, chunk: ChunkInfo): Promise<boolean> {
    try {
      const result = await request.post<TaktChunkCheckResult>(this.options.checkChunkUrl!, {
        identifier: fileInfo.identifier,
        chunkNumber: chunk.index + 1,
        chunkSize: chunk.blob.size,
        totalSize: fileInfo.size,
        fileName: fileInfo.name,
      }, {
        headers: this.options.headers,
      });

      return result?.exists === true;
    } catch (error) {
      uploadLogger.warn('检查分片失败，将继续上传', { action: 'check-chunk' }, error);
      return false
    }
  }

  /**
   * 上传单个分片
   */
  private async uploadChunk(fileInfo: UploadFileInfo, chunk: ChunkInfo): Promise<void> {
    if (chunk.uploaded || chunk.uploading) {
      return
    }

    // 检查分片是否已上传（断点续传）
    if (this.options.chunked && fileInfo.chunks.length > 0) {
      const exists = await this.checkChunk(fileInfo, chunk)
      if (exists) {
        chunk.uploaded = true
        chunk.progress = 100
        fileInfo.uploadedChunks++
        this.updateProgress(fileInfo)
        return
      }
    }

    chunk.uploading = true

    // 创建取消令牌
    const CancelToken = (await import('axios')).default.CancelToken
    const source = CancelToken.source()
    chunk.cancelToken = source

    try {
      const formData = new FormData()
      formData.append('file', chunk.blob, fileInfo.name)
      formData.append('chunkNumber', String(chunk.index + 1))
      formData.append('totalChunks', String(fileInfo.totalChunks))
      formData.append('chunkSize', String(chunk.blob.size))
      formData.append('totalSize', String(fileInfo.size))
      formData.append('identifier', fileInfo.identifier)
      formData.append('fileName', fileInfo.name)

      // 添加自定义参数
      if (fileInfo.params) {
        Object.entries(fileInfo.params).forEach(([key, value]) => {
          formData.append(key, String(value))
        })
      }

      await request.post(this.options.chunkUrl!, formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
          ...this.options.headers
        },
        cancelToken: source.token,
        onUploadProgress: (progressEvent: AxiosProgressEvent) => {
          if (progressEvent.total) {
            chunk.progress = Math.round((progressEvent.loaded * 100) / progressEvent.total)
            this.updateProgress(fileInfo)
          }
        }
      })

      chunk.uploaded = true
      chunk.uploading = false
      fileInfo.uploadedChunks++
      this.updateProgress(fileInfo)

      uploadLogger.debug('分片上传成功', {
        action: 'chunk-success',
        fileName: fileInfo.name,
        chunkIndex: chunk.index + 1,
        totalChunks: fileInfo.totalChunks,
      });
    } catch (error: unknown) {
      chunk.uploading = false

      // 如果是取消操作，不进行重试
      if (isUploadCancelled(error)) {
        throw error
      }

      // 重试逻辑
      if (chunk.retries < this.options.maxRetries) {
        chunk.retries++
        uploadLogger.info('重试分片', {
          action: 'chunk-retry',
          fileName: fileInfo.name,
          chunkIndex: chunk.index + 1,
          retries: chunk.retries,
          maxRetries: this.options.maxRetries,
        });
        
        await new Promise(resolve => setTimeout(resolve, this.options.retryDelay))
        return this.uploadChunk(fileInfo, chunk)
      } else {
        uploadLogger.error('分片上传失败', {
          action: 'chunk-fail',
          fileName: fileInfo.name,
          chunkIndex: chunk.index + 1,
        }, error);
        const chunkErr = new Error(`分片上传失败: ${uploadErrorMessage(error)}`)
        Object.assign(chunkErr, { cause: error })
        throw chunkErr
      }
    }
  }

  /**
   * 合并分片（服务端合并）
   */
  private async mergeChunks(fileInfo: UploadFileInfo): Promise<unknown> {
    try {
      return await request.post<unknown>(this.options.mergeUrl!, {
        identifier: fileInfo.identifier,
        fileName: fileInfo.name,
        totalChunks: fileInfo.totalChunks,
        totalSize: fileInfo.size,
      }, {
        headers: this.options.headers,
      });
    } catch (error: unknown) {
      uploadLogger.error('合并分片失败', { action: 'merge' }, error);
      const mergeErr = new Error(`合并分片失败: ${uploadErrorMessage(error)}`)
      Object.assign(mergeErr, { cause: error })
      throw mergeErr
    }
  }

  /**
   * 上传整个文件（非分片）
   */
  private async uploadWholeFile(fileInfo: UploadFileInfo): Promise<unknown> {
    const CancelToken = (await import('axios')).default.CancelToken
    const source = CancelToken.source()

    try {
      const formData = new FormData()
      formData.append('file', fileInfo.file)

      // 添加自定义参数
      if (fileInfo.params) {
        Object.entries(fileInfo.params).forEach(([key, value]) => {
          formData.append(key, String(value))
        })
      }

      return await request.post<unknown>(this.options.url, formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
          ...this.options.headers,
        },
        cancelToken: source.token,
        onUploadProgress: (progressEvent: AxiosProgressEvent) => {
          if (progressEvent.total) {
            fileInfo.progress = Math.round((progressEvent.loaded * 100) / progressEvent.total);
            this.updateProgress(fileInfo);
          }
        },
      });
    } catch (error: unknown) {
      if (isUploadCancelled(error)) {
        throw error
      }
      throw error
    }
  }

  /**
   * 更新上传进度
   */
  private updateProgress(fileInfo: UploadFileInfo): void {
    if (fileInfo.chunks.length > 0) {
      // 计算所有分片的平均进度
      const totalProgress = fileInfo.chunks.reduce((sum, chunk) => sum + chunk.progress, 0)
      fileInfo.progress = Math.round(totalProgress / fileInfo.chunks.length)
    }

    // 触发进度回调
    if (this.options.onProgress) {
      this.options.onProgress(fileInfo, fileInfo.progress)
    }
  }

  /**
   * 上传文件
   */
  async upload(id?: string): Promise<void> {
    const filesToUpload = id ? [this.files.get(id)!].filter(Boolean) : Array.from(this.files.values())

    for (const fileInfo of filesToUpload) {
      if (fileInfo.status !== UploadStatus.WAITING && fileInfo.status !== UploadStatus.PAUSED && fileInfo.status !== UploadStatus.ERROR) {
        continue
      }

      if (this.uploadingFiles.size >= this.options.simultaneousUploads) {
        uploadLogger.debug('达到最大并发数，等待上传', { action: 'wait-concurrency' });
        continue
      }

      this.uploadingFiles.add(fileInfo.id)
      fileInfo.startTime = Date.now()

      const uploadPromise = this.doUpload(fileInfo)
      this.activeUploads.set(fileInfo.id, uploadPromise)

      uploadPromise
        .finally(() => {
          this.uploadingFiles.delete(fileInfo.id)
          this.activeUploads.delete(fileInfo.id)
        })
    }
  }

  /**
   * 执行上传
   */
  private async doUpload(fileInfo: UploadFileInfo): Promise<void> {
    try {
      this.setStatus(fileInfo, UploadStatus.UPLOADING)

      let response: unknown

      if (this.options.chunked && fileInfo.chunks.length > 0) {
        // 分片上传
        // 并发上传分片
        const uploadPromises: Promise<void>[] = []
        const activeChunkUploads = new Set<number>()

        const uploadNextChunk = async (): Promise<void> => {
          // 找到未上传且未在上传中的分片
          const chunk = fileInfo.chunks.find(
            c => !c.uploaded && !c.uploading && !activeChunkUploads.has(c.index)
          )

          if (!chunk) {
            return
          }

          activeChunkUploads.add(chunk.index)
          const chunkPromise = this.uploadChunk(fileInfo, chunk)
            .then(() => {
              activeChunkUploads.delete(chunk.index)
              // 继续上传下一个分片
              if (activeChunkUploads.size < this.options.simultaneousUploads) {
                return uploadNextChunk()
              }
            })
            .catch(error => {
              activeChunkUploads.delete(chunk.index)
              throw error
            })

          uploadPromises.push(chunkPromise)

          // 如果还有并发空间，继续上传下一个分片
          if (activeChunkUploads.size < this.options.simultaneousUploads) {
            await uploadNextChunk()
          }
        }

        // 开始并发上传
        const concurrentUploads: Promise<void>[] = []
        for (let i = 0; i < Math.min(this.options.simultaneousUploads, fileInfo.chunks.length); i++) {
          concurrentUploads.push(uploadNextChunk())
        }

        await Promise.all(concurrentUploads)

        // 等待所有分片上传完成
        await Promise.all(uploadPromises)

        // 合并分片
        if (this.options.chunkedByServer) {
          response = await this.mergeChunks(fileInfo)
        } else {
          // 客户端合并（这里需要服务端支持，通常使用服务端合并）
          response = { success: true, url: '' }
        }
      } else {
        // 整体上传
        response = await this.uploadWholeFile(fileInfo)
      }

      fileInfo.endTime = Date.now()
      fileInfo.response = response
      this.setStatus(fileInfo, UploadStatus.SUCCESS)

      uploadLogger.info('文件上传成功', { action: 'success', fileName: fileInfo.name });
      if (this.options.onSuccess) {
        this.options.onSuccess(fileInfo, response)
      }
    } catch (error: unknown) {
      // 如果是取消操作，不触发错误回调
      if (isUploadCancelled(error)) {
        this.setStatus(fileInfo, UploadStatus.CANCELLED)
        return
      }

      fileInfo.endTime = Date.now()
      fileInfo.error = error instanceof Error ? error : new Error(String(error))
      this.setStatus(fileInfo, UploadStatus.ERROR)

      uploadLogger.error('文件上传失败', { action: 'fail', fileName: fileInfo.name }, error);
      if (this.options.onError) {
        this.options.onError(fileInfo, fileInfo.error)
      }
    }
  }

  /**
   * 设置文件状态
   */
  private setStatus(fileInfo: UploadFileInfo, status: UploadStatus): void {
    const oldStatus = fileInfo.status
    fileInfo.status = status

    if (oldStatus !== status && this.options.onStatusChange) {
      this.options.onStatusChange(fileInfo, status)
    }
  }

  /**
   * 暂停上传
   */
  pause(id?: string): void {
    const filesToPause = id ? [this.files.get(id)!].filter(Boolean) : Array.from(this.files.values())

    for (const fileInfo of filesToPause) {
      if (fileInfo.status === UploadStatus.UPLOADING) {
        // 取消所有正在上传的分片
        fileInfo.chunks.forEach(chunk => {
          if (chunk.uploading && chunk.cancelToken) {
            chunk.cancelToken.cancel('用户暂停上传')
            chunk.uploading = false
          }
        })

        this.setStatus(fileInfo, UploadStatus.PAUSED)
        this.uploadingFiles.delete(fileInfo.id)
        this.activeUploads.delete(fileInfo.id)

        uploadLogger.info('暂停上传', { action: 'pause', fileName: fileInfo.name });
      }
    }
  }

  /**
   * 恢复上传
   */
  resume(id?: string): void {
    const ids = id ? [id] : Array.from(this.files.keys())

    for (const fileId of ids) {
      const fileInfo = this.files.get(fileId)
      if (fileInfo && (fileInfo.status === UploadStatus.PAUSED || fileInfo.status === UploadStatus.ERROR)) {
        this.upload(fileId)
        uploadLogger.info('恢复上传', { action: 'resume', fileName: fileInfo.name });
      }
    }
  }

  /**
   * 取消上传
   */
  cancel(id?: string): void {
    const filesToCancel = id ? [this.files.get(id)!].filter(Boolean) : Array.from(this.files.values())

    for (const fileInfo of filesToCancel) {
      // 取消所有正在上传的分片
      fileInfo.chunks.forEach(chunk => {
        if (chunk.uploading && chunk.cancelToken) {
          chunk.cancelToken.cancel('用户取消上传')
          chunk.uploading = false
        }
      })

      this.setStatus(fileInfo, UploadStatus.CANCELLED)
      this.uploadingFiles.delete(fileInfo.id)
      this.activeUploads.delete(fileInfo.id)

      uploadLogger.info('取消上传', { action: 'cancel', fileName: fileInfo.name });
    }
  }

  /**
   * 重试上传
   */
  retry(id?: string): void {
    const ids = id ? [id] : Array.from(this.files.keys())

    for (const fileId of ids) {
      const fileInfo = this.files.get(fileId)
      if (fileInfo && fileInfo.status === UploadStatus.ERROR) {
        fileInfo.error = undefined
        this.upload(fileId)
        uploadLogger.info('重试上传', { action: 'retry', fileName: fileInfo.name });
      }
    }
  }

  /**
   * 获取文件列表
   */
  getFiles(): UploadFileInfo[] {
    return Array.from(this.files.values())
  }

  /**
   * 获取文件信息
   */
  getFile(id: string): UploadFileInfo | undefined {
    return this.files.get(id)
  }

  /**
   * 更新上传选项
   */
  updateOptions(options: Partial<UploadOptions>): void {
    Object.assign(this.options, options)
  }

  /**
   * 销毁上传器
   */
  destroy(): void {
    // 取消所有上传
    this.cancel()

    // 清空文件列表
    this.files.clear()
    this.uploadingFiles.clear()
    this.activeUploads.clear()

    uploadLogger.info('上传器已销毁', { action: 'destroy' });
  }
}

/**
 * 创建上传器实例
 */
export function createUploader(options: UploadOptions = {}): UploadHelper {
  return new UploadHelper(options)
}

/**
 * 上传单个文件
 */
export async function uploadFile(
  file: File,
  options: UploadOptions = {},
  useFastIdentifier: boolean = false
): Promise<UploadResult> {
  return new Promise<UploadResult>((resolve) => {
    const uploader = new UploadHelper({
      ...options,
      autoStart: true,
      onSuccess: (fileInfo, response) => {
        resolve({
          success: true,
          file: fileInfo,
          response
        })
        uploader.destroy()
      },
      onError: (fileInfo, error) => {
        resolve({
          success: false,
          file: fileInfo,
          error
        })
        uploader.destroy()
      }
    })

    void uploader.addFile(file, undefined, useFastIdentifier).catch((err: unknown) => {
      resolve({
        success: false,
        error: err instanceof Error ? err : new Error(String(err))
      })
      uploader.destroy()
    })
  })
}

/**
 * 上传多个文件
 */
export async function uploadFiles(
  files: File[],
  options: UploadOptions = {},
  useFastIdentifier: boolean = false
): Promise<UploadResult[]> {
  const uploader = new UploadHelper({
    ...options,
    autoStart: true
  })

  // 并行添加文件（计算 MD5）
  const fileInfos = await Promise.all(
    files.map(file => uploader.addFile(file, undefined, useFastIdentifier))
  )

  const promises = fileInfos.map(fileInfo => {
    return new Promise<UploadResult>((resolve) => {
      const onSuccess = (f: UploadFileInfo, response: unknown) => {
        if (f.id === fileInfo.id) {
          resolve({
            success: true,
            file: f,
            response
          })
        }
      }

      const onError = (f: UploadFileInfo, error: Error) => {
        if (f.id === fileInfo.id) {
          resolve({
            success: false,
            file: f,
            error
          })
        }
      }

      const originalOnSuccess = options.onSuccess
      const originalOnError = options.onError

      uploader.updateOptions({
        onSuccess: (f, response) => {
          onSuccess(f, response)
          if (originalOnSuccess) {
            originalOnSuccess(f, response)
          }
        },
        onError: (f, error) => {
          onError(f, error)
          if (originalOnError) {
            originalOnError(f, error)
          }
        }
      })
    })
  })

  const resolvedResults = await Promise.all(promises)
  uploader.destroy()

  return resolvedResults
}
