import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account-service.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  model: any = {};
  constructor(private accountServices: AccountService) { }

  ngOnInit(): void {
  }

  register() {
    this.accountServices.register(this.model).subscribe({
      next: responce => {
        this.cancle();
        console.log(this.model);
      },
      error: error => {
        console.log(error);
      }

    })
  }

  cancle() {
    this.cancelRegister.emit(false);
    console.log('cancle');
  }

}
