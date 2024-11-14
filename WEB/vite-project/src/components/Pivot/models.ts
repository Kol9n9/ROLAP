export type MdxMemberModel = {
    Key: string,
    Name: string
}
export type MdxTupleModel = {
    Members: Array<MdxMemberModel>
}
export type MdxSetModel = {
    Tuples: Array<MdxTupleModel>
}
export type MdxValueModel = {
    Value: number,
    FormattedValue: string
}
export type MdxDataModel = {
    Sets: Array<MdxDataModel>,
    Values: Array<MdxDataModel>
}
export type HeaderCellModel = {
    IsTotal: boolean,
    Name: string,
    Key: string,
    Children: Array<HeaderCellModel>,
    DataIndex?: number
}

export type ValueModel = MdxValueModel & {
    DataIndex: number
}

export type MdxParseResult = {
    Columns: HeaderCellModel,
    Rows: HeaderCellModel | null,
    Values: ValueModel[]
}