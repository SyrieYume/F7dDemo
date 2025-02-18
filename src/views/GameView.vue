<script setup>
import { inject, onMounted, onUnmounted, reactive, useTemplateRef } from 'vue';
import Dialog from '../components/Dialog.vue';
import Roles from '../components/Roles.vue';
import { Global } from '../global';
import { audioSwitch, loadYaml, playAudio, setAudio, sleep, waitUntil } from '../utils';
import { ImageCache } from '../cache';

// 游戏状态
let gameState = reactive({
    bg: "",
    bgm: "",
    audio: "",
    audioStart: 0,
    audioEnd: 0,
    roles: [],
    role: "",
    text: "",
    autoPlay: false,
    clicked: false
} )

// 页面中的UI元素
const background = inject("background")
const bgmPlayer = inject("bgm")
const audioPlayer = inject("audio")

const view = useTemplateRef("view")
const roles = useTemplateRef("roles")
const dialog = useTemplateRef("dialog")


// 播放剧情脚本
const playScript = async (script) => {
    for(const newState of script) {
        if('bg' in newState) {
            gameState.role = ""
            gameState.roles = []
            gameState.text = ""
            await Promise.all([roles.value.animateTo(gameState), dialog.value.animateTo(gameState)])
            if(gameState.bg != "")
                await background.value.fadeOut()
            await background.value.fadeIn(`/res/bg/${newState.bg}`)
        }

        Object.assign(gameState, newState)

        if('bgm' in newState) 
            await audioSwitch(bgmPlayer.value, `/res/bgm/${gameState.bgm}`)

        if('audio' in newState)
            await setAudio(audioPlayer.value, `/res/audio/${gameState.audio}`)
        
        roles.value.animateTo(gameState)
        
        if('text' in newState) {
            await dialog.value.animateTo(gameState)
            gameState.clicked = false

            const condition = () => !gameState.clicked || gameState.autoPlay
            const promises = [ dialog.value.typeText(condition) ]

            if('audioStart' in newState && 'audioEnd' in newState) {
                promises.push(
                    playAudio(audioPlayer.value, gameState.audioStart, gameState.audioEnd, condition)
                    .then(() => sleep(500))
                )
            }
            else {
                if(gameState.autoPlay)
                    promises.push(sleep(gameState.text.length *  150 + 1000))
                else
                    promises.push(waitUntil(() => gameState.clicked || gameState.autoPlay))
            }
            
            await Promise.all(promises)
            
            if(!gameState.autoPlay)
                await waitUntil(() => gameState.clicked || gameState.autoPlay)
        }
    }
}


onMounted(async () => {
    // 监听键盘事件
    view.value.addEventListener("keyup", (e) => {
        if(e.key == 'k' || e.key == 'K')
            gameState.autoPlay = !gameState.autoPlay
        if(e.key == "Escape")
            Global.gotoView('PagesView', Global.viewArgs)
    })
    view.value.addEventListener("click", () => gameState.clicked = true)
    view.value.focus()

    if("script" in Global.viewArgs) {
        const scriptName = Global.viewArgs.script
        const script = await loadYaml(`/res/script/${scriptName}`)
        await ImageCache.preload( script.filter(s => "roles" in s).flatMap(s => s.roles).map(role => `/res/role/${role.name}-${role.act}.png`))
        playScript(script)
    }
})

onUnmounted(async () => {
    ImageCache.clearAll()
    await background.value.fadeOut()
})

</script>

<template>
<div class="view" ref="view" tabindex="1">
    <Roles ref="roles" />

    <Dialog ref="dialog" />

    <div class="top-bar">
        <img class="button" @click="Global.gotoView('PagesView', Global.viewArgs)" src="/res/icon/back.png" />
        <p class="autoplay" v-show="gameState.autoPlay">自动播放中...</p>
    </div>

</div>
</template>

<style scoped>
div.top-bar {
    position: absolute;

    & > p.autoplay {
        position: absolute;
        right: 0;
        bottom: 0;
        margin-right: calc(var(--window-scale) * 50px);
        color: #ffffff;
        font-size: calc(var(--window-scale) * 20px);
    }
}
</style>