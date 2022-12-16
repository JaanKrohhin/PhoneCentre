import React from "react";
import {TableItem} from "./TableItem";

//Component responsible for call details page
class  Detail extends React.Component {
    //Defining the state
    constructor(props) {
        super(props);
        this.state = {
            data: [],
            id: window.location.pathname.split("/")[2],
        }
    }

    //fetching the data from the API on component mount
    componentDidMount() {
        fetch(`events/details/${this.state.id}`)
            .then(response => response.json())
            .then(item => {
                this.setState({data: item});
            });

    }

        render() {
            if (this.state.data.length === 0){
                return <div>Loading...</div>
            }else {
                return(
                    <div className="table-wrapper">

                        {/* Deciding the title of the page based on the number of events*/}

                        <h2>{this.state.data[0].call_.caller_}#: {this.state.data.length < 3 ? "Non-dialed call" : this.state.data.length < 5 ? "Cancelled call" : "Regular call"}</h2>
                        <table className="fl-table">
                            <thead>
                            <tr>
                                <th>Caller</th>
                                <th>Event</th>
                                <th>Receiver</th>
                                <th>Timestamp</th>
                            </tr>
                            </thead>
                            <tbody>
                            {

                                this.state.data.map((item) => {
                                    return <TableItem key={item.record_id} item={{item:item, type:"detail"}} />
                                })
                            }
                            </tbody>
                        </table><br/>
                    </div>
                )
        }
    }
}
export default Detail;