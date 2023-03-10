import {formatDate} from "./TableItem"
import { withTranslation } from 'react-i18next';

function HistoryItem({ item, t, i18n }) {
    //setiing the rwoItem from parameters
    const HistoryOfCallsArrayItem = item;
    function handleDate(){
        let pickUpIndex = 0
        HistoryOfCallsArrayItem.forEach((item, index) => {
            if(item.record_Event_Id.trim() === "EVENT_PICK_UP"){
                pickUpIndex = index
            }
        })
        return formatDate(HistoryOfCallsArrayItem[pickUpIndex].record_Date)
    }

    //Function for calculating the duration of the call
    function handleTime(){
        let callStart = 0;
        let callEnd = 0;


        HistoryOfCallsArrayItem.forEach((item) => {

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
        if (HistoryOfCallsArrayItem.length < 3){
            return t("callTypes.nonDialedCall")
        }else if (HistoryOfCallsArrayItem.length < 5){
            return t("callTypes.cancelledCall")
        }else{
            return t("callTypes.regularCall")
        }
    }

    let historyReceiver = HistoryOfCallsArrayItem[0].call_.receiver ? HistoryOfCallsArrayItem[0].call_.receiver : "---"


    return(
        <tr>
            <td>{handleDate()}</td>
            <td>{handleTime()}</td>
            <td>{historyReceiver}</td>
            <td>{handleType()}</td>
        </tr>
    )

}
export default withTranslation()(HistoryItem)