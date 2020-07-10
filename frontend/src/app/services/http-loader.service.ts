import { HttpClient } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

export function HttpLoaderService(http: HttpClient) {
  return new TranslateHttpLoader(http);
}

