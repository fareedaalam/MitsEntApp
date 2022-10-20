import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ThumbnailsMode } from 'ng-gallery';
import { ToastrService } from 'ngx-toastr';
import { UpdatePwdDto } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account-service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit {
  forgotpwdPanel = false;
  forgotForm: FormGroup;
  updateForm: FormGroup;

  updateToken: UpdatePwdDto;
  txtOtp = false;
  showOtp = false;

  constructor(private accountServices: AccountService, private toastr: ToastrService,
    private fb: FormBuilder, private router: Router) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.forgotForm = this.fb.group({
      username: ['', Validators.required],
      phonenumber: ['', [Validators.required, Validators.pattern("^((\\+91-?)|0)?[0-9]{10}$")]],
      otp: [''],
    });

    this.updateForm = this.fb.group({
      username: [''],
      token: [''],
      password: ['', [Validators.required,Validators.minLength(4),Validators.maxLength(28)]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]]

    });
    this.updateForm.controls.password.valueChanges.subscribe(() => {
      this.updateForm.controls.confirmPassword.updateValueAndValidity();
    });
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value
        ? null : { isMatching: true }
    }
  }


  sendOtp() {
    const mobile = this.forgotForm.get('phonenumber').value;
    this.txtOtp = true;
    if (this.forgotForm.valid && this.forgotForm.get('otp').value != '') {
      this.accountServices.VerifyForgotPwd(this.forgotForm.value).subscribe(
        {
          next: result => {
            this.showOtp = true;
            this.forgotpwdPanel = true;
            this.updateToken = result;
           // console.log(this.updateToken);
          }
        });
    } else {
      this.accountServices.sendOtp(mobile).subscribe({
        next: resp => {
          console.log(resp);
        },
        error: err => {
          console.log(err);
        }
      })
    }
  }

  UpdatePwd() {
    this.updateToken.password = this.updateForm.get('password').value;
    if (this.updateForm.valid) {
      this.accountServices.UpdatePassword(this.updateToken).subscribe({
        next: result => {
          this.router.navigateByUrl('/');
        },
        error: err => {
          console.log(err);
        }
      });
    }
  }
}
