import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators'
import { UserEntity } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = 'https://localhost:5001/api/';
  private currentUserSource = new ReplaySubject<UserEntity>(1);
  currentUser$ = this.currentUserSource.asObservable();


  constructor(private http: HttpClient) { }

  login(model: UserEntity) {   
    return this.http.post(this.baseUrl + 'account/login', model).pipe(
      map((responce: any) => {
        const user = responce;
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })

    );
  }
  register(model: any) {   
    return this.http.post(this.baseUrl + 'account/register', model).pipe(
      map((responce: UserEntity) => {        
        if (responce) {
          localStorage.setItem('user', JSON.stringify(responce));
          this.currentUserSource.next(responce);
        }
      })

    );
  }
  setCurrentUser(user: UserEntity) {
    this.currentUserSource.next(user);
  }
  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
}
