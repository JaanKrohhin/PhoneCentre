import React from "react";
import Table from "./TableComponents/Table";

const columnNames = ['Caller', 'Event', 'Receiver', 'Timestamp']
const title = "Records Query"
const rowSizes = [5,10,25]
const checkedStateOfEvents = ["","","","",""]
//The main component that renders the table. Records Query
class RecordsQueryPage extends React.Component {


    dataSource = (arg, data) => {
        fetch(`events/${arg.selectedSize}/${arg.pageNumber}/${arg.sortColumnName}/${arg.sortDirection}+${arg.search}+${arg.checkedState.join("-")}`)
            .then(response => response.json())
            .then(fetchedData => {
                data = fetchedData
                console.log(data)
                return fetchedData
            })
    }

    //The Table
    render() {
        return (
            <Table
                IsMainTable={true}
                RowsDataSource={this.dataSource}
                Title={title}
                ColumnNames={columnNames}
                RowSizes={rowSizes}
                CheckedStateOfEvents={checkedStateOfEvents}
            />
        );
    }




}
export default RecordsQueryPage;