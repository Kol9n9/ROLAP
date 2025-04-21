import { HeaderCellModel, HeaderTd, HeaderTr, PivotData } from "../model/models";


function mapHeaderCellModel(cell: HeaderCellModel): HeaderTd {
    return {
        Key: cell.Name,
        Name: cell.Name,
        DisplayName: cell.DisplayName,
        RowSpan: cell.RowSpan,
        ColSpan: cell.ColSpan,
        DataIndex: cell.DataIndex,
        IsTotal: cell.IsTotal,
        Hierarchy: cell.Hierarchy
    }
}

const measureName = '[Индикаторы]';

function isTotalDimension(cell: HeaderCellModel): boolean {
    const hierarchyParts = cell.Hierarchy.split('.');
    return cell.Name === measureName || (hierarchyParts.length === 2 && hierarchyParts[1] === '[All]' && (cell.DataIndex === undefined || cell.DataIndex === null) && cell.Key !== '[All]');
}

function addRowHeaderToColumns(rowHeader: HeaderTr, trs: HeaderTr[]) {
    for (const [index, row] of rowHeader.Cells.entries()) {
        row.RowSpan = trs.length;
        trs[0].Cells.splice(index, 0, row);
    }
}

type TrRow = {
    Cells: HeaderCellModel[]
}

function copyCell(cell: HeaderCellModel): HeaderCellModel {
    return {
        IsTotal: cell.IsTotal,
        Name: cell.Name,
        DisplayName: cell.DisplayName,
        Key: cell.Key,
        Children: [],
        DataIndex: cell.DataIndex,
        Hierarchy: cell.Hierarchy,
        RowSpan: cell.RowSpan,
        ColSpan: cell.ColSpan,
        IsDeleted: cell.IsDeleted
    }
}

function getFlatRows(cell: HeaderCellModel, prevRow: TrRow = { Cells: [] }): TrRow[] {
    const res: TrRow[] = [];

    if (cell.Children.length) {
        for (const kind of cell.Children) {
            const copy: TrRow = {
                Cells: [...prevRow.Cells, copyCell(cell)]
            }
            res.push(...getFlatRows(kind, copy));
        }
    } else {
        const copy = [...prevRow.Cells, cell].map(copyCell)
        res.push({
            Cells: copy
        })
    }
    return res;
}

type DimensionsChain = Object & {
    [key: string]: number
}

function calculateAndGetDimensionsChain(flatRows: TrRow[]): DimensionsChain {
    const longChains: DimensionsChain = {};

    for (const row of flatRows) {
        let index = 0;
        while (row.Cells[index]) {
            const currentDimension = row.Cells[index].Hierarchy.split('.')[0];
            const dimensionParts = row.Cells.filter(cell => cell.Hierarchy.startsWith(currentDimension));
            if (!longChains.hasOwnProperty(currentDimension)) {
                longChains[currentDimension] = 0;
            }
            if (longChains[currentDimension] < dimensionParts.length) longChains[currentDimension] = dimensionParts.length;
            index += dimensionParts.length;
        }
    }

    for (const row of flatRows) {
        let index = 0;
        while (row.Cells[index]) {
            const currentDimension = row.Cells[index].Hierarchy.split('.')[0];
            const dimensionParts = row.Cells.filter(cell => cell.Hierarchy.startsWith(currentDimension));
            const dimensionChains = longChains[currentDimension];
            if (dimensionParts.length !== dimensionChains) {
                const last = dimensionParts.slice(-1)[0];
                row.Cells.splice(dimensionParts.length + index, 0, ...Array.from({ length: dimensionChains - dimensionParts.length }, () => copyCell(last)));
            }
            index += dimensionChains;
        }
    }

    return longChains;
}

function addUniqueCellToCells(cells: HeaderCellModel[], cell: HeaderCellModel) {
    const names = cells.map(i => i.Name);
    if (names.includes(cell.Name)) return;
    cells.push(cell);
}

function isMeasure(cell: HeaderCellModel): boolean {
    return cell.Hierarchy.split('.')[0] === measureName;
}

