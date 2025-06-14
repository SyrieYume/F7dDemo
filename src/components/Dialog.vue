<script setup>
import { computed, reactive, useTemplateRef } from 'vue';
import { Ani, setIntervalAsync } from '../utils';
import { Global } from '../global';

const dialog = useTemplateRef("dialog")

const state = reactive({
    role: "",
    text: "",
    dialogType: 0,
    clearText: false
})

let textToAdd = ""


const getDialogType = (role) => {
    switch(role) {
        case "": return 0;
        case "旁白": return 1;
        case "指挥使": return 2;
        default: return 3;
    }
}


const displayName = computed(() => {
    switch(state.role) {
        case "": case "旁白":  return ""; 
        case "指挥使": return Global.config.playerName; 
        default: return state.role
    }
})


const dialogImgSrc = computed(() => {
    if(state.dialogType == 0) return " "
    else return `/res/ui/dialog${state.dialogType}.png`
})


async function animateTo(newState) {    
    const aniOptions = { duration: 750, easing: "ease", fill: "forwards" }

    if(state.role != newState.role) {
        const newDialogType = getDialogType(newState.role)
        const leaveAni = Ani.fade(1, 0)
        const enterAni = newDialogType > 1 ? Ani.blend(Ani.fade(0, 1), Ani.scale(0.9, 1)) : Ani.fade(0, 1)
        
        if(state.dialogType > 0)
            await dialog.value.animate(leaveAni, aniOptions).finished

        state.dialogType = newDialogType
        state.role = newState.role
        state.text = ""

        if(newDialogType > 0)
            await dialog.value.animate(enterAni, aniOptions).finished
    }

    if(state.clearText) {
        state.text = ""
        state.clearText = false
    }

    textToAdd = newState.text.trim()
        .replaceAll("\\n", "\n")
        .replaceAll("<PlayerName>", Global.config.playerName)

    if(textToAdd.endsWith("<Clear>")) {
        state.clearText = true
        textToAdd = textToAdd.substring(0, textToAdd.length - 7)
    }
}

async function typeText(condition) {
    let i = 0
    await setIntervalAsync(
        () => state.text += textToAdd[i++], 50, 
        () => i < textToAdd.length && condition()
    )

    if(i < textToAdd.length)
        state.text += textToAdd.substring(i, textToAdd.length)
}


defineExpose({ 
    'animateTo': animateTo,
    'typeText': typeText
})

</script>

<template>
<div class="dialog" ref="dialog" v-show="state.dialogType > 0">
    <img ref="background" class="background" :src="dialogImgSrc" />
    <p ref="name" class="name">{{ displayName }}</p>
    <pre class="text">{{ state.text }}</pre>
</div>
</template>

<style scoped>
div.dialog {
    position: absolute;
    bottom: 0;
    left: 0;
    right: 0;
    margin: 0 auto;
    width: calc(var(--window-scale) * 1280px);
    height: calc(var(--window-scale) * 266px);
    z-index: 10;

    & > p.name {
        position: absolute;
        left: 17.5%;
        top: 12.05%;
        color: #ffffff;
        font-size: calc(var(--window-scale) * 32px);
    }

    & > pre.text {
        position: absolute;
        left: 17.8%;
        top: 30%;
        width: 64%;
        height: 55%;
        color: #373942;
        font-family: "BigYoungBold";
        font-weight: bold;
        font-size: calc(var(--window-scale) * 32px);
        line-height: calc(var(--window-scale) * 45px);
        white-space: pre-wrap;
    }
}
</style>