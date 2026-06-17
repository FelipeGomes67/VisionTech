import "./Botao.css"

const Botao = (props) => {
    
    const lidarComClique = (e) => {
        if (props.btnEditar && props.cancelarEdicao) {
            props.cancelarEdicao();
        }
        
        if (props.onClick) {
            props.onClick(e);
        }
    };

    return (
        <button
            className="botao" 
            type={props.type || (props.btnEditar ? "button" : "submit")}
            onClick={lidarComClique}
        >
            {props.nomeDoBotao}
        </button>
    );
};

export default Botao;