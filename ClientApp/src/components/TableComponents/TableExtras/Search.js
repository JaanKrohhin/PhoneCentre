import { withTranslation } from 'react-i18next';

function SearchBar({ t, OnChange  }){

    return (
        <div id={"searchBar"}>
            <input type={"text"} onChange={OnChange} placeholder={t('table.header.searchText')}/>
        </div>
    )
}
export default withTranslation()(SearchBar)