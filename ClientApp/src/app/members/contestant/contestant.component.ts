import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/pagination';
import { UserParams } from 'src/app/_models/userParams';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-contestant',
  templateUrl: './contestant.component.html',
  styleUrls: ['./contestant.component.css']
})
export class ContestantComponent implements OnInit {
  members: Member[];
  pagination: Pagination;
  userParams:UserParams;
  constructor(private memberServices: MembersService) { 
    this.userParams = this.memberServices.getUserParams();    
  }

  ngOnInit(): void {
    this.loadMembers();
  }


  loadMembers() {
    //console.log(this.userParams);
    //this.memberServices.setUserParams(this.userParams);
    this.memberServices.getContestant().subscribe(response => {
      this.members = response;
     // console.log(response);
    })
  }

}
