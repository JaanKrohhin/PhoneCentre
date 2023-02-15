import React from "react";
import Table from "./TableComponents/Table";
import { withTranslation } from 'react-i18next';

//The main component that renders the table. Records Query
class RecordsQueryPage extends React.Component {


    columnNames = ['table.columnNames.caller', 'table.columnNames.event', 'table.columnNames.receiver', 'table.columnNames.timestamp']
    title = 'pages.recordsQuery.title'
    rowSizes = [5, 10, 25]
    checkedStateOfEvents = ["", "", "", "", ""]

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
                DefaultExportHandleSource={this.exportSource}
                Title={this.title}
                ColumnNames={this.columnNames}
                RowSizes={this.rowSizes}
                CheckedStateOfEvents={this.checkedStateOfEvents}
            />
        );
    }

}

export default withTranslation()(RecordsQueryPage);