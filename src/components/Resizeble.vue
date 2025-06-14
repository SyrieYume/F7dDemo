<!-- 可以调整大小的Div -->

<script setup>
import { useTemplateRef } from 'vue'

const props = defineProps({
    minWidth  : { type: Number, default: 40 },
    minHeight : { type: Number, default: 40 },
    keepAspectRatio : { type: Boolean, default: false },
    directions: { type: String, default: "right bottom right-bottom" }
})

const targetElement = useTemplateRef("targetElement")

function startResize(event) {
    const startX = event.clientX
    const startY = event.clientY
    const startWidth = targetElement.value.offsetWidth
    const startHeight = targetElement.value.offsetHeight
    const aspectRatio = startWidth / startHeight
    const isResizeWidth = event.target.className.includes("right")
    const isResizeHeight = event.target.className.includes("bottom")

    function resize(resizeEvent) {
        const dx = resizeEvent.clientX - startX
        const dy = resizeEvent.clientY - startY

        let newWidth = startWidth + (isResizeWidth ? dx: 0)
        let newHeight = startHeight + (isResizeHeight ? dy: 0)

        if(newWidth < props.minWidth || newHeight < props.minHeight)
            return

        if(props.keepAspectRatio) {
            if(isResizeWidth)
                newHeight = newWidth / aspectRatio
            else if(isResizeHeight)
                newWidth = newHeight * aspectRatio
        }

        targetElement.value.style.width = `${newWidth}px`
        targetElement.value.style.height = `${newHeight}px`
    }

    function stopResize() {
        document.removeEventListener("mousemove", resize)
        document.removeEventListener("mouseup", stopResize)
    }

    document.addEventListener("mousemove", resize)
    document.addEventListener("mouseup", stopResize)
}
</script>

<template>
<div class="resizeble-div" ref="targetElement">
<slot />
<div class="resize-handles">
    <div 
        v-for="d in directions.split(' ')" 
        :class="['resize-handle', d]" 
        @mousedown.prevent="startResize">
    </div>
</div>
</div>

</template>

<style scoped>
.resizeble-div {
    position: relative;
    width: 100%;
    height: 100%;
}

.resize-handle {
    position: absolute;
    width: 16px;
    height: 16px;
}

.right-bottom {
    bottom: -8px;
    right: -8px;
    cursor: se-resize;
}

.right {
    top: 15%;
    right: -8px;
    height: 70%;
    cursor: e-resize;
}

.bottom {
    left: 15%;
    bottom: -8px;
    width: 70%;
    cursor: s-resize;
}
</style>