<template>
    <table class="pivot">
        <PivotHeader :Columns="pivotData.columns" />
        <PivotData :Columns="pivotData.columns[pivotData.columns.length - 1]" :Rows="pivotData.rows" :Data="pivotData.data" />
    </table>
</template>
<script lang="ts" setup>

import { defineProps, watch, ref, reactive } from 'vue'
import axios from 'axios';

import parseMDX from '../../utils/parserMdx';
import { getPivotHeaders } from '../../utils/generatePivotData';
import PivotHeader from './PivotHeader.vue';
import PivotData from './PivotData.vue';


const props = defineProps({
    QueryString: {
        type: String,
        required: true
    },
    Api: {
        type: String,
        required: true
    }
})

watch(()=>props.QueryString, async (query: String)=>{
    Object.assign(pivotData,{
            columns: [],
            rows: [],
            data: []
        })
    try{
        const res = await axios.get<MdxDataModel>(props.Api + query);

        const mdxData = parseMDX(res.data);
        const mdxHeaders = getPivotHeaders(mdxData.Columns,mdxData.Rows);
        Object.assign(pivotData,{
            columns: mdxHeaders.columns,
            rows: mdxHeaders.rows,
            data: mdxData.Values
        })

    } catch(e: any){
        alert(e.response.data);
    }
})



import { HeaderTr, MdxDataModel,ValueModel } from '../../model/models';

type PivotData = {
    columns: HeaderTr[],
    rows: HeaderTr[],
    data: ValueModel[]
}

const pivotData: PivotData = reactive({
    columns: [],
    rows: [],
    data: []
})

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