import TableHead from "./TableHead";
import TableBody from "./TableBody";
import {SearchBar} from "./TableExtras/Search";
import {TableFooter} from "./TableExtras/TableFooter";
import React from "react";
import {ExportButton} from "./TableExtras/ExportButton";
export default function Table({Title, RowsData, HeaderClick, SearchHandler, FooterHandlers, IsMainTable, RowSizes, SelectedRowSize, CheckedStateOfEvents, ExportHandle, ColumnNames, IsHistory }) {
    let isMain = IsMainTable
    let title = Title
    let data = RowsData
    let columnNames = ColumnNames
    let isHistory = IsHistory

    return (
        <div className="table-wrapper">
            {isMain ? <h1>{title}</h1> : <h2>{title}</h2>}
            {isMain ? <SearchBar OnChange={SearchHandler} /> : null}
            {isMain ? <ExportButton onClick={ExportHandle} /> : null}
            <table className="fl-table main-table">
                <TableHead HeaderClickHandle={HeaderClick} ColumnHeaders = {columnNames} />
                <TableBody Data={data} IsHistory={isHistory} IsMain={isMain}/>
                {isMain ? <TableFooter Handlers={FooterHandlers} CheckedStateOfEvents={CheckedStateOfEvents} RowSizes={RowSizes} SelectedRowSize={SelectedRowSize} /> : null}
            </table>
            <br/>
        </div>
    )
}
