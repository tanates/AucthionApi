import axios from "axios";
import ErrorHandler from "../ErrorHandler/ErrorHandler";
import ReqInApi from "../Type/ReqInApiType";


export default class ApiClient {

    baseUrl = "";
    constructor() {
        try {
            this.api = axios.creat({
                baseUrl: baseUrl,
                Headers: {
                    'Content-Type': 'application/json',
                },
            });
        } catch (e) {
            return e;
        }
       
    }

    async postRequest(endpoint, servisecName, data) {
        return ErrorHandler.ErrorHandler(async () => {
            const requestModel = new ReqInApi(servisecName, data);
            const response = await this.api.post(endpoint, requestModel);
            return response.data;
        })
    }
}