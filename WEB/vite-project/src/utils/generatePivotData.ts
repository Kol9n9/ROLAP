import { HeaderCellModel, HeaderTd, HeaderTr, PivotData } from "../model/models";


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

export function getPivotHeaders(columns: HeaderCellModel, rows: HeaderCellModel | null): PivotData {
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

type HeaderTdModelWithParent = HeaderTd & {
    Parent?: HeaderTd
}

type HeaderTrWithParents = Omit<HeaderTr, 'Cells'> & {
    Cells: HeaderTdModelWithParent[]
}

type HeaderCellModelWithParent = HeaderCellModel & {
    Parent?: HeaderTd
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
                if(!item.item.Children.length && item.item.Parent?.Hierarchy === item.item.Hierarchy) item.td.RowSpan = maxDepthLevel;
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

    return trs;
}

function getRows(rows: HeaderCellModel): HeaderTr[] {
    const memory = [rows];
    const headerTr: HeaderTr = { Cells: [] };
    const trs: HeaderTr[] = [{ Cells: [] }];
    let currentTr = trs[0];
    const parents: HeaderTr[] = [];
    const maxDepthes: number[] = [];
    while (memory.length) {
        const first = memory.splice(0, 1)[0];
        const map = mapHeaderCellModel(first);
        const depth = maxDepthes.splice(0, 1)[0];
        const depthLevel = getHierarchyDepth(first);
        memory.unshift(...first.Children);
        if (isTotalDimension(first)) {
            map.ColSpan = depthLevel - 1;
            headerTr.Cells.push(map);
            if (first.Children.length) {
                const firstParent = parents.length ? { Cells: [...parents.splice(0, 1)[0].Cells] } : { Cells: [] };
                parents.unshift(...Array.from({ length: first.Children.length }, () => firstParent))
                maxDepthes.unshift(...Array.from({ length: first.Children.length }, () => depthLevel - 1));
            }
            continue;
        }

        if (!first.Children.length || !first.Children[0].Hierarchy.startsWith(first.Hierarchy.split('.').slice(0, -1).join('.'))) {
            map.ColSpan = depth;
        }

        currentTr.Cells.push(map);
        if (first.Children.length) {
            const firstParent = parents.length ? { Cells: [...parents.splice(0, 1)[0].Cells] } : { Cells: [] };
            firstParent.Cells.push(map);
            parents.unshift(...Array.from({ length: first.Children.length }, () => firstParent));
            maxDepthes.unshift(...Array.from({ length: first.Children.length }, () => depthLevel - 1));
        } else {
            currentTr = { Cells: [] };
            trs.push(currentTr);
            const firstParent = parents.splice(0, 1)[0];
            for (const cell of firstParent.Cells) {
                cell.RowSpan++;
            }
        }
    }
    if (!trs[trs.length - 1].Cells.length) trs.splice(trs.length - 1, 1);
    headerTr.Cells = headerTr.Cells.filter((cell, index) => headerTr.Cells.findIndex(i => i.Hierarchy === cell.Hierarchy) === index);
    for (const tr of trs) {
        for (const cell of tr.Cells) {
            cell.RowSpan = cell.RowSpan > 1 ? cell.RowSpan - 1 : 1;
        }
    }
    return [headerTr, ...trs];
}