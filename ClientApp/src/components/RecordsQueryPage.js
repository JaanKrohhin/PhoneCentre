import React from "react";
import Table from "./TableComponents/Table";
import { withTranslation } from 'react-i18next';

//The main component that renders the table. Records Query
class RecordsQueryPage extends React.Component {


    columnNames = [this.props.t('table.columnNames.caller'), this.props.t('table.columnNames.event'), this.props.t('table.columnNames.receiver'), this.props.t('table.columnNames.timestamp')]
    title = this.props.t('pages.recordsQuery.title')
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
                DefaultExportHandleSource={this.exportSource }
                Title={this.title}
                ColumnNames={this.columnNames}
                RowSizes={this.rowSizes}
                CheckedStateOfEvents={this.checkedStateOfEvents}
            />
        );
    }

}


/*RecordsQueryPage = () => {

    const dataSource = (stateOfTable) => {
        let filter = stateOfTable.checkedState.join("-")
        return `events/${stateOfTable.selectedSize}/${stateOfTable.pageNumber}/${stateOfTable.sortColumnName}/${stateOfTable.sortDirection}+${stateOfTable.search}+${filter}`
    }

    const exportSource = (stateOfTable) => {
        let filter = stateOfTable.checkedState.join("-")
        return `events/download/${stateOfTable.sortColumnName}/${stateOfTable.sortDirection}+${stateOfTable.search}+${filter}`
    }

    return (
        <Table
            IsMainTable={true}
            RowsDataSource={this.dataSource}
            DefaultExportHandleSource={this.exportSource}
            Title={title}
            ColumnNames={columnNames}
            RowSizes={rowSizes}
            CheckedStateOfEvents={checkedStateOfEvents}
        />
    );
}*/


export default withTranslation()(RecordsQueryPage);