function mergeFlatRows(flatRows: TrRow[], chains: DimensionsChain, isRow: boolean = true): void {
    const headerCells: HeaderCellModel[] = [];
    for (let currentRowIndex = 0; currentRowIndex < flatRows.length; currentRowIndex++) {
        let index: number = 0;
        while (flatRows[currentRowIndex].Cells[index]) {
            const currentCell = flatRows[currentRowIndex].Cells[index];
            let rowIndex = currentRowIndex;
            if (currentCell.IsDeleted) {
                while (currentCell.Hierarchy === flatRows[rowIndex + 1]?.Cells[index]?.Hierarchy && currentCell.Key === flatRows[rowIndex + 1]?.Cells[index]?.Key) {
                    flatRows[rowIndex + 1].Cells[index].IsDeleted = true;
                    rowIndex++;
                }
                index++;
                continue;
            }

            if (isRow && isTotalDimension(currentCell)) {
                addUniqueCellToCells(headerCells, currentCell);
                for (const row of flatRows) {
                    row.Cells.splice(index, 1)
                }
                continue;
            }

            let rowSpan = 0;
            while (!isMeasure(currentCell) && currentCell.Hierarchy === flatRows[rowIndex + 1]?.Cells[index]?.Hierarchy && currentCell.Key === flatRows[rowIndex + 1]?.Cells[index]?.Key && currentCell.Name === flatRows[rowIndex + 1]?.Cells[index]?.Name) {
                flatRows[rowIndex + 1].Cells[index].IsDeleted = true;
                rowSpan++;
                rowIndex++;
            }

            let cellIndex = index;
            let colSpan = 0;

            if (isRow) {
                while (!isMeasure(currentCell) && currentCell.Hierarchy === flatRows[currentRowIndex]?.Cells[cellIndex + 1]?.Hierarchy && currentCell.Key === flatRows[currentRowIndex]?.Cells[cellIndex + 1]?.Key && currentCell.Name === flatRows[currentRowIndex]?.Cells[cellIndex + 1]?.Name) {
                    flatRows[currentRowIndex].Cells[cellIndex + 1].IsDeleted = true;
                    colSpan++;
                    cellIndex++;
                }
            } else {

                function isParentsEqual(cellIndex: number): boolean {
                    if (isMeasure(currentCell)) {
                        return false;
                    }
                    if (currentRowIndex === 0) return true;
                    const currentParentCell = flatRows[currentRowIndex - 1].Cells[index];
                    const nextParentCell = flatRows[currentRowIndex - 1].Cells[cellIndex];
                    return currentParentCell?.Hierarchy === nextParentCell?.Hierarchy;
                }

                while (isParentsEqual(cellIndex + 1) && currentCell.Hierarchy === flatRows[currentRowIndex]?.Cells[cellIndex + 1]?.Hierarchy && currentCell.Key === flatRows[currentRowIndex]?.Cells[cellIndex + 1]?.Key && currentCell.Name === flatRows[currentRowIndex]?.Cells[cellIndex + 1]?.Name) {
                    flatRows[currentRowIndex].Cells[cellIndex + 1].IsDeleted = true;
                    colSpan++;
                    cellIndex++;
                }
            }



            currentCell.RowSpan = rowSpan + 1;
            currentCell.ColSpan = colSpan + 1;

            index++;
        }
    }
    if (isRow) {
        for (const cell of headerCells) {
            cell.RowSpan = 1;
            const currentDimension = cell.Hierarchy.split('.')[0]
            cell.ColSpan = chains[currentDimension] - 1
        }
        flatRows.splice(0, 0, {
            Cells: headerCells
        })
    }
}

function getHeaderTr(flatRows: TrRow[]): HeaderTr[] {
    const trs: HeaderTr[] = [];

    for (const row of flatRows) {
        const cells = row.Cells.filter(cell => !cell.IsDeleted);
        trs.push({
            Cells: cells.map(mapHeaderCellModel)
        })
    }

    return trs;
}

