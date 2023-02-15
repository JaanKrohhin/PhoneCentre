import React from 'react';
import { withTranslation } from 'react-i18next';
function TableHead({ ColumnHeaders, HeaderClickHandle, t }) {
    return(
        <thead>
        <tr className={"row-header-click"}>
            {
                ColumnHeaders.map((header, index) => {
                    return (

                        <th key={index}
                            onClick={index === 0 || index === 2 ? () => HeaderClickHandle({column: header}) : undefined}>{t(header)}</th>
                    );
                })
            }
        </tr>
        </thead>
    )
}

TableHead.defaultProps = {
    HeaderClickHandle: () => {}
}

export default withTranslation()(TableHead);