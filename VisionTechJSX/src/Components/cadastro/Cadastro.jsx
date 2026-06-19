import "./Cadastro.css";
import Botao from "../../components/botao/Botao";

const Cadastro = (props) => {

    const exibirCamposProduto = props.visibilidade !== "none";

    return (
        <section className="section_cadastro">
            <form onSubmit={props.funcCadastro} className="layout_grid form_cadastro">
                <h1>{props.nomeCadastro}</h1>
                <hr />
                <div className="campos_cadastro">

                    {/* Campo Nome */}
                    <div className="campo_cad_nome">
                        <label htmlFor="nome">Nome</label>
                        <input
                            type="text"
                            name="nome"
                            placeholder={`Digite o nome do(a) ${props.placeholder || 'produto'}`}
                            value={props.valor}
                            onChange={(e) => props.setValor(e.target.value)}
                        />
                    </div>

                    {/* Campo Imagem */}
                    {exibirCamposProduto && (
                        <div className="campo_cad_imagem">
                            <label htmlFor="imagem">Imagem</label>
                            <input
                                type="file"
                                name="imagem"
                                id="imagem"
                                accept="image/*"
                                onChange={(e) => props.setImagem(e.target.files[0])}
                            />
                        </div>
                    )}

                    {/* Campo Estoque Customizado */}
                    {exibirCamposProduto && (
                        <div className="campo_cad_estoque">
                            <label htmlFor="estoque">Quantidade em Estoque</label>

                            <div className="estoque_container">
                                {/* Botão de Menos */}
                                <button
                                    type="button"
                                    className="estoque_btn"
                                    onClick={() => props.setValorEstoque(Math.max(0, (props.valorEstoque || 0) - 1))}
                                >
                                    -
                                </button>

                                {/* Input numérico centralizado */}
                                <input
                                    type="number"
                                    name="estoque"
                                    id="estoque"
                                    min="0"
                                    className="estoque_input"
                                    value={props.valorEstoque ?? 0}
                                    onChange={(e) => {
                                        const val = parseInt(e.target.value, 10);
                                        props.setValorEstoque(isNaN(val) || val < 0 ? 0 : val);
                                    }}
                                />

                                {/* Botão de Mais */}
                                <button
                                    type="button"
                                    className="estoque_btn"
                                    onClick={() => props.setValorEstoque((props.valorEstoque || 0) + 1)}
                                >
                                    +
                                </button>
                            </div>
                        </div>
                    )}

                    {/* Campo Categoria */}
                    <div className="campo_cad_categoria" style={{ display: props.visibilidade }}>
                        <label htmlFor="categoria">Categoria</label>
                        <select name="categoria" id="categoria" value={props.valorCategoria || ""} onChange={(e) => props.setValorCategoria(e.target.value)}>
                            <option value="">Selecione</option>
                            {
                                props.listaCategorias?.map((item) => {
                                    return (
                                        <option key={item.idCategoria} value={item.idCategoria}>{item.nome}</option>
                                    )
                                })
                            }
                        </select>
                    </div>

                    {/* Botões */}
                    {props.btnEditar &&
                        <Botao
                            nomeDoBotao="Cancelar"
                            btnEditar={props.btnEditar}
                            cancelarEdicao={props.cancelarEdicao}
                        />}
                    <Botao nomeDoBotao={props.btnEditar ? "Salvar" : "Cadastrar"} />
                </div>
            </form>
        </section>
    )
}

export default Cadastro;