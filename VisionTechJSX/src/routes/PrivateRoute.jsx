import { useContext } from "react";
import { UsuarioContext } from "../context/UsuarioContext";
import { Navigate } from "react-router-dom";

const PrivateRoute = ({children}) => {
    const {usuario} = useContext(UsuarioContext)

    return usuario ? children : <Navigate to={"/"} replace/>
}

export default PrivateRoute