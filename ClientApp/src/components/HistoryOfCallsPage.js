import React from "react";
import Table from "./TableComponents/Table";

let title = "#:All calls"
const columnNames = ["Timestamp", "Talk Duration", "Receiver", "Type"]
//Component for the all calls page
class HistoryOfCallsPage extends React.Component {

    //defining the state
    constructor(props) {
        super(props);
        this.state = {

            number: window.location.pathname.split("/")[2],
        }
    }


    dataSource = () => {
        fetch("events/history/" + this.state.number)
            .then(response => response.json())
            .then(fetchedData => {
                title = fetchedData[0][0].call_.caller + "#:All calls"
                return fetchedData
            })
    }

    render() {

        return (
            <Table IsMainTable={false} RowsData={this.dataSource} Title={title} ColumnNames={columnNames} IsHistory={true} />
        )
    }
}
export default HistoryOfCallsPage;

