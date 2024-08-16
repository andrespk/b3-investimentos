import { Injectable } from '@angular/core';
import axios from 'axios';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApiGatewayService {
   async enviar<T>(path: string, parameters: T): Promise<any> {
    const resposta = await axios.post(`${environment.apiBaseUrl}/${path}`, parameters);
    return resposta.data;
  }
}
