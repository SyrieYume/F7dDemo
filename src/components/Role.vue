<script setup>
import { computed, reactive, useTemplateRef } from 'vue'
import { Ani } from '../utils'
import { ImageCache } from '../cache'

const state = reactive({ name: "", act: "", active: true })
 
const role = useTemplateRef("role")
const mask = useTemplateRef("mask")
let img1 = useTemplateRef("img1")
let img2 = useTemplateRef("img2")


const imgSrc = computed(() => {
    return (state.name != "" && state.act != "") ? 
        ImageCache.get(`/res/role/${state.name}-${state.act}.png`) : " "
})


async function animateTo(newState) {
    const aniOptions = { duration: 1200, easing: "ease", fill: "forwards" }

    if(newState.name != state.name) {
        if(newState.name == '') {
            await role.value.animate(Ani.fade(1, 0), aniOptions).finished
            Object.assign(state, newState)
        }

        else {
            Object.assign(state, newState)
            await role.value.animate(Ani.fade(0, 1), aniOptions).finished
        }
        
        return
    }

    if(newState.active != state.active) {
        state.active = newState.active
        const maskAni = newState.active ? Ani.fade(0.5, 0) : Ani.fade(0, 0.5)
        await mask.value.animate(maskAni, aniOptions).finished
    }

    if(newState.act != state.act) {
        img2.value.src = ImageCache.get(`/res/role/${newState.name}-${newState.act}.png`)

        await Promise.all([
            img1.value.animate(Ani.fade(1, 0), aniOptions).finished,
            img2.value.animate(Ani.fade(0, 1), aniOptions).finished
        ]);

        [img1, img2] = [img2, img1]
        state.act = newState.act
    }

}

defineExpose({ 
    'animateTo': animateTo
})
</script>


<template>
<div ref="role" class="role" v-show="state.name != ' '"> 
    <img ref="img1" :src="imgSrc" />
    <img ref="img2" :src="imgSrc" style="opacity: 0;" />
    <img ref="mask" class="mask" :src="imgSrc" style="opacity: 0;" />
</div>
</template>


<style scoped>
div.role {
    position: absolute;
    width: 100%;
    height: 100%;

    & > img {
        position: absolute;
        left: 50%;
        bottom: 0;
        transform: translateX(-50%);
        height: calc(var(--window-scale) * 720px);
        clip-path: polygon(12% 100%, 12% 0, 100% 0, 100% 100%);
        object-fit: cover;
        object-position: center bottom;
        filter: url(#black-to-transparent);
    }

    & > img.mask {
        opacity: 50%;
        filter: url(#black-to-transparent) brightness(0%);
    }
}
</style>