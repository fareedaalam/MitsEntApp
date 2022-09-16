import { Member } from "../_models/member";
import { Resolve,ActivatedRouteSnapshot,RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs";
import { Injectable } from "@angular/core";
import { MembersService } from "../_services/members.service";
@Injectable({
    providedIn:'root'
})

export class MemberDetailedResolver implements Resolve<Member>{
    constructor(private memebrService:MembersService){
    }

    resolve(route: ActivatedRouteSnapshot):Observable<Member> {
        return this.memebrService.getMember(route.paramMap.get('username'));
    }
}