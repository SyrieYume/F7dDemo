<!-- 章节选择页面 -->
<script setup>
import { ref, inject, onMounted, onUnmounted } from 'vue';
import PageItem from '../components/PageItem.vue';
import Draggable from '../components/Draggable.vue';
import { Global } from '../global';
import { audioSwitch, loadYaml } from '../utils';

const background = inject("background")
const bgmPlayer = inject("bgm")

let route = []
const currentPage = ref({})


const gotoPage = page => {
    if(!page.list) { 
        Global.gotoView("GameView", { route: route, script: page.script })
        return
    }
    route.push(page)
    currentPage.value = page
}


function backPage() {
    if(route.length == 1)
        Global.gotoView("StartView")
    else 
        route.pop()
    currentPage.value = route[route.length - 1]
}


onMounted(async () => {
    const datas = await loadYaml("/res/pages.yaml")
    route = Global.viewArgs ? Global.viewArgs.route : [datas]
    currentPage.value = route[route.length - 1]
    
    audioSwitch(bgmPlayer.value, "/res/bgm/记忆之扉.mp3")
    await background.value.fadeIn("/res/ui/bg3.jpg")
})


onUnmounted(async () => {
    await background.value.fadeOut()
})

</script>


<template>
<div class="view">
    <div class="top-bar">
        <img class="button" @click="backPage" src="/res/icon/back.png" />
        <p class="title">{{ currentPage.name }}</p>
    </div>

    <div v-if="currentPage.type == 'type1'" :class="['container', currentPage.type]">
        <PageItem
            v-for="(page, i) in currentPage.list"
            :key="`${currentPage.name}${i}`"
            :cover="page.cover"
            @click.prevent="gotoPage(page)" />

        <PageItem locked
            v-for="i in Math.max(0, 6 - currentPage.list.length)"
            :key="`${currentPage.name}${i + currentPage.list.length}`"/>
    </div>

    <Draggable v-else :class="['container', currentPage.type]">
        <PageItem
            v-for="(page, i) in currentPage.list"
            :cover="page.cover"
            :title="page.name"
            :line="i < currentPage.list.length - 1"
            @click.prevent="gotoPage(page)" />
    </Draggable>
</div>
</template>


<style scoped>
div.top-bar > p.title {
    font-size: calc(var(--window-scale) * 24px);
    color: #ffffff;
}

div.container {
    position: absolute;
    left: 50%;
    top: 50%;
    transform: translate(-50%, -50%);
    padding: 0 calc(var(--window-scale) * 40px);
}

div.container.type1 {
    display: grid;
    grid-template-columns: repeat(3, 1fr); 
    grid-template-rows: repeat(2, 1fr);
    gap: calc(var(--window-scale) * 20px); 
    width: calc(var(--window-scale) * 1280px);
    height: calc(var(--window-scale) * 560px);
}

div.container.type2 {
    display: flex;
    align-items: center;
    flex-shrink: 0;
    width: 100%;
    height: calc(var(--window-scale) * 600px);
    overflow-x: scroll;
    
    & > * {
        display: inline-flex;
        flex: 0 0 calc(var(--window-scale) * 300px);
        height: calc(var(--window-scale) * 210px);
    }
}

</style>