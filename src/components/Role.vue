<script setup>
import { useTemplateRef } from 'vue'
import { Ani, setImage } from '../utils'
import { ImageCache } from '../cache'

const state = { name: "", act: "", active: true }
 
const role = useTemplateRef("role")
const mask = useTemplateRef("mask")
let img1 = useTemplateRef("img1")
let img2 = useTemplateRef("img2")


async function animateTo(newState) {
    const aniOptions = { duration: 1200, easing: "ease", fill: "forwards" }

    if(newState.name != state.name) {
        if(newState.name == '')
            await role.value.animate(Ani.fade(1, 0), aniOptions).finished

        else {
            const roleImage = ImageCache.get(`/res/role/${newState.name}-${newState.act}.png`)
            await setImage(img1.value, roleImage)
            await setImage(mask.value, roleImage)
            mask.value.style.opacity = newState.active ? 0 : 0.5
            await role.value.animate(Ani.fade(0, 1), aniOptions).finished
        }
        
        Object.assign(state, newState)
        return
    }

    if(newState.active != state.active) {
        const maskAni = newState.active ? Ani.fade(0.5, 0) : Ani.fade(0, 0.5)
        await mask.value.animate(maskAni, aniOptions).finished
        state.active = newState.active
    }

    if(newState.act != state.act) {
        const roleImage = ImageCache.get(`/res/role/${newState.name}-${newState.act}.png`)
        await setImage(img2.value, roleImage)
        await setImage(mask.value, roleImage)

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
<div ref="role" class="role"> 
    <img ref="img1" />
    <img ref="img2" style="opacity: 0;" />
    <img ref="mask" class="mask" style="opacity: 0;" />
</div>
</template>


<style scoped>
div.role {
    position: absolute;
    width: 100%;
    height: 100%;
    opacity: 0;

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