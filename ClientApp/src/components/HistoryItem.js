import {formatDate} from "./TableItem"

export const HistoryItem = (parameters) =>{

    //setiing the rwoItem from parameters
    const rowItem = parameters.item;


    function handleDate(){
        let pickUpIndex = 0
        rowItem.forEach((item, index) => {
            if(item.record_Event_Id.trim() === "EVENT_PICK_UP"){
                pickUpIndex = index
                console.log(formatDate(item.record_Date))
            }
        })
        return formatDate(rowItem[pickUpIndex].record_Date)
    }

    //Function for calculating the duration of the call
    function handleTime(){
        let callStart = 0;
        let callEnd = 0;


        rowItem.forEach((item) => {
            if(item.record_Event_Id.trim() === "EVENT_CALL_ESTABLISHED"){
                callStart = new Date(item.record_Date);
            }else if (item.record_Event_Id.trim() === "EVENT_CALL_END"){
                callEnd = new Date(item.record_Date);
            }
        })

        if (callStart === 0 || callEnd === 0){
            return "---"
        }
        let durationInMilliseconds = (callEnd - callStart);


        return millisToMinutesAndSeconds(durationInMilliseconds);
    }


    //Converts milliseconds to minutes and seconds
    function millisToMinutesAndSeconds(millis) {
        var minutes = Math.floor(millis / 60000);
        var seconds = ((millis % 60000) / 1000).toFixed(2);
        return minutes + " min " + (seconds < 10 ? '0' : '') + seconds + " sec";
    }


    //Sets the type for the call
    function handleType() {
        if (rowItem.length < 3){
            return "Non-dialled call"
        }else if (rowItem.length < 5){
            return "Cancelled call"
        }else{
            return "Regular call"
        }
    }
    return(
        <tr>
            <td>{handleDate()}</td>
            <td>{handleTime()}</td>
            <td>{rowItem[0].call_.receiver ? rowItem[0].call_.receiver : "---" }</td>
            <td>{handleType()}</td>
        </tr>
    )

}