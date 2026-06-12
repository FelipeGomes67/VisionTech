import { useEffect, useState } from "react"
import { UsuarioContext } from "./UsuarioContext"

const UsuarioProvider = ({children}) => {
    const [usuario, setUsuario] = useState(null)


    useEffect(() => {
        const usuarioStorage = JSON.parse(localStorage.getItem("usuario")) || ""
        setUsuario(usuarioStorage)
    }, [])

        return(
            <UsuarioContext.Provider
                value={{
                    usuario, setUsuario
                }}
            >
                {children}
            </UsuarioContext.Provider>
        )
}

export default UsuarioProvider