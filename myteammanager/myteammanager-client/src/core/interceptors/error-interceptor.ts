import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { ToastrService } from '../service/toastr-service';
import { NavigationExtras, Router } from '@angular/router';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {

  const toast = inject(ToastrService);
  const router = inject(Router);
  return next(req).pipe(
    catchError(error => {
      console.log("error inside the interceptor ",error);
      if(error){
          switch (error.status) {
            case 500:
              const navigationExtras : NavigationExtras = {state: {error: error.error}}
              router.navigateByUrl("/server-error", navigationExtras);
              break;
            case 401:
              toast.showToast("Unauthorized Action", "Error");
              break;
            case 404:
              console.log("inside the 404");
              router.navigateByUrl("not-found");
              break;
            case 400:
              toast.showToast(error.error, "Error");
              break;
              
              default:
              toast.showToast("Something went wrong !", "Error");
              break;
          }
      }
      return throwError(() => error);

    })
  )
};
 