<!-- 可以拖动的Div -->

<script setup>
import { useTemplateRef } from 'vue'

const props = defineProps({
    direction: { type: String, default: "horizontal" },
    sensitivity: { type: Number, default: 0.75 }
})

let isDragging = false
let startX = 0, startY = 0
let scrollLeft = 0, scrollTop = 0
const container = useTemplateRef("container")


function startDrag(e) {
    isDragging = true

    // 处理触摸事件和鼠标事件的坐标差异
    startX = e.touches ? e.touches[0].clientX : e.clientX
    startY = e.touches ? e.touches[0].clientY : e.clientY
    scrollLeft = container.value.scrollLeft
    scrollTop = container.value.scrollTop
}


function onDrag(e) {
    if (!isDragging) 
        return

    e.preventDefault()

    const clientX = e.touches ? e.touches[0].clientX : e.clientX
    const clientY = e.touches ? e.touches[0].clientY : e.clientY
    const walkX = (clientX - startX) * props.sensitivity
    const walkY = (clientY - startY) * props.sensitivity
    if(props.direction == "horizontal")
        container.value.scrollLeft = scrollLeft - walkX
    else if(props.direction == "vertical")
        container.value.scrollTop = scrollTop - walkY
}

const endDrag = () => isDragging = false
</script>


<template>
<div 
    ref="container"
    class="draggable"
    @mousedown="startDrag"
    @touchstart="startDrag"
    @mousemove="onDrag"
    @touchmove="onDrag"
    @mouseup="endDrag"
    @mouseleave="endDrag"
    @touchend="endDrag">
    <slot />
</div>
</template>

<style scoped>

</style>