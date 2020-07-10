import { Component } from '@angular/core';
import { PageService } from './services/page.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Monarchy';

  constructor(public pageService: PageService, public translateService: TranslateService) {
    this.translateService.use('en');
  }
}
