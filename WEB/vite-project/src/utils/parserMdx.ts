import { MdxDataModel, MdxSetModel, MdxTupleModel, ValueModel, HeaderCellModel, MdxValueModel, MdxParseResult, MdxMemberModel } from '../model/models';

function parseSet(set: MdxSetModel): HeaderCellModel{
    const res: HeaderCellModel = mapTupleModel(set.Tuples[0],0);

    for(let i = 1; i < set.Tuples.length; i++){
        mergeCells(res,mapTupleModel(set.Tuples[i],i));
    }

    return res;
}

function createCell(key: string, name: string, isTotal: boolean, hierarchy: string): HeaderCellModel{
    return {
        IsTotal: isTotal,
        Key: key,
        Name: name,
        DisplayName: name,
        Hierarchy: hierarchy,
        Children: [],
        RowSpan: 1,
        ColSpan: 1
    }
}

function createCells(member: MdxMemberModel): HeaderCellModel{
    const keys = member.Key.split('.');
    const names = member.Name.split('.');
    const cell = createCell(keys[0],names[0],keys[0] === '[All]',names[0]);

    let currentCell = cell;
    for(let i = 1; i < keys.length; i++){
        const createdCell = createCell(keys[i],names[i],keys[i] === '[All]',names.slice(0,i+1).join('.'))
        currentCell.Children.push(createdCell);
        currentCell = createdCell;
    }
    return cell;
}

function mapTupleModel(tuple: MdxTupleModel, dataIndex: number): HeaderCellModel{
    const cell = createCells(tuple.Members[0]);

    let currentCell = cell;

    for(let i = 1; i < tuple.Members.length; i++){
        while(currentCell.Children.length) currentCell = currentCell.Children[0];
        
        const createdCell = createCells(tuple.Members[i]);
        currentCell.Children.push(createdCell);
        currentCell = createdCell;
    }

    while(currentCell.Children.length) currentCell = currentCell.Children[0];
    currentCell.DataIndex = dataIndex;

    return cell;
}

function mergeCells(parentCell: HeaderCellModel, mergedCell: HeaderCellModel){
    for(const cell of mergedCell.Children){
        const find = parentCell.Children.find(i => i.Key === cell.Key);
        if(!find){
            const isTotalChildIndex = parentCell.Children.findIndex(i => i.Key === '[All]');
            if(isTotalChildIndex !== -1){
                parentCell.Children.splice(isTotalChildIndex,0,cell);
            } else{
                parentCell.Children.push(cell);
            }
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

export default function(cubeData: MdxDataModel): MdxParseResult{
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