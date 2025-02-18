<!-- 设置页面 -->
<script setup>
import { Global } from '../global';
</script>

<template>
<div class="view">
    <img class="background" src="/res/ui/bg2.jpg"/>
    <div class="top-bar">
        <img class="button" @click="Global.gotoView('StartView')" src="/res/icon/back.png" />
    </div>
</div>
</template>

<style scoped>
div.top-bar {
    background-image: url("/res/ui/topbar.jpg");
    background-repeat: no-repeat;
    background-position: left top;
    background-size: auto 100%;
    background-color: #09080e;
}

</style>