<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise  -->
<!-- 命名空间：@/components/common/TaktUploadFile/cropper -->
<!-- 文件名称：index.vue -->
<!-- 创建时间：2025-01-20 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：通用图片裁剪组件，基于 cropperjs 2.0 -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="takt-cropper">
    <div
      v-if="imageSrc"
      class="takt-cropper-container"
      :style="{ width: `${width}px`, height: `${height}px` }"
    >
      <img
        ref="cropperImgRef"
        :src="imageSrc"
        :alt="t('components.common.page.upload.cropimage')"
        class="takt-cropper-img"
      >
    </div>
    <div
      v-else
      class="takt-cropper-empty"
    >
      <a-empty :description="t('components.common.page.upload.selectimagefirst')" />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, watch, onUnmounted, nextTick } from 'vue'
import { useI18n } from 'vue-i18n'
import Cropper from 'cropperjs'

const { t } = useI18n()

/**
 * Cropper.js 2.0 类型定义
 */
interface CropperInstance {
  destroy(): void
  getCropperCanvas(): { disabled?: boolean } | null
  getCropperImage(): {
    src?: string
    $resetTransform: () => void
    $move: (x: number, y: number) => void
    $moveTo: (x: number, y: number) => void
    $scale: (scaleX: number, scaleY?: number) => void
    $rotate: (degree: number) => void
    $setTransform: (transform: Record<string, unknown>) => void
    $getTransform: () => Record<string, unknown> | null
  } | null
  getCropperSelection(): {
    aspectRatio?: number
    initialCoverage?: number
    hidden?: boolean
    x?: number
    y?: number
    width?: number
    height?: number
    $toCanvas: (options?: Record<string, unknown>) => Promise<HTMLCanvasElement>
    $reset: () => void
    $center: () => unknown
    $change: (x: number, y: number, width?: number, height?: number, aspectRatio?: number) => unknown
  } | null
}

interface Props {
  /** 图片地址 */
  imageSrc?: string
  /** 裁剪比例(如 1:1, 16:9),传 undefined 或 'free' 表示自由比例 */
  aspectRatio?: number | 'free'
  /** 容器宽度(默认 512) */
  width?: number
  /** 容器高度(默认 512) */
  height?: number
  /** 裁剪后图片宽度(可选,不指定则使用原始裁剪尺寸) */
  cropWidth?: number | undefined
  /** 裁剪后图片高度(可选,不指定则使用原始裁剪尺寸) */
  cropHeight?: number | undefined
  /** 图片质量(0-1,默认 0.9) */
  quality?: number
  /** 图片格式(默认 'image/png') */
  format?: 'image/png' | 'image/jpeg' | 'image/webp'
}

/**
 * 裁剪选项
 */
export interface CropOptions {
  /** 裁剪后图片宽度 */
  width?: number
  /** 裁剪后图片高度 */
  height?: number
  /** 图片质量（0-1） */
  quality?: number
  /** 图片格式 */
  format?: 'image/png' | 'image/jpeg' | 'image/webp'
  /** 是否填充背景（仅对非透明格式有效） */
  fillColor?: string
}

/**
 * 裁剪结果
 */
export interface CropResult {
  /** Blob 对象 */
  blob: Blob
  /** DataURL 字符串 */
  dataUrl: string
  /** Canvas 对象（可选） */
  canvas?: HTMLCanvasElement
  /** File 对象（可选，需要提供文件名） */
  file?: File
}

const props = withDefaults(defineProps<Props>(), {
  imageSrc: '',
  aspectRatio: 'free',
  width: 512,
  height: 512,
  cropWidth: undefined,
  cropHeight: undefined,
  quality: 0.9,
  format: 'image/png'
})

const emit = defineEmits<{
  /** 裁剪完成事件 */
  'crop': [blob: Blob, dataUrl: string]
  /** 裁剪错误事件 */
  'error': [error: Error]
}>()

