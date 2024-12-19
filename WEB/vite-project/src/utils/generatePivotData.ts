import { HeaderCellModel, HeaderTd, HeaderTr, PivotData } from "../model/models";


function mapHeaderCellModel(cell:HeaderCellModel): HeaderTd{
    return {
        Key: cell.Name,
        Name: cell.Name,
        DisplayName: cell.DisplayName,
        RowSpan: 1,
        ColSpan: 1,
        DataIndex: cell.DataIndex,
        IsTotal: cell.IsTotal,
        Hierarchy: cell.Hierarchy
    }
}


function getChildrenLength(trs: HeaderTr[]): number{
    if(!trs.length) return 1;
    const firstTr = trs[0];
    return firstTr.Cells.reduce((prev: number, current: HeaderTd): number=>{
        return prev + current.ColSpan
    },0) || 1;
}

function parseColumns(cell: HeaderCellModel): HeaderTr[]{
    const td: HeaderTd = mapHeaderCellModel(cell);
    const childen = parseColumnChildren(cell.Children.sort(sortHeaderCellByTotal)).filter(i => i.Cells.length); 
    td.ColSpan = getChildrenLength(childen);
    return [{
        Cells: [td]
    }, ...childen];
}

function parseColumnChildren(children: HeaderCellModel[]): HeaderTr[]{
    const tr: HeaderTr = {
        Cells: []
    }

    const childtr: HeaderTr[] = [];


    for(const child of children){
        const parsed = parseColumns(child);
        tr.Cells.push(...parsed[0].Cells);

        const diff = childtr.length - (parsed.length - 1)

        if(childtr.length &&  diff !== 0){ // отличаются уровни
            if(diff > 0){
                for(const cell of parsed[0].Cells) cell.RowSpan+=diff;
                for(const [index,parsedTr] of parsed.slice(1).entries()){
                    childtr[index+diff].Cells.push(...parsedTr.Cells);
                }
            }
            else {
                var trs = childtr.splice(0);
                for(const [index,parsedTr] of parsed.slice(1).entries()){
                    childtr.push({
                        Cells: [...parsedTr.Cells]
                    })
                }
                for(const [index,tr] of trs.entries()){
                    childtr[childtr.length + diff].Cells.unshift(...tr.Cells);
                }
                tr.Cells[tr.Cells.length-1+diff].RowSpan+=-diff;
                
            }
        } else {
            for(const [index,parsedTr] of parsed.slice(1).entries()){
                if(!childtr[index]){
                    childtr[index] = parsedTr;
                } else {
                    childtr[index].Cells.push(...parsedTr.Cells);
                }
            }
        }
    }

    return [tr,...childtr];
}

function sortHeaderCellByTotal(a: HeaderCellModel,b: HeaderCellModel): number{
    const aTotal = a.Name.endsWith('[All]');
    const bTotal = b.Name.endsWith('[All]');
    if(aTotal && !bTotal) return 1;
    if(!aTotal && bTotal) return -1;
    return 0;
}


function isTotalDimension(cell:HeaderCellModel): boolean{
    const hierarchyParts = cell.Hierarchy.split('.');
    return cell.Name === '[Индикаторы]' || (hierarchyParts.length === 2 && hierarchyParts[1] === '[All]' && (cell.DataIndex === undefined || cell.DataIndex === null) && cell.Key !== '[All]');
}
type StringKey = string;
type HierarchyLevels = Object & {
    [key: StringKey]: number
};

function addHierarchyLevels(cell: HeaderCellModel, hierarchyLevels: HierarchyLevels){
    const hierarchy = cell.Hierarchy.split('.');
    const dimension = hierarchy.splice(0,1)[0];
    if(!hierarchyLevels.hasOwnProperty(dimension)){
        hierarchyLevels[dimension] = 0;
    }
    if(hierarchy.length > hierarchyLevels[dimension]) hierarchyLevels[dimension] = hierarchy.length;
}

function addUniqueTdToTr(tr: HeaderTr, cells: HeaderTd[]){
    const names = tr.Cells.map(i => i.Name);
    const insertCells = cells.filter(cell => !names.includes(cell.Name))
    tr.Cells.unshift(...insertCells);
}

function parseRows(cell:HeaderCellModel, hierarchyLevels: HierarchyLevels): HeaderTr[]{
    const trs: HeaderTr[] = [
        { // headers
            Cells: []
        }
    ];

    const childTrs: HeaderTr[] = [];
    for(const kind of cell.Children){
        const parsed = parseRows(kind,hierarchyLevels);
        addUniqueTdToTr(trs[0],parsed[0].Cells)
        childTrs.push(...parsed.slice(1));
    }
    addHierarchyLevels(cell,hierarchyLevels);

    const mapped = mapHeaderCellModel(cell);
    
    if(isTotalDimension(cell)){
        trs[0].Cells.unshift(mapped)
        trs.push(...childTrs);
    } else{
        mapped.RowSpan = childTrs.length || 1;
        trs.push({
            Cells: [mapped,...childTrs[0]?.Cells ?? []]
        })
        trs.push(...childTrs.slice(1));
    }

    return trs;
}

function fixRowCellColSpan(cell: HeaderTd, hierarchyLevels: HierarchyLevels){
    const hierarchy = cell.Hierarchy.split('.');
    const dimension = hierarchy.splice(0,1)[0];
    const level = hierarchyLevels[dimension];
    const hierarchyLength = hierarchy.length ;
    if(hierarchyLength === level - 1) return;
    cell.ColSpan = level - (hierarchyLength) || 1;
}

function fixRowsColSpans(trs: HeaderTr[], hierarchyLevels: HierarchyLevels){
    for(const tr of trs){
        for(const cell of tr.Cells){
            fixRowCellColSpan(cell,hierarchyLevels);
        }
    }
}


function addRowHeaderToColumns(rowHeader: HeaderTr, trs: HeaderTr[]){
    for(const [index,row] of rowHeader.Cells.entries()){
        row.RowSpan = trs.length;
        trs[0].Cells.splice(index,0,row);
    }
}



export function getPivotHeaders(columns: HeaderCellModel, rows: HeaderCellModel | null): PivotData{
    const parsedColumns = parseColumns(columns);
    const parsedRows = rows ? (function(){
        const hierarchyLevels: HierarchyLevels = {};
        const parsedRows = parseRows(rows,hierarchyLevels);
        fixRowsColSpans(parsedRows,hierarchyLevels);
        return parsedRows;
    })() : undefined;  

    if(parsedRows){
        addRowHeaderToColumns(parsedRows[0],parsedColumns);
        parsedRows.splice(0,1);
    }

    return {
        columns: parsedColumns,
        ...(parsedRows ? {rows: parsedRows} : {})
    }
}