import "./login.css";
import Botao from "../../components/botao/Botao";
import Logo from "../../assets/img/logo.png";
import { useContext, useEffect, useState } from "react";
import { UsuarioContext } from "../../context/UsuarioContext";
import { useNavigate } from "react-router-dom";
import { Alerta } from "../../components/alerta/Alerta";
import api from "../../services/Services";
import { jwtDecode } from "jwt-decode";

const Login = () => {
  const navigate = useNavigate();
  const { usuario, setUsuario } = useContext(UsuarioContext);
  const [novoUsuario, setNovoUsuario] = useState("");
  const [senha, setSenha] = useState("");

  useEffect(() => {
    const logado = JSON.parse(localStorage.getItem("usuario"));
    if (logado) {
      setUsuario(logado);
      navigate("/categorias");
    }
  }, []);



  const loginUser = async (e) => {
    e.preventDefault();

    const email = novoUsuario.trim();
    const senhaDigitada = senha.trim();

    if (senhaDigitada.length === 0 || email.length === 0) {
      Alerta({
        title: "Campo obrigatório",
        text: "Por favor, insira seu e-mail e senha antes de continuar.",
        icon: "warning",
        confirmButtonColor: "#CC3F55",
        confirmButtonText: "Ok",
      });
      return;
    }
    const dadosLogin = {
      Email: email,
      Senha: senhaDigitada,
    };

    try {
      const retornoAPI = await api.post("/Login", dadosLogin);

      const Token = retornoAPI.data.token;
      const usuarioDecoder = jwtDecode(Token);


      setUsuario(usuarioDecoder)
      localStorage.setItem("usuario", JSON.stringify(usuarioDecoder));
      setNovoUsuario("");
      setSenha("");
      Alerta({
        title: "Login bem-sucedido",
        text: "Bem-vindo de volta!",
        icon: "success",
        confirmButtonColor: "#CC3F55",
      });
      navigate("/generos");
    }
    catch (err) {
      console.error("Erro detalhado retornado da API:", err.response?.data);

      Alerta({
        title: "Erro no login",
        text: err.response?.data || "E-mail ou senha incorretos.",
        icon: "error",
        confirmButtonColor: "#CC3F55",
      });
    }
  };



  return (
    <>
      <main className="main_login">
        <div className="banner"></div>
        <section className="section_login">
          <img src={Logo} alt="Logo do Filmoteca" />

          <form onSubmit={loginUser} className="form_login">
            <h1>Login</h1>
            <div className="campos_login">
              <div className="campo_input">
                <label htmlFor="email">Email:</label>
                <input
                  type="email"
                  value={novoUsuario}
                  name="email"
                  placeholder="Digite seu e-mail"
                  onChange={(e) => setNovoUsuario(e.target.value)}
                />
              </div>
              <div className="campo_input">
                <label htmlFor="senha">Senha:</label>
                <input type="password" name="senha" value={senha} onChange={(e) => setSenha(e.target.value)} placeholder="Digite sua senha" />
              </div>
            </div>

            <Botao nomeDoBotao="Entrar" type="submit" />

          </form>
        </section>
      </main>
    </>
  );
};

export default Login;