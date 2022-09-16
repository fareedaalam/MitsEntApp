import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';
import { GalleryItem, ImageItem } from 'ng-gallery';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { IfStmt } from '@angular/compiler';
import { Message } from 'src/app/_models/message';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  //put static true to get queryParam value 
  @ViewChild('memberTabs',{static:true}) memberTabs: TabsetComponent;
  activeTab: TabDirective; //for getting access the tab directive from dom
  member: Member
  images: GalleryItem[];
  messages: Message[] = [];


  constructor(private memberService: MembersService, private route: ActivatedRoute,
    private messageServices: MessageService) { }

  ngOnInit(): void {
    this.route.data.subscribe(data=>{
      this.member=data.member;
    })

    // Set gallery items array
    this.images = this.getImages();    
  
    //get the param value to activate the tab
    this.route.queryParams.subscribe(params => {
      params.tab ? this.selectTab(params.tab) : this.selectTab(0);
    })

  }

  getImages(): ImageItem[] {
    const imagesUrls = [];
    for (const photo of this.member.photos) {
      imagesUrls.push(new ImageItem({
        src: photo?.url,
        thumb: photo?.url
      }))
    }
    return imagesUrls;
  }

  // loadMember() {
  //   this.memberService.getMember(this.route.snapshot.paramMap.get('username')).subscribe(member => {
  //     this.member = member;
      

  //   });
  // }

  loadMessages() {
    this.messageServices.getMessageThread(this.member.username).subscribe(message => {
      this.messages = message;
    })
  }

  selectTab(tabId: number) {
    //console.log(tabId);
    this.memberTabs.tabs[tabId].active = true;
  }

  onTabActivated(data: TabDirective) {
    this.activeTab = data;
    if (this.activeTab.heading === 'Messages' && this.messages.length === 0) {
      //console.log('mesage tab click')
      this.loadMessages();
    }
  }


}
