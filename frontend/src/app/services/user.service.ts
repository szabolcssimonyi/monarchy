import { Injectable } from '@angular/core';
import { BehaviorSubject, Subject, Observable } from 'rxjs';
import { User } from '../interfaces/user';
import { TokenService } from './token.service';
import { Permission } from '../interfaces/permission';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Login } from '../interfaces/login';
import { Token } from '../interfaces/token';
import { Role } from '../interfaces/role';
import { map } from 'rxjs/operators';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private _user: BehaviorSubject<User> = null;
  private _permissions: Permission[] = [];
  private _userLoaded = false;
  private _loginResult: Subject<boolean> = null;

  public get isUserLoaded() {
    return this._userLoaded;
  }

  public get Permissions(): Permission[] {
    return this._permissions;
  }

  public get isLoggedIn(): boolean {
    return this.tokenService.isValid();
  }

  public get loginResult(): Subject<boolean> {
    if (!this._loginResult) {
      this._loginResult = new Subject<boolean>();
    }
    return this._loginResult;
  }

  public get permissions(): Permission[] {
    return this._permissions;
  }

  public get user(): BehaviorSubject<User> {
    if (Boolean(this._user)) {
      return this._user;
    }
    this._user = new BehaviorSubject<User>({ roles: [] } as User);
    if (!this.tokenService.isValid()) {
      this.clear();
      this._userLoaded = true;
      this.loginResult.next(false);
      return this._user;
    }
    this.load();
    return this._user;
  }

  constructor(
    private client: HttpClient,
    private tokenService: TokenService) { }

  public login(login: Login): Observable<false | Token> {
    return this.client
      .post<Token>(`${environment.apiUrl}${environment.urls.login}`, login)
      .pipe(map(token => {
        if (this.tokenService.isToken(token)) {
          this.tokenService.set(token);
        }
        return this.tokenService.get() as Token;
      }));
  }

  public requestPasswordReset(email: string): Observable<void> {
    const header = new HttpHeaders({ 'Content-Type': 'application/x-www-form-urlencoded' });
    const params: URLSearchParams = new URLSearchParams();
    params.append('email', email);
    return this.client.post<void>(`${environment.apiUrl}${environment.urls.password_reset}`,
      params.toString(),
      { headers: header });
  }

  public load() {
    this._userLoaded = false;
    if (!this.tokenService.isValid()) {
      return;
    }
    this.client.get<User>(`${environment.apiUrl}${environment.urls.me}`,
      {
        headers: new HttpHeaders({
          'Content-Type': 'application/x-www-form-urlencoded',
        })
      }).subscribe(u => {
        this._userLoaded = true;
        this.convertPermissions(this.getPermissions(u.roles));
        this._user.next(u);
        this.loginResult.next(Boolean(this._user.getValue()?.id));
      });
  }

  public setPassword(id: string, password: string, path: string): Observable<any> {
    const header = new HttpHeaders({ 'Content-Type': 'application/x-www-form-urlencoded' });
    let params = new HttpParams();
    params = params.append('password', password);
    params = params.append('id', id);
    return this.client
      .post<any>(`${environment.apiUrl}${environment.urls.set_password}`,
        params.toString(),
        { headers: header });
  }

  public clear() {
    this.tokenService.clear();
    if (!Boolean(this._user)) {
      this._user = new BehaviorSubject<User>({ roles: [] } as User);
    } else {
      this._user.next({ roles: [] } as User);
    }
  }

  public get isLoggedin(): boolean {
    return this.tokenService.isValid();
  }

  private convertPermissions(permissions: string[]) {
    this._permissions = permissions.map(p => {
      const tags = p.split('.');
      if (!Boolean(tags) || tags.length === 0) {
        return {
          action: null,
          modules: []
        } as Permission;
      }
      const action = tags.pop();
      const modules = tags;
      return {
        action,
        modules: modules.map(m => `${m.toLocaleLowerCase().replace(/ /g, '')}`)
      } as Permission;
    }).filter(p => Boolean(p.action));
  }

  public hasPermission(module: string, action?: string | string[] | undefined): boolean {
    if (!Boolean(this._permissions)) {
      return false;
    }
    if (Boolean(action)) {
      return this._permissions.some(permission => {
        if (permission.modules.length === 0) {
          return false;
        }
        const m = permission.modules[permission.modules.length - 1];
        return m === module
          && (action instanceof Array ? action.some(a => a === permission.action) : action === permission.action);
      });
    } else {
      return this._permissions.some(permission => permission.modules.some(m => m === module));
    }
  }

  private clearPermissions() {
    this._permissions = [];
  }

  private getPermissions(roles: Role[]): string[] {
    let permissions: string[] = [];
    roles.forEach(r => {
      permissions = [...permissions, ...r.permissions];
    });
    return permissions;
  }
}
