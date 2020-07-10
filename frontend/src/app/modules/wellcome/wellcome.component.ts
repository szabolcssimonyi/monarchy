import { Component, OnInit, OnDestroy } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { shareReplay, map, takeUntil } from 'rxjs/operators';
import { PageService } from 'src/app/services/page.service';
import { UserService } from 'src/app/services/user.service';
import { User } from 'src/app/interfaces/user';
import { MenuItem } from 'src/app/interfaces/menu-item';
import { TranslateService } from '@ngx-translate/core';
import { LoginComponent } from '../login/login.component';
import { Login } from 'src/app/interfaces/login';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-wellcome',
  templateUrl: './wellcome.component.html',
  styleUrls: ['./wellcome.component.scss']
})
export class WellcomeComponent implements OnInit, OnDestroy {

  private $unsubscribe = new Subject<void>();
  public user: User;
  public menu = [
    {
      label: 'TOOLBAR.DASHBOARD_LINK',
      route: ['/'],
      permissions: []
    }
  ] as MenuItem[];

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches),
      shareReplay()
    );

  constructor(
    public dialog: MatDialog,
    private breakpointObserver: BreakpointObserver,
    public pageService: PageService,
    public userService: UserService,
    public translateService: TranslateService) { }

  ngOnInit(): void {
    this.userService.user.pipe(takeUntil(this.$unsubscribe)).subscribe(user => {
      this.user = user;
    });
  }

  public login() {
    const dialogRef = this.dialog.open(LoginComponent, {
      width: '450px',
      data: {} as Login
    });
    dialogRef.afterClosed().subscribe((login: Login) => {
      if (!Boolean(login)) {
        console.log('login cancelled');
        return;
      }
      this.userService.login(login).subscribe(result => {
        if (!Boolean(result)) {
          this.pageService.showError(this.translateService.instant('LOGIN.ERROR.FAILED'));
          return;
        }
        this.pageService.showSuccess(this.translateService.instant('LOGIN.ERROR.SUCCESS'));
        this.userService.load();
      });
    });
  }

  ngOnDestroy(): void {
    this.$unsubscribe.next();
    this.$unsubscribe.complete();
  }


}
