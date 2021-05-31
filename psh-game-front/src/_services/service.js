const urlBase = "https://localhost:44333/Stats";

export const service = {
  topAllTime,
  topLastTime,
}

/**
 * Comunicación con la API
 * @param {*} url
 * @param {*} method
 * @param {*} body
 * @param {*} contentType
 */
export async function callApi(url, method, body) {

  return fetch(url, {
    method,
    headers: new Headers({
      "Content-Type": "application/json",
    }),
    body: JSON.stringify(body),
  })
    .then(handleResponse)
    .catch(handleError);
}

// Manejo de Response
function handleResponse(response) {
  return response.text()
    .then((text) => {
      let data;

      // Parseo de la informacion devuelta por la api
      try {
        data = text && JSON.parse(text);
      } catch (e) {
        data = text;
      }

      // En caso de error se extrae el mensaje para mostrarlo
      // Se devuelve el mensaje de error enviado por la api
      if (!response.ok) {
        const error = [];
        error["msg"] = !data ? { msg: response.statusText } : data ?? { msg: response.statusText };
        error["code"] = response.status;
        return Promise.reject(error);
      }
      return data;
    });
}

function handleError(error) {
  // Valida si no se le pega a la api
  if (error.message === "Failed to fetch") {
    error.message = "El Servicio no se encuentra disponible";
  }
  return Promise.reject(error);
}

async function topAllTime() {
  return callApi(`${urlBase}/TopAllTime`, "GET");
}

async function topLastTime() {
  return callApi(`${urlBase}/TopLastTime`, "GET");
}
