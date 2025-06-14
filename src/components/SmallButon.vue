<script setup>
const props = defineProps({
    iconSrc: { type: String, default: "" },
    text: { type: String, default: "" },
})
</script>

<template>
<div class="button">
    <div></div>
    <img :src="props.iconSrc" />
    <b>{{ props.text }}</b>
</div>
</template>

<style scoped>
.button {
    display: flex;
    align-items: center;
    justify-content: center;
    position: relative;
    width: calc(var(--window-scale) * 75px);
    height: calc(var(--window-scale) * 75px);

    &:hover {
        & > div { background-color: #5882b490; }
    }

    & > div {
        position: absolute;
        width: 100%;
        height: 100%;
        background-color: #848593a0;
        clip-path: polygon(5% 25%, 50% 0, 95% 25%, 95% 75%, 50% 100%, 5% 75%);
        transition: background-color .7s ease;
    }

    & > img {
        position: absolute;
        width: 60%;
        height: 60%;
    }

    & > b {
        position: absolute;
        bottom: -10%;
        color: #d3deea;
        font-size: calc(var(--window-scale) * 24px);
    }
}
</style>