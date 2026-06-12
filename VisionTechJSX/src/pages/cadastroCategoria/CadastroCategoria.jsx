import "./CadastroCategoria.css";
import Header from "../../Components/header/Header";
import Footer from "../../Components/footer/Footer";
import Cadastro from "../../Components/cadastro/Cadastro";
import Lista from "../../components/lista/Lista";
import { useEffect, useState } from "react";
import api from "../../services/Services";
import { Alerta } from "../../components/alerta/Alerta";

const CadastroCategoria = () => {
  const [valor, setValor] = useState("");
  const [listaCategorias, setListaCategorias] = useState([]);

  const getCategorias = async () => {
    try {
      const retornoAPI = await api.get("/categoria");
      const dados = retornoAPI.data;
      setListaCategorias(dados);
    } catch (error) {
      Alerta({
        title: 'Cadastro de Categoria',
        text: 'Problema ao carregar dados da api',
        icon: 'error',
        confirmButtonText: 'OK'
      })
    }
  };

  const cadastrarCategoria = async (e) => {
    e.preventDefault();

    if (valor.trim().length === 0) {
      Alerta({
        title: 'Cadastro de Categoria',
        text: 'Preencha o campo de categoria!',
        icon: 'error',
        confirmButtonText: 'OK'
      });
      return false;
    }

    const objCadastro = {
      idCategoria: "",
      nome: valor,
    };

    try {
      const retornoAPI = await api.post("/categoria", objCadastro);

      if (retornoAPI.status === 201 || retornoAPI.status === 200) {
        Alerta({
          title: 'Cadastro de Categoria',
          text: `${valor} cadastrado com sucesso!`,
          icon: 'success',
          confirmButtonText: 'OK'
        });
        setValor("");
        getCategorias();
      } else {
        Alerta({
          title: 'Cadastro de Categoria',
          text: 'Algum problema aconteceu ao cadastrar!',
          icon: 'error',
          confirmButtonText: 'OK'
        });
      }
    } catch (error) {
      console.log(error);
      Alerta({
        title: 'Cadastro de Categoria',
        text: 'Erro ao chamar a API no cadastro',
        icon: 'error',
        confirmButtonText: 'OK'
      });
    }
  };

const preEditar = async (item) => {
    const idEditar = item.idCategoria || item.id;

    const resultado = await Alerta({
        title: "Editar Categoria",
        input: "text",
        inputLabel: "Nome da categoria",
        inputValue: item.nome,
        showCancelButton: true,
        confirmButtonText: "Salvar",
        cancelButtonText: "Cancelar",
        inputValidator: (value) => {
            if (!value || value.trim().length === 0)
                return "Preencha o campo de nome!";
        },
    });

    const novoNome = resultado?.value || resultado;

    if (!novoNome || typeof novoNome !== "string" || novoNome.trim().length === 0) return;

    const objEditar = {
        idCategoria: idEditar,
        nome: novoNome.trim()
    };

    try {
        const retornoAPI = await api.put(`/categoria/${idEditar}`, objEditar);

        if (retornoAPI.status === 204 || retornoAPI.status === 200) {
            Alerta({
                title: 'Edição de Categoria',
                text: 'Categoria atualizada com sucesso!',
                icon: 'success',
                confirmButtonText: 'OK'
            });
            
            getCategorias(); 
        } else {
            Alerta({
                title: 'Edição de Categoria',
                text: 'Algum problema aconteceu ao editar!',
                icon: 'error',
                confirmButtonText: 'OK'
            });
        }
    } catch (error) {
        console.error(error);
        Alerta({
            title: 'Edição de Categoria',
            text: 'Erro ao chamar a API na edição',
            icon: 'error',
            confirmButtonText: 'OK'
        });
    }
};

  const excluirCategoria = async (item) => {
    const idExcluir = item.id || item.idCategoria;

    const result = await Alerta({
      title: "Você tem certeza?",
      text: "Quer apagar a categoria " + item.nome + "?",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Apagar",
      cancelButtonText: "Cancelar",
    });

    if (!result.isConfirmed) {
      return;
    }

    try {
      const retornoAPI = await api.delete(`/categoria/${idExcluir}`);

      if (retornoAPI.status === 204 || retornoAPI.status === 200) {
        Alerta({
          title: 'Exclusão de Categoria',
          text: 'Categoria excluída com sucesso!',
          icon: 'success',
          confirmButtonText: 'OK'
        });
        getCategorias();
      } else {
        Alerta({
          title: 'Exclusão de Categoria',
          text: 'Não foi possível excluir a categoria.',
          icon: 'error',
          confirmButtonText: 'OK'
        });
      }
    } catch (error) {
      Alerta({
        title: 'Exclusão de Categoria',
        text: 'Ocorreu um erro ao excluir a categoria.',
        icon: 'error',
        confirmButtonText: 'OK'
      });
      console.log(error);
    }
  };

  useEffect(() => {
    getCategorias();
  }, []);

  return (
    <>
      <Header />
      <main>
        <Cadastro
          nomeCadastro="Cadastro de Categoria"
          placeholder="categoria"
          visibilidade="none"
          funcCadastro={cadastrarCategoria}
          valor={valor}
          setValor={setValor}
        />

        <Lista
          tituloLista="Lista de Categorias"
          visibilidade="none"
          lista={listaCategorias}
          tipoLista="categoria"
          funcExcluir={excluirCategoria}
          funcEditar={preEditar}
        />
      </main>
      <Footer />
    </>
  );
};

export default CadastroCategoria;