function transparentRows(flatRows: TrRow[]) {
    const transparent: TrRow[] = [];

    for (let i = 0; i < flatRows.length; i++) {
        for (let j = 0; j < flatRows[i].Cells.length; j++) {
            transparent[j] = transparent[j] || { Cells: [] };
            transparent[j].Cells.splice(i, 0, flatRows[i].Cells[j])
        }
    }

    flatRows.splice(0);
    flatRows.push(...transparent);
}

function getColumnsTr(columns: HeaderCellModel): HeaderTr[] {
    const flatRows = getFlatRows(columns);
    const chains = calculateAndGetDimensionsChain(flatRows);
    transparentRows(flatRows);
    mergeFlatRows(flatRows, chains, false);
    return getHeaderTr(flatRows);
}

function getRowsTr(rows: HeaderCellModel): HeaderTr[] {
    const flatRows = getFlatRows(rows);
    const chains = calculateAndGetDimensionsChain(flatRows);
    mergeFlatRows(flatRows, chains);
    return getHeaderTr(flatRows);
}

export function getPivotHeaders(columns: HeaderCellModel, rows: HeaderCellModel | null): PivotData {
    // const parsedColumns = getColumnsTr(columns);
    // const parsedRows = rows ? getRowsTr(rows) : undefined;

    // if (parsedRows) {
    //     addRowHeaderToColumns(parsedRows[0], parsedColumns);
    //     parsedRows.splice(0, 1);
    // }

    // getPivot(columns, rows);

    // return {
    //     columns: parsedColumns,
    //     ...(parsedRows ? { rows: parsedRows } : {})
    // }

    const parsedColumns = newColumns(columns);
    const parsedRows = rows ? newRows(rows) : undefined;
    if(parsedRows){
        addRowHeaderToColumns(parsedRows[0],parsedColumns);
        parsedRows.splice(0, 1);
    }

    return {
        columns: parsedColumns,
        ...(parsedRows ? { rows: parsedRows } : {})
    };
}

function getHierarchyDepth(cell: HeaderCellModel): number {
    let maxDepth = 0;
    const hierarchy = cell.Hierarchy.split(".").slice(0, -1).join('.');
    const memory = [cell];
    while (memory.length) {
        maxDepth++;
        const memoryItems = memory.splice(0);
        for (const item of memoryItems) {
            for (const child of item.Children) {
                if (child.Hierarchy.startsWith(hierarchy)) {
                    memory.push(child)
                }
            }
        }
    }
    return maxDepth;
}


function takeDimensions(memory: HeaderCellModel[]): HeaderCellModel[]{
    const res: HeaderCellModel[] = [];

    const first = memory.splice(0,1)[0];
    const hierarchy = first.Hierarchy.split(".")[0];
    res.push(first);
    for(let i = 0; i < memory.length; i++){
        if(memory[i].Hierarchy.startsWith(hierarchy)){
            res.push(memory.splice(0,1)[0]);
            i = -1;
            continue;
        }
        break;
    }

    return res;
}

// function newColumns(columns: HeaderCellModel): HeaderTr[] {
//     const memory = [columns];
//     const trs: HeaderTr[] = [{ Cells: [] }];
//     let currentTr = trs[0];
//     const parents: HeaderTd[] = [];
//     while (memory.length) {
//         const memoryItems = memory.splice(0);
//         const currentParents: HeaderTd[] = [];

//         const currentItemDepthLevels: Record<string, {
//             td: HeaderTd,
//             item: HeaderCellModel,
//             depthLevel: number
//         }[]> = {};

//         for (const item of memoryItems) {
//             const depthLevel = getHierarchyDepth(item)
//             const td = mapHeaderCellModel(item);
//             currentTr.Cells.push(td);
//             memory.push(...item.Children);
//             currentParents.push(...Array.from({ length: item.Children.length }, () => td))
//             const hierarchy = item.Hierarchy.split(".")[0];
//             currentItemDepthLevels[hierarchy] = currentItemDepthLevels[hierarchy] ?? [];

//             currentItemDepthLevels[hierarchy].push({
//                 td,
//                 depthLevel,
//                 item
//             });
//         }

