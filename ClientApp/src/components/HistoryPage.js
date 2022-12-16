import React from "react";
import {HistoryItem} from "./HistoryItem";

//Component for the all calls page
class HistoryPage extends React.Component{

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
            let tableData = this.state.data;
            return (
                <div className="table-wrapper">
                    <h2>{this.state.data[0][0].call_.caller_}#: All calls</h2>
                    <table className="fl-table">
                        <thead>
                        <tr>
                            <th>Timestamp</th>
                            <th>Talk Duration</th>
                            <th>Receiver</th>
                            <th>Type</th>
                        </tr>
                        </thead>
                        <tbody>
                        {
                            tableData.map((itemArray) => {
                                return <HistoryItem item={itemArray} />
                            })
                        }
                        <tr className="options">
                            <td colSpan={4}>
                            </td>
                        </tr>
                        </tbody>
                    </table><br/>
                </div>

            )
        }
    }
}
export default HistoryPage;