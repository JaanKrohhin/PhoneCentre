import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';

import en_translate from './locales/en/translation.json';
import ee_transltate from './locales/ee/translation.json';



i18n.use(initReactI18next)

    .init({
        resources: {
            en: {
                translation: en_translate
            },
            ee: {
                translation: ee_transltate
            }
        },
        lng:"en",
        debug: true,
        appendNamespaceToMissingKey: true,
        interpolation: {
            escapeValue: false, 
        }
    })


export default i18n;