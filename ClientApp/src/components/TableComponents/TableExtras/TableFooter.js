import {PageButtons} from "./PageButtons";
import React from "react";
import {RowSelection} from "./RowSelect"
import {EventFilter} from "./EventFilterSelect";
export const TableFooter = ({Handlers, RowSizes, SelectedRowSize, CheckedStateOfEvents}) => {
    const {updateSelectedSize, previousPageHandle, nextPageHandle, eventFilterHandle} = Handlers;

    return (
        <tfoot>
        <tr>
            <td colSpan={4}>
                <div className="table-footer">
                    <RowSelection rowSizes={RowSizes} selectedRowSize={SelectedRowSize} UpdateSelectedSize={updateSelectedSize} />
                    <PageButtons PreviousPageHandle={previousPageHandle} NextPageHandle={nextPageHandle} />
                    <EventFilter EventFilterHandle={eventFilterHandle} CheckedStateOfEvents={CheckedStateOfEvents}/>
                </div>
            </td>
        </tr>
        </tfoot>
    )
}