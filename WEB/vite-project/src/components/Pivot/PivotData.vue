<script lang="ts">

import { defineComponent, h, PropType, VNode } from 'vue'
import { HeaderTd, HeaderTr, ValueModel } from '../../model/models';

function renderCell(td: HeaderTd, value: string): VNode{
    return h('td',{
        rowSpan: td.RowSpan,
        colSpan: td.ColSpan
    },value)
}

function isTotalTr(tr?: HeaderTr){
    if(!tr) return true;
    return tr.Cells[tr.Cells.length - 1].IsTotal;
}

function renderTr(columns: HeaderTr, data: ValueModel[], tr?: HeaderTr): VNode{
    const cells: VNode[] = [];
    const rowDataIndex = tr ? (tr.Cells[tr.Cells.length - 1].DataIndex)! : 0;
    const isTotal = isTotalTr(tr);
    if(tr){
        cells.push(...tr.Cells.map(i => renderCell(i,i.DisplayName)))
    }

    for(const col of columns.Cells){
        const valueIndex = rowDataIndex * columns.Cells.length + (col.DataIndex)!;
        const value = data ? data[valueIndex]?.FormattedValue ?? '' : '';
        cells.push(renderCell(col,value))
    }


    return h('tr',{
        className: isTotal ? 'aggregation' : ''
    },cells);
}

function renderTrs(columns: HeaderTr,  data: ValueModel[], rows?: HeaderTr[]){

    const trs: VNode[] = [];

    if(rows){
        for(const row of rows){
            trs.push(renderTr(columns,data,row))
        }
    } else {
        
        trs.push(renderTr(columns,data))
    }

    return trs;
}

function renderTBody(columns: HeaderTr, data: ValueModel[], rows?: HeaderTr[]): VNode{
    return h('tbody',{

    }, renderTrs(columns, data, rows))
}

export default defineComponent({
    name: 'PivotHeader',
    props: {
        Columns: {
            type: Object as PropType<HeaderTr>,
            required: true
        },
        Rows: {
            type: Object as PropType<HeaderTr[]>,
            required: false
        },
        Data: {
            type: Object as PropType<ValueModel[]>,
            required: true
        }
    },
    render(){
        return renderTBody(this.$props.Columns,this.$props.Data,this.$props.Rows)
    }
})

</script>
<style>
</style>