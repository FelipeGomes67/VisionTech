import "./Header.css";
import Logo from "../../assets/img/logo.png";
import Botao from "../../components/botao/Botao";
import { Link } from "react-router-dom";
import { useContext } from "react"
import { UsuarioContext } from "../../context/UsuarioContext"
import { useNavigate } from "react-router-dom";

const Header = () => {
  const { setUsuario } = useContext(UsuarioContext);
  const navigate = useNavigate();

  const logout = () => {
    setUsuario("")
    localStorage.removeItem("usuario")

    navigate("/");

  }

  return (
    <header>
      <div className="layout_grid cabecalho">
        <Link to="/">
          <img src={Logo} alt="Logo" />
        </Link>

        <nav className="nav_header">
          <Link className="link_header" to="/">Home</Link>
          <Link className="link_header" to="/produtos">Produtos</Link>
          <Link className="link_header" to="/categorias">Categorias</Link>
          
        {/* <button className="theme_toggle" onClick={toggleTheme}>
          {isDark ? "☀️ Light" : "🌙 Dark"}
        </button> */}

        <Botao nomeDoBotao="Sair" type="submit" onclick={() => logout()} />
        </nav>


      </div>
    </header>
  );
};

export default Header;