import { HistoryItem } from "./HistoryItem";
import { TableItem } from "./TableItem";

const TableBody = ({ Data, IsMain, IsHistory }) => {


    //IsHistory parameter is a boolean which has a value opposite to IsMain. There needs to be another variable that is able to determine
    if (Data === undefined || Data.length === 0) {
        return (
            <tbody>
                <tr>
                    <td className={"loadingDiv"} colSpan="4">
                        Loading...
                    </td>
                </tr>
            </tbody>
        );
    }

    return (
        <tbody>
            {Data.map((item, index) => {
                if (IsHistory) {
                    return <HistoryItem key={index} item={item} />;
                }

                return (
                    <TableItem key={index} item={{ item: item, type: IsMain ? "" : "detail" }} />
                );
            })}
        </tbody>
    );
};

export default TableBody;
