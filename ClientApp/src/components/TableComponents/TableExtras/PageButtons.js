import React from 'react';
export const PageButtons = ({PreviousPageHandle, NextPageHandle}) => {
    return (
        <div id={"page-btns"}>
            <button onClick={PreviousPageHandle} className={"page-btn"}>&#11164;</button>
            <button onClick={NextPageHandle} className={"page-btn"}>&#11166;</button>
        </div>
    )
}
