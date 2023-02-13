import React from "react";
import Table from "./TableComponents/Table";

const columnNames = ["Timestamp", "Talk Duration", "Receiver", "Type"]
//Component for the all calls page
class HistoryOfCallsPage extends React.Component {

    //defining the state
    constructor(props) {
        super(props);
        this.state = {
            title: "All calls",
            number: window.location.pathname.split("/")[2],
        }
    }


    dataSource = () => {

        return "events/history/" + this.state.number
    }

    dataConn = (rowData) => {
        this.setState(() => {
            return { ...this.state, title: rowData[0][0].call_.caller + "#:All calls" }
        })
    }
    render() {

        return (
            <Table IsMainTable={false} RowsDataSource={this.dataSource} Title={this.state.title} HeaderClick={() => { } } ColumnNames={columnNames} IsHistory={true} RowSizes={[5, 10, 25]} DataConnection={ this.dataConn} />
        )
    }
}
export default HistoryOfCallsPage;

