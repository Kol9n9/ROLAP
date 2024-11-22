<script setup lang="ts">

import { ref }from 'vue';

import Pivot from './components/Pivot/Pivot.vue';
import QueryInput from './components/QueryInput.vue';

const queryString = ref("");

</script>

<template>

    <div>
        <QueryInput @search="(val)=>queryString=val"/>
        <Pivot :QueryString="queryString" Api="http://localhost:5032/api/cube?query="/>
    </div>

</template>
 
<style scoped>

</style>
