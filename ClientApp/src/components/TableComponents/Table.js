
import TableHead from "./TableHead";
import TableBody from "./TableBody";
import { SearchBar } from "./TableExtras/Search";
import { TableFooter } from "./TableExtras/TableFooter";
import React, { useEffect } from "react";
import { ExportButton } from "./TableExtras/ExportButton";
import { useState } from "react";

//The page to fall back on
const pageReset = 1;

export default function Table({ Title, RowsDataSource, DefaultExportHandleSource, HeaderClick, SearchHandler, FooterHandlers, IsMainTable, RowSizes, SelectedRowSize, CheckedStateOfEvents, ExportHandle, ColumnNames, IsHistory, DataConnection }) {


    const [state, setState] = useState({
        sortColumnName: "Caller",
        rowSizes: RowSizes,
        selectedSize: SelectedRowSize === undefined ? RowSizes[0] : SelectedRowSize,
        pageNumber: pageReset,
        sortDirection: "asc",
        search: "",
        checkedState: CheckedStateOfEvents
    })
    const [rowData, setData] = useState([])
    let isMain = IsMainTable
    let title = Title
    let columnNames = ColumnNames
    let isHistory = IsHistory


    const updateState = (sortColumnName, selectedSize, pageNumber, sortDirection, search, checkedState) => {

        setState((prevState) => {

            let temp = { ...prevState }

            temp.sortColumnName = sortColumnName
            temp.selectedSize = selectedSize
            temp.pageNumber = pageNumber
            temp.sortDirection = sortDirection
            temp.search = search
            temp.checkedState = checkedState

            fetchDataFromApi(temp)
            return temp
        })
    }

    useEffect(() => {
        fetchDataFromApi(state)
    }, [])


    const fetchDataFromApi = (passedState) => {

        fetch(RowsDataSource(passedState))
            .then(response => response.json())
            .then(fetchedData => {
                if (DataConnection !== undefined) {
                    DataConnection(fetchedData)
                }

                setData(() => {
                    return fetchedData
                })
            })

    }


    
    //On click, fetches the sorted and filtered data as an .csv file
    const exportDataHandle = ExportHandle === undefined ? () => {
        const link = document.createElement("a")
        fetch(DefaultExportHandleSource(state)).then(response => response.blob())
            .then( responseBlob => {
                link.href = URL.createObjectURL(responseBlob)
                console.log(link)
                link.target = "_blank"
                link.download = "a.csv"
                link.click()
                }
            )
        window.open(DefaultExportHandleSource(state), "_blank");

    } : ExportHandle(state)


    //On change, gets the new search value and fetches the data with the new search value. Always goes to the first page.
    const searchFor = SearchHandler === undefined ? (event) => {

        let searchValue = event.target.value;

        updateState(state.sortColumnName, state.selectedSize, pageReset, state.sortDirection, searchValue, state.checkedState)

    } : SearchHandler(state)


    //On header "caller" click, sort the data by caller. On header "reciever" click, sort the data by reciever. A second click on the same header will reverse the sort order. Always fetches data
    const headerClickHandle = HeaderClick === undefined ? (event) => {
        let sortColumnName = event.column;

        let sortDirection = state.sortDirection === "asc" ? "desc" : "asc";

        updateState(sortColumnName, state.selectedSize, state.pageNumber, sortDirection, state.search, state.checkedState)

    } : HeaderClick(state)

    



    //All the event handlers needed for the footer binded to this page's data. Made into object for readability of the Table component.
    const footerHandlers = FooterHandlers === undefined ? {


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

            updateState(state.sortColumnName, state.selectedSize, pageReset, state.sortDirection, state.search, newFilter)

        },


        //On page number click, fetches the data for the next page. If the next page is not empty then the page number is incremented and data is updated. Else, stays on the same page
        nextPageHandle: () => {
            let nextPageNumber = state.pageNumber + 1;

            var temp = { ...state }
            temp.pageNumber = nextPageNumber
            fetch(RowsDataSource(temp))

                .then(response => response.json())

                .then(nextPageData => {

                    console.table(nextPageData)
                    if (nextPageData.length > 0) {
                        console.log("Passed")
                        setState(() => {                        
                            return temp
                        })
                        setData(() => {
                            return  nextPageData
                        })
                    }

                });


        },

        

        //On click, fetches the last page. If the page number is 0 returns to the first page. Else, decrements the page number and updates the data
        previousPageHandle: () => {
            let backPageNumber = state.pageNumber - 1 !== 0 ? state.pageNumber - 1 : 1;

            updateState(state.sortColumnName, state.selectedSize, backPageNumber, state.sortDirection, state.search, state.checkedState)

        },


        //On selection, sets the new size and fetches the data for the new size. Always goes to the first page.
        updateSelectedSize: (event) => {

            let newSize = parseInt(event.target.value, 10);

            updateState(state.sortColumnName, newSize, pageReset, state.sortDirection, state.search, state.checkedState)

        },


    } : FooterHandlers(state)



    return (
        <div className="table-wrapper">
            {isMain ? <h1>{title}</h1> : <h2>{title}</h2>}
            {isMain ? <SearchBar OnChange={searchFor} /> : null}
            {isMain ? <ExportButton onClick={exportDataHandle} /> : null}
            <table className="fl-table main-table">
                <TableHead HeaderClickHandle={headerClickHandle} ColumnHeaders={columnNames} />
                <TableBody Data={rowData} IsHistory={isHistory} IsMain={isMain} />
                {isMain ? <TableFooter Handlers={footerHandlers} CheckedStateOfEvents={state.checkedState}
                    RowSizes={state.rowSizes} SelectedRowSize={state.selectedSize} /> : null}
            </table>
            <br />
        </div>
    )
}
