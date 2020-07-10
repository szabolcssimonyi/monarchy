import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { PageService } from '../services/page.service';
import { finalize } from 'rxjs/operators';

@Injectable()
export class LoaderInterceptor implements HttpInterceptor {

  private count = 0;

  constructor(private pageService: PageService) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    this.count++;
    this.pageService.isLoading = true;
    return next.handle(request)
      .pipe(finalize(() => {
        if (--this.count <= 0) {
          this.count = 0;
          this.pageService.isLoading = false;
        }
      }));
  }
}