const cropperImgRef = ref<HTMLImageElement | null>(null)
const cropperInstance = ref<CropperInstance | null>(null)

/**
 * 按 cropWidth/cropHeight 设置默认裁剪框并居中（头像场景默认 256×256）
 */
const applyDefaultSelection = () => {
  const selection = cropperInstance.value?.getCropperSelection()
  if (!selection) return

  const aspect = typeof props.aspectRatio === 'number' ? props.aspectRatio : undefined
  if (aspect != null) {
    selection.aspectRatio = aspect
  }

  const boxW = props.cropWidth
  const boxH = props.cropHeight ?? props.cropWidth
  if (boxW == null || boxH == null || boxW <= 0 || boxH <= 0) {
    if (aspect != null) {
      selection.initialCoverage = 0.5
    }
    return
  }

  try {
    selection.$change(0, 0, boxW, boxH, aspect)
    selection.$center()
  } catch (error: unknown) {
    console.warn('[TaktCropper] 设置默认裁剪框失败，回退 initialCoverage', error)
    const side = Math.min(props.width, props.height)
    selection.initialCoverage = Math.min(1, Math.max(0.1, boxW / side))
  }
}

// 初始化裁剪器
const initCropper = () => {
  if (!cropperImgRef.value || !props.imageSrc) return

  // 销毁旧实例
  if (cropperInstance.value) {
    cropperInstance.value.destroy()
    cropperInstance.value = null
  }

  nextTick(() => {
    if (!cropperImgRef.value) return

    try {
      // cropperjs 2.0: 使用构造函数创建实例
      cropperInstance.value = new Cropper(cropperImgRef.value, {}) as unknown as CropperInstance
      // 选区挂载略晚于构造；短延迟后设默认 256 居中框
      const tryApply = (left: number) => {
        const selection = cropperInstance.value?.getCropperSelection()
        if (selection) {
          applyDefaultSelection()
          return
        }
        if (left > 0) {
          window.setTimeout(() => tryApply(left - 1), 50)
        }
      }
      requestAnimationFrame(() => tryApply(6))
    } catch (error: unknown) {
      console.error('[TaktCropper] 初始化裁剪器失败:', error)
      emit('error', error instanceof Error ? error : new Error(String(error)))
    }
  })
}

// 监听图片地址变化
watch(() => props.imageSrc, (newSrc) => {
  if (newSrc) {
    nextTick(() => {
      initCropper()
    })
  } else {
    // 清空时销毁裁剪器
    if (cropperInstance.value) {
      cropperInstance.value.destroy()
      cropperInstance.value = null
    }
  }
}, { immediate: true })

// 监听比例变化
watch(() => props.aspectRatio, () => {
  if (cropperInstance.value && props.imageSrc) {
    const selection = cropperInstance.value.getCropperSelection()
    if (selection) {
      if (typeof props.aspectRatio === 'number') {
        selection.aspectRatio = props.aspectRatio
      } else {
        delete selection.aspectRatio
      }
    }
  }
})

// 组件卸载时清理
onUnmounted(() => {
  if (cropperInstance.value) {
    cropperInstance.value.destroy()
    cropperInstance.value = null
  }
})

/**
 * 获取裁剪后的 Canvas
 * @param options 裁剪选项
 * @returns Promise<HTMLCanvasElement>
 */
const getCroppedCanvas = async (options?: CropOptions): Promise<HTMLCanvasElement> => {
  if (!cropperInstance.value) {
    throw new Error('裁剪器未初始化')
  }

  try {
    const selection = cropperInstance.value.getCropperSelection()
    if (!selection) {
      throw new Error('裁剪选择区域未初始化')
    }

    const width = options?.width || props.cropWidth
    const height = options?.height || props.cropHeight

    const canvasOptions: Record<string, unknown> = {
      imageSmoothingEnabled: true,
      imageSmoothingQuality: 'high'
    }
    
    if (width) {
      canvasOptions.width = width
    }
    if (height) {
      canvasOptions.height = height
    }
    if (options?.fillColor) {
      canvasOptions.fillColor = options.fillColor
    }

    // Cropper.js 2.0: 使用 $toCanvas 方法（返回 Promise）
    const canvas = await selection.$toCanvas(canvasOptions)
    return canvas
  } catch (error: unknown) {
    console.error('[TaktCropper] 获取裁剪 Canvas 失败:', error)
    const err = error instanceof Error ? error : new Error(String(error))
    emit('error', err)
    throw err
  }
}

