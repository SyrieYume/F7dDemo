<script setup>
import { reactive, useTemplateRef } from 'vue'
import Role from './Role.vue';

const role1 = useTemplateRef("role1")
const role2 = useTemplateRef("role2")
const roles = [role1 , role2]

const states = reactive([{ name: "", act: "", active: true }, { name: "", act: "", active: true }])

async function animateTo(newState) {
  const newRoles = newState.roles

  for(let i = 0; i < roles.length; i++) {
    states[i] = i < newRoles.length ? 
      { name: newRoles[i].name, act: newRoles[i].act, active: newState.role == newRoles[i].name }:
      { name: "", act: "", active: true }

    roles[i].value.animateTo(states[i])
  }

}

defineExpose({ 
    'animateTo': animateTo
})
</script>



<template>
<svg height="0" width="0" style="display:none;position:absolute;">
  <defs>
    <!-- 滤镜：对图像中所有像素(r,g,b,a,1)进行矩阵运算，使图片中的纯黑色像素变为全透明像素 -->
    <filter id="black-to-transparent">
        <feColorMatrix in="SourceGraphic" result="result1"
            type="matrix" 
            values="1 0 0 0 0
                    0 1 0 0 0
                    0 0 1 0 0
                    500 500 500 0 0"/>
    </filter>
  </defs>
</svg> 
  
<div class="roles" :class="{ two_roles: states[1].name != '' }">
    <Role ref="role1" />
    <Role ref="role2" />
</div>
</template>  



<style scoped>
div.roles {
  & > * {
    transition: transform .8s ease;
  }

  &.two_roles > *:first-child {
    transform: translateX(-20%);
  }

  &.two_roles > *:nth-child(2) {
    transform: translateX(20%);
  }
}
</style>