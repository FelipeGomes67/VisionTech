import "./CadastroProduto.css"
import Header from "../../Components/header/Header"
import Footer from "../../Components/footer/Footer"
import Cadastro from "../../Components/cadastro/Cadastro"
import Lista from "../../components/lista/Lista"
import { useEffect, useState } from "react"
import api from "../../services/Services"
import { gerarResumo } from "../../services/IAServices"
import { Alerta } from "../../components/alerta/Alerta"

const CadastroProduto = () => {

    const [valor, setValor] = useState("");
    const [idCategoria, setIdCategoria] = useState("");
    const [imagem, setImagem] = useState(null);
    const [listaProdutos, setListaProdutos] = useState([]);
    const [listaCategorias, setListaCategorias] = useState([]);
    const [showLoading, setShowLoading] = useState(false);

    const getCategorias = async () => {
        try {
            const retornoAPI = await api.get("/categoria");
            const dados = retornoAPI.data;
            setListaCategorias(dados);
        } catch (error) {
            Alerta({
                title: 'Cadastro de Produto',
                text: 'Problema ao carregar dados da api',
                icon: 'error',
                confirmButtonText: 'OK'
            })
        }
    }

    const getProdutos = async () => {
        try {
            const retornoAPI = await api.get("/produto");
            const dados = retornoAPI.data;
            setListaProdutos(dados);
        } catch (error) {
            Alerta({
                title: 'Cadastro de Produto',
                text: 'Problema ao carregar dados da api',
                icon: 'error',
                confirmButtonText: 'OK'
            })
        }
    }

    const cadastrarProduto = async (e) => {
        e.preventDefault();

        // 1. Validação de campos vazios (Usando Alerta em vez de Swal)
        if (valor.trim().length === 0) {
            Alerta({
                title: 'Cadastro de Produto',
                text: 'Preencha o campo de nome!',
                icon: 'error',
                confirmButtonText: 'OK'
            });
            return false;
        }
        if (!idCategoria) {
            Alerta({
                title: 'Cadastro de Produto',
                text: 'Por favor, selecione uma categoria',
                icon: 'warning',
                confirmButtonText: 'OK'
            });
            return false;
        }

        try {
            // 2. Criação do FormData exigido pelo [FromForm] da API do C#
            const formData = new FormData();

            // Chaves mapeadas exatamente como o ProdutoDTO espera no C# (Nome, IdCategoria, Imagem)
            formData.append("Nome", valor.trim());
            formData.append("IdCategoria", idCategoria);

            // Se houver arquivo de imagem selecionado no estado do seu formulário, anexa ele
            if (imagem) {
                formData.append("Imagem", imagem);
            }

            // 3. Envio da requisição para a rota correta
            const retornoAPI = await api.post("/produto", formData);

            if (retornoAPI.status === 201 || retornoAPI.status === 200) {
                Alerta({
                    title: 'Cadastro de Produto',
                    text: `${valor} cadastrado com sucesso!`,
                    icon: 'success',
                    confirmButtonText: 'OK'
                });

                // Limpa os campos do formulário após o sucesso
                setValor("");
                setIdCategoria("");
                if (setImagem) setImagem(null); // Reseta o input de arquivo, se aplicável

                // Atualiza a listagem na tela
                getProdutos();
            } else {
                Alerta({
                    title: 'Cadastro de Produto',
                    text: 'Algum problema aconteceu ao cadastrar!',
                    icon: 'error',
                    confirmButtonText: 'OK'
                });
            }
        } catch (error) {
            console.error("Erro retornado pela API:", error.response?.data || error);

            // Pega os detalhes do BadRequest enviados pelo C# caso caia nas validações
            const mensagemErro = error.response?.data || 'Erro ao chamar a API no cadastro';

            Alerta({
                title: 'Cadastro de Produto',
                text: typeof mensagemErro === 'string' ? mensagemErro : 'Erro nos dados enviados.',
                icon: 'error',
                confirmButtonText: 'OK'
            });
        }
    };

    const preEditar = async (item) => {
        const idEditar = item.idProduto || item.id;

        // 1. Passo: Editar o Nome do Produto
        const resultadoNome = await Swal.fire({
            title: "Editar Produto",
            input: "text",
            inputLabel: "Nome do produto",
            inputValue: item.nome,
            showCancelButton: true,
            confirmButtonText: "Próximo",
            cancelButtonText: "Cancelar",
            confirmButtonColor: "#112b82",
            inputValidator: (value) => {
                if (!value || value.trim().length === 0)
                    return "Preencha o campo de nome!";
            },
        });

        if (!resultadoNome.value) return;
        const novoNome = resultadoNome.value;

        // Mapeia o array de categorias para o formato de objeto key/value do SweetAlert select
        const opcoesCategoria = {};
        listaCategorias.forEach((c) => {
            const id = c.idCategoria || c.id;
            opcoesCategoria[id] = c.nome;
        });

        const idCategoriaAtual = item.idCategoria || item.categoria?.idCategoria || "";

        // 2. Passo: Editar a Categoria do Produto
        const resultadoCategoria = await Swal.fire({
            title: "Editar Produto",
            input: "select",
            inputLabel: "Categoria do produto",
            inputValue: idCategoriaAtual,
            inputOptions: opcoesCategoria,
            showCancelButton: true,
            confirmButtonText: "Próximo",
            cancelButtonText: "Cancelar",
            confirmButtonColor: "#112b82",
            inputValidator: (value) => {
                if (!value) return "Selecione uma categoria!";
            },
        });

        if (!resultadoCategoria.value) return;
        const novoIdCategoria = resultadoCategoria.value;

        // 3. Passo: Mudar a Imagem do Produto (Opcional)
        const resultadoImagem = await Swal.fire({
            title: "Editar Imagem do Produto",
            input: "file",
            inputLabel: "Selecione uma nova imagem (Deixe vazio para manter a atual)",
            inputAttributes: {
                "accept": "image/*"
            },
            showCancelButton: true,
            confirmButtonText: "Salvar",
            cancelButtonText: "Cancelar",
            confirmButtonColor: "#112b82"
        });

        try {
            const formData = new FormData();
            formData.append("idProduto", idEditar);
            formData.append("nome", novoNome);
            formData.append("idCategoria", novoIdCategoria);

            // Se o usuário selecionou um novo arquivo de imagem, adiciona ao form, caso contrário mantém a antiga string
            if (resultadoImagem.value) {
                formData.append("imagem", resultadoImagem.value);
            } else {
                formData.append("imagem", item.imagem || "");
            }

            // Removido o "/api" manual para evitar o erro de rota duplicada (.../api/api/...)
            const retornoAPI = await api.put(`/Produto/${idEditar}`, formData);

            if (retornoAPI.status === 204 || retornoAPI.status === 200) {
                Swal.fire({
                    title: 'Edição de Produto',
                    text: 'Produto atualizado com sucesso!',
                    icon: 'success',
                    confirmButtonText: 'OK'
                });
                getProdutos();
            } else {
                Swal.fire({
                    title: 'Edição de Produto',
                    text: 'Algum problema aconteceu ao editar!',
                    icon: 'error',
                    confirmButtonText: 'OK'
                });
            }
        } catch (error) {
            console.error(error);
            Swal.fire({
                title: 'Edição de Produto',
                text: 'Erro ao chamar a API na edição',
                icon: 'error',
                confirmButtonText: 'OK'
            });
        }
    };

    const excluirProduto = async (item) => {
        const idExcluir = item.id || item.idProduto;

        const result = await Alerta({
            title: "Você tem certeza?",
            text: "Quer apagar o produto " + item.nome + "?",
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
            const retornoAPI = await api.delete(`/produto/${idExcluir}`);

            if (retornoAPI.status === 204 || retornoAPI.status === 200) {
                Alerta({
                    title: 'Excluir Produto',
                    text: 'Produto excluído com sucesso!',
                    icon: 'success',
                    confirmButtonText: 'OK'
                });
                getProdutos();
            } else {
                Alerta({
                    title: 'Excluir Produto',
                    text: 'Algum problema aconteceu ao excluir!',
                    icon: 'error',
                    confirmButtonText: 'OK'
                });
            }
        } catch (error) {
            console.log(error);
            Alerta({
                title: 'Excluir Produto',
                text: 'Erro ao chamar a API na exclusão',
                icon: 'error',
                confirmButtonText: 'OK'
            });
        }
    }

    const resumoDoProduto = async (produto) => {
        setShowLoading(true);

        try {
            const resumoIA = await gerarResumo(produto.nome);

            setShowLoading(false);

            Alerta({
                title: `Resumo de ${produto.nome}`,
                text: resumoIA,
                icon: 'info',
                confirmButtonText: 'OK'
            });
        } catch (error) {
            Alerta({
                title: `Resumo de ${produto.nome}`,
                text: 'Erro ao gerar resumo.',
                icon: 'error',
                confirmButtonText: 'OK'
            });
            console.log(error);
            setShowLoading(false);
        }
    }



    useEffect(() => {
        getCategorias();
        getProdutos();
    }, [])

    return (
        <>
            <Header />

            <main>
                <Cadastro
                    nomeCadastro="Cadastro de Produto"
                    funcCadastro={cadastrarProduto}
                    valor={valor}
                    setValor={setValor}
                    valorCategoria={idCategoria}
                    setValorCategoria={setIdCategoria}
                    listaCategorias={listaCategorias}
                    setImagem={setImagem}
                />

                <Lista
                    tituloLista="Lista de Produtos"
                    lista={listaProdutos}
                    tipoLista="produto"
                    funcExcluir={excluirProduto}
                    funcEditar={preEditar}
                    listaCategorias={listaCategorias}
                    fnResumo={resumoDoProduto}
                />
            </main>

            <Footer />
        </>
    )
}

export default CadastroProduto;