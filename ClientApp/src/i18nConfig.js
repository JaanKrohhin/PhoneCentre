import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';

import en_translate from './locales/en/translation.json';
import ee_translate from './locales/ee/translation.json';

const resources = {
    en: {
        translation: en_translate
    },
    ee: {
        translation: ee_translate
    }
}
if (window.localStorage.defaultLanguage === undefined) {
    window.localStorage.defaultLanguage =  "en"
} 

i18n.use(initReactI18next)
    .init({
        returnObjects: true,
        resources,
        debug:false,
        lng: window.localStorage.defaultLanguage,
        fallbackLng: ["ee", "en"],
        react: {
            bindI18n: 'loaded languageChanged',
            bindI18nStore: 'added',
        },
        appendNamespaceToMissingKey: true,
        interpolation: {
            escapeValue: false, 
        }
    })


export default i18n;