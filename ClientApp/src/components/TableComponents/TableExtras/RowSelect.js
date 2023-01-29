import React from 'react';
export const RowSelection = ({UpdateSelectedSize, rowSizes, selectedRowSize}) => {
    return (
        <div id={"row-selection"}>
            <select name="rowSize" id="rowSize" onChange={UpdateSelectedSize} value={selectedRowSize}>
                {rowSizes.map((size, index) => {
                    return <option key={index} value={size}>{size}</option>
                })}
            </select>
            <label htmlFor={"rowSize"}>Rows per page</label>
        </div>
    )
}
