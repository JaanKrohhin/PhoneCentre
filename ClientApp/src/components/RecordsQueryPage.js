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
        let filter = "filter[]="+stateOfTable.checkedState.join("&filter[]=")
        return `events?rowSize=${stateOfTable.selectedSize}&pageNumber=${stateOfTable.pageNumber}&sortColumn=${stateOfTable.sortColumnName}&sortDirection=${stateOfTable.sortDirection}&searchString=${stateOfTable.search}&${filter}`
    }

    exportSource = (stateOfTable) => {
        let filter = "filter[]="+stateOfTable.checkedState.join("&filter[]=")
        return `events/download?sortColumn=${stateOfTable.sortColumnName}&sortDirection=${stateOfTable.sortDirection}&searchString=${stateOfTable.search}&${filter}`
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