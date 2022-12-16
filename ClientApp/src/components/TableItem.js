import React from "react";

export const TableItem = (parameters) =>{
    const rowItem = parameters.item.item;
    const rowType = parameters.item.type;

    //Handles the redirection to the detail page and all calls page
    const routeChange = (e) =>{
        if (e.target.dataset.caller === "true"){
            let path = `history/${rowItem.call_.caller_}`;
            window.open(path, "_blank");
        }else {
            let path = `details/${rowItem.call_Id}`;
            window.open(path, "_blank");
        }
    }


    //Return different table rows depending on the type of the row
    if (rowType === "detail" ){
        return (
            <tr>
                <td>{rowItem.call_.caller_ ? rowItem.call_.caller_ : "---" }</td>
                <td>{rowItem.event_Type.event_Type}</td>
                <td>{rowItem.call_.receiver ? rowItem.call_.receiver : "---"}</td>
                <td>{formatDate(rowItem.record_Date)}</td>
            </tr>
        )
    }
    else {
        return (
            <tr onClick={(e) => routeChange(e)}>
                <td data-caller="true">{rowItem.call_.caller_ ? rowItem.call_.caller_ : "---"}</td>
                <td data-caller="false">{rowItem.event_Type.event_Type}</td>
                <td data-caller="false">{rowItem.call_.receiver ? rowItem.call_.receiver : "---"}</td>
                <td data-caller="false">{formatDate(rowItem.record_Date)}</td>
            </tr>
        )
    }
}


//Formats the date to the desired format. Is used in different components
export function formatDate(dateInput){
    let date = new Date(dateInput);
    let month = '' + (date.getMonth() + 1);
    let day = '' + date.getDate();
    let year = date.getFullYear();
    let hours = date.getHours();
    let minutes = date.getMinutes();
    let seconds = date.getSeconds();


    if (month.length < 2) month = '0' + month;

    if (day.length < 2) day = '0' + day;

    if (hours < 10) hours = '0' + hours;

    if (minutes < 10) minutes = '0' + minutes;

    if (seconds < 10) seconds = '0' + seconds;

    return [day, month, year].join('-') + ' ' + [hours, minutes, seconds].join(':');
}
