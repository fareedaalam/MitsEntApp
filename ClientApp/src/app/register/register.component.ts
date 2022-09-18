import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account-service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  contestant=true;
  registerForm: FormGroup;
  maxDate: Date;
  validationErrors: string[] = [];

  constructor(private accountServices: AccountService, private toastr: ToastrService,
    private fb: FormBuilder, private router: Router) {

  }

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 10);
  }

  initializeForm() {
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['contestant', Validators.required],
      dateOfBirth: ['', Validators.required],
      //city: ['', Validators.required],
      //country: ['', Validators.required],
      //email: ['', Validators.email],
      mobile: ['', [Validators.required, Validators.pattern("^((\\+91-?)|0)?[0-9]{10}$")]],
      password: ['', [Validators.required,
      Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]]
    })
    this.registerForm.controls.password.valueChanges.subscribe(() => {
      this.registerForm.controls.confirmPassword.updateValueAndValidity();
    })
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value
        ? null : { isMatching: true }
    }
  }

  register() {   
   console.log(this.registerForm.value) ;
    this.accountServices.register(this.registerForm.value).subscribe({
      next: responce => {  
       // this.cancle();    
        this.router.navigateByUrl('/members');
      },
      error: error => {        
        console.log('error registrations', error);
      }
    })
   }

  cancle() {
    this.cancelRegister.emit(false);
    //console.log('cancle');
  }

}
