import { Injectable } from '@angular/core';
import { UserEntity } from '../_models/user';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, take } from 'rxjs';
import { AccountService } from '../_services/account-service.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private accountService:AccountService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let currentUser:UserEntity;
    this.accountService.currentUser$.pipe(take(1)).subscribe(user=>currentUser=user);
    if(currentUser){     
      request =request.clone({        
        setHeaders:{
          Authorization: 'Bearer ' +currentUser.token
        }
      })
    }


    return next.handle(request);
  }
}
