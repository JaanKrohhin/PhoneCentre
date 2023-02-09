import React from "react";
import Table from "./TableComponents/Table";


const title = "Records Query"
const pageReset = 1;


//The main component that renders the table. Records Query
class RecordsQueryPage extends React.Component {
    static firstRender = true;
    //defining the state
    constructor(props) {
        super(props);
        this.state = {
            data: null,
            sortColumnName: "Caller",
            rowSizes: [5,10,25],
            selectedSize: 5,
            pageNumber: pageReset,
            sortDirection: "asc",
            search: "",
            checkedState: ["","","","",""]
        }
    }

    //Method, which retrieves the data from the backend. The data is then set to the state. It Is used after every state change
    getEventData(selectedSize, pageNumber, sortColumnName, sortDirection = this.state.sortDirection, search = this.state.search, eventType = this.state.checkedState) {
        let typeFilter = eventType.join("-");

        fetch(`events/${selectedSize}/${pageNumber}/${sortColumnName}/${sortDirection}+${search}+${typeFilter}`)
            .then(response => response.json())
            .then(fetchedData => {
                this.setState({ data: fetchedData});
            });
    }

    componentDidMount() {
        this.getEventData();
    }

    //On click, fetches the sorted and filtered data as an .csv file
    exportData = () => {

        let typeFilter = this.state.checkedState.join("-");

        window.open(`events/download/${this.state.sortColumnName}/${this.state.sortDirection}+${this.state.search}+${typeFilter}`, "_blank");
    }




    //On change, gets the new search value and fetches the data with the new search value. Always goes to the first page.
    searchFor = (event) => {

        let value = event.target.value;

        this.setState(() => {
            return {
                pageNumber: pageReset,
                search: value
            }
        });

        this.getEventData(this.state.selectedSize,this.state.pageNumber
            ,this.state.sortColumnName, this.state.sortDirection
            , value, this.state.checkedState.join("-"))
    }




    //On header "caller" click, sort the data by caller. On header "reciever" click, sort the data by reciever. A second click on the same header will reverse the sort order. Always fetches data
    headerClickHandle = (event) => {

        let sortColumnName = event.column;

        let sortDirection = this.state.sortDirection === "asc" ? "desc" : "asc";

        this.setState(() => {
            return {
                sortColumnName: sortColumnName, sortDirection: sortDirection
            }
        });

        this.getEventData(this.state.selectedSize,this.state.pageNumber
            ,sortColumnName, sortDirection
            , this.state.search, this.state.checkedState.join("-"))
    }




    //All the event handlers needed for the footer binded to this page's data. Made into object for readability of the Table component.
    footerHandlers = {



        //On selection, gets the new event type filter and fetches the data with the new filter. Always goes to the first page.
        eventFilterHandle: (index, event) => {
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


            this.setState(() => {
                return {
                    checkedState: newFilter,
                    pageNumber: pageReset
                }
            });

            this.getEventData(this.state.selectedSize,pageReset
                ,this.state.sortColumnName, this.state.sortDirection
                , this.state.search, newFilter.join("-"))
        },




        //On page number click, fetches the data for the next page. If the next page is not empty then the page number is incremented and data is updated. Else, stays on the same page
        nextPageHandle: () => {
            let value = this.state.pageNumber + 1;

            let typeFilter = this.state.checkedState.join("-");

            fetch(`events/${this.state.selectedSize}/${value}/${this.state.sortColumnName}/${this.state.sortDirection}+${this.state.search}+${typeFilter}`)
                .then(response => response.json())
                .then(nextPage => {
                    if (nextPage.length > 0) {
                        let newpageNumber = 0
                        this.setState((prevState) => {
                            newpageNumber = prevState.pageNumber + 1
                            return {
                                data: nextPage,
                                pageNumber: newpageNumber
                            };
                        });
                    }
                });


        },



        //On click, fetches the last page. If the page number is 0 returns to the first page. Else, decrements the page number and updates the data
        previousPageHandle: () => {
            this.setState((prevState) => {
                let value =
                    prevState.pageNumber - 1 !== 0 ? prevState.pageNumber - 1 : 1;
                this.getEventData(this.state.selectedSize,value
                    ,this.state.sortColumnName, this.state.sortDirection
                    , this.state.search, this.state.checkedState.join("-"))
                return {
                    pageNumber: value
                };
            });
        },




        //On selection, sets the new size and fetches the data for the new size. Always goes to the first page.
        updateSelectedSize: (event) => {
            let size = parseInt(event.target.value, 10);

            this.setState(() => {
                return {
                    selectedSize: size,
                    pageNumber: pageReset
                 }
            });
            this.getEventData(size,this.state.pageNumber
                ,this.state.sortColumnName, this.state.sortDirection
                , this.state.search, this.state.checkedState.join("-"))
        },


    }



    //The Table
    render() {
        if (this.state.data === null){
            return <div>Loading...</div>
        }

        let columnNames = ['Caller','Event', 'Receiver','Timestamp']

        return (
            <Table
                IsMainTable={true}
                DetailView={false}
                HeaderClick={this.headerClickHandle}
                RowsData={this.state.data}
                Title={title}
                SearchHandler={this.searchFor}
                CheckedStateOfEvents={this.state.checkedState}
                RowSizes={this.state.rowSizes}
                SelectedRowSize={this.state.selectedSize}
                FooterHandlers={this.footerHandlers}
                ExportHandle={this.exportData}
                ColumnNames={columnNames}
            />
        );
    }




}
export default RecordsQueryPage;