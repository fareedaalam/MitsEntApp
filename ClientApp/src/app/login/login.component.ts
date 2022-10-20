import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account-service';

type NewType = FormGroup;

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  registerMode=false;
  model: any = {}
  loginForm: NewType;
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
        this.toastr.info('loged in');
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


  
  registerToggle(){
    this.registerMode=!this.registerMode;

  }

  cancelRegisterMode(event:boolean){
    this.registerMode=event;
  }

}

