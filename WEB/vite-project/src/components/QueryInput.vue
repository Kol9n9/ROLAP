<template>
    <div>
        <n-card title="Cube request">
            <n-input v-model:value="queryString"/>
            <n-button @click="$emit('search',queryString)">Search</n-button>
        </n-card>
    </div>
    
</template>
<script lang="ts" setup>
import { ref, defineEmits } from 'vue';
import {NButton, NInput, NCard} from 'naive-ui';
    const emit = defineEmits(["search"]);
    const queryString = ref('SELECT {[Страна].[Россия],[Страна].[Казахстан]} ON 0, CROSSJOIN({[ОКВЭД].[01 Растениеводство и животноводство],[ОКВЭД].[06 Добыча сырой нефти и природного газа]},{[Measure].[Прибыль],[Measure].[Расход]}) ON 1 FROM [example1]');
</script>
<style>
</style>