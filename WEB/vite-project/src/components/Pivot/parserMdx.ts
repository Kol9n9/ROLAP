import { MdxDataModel, MdxSetModel, MdxTupleModel, ValueModel, HeaderCellModel, MdxValueModel, MdxParseResult } from './models';

import cubeData from '../../data/testCube.json' assert { type: "json" };

function parseSet(set: MdxSetModel): HeaderCellModel{
    const res: HeaderCellModel = mapTupleModel(set.Tuples[0],0);

    for(let i = 1; i < set.Tuples.length; i++){
        mergeCells(res,mapTupleModel(set.Tuples[i],i));
    }

    return res;
}

function createCell(hierarchyParts: string[][], index: number): HeaderCellModel{
    const isTotal = hierarchyParts[2][index].toLowerCase() === 'true';
    return {
        IsTotal: isTotal,
        Key: hierarchyParts[0][index],
        Name: hierarchyParts[1][index],
        Children: []
    }
}


function mapTupleModel(tuple: MdxTupleModel, dataIndex: number): HeaderCellModel{
    const hierarchyParts: string[][] = [[],[],[]];

    for(const member of tuple.Members){
        const isTotal = member.Name.endsWith('[All]').toString()
        const keys = member.Key.split('.');
        const names = member.Name.split('.');
        hierarchyParts[0].push(...keys);
        hierarchyParts[1].push(...names);
        hierarchyParts[2].push(...Array.from({
            length: 2
        },()=>isTotal))
    }

    const cell: HeaderCellModel = createCell(hierarchyParts,0)
    let currentCell = cell;
    
    for(let i = 1; i < hierarchyParts[0].length; i++){
        const createdCell = createCell(hierarchyParts,i);
        currentCell.Children.push(createdCell);
        currentCell = createdCell;
    }

    currentCell.DataIndex = dataIndex;

    return cell;
}

function mergeCells(parentCell: HeaderCellModel, mergedCell: HeaderCellModel){
    for(const cell of mergedCell.Children){
        const find = parentCell.Children.find(i => i.Key === cell.Key);
        if(!find){
            parentCell.Children.push(cell);
            return;
        }
        if(find.Children.length === 0){
            find.Children.push(...cell.Children);
            return
        }
        mergeCells(find,cell);
    }
}

function parseValues(values: MdxValueModel[]): ValueModel[]{
    const res: ValueModel[] = [];

    for(const [index,value] of values.entries()){
        res.push({
            Value: value.Value,
            FormattedValue: value.FormattedValue,
            DataIndex: index
        })
    }


    return res;
}

export function parseMDX(): MdxParseResult{
    const axes = [];
    for(const set of cubeData.Sets){
        axes.push(parseSet(set));
    }

    return {
        Columns: axes[0],
        Rows: axes[1],
        Values: parseValues(cubeData.Values)
    }
}