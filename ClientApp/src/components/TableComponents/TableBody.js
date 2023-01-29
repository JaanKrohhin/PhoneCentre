import {HistoryItem} from "./HistoryItem";
import {TableItem} from "./TableItem";

const TableBody = ({ Data, IsHistory, IsMain }) => {
    return (
        <tbody>
        {
            Data.map((item) => {

                if (IsHistory){
                    return (
                        <HistoryItem item={item}/>
                    )
                }

                return (
                    <TableItem item={{item: item,type: IsMain ? "" : "detail"}}/>
                )

            })
        }
        </tbody>
    )
}

export default TableBody;