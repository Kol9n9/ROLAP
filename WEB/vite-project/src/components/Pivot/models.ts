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