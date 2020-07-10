import { Injectable } from '@angular/core';
import {
  CanActivate, CanActivateChild, CanLoad, Route, UrlSegment,
  ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router
} from '@angular/router';
import { Observable, Subject } from 'rxjs';
import { UserService } from '../services/user.service';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationGuard implements CanActivate, CanActivateChild, CanLoad {
  constructor(private router: Router, private userService: UserService) { }
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.authorize(next.data.module, next.data.action);
  }
  canActivateChild(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.authorize(next.data.module, next.data.action);
  }
  canLoad(
    route: Route,
    segments: UrlSegment[]): Observable<boolean> | Promise<boolean> | boolean {
    return this.authorize(route.data.module, route.data.action);
  }

  private authorize(moduleName: string, action?: string | string[] | undefined): Observable<boolean> | Promise<boolean> | boolean {
    const authorization = new Subject<boolean>();
    if (!this.userService.isUserLoaded) {
      this.userService.load();
      let authResult = false;
      this.userService.loginResult.subscribe(result => {
        if (!Boolean(result)) {
          authResult = false;
        } else {
          authResult = this.userService.hasPermission(moduleName, action);
        }
        authorization.next(authResult);
        authorization.complete();
        if (!authResult) {
          this.router.navigate(['/']);
        }
      });
      return authorization;
    }
    if (!this.userService.permissions.some(() => true)) {
      this.router.navigate(['/']);
      return false;
    }
    return this.userService.hasPermission(moduleName, action);
  }
}