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
    Sets: Array<MdxSetModel>,
    Values: Array<MdxValueModel>
}
export type HeaderCellModel = {
    IsTotal: boolean,
    Name: string,
    DisplayName: string,
    Key: string,
    Children: Array<HeaderCellModel>,
    DataIndex?: number,
    Hierarchy: string
}

export type ValueModel = MdxValueModel & {
    DataIndex: number
}

export type MdxParseResult = {
    Columns: HeaderCellModel,
    Rows: HeaderCellModel | null,
    Values: ValueModel[]
}

export type HeaderTr = {
    Cells: HeaderTd[]
}

export type HeaderTd = {
    Key: string,
    Name: string,
    DisplayName: string,
    RowSpan: number,
    ColSpan: number,
    DataIndex?: number,
    IsTotal: boolean,
    Hierarchy: string
}
export type PivotData = {
    columns: HeaderTr[],
    rows?: HeaderTr[]
}