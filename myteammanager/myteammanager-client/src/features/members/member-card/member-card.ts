import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, inject, Input } from '@angular/core';
import { UploadResponse } from "../member-card/card-response";
import { tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AccountService } from '../../../core/service/account-service';

@Component({
  selector: 'app-member-card',
  imports: [],
  templateUrl: './member-card.html',
  styleUrl: './member-card.css',
})
export class MemberCard {

  @Input() member: any;

  private http = inject(HttpClient);
  public  baseUrl = environment.apiUrl;
  private accountService = inject(AccountService);

  uploadPhoto(fileInput: HTMLInputElement){
    const file = fileInput.files?.[0];
    const dataId = fileInput.getAttribute('data-id');
    console.log("file", file);
    console.log("dataId",dataId);

    const credentials = {
      "id" :  dataId,
      "file_details": file
    };

    const formData = new FormData();
    formData.append("id",dataId!);
    formData.append('file_details', file!);

    console.log("credential",credentials);
  var apiResult =  this.http.post<UploadResponse>(this.baseUrl + "account/upload-photo", formData, this.getHttpOptions()).pipe(
    tap((result: UploadResponse) => {
        console.log("result", result);
      })
  );

   apiResult.subscribe({
         next: (result: any) => {
          console.log("reault",result);
         },
         error: (error: any) => {
          
         }
      })
  }

  private getHttpOptions(){
    return {
      headers: new HttpHeaders({
        Authorization: "Bearer " +  this.accountService.memberData()?.token
      })
    }
  }


  checkGrpc(){
    var apiResult =  this.http.post<UploadResponse>(this.baseUrl + "account/check-grpc",{}, this.getHttpOptions()).pipe(
      tap((result: UploadResponse) => {
          console.log("result", result);
        })
    );

    apiResult.subscribe({
        next: (result: any) => {
          console.log("reault",result);
        },
        error: (error: any) => {
          
        }
    })
  }


   checkAzureFunction(){
    var apiResult =  this.http.post<UploadResponse>(this.baseUrl + "account/check-azure-function",{}, this.getHttpOptions()).pipe(
      tap((result: UploadResponse) => {
          console.log("result", result);
        })
    );

    apiResult.subscribe({
        next: (result: any) => {
          console.log("reault",result);
        },
        error: (error: any) => {
          
        }
    })
  }

}
