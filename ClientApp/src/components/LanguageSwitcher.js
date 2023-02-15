import { useState } from 'react';
import i18n from "../i18nConfig";

function LanguageSwitcher() {

    const onChange = (event) => {
        let lng = event.target.value
        i18n.changeLanguage(lng)
        setLang(i18n.language)
        window.localStorage.defaultLanguage = i18n.language
    }
    const [lang, setLang] = useState(i18n.language)

    return <select name="lng" id="lng" onChange={event => onChange(event)} value={lang }>
        {
            i18n.languages.map((item, i) => {
                return <option key={ i} value={item}>{item.toUpperCase()}</option>
            })
        }
    </select>

        
}
export default LanguageSwitcher