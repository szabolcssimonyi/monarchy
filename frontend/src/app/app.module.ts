import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LayoutModule } from '@angular/cdk/layout';
import { SharedModule } from './shared/shared.module';
import { MaterialModule } from './shared/material.module';
import { LoginComponent } from './modules/login/login.component';
import { LandingComponent } from './modules/landing/landing.component';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { DashboardComponent } from './modules/dashboard/dashboard.component';
import { WellcomeComponent } from './modules/wellcome/wellcome.component';
import { MessageComponent } from './components/message/message.component';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { HttpLoaderService } from './services/http-loader.service';
import { HttpClient } from '@angular/common/http';
import { ConfirmComponent } from './components/confirm/confirm.component';

const translationSetup = {
  extend: true,
  loader: {
    provide: TranslateLoader,
    useFactory: HttpLoaderService,
    deps: [HttpClient]
  }
} as any;

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    LandingComponent,
    DashboardComponent,
    WellcomeComponent,
    MessageComponent,
    ConfirmComponent,
  ],
  imports: [
    SharedModule,
    MaterialModule,
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    LayoutModule,
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatIconModule,
    MatListModule,
    TranslateModule.forRoot(translationSetup)
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
