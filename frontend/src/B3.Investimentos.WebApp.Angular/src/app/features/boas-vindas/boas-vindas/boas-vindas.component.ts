import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-boas-vindas',
  standalone: true,
  imports: [],
  templateUrl: './boas-vindas.component.html',
  styleUrl: './boas-vindas.component.scss'
})
export class BoasVindasComponent {
  constructor(private router: Router) {}

  navegarParaResgatarCdb() {
    this.router.navigate(['cdb/resgatar']);
  }
}
