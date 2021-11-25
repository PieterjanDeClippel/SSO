import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { User } from '../../entities/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  public csrfRefresh() {
    return this.httpClient.post(`${this.baseUrl}/web/Account/csrf-refresh`, {});
  }

  public currentUser() {
    return this.httpClient.get<User>(`${this.baseUrl}/web/Account/current-user`);
  }

  public logout() {
    return this.httpClient.post(`${this.baseUrl}/web/Account/logout`, {});
  }

}
