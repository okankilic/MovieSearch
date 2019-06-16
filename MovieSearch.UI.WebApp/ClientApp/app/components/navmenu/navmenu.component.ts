import { JwtHelper } from 'angular2-jwt';
import { Component } from '@angular/core';
import { Router } from "@angular/router";

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.css']
})
export class NavMenuComponent {

    constructor(private router: Router, private jwtHelper: JwtHelper) {
    }

    isUserAuthenticated() {
        let token: any = localStorage.getItem("jwt");
        if (token && !this.jwtHelper.isTokenExpired(token)) {
            return true;
        }
        else {
            return false;
        }
    }

    logOut() {
        localStorage.removeItem("jwt");
        this.router.navigate(["login"]);
    }
}
