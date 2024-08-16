import { Injectable } from '@angular/core';
import { LogLevel } from './log-level.enum';
import color from 'picocolors';


@Injectable({
  providedIn: 'root',
})
export class LogService {
  info(origem: string, mensagem: string): void {
    this.logarCom(LogLevel.Information, origem, mensagem);
  }

  aviso(origem: string, mensagem: string): void {
    this.logarCom(LogLevel.Warning, origem, mensagem);
  }

  erro(erro: any): void {
    this.logarCom(LogLevel.Error, erro.name, JSON.stringify(erro));
  }

  private logarCom(level: any, origem: string, mensagem: string): void {
    switch (level) {
      case LogLevel.None:
        return console.log(mensagem);
      case LogLevel.Information:
        return console.info(color.blue(`${new Date().toISOString()} [INFO] ${origem}: ${mensagem}`));
      case LogLevel.Warning:
        return console.warn(color.yellow(`${new Date().toISOString()} [WARN] ${origem}: ${mensagem}`));
      case LogLevel.Error:
        return console.error(color.red(`${new Date().toISOString()} [ERR] ${origem}: ${mensagem}`));
      default:
        console.debug(mensagem);
    }
  }
}
