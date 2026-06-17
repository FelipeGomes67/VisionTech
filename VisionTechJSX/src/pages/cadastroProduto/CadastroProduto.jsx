import "./CadastroProduto.css"
import Header from "../../Components/header/Header"
import Footer from "../../Components/footer/Footer"
import Cadastro from "../../Components/cadastro/Cadastro"
import Lista from "../../components/lista/Lista"
import { useEffect, useState } from "react"
import api from "../../services/Services"
import { gerarResumo } from "../../services/IAServices"
import { Alerta } from "../../Components/alerta/Alerta"
import Swal from "sweetalert2"; 

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
        const idCategoriaAtual = item.idCategoria || item.categoria?.idCategoria || "";

        // Constrói as opções do select com a categoria atual pré-selecionada
        let selectOptionsHtml = `<option value="">Selecione uma categoria</option>`;
        listaCategorias.forEach((c) => {
            const id = c.idCategoria || c.id;
            const selected = id == idCategoriaAtual ? "selected" : "";
            selectOptionsHtml += `<option value="${id}" ${selected}>${c.nome}</option>`;
        });

        // 1. Popup único com múltiplos inputs usando HTML
        const { value: formValues } = await Swal.fire({
            title: "Editar Produto",
            html: `
                <div style="text-align: left; margin-bottom: 5px; font-size: 18px; font-weight: bold;">Nome do produto:</div>
                <input id="swal-input-nome" class="swal2-input" style="width: 80%; margin: 0 auto 20px;" value="${item.nome || ""}">
                
                <div style="text-align: left; margin-bottom: 5px; font-size: 18px; font-weight: bold;">Categoria do produto:</div>
                <select id="swal-input-categoria" class="swal2-select" style="display: flex; width: 80%; margin: 0 auto 20px;">
                    ${selectOptionsHtml}
                </select>

                <div style="text-align: left; margin-bottom: 5px; font-size: 18px; font-weight: bold;">Nova imagem (opcional):</div>
                <input type="file" id="swal-input-imagem" class="swal2-file" style="display: flex; width: 80%; margin: 0 auto;" accept="image/*">
            `,
            focusConfirm: false,
            showCancelButton: true,
            confirmButtonText: "Salvar",
            cancelButtonText: "Cancelar",
            confirmButtonColor: "#112b82",
            preConfirm: () => {
                const nome = document.getElementById("swal-input-nome").value;
                const idCategoria = document.getElementById("swal-input-categoria").value;
                const imagemInput = document.getElementById("swal-input-imagem");
                const imagem = imagemInput.files.length > 0 ? imagemInput.files[0] : null;

                if (!nome || nome.trim().length === 0) {
                    Swal.showValidationMessage("Preencha o campo de nome!");
                    return false;
                }
                if (!idCategoria) {
                    Swal.showValidationMessage("Selecione uma categoria!");
                    return false;
                }

                return { nome, idCategoria, imagem };
            }
        });

        // Se o usuário clicar em Cancelar, formValues será undefined
        if (!formValues) return;

        try {
            const formData = new FormData();
            formData.append("idProduto", idEditar);
            formData.append("nome", formValues.nome);
            formData.append("idCategoria", formValues.idCategoria);

            // Se o usuário selecionou um novo arquivo de imagem, adiciona ao form, caso contrário mantém a antiga string
            if (formValues.imagem) {
                formData.append("imagem", formValues.imagem);
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