import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import {User} from "../../types/types";
import { tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private account = inject(HttpClient);
  public  baseUrl = "http://localhost:5052/api/";
  public memberData = signal<User | null>(null);

  login(credentials: any){
    return this.account.post<User>(this.baseUrl + "account/login", credentials).pipe(
      tap(user=> {
        if(user){
          console.log("user", user);
          this.memberData.set(user);
          localStorage.setItem("memberData", JSON.stringify(user));
        }      
      })
    );
  }
  
}
