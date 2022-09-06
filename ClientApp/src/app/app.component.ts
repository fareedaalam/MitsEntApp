import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { UrlSegment } from '@angular/router';
import { UserEntity } from './_models/user';
import { AccountService } from './_services/account-service.service';



@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'MitsEntertainment';
  users: any;

  constructor(private http: HttpClient, private accountServices: AccountService) {
  }

  ngOnInit(): void {
    this.getUsers();
    this.setCurrentUser();
  }
  //Set User from local storage
  setCurrentUser() {
    const user: UserEntity = JSON.parse(localStorage.getItem('user'));
    this.accountServices.setCurrentUser(user);
  }
  //Get All Users List
  getUsers() {
    this.http.get('https://localhost:5001/api/users').subscribe({
      next: responce => this.users = responce,
      error: error => console.log(error)
    })
  }

}
