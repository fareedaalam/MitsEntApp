import { Component, OnInit } from '@angular/core';
import { Router, UrlSerializer } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account-service.service';


@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {}
  //loggedIn: boolean;
  //curretnUser$:Observable<UserEntity>;

  constructor(public accountService: AccountService,private router:Router,private toastr :ToastrService) { }

  ngOnInit(): void {
    //this.curretnUser$=this.accountService.currentUser$;
    // this.getCurrentUser();
  }

  login() {
    this.accountService.login(this.model).subscribe({
      next: responce => {
        this.router.navigateByUrl('/members');
      },
      error: error => {        
       // this.toastr.error(error.message);
      }
    })
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }

}

