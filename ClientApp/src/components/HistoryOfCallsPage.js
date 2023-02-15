import React from "react";
import Table from "./TableComponents/Table";
import { withTranslation } from 'react-i18next';

class HistoryOfCallsPage extends React.Component {
    columnNames = ['table.columnNames.timestamp', "table.columnNames.talkDuration","table.columnNames.receiver", "table.columnNames.type"]


    constructor(props) {
        super(props);
        this.state = {
            title: this.props.t('pages.historyOfCalls.title'),
            number: window.location.pathname.split("/")[2],
        }
    }


    dataSource = () => {

        return "events/history/" + this.state.number
    }

    dataConn = (rowData) => {
        this.setState(() => {
            return { ...this.state, title: rowData[0][0].call_.caller + "#" + 'pages.historyOfCalls.title' }
        })
    }
    render() {
        return (
            <Table IsMainTable={false} RowsDataSource={this.dataSource} Title={this.state.title} HeaderClick={() => { } } ColumnNames={this.columnNames} IsHistory={true} RowSizes={[5, 10, 25]} DataConnection={ this.dataConn} />
        )
    }
}
export default withTranslation()(HistoryOfCallsPage);

