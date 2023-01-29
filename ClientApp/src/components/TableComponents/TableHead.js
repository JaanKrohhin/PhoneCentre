import React from 'react';

const TableHead = ({ ColumnHeaders, HeaderClickHandle }) => {
    return(
        <thead>
        <tr className={"row-header-click"}>
            {
                ColumnHeaders.map((header, index) => {
                    return (

                        <th key={index}
                            onClick={index === 0 || index === 2 ? () => HeaderClickHandle({column: header}) : undefined}>{header}</th>
                    );
                })
            }
        </tr>
        </thead>
    )
}

TableHead.defaultProps = {
    ColumnHeaders: ["Caller", "Event", "Receiver", "Timestamp"],
    HeaderClickHandle: () => {}
}

export default TableHead;