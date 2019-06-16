import { AuthGuard } from './components/guards/auth-guard.service';

import { NgModule } from '@angular/core';

import { BrowserModule } from '@angular/platform-browser';
import { AppModuleShared } from './app.shared.module';

import { JwtHelper } from 'angular2-jwt';

import { AppComponent } from './components/app/app.component';

@NgModule({
    bootstrap: [ AppComponent ],
    imports: [
        BrowserModule,
        AppModuleShared
    ],
    providers: [
        AuthGuard,
        JwtHelper,
        {
            provide: 'BASE_URL',
            useFactory: getBaseUrl
        }
    ]
})
export class AppModule {
}

export function getBaseUrl() {
    return document.getElementsByTagName('base')[0].href;
}
