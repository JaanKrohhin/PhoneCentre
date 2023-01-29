export const SearchBar = ({OnChange }) => {
    return (
        <div id={"searchBar"}>
            <input type={"text"} onChange={OnChange} placeholder={"Caller or Receiver number"}/>
        </div>
    )
}