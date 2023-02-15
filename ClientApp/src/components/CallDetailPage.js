import React from "react";
import Table from "./TableComponents/Table";
import { withTranslation } from 'react-i18next';
//Component responsible for call details page
class CallDetailPage extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            title: "",
            id: window.location.pathname.split("/")[2],
        }
    }
    columnNames = ['table.columnNames.caller', 'table.columnNames.event', 'table.columnNames.receiver', 'table.columnNames.timestamp']
    nonDialedCall = "callTypes.nonDialedCall"
    cancelledCall = "callTypes.cancelledCall"
    regularCall = "callTypes.regularCall"

    //fetching the data from the API on component mount
    dataSource = () => {
        return `events/details/${this.state.id}`
    }

    dataConn = (rowData) => {
        this.setState(() => {
            return {
                id: this.state.id,
                title: `${rowData[0].call_.caller}#${rowData.length < 3 ? this.nonDialedCall : rowData.length < 5 ? this.cancelledCall : this.regularCall}`

            }
        }) 
    }

    render() {
        return (
            <Table RowsDataSource={this.dataSource} Title={this.state.title} ColumnNames={this.columnNames} IsMainTable={false} IsHistory={false} RowSizes={[5, 10, 25]} DataConnection={ this.dataConn} />
        )

    }
}
export default withTranslation()(CallDetailPage);
