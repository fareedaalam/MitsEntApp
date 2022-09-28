 import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, UrlSerializer } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account-service';


@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {}
  loginForm: FormGroup;
  //loggedIn: boolean;
  //curretnUser$:Observable<UserEntity>;

  constructor(public accountService: AccountService,private router:Router,private toastr :ToastrService,
    private fb: FormBuilder) { }

  ngOnInit(): void {
    this.initializeForm()
    //this.curretnUser$=this.accountService.currentUser$;
    // this.getCurrentUser();
  }

  initializeForm() {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],     
      password: ['', Validators.required]
    })
  }

  login() {
    this.accountService.login(this.loginForm.value).subscribe({
      next: responce => {
        this.router.navigateByUrl('/contestant');
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

