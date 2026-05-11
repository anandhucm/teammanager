import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../core/service/account-service';
import { Router, RouterLink } from '@angular/router';
import { ToastrService } from '../../core/service/toastr-service';

@Component({
  selector: 'app-nav',
  imports: [FormsModule],
  templateUrl: './nav.html',
  styleUrl: './nav.css'
})
export class Nav {

  protected credentials: any = {};
  protected accountService = inject(AccountService);
  protected darkModeVar = signal(false);
  protected memberData = this.accountService.memberData;
  protected router = inject(Router);
  protected toastrService = inject(ToastrService);

  login(){
    if(Object.keys(this.credentials).length === 0){
      console.log("this credentiald dds",this.credentials);
      this.toastrService.showToast("Please Enter Username and Password.", "Error");
    }else{
      this.accountService.login(this.credentials).subscribe({
         next: (result: any) => {
          this.toastrService.showToast("Successfully logged in", "Success");
          this.credentials = {};
          this.router.navigate(['/dashboard']);
          
         },
         error: (error: any) => {
          console.log("login error", error);
          if(typeof error.error === "string"){
            this.toastrService.showToast(error.error, "Error");
          }else{
            this.toastrService.showToast("Cannot login, issue occured", "Error");
          }
         }
      })
    }
  }

  darkMode(){

    this.darkModeVar.set(true);

  }

  NavigateToErrorPage(){
    console.log("inside the navigate to erro fuincton");
    this.router.navigate(['/errors']);
  }

}
