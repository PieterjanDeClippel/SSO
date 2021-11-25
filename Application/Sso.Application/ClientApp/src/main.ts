import { enableProdMode, StaticProvider } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

export function getBaseUrl() {
  return document.getElementsByTagName('base')[0].href.slice(0, -1);
}

export function getExternalUrl(baseUrl: string) {
  // If localhost, not android, so simply return the same value.
  if (new RegExp("\\blocalhost\\b").test(baseUrl)) {
    return baseUrl;
  }

  // If baseUrl contains a scheme
  const match = new RegExp("^(http[s]{0,1})\\:\\/\\/(.*)$").exec(baseUrl);
  if (match !== null) {
    let protocol = match[1];
    let url = match[2];

    return `${protocol}://external.${url}`;
  }

  // No scheme in baseUrl
  const noSchemeMatch = new RegExp("^\\/\\/(.*)$").exec(baseUrl);
  if (!noSchemeMatch) {
    throw 'baseUrl does not start with //';
  }

  const url = noSchemeMatch[1];
  return `//external.${url}`;
}

const providers: StaticProvider[] = [
  { provide: 'BASE_URL', useFactory: getBaseUrl, deps: [] },
  { provide: 'EXTERNAL_URL', useFactory: getExternalUrl, deps: ['BASE_URL'] }
];

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic(providers).bootstrapModule(AppModule)
  .catch(err => console.log(err));
