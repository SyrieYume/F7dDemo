<script setup>
import { ref, useTemplateRef } from 'vue'
import { Ani } from '../utils'

const bg = ref("")
const blackscreen = useTemplateRef("blackscreen")

async function fadeOut() {
    blackscreen.value.style.display = "block"
    await blackscreen.value.animate(Ani.fade(0, 1), { duration: 1000, easing: "ease", fill: "forwards" }).finished
}

async function fadeIn(newBg) {
    bg.value = newBg
    await blackscreen.value.animate(Ani.fade(1, 0), { duration: 1000, easing: "ease", fill: "forwards" }).finished
    blackscreen.value.style.display = "none"
}

defineExpose({ fadeIn, fadeOut })
</script>

<template>
<img class="background" :src="`${bg}`" />
<div ref="blackscreen" class="blackscreen"></div>
</template>

<style scoped>
div.blackscreen {
    position: absolute;
    width: 100%;
    height: 100%;
    background-color: black;
    z-index: 90;
}
</style>