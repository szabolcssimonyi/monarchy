import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { TokenService } from '../services/token.service';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationInterceptor implements HttpInterceptor {
  constructor(private tokenService: TokenService) { }
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
      const token = this.tokenService.get();
      let request = req;
      const { url, method, headers, body } = req;
      if (Boolean(token) && method.toUpperCase() === 'POST') {
          request = req.clone({
              body: { ...body, ...{ sessionToken: token } }
          });
      }
      return next.handle(request);
  }
}
