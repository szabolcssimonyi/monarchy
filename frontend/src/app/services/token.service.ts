import { Injectable } from '@angular/core';
import { Token } from '../interfaces/token';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  public get tokenName() {
    return 'monarchy-token';
  }

  constructor() { }

  public get(): false | Token {
    if (!this.has()) {
      return false;
    }
    const token = JSON.parse(localStorage.getItem(this.tokenName)) as Token;
    return token;
  }

  public set(token: Token): void {
    const copy = Object.assign({}, token);
    localStorage.setItem(this.tokenName, JSON.stringify(copy));
  }

  public has(): boolean {
    const str = localStorage.getItem(this.tokenName);
    if (!Boolean(str)) {
      return false;
    }
    const token = JSON.parse(str) as Token;
    return Boolean(token) && Boolean(token.accessToken);
  }

  public isValid(): boolean {
    const token = this.get();
    if (!Boolean(token)) {
      return false;
    }
    return true;
  }

  public clear(): void {
    localStorage.removeItem(this.tokenName);
  }

  public isToken(arg: any): arg is Token {
    return Boolean(arg)
      && typeof (arg['accessToken']) !== 'undefined';
  }

}
