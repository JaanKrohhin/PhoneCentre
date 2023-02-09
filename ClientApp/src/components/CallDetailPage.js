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
            data: [],
            id: window.location.pathname.split("/")[2],
        }
    }

    //fetching the data from the API on component mount
    dataSource = () => {
        fetch(`events/details/${this.state.id}`)
            .then(response => response.json())
            .then(fetchedData => {
                title = `${fetchedData[0].call_.caller}#: ${fetchedData.length < 3 ? "Non-dialed call" : fetchedData.length < 5 ? "Cancelled call" : "Regular call"}`
                return fetchedData
            });

    }

    render() {

        return (
            <Table RowsData={this.dataSource} Title={title} ColumnNames={columnNames} IsMainTable={false} IsHistory={false} />
        )

    }
}
export default CallDetailPage;
