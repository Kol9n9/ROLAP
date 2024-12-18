<script lang="ts">

import { defineComponent, h, PropType, VNode } from 'vue'
import { HeaderTd, HeaderTr } from '../../model/models';


function renderTd(td: HeaderTd): VNode{
    return h('td',{
        rowSpan: td.RowSpan,
        colSpan: td.ColSpan,
    },td.DisplayName)
}

function renderTr(tr: HeaderTr): VNode{
    return h('tr',{

    }, tr.Cells.map(renderTd))
}

function renderThead(columns: HeaderTr[]): VNode{
    return h('thead',{

    }, columns.map(renderTr))
}

export default defineComponent({
    name: 'PivotHeader',
    props: {
        Columns: {
            type: Object as PropType<HeaderTr[]>,
            required: true
        }
    },
    render(){
        return renderThead(this.$props.Columns)
    }
})

</script>
<style>
</style>