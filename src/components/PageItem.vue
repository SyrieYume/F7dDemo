<script setup>
const props = defineProps({
    cover: { type: String, default: "" },
    title: { type: String, default: "" },
    locked: { type: Boolean, default: false },
    line: { type: Boolean, default: false },
})
</script>

<template>
<div :class="['page-item', locked ? 'lock' : 'normal']">
    <img v-if="locked" src="/res/icon/lock.svg" />
    <img v-else :src="cover" />
    <b v-if="locked">敬请期待</b>
    <p v-if="title.length > 0">{{ title }}</p>
    <div v-if="line" class="line"></div>
</div>
</template>

<style scoped>
div.page-item {
    position: relative;
    transform: translate(20%, -5%);
    opacity: 0; 
    animation: 1.5s ease 0.5s page-item-ani;
    animation-fill-mode: forwards;

    & > .line {
        position: absolute;
        top: 45%;
        left: 89%;
        width: 21%;
        height: calc(var(--window-scale) * 2px);
        background-color: #ffffff60;
        z-index: -1;
    }

    & > p {
        font-size: calc(var(--window-scale) * 24px);
        color: #e0e0e0;
        padding-right: 10%;
    }
}

div.page-item.normal {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    filter: drop-shadow(0px 0px calc(var(--window-scale) * 10px) rgba(195, 115, 126, 0.8));
    cursor: pointer;

    & > img {
        width: 100%;
    }

    transition: filter .7s ease;
    &:hover {
        filter: drop-shadow(0px 0px calc(var(--window-scale) * 45px) rgba(195, 115, 126, 0.8));
    }
}

div.page-item.lock {
    display: flex;
    position: relative;
    align-items: center;
    justify-content: center;

    & ::before {
        content: "";
        position: absolute;
        width: 100%;
        height: 100%;
        left: 0;
        top: 0;
        clip-path: polygon(20% 15%, 100% 15%, 80% 85%, 0% 85%);
        background-color: #68445660;
    }

    & > img {
        position: absolute;
        width: 16%;
        z-index: 0;
    }

    & > b {
        color: #917889a0;
        font-size: calc(var(--window-scale) * 20px); 
        z-index: 1;
    }
}

@keyframes page-item-ani {
    from { 
        transform: translate(12%, -2%);
        opacity: 0; 
    }
    to {
        transform: translate(0, 0); 
        opacity: 1;
    }
}
</style>