import React from "react";
import Table from "./TableComponents/Table";

const columnNames = ['Caller', 'Event', 'Receiver', 'Timestamp']
const title = "Records Query"


//The main component that renders the table. Records Query
class RecordsQueryPage extends React.Component {


    dataSource = (arg) => {
        fetch(`events/${arg.selectedSize}/${arg.pageNumber}/${arg.sortColumnName}/${arg.sortDirection}+${arg.search}+${arg.typeFilter}`)
            .then(response => response.json())
            .then(fetchedData => {
                return fetchedData
            });
    }

    //The Table
    render() {

        return (
            <Table
                IsMainTable={true}
                DetailView={false}
                RowsDataSource={this.dataSource}
                Title={title}
                ColumnNames={columnNames}
            />
        );
    }




}
export default RecordsQueryPage;