﻿import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from "@angular/router";
import { NgForm } from '@angular/forms';

import { AppSettings } from '../../appsettings';

@Component({
    selector: 'login',
    templateUrl: './login.component.html'
})

export class LoginComponent {

    invalidLogin: boolean = false;

    constructor(private router: Router, private http: HttpClient) { }

    login(form: NgForm) {
        let credentials = JSON.stringify(form.value);
        let settings = new AppSettings();
        this.http.post(settings.movieSearchApiLoginUrl, credentials, {
            headers: new HttpHeaders({
                "Content-Type": "application/json"
            })
        }).subscribe((response: any) => {
            let token = (<any>response).token;
            localStorage.setItem("jwt", token);
            this.invalidLogin = false;
            this.router.navigate(["/"]);
        }, (err: any) => {
            this.invalidLogin = true;
        });
    }
}