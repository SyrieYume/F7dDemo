<script setup>
import { defineAsyncComponent, onMounted, useTemplateRef, provide } from 'vue';
import { currentView } from './global';
import { debounce } from './utils';
import Background from './components/Background.vue';

const background = useTemplateRef("background")
const bgmPlayer = useTemplateRef("bgm")
const audioPlayer = useTemplateRef("audio")

provide("background", background)
provide("bgm", bgmPlayer)
provide("audio", audioPlayer)

const views = {
  "StartView": defineAsyncComponent(() => import("./views/StartView.vue")), 
  "PagesView": defineAsyncComponent(() => import("./views/PagesView.vue")), 
  "GameView": defineAsyncComponent(() => import("./views/GameView.vue")), 
}

// 根据页面大小计算css中的 --window-scale 值
const adjustWindowScale = () => {
  const width = document.body.clientWidth
  const height = document.body.clientHeight
  const scale = (width / height > 16 / 9) ? height / 720 : width / 1280
  document.body.style.setProperty("--window-scale", scale)
}

onMounted(() => {
  window.addEventListener("resize", debounce(adjustWindowScale, 100))
  adjustWindowScale()
  bgmPlayer.value.volume = 0.6
})

</script>

<template>
  <Background ref="background" />
  <audio ref="bgm" loop style="position: absolute;"></audio>
  <audio ref="audio" style="position: absolute;"></audio>
  <component :is="views[currentView]" />
</template>
