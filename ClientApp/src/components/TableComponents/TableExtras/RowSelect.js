import React from 'react';
import { useTranslation } from "react-i18next";

export const RowSelection = ({ UpdateSelectedSize, rowSizes, selectedRowSize }) => {

    const { t } = useTranslation();


    return (
        <div id={"row-selection"}>
            <select name="rowSize" id="rowSize" onChange={UpdateSelectedSize} value={selectedRowSize}>
                {rowSizes.map((size, index) => {
                    return <option key={index} value={size}>{size}</option>
                })}
            </select>
            <label htmlFor={"rowSize"}>{ t('table.footer.rowsPerPage')}</label>
        </div>
    )
}
