import Table from "./components/Table";
import Detail from "./components/Detail";
import HistoryPage from "./components/HistoryPage";

//Define the routes of the application
const AppRoutes = [
  {
    path: '/',
    element: <Table />
  },
  {
    path: 'details/:id',
    element: <Detail />
  },
  {
    path: 'history/:id',
    element: <HistoryPage />
  }
];

export default AppRoutes;
