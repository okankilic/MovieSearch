import { NgModule } from '@angular/core';

import { ServerModule } from '@angular/platform-server';
import { AppModuleShared } from './app.shared.module';

import { JwtHelper } from 'angular2-jwt';

import { AppComponent } from './components/app/app.component';

@NgModule({
    bootstrap: [ AppComponent ],
    imports: [
        ServerModule,
        AppModuleShared
    ],
    providers: [JwtHelper]
})
export class AppModule {
}
