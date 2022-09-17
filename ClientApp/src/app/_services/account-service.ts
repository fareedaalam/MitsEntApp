import { JsonPipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators'
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();


  constructor(private http: HttpClient) { }

  login(model: User) {   
    return this.http.post(this.baseUrl + 'account/login', model).pipe(
      map((responce: any) => {
        const user = responce;
        if (user) {
          // localStorage.setItem('user', JSON.stringify(user));
          // this.currentUserSource.next(user);
          this.setCurrentUser(user);
        }
      })

    );
  }
  register(model: any) {   
    return this.http.post(this.baseUrl + 'account/register', model).pipe(
      map((user: User) => {        
        if (user) {
          //localStorage.setItem('user', JSON.stringify(responce));
          //this.currentUserSource.next(user);
          this.setCurrentUser(user);
        }
      })

    );
  }

  setCurrentUser(user: User) {
    //add roles
    user.roles=[];
    const role=this.getDecodedToken(user.token).role;
    //check multi roles array of single string role
    Array.isArray(role) ? user.roles= role : user.roles.push(role);

    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }

  getDecodedToken(token){
    //var k: any =JSON.parse(Buffer.from(token.split('.')[1],'base64'))
    return JSON.parse(atob(token.split('.')[1]))

  }
}
 