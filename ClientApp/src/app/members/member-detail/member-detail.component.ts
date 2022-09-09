import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';
import { GalleryItem, ImageItem } from 'ng-gallery';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  member: Member
  images: GalleryItem[];
  

  constructor(private memberService: MembersService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.images
    this.loadMember();
  }
  loadMember() {
    this.memberService.getMember(this.route.snapshot.paramMap.get('username')).subscribe(member => {
      this.member = member;
      // Set gallery items array
      this.images = this.getImages();

    });
  }

  getImages(): ImageItem[] {
    const imagesUrls = [];
    for (const photo of this.member.photos) {
      imagesUrls.push(new ImageItem({
        src: photo?.url,
        thumb: photo?.url
      }))
      imagesUrls.push(new ImageItem({
        src: photo?.url,
        thumb: photo?.url
      }))
      imagesUrls.push(new ImageItem({
        src: photo?.url,
        thumb: photo?.url
      }))
    }
    return imagesUrls;
  }
}
