import { useTranslation } from "react-i18next";

export const ExportButton = ({ onClick }) => {
    const { t } = useTranslation()

    return (
        <button onClick={onClick} id={"exportBtn"}>{t('table.header.export')}</button>
    );
};
