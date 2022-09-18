import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { UrlSegment } from '@angular/router';
import { environment } from 'src/environments/environment';
import { User } from './_models/user';
import { AccountService } from './_services/account-service';



@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  baseUrl = environment.apiUrl;
  title = 'MitsEntertainment';
  users: any;


  constructor(private http: HttpClient, private accountServices: AccountService) {
  }

  ngOnInit(): void {
    // this.getUsers();
    this.setCurrentUser();
  }
  //Set User from local storage
  setCurrentUser() {
    const user: User = JSON.parse(localStorage.getItem('user'));
    this.accountServices.setCurrentUser(user);
  }
  //Get All Users List
  getUsers() {
    this.http.get(this.baseUrl + 'api/users').subscribe({
      next: responce => this.users = responce,
      error: error => console.log(error)
    })
  }

}
