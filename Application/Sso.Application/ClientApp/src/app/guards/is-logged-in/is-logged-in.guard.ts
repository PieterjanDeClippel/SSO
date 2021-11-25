import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { AccountService } from '../../services/account/account.service';


@Injectable({
  providedIn: 'root'
})
export class IsLoggedInGuard implements CanActivate {
  constructor(
    private accountService: AccountService,
    private router: Router,
  ) {
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    return this.accountService.currentUser().toPromise().then((user) => {
      return true;
    }).catch((error: HttpErrorResponse) => {
      this.router.navigate(['/account', 'login'], {
        queryParams: {
          return: state.url
        }
      });
      return false;
    });
  }
}
