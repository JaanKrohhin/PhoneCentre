import React from "react";
import Table from "./TableComponents/Table";

const columnNames = ['Caller', 'Event', 'Receiver', 'Timestamp']
const title = "Records Query"
const rowSizes = [5,10,25]
const checkedStateOfEvents = ["","","","",""]
//The main component that renders the table. Records Query
class RecordsQueryPage extends React.Component {


    dataSource = (stateOfTable) => {
        let filter = stateOfTable.checkedState.join("-")
        return `events/${stateOfTable.selectedSize}/${stateOfTable.pageNumber}/${stateOfTable.sortColumnName}/${stateOfTable.sortDirection}+${stateOfTable.search}+${filter}`
    }
 
    exportSource = (stateOfTable) => {
        let filter = stateOfTable.checkedState.join("-")
        return `events/download/${stateOfTable.sortColumnName}/${stateOfTable.sortDirection}+${stateOfTable.search}+${filter}`
    }
    //The Table
    render() {
        return (
            <Table
                IsMainTable={true}
                RowsDataSource={this.dataSource}
                DefaultExportHandleSource={this.exportSource }
                Title={title}
                ColumnNames={columnNames}
                RowSizes={rowSizes}
                CheckedStateOfEvents={checkedStateOfEvents}
            />
        );
    }




}
export default RecordsQueryPage;