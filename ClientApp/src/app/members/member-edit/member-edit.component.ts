import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account-service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  //access the form property
  @ViewChild('editForm') editForm: NgForm;
  member: Member;
  user: User;
  maxDate: Date;
  //access the browser events
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    $event.returnValue = true;
  }

  constructor(private accountService: AccountService, private memberService: MembersService,
    private toastr: ToastrService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
  }

  ngOnInit(): void {
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 10);
    this.loadMember();    
  }
  loadMember() {
    this.memberService.getMember(this.user.username).subscribe(member => {
      this.member = member;
      console.log('member',this.member);
    })
  }
  updateMember() {
    this.memberService.updateMember(this.member).subscribe(() => {
      this.toastr.success('Profile updated successfully');
      this.editForm.reset(this.member);
    })


  }

}
