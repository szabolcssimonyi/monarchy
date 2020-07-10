import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { PageService } from '../services/page.service';
import { UserService } from '../services/user.service';
import { TranslateService } from '@ngx-translate/core';
import { Router } from '@angular/router';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ErrorInterceptorService implements HttpInterceptor {

  constructor(
    private userService: UserService,
    private pageService: PageService,
    private translateService: TranslateService,
    private router: Router) {
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(catchError(err => {
      if (err.status === 401) {
        this.userService.clear();
        this.pageService.showError(this.translateService.instant('LOGIN.ERROR.UNAUTHORIZED'));
        return throwError(err);
      }
      if (err.status === 422) {
        const fields = (err.error as any[]).map(e => e.field).join(',');
        const messages = (err.error as any[]).map(e => this.translateService.instant(e.message)).join(',');
        this.pageService.showError(`${fields}: ${messages}`);
        return throwError(err);
      }
      console.log(err);
      const message = Boolean(err.message) ? err.message : 'Fatal error';
      this.pageService.showError(message);
      return throwError(err);
    }));
  }
}
