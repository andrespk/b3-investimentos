import { Injectable } from '@angular/core';
import { ResgatarCdbRequestDto } from './dtos/resgatar-cdb-request.dto';
import { ResgatarCdbResponseDto } from './dtos/resgatar-cdb-response.dto';
import { ApiGatewayService } from '../../../infra/api-gateway.service';

@Injectable({
  providedIn: 'root'
})
export class ResgatarCdbService {
  constructor(private apiGatewayService: ApiGatewayService) {}

  async resgatar(request: ResgatarCdbRequestDto): Promise<ResgatarCdbResponseDto> {
      return await this.apiGatewayService.enviar<ResgatarCdbRequestDto>('/cdb/resgatar', request);
  }
}
