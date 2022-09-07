import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';

import { map } from 'rxjs/operators';
import { User } from '../_models/User';


@Injectable({
  providedIn: 'root'
})
export class AccountService 
{
  baseUrl = 'https://localhost:5001/api/';
  private currentUserSource = new ReplaySubject<User>(1); //1 is Observable buffer
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) { }

  login(model: any)
  {
    return this.http.post(this.baseUrl + 'account/login', model)
      .pipe //function in RxJS: You can use pipes to link operators together. Pipes let you combine multiple functions into a single function. 
      (
        map((response: User) => 
        {
          const user = response;
          if(user)
          {
            localStorage.setItem('user', JSON.stringify(user));
            this.currentUserSource.next(user);
          }
        })
      )
  }

  register(model: any)
  {
    return this.http.post(this.baseUrl + 'account/register', model).pipe
      (
        map((user: User) =>
        {
          if (user)
          {
            localStorage.setItem('user', JSON.stringify(user));
            this.currentUserSource.next(user);
          }
          return user;
        })
      )
  }

  setCurrentUser(user: User)
  {
    this.currentUserSource.next(user);
  }

  logout()
  {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
}
