import { Component, inject } from '@angular/core';
import { MemberService } from '../../../core/service/member-service';
import { Observable } from 'rxjs';
import { User } from '../../../types/types';
import { AsyncPipe } from '@angular/common';
import { MemberCard } from '../member-card/member-card';

@Component({
  selector: 'app-member-list',
  imports: [
    AsyncPipe,
    MemberCard
  ],
  templateUrl: './member-list.html',
  styleUrl: './member-list.css',
})
export class MemberList {

  private memberService = inject(MemberService);

  public members$ : Observable<User[]>;

  constructor(){
    this.members$ = this.memberService.getMembers();
    console.log("memebrs", this.members$);
  }

}
