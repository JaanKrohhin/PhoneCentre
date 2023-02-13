import React from "react";
import Table from "./TableComponents/Table";

const columnNames = ["Caller", "Event", "Receiver", "Timestamp"];
let title = `Call`;
//Component responsible for call details page
class CallDetailPage extends React.Component {
    //Defining the state
    constructor(props) {
        super(props);
        this.state = {
            title: "",
            id: window.location.pathname.split("/")[2],
        }
    }

    //fetching the data from the API on component mount
    dataSource = () => {
        return `events/details/${this.state.id}`
    }

    dataConn = (rowData) => {
        console.log(rowData[0])
        this.setState(() => {
            return {
                id: this.state.id,
                title: `${rowData[0].call_.caller}#: ${rowData.length < 3 ? "Non-dialed call" : rowData.length < 5 ? "Cancelled call" : "Regular call"}`

            }
        }) 
    }

    render() {

        return (
            <Table RowsDataSource={this.dataSource} Title={this.state.title} ColumnNames={columnNames} IsMainTable={false} IsHistory={false} RowSizes={[5, 10, 25]} DataConnection={ this.dataConn} />
        )

    }
}
export default CallDetailPage;
