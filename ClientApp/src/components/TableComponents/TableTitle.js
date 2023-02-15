import { withTranslation } from 'react-i18next';

function TableTitle({ IsMain, Title, t }) {
    if (IsMain) {
        return <h1>{t(Title)}</h1>
    } else {
        let firstHalf = Title.split('#')[0] + "#"
        let secondHalf = t(Title.split('#')[1])


        return <h2>{firstHalf + secondHalf}</h2>
    }
}
export default withTranslation()(TableTitle)