// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/components/business/TaktFlowAntDesigner/config
// 文件名称：taktFlowZoom.ts
// 创建时间：2026-04-07
// 创建人：Takt365(Cursor AI)
// 功能描述：设计器画布缩放、滚轮缩放与拖拽平移
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

let scale = 1
let callback: ((percent: number) => void) | null = null
let translateX = 0
let translateY = 0
let isDragging = false
let startX = 0
let startY = 0
let target: HTMLElement | null = null
let wrapper: HTMLElement | null = null

function updateTransform() {
  if (!target) return
  target.style.transform = `translate(${translateX}px, ${translateY}px) scale(${scale})`
}

export function zoomInit(
  wrapperRef: { value: HTMLElement | null },
  targetRef: { value: HTMLElement | null },
  cb: (percent: number) => void
) {
  callback = cb
  scale = 1
  translateX = 0
  translateY = 0
  wrapper = wrapperRef.value
  target = targetRef.value
  if (!wrapper || !target) return

  /** 局部固定引用，避免闭包内对 `let target` 推断为可能为 null（TS18047） */
  const canvasEl = target

  wrapper.addEventListener('wheel', (event) => {
    event.preventDefault()
    const rect = canvasEl.getBoundingClientRect()
    const offsetX = event.clientX - rect.left
    const offsetY = event.clientY - rect.top
    const originX = offsetX / rect.width
    const originY = offsetY / rect.height
    wheelZoomFunc({ scaleFactor: event.deltaY, originX, originY })
  }, { passive: false })

  wrapper.addEventListener('mousedown', (event) => {
    isDragging = true
    startX = event.clientX - translateX
    startY = event.clientY - translateY
    canvasEl.style.cursor = 'grabbing'
  })

  wrapper.addEventListener('mousemove', (event) => {
    if (!isDragging) return
    translateX = event.clientX - startX
    translateY = event.clientY - startY
    updateTransform()
  })

  wrapper.addEventListener('mouseup', () => {
    isDragging = false
    canvasEl.style.cursor = 'grab'
  })

  canvasEl.style.cursor = 'grab'
  updateTransform()
}

export function resetImage() {
  scale = 1
  translateX = 0
  translateY = 0
  isDragging = false
  if (target) {
    target.style.transform = 'translate3d(0px, 0px, 0px) scale(1)'
    target.style.cursor = 'grab'
  }
  callback?.(100)
}

export function wheelZoomFunc({
  scaleFactor,
  originX = 0.5,
  originY = 0.5,
  isExternalCall = false
}: {
  scaleFactor: number
  originX?: number
  originY?: number
  isExternalCall?: boolean
}) {
  const ratio = 1.1
  const deltaRatio = scaleFactor > 0 ? 1 / ratio : ratio
  if (!target) return

  const rect = target.getBoundingClientRect()
  const newScale = isExternalCall ? scaleFactor : scale * deltaRatio
  const clampedScale = Math.max(0.5, Math.min(newScale, 5))
  const deltaScale = clampedScale / scale

  translateX -= (originX - 0.5) * rect.width * (deltaScale - 1)
  translateY -= (originY - 0.5) * rect.height * (deltaScale - 1)
  scale = clampedScale
  callback?.(Math.round(scale * 100))
  updateTransform()
}
