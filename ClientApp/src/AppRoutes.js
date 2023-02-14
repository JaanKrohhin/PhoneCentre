import CallDetailPage from "./components/CallDetailPage";
import HistoryOfCallsPage from "./components/HistoryOfCallsPage";
import RecordsQueryPage from "./components/RecordsQueryPage";

//Define the routes of the application
const AppRoutes = [
  {
    path: '/',
        element: <RecordsQueryPage useSuspense={true} />
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
