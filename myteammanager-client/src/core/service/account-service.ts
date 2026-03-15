import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import {User} from "../../types/types";
import { tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private account = inject(HttpClient);
  public  baseUrl = "https://localhost:7146/api/";
  public memberData = signal<User | null>(null);
  protected isLoggedIn = signal(false);

  login(credentials: any){
    return this.account.post<User>(this.baseUrl + "account/login", credentials).pipe(
      tap(user=> {
        if(user){
          console.log("user", user);
          this.memberData.set(user);
          this.isLoggedIn.set(true);
          localStorage.setItem("memberData", JSON.stringify(user));
        }      
      })
    );
  }

  logout(){
    this.isLoggedIn.set(false);
    localStorage.removeItem("memberData");
    this.memberData.set(null);
  }

}
