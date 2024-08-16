import { ErrorHandler, Injectable } from '@angular/core';
import { LogService } from './log.service';

@Injectable({
  providedIn: 'root',
})
export class GlobalErrorHandlerService implements ErrorHandler {
  constructor(private readonly logService: LogService) {}

  handleError(error: any): void {
    this.logService.erro(error);
  }
}
