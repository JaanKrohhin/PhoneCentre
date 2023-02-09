/* eslint-disable no-unused-vars */
import TableHead from "./TableHead";
import TableBody from "./TableBody";
import { SearchBar } from "./TableExtras/Search";
import { TableFooter } from "./TableExtras/TableFooter";
import React, { useEffect } from "react";
import { ExportButton } from "./TableExtras/ExportButton";
import { useState } from "react";

//The page to fall back on
const pageReset = 1;

export default function Table({ Title, RowsDataSource, HeaderClick, SearchHandler, FooterHandlers, IsMainTable, RowSizes, SelectedRowSize, CheckedStateOfEvents, ExportHandle, ColumnNames, IsHistory }) {
    const [state, setState] = useState({
        data: [],
        sortColumnName: "Caller",
        rowSizes: [5, 10, 25],
        selectedSize: 5,
        pageNumber: pageReset,
        sortDirection: "asc",
        search: "",
        checkedState: ["", "", "", "", ""]
    })

    let isMain = IsMainTable
    let title = Title
    let columnNames = ColumnNames
    let isHistory = IsHistory


    //Method, which retrieves the data from the backend. The data is then set to the state. It Is used after every state change
    const getEventData = (selectedSize, pageNumber, sortColumnName, sortDirection, search, eventType) => {
        let typeFilter = eventType.join("-")
        setState((prevState) => {
            let temp = prevState//Object.assign({}, prevState.state);
            temp.data = RowsDataSource(state)
            return temp
        });

    }

    useEffect(() => {
        getEventData(state.selectedSize, state.pageNumber
            , state.sortColumnName, state.sortDirection
            , state.search, state.checkedState)

    })

    //On click, fetches the sorted and filtered data as an .csv file
    const exportDataHandle = () => {

        let typeFilter = state.checkedState.join("-");

        window.open(`events/download/${state.sortColumnName}/${state.sortDirection}+${state.search}+${typeFilter}`, "_blank");
    }




    //On change, gets the new search value and fetches the data with the new search value. Always goes to the first page.
    const searchFor = (event) => {

        let value = event.target.value;

        setState((prevState) => {
            let temp = prevState
            temp.pageNumber = pageReset
            temp.search = value
            return temp
        });

        getEventData(state.selectedSize, state.pageNumber
            , state.sortColumnName, state.sortDirection
            , value, state.checkedState)
    }




    //On header "caller" click, sort the data by caller. On header "reciever" click, sort the data by reciever. A second click on the same header will reverse the sort order. Always fetches data
    const headerClickHandle = (event) => {

        let sortColumnName = event.column;

        let sortDirection = state.sortDirection === "asc" ? "desc" : "asc";

        setState((prevState) => {
            let temp = prevState
            temp.sortColumnName = sortColumnName
            temp.sortDirection = sortDirection
            return {
                sortColumnName: sortColumnName, sortDirection: sortDirection
            }
        });

        getEventData(state.selectedSize, state.pageNumber
            , sortColumnName, sortDirection
            , state.search, state.checkedState)
    }




    //All the event handlers needed for the footer binded to this page's data. Made into object for readability of the Table component.
    const footerHandlers = {



        //On selection, gets the new event type filter and fetches the data with the new filter. Always goes to the first page.
        eventFilterHandle: (index, event) => {
            let type = event.target.value;

            let newFilter = state.checkedState.map((item, i) => {
                if (i === index && (item === "" && event.target.checked)) {
                    return type;
                } else if (item === event.target.id) {
                    return "";
                } else {
                    return item;
                }
            });


            setState((prevState) => {
                let temp = prevState
                temp.checkedState = newFilter
                temp.pageNumber = pageReset
                return temp
            });

            getEventData(state.selectedSize, pageReset
                , state.sortColumnName, state.sortDirection
                , state.search, newFilter)
        },




        //On page number click, fetches the data for the next page. If the next page is not empty then the page number is incremented and data is updated. Else, stays on the same page
        nextPageHandle: () => {
            let value = state.pageNumber + 1;

            let typeFilter = state.checkedState.join("-");

            fetch(`events/${state.selectedSize}/${value}/${state.sortColumnName}/${state.sortDirection}+${state.search}+${typeFilter}`)
                .then(response => response.json())
                .then(nextPageData => {
                    if (nextPageData.length > 0) {
                        let newpageNumber = 0
                        setState((prevState) => {
                            newpageNumber = prevState.pageNumber + 1
                            var temp = prevState
                            temp.data = nextPageData
                            temp.pageNumber = newpageNumber
                            return temp
                        });
                    }
                });


        },



        //On click, fetches the last page. If the page number is 0 returns to the first page. Else, decrements the page number and updates the data
        previousPageHandle: () => {
            let value = 0;
            setState((prevState) => {
                value = prevState.pageNumber - 1 !== 0 ? prevState.pageNumber - 1 : 1;
                let temp = prevState
                temp.pageNumber = value
                return temp;
            });
            getEventData(state.selectedSize, value
                , state.sortColumnName, state.sortDirection
                , state.search, state.checkedState)
        },




        //On selection, sets the new size and fetches the data for the new size. Always goes to the first page.
        updateSelectedSize: (event) => {
            let size = parseInt(event.target.value, 10);

            setState((prevState) => {
                let temp = prevState
                temp.selectedSize = size
                temp.pageReset = pageReset
                return temp
            });
            getEventData(size, state.pageNumber
                , state.sortColumnName, state.sortDirection
                , state.search, state.checkedState)
        },


    }

    console.log(state)

    return (

        <div className="table-wrapper">
            {isMain ? <h1>{title}</h1> : <h2>{title}</h2>}
            {isMain ? <SearchBar OnChange={searchFor} /> : null}
            {isMain ? <ExportButton onClick={exportDataHandle} /> : null}
            <table className="fl-table main-table">
                <TableHead HeaderClickHandle={headerClickHandle} ColumnHeaders={columnNames} />
                <TableBody Data={state.data} IsHistory={isHistory} IsMain={isMain} />
                {isMain ? <TableFooter Handlers={footerHandlers} CheckedStateOfEvents={state.checkedState} RowSizes={state.rowSizes} SelectedRowSize={state.selectedSize} /> : null}
            </table>
            <br />
        </div>
    )
}
