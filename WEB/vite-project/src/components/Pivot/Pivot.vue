<template>
    <table class="pivot">
        <PivotHeader :Columns="columns" />
        <PivotData :Columns="columns[columns.length - 1]" :Rows="rows" :Data="data.Values" />
    </table>
</template>
<script lang="ts" setup>

import parseMDX from '../../utils/parserMdx';
import { getPivotHeaders } from '../../utils/generatePivotData';
import PivotHeader from './PivotHeader.vue';
import PivotData from './PivotData.vue';

import cubeData from '../../data/testCube.json' with {type: 'json'}
const data = parseMDX(cubeData);
const { columns, rows } = getPivotHeaders(data.Columns, data.Rows);


</script>
<style>
.pivot {
    border-collapse: collapse;
}

.pivot th,
.pivot td {
    padding: 8px;
    text-align: center;
    border: 1px solid #ddd;
}

.pivot th {
    background-color: #f4f4f4;
}

.pivot .row-header {
    font-weight: bold;
    text-align: left;
}

.pivot .aggregation {
    font-weight: bold;
    background-color: #e8e8e8;
}
</style>