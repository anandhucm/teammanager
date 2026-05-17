import { Component, inject } from '@angular/core';
import { NavTop } from '../../layout/nav-top/nav-top';
import { MemberList } from '../members/member-list/member-list';

@Component({
  selector: 'app-home',
  imports: [NavTop,
    MemberList
  ],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home {
  

  // private memberList = inject(MemberList);

  protected getAllTeamMembers(){

    // return this.memberList.members$

  }

}
