import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { HttpLoaderService } from '../services/http-loader.service';
import { HasPermissionDirective } from '../directives/has-permission.directive';

const translationSetup = {
  extend: true,
  loader: {
    provide: TranslateLoader,
    useFactory: HttpLoaderService,
    deps: [HttpClient]
  }
} as any;

@NgModule({
  declarations: [HasPermissionDirective],
  imports: [
    CommonModule,
    RouterModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    TranslateModule.forChild(translationSetup),
  ],
  exports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    HasPermissionDirective
  ]
})
export class SharedModule { }
