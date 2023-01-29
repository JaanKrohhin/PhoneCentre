import Table from "./components/RecordsQueryPage";
import CallDetailPage from "./components/CallDetailPage";
import HistoryOfCallsPage from "./components/HistoryOfCallsPage";

//Define the routes of the application
const AppRoutes = [
  {
    path: '/',
    element: <Table />
  },
  {
    path: 'details/:id',
    element: <CallDetailPage />
  },
  {
    path: 'history/:id',
    element: <HistoryOfCallsPage />
  }
];

export default AppRoutes;
