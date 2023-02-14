import React from "react";
import Table from "./TableComponents/Table";
import { withTranslation } from 'react-i18next';
import { useTranslation } from "react-i18next";
//Component responsible for call details page
class CallDetailPage extends React.Component {


    columnNames = [this.props.t('table.columnNames.caller'), this.props.t('table.columnNames.event'), this.props.t('table.columnNames.receiver'), this.props.t('table.columnNames.timestamp')]
    //Defining the state
    constructor(props) {
        super(props);
        this.state = {
            title: "",
            id: window.location.pathname.split("/")[2],
        }
    }

    //fetching the data from the API on component mount
    dataSource = () => {
        return `events/details/${this.state.id}`
    }

    dataConn = (rowData) => {
        this.setState(() => {
            return {
                id: this.state.id,
                //title: `${rowData[0].call_.caller}#: ${rowData.length < 3 ? this.prop.t("callTypes.nonDialedCall") : rowData.length < 5 ? this.prop.t("callTypes.cancelledCall") : this.prop.t("callTypes.regularCall")}`

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
