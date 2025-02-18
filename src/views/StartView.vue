<!-- 开始页面 -->
<script setup>
import { inject, onMounted, onUnmounted, useTemplateRef } from 'vue';
import { Global } from '../global';
import { audioSwitch } from '../utils';

const background = inject("background")
const bgmPlayer = inject("bgm")
const view = useTemplateRef("view")

audioSwitch(bgmPlayer.value, "")

onMounted(async () => {
    await background.value.fadeIn("/res/bg/cg000.png")
    view.value.addEventListener("click", () => Global.gotoView("PagesView"))
})

onUnmounted(async () => {
    await background.value.fadeOut()
})
</script>

<template>
<div class="view" ref="view">
    <p>点击屏幕开始游戏</p>
</div>
</template>

<style scoped>
div.view {
    & > p {
        display: block;
        position: absolute;
        left: 50%;
        bottom: 10%;
        transform: translateX(-50%);
        color: #ffffff;
        font-size: calc(var(--window-scale) * 24px);
    }
}

div.menu {
    position: absolute;
    left: 1.5%;
    top: 50%;
    width: 30%;
    transform: translateY(-50%);
}

div.menu .button {
    display: block;
    width: fit-content;
    font-size: calc(var(--window-scale) * 40px);
    margin: 3% 4%;
    filter: drop-shadow(2px 1px 4px #00000040);

    transition: all .7s ease;
    &:hover { 
        color: aliceblue; 
        filter: drop-shadow(2px 1px 4px #ffffff80);
    }
}
</style>