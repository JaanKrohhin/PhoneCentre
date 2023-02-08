import React from "react";
import Table from "./TableComponents/Table";

//Component responsible for call details page

const columnNames = ["Caller", "Event", "Receiver", "Timestamp"];

class CallDetailPage extends React.Component {
    //Defining the state
    constructor(props) {
        super(props);
        this.state = {
            data: null,
            id: window.location.pathname.split("/")[2],
        }
    }

    //fetching the data from the API on component mount
    componentDidMount() {
        fetch(`events/details/${this.state.id}`)
            .then(response => response.json())
            .then(fetchedData => {
                this.setState({ data: fetchedData});
            });

    }

        render() {
            if (this.state.data.length === 0){
                return <div>Loading...</div>
            }
            else {

                var title = `${this.state.data[0].call_.caller}#: ${this.state.data.length < 3 ? "Non-dialed call" : this.state.data.length < 5 ? "Cancelled call" : "Regular call"}`;

                return(
                    <Table RowsData={this.state.data}
                        Title={title}
                        ColumnNames={columnNames}
                        IsMainTable={false}
                        IsHistory={false}
                    />
                )
        }
    }
}
export default CallDetailPage;
