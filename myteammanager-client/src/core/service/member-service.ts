import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import {User} from "../../types/types";
import { AccountService } from './account-service';

@Injectable({
  providedIn: 'root',
})
export class MemberService {

  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl;
  private accountService = inject(AccountService);

  getMembers(){
    return this.http.get<User[]>(this.baseUrl + "account/members", this.getHttpOptions());
  }

  getMember(id: string){
    return this.http.get<User>(this.baseUrl + "account/members" + id, this.getHttpOptions());
  }

  private getHttpOptions(){
    return {
      headers: new HttpHeaders({
        Authorization: "Bearer " +  this.accountService.memberData()?.token
      })
    }
  }
  
}
