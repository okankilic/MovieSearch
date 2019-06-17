import { HttpClient, HttpHeaders } from '@angular/common/http';

import { JwtHelper } from 'angular2-jwt';
import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { NgForm } from '@angular/forms';

import { AppSettings } from '../../appsettings';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: []
})

export class HomeComponent {

    movie: any;
    error: any;

    constructor(private jwtHelper: JwtHelper,
        private router: Router,
        private http: HttpClient) {
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

    searchMovie(form: NgForm) {

        let credentials = JSON.stringify(form.value);
        let token = localStorage.getItem("jwt");
        let settings = new AppSettings();

        this.http.post(settings.movieSearchApiMovieSearchUrl, credentials, {
            headers: new HttpHeaders({
                "Authorization": "Bearer " + token,
                "Content-Type": "application/json"
            })
        }).subscribe(response => {
            this.movie = response;
            if (this.movie) {
                this.error = null;
            } else {
                this.error = "Movie not found";
            }
        }, err => {
            //this.error = err;
            this.movie = null;
            console.log(err)
        });
    }
    
}