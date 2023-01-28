import React from "react";
import {TableItem} from "./TableItem";
import {events} from "./eventTypes";

//The main component that renders the table. Records Query
class Table extends React.Component {
    //defining the state
    constructor(props) {
        super(props);
        this.state = {
            data: null,
            sortColumnName: "Caller_",
            rowSizes: [5,10,25],
            selectedSize: 5,
            pageNumber: 1,
            sortDirection: "asc",
            search: "",
            checkedState: ["","","","",""]
        }
    }

    //On header "caller" click, sort the data by caller. On header "reciever" click, sort the data by reciever. A second click on the same header will reverse the sort order. Always fetches data
    headerClickHandle = (event) => {
        let sortColumnName = event.column;
        let sortDirection = this.state.sortDirection === "asc" ? "desc" : "asc";

        this.setState({sortColumnName: sortColumnName, sortDirection: sortDirection});
        this.getEventData(this.state.selectedSize, this.state.pageNumber, sortColumnName, sortDirection);
    }

    //On page number click, fetches the data for the next page. If the next page is not empty then the page number is incremented and data is updated. Else, stays on the same page
    nextPage = () => {
        let value = this.state.pageNumber + 1;

        let typeFilter = this.state.checkedState.join("-");

        fetch(`events/${this.state.selectedSize}/${value}/${this.state.sortColumnName}/${this.state.sortDirection}+${this.state.search}+${typeFilter}`)
            .then(response => response.json())
            .then(nextPage => {
                if (nextPage.length > 0) {
                    this.setState({data: nextPage, pageNumber: value});
                }
            });
    };


    //On click, fetches the last page. If the page number is 0 returns to the first page. Else, decrements the page number and updates the data
    previousPage = () => {
        let value = this.state.pageNumber - 1 !== 0 ? this.state.pageNumber - 1 : 1

        this.setState((state) => ({
            pageNumber: value
        }))
        this.getEventData(this.state.selectedSize,value, this.state.sortColumnName);
    };

    //On selection, sets the new size and fetches the data for the new size. Always goes to the first page.
    updateSelectedSize = (event) => {
        let size = parseInt(event.target.value);

        this.setState((state) => ({
                selectedSize: size,
                pageNumber: 1
        }));

        this.getEventData(size, 1, this.state.sortColumnName);
    }

    //Method, which retrieves the data from the server. The data is then set to the state. Is used after every state change(I had problems with the state not updating properly and this solution worked)
    getEventData(selectedSize, pageNumber, sortColumnName, sortDirection = this.state.sortDirection, search = this.state.search, eventType = this.state.checkedState) {
        let typeFilter = eventType.join("-");

        fetch(`events/${selectedSize}/${pageNumber}/${sortColumnName}/${sortDirection}+${search}+${typeFilter}`)
            .then(response => response.json())
            .then(data => {{
                this.setState({data: data});
            }
            });
    }

    //On click, fetches the sorted and filtered data as an .csv file
    exportData(){
        let typeFilter = this.state.checkedState.join("-");
        window.open(`events/download/${this.state.sortColumnName}/${this.state.sortDirection}+${this.state.search}+${typeFilter}`, "_blank");
    }


    componentDidMount() {
        this.getEventData(this.state.selectedSize,this.state.pageNumber, this.state.sortColumnName, this.state.sortDirection, this.state.search, this.state.checkedState);
    }


    //The Table
    render() {
        if (this.state.data === null){
            return <div>Loading...</div>
        }
        return (
            <div className="table-wrapper">
                <h1>Records Query</h1>
                <div id={"searchBar"}>
                    <input type={"text"} onChange={event => this.searchFor(event)} placeholder={"Caller or Receiver number"}/>
                </div>
                <button onClick={() => this.exportData()} id={"exportBtn"}>Export</button>
                <table className="fl-table main-table">
                    <thead>
                    <tr className={"row-header-click"}>
                        <th onClick={() => this.headerClickHandle({column:"Caller_"})}>Caller</th>
                        <th>Event</th>
                        <th onClick={() => this.headerClickHandle({column:"Receiver"})}>Receiver</th>
                        <th>Timestamp</th>
                    </tr>
                    </thead>
                    <tbody>
                    {
                        this.state.data.map((item,index) => {
                            return (
                                //The TableItem component is used to render the data
                                <TableItem item={{item}}/>
                            )
                        })
                    }
                    </tbody>
                    <tfoot>
                    <tr>
                        <td colSpan={4}>
                            <div className="table-footer">
                                <div id={"row-selection"}>
                                    <select name="rowSize" id="rowSize" onChange={this.updateSelectedSize } value={this.state.selectedSize}>
                                        {this.state.rowSizes.map((size,index) => {
                                            return <option key={index} value={size}>{size}</option>
                                        })}
                                    </select><label htmlFor={"rowSize"}>Rows per page</label>

                                </div>
                                <div id={"page-btns"}>
                                        <button onClick={this.previousPage} className={"page-btn"}>&#11164;</button>
                                        <button onClick={this.nextPage} className={"page-btn"}>&#11166;</button>
                                </div>
                                <div id={"event-filter"}>
                                    {events.map((event,index) => {

                                        return (<div>
                                            <input type="checkbox" onChange={(event) => this.handleFilter(index, event) } value={event.id} id={event.id} name={"eventFilter"} checked={this.state.checkedState[index] !== ""}/>
                                            <label htmlFor={event.id}>{event.name}</label>
                                        </div>)

                                    })}
                                </div>
                            </div>
                        </td>
                    </tr>
                    </tfoot>
                </table><br/>
            </div>
        );
  }
    //On selection, gets the new event type filter and fetches the data with the new filter. Always goes to the first page.
    handleFilter(index, event) {
        let type = event.target.value;

        let newFilter = this.state.checkedState.map((item, i) => {
            if (i === index && ( item === "" && event.target.checked )) {
                return type;
            }else if (item === event.target.id) {
                return "";
            }else {
                return item;
            }
        });


        this.setState({
            checkedState: newFilter,
            pageNumber: 1
        });
        this.getEventData(this.state.selectedSize, 1, this.state.sortColumnName, this.state.sortDirection, this.state.search, newFilter);
    }

    //On change, gets the new search value and fetches the data with the new search value. Always goes to the first page.
    searchFor(event) {
        let value = event.target.value;
        this.setState(() => ({
            pageNumber: 1,
            search: value
        }));
        this.getEventData(this.state.selectedSize, 1, this.state.sortColumnName, this.state.sortDirection, value);
    }
}
export default Table;