import { Component } from '@angular/core';
import { ResgatarCdbRequestDto } from './dtos/resgatar-cdb-request.dto';
import { ResgatarCdbResponseDto } from './dtos/resgatar-cdb-response.dto';
import { ResgatarCdbService } from './resgatar.service';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-resgatar',
  standalone: true,
  imports: [],
  templateUrl: './resgatar.component.html',
  styleUrl: './resgatar.component.scss'
})
export class ResgatarCdbComponent {
  valorInicial: number = 0;
  prazoEmMeses: number = 0;
  response: ResgatarCdbResponseDto = new ResgatarCdbResponseDto();
  isPanelCollapsed = true;

  constructor(private resgatarCdbService: ResgatarCdbService) {}

  async onSubmit() {
    const request: ResgatarCdbRequestDto = {
      valorInicial: this.valorInicial,
      prazoEmMeses: this.prazoEmMeses,
      percentualCdi: environment.percentualPadraoCdi,
      percentualCdiPagoPeloBanco: environment.percentualPadraoCdiPagoPeloBanco
    };
    this.response = await this.resgatarCdbService.resgatar(request);
    this.isPanelCollapsed = false;
  }

  clearValues() {
    this.valorInicial = 0;
    this.prazoEmMeses = 0;
    this.isPanelCollapsed = true;
  }
}
