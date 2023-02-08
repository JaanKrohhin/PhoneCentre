import React from "react";
import Table from "./TableComponents/Table";

//Component for the all calls page
class HistoryOfCallsPage extends React.Component{

    //defining the state
    constructor(props) {
        super(props);
        this.state = {
            data: null,
            number: window.location.pathname.split("/")[2],
        }
    }


    componentDidMount() {
        fetch("events/history/" + this.state.number)
            .then(response => response.json())
            .then(data => this.setState({data: data}))
    }

    render() {
        if (this.state.data === null) {
            return (
                <div>
                    <h1>History</h1>
                    <p>Loading...</p>
                </div>
            )
        } else{


            var title = this.state.data[0][0].call_.caller+"#:All calls"
            var columnNames = ["Timestamp","Talk Duration","Receiver","Type"]

            return (
                <Table IsMainTable={false} RowsData={this.state.data} Title={title} ColumnNames={columnNames} IsHistory={true}/>
            )
        }
    }
}
export default HistoryOfCallsPage;

