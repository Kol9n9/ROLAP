<template>
    <table class="pivot">
        <PivotHeader :Columns="pivotData.columns" />
        <PivotData :Columns="pivotColumns!" :Rows="pivotData.rows" :Data="pivotData.data" />
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

const pivotColumns = ref<HeaderTr>();

watch(()=>props.QueryString, async (query: String)=>{
    Object.assign(pivotData,{
            columns: [],
            rows: [],
            data: []
        })
    try{
        const res = await axios.get<MdxDataModel>(props.Api + query);

        try{
            const mdxData = parseMDX(res.data);
            console.log('mdxData',mdxData);
            const mdxHeaders = getPivotHeaders(mdxData.Columns,mdxData.Rows,res.data.IsAggregated);
            pivotColumns.value = getPivotColumns(mdxHeaders.columns,mdxData.Columns)
            Object.assign(pivotData,{
                columns: mdxHeaders.columns,
                rows: mdxHeaders.rows,
                data: mdxData.Values
            })
        } catch(e){
            console.error(e);
        }
     

    } catch(e: any){
        alert(e.response.data);
    }
})

function getPivotColumns(trsColumns: HeaderTr[], columns: HeaderCellModel) : HeaderTr{
    const cells: HeaderTd[] = [];

    for(let i = trsColumns.length - 1; i >= 0; i--){
        for(const cell of trsColumns[i].Cells){
            if(cell.DataIndex !== undefined && cell.DataIndex !== null){
                cells.push(cell)
            }
        }
    }

    const dataIndexes: number[] = [];
    const memory: HeaderCellModel[] = [columns];
    
    while(memory.length){
        const current = memory.shift()!;
        if(current.Children.length){
            memory.unshift(...current.Children);
        } else{
            dataIndexes.push(current.DataIndex!)
        }
    }

    var result: HeaderTr = {
        Cells: []
    }

    for(const dataIndex of dataIndexes){
        var cell = cells.find(cell => cell.DataIndex! === dataIndex)!;
        result.Cells.push(cell);
    }

    return result;
}



import { HeaderCellModel, HeaderTd, HeaderTr, MdxDataModel,ValueModel } from '../../model/models';

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