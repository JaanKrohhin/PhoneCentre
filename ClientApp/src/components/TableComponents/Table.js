import TableHead from "./TableHead";
import TableBody from "./TableBody";
import {SearchBar} from "./TableExtras/Search";
import {TableFooter} from "./TableExtras/TableFooter";
import React from "react";
import {ExportButton} from "./TableExtras/ExportButton";
export default function Table({Title, IsHistoryTable, RowsData, HeaderClick, SearchHandler, FooterHandlers, IsMainTable, RowSizes, SelectedRowSize, CheckedStateOfEvents, ExportHandle}) {
    let isMain = IsMainTable
    let title = Title
    let isHistory = IsHistoryTable
    let data = RowsData
    let columnNames = isHistory ? ["Timestamp","Talk Duration","Receiver","Type"] : ["Caller","Event","Receiver","Timestamp"]


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
/*
"Implement a flexible table component that can be reused on all pages,
all the markup and logic related to drawing the table needs to be in this component"

Quite frankly I knew this was a problem when I did this project.
I have always done poorly with frontend because I don't have a creative mind needed for design.
And React is definitely something I haven't used much. However, I am satisfied with this "rework" (at least more than the previous one).

*/