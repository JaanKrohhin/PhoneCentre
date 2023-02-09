import React from "react";
import { events } from "../../eventTypes";
export const EventFilter = ({ EventFilterHandle, CheckedStateOfEvents }) => {
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
                        <label htmlFor={event.id}>{event.name}</label>
                    </div>
                );
            })}
        </div>
    );
};
