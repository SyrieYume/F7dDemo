<!-- 开始页面 -->
<script setup>
import { inject, onMounted, onUnmounted, ref } from 'vue';
import { Global } from '../global';
import { audioSwitch } from '../utils';
import ConfigEditor from '../components/ConfigEditor.vue';
import SmallButon from '../components/SmallButon.vue';

const background = inject("background")
const bgmPlayer = inject("bgm")

const isDisplayConfig = ref(false)


onMounted(async () => {
    audioSwitch(bgmPlayer.value, "")
    await background.value.fadeIn("/res/bg/cg000.png")
})

onUnmounted(async () => {
    await background.value.fadeOut()
})
</script>

<template>
<div class="view" ref="view">
    <div 
        style="width: 100%; height: 100%;"
        @click="Global.gotoView('PagesView')">
    </div>

    <p>点击屏幕开始游戏</p>

    <SmallButon 
        class="configButton" 
        iconSrc="/res/icon/settings.svg" 
        text="设置" 
        @click="isDisplayConfig = true"/>

    <ConfigEditor 
        class="configEditor"
        v-if="isDisplayConfig" 
        @dismiss="isDisplayConfig = false" />
</div>
</template>

<style scoped>
@keyframes blink {
  0% { opacity: 1; }
  50% { opacity: 0.5; }
  100% { opacity: 1; }
}

div.view {
    & > p {
        display: block;
        position: absolute;
        left: 50%;
        bottom: 10%;
        transform: translateX(-50%);
        color: #ffffff;
        font-size: calc(var(--window-scale) * 24px);
        animation: blink 3s ease infinite;
    }

}

.configButton {
    position: absolute;
    right: 6%;
    top: 6%;
}

.configEditor {
    position: absolute;
    left: 50%;
    top: 50%;
    transform: translate(-50%, -50%);
    width: 60%;
    height: 60%;
    border-radius: calc(var(--window-scale) * 20px);
    box-shadow: rgba(0, 0, 0, 0.1) 0px 4px 12px;
}
</style>