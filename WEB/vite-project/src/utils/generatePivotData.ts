import { HeaderCellModel, HeaderTd, HeaderTr, PivotData } from "../model/models";


function mapHeaderCellModel(cell:HeaderCellModel): HeaderTd{
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

function isTotalDimension(cell:HeaderCellModel): boolean{
    const hierarchyParts = cell.Hierarchy.split('.');
    return cell.Name === measureName || (hierarchyParts.length === 2 && hierarchyParts[1] === '[All]' && (cell.DataIndex === undefined || cell.DataIndex === null) && cell.Key !== '[All]');
}

function addRowHeaderToColumns(rowHeader: HeaderTr, trs: HeaderTr[]){
    for(const [index,row] of rowHeader.Cells.entries()){
        row.RowSpan = trs.length;
        trs[0].Cells.splice(index,0,row);
    }
}

type TrRow = {
    Cells: HeaderCellModel[]
}

function copyCell(cell: HeaderCellModel): HeaderCellModel{
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

function getFlatRows(cell: HeaderCellModel, prevRow: TrRow = {Cells: []}): TrRow[]{
    const res: TrRow[] = [];

    if(cell.Children.length){
        for(const kind of cell.Children){
            const copy: TrRow = {
                Cells: [...prevRow.Cells,copyCell(cell)]
            }
            res.push(...getFlatRows(kind,copy));
        }
    } else{
        const copy = [...prevRow.Cells,cell].map(copyCell)
        res.push({
            Cells: copy
        })
    }
    return res;
}

type DimensionsChain = Object & {
    [key: string]: number
}

function calculateAndGetDimensionsChain(flatRows: TrRow[]): DimensionsChain{
    const longChains: DimensionsChain = {};

    for(const row of flatRows){
        let index = 0;
        while(row.Cells[index]){
            const currentDimension = row.Cells[index].Hierarchy.split('.')[0];
            const dimensionParts = row.Cells.filter(cell => cell.Hierarchy.startsWith(currentDimension));
            if(!longChains.hasOwnProperty(currentDimension)){
                longChains[currentDimension] = 0;
            }
            if(longChains[currentDimension] < dimensionParts.length) longChains[currentDimension] = dimensionParts.length;
            index += dimensionParts.length;
        }
    }

    for(const row of flatRows){
        let index = 0;
        while(row.Cells[index]){
            const currentDimension = row.Cells[index].Hierarchy.split('.')[0];
            const dimensionParts = row.Cells.filter(cell => cell.Hierarchy.startsWith(currentDimension));
            const dimensionChains = longChains[currentDimension];
            if(dimensionParts.length !== dimensionChains){
                const last = dimensionParts.slice(-1)[0];
                row.Cells.splice(dimensionParts.length + index,0,...Array.from({length: dimensionChains - dimensionParts.length}, ()=>copyCell(last)));
            }
            index += dimensionChains;
        }
    }

    return longChains;
}

function addUniqueCellToCells(cells: HeaderCellModel[], cell: HeaderCellModel){
    const names = cells.map(i => i.Name);
    if(names.includes(cell.Name)) return;
    cells.push(cell);
}

function mergeFlatRows(flatRows: TrRow[], chains: DimensionsChain, isRow: boolean = true): void{
    const headerCells: HeaderCellModel[] = [];
    for(let currentRowIndex = 0; currentRowIndex < flatRows.length; currentRowIndex++){
        let index: number = 0;
        while(flatRows[currentRowIndex].Cells[index]){
            const currentCell = flatRows[currentRowIndex].Cells[index];
            if(currentCell.Hierarchy.split('.')[0] === measureName){
                index++;
                continue;
            }
            let rowIndex = currentRowIndex;
            if(currentCell.IsDeleted){
                while(currentCell.Hierarchy === flatRows[rowIndex+1]?.Cells[index]?.Hierarchy && currentCell.Key === flatRows[rowIndex+1]?.Cells[index]?.Key){
                    flatRows[rowIndex+1].Cells[index].IsDeleted = true;
                    rowIndex++;
                }
                index++;
                continue;
            }
    
            if(isRow && isTotalDimension(currentCell)){
                addUniqueCellToCells(headerCells,currentCell);
                for(const row of flatRows){
                    row.Cells.splice(index,1)
                }
                continue;
            }
    
            let rowSpan = 0;
            while(currentCell.Hierarchy === flatRows[rowIndex+1]?.Cells[index]?.Hierarchy && currentCell.Key === flatRows[rowIndex+1]?.Cells[index]?.Key){
                flatRows[rowIndex+1].Cells[index].IsDeleted = true;
                rowSpan++;
                rowIndex++;
            }
    
            let cellIndex = index;
            let colSpan = 0;
    
            if(isRow){
                while(currentCell.Hierarchy === flatRows[currentRowIndex]?.Cells[cellIndex+1]?.Hierarchy && currentCell.Key === flatRows[currentRowIndex]?.Cells[cellIndex+1]?.Key){
                    flatRows[currentRowIndex].Cells[cellIndex+1].IsDeleted = true;
                    colSpan++;
                    cellIndex++;
                }
            } else {

                function isParentsEqual(cellIndex: number): boolean{
                    if(currentRowIndex === 0) return true;
                    const currentParentCell = flatRows[currentRowIndex-1].Cells[index];
                    const nextParentCell = flatRows[currentRowIndex-1].Cells[cellIndex];
                    return currentParentCell?.Hierarchy === nextParentCell?.Hierarchy;
                }

                while(isParentsEqual(cellIndex+1) && currentCell.Hierarchy === flatRows[currentRowIndex]?.Cells[cellIndex+1]?.Hierarchy && currentCell.Key === flatRows[currentRowIndex]?.Cells[cellIndex+1]?.Key){
                    flatRows[currentRowIndex].Cells[cellIndex+1].IsDeleted = true;
                    colSpan++;
                    cellIndex++;
                }
            }

            
    
            currentCell.RowSpan = rowSpan + 1;
            currentCell.ColSpan = colSpan + 1;

            index++;
        }
    }
    if(isRow){
        for(const cell of headerCells){
            cell.RowSpan = 1;
            const currentDimension = cell.Hierarchy.split('.')[0]
            cell.ColSpan = chains[currentDimension] - 1
        }
        flatRows.splice(0,0,{
            Cells: headerCells
        })
    }
}

function getHeaderTr(flatRows: TrRow[]): HeaderTr[]{
    const trs: HeaderTr[] = [];

    for(const row of flatRows){
        const cells = row.Cells.filter(cell => !cell.IsDeleted);
        trs.push({
            Cells: cells.map(mapHeaderCellModel)
        })
    }

    return trs;
}

function transparentRows(flatRows: TrRow[]){
    const transparent: TrRow[] = [];

    for(let i = 0; i < flatRows.length; i++){
        for(let j = 0; j < flatRows[i].Cells.length; j++){
            transparent[j] = transparent[j] || {Cells: []};
            transparent[j].Cells.splice(i,0,flatRows[i].Cells[j])
        }
    }

    flatRows.splice(0);
    flatRows.push(...transparent);
}

function getColumnsTr(columns: HeaderCellModel): HeaderTr[]{
    const flatRows = getFlatRows(columns);
    const chains = calculateAndGetDimensionsChain(flatRows);
    transparentRows(flatRows);
    mergeFlatRows(flatRows,chains,false);
    return getHeaderTr(flatRows);
}

function getRowsTr(rows: HeaderCellModel): HeaderTr[]{
    const flatRows = getFlatRows(rows);
    const chains = calculateAndGetDimensionsChain(flatRows);
    mergeFlatRows(flatRows,chains);
    return getHeaderTr(flatRows);
}

export function getPivotHeaders(columns: HeaderCellModel, rows: HeaderCellModel | null): PivotData{
    const parsedColumns = getColumnsTr(columns);
    const parsedRows = rows ? getRowsTr(rows) : undefined;

    if(parsedRows){
        addRowHeaderToColumns(parsedRows[0],parsedColumns);
        parsedRows.splice(0,1);
    }

    return {
        columns: parsedColumns,
        ...(parsedRows ? {rows: parsedRows} : {})
    }
}