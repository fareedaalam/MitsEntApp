import { Component, OnInit } from '@angular/core';
import { ThumbnailsPosition } from 'ng-gallery';
import { Observable, take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/pagination';
import { User } from 'src/app/_models/user';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_services/account-service.service';
import { MembersService } from 'src/app/_services/members.service';


@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  //members$: Observable<Member[]>;
  members: Member[];
  pagination: Pagination;
  userParams:UserParams;
  user:User;
  genderList = [{value: 'male',display: 'Males'},{value: 'female',display: 'Females'}]

  constructor(private memberServices: MembersService) { 
   this.userParams = this.memberServices.getUserParams();    
  }

  ngOnInit(): void {
    //this.members$= this.memberServices.getMembers();   
    this.loadMembers();
  }

  loadMembers() {
    //console.log(this.userParams);
    this.memberServices.setUserParams(this.userParams);
    this.memberServices.getMembers(this.userParams).subscribe(response => {
      this.members = response.result;
      this.pagination = response.pagination;
    })
  }
  
  resetFilters(){
    this.userParams = this.memberServices.resetUserParams();
    this.loadMembers();
  }

  pageChanged(event: any) {
    this.userParams.pageNumber = event.page;
    this.memberServices.setUserParams(this.userParams);
    this.loadMembers();
  }

}
