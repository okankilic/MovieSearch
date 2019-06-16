import { JwtHelper } from 'angular2-jwt';
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

import { PLATFORM_ID } from '@angular/core';

import { isPlatformBrowser, isPlatformServer } from '@angular/common';

@Injectable()
export class AuthGuard {

    constructor(
        private jwtHelper: JwtHelper,
        private router: Router) {
    }

    canActivate() {

        if (typeof window !== 'undefined') {

            var token = window.localStorage.getItem("jwt");

            if (token && !this.jwtHelper.isTokenExpired(token)) {
                return true;
            }
        }

        this.router.navigate(["login"]);

        return false;
    }

    canDeactivate() {

        if (typeof window !== 'undefined') {

            var token = window.localStorage.getItem("jwt");

            if (token && !this.jwtHelper.isTokenExpired(token)) {

                this.router.navigate(["home"]);

                return true;
            }
        }

        return false;
    }
}