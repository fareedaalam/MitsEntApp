import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account-service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  user: User;
  registerForm: FormGroup;
  otpForm: FormGroup;
  showMemberRadio = false;
  maxDate: Date;
  validationErrors: string[] = [];

  showOtp = false;
  txtOtp = false;


  constructor(private accountServices: AccountService, private toastr: ToastrService,
    private fb: FormBuilder, private router: Router) {
    this.accountServices.currentUser$.pipe(take(1)).subscribe(user => this.user = user);


  }

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 10);
    this.showcontestantRedio();
  }

  showcontestantRedio() {
    if (this.user) {
      if (this.user.username === 'Admin' || this.user.username === 'admin') {
        this.showMemberRadio = !this.showMemberRadio;
      }
    }
  }

  initializeForm() {
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['member', Validators.required],
      // dateOfBirth: ['', Validators.required],
      mobile: ['', [Validators.required, Validators.pattern("^((\\+91-?)|0)?[0-9]{10}$")]],
      password: ['', [Validators.required,
      Validators.minLength(4),
      Validators.maxLength(28)],
      ],

      confirmPassword: ['', [Validators.required, this.matchValues('password')]]
    });

    this.registerForm.controls.password.valueChanges.subscribe(() => {
      this.registerForm.controls.confirmPassword.updateValueAndValidity();
    });

    this.otpForm = this.fb.group({
      mobile: ['', [Validators.required, Validators.pattern("^((\\+91-?)|0)?[0-9]{10}$")]],
      otp: [''],
    });
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value
        ? null : { isMatching: true }
    }
  }

  register() {
    this.accountServices.register(this.registerForm.value).subscribe({
      next: responce => { this.router.navigateByUrl('/contestant'); },
      error: error => {
        console.log(error);
        this.toastr.error(error.error)

        if (typeof (error.error) === 'object') {
          const modelStateErrors = [];
          for (const key in error.error) {
            if (error.error[key]) {
              modelStateErrors.push(error.error[key].code)
            }
            console.log(modelStateErrors);
            console.log(modelStateErrors.flat());
          }}
        //this.validationErrors = array;
      }

    })
  }

  sendOtp() {
    const mobile = this.otpForm.get('mobile').value;
    this.txtOtp = true;
    if (this.otpForm.valid && this.otpForm.get('otp').value != '') {
      this.accountServices.VerifyOtp(this.otpForm.value).subscribe({
        next: next => {
          // console.log(next);
          this.showOtp = true;
          this.otpForm.reset();
        }
      });
    } else {
      this.accountServices.sendOtp(mobile).subscribe({
        next: resp => {
          this.registerForm.patchValue({ mobile: mobile });

        },
        error: err => {
          console.log(err);
        }
      })

    }

  }

  cancle() {
    this.cancelRegister.emit(false);
  }

}
