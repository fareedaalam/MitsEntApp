import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrModule } from 'ngx-toastr';
import { RolesModalComponent } from 'src/app/modals/roles-modal/roles-modal.component';
import { User } from 'src/app/_models/user';
import { AdminService } from 'src/app/_services/admin.service';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit {
  users: Partial<User[]>;
  bsModelRef: BsModalRef;

  constructor(private adminService: AdminService, private modalService: BsModalService,
    toastr:ToastrModule) { }

  ngOnInit(): void {
    this.getUsersWithRoles();
  }

  getUsersWithRoles() {
    this.adminService.getUserWithRoles().subscribe(users => {
      this.users = users;
      // console.log(this.users);
    })
  }
  ƒ
  openRolesModal(user: User) {
    const config = {
      class: 'model-dialog-centered',
      initialState: {
        user,
        roles: this.getRolesArray(user)

      }
    }
    this.bsModelRef = this.modalService.show(RolesModalComponent, config);
    this.bsModelRef.content.updateSelectedRoles.subscribe(values => {
      console.log('user-mgt-comp', values);
      const rolesToUpdate = {
        roles: [...values.filter(el => el.checked === true).map(el => el.name)]
      };
      if (rolesToUpdate) {
        console.log('user-mgt-comp', rolesToUpdate, user);
        this.adminService.updateUserRole(user.username, rolesToUpdate.roles).subscribe(() => {
          user.roles = [...rolesToUpdate.roles]
        })
      }
    })
  }

  onCheckboxChange(e) {
    this.adminService.DeActivateUser(e.target.value).subscribe({
      // next(res) {
      //  // console.log(res);
      
      //   },
      // error(error) { 
      //   console.log(error);
      // }
    })
  }

  private getRolesArray(user) {
    const roles = [];
    const userRoles = user.roles;
    const availableRoles: any[] = [
      { name: 'Admin', value: 'Admin' },
      { name: 'Moderator', value: 'Moderator' },
      { name: 'Member', value: 'Member' },
    ]

    availableRoles.forEach(role => {
      let isMatch = false;
      for (const userRole of userRoles) {
        if (role.name === userRole) {
          isMatch = true;
          role.checked = true;
          roles.push(role);
          break;
        }
      }
      if (!isMatch) {
        role.checked = false;
        roles.push(role);
      }
    })
    return roles;
  }

}
