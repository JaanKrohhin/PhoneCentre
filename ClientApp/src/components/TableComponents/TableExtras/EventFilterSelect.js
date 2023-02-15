import React from "react";
import {events} from "../../eventTypes";
import { withTranslation } from 'react-i18next';

function EventFilter({ EventFilterHandle, CheckedStateOfEvents, t, i18n }) {

    return (
        <div id={"event-filter"}>
            {events.map((event, index) => {

                return (
                    <div key={index}>
                        <input
                            type="checkbox"
                            onChange={(event) => EventFilterHandle(index, event)}
                            value={event.id}
                            id={event.id}
                            name={"eventFilter"}
                            checked={CheckedStateOfEvents[index] !== ""}
                        />
                        <label htmlFor={event.id}>{t("events."+ event.name)}</label>
                    </div>
                );
            })}
        </div>
    );
};
export default withTranslation()(EventFilter)