//         trs.push({ Cells: [] });
//         currentTr = trs[trs.length - 1];

//         for (const hierarchy in currentItemDepthLevels) {
//             if (currentItemDepthLevels[hierarchy].length === 1) continue;

//             const maxDepthLevel = currentItemDepthLevels[hierarchy].map(td => td.depthLevel).reduce((prev: number, curr: number) => {
//                 if (curr > prev) return curr;
//                 return prev;
//             }, 0);

//             for (const item of currentItemDepthLevels[hierarchy]) {
//                 const childHierarchy = item.item.Hierarchy.split('.').slice(0, -1).join('.');
//                 if (item.item.Children.length && !item.item.Children[0].Hierarchy.startsWith(childHierarchy)) item.td.RowSpan = maxDepthLevel;
//             }
//         }

//         parents.unshift(...currentParents);
//         if (memory.length) {
           
//         } else {
//             let index = trs.length - 1;
//             while (parents.length) {
//                 for (const cell of trs[index].Cells) {
//                     cell.ColSpan = cell.ColSpan > 1 ? cell.ColSpan - 1 : cell.ColSpan;
//                     parents[0].ColSpan += cell.ColSpan;
//                     parents.splice(0, 1)
//                 }
//                 index--;
//             }
//         }
//     }
//     trs[0].Cells[0].ColSpan = trs[0].Cells[0].ColSpan > 1 ? trs[0].Cells[0].ColSpan - 1 : trs[0].Cells[0].ColSpan;
//     return trs;
// }

function newColumns(columns: HeaderCellModel): HeaderTr[]{
    console.log('columns',columns);
    const res: HeaderTr[] = [];
    const memory = [columns];


    return res;
}

function newRows(rows: HeaderCellModel): HeaderTr[] {
    const memory = [rows];
    const headerTr: HeaderTr = {Cells: []};
    const trs: HeaderTr[] = [{Cells: []}];
    let currentTr = trs[0];
    const parents: HeaderTr[] = [];
    const maxDepthes: number[] = [];
    while(memory.length){
        const first = memory.splice(0,1)[0];
        const map = mapHeaderCellModel(first);
        const depth = maxDepthes.splice(0,1)[0];
        const depthLevel = getHierarchyDepth(first);
        memory.unshift(...first.Children);
        if(isTotalDimension(first)){
            map.ColSpan = depthLevel - 1;
            headerTr.Cells.push(map);
            if(first.Children.length){
                const firstParent = parents.length ? {Cells: [...parents.splice(0,1)[0].Cells]}: {Cells: []};
                parents.unshift(...Array.from({length: first.Children.length},()=>firstParent))
                maxDepthes.unshift(...Array.from({length: first.Children.length}, ()=>depthLevel - 1));
            }
            continue;
        }

        if(!first.Children.length || !first.Children[0].Hierarchy.startsWith(first.Hierarchy.split('.').slice(0,-1).join('.'))){
            map.ColSpan = depth;
        }

        currentTr.Cells.push(map);
        if(first.Children.length){
            const firstParent = parents.length ? {Cells: [...parents.splice(0,1)[0].Cells]}: {Cells: []};
            firstParent.Cells.push(map);
            parents.unshift(...Array.from({length: first.Children.length},()=>firstParent));
            maxDepthes.unshift(...Array.from({length: first.Children.length}, ()=>depthLevel - 1));
        } else{
            currentTr = {Cells: []};
            trs.push(currentTr);
            const firstParent = parents.splice(0,1)[0];
            for(const cell of firstParent.Cells){
                cell.RowSpan++;
            }
        }
    }
    if(!trs[trs.length - 1].Cells.length) trs.splice(trs.length-1,1);
    headerTr.Cells = headerTr.Cells.filter((cell,index) => headerTr.Cells.findIndex(i => i.Hierarchy === cell.Hierarchy) === index);
    for(const tr of trs){
        for(const cell of tr.Cells){
            cell.RowSpan = cell.RowSpan > 1 ? cell.RowSpan - 1 : 1;
        }
    }
    return [headerTr,...trs];
}