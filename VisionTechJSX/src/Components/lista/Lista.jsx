import "./Lista.css";

// Importação de imagens:
import Editar from "../../assets/img/boxicons_edit-filled.svg";
import Excluir from "../../assets/img/gg_trash.svg";
import Visualizar from "../../assets/img/eye.svg";

const Lista = (props) => {

    const localAPIImagePath = "https://localhost:7285/imagens/";

    const exibirCamposProduto = props.tipoLista === "produto";

    const obterNomeCategoria = (item) => {
        if (item.categoria?.nome) {
            return item.categoria.nome;
        }
        if (item.idCategoriaNavigation?.nome) {
            return item.idCategoriaNavigation.nome;
        }

        if (props.listaCategorias && (item.idCategoria || item.categoria?.idCategoria)) {
            const idCat = item.idCategoria ?? item.categoria?.idCategoria;
            const categoria = props.listaCategorias.find((c) => c.idCategoria === idCat || c.id === idCat);
            return categoria?.nome || '-';
        }
        return '-';
    };

    return (
        <section className="layout_grid">
            <div className="listagem">

                <h1>{props.tituloLista}</h1>
                <hr />
                <div className="tabela">
                    <table>
                        <thead>
                            <tr className="table_cabecalho">
                                <th>Nome</th>
                                {exibirCamposProduto && <th>Categoria</th>}
                                {exibirCamposProduto && <th>Imagem</th>}
                                <th>Editar</th>
                                {props.fnResumo && <th>Resumo</th>}
                                <th>Excluir</th>
                            </tr>
                        </thead>
                        <tbody>
                            {props.lista && props.lista.length > 0 ? (
                                props.lista.map((item, index) => (
                                    <tr className="item_lista" key={item.id || item.idProduto || item.idCategoria || index}>

                                        <td data-cell="Nome">
                                            {item.nome}
                                        </td>

                                        {exibirCamposProduto && (
                                            <td data-cell="Categoria">
                                                {obterNomeCategoria(item)}
                                            </td>
                                        )}

                                        {exibirCamposProduto && (
                                            <td data-cell="Imagem">
                                                {item.imagem ? (
                                                    <img src={`${localAPIImagePath}${item.imagem}`} alt={item.nome} />
                                                ) : (
                                                    '-'
                                                )}
                                            </td>
                                        )}

                                        <td data-cell="Editar">
                                            <button className="icon" onClick={() => props.funcEditar(item)}>
                                                <img src={Editar} alt="Caneta" />
                                            </button>
                                        </td>

                                        {
                                            props.fnResumo && (
                                                <td data-cell="Editar">
                                                    <button className="icon" onClick={() => props.fnResumo(item)}>
                                                        <img src={Visualizar} alt="Visualizar" />
                                                    </button>
                                                </td>
                                            )
                                        }

                                        <td data-cell="Excluir">
                                            <button className="icon" onClick={() => props.funcExcluir(item)}>
                                                <img src={Excluir} alt="Lixeira" />
                                            </button>
                                        </td>
                                    </tr>
                                ))
                            ) : (
                                <tr>
                                    <td colSpan={exibirCamposProduto ? 5 : 3} style={{ textAlign: "center", padding: "20px" }}>
                                        Nenhum registro encontrado.
                                    </td>
                                </tr>
                            )}
                        </tbody>
                    </table>
                </div>
            </div>
        </section>
    );
};

export default Lista;