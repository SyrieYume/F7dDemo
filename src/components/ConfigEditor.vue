<script setup>
import { onMounted, ref, useTemplateRef } from 'vue';
import Resizeble from './Resizeble.vue';
import hljs from 'highlight.js/lib/core';
import hljsYaml from 'highlight.js/lib/languages/yaml';
import 'highlight.js/styles/github.css'
import yaml from "js-yaml";
import { Global } from '../global';

hljs.registerLanguage("yaml", hljsYaml)

const highlightElement = useTemplateRef("highlightElement")
const content = ref("")

const updateHighlight = () => {
    highlightElement.value.innerHTML = content.value
    highlightElement.value.dataset.highlighted = ""
    hljs.highlightElement(highlightElement.value)
}

onMounted(() => {
    content.value = yaml.dump(Global.config)
    updateHighlight()
})
</script>


<template>
<Resizeble class="configEditor">
    <textarea rows="12" @input="updateHighlight" v-model="content"></textarea>
    <pre><code class="language-yaml" ref="highlightElement"></code></pre>
    <div class="buttons">
        <button @click="Global.setConfig(yaml.load(content));$emit('dismiss')" style="background-color:#EE7785;">保存</button>
        <button @click="$emit('dismiss')" style="background-color:#c0c0c0;">取消</button>
    </div>
</Resizeble>
</template>


<style scoped>
.configEditor {
    display: flex;
    justify-content:space-around;
    flex-direction: column;
    background-color: white;

    & > textarea {
        height: 70%;
        margin: 4%;
        color:transparent;
        caret-color: #39c5bccc;
        border: 2px dashed #ffffff00;
    }

    & > pre {
        height: 70%;
        margin: 4%;
        overflow: hidden;
        border: 2px dashed #39C5BB80;
        pointer-events: none;
    }

    & > code {
        height: 100%;
        overflow: hidden;
    }

    & > textarea, & > pre, & code {
        display: block;
        position: absolute;
        background: none;
        width: 92%;
        top: 0;
        left: 0;
        padding: calc(var(--window-scale) * 24px);
        font-size: calc(var(--window-scale) * 20px);
        box-sizing: border-box;
        border-radius: calc(var(--window-scale) * 10px);
        resize: none;
        outline: none;
        user-select: all;
    }

    & > .buttons {
        width: 100%;
        display: flex;
        justify-content: center;
        position: absolute;
        bottom: 5%;
    }

    & button {
        font-size: calc(var(--window-scale) * 20px);
        padding: calc(var(--window-scale) * 8px) calc(var(--window-scale) * 14px);
        border-radius: calc(var(--window-scale) * 3px);
        margin: 0 5%;
        outline: none;
        border: none;
    }
}
</style>