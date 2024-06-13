import Registration from './Components/Registration';
import Login from './Components/Login';
import {
    createBrowserRouter,
    RouterProvider,
  } from "react-router-dom";
  const router = createBrowserRouter([
    {
        path: "/",
        element: <> <Login/></>
      },
    {
      path: "/login",
      element:  <><Login/></>
    },
    {
      path: "/Register",
      element:<><Registration/></> 
    },
  ]);
  

function App() {
    return (
       <>
        <RouterProvider router={router} />
       </>
    );

}

export default App;