import { HeaderCellModel, HeaderTd, HeaderTr, PivotData } from "../model/models";

type HeaderTdModelWithParent = HeaderTd & {
    Parent?: HeaderTd
}

type HeaderTrWithParents = Omit<HeaderTr, 'Cells'> & {
    Cells: HeaderTdModelWithParent[]
}

type HeaderCellModelWithParent = HeaderCellModel & {
    Parent?: HeaderTd
}

function mapHeaderCellModel(cell: HeaderCellModel): HeaderTdModelWithParent {
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

function addRowHeaderToColumns(rowHeader: HeaderTr, trs: HeaderTr[]) {
    for (const [index, row] of rowHeader.Cells.entries()) {
        row.RowSpan = trs.length;
        trs[0].Cells.splice(index, 0, row);
    }
}

export function getPivotHeaders(columns: HeaderCellModel, rows: HeaderCellModel | null, IsAggregated: boolean): PivotData {
    const parsedColumns = getColumns(columns);
    const parsedRows = rows ? getRows(rows) : undefined;
    if (parsedRows) {
        addRowHeaderToColumns(parsedRows[0], parsedColumns);
        parsedRows.splice(0, 1);
    }

    return {
        columns: parsedColumns,
        ...(parsedRows ? { rows: parsedRows } : {})
    };
}

function getHierarchyDepth(cell: HeaderCellModel): number{
    const stack: Array<[HeaderCellModel,number]> = [[cell,1]];
    const hierarchyStarts = cell.Hierarchy.split('.')[0];
    let maxDepth = 1;
    while(stack.length){
        const [item,depth] = stack.pop()!;
        if(item.Hierarchy.startsWith(hierarchyStarts) && depth > maxDepth){
            maxDepth = depth;
        }
        for(let i = item.Children.length - 1; i >= 0; i--){
            stack.push([item.Children[i],depth+1])
        }
    }
    return maxDepth;
}


function takeDimensions(memory: HeaderCellModel[]): HeaderCellModelWithParent[] {
    const res: HeaderCellModel[] = [];

    const first = memory.splice(0, 1)[0];
    const hierarchy = first.Hierarchy.split(".")[0];
    res.push(first);
    for (let i = 0; i < memory.length; i++) {
        if (memory[i].Hierarchy.startsWith(hierarchy)) {
            res.push(memory.splice(0, 1)[0]);
            i = -1;
            continue;
        }
        break;
    }

    return res;
}


function findLastIndex<T>(array: T[], predicate: (item: T) => boolean): number {
    for (let i = array.length - 1; i >= 0; i--) {
        if (predicate(array[i])) return i;
    }
    return -1;
}

function insertInto(memory: HeaderCellModelWithParent[], item: HeaderCellModel, td: HeaderTd) {
    if (!memory.length) {
        memory.push(...item.Children.map(i => ({
            ...i,
            Parent: td
        })));
        return;
    }

    const hierarchy = item.Hierarchy.split('.')[0];

    let lastIndex = findLastIndex(memory, (i) => i.Hierarchy.startsWith(hierarchy) || i.Hierarchy === item.Children[0]?.Hierarchy);

    memory.splice(lastIndex + 1, 0, ...item.Children.map(i => ({
        ...i,
        Parent: td
    })));
}

function getColumns(columns: HeaderCellModel): HeaderTr[] {
    const memory: HeaderCellModelWithParent[] = [columns];
    const trs: HeaderTrWithParents[] = [{ Cells: [] }];
    let currentTr = trs[0];
    while (memory.length) {
        const memoryItems = takeDimensions(memory);

        const currentItemDepthLevels: Record<string, {
            td: HeaderTd,
            item: HeaderCellModelWithParent,
            depthLevel: number
        }[]> = {};

        for (const item of memoryItems) {
            const depthLevel = getHierarchyDepth(item)
            const td = mapHeaderCellModel(item);
            if (item.Parent) {
                td.Parent = item.Parent;
            }
            currentTr.Cells.push(td);

            insertInto(memory, item, td);

            const hierarchy = item.Hierarchy.split(".")[0];
            currentItemDepthLevels[hierarchy] = currentItemDepthLevels[hierarchy] ?? [];

            currentItemDepthLevels[hierarchy].push({
                td,
                depthLevel,
                item
            });
        }

        trs.push({ Cells: [] });
        currentTr = trs[trs.length - 1];

        for (const hierarchy in currentItemDepthLevels) {
            if (currentItemDepthLevels[hierarchy].length === 1) continue;


            const maxDepthLevel = currentItemDepthLevels[hierarchy].map(td => td.depthLevel).reduce((prev: number, curr: number) => {
                if (curr > prev) return curr;
                return prev;
            }, 0);

            for (const item of currentItemDepthLevels[hierarchy]) {
                const childHierarchy = item.item.Hierarchy.split('.').slice(0, -1).join('.');
                if (item.item.Children.length && !item.item.Children[0].Hierarchy.startsWith(childHierarchy)) item.td.RowSpan = maxDepthLevel;
                if (!item.item.Children.length) item.td.RowSpan = maxDepthLevel + 1;
            }
        }
    }

    let index = trs.length - 1;
    while (index >= 0) {
        for (const cell of trs[index].Cells) {
            cell.ColSpan = cell.ColSpan > 1 ? cell.ColSpan - 1 : cell.ColSpan;
            if (cell.Parent) {
                cell.Parent.ColSpan += cell.ColSpan;
                delete cell.Parent;
            }
        }
        index--;
    }

    if (!trs.slice(-1)[0].Cells.length) trs.splice(trs.length - 1);

    return trs;
}

function isDimensionHierarchy(cell: HeaderCellModel): boolean{
    return (cell.Hierarchy.split('.').length === 1) || (cell.Children.length !== 0 && cell.Children[0].Hierarchy === cell.Hierarchy);
}

type HeaderCellModelWithDepth = HeaderCellModel & {
    depthLevel?: number
}

type StackItem = {
    model: HeaderCellModelWithDepth,
    isProcessed: boolean,
    parent?: StackItem,
    trs: HeaderTr[]
}


function getRows(rows: HeaderCellModel): HeaderTr[] {
    const headerTr: HeaderTr = {Cells: []};
    const trs: HeaderTr[] = [];
    const stack: Array<StackItem> = [{
        model: rows,
        isProcessed: false,
        trs: []
    }];

    while(stack.length){
        const item = stack.pop()!;
        if(!item.isProcessed){
            stack.push({
                ...item,
                isProcessed: true
            });

            for(let i = item.model.Children.length - 1; i >= 0; i--){
                const stackItem: StackItem  = {
                    model: item.model.Children[i],
                    isProcessed: false,
                    parent: item,
                    trs: []
                }
                stackItem.model.depthLevel = getHierarchyDepth(stackItem.model);
                stack.push(stackItem);
            }
        } else {
            if(isDimensionHierarchy(item.model)){
                const headerCell = (()=>{
                    const index = headerTr.Cells.findIndex(c => c.Hierarchy === item.model.Hierarchy);
                    if(index !== -1) return headerTr.Cells[index];
                    headerTr.Cells.unshift(item.model);
                    return headerTr.Cells[0]
                })();

                const maxDepthLevel = getHierarchyDepth(item.model);
                for(const child of (item.model.Children as HeaderCellModelWithDepth[])){
                    if(maxDepthLevel < child.depthLevel!) continue;
                    child.ColSpan = maxDepthLevel - child.depthLevel!;
                }

                if(item.parent){
                    item.parent.trs.push(...item.trs)
                } else {
                    trs.push(...item.trs);
                }
                headerCell.ColSpan = (item.model.Children as HeaderCellModelWithDepth[]).reduce((prev:number,curr: HeaderCellModelWithDepth)=>{
                    if(curr.depthLevel && curr.depthLevel > prev) return curr.depthLevel;
                    return prev;
                },1);
                continue;
            }

            if(item.trs.length){
                item.model.RowSpan = item.trs.length;
                const maxDepthLevel = item.model.depthLevel!;
                for(const child of (item.model.Children as HeaderCellModelWithDepth[])){
                    if(maxDepthLevel < child.depthLevel!) continue;
                    child.ColSpan = maxDepthLevel - child.depthLevel!;
                }
                item.trs[0].Cells.unshift(item.model)
                if(item.parent){
                    item.parent.trs.push(...item.trs)
                }
            } else if(item.parent){
                item.parent.trs.push({Cells: [item.model]})
                item.parent.model.RowSpan += 1;
            }
            if(!item.parent){
                trs.push(...item.trs);
            }
        }
    }

    return [headerTr,...trs];
}