/**
 * 获取裁剪后的图片（通用方法）
 * @param options 裁剪选项
 * @param fileName 文件名（如果提供，将生成 File 对象）
 * @returns Promise<CropResult>
 */
const getCroppedImage = async (options?: CropOptions, fileName?: string): Promise<CropResult> => {
  if (!cropperInstance.value) {
    throw new Error('裁剪器未初始化')
  }

  try {
    const format = options?.format || props.format
    const quality = options?.quality ?? props.quality

    // 获取裁剪后的 canvas
    const canvas = await getCroppedCanvas(options)

    // 将 canvas 转换为 Blob 和 DataURL
    return convertCanvasToResult(canvas, format, quality, fileName)
  } catch (error: unknown) {
    console.error('[TaktCropper] 获取裁剪图片失败:', error)
    const err = error instanceof Error ? error : new Error(String(error))
    emit('error', err)
    throw err
  }
}

/**
 * 将 Canvas 转换为裁剪结果
 */
const convertCanvasToResult = (
  canvas: HTMLCanvasElement,
  format: string,
  quality: number,
  fileName?: string
): Promise<CropResult> => {
  return new Promise((resolve, reject) => {
    canvas.toBlob(
      (blob: Blob | null) => {
        if (!blob) {
          reject(new Error('裁剪失败：无法生成图片'))
          return
        }

        const dataUrl = canvas.toDataURL(format, quality)
        const result: CropResult = {
          blob,
          dataUrl,
          canvas
        }

        // 如果提供了文件名，生成 File 对象
        if (fileName) {
          result.file = new File([blob], fileName, { type: format })
        }

        resolve(result)
      },
      format,
      quality
    )
  })
}

/**
 * 获取裁剪后的 Blob
 * @param options 裁剪选项
 * @returns Promise<Blob>
 */
const getCroppedBlob = async (options?: CropOptions): Promise<Blob> => {
  const result = await getCroppedImage(options)
  return result.blob
}

/**
 * 获取裁剪后的 DataURL
 * @param options 裁剪选项
 * @returns Promise<string>
 */
const getCroppedDataUrl = async (options?: CropOptions): Promise<string> => {
  const result = await getCroppedImage(options)
  return result.dataUrl
}

/**
 * 获取裁剪后的 File
 * @param fileName 文件名
 * @param options 裁剪选项
 * @returns Promise<File>
 */
const getCroppedFile = async (fileName: string, options?: CropOptions): Promise<File> => {
  const result = await getCroppedImage(options, fileName)
  if (!result.file) {
    throw new Error('无法生成 File 对象')
  }
  return result.file
}

/**
 * 裁剪并触发事件
 * @param options 裁剪选项
 */
const crop = async (options?: CropOptions) => {
  const result = await getCroppedImage(options)
  emit('crop', result.blob, result.dataUrl)
  return result
}

/**
 * 重置裁剪区域
 */
const reset = () => {
  if (cropperInstance.value) {
    const image = cropperInstance.value.getCropperImage()
    const selection = cropperInstance.value.getCropperSelection()
    if (image) {
      image.$resetTransform()
    }
    if (selection) {
      selection.$reset()
    }
  }
}

/**
 * 清除裁剪区域
 */
const clear = () => {
  if (cropperInstance.value) {
    const selection = cropperInstance.value.getCropperSelection()
    if (selection) {
      selection.$reset()
      selection.hidden = true
    }
  }
}

/**
 * 替换图片
 */
