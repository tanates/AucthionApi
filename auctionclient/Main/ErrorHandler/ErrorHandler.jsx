import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
toast.configure;
export default class ErrorHandler {
    constructor(operation){
        this.operation = operation
    }

    async ErrorHandler(operation) {
        try {
            return await operation();

        } catch (error) {
            toast.error(this.error);
            throw error;
        }
    }
}