import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs/operators';
import { ɵangular_packages_platform_browser_platform_browser_k } from '@angular/platform-browser';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor 
{
  constructor(private router: Router, private toastr: ToastrService) 
  {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> 
  {
    return next.handle(request).pipe
    (
      catchError
      (httpErrorResponse =>
        {
          if(httpErrorResponse)
          {
            switch (httpErrorResponse.status)
            {
              case 400:
                if(httpErrorResponse.error.errors)
                {
                  const modalStateErrors = [];
                  for (const key in httpErrorResponse.error.errors)
                  {
                    if(httpErrorResponse.error.errors[key])
                    {
                      modalStateErrors.push(httpErrorResponse.error.errors[key]);
                    }
                  }
                  throw modalStateErrors.flat();
                }
                else
                {
                  this.toastr.error(httpErrorResponse.statusText, httpErrorResponse.status);
                }
                break;

              case 401:
                this.toastr.error(httpErrorResponse.statusText, httpErrorResponse.status);
                break;
              
                case 404:
                this.router.navigateByUrl('/not-found');
                break;
              
              case 500:
                const navigationExtras: NavigationExtras = {state: {error: httpErrorResponse.error}};
                this.router.navigateByUrl('/server-error', navigationExtras);
                break;

              default:
                this.toastr.error('Somthing wnet wrong');
                console.log(httpErrorResponse);
                break;
            }
          }
          return throwError(httpErrorResponse);
        })
    )
  }
}
