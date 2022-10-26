import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AdminService } from 'src/app/_services/admin.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-update-user',
  templateUrl: './update-user.component.html',
  styleUrls: ['./update-user.component.css']
})
export class UpdateUserComponent implements OnInit {
  //@Input() user ;
  updateForm: FormGroup;
  maxDate: Date;
  constructor(private activerouter: ActivatedRoute,private router:Router, private fb: FormBuilder, private meemberService: MembersService, private adminService: AdminService) { }

  ngOnInit(): void {
    this.initializeForm();
    this.getUsers(this.activerouter.snapshot.params.username);

  }

  initializeForm() {
    this.updateForm = this.fb.group({
      introduction: [''],
      lookingFor: [''],
      interests: [''],
      city: [''],
      country: [''],
      gender: [''],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      phonenumber: ['', [Validators.required, Validators.pattern("^((\\+91-?)|0)?[0-9]{10}$")]],
      isActive: ['']

    });
  }
  getUsers(username: string) {
    this.meemberService.getMember(username).subscribe(result => {

      this.updateForm.patchValue({ introduction: result.introduction });
      this.updateForm.patchValue({ lookingFor: result.lookingFor });
      this.updateForm.patchValue({ interests: result.interests });
      this.updateForm.patchValue({ city: result.city });
      this.updateForm.patchValue({ country: result.country });
      this.updateForm.patchValue({ gender: result.gender });
      this.updateForm.patchValue({ knownAs: result.knownAs });
      this.updateForm.patchValue({ dateOfBirth: result.dateOfBirth });
      this.updateForm.patchValue({ phonenumber: result.phoneNumber });
      this.updateForm.patchValue({ isActive: result.isActive });

       console.log(result);
    },
    err=>{
      console.log(err);
    })

  }


  update() {
    //console.log(this.updateForm.value);
    this.adminService.updateUser(this.activerouter.snapshot.params.username, this.updateForm.value).subscribe(result => {
      this.router.navigateByUrl('/admin');
    },
    err=>{
      console.log(err);
    });
  }
}
