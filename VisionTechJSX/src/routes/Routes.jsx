import { BrowserRouter, Route, Routes } from "react-router-dom";
import Login from "../pages/login/Login";
import CadastroProduto from "../pages/cadastroProduto/CadastroProduto";
import PrivateRoute from "../routes/PrivateRoute";
import CadastroCategoria from "../pages/cadastroCategoria/CadastroCategoria";

const Rotas = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Login />} />


        <Route path="/produtos"
          element={
            <PrivateRoute>
              <CadastroProduto />
            </PrivateRoute>
          } />


        <Route path="/categorias"
          element={
            <PrivateRoute>
              <CadastroCategoria />
            </PrivateRoute>
          }
        />

      </Routes>
    </BrowserRouter>
  );
};

export default Rotas;