const replace = (url: string) => {
  if (cropperInstance.value) {
    const image = cropperInstance.value.getCropperImage()
    if (image) {
      image.src = url
    }
  }
}

/**
 * 启用裁剪
 */
const enable = () => {
  if (cropperInstance.value) {
    const canvas = cropperInstance.value.getCropperCanvas()
    if (canvas) {
      canvas.disabled = false
    }
  }
}

/**
 * 禁用裁剪
 */
const disable = () => {
  if (cropperInstance.value) {
    const canvas = cropperInstance.value.getCropperCanvas()
    if (canvas) {
      canvas.disabled = true
    }
  }
}

/**
 * 移动到指定位置
 */
const move = (x: number, y: number) => {
  if (cropperInstance.value) {
    const image = cropperInstance.value.getCropperImage()
    if (image) {
      image.$move(x, y)
    }
  }
}

/**
 * 移动到指定位置（绝对）
 */
const moveTo = (x: number, y: number) => {
  if (cropperInstance.value) {
    const image = cropperInstance.value.getCropperImage()
    if (image) {
      image.$moveTo(x, y)
    }
  }
}

/**
 * 缩放
 */
const scale = (scaleX: number, scaleY?: number) => {
  if (cropperInstance.value) {
    const image = cropperInstance.value.getCropperImage()
    if (image) {
      image.$scale(scaleX, scaleY)
    }
  }
}

/**
 * 旋转（度数）
 */
const rotate = (degree: number) => {
  if (cropperInstance.value) {
    const image = cropperInstance.value.getCropperImage()
    if (image) {
      image.$rotate(degree)
    }
  }
}

/**
 * 设置变换
 */
const setTransform = (transform: Record<string, unknown>) => {
  if (cropperInstance.value) {
    const image = cropperInstance.value.getCropperImage()
    if (image) {
      image.$setTransform(transform)
    }
  }
}

/**
 * 获取图片变换数据
 */
const getImageTransform = () => {
  if (!cropperInstance.value) {
    return null
  }
  const image = cropperInstance.value.getCropperImage()
  if (!image) {
    return null
  }
  return image.$getTransform()
}

/**
 * 设置裁剪框数据
 */
const setCropBoxData = (x: number, y: number, width: number, height: number) => {
  if (cropperInstance.value) {
    const selection = cropperInstance.value.getCropperSelection()
    if (selection) {
      selection.x = x
      selection.y = y
      selection.width = width
      selection.height = height
    }
  }
}

/**
 * 获取裁剪框数据
 */
const getCropBoxData = () => {
  if (!cropperInstance.value) {
    return null
  }
  const selection = cropperInstance.value.getCropperSelection()
  if (!selection) {
    return null
  }
  return {
    x: selection.x ?? 0,
    y: selection.y ?? 0,
    width: selection.width ?? 0,
    height: selection.height ?? 0
  }
}

// 暴露方法供父组件调用
defineExpose({
  // 核心裁剪方法
  getCroppedImage,
  getCroppedBlob,
  getCroppedDataUrl,
  getCroppedFile,
  getCroppedCanvas,
  crop,
  // 裁剪器控制方法
  reset,
  clear,
  replace,
  enable,
  disable,
  move,
  moveTo,
  scale,
  rotate,
  setTransform,
  // 数据获取和设置
  getImageTransform,
  getCropBoxData,
  setCropBoxData,
  // 原始裁剪器实例
  cropper: cropperInstance
})
</script>

<style scoped lang="css">
.takt-cropper {
  width: 100%;
  
  .takt-cropper-container {
    width: 100%;
    height: 100%;
    min-height: 400px;
    display: flex;
    justify-content: center;
    align-items: center;
    overflow: hidden;
    
    .takt-cropper-img {
      max-width: 100%;
      max-height: 100%;
      display: block;
    }
  }

  .takt-cropper-empty {
    display: flex;
    justify-content: center;
    align-items: center;
    min-height: 400px;
  }
}
</style>