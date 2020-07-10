import { Injectable } from '@angular/core';
import { MessageComponent } from '../components/message/message.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Message, Action } from '../interfaces/message';
import { MatDialogRef, MatDialog } from '@angular/material/dialog';
import { ConfirmComponent } from '../components/confirm/confirm.component';
import { BehaviorSubject } from 'rxjs';
import { TokenService } from './token.service';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class PageService {

  public isLoading = false;
  private languageKey = 'monarchy-lang';
  private languages: string[] = ['en', 'hu'];
  private _language = new BehaviorSubject<string>('en');

  public get language(): BehaviorSubject<string> {
    return this._language;
  }

  constructor(
    private client: HttpClient,
    private snackBar: MatSnackBar,
    private confirmDialog: MatDialog,
    private tokenService: TokenService) {
  }


  public getDefaultLanguage() {
    const defLang = localStorage.getItem(this.languageKey);
    if (Boolean(defLang)) {
      return defLang;
    }
    return 'HU';
  }

  public showError(description: string, title?: string) {
    this.show(description, 'error', title);
  }

  public showSuccess(description: string, title?: string) {
    this.show(description, 'success', title);
  }

  private show(description: string, action: Action, title?: string) {
    this.snackBar.openFromComponent(MessageComponent, {
      duration: 5000,
      horizontalPosition: 'center',
      verticalPosition: 'top',
      data: {
        action,
        description,
        title,
        showCancel: true
      } as Message
    });

  }

  public info(description: string, title: string) {
    return this.confirmDialog.open(ConfirmComponent, {
      data: { description, title, showCancel: false, action: 'info' } as Message
    });
  }

  public confirm(message: Message): MatDialogRef<ConfirmComponent> {
    return this.confirmDialog.open(ConfirmComponent, {
      data: message
    });
  }
}

