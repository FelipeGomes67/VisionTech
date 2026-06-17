import api from "./Services";

const API_KEY = "";

export const gerarResumo = async (titulo, descricao = "") => {
  try {
    const resposta = await api.post(
      "https://api.groq.com/openai/v1/chat/completions",
      {
        model: "llama-3.3-70b-versatile",
        messages: [
          {
            role: "user",
            content: `
              Faça um resumo simples e bem curto do produto abaixo.
              
              ATENÇÃO ÀS REGRAS DE FORMATAÇÃO:
              - Responda APENAS em texto puro (plain text).
              - Não use nenhum asterisco (* ou **) em momento algum.
              - Não use negrito, itálico ou listas com marcadores.
              - Escreva a resposta em um único parágrafo contínuo.

              Produto: ${titulo}
            `,
          },
        ],
      },
      {
        headers: {
          Authorization: `Bearer ${API_KEY}`,
          "Content-Type": "application/json",
        },
      },
    );

    return resposta.data.choices[0].message.content;
  } catch (erro) {
    console.error(erro);
    return "Erro ao gerar resumo.";
  }
};


//  Descrição:
// ${